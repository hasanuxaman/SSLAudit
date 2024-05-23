using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Advance
{
	public interface IAdvancesRepository : IBaseRepository<Advances>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        Advances MultiplePost(Advances objAdvances);
		Advances MultipleUnPost(Advances model);
		List<Advances> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<Advances> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

	}
}
