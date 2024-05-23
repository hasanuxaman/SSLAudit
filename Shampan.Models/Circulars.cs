using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;

namespace Shampan.Models
{
    public class Circulars
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        [Display(Name = "Circular Summary")]
        public string? CircularSummary { get; set; }
		[Display(Name = "Circular Details")]
		public string? CircularDetails { get; set; }
		[Display(Name = "Circular Type")]
		public int CircularType { get; set; }
		public string Name { get; set; }
		[Display(Name = "Circular Date")]
		public string CircularDate { set; get; }
        public string? Attachment { set; get; }
        public string Operation { set; get; }
		

		public bool IsPublished { set; get; }  
        public string FilesToBeUploaded { get; set; }
        public string Edit { get; set; }

		public Audit Audit;
		public List<CircularAttachments> AttachmentsList { get; set; }

		public IList<IFormFile>? Attachments { get; set; }

		public Circulars()
        {
            Audit = new Audit();
			AttachmentsList = new List<CircularAttachments>();

		}
	}
}
