CREATE PROCEDURE [dbo].[spSelectProducts]
AS
BEGIN
    SET NOCOUNT ON;

	SELECT 
        [Id],
        [ProductName],
        [Description],
        [RetailPrice],
        [QuantityInStock]
	FROM dbo.Product p
	ORDER BY p.ProductName
END