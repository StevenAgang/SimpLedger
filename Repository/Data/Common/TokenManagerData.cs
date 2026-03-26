using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interfaces.Common;
using SimpLedger.Repository.Interfaces.Data;
using SimpLedger.Repository.Models.Auth;
using SimpLedger.Repository.Models.Verification;

namespace SimpLedger.Repository.Data.Common
{
    public class TokenManagerData(DatabaseContext context) : BaseData(context), ITokenManagerData
    {
        private readonly DatabaseContext _context = context;

        private IQueryable<VerificationCode> VerificationCodeBaseQuery(bool withTracking, bool? isActive)
        {
            var query = _context.VerificationCodes.Include(u => u.UserAccount).Where(v => v.IsActive == isActive);

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
        public async Task<VerificationCode> GetActiveVerificationCodeByUserId(int userId, bool withTracking)
        {
            var token = await VerificationCodeBaseQuery(withTracking, true).FirstOrDefaultAsync(u => u.UserAccount_Id == userId);

            return token;
        }

        public async Task<VerificationCode> GetActiveVerificationCodeByToken(string token, bool withTracking)
        {
            var tkn = await VerificationCodeBaseQuery(withTracking, true).FirstOrDefaultAsync(t => t.Token == token);

            return tkn;
        }

        public async Task<bool> GetExpiredJwtToken(string jti)
        {
            var token = await _context.ExpiredTokens.AsNoTracking().AnyAsync(t => t.Jti == jti);

            return token;
        }
    }
}
            