namespace FunkyInventoryWPF.Models.RecipeModels;

public class UpdateRecipeRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Directions { get; set; }
    public string? Notes { get; set; }
    public string? Type { get; set; }

    public int Rating { get; set; } = 0;

    public List<RecipeIngredient> Ingredients { get; set; }
}
