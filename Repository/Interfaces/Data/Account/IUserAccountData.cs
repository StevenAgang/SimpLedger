using SimpLedger.Repository.Models.Account;

namespace SimpLedger.Repository.Interfaces.Data.Account
{
    public interface IUserAccountData : IBaseData
    {
        /// <summary>
        ///  This will get the user email base on the email provided
        /// </summary>
        /// <param name="email">Email to be search</param>
        /// <param name="withTracking">allow tracking of data or not</param>
        /// <param name="cancellation">tracking cancellation token</param>
        /// <returns>Returns a useraccount object</returns>
        Task<UserAccount> GetUserActiveByEmail(string email, bool withTracking, CancellationToken cancellation = default);
        Task<UserAccount> GetUserNotActiveByEmail(string email, bool withTracking, CancellationToken cancellation = default);
        Task<UserAccount> GetUserActiveById(int userId, bool withTracking, CancellationToken cancellation = default);
    }
}
