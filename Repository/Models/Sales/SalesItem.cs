using System.ComponentModel.DataAnnotations.Schema;
using Inv = SimpLedger.Repository.Models.Inventory;

namespace SimpLedger.Repository.Models.Sales
{
    public class SalesItem : BaseModel
    {
        [ForeignKey("Sales")]
        public int Sales_Id { get; set; }
        public Sale? Sales { get; set; }
        [ForeignKey("Inventory")]
        public int Inventory_Id { get; set; }
        public Inv.Inventory? Inventory { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Subtotal { get; set; }
    }
}
