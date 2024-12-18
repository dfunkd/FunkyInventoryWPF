using System.ComponentModel;

namespace FunkyInventoryWPF.ViewModels;

public class LoginControlViewModel : ViewModelBase, INotifyPropertyChanged
{
    private string? password;
    public string? Password
    {
        get => password;
        set
        {
            password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    private string? userName;
    public string? UserName
    {
        get => userName;
        set
        {
            userName = value;
            OnPropertyChanged(nameof(UserName));
        }
    }

    public bool IsValid
        => !(string.IsNullOrEmpty(Password)
        || string.IsNullOrEmpty(userName))
        && Password.IsValidPassword();
}
