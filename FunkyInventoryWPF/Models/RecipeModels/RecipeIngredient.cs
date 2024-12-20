namespace FunkyInventoryWPF.Models.RecipeModels;

public class RecipeIngredient : BaseModel
{
    public Guid RecipeId { get; set; }
    public Guid IngredientId { get; set; }

    public required string Measurement { get; set; }
}
