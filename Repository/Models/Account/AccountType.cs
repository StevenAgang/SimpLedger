namespace SimpLedger.Repository.Models.Account
{
    public class AccountType : BaseModel
    {
        public string? Type { get; set; }
        public ICollection<UserAccount>? UserAccounts { get; set; } = [];
    }
}
