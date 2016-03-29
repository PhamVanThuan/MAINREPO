USE [2AM]
GO
/****** Object:  StoredProcedure [test].[GetCasesInWorklistByStateAndTypeAndLTV]    Script Date: 10/06/2010 12:51:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[CleanupNewBusinessOffer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[CleanupNewBusinessOffer]
go

USE [2AM]
GO
/****** Object:  StoredProcedure [test].[CleanupNewBusinessOffer]    Script Date: 10/07/2010 11:55:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [test].[CleanupNewBusinessOffer]
		@offerKey int

AS
begin
--we need to give legal entities without ID numbers a valid ID number
declare @lekeys table (ipk int identity(1,1),legalEntityKey int)
declare @offerRoleKey int
declare @IDNumber varchar(20)
declare @addresskey int
declare @legalentityaddresskey int
declare @legalEntityKey int
declare @x int
declare @y int
set @x=1

insert into @lekeys
select le.legalentitykey
FROM   [2AM].dbo.offer o   
       JOIN [2AM].dbo.offerrole ofr   
         ON o.offerkey = ofr.offerkey   
       JOIN [2AM].dbo.legalentity le   
         ON ofr.legalentitykey = le.legalentitykey   
WHERE   
o.offerkey = @offerKey 
AND offerroletypekey IN (8,10,12,11)  
AND legalentitytypekey in (2)  
AND (IDNumber IS NULL OR DateOfBirth IS NULL)

set @y = (select max(ipk) from @lekeys)

while (@x <= @y)
	begin
		select top 1 @IDNumber = id.IDNumber from test.IDNumbers id
		left join [2AM].dbo.legalEntity le on id.idNumber=le.idNumber
		where le.legalEntityKey is null
		
		select @legalEntityKey = legalEntityKey from @lekeys where ipk = @x
		
		update [2AM].dbo.legalEntity
		set idNumber = @IDNumber,
		dateOfBirth = '19'+substring(@IDNumber,0,3)+'-'+substring(@IDNumber,3,2)+'-'+substring(@IDNumber,5,2)+' 00:00:00.000'
		where legalEntityKey = @legalEntityKey

	set @x = @x + 1
end

--clean up legal entity data
exec test.CleanUpLegalEntityData @offerKey

--Offer Declarations
delete od
from [2AM].dbo.offerdeclaration od
join [2AM].dbo.offerrole ofr on od.offerrolekey=ofr.offerrolekey
where ofr.offerkey=@offerKey 

print 'Inserting Offer Declaration Records for: ' + cast(@offerKey as varchar(10))

exec test.insertdeclarations @offerKey,33,17 

--WE NEED A PROPERTY TO ASSOCIATE IT TO THE OFFER
--update the offerMortgageLoan table 
if (select PropertyKey from [2am].dbo.OfferMortgageLoan 
where offerKey=@OfferKey) is null
begin

	declare @propertyKey int;

	with allProperties as 
	(
		select   
			top 50 o.offerstatuskey, p.*
		from [2am].dbo.property p
		join [2am].dbo.offermortgageLoan oml on p.propertyKey=oml.propertyKey
		join [2am].dbo.offer o on oml.offerKey=o.offerKey 
		join [2am].dbo.valuation v on p.propertyKey=v.propertyKey and isActive=1
		left join [2am].fin.mortgageLoan mlp on p.propertyKey=mlp.propertyKey
		where mlp.propertyKey is null
		and v.valuationdate < dateadd(mm, -12, getdate())
		and propertyDescription1 is not null
		and propertyDescription2 is not null
		and propertyDescription3 is not null
		and erfnumber is not null
		and erfportionnumber is not null
		and deedsPropertyTypeKey is not null
		and erfSuburbDescription is not null
		and erfMetroDescription is not null
		and deedsOfficeValue is not null
		and CurrentBondDate is not null
		order by newid()
	)
	
	select	top 1 @propertyKey = propertykey
	from	allProperties
	where	offerstatuskey in (4,5)
	and propertykey not in (select PropertyKey from allProperties where offerstatuskey in (1,2,3))
	order by 1 desc
   update [2am].dbo.OfferMortgageLoan
   set PropertyKey = @propertyKey
   where offerKey=@OfferKey
end


declare @existingPropertyKey int
select @existingPropertyKey = PropertyKey from [2am].dbo.OfferMortgageLoan where offerKey=@OfferKey

--delete and insert new property access details
delete from dbo.PropertyAccessDetails where propertyKey = @existingPropertyKey
--insert
insert into dbo.PropertyAccessDetails(PropertyKey, Contact1, Contact1Phone)
values
(@existingPropertyKey, 'Clinton Speed', '0315713036')

--Remove Old offerRoleAttribute
delete ora
from [2am].dbo.offerRole ofr  
join [2am].dbo.offerRoleAttribute ora on ofr.offerRoleKey=ora.offerRoleKey and offerRoleAttributeTypeKey=1  
where ofr.offerKey=@OfferKey  

--Update Income Contributor
insert into [2am].dbo.offerRoleAttribute(OfferRoleKey,OfferRoleAttributeTypeKey)
select ofr.offerRoleKey,1
from [2am].dbo.offerRole ofr  
left join [2am].dbo.offerRoleAttribute ora on ofr.offerRoleKey=ora.offerRoleKey and offerRoleAttributeTypeKey=1  
where ofr.offerKey=@OfferKey  
and ofr.offerRoleTypeKey in (8, 10, 11, 12) and ora.offerRoleAttributeKey is null

end
