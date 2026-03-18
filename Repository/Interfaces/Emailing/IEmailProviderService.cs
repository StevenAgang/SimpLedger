using Google.Apis.Auth.OAuth2;
using SimpLedger.Repository.Configurations;
using SimpLedger.Repository.Models.Auth;
using SimpLedger.Repository.ViewModels.Emailing;

namespace SimpLedger.Repository.Interfaces.Emailing
{
    public interface IEmailProviderService
    {
        Task SendMail(EmailSenderViewModel mail);
        UserCredential ConstructToken(string clientId, string clientSecret, string refreshToken, string scope, string email);
        EmailProviderSetting GetProviderSetting();
        string ConstructBody(string header, string message);
    }
}
