USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[CleanUpFLOfferData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[CleanUpFLOfferData]
	Print 'Dropped Proc [test].[CleanUpFLOfferData]'
End
Go

CREATE PROCEDURE [test].[CleanUpFLOfferData]

	@OfferKey INT

AS
BEGIN

--remove account subsidy records
delete from [2AM].dbo.accountsubsidy where subsidykey in
(
select s.subsidykey from [2AM].dbo.offerrole r
join [2AM].dbo.employment e
	on r.legalentitykey=e.legalentitykey
join [2AM].dbo.subsidy s
	on e.employmentkey=s.employmentkey
where r.offerkey=@OfferKey and r.offerroletypekey in (8,10,11,12)
)
--remove account subsidy records
delete from [2AM].dbo.OfferSubsidy where subsidykey in
(
select s.subsidykey from [2AM].dbo.offerrole r
join [2AM].dbo.employment e
	on r.legalentitykey=e.legalentitykey
join [2AM].dbo.subsidy s
	on e.employmentkey=s.employmentkey
where r.offerkey=@OfferKey and r.offerroletypekey in (8,10,11,12)
)
--remove subsidy records
delete from [2AM].dbo.subsidy where subsidykey in 
(
select s.subsidykey from [2AM].dbo.offerrole r
join [2AM].dbo.employment e
	on r.legalentitykey=e.legalentitykey
join [2AM].dbo.subsidy s
	on e.employmentkey=s.employmentkey
where r.offerkey=@OfferKey and r.offerroletypekey in (8,10,11,12)
)
--remove confirmation records first
delete from [2am].dbo.EmploymentVerificationProcess
where employmentkey in (
select e.employmentKey from [2am].[dbo].offerRole o
join [2am].[dbo].employment e on o.legalentitykey=e.legalentitykey 
where o.offerkey=@OfferKey and o.offerRoleTypeKey in (8,10,11,12) 
)
--remove existing employment records
delete from [2am].[dbo].employment where employmentkey in (
select e.employmentKey from [2am].[dbo].offerRole o
join [2am].[dbo].employment e on o.legalentitykey=e.legalentitykey 
where o.offerkey=@OfferKey and o.offerRoleTypeKey in (8,10,11,12) 
)
--get a count of applicants
declare @count int
select @count = count(*) from [2am].dbo.OfferRole o
where o.offerKey = @OfferKey and o.offerRoleTypeKey in (8,10,11,12) 

--we need employment records
declare @employmenttypekey int
declare @householdincome float
declare @legalentitykey int
declare @offerCursorKey int

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

insert into employment (employerkey,employmenttypekey,remunerationtypekey,employmentstatuskey, legalentitykey, employmentstartdate,employmentenddate,
contactperson, contactphonenumber, contactphonecode,basicincome)
select 4, @employmenttypekey, case when @employmenttypekey = 1 then 2 else 11 end,1, @legalentitykey, 
dateadd(mm,-12,getdate()),NULL, 'SAHL\TestUser','7657182','031',@householdincome/@count

fetch next from employmentCursor into
@employmenttypekey, @householdincome, @legalentitykey,@offerCursorKey

end

close employmentCursor;
deallocate employmentCursor;


PRINT 'Updating Employment Type for: ' + cast(@OfferKey as varchar(10))

UPDATE oivl 
SET    employmenttypekey = 1 
FROM   offer o 
       JOIN [2AM].dbo.offermortgageloan oml 
         ON o.offerkey = oml.offerkey 
       JOIN [2AM].dbo.offerinformation oi 
         ON o.offerkey = oi.offerkey 
       JOIN [2AM].dbo.offerinformationvariableloan oivl 
         ON oi.offerinformationkey = oivl.offerinformationkey 
WHERE  
o.offerkey = @OfferKey

PRINT 'Update employment records to be confirmed for: ' + cast(@OfferKey as varchar(10))
update [2am].dbo.Employment
set
ConfirmedIncomeFlag = 1, 
ConfirmedEmploymentFlag=1,
EmploymentConfirmationSourceKey = 2,
ConfirmedBasicIncome = BasicIncome, 
ConfirmedCommission = Commission,
Department = 'HR',
confirmedby = 'SAHL\FLAppProcUser3',
confirmeddate = getdate(),
SalaryPaymentDay = 26,
UnionMember = 0
where employmentkey in (
select e.employmentKey from [2am].dbo.offerRole o
join [2am].dbo.employment e on o.legalentitykey=e.legalentitykey 
	and e.employmentStatusKey=1
where o.offerkey=@OfferKey
)

--we need to do the verification type too for employment records that do not have it
INSERT INTO [2am].dbo.EmploymentVerificationProcess
(EmploymentKey, EmploymentVerificationProcessTypeKey, UserId, ChangeDate)
SELECT 
e.employmentKey, 1, 'SAHL\FLAppProcUser3', getdate()
FROM [2am]..OfferRole o with (nolock)
join employment e with (nolock) on o.legalentitykey=e.legalentitykey
and e.employmentstatuskey=1
left join [2am]..EmploymentVerificationProcess ver with (nolock) 
on e.employmentKey=ver.employmentKey
where o.offerKey=@OfferKey and ver.employmentKey is null


PRINT 'Updating Legal Entity Records for: ' + cast(@OfferKey as varchar(10))
--update missing le info 
UPDATE le 
SET    maritalstatuskey = 2, 
       populationgroupkey = 1,
       educationkey = 1 
FROM   [2AM].dbo.offer o 
       JOIN [2AM].dbo.offerrole ofr 
         ON o.offerkey = ofr.offerkey 
       JOIN [2AM].dbo.legalentity le 
         ON ofr.legalentitykey = le.legalentitykey 
WHERE 
o.offerkey = @OfferKey 
AND offerroletypekey IN (8,10,12,11)
AND legalentitytypekey in (2)

UPDATE le 
SET    citizentypekey=1
FROM   [2AM].dbo.offer o 
       JOIN [2AM].dbo.offerrole ofr 
         ON o.offerkey = ofr.offerkey 
       JOIN [2AM].dbo.legalentity le 
         ON ofr.legalentitykey = le.legalentitykey 
WHERE 
o.offerkey = @OfferKey 
AND offerroletypekey IN (8,10,12,11)
AND legalentitytypekey in (2) and citizentypekey is null

UPDATE le 
SET    salutationkey=1
FROM   [2AM].dbo.offer o 
       JOIN [2AM].dbo.offerrole ofr 
         ON o.offerkey = ofr.offerkey 
       JOIN [2AM].dbo.legalentity le 
         ON ofr.legalentitykey = le.legalentitykey 
WHERE 
o.offerkey = @OfferKey 
AND offerroletypekey IN (8,10,12,11)
AND legalentitytypekey in (2) and salutationkey is null

UPDATE le 
SET    genderkey=1
FROM   [2AM].dbo.offer o 
       JOIN [2AM].dbo.offerrole ofr 
         ON o.offerkey = ofr.offerkey 
       JOIN [2AM].dbo.legalentity le 
         ON ofr.legalentitykey = le.legalentitykey 
WHERE 
o.offerkey = @OfferKey 
AND offerroletypekey IN (8,10,12,11)
AND legalentitytypekey in (2) and genderkey is null

UPDATE le 
SET    initials='A'
FROM   [2AM].dbo.offer o 
       JOIN [2AM].dbo.offerrole ofr 
         ON o.offerkey = ofr.offerkey 
       JOIN [2AM].dbo.legalentity le 
         ON ofr.legalentitykey = le.legalentitykey 
WHERE 
o.offerkey = @OfferKey 
AND offerroletypekey IN (8,10,12,11)
AND legalentitytypekey in (2) and initials is null

PRINT 'Update to ensure at least one contact detail for: ' + cast(@OfferKey as varchar(10))
UPDATE le 
SET    CellPhonenumber = '0123456789', 
       EmailAddress = 'test@test.co.za' 
FROM   [2AM].dbo.offer o 
       JOIN [2AM].dbo.offerrole ofr 
         ON o.offerkey = ofr.offerkey 
       JOIN [2AM].dbo.legalentity le 
         ON ofr.legalentitykey = le.legalentitykey 
WHERE 
o.offerkey = @OfferKey 
AND offerroletypekey IN (8,10,12,11)
AND (le.CellPhonenumber is null or len(le.CellPhonenumber)=0) 
AND (le.EmailAddress is null or len(le.EmailAddress) = 0)

PRINT 'Update Trust/CC Details for: ' + cast(@OfferKey as varchar(10))
UPDATE le 
SET    TradingName=RegisteredName, 
       WorkPhoneCode='031',
	   WorkPhoneNumber='1235467'	 
FROM   [2AM].dbo.offer o 
       JOIN [2AM].dbo.offerrole ofr 
         ON o.offerkey = ofr.offerkey 
       JOIN [2AM].dbo.legalentity le 
         ON ofr.legalentitykey = le.legalentitykey 
WHERE 
o.offerkey = @OfferKey 
AND offerroletypekey IN (8,10,12,11)
AND legalentitytypekey in (3,4,5)
 
PRINT 'Updating Employment Records for: ' + cast(@OfferKey as varchar(10))
--update employment records 
UPDATE e 
SET    employmenttypekey = 1, 
       remunerationtypekey = 2, 
       employmentstartdate = Dateadd(mm,-2,Getdate()) 
FROM   [2AM].dbo.offer o 
       JOIN [2AM].dbo.offerrole ofr 
         ON o.offerkey = ofr.offerkey 
       JOIN [2AM].dbo.employment e 
         ON ofr.legalentitykey = e.legalentitykey 
            AND employmentstatuskey = 1 
WHERE  
o.offerkey = @OfferKey
AND offerroletypekey IN (8,10,12,11)

if not exists(
select 1 from OfferRole ofr 
join legalentity le 
on ofr.legalentitykey=le.legalentitykey and legalentitytypekey=2
join OfferDeclaration od
on ofr.offerrolekey=od.offerrolekey where ofr.offerkey=@OfferKey
and ofr.offerroletypekey in (11,12)
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
join legalentity le on ofr.legalentitykey=le.legalentitykey and legalentitytypekey=2
where ofr.offerkey=@OfferKey
and offerroletypekey in (11,12)
order by 1

end

--we need property inspection contact details to be provided
declare @PropertyKey int
select @PropertyKey=PropertyKey from [2am].dbo.OfferMortgageLoan where OfferKey=@OfferKey
	
if exists (select 1 from offermortgageloan oml
join property p on oml.propertykey=p.propertykey
join propertyAccessDetails pad on p.propertykey=pad.propertykey
where oml.offerkey=@OfferKey)

 begin
	print 'Cleaning up Property Inspection Data for: ' + cast(@OfferKey as varchar(10))
	update pad 
	set Contact1='Test Contact', Contact1Phone='012 3456789'
	from offermortgageloan oml
	join property p on oml.propertykey=p.propertykey
	join propertyAccessDetails pad on p.propertykey=pad.propertykey
	where oml.offerkey=@OfferKey
 end
else
 begin
	print 'Inserting Property Inspection Data for: ' + cast(@OfferKey as varchar(10))
	insert into PropertyAccessDetails (PropertyKey, Contact1, Contact1Phone)
	Values (@PropertyKey, 'Test Contact','012 3456789')
 end

--we need to cleanup property description, erf, suburb and metro information
--we need to cleanup property description, erf, suburb and metro information
if ((select isnull(propertydescription1,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set PropertyDescription1 = 'PropertyDescription1' where propertyKey = @propertyKey
	end
if ((select isnull(propertydescription2,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set PropertyDescription2 = 'PropertyDescription2' where propertyKey = @propertyKey
	end
if ((select isnull(propertydescription3,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set PropertyDescription3 = 'PropertyDescription3' where propertyKey = @propertyKey
	end
if ((select isnull(ErfMetroDescription,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set ErfMetroDescription = 'ErfMetroDescription' where propertyKey = @propertyKey
	end
if ((select isnull(ErfSuburbDescription,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set ErfSuburbDescription = 'ErfSuburbDescription' where propertyKey = @propertyKey
	end
if ((select isnull(ErfPortionNumber,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set ErfPortionNumber = 'ErfPortionNumber' where propertyKey = @propertyKey
	end
if ((select isnull(ErfNumber,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set ErfNumber = 'ErfNumber' where propertyKey = @propertyKey
	end
if ((select isnull(SectionalSchemeName,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set SectionalSchemeName = 'SectionalSchemeName' where propertyKey = @propertyKey
	end
if ((select isnull(SectionalUnitNumber,'') from [2am].dbo.Property where propertyKey = @propertyKey) = '')
	begin
		update [2am].dbo.Property set SectionalUnitNumber = '1' where propertyKey = @propertyKey
	end

--check the title type, if sectional title and account has HOC then we need to update
--to 'Sectional Title with HOC'

if (
select tt.description from offermortgageloan o
inner join [2am].dbo.property p on o.propertykey=p.propertykey
inner join [2am].dbo.titleType tt on p.titletypekey=tt.titletypekey
where o.offerkey=@OfferKey) = 'Sectional Title'
and exists (
	select o.accountkey , hoc.*
	from [2am].dbo.Offer o 
	inner join [2am].dbo.Account ar on o.accountkey=ar.parentAccountKey
		and ar.rrr_productkey=3
	inner join [2am].dbo.financialService fs on ar.accountkey=fs.accountkey
	inner join [2am].dbo.HOC on fs.financialservicekey=hoc.financialservicekey
	where offerkey=@OfferKey
)

begin 
	print 'Updating Title Type for Property on: ' + cast(@OfferKey as varchar(10))
	update p
	set TitleTypeKey=7
	from [2am].dbo.offerMortgageLoan o
	join [2am].dbo.Property p on o.propertykey=p.propertykey
	where o.offerkey=@OfferKey
end

--we need to remove the income contributor flag for LE's that dont have income
print 'Removing income contributor attributes for LEs without income for: ' + cast(@OfferKey as varchar(10))
delete from offerRoleAttribute where offerRoleKey in (
select ofr.offerRoleKey from offerRole ofr
where ofr.offerKey=@offerKey and offerRoleTypeKey in (8,10,11,12)
)

insert into offerRoleAttribute 
select offerRoleKey,1 from [2am].dbo.OfferRole ofr
join employment e on ofr.legalentityKey=e.legalentitykey and employmentstatuskey=1
where offerKey=@offerKey and offerRoleTypeKey in (8,10,11,12)



END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

