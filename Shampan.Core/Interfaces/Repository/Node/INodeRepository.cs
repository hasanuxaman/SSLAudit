using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Node
{
	public interface INodeRepository : IBaseRepository<SubmanuList>
	{
		SubmanuList GetNodeById(string id);


    }
}
