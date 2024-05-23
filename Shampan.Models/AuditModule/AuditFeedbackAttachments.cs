namespace Shampan.Models.AuditModule;

public class AuditFeedbackAttachments
{
    private string? _displayName;
    public int Id { get; set; }
    public int AuditId { get; set; }
    public int AuditIssueId { get; set; }
    public int AuditFeedbackId { get; set; }
    public string FileName { get; set; }


    public string DisplayName
    {
        get
        {
            if (!string.IsNullOrEmpty(_displayName)) return _displayName;

            if (FileName is not null)
                return Path.GetFileNameWithoutExtension(this.FileName).Split("_shp_")[0] + Path.GetExtension(this.FileName);

            return "";
        }


        set
        {
            this._displayName = value;
        }
    }
}