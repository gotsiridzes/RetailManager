CREATE PROCEDURE [dbo].[spProductSelectAll]
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
	ORDER BY p.ProductName
END