USE [2AM]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'InvoiceLineItemCategory'))
BEGIN
    /****** Object:  Table [dbo].[InvoiceLineItemCategory]    Script Date: 2015-03-26 11:27:14 AM ******/


	CREATE TABLE [dbo].[InvoiceLineItemCategory](
		[InvoiceLineItemCategoryKey] [int] NOT NULL,
		[InvoiceLineItemCategory] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_InvoiceLineItemCategory] PRIMARY KEY CLUSTERED 
	(
		[InvoiceLineItemCategoryKey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	GRANT INSERT ON Object::[dbo].[InvoiceLineItemCategory] TO [ServiceArchitect]

	GRANT SELECT ON Object::[dbo].[InvoiceLineItemCategory] TO [ServiceArchitect]

	GRANT UPDATE ON Object::[dbo].[InvoiceLineItemCategory] TO [ServiceArchitect]

	GRANT DELETE ON Object::[dbo].[InvoiceLineItemCategory] TO [ServiceArchitect]
END

GO

SET ANSI_PADDING OFF
GO
