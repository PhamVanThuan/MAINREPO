USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'test.InsertValuationForOffer') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE test.InsertValuationForOffer
	PRINT 'dropped proc test.InsertValuationForOffer'
END
GO

CREATE PROCEDURE test.InsertValuationForOffer
	@OfferKey int,
	@IsEzVal bit,
	@ValuationKey int out
AS
BEGIN
	DECLARE 
		@ValuationDate datetime,
		@ValuationAmount float,
		@ValuationHOCValue float,
		@ValuationMunicipal float,
		@ValuationUserID nvarchar(4000),
		@HOCThatchAmount float,
		@HOCConventionalAmount float,
		@HOCShingleAmount float,
		@ValuationEscalationPercentage float,
		@Data xml,
		@IsActive bit,
		@PropertyKey int,
		@ValuatorKey int,
		@ValuationClassificationKey int,
		@ValuationStatusKey int,
		@HOCRoofKey int,
		@ValuationDataProviderDataServiceKey int,
		@Extent float,
		@Rate float,
		@ValuationRoofTypeKey int,
		@MortgageLoanPurposeKey int,
		@EmploymentTypeKey int,
		@TotalLoanAmount float,
		@LTV float,
		@OriginationSourceKey int,
		@ProductKey int,
		@UnspecifiedMaxIncome int,
		@Income float


	SELECT 
		@ValuationAmount=oml.ClientEstimatePropertyValuation,
		@PropertyKey=oml.PropertyKey 
	FROM [2am].dbo.OfferMortgageLoan oml
	WHERE oml.OfferKey = @OfferKey	
		
	SELECT 
		@ValuationDate=getdate(),
		@ValuationHOCValue=@ValuationAmount*1.2,
		@ValuationMunicipal=@ValuationAmount,
		@ValuationUserID=N'SAHL\VPUser',
		@HOCThatchAmount=0,
		@HOCConventionalAmount=@ValuationAmount*1.2,
		@HOCShingleAmount=0,
		@ValuationEscalationPercentage=20,
		@IsActive=1,
		@Data = NULL,
		@ValuatorKey=2,
		@ValuationClassificationKey=1,
		@ValuationStatusKey=2,
		@HOCRoofKey=2,
		@ValuationDataProviderDataServiceKey=1	
		
	--------------------------------------------------------------------------MANUAL VALUATIONS------------------------------------------------------------------------------------------
	IF (@IsEzVal = 0)
	 BEGIN
		UPDATE v
		SET IsActive = 0 
		FROM OfferMortgageloan oml Inner Join Valuation v ON oml.PropertyKey = v.PropertyKey WHERE oml.offerkey =@OfferKey
	 END
	--------------------------------------------------------------------------EZVAL--------------------------------------------------------------------------------------------
	ELSE
	 BEGIN		
		SELECT
			@ValuationHOCValue=0,
			@ValuationMunicipal=0,
			@HOCThatchAmount=0,
			@HOCConventionalAmount=0,
			@HOCShingleAmount=0,
			@ValuationEscalationPercentage=20,
			@ValuatorKey=ValuatorKey, -- ValCo1
			@ValuationStatusKey=1, -- Pending
			@IsActive=0,
			@ValuationDataProviderDataServiceKey = 7,
			@ValuationUserID=N'SAHL\VPUser'		
		from [2am]..Valuator 			
		where ValuatorContact = 'ValCo1 '
	 END
		
	DECLARE @OutTable table(ValuationKey int)

	INSERT INTO dbo.Valuation (
		ValuationDate, 
		ValuationAmount, 
		ValuationHOCValue, 
		ValuationMunicipal, 
		ValuationUserID, 
		HOCThatchAmount, 
		HOCConventionalAmount, 
		HOCShingleAmount, 	
		ValuationEscalationPercentage, 
		Data, 
		IsActive, 
		PropertyKey, 
		ValuatorKey, 
		ValuationClassificationKey, 
		ValuationStatusKey, 
		HOCRoofKey, 
		ValuationDataProviderDataServiceKey) 
	OUTPUT INSERTED.ValuationKey
		INTO @OutTable
	VALUES (
		@ValuationDate, 
		@ValuationAmount, 
		@ValuationHOCValue, 
		@ValuationMunicipal, 
		@ValuationUserID, 
		@HOCThatchAmount, 
		@HOCConventionalAmount,  
		@HOCShingleAmount,
		@ValuationEscalationPercentage, 
		@Data, 
		@IsActive, 
		@PropertyKey, 
		@ValuatorKey, 
		@ValuationClassificationKey, 
		@ValuationStatusKey, 
		@HOCRoofKey,
		@ValuationDataProviderDataServiceKey)

	SELECT @Extent=Round(@ValuationAmount/9000,2),
		@Rate=9000,
		@ValuationRoofTypeKey=1,
		@ValuationKey=ot.ValuationKey
	FROM @OutTable ot
		
	SELECT * FROM @OutTable
	
	INSERT INTO dbo.ValuationMainBuilding (
		Extent, 
		Rate, 
		ValuationRoofTypeKey, 
		ValuationKey) 
	VALUES (
		@Extent, 
		@Rate, 
		@ValuationRoofTypeKey, 
		@ValuationKey)
END
go