using FunkyInventoryWPF.Models.UserModels;
using FunkyInventoryWPF.Services;
using FunkyInventoryWPF.ViewModels;
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
        private async void ExecutedDeleteUserCommand(object sender, ExecutedRoutedEventArgs e)
        {
            bool success = false;
            if (Guid.TryParse((e.OriginalSource as Button)?.Tag.ToString(), out Guid userId) && await userService.DeleteUser(userId, cancellationToken))
            {
                UserAdministrationControlViewModel vm = DataContext as UserAdministrationControlViewModel;
                if (vm is not null)
                {
                    User user = vm.Users.Where(w => w.UserId == userId).FirstOrDefault();
                    if (user is not null)
                    {
                        vm.Users.Remove(user);
                        success = true;
                    }
                }
            }

            if (!success)
                MessageBox.Show(inventoryWindow, "User was not deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region SaveUserCommand
        private static readonly RoutedCommand saveUserCommand = new();
        public static RoutedCommand SaveUserCommand = saveUserCommand;
        private void CanExecuteSaveUserCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            UserAdministrationControlViewModel vm = DataContext as UserAdministrationControlViewModel;
            e.CanExecute = vm is not null && vm.IsValid();
        }
        private void ExecutedSaveUserCommand(object sender, ExecutedRoutedEventArgs e)
        {

        }
        #endregion
        #endregion

        #region Properties
        private IRoleService roleService;
        private IUserService userService;
        private CancellationToken cancellationToken = default;
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
        }

        #region Events
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
                (sender as TextBox).SelectAll();
            if (sender is PasswordBox)
                (sender as PasswordBox).SelectAll();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            UserAdministrationControlViewModel vm = DataContext as UserAdministrationControlViewModel;
            if (vm is not null)
            {
                vm.Users = [.. await userService.GetAllUsers()];
                List<Role> roles = [.. await roleService.GetAllRoles(cancellationToken)];
                vm.Roles = [.. roles.OrderBy(o => o.RoleName)];
            }
        }

        private void OnSelectedRoleChanged(object sender, SelectionChangedEventArgs e)
        {
            UserAdministrationControlViewModel vm = (DataContext as UserAdministrationControlViewModel);
            if (vm is not null)
                vm.SelectedRole = cboRole.SelectedItem as Role;
        }

        private void OnSelectedUserChanged(object sender, SelectionChangedEventArgs e)
        {
            UserAdministrationControlViewModel vm = (DataContext as UserAdministrationControlViewModel);
            if (vm is not null)
                foreach (Role item in cboRole.Items)
                    if (item.RoleId == vm.SelectedUser.RoleId)
                        cboRole.SelectedItem = item;
        }

        private void OnShowPasswordClicked(object sender, RoutedEventArgs e)
        {
            if (cbShowPassword.IsChecked == true)
            {
                txtEncrypted.Visibility = Visibility.Collapsed;
                txtDecrypted.Visibility = Visibility.Visible;
            }
            else
            {
                txtEncrypted.Visibility = Visibility.Visible;
                txtDecrypted.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
    }
}
