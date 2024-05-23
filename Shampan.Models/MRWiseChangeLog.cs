using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class MRWiseChangeLog
    {

        public int Id { set; get; }
		public string Operation { set; get; }
		public string Edit { set; get; }
		public string MRNo { set; get; }   
        public string PCNo { set; get; }
        public string UserId { set; get; }
        public string EditDate { set; get; }
        public string Status { set; get; }

        public decimal MRNet { set; get; }
        public decimal MRVat { set; get; }
        public decimal MRStamp { set; get; }
        public decimal MRCoinsPayable { set; get; }

        public string MRDateTime { set; get; }

        public Audit Audit;
        public List<MRWiseChangeLog> MRWiseChangeLogDetails { get; set; }

        public MRWiseChangeLog()
        {
            Audit = new Audit();
            MRWiseChangeLogDetails = new List<MRWiseChangeLog>();
        }

    }
}
