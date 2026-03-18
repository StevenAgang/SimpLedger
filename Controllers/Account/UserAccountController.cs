using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SimpLedger.Repository.Configuration.Helper;
using SimpLedger.Repository.Configurations;
using SimpLedger.Repository.Configurations.Validation.Account;
using SimpLedger.Repository.Interfaces.Account;
using SimpLedger.Repository.ViewModels.Account;
using System.IdentityModel.Tokens.Jwt;

namespace SimpLedger.Controllers.UserAccount
{
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UserAccountController
        (
        UserAccountValidation userAccountValidation, 
        IUserAccountService userAccountService, 
        ResponseHelper response,
        IConfiguration configuration,
        ISetTokenSevice setTokenService
        ) : ControllerBase
    {
        private readonly ResponseHelper _response = response;
        private readonly IUserAccountService _userAccountService = userAccountService;
        private readonly UserAccountValidation _userAccountValidation = userAccountValidation;
        private readonly IConfiguration _configuration = configuration;
        private readonly ISetTokenSevice _setTokenSevice = setTokenService;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]UserAccountLogin user, CancellationToken cancellationToken)
        {

            var users = HttpContext.User;
            var _= users.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            _userAccountValidation.NotNullEmailAndPassword(user);

            var token = await _userAccountService.Authenticate(user,cancellationToken);

            var httpContextSettings = new HttpContextSettings();

            _configuration.GetSection("HttpContextSettings").Bind(httpContextSettings);

            _setTokenSevice.SetCookie("AccessToken", token.Token, httpContextSettings, HttpContext);

            token.Token = "";

            return StatusCode(200, _response.Success(200, true, "Login successfully", token));
        }

        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "https://localhost:4200/signin" }, "GoogleOpenIdConnect");
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleLoginCallBack()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");

            var name = User.FindFirst("name");
            var email = User.FindFirst("email");
            return Ok(new { accessToken, idToken });
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]UserAccountCreation user)
        {
            _userAccountValidation.ValidInformation(user);
            _userAccountValidation.ValidEmailAndPassword(user.Email, user.Password);

            var token =  await _userAccountService.CreateAccount(user);

            return StatusCode(200, _response.Success(200, true, "Proceed to account activation", token.Token));
        }

        [AllowAnonymous]
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateAccount([FromBody] ActivateRequest email)
        {
            var token = await _userAccountService.ActivateAccount(email.Email);

            return StatusCode(200, _response.Success(200, true, "Activation code sent", token.Token));
        }

        [AllowAnonymous]
        [HttpGet("resend")]
        public async Task<IActionResult> VerificationResend([FromQuery] int id)
        {
            await _userAccountService.ResendCode(id);
            return StatusCode(200, _response.Success(200, true, "Activation code sent", null));
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCode code)
        {
           var user = await _userAccountService.CodeVerification(code);

            var httpContextSettings = new HttpContextSettings();

            _configuration.GetSection("HttpContextSettings").Bind(httpContextSettings);

            _setTokenSevice.SetCookie("AccessToken", user.Token, httpContextSettings, HttpContext);

            object newUser = new
            {
                Id = user.Id,
                Name = user.Name
            };

            return StatusCode(200, _response.Success(200, true, "Activated SucessFully", newUser));
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(int id)
        {
            await _userAccountService.Logout(HttpContext, id);
            HttpContext.Response.Cookies.Delete("AccessToken");
            return StatusCode(200, _response.Success(200, true, "Logged out successfully", null));
        }
    }
}
