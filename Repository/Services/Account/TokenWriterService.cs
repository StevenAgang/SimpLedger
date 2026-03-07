using Microsoft.IdentityModel.Tokens;
using SimpLedger.Repository.Configurations;
using SimpLedger.Repository.Interfaces.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpLedger.Repository.Services.Account
{
    public class TokenWriterService(IConfiguration config) : ITokenWriterSevice
    {
        private readonly IConfiguration _config = config;
        public string GenerateToken(string id, string name)
        {
            var jwtSettings = new JwtSettings();
            _config.GetSection("Jwt").Bind(jwtSettings);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(ClaimTypes.Name, name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
    }
}