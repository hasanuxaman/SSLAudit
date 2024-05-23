using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using StackExchange.Exceptional;

namespace SageERP.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class BranchProfileController: Controller
    {
        
        private readonly ApplicationDbContext _applicationDb;
        private readonly IBranchProfileService _branchProfileService;

        public BranchProfileController(ApplicationDbContext applicationDb, IBranchProfileService branchProfileService)
        {
            
            _applicationDb = applicationDb;
            _branchProfileService = branchProfileService;


        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            ModelState.Clear();
            BranchProfile vm = new BranchProfile();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(BranchProfile master)
        {
            ResultModel<BranchProfile> result = new ResultModel<BranchProfile>();
            try
            {
               
                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _branchProfileService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _branchProfileService.Insert(master);

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

               

              
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();

                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();

                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();

                index.OrderName = "BranchID";

                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);


                index.createdBy = userName;


                string[] conditionalFields = new[]
                                                        {

                          
                            "BranchCode like",
                            "BranchName like",

                            "Address like",
                            "TelephoneNo like",

                             "BranchCode like",
                            "BranchName like",

                            "Address like",
                            "TelephoneNo like"

                        };
                string?[] conditionalValue = new[] { search,search,search,search, users , branch , Address, TelephoneNo };
                ResultModel<List<BranchProfile>> indexData =
                    _branchProfileService.GetIndexData(index,
                        conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _branchProfileService.GetIndexDataCount(index,
                       conditionalFields, conditionalValue);

                int result = _branchProfileService.GetCount(TableName.BranchProfile, "BranchID", new[]
                        {
                            "BranchProfile.createdBy",
                        }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<BranchProfile>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }


        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<BranchProfile>> result =
                    _branchProfileService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });

                BranchProfile branchProfile = result.Data.FirstOrDefault();
                branchProfile.Operation = "update";
                branchProfile.BranchID = id;

                return View("CreateEdit", branchProfile);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


		public ActionResult BranchProfileDelete(BranchProfile master)
		{
			ResultModel<BranchProfile> result = new ResultModel<BranchProfile>();
			try
			{
					string userName = User.Identity.Name;
					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.LastUpdateBy = user.UserName;
					master.Audit.LastUpdateOn = DateTime.Now;
					master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
					result = _branchProfileService.BranchProfileUpdate(master);
					return Ok(result);			
			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}
			return Ok(result);
		}


	}
}
