using SimpLedger.Repository.Models.Account;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpLedger.Repository.Models.Enterprise
{
    public class Employee : BaseModel
    {
        [ForeignKey("UserAccount")]
        public int UserAccount_Id { get; set; }
        public UserAccount? UserAccount { get; set; }
        [ForeignKey("Branch")]
        public int? Branch_Id { get; set; }
        public Branch? Branch { get; set; }

    }
}
