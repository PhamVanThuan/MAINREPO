USE [FETest]
GO

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'ClientSearch' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[ClientSearch]
	end

GO

CREATE TABLE [dbo].[ClientSearch](
	[Id] INT IDENTITY(1,1),
	[IdNumber] [VARCHAR] (MAX) NOT NULL,
	[LegalName] [VARCHAR](MAX) NOT NULL,
	[Email] [VARCHAR](MAX) NOT NULL,
	[HasMultipleRoles] [BIT] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO