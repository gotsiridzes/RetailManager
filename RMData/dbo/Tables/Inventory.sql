CREATE TABLE [dbo].[Inventory]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1), 
    [ProductId] INT NOT NULL, 
    [Quantity] INT NOT NULL DEFAULT 1, 
    [PurchasePrice] MONEY NOT NULL, 
    [PurchaseDate] DATETIME2 NOT NULL DEFAULT getutcdate()
)
