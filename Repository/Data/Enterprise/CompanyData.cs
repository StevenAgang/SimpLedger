using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interfaces.Data;
using SimpLedger.Repository.Interfaces.Data.Enterprise;
using SimpLedger.Repository.Models.Enterprise;

namespace SimpLedger.Repository.Data.Enterprise
{
    public class CompanyData(DatabaseContext context) : BaseData(context), ICompanyData
    {
        private readonly DatabaseContext _context = context;

        public async Task<Company> GetCompanyByUserId(int userId, bool withTracking)
        {
            var company = _context.Company.Include(u => u.UserAccount).Where(c => c.UserAccount_Id == userId);

            if (!withTracking)
            {
                company = company.AsNoTracking();
            }

            var user = await company.FirstOrDefaultAsync(c => c.UserAccount_Id == userId);

            return user;
        }
    }
}
