USE [2AM]
GO

IF COL_LENGTH('[2AM].[dbo].[ThirdPartyInvoice]', 'CapitaliseInvoice') IS NULL

BEGIN

ALTER TABLE [2AM].[dbo].[ThirdPartyInvoice]
ADD CapitaliseInvoice BIT NULL

END

GO
