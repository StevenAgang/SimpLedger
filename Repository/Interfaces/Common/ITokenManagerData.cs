using SimpLedger.Repository.Interfaces.Data;
using SimpLedger.Repository.Models.Verification;

namespace SimpLedger.Repository.Interfaces.Common
{
    public interface ITokenManagerData : IBaseData
    {
        Task<VerificationCode> GetActiveVerificationCodeByUserId(int userId, bool withTracking);
        Task<VerificationCode> GetActiveVerificationCodeByToken(string token, bool withTracking);
        Task<bool> GetExpiredJwtToken(string jti);
    }
}
