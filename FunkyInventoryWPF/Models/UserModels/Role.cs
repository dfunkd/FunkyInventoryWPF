namespace FunkyInventoryWPF.Models.UserModels;

public class Role : BaseModel
{
    public Guid RoleId { get; set; }
    public string? RoleName { get; set; }
    public string? NormalizedName { get; set; }
}
