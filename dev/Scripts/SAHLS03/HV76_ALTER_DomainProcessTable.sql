USE [2AM]
GO

IF COL_LENGTH('[2AM].[dbo].[DomainProcess]', 'DataModel') IS NULL
BEGIN

	ALTER TABLE [2AM].[dbo].[DomainProcess]
	ADD DataModel NVARCHAR(MAX) NULL,
		DateCreated DATETIME NULL,
		DateModified DATETIME NULL

END
GO
