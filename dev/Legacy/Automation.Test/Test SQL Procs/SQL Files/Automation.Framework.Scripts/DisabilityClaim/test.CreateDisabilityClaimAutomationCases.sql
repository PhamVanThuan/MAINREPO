use [2AM]
go

set ansi_nulls on
go
set quoted_identifier on
go

if exists (select * from sys.objects where object_id = object_id(N'[2AM].[test].[CreateDisabilityClaimAutomationCases]') and type in (N'P',N'PC'))
drop procedure test.CreateDisabilityClaimAutomationCases
go

create procedure test.CreateDisabilityClaimAutomationCases
as

if exists (select * from information_schema.tables where table_schema = 'test' and table_name = 'DisabilityClaimAutomationCases')
begin
	drop table test.DisabilityClaimAutomationCases
	print 'Dropping table test.DisabilityClaimAutomationCases'
end

create table test.DisabilityClaimAutomationCases
	(	
		LifeAccountKey int,
		LegalEntityKey int,
		DisabilityClaimKey int,
		ExpectedEndState varchar(100),
		ScriptFile varchar(100),
		ScriptToRun varchar(100)
	)			

create table #OpenLifeAccountAndAssuredLifeLegalEntity (lifeAccountKey int, legalEntityKey int)

insert into #OpenLifeAccountAndAssuredLifeLegalEntity
select top 250
c.AccountKey as LifeAccountKey, le.LegalEntityKey
from [2AM].dbo.Account p
join [2AM].dbo.Account c on p.AccountKey = c.ParentAccountKey
join [2AM].dbo.Role r on c.AccountKey = r.AccountKey
join [2AM].dbo.LegalEntity le on r.LegalEntityKey = le.LegalEntityKey
where p.AccountStatusKey = 1
and c.AccountStatusKey = 1 and c.RRR_ProductKey = 4
and r.RoleTypeKey = 1 and r.GeneralStatusKey = 1
and c.AccountKey not in (select AccountKey from [2AM].dbo.DisabilityClaim where DisabilityClaimStatusKey in (1,3))
order by newid()

insert into test.DisabilityClaimAutomationCases (LifeAccountKey, LegalEntityKey, ExpectedEndState, ScriptFile, ScriptToRun)
select top 10 
	acc_le.lifeAccountKey, 
	acc_le.legalEntityKey, 
	'Claim Details', 
	'DisabilityClaim.xaml',
	'CreateCaseToClaimDetails'
from #OpenLifeAccountAndAssuredLifeLegalEntity acc_le
	left join test.DisabilityClaimAutomationCases aut on acc_le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.DisabilityClaimAutomationCases (LifeAccountKey, LegalEntityKey, ExpectedEndState, ScriptFile, ScriptToRun)
select top 10 
	acc_le.lifeAccountKey, 
	acc_le.legalEntityKey, 
	'Assess Claim', 
	'DisabilityClaim.xaml',
	'CreateCaseToAssessClaim'
from #OpenLifeAccountAndAssuredLifeLegalEntity acc_le
	left join test.DisabilityClaimAutomationCases aut on acc_le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.DisabilityClaimAutomationCases (LifeAccountKey, LegalEntityKey, ExpectedEndState, ScriptFile, ScriptToRun)
select top 10 
	acc_le.lifeAccountKey, 
	acc_le.legalEntityKey, 
	'Send Approval Letter', 
	'DisabilityClaim.xaml',
	'CreateCaseToSendApprovalLetter'
from #OpenLifeAccountAndAssuredLifeLegalEntity acc_le
	left join test.DisabilityClaimAutomationCases aut on acc_le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.DisabilityClaimAutomationCases (LifeAccountKey, LegalEntityKey, ExpectedEndState, ScriptFile, ScriptToRun)
select top 10 
	acc_le.lifeAccountKey, 
	acc_le.legalEntityKey, 
	'Approved Claim', 
	'DisabilityClaim.xaml',
	'CreateCaseToApprovedClaim'
from #OpenLifeAccountAndAssuredLifeLegalEntity acc_le
	left join test.DisabilityClaimAutomationCases aut on acc_le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

drop table #OpenLifeAccountAndAssuredLifeLegalEntity