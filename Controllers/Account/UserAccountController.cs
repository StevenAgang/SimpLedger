using Microsoft.AspNetCore.Mvc;
using SimpLedger.Repository.Configurations.Validation.Account;
using SimpLedger.Repository.Services.Account;
using SimpLedger.Repository.ViewModels.Account;

namespace SimpLedger.Controllers.UserAccount
{
    [ApiController]
    [Route("user")]
    public class UserAccountController(LoginValidation loginValidation, UserAccountService userAccountService) : ControllerBase
    {
        private readonly UserAccountService _userAccountService = userAccountService;
        private readonly LoginValidation _loginValidation = loginValidation;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserAccountLogin user, CancellationToken cancellationToken)
        {
           _loginValidation.ValidInput(user);

            await _userAccountService.Authenticate(user,cancellationToken);

            throw new NotImplementedException();
        }
    }
}
