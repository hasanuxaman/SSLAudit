using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using StackExchange.Exceptional;

namespace SageERP.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class UserRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ICompanyInfoService _companyInfoService;

        //private readonly IScreenService _IScreenService;

        public UserRoleController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            ICompanyInfoService companyInfoService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _companyInfoService = companyInfoService;
        }

        public IActionResult UserRoleIndex()
        {
            return View();
        }

        public ActionResult Create(UserRole vm)
        {

            ModelState.Clear();




            return View("CreateEdit", vm);

        }

        public async Task<ActionResult> CreateEdit(UserRole master)
        {
            ResultModel<UserRole> result = new ResultModel<UserRole>();
            try
            {
                var user = await _userManager.FindByIdAsync(master.UserId);
                var rolename = _userManager.GetRolesAsync(user).Result.SingleOrDefault();

                string CurentBranchid = User.GetCurrentBranchId();
                var result1 = await _userManager.AddToRoleAsync(user, master.Role);
                if (result1.Succeeded)
                {
                    result.Success = true;
                    result.Status = Status.Success;
                    result.Message = "Successfully Role Created";
                }
                else

                {
                    result.Success = false;
                    result.Status = Status.Warning;
                    result.Message = result1.Errors.SingleOrDefault().Description;
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                await ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

    }
}
