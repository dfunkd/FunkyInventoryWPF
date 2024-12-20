namespace FunkyInventoryWPF.ViewModels;

public class TitleControlViewModel : ViewModelBase
{
    private bool isAdmin = false;
    public bool IsAdmin
    {
        get => isAdmin;
        set
        {
            if (isAdmin != value)
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }
    }
}
