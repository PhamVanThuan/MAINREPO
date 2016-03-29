Use [2am]
GO

IF NOT EXISTS (SELECT * FROM [2AM].dbo.[InvoiceStatus] WHERE [Description] = 'Received')

BEGIN
	INSERT INTO [2AM].dbo.InvoiceStatus (InvoiceStatusKey, [Description]) VALUES (1, 'Received')
END

IF NOT EXISTS (SELECT * FROM [2AM].dbo.[InvoiceStatus] WHERE Description = 'Awaiting Approval')

BEGIN 
	INSERT INTO [2AM].dbo.InvoiceStatus (InvoiceStatusKey, [Description]) VALUES (2, 'Awaiting Approval')
END

IF NOT EXISTS (SELECT * FROM [2AM].dbo.[InvoiceStatus] WHERE Description = 'Approved')

BEGIN 
	INSERT INTO [2AM].dbo.InvoiceStatus (InvoiceStatusKey, [Description]) VALUES (3, 'Approved')
END

IF NOT EXISTS (SELECT * FROM [2AM].dbo.[InvoiceStatus] WHERE Description = 'Processing Payment')

BEGIN 
	INSERT INTO [2AM].dbo.InvoiceStatus (InvoiceStatusKey, [Description]) VALUES (4, 'Processing Payment')
END

IF NOT EXISTS (SELECT * FROM [2AM].dbo.[InvoiceStatus] WHERE Description = 'Rejected')

BEGIN 
	INSERT INTO [2AM].dbo.InvoiceStatus (InvoiceStatusKey, [Description]) VALUES (5, 'Rejected')
END

IF NOT EXISTS (SELECT * FROM [2AM].dbo.[InvoiceStatus] WHERE Description = 'Paid')

BEGIN 
	INSERT INTO [2AM].dbo.InvoiceStatus (InvoiceStatusKey, [Description]) VALUES (6, 'Paid')
END

IF NOT EXISTS (SELECT * FROM [2AM].dbo.[InvoiceStatus] WHERE Description = 'Captured')

BEGIN 
	INSERT INTO [2AM].dbo.InvoiceStatus (InvoiceStatusKey, [Description]) VALUES (7, 'Captured')
END