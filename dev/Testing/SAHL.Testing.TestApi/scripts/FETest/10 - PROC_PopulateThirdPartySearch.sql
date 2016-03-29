
USE [FeTest]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateThirdPartySearch')
	DROP PROCEDURE dbo.PopulateThirdPartySearch
GO

CREATE PROCEDURE dbo.PopulateThirdPartySearch

AS

IF OBJECT_ID (N'dbo.ThirdPartySearch', N'U') IS NOT NULL
BEGIN

	TRUNCATE TABLE dbo.ThirdPartySearch
	
	INSERT INTO dbo.ThirdPartySearch
	SELECT DISTINCT TOP 10 
		[2AM].test.LegalEntityLegalName(tp.legalentitykey,1), 
		le.EmailAddress
	FROM [2AM].dbo.ThirdParty tp
		JOIN [2AM].dbo.LegalEntity le ON le.LegalEntityKey=tp.LegalEntityKey
END

