CREATE PROCEDURE [dbo].[spSaleReport]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[u].[FirstName],
		[u].[LastName],
		[u].[EmailAddress],
		[s].[SaleDate], 
		[s].[SubTotal], 
		[s].[Tax], 
		[s].[Total]
	FROM dbo.Sale s
	INNER JOIN dbo.[User] u ON u.Id = s.CashierId

END