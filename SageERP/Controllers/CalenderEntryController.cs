using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Calender;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Services.Advance;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class CalenderEntryController : Controller
    {


		private readonly ApplicationDbContext _applicationDb;
		private readonly ICalendersService _calendersService;

		public CalenderEntryController(ApplicationDbContext applicationDb, ICalendersService calendersService)
		{

			_applicationDb = applicationDb;
			_calendersService = calendersService;

		}

		public IActionResult Index()
        {
            return View();
        }

		public ActionResult Create()
		{

			ModelState.Clear();
			Calenders vm = new Calenders();
			vm.Operation = "add";
			return View("CreateEdit", vm);


		}
		public ActionResult CreateEdit(Calenders master)
		{
			ResultModel<Calenders> result = new ResultModel<Calenders>();
			try
			{



				if (master.Operation == "update")
				{

					string userName = User.Identity.Name;
					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.LastUpdateBy = user.UserName;
					master.Audit.LastUpdateOn = DateTime.Now;
					master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

					result = _calendersService.Update(master);

					return Ok(result);
				}
				else
				{

					string userName = User.Identity.Name;

					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.CreatedBy = user.UserName;
					master.Audit.CreatedOn = DateTime.Now;
					master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
					result = _calendersService.Insert(master);

					return Ok(result);
				}


			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}

			return Ok(result);
		}


		public IActionResult _index()
		{
			try
			{
				IndexModel index = new IndexModel();
				string userName = User.Identity.Name;
				ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

				var search = Request.Form["search[value]"].FirstOrDefault();
				string users = Request.Form["BranchCode"].ToString();
				string branch = Request.Form["BranchName"].ToString();
				string Address = Request.Form["Address"].ToString();
				string TelephoneNo = Request.Form["TelephoneNo"].ToString();

				string code = Request.Form["code"].ToString();
				string title = Request.Form["title"].ToString();
				string color = Request.Form["color"].ToString();

				string post = Request.Form["ispost"].ToString();


				if (post == "Select")
				{
					post = "";

				}





				string draw = Request.Form["draw"].ToString();
				var startRec = Request.Form["start"].FirstOrDefault();
				var pageSize = Request.Form["length"].FirstOrDefault();
				var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();

				var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();

				index.SearchValue = Request.Form["search[value]"].FirstOrDefault();

				index.OrderName = "Id";

				index.orderDir = orderDir;
				index.startRec = Convert.ToInt32(startRec);
				index.pageSize = Convert.ToInt32(pageSize);


				index.createdBy = userName;


				string[] conditionalFields = new[]
				{
							"Code like",
                            "Title like",
							"Color like"
							
				};

				string?[] conditionalValue = new[] { code, title, color };

				ResultModel<List<Calenders>> indexData =
					_calendersService.GetIndexData(index, conditionalFields, conditionalValue);



				ResultModel<int> indexDataCount =
				_calendersService.GetIndexDataCount(index, conditionalFields, conditionalValue);


				int result = _calendersService.GetCount(TableName.Calenders, "Id", new[] { "Calenders.createdBy", }, new[] { userName });


				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<Calenders>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}


		}


		public ActionResult Edit(int id)
		{
			try
			{
				ResultModel<List<Calenders>> result =
					_calendersService.GetAll(new[] { "Id" }, new[] { id.ToString() });

				Calenders advances = result.Data.FirstOrDefault();
				advances.Operation = "update";
				advances.Id = id;

				return View("CreateEdit", advances);
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return RedirectToAction("Index");
			}
		}





	}
}
