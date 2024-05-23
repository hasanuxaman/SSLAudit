using System.ComponentModel.DataAnnotations;
using Shampan.Models;

namespace ShampanERP.Models;

public class LoginResource
{
    public LoginResource()
    {
        CompanyInfos = new List<CompanyInfo>();
    }

    [Required]
    public string UserName { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "MIN_PASSWORD_LENGTH")]
    public string Password { get; set; }

    public string? Message { get; set; }

    [Required]
    public string CompanyName { get; set; }

    public List<CompanyInfo> CompanyInfos { get; set; }

	 public string returnUrl { get; set; }
	 public string Email { get; set; }
}