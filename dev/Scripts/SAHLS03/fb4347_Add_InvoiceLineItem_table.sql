USE [2AM]
GO

/****** Object:  Table [dbo].[AttorneyInvoiceLineItem]    Script Date: 2015-03-26 01:38:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'InvoiceLineItem'))
BEGIN

CREATE TABLE [dbo].[InvoiceLineItem](
	[InvoiceLineItemKey] [int] IDENTITY(1,1) NOT NULL,
	[ThirdPartyInvoiceKey] [int] NOT NULL,
	[InvoiceLineItemDescriptionKey] [int] NOT NULL,
	[Amount] [decimal](22, 10) NOT NULL,
	[IsVATItem] [bit] NOT NULL,
	[VATAmount] [decimal](22, 10) NULL,
	[TotalAmountIncludingVAT] [decimal](22, 10) NOT NULL,
 CONSTRAINT [PK_InvoiceLineItem] PRIMARY KEY CLUSTERED 
(
	[InvoiceLineItemKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[InvoiceLineItem]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLineItem_InvoiceLineItemDescription] FOREIGN KEY([InvoiceLineItemDescriptionKey])
REFERENCES [dbo].[InvoiceLineItemDescription] ([InvoiceLineItemDescriptionKey])

ALTER TABLE [dbo].[InvoiceLineItem] CHECK CONSTRAINT [FK_InvoiceLineItem_InvoiceLineItemDescription]

ALTER TABLE [dbo].[InvoiceLineItem]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLineItem_ThirdPartyInvoice] FOREIGN KEY([ThirdPartyInvoiceKey])
REFERENCES [dbo].[ThirdPartyInvoice] ([ThirdPartyInvoiceKey])

ALTER TABLE [dbo].[InvoiceLineItem] CHECK CONSTRAINT [FK_InvoiceLineItem_ThirdPartyInvoice]
	
END
GO


GRANT INSERT ON Object::[dbo].[InvoiceLineItem] TO [ServiceArchitect]

GRANT SELECT ON Object::[dbo].[InvoiceLineItem] TO [ServiceArchitect]

GRANT UPDATE ON Object::[dbo].[InvoiceLineItem] TO [ServiceArchitect]

GRANT DELETE ON Object::[dbo].[InvoiceLineItem] TO [ServiceArchitect]
GO