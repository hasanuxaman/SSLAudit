using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class AuditReports
    {
        public decimal Completed { set; get; }
        public decimal Ongoing { set; get; }
        public decimal Remaining { set; get; }

		public decimal UnPlanCompleted { set; get; }
		public decimal UnPlanOngoing { set; get; }
		public decimal UnPlanRemaining { set; get; }




	}
}
