CREATE TABLE [dbo].[Customer] (
	[CustomerId] [int] IDENTITY(1,1) NOT NULL CONSTRAINT PK_Customer PRIMARY KEY,
	[Name] [nvarchar](256) NULL,
	[City] [nvarchar](256) NULL)

CREATE TABLE [dbo].[Order] (
	[OrderId] [int] IDENTITY(1,1) NOT NULL CONSTRAINT PK_Order PRIMARY KEY,
	[ProductName] [nvarchar](256) NULL,
	[Price] [int] NULL,
	[CustomerId] [int])

