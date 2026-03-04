using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpLedger.Repository.Configuration.Helper;
using SimpLedger.Repository.ViewModel.Sales;

namespace SimpLedger.Controllers.Sales
{
    [Authorize]
    [ApiController]
    [Route("sales")]
    public class SalesController(ResponseHelper response) : ControllerBase
    {
        private readonly ResponseHelper _response = response;

        [HttpGet("report")]
        public async Task<IActionResult> GetSalesReport()
        {
            return StatusCode(200, _response.Success(200, true, null, null));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddSales(SalesPostModel sales)
        {
            return StatusCode(200, _response.Success(200, true, null, null));
        }

        [HttpGet("push")]
        public async Task<IActionResult> Push()
        {
            throw new NotImplementedException();
        }
    }                                                                                                                        
}
