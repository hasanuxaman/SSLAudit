using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class Calenders
	{
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
		[Display(Name = "Start Date")]

		public string Start { get; set; }
		[Display(Name = "End Date")]

		public string End { get; set; }
        public string Color { get; set; }

		public bool AllDay { get; set; }
		public string Operation { set; get; }

		[Display(Name = "Active")]
        public bool IsActive { get; set; }

		public Audit Audit;

		public Calenders()
		{
			Audit = new Audit();
		}
	}
}
