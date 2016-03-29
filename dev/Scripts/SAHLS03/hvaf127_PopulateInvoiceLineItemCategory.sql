USE [2AM]
GO

declare @CategoryDescription varchar(50)
declare @InvoiceLineItemCategoryKey int
set @CategoryDescription = 'Disbursements'
set @InvoiceLineItemCategoryKey = 1

IF NOT EXISTS (SELECT 1 FROM dbo.[InvoiceLineItemCategory] WHERE InvoiceLineItemCategory = @CategoryDescription and InvoiceLineItemCategoryKey = @InvoiceLineItemCategoryKey)		
	BEGIN
	INSERT INTO [2AM].[dbo].[InvoiceLineItemCategory]
           ([InvoiceLineItemCategoryKey], [InvoiceLineItemCategory])
     VALUES
          ( 1,  @CategoryDescription)
	END

set @CategoryDescription = 'Legal Fees'
set @InvoiceLineItemCategoryKey = 2

IF NOT EXISTS (SELECT 1 FROM dbo.[InvoiceLineItemCategory] WHERE InvoiceLineItemCategory = @CategoryDescription and InvoiceLineItemCategoryKey = @InvoiceLineItemCategoryKey)		
	BEGIN
	INSERT INTO [2AM].[dbo].[InvoiceLineItemCategory]
           ([InvoiceLineItemCategoryKey], [InvoiceLineItemCategory])
     VALUES
          (2, @CategoryDescription)
	END


