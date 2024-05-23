using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class UserRolls
	{
        public int Id { get; set; }
        public string UserName { get; set; }

        [Display(Name = "User Name")]
        public string UserId { get; set; }
		[Display(Name = "Audit")]
		public bool IsAudit  { get; set; }
		[Display(Name = "Hierarchy 1")]

		public bool AuditApproval1 { get; set; }
		[Display(Name = "Hierarchy 2")]

		public bool AuditApproval2 { get; set; }
		[Display(Name = "Hierarchy 3")]

		public bool AuditApproval3 { get; set; }
		[Display(Name = "Hierarchy 4")]
		public bool AuditApproval4 { get; set; }
		[Display(Name = "Tour")]

		public bool IsTour { get; set; }
		[Display(Name = "Hierarchy 1")]

		public bool TourApproval1 { get; set; }
		[Display(Name = "Hierarchy 2")]

		public bool TourApproval2 { get; set; }
		[Display(Name = "Hierarchy 3")]

		public bool TourApproval3 { get; set; }
		[Display(Name = "Hierarchy 4")]
		public bool TourApproval4 { get; set; }
		[Display(Name = "Advance")]

		public bool IsAdvance { get; set; }
		[Display(Name = "Hierarchy 1")]

		public bool AdvanceApproval1 { get; set; }
		[Display(Name = "Hierarchy 2")]

		public bool AdvanceApproval2 { get; set; }
		[Display(Name = "Hierarchy 3")]

		public bool AdvanceApproval3 { get; set; }
		[Display(Name = "Hierarchy 4")]

		public bool AdvanceApproval4 { get; set; }
		[Display(Name = "Taransport")]

		public bool IsTa { get; set; }
		[Display(Name = "Hierarchy 1")]

		public bool IsTaApproval1 { get; set; }
		[Display(Name = "Hierarchy 2")]

		public bool IsTaApproval2 { get; set; }
		[Display(Name = "Hierarchy 3")]

		public bool IsTaApproval3 { get; set; }
		[Display(Name = "Hierarchy 4")]

		public bool IsTaApproval4 { get; set; }
		[Display(Name = "Tour Completion Report")]

		public bool IsTourCompletionReport { get; set; }
		[Display(Name = "Hierarchy 1")]

		public bool TourCompletionReportApproval1 { get; set; }
		[Display(Name = "Hierarchy 2")]

		public bool TourCompletionReportApproval2 { get; set; }
		[Display(Name = "Hierarchy 3")]

		public bool TourCompletionReportApproval3 { get; set; }
		[Display(Name = "Hierarchy 4")]

		public bool TourCompletionReportApproval4 { get; set; }
		[Display(Name = "Audit Issue")]

		public bool IsAuditIssue { get; set; }
		[Display(Name = "Hierarchy 1")]

		public bool AuditIssueApproval1 { get; set; }
		[Display(Name = "Hierarchy 2")]

		public bool AuditIssueApproval2 { get; set; }
		[Display(Name = "Hierarchy 3")]

		public bool AuditIssueApproval3 { get; set; }
		[Display(Name = "Hierarchy 4")]

		public bool AuditIssueApproval4 { get; set; }
		[Display(Name = "Audit Feedback")]


		public bool IsAuditFeedback { get; set; }
		[Display(Name = "Hierarchy 1")]

		public bool AuditFeedbackApproval1 { get; set; }
		[Display(Name = "Hierarchy 2")]

		public bool AuditFeedbackApproval2 { get; set; }
		[Display(Name = "Hierarchy 3")]

		public bool AuditFeedbackApproval3 { get; set; }
		[Display(Name = "Hierarchy 4")]

		public bool AuditFeedbackApproval4 { get; set; }

        public string Operation { set; get; }


        public Audit Audit;
		public UserRolls()
		{
			Audit = new Audit();
		}


	}
}
