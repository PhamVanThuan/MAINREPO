USE [2AM]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[ConfirmEmployment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[ConfirmEmployment]
go

/****** Object:  StoredProcedure [test].[CleanupNewBusinessOffer]    Script Date: 10/07/2010 11:55:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [test].[ConfirmEmployment]
		@offerkey int,
			@ConfirmedBy varchar(50)

AS

--Get all active, unconfirmed Employment records for all Legal Entities on an Offer
select	e.employmentkey,
			o.offerkey,
			le.legalentitykey
into	#updateemployment
from	offerrole o
			join legalentity le on o.legalentitykey = le.legalentitykey 
			join employment e on le.legalentitykey = e.legalentitykey
where	o.offerroletypekey in (8, 10, 11, 12)
			and le.legalentitystatuskey = 1
			and e.employmentstatuskey = 1
			and e.confirmeddate is null
			and o.offerkey = @offerkey

--Update Employment record
Update e
Set
	e.ConfirmedDate = getdate(),
	e.ConfirmedBasicIncome = e.BasicIncome,
	e.ConfirmedCommission = e.Commission,
	e.ConfirmedEmploymentFlag = 1,
	e.ConfirmedIncomeFlag = 1,
	e.ConfirmedBy = @ConfirmedBy,
	e.ContactPerson = 'Tester',
	e.ContactPhoneNumber = '1111111',
	e.ContactPhoneCode = '111',
	e.Department = 'Testing',
	e.EmploymentConfirmationSourceKey = (select top 1 EmploymentConfirmationSourceKey from EmploymentConfirmationSource where generalstatuskey = 1)
From	dbo.Employment e
			join #updateemployment u on e.employmentkey = u.employmentkey

--Insert EmploymentVerificationProcess record
if not exists(select 1 from	dbo.EmploymentVerificationProcess evp 
							join #updateemployment u on evp.employmentkey = u.employmentkey)
begin
	Insert Into dbo.EmploymentVerificationProcess (EmploymentKey,
				EmploymentVerificationProcessTypeKey,
				UserID,
				ChangeDate)
	Select	u.employmentkey,
				2, --(select top 1 EmploymentVerificationProcessTypeKey from EmploymentVerificationProcessType where generalstatuskey = 1),
				@ConfirmedBy,
				getdate()
	From	#updateemployment u
end

--Insert Subsidy record
if	not exists(Select 1 from offersubsidy s join #updateemployment u on u.offerkey = s.offerkey)
begin

	insert into	subsidy (SubsidyProviderkey,
					EmploymentKey,
					LegalEntityKey,
					SalaryNumber,
					Paypoint,
					Notch,
					Rank,
					GeneralStatusKey,
					StopOrderAmount)
	Select	(Select top 1 subsidyproviderkey from SubsidyProvider),
				u.employmentkey,
				u.legalentitykey,
				'EMP123456789',
				'',
				'',
				'',
				1,
				1000
	from #updateemployment u

	insert into offersubsidy (offerkey, subsidykey)
	select	u.offerkey, s.subsidykey
	from	subsidy s 
				join #updateemployment u  on s.employmentkey = u.employmentkey
end
	
drop table #updateemployment
go
--select * from dbo.EmploymentVerificationProcessType evpt 
--On evp.EmploymentVerificationProcessTypeKey = evpt.EmploymentVerificationProcessTypeKey

/*
begin transaction test
--rollback
--commit

declare @offerkey int
set @offerkey = 904591

update e set confirmeddate = null 
from	offerrole o 
			join legalentity le on o.legalentitykey = le.legalentitykey 
			join employment e on le.legalentitykey = e.legalentitykey
where o.offerkey = @offerkey

execute test.ConfirmEmployment @offerkey, 'sahl\andrewk'

select e.*			
from	offerrole o 
			join legalentity le on o.legalentitykey = le.legalentitykey 
			join employment e on le.legalentitykey = e.legalentitykey
where o.offerkey = @offerkey

select evp.*
from	offerrole o 
			join legalentity le on o.legalentitykey = le.legalentitykey 
			join employment e on le.legalentitykey = e.legalentitykey
			join EmploymentVerificationProcess evp on e.employmentkey = evp.employmentkey
where o.offerkey = @offerkey
*/