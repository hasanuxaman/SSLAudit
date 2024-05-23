using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.UsersPermission
{
	public interface IUsersPermissionService : IBaseService<Users>
	{
        ResultModel<List<SubmanuList>> GetNodesIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> GetNodesIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);      
        ResultModel<SubmanuList> UpdateNodes(SubmanuList model);
        void ImageInsert(UserProfile model);
        void ImageUpdate(UserProfile model);
        UserProfileAttachments GetUserIdByName(string Name);
        ResultModel<List<UserProfileAttachments>> GetAllImage(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<UserProfileAttachments>> GetImageByUserName(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<UserProfile> UserProfileDeActivate(UserProfile model);
    }
}
