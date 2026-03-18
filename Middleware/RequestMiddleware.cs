using SimpLedger.Repository.Configuration.Helper;
using SimpLedger.Repository.Configurations.Exception_Extender;
using SimpLedger.Repository.Interfaces.Account;
using SimpLedger.Repository.Services.Account;

namespace SimpLedger.Middleware
{
    public class RequestMiddleware(RequestDelegate next, ResponseHelper response, IServiceScopeFactory scope)
    {
        private readonly RequestDelegate _next = next;
        private readonly ResponseHelper _response = response;
        private readonly IServiceScopeFactory _scope = scope;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Authorize(context);
                await _next(context);
            }
            catch (BadRequest ex)
            {
                await Response(context, 400, "application/json", ex.Message);
            }
            catch(Conflict ex)
            {
                await Response(context, 409, "application/json", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await Response(context, 401, "application/json", ex.Message);
            }
            catch(Exception ex)
            {
                await Response(context,500,"application/json", ex.Message);
            }
        }

        public async Task<HttpContext> Response(HttpContext context, int statusCode, string contentType, string error)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = contentType;
            await context.Response.WriteAsJsonAsync(_response.Success(statusCode, false, error, null));

            return context;
        }

        /// Todo: make a different middleware for logout function. because the current version does is all the route is coming into this
        /// function resulting in blacklisting of token in every request in every controller

        public async Task Authorize(HttpContext context)
        {
            using var service = _scope.CreateScope();
            var tokenService = service.ServiceProvider.GetRequiredService<IUserAccountService>();
            await tokenService.IsBlackListed(context);
        }
    }
}
