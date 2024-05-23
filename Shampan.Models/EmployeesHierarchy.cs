using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shampan.Models
{
	public class EmployeesHierarchy
	{
		public int EmployeeId { get; set; }
		public string? Code { get; set; }
		public string Name { set; get; }
		public int Names { set; get; }
		public string Designation { set; get; }
		[Display(Name= "Reporting Manager")]
		public int ReportingManager { set; get; }
		public string Operation { set; get; }

		public Audit Audit;

		public List<EmployeesHierarchyAttachments> AttachmentsList { get; set; }

		public IList<IFormFile>? Attachments { get; set; }

		public EmployeesHierarchy()
		{
			Audit = new Audit();
			AttachmentsList = new List<EmployeesHierarchyAttachments>();
		}

	}
}
