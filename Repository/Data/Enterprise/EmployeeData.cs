using Microsoft.EntityFrameworkCore;
using SimpLedger.Repository.Interfaces.Data;
using SimpLedger.Repository.Interfaces.Data.Enterprise;
using SimpLedger.Repository.Models.Enterprise;

namespace SimpLedger.Repository.Data.Enterprise
{
    public class EmployeeData(DatabaseContext context) : BaseData(context), IEmployeeData
    {
        private readonly DatabaseContext _context = context;

        public async Task<Employee> GetEmployeeByUserId(int userId, bool withTracking)
        {
            var employee = _context.Employee.Include(u => u.UserAccount).Where(e => e.UserAccount_Id == userId);

            if (!withTracking)
            {
                employee = employee.AsNoTracking();
            }

            return await employee.FirstOrDefaultAsync();
        }
    }
}
