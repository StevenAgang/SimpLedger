using SimpLedger.Repository.Models.Enterprise;
using SimpLedger.Repository.Models.Sales;
using System.ComponentModel.DataAnnotations.Schema;
using Sl = SimpLedger.Repository.Models.Sales;

namespace SimpLedger.Repository.Models.Inventory
{
    public class Inventory : BaseModel
    {
        [ForeignKey("Branch")]
        public int Branch_Id { get; set; }
        public Branch? Branch { get; set; }
        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product? Product { get; set; }
        public int Stock { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public double CostPrice { get; set; }

        public ICollection<SalesItem>? SalesItems { get; set; } = [];
    }
}
