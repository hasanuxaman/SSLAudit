using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shampan.Models.AuditModule;

namespace Shampan.Models;

public class AuditIssue
{
    public AuditIssue()
    {
        AuditBranchFeedbackList = new List<AuditBranchFeedback>();
        AuditFeedbackList = new List<AuditFeedback>();
        AuditBranchFeedbackDepartmentFollowUpList = new List<AuditBranchFeedback>();
        AuditBranchFeedbackAuditResponserFollwoUpList = new List<AuditBranchFeedback>();
        Audit = new Audit();
        AttachmentsList = new List<AuditIssueAttachments>();      
        AuditMaster = new AuditMaster();
        AuditBranchFeedback = new AuditBranchFeedback();
    }

    public int Id { get; set; }
    [Display(Name = "Audit Name")]
    public int AuditId { get; set; }
    public string AuditStatus { get; set; }

    [Display(Name = "Issue Heading")]
    public string IssueName { get; set; }
    public string Name { get; set; }

    [Display(Name = "Issue Details")]
    public string IssueDetails { get; set; }
    [Display(Name = "Issue Details")]
    public string FeedbackDetails { get; set; }
    public string Issues { get; set; }

	[Display(Name = "Issue Detail")]
	public string IssueDetailsUpdate { get; set; }

    [Display(Name = "Date Of Submission")]
    public string DateOfSubmission { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    [Display(Name = "Issue Open Date")]
    public string IssueOpenDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    [Display(Name = "Issue Dead Line(Branch)")]
    public string IssueDeadLine { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    [Display(Name = "Implementation Date(Branch)")]
    public string ImplementationDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

    [Display(Name = "Issue Rating")]
    public string IssuePriority { get; set; }
	[Display(Name = "Issue Status")]
	public string IssueStatus { get; set; }

	public string IssueDetailsData { get; set; }
	public string? Risk { get; set; }

    [Display(Name = "Reporting Category")]
    //[Display(Name = "Audit Type")]
    public string AuditType { get; set; }
	public IList<IFormFile>? Attachments { get; set; }
    public List<AuditIssueAttachments> AttachmentsList { get; set; }
    public Audit Audit { get; set; }
    public AuditMaster AuditMaster { get; set; }
    public AuditBranchFeedback AuditBranchFeedback { get; set; }
    //NewListAddForReport
    public List<AuditBranchFeedback> AuditBranchFeedbackList { get; set; }
    public List<AuditFeedback> AuditFeedbackList { get; set; }
    public List<AuditBranchFeedback> AuditBranchFeedbackDepartmentFollowUpList { get; set; }
    public List<AuditBranchFeedback> AuditBranchFeedbackAuditResponserFollwoUpList { get; set; }
    //End
    public AuditBranchFeedback AuditBranchFeedbackDepartmentFollowUp { get; set; }
    public AuditBranchFeedback AuditBranchFeedbackAuditResponserFollwoUp { get; set; }
    public AuditFeedback AuditFeedback { get; set; }
    public List<string> IDs { get; set; }
    [Display(Name = "Investigation/Forensis")]
    public bool InvestigationOrForensis { get; set; }
    [Display(Name = "Stratigic Meeting")]
    public bool StratigicMeeting { get; set; }
    [Display(Name = "Management ReviewMeeting")]
    public bool ManagementReviewMeeting { get; set; }
    [Display(Name = "Other Meeting")]
    public bool OtherMeeting { get; set; }
    public bool Training { get; set; }
    public bool Operational { get; set; }
    public bool Compliance { get; set; }
    public bool Financial { get; set; }
    public string? IsPost { get; set; }
    public string? ReasonOfUnPost { get; set; }
    public string? UnPostReasonOfIssue { get; set; }
    public string Edit { get; set; } = "";
    public string Operation { get; set; } = "add";
    public bool IsPosted { get; set; }



    //CommonFields
    public string PostedBy { get; set; }
    public string PostedOn { get; set; }
    public string PostedFrom { get; set; }
    public string? CreatedBy { get; set; }
    public string ReportStatus { get; set; }
	[Display(Name = "Report Status")]
	public string ReportStatusModal { get; set; }
	[Display(Name = "Issue Priority")]
	public string IssuePriorityUpdate { get; set; } 
	public string SelectedAuditTypes { get; set; } 
	public string AllAuditTypes { get; set; }
	public string EnumValue { get; set; }
    public string CreatedOn { get; set; }
    public string IssueProcess { get; set; }
    public string Code { get; set; }
    public string BranchFeedBackDetails { get; set; }
    public string FeedbackHeading { get; set; }
    public string BranchFeedBackHeading { get; set; }
    public bool IssueCheck { get; set; }
    public int TeamId { get; set; }

    public string OperationalText { get; set; }
    public string ComplianceText { get; set; }
    public string FinancialText { get; set; }
}


