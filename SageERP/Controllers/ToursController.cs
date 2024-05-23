using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.Tour;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services.Advance;
using Shampan.Services.UserRoll;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Models;
using StackExchange.Exceptional;
using Microsoft.Extensions.Caching.Memory;
using SixLabors.ImageSharp.ColorSpaces;
using System.Globalization;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class ToursController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IToursService _toursService;
		private readonly IUserRollsService _userRollsService;
		private readonly IMemoryCache _memoryCache;


		public ToursController(ApplicationDbContext applicationDb, ITeamsService teamsService, IToursService toursService, 
		IMemoryCache memoryCache,IUserRollsService userRollsService)
        {

            _applicationDb = applicationDb;
            _toursService = toursService;
			_userRollsService = userRollsService;
			_memoryCache = memoryCache;

		}

		//public IActionResult Index()
		public async Task<IActionResult> Index()
		{

			var cacheKey = $"{User.Identity.Name}_BranchClaim";
			if (_memoryCache.TryGetValue(cacheKey, out BranchClaim branchClaim))
			{
				string name = branchClaim.BranchName;

				var identity = new ClaimsIdentity(User.Identity);
				identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branchClaim.BranchId.ToString()));
				identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branchClaim.BranchName.ToString().Trim()));

				var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });
          

            }


			if (User.Identity.IsAuthenticated)
            {
				string userName = User.Identity.Name;
				List<UserManuInfo> manu = _userRollsService.GetUserManu(userName);
			}
            else
            {
                return RedirectToAction("Index", "Login");
            }
		
			return View();
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

                //index.orderDir = orderDir;
                index.orderDir = "desc";

                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);


                index.createdBy = userName;


                string[] conditionalFields = new[]
                {
                            "Tours.Code like",
                            "TeamName like",
                            "Tours.Description like",
                            "Tours.IsPost like"

                };

                string?[] conditionalValue = new[] { code, teamname, description, post };

                ResultModel<List<Tours>> indexData =
                    _toursService.GetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _toursService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _toursService.GetCount(TableName.Tours, "Id", new[] { "Tours.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Tours>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

        public IActionResult ApproveStatusIndex()
        {
			if (User.Identity.IsAuthenticated)
			{
				string userName = User.Identity.Name;
				List<UserManuInfo> manu = _userRollsService.GetUserManu(userName);
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}
			return View();
        }

        public IActionResult ApproveSelfStatusIndex()
        {
			if (User.Identity.IsAuthenticated)
			{
				string userName = User.Identity.Name;
				List<UserManuInfo> manu = _userRollsService.GetUserManu(userName);
			}
			else
			{
				return RedirectToAction("Index", "Login");
			}
			return View();
        }

        public IActionResult _approveSelfStatusIndex()
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                index.UserName = userName;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();



                string code = Request.Form["code"].ToString();
                string teamname = Request.Form["teamname"].ToString();
                string description = Request.Form["description"].ToString();
                string approveStatus = Request.Form["approveStatus"].ToString();


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
                            "Tours.Code like",
                            "TeamName like",
                            "Description like",
                            "Tours.IsPost "
                };

                string?[] conditionalValue = new[] { code, teamname, description, "Y" };

                ResultModel<List<Tours>> indexData =
                    _toursService.GetIndexDataSelfStatus(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _toursService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _toursService.GetCount(TableName.Tours, "Id", new[] { "Tours.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Tours>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }


        public IActionResult _approveStatusIndex(string status)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                index.UserName = userName;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();



                string code = Request.Form["code"].ToString();
                string teamname = Request.Form["teamname"].ToString();
                string description = Request.Form["description"].ToString();
                string approveStatus = Request.Form["approveStatus"].ToString();


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

                string[] conditionalFields = null;
                string?[] conditionalValue = null;

                if (status == "self")
                {
                     conditionalFields = new[]

                     {
                            "Tours.Code like",
                            "TeamName like",
                            "Description like",
                            "Tours.IsPost",
                            "Tours.CreatedBy",
                     };

                     conditionalValue = new[] { code, teamname, description, "Y", userName };
                }

                else
                {
                    conditionalFields = new[]
                    {
                            "Tours.Code like",
                            "TeamName like",
                            "Description like",
                            "Tours.IsPost "
                    };

                    conditionalValue = new[] { code, teamname, description, "Y" };
                }



                ResultModel<List<Tours>> indexData =
                    _toursService.GetIndexDataStatus(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _toursService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _toursService.GetCount(TableName.Tours, "Id", new[] { "Tours.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Tours>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

        public ActionResult Create()
        {

            ModelState.Clear();
            Tours vm = new Tours();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(Tours master)
        {
            ResultModel<Tours> result = new ResultModel<Tours>();
            try
            {



                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _toursService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _toursService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }




        //public ActionResult Edit(int id)
        public ActionResult Edit(int id, string edit = "audit")
        {
            try
            {
                ResultModel<List<Tours>> result =
                    _toursService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                Tours tour = result.Data.FirstOrDefault();
                tour.Operation = "update";
                tour.Edit = edit;

                tour.Id = id;

                return View("CreateEdit", tour);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }
        //Report
        public ActionResult TourReportCreateEdit(int id)
        {
            try
            {
                ResultModel<List<Tours>> result =
                    _toursService.GetAllForReports(new[] { "Tours.Id" }, new[] { id.ToString() });

                Tours tour = result.Data.FirstOrDefault();
                tour.Operation = "update";
               
                tour.Id = id;

                //return View("CreateEdit", tour);
                return View(tour);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


        public IActionResult TourReport(int id)
        {
            ResultModel<List<Tours>> result =_toursService.GetAllForReports(new[] { "Tours.Id" }, new[] { id.ToString() });

            List<string> str = new List<string>();
            string FullName = "";
            string FullDesignation = "";
            int count = result.Data.Count();
            int i = 0;

            foreach (var item in result.Data)
            {
                //str.Add(item.Name);
                //str.Add("/");
                FullName = FullName + item.Name;
                i++;
                if (i != count)
                {
                    FullName = FullName + "/";
                }
             
            }
            i = 0;
            foreach (var designation in result.Data)
            {
                FullDesignation = FullDesignation + designation.Designation;
                i++;
                if (i != count)
                {
                    FullDesignation = FullDesignation + "/";
                }
            }

            //DateTime fromdate = Convert.ToDateTime(result.Data.FirstOrDefault().FromDate);
            //DateTime todate = Convert.ToDateTime(result.Data.FirstOrDefault().ToDate); 
            //TimeSpan difference = todate.AddDays(1) - fromdate;
            //int daysCount = difference.Days;

            string fromDateStr = result.Data.FirstOrDefault().FromDate;
            string toDateStr = result.Data.FirstOrDefault().ToDate;
            DateTime fromdate;
            DateTime todate;
            int daysCount=0;

            if (DateTime.TryParseExact(fromDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromdate) &&
                DateTime.TryParseExact(toDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out todate))
            {               
                TimeSpan difference = todate.AddDays(1) - fromdate;
                daysCount = difference.Days;
                
            }
            
          


            Tours tour = result.Data.FirstOrDefault();
            tour.Name = FullName;
            tour.Designation = FullDesignation;

            tour.DayCount = daysCount;
            var word =NumberToWord.ConvertNumberToString(Convert.ToInt32(tour.Amount));
            tour.Word = word;

            return View(tour);
       
        }


        public ActionResult MultiplePost(Tours master)
        {
            ResultModel<Tours> result = new ResultModel<Tours>();

            try
            {

                foreach (string ID in master.IDs)
                {


                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "post";
                    result = _toursService.MultiplePost(master);


                }




                return Ok(result);


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleUnPost(Tours master)
        {
            ResultModel<Tours> result = new ResultModel<Tours>();

            try
            {

                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "unpost";
                    result = _toursService.MultipleUnPost(master);
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleReject(Tours master)
        {
            ResultModel<Tours> result = new ResultModel<Tours>();

            try
            {

                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Approval.IsRejected = true;
                    master.Approval.RejectedBy = user.UserName;
                    master.Approval.RejectedDate = DateTime.Now;

                    //master.Approval.RejectedComments = DateTime.Now;
                    master.Operation = "reject";
                    result = _toursService.MultipleUnPost(master);
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }


        public ActionResult MultipleApproved(Tours master)
        {
            ResultModel<Tours> result = new ResultModel<Tours>();

            try
            {

                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    //master.Approval.RejectedComments = DateTime.Now;
                    master.Operation = "approved";
                    result = _toursService.MultipleUnPost(master);
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }




    


    }
}
