USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetLegalEntityAssetsLiabilities]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetLegalEntityAssetsLiabilities]
	Print 'Dropped procedure [test].[GetLegalEntityAssetsLiabilities]'
End
Go

CREATE PROCEDURE [test].[GetLegalEntityAssetsLiabilities]
	@legalentitykey int,
	@assetliabilitykey int = 0
AS
BEGIN
	SELECT
		leasslib.*,
		asslib.*,
		asslibtype.Description as AssetLiabilityTypeDescription,
		asslibsubtype.Description as AssetLiabilitySubTypeDescription,
		a.addresskey
	FROM dbo.LegalEntityAssetLiability leasslib
		inner join dbo.AssetLiability asslib
			on leasslib.AssetLiabilityKey = asslib.AssetLiabilityKey
		inner join dbo.AssetLiabilityType asslibtype
			on asslib.AssetLiabilityTypeKey = asslibtype.AssetLiabilityTypeKey
		left join dbo.AssetLiabilitySubType asslibsubtype
			on asslib.AssetLiabilitySubTypeKey = asslibsubtype.AssetLiabilitySubTypeKey
		left join dbo.address a
			on asslib.addresskey=a.addresskey
	WHERE leasslib.legalentitykey =@legalentitykey or leasslib.AssetLiabilityKey =@assetliabilitykey
END