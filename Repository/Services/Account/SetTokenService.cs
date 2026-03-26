using Microsoft.IdentityModel.Tokens;
using SimpLedger.Repository.Configurations;
using SimpLedger.Repository.Interfaces.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SimpLedger.Repository.Services.Account
{
    public class SetTokenService(IConfiguration config) : ISetTokenSevice
    {
        private readonly IConfiguration _config = config;
        public string GenerateJwtToken(int id, string name)
        {
            var jwtSettings = new JwtSettings();
            _config.GetSection("Jwt").Bind(jwtSettings);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, id.ToString()),
                new (JwtRegisteredClaimNames.Name, name),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpiredInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public HttpContext SetCookie(string tokenName, string actualToken, HttpContextSettings httpContextSettings, HttpContext context)
        {
            context.Response.Cookies.Append("AccessToken", actualToken, new CookieOptions
            {
                HttpOnly = httpContextSettings.IsHttpOnly,
                Secure = httpContextSettings.IsSecure,
                SameSite = Enum.TryParse<SameSiteMode>(httpContextSettings.SameSite, out var mode) ? mode : SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddMinutes(httpContextSettings.ExpireInMinutes)
            });

            return context;
        }

        public string GenerateGenericToken(int code)
        {
            string salt = GenerateSalt();
            return Hashed(code, salt);
        }

        public int GenerateCode()
        {
            var rand = new Random();
            return rand.Next(100000, 999999);
        }

        public string Hashed<T>(T value, string salt)
        {
            var combined = Encoding.UTF8.GetBytes(value + salt);
            var hash = SHA256.HashData(combined);
            var base64 = Convert.ToBase64String(hash);

            return base64.Replace('/', '_')
                .Replace('=', '_')
                .Replace('+', '-')
                .TrimEnd('=');
        }

        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }
}