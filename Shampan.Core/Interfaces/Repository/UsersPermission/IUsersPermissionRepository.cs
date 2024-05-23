using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.UsersPermission
{
	public interface IUsersPermissionRepository : IBaseRepository<Users>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        List<SubmanuList> GetNodesIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int GetNodesIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        SubmanuList UpdateNoes(SubmanuList model);
        bool CheckUserStatus(Users model);
        UserProfileAttachments ImageInsert(UserProfileAttachments model);
        UserProfileAttachments ImageUpdate(UserProfileAttachments model);
        List<UserProfileAttachments> GetAllImage(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<UserProfileAttachments> GetImageByUserName(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<UserProfileAttachments> GetUserIdByName(string Name);
        UserProfile UserProfileDeActivate(UserProfile model);

    }
}
