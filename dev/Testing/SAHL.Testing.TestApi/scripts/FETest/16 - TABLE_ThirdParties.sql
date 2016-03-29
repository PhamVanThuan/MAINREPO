USE FETest

go

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'ThirdParties' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[ThirdParties]
	end
go

CREATE TABLE [dbo].[ThirdParties](
    [Id] [uniqueidentifier] NOT NULL,
	[ThirdPartyKey] [int] NOT NULL PRIMARY KEY,
	[LegalEntityKey] [int] NOT NULL,
	[TradingName] [varchar](50) NULL,
	[Contact] [varchar](50) NULL,
	[GeneralStatusKey] [int] NOT NULL,
	[GenericKey] [int] NULL,
	[GenericKeyTypeKey] [int] NULL,
	[GenericKeyTypeDescription] [varchar](50) NULL,
	[HasBankAccount] [bit] NOT NULL,
	[PaymentEmailAddress] [varchar](100) NULL,
	[BankAccountKey] [int] NULL,
	[BankName] [varchar](50) NULL,
	[BranchCode] [varchar](10) NULL,
	[BranchName] [varchar](50) NULL,
	[AccountName] [varchar](255) NULL,
	[AccountNumber] [varchar](25) NULL,
	[AccountType] [varchar](50) NULL,
) ON [PRIMARY]

go