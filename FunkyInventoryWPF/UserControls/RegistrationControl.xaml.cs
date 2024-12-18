using FunkyInventoryWPF.Models.UserModels;
using FunkyInventoryWPF.Services;
using FunkyInventoryWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FunkyInventoryWPF.UserControls;

public partial class RegistrationControl : UserControl
{
    #region Dependency Properties
    #region ConfirmPassword dependency property
    public static DependencyProperty ConfirmPasswordProperty = DependencyProperty.Register("ConfirmPassword", typeof(string), typeof(RegistrationControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((RegistrationControl)obj).OnConfirmPasswordChanged(args)));
    public string ConfirmPassword
    {
        get => (string)GetValue(ConfirmPasswordProperty);
        set => SetValue(ConfirmPasswordProperty, value);
    }
    private void OnConfirmPasswordChanged(DependencyPropertyChangedEventArgs args)
        => VM.ConfirmPassword = args.NewValue as string;
    #endregion

    #region Password dependency property
    public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(RegistrationControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((RegistrationControl)obj).OnPasswordChanged(args)));
    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }
    private void OnPasswordChanged(DependencyPropertyChangedEventArgs args)
        => VM.Password = args.NewValue as string;
    #endregion
    #endregion

    #region Routed Commands
    #region CancelCommand
    private static readonly RoutedCommand cancelCommand = new();
    public static RoutedCommand CancelCommand = cancelCommand;
    private void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedCancelCommand(object sender, ExecutedRoutedEventArgs e)
    {
        inventoryWindow.dpContent.Children.Clear();
        inventoryWindow.dpContent.Children.Add(((App)Application.Current).LoginControl);
    }
    #endregion

    #region RegisterCommand
    private static readonly RoutedCommand registerCommand = new();
    public static RoutedCommand RegisterCommand = registerCommand;
    private void CanExecuteRegisterCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        var isValid = (DataContext as RegistrationControlViewModel).IsValid();
        e.CanExecute = e.Source is Control && isValid;
    }
    private async void ExecutedRegisterCommand(object sender, ExecutedRoutedEventArgs e)
    {
        string? pass = Password.HashPassword()?.CalculateSHA256()?.CalculateSHA512().Encrypt();

        RegistrationControlViewModel vm = DataContext as RegistrationControlViewModel;
        if (vm is null)
            return;
        if (await userService.UserNameOrEmailExists(vm.Email, vm.UserName, cancellationToken))
            MessageBox.Show(inventoryWindow, @$"User: {VM.UserName}
Email: {VM.Email}

One or both already exist.", "Duplicate", MessageBoxButton.OK, MessageBoxImage.Error);
        else
        {
            AddUserRequest req = vm.GetAddUserRequest();
            User? user = await userService.AddUser(req, cancellationToken);

            inventoryWindow.dpContent.Children.Clear();
            inventoryWindow.dpContent.Children.Add(((App)Application.Current).LoginControl);
        }
    }
    #endregion
    #endregion

    #region Properties
    private IUserService userService;
    private CancellationToken cancellationToken = default;
    private readonly MainWindow inventoryWindow;
    private RegistrationControlViewModel VM;
    private bool _suspendConfirmPasswordChangeHandlers = false;
    private bool _suspendPasswordChangeHandlers = false;
    #endregion

    public RegistrationControl(MainWindow parent, RegistrationControlViewModel vm, IUserService userService)
    {
        InitializeComponent();

        DataContext = vm;
        inventoryWindow = parent;
        this.userService = userService;
        VM = vm;
    }

    #region Events
    private void Hidden_ConfirmPasswordChanged(object sender, RoutedEventArgs e)
        => ConfirmPasswordChanged(pwbConfirmPassword, txtConfirmPassword);

    private void Hidden_PasswordChanged(object sender, RoutedEventArgs e)
        => PasswordChanged(pwbPassword, txtPassword);

    private void OnShowPasswordClicked(object sender, RoutedEventArgs e)
    {
        if (cbShowPassword.IsChecked == true)
        {
            pwbConfirmPassword.Visibility = Visibility.Collapsed;
            txtConfirmPassword.Visibility = Visibility.Visible;
            pwbPassword.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
        }
        else
        {
            pwbConfirmPassword.Visibility = Visibility.Visible;
            txtConfirmPassword.Visibility = Visibility.Collapsed;
            pwbPassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Collapsed;
        }
    }

    private void Shown_ConfirmPasswordChanged(object sender, TextChangedEventArgs e)
        => PasswordChanged(txtPassword, pwbPassword);

    private void Shown_PasswordChanged(object sender, TextChangedEventArgs e)
        => PasswordChanged(txtConfirmPassword, pwbConfirmPassword);
    #endregion

    #region Functions
    private void ConfirmPasswordChanged(Control active, Control passive)
    {
        if (_suspendConfirmPasswordChangeHandlers)
            return;

        _suspendConfirmPasswordChangeHandlers = true;

        if (active is PasswordBox && passive is TextBox)
            SetCurrentValue(ConfirmPasswordProperty, (active as PasswordBox)?.Password);
        else if (active is TextBox && passive is PasswordBox)
            SetCurrentValue(ConfirmPasswordProperty, (active as TextBox)?.Text);

        _suspendConfirmPasswordChangeHandlers = false;
    }

    private void PasswordChanged(Control active, Control passive)
    {
        if (_suspendPasswordChangeHandlers)
            return;

        _suspendPasswordChangeHandlers = true;

        if (active is PasswordBox && passive is TextBox)
            SetCurrentValue(PasswordProperty, (active as PasswordBox)?.Password);
        else if (active is TextBox && passive is PasswordBox)
            SetCurrentValue(PasswordProperty, (active as TextBox)?.Text);

        _suspendPasswordChangeHandlers = false;
    }
    #endregion
}
