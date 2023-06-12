using Recipes.BO;
using Recipes.BO.Recipe;

namespace Recipes.BI;

public interface IRecipesDataAccess
{
    Task<ReturnResponse> SaveRecipeAsync(long authorId, string imageName, SaveRecipeModel model);
    Task<PaginationResult<ViewRecipeModel>> GetRecipeAsync(PageFilterModel model);
    Task<IEnumerable<IngredientviewModel>> GetIngredientsByRecipeIdAsync(int recipeId);
}
