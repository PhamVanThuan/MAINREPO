USE [2AM]
GO

IF COL_LENGTH('[2AM].[dbo].[ThirdPartyInvoice]', 'ReceivedFromEmailAddress') IS NULL

BEGIN

ALTER TABLE [2AM].[dbo].[ThirdPartyInvoice]
ADD ReceivedFromEmailAddress VARCHAR(100) NULL

END

GO
