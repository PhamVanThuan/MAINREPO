USE [2AM]

GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateInvoiceLineItemDescription')
	BEGIN
		DROP PROCEDURE dbo.PopulateInvoiceLineItemDescription
	END
GO

CREATE PROCEDURE dbo.PopulateInvoiceLineItemDescription

@CategoryKey int,
@InvoiceLineItemDescription nvarchar(50)

AS

BEGIN

IF NOT EXISTS (SELECT 1 FROM [dbo].[InvoiceLineItemDescription] WHERE InvoiceLineItemCategoryKey = @CategoryKey and InvoiceLineItemDescription = @InvoiceLineItemDescription)

	BEGIN

		INSERT INTO [dbo].[InvoiceLineItemDescription]
				   ([InvoiceLineItemCategoryKey]
				   ,[InvoiceLineItemDescription])
			 VALUES
				   (@CategoryKey,
				   @InvoiceLineItemDescription)
	END

END

GO

DECLARE @LineItemCategoryKey INT
DECLARE @LineItemDescription nvarchar(50)

SET @LineItemCategoryKey = 1
SET @LineItemDescription = 'Advertising Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Correspondent Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Counsel Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Courier Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Other Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Sheriff Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Travel Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemCategoryKey = 2
SET @LineItemDescription = 'Additional Defendant'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Commission'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'DC - Correspondence Fee'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'DC - Miscellaneous Fee 1'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'DC - Miscellaneous Fee 2'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'DC - Miscellaneous Fee 3'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Rescission of Order'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'DC - Stage 1'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'DC - Stage 2'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'DC - Stage 3'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'DC - Variation of Order'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Deceased Estates - Opposed'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Deceased Estates - Unopposed'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Defended Miscellaneous'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Default Judgement Miscellaneous'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Evictions - Opposed'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Evictions - Unopposed'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Insolvency - Opposed'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Insolvency - Unopposed'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Instruct Attorney'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Judgment Granted'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Legal Perfect'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'LOD'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'LOD Miscellaneous'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Notice of Motion'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Notice of Motion Miscellaneous'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Rescission of Judgement'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Rule 31(1)'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Rule 46(1)'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'S129 or S86(10)'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Sale in Execution'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Sales Date Set'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Sales Date Set Miscellaneous'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Summons'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Summons Miscellaneous'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Table 7 Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Table 8 Fees'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Third Party Interdicts'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Warrant of Execution and Attachment'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Withdrawal of Action'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

SET @LineItemDescription = 'Writ Miscellaneous'

EXEC dbo.PopulateInvoiceLineItemDescription @LineItemCategoryKey, @LineItemDescription

GO

DROP PROCEDURE dbo.PopulateInvoiceLineItemDescription



