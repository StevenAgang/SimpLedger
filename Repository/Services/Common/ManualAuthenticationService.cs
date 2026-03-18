using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interfaces.Common;

namespace SimpLedger.Repository.Services.Common
{
    public class ManualAuthenticationService(DatabaseContext context) : IManualAuthenticationService
    {
        private readonly DatabaseContext _context = context;


        public async Task<object> VerificationToken(string token)
        {
            var cred = await _context.VerificationCodes.Include(u => u.UserAccount).FirstOrDefaultAsync(t => t.Token == token && t.IsActive == true);

            if (cred == null) throw new UnauthorizedAccessException("Unauthorized Action");

            if (DateTime.UtcNow > cred.ExpiresIn)
            {
                cred.IsActive = false;
                cred.Updated_At = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                throw new UnauthorizedAccessException("Token is already expired");
            }

            var user = new
            {
                id = cred.UserAccount.Id,
                email = cred.UserAccount.Email
            };

            return user;
        }
    }
}
