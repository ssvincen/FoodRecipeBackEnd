namespace Recipes.BO
{
    public class SaveRecipeModel
    {
        public string Name { get; set; }
        public string Instructions { get; set; }
        public List<IngredientviewModel> Ingredient { get; set; }
    }
}
