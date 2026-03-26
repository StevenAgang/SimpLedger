using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interfaces.Common;

namespace SimpLedger.Repository.Services.Common
{
    public class ManualAuthenticationService(DatabaseContext context) : IManualAuthenticationService
    {
        private readonly DatabaseContext _context = context;


        public async Task<bool> VerificationToken(string token)
        {
            var cred = await _context.VerificationCodes.FirstOrDefaultAsync(t => t.Token == token && t.IsActive == true);

            if (cred == null) return false;

            if (DateTime.UtcNow > cred.ExpiresIn)
            {
                cred.IsActive = false;
                cred.Updated_At = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return false;
            }

            return true;
        }
    }
}
