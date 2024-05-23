using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Services.Audit
{
    public interface IAuditMasterService : IBaseService<AuditMaster>
    {
        ResultModel<List<AuditReportHeading>> GetReportHeadingById(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditReportHeading>> GetReportHeadingData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditIssue>> GetReportData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditBranchFeedback>> GetReportBranchFeedbackData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditBranchFeedback>> GetBranchFeedbackDeprtemnetFollowUpData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditBranchFeedback>> GetBranchFeedbackAuditResponseFollowUpData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditFeedback>> GetReportAuditFeedbackData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<AuditMaster> MultiplePost(AuditMaster model);
        ResultModel<AuditMaster> MultipleUnPost(AuditMaster model);
        ResultModel<AuditMaster> MultipleAuditApproval(AuditMaster model);
        ResultModel<AuditMaster> ReportDataUpdate(AuditMaster model);
        ResultModel<List<AuditUser>> GetAuditUserTeamId(string TeamId, PeramModel pm = null);
        ResultModel<List<AuditIssueUser>> GetAuditIssueUserById(int AuditId);
        ResultModel<List<AuditApprove>> GetAuditById(int id, string UserName);
        ResultModel<List<UserRolls>> GetUserRoles(string UserName);
        ResultModel<List<UserBranch>> GetUserIdbyUserName(string UserName);
        ResultModel<UserBranch> UpdateBranchName(UserBranch user);
        ResultModel<List<AuditUser>> GetAuditUsers(int Id);
        ResultModel<List<AuditUser>> GetAuditIssueUsers(int Id);
        ResultModel<List<AuditUser>> GetAuditUserAuditId(string TeamId);
        ResultModel<List<AuditMaster>> GetAuditApproveIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditMaster>> GetAuditStatusData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditMaster>> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditMaster>> IssueGetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditMaster>> FeedBackGetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> GetAuditApprovDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> GetAuditIssueDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> GetAuditFeedBackDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<AuditMaster> AuditStatusUpdate(AuditMaster model);
        ResultModel<int> GetAuditStatusDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> GetStatusDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditResponse>> AuditResponseGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> AuditResponseGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<MailSetting> SaveUrl(MailSetting url);
        ResultModel<List<UserProfile>> GetEamil(UserProfile Email);
        ResultModel<List<UserProfile>> GetEamilForIssue(UserProfile Url);
        ResultModel<List<UserProfile>> GetEamilForBranchFeedback(UserProfile Url);
        ResultModel<List<MailSetting>> GetUrl(MailSetting Url);
        ResultModel<AuditMaster> IssueCompleted(AuditMaster model);
        ResultModel<AuditMaster> BranchFeedbackCompleteCompleted(AuditMaster model);
        ResultModel<AuditMaster> FeedbackComplete(AuditMaster model);
        ResultModel<AuditMaster> UpdateHOD(AuditMaster model);
        ResultModel<HODdetails> GetHODdetails();
        ResultModel<AuditReportHeading> AuditReportHeadingInsert(AuditReportHeading model);
        ResultModel<AuditReportHeading> AuditReportHeadingUpdate(AuditReportHeading model);
        int GetTotoalIssuesById(int id, string UserName);
        ResultModel<DataTable> DetailsInformation(ReportModel model);

    }
}
