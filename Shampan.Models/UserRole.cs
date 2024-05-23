using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class UserRole
    {
        [Display(Name = "User Name")]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }


    }
}
