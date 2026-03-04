namespace SimpLedger.Repository.Models.Inventory
{
    public class Product : BaseModel
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public ICollection<Inventory>? Inventories { get; set; } = [];
    } 
}
