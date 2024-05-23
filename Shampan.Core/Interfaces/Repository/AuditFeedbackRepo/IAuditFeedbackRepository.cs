using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Repository.AuditFeedbackRepo
{
    public interface IAuditFeedbackRepository : IBaseRepository<AuditFeedback>
    {
		List<AuditFeedback> GetAuditUsers(string tableName, string[] conditionalFields, string[] conditionalValue);

        List<AuditIssue> GetAuditLastFeedback(AuditIssue Email);
        AuditFeedback MultiplePost(AuditFeedback model);
        AuditFeedback MultipleUnPost(AuditFeedback model);
        List<AuditFeedback> GetAuditFeedbackByAuditId(AuditMaster model);

    }
}
