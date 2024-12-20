namespace FunkyInventoryWPF.Models.RecipeModels;

public class AddDirectionToRecipeRequest
{
    public required string Direction { get; set; }
    public int Order { get; set; }
}
