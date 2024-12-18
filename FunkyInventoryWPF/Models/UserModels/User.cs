using System.Data;

namespace FunkyInventoryWPF.Models.UserModels;

public class User : BaseModel
{
    public DateTime? LastLogin { get; set; }

    public Guid RoleId { get; set; }
    public Guid UserId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? EncryptedPassword { get; set; }

    public string? LoggedInAs => $"Logged In As: {FirstName} {LastName} : {Role?.RoleName}";

    public Role? Role { get; set; }
}
