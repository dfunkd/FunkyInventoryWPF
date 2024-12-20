namespace FunkyInventoryWPF.ViewModels;

public class SplashControlViewModel : ViewModelBase
{
    private string copyRight = $"Funky Designs © {DateTime.Now:yyyy}";
    public string CopyRight
    {
        get => copyRight;
        set
        {
            if (copyRight != value)
            {
                copyRight = value;
                OnPropertyChanged();
            }
        }
    }

    private string title = "Funky Inventory";
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
}
