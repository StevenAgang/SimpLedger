using SimpLedger.Repository.Models.Enterprise;

namespace SimpLedger.Repository.Interfaces.Data.Enterprise
{
    public interface IEmployeeData : IBaseData
    {
        Task<Employee> GetEmployeeByUserId(int userId, bool withTracking);
    }
}
