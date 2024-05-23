using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Oragnogram
{
	public interface IOragnogramRepository : IBaseRepository<EmployeesHierarchy>
	{
		List<object> GetEmployeesData();
        string CodeGeneration(string CodeGroup, string CodeName);

    }
}
