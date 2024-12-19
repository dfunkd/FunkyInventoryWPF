using FunkyInventoryWPF.Models;
using FunkyInventoryWPF.Models.UserModels;
using FunkyInventoryWPF.Services;
using FunkyInventoryWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FunkyInventoryWPF.UserControls;

public partial class LoginControl : UserControl
{
    #region Dependency Properties
    #region Password dependency property
    public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(LoginControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (obj, args) => ((LoginControl)obj).OnPasswordChanged(args)));
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
        => inventoryWindow.Close();
    #endregion

    #region LoginCommand
    private static readonly RoutedCommand loginCommand = new();
    public static RoutedCommand LoginCommand = loginCommand;
    private void CanExecuteLoginCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control && VM is not null && VM.IsValid;
    private async void ExecutedLoginCommand(object sender, ExecutedRoutedEventArgs e)
    {
        string? pass = Password.HashPassword()?.CalculateSHA256()?.CalculateSHA512().Encrypt();

        User? user = await loginService.Login(VM.UserName, pass);

        if (user == null)
        {
            MessageBox.Show(inventoryWindow, "The credentials you entered were incorrect.", "Invalid Credentials", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Authentication.TokenExpiration = DateTime.Now.AddMinutes(15);

        inventoryWindow.dpContent.Children.Clear();
        MainWindowViewModel vm = (inventoryWindow.DataContext as MainWindowViewModel);
        if (vm is not null)
            vm.LoggedInUser = user;
        //inventoryWindow.dpContent.Children.Add(((App)Application.Current).TitleControl);
    }
    #endregion

    #region RegisterCommand
    private static readonly RoutedCommand registerCommand = new();
    public static RoutedCommand RegisterCommand = registerCommand;
    private void CanExecuteRegisterCommand(object sender, CanExecuteRoutedEventArgs e)
        => e.CanExecute = e.Source is Control;
    private void ExecutedRegisterCommand(object sender, ExecutedRoutedEventArgs e)
    {
        inventoryWindow.dpContent.Children.Clear();
        inventoryWindow.dpContent.Children.Add(((App)Application.Current).RegistrationControl);
    }
    #endregion
    #endregion

    #region Properties
    private ILoginService loginService;
    private CancellationToken cancellationToken = default;
    private LoginControlViewModel VM;
    private readonly MainWindow inventoryWindow;
    private bool _suspendChangeHandlers = false;
    #endregion

    public LoginControl(MainWindow parent, LoginControlViewModel vm, ILoginService loginService)
    {
        InitializeComponent();

        this.loginService = loginService;
        inventoryWindow = parent;
        DataContext = vm;
        VM = vm;

        Resources["ShowPassStyle"] = (Style)Application.Current.Resources["ShowPassStyle"];
    }

    #region Events
    private void Hidden_PasswordChanged(object sender, RoutedEventArgs e)
        => PasswordChanged(pwbPassword, txtPassword);

    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox)
            (sender as TextBox).SelectAll();
        if (sender is PasswordBox)
            (sender as PasswordBox).SelectAll();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
        => txtUserName.Focus();

    private void OnShowPasswordClicked(object sender, RoutedEventArgs e)
    {
        if (cbShowPassword.IsChecked == true)
        {
            pwbPassword.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
        }
        else
        {
            pwbPassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Collapsed;
        }
    }

    private void Shown_PasswordChanged(object sender, TextChangedEventArgs e)
        => PasswordChanged(txtPassword, pwbPassword);
    #endregion

    #region Functions
    private void PasswordChanged(Control active, Control passive)
    {
        if (_suspendChangeHandlers)
            return;

        _suspendChangeHandlers = true;

        if (active is PasswordBox && passive is TextBox)
            SetCurrentValue(PasswordProperty, (active as PasswordBox)?.Password);
        else if (active is TextBox && passive is PasswordBox)
        {
            SetCurrentValue(PasswordProperty, (active as TextBox)?.Text);
            (passive as PasswordBox).Password = Password;
        }

        _suspendChangeHandlers = false;
    }
    #endregion
}
