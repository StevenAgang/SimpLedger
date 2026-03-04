using SimpLedger.Repository.Configuration.Helper;

namespace SimpLedger.Middleware
{
    public class RequestMiddleware(RequestDelegate next, ResponseHelper response)
    {
        private readonly RequestDelegate _next = next;
        private readonly ResponseHelper _response = response;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (Authorize() == false) throw new UnauthorizedAccessException();
                await _next(context);
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

        public bool Authorize()
        {
            return true;
        }
    }
}
