USE [TransactionFeed]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 10/17/2019 5:19:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TransactionIdentificator] [varchar](200) NULL,
	[Amount] [numeric](18, 5) NULL,
	[CurrencyCode] [varchar](3) NULL,
	[TransactionDate] [datetime] NULL,
	[TransactionStatus] [int] NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [NCI_CURRENCY] ON [dbo].[Transaction]
(
	[CurrencyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [NCI_TRANSACTIONDATE] ON [dbo].[Transaction]
(
	[TransactionDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [NCI_TRANSACTIONSTATUS] ON [dbo].[Transaction]
(
	[TransactionStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [NCI_CURRENCY_DATE_STATUS] ON [dbo].[Transaction]
(
	[CurrencyCode] ASC,
	[TransactionDate] ASC,
	[TransactionStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO




SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [TransactionStatus]) VALUES (1, N'Inv00001', CAST(200.00000 AS Numeric(18, 5)), N'USD', CAST(N'2019-10-06T10:10:10.000' AS DateTime), N'4')
INSERT [dbo].[Transaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [TransactionStatus]) VALUES (2, N'Inv00002', CAST(10000.05000 AS Numeric(18, 5)), N'EUR', CAST(N'2019-10-10T20:00:00.000' AS DateTime), N'2')
INSERT [dbo].[Transaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [TransactionStatus]) VALUES (3, N'Invoice0000001', CAST(1000.00000 AS Numeric(18, 5)), N'USD', CAST(N'2019-10-11T00:00:00.000' AS DateTime), N'1')
INSERT [dbo].[Transaction] ([ID], [TransactionIdentificator], [Amount], [CurrencyCode], [TransactionDate], [TransactionStatus]) VALUES (4, N'Invoice0000002', CAST(300.00000 AS Numeric(18, 5)), N'USD', CAST(N'2019-10-12T00:00:00.000' AS DateTime), N'2')
SET IDENTITY_INSERT [dbo].[Transaction] OFF
