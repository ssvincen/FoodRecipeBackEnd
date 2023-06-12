namespace Recipes.BO;

public class PageFilterModel
{
    public string SearchString { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 6;
}
