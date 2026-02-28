using SimpLedger.Repository.ViewModel;

namespace SimpLedger.Repository.Configuration.Helper
{
    public class ResponseHelper
    {
        public ResponseViewModel response(int status, bool success, string? message, object? content)
        {
            return new ResponseViewModel
            {
                status = status,
                success = success,
                message = message,
                content = content
            };
        }
    }
}
