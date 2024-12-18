namespace FunkyInventoryWPF.Models.UserModels;

public class AddRoleRequest
{
    public required string RoleName { get; set; }
    public required string NormalizedName { get; set; }
}
