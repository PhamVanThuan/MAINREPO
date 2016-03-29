USE [2AM]
GO

IF COL_LENGTH('[2AM].[dbo].[ThirdPartyInvoice]', 'PaymentReference') IS NULL

BEGIN

	ALTER TABLE [2AM].[dbo].[ThirdPartyInvoice]
	ADD PaymentReference VARCHAR(30) NULL
END

GO
