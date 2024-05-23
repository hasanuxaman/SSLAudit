using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using Shampan.Services.Branch;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class TeamsController : Controller
	{

		
		private readonly ApplicationDbContext _applicationDb;
		private readonly ITeamsService _teamsService;
		private readonly IUserRollsService _userRollsService;

		public TeamsController(ApplicationDbContext applicationDb, ITeamsService teamsService, IUserRollsService userRollsService)
		{

			_applicationDb = applicationDb;
			_teamsService = teamsService;
			_userRollsService = userRollsService;


		}

		public IActionResult Index()
		{
			string userName = User.Identity.Name;
			//List<UserManuInfo> manu = _userRollsService.GetUserManu(userName);
			//ViewBag.ManuList = manu;
			return View();
		}
        public ActionResult Create()
        {

            ModelState.Clear();
            Teams vm = new Teams();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(Teams master)
        {
            ResultModel<Teams> result = new ResultModel<Teams>();
            try
            {



                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _teamsService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _teamsService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }


        public IActionResult _index(string self)
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
                            "TeamName like"
                            
                };
				string?[] conditionalValue = new[] { code, teamname };

				//if (self.ToLower()=="y")
               //  {
				//	conditionalFields = new[]
                //{
				//			"Code like",
				//			"TeamName like",
				//			"Tours.CreatedBy"

				//};
				//	  conditionalValue = new[] { code, teamname, userName };
				//}
               


				ResultModel<List<Teams>> indexData =
                    _teamsService.GetIndexData(index,conditionalFields, conditionalValue);


                //ResultModel<int> indexDataCount = new ResultModel<int>();
                //indexDataCount.Data = 5;
                ResultModel<int> indexDataCount =
                _teamsService.GetIndexDataCount(index,conditionalFields, conditionalValue);

                //int result = 3;
                int result = _teamsService.GetCount(TableName.Teams, "Id", new[]{ "A_Teams.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }


        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<Teams>> result =
                    _teamsService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                Teams teams = result.Data.FirstOrDefault();
                teams.Operation = "update";
                teams.Id = id;

                return View("CreateEdit", teams);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


    }
}
