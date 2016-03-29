USE FETest

go

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'ForeignNaturalPersonClients' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[ForeignNaturalPersonClients]
	end

GO

CREATE TABLE [dbo].[ForeignNaturalPersonClients](
	[Id] INT IDENTITY(1,1),
	[LegalEntityKey] [int] NOT NULL,
	[CitizenshipTypeKey] [int] NOT NULL,
	[PassportNumber] [varchar](50) NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


