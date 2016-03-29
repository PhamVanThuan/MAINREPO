USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertEmploymentRecords******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'test.InsertEmploymentRecords') AND type in (N'P', N'PC'))
DROP PROCEDURE test.InsertEmploymentRecords
go

USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertEmploymentRecords******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE test.InsertEmploymentRecords

@offerKey int

AS

declare @pti float
declare @income float
declare @instalment float
declare @offertype int
declare @employmenttypekey int
declare @householdincome float
declare @legalentitykey int
declare @offerCursorKey int
declare @maxOfferInformationKey int
declare @tempTable table (employmenttypekey int, householdincome float, legalentitykey int, offerkey int)
declare @applicantCount int


set @offertype = (select top 01 offertypekey from [2am].dbo.offer where offerkey = @offerKey)
set @maxOfferInformationKey = (select max(offerInformationKey) from [2am].dbo.offerInformation where offerKey= @OfferKey)
set @applicantCount = (select count(offerRoleKey) from [2am].dbo.OfferRole where offerKey = @offerKey and offerroletypekey in (8,10,11,12))

if (@offertype = 11)
begin
	insert into @tempTable
	select 1,100000,legalentitykey,@offerKey from [2am].dbo.externalrole
	where generickey = @offerKey
end
else 
begin
	select @pti = pti, @income = householdincome, @instalment = monthlyInstalment 
	from [2am].dbo.offerInformation oi
	join [2am].dbo.offerInformationVariableLoan oivl on oi.offerInformationKey=oivl.offerInformationKey
	where oi.offerKey=@OfferKey
	and oi.offerInformationKey = @maxOfferInformationKey

	if @pti > 0.34
		begin
		--we need to get a better household income
		set @income =  @instalment/0.25
		update [2am].dbo.offerInformationVariableLoan
		set  householdincome = @income, pti = 0.25
		where offerInformationKey = @maxOfferInformationKey
	end
	
	insert into @tempTable
	select oivl.employmenttypekey, householdincome/@applicantCount, orr.legalentitykey, o.offerkey 
	from [2am].dbo.offer o 
	join [2am].dbo.offerinformation oi on oi.OfferInformationKey = @maxOfferInformationKey
	join [2am].dbo.offerinformationvariableloan oivl on oi.offerinformationkey = oivl.offerinformationkey
	join [2am].dbo.offerrole orr on o.offerkey = orr.offerkey 
		and offerroletypekey in  (8,10,11,12)
	left join [2am].dbo.employment e on orr.legalentitykey = e.legalentitykey
	where e.employmentkey is null and o.offerKey = @offerKey
end

declare employmentCursor cursor local
for 
select * from @tempTable

open employmentCursor;
fetch next from employmentCursor into
@employmenttypekey, @householdincome, @legalentitykey,@offerCursorKey

while (@@fetch_status = 0)

begin

insert into [2am].dbo.employment 
( 
employerkey,
employmenttypekey,
remunerationtypekey,
employmentstatuskey,
legalentitykey,
employmentstartdate,
employmentenddate,
contactperson,
contactphonenumber,
contactphonecode,
basicincome,
salaryPaymentDay,
UnionMember
)
select 
4, 
@employmenttypekey, 
case @employmenttypekey 
when 1 then 2
when 2 then 11
when 3 then 2
when 4 then 1
when 5 then 1 end,1, 
@legalentitykey, 
dateadd(mm,-12,getdate()),
NULL, 
'Barney Stinson',
'7657182',
'031',
@householdincome,
26,
1

fetch next from employmentCursor into
@employmenttypekey, @householdincome, @legalentitykey,@offerCursorKey

end

close employmentCursor;
deallocate employmentCursor;

if @offerType <> 11
begin
	update [2am].dbo.Employment
	set
	ConfirmedIncomeFlag = 1, 
	ConfirmedEmploymentFlag=1,
	EmploymentConfirmationSourceKey = 2,
	ConfirmedBasicIncome = BasicIncome, 
	ConfirmedCommission = Commission,
	Department = 'HR',
	confirmedby = 'SAHL\TestUser',
	confirmeddate = getdate()
	where employmentkey in (
	select e.employmentKey 
	from [2am].dbo.offerRole o
	join [2am].dbo.employment e on o.legalentitykey=e.legalentitykey 
		and e.employmentStatusKey=1
	where o.offerkey=@OfferKey
	)
end

if @offerType = 11
begin
	update [2am].dbo.Employment
	set
	ConfirmedIncomeFlag = 1, 
	ConfirmedEmploymentFlag=1,
	EmploymentConfirmationSourceKey = 2,
	ConfirmedBasicIncome = BasicIncome, 
	ConfirmedCommission = Commission,
	Department = 'HR',
	confirmedby = 'SAHL\TestUser',
	confirmeddate = getdate()
	where employmentkey in (
	select e.employmentKey 
	from [2am].dbo.externalRole er
	join [2am].dbo.employment e on er.legalentitykey=e.legalentitykey 
		and e.employmentStatusKey=1
	where er.genericKey=@OfferKey and er.genericKeyTypeKey = 2
	)
end

