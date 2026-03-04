namespace SimpLedger.Repository.ViewModel.Sales
{
    public class SalesViewModel
    {
        public int Id { get; set; }
        public int Inventory_Id { get; set; }
        public  double Amount { get; set; }
    }

    public class SalesPostModel
    {
        public int Inventory_Id { get; set; }
        public double Amount { get; set; }
        public int User_Id { get; set; }
    }
}
