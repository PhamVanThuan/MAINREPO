USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertValuationRecord') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	drop procedure test.InsertValuationRecord
	print 'dropped proc test.InsertValuationRecord'
End

go
create procedure test.InsertValuationRecord
	   @ValuatorKey int,
	   @ValuationDate datetime,
	   @ValuationAmount float,
	   @ValuationHOCValue float,
	   @ValuationMunicipal float,
	   @ValuationUserID varchar(max),
	   @PropertyKey int,
	   @HOCThatchAmount float,
	   @HOCConventionalAmount float,
	   @HOCShingleAmount float,
	   @ChangeDate datetime,
	   @ValuationClassificationKey int,
	   @ValuationEscalationPercentage float,
	   @ValuationStatusKey int,
	   @Data xml,
	   @ValuationDataProviderDataServiceKey int,
	   @IsActive bit,
	   @HOCRoofKey int
as

begin
	INSERT INTO [2AM].[dbo].[Valuation]
				  (
					[ValuatorKey]
				   ,[ValuationDate]
				   ,[ValuationAmount]
				   ,[ValuationHOCValue]
				   ,[ValuationMunicipal]
				   ,[ValuationUserID]
				   ,[PropertyKey]
				   ,[HOCThatchAmount]
				   ,[HOCConventionalAmount]
				   ,[HOCShingleAmount]
				   ,[ChangeDate]
				   ,[ValuationClassificationKey]
				   ,[ValuationEscalationPercentage]
				   ,[ValuationStatusKey]
				   ,[Data]
				   ,[ValuationDataProviderDataServiceKey]
				   ,[IsActive]
				   ,[HOCRoofKey]
			   )
	VALUES
			   (
				   @ValuatorKey,
				   @ValuationDate ,
				   @ValuationAmount ,
				   @ValuationHOCValue,
				   @ValuationMunicipal ,
				   @ValuationUserID ,
				   @PropertyKey,
				   @HOCThatchAmount,
				   @HOCConventionalAmount ,
				   @HOCShingleAmount,
				   @ChangeDate ,
				   @ValuationClassificationKey,
				   @ValuationEscalationPercentage,
				   @ValuationStatusKey ,
				   @Data ,
				   @ValuationDataProviderDataServiceKey ,
				   @IsActive ,
				   @HOCRoofKey
			   )
	DECLARE @ValuationKey INT
	SET @ValuationKey =  SCOPE_IDENTITY()
	SELECT * FROM [2AM].[dbo].[Valuation]
	WHERE ValuationKey =@ValuationKey
end
