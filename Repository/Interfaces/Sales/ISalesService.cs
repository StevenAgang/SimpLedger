using SimpLedger.Repository.ViewModel.Sales;

namespace SimpLedger.Repository.Interface.Sales
{
    public interface ISalesService
    {
        /// <summary>
        ///  A function that will return all sales information in the database
        /// </summary>
        /// <returns></returns>
        Task<List<SalesViewModel>> GetAllSales();

        /// <summary>
        ///  A function that will add a single sales in the database
        /// </summary>
        /// <param name="sales">Sales object consist of inventory id and the amount of specific item in the inventory</param>
        /// <returns>This method will not return anything</returns>

        Task AddSales(SalesPostModel sales);
    }
}
