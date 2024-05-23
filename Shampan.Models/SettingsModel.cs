using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
	public class SettingsModel
	{	
		public int Id { get; set; }
		public string SettingGroup { get; set; }
		public string SettingName { get; set; }
		[Required(ErrorMessage = "Setting Value is required")]
		public string SettingValue { get; set; }
		public string SettingType { get; set; }
		public string? Remarks { get; set; }
		public bool IsActive { get; set; }
		public bool IsArchive { get; set; }
		public Audit Audit { get; set; }
		public SettingsModel()
		{
			Audit = new Audit();

		}


	}
}
