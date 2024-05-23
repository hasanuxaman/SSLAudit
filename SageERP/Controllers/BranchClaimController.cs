using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shampan.Models;
using SSLAudit.Models;
using ShampanERP.Persistence;
using Shampan.Core.Interfaces.Services.UserRoll;

namespace SSLAudit.Controllers
{
	public class BranchClaimController : Controller
	{
		private readonly IMemoryCache _memoryCache;
		private readonly ApplicationDbContext _applicationDb;

		public BranchClaimController(IMemoryCache memoryCache, ApplicationDbContext applicationDb)
		{
			_memoryCache = memoryCache;
			_applicationDb = applicationDb;

		}

		[Authorize] 
		public async Task<IActionResult> SignInUser(string cacheKey)
		{
			//var cacheKey = $"{User.Identity.Name}_BranchClaim";
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
					new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now });

				
			}
			else
			{
				
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
