using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Node
{
	public interface INodeService : IBaseService<SubmanuList>
	{
        SubmanuList GetNodeById(string id);
        //ResultModel<SubmanuList> Delete(int id);
    }
}
