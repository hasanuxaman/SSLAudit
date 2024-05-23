using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class IndexModel
    {
        public string? SearchValue { get; set; }
        public string OrderName { get; set; }
        public string orderDir { get; set; }
        public int startRec { get; set; }
        public int pageSize { get; set; }

        public string IsArchive { get; set; }
        public string IsActive { get; set; }

        public string createdBy { get; set; }
        public string FixedParam { get; set; }
        public string CurrentBranchid { get; set; }
        public string CurrentBranchCode { get; set; }

        public int AuditId { get; set; }
        public string TeamId { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int AuditIssueId { get; set; }
        public string UserName { get; set; }
        public bool self { get; set; }
        public string Status { get; set; }
        public string AuditPlan { get; set; }
        public string AuditApprove { get; set; }
        public string Feedback { get; set; }
        public string BranchFeedback { get; set; }
        public string IssueComplete { get; set; }
        public string AuditStatus { get; set; }
        public string ModuleId { set; get; }
    }
}
