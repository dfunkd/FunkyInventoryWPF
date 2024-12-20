using FunkyInventoryWPF.Models.UserModels;
using System.ComponentModel;
using System.Windows;

namespace FunkyInventoryWPF.ViewModels;

internal class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private string title = "Main Window";
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

    private User? loggedInUser = null;
    public User? LoggedInUser
    {
        get => loggedInUser;
        set
        {
            if (loggedInUser != value)
            {
                loggedInUser = value;
                OnPropertyChanged();
            }
        }
    }

    private Visibility visibility = Visibility.Collapsed;
    public Visibility Visibility
    {
        get => visibility;
        set
        {
            if (visibility != value)
            {
                visibility = value;
                OnPropertyChanged();
            }
        }
    }
}
