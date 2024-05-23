using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShampanERP.Models;
using ShampanERP.Persistence;

namespace ShampanERP.Controllers.ViewComponent
{
    public class NavigationViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        //private readonly IScreenService _IScreenService;

        public NavigationViewComponent(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context 
            //IScreenService iScreenService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            //_IScreenService = iScreenService;
        }
        public IViewComponentResult Invoke()
        {
            //ResultModel<List<Screen>> result = new ResultModel<List<Screen>>();
            try
            {
                //string userName = User.Identity.Name;
                //result = _IScreenService.GetAll(null, null);
                //for (int i = 0; i < result.Data.FirstOrDefault().chieldScreens.Count(); i++)
                //{
                //    var test = result.Data.FirstOrDefault().chieldScreens.FirstOrDefault().URL.Split("/");
                   
                //        var ActionName = test[0];
                //        var Index = test[1];
                //    //if (ActionName == "#")
                //    //    {
                //    //        Index = "";
                //    //    }
                //    //    else
                //    //    {
                //    //        Index = test[1];
                //    //    }

                //    result.Data.FirstOrDefault().chieldScreens.FirstOrDefault().ActionName = ActionName;
                //    result.Data.FirstOrDefault().chieldScreens.FirstOrDefault().Index = Index;
                   
                //}

            }
            catch (Exception ex)
            {

            }
            return View("_leftSideBar", "");
        }
    }
}
