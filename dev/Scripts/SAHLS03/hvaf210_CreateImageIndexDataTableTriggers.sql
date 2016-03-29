USE [ImageIndex];

IF EXISTS (SELECT '1' FROM sys.objects 
WHERE object_id = OBJECT_ID(N'dbo.ThirdPartyInvoiceDocumentInsertTrigger'))
BEGIN
	DROP Trigger dbo.ThirdPartyInvoiceDocumentInsertTrigger
END
Go

CREATE TRIGGER dbo.ThirdPartyInvoiceDocumentInsertTrigger
	ON  ImageIndex.dbo.Data 
	AFTER INSERT
AS 
BEGIN	
	SET NOCOUNT ON;

	Insert into [2AM].[datastor].[ThirdPartyInvoicesSTOR] (AccountKey, ThirdPartyInvoiceKey, EmailSubject, FromEmailAddress, InvoiceFileName, Category, DateReceived, DateProcessed, STORKey, DocumentGuid) 
	Select i.Key1, i.Key2, i.Key3, i.Key4, i.Key5, i.Key6, i.Key7, i.Key8, i.STOR, i.GUID
	From Inserted i  
	Where  i.STOR = 44
END
Go

IF EXISTS (SELECT '1' FROM sys.objects 
WHERE object_id = OBJECT_ID(N'dbo.ThirdPartyInvoiceDocumentUpdateTrigger'))
BEGIN
	DROP Trigger dbo.ThirdPartyInvoiceDocumentUpdateTrigger
END
Go

CREATE TRIGGER dbo.ThirdPartyInvoiceDocumentUpdateTrigger
	ON  ImageIndex.dbo.Data 
	AFTER UPDATE
AS 
BEGIN	
	SET NOCOUNT ON;

	declare @updatedStor int;

	SELECT @updatedStor = i.STOR from deleted i

	if (@updatedStor = 44)
	BEGIN
		UPDATE [2AM].[datastor].[ThirdPartyInvoicesSTOR]
		SET  AccountKey = i.Key1,
			ThirdPartyInvoiceKey = i.Key2, 
			EmailSubject = i.Key3, 
			FromEmailAddress = i.Key4, 
			InvoiceFileName = i.Key5, 
			Category = i.Key6, 
			DateReceived = i.Key7, 
			DateProcessed = i.Key8, 
			STORKey = i.STOR, 
			DocumentGuid = i.GUID
		From Inserted i   
		WHERE [2AM].[datastor].[ThirdPartyInvoicesSTOR].ThirdPartyInvoiceKey = i.Key2
	END
END
Go

IF EXISTS (SELECT '1' FROM sys.objects 
WHERE object_id = OBJECT_ID(N'dbo.ThirdPartyInvoiceDocumentDeleteTrigger'))
BEGIN
	DROP Trigger dbo.ThirdPartyInvoiceDocumentDeleteTrigger
END
Go

CREATE TRIGGER dbo.ThirdPartyInvoiceDocumentDeleteTrigger
	ON  ImageIndex.dbo.Data 
	AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [2AM].[datastor].[ThirdPartyInvoicesSTOR]	
	WHERE [2AM].[datastor].[ThirdPartyInvoicesSTOR].ThirdPartyInvoiceKey IN (select deleted.Key2 from deleted Where deleted.STOR = 44)
	
END
Go
