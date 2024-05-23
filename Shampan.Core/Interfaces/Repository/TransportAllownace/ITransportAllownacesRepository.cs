using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.TransportAllownace
{
	public interface ITransportAllownacesRepository : IBaseRepository<TransportAllownaces>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
		TransportAllownaces MultiplePost(TransportAllownaces objTransport);
		TransportAllownaces MultipleUnPost(TransportAllownaces model);
		List<TransportAllownaces> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<TransportAllownaces> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<TransportAllownaces> GetTADAForReports(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
