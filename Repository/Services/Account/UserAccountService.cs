using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Configurations.Exception_Extender;
using SimpLedger.Repository.Configurations.Validation.Account;
using SimpLedger.Repository.Interfaces.Account;
using SimpLedger.Repository.Interfaces.Common;
using SimpLedger.Repository.Interfaces.Data;
using SimpLedger.Repository.Interfaces.Data.Account;
using SimpLedger.Repository.Interfaces.Data.Enterprise;
using SimpLedger.Repository.Interfaces.Emailing;
using SimpLedger.Repository.Models.Account;
using SimpLedger.Repository.Models.Auth;
using SimpLedger.Repository.Models.Enterprise;
using SimpLedger.Repository.Models.Verification;
using SimpLedger.Repository.ViewModels.Account;
using SimpLedger.Repository.ViewModels.Emailing;
using SimpLedger.Repository.ViewModels.Verification;
using System.IdentityModel.Tokens.Jwt;

namespace SimpLedger.Repository.Services.Account
{
    public class UserAccountService
        (
        IUserAccountData userAccountData, 
        ISetTokenSevice tokenWriterSevice, 
        IEmailProviderService emailProviderService,
        ICompanyData companyData,
        IEmployeeData employeeData,
        ITokenManagerData tokenManaganerData
        ) : IUserAccountService
    {
        private readonly ISetTokenSevice _tokenWriterSevice = tokenWriterSevice;
        private readonly IEmailProviderService _emailProviderService = emailProviderService;
        private readonly IUserAccountData _userAccountData = userAccountData;
        private readonly ICompanyData _companyData = companyData;
        private readonly IEmployeeData _employeeData = employeeData;
        private readonly ITokenManagerData _tokenManagerData = tokenManaganerData;

        public async Task<AuthenticationResponse> Authenticate(UserAccountLogin user, CancellationToken cancellation)
        {
            UserAccountValidation.NotNullEmailAndPassword(user);

            var auth = await _userAccountData.GetUserActiveByEmail(user.Email,true,cancellation);

            if (auth == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var hashedPassword = _tokenWriterSevice.Hashed(user.Password, auth.Salt);

            if (auth.Password != hashedPassword)
            {
                throw new UnauthorizedAccessException("Invalid Credentials");
            }

            string tkn = _tokenWriterSevice.GenerateJwtToken(auth.Id, $"{auth.FirstName} {(auth.MiddleName != "" ? $"{auth.MiddleName[0]}." : "")} {auth.LastName}");

            var token = new AuthenticationResponse
            {
                Id = auth.Id,
                Token = tkn,
                FullName = $"{auth.FirstName} {(auth.MiddleName != "" ? $"{auth.MiddleName[0]}." : "")} {auth.LastName}",
            };

            return token;
        }

        public async Task<ActivateAccountResponse> CreateAccount(UserAccountCreation user, CancellationToken cancellation)
        {
            UserAccountValidation.ValidInformation(user);
            UserAccountValidation.ValidEmailAndPassword(user.Email, user.Password);

            var exist = await _userAccountData.GetUserNotActiveByEmail(user.Email, false, cancellation);

            if (exist != null && exist.IsActive == true)
            {
                throw new Conflict($"{user.Email} is already associated with another account");
            }
            if (exist != null && exist.IsActive == false)
            {
                throw new Conflict($"{user.Email} is already associated with another account but not yet active, go to activation account page to activate your account");
            }

            var salt = _tokenWriterSevice.GenerateSalt();
            var password = _tokenWriterSevice.Hashed(user.Password, salt);

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

            await _userAccountData.Save(account, cancellation);

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

                await _companyData.Save(company, cancellation);
            }
            else
            {
                var employee = new Employee
                {
                    UserAccount_Id = account.Id,
                    IsActive = false,
                    Created_At = DateTime.UtcNow,
                };

                await _employeeData.Save(employee, cancellation);
            }


            var inf = new EmailVerificationRequestField
            {
                Id = account.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Email = user.Email
            };

            return await GenerateEmailVerification(inf);
        }

        public async Task<ActivateAccountResponse> ActivateAccount(string email, string purpose, CancellationToken cancellation)
        {
            var account = new UserAccount();

            if(purpose == "activation")
            {
                account = await _userAccountData.GetUserNotActiveByEmail(email,false, cancellation);
            }

            if(purpose == "recovery")
            {
                account = await _userAccountData.GetUserActiveByEmail(email,true,cancellation);
            }

            if (account == null)
            {
                int sixDigitCode = _tokenWriterSevice.GenerateCode();
                return new ActivateAccountResponse
                {
                    Id = 0,
                    Token = _tokenWriterSevice.GenerateGenericToken(sixDigitCode),
                };
            }

            var existingCode = await _tokenManagerData.GetActiveVerificationCodeByUserId(account.Id, true);

            if (existingCode != null)
            {
                existingCode.IsActive = false;
                existingCode.Updated_At = DateTime.UtcNow;
                await _tokenManagerData.SaveChanges();
            }

            var inf = new EmailVerificationRequestField
            {
                Id = account.Id,
                FirstName = account.FirstName,
                MiddleName = account.MiddleName,
                LastName = account.LastName,
                Email =  account.Email
            };

            return await GenerateEmailVerification(inf);
        }

        private async Task<ActivateAccountResponse> GenerateEmailVerification(EmailVerificationRequestField inf)
        {
            int sixDigitCode = _tokenWriterSevice.GenerateCode();
            var localToken = _tokenWriterSevice.GenerateGenericToken(sixDigitCode);

            SaveVerificationCode(inf.Id, sixDigitCode, localToken);

            var mail = new EmailSenderViewModel
            {
                ToName = $"{inf.FirstName} {(inf.MiddleName != "" ? $"{inf.MiddleName[0]}." : "")} {inf.LastName}",
                ToMail = inf.Email,
                Subject = "Verification Code",
                Message = $"{sixDigitCode}",
                HeaderMessage = "Your six digit verification code"
            };

            await _emailProviderService.SendMail(mail);

            return new ActivateAccountResponse
            {
                Id = inf.Id,
                Token = localToken,
            };
        }

        private async Task SaveVerificationCode(int id, int sixDigitCode, string localToken)
        {
            var verification = new VerificationCode
            {
                UserAccount_Id = id,
                Code = sixDigitCode,
                Token = localToken,
                ExpiresIn = DateTime.UtcNow.AddMinutes(5),
                Created_At = DateTime.UtcNow,
                IsActive = true
            };

            await _tokenManagerData.Save(verification);
        }

        public async Task ResendCode(string token)
        {
            if (token == "")
            {
                return;
            }

            var existingCode = await _tokenManagerData.GetActiveVerificationCodeByToken(token, true);

            if(existingCode == null)
            {
                throw new ResourceNotFound("This request is already expired, please request a new one");
            }

            int sixDigitCode = _tokenWriterSevice.GenerateCode();

            existingCode.Code = sixDigitCode;
            existingCode.Updated_At = DateTime.UtcNow;
            existingCode.Updated_By = existingCode.UserAccount_Id;
            existingCode.ExpiresIn = DateTime.UtcNow.AddMinutes(5);

            await _tokenManagerData.SaveChanges();

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
            if(code == null || code.Token == "" || code.Code == 0)
            {
                throw new BadRequest("Invalid code");
            }

            var auth = await _tokenManagerData.GetActiveVerificationCodeByToken(code.Token, true);
            dynamic? type;

            
            if (auth == null)
            {
                throw new BadRequest("Code is invalid");
            }

            if(auth.UserAccount.AccountType_Id == 1)
            {
                type = await _companyData.GetCompanyByUserId(auth.UserAccount_Id, true);
            }
            else
            {
                type = await _employeeData.GetEmployeeByUserId(auth.UserAccount_Id, true);
            }

            if (DateTime.UtcNow > auth.ExpiresIn)
            {
                auth.IsActive = false;
                auth.Updated_At = DateTime.UtcNow;

                await _tokenManagerData.SaveChanges();

                throw new AcceptedDbCommit("Code is expired");
            }

            if(auth.Code == code.Code)
            {
                type.IsActive = true;
                auth.UserAccount.IsActive = true;
                auth.UserAccount.Updated_At = DateTime.UtcNow;
                type.Updated_At = DateTime.UtcNow;
                auth.IsActive = false;

                await _userAccountData.SaveChanges();

                string tkn = "";
                if(code.ReturnJwtToken == true)
                {
                    tkn = _tokenWriterSevice.GenerateJwtToken(auth.UserAccount.Id, $"{auth.UserAccount.FirstName} {(auth.UserAccount.MiddleName != "" ? $"{auth.UserAccount.MiddleName[0]}." : "")} {auth.UserAccount.LastName}");
                }
                else
                {
                    int sixDigitCode = _tokenWriterSevice.GenerateCode();
                    tkn = _tokenWriterSevice.GenerateGenericToken(sixDigitCode);
                    await SaveVerificationCode(auth.UserAccount_Id, 0, tkn);
                }

                var user = new UserAccountViewModel
                {
                   Id = auth.UserAccount_Id,
                   Name = auth.UserAccount.FirstName,
                   Token = tkn
                };

                return user;
            }
            throw new UnauthorizedAccessException("Invalid Code");
        }

        public async Task<UserAccountViewModel> ChangePassword(UserRecoveryViewModel user)
        {
            UserAccountValidation.ValidPassword(user.Password);

            var code = await _tokenManagerData.GetActiveVerificationCodeByToken(user.Token, true);

            if(code != null && code.IsActive == false)
            {
                throw new BadRequest("Invalid request");
            }

            var account = await _userAccountData.GetUserActiveById(code.UserAccount_Id, true);

            if (account == null)
            {
                throw new ResourceNotFound("Resource not found");
            }

            var salt = _tokenWriterSevice.GenerateSalt();
            var password = _tokenWriterSevice.Hashed(user.Password,salt);

            code.IsActive = false;
            account.Salt = salt;
            account.Password = password;

            await _userAccountData.SaveChanges();

            string tkn = _tokenWriterSevice.GenerateJwtToken(account.Id, $"{account.FirstName} {(account.MiddleName != "" ? $"{account.MiddleName[0]}." : "")} {account.LastName}");

            return new UserAccountViewModel
            {
                Id = account.Id,
                Name = account.FirstName,
                Token = tkn
            };
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

            await _tokenManagerData.Save(blackListToken);
        }

        public async Task IsBlackListed(HttpContext context) 
        {
            var user = context.Request.Headers.Cookie.ToString();

            if(!string.IsNullOrWhiteSpace(user) && user.StartsWith("AccessToken="))
            {
                var token = user.Replace("AccessToken=", "");
                var handler = new JwtSecurityTokenHandler();
                var jti = "";

                if (handler.CanReadToken(token))
                {
                    try
                    {
                        var jwtToken = handler.ReadJwtToken(token);
                        jti = jwtToken.Claims.FirstOrDefault(j => j.Type == "jti")?.Value;

                    }
                    catch (Exception ex)
                    {
                        context.Response.Cookies.Delete("AccessToken");
                        throw new UnauthorizedAccessException("Unauthorize Access");
                    }
                }
                else
                {
                    context.Response.Cookies.Delete("AccessToken");
                    throw new UnauthorizedAccessException("Unauthorize Access");
                }

                if (!string.IsNullOrEmpty(jti))
                {
                    bool isBlackListed = await _tokenManagerData.GetExpiredJwtToken(jti);

                    if (isBlackListed)
                    {
                        context.Response.Cookies.Delete("AccessToken");
                        throw new UnauthorizedAccessException("Unauthorize Access, Please stop stealing Cookies :(");
                    }
                }
            }
            return;
        }
    }
}
