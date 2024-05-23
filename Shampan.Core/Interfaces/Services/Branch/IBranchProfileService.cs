using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Branch
{
   public interface IBranchProfileService : IBaseService<BranchProfile>
    {
		ResultModel<BranchProfile> BranchProfileUpdate(BranchProfile model);

	}
}
