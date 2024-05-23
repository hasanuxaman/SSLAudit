using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Calender;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using ShampanERP.Persistence;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class CalenderController : Controller
    {


		private readonly ApplicationDbContext _applicationDb;
		private readonly ICalendersService _calendersService;

		public CalenderController(ApplicationDbContext applicationDb, ICalendersService calendersService)
		{

			_applicationDb = applicationDb;
			_calendersService = calendersService;

		}

		public IActionResult Index()
        {
            return View();
        }


		public ActionResult GetEvents()
		{

			ResultModel<List<Calenders>> calenderData = _calendersService.GetCalenderData();
			List<Calenders> birthdayList = calenderData.Data;

			birthdayList = calenderData.Data;
		

			return Ok(birthdayList);
		}


	}
}
