using FunkyInventoryWPF.Models.UserModels;
using FunkyInventoryWPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FunkyInventoryWPF.ViewModels;

public class UserAdministrationControlViewModel : ViewModelBase, INotifyPropertyChanged
{
    private Predicate<object> filter;
    public  Predicate<object> Filter
    {
        get => filter;
        set
        {
            if (filter != value)
            {
                filter = value;
                OnPropertyChanged();
            }
        }
    }

    private string password;
    public string Password
    {
        get => password;
        set
        {
            if (password != value)
            {
                password = value;
                OnPropertyChanged();
            }
        }
    }

    private string searchString;
    public string SearchString
    {
        get => searchString;
        set
        {
            if (searchString != value)
            {
                searchString = value;
                OnPropertyChanged();
                Filter = string.IsNullOrEmpty(searchString) ? (Predicate<object>)null : this.IsMatch;
            }
        }
    }

    private string title;
    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged();
            }
        }
    }

    private string userName;
    public string UserName
    {
        get => userName;
        set
        {
            if (userName != value)
            {
                userName = value;
                OnPropertyChanged();
            }
        }
    }

    private Role? selectedRole = null;
    public Role? SelectedRole
    {
        get => selectedRole;
        set
        {
            if (selectedRole != value)
            {
                selectedRole = value;
                OnPropertyChanged();
            }
        }
    }

    private User? selectedUser = null;
    public User? SelectedUser
    {
        get => selectedUser;
        set
        {
            if (selectedUser != value)
            {
                selectedUser = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<Role> roles = [];
    public ObservableCollection<Role> Roles
    {
        get => roles;
        set
        {
            if (roles != value)
            {
                roles = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<User> users = [];
    public ObservableCollection<User> Users
    {
        get => users;
        set
        {
            if (users != value)
            {
                users = value;
                OnPropertyChanged();
            }
        }
    }

    public AddUserRequest? GetAddUserRequest()
        => SelectedUser is not null ? new()
        {
            Email = SelectedUser.Email,
            EncryptedPassword = SelectedUser.EncryptedPassword,
            FirstName = SelectedUser.FirstName,
            LastName = SelectedUser.LastName,
            Password = SelectedUser.Password,
            RoleId = SelectedUser.RoleId,
            UserName = SelectedUser.UserName
        } : null;

    public UpdateUserRequest? GetUpdateUserRequest()
        => SelectedUser is not null ? new()
        {
            Email = SelectedUser.Email,
            FirstName = SelectedUser.FirstName,
            LastName = SelectedUser.LastName,
            LastLogin = SelectedUser.LastLogin,
            RoleId = SelectedUser.RoleId,
            UserName = SelectedUser.UserName
        } : null;

    private bool IsMatch(object item)
        => IsMatch((User)item, searchString);

    private static bool IsMatch(User user, string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
            return true;

        var name = user.UserName;
        if (string.IsNullOrEmpty(name))
            return false;

        if (searchString.Length == 1)
            return name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase);

        return name.IndexOf(searchString, 0, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public bool IsValid()
    {
        return SelectedUser is not null
            && SelectedUser.Email is not null
            && SelectedUser.EncryptedPassword is not null
            && SelectedUser.FirstName is not null
            && SelectedUser.LastName is not null
            && SelectedUser.Password is not null
            && SelectedUser.UserName is not null
            && SelectedUser.Role is not null;
    }
}
