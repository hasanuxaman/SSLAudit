using Shampan.Models.AuditModule;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.AuditFeedbackService
{
    public interface IAuditFeedbackService : IBaseService<Models.AuditModule.AuditFeedback>
    {
        ResultModel<AuditIssue> GetAuditLastFeedback(AuditIssue model);
        ResultModel<AuditFeedback> MultiplePost(AuditFeedback model);
        ResultModel<AuditFeedback> MultipleUnPost(AuditFeedback model);
        ResultModel<List<AuditFeedback>> GetAuditFeedbackByAuditId(AuditMaster model);

    }
}
