namespace Recipes.BO;

public class IngredientModel
{
    public string Name { get; set; }
}

public class IngredientviewModel : IngredientModel
{
    public int Id { get; set; }
}
