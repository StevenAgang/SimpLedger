using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Configurations.Exception_Extender;
using SimpLedger.Repository.Interfaces.Account;
using SimpLedger.Repository.Interfaces.Emailing;
using SimpLedger.Repository.Models.Account;
using SimpLedger.Repository.Models.Auth;
using SimpLedger.Repository.Models.Enterprise;
using SimpLedger.Repository.Models.Verification;
using SimpLedger.Repository.ViewModels.Account;
using SimpLedger.Repository.ViewModels.Emailing;
using System.IdentityModel.Tokens.Jwt;

namespace SimpLedger.Repository.Services.Account
{
    public class UserAccountService(DatabaseContext context, ISetTokenSevice tokenWriterSevice, IEmailProviderService emailProviderService) : IUserAccountService
    {
        private readonly ISetTokenSevice _tokenWriterSevice = tokenWriterSevice;
        private readonly IEmailProviderService _emailProviderService = emailProviderService;
        private readonly DatabaseContext _context = context;
        public async Task<AuthenticationResponse> Authenticate(UserAccountLogin user, CancellationToken cancellationToken)
        {
            var auth = await _context.UserAccount.FirstOrDefaultAsync(u => u.Email == user.Email && u.IsActive == true, cancellationToken);

            if (auth == null) throw new UnauthorizedAccessException("Invalid credentials");

            var hashedPassword = _tokenWriterSevice.Hashed(user.Password, auth.Salt);

            if (auth.Password != hashedPassword) throw new UnauthorizedAccessException("Invalid Credentials");

            string tkn = _tokenWriterSevice.GenerateJwtToken(auth.Id, $"{auth.FirstName} {(auth.MiddleName != "" ? $"{auth.MiddleName[0]}." : "")} {auth.LastName}");

            var token = new AuthenticationResponse
            {
                Id = auth.Id,
                Token = tkn,
                FullName = $"{auth.FirstName} {(auth.MiddleName != "" ? $"{auth.MiddleName[0]}." : "")} {auth.LastName}",
            };

            return token;
        }

        public async Task<ActivateAccountResponse> CreateAccount(UserAccountCreation user)
        {
            var exist = await _context.UserAccount.AsNoTracking().FirstOrDefaultAsync(u => u.Email == user.Email);

            if (exist != null && exist.IsActive == true) throw new Conflict($"{user.Email} is already associated with another account");
            if (exist != null && exist.IsActive == false) throw new Conflict($"{user.Email} is already associated with another account but not yet active, go to activation account page to activate your account");

            var salt = _tokenWriterSevice.GenerateSalt();
            var password = _tokenWriterSevice.Hashed(user.Password, salt);
            int sixDigitCode = _tokenWriterSevice.GenerateCode();
            var localToken = _tokenWriterSevice.GenerateGenericToken(sixDigitCode);

            var mail = new EmailSenderViewModel
            {
                ToName = $"{user.FirstName} {(user.MiddleName != "" ? $"{user.MiddleName[0]}." : "")} {user.LastName}",
                ToMail = user.Email,
                Subject = "Verification Code",
                Message = $"{sixDigitCode}",
                HeaderMessage = "Your six digit verification code"
            };

            await _emailProviderService.SendMail(mail);

            var account = new UserAccount
            {
                AccountType_Id = Convert.ToInt32(user.UserType),
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Email = user.Email,
                Password = password,
                Salt = salt,
                Created_At = DateTime.UtcNow,
                IsActive = false
            };

            _context.Add(account);
            await _context.SaveChangesAsync();

            if (user.UserType == "1")
            {
                var company = new Company
                {
                    UserAccount_Id = account.Id,
                    Name = user.BusinessName,
                    Address = user.BusinessAddress,
                    Email = user.BusinessEmail,
                    Created_By = account.Id,
                    Created_At = DateTime.UtcNow,
                    IsActive = false
                };
                _context.Add(company);
            }
            else
            {
                var employee = new Employee
                {
                    UserAccount_Id = account.Id,
                    IsActive = false,
                    Created_At = DateTime.UtcNow,
                };

                _context.Add(employee);
            }

                var verification = new VerificationCode
                {
                    UserAccount_Id = account.Id,
                    Code = sixDigitCode,
                    Token = localToken,
                    ExpiresIn = DateTime.UtcNow.AddMinutes(5),
                    Created_At = DateTime.UtcNow,
                    IsActive = true
                };

            _context.Add(verification);
            await _context.SaveChangesAsync();

            var token = new ActivateAccountResponse
            {
                Token = localToken,
            };

            return token;
        }


        public async Task<ActivateAccountResponse> ActivateAccount(string email)
        {
            var account = await _context.UserAccount.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email && a.IsActive == false);

            if (account == null) throw new BadRequest($"Email not found or is already activated");

            var existingCode = await _context.VerificationCodes.FirstOrDefaultAsync(c => c.UserAccount_Id == account.Id && c.IsActive == true);

            int sixDigitCode = _tokenWriterSevice.GenerateCode();
            var localToken = _tokenWriterSevice.GenerateGenericToken(sixDigitCode);

