using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FunkyInventoryWPF.Models;

public class BaseModel : INotifyPropertyChanged
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new(propertyName));
}
