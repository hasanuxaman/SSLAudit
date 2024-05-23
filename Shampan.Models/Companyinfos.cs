using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class Companyinfos
    {
        public int CompanyID { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Company Legal Name")]
        public string CompanyLegalName { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        [Display(Name = "Telephone No")]
        public string TelephoneNo { get; set; }
        public string FaxNo { get; set;}
        public string Email { get; set; }
        [Display(Name = "Contact Person")]

        public string ContactPerson { get; set; }
        [Display(Name = "Contact Person Designation")]
        public string ContactPersonDesignation { get; set; }
        [Display(Name = "Contact Phone")]

        public string ContactPhone { get; set; }
        [Display(Name = "Contact Person Telephone")]

        public string ContactPersonTelephone { get; set; }
        [Display(Name = "Contact Person Email")]
        public string ContactPersonEmail { get; set; }
        public string? TINNo { get; set; }
        public string? BIN { get; set; }
        [Display(Name = "Vat Registration No")]
        public string? VatRegistrationNo { get; set; }
        public string Comments { get; set; }
        public bool ActiveStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string LastModifiedBy { get; set;}
        public string LastModifiedOn { get; set;}
        public string Operation { get; set; }
        public string ErrorMsg { get; set; }
        public Audit Audit { get; set; }

        public Companyinfos()
        {



            Audit = new Audit();
        }

    }

    public class CompanyInfo
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDataBase { get; set; }
        public int SerialNo { get; set; }
        public string DatabaseName { get; set; }



    }

}
