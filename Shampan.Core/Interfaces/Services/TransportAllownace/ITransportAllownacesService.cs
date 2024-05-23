using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.TransportAllownace
{
	public interface ITransportAllownacesService : IBaseService<TransportAllownaces>
	{
		ResultModel<TransportAllownaces> MultiplePost(TransportAllownaces model);
		ResultModel<TransportAllownaces> MultipleUnPost(TransportAllownaces model);
		ResultModel<List<TransportAllownaces>> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<TransportAllownaces>> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<TransportAllownaces>> GetTADAForReports(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
