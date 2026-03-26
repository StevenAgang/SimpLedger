using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interfaces.Data;

namespace SimpLedger.Repository.Data
{
    public class BaseData(DatabaseContext context) : IBaseData
    {
        private readonly DatabaseContext _context = context;

        public async Task Save<T>(T data, CancellationToken cancellation = default) where T : class
        {
            _context.Add(data);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task SaveChanges(CancellationToken cancellation = default)
        {
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
