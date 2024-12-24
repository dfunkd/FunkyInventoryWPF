using Dapper;
using FunkyInventoryWPF.Models.RecipeModels;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FunkyInventoryWPF.Data.Repositories;

public interface IRecipeRepository
{
    Task AddDirectionToRecipe(Guid recipeId, AddDirectionToRecipeRequest req, CancellationToken cancellationToken = default);
    Task<Recipe?> AddRecipe(AddRecipeRequest recipe, CancellationToken cancellationToken = default);
    Task<bool> DeleteRecipe(Guid recipeId, CancellationToken cancellationToken = default);
    Task DeleteRecipeDirection(Guid recipeId, Guid directionId, CancellationToken cancellationToken = default);
    Task<List<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default);
    Task<Recipe?> GetRecipeById(Guid recipeId, CancellationToken cancellationToken = default);
    Task<Recipe?> UpdateRecipe(Guid recipeId, UpdateRecipeRequest recipe, CancellationToken cancellationToken = default);
}

public class RecipeRepository : IRecipeRepository
{
    public async Task AddDirectionToRecipe(Guid recipeId, AddDirectionToRecipeRequest req, CancellationToken cancellationToken = default)
    {
        const string iSql = @"
INSERT INTO RecipeDirection (RecipeId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Direction, [Order])
VALUES (@recipeId, @by, @now, @by, @now, @direction, @order)
";

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new
        {
            recipeId,
            By = "System",
            DateTime.Now,
            req.Direction,
            req.Order
        };

        try
        {
            CommandDefinition iCmd = new(iSql, parms, trx, 150, cancellationToken: cancellationToken);

            await conn.ExecuteAsync(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }
    }

    public async Task<Recipe?> AddRecipe(AddRecipeRequest recipe, CancellationToken cancellationToken = default)
    {
        Recipe? ret = null;
        Guid? res = null;

        const string iSql = @"
INSERT INTO dbo.Recipe (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, [Name], [Description], Note, [Type], Rating)
OUTPUT INSERTED.RecipeId
VALUES (@by, @now, @by, @now, @name, @description, @note, @type, @rating)
";

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new
        {
            By = "System",
            DateTime.Now,
            recipe.Description,
            recipe.Name,
            recipe.Note,
            recipe.Rating,
            recipe.Type
        };

        try
        {
            CommandDefinition iCmd = new(iSql, parms, trx, 150, cancellationToken: cancellationToken);

            res = await conn.QuerySingleAsync<Guid>(iCmd);

            if (res is not null)
            {
                await AddRecipeIngredients(conn, trx, res.Value, recipe.Ingredients, cancellationToken);
                await AddRecipeDirections(conn, trx, res.Value, recipe.Directions, cancellationToken);
            }

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
        }
        finally
        {
            conn.Close();
        }

        if (res is not null)
            ret = await GetRecipeById(res.Value, cancellationToken);

        return ret;
    }

    public async Task<bool> DeleteRecipe(Guid recipeId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteRecipeDirection(Guid recipeId, Guid directionId, CancellationToken cancellationToken = default)
    {
        const string dSql = @"
DELETE FROM RecipeDirection
WHERE RecipeId = @recipeId AND DirectionId = @directionId
";

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new
        {
            recipeId,
            directionId
        };

        try
        {
            CommandDefinition dCmd = new(dSql, parms, trx, 150, cancellationToken: cancellationToken);

            await conn.ExecuteAsync(dCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }
    }

    public async Task<List<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default)
    {
        List<Recipe> ret = [];
        Dictionary<Guid, Recipe> res = [];

        const string sSql = @"
SELECT r.RecipeId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.[Name], r.[Description], r.Note, r.[Type], r.Rating,
	i.RecipeId, i.IngredientId, i.CreatedBy, i.CreatedDate, i.ModifiedBy, i.ModifiedDate, i.Measurement, i.[Name],
	d.RecipeId, d.DirectionId, d.CreatedBy, d.CreatedDate, d.ModifiedBy, d.ModifiedDate, d.Direction, d.[Order]
FROM dbo.Recipe AS r
	INNER JOIN RecipeIngredient AS i ON i.RecipeId = r.RecipeId
    INNER JOIN RecipeDirection AS d ON d.RecipeId = r.RecipeId
";

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        try
        {
            CommandDefinition sCmd = new(sSql, null, null, 150, cancellationToken: cancellationToken);

            _ = await conn.QueryAsync<Recipe, RecipeIngredient, RecipeDirection, Recipe>(sCmd, (recipe, ingredient, direction) =>
            {
                if (recipe?.RecipeId is null)
                    return recipe;

                recipe = res.TryGetValue(recipe.RecipeId, out Recipe? value) ? value : recipe;
                recipe.Ingredients ??= [];
                recipe.Directions ??= [];

                if (!recipe.Ingredients.Any(a => a.IngredientId == ingredient.IngredientId))
                    recipe.Ingredients.Add(ingredient);

                if (!recipe.Directions.OrderBy(o => o.Order).Any(a => a.DirectionId == direction.DirectionId))
                    recipe.Directions.Add(direction);

                if (recipe.RecipeId != Guid.Empty)
                    res[recipe.RecipeId] = recipe;

                return recipe;
            },
            splitOn: "RecipeId, RecipeId, RecipeId");
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        if (res is not null)
            ret = [.. res.Values];

        return ret;
    }

    public async Task<Recipe?> GetRecipeById(Guid recipeId, CancellationToken cancellationToken = default)
    {
        Recipe? ret = null;
        Dictionary<Guid, Recipe> res = [];

        const string sSql = @"
SELECT r.RecipeId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.[Name], r.[Description], r.Note, r.[Type], r.Rating,
	i.RecipeId, i.IngredientId, i.CreatedBy, i.CreatedDate, i.ModifiedBy, i.ModifiedDate, i.Measurement, i.[Name],
	d.RecipeId, d.DirectionId, d.CreatedBy, d.CreatedDate, d.ModifiedBy, d.ModifiedDate, d.Direction, d.[Order]
FROM dbo.Recipe AS r
	INNER JOIN RecipeIngredient AS i ON i.RecipeId = r.RecipeId
    INNER JOIN RecipeDirection AS d ON d.RecipeId = r.RecipeId
WHERE r.RecipeId = @recipeId
";

        var parms = new { recipeId };

        using IDbConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["InventoryDb"].ConnectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        try
        {
            CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);

            _ = await conn.QueryAsync<Recipe, RecipeIngredient, RecipeDirection, Recipe>(sCmd, (recipe, ingredient, direction) =>
            {
                if (recipe?.RecipeId is null)
                    return recipe;

                recipe = res.TryGetValue(recipe.RecipeId, out Recipe? value) ? value : recipe;
                recipe.Ingredients ??= [];
                recipe.Directions ??= [];

                if (!recipe.Ingredients.Any(a => a.IngredientId == ingredient.IngredientId))
                    recipe.Ingredients.Add(ingredient);

                if (!recipe.Directions.OrderBy(o => o.Order).Any(a => a.DirectionId == direction.DirectionId))
                    recipe.Directions.Add(direction);

                if (recipe.RecipeId != Guid.Empty)
                    res[recipe.RecipeId] = recipe;

                return recipe;
            },
            splitOn: "RecipeId, RecipeId, RecipeId");

            if (res is not null)
                ret = res.Values.FirstOrDefault();
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        return ret;
    }

    public async Task<Recipe?> UpdateRecipe(Guid recipeId, UpdateRecipeRequest recipe, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task AddRecipeIngredients(IDbConnection conn, IDbTransaction trx, Guid recipeId, List<AddIngredientToRecipeRequest> reqs,
        CancellationToken cancellationToken = default)
    {
        const string iSql = @"
INSERT INTO dbo.RecipeIngredient (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, RecipeId, IngredientId, Measurement)
VALUES (@by, @now, @by, @now, @recipeId, @ingredientId, @measurement)
";

        try
        {
            foreach (var req in reqs)
            {
                var parms = new
                {
                    By = "System",
                    DateTime.Now,
                    recipeId,
                    req.IngredientId,
                    req.Measurement
                };

                CommandDefinition iCmd = new(iSql, parms, trx, 150, cancellationToken: cancellationToken);
                await conn.ExecuteAsync(iCmd);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task AddRecipeDirections(IDbConnection conn, IDbTransaction trx, Guid recipeId, List<AddDirectionToRecipeRequest> reqs,
        CancellationToken cancellationToken = default)
    {
        const string iSql = @"
INSERT INTO dbo.RecipeDirection (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, RecipeId, Direction, [Order])
VALUES (@by, @now, @by, @now, @recipeId, @direction, @order)
";

        try
        {
            foreach (var req in reqs)
            {
                var parms = new
                {
                    By = "System",
                    DateTime.Now,
                    recipeId,
                    req.Direction,
                    req.Order
                };

                CommandDefinition iCmd = new(iSql, parms, trx, 150, cancellationToken: cancellationToken);
                await conn.ExecuteAsync(iCmd);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
