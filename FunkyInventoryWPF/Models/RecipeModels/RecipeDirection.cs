namespace FunkyInventoryWPF.Models.RecipeModels;

public class RecipeDirection : BaseModel
{
    public Guid RecipeId { get; set; }
    public Guid DirectionId { get; set; }

    public required string Direction { get; set; }

    public int Order { get; set; }
}
