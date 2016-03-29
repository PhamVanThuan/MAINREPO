USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.CleanUpCreditScoringData') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.[CleanUpCreditScoringData]
	Print 'Dropped Proc test.[CleanUpCreditScoringData]'
End
Go

/****** Object:  StoredProcedure [test].[CleanUpOfferDebitOrder]  Script Date: 10/20/2010 07:23:42 ******/


CREATE PROCEDURE [test].[CleanUpCreditScoringData]

@testIdentifier varchar(255),
@offerKey int

AS

--set @testIdentifier='PrimMAAcceptSecSurDeclineHighLTV'	
--set @offerKey = 885304 
--set the ID number

if not exists(  
select 1 from [2am].dbo.OfferRole ofr   
join [2am].dbo.legalentity le   
on ofr.legalentitykey=le.legalentitykey and legalentitytypekey=2  
join OfferDeclaration od  
on ofr.offerrolekey=od.offerrolekey where ofr.offerkey=@OfferKey  
and ofr.offerroletypekey IN (8,10,12,11)   
)  
  
begin   
  
print 'Inserting Offer Declaration Records for: ' + cast(@OfferKey as varchar(10))  
  
declare @key int  
set @key=1;  
  
with declarations (key1,key2,key3) as   
(  
select 1, 1,2  
union  
select 1, 2,3  
union  
select 1, 3,2  
union  
select 1, 4,3  
union  
select 1, 5,2  
union  
select 1, 6,2  
union  
select 1, 7,1  
)  
  
insert into [2am].dbo.OfferDeclaration  
select offerrolekey, d.key2,d.key3,NULL   
from [2am].dbo.OfferRole ofr  
join declarations d on @key=d.key1  
join [2am].dbo.legalentity le on ofr.legalentitykey=le.legalentitykey and legalentitytypekey=2  
where ofr.offerkey=@OfferKey  
and offerroletypekey in (8,10,12,11)   
order by 1  
  
end  

--WE NEED A PROPERTY TO ASSOCIATE IT TO THE OFFER
--update the offerMortgageLoan table 
if (select PropertyKey from [2am].dbo.OfferMortgageLoan 
where offerKey=@OfferKey) is null
begin

	declare @propertyKey int;

with allProperties as (
	select   
	top 50 o.offerstatuskey, p.*
	from [2am].dbo.property p
	join [2am].dbo.offermortgageLoan oml on p.propertyKey=oml.propertyKey
	join [2am].dbo.offer o on oml.offerKey=o.offerKey 
	join [2am].dbo.valuation v on p.propertyKey=v.propertyKey and isActive=1
	left join [2am].fin.mortgageLoan ml on p.propertyKey=ml.propertyKey
	where ml.propertyKey is null
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
	and CurrentBondDate is not null)
	select	top 1 @propertyKey = 
propertykey
	from	allProperties
	where	offerstatuskey in (4,5)
	and propertykey not in (select PropertyKey from allProperties where offerstatuskey in (1,2,3))
	order by 1 desc

  update [2am].dbo.OfferMortgageLoan
  set PropertyKey = @propertyKey
  where offerKey=@OfferKey
end

--we need to allocate the correct legal entities as income contributors
--remove any existing records
delete from offerRoleAttribute where offerRoleKey in (
select offerRoleKey from offerRole where offerKey=@offerKey
and offerRoleTypeKey in (8,10,11,12)
)

--we need to add our income contributors

insert into [2am].dbo.OfferRoleAttribute
select ofr.offerRoleKey,1 from test.CreditScoringTestApplicants test
join legalEntity le on test.legalEntityID=le.IDNumber
join offerRole ofr on le.legalEntityKey=ofr.legalentitykey
where ofr.offerKey = @offerKey and test.incomeContributor=1
and testIdentifier=@testIdentifier

 --WE NEED AN ADDRESS FOR THE LEGAL ENTITY ADDRESS
declare @addressKey int

