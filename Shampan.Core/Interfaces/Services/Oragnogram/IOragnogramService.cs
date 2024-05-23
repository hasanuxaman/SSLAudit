using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Oragnogram
{
	public interface IOragnogramService : IBaseService<EmployeesHierarchy>
	{
		ResultModel<List<object>> GetEmployeesData();

	}
}
