using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public int BranchId { get; set; }

        public int CompanyId { get; set; }


        public string Operation { get; set; }

        public Audit Audit { get; set; }

    }
}
