USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertProperty') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	drop procedure test.InsertProperty
	print 'dropped proc test.InsertProperty'
End

go
create procedure test.InsertProperty
	@PropertyTypeKey int,
	@TitleTypeKey int,
	@AreaClassificationKey int,
	@OccupancyTypeKey int,
	@AddressKey int,
	@PropertyDescription1 varchar(250),
	@PropertyDescription2 varchar(250),
	@PropertyDescription3 varchar(250),
	@DeedsOfficeValue float,
	@CurrentBondDate datetime,
	@ErfNumber varchar(25),
	@ErfPortionNumber varchar(25),
	@SectionalSchemeName varchar(50),
	@SectionalUnitNumber varchar(25),
	@DeedsPropertyTypeKey int,
	@ErfSuburbDescription varchar(50),
	@ErfMetroDescription varchar(50),
	@DataProviderKey int
as

begin
	INSERT INTO [2am].[dbo].[Property]
           ([PropertyTypeKey]
           ,[TitleTypeKey]
           ,[AreaClassificationKey]
           ,[OccupancyTypeKey]
           ,[AddressKey]
           ,[PropertyDescription1]
           ,[PropertyDescription2]
           ,[PropertyDescription3]
           ,[DeedsOfficeValue]
           ,[CurrentBondDate]
           ,[ErfNumber]
           ,[ErfPortionNumber]
           ,[SectionalSchemeName]
           ,[SectionalUnitNumber]
           ,[DeedsPropertyTypeKey]
           ,[ErfSuburbDescription]
           ,[ErfMetroDescription]
           ,[DataProviderKey])
     VALUES
           (@PropertyTypeKey,
			@TitleTypeKey ,
			@AreaClassificationKey ,
			@OccupancyTypeKey ,
			@AddressKey ,
			@PropertyDescription1 ,
			@PropertyDescription2 ,
			@PropertyDescription3 ,
			@DeedsOfficeValue,
			@CurrentBondDate,
			@ErfNumber,
			@ErfPortionNumber,
			@SectionalSchemeName,
			@SectionalUnitNumber,
			@DeedsPropertyTypeKey,
			@ErfSuburbDescription,
			@ErfMetroDescription,
			@DataProviderKey 
			)
		           
	DECLARE @PropertyKey INT
	SET @PropertyKey =  SCOPE_IDENTITY()
	
	select
		property.*,
		[2am].dbo.fGetFormattedAddressDelimited (property.addresskey,0) as formattedPropertyAddress,
		propertytype.Description as PropertyTypeDescription,
		titletype.Description as TitleTypeDescription,
		areaclassification.Description as AreaClassificationDescription,
		OccupancyType.Description as OccupancyTypeDescription,
		DeedsPropertyType.Description as DeedsPropertyTypeDescription,
		PropertyTitleDeed.TitleDeedNumber,
		hocroof.description as HOCRoofDescription,
		propertydata.data.value('(/NewDataSet/Table1/BondAccountNumber/text())[1]', 'nvarchar(max)' ) as BondAccountNumber,
		(
			select description from deedsoffice 
			where deedsofficekey = propertydata.data.value('(/NewDataSet/Table1/DeedsOfficeKey/text())[1]', 'int' ) 
	     ) as DeedsOfficeName,
	    valuation.*,
	    ValuationCombinedThatch.*,
	    ValuationCottage.*,
	    ValuationImprovement.*,
	    ValuationMainBuilding.*,
	    ValuationOutbuilding.*,
	     valuator.*,
	     legalentity.*
	from dbo.property
		inner join dbo.propertytype
			on property.propertytypekey = propertytype.propertytypekey
		inner join dbo.titletype 
			on  property.titletypekey = titletype.titletypekey
		inner join dbo.areaclassification
			on property.areaclassificationkey= areaclassification.areaclassificationkey
		inner join dbo.OccupancyType
			on property.OccupancyTypeKey= OccupancyType.OccupancyTypeKey
		inner join dbo.DeedsPropertyType
			on property.DeedsPropertyTypeKey = DeedsPropertyType.DeedsPropertyTypeKey
		left join dbo.PropertyTitleDeed
			on property.propertykey = PropertyTitleDeed.PropertyKey
		left join dbo.propertydata
			on property.propertykey = propertydata.propertykey
			and propertydata.propertydataproviderdataservicekey = 3
		left join dbo.valuation
			on property.propertykey = valuation.propertykey
		left join dbo.hocroof
			on valuation.hocroofkey = hocroof.hocroofkey
		left join dbo.valuator
			on valuation.valuatorkey = valuator.valuatorkey
		left join dbo.legalentity
			on valuator.legalentitykey = legalentity.legalentitykey
		left join (select valuationkey as CombinedThatchValuationKey,ValuationCombinedThatch.* 
					from dbo.ValuationCombinedThatch
				) as ValuationCombinedThatch
					on valuation.valuationkey = ValuationCombinedThatch.valuationkey
		left join (select valuationkey as ValuationCottageValuationKey, ValuationCottage.* 
					from  dbo.ValuationCottage) as ValuationCottage
						on valuation.valuationkey = ValuationCottage.valuationkey
		left join (select valuationkey as ValuationImprovementValuationKey, ValuationImprovement.* 
					from  dbo.ValuationImprovement) as ValuationImprovement
						on valuation.valuationkey = ValuationImprovement.valuationkey
	    left join (select valuationkey as ValuationMainBuildingValuationKey, ValuationMainBuilding.* 
					from  dbo.ValuationMainBuilding) as ValuationMainBuilding
						on valuation.valuationkey = ValuationMainBuilding.valuationkey
		left join (select valuationkey as ValuationOutbuildingValuationKey, ValuationOutbuilding.* 
					from  dbo.ValuationOutbuilding) as ValuationOutbuilding
						on valuation.valuationkey = ValuationOutbuilding.valuationkey
	where property.propertykey = @PropertyKey
end
