using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using Shampan.Models.AuditModule;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using SSLAudit.Models;
using StackExchange.Exceptional;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;


namespace ShampanERP.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    public class HomeController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _context;
		private readonly ICompanyInfoService _companyInfoService;
		private readonly IUserRollsService _userRollsService;
		private readonly IAuditMasterService _auditMasterService;
		private readonly IMemoryCache _memoryCache;
		//private readonly IScreenService _IScreenService;

		public HomeController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<IdentityRole> roleManager,
			IConfiguration configuration,
			ApplicationDbContext context,
			ICompanyInfoService companyInfoService,
			IUserRollsService userRollsService,
			IAuditMasterService auditMasterService,
			IMemoryCache memoryCache
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_context = context;
			_companyInfoService = companyInfoService;
			_userRollsService = userRollsService;
			_auditMasterService = auditMasterService;
			_memoryCache = memoryCache;

		}



		[Authorize]
		public IActionResult Index(string IsShow)
		{
			//ResultModel<int> indexDataCount = _auditMasterService.GetAuditStatusDataCount(null, null, null);
			List<BranchProfile> branchProfiles = new List<BranchProfile>();
			Claim? currentBranchClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimNames.CurrentBranch);

			if (IsShow == "showpopup")
			{
				if (currentBranchClaim is null)
				{
					var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
					if (userIdClaim is null) return RedirectToAction("Index", "Login");
					var resultModel = _companyInfoService.GetBranches(new[] { "userid" }, new[] { userIdClaim.Value });
					if (resultModel.Status != Status.Success) return RedirectToAction("Index", "Login");
					branchProfiles = resultModel.Data;
				}
				return View(branchProfiles);
			}
			else
			{
                return View(branchProfiles);

            }
            //if (currentBranchClaim is null)
            //{
            //	var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
            //	if (userIdClaim is null) return RedirectToAction("Index", "Login");
            //	var resultModel = _companyInfoService.GetBranches(new[] { "userid" }, new[] { userIdClaim.Value });
            //	if (resultModel.Status != Status.Success) return RedirectToAction("Index", "Login");
            //	branchProfiles = resultModel.Data;
            //}
            //return View(branchProfiles);
        }



		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AssignBranch(BranchProfile branch)
		{
			try
			{
				if (branch.BranchID == 0) RedirectToAction("Index");

				var identity = new ClaimsIdentity(User.Identity);
				identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branch.BranchID.ToString()));
				identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branch.BranchName.ToString().Trim()));
				// Create a new principal with the updated identity
				var principal = new ClaimsPrincipal(identity);
				// Sign in the user with the new principal
				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					principal,
					new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

				//change of brach
				//Store the same data in the cache
				var cacheKey = $"{User.Identity.Name}_BranchClaim";
				var branchClaim = new BranchClaim
				{
					BranchId = branch.BranchID,
					BranchName = branch.BranchName.Trim()
				};

                //AdditionForUpdateBranchName
                string userName = User.Identity.Name;
                ResultModel<List<UserBranch?>> rolls = _auditMasterService.GetUserIdbyUserName(userName);
                UserBranch ur = rolls.Data.FirstOrDefault();
				ur.BranchName = branch.BranchName.Trim();

                ResultModel<UserBranch>  model = _auditMasterService.UpdateBranchName(ur);




                //Set the cache entry with a specified expiration time(e.g., 1 hour)
                _memoryCache.Set(cacheKey, branchClaim, TimeSpan.FromHours(1));
				//end 
			}
			catch (Exception e)
			{
				await e.LogAsync(ControllerContext.HttpContext);
			}
			return RedirectToAction("Index");
		}





		[Authorize]
		public async Task<IActionResult> ChangeBranch()
		{
			try
			{
				var identity = User.Identity as ClaimsIdentity;
				Claim currentBranchClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimNames.CurrentBranch);
				Claim currentBranchNameClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimNames.CurrentBranchName);

				if (currentBranchClaim is null) RedirectToAction("Index");

				identity?.RemoveClaim(currentBranchClaim);
				identity?.RemoveClaim(currentBranchNameClaim);
				// Create a new principal with the updated identity
				var principal = new ClaimsPrincipal(identity);
				// Sign in the user with the new principal
				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					principal,
					new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

			}
			catch (Exception e)
			{
				await e.LogAsync(ControllerContext.HttpContext);
			}
			//return RedirectToAction("Index");
			return RedirectToAction("Index",new { IsShow = "showpopup" });
		}


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


		[HttpGet]
		public async Task<ActionResult> LoginWeb()
		{
			List<CompanyInfo> companyInfos = new List<CompanyInfo>();
			LoginModel loginModel = new LoginModel();

			var resultModel = _companyInfoService.GetAll(null, null);

			if (resultModel.Success)
			{
				companyInfos = resultModel.Data;
			}

			loginModel.CompanyInfos = companyInfos;

			return View("Login", loginModel);
		}



		[HttpPost]
		public async Task<ActionResult> LoginWeb(LoginResource model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("UserName", "Invalid username or password.");

				//return RedirectToAction("Index", "Login");
				return View("index", model);
				// return BadRequest();
			}

			var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

			if (!result.Succeeded) return BadRequest("Wrong username or password");

			var user = _userManager.Users.SingleOrDefault(x => x.UserName == model.UserName);

			if (user == null) return BadRequest("Wrong username or password");

			IList<string> roles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
			};

			if (roles.Any())
				claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var claimsIdentity = new ClaimsIdentity(
				claims, CookieAuthenticationDefaults.AuthenticationScheme);

			var authProperties = new AuthenticationProperties
			{
				
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
				IssuedUtc = DateTimeOffset.Now,			
				RedirectUri = "/"
				
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);

			ViewData["Username"] = model.UserName;

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<ActionResult> RegistrationWeb()
		{
			return View("RegistrationWeb");
		}


		[HttpPost]
		public async Task<ActionResult> RegistrationWeb(LoginResource model)
		{
			if (!ModelState.IsValid)
			{
				// BadRequest();
				return RedirectToAction("RegistrationWeb", "Home");
			}

			if (ModelState.IsValid)
			{
				var _user = new ApplicationUser { UserName = model.UserName };
				var _result = await _userManager.CreateAsync(_user, model.Password);



				if (_result.Succeeded)
				{
					await _signInManager.SignInAsync(_user, false);

				}
				else
				{
					foreach (var error in _result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return RedirectToAction("RegistrationWeb", "Home");
				}
				//}



				var authProperties = new AuthenticationProperties
				{
					//AllowRefresh = <bool>,
					// Refreshing the authentication session should be allowed.

					//ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
					// The time at which the authentication ticket expires. A 
					// value set here overrides the ExpireTimeSpan option of 
					// CookieAuthenticationOptions set with AddCookie.

					//IsPersistent = true,
					// Whether the authentication session is persisted across 
					// multiple requests. When used with cookies, controls
					// whether the cookie's lifetime is absolute (matching the
					// lifetime of the authentication ticket) or session-based.

					//IssuedUtc = <DateTimeOffset>,
					// The time at which the authentication ticket was issued.

					//RedirectUri = <string>
					// The full path or absolute URI to be used as an http 
					// redirect response value.
				};



			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<ActionResult> LogOutWeb()
		{
			await HttpContext.SignOutAsync(
				CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Index", "Login");

		}



		[HttpGet]
		public ActionResult LeftSideBar()
		{
			//ResultModel<List<Screen>> result = new ResultModel<List<Screen>>();
			try
			{
				//string userName = User.Identity.Name;
				//result = _IScreenService.GetAll(null, null);
				//for(int i=0; i<result.Data.Count(); i++)
				//{
				//    var test = result.Data[i].URL.Split("/");
				//    if (test.Length > 0)
				//    {
				//        var ts = test[0];
				//        var ts1 = test[1];
				//        result.Data[i].ActionName = ts;
				//        result.Data[i].Index = ts1;
				//    }
				//}

			}
			catch (Exception ex)
			{

			}
			return PartialView("_leftSideBar", "");
		}




		//[ChildActionOnly]
		public List<UserManuInfo> GetSideBar()
		{
			string userName = User.Identity.Name;

			List<UserManuInfo> manu = _userRollsService.GetUserManu(userName);

			return manu;
		}




		// [HttpGet]
		//public async Task<ActionResult<List<Color>>> GetColors()
		//{
		//    try
		//    {
		//        var resultModel = _colorService.GetAll();


		//        if (resultModel.Status == Status.Success)
		//        {
		//            return resultModel.Data;
		//        }

		//        return BadRequest();

		//    }
		//    catch (Exception e)
		//    {

		//        return BadRequest(e);
		//    }

		//}



		//[Authorize(Roles = "Admin")]
		public async Task Exceptions()
		{
			await ExceptionalMiddleware.HandleRequestAsync(HttpContext).ConfigureAwait(false);
		}


		public ActionResult GetEvents()
		{
			List<object> birthdayList = new List<object>
			{
				new
				{
					title = "Birthday",
					description = "Birthday",
					start = DateTime.Parse("2023-07-01"),
					end = DateTime.Parse("2023-07-01"),
					color = "blue",
					allDay = true
				},
				new
				{
					title = "Another Birthday",
					description = "Another Birthday",
					start = DateTime.Parse("2023-08-15"),
					end = DateTime.Parse("2023-08-15"),
					color = "red",
					allDay = true
				},
                // Add more objects as needed
            };

			return Ok(birthdayList);
		}


		public ActionResult GetEmployees()
		{
			List<object> chartData = new List<object>();

			string query = "SELECT EmployeeId, Name, Designation, isnull(ReportingManager,'')ReportingManager ";
			query += " FROM EmployeesHierarchy";

			string constr = "Data Source=192.168.15.100,1419\\MSSQLSERVER2019;Initial Catalog=AjaxSamples;User id=sa;Password=S123456_;";
			using (SqlConnection con = new SqlConnection(constr))
			{
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					con.Open();

					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						foreach (DataRow row in dt.Rows)
						{
							chartData.Add(new object[]
							{
								row["EmployeeId"], row["Name"], row["Designation"], row["ReportingManager"]
							});
						}
					}



					con.Close();
				}
			}

			return Ok(chartData);

		}





        //ForPlanUnPlan
        [HttpPost]
        public IActionResult UpdateAuditComponents(string selectedValue)
        {
            // Use the selectedValue to update AuditComponentList
            var updatedAuditComponents = _userRollsService.GetAuditComponents();


            // Update the HttpContext.Items
            //HttpContext.Items["AuditComponentList"] = updatedAuditComponents;
            HttpContext.Items["AuditComponentList"] = null;

			// Return the updated data if needed
			//return Json(updatedAuditComponents);
			return RedirectToAction("Index");
        }


    }
}