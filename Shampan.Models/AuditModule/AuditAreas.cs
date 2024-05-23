using System.ComponentModel.DataAnnotations;

namespace Shampan.Models.AuditModule;

public class AuditAreas
{

    public int Id { get; set; }
    public int AuditId { get; set; }


    [Display(Name = "Audit Type")]
    public string AuditType { get; set; }


    [Display(Name = "Audit Area")]
    public string AuditArea { get; set; }


    public string? AreaDetails { get; set; }
    public string Operation { get; set; } = "add";


    public string Edit { get; set; } = "";
}