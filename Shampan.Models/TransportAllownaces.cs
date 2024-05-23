using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
	public class TransportAllownaces
	{
		public int Id { get; set; }
		[Display(Name = "Code")]
		public string? Code { get; set; }
		public string Name { get; set; }
		public string? Operation { get; set; }
		[Display(Name = "Audit Name")]
		public int AuditId { get; set; }
		[Display(Name = "Team Name")]
		public int TeamId { set; get; }
		public string TeamName { set; get; }
		public string? Description { set; get; }
		[Display(Name = "To Date")]
		public string ToDate { set; get; }
		[Display(Name = "From Date")]
		public string FromDate { set; get; }
		public List<string> IDs { get; set; }
		[Display(Name = "Amount")]
		public decimal Amount { set; get; }
		[Display(Name = "Visiting Palce")]

		public string VisitingPalce { set; get; }
		[Display(Name = "Departure Date")]

		public string DepartureDate { set; get; }
		[Display(Name = "Arrival Date")]

		public string ArrivalDate { set; get; }
		public string Designation { set; get; }
		public string IsPost { set; get; }
		public string? ReasonOfUnPost { set; get; }
		public string? ApproveStatus { set; get; }
        [Display(Name = "Rejected Comments")]

        public string? RejectedComments { set; get; }
        [Display(Name = "Approved Comments")]

        public string? CommentsL1 { set; get; }
		public string Edit { get; set; } = "Audit";
		public string Word { get; set; } 
		public int DayCount { get; set; } 

		public List<TransportAllownaceDetail> TransportAllownaceDetails { set; get; }
		public List<TransportFoodAndOthers> TransportFoodAndOthersDetails { set; get; }
		public List<LessAdvance> LessAdvanceDetails { set; get; }

		public Audit Audit;
        public Approval Approval;

        public TransportAllownaces()
		{
			Audit = new Audit();
            Approval = new Approval();
			TransportAllownaceDetails = new List<TransportAllownaceDetail>();
            TransportFoodAndOthersDetails = new List<TransportFoodAndOthers>();
            LessAdvanceDetails = new List<LessAdvance>();

        }
    }
}
