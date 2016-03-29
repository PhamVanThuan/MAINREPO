USE [2AM]
GO

IF COL_LENGTH('dbo.ThirdPartyInvoice', 'ThirdPartyId') IS NULL

BEGIN

        ALTER TABLE dbo.ThirdPartyInvoice
        ADD [ThirdPartyId] UNIQUEIDENTIFIER NULL
		 
		
		ALTER TABLE [dbo].[ThirdPartyInvoice]  WITH CHECK ADD  CONSTRAINT [FK_ThirdPartyInvoice_ThirdParty] FOREIGN KEY([ThirdPartyId])
		REFERENCES [dbo].[ThirdParty] ([Id])
		
END
GO 