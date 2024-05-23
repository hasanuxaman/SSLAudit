using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
	public class PrePaymentMaster
	{
		
		public int Id { get; set; }
		[Display(Name = "Code")]
		public string? Code { get; set; }
		public string? Auditor { get; set; }
		public string? EmployeeName { set; get; }
		public string? Details { set; get; }
		public string FinalCorrectionDate { set; get; }
		public decimal FinalAmount { set; get; }
		public decimal PreviousAmount { set; get; }
		public decimal CorrectedAmount { set; get; }
		public decimal AdditionalPayment { set; get; }
		public string PaymentMemoReferenceNo { set; get; }
		public string Department { set; get; }
		public string Remarks { set; get; }
		public string? Operation { get; set; }
		[Display(Name = "Advance Date")]
		public string AdvanceDate { set; get; }
		[Display(Name = "Advance Amount")]
		public decimal AdvanceAmount { set; get; }
		public string IsPost { set; get; }


		public List<PrePaymentMaster> PrePaymentDetails { get; set; }
		public List<PrePaymentMaster> NonFinancialDetails { get; set; }
		public string Edit { get; set; } = "Audit";

		public Audit Audit;
	    public PrepaymentReview PrepaymentReview;

        public PrePaymentMaster()
		{
			PrePaymentDetails = new List<PrePaymentMaster>();
			NonFinancialDetails = new List<PrePaymentMaster>();
			Audit = new Audit();
            PrepaymentReview = new PrepaymentReview();
		}
	}
}
