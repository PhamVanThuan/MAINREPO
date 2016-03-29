USE [2AM]
GO

IF NOT EXISTS (SELECT 1 FROM SYS.COLUMNS 
WHERE NAME = 'AmountExcludingVAT' and object_id = object_id('dbo.ThirdPartyInvoice'))

BEGIN

	ALTER TABLE dbo.ThirdPartyInvoice ADD
		AmountExcludingVAT decimal(22, 10) NULL
END

IF NOT EXISTS (SELECT 1 FROM SYS.COLUMNS 
WHERE NAME = 'VATAmount' and object_id = object_id('dbo.ThirdPartyInvoice'))

BEGIN

	ALTER TABLE dbo.ThirdPartyInvoice ADD
	    VATAmount decimal(22, 10) NULL
END


IF NOT EXISTS (SELECT 1 FROM SYS.COLUMNS 
WHERE NAME = 'TotalAmountIncludingVAT' and object_id = object_id('dbo.ThirdPartyInvoice'))

BEGIN

	ALTER TABLE dbo.ThirdPartyInvoice ADD
		TotalAmountIncludingVAT decimal(22, 10) NULL
END