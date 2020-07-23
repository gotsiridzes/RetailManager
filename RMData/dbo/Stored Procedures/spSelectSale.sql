CREATE PROCEDURE [dbo].[spSelectSale]
	@CashierId nvarchar(128),
	@SaleDate datetime2
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.Id
	FROM dbo.Sale s
	WHERE s.CashierId = @CashierId 
		AND s.SaleDate = @SaleDate
END
