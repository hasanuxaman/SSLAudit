using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Calender
{
	public interface ICalendersRepository : IBaseRepository<Calenders>
	{
		List<Calenders> GetCalenderData();
		string CodeGeneration(string CodeGroup, string CodeName);

	}
}
