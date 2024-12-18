using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FunkyInventoryWPF.ViewModels;

public partial class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new(propertyName));
}