using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class IssueRejectComments
    {
        public IssueRejectComments()
        {
            Audit = new Audit();
        }
        public int Id { get; set; }
        public int AuditId { get; set; }
        public string AuditIssueId { get; set; }
        public bool IsIssueApprove { get; set; }
        public string IssueApproveComments { get; set; }
        public bool IsIssueReject { get; set; }
        public string IssuesRejectComments { get; set; }
        public string CreatedOn { get; set; }

        public Audit Audit { set; get; }
    }
}
