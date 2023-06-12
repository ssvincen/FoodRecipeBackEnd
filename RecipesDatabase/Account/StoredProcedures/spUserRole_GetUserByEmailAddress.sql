CREATE PROCEDURE [dbo].[spUserRole_GetUserByEmailAddress]
	@EmailAddress NVARCHAR(200)
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
	WHERE EmailAddress = @EmailAddress

	SELECT r.Id AS RoleId
		  ,r.[Name] AS RoleName
	FROM dbo.[Users] u 
	INNER JOIN dbo.[UserRoles] ur ON u.Id = ur.FK_UserId 
	INNER JOIN dbo.[Roles] r ON ur.FK_RoleId = r.Id
	WHERE u.EmailAddress = @EmailAddress

END
