using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Office.Interop.Word;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers.Audit;
using StackExchange.Exceptional;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    public class ForgetPasswordController : Controller
    {
        private readonly ICompanyInfoService _companyInfoService;
        private readonly IMemoryCache _memoryCache;
        private readonly ApplicationDbContext _applicationDb;
        private readonly ILogger<AuditController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private DbConfig _dbConfig;

        public ForgetPasswordController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           IConfiguration configuration,
           ApplicationDbContext context,
          IMemoryCache memoryCache
         , ICompanyInfoService companyInfoService, DbConfig dbConfig
          , ApplicationDbContext applicationDb
           , ILogger<AuditController> logger


            )
        {

            _companyInfoService = companyInfoService;
            _memoryCache = memoryCache;
            this._dbConfig = dbConfig;
            _applicationDb = applicationDb;
            _logger = logger;
            _userManager = userManager;

        }

        public IActionResult Index(string returnurl)
        {
            return View();
        }
        [HttpPost]
        public ActionResult MailSent(LoginResource master)
        {

            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();

            try
            {
                var users = _userManager.Users;
                var namesList = users.ToList();
                string userName = User.Identity.Name;
                bool userck = false;
                bool email = false;
                var emailData = "";

                foreach (var ur in namesList)
                {
                    if (ur.UserName == master.UserName)
                    {
                        userck = true;
                        emailData = ur.Email;
                        break;
                    }
                }
                if (emailData == master.Email)
                {
                    email = true;
                }
                if (userck == true && email == true)
                {
                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];

                  
                    string encryptedEmail = Base64Encode(master.Email);
                    string encryptedUsername = Base64Encode(master.UserName);
                    

                    string Url = urlhttp + "//" + HostUrl + "/ForgetPassword/ChangePasswordIndex" + "?email=" + encryptedEmail + "&username=" + encryptedUsername;
                    
                    //string Url = urlhttp + "//" + HostUrl + "/ForgetPassword/ChangePasswordIndex" + "?email=" + master.Email + "&username=" + master.UserName;              
                    MailService.SendForgetPasswordMail(master.Email, Url);

                    var dataitem = new ResultModel<LoginResource>()
                    {
                        Status = Status.Success,
                        Message = "",
                        Data = null
                    };
                    return Ok(dataitem);
                }
                else
                {
                    var item = new ResultModel<LoginResource>()
                    {
                        Status = Status.Fail,
                        Message = "UserName Or Password is not collerct",
                        Data = null
                    };
                    return Ok(item);
                }

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);

        }

        public ActionResult ChangePasswordIndex(string email, string username)
        {
           
            string decreptedEmail = Encoding.UTF8.GetString(Convert.FromBase64String(email));
            string decreptedUserName = Encoding.UTF8.GetString(Convert.FromBase64String(username));

            UserProfile vm = new UserProfile
            {
                UserName = decreptedUserName,
                Email = decreptedEmail
            };

            return View(vm);
            //return RedirectToAction("Index", "Login");

        }
        [HttpPost]
        public async Task<ActionResult> CreateEditAsync(UserProfile model)
        {
            ResultModel<UserProfile> result = new ResultModel<UserProfile>();
            try
            {
                string decreptedEmail = Encoding.UTF8.GetString(Convert.FromBase64String(model.Email));
                string decreptedUserName = Encoding.UTF8.GetString(Convert.FromBase64String(model.UserName));



                string SageDbName = _dbConfig.SageDbName;             
                var user = await _userManager.FindByNameAsync(decreptedUserName);
                //var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    result.Message = "User not found.";
                    return Ok(result);
                }
	
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				//var resetLink = $"{Request.Scheme}://{Request.Host}/ResetPassword?userId={user.Id}&token={WebUtility.UrlEncode(token)}";
                if(model.Password != null && model.ConfirmPassword != null)
                {
                    if (model.Password == model.ConfirmPassword)
                    {
                        var updateResult = await _userManager.ResetPasswordAsync(user, token, model.ConfirmPassword);
                        if (updateResult.Succeeded)
                        {
                            result.Status = Status.Success;
                            result.Message = "Password updated successfully.";
                            result.Data = model;
                            return Ok(result);
                            //return RedirectToAction("Index", "Login");
                          
                        }
                    }
                }

                result.Message = "Failed to update user profile.";
                result.Status = Status.Fail;
                return Ok(result);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

        public static string Base64Encode(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(bytes);
        }



    }
}
