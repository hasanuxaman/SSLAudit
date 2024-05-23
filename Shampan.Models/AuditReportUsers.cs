using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class AuditReportUsers
    {
        public AuditReportUsers()
        {
            Audit = new Audit();
            AuditReportUserList = new List<AuditReportUsers>();
        }
        public Audit Audit { set; get; }
        public List<AuditReportUsers> AuditReportUserList { set; get; }
        public int Id { set; get; }
        public int AuditId { set; get; }
        [Display(Name ="User Name")]
        public string UserId { set; get; }
        [Display(Name = "Email Address")]
        public string EmailAddress { set; get; }
        public string UserName { set; get; }
        public string Operation { set; get; }
        public string Remarks { set; get; }
        public string Edit { set; get; }
        public string Status { set; get; }
    }
}
