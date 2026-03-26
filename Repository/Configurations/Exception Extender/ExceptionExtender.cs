namespace SimpLedger.Repository.Configurations.Exception_Extender
{
    public class ExceptionExtender
    {
    }

    public class BadRequest(string message, Exception? innerException = null) : Exception(message, innerException) { }
    public class Conflict(string message, Exception? innerException = null) : Exception(message, innerException) { }
    public class ResourceNotFound(string message, Exception? innerException = null) : Exception(message, innerException);
    public class AcceptedDbCommit(string message, Exception? innerException = null): Exception(message, innerException);
}
