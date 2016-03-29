USE Process
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE Name = 'solr')
	BEGIN
		EXEC ('CREATE SCHEMA solr')
	END
GO

USE [2AM]
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE Name = 'solr')
	BEGIN
		EXEC ('CREATE SCHEMA solr')
	END
GO