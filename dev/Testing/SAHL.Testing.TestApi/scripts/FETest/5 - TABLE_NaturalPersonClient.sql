USE [FETest]
GO

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'NaturalPersonClient' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[NaturalPersonClient]
	end
go

/****** Object:  Table [dbo].[NaturalPersonClient]    Script Date: 16/01/2015 10:40:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[NaturalPersonClient](
	[Id] INT IDENTITY(1,1),
	[LegalEntityKey] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_dbo.NaturalPersonClient_IsActive]  DEFAULT ((0)),
	[IdNumber] [varchar](20) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


