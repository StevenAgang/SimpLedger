namespace SimpLedger.Repository.Models.Auth
{
    public class ExpiredToken : BaseModel
    {
        public required string Jti { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
