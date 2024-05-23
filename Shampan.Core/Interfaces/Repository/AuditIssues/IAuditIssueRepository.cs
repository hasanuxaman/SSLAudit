using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Repository.AuditIssues
{
    public interface IAuditIssueRepository : IBaseRepository<AuditIssue>
    {
        AuditIssue MultiplePost(AuditIssue objAdvances);
		AuditIssue MultipleUnPost(AuditIssue model);
		AuditIssue ReportStatusUpdate(AuditIssue model);
        List<AuditIssue> GetAuditIssueIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int GetAuditIssueIndexCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        AuditIssue IssueApproveStatusUpdate(AuditIssue model);
        IssueRejectComments IssueApprove(IssueRejectComments model);
        IssueRejectComments IssueRejectStatusUpdate(IssueRejectComments model);
        //ForExcel
        List<AuditIssue> GetExcelIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int GetExcelIndexCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditIssue> GetIssuesByAuditId(AuditMaster model);


    }
}
