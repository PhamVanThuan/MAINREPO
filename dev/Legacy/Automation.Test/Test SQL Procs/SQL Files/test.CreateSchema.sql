USE [2AM]
GO

IF SCHEMA_ID('test') IS NULL
	BEGIN
		EXECUTE('CREATE SCHEMA test') 
	END

USE [X2]
GO

IF SCHEMA_ID('test') IS NULL
	BEGIN
		EXECUTE('CREATE SCHEMA test') 
	END
	
USE [e-Work]
GO

IF SCHEMA_ID('test') IS NULL
	BEGIN
		EXECUTE('CREATE SCHEMA test') 
	END


