using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Shampan.Models
{
	[HtmlTargetElement("current-url")]
	public class CurrentUrlTagHelper : TagHelper
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CurrentUrlTagHelper(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext == null)
			{
				return;
			}

			var path = httpContext.Request.Path;
			var queryString = httpContext.Request.QueryString;

			var currentUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{path}{queryString}";

			output.TagName = null; // Remove the <current-url> tag from the output
			output.Content.SetHtmlContent(currentUrl);
		}
	}
}
