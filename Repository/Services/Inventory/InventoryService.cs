using SimpLedger.Repository.Interface.Inventory;
using SimpLedger.Repository.ViewModel.Inventory;

namespace SimpLedger.Repository.Service.Inventory
{
    public class InventoryService(DatabaseContext context) : IInventoryInterface
    {
        private readonly DatabaseContext _context = context;

        public async Task<InventoryViewModel> GetAllInventory()
        {
            throw new NotImplementedException();
        }

        public async Task AddItem(InventoryPostModel item)
        {

        }
    }
}
