USE [FETest]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'OpenMortgageLoanAccounts')
	DROP TABLE dbo.[OpenMortgageLoanAccounts]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OpenMortgageLoanAccounts](
    [Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountKey] [int] NOT NULL,
	[ProductKey] [int] NOT NULL,
	[HasThirdPartyInvoice] [bit] NOT NULL CONSTRAINT [DF_ActiveNewBusinessAccounts_HasThirdPartyInvoice]  DEFAULT ((0))
) ON [PRIMARY]

GO


