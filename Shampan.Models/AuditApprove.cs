using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class AuditApprove
    {
        public bool IsApprovedL1 { set; get; }
        public bool IsApprovedL2 { set; get; }
        public bool IsApprovedL3 { set; get; }
        public bool IsApprovedL4 { set; get; }
        public bool IsAudited { set; get; }

        public bool IssueIsApprovedL1 { set; get; }
        public bool IssueIsApprovedL2 { set; get; }
        public bool IssueIsApprovedL3 { set; get; }
        public bool IssueIsApprovedL4 { set; get; }


        public bool BFIsApprovedL1 { set; get; }
        public bool BFIsApprovedL2 { set; get; }
        public bool BFIsApprovedL3 { set; get; }
        public bool BFIsApprovedL4 { set; get; }


        public bool IssueIsAudited { set; get; }

        public string ApprovedByL1 { set; get; }
        public string ApprovedByL2 { set; get; }
        public string ApprovedByL3 { set; get; }
        public string ApprovedByL4 { set; get; }

        public string IssueApprovedByL1 { set; get; }
        public string IssueApprovedByL2 { set; get; }
        public string IssueApprovedByL3 { set; get; }
        public string IssueApprovedByL4 { set; get; }

        public string BFApprovedByL1 { set; get; }
        public string BFApprovedByL2 { set; get; }
        public string BFApprovedByL3 { set; get; }
        public string BFApprovedByL4 { set; get; }



    }
}
