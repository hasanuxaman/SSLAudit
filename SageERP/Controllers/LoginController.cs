using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using Shampan.Models.AuditModule;
using ShampanERP.Models;
using ShampanERP.Persistence;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using Microsoft.AspNetCore.Hosting.Server.Features;
using SSLAudit.Controllers;
using SSLAudit.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ShampanERP.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]   
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ICompanyInfoService _companyInfoService;
        private readonly IMemoryCache _memoryCache;

        private DbConfig _dbConfig;

        public LoginController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           IConfiguration configuration,
           ApplicationDbContext context,

            IMemoryCache memoryCache



            , ICompanyInfoService companyInfoService, DbConfig dbConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _companyInfoService = companyInfoService;
            _memoryCache = memoryCache;


            this._dbConfig = dbConfig;

        }


        public IActionResult Index(string returnurl)
        {


            List<CompanyInfo> companyInfos = new List<CompanyInfo>();
            LoginResource loginModel = new LoginResource();

            var resultModel = _companyInfoService.GetAll(null, null);

            if (resultModel.Status == Status.Success)
            {
                companyInfos = resultModel.Data;
            }

            loginModel.CompanyInfos = companyInfos;
            loginModel.returnUrl = returnurl;




            return View(loginModel);
        }

        public async Task<IActionResult> CreateClaims()
        {
            string SageDbName = _dbConfig.SageDbName;

            string userName = "erp";

            var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);

            IdentityResult? result = await _userManager.AddClaimAsync(user, new Claim(ClaimNames.SageDatabase, SageDbName));



            return RedirectToAction("Index", "Home");

        }



        [HttpPost]
        public async Task<ActionResult> Index(LoginResource model, string returnurl)
        {


            //Uri returnurl = new Uri(Request.Host.ToString());
            var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            var url = location.AbsoluteUri;
            List<CompanyInfo> companyInfos = new List<CompanyInfo>();
            var resultModel = _companyInfoService.GetAll(null, null);
            if (resultModel.Status == Status.Success)
            {
                companyInfos = resultModel.Data;
            }
            model.CompanyInfos = companyInfos;


            //if (!ModelState.IsValid)
            //{
            //    //ModelState.AddModelError("UserName", "Invalid Username ");


            //    return View(model);

            //}


            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);


            if (!result.Succeeded)
            {
                ModelState.AddModelError("Message", "Wrong username or password");

                return View(model);
            }

            var user = _userManager.Users.SingleOrDefault(x => x.UserName == model.UserName);

            if (user == null)
            {
                ModelState.AddModelError("Message", "Wrong username or password");

                return View(model);
            }


            IList<string> roles = await _userManager.GetRolesAsync(user);

            var userClaims = await _userManager.GetClaimsAsync(user);

            var dbClaim = userClaims.FirstOrDefault(x => x.Type == ClaimNames.Database);

            if (dbClaim == null)
            {
                ModelState.AddModelError("Message", "Wrong username or password");

                return View(model);
            }

            if (dbClaim.Value != model.CompanyName)
            {
                ModelState.AddModelError("Message", "Wrong username or password");

                return View(model);
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.UserData, user.Id),
            };

            if (roles.Any())
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            claims.AddRange(userClaims);

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {

                IssuedUtc = DateTimeOffset.Now,

            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            ViewData["Username"] = model.UserName;
            //return RedirectToAction("Index", "Home");

            bool b = User.Identity.IsAuthenticated;
            string name = User.Identity.Name;
            //string[] parts = new string[] {"","" };

            //if (returnurl != null)
            //{
            //    //parts  = returnurl.TrimStart('/').Split('/');
            //     parts = returnurl.TrimStart('/').Split('/', '?');
            //}

            //parts  = returnurl.TrimStart('/').Split('/');
            //string a1 = parts[0]; // "Audit"
            //string b1 = parts[1]; // "Edit"
            //string c1 = parts[2]; // "155"


            //string query = parts[3]; 



            if (returnurl == null)
            {
                return RedirectToAction("Index", "Home");
                //return RedirectToAction("ApproveStatusIndex", "Audit");
            }
            //         {
            else //if(returnurl != null)
            {

                // return RedirectToAction(b1, a1);
                return Redirect(returnurl);

                // / Audit / ApproveStatusIndex

            }
            //else
            //{
            //    returnurl = "https://localhost:7031/Audit/Edit/234?edit=auditStatus";
            //    return Redirect(returnurl);
            //}

            //}

        }


        [HttpGet]
        public ActionResult LogOff()
        {
            try
            {
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
                _signInManager.SignOutAsync();
                HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}











