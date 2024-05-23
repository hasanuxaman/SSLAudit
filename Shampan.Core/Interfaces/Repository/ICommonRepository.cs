using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Repository
{
    public interface ICommonRepository : IcommonBaseRepository<CommonDropDown>
    {
        List<CommonDropDown>? GetAuditType(bool isPlanned = true);
        List<CommonDropDown>? GetTeams();
        List<CommonDropDown>? GetReportStatus();
        List<CommonDropDown>? GetCircularType();
        List<CommonDropDown>? GetAuditStatus();
        List<CommonDropDown>? GetAuditName();
        List<CommonDropDown>? GetIssuePriority();
        List<CommonDropDown>? GetIssueStatus();
        List<CommonDropDown>? GetStatus();
        List<CommonDropDown>? GetAuditTypes();
        List<CommonDropDown>? GetIssues(string auditId);
        List<CommonDropDown>? GetBranchFeedbackIssues(string auditId,string userName,bool team);


    }
}
