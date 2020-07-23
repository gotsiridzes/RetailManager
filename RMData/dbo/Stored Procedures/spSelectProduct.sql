CREATE PROCEDURE [dbo].[spSelectProduct]
    @id INT
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
    WHERE p.[ID] = @id
END