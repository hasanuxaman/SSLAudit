using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
	public class Teams
	{
		public int Id { get; set; }
		[Display(Name = "Code")]
		public string? Code { get; set; }

		[Display(Name = "Team Name")]
		public string TeamName { get; set; }
		public string Operation { set; get; }

		public Audit Audit;

		public Teams()
		{	
			Audit = new Audit();
		}

	}
}
