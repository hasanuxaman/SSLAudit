using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Services.Deshboard
{
    public interface IDeshboardService : IBaseService<AuditMaster>
    {
		List<AuditReports> TotalCompletedOngoingRemaing(string UserName);
		List<AuditReports> UnPlan(string UserName);
		ResultModel<PrePaymentMaster> PrePaymentUpdate(PrePaymentMaster model);
		ResultModel<PrePaymentMaster> PrePaymentInsert(PrePaymentMaster model);
        ResultModel<int> TotalAdditionalPaymentCount();
        ResultModel<PrePaymentMaster> NonFinancialUpdate(PrePaymentMaster model);
		ResultModel<PrePaymentMaster> NonFinancialInsert(PrePaymentMaster model);
		ResultModel<List<PrePaymentMaster>> PrePaymentGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> PrePaymentGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int PrePaymentGetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<PrePaymentMaster>> PrePaymentGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		ResultModel<List<PrePaymentMaster>> NonFinancialGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		ResultModel<int> NonFinancialGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		int NonFinancialGetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		ResultModel<List<PrePaymentMaster>> NonFinancialGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditComponent> GetUnPlanAuditComponents();
        int TotalAuditApproved(string UserName);
		int TotalAuditRejected(string UserName);
		int TotalIssueRejected(string UserName);
		int DeadLineForResponse(string UserName);
		int TotalRisk(string UserName);
		int BeforeDeadLineIssue(string UserName);
		int MissDeadLineIssues(string UserName);
		int TotalFollowUpAudit(string UserName);
		int TotalPendingIssueReview(string UserName);
		int PendingForAuditFeedback(string UserName);
		int FollowUpAuditIssues(string UserName);
		int PendingForApproval(string UserName);
		int PendingAuditResponse(string UserName);
		int PendingAuditApproval(string UserName);
		int FinalAuidtApproved(string UserName);
		int PendingForReviewerFeedback(string UserName);
		int PendingForIssueApproval(string UserName);
        PrePaymentMaster GetPrePayment();
        PrepaymentReview PrepaymentReview();
        UserBranch GetBranchName(string UserId);
        bool AuditBranchUserGetAll(string UserName);
        ResultModel<PrepaymentReview> PrepaymentReviewInsert(PrepaymentReview model);


    }
}
