using SimpLedger.Repository.Models.Enterprise;

namespace SimpLedger.Repository.Interfaces.Data.Enterprise
{
    public interface ICompanyData : IBaseData
    {
        Task<Company> GetCompanyByUserId(int userId, bool withTracking);
    }
}
