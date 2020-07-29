CREATE PROCEDURE [dbo].[spProductSelect]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

	SELECT 
        [Id],
        [ProductName],
        [Description],
        [RetailPrice],
        [QuantityInStock],
        [IsTaxable]
	FROM dbo.Product p
    WHERE p.[ID] = @Id
END