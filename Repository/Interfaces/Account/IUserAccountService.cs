using SimpLedger.Repository.ViewModels.Account;

namespace SimpLedger.Repository.Interfaces.Account
{
    public interface IUserAccountService
    {
        Task<AuthenticationResponse> Authenticate(UserAccountLogin user, CancellationToken cancellation);
        Task<ActivateAccountResponse> CreateAccount(UserAccountCreation user, CancellationToken cancellation);
        Task Logout(HttpContext context,int id);
        Task IsBlackListed(HttpContext context);
        Task<ActivateAccountResponse> ActivateAccount(string email, string purpose, CancellationToken cancellation);
        Task ResendCode(string oken);
        Task<UserAccountViewModel> CodeVerification(VerifyCode code);
        Task<UserAccountViewModel> ChangePassword(UserRecoveryViewModel user);
    }
}
