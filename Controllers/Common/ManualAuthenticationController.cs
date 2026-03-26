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
        [HttpGet("verify-token")]
        public async Task<IActionResult> ChangePasswordToken([FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new UnauthorizedAccessException("Unauthorized Action");
            var user = await _manualAuthenticationService.VerificationToken(token);
            return StatusCode(200, _response.Success(200, true, "Proceed to account activation", user));
        }

        [AllowAnonymous]
        [HttpGet("authorize")]
        public IActionResult AuthorizeView()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            var token = Request.Cookies["AccessToken"];

            if (!string.IsNullOrWhiteSpace(token))
            {
                return StatusCode(200, _response.Success(200,true,null,null));
            }
            return StatusCode(200, _response.Success(401,false,null,null));
        }
    }
}
