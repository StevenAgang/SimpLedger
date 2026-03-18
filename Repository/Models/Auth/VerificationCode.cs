using SimpLedger.Repository.Models.Account;
using SimpLedger.Repository.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpLedger.Repository.Models.Verification
{
    public class VerificationCode : BaseModel
    {
        [ForeignKey("UserAccount")]
        public int UserAccount_Id { get; set; }
        public UserAccount? UserAccount { get; set; }
        public string? Token { get; set; }
        public int Code { get; set; }
        public DateTime? ExpiresIn { get;set; }
    }
}
