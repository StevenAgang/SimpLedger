using SimpLedger.Repository.Models.Enterprise;
using System.ComponentModel.DataAnnotations.Schema;
using Inv = SimpLedger.Repository.Models.Inventory;

namespace SimpLedger.Repository.Models.Sales
{
    public class Sale : BaseModel
    {
        [ForeignKey("Branch")]
        public int Branch_Id { get; set; }
        public Branch? Branch { get; set; }
        public double TotalAmount { get; set; }
        public ICollection<SalesItem>? SalesItems { get; set; } = [];
    }
}

    