using Microsoft.AspNetCore.Mvc.Filters;
using SimpLedger.Repository.Configurations.Exception_Extender;

namespace SimpLedger.Repository.Configurations.AttributeExtender
{
    public class TransactionAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();

            if(dbContext == null)
            {
                await next();
                return;
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            var result = await next();

            if(result.Exception == null || result.ExceptionHandled || result.Exception is AcceptedDbCommit)
            {
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();

                //For development
                //throw new Exception($"Transaction rolled back due to an error during the process. {result.Exception} ");

                //For production
                if(result.Exception is not BadRequest && result.Exception is not Conflict && result.Exception is not ResourceNotFound)
                {
                    throw new Exception("Something went wrong please try again later");
                }
                return;
            }
        }
    }
}
