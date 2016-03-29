USE [2AM]
GO
/****** Object:  StoredProcedure [test].[UpdateAddressDetailsWherePropertyValuation12MonthsOld]    Script Date: 10/06/2010 12:51:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[UpdateAddressDetailsWherePropertyValuation12MonthsOld]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[UpdateAddressDetailsWherePropertyValuation12MonthsOld]
go

USE [2AM]
GO
/****** Object:  StoredProcedure [test].[UpdateAddressDetailsWherePropertyValuation12MonthsOld]    Script Date: 10/07/2010 11:55:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [test].[UpdateAddressDetailsWherePropertyValuation12MonthsOld]
		@offerKey int,
		@marketValue int

AS
--WE NEED A PROPERTY TO ASSOCIATE IT TO THE OFFER
--update the offerMortgageLoan table 
if (select PropertyKey from [2am].dbo.OfferMortgageLoan 
where OfferKey = @offerKey) is null
begin

	declare @propertyKey int

	SELECT	top 1 @propertyKey = p.propertyKey 
		FROM	Address AS a INNER JOIN
					Property AS p ON a.AddressKey = p.AddressKey INNER JOIN
					Valuation AS v ON p.PropertyKey = v.PropertyKey --INNER JOIN
                    --PropertyAccessDetails AS pad ON p.PropertyKey = pad.PropertyKey
		WHERE	a.Streetnumber is not null and
					a.Streetnumber <> '' and
					a.Streetname is not null and
					a.Streetname <> '' and                    
                    p.propertyDescription1 is not null and 
					p.propertyDescription2 is not null and 
					p.propertyDescription3 is not null and 
					p.erfnumber is not null and 
					p.erfportionnumber is not null and 
					p.sectionalschemename is not null and 
					p.sectionalUnitNumber is not null and  
					p.deedsPropertyTypeKey is not null and 
					p.erfSuburbDescription is not null and 
					p.erfMetroDescription is not null and 
					p.deedsOfficeValue is not null and  
					p.CurrentBondDate is not null and 
					v.Isactive = 1 and
					v.ValuationStatusKey = 2 and
					DateDiff (month,v.ValuationDate,getdate()) = 12 and 
					v.ValuationAmount > @marketValue
					order by 1 desc   
					         
	--select * from valuationstatus
	
  update [2am].dbo.OfferMortgageLoan
  set PropertyKey = @propertyKey
  where OfferKey = @offerKey
  
end