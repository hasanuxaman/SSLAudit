using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Circular
{
	public interface ICircularsRepository : IBaseRepository<Circulars>
	{
		string CodeGeneration(string CodeGroup, string CodeName);


	}
}
