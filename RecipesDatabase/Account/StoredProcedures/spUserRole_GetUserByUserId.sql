CREATE PROCEDURE [dbo].[spUserRole_GetUserByUserId]
	@UserId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 Id
	      ,FirstName
		  ,Surname
		  ,MobileNumber
		  ,EmailAddress
		  ,DateCreated
		  ,LastLogin
	FROM dbo.[Users]
	WHERE Id = @UserId

	SELECT r.Id AS RoleId
		  ,r.[Name] AS RoleName
	FROM dbo.[Users] u 
	INNER JOIN dbo.[UserRoles] ur ON u.Id = ur.FK_UserId 
	INNER JOIN dbo.[Roles] r ON ur.FK_RoleId = r.Id
	WHERE u.Id = @UserId

END
