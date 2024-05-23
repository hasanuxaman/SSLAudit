using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Repository.AuditFeedbackRepo
{
    public interface IAuditBranchFeedbackRepository : IBaseRepository<AuditBranchFeedback>
    {
        AuditBranchFeedback MultiplePost(AuditBranchFeedback objAdvances);
        AuditBranchFeedback MultipleUnPost(AuditBranchFeedback model);
		List<AuditBranchFeedback> GetAuditIssueUsers(string tableName, string[] conditionalFields, string[] conditionalValue);
        List<AuditBranchFeedback> GetAuditIssuesFromBranch(AuditBranchFeedback model);
        AuditBranchFeedback GetTopOneBranchId(int id);
        List<IssueRejectComments> GetIssueRejectComments(int model);
        List<IssueRejectComments> GetIssueApproveComments(int model);

    }
}
