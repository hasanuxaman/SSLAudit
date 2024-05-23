using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class TransportAllownaceDetail
    {
        public int Id { set; get; }
        [Display(Name = "Transport Allowance")]
        public int TransportAllowanceId { set; get; }
        public string Operation { set; get; }
        public string Edit { set; get; }
        public string Date { set; get; }
        public string Particulars { set; get; }
        [Display(Name ="Amount(BDT)")]
        public decimal Amount { set; get; }
        public Audit Audit;
        public List<TransportAllownaceDetail> TADABillDetails { get; set; }
        public TransportAllownaceDetail()
        {
            Audit = new Audit();
            TADABillDetails = new List<TransportAllownaceDetail>();
        }



    }
}
