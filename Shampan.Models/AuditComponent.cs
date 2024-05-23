using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class AuditComponent
    {
        public string Name { set; get; }
        //public int BranchPlan { set; get; }
        public decimal BranchPlan { set; get; }
        //public int BranchOngoin { set; get; }
        public decimal BranchOngoin { set; get; }
        //public int BranchCompleted { set; get; }
        public decimal BranchCompleted { set; get; }
        public int BranchRemaining { set; get; }
        //public int BranchCompletedOngoing { set; get; }
        public decimal BranchCompletedOngoing { set; get; }
    }
}
