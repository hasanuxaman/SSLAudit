using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class ModulePermission
	{
        public int Id { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "User Name")]
        public string UserId { get; set; }
		[Display(Name = "Module Name")]

		public string ModuleName { get; set; }
		public string Modul { get; set; }
        public string Operation { get; set; }
		[Display(Name = "Active")]
		public bool IsActive { get; set; }
		public int UserPermissionId { get; set; }

		public Audit Audit;

        public List<ModulePermission> ModuleList { set; get; }

        public ModulePermission()
        {
            ModuleList = new List<ModulePermission>();
            Audit = new Audit();
            
        }


    }

}
