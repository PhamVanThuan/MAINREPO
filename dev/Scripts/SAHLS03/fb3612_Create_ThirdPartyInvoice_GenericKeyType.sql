use [2am]
go

IF NOT EXISTS (SELECT 1 FROM [2AM].[dbo].[GenericKeyType] WHERE Description = 'ThirdPartyInvoice')

BEGIN

	INSERT INTO [2AM].dbo.GenericKeyType
	(GenericKeyTypeKey, Description, TableName, PrimaryKeyColumn)
	VALUES
	(54, 'ThirdPartyInvoice', '[2AM].dbo.ThirdPartyInvoice', 'ThirdPartyInvoiceKey')

END

