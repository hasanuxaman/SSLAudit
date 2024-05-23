using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.TeamMember
{
    public interface ITeamMembersRepository : IBaseRepository<TeamMembers>
    {
        List<UserProfileAttachments> GetUserProfileName(int TeamId);
    }
}
