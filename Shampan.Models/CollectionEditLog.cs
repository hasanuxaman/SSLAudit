using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
	public class CollectionEditLog
	{
		public int Id { set; get; }
		public string Operation { set; get; }
		public string Edit { set; get; }
		public string MR { set; get; }
		public string PCName { set; get; }
		public string UserId { set; get; }
		public string EditDate { set; get; }
		public string Status { set; get; }
		public string SlNo { set; get; }
		public decimal Deposit { set; get; }
		public string DepositDate { set; get; }
		public decimal Record { set; get; }

		public Audit Audit;
		public List<CollectionEditLog> CollectionEditLogDetails { get; set; }

		public CollectionEditLog()
		{
			Audit = new Audit();
			CollectionEditLogDetails = new List<CollectionEditLog>();
		}
	}
}
