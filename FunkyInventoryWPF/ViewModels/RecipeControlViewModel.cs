using FunkyInventoryWPF.Models.RecipeModels;
using FunkyInventoryWPF.Models.UserModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FunkyInventoryWPF.ViewModels;

public class RecipeControlViewModel : ViewModelBase
{
    private Predicate<object> filter;
    public Predicate<object> Filter
    {
        get => filter;
        set
        {
            if (filter != value)
            {
                filter = value;
                OnPropertyChanged();
            }
        }
    }

    private string searchString = string.Empty;
    public string SearchString
    {
        get => searchString;
        set
        {
            if (searchString != value)
            {
                searchString = value;
                OnPropertyChanged();
                Filter = string.IsNullOrEmpty(searchString) ? (Predicate<object>)null : this.IsMatch;
            }
        }
    }

    private bool isEditable = true;
    public bool IsEditable
    {
        get => isEditable;
        set
        {
            if (isEditable != value)
            {
                isEditable = value;
                OnPropertyChanged();
            }
        }
    }

    private Recipe? selectedRecipe;
    public Recipe? SelectedRecipe
    {
        get => selectedRecipe;
        set
        {
            if (selectedRecipe != value)
            {
                selectedRecipe = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<Recipe> recipes = [];
    public ObservableCollection<Recipe> Recipes
    {
        get => recipes;
        set
        {
            if (recipes != value)
            {
                recipes = value;
                OnPropertyChanged();
            }
        }
    }

    private bool IsMatch(object item)
        => IsMatch((Recipe)item, searchString);

    private static bool IsMatch(Recipe user, string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
            return true;

        var name = user.Name;
        if (string.IsNullOrEmpty(name))
            return false;

        if (searchString.Length == 1)
            return name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase);

        return name.IndexOf(searchString, 0, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}