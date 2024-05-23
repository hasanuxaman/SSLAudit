using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShampanERP.Models;
using ShampanERP.Persistence;
using ShampanProcurement.Security;

namespace ShampanERP.Controllers.API
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody]LoginResource model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (!result.Succeeded) return BadRequest("Wrong username or password");

            var user = _userManager.Users.SingleOrDefault(x => x.UserName == model.UserName);

            if (user == null) return BadRequest("Wrong username or password");

            var jsonHandler = new JwtTokenHandler(_configuration,_userManager);

            var authData = new AuthDataResource
            {
                Token = await jsonHandler.GenerateJwtToken(user),
                Id = user.Id,
                ExpireTime = DateTime.Now.AddHours(Convert.ToDouble(_configuration["JwtExpireDays"])),
                Name = user.UserName 
            };

            return Ok(authData);
        }


        [HttpGet,Authorize]
        public IActionResult Values()
        {
            var user = _userManager.Users.SingleOrDefault(x => User.Identity != null && x.UserName == User.Identity.Name);

            return Ok(new {HttpStatusCode.Accepted, user });
        }


        //[HttpPost, Authorize]
        //public async Task<IActionResult> RoleSeed()
        //{
        //    var admin = await _roleManager.FindByNameAsync("Admin");

        //    if (admin == null)
        //        await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

        //    var user = _userManager.Users.SingleOrDefault(x => User.Identity != null && x.UserName == User.Identity.Name);

        //    await _userManager.AddToRoleAsync(user, "Admin");


        //    return Ok();
        //}


        
    }
}
