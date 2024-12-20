using FunkyInventoryWPF.Data.Repositories;
using FunkyInventoryWPF.Services;
using FunkyInventoryWPF.UserControls;
using FunkyInventoryWPF.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FunkyInventoryWPF;

//https://gist.github.com/sajidmohammed88/99928bbb3e94028fc6be175d67c0b46f
public partial class App : Application
{
    #region UserControls
    public LoginControl LoginControl { get; set; }
    public RegistrationControl RegistrationControl { get; set; }
    public UserAdministrationControl UserAdministrationControl { get; set; }
    #endregion

    public MainWindow MainWindow { get; set; }

    public App()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.ConfigureServices();

        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        LoginControl = serviceProvider.GetRequiredService<LoginControl>();
        RegistrationControl = serviceProvider.GetRequiredService<RegistrationControl>();
        UserAdministrationControl = serviceProvider.GetRequiredService<UserAdministrationControl>();

        MainWindow = serviceProvider.GetRequiredService<MainWindow>();
        MainWindow.Show();
    }
}

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<LoginControlViewModel>();
        services.AddSingleton<LoginControl>();

        services.AddSingleton<RegistrationControlViewModel>();
        services.AddSingleton<RegistrationControl>();

        services.AddSingleton<UserAdministrationControlViewModel>();
        services.AddSingleton<UserAdministrationControl>();
        //services.AddSingleton<UserListViewModel>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddScoped<ILoginService,  LoginService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ILoginRepository,  LoginRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
