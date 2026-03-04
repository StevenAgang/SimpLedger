using SimpLedger.Repository.ViewModel;

namespace SimpLedger.Repository.Configuration.Helper
{
    public class ResponseHelper
    {
        public ResponseViewModel Success(int status, bool success, string? message, object? content)
        {

            return new ResponseViewModel
            {
                Status = status,
                Success = success,
                Message = message,
                Content = content
            };
        }
    }
}
