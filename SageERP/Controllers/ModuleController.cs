using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.Modules;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.Tour;
using Shampan.Models;
using Shampan.Services.Tour;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	public class ModuleController : Controller
	{

		private readonly ApplicationDbContext _applicationDb;
        private readonly IModuleService _moduleService;


        public ModuleController(ApplicationDbContext applicationDb,IModuleService moduleService)
		{
			_applicationDb = applicationDb;
            _moduleService = moduleService;

		}


		public ActionResult Index()
		{
			return View();
		}


		public ActionResult Create()
		{

			ModelState.Clear();
			ModulePermission vm = new ModulePermission();
			vm.Operation = "add";
			return View("CreateEdit", vm);


		}
		public ActionResult CreateEdit(ModulePermission master)
		{
			ResultModel<ModulePermission> result = new ResultModel<ModulePermission>();
			try
			{

				if (master.Operation == "update")
				{

					string userName = User.Identity.Name;
					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.LastUpdateBy = user.UserName;
					master.Audit.LastUpdateOn = DateTime.Now;
					master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _moduleService.Update(master);

                    return Ok(result);
				}
				else
				{

					string userName = User.Identity.Name;

					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.CreatedBy = user.UserName;
					master.Audit.CreatedOn = DateTime.Now;
					master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
					result = _moduleService.Insert(master);

					return Ok(result);
				}


			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}

			return Ok(result);
		}


        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<ModulePermission>> result =
                    _moduleService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                ModulePermission module = result.Data.FirstOrDefault();
                module.Operation = "update";
                module.Id = id;

                return View("CreateEdit", module);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
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
                string teamname = Request.Form["teamname"].ToString();
                string description = Request.Form["description"].ToString();
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
                            "TBLModul.Module like",
                            "TBLModul.IsActive like"
                            

                };

                string?[] conditionalValue = new[] { code, teamname };

                ResultModel<List<ModulePermission>> indexData =
                    _moduleService.GetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _moduleService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _moduleService.GetCount(TableName.Module, "Id", new[] { "TBLModul.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Tours>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }





    }
}
