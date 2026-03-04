using SimpLedger.Repository.Configuration.Helper;

namespace SimpLedger.Middleware
{
    public class EventsMiddleware(RequestDelegate next,ResponseHelper response)
    {
        private readonly ResponseHelper _response = response;
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync($"{_response.Success(500, false, ex.Message, null)}");
            }
        }

        public async static Task RunProcess(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                try
                {

                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    //context.Response.WriteAsJsonAsync(_response.Success(200,false,ex.Message,null));
                }
                await next();
            });
        }
    }
}
