using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Advance
{
	public interface IAdvancesService : IBaseService<Advances>
	{
        ResultModel<Advances> MultiplePost(Advances model);
		ResultModel<Advances> MultipleUnPost(Advances model);
		ResultModel<List<Advances>> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<Advances>> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

	}
}
