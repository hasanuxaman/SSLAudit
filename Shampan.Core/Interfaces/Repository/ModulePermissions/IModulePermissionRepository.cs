using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.ModulePermissions
{
	public interface IModulePermissionRepository : IBaseRepository<ModulePermission>
	{
        List<ModulePermission> GetModulListData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
