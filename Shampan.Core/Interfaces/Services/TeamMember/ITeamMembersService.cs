using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Services.TeamMember
{
    public interface ITeamMembersService : IBaseService<TeamMembers>
    {
        ResultModel<List<UserProfileAttachments>> GetUserProfileName(int TeamId);

    }
}
