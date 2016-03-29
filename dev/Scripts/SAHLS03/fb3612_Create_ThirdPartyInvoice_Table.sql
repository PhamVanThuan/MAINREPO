USE [2AM]
GO
If Not Exists(Select * From INFORMATION_SCHEMA.TABLES Where TABLE_NAME = 'ThirdPartyInvoice' And TABLE_SCHEMA = 'dbo')

BEGIN

BEGIN TRANSACTION

	CREATE TABLE [dbo].[ThirdPartyInvoice](
		[ThirdPartyInvoiceKey] [int] IDENTITY(1,1) NOT NULL,
		[SahlReference] VARCHAR(30) NOT NULL,
		[InvoiceStatusKey] [int] NOT NULL DEFAULT ((1)),
		[AccountKey] [int] NOT NULL,
		[ThirdPartyId] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_ThirdPartyInvoice] PRIMARY KEY CLUSTERED 
	(
		[ThirdPartyInvoiceKey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ThirdPartyInvoice]  WITH CHECK ADD FOREIGN KEY([AccountKey])
	REFERENCES [dbo].[Account] ([AccountKey])
	

	ALTER TABLE [dbo].[ThirdPartyInvoice]  WITH CHECK ADD FOREIGN KEY([InvoiceStatusKey])
	REFERENCES [dbo].[InvoiceStatus] ([InvoiceStatusKey])
	

	ALTER TABLE [dbo].[ThirdPartyInvoice]  WITH CHECK ADD  CONSTRAINT [FK_ThirdPartyInvoice_ThirdParty] FOREIGN KEY([ThirdPartyId])
	REFERENCES [dbo].[ThirdParty] ([Id])
	

	ALTER TABLE [dbo].[ThirdPartyInvoice] CHECK CONSTRAINT [FK_ThirdPartyInvoice_ThirdParty]
COMMIT TRANSACTION
END 

GO

GRANT INSERT ON Object::[dbo].[ThirdPartyInvoice] TO [ServiceArchitect]

GRANT SELECT ON Object::[dbo].[ThirdPartyInvoice] TO [ServiceArchitect]

GRANT UPDATE ON Object::[dbo].[ThirdPartyInvoice] TO [ServiceArchitect]

GRANT DELETE ON Object::[dbo].[ThirdPartyInvoice] TO [ServiceArchitect]





