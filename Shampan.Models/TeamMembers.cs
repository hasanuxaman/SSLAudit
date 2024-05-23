using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class TeamMembers
    {

        public int Id { get; set; }
        [Display(Name = "User Name")]
        public string UserId { get; set; }
        [Display(Name = "Team Name")]
        public string TeamId { get; set; }
        public string UserName { get; set; }
        public string TeamName { get; set; }
        public string Operation { get; set; }
        public string memberId { get; set; }

        public Audit Audit;
        public TeamMembers()
        {
            Audit = new Audit();
        }

    }
}
