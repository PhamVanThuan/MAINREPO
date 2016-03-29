
USE [FeTest]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateClientSearch')
	DROP PROCEDURE dbo.PopulateClientSearch
GO

CREATE PROCEDURE dbo.PopulateClientSearch

AS

IF OBJECT_ID (N'dbo.ClientSearch', N'U') IS NOT NULL
BEGIN
	
	INSERT INTO dbo.ClientSearch
	SELECT top 2000
		le.IdNumber,
		[2AM].dbo.LegalEntityLegalName(le.legalentitykey,0), 
		le.EmailAddress,
		MultipleRoles.HasMutlipleRoles
	FROM [2AM].dbo.LegalEntity le 
		JOIN (
			SELECT
				legalentitykey,
				1 as HasMutlipleRoles 
			FROM [2am].dbo.OfferRole ofr
			WHERE ofr.GeneralStatusKey = 1 AND ofr.OfferRoleTypeKey in (11, 12, 13)
			GROUP BY legalentitykey
			HAVING COUNT(ofr.legalentitykey) > 1
			UNION
			SELECT
				legalentitykey,
				0 as HasMutlipleRoles 
			FROM [2am].dbo.OfferRole ofr
			WHERE ofr.GeneralStatusKey = 1 AND ofr.OfferRoleTypeKey in (11, 12, 13)
			GROUP BY legalentitykey
			HAVING COUNT(ofr.legalentitykey) = 1
		) As MultipleRoles ON MultipleRoles.LegalEntityKey = le.LegalEntityKey
	WHERE le.legalentitytypekey = 2 AND LEN(le.EmailAddress) > 1
END

