using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
	public class MailSetting
	{
		public int Id { set; get; }
		public int AutidId { set; get; }
		public int AuditIssueId { set; get; }
		public int BranchFeedbackId { set; get; }
		public string ApprovedUrl { set; get; }
		public string ApproveOparetion { set; get; }
		public string Status { set; get; }
		public bool IsMailed { set; get; }

	}
}
