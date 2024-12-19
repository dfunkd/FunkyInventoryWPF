using FunkyInventoryWPF.Models.UserModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace FunkyInventoryWPF.ViewModels;

public class UserAdministrationControlViewModel : ViewModelBase, INotifyPropertyChanged
{
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
