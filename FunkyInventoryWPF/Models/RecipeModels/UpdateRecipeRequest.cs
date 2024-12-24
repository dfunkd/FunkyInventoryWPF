namespace FunkyInventoryWPF.Models.RecipeModels;

public class UpdateRecipeRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public string? Type { get; set; }

    public int Rating { get; set; } = 0;

    public List<RecipeDirection> Directions { get; set; }
    public List<RecipeIngredient> Ingredients { get; set; }
}
