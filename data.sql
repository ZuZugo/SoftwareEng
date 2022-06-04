CREATE DATABASE TestCNPM
go
use TestCNPM
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warehouse](
	[GoodID] [int] identity NOT NULL,
	[Item] [varchar](100) NULL,
	[Quantity] [int] null,
	[Price] [decimal](18, 0) null
PRIMARY KEY CLUSTERED 
(
	[GoodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warehouse_Receipt](
	[No] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Received_Goods](
	[No] [int] NOT NULL,
	[GoodID] [int] NOT NULL,
	[Quantity] [int] null,
	[Price] [decimal](18, 0) null
PRIMARY KEY CLUSTERED 
(
	[No] ASC,
	[GoodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID][int] identity not null,
	[Name][varchar](100)null,
	[Account][varchar](100)null,
	[Password][varchar](100)null,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [int] identity  NOT NULL,
	[Paid] [varchar](50) NULL,
	[Status][varchar](50) null,
	[CustomerID][int] not null
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Detail_Order](
	[OrderID] [int] NOT NULL,
	[GoodID][int] not NULL,
	[Quantity][int]null,
	[Price][decimal](18,0)null
PRIMARY KEY CLUSTERED 
(	
	[GoodID] ASC,
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO 
/****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Delivery_Note](
	[DeliveryID] [int]  NOT NULL,
	[CreateTime] [datetime] NULL,
	[TotalPrice][decimal](18,0) null,
	[OrderID] [int] not null
PRIMARY KEY CLUSTERED 
(
	[DeliveryID] ASC,
	[OrderID] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Received_Goods]  WITH CHECK ADD FOREIGN KEY([No])
REFERENCES [dbo].[Warehouse_Receipt] ([No])
GO

ALTER TABLE [dbo].[Received_Goods]  WITH CHECK ADD FOREIGN KEY([GoodID])
REFERENCES [dbo].[Warehouse] ([GoodID])
GO

ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO

ALTER TABLE [dbo].[Detail_Order]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO

ALTER TABLE [dbo].[Detail_Order]  WITH CHECK ADD FOREIGN KEY([GoodID])
REFERENCES [dbo].[Warehouse] ([GoodID])
GO

ALTER TABLE [dbo].[Delivery_Note]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO


INSERT [dbo].[Warehouse_Receipt] ([No], [CreateTime]) VALUES (1111, '2020-05-15 10:52:22')
INSERT [dbo].[Warehouse_Receipt] ([No], [CreateTime]) VALUES (2222, '2021-03-15 10:52:22')
INSERT [dbo].[Warehouse_Receipt] ([No], [CreateTime]) VALUES (3333, '2021-03-15 10:52:22')
INSERT [dbo].[Warehouse_Receipt] ([No], [CreateTime]) VALUES (4444, '2021-04-15 10:52:22')
INSERT [dbo].[Warehouse_Receipt] ([No], [CreateTime]) VALUES (5555, '2022-04-15 10:52:22')


INSERT [dbo].[Warehouse] ( [Item], [Quantity], [Price]) VALUES ('Ti vi', 500, 20000)
INSERT [dbo].[Warehouse] ( [Item], [Quantity], [Price]) VALUES ('May chieu', 250, 50000)
INSERT [dbo].[Warehouse] ( [Item], [Quantity], [Price]) VALUES ('Chuot may tinh', 175, 15000)
INSERT [dbo].[Warehouse] ( [Item], [Quantity], [Price]) VALUES ('Loa vi tinh', 200, 250000)
INSERT [dbo].[Warehouse] ( [Item], [Quantity], [Price]) VALUES ('Loa karaoke sach tay', 700, 90000)
INSERT [dbo].[Warehouse] ( [Item], [Quantity], [Price]) VALUES ('Bo loa thung', 209, 60000)
INSERT [dbo].[Warehouse] ( [Item], [Quantity], [Price]) VALUES ('May tinh', 200, 40000)


INSERT [dbo].[Received_Goods] ([GoodID], [No], [Quantity], [Price]) VALUES (1, 1111, 10, 15000)
INSERT [dbo].[Received_Goods] ([GoodID], [No], [Quantity], [Price]) VALUES (2, 1111, 15, 30000)
INSERT [dbo].[Received_Goods] ([GoodID], [No], [Quantity], [Price]) VALUES (3, 2222, 30, 60000)
INSERT [dbo].[Received_Goods] ([GoodID], [No], [Quantity], [Price]) VALUES (3, 3333, 30, 60000)
INSERT [dbo].[Received_Goods] ([GoodID], [No], [Quantity], [Price]) VALUES (5, 4444, 30, 60000)
INSERT [dbo].[Received_Goods] ([GoodID], [No], [Quantity], [Price]) VALUES (6, 2222, 30, 60000)
INSERT [dbo].[Received_Goods] ([GoodID], [No], [Quantity], [Price]) VALUES (7, 1111, 30, 60000)
INSERT [dbo].[Received_Goods] ([GoodID], [No], [Quantity], [Price]) VALUES (4, 1111, 30, 60000)


INSERT [dbo].[Customer] ([Name], [Account], [Password]) VALUES ('Huy', 'abc', 'abc')
INSERT [dbo].[Customer] ([Name], [Account], [Password]) VALUES ('Huy', 'qwe', 'abc')
