using Dapper;
using Recipes.BO;
using Recipes.BO.Recipe;
using System.Data;

namespace Recipes.BI;

public class RecipesDataAccess : IRecipesDataAccess
{
    private readonly IConnectionManager _connectionManager;


    public RecipesDataAccess(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task<IEnumerable<IngredientviewModel>> GetIngredientsByRecipeIdAsync(int recipeId)
    {
        var param = new DynamicParameters();
        param.Add("@RecipeId", dbType: DbType.Int32, value: recipeId, direction: ParameterDirection.Input);

        using var db = _connectionManager.MainConnection();
        return await db.QueryAsync<IngredientviewModel>("dbo.spRecipeIngredient_GetIngredientsByRecipeId",
               commandType: CommandType.StoredProcedure, param: param);
    }

    public async Task<PaginationResult<ViewRecipeModel>> GetRecipeAsync(PageFilterModel model)
    {
        List<ViewRecipeModel> viewRecipeModel = new List<ViewRecipeModel>();
        var param = new DynamicParameters();
        param.Add("@SearchString", dbType: DbType.String, value: model.SearchString, direction: ParameterDirection.Input);
        param.Add("@PageNumber", dbType: DbType.Int32, value: model.PageNumber, direction: ParameterDirection.Input);
        param.Add("@PageSize", dbType: DbType.Int32, value: model.PageSize, direction: ParameterDirection.Input);

        using var db = _connectionManager.MainConnection();
        using var multi = await db.QueryMultipleAsync("dbo.spRecipeUser_GetRecipes", param, commandType: CommandType.StoredProcedure);
        var data = multi.ReadAsync<ViewRecipeModel>();
        var totalData = await multi.ReadFirstOrDefaultAsync<int>();

        foreach (var item in data.Result)
        {
            viewRecipeModel.Add(new ViewRecipeModel()
            {
                Id = item.Id,
                Name = item.Name,
                Author = item.Author,
                Instructions = item.Instructions,
                ImageName = item.ImageName,
                Ingredient = await GetIngredientsByRecipeIdAsync(item.Id) as List<IngredientviewModel>,
                DateAdd = item.DateAdd,

            });
        }
        return new PaginationResult<ViewRecipeModel>(totalData, model.PageNumber, model.PageSize)
        {
            Items = viewRecipeModel
        };
    }

    public async Task<ReturnResponse> SaveRecipeAsync(long authorId, string imageName, SaveRecipeModel model)
    {
        throw new NotImplementedException();
    }
}
