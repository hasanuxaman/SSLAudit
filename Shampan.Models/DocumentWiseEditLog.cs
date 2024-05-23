using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
	public class DocumentWiseEditLog
	{
		public int Id { set; get; }
		public string Operation { set; get; }
		public string Edit { set; get; }
		public string DocNo { set; get; }
		public string DocDate { set; get; }
		public string PCName { set; get; }
		public string UserId { set; get; }
		public string EntryDate { set; get; }
		public string ClassCode { set; get; }
		public string Status { set; get; }
		public string CustomerId { set; get; }
		public decimal NetPremium { set; get; }
		public decimal SumInsured { set; get; }
		public decimal VatAmount { set; get; }
		public decimal StampAmount { set; get; }
		public decimal ProducerCode { set; get; }
		public string BusinessStatus { set; get; }

		public Audit Audit;
		public List<DocumentWiseEditLog> DocumentWiseEditLogDetails { get; set; }

		public DocumentWiseEditLog()
		{
			Audit = new Audit();
			DocumentWiseEditLogDetails = new List<DocumentWiseEditLog>();
		}
	}
}