            var mail = new EmailSenderViewModel
            {
                ToName = $"{account.FirstName} {(account.MiddleName != "" ? $"{account.MiddleName[0]}." : "")} {account.LastName}",
                ToMail = account.Email,
                Subject = "Verification Code",
                Message = $"{sixDigitCode}",
                HeaderMessage = "Your six digit verification code"
            };

            await _emailProviderService.SendMail(mail);

            if (existingCode != null)
            {
                existingCode.IsActive = false;
                existingCode.Updated_At = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            var verification = new VerificationCode
            {
                UserAccount_Id = account.Id,
                Code = sixDigitCode,
                Token = localToken,
                ExpiresIn = DateTime.UtcNow.AddMinutes(5),
                Created_At = DateTime.UtcNow,
                IsActive = true
            };

            _context.Add(verification);
            await _context.SaveChangesAsync();

            return new ActivateAccountResponse
            {
                Token = localToken,
            };
        }

        public async Task ResendCode(int id)
        {
            var existingCode = _context.VerificationCodes.Include(u => u.UserAccount).FirstOrDefault(u => u.UserAccount_Id == id && u.IsActive == true);

            int sixDigitCode = _tokenWriterSevice.GenerateCode();

            existingCode.Code = sixDigitCode;
            existingCode.Updated_At = DateTime.UtcNow;
            existingCode.Updated_By = existingCode.UserAccount_Id;
            existingCode.ExpiresIn = DateTime.UtcNow.AddMinutes(5);

            await _context.SaveChangesAsync();

            var mail = new EmailSenderViewModel
            {
                ToName = $"{existingCode.UserAccount.FirstName} {(existingCode.UserAccount.MiddleName != "" ? $"{existingCode.UserAccount.MiddleName[0]}." : "")} {existingCode.UserAccount.LastName}",
                ToMail = existingCode.UserAccount.Email,
                Subject = "Verification Code",
                Message = $"{sixDigitCode}",
                HeaderMessage = "Your six digit verification code"
            };

            await _emailProviderService.SendMail(mail);
        }

        public async Task<UserAccountViewModel> CodeVerification(VerifyCode code)
        {
            var auth = await _context.VerificationCodes.Include(u => u.UserAccount).FirstOrDefaultAsync(c => c.UserAccount_Id == code.Id && c.IsActive == true);
            dynamic? type;

            if(auth.UserAccount.AccountType_Id == 1)
            {
                type = await _context.Company.FirstOrDefaultAsync(u => u.UserAccount_Id == code.Id);
            }
            else
            {
                type = await _context.Employee.FirstOrDefaultAsync(u => u.UserAccount_Id == code.Id);
            }

            if (auth == null) throw new Exception("Token is expired");
            if (DateTime.UtcNow > auth.ExpiresIn)
            {
                auth.IsActive = false;
                auth.Updated_At = DateTime.UtcNow;
                throw new Exception("Token is expired");
            }

            if(auth.Code == code.Code)
            {
                type.IsActive = true;
                auth.UserAccount.IsActive = true;
                auth.UserAccount.Updated_At = DateTime.UtcNow;
                type.Updated_At = DateTime.UtcNow;
                auth.IsActive = false;

                await _context.SaveChangesAsync();

                string tkn = _tokenWriterSevice.GenerateJwtToken(auth.UserAccount.Id, $"{auth.UserAccount.FirstName} {(auth.UserAccount.MiddleName != "" ? $"{auth.UserAccount.MiddleName[0]}." : "")} {auth.UserAccount.LastName}");

                var user = new UserAccountViewModel
                {
                    Id = auth.UserAccount_Id,
                    Name = auth.UserAccount.FirstName,
                    Token = tkn
                };

                return user;
            }
            throw new UnauthorizedAccessException("Code is invalid");
        }

        public async Task Logout(HttpContext context, int id)
        {
            var user = context.User;
            var jti = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var expiresAt = user.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

            var blackListToken = new ExpiredToken 
            {
                Jti = jti!,
                ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiresAt!)).UtcDateTime,
                Created_At = DateTime.UtcNow,
                Created_By = id,
                IsActive = true
            };

            _context.Add(blackListToken);
            await _context.SaveChangesAsync();
        }

        public Task IsBlackListed(HttpContext context) 
        {
            var user = context.Request.Headers.Cookie.ToString();

            if(!string.IsNullOrWhiteSpace(user) && user.StartsWith("AccessToken="))
            {
                var token = user.Replace("AccessToken=", "");
                var handler = new JwtSecurityTokenHandler();
                var jti = "";

                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    jti = jwtToken.Claims.FirstOrDefault(j => j.Type == "jti")?.Value;
                }

                if (!string.IsNullOrEmpty(jti))
                {
                    bool isBlackListed = _context.ExpiredTokens.AsNoTracking().Any(j => j.Jti == jti);

                    if (isBlackListed)
                    {
                        context.Response.Cookies.Delete("AccessToken");
                        throw new UnauthorizedAccessException("Unauthorize Access, Please stop stealing Cookies :(");
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
