using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Repository.Audit
{
    public interface IAuditMasterRepository : IBaseRepository<AuditMaster>
    {
        List<AuditReportHeading> GetReportHeadingById(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditReportHeading> GetReportHeadingData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditIssue> GetReportData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditBranchFeedback> GetReportBranchFeedbackData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditBranchFeedback> GetBranchFeedbackDeprtemnetFollowUpData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditBranchFeedback> GetBranchFeedbackAuditResponseFollowUpData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditFeedback> GetReportAuditFeedbackData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        AuditMaster MultiplePost(AuditMaster model);
        AuditMaster MultipleUnPost(AuditMaster model);
        //MultipleAuditApprove
        AuditMaster MultipleAuditApproval(AuditMaster model);
        AuditMaster ReportDataUpdate(AuditMaster model);
        List<AuditUser> GetAuditUserByTeamId(string TeamId, PeramModel pm);
        List<AuditIssueUser> GetAuditIssueUserById(int AuditId);
        List<AuditUser> GetAuditUsers(int Id);
        List<AuditUser> GetAuditIssueUsers(int Id);
        List<AuditApprove> GetAuditById(int id,string UserNamse);
        List<UserRolls> GetUserRoles(string UserName);
        List<UserBranch> GetUserIdbyUserName(string UserName);
        UserBranch UpdateBranchName(UserBranch user);
        HODdetails GetHODdetails();
        List<AuditUser> GetAuditUserByAuditId(string TeamId);
        //List<AuditMaster> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditMaster> GetAuditApproveIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditMaster> GetAuditStatusData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditMaster> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditMaster> IssueGetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<AuditMaster> FeedBackGetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		int GetAuditApprovedDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		int GetStatusDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		int GetAuditIssueDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		int GetAuditFeedBackDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		AuditMaster AuditStatusUpdate(AuditMaster model);
		int GetAuditStatusDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

		bool CheckUnPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue);
		List<AuditResponse> AuditResponseGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
       int AuditResponseGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);  
        MailSetting SaveUrl(MailSetting Url);
		List<UserProfile> GetEamil(UserProfile Email);
		List<UserProfile> GetEamilForIssue(UserProfile Email);
		List<UserProfile> GetEamilForBranchFeedback(UserProfile Email);

		List<MailSetting> GetUrl(MailSetting Email);
		List<MailSetting> GetUrlForIssue(MailSetting Email);
        AuditMaster IssueCompleted(AuditMaster model);
        AuditMaster BranchFeedbackCompleteCompleted(AuditMaster model);
        AuditMaster FeedbackComplete(AuditMaster model);
        AuditMaster UpdateHOD(AuditMaster model);
		AuditReportHeading AuditReportHeadingInsert(AuditReportHeading model);
		AuditReportHeading AuditReportHeadingUpdate(AuditReportHeading model);
        int GetTotoalIssuesById(int id, string UserName);
        DataTable DetailsInformation(ReportModel vm, string[] conditionFields = null, string[] conditionValues = null);


    }
}
