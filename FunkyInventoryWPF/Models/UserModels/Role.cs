namespace FunkyInventoryWPF.Models.UserModels;

public class Role : BaseModel
{
    private Guid roleId;
    public Guid RoleId
    {
        get => roleId;
        set
        {
            if (roleId != value)
            {
                roleId = value;
                OnPropertyChanged();
            }
        }
    }

    private string? roleName;
    public string? RoleName
    {
        get => roleName;
        set
        {
            if (roleName != value)
            {
                roleName = value;
                OnPropertyChanged();
            }
        }
    }

    private string? normalizedName;
    public string? NormalizedName
    {
        get => normalizedName;
        set
        {
            if (normalizedName != value)
            {
                normalizedName = value;
                OnPropertyChanged();
            }
        }
    }
}
