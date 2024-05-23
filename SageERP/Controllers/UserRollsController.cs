using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class UserRollsController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IUserRollsService _userRollsService;

        public UserRollsController(ApplicationDbContext applicationDb, ITeamsService teamsService, IUserRollsService userRollsService)
        {

            _applicationDb = applicationDb;
			_userRollsService = userRollsService;

        }

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {

            ModelState.Clear();
            UserRolls vm = new UserRolls();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(UserRolls master)
        {
            ResultModel<UserRolls> result = new ResultModel<UserRolls>();
            try
            {

                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

					result = _userRollsService.Update(master);

					return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _userRollsService.Insert(master);

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
                string advanceAmount = Request.Form["advanceAmount"].ToString();
                string description = Request.Form["description"].ToString();
                string post = Request.Form["ispost"].ToString();


                string userNameVal = Request.Form["userName"].ToString();
                string isAuditVal = Request.Form["isAudit"].ToString();
                string isTour1Val = Request.Form["isTour"].ToString();
                string isAdvanceVal = Request.Form["isAdvance"].ToString();
                string isTaVal = Request.Form["isTa"].ToString();
                string isTourCompletionReportVal = Request.Form["isTourCompletionReport"].ToString();


                string isAudit = CheckTrueFalse(isAuditVal);
                string isTour = CheckTrueFalse(isTour1Val);
                string isAdvance = CheckTrueFalse(isAdvanceVal);
                string isTa = CheckTrueFalse(isTaVal);
                string isTourCompletionReport = CheckTrueFalse(isTourCompletionReportVal);



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
                            "UserName like",
                            "IsAudit like",
                            "IsTour like",
                            "IsAdvance like",
                            "IsTa like",
							"IsTourCompletionReport like"

				};

                string?[] conditionalValue = new[] { userNameVal,isAudit, isTour, isAdvance, isTa, isTourCompletionReport };

                ResultModel<List<UserRolls>> indexData =
					_userRollsService.GetIndexData(index, conditionalFields, conditionalValue);


               
                ResultModel<int> indexDataCount =
				_userRollsService.GetIndexDataCount(index, conditionalFields, conditionalValue);

              
                int result = _userRollsService.GetCount(TableName.UserRolls, "Id", new[] { "UserRolls.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<UserRolls>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }


        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<UserRolls>> result =
					_userRollsService.GetAll(new[] { "Id" }, new[] { id.ToString() });

				UserRolls userrolls = result.Data.FirstOrDefault();
				userrolls.Operation = "update";
				userrolls.Id = id;

                return View("CreateEdit", userrolls);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }



       public string CheckTrueFalse(string check)
       {
			if (check == "" || check == "Select")
			{			
                return "";
			}
			else
			{
				if (check == "True")
				{	
					return "1";

				}
				else
				{	
					return "0";

				}
			}


			
		}

	}
}
