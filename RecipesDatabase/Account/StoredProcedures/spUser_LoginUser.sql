CREATE PROCEDURE [dbo].[spUser_LoginUser]
	@EmailAddress NVARCHAR(200), 
	@Password NVARCHAR(MAX)
AS
DECLARE @IsSuccess BIT = 0, @Message VARCHAR(250) = '', @Status VARCHAR(250) = '';
BEGIN
	SET NOCOUNT ON;
	
	IF EXISTS (SELECT TOP 1 Id FROM dbo.Users WHERE EmailAddress = @EmailAddress AND PasswordHash = @Password)
		BEGIN
			UPDATE dbo.Users
			SET LastLogin = GETDATE()
			WHERE EmailAddress = @EmailAddress 
			AND PasswordHash = @Password

			SET @IsSuccess = 1;
			SET @Message = 'Login Success';
		END
	ELSE
		BEGIN
			SET @IsSuccess = 0;
			SET @Message = 'Invalid Login Details';		
		END
END
SELECT @IsSuccess [IsSuccess], @Message [Message], @Status [Status]
