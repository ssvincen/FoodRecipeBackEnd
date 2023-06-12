namespace Recipes.BO.Recipe
{
    public class ViewRecipeModel : SaveRecipeModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Author { get; set; }
        public string ImageData { get; set; }
        public DateTime DateAdd { get; set; }
    }
}

