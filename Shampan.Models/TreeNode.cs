using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class TreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TreeNode> Children { get; set; }
    }
}
