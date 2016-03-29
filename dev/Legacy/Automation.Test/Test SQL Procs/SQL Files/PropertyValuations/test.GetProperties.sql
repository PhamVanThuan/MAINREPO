USE [2AM]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetProperties]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetProperties
	Print 'Dropped procedure [test].[GetProperties]'
End
Go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [test].[GetProperties]
	@PropertyKey int = 0,
	@OfferKey int = 0,
	@AccountKey int = 0
AS
BEGIN

	if (@AccountKey > 0)
		begin
			select top 1  
					@PropertyKey = prop.PropertyKey
			from [2am].[dbo].Account acc (nolock)
			join [2am].[dbo].FinancialService fs (nolock) on acc.accountKey = fs.accountKey
			join [2am].[fin].MortgageLoan ml (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
			join [2am].[dbo].[Property] prop (nolock) on ml.PropertyKey = prop.PropertyKey
			join [2am].[dbo].[Address] a (nolock) on prop.AddressKey = a.AddressKey
			where acc.accountKey = @AccountKey
		end
	else if (@OfferKey > 0)
		begin
			select top 1  
					@PropertyKey = p.PropertyKey
            from [2am].dbo.offer o with (nolock)
            join [2am].dbo.offermortgageloan oml with (nolock) on o.offerkey=oml.offerkey
            join [2am].dbo.property p  with (nolock) on oml.propertykey=p.propertykey 
            where o.offerKey = @OfferKey
		end

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
		case when 
			propertydata.data.exist('(//BondAccountNumber)') = 1
		then 
			propertydata.data.value('(/NewDataSet/Table1/BondAccountNumber/text())[1]', 'nvarchar(max)' )
		else
			NULL
		end BondAccountNumber,
		case when 
			propertydata.data.exist('(//DeedsOfficeKey)') = 1
		then 
			(
				select description from deedsoffice 
				where deedsofficekey = propertydata.data.value('(/NewDataSet/Table1/DeedsOfficeKey/text())[1]', 'int' ) 
			) 
		else
			NULL
		end DeedsOfficeName,
	    valuation.*,
	    valuationCombinedThatch.*,
	    valuationCottage.*,
	    valuationImprovement.*,
	    valuationMainBuilding.*,
	    valuationOutbuilding.*,
	    valuator.*,
	    legalentity.*,
	    propertydata.propertyid
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

END




