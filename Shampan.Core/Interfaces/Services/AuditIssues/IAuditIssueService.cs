using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Services.AuditIssues;

public interface IAuditIssueService : IBaseService<AuditIssue>
{
    ResultModel<AuditIssue> MultiplePost(AuditIssue model);
	ResultModel<AuditIssue> MultipleUnPost(AuditIssue model);

	ResultModel<AuditIssue> ReportStatusUpdate(AuditIssue model);
    ResultModel<List<AuditIssue>> GetAuditIssueIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    ResultModel<int> GetAuditIssueIndexCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    ResultModel<AuditIssue> IssueApproveStatusUpdate(AuditIssue model);
    ResultModel<IssueRejectComments> IssueApprove(IssueRejectComments model);
    ResultModel<IssueRejectComments> IssueRejectStatusUpdate(IssueRejectComments model);
    ResultModel<List<AuditIssue>> GetExcelIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    ResultModel<int> GetExcelIndexCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    ResultModel<List<AuditIssue>> GetIssuesByAuditId(AuditMaster model);



}