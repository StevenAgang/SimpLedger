using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpLedger.Repository.Configuration.Helper;
using SimpLedger.Repository.Interfaces.Common;
using System.Linq;
using static Google.Apis.Requests.BatchRequest;

namespace SimpLedger.Controllers.Common
{
    [ApiController]
    [Route("common")]
    public class ManualAuthenticationController(IManualAuthenticationService manualAuthenticationService, ResponseHelper response) : ControllerBase
    {
        private readonly IManualAuthenticationService _manualAuthenticationService = manualAuthenticationService;
        private readonly ResponseHelper _response = response;

        [AllowAnonymous]
        [HttpGet("verification")]
        public async Task<IActionResult> VerificationCodesToken([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new UnauthorizedAccessException("Unauthorized Action");
            var user = await _manualAuthenticationService.VerificationToken(token);
            return StatusCode(200, _response.Success(200, true, "Proceed to account activation", user));
        }
    }
}
