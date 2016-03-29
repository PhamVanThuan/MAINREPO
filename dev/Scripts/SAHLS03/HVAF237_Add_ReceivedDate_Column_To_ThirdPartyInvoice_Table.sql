USE [2AM]
GO

IF COL_LENGTH('[2AM].[dbo].[ThirdPartyInvoice]', 'ReceivedDate') IS NULL

BEGIN

ALTER TABLE [2AM].[dbo].[ThirdPartyInvoice]
ADD ReceivedDate DATETIME NULL

END

GO
