using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Services.UserRoll
{
	public interface IUserRollsService : IBaseService<UserRolls>
	{
		List<UserManuInfo> GetUserManu(string Username);
		List<SubmanuList> GetUserSubManu(string Username);



		List<AuditComponent> GetAuditComponents();
		
		List<BranchCoverage> GetBranchCoverages();
		List<BasicData> GetBasicData();
		List<SpecialEngagements> GetSpecialEngagements();
		List<AddHocEngagements> GetAddHocEngagements();

		int GetIssues(string UserName);
	}
}
