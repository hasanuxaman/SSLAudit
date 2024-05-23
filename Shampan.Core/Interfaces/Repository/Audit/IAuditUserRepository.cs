using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Repository.Audit
{
    public interface IAuditUserRepository : IBaseRepository<AuditUser>
    {
		//List<AuditUser> GetAuditUsers(string tableName, string[] conditionalFields, string[] conditionalValue);
	}
}
