CREATE PROCEDURE [dbo].[spRecipeIngredient_GetIngredientsByRecipeId]
	@RecipeId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT i.[Id]
		  ,i.[Name]
	FROM [dbo].[Ingredient] i INNER JOIN [dbo].[RecipeIngredient] ri
	ON i.Id = ri.FK_IngredientId
	WHERE ri.FK_RecipeId = @RecipeId

END
