namespace Shampan.Models;

public class AccountCode
{
    public string AccountNumber { get; set; }

    public string Description { get; set; }

    public string Status { get; set; }

    public string Type { get; set; }
}

public class DbConfig
{
    public string? DbName { get; set; }

    public string? UserId { get; set; }

    public string? Server { get; set; }
    public string? SageDbName { get; set; }

    public string? AuthDB { get; set; }
}

public static class AuthDbConfig
{
    public static string? AuthDB { get; set; }

}