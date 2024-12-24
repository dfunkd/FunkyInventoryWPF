namespace FunkyInventoryWPF.Models.RecipeModels;

public class Ingredient : BaseModel
{
    public Guid IngredientId { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }
}
