using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shampan.Models.AuditModule;

public class AuditMaster
{
    public int Id { get; set; }
    [Display(Name = "Code(Auto Generate)")]
    public string? Code { get; set; }
    [Display(Name = "Audit Name")]
    public string Name { get; set; }
    [Display(Name = "Audit Type")]
    public string AuditTypeId { get; set; }

    [Display(Name = "Planned")]
    public bool IsPlaned { get; set; }
    public string Location { get; set; }
    [Display(Name = "Team Name")]
    public string TeamId { get; set; }

    [Display(Name = "Branch Name")]
    public string BranchID { get; set; }
	[Display(Name = "Branch Name")]
	public string BranchIDStatus { get; set; }

    [Display(Name = "Audit Start Date")]
    public string StartDate { get; set; }

    [Display(Name = "Audit End Date")]
    public string EndDate { get; set; }
    public string Duratiom { get; set; }
    [Display(Name = "Business Target")]
    public string? BusinessTarget { get; set; }

    [Display(Name = "Audit Status")]
    public string AuditStatus { get; set; }
    [Display(Name = "Report Status")]
    public string ReportStatus { get; set; }
    public string? Remarks { get; set; }

    [Display(Name = "Audit Type Name")]
    public string AuditTypeName { get; set; }

    [Display(Name = "Team Name")]
    public string TeamName { get; set; }
    public string IsPosted { get; set; }
    public string PostedBy { get; set; }
    public string PostedOn { get; set; }
    public string PostedFrom { get; set; }
    public string? ReasonOfUnPost { get; set; }
    public string CompanyId { get; set; }
    public string IsPost { get; set; }
	[Display(Name = "Approve Comments")]
	public string? CommentsL1 { set; get; }
	[Display(Name = "Reject Comments")]
	public string? RejectedComments { set; get; }
    public string? UserId { set; get; }
    public string? Email { set; get; }
    [Display(Name = "Issue Completed")]
    public bool IssueCompleted { set; get; }
	[Display(Name = "Feedback Completed")]
	public bool FeedbackCompleted { set; get; }
    public string? ApproveStatus { set; get; }
	[Display(Name = "Report Status")]
	public string ReportStatusModal { get; set; }
    public string Operation { get; set; }
    public string Edit { get; set; } = "Audit";
    public string IssuePriority { get; set; } = "Audit";
    public int PlanCount { get; set; }
    public string Url { get; set; }
	public bool IsApprovedL4 { get; set; }
    [Display(Name = "Branch Feedback Completed")]
    public bool BranchFeedbackCompleted { get; set; }
    public bool IsCompleteIssue { get; set; }
    [Display(Name = "Feedback Completed")]
    public bool IsCompleteIssueTeamFeedback { get; set; }
    public bool IsCompleteIssueBranchFeedback { get; set; }
    public bool IssueCompletedCheck { get; set; }
    public bool IsHOD { get; set; }
    public bool IsReviewer { get; set; }
    public bool IsBranchComplete { get; set; }
    public string BranchName { get; set; }
    public string ApprovalStatus { get; set; }
    public string AuditReportDetails { get; set; }
	public string AuditSecondReportDetails { get; set; }
	public string AuditAnnexureDetails { get; set; }
	public string IssueStatus { get; set; }
    public bool IsTeam { get; set; }
    public bool IsIssueUser { get; set; }
    public bool IsRejected { get; set; }
    public bool IssueIsRejected { get; set; }
    public bool BFIsRejected { get; set; }
    public string HideMessage { get; set; }
    public string TeamValue { get; set; }
    public int TotalIssues { get; set; }
    public string Status { get; set; } = "";
    [Display(Name = "From Date")]
    public string FromDate { get; set; }
    [Display(Name = "To Date")]
    public string ToDate { get; set; }


    public IList<IFormFile>? Attachments { get; set; }
    public List<string> IDs { get; set; }
    public List<string> IssueIDs { get; set; }
    public List<AuditAreas> AuditAreas { get; set; }
    public List<AuditIssue> auditIssue { get; set; }
    public List<AuditMaster> auditMasterList { get; set; }
    public Audit Audit { get; set; }
    public Approval Approval;

    public AuditMaster()
    {
        auditIssue = new List<AuditIssue>();
        AuditAreas = new List<AuditAreas>();
        Audit = new Audit();
        Approval = new Approval();
        auditMasterList = new List<AuditMaster>();
    }

}