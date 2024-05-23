using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
	public class AuditReportHeading
	{
		public int Id { get; set; }
		public string? AuditReportDetails { get; set; }
		public string? AuditSecondReportDetails { get; set; }
		public string? AuditAnnexureDetails { get; set; }
		public string? AuditSeeAllDetails { get; set; }

		public int AuditId { get; set; }

		[Display(Name = "Audit Area")]
		public string Operation { get; set; } = "add";

		public string Edit { get; set; } = "";
		public string Check { get; set; } = "";
		public string? AllDetails { get; set; }



	}
}
