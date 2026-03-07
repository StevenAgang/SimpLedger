using SimpLedger.Repository.ViewModels.Account;

namespace SimpLedger.Repository.Interfaces.Account
{
    public interface IUserAccountService
    {
        Task<AuthenticationResponse> Authenticate(UserAccountLogin user, CancellationToken cancellationToken);
        string GenerateSalt();
        string HashPassword(string password, string salt);
        
    }
}
