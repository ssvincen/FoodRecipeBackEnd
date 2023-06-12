CREATE TABLE [dbo].[RecipeIngredient]
(
	[Id] INT NOT NULL,
	[FK_RecipeId] INT,
	[FK_IngredientId] BIGINT,
	CONSTRAINT [PK_RecipeIngredient] PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT [FK_RecipeId] FOREIGN KEY ([FK_RecipeId]) REFERENCES [dbo].[Recipe](Id),
	CONSTRAINT [FK_IngredientId] FOREIGN KEY ([FK_IngredientId]) REFERENCES [dbo].[Ingredient](Id)
)
