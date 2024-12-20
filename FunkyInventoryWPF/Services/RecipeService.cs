using FunkyInventoryWPF.Data.Repositories;
using FunkyInventoryWPF.Models.RecipeModels;

namespace FunkyInventoryWPF.Services;

public interface IRecipeService
{
    Task<Recipe?> AddRecipe(AddRecipeRequest recipe, CancellationToken cancellationToken = default);
    Task<bool> DeleteRecipe(Guid recipeId, CancellationToken cancellationToken = default);
    Task<List<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default);
    Task<Recipe?> GetRecipeById(Guid recipeId, CancellationToken cancellationToken = default);
    Task<Recipe?> UpdateRecipe(Guid recipeId, UpdateRecipeRequest recipe, CancellationToken cancellationToken = default);
}

public class RecipeService(IRecipeRepository repo) : IRecipeService
{
    public async Task<Recipe?> AddRecipe(AddRecipeRequest recipe, CancellationToken cancellationToken = default)
        => await repo.AddRecipe(recipe, cancellationToken);

    public async Task<bool> DeleteRecipe(Guid recipeId, CancellationToken cancellationToken = default)
        => await repo.DeleteRecipe(recipeId, cancellationToken);

    public async Task<List<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default)
        => await repo.GetAllRecipes(cancellationToken);

    public async Task<Recipe?> GetRecipeById(Guid recipeId, CancellationToken cancellationToken = default)
        => await repo.GetRecipeById(recipeId, cancellationToken);

    public async Task<Recipe?> UpdateRecipe(Guid recipeId, UpdateRecipeRequest recipe, CancellationToken cancellationToken = default)
        => await repo.UpdateRecipe(recipeId, recipe, cancellationToken);
}
