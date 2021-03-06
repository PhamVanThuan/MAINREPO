
USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[createPersonalLoanAutomationCases]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[createPersonalLoanAutomationCases]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].createPersonalLoanAutomationCases

GO

CREATE PROCEDURE test.createPersonalLoanAutomationCases

AS


if exists (select * from INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'test' 
and TABLE_NAME = 'PersonalLoanAutomationCases')
begin
	drop table test.PersonalLoanAutomationCases
	PRINT 'Dropping table test.PersonalLoanAutomationCases'
End

			CREATE TABLE test.PersonalLoanAutomationCases
				(	
					LegalEntityKey int,
					OfferKey int,
					ExpectedEndState varchar(100),
					ScriptFile varchar(100),
					ScriptToRun varchar(100),
					ExcludeCreditLife int null
				)			

CREATE TABLE #LegalEntities (legalEntityKey INT)

INSERT INTO #LegalEntities
SELECT TOP 10000 le.legalEntityKey FROM
dbo.legalentity as le
INNER JOIN dbo.role as r on le.legalentitykey = r.legalentitykey
    AND r.roletypekey = 2
    AND r.GeneralStatusKey = 1
INNER JOIN dbo.account as a
    on r.accountkey = a.accountkey
    AND a.accountstatuskey = 1
    AND a.RRR_ProductKey IN (1,2,5,6,9,11)
LEFT JOIN dbo.externalrole as er on le.legalentitykey = er.legalentitykey
    AND er.generickeytypekey = 2
LEFT JOIN dbo.offer as o on er.generickey = o.offerkey
LEFT JOIN debtcounselling.DebtCounselling dc ON a.AccountKey = dc.AccountKey
	AND dc.DebtCounsellingStatusKey = 1
LEFT JOIN (select ofr.legalEntityKey from dbo.Offer o
join dbo.OfferAttribute oa on o.offerKey = oa.offerKey
	and oa.OfferAttributeTypeKey = 30
join dbo.offerRole ofr on o.offerKey = ofr.offerKey
	and ofr.offerRoleTypeKey in (8,10,11,12)
	) as CapitecClients on le.LegalEntityKey = CapitecClients.LegalEntityKey
where 
er.externalrolekey IS NULL 
AND o.offerkey IS NULL
AND dc.DebtCounsellingKey IS NULL 
AND le.LegalEntityTypeKey = 2
AND le.MaritalStatusKey IS NOT NULL
AND le.Salutationkey IS NOT NULL
AND le.GenderKey IS NOT NULL
AND le.CitizenTypeKey = 1
AND len(idNumber) = 13
AND le.DateOfBirth IS NOT NULL
AND CapitecClients.LegalEntityKey IS NULL

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 50 le.legalEntityKey, 'Manage Lead', 'PersonalLoans.xaml','CreateCaseToManageLead' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 50 le.legalEntityKey, 'Manage Application', 'PersonalLoans.xaml','CreateCaseToManageApplication' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 50 le.legalEntityKey, 'Document Check', 'PersonalLoans.xaml','CreateCaseToDocumentCheck' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'Credit', 'PersonalLoans.xaml','CreateCaseToCredit' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'Declined by Credit', 'PersonalLoans.xaml','CreateCaseToDeclinedByCredit' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'Legal Agreements', 'PersonalLoans.xaml','CreateCaseToLegalAgreements' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'NTU', 'PersonalLoans.xaml','CreateAndNTUFromManageApplication' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'NTU', 'PersonalLoans.xaml','CreateAndNTUFromLegalAgreements' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'Verify Documents', 'PersonalLoans.xaml','CreateCaseToVerifyDocuments' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'Disbursement', 'PersonalLoans.xaml','CreateCaseToDisbursement' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'NTU', 'PersonalLoans.xaml','CreateAndNTUFromDisbursement' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun)
select top 25 le.legalEntityKey, 'Disbursed', 'PersonalLoans.xaml', 'CreateCaseToDisbursed' 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun, ExcludeCreditLife)
select top 10 le.legalEntityKey, 'Disbursed', 'PersonalLoans.xaml', 'CreateCaseToDisbursed', 1 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun, ExcludeCreditLife)
select top 25 le.legalEntityKey, 'Credit', 'PersonalLoans.xaml', 'CreateCaseToCreditAndEscalateToExceptions', 1 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun, ExcludeCreditLife)
select top 50 le.legalEntityKey, 'Manage Application', 'PersonalLoans.xaml','CreateCaseToManageApplication', 1 
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun, ExcludeCreditLife)
select top 10 le.legalEntityKey, 'Disbursement', 'PersonalLoans.xaml','CreateCaseToDisbursement', 1
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

insert into test.personalLoanAutomationCases
(legalEntityKey, expectedEndState, ScriptFile, ScriptToRun, ExcludeCreditLife)
select top 50 le.legalEntityKey, 'Manage Lead', 'PersonalLoans.xaml','CreateCaseToManageLead', 1
from #LegalEntities le
left join test.personalLoanAutomationCases aut
on le.legalentitykey = aut.legalentitykey
where aut.legalentitykey is null
order by newid()

DROP TABLE #LegalEntities





