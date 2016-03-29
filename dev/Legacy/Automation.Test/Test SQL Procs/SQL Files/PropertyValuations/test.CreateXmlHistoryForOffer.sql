USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'test.CreateXmlHistoryForOffer') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE test.CreateXmlHistoryForOffer
	PRINT 'dropped proc test.CreateXmlHistoryForOffer'
END
GO

CREATE PROCEDURE test.CreateXmlHistoryForOffer
	@OfferKey int
AS
BEGIN
	DECLARE
		@Data xml,
		@PropertyKey int,
		@XmlHistoryKey varchar(max),
		@OfferTypeDesc varchar(max),
		@TitleTypeDescription Varchar(max),
		@AssessmentDate varchar(max),
		@LightstoneId varchar(max),
		@streetNumber varchar(max),
		@streetName  varchar(max),
		@suburb  varchar(max),
		@city  varchar(max),
		@province  varchar(max),
		@country  varchar(max),
		@postalCode varchar(max),
		@erfNumber varchar(max),
		@erfMetroDescription varchar(max),
		@propertyDescription1 varchar(max),
		@propertyDescription2 varchar(max),
		@propertyDescription3 varchar(max),
		@PreviousUniqueId varchar(max),
		@UniqueId varchar(max),
		@ValuationUserID varchar(max)

	INSERT INTO [2AM].[dbo].[XMLHistory] ([GenericKeyTypeKey],[GenericKey],[XMLData],[InsertDate]) 
	VALUES (2 , @OfferKey, '<RESERVED/>',getdate())
	
	SET @PropertyKey = (SELECT TOP 01 PropertyKey FROM dbo.OfferMortgageLoan WHERE OfferKey =@OfferKey )
	SET @XmlHistoryKey =  convert(int,SCOPE_IDENTITY())
	SET @OfferTypeDesc = (SELECT ot.Description FROM dbo.Offer o JOIN dbo.OfferType ot ON o.OfferTypeKey=ot.OfferTypeKey WHERE OfferKey = @OfferKey)
	SET @TitleTypeDescription = (SELECT tt.Description FROM dbo.Property p JOIN dbo.TitleType tt ON p.TitleTypeKey=tt.TitleTypeKey WHERE PropertyKey = @PropertyKey)
	SELECT @LightstoneId = propertyid FROM dbo.propertydata WHERE PropertyKey = @PropertyKey
	SET @PreviousUniqueId = '' 
	 	
	IF NOT EXISTS(SELECT TOP 01 * FROM dbo.[PropertyAccessDetails] WHERE PropertyKey = @PropertyKey)
	BEGIN 
		INSERT INTO [2AM].[dbo].[PropertyAccessDetails]([PropertyKey],[Contact1],[Contact1Phone],[Contact1WorkPhone],[Contact1MobilePhone] ,[Contact2],[Contact2Phone])
		VALUES(@PropertyKey,'contact1','phone1','workphone','mobilePhone','contact2','phone2')
	END
		
		SELECT @streetNumber =a.StreetNumber,
			   @streetName  =a.StreetName,
			   @suburb  = a.RRR_SuburbDescription,
			   @city = a.RRR_CityDescription,
			   @province  = a.RRR_ProvinceDescription,
			   @country = a.RRR_CountryDescription,
			   @postalCode = a.RRR_PostalCode,
			   @erfNumber = p.ErfNumber,
			   @erfMetroDescription = p.ErfMetroDescription,
			   @propertyDescription1=  p.PropertyDescription1,
			   @propertyDescription2 =  p.PropertyDescription2,
			   @propertyDescription3 =  p.PropertyDescription3
		FROM dbo.property p
			join address a
				on a.addresskey=p.addresskey
		WHERE p.PropertyKey = @PropertyKey
		
	SET @AssessmentDate = CONVERT(varchar(max),getdate(),103)+ +' '+  CONVERT(VARCHAR(5), getdate(), 108) +' ' + SUBSTRING(CONVERT(VARCHAR(19),  getdate(), 100),18,2)
	SET @ValuationUserID=N'SAHL\VPUser'
	
	--Use the latest completed valuation (not amended) as the previous unique id. 
	SELECT TOP 01 @UniqueId=data.value('(/SAHL.SubmitCompletedValuationLightstone/Request/PHYSICALVALUATION/INSTRUCTIONDETAILS/UniqueID)[1]','varchar(max)')
	FROM dbo.valuation
	WHERE propertykey = @PropertyKey and data is not null
	ORDER BY valuationkey desc
		
	IF (@UniqueId is not null and @UniqueId <> 0)
	BEGIN
		SET @PreviousUniqueId = '<PreviousUniqueID>'+@UniqueId+'</PreviousUniqueID>'
	END
		
	IF (@LightstoneId is null OR @LightstoneId = 0)
	 BEGIN
		SELECT @Data = '<Lightstone.newPhysicalInstruction url="http://preprod.lightstone.co.za/avm/webservices/sahl.asmx">
								<Request>
									<LightstoneNonValidatedProperty>
										<PropertyDetails>
											<UniqueID>'+@XmlHistoryKey+'</UniqueID>
											'+@PreviousUniqueId+'
											<SAHLUser>'+@ValuationUserID+'</SAHLUser>
											<PropertyTitleType>'+@TitleTypeDescription+'</PropertyTitleType>
											<ValuationReason>special instruct reason</ValuationReason>
											<ValuationRequiredDate>'+@AssessmentDate+'</ValuationRequiredDate>
											<ValuationInstructions>Offer:'+@OfferTypeDesc+'</ValuationInstructions>
											<Contact1>contact1</Contact1>
											<Contact1Phone>phone1</Contact1Phone>
											<Contact1WorkPhone>workphone</Contact1WorkPhone>
											<Contact1MobilePhone>mobilePhone</Contact1MobilePhone>
											<Contact2>contact2</Contact2>
											<Contact2Phone>phone2</Contact2Phone>
											<StreetNumber>'+@streetNumber+'</StreetNumber>
											<StreetName>'+@streetName+'</StreetName>
											<Suburb>'+@suburb+'</Suburb>
											<City>'+@city+'</City>
											<Province>'+@province+'</Province>
											<Country>'+@country+'</Country>
											<PostalCode>'+@postalCode+'</PostalCode>
											<ErfNumber>'+@erfNumber+'</ErfNumber>
											<ErfMetroDescription>'+@erfMetroDescription+'</ErfMetroDescription>
											<PropertyDescription1>'+@propertyDescription1+'</PropertyDescription1>
											<PropertyDescription2>'+@propertyDescription2+'</PropertyDescription2>
											<PropertyDescription3>'+@propertyDescription3+'</PropertyDescription3>
										</PropertyDetails>
									</LightstoneNonValidatedProperty>
								</Request>
							 </Lightstone.newPhysicalInstruction>'
		 END
		ELSE
		 BEGIN
			SELECT @Data = '<Lightstone.newPhysicalInstruction url="http://preprod.lightstone.co.za/avm/webservices/sahl.asmx">
								<Request>
									<LightstoneValidatedProperty>
									  <PropertyDetails>
										<UniqueID>'+@XmlHistoryKey+'</UniqueID>
										'+@PreviousUniqueId+'
										<SAHLUser>'+@ValuationUserID+'</SAHLUser>
										<LightstonePropertyID>'+@LightstoneId+'</LightstonePropertyID>
										<PropertyTitleType></PropertyTitleType>
										<ValuationReason>special instruct reason</ValuationReason>
										<ValuationRequiredDate>'+@AssessmentDate+'</ValuationRequiredDate>
										<ValuationInstructions>Offer:'+@OfferTypeDesc+'</ValuationInstructions>
										<Contact1>contact1</Contact1>
										<Contact1Phone>phone1</Contact1Phone>
										<Contact1WorkPhone>workphone</Contact1WorkPhone>
										<Contact1MobilePhone>mobilePhone</Contact1MobilePhone>
										<Contact2>contact2</Contact2>
										<Contact2Phone>phone2</Contact2Phone>
									  </PropertyDetails>
									</LightstoneValidatedProperty>
								</Request>
							 </Lightstone.newPhysicalInstruction>'
		 END
	UPDATE [2AM].[dbo].[XMLHistory] 
	SET XMLData = @Data
	WHERE XmlHistoryKey = @XmlHistoryKey
END
go