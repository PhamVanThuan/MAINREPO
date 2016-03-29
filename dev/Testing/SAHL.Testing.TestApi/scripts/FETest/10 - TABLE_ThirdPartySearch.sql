USE [FETest]
GO

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'ThirdPartySearch' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[ThirdPartySearch]
	end

GO

CREATE TABLE [dbo].[ThirdPartySearch](
	[Id] INT IDENTITY(1,1),
	[LegalName] [VARCHAR](MAX) NOT NULL,
	[Email] [VARCHAR](MAX) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO