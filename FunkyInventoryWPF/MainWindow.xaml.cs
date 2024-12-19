using FunkyInventoryWPF.Models.UserModels;
using FunkyInventoryWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FunkyInventoryWPF
{
    public partial class MainWindow : Window
    {
        #region Dependency Properties
        #region LoggedInUser
        public static DependencyProperty LoggedInUserProperty = DependencyProperty.Register("LoggedInUser", typeof(User), typeof(MainWindow),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((MainWindow)obj).OnLoggedInUserChanged(args)));
        public User? LoggedInUser
        {
            get => (User)GetValue(LoggedInUserProperty);
            set => SetValue(LoggedInUserProperty, value);
        }
        private void OnLoggedInUserChanged(DependencyPropertyChangedEventArgs args)
            => (DataContext as MainWindowViewModel).LoggedInUser = args.NewValue is not null ? args.NewValue as User : null;
        #endregion
        #endregion

        #region RoutedCommands
        #region CancelCommand
        private static readonly RoutedCommand cancelCommand = new();
        public static RoutedCommand CancelCommand = cancelCommand;
        private void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control;
        private void ExecutedCancelCommand(object sender, ExecutedRoutedEventArgs e)
            => Close();
        #endregion
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            //var test1 = "Samuelxf11041997!".Encrypt();
            //var test2 = test1.Decrypt();
            //var test3 = "6iJ3mW8q1SfP8N1blxMLN7l3lXP1o9_6A5an6oo3ClU=".Decrypt();

            dpContent.Children.Clear();
            //dpContent.Children.Add(((App)Application.Current).LoginControl);
            dpContent.Children.Add(((App)Application.Current).UserAdministrationControl);
            //dpContent.Children.Add(((App)Application.Current).RegistrationControl);
        }
    }
}