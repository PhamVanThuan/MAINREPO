USE [2AM]
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'InvoiceStatus' AND TABLE_SCHEMA = 'dbo')
BEGIN
	CREATE TABLE [dbo].[InvoiceStatus]
	(
		InvoiceStatusKey INT NOT NULL PRIMARY KEY,
		Description VARCHAR(20) NOT NULL
	)
END
GO

GRANT SELECT ON [dbo].[InvoiceStatus] TO [AppRole] AS [dbo]
GO

GRANT SELECT ON [dbo].[DomainProcess] TO [ServiceArchitect] AS [dbo]
GO

IF NOT EXISTS (SELECT 1 FROM [2AM].dbo.[InvoiceStatus] WHERE [Description] = 'Received')
BEGIN 
	INSERT INTO [2AM].dbo.InvoiceStatus (InvoiceStatusKey, [Description]) VALUES (1, 'Received')
END