using FunkyInventoryWPF.Models.UserModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FunkyInventoryWPF.ViewModels;

public class UserAdministrationControlViewModel : ViewModelBase, INotifyPropertyChanged
{
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
}
