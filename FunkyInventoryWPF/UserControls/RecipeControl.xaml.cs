using FunkyInventoryWPF.Core.Extensions;
using FunkyInventoryWPF.Models.RecipeModels;
using FunkyInventoryWPF.Services;
using FunkyInventoryWPF.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FunkyInventoryWPF.UserControls;

public partial class RecipeControl : UserControl
{
    #region Routed Commands
    #region AddDirectionCommand
    private static readonly RoutedCommand addDirectionCommand = new();
    public static RoutedCommand AddDirectionCommand = addDirectionCommand;
    private void CanExecuteAddDirectionCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private async void ExecutedAddDirectionCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is RecipeControlViewModel vm && vm.SelectedRecipe is not null)
        {
            Guid recipeId = vm.SelectedRecipe.RecipeId;
            int? nextIndex = vm.SelectedRecipe.Directions?.Select(s => s.Order).Max(m => m);
            if (nextIndex is not null)
                nextIndex += 1;

            AddDirectionToRecipeRequest req = new() { Direction = "Test Add Direction", Order = nextIndex ?? 1 };
            await recipeService.AddDirectionToRecipe(vm.SelectedRecipe.RecipeId, req, cancellationToken);

            LoadData();

            vm.SelectedRecipe = vm.Recipes.Where(w => w.RecipeId == recipeId).FirstOrDefault();

            //vm.SelectedRecipe.Directions?.Add(new() { Direction = "Test Add Direction", Order = nextIndex ?? 1 });
        }
    }
    #endregion

    #region AddRecipeCommand
    private static readonly RoutedCommand addRecipeCommand = new();
    public static RoutedCommand AddRecipeCommand = addRecipeCommand;
    private void CanExecuteAddRecipeCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        bool canExecute = false;

        if (inventoryWindow?.DataContext is MainWindowViewModel vm)
            canExecute = vm.LoggedInUser?.Role?.RoleName == "Admin";

        e.CanExecute = canExecute;
    }
    private void ExecutedAddRecipeCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is RecipeControlViewModel vm)
        {
            lvRecipes.SelectedIndex = -1;
            dpRecipeName.Visibility = Visibility.Visible;
            vm.SelectedRecipe = new() { Name = "" };
            vm.IsEditable = true;
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
        if (DataContext is RecipeControlViewModel vm)
        {
            vm.SelectedRecipe = null;
            lvRecipes.SelectedIndex = 1;
            dpRecipeName.Visibility = Visibility.Collapsed;
            vm.IsEditable = false;
        }
    }
    #endregion

    #region DeleteDirectionCommand
    private static readonly RoutedCommand deleteDirectionCommand = new();
    public static RoutedCommand DeleteDirectionCommand = deleteDirectionCommand;
    private void CanExecuteDeleteDirectionCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void ExecutedDeleteDirectionCommand(object sender, ExecutedRoutedEventArgs e)
    {
    }
    #endregion

    #region DeleteIngredientCommand
    private static readonly RoutedCommand deleteIngredientCommand = new();
    public static RoutedCommand DeleteIngredientCommand = deleteIngredientCommand;
    private void CanExecuteDeleteIngredientCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void ExecutedDeleteIngredientCommand(object sender, ExecutedRoutedEventArgs e)
    {
    }
    #endregion

    #region DeleteRecipeCommand
    private static readonly RoutedCommand deleteRecipeCommand = new();
    public static RoutedCommand DeleteRecipeCommand = deleteRecipeCommand;
    private void CanExecuteDeleteRecipeCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void ExecutedDeleteRecipeCommand(object sender, ExecutedRoutedEventArgs e)
    {
    }
    #endregion

    #region EditCommand
    private static readonly RoutedCommand editCommand = new();
    public static RoutedCommand EditCommand = editCommand;
    private void CanExecuteEditCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        bool canExecute = false;

        if (inventoryWindow?.DataContext is MainWindowViewModel vm)
            canExecute = vm.LoggedInUser?.Role?.RoleName == "Admin";

        e.CanExecute = canExecute;
    }
    private void ExecutedEditCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (DataContext is RecipeControlViewModel vm)
        {
            dpRecipeName.Visibility = Visibility.Visible;
            vm.IsEditable = true;
        }
    }
    #endregion

    #region SaveCommand
    private static readonly RoutedCommand saveCommand = new();
    public static RoutedCommand SaveCommand = saveCommand;
    private void CanExecuteSaveCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void ExecutedSaveCommand(object sender, ExecutedRoutedEventArgs e)
    {

        dpRecipeName.Visibility = Visibility.Collapsed;
    }
    #endregion
    #endregion

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

    #region Events
    private void OnDirectionsLoaded(object sender, RoutedEventArgs e)
        => lvDirections.Items.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));

    private void OnGotFocus(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox)
            (sender as TextBox).SelectAll();
        if (sender is PasswordBox)
            (sender as PasswordBox).SelectAll();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
        => LoadData();
    #endregion

    #region Functions
    private async void LoadData()
    {
        if (DataContext is RecipeControlViewModel vm)
        {
            vm.Recipes = [.. await recipeService.GetAllRecipes()];
            vm.IsEditable = false;
        }
    }
    #endregion

    private async void OnDeleteDirection(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is RecipeDirection dir && DataContext is RecipeControlViewModel vm)
        {
            var recipeId = dir.RecipeId;
            await recipeService.DeleteRecipeDirection(recipeId, dir.DirectionId, cancellationToken);
            LoadData();

            //vm.SelectedRecipe = vm.Recipes.Where(w => w.RecipeId == recipeId).FirstOrDefault();
            for (int i = 0; i < lvRecipes.Items.Count; i++)
                if (((Recipe)lvRecipes.Items[i]).RecipeId == recipeId)
                    lvRecipes.SelectedIndex = i;

            //vm.SelectedRecipe?.Directions?.Remove(dir);
            //for (int i = 0; i < vm.SelectedRecipe?.Directions?.Count; i++)
            //    vm.SelectedRecipe.Directions[i].Order = i + 1;
        }
    }
}
