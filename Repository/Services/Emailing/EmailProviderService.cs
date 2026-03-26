using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using MimeKit;
using SimpLedger.Repository.Configurations;
using SimpLedger.Repository.Interfaces.Emailing;
using SimpLedger.Repository.ViewModels.Emailing;
using System.Net.Http.Headers;
using System.Text.Json;


namespace SimpLedger.Repository.Services.Emailing
{
    public class EmailProviderService(IConfiguration configuration) : IEmailProviderService
    {
        private readonly IConfiguration _configuration = configuration;
        public async Task SendMail(EmailSenderViewModel mail)
        {
            var setting = GetProviderSetting();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(setting.Name,setting.Email));
            message.To.Add(new MailboxAddress(mail.ToName, mail.ToMail));
            message.Subject = mail.Subject;

            message.Body = new TextPart("html")
            {
                Text = ConstructBody(mail.HeaderMessage, mail.Message)
            };

            var credentials = ConstructToken(setting.ClientId, setting.ClientSecret, setting.RefreshToken, setting.Scope, setting.Email);
            var accessToken = await credentials.GetAccessTokenForRequestAsync();

            using var HttpClient = new HttpClient();
            using var stream = new MemoryStream();

            await message.WriteToAsync(stream);

            var rawEmail = Convert.ToBase64String(stream.ToArray())
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

            var mailRequest = new HttpRequestMessage(HttpMethod.Post, "https://gmail.googleapis.com/gmail/v1/users/me/messages/send");
            mailRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var json = JsonSerializer.Serialize(new { raw = rawEmail });
            mailRequest.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await HttpClient.SendAsync(mailRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Cant send message");
            }

            //var oauth2 = new SaslMechanismOAuthBearer(credentials.UserId, accessToken);
            //using var client = new SmtpClient();
            //await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //await client.AuthenticateAsync(oauth2);
            //await client.SendAsync(message);
            //await client.DisconnectAsync(true);
        }
        
        public EmailProviderSetting GetProviderSetting()
        {
            var setting = new EmailProviderSetting();
            _configuration.GetSection("EmailProviderSetting").Bind(setting);

            return setting;
        }

        public UserCredential ConstructToken(string clientId, string clientSecret, string refreshToken, string scope, string email)
        {
            var credentials = new UserCredential(
                new GoogleAuthorizationCodeFlow(
                        new GoogleAuthorizationCodeFlow.Initializer
                        {
                            ClientSecrets = new ClientSecrets
                            {
                                ClientId = clientId,
                                ClientSecret = clientSecret,
                            },
                            Scopes = [scope]
                        }),
                        "user",
                        new TokenResponse
                        {
                            RefreshToken = refreshToken
                        }
                );

            return credentials;
        }

        public string ConstructBody(string header, string message)
        {

            string body = $@"<main style=""width: 30%; margin: auto"">
                <header style= ""background-color: #1666ba;padding: 10px;text-align: center;font-size: 2rem;color: white;font-weight: bold;border-radius: 10px;"">{header}</header>
                <section style= ""width: 100%; text-align: center; font-size: 2rem""> <p> {message}</p> </section>
                </main>";

            return body;
        }
    }
}
    
