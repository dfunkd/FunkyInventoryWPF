using FunkyInventoryWPF.Models.UserModels;
using FunkyInventoryWPF.Services;
using FunkyInventoryWPF.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FunkyInventoryWPF.UserControls
{
    public partial class UserAdministrationControl : UserControl
    {
        #region RoutedCommands
        #region CancelCommand
        private static readonly RoutedCommand cancelCommand = new();
        public static RoutedCommand CancelCommand = cancelCommand;
        private void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control;
        private void ExecutedCancelCommand(object sender, ExecutedRoutedEventArgs e)
        {

        }
        #endregion

        #region DeleteUserCommand
        private static readonly RoutedCommand deleteUserCommand = new();
        public static RoutedCommand DeleteUserCommand = deleteUserCommand;
        private void CanExecuteDeleteUserCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control;
        private void ExecutedDeleteUserCommand(object sender, ExecutedRoutedEventArgs e)
        {

        }
        #endregion

        #region SaveUserCommand
        private static readonly RoutedCommand saveUserCommand = new();
        public static RoutedCommand SaveUserCommand = saveUserCommand;
        private void CanExecuteSaveUserCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control;
        private void ExecutedSaveUserCommand(object sender, ExecutedRoutedEventArgs e)
        {

        }
        #endregion
        #endregion

        #region Properties
        private IRoleService roleService;
        private IUserService userService;
        private CancellationToken cancellationToken = default;
        private UserAdministrationControlViewModel VM;
        private readonly MainWindow inventoryWindow;
        private bool _suspendChangeHandlers = false;
        #endregion

        public UserAdministrationControl(MainWindow parent, UserAdministrationControlViewModel vm, IUserService userService, IRoleService roleService)
        {
            InitializeComponent();

            this.roleService = roleService;
            this.userService = userService;
            inventoryWindow = parent;
            DataContext = vm;
            VM = vm;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
                (sender as TextBox).SelectAll();
            if (sender is PasswordBox)
                (sender as PasswordBox).SelectAll();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            (DataContext as UserAdministrationControlViewModel).Users = [.. await userService.GetAllUsers()];
            ObservableCollection<Role> roles = [.. await roleService.GetAllRoles(cancellationToken)];
        }
    }
}
