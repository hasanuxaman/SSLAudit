namespace ShampanERP.Models;

public class AuthDataResource
{
    public string Token { get; set; }

    public DateTime ExpireTime { get; set; }

    public string Id { get; set; }

    public string Name { get; set; }

    public IList<string> Roles { get; set; }
}