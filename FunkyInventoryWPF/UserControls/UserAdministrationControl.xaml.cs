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
        #region AddUserCommand
        private static readonly RoutedCommand addUserCommand = new();
        public static RoutedCommand AddUserCommand = addUserCommand;
        private void CanExecuteAddUserCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = true;
        private async void ExecutedAddUserCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is UserAdministrationControlViewModel vm)
            {
                vm.Title = "Add User";
                lvUsers.SelectedIndex = -1;
                var pass = "P@ssw0rd";
                vm.SelectedUser = new() { Password = pass.HashPassword()?.CalculateSHA256()?.CalculateSHA512().Encrypt(), EncryptedPassword = pass.Encrypt() };
            }
        }
        #endregion

        #region CancelCommand
        private static readonly RoutedCommand cancelCommand = new();
        public static RoutedCommand CancelCommand = cancelCommand;
        private void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = e.Source is Control;
        private void ExecutedCancelCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is UserAdministrationControlViewModel vm)
                vm.SelectedUser = null;
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
                if (DataContext is UserAdministrationControlViewModel vm)
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

        #region ResetPasswordCommand
        private static readonly RoutedCommand resetPasswordCommand = new();
        public static RoutedCommand ResetPasswordCommand = resetPasswordCommand;
        private void CanExecuteResetPasswordCommand(object sender, CanExecuteRoutedEventArgs e)
            => e.CanExecute = DataContext is UserAdministrationControlViewModel vm && vm.SelectedUser is not null;
        private async void ExecutedResetPasswordCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var pass = "P@ssw0rd";
            if (DataContext is UserAdministrationControlViewModel vm)
            {
                var user = await userService.UpdatePassword(vm.SelectedUser.UserId, pass.HashPassword()?.CalculateSHA256()?.CalculateSHA512().Encrypt(), pass.Encrypt(),
                    cancellationToken);
                if (user is not null)
                {
                    LoadData();
                    selectedUser = user;
                }
            }
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
        private async void ExecutedSaveUserCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is UserAdministrationControlViewModel vm && vm.SelectedUser is not null)
            {
                User? user = null;
                if (vm.SelectedUser.UserId != Guid.Empty)
                {
                    UpdateUserRequest? updateReq = vm.GetUpdateUserRequest();
                    if (updateReq is not null)
                        user = await userService.UpdateUser(vm.SelectedUser.UserId, updateReq, cancellationToken);
                }
                else
                {
                    AddUserRequest? addReq = vm.GetAddUserRequest();
                    if (addReq is not null)
                        user = await userService.AddUser(addReq, cancellationToken);
                }
                if (user is not null)
                {
                    LoadData();
                    selectedUser = user;
                }
            }
        }
        #endregion
        #endregion

        #region Properties
        private IRoleService roleService;
        private IUserService userService;
        private CancellationToken cancellationToken = default;
        private readonly MainWindow inventoryWindow;
        private bool _suspendChangeHandlers = false;
        private User? selectedUser = null;
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

        private void OnLoad(object sender, RoutedEventArgs e)
            => LoadData();

        private void OnSelectedRoleChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is UserAdministrationControlViewModel vm && vm.SelectedUser is not null && vm.SelectedRole is not null)
            {
                vm.SelectedRole = cboRole.SelectedItem as Role;
                vm.SelectedUser.Role = cboRole.SelectedItem as Role;
                vm.SelectedUser.RoleId = (cboRole.SelectedValue as Role).RoleId;
            }
        }

        private void OnSelectedUserChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is UserAdministrationControlViewModel vm)
            {
                if (vm.SelectedUser?.UserId is not null && vm.SelectedUser?.UserId != Guid.Empty)
                    vm.Title = "Update User";
                foreach (Role item in cboRole.Items)
                    if (item.RoleId == vm.SelectedUser?.RoleId)
                        cboRole.SelectedItem = item;
            }
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

        #region Functions
        private async void LoadData()
        {
            if (DataContext is UserAdministrationControlViewModel vm)
            {
                vm.Users = [.. await userService.GetAllUsers()];
                List<Role> roles = [.. await roleService.GetAllRoles(cancellationToken)];
                vm.Roles = [.. roles.OrderBy(o => o.RoleName)];
            }
        }
        #endregion
    }
}
