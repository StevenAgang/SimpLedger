using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interface.Sales;
using SimpLedger.Repository.Models.Sales;
using SimpLedger.Repository.ViewModel.Sales;

namespace SimpLedger.Repository.Service.Sales
{
    public class SalesService(DatabaseContext context) : ISalesService
    {
        private readonly DatabaseContext _context = context;

        public async Task<List<SalesViewModel>> GetAllSales()
        {
            //var sales = await  _context.Sales
            //    .AsNoTracking()
            //    .OrderByDescending(i => i.Id)
            //    .Select(s => new SalesViewModel
            //    {
            //        Id = s.Id,
            //        Inventory_Id = s.Inventory_Id,
            //        Amount = s.Amount,
            //    })
            //    .ToListAsync();

            //return sales;
            throw new NotImplementedException();
        }

        public async Task AddSales(SalesPostModel salesInput)
        {
            // var sales = new Sale
            // {
            //     Inventory_Id = salesInput.Inventory_Id,
            //     Amount = salesInput.Amount,
            //     Created_By = salesInput.user_Id,
            //     Created_At = DateTime.UtcNow,
            // };

            //_context.Add(sales);
            //await _context.SaveChangesAsync();

            throw new NotImplementedException();
        }
    }
}
