USE FETest

go

IF(EXISTS(SELECT 1 FROM SYS.sysobjects where name = 'SPVs' and type = 'U'))
	BEGIN
		DROP TABLE [FETest].[dbo].[SPVs]
	end
go

CREATE TABLE [dbo].[SPVs](
	[SPVKey] [int] NOT NULL PRIMARY KEY,
	[Description] [varchar](70) NULL,
	[ReportDescription] [varchar](70) NULL,
	[ParentSPVKey] [int] NOT NULL,
	[ParentSPVDescription] [varchar](70) NULL,
	[ParentSPVReportDescription] [varchar](70) NULL,
	[SPVCompanyKey] [int] NOT NULL,
	[SPVCompanyDescription] [varchar](255) NOT NULL,
	[GeneralStatusKey] [int] NOT NULL,
	[BankAccountKey] [int] NOT NULL,
	[BankName] [varchar](50) NOT NULL,
	[BranchCode] [varchar](10) NOT NULL,
	[BranchName] [varchar](50) NOT NULL,
	[AccountName] [varchar](255) NULL,
	[AccountNumber] [varchar](25) NOT NULL,
	[AccountType] [varchar](50) NOT NULL,
) ON [PRIMARY]

go