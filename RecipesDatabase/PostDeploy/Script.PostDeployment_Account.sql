/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
DECLARE @AdminRoleId INT, @GeneralRoleId INT, @UserId INT;

CREATE TABLE #NetRoles ([Id] [INT] IDENTITY(1,1) NOT NULL, [Name] [NVARCHAR](100) NOT NULL, [Description] [NVARCHAR](250) )
INSERT INTO #NetRoles([Name], [Description])
VALUES ('Admin', 'Administrator'),
	   ('General', 'General User')
--Insert into physical table
INSERT INTO [dbo].[Roles] ([Name], [Description])
SELECT [Name], [Description] 
FROM #NetRoles
WHERE [Name] NOT IN (SELECT [Name] FROM [dbo].[Roles] WITH (NOLOCK))
DROP TABLE #NetRoles

CREATE TABLE #NetUser
(
	[Id] BIGINT NOT NULL IDENTITY(1,1), 
	[FirstName] NVARCHAR(150), 
	[Surname] NVARCHAR(150), 
	[MobileNumber] NVARCHAR(20), 
	[EmailAddress] NVARCHAR(200), 
	[PasswordHash] NVARCHAR(MAX), 
	[DateCreated] DATETIME DEFAULT GETDATE(),
	[LastLogin] DATETIME
)

--"password": "Admin"
INSERT INTO #NetUser([FirstName], [Surname], [MobileNumber], [EmailAddress], [PasswordHash], [DateCreated], [LastLogin])
VALUES('Sifiso', 'Sikhakhane', '27719217216', 'ss.vincen@gmail.com', 'wcIksDzZvHtqhtd/XazkAZF2bEhc1V3EjK+ayHMzXW8=', GETDATE(), GETDATE())

--Insert into physical table
INSERT INTO [dbo].[Users] ( 
	 [FirstName]
	,[Surname]
	,[MobileNumber]
	,[EmailAddress]
	,[PasswordHash]
	,[DateCreated] 
	,[LastLogin]
)
SELECT [FirstName]
	,[Surname]
	,[MobileNumber]
	,[EmailAddress]
	,[PasswordHash]
	,[DateCreated] 
	,[LastLogin]
FROM #NetUser
WHERE [EmailAddress] NOT IN (SELECT [EmailAddress] FROM [dbo].[Users] WITH (NOLOCK))
DROP TABLE #NetUser

SELECT TOP 1 @AdminRoleId = [Id] FROM [dbo].[Roles] WITH (NOLOCK) WHERE [Name] = 'Admin'
SELECT TOP 1 @GeneralRoleId = [Id] FROM [dbo].[Roles] WITH (NOLOCK) WHERE [Name] = 'General'
SELECT TOP 1 @UserId = [Id] FROM [dbo].[Users] WITH (NOLOCK) WHERE [EmailAddress] = 'ss.vincen@gmail.com'


CREATE TABLE #NetUserRoles
(
	[Id] BIGINT NOT NULL IDENTITY(1,1), 
	[UserId] BIGINT NOT NULL, 
	[RoleId] INT NOT NULL
)

INSERT INTO #NetUserRoles(UserId, RoleId)
VALUES (@UserId, @AdminRoleId),
	   (@UserId, @GeneralRoleId)


INSERT INTO dbo.UserRoles([FK_UserId], [FK_RoleId])
SELECT UserId
	  ,RoleId
FROM #NetUserRoles
WHERE UserId NOT IN (SELECT [FK_UserId] FROM [dbo].[UserRoles] WITH (NOLOCK))
AND RoleId NOT IN (SELECT [FK_RoleId] FROM [dbo].[UserRoles] WITH (NOLOCK))
DROP TABLE #NetUserRoles


CREATE TABLE #NetIngredient ([Id] [INT] IDENTITY(1,1) NOT NULL, [Name] [NVARCHAR](100) NOT NULL)
INSERT INTO #NetIngredient([Name])
VALUES ('Tomato'),
	   ('Onions'),
	   ('Mushrooms'),
	   ('Turnip'),
	   ('Drumstick'),
	   ('Sorrel Leaves'),
	   ('Rocket Leaves'),
	   ('Shark')
--Insert into physical table
INSERT INTO [dbo].[Ingredient] ([Name])
SELECT [Name] 
FROM #NetIngredient
WHERE [Name] NOT IN (SELECT [Name] FROM [dbo].[Ingredient] WITH (NOLOCK))
DROP TABLE #NetIngredient



