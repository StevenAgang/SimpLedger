using SimpLedger.Repository.ViewModels.Account;

namespace SimpLedger.Repository.Interfaces.Account
{
    public interface IUserAccountService
    {
        Task<AuthenticationResponse> Authenticate(UserAccountLogin user, CancellationToken cancellationToken);
        Task<ActivateAccountResponse> CreateAccount(UserAccountCreation user);
        Task Logout(HttpContext context,int id);
        Task IsBlackListed(HttpContext context);
        Task<ActivateAccountResponse> ActivateAccount(string email);
        Task ResendCode(int id);
        Task<UserAccountViewModel> CodeVerification(VerifyCode code);

    }
}