select top 1 @addressKey = a.addressKey from [2am].dbo.address a
where 
addressFormatKey=1
and StreetNumber is not null 
and StreetName is not null 
and SuburbKey is not null 
and RRR_CountryDescription is not null 
and RRR_ProvinceDescription is not null 
and RRR_CityDescription is not null 
and RRR_SuburbDescription is not null 
and RRR_PostalCode is not null 
order by 1 desc


-- Clean Up

delete lea
from [2am].dbo.offer o
join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey AND offerroletypekey IN (8,10,12,11)  
join [2am].dbo.legalEntityAddress lea on ofr.legalEntityKey=lea.legalentityKey
where  o.offerKey=@OfferKey 

delete 
from OfferMailingAddress 
where offerKey = @OfferKey

--WE NEED TO GIVE THE LEGAL ENTITY ADDRESS RECORDS
insert into [2am].dbo.legalEntityAddress (
LegalEntityKey,
AddressKey,
AddressTypeKey,
EffectiveDate,
GeneralStatusKey
)
select ofr.legalEntityKey, @addressKey,1,getdate(),1
from [2am].dbo.offer o
join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey AND offerroletypekey IN (8,10,12,11)  
left join [2am].dbo.legalEntityAddress lea on ofr.legalEntityKey=lea.legalentityKey
where lea.legalEntityKey is null and o.offerKey=@OfferKey
order by 1 desc

--Insert OfferMailingAddress
INSERT INTO OfferMailingAddress
( OfferKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey)

select top 1 o.offerKey,lea.Addresskey,1,1,1, ofr.legalEntityKey,1
from [2am].dbo.offer o
join [2am].dbo.offerRole ofr on o.offerKey=ofr.offerKey AND offerroletypekey IN (8,10,12,11)  
join [2am].dbo.legalEntityAddress lea on ofr.legalEntityKey=lea.legalentityKey
where o.offerKey=@OfferKey
order by 1 desc
      
--we need employment records

--we need to check the PTI first
declare @pti float
declare @income float
declare @instalment float

declare @employmenttypekey int
declare @householdincome float
declare @legalentitykey int
declare @offerCursorKey int

declare @CreditScoringIncome table (LegalEntityKey int, income int)
insert into @CreditScoringIncome
select le.legalentitykey, test.income 
from test.CreditScoringTestApplicants test
join legalEntity le on test.legalEntityID=le.idNumber
where  testIdentifier=@testIdentifier

declare employmentCursor cursor local
for 
select oivl.employmenttypekey, householdincome, orr.legalentitykey,o.offerkey from offer o 
join (SELECT        
o.offerkey as OfferKey, max(offerinformationkey) as OfferInformationKey 
FROM  [2am]..[Offer] O
JOIN [2am]..[OfferInformation] OI (NOLOCK) ON O.OfferKey = OI.OfferKey
WHERE   o.offerKey = @OfferKey
GROUP BY O.OfferKey
) as SelectedOffers on o.offerkey = SelectedOffers.OfferKey
join offerinformation oi
on SelectedOffers.OfferInformationKey = oi.OfferInformationKey 
join offerinformationvariableloan oivl on oi.offerinformationkey = oivl.offerinformationkey
join offerrole orr on o.offerkey = orr.offerkey and offerroletypekey in  (8,10,11,12)
left join employment e on orr.legalentitykey = e.legalentitykey
where e.employmentkey is null

open employmentCursor;
fetch next from employmentCursor into
@employmenttypekey, @householdincome, @legalentitykey,@offerCursorKey

while (@@fetch_status = 0)

begin

select @householdincome = Income from @CreditScoringIncome where LegalEntityKey=@legalentitykey

insert into employment (employerkey,employmenttypekey,remunerationtypekey,employmentstatuskey, legalentitykey, employmentstartdate,employmentenddate,
contactperson, contactphonenumber, contactphonecode,basicincome)
select 4, @employmenttypekey, case when @employmenttypekey = 1 then 2 else 11 end,1, @legalentitykey, 
dateadd(mm,-12,getdate()),NULL, 'Barney Stinson','7657182','031',@householdincome

fetch next from employmentCursor into
@employmenttypekey, @householdincome, @legalentitykey,@offerCursorKey

end

close employmentCursor;
deallocate employmentCursor;

GO


