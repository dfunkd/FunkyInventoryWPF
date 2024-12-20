using FunkyInventoryWPF.Models.UserModels;
using FunkyInventoryWPF.Services;
using FunkyInventoryWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FunkyInventoryWPF.UserControls;

public partial class RecipeControl : UserControl
{
    #region Properties
    private IRecipeService recipeService;
    private CancellationToken cancellationToken = default;
    private RecipeControlViewModel VM;
    private readonly MainWindow inventoryWindow;
    private bool _suspendChangeHandlers = false;
    #endregion

    public RecipeControl(MainWindow parent, RecipeControlViewModel vm, IRecipeService recipeService)
    {
        InitializeComponent();

        this.recipeService = recipeService;
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

    private async void OnLoaded(object sender, RoutedEventArgs e)
        => LoadData();

    #region Functions
    private async void LoadData()
    {
        if (DataContext is RecipeControlViewModel vm)
            vm.Recipes = [.. await recipeService.GetAllRecipes()];
    }
    #endregion
}
