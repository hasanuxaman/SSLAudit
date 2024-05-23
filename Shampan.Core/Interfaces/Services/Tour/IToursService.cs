using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Tour
{
	public interface IToursService : IBaseService<Tours>
	{
		ResultModel<Tours> MultiplePost(Tours model);
		ResultModel<Tours> MultipleUnPost(Tours model);
		ResultModel<List<Tours>> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<Tours>> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        ResultModel<List<Tours>> GetAllForReports(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
