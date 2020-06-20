﻿CREATE TABLE [dbo].[Sale]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1), 
    [CashierId] NVARCHAR(128) NOT NULL, 
    [SaleDate] DATETIME2 NOT NULL, 
    [SubTotal] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL, 
    [Total] MONEY NOT NULL,

)
