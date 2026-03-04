using SimpLedger.Repository.Models.Enterprise;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpLedger.Repository.Models.Account
{
    public class UserAccount : BaseModel
    {
        [ForeignKey("AccountType")]
        public int AccountType_Id { get; set; }
        public AccountType? AccountType { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }

        public Company? Company { get; set; }
        public Employee? Employee { get; set; }

    }
}
