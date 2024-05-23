using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Team
{
	public interface ITeamsRepository : IBaseRepository<Teams>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
		string GetAuditTypeeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue);
		string GetBranchByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue);

    }
}
