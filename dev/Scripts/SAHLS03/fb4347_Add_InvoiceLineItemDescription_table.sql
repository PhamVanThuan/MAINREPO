USE [2AM]
GO

/****** Object:  Table [dbo].[InvoiceLineItemDescription]    Script Date: 2015-03-30 03:11:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'InvoiceLineItemDescription'))
BEGIN
CREATE TABLE [dbo].[InvoiceLineItemDescription](
	[InvoiceLineItemDescriptionKey] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceLineItemCategoryKey] [int] NOT NULL,
	[InvoiceLineItemDescription] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_InvoiceLineItemDescription] PRIMARY KEY CLUSTERED 
(
	[InvoiceLineItemDescriptionKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[InvoiceLineItemDescription]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLineItemDescription_InvoiceLineItemCategory] FOREIGN KEY([InvoiceLineItemCategoryKey])
REFERENCES [dbo].[InvoiceLineItemCategory] ([InvoiceLineItemCategoryKey])
ALTER TABLE [dbo].[InvoiceLineItemDescription] CHECK CONSTRAINT [FK_InvoiceLineItemDescription_InvoiceLineItemCategory]

End
GO

GRANT INSERT ON Object::[dbo].[InvoiceLineItemDescription] TO [ServiceArchitect]

GRANT SELECT ON Object::[dbo].[InvoiceLineItemDescription] TO [ServiceArchitect]

GRANT UPDATE ON Object::[dbo].[InvoiceLineItemDescription] TO [ServiceArchitect]

GRANT DELETE ON Object::[dbo].[InvoiceLineItemDescription] TO [ServiceArchitect]
