USE [FETest]
GO
IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'HaloUsers' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].HaloUsers
	end

GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].HaloUsers(
	[ADUserKey] [int] NOT NULL,
	[ADUserName] [varchar](50) NOT NULL,
	[LegalEntityKey] [varchar](50) NOT NULL,
	[UserOrganisationStructureKey] [int] NOT NULL,
	[OrgStructureDescription] [varchar](max) NOT NULL,
	[Capabilities] [varchar](250) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


