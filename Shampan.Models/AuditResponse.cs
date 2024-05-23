using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
	public class AuditResponse
	{
		public int Id { set; get; }
		public string AuditName { set; get; }
		public string IssueName { set; get; }
		public string DateOfSubmission { set; get; }
		public string IssuePriority { set; get; }
	}
}
