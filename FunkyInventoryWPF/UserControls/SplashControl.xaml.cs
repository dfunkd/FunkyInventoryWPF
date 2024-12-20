using FunkyInventoryWPF.ViewModels;
using System.Windows.Controls;

namespace FunkyInventoryWPF.UserControls;

public partial class SplashControl : UserControl
{
    public SplashControl(MainWindow parent, SplashControlViewModel vm)
    {
        InitializeComponent();
    }
}
