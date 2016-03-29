USE FETest

go

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'ClientAddresses' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[ClientAddresses]
	end
go

CREATE TABLE [dbo].[ClientAddresses](
    [Id] [int] IDENTITY(1,1) NOT NULL,
	[LegalEntityAddressKey] [int] NOT NULL,
	[AddressKey] [int] NOT NULL,
	[AddressTypeKey] [int] NOT NULL,
	[AddressFormatKey] [int] NOT NULL,
	[LegalEntityKey] [int] NULL
) ON [PRIMARY]

go