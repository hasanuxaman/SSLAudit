using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class PrepaymentReview
    {
        public int Id { set; get; }
        public decimal Value { set; get; }
        public Audit Audit;

        public PrepaymentReview()
        {
            Audit = new Audit();
        }
    }
}
