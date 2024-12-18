namespace FunkyInventoryWPF.Models;

public static class Authentication
{
    public static string Token { get; set; } = string.Empty;
    public static DateTime TokenExpiration { get; set; } = DateTime.Now;
}
