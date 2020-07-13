CREATE PROCEDURE [dbo].[spSelectUser]
	@id nvarchar(128)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT u.Id, u.FirstName, u.LastName, u.EmailAddress, u.CreatedDate
	FROM [dbo].[User] u
	WHERE u.Id = @id;
END