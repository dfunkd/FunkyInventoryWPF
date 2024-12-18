namespace FunkyInventoryWPF.Models.UserModels.Requests;

public class UpdateUserRequest
{
    public DateTime LastLogin { get; set; } = DateTime.Now;

    public Guid RoleId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
