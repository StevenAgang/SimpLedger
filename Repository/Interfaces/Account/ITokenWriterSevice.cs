namespace SimpLedger.Repository.Interfaces.Account
{
    public interface ITokenWriterSevice
    {
        string GenerateToken(string id, string name);
    }
}
