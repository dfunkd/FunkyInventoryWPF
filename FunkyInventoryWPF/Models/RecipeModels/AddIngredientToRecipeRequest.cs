namespace FunkyInventoryWPF.Models.RecipeModels;

public class AddIngredientToRecipeRequest
{
    public Guid IngredientId { get; set; }
    public required string Measurement { get; set; }
}
