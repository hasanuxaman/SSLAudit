using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Team
{
	public interface ITeamsService : IBaseService<Teams>
	{

		string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue);
		string GetAuditTypeeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue);
		string GetBranchByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue);


	}
}
