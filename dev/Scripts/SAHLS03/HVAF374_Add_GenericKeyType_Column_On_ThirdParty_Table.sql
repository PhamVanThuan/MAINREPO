
USE [2AM]
GO

IF NOT EXISTS (SELECT 1 FROM SYS.COLUMNS WHERE NAME = 'GenericKeyTypeKey' and object_id = object_id('dbo.ThirdParty'))
BEGIN
	Alter Table dbo.ThirdParty Add GenericKeyTypeKey int null
END
GO