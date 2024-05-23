using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Company
{
    public interface ICompanyInfoRepository : IBaseRepository<CompanyInfo>
    {
        List<BranchProfile> GetBranches(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    }
}
