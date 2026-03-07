using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interfaces.Account;
using SimpLedger.Repository.ViewModels.Account;
using System.Security.Cryptography;
using System.Text;

namespace SimpLedger.Repository.Services.Account
{
    public class UserAccountService(DatabaseContext context) : IUserAccountService
    {
        private readonly DatabaseContext _context = context;
        public async Task<AuthenticationResponse> Authenticate(UserAccountLogin user, CancellationToken cancellationToken)
        {
            var auth = await _context.UserAccount.FirstOrDefaultAsync(u => u.Email == user.Email && u.IsActive == true);

            if (auth == null) throw new UnauthorizedAccessException("Invalid credentials");

            var hashedPassword = HashPassword(user.Password, auth.Salt);

            if (auth.Password != hashedPassword) throw new UnauthorizedAccessException("Invalid Credentials");

            
        }

        private string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        private string GenerateSalt()
        {
            return "";
        }
    }
}
