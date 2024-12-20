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

        #region LogoutCommand
        private static readonly RoutedCommand logoutCommand = new();
        public static RoutedCommand LogoutCommand = logoutCommand;
        private void CanExecuteLogoutCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control;
        private void ExecutedLogoutCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.LoggedInUser = null;
                vm.Visibility = Visibility.Collapsed;
                ChangeContent(((App)Application.Current).LoginControl);
            }
        }
        #endregion
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events
        private void OnDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
            => ChangeContent(((App)Application.Current).LoginControl);
            //=> ChangeContent(((App)Application.Current).TitleControl);
        #endregion

        #region Functions
        public void ChangeContent(Control control)
        {
            dpContent.Children.Clear();
            dpContent.Children.Add(control);
        }
        #endregion
    }
}