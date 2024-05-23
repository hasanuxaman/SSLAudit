using System.ComponentModel.DataAnnotations;

namespace Shampan.Models.AuditModule;

public class AuditUser
{


    public AuditUser()
    {
        Audit= new Audit();;
    }
    public int Id { get; set; }

    public int AuditId { get; set; }


    [Display(Name = "Users")]
    public string? UserId { get; set; }


    [Display(Name = "Email Address")]

    public string EmailAddress { get; set; }

    public string UserName { get; set; }

    public string? Remarks { get; set; }

    [Display(Name = "Issue Priority")]
    public string IssuePriority { get; set; }
	[Display(Name = "Issue Status")]
	public string IssueStatus { get; set; }

	public Audit Audit { get; set; }

    public string Operation { get; set; } = "add";
    public string Edit { get; set; } = "";
    public string TeamId { get; set; }

}

public class AuditIssueUser
{


    public AuditIssueUser()
    {
        Issue = new Issue();
        AuditIssueUserList = new List<AuditIssueUser>();
    }
    public List<AuditIssueUser> AuditIssueUserList { set; get; }

    public int Id { get; set; }

    public int AuditId { get; set; }
    public int AuditIssueId { get; set; }

    [Display(Name = "Users")]
    public string? UserId { get; set; }


    [Display(Name = "Email Address")]

    public string EmailAddress { get; set; }

    public string UserName { get; set; }

    public string? Remarks { get; set; }

    [Display(Name = "Issue Priority")]
    public string IssuePriority { get; set; }

    public Issue Issue { get; set; }

    public string Operation { get; set; } = "add";
    public string Edit { get; set; } = "";
    public string Status { get; set; } = "";

}
