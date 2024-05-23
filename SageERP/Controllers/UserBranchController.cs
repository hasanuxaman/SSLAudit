using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Core.Interfaces.Services.User;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using StackExchange.Exceptional;

namespace SageERP.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class UserBranchController : Controller
    {
        
        private readonly ApplicationDbContext _applicationDb;
        private readonly IUserBranchService _userBranchService;
        private readonly ICommonService _commonService;

        public UserBranchController(ApplicationDbContext applicationDb, IUserBranchService userBranchService,
            ICommonService commonService)
        {
            
            _applicationDb = applicationDb;
            _userBranchService = userBranchService;
            _commonService = commonService;

        }

        public IActionResult Index(string? id)
        {
            ViewBag.UserId = id;
            return View();
        }

        public ActionResult Create(string? id)
        {
            ModelState.Clear();
            UserBranch vm = new UserBranch();
            vm.Operation = "add";
            vm.UserId = id;
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(UserBranch master)
        {
            ResultModel<UserBranch> result = new ResultModel<UserBranch>();
            try
            {

                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();


                    result = _userBranchService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _userBranchService.Insert(master);

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
                string users = Request.Form["UserName"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string userId = Request.Form["userId"].ToString();

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
                            "BranchName like",

                            "UserName like",
                            "BranchName like",
                            "userId like"

                        };
                string?[] conditionalValue = new[] { search,search, users, branch, userId };
                ResultModel<List<UserBranch>> indexData =
                    _userBranchService.GetIndexData(index,
                        conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _userBranchService.GetIndexDataCount(index,
                       conditionalFields, conditionalValue);

                int result = _userBranchService.GetCount(TableName.UserBranch, "Id", new[]
                        {
                            "UserBranch.createdBy",
                        }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<UserBranch>(), draw = "", recordsTotal = 0, recordsFiltered = "" });
            }


        }

        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<UserBranch>> result =
                    _userBranchService.GetAll(new[] { "u.Id" }, new[] { id.ToString() });

                UserBranch userBranch = result.Data.FirstOrDefault();
                userBranch.Operation = "update";
                userBranch.Id = id;
                
                return View("CreateEdit", userBranch);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }
        public ActionResult<IList<CommonDropDown>> UserBranch()
        {
            var result = _commonService.UserBranch();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> UserId()
        {
            var result = _commonService.UserId();
            return Ok(result);
        }
    }
}
