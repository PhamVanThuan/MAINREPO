USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertEmploymentRecords******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'test.InsertOfferAssetLiability') AND type in (N'P', N'PC'))
DROP PROCEDURE test.InsertOfferAssetLiability
go

USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertOfferMailingAddress******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE test.InsertOfferAssetLiability

@offerKey int

AS

BEGIN

DECLARE @assetLiabilityKey INT
DECLARE @legalEntityKey INT
DECLARE @LAA FLOAT

SELECT @legalEntityKey = legalEntityKey FROM [2am].dbo.OfferRole
WHERE offerKey = @offerKey AND offerRoleTypeKey IN (8,10,11,12) AND generalStatusKey=1

SELECT @LAA = oivl.loanagreementamount FROM 
[2am].dbo.offerinformationvariableloan oivl
WHERE offerInformationKey = (SELECT max(offerInformationKey) FROM [2am].dbo.offerInformation oi WHERE oi.offerKey = @offerKey) 


IF (@LAA > 1500000)
	BEGIN
		INSERT INTO [2am].dbo.AssetLiability 
		(assetLiabilityTypeKey, AssetValue, CompanyName)
		VALUES
		(2, 1500000, 'Google')
		SET @assetLiabilityKey = scope_identity()
		INSERT INTO [2am].dbo.LegalEntityAssetLiability
		(legalEntityKey, AssetLiabilityKey, GeneralStatusKey)
		VALUES
		(@legalEntityKey, @assetLiabilityKey, 1)
	END
END
