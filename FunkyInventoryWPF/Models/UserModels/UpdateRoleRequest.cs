namespace FunkyInventoryWPF.Models.UserModels;

public class UpdateRoleRequest
{
    public required string RoleName { get; set; }
    public required string NormalizedName { get; set; }
}
