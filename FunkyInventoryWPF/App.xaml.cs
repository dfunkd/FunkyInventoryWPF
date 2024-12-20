using FunkyInventoryWPF.Data.Repositories;
using FunkyInventoryWPF.Services;
using FunkyInventoryWPF.UserControls;
using FunkyInventoryWPF.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Security.Policy;
using System.Windows;

namespace FunkyInventoryWPF;

//https://gist.github.com/sajidmohammed88/99928bbb3e94028fc6be175d67c0b46f
public partial class App : Application
{
    #region UserControls
    public LoginControl LoginControl { get; set; }
    public RecipeControl RecipeControl { get; set; }
    public RegistrationControl RegistrationControl { get; set; }
    public SplashControl SplashControl { get; set; }
    public TitleControl TitleControl { get; set; }
    public UserAdministrationControl UserAdministrationControl { get; set; }
    #endregion

    public MainWindow MainWindow { get; set; }

    public App()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.ConfigureServices();

        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        LoginControl = serviceProvider.GetRequiredService<LoginControl>();
        RecipeControl = serviceProvider.GetRequiredService<RecipeControl>();
        RegistrationControl = serviceProvider.GetRequiredService<RegistrationControl>();
        SplashControl = serviceProvider.GetRequiredService<SplashControl>();
        TitleControl = serviceProvider.GetRequiredService<TitleControl>();
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

        services.AddSingleton<RecipeControlViewModel>();
        services.AddSingleton<RecipeControl>();

        services.AddSingleton<RegistrationControlViewModel>();
        services.AddSingleton<RegistrationControl>();

        services.AddSingleton<SplashControlViewModel>();
        services.AddSingleton<SplashControl>();

        services.AddSingleton<TitleControlViewModel>();
        services.AddSingleton<TitleControl>();

        services.AddSingleton<UserAdministrationControlViewModel>();
        services.AddSingleton<UserAdministrationControl>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddScoped<ILoginService,  LoginService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ILoginRepository,  LoginRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
