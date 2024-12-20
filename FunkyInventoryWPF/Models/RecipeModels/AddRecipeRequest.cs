namespace FunkyInventoryWPF.Models.RecipeModels;

public class AddRecipeRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public string? Type { get; set; }

    public int Rating { get; set; } = 0;

    public List<AddIngredientToRecipeRequest> Ingredients { get; set; }
    public List<AddDirectionToRecipeRequest> Directions { get; set; }
}
