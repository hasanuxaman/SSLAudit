using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Repository.TransportAllownaceDetails
{
    public interface ITransportAllownaceDetailRepository : IBaseRepository<TransportAllownaceDetail>
    {
        TransportFoodAndOthers InsertFoodAndOthers(TransportFoodAndOthers model);
        LessAdvance InsertLessAdvance(LessAdvance model);
        List<TransportFoodAndOthers> GetAllFoodAndOthers(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<LessAdvance> GetAllLessAdvance(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    }
}
