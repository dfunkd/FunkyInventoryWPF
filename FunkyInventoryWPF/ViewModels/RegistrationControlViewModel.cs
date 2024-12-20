using FunkyInventoryWPF.Models.UserModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FunkyInventoryWPF.ViewModels;

public class RegistrationControlViewModel : ViewModelBase, INotifyPropertyChanged
{
    private string? _firstName;
    public required string FirstName
    {
        get => _firstName;
        set
        {
            if (_firstName != value)
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _lastName;
    public required string LastName
    {
        get => _lastName;
        set
        {
            if (_lastName != value)
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _userName;
    public required string UserName
    {
        get => _userName;
        set
        {
            if (_userName != value)
            {
                _userName = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _email;
    public required string Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _password;
    public required string Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _confirmPassword;
    public required string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            if (_confirmPassword != value)
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsValid()
        => !(string.IsNullOrEmpty(FirstName)
        || string.IsNullOrEmpty(LastName)
        || string.IsNullOrEmpty(Email)
        || string.IsNullOrEmpty(Password)
        || string.IsNullOrEmpty(Email)
        || string.IsNullOrEmpty(ConfirmPassword))
        && Email.IsValidEmail()
        && Password.IsValidPassword()
        && Password == ConfirmPassword;

    public void ResetViewModel()
    {
        ConfirmPassword = string.Empty;
        Email = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Password = string.Empty;
        UserName = string.Empty;
    }

    public AddUserRequest GetAddUserRequest()
        => new()
        {
            Email = Email,
            Password = Password.HashPassword()?.CalculateSHA256()?.CalculateSHA512().Encrypt(),
            EncryptedPassword = Password.Encrypt(),
            FirstName = FirstName,
            LastName = LastName,
            RoleId = Guid.Empty,
            UserName = UserName
        };
}
