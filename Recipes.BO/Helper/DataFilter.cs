namespace Recipes.BO;

public static class DataFilter
{
    public static PageFilterModel PageFilterModel(string searchString, int pageNumber, int pageSize)
    {
        return new PageFilterModel()
        {
            SearchString = searchString,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
