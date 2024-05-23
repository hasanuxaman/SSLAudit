using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
	public class DateWisePolicyEditLogWithDetails
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
		public decimal Vat { set; get; }
		public decimal Stamp { set; get; }
		public decimal Gross { set; get; }

		public Audit Audit;
		public List<DateWisePolicyEditLogWithDetails> DateWisePolicyEditLogDetailsData { get; set; }

		public DateWisePolicyEditLogWithDetails()
		{
			Audit = new Audit();
			DateWisePolicyEditLogDetailsData = new List<DateWisePolicyEditLogWithDetails>();
		}
	}
}
