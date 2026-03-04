using SimpLedger.Repository.Models.Sales;
using System.ComponentModel.DataAnnotations.Schema;
using Inv = SimpLedger.Repository.Models.Inventory;

namespace SimpLedger.Repository.Models.Enterprise
{
    public class Branch : BaseModel
    {
        [ForeignKey("Company")]
        public int Company_Id { get; set; }
        public Company? Company { get; set; }
        public string? Address { get; set; }
        public ICollection<Sale>? Sales { get; set; } = [];
        public ICollection<Inv.Inventory>? Inventories { get; set; } = [];
        public ICollection<Employee>? Employees { get; set; } = [];
    }
}
