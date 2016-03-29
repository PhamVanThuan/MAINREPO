USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetCreditScoringApplicants') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetCreditScoringApplicants
	Print 'Dropped Proc test.GetCreditScoringApplicants'
End
Go


CREATE PROCEDURE test.GetCreditScoringApplicants

@offerkey int

AS

declare @legalEntity table (legalEntityKey int, offerRoleTypeKey int, Income float, ranked int)
declare @PrimaryLegalEntityKey int
declare @SecondaryLegalEntityKey int

insert into @legalEntity
select le.legalEntityKey, ofr.offerRoleTypeKey, 
case when sum(confirmedincome)=0 or sum(confirmedincome) is null then sum(monthlyincome) else sum(confirmedincome) end as Income,
DENSE_RANK() over 
(partition by offerRoleTypeKey order by 
case when sum(confirmedincome)=0 or sum(confirmedincome) is null 
then sum(monthlyincome) 
else sum(confirmedincome) 
end desc)
from offerRole ofr
join legalEntity le on ofr.legalEntityKey=le.legalEntityKey
join offerRoleAttribute ora on ofr.offerRoleKey=ora.offerRoleKey and offerRoleAttributeTypeKey=1
join employment e on le.legalEntityKey=e.legalEntityKey
where ofr.offerRoleTypeKey in (8,10,11,12)
and le.legalEntityTypeKey = 2
and ofr.offerkey= @offerKey
group by le.legalEntityKey, ofr.offerRoleTypeKey

--one main applicant, no sureties
if (select count(*) from @legalEntity where offerRoleTypeKey in (8,11)) = 1
	and (select count(*) from @LegalEntity where offerRoleTypeKey in (10,12)) = 0
	begin
		select @PrimaryLegalEntityKey = LegalEntityKey from @legalEntity
		where ranked = 1
		print @PrimaryLegalEntityKey
	end
--more than one main applicant
if (select count(*) from @legalEntity where offerRoleTypeKey in (8,11)) > 1
	begin
		select @PrimaryLegalEntityKey = LegalEntityKey from @legalEntity
		where ranked = 1 and offerRoleTypeKey in (8,11)
		select @SecondaryLegalEntityKey = LegalEntityKey from @legalEntity
		where ranked = 2 and offerRoleTypeKey in (8,11)
	end
	
--one main applicant with sureties
if (select count(*) from @legalEntity where offerRoleTypeKey in (8,11)) = 1
	and (select count(*) from @LegalEntity where offerRoleTypeKey in (10,12)) >= 1
	begin
		select @PrimaryLegalEntityKey = LegalEntityKey from @legalEntity
		where ranked = 1 and offerRoleTypeKey in (8,11)
		select @SecondaryLegalEntityKey = LegalEntityKey from @legalEntity
		where ranked = 1 and offerRoleTypeKey in (10,12)
	end

--only single surety
if (select count(*) from @legalEntity where offerRoleTypeKey in (8,11)) = 0
	and (select count(*) from @LegalEntity where offerRoleTypeKey in (10,12)) = 1
	begin
		select @PrimaryLegalEntityKey = LegalEntityKey from @legalEntity
		where ranked = 1 and offerRoleTypeKey in (10,12)
	end

--multiple sureties no main applicants
if (select count(*) from @legalEntity where offerRoleTypeKey in (8,11)) = 0
	and (select count(*) from @LegalEntity where offerRoleTypeKey in (10,12)) > 1
	begin
		select @PrimaryLegalEntityKey = LegalEntityKey from @legalEntity
		where ranked = 1 and offerRoleTypeKey in (10,12)
		select @SecondaryLegalEntityKey = LegalEntityKey from @legalEntity
		where ranked = 2 and offerRoleTypeKey in (10,12)
	end
	;
with results as (
select 'Primary' as ApplicantType, @PrimaryLegalEntityKey as LegalEntityKey, 
dbo.legalEntityLegalName(@PrimaryLegalEntityKey,0) as Name
union all
select 'Secondary', @SecondaryLegalEntityKey as Secondary_LegalEntityKey,
dbo.legalEntityLegalName(@SecondaryLegalEntityKey,1) as Name
)

select r.*,le.idnumber from results r
join LegalEntity le on r.LegalEntityKey=le.legalEntityKey

