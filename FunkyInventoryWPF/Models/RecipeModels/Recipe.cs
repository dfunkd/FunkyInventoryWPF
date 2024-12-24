namespace FunkyInventoryWPF.Models.RecipeModels;

public class Recipe : BaseModel
{
    public Guid RecipeId { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public string? Type { get; set; }

    public List<RecipeDirection>? Directions { get; set; }
    public List<RecipeIngredient>? Ingredients { get; set; }

    public UpdateRecipeRequest GetUpdateRecipeViewModel()
    {
        return new()
        {
            Description = Description,
            Directions = Directions ?? [],
            Ingredients = Ingredients ?? [],
            Name = Name,
            Note = Note,
            Type = Type
        };
    }
}
