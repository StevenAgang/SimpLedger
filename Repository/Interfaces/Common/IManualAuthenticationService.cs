namespace SimpLedger.Repository.Interfaces.Common
{
    public interface IManualAuthenticationService
    {
        /// <summary>
        ///     This checks if the token provided by the user is authentic
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> VerificationToken(string token);
    }
}
