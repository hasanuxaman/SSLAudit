using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Company
{
    public interface ICompanyInfoService : IBaseService<CompanyInfo>
    {
        ResultModel<List<BranchProfile>> GetBranches(string[] conditionalFields, string[] conditionalValue);
    }
}
