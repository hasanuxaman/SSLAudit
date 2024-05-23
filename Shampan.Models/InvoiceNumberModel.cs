using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class InvoiceNumberModel
    {
        public int Id { set; get; }
        public string InvoiceNumber { set; get; }

        public string Vendor { set; get; }
        public string Name { set; get; }
       
        
    }
}
