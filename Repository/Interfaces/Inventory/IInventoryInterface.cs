using SimpLedger.Repository.ViewModel.Inventory;

namespace SimpLedger.Repository.Interface.Inventory
{
    public interface IInventoryInterface
    {
        /// <summary>
        ///  A function that will return all items in database
        /// </summary>
        /// <returns></returns>
        Task<InventoryViewModel> GetAllInventory();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task AddItem(InventoryPostModel item);
    }
}
