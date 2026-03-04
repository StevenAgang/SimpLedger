namespace SimpLedger.Repository.ViewModel.Inventory
{
    public class InventoryViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Stock { get; set; }
        public DateTime? Expiration { get; set; }
    }

    public class InventoryPostModel
    {
        public string? Name { get; set; }
        public int Stock { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
