USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertLegalEntityAssetLiability******/
IF  EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'test.InsertLegalEntityAssetLiability') 
		AND type in (N'P', N'PC'))
	begin
		DROP PROCEDURE test.InsertLegalEntityAssetLiability
	end
USE [2AM]
GO

/****** Object:  StoredProcedure test.InsertLegalEntityAssetLiability******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE test.InsertLegalEntityAssetLiability
	@LegalEntityKey int,
	@GeneralStatusKey int,
	@AssetLiabilityTypeKey int,
	@AssetLiabilitySubTypeKey int,
	@AddressKey int,
	@AssetValue float,
	@LiabilityValue float,
	@CompanyName varchar(max),
	@Cost float,
	@Date datetime,
	@Description varchar(max),
	@AssetLiabilityKey int output
AS
BEGIN
	set @AssetLiabilitySubTypeKey = (select top 01
					case when @AssetLiabilitySubTypeKey = 0 then null
					else @AssetLiabilitySubTypeKey end)
	set @AddressKey = (select top 01
							case when @AddressKey = 0 then null 
							else @AddressKey end)
	INSERT INTO [2AM].[dbo].[AssetLiability] 
	(
		AssetLiabilityTypeKey,
		AssetLiabilitySubTypeKey,
		AddressKey,
		AssetValue,
		LiabilityValue,
		CompanyName,
		Cost,
		Date,
		Description
	)
	VALUES
	(
		@AssetLiabilityTypeKey,
		@AssetLiabilitySubTypeKey,
		@AddressKey,
		@AssetValue,
		@LiabilityValue,
		@CompanyName,
		@Cost,
		@Date,
		@Description
	)
	
	SET @AssetLiabilityKey = SCOPE_IDENTITY()
	
	INSERT INTO [2AM].[dbo].[LegalEntityAssetLiability] 
	(
		[LegalEntityKey],
		[AssetLiabilityKey],
		[GeneralStatusKey]
	)
	VALUES
	(
		@LegalEntityKey,
		@AssetLiabilityKey,
		@GeneralStatusKey
	)
END