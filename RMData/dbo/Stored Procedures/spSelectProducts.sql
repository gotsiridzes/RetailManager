CREATE PROCEDURE [dbo].[spSelectProducts]
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