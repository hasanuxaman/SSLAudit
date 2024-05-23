using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
	public class DateWisePolicyEditLog
	{
		public int Id { set; get; }
		public string Operation { set; get; }
		public string Edit { set; get; }
		public string DocNo { set; get; }
		public string PCName { set; get; }
		public string UserId { set; get; }
		public string EntryDate { set; get; }
		public string BranchCode { set; get; }
		public string Class { set; get; }
		public string Status { set; get; }
		public string DocDate { set; get; }
		public decimal CustomerId { set; get; }
		public decimal NetPremium { set; get; }
		public decimal SumInsured { set; get; }
		public decimal ProdueerCode { set; get; }
		public decimal Business { set; get; }

		public Audit Audit;
		public List<DateWisePolicyEditLog> DateWisePolicyEditLogDetails { get; set; }

		public DateWisePolicyEditLog()
		{
			Audit = new Audit();
			DateWisePolicyEditLogDetails = new List<DateWisePolicyEditLog>();
		}
	}
}
