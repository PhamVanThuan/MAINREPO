USE [FETest]
GO

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'EmptyThirdPartyInvoices' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].EmptyThirdPartyInvoices
	end
go

/****** Object:  Table [dbo].[CompositeDefinitions]    Script Date: 21/01/2015 09:03:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].EmptyThirdPartyInvoices(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ThirdPartyInvoiceKey] [int] NOT NULL
 CONSTRAINT [PK_EmptyThirdPartyInvoices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


