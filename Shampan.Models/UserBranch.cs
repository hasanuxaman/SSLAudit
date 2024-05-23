using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class UserBranch
    {
       public int Id { get; set; }
        [Display(Name = "User Name")]
        public string UserId { get; set; }
        
        public string UserName { get; set; }
        public string BranchName{ get; set; }
        [Display(Name = "Branch Name")]
        public int BranchId { get; set; }
       
        public string Operation { get; set; }

        public Audit Audit;
        public UserBranch()
        {
            Audit = new Audit();
        }


    }
}
