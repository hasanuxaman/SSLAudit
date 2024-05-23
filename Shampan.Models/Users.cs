using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class Users
    {
        public int Id { set; get; }
        public string? Code { set; get; }
        public string UserName { set; get; }
		[Display(Name = "User Name")]
		public string UserId { get; set; }
        public List<ModulePermission> ModuleList { set; get; }
        public List<SubmanuList> NodeList { set; get; }
        public ModulePermission Module{ set; get; }
        public string Operation { set; get; }
        public bool IsModuleActive { set; get; }
        public int ModuleID { set; get; }
        public string Modul { set; get; }
        public bool IsAllowByUser { set; get; }
        public string Node { set; get; }
        public Audit Audit;
        public Users()
        {
            Audit = new Audit();
            ModuleList = new List<ModulePermission>();
            NodeList = new List<SubmanuList>();
            Module = new ModulePermission();
        }
    }

	public class UserDetail
	{
        [Display(Name = "R1")]
        public bool R1 { get; set; }
        public int R1Value { get; set; }
        [Display(Name = "R2")]
        public bool R2 { get; set; }
        public int R2Value { get; set; }

        [Display(Name = "R3")]
        public bool R3 { get; set; }
        public int R3Value { get; set; }

        [Display(Name = "R4")]
        public bool R4 { get; set; }
        public int R4Value { get; set; }
        [Display(Name = "R5")]
        public bool R5 { get; set; }
        public int R5Value { get; set; }


    }
}
