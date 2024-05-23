using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class LessAdvance
    {

        public int Id { set; get; }
        public int TransportAllowanceId { set; get; }
        public string Date { set; get; }
        public string Details { set; get; }
        [Display(Name = "Amount(BDT)")]
        public decimal Amount { set; get; }
        public string Operation { set; get; }
        public string Edit { set; get; }

        public Audit Audit;
        public LessAdvance()
        {
            Audit = new Audit();

        }

    }
}
