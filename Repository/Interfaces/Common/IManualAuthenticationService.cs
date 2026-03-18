namespace SimpLedger.Repository.Interfaces.Common
{
    public interface IManualAuthenticationService
    {
        Task<object> VerificationToken(string token);
    }
}
