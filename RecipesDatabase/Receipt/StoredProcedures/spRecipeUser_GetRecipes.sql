CREATE PROCEDURE [dbo].[spRecipeUser_GetRecipes]
	@SearchString NVARCHAR(50) = NULL,
	@PageNumber INT = 1,
	@PageSize INT = 6
AS
BEGIN
	SET NOCOUNT ON;

	SELECT r.[Id] 
		  ,u.FirstName + ' ' + u.Surname 'Author'
		  ,r.ImageName
		  ,r.[Name]
		  ,r.Instructions
		  ,r.[DateAdd]
		  ,COUNT(r.Id) OVER() AS TotalItems
	INTO #TempResult
	FROM [dbo].[Recipe] r INNER JOIN [dbo].[Users] u
	ON r.FK_AuthorId = u.Id
	WHERE (@SearchString IS NULL OR [Name] LIKE '%'+ @SearchString +'%')
	ORDER BY r.Id
	OFFSET @PageSize * (@PageNumber - 1) ROWS
    FETCH NEXT @PageSize ROWS ONLY;


	SELECT Id
		  ,Author
		  ,ImageName
		  ,[Name]
		  ,Instructions
		  ,[DateAdd]
	FROM #TempResult

	SELECT TOP 1 TotalItems FROM dbo.[#TempResult]

END

