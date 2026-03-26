using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interfaces.Data.Account;
using SimpLedger.Repository.Models.Account;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace SimpLedger.Repository.Data.Account
{
    public class UserAccountData(DatabaseContext context) : BaseData(context) ,IUserAccountData
    {
        public DatabaseContext _context = context;

        private IQueryable<UserAccount> BaseQuery(bool withTracking, bool isActive)
        {
            var query =  _context.UserAccount.Where(u => u.IsActive == isActive);

            if (!withTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public async Task<UserAccount> GetUserActiveByEmail(string email, bool withTracking, CancellationToken cancellation = default)
        {
            var user =  await BaseQuery(withTracking, true).FirstOrDefaultAsync(u => u.Email == email, cancellation);
            return user;
        }

        public async Task<UserAccount> GetUserNotActiveByEmail(string email, bool withTracking, CancellationToken cancellation = default)
        {
            var user = await BaseQuery(withTracking, false).FirstOrDefaultAsync(u => u.Email == email, cancellation);

            return user;
        }

        public async Task<UserAccount> GetUserActiveById(int userId, bool withTracking, CancellationToken cancellation = default)
        {
            var user = await BaseQuery(withTracking, true).FirstOrDefaultAsync(u => u.Id == userId, cancellation);

            return user;
        }
    }
}
