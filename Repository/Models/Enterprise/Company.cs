using SimpLedger.Repository.Models.Account;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpLedger.Repository.Models.Enterprise
{
    public class Company : BaseModel
    {
        [ForeignKey("UserAccount")]
        public int UserAccount_Id { get; set; }
        public UserAccount? UserAccount { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        public ICollection<Branch>? Branches { get; set; } = [];
    }
}
