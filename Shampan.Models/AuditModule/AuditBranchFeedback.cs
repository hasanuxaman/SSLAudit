using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models.AuditModule;

public class AuditBranchFeedback
{

    public AuditBranchFeedback()
    {
        AttachmentsList = new List<AuditBranchFeedbackAttachments>();
        Audit = new Audit();
        CommonFields = new CommonFields();
    }
    public int Id { get; set; }
    public int AuditId { get; set; }
    [Display(Name = "Issue Heading")]
    public int AuditIssueId { get; set; }
    public int AuditBranchIssueId { get; set; }
    [Display(Name = "Feedback Details")]
    public string IssueDetails { get; set; }
    //[Display(Name = "BranchFeedback Details")]
    [Display(Name = "Details")]
    public string BranchFeedbackDetails { get; set; }
    //[Display(Name = "Implementation Date")]
    [Display(Name = "Implementation Date")]
    public string ImplementationDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    [Display(Name = "Submission Date")]
    public string SubmissionDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    [Display(Name = "Extend Date")]
    public string ExtendDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    [Display(Name = "DeadLine Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public string DeadLineDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    [Display(Name = "Feedback Heading")]
    public string? Heading { get; set; }
    [Display(Name = "Status")]
    public string Status { get; set; }
    [Display(Name = "Issue Status")]
    public string IssueStatus { get; set; }
    [Display(Name = "Issue Status")]
    public string DisplayIssueStatus { get; set; }   
    public IList<IFormFile>? Attachments { get; set; }
    public string Operation { get; set; } = "add";
    public string Edit { get; set; } = "";
    public string? ReasonOfUnPost { get; set; }
    public string? UnPostReasonOfBranchFeedback { get; set; }
    public bool IsPosted { get; set; }
    public string IsPost { get; set; }
    [Display(Name = "Extend Reason")]
    public string? DateExtendReason { get; set; }
    public string? ImplementationStatus { get; set; }
    public string IssueRejectComments { get; set; }
    public string IssueApproveComments { get; set; }
    public List<string> IDs { get; set; }
    public Audit Audit { get; set; }
    public List<AuditBranchFeedbackAttachments> AttachmentsList { get; set; }
    public CommonFields CommonFields { set; get; }



    //CommonFields  
    public string PostedBy { get; set; }
    public string PostedOn { get; set; }
    public string PostedFrom { get; set; }   
    public string AuditName { get; set; }
    public string IssueName { get; set; }
    public string UserName { get; set; }
    public string CreatedBy { get; set; }
    public string TeamCheck { get; set; }
    public string CreatedOn { get; set; }
    public int BranchEmailIssueId { get; set; }
    public string TeamId { get; set; } 
    public bool IsTeam { get; set; }
    public bool IsReport { get; set; }
    public string RejectedDate { get; set; }


}