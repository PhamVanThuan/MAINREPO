USE [2am]
IF Object_id('Tempdb..#TmpFS') IS NOT NULL
	DROP TABLE 	#TmpFS;
IF Object_id('Tempdb..#LEAge') IS NOT NULL
	DROP TABLE 	#LEAge;	
IF Object_id('test.QualifyingAccounts') IS NOT NULL
	DROP TABLE test.QualifyingAccounts

Create Table #TmpFS 
	(
		MLAccountKey INT,
		FSKey INT,
		OfferKey INT
	)

;with CTE(AccountKey, FSKey, OfferKey) as
(
	----------------
	-- READVANCES --
	----------------
	select	fs.AccountKey,
			ft.FinancialServiceKey,
			-1
	from [2am].fin.FinancialTransaction ft  (nolock)
	join [2am].dbo.FinancialService fs  (nolock) on ft.FinancialServiceKey=fs.FinancialServiceKey
	join [2am].dbo.Account a  (nolock) on fs.AccountKey=a.AccountKey and a.AccountStatusKey=1
	where ft.TransactionTypeKey = 140 
	AND a.RRR_OriginationSourceKey <> 4 
	and ft.InsertDate > convert(datetime,convert(varchar,getdate() - 800,103),103) 
		
	UNION 
	----------------------------------
	-- NEW BUSINESS & FURTHER LOANS --
	----------------------------------	
	select distinct 
			fs.AccountKey,
			fs.FinancialServiceKey,
			o.OfferKey
	from [2am].dbo.Offer o (nolock)
	join [2am].dbo.FinancialService fs  (nolock) on o.AccountKey=fs.AccountKey and fs.FinancialServiceTypeKey = 1
	join [2am].dbo.OfferInformation oi (nolock) on o.OfferKey = oi.OfferKey
	join [2am].dbo.StageTransition st (nolock) on o.OfferKey = st.GenericKey
	join [2am].dbo.StageDefinitionStageDefinitionGroup sdsdg (nolock) on sdsdg.StageDefinitionStagedefinitionGroupKey = st.StageDefinitionStagedefinitionGroupKey
	join [2am].dbo.StageDefinition sd (nolock) on sd.StageDefinitionKey = sdsdg.StageDefinitionKey 
	join [2am].dbo.StageDefinitionGroup stg (nolock) on stg.StageDefinitionGroupKey = sdsdg.StageDefinitionGroupKey
	where o.AccountKey is not null 
	and o.OfferStatusKey = 1 -- open
	and o.OfferTypeKey in (4, 6, 7, 8) -- Further Loan, Switch Loan, New Purchase Loan, Refinance Loan
	and oi.ProductKey not in (3,4,10) -- Home Owners Cover(HOC), Life Policy, Quick Cash
	and o.OriginationSourceKey not in (4,5) -- RCS, SA Life
	and sd.Description = 'AttAssigned' 
	and stg.Description = 'Registrations' 
	and oi.OfferInformationTypeKey = 3 -- Accepted Offer
	and oi.OfferInformationKey in 
	(
		select max(offerinformationkey) 
		from [2am].dbo.OfferInformation (nolock) 
		where OfferKey = o.OfferKey 
		group by OfferKey
	) -- latest offerinformation
	and st.TransitionDate >= convert(datetime,convert(varchar,getdate() - 800,103),103) -- less than 1 day ago
	)
insert into 
	#tmpFS
select 
	CTE.AccountKey, CTE.FSKey, CTE.OfferKey
from 
	CTE
-----------------------------------------------------------------------------------
-- Exclude loans that have existing SA Life Applications unless they have been : 
-- 1. closed
-- 2. accepted and account was closed more than 12 months ago (ie policy cancelled)
-- 3. ntu'd more than 12 months ago
-----------------------------------------------------------------------------------
delete t
from #TmpFS t 
join [2am].dbo.Account la (nolock) on t.mlAccountKey = la.parentAccountKey
join [2am].dbo.Offer o (nolock) on  o.AccountKey=la.AccountKey
join [2am].dbo.OfferLife ol (nolock) on ol.OfferKey = o.OfferKey 
where o.OfferStatusKey not in (2,3,4,5) -- Closed, Accepted, NTU, Decline
or (o.OfferStatusKey = 3 and la.CloseDate >= dateadd(month, -12,getdate())) -- offer accepted and accout is closed within last 12 mnths
or (o.OfferStatusKey in (4,5) and ol.DateLastUpdated >= dateadd(month, -12,getdate())) -- NTU'd within last 12 mnths
	
------------------------------------------------------------------------------------------------
-- Reject loan if the offer has ever been resubbed and it has previously had a life lead created
------------------------------------------------------------------------------------------------
delete t
from #TmpFS t 
join [2am].dbo.OfferAccountRelationship oar (nolock) on oar.OfferKey  = t.OfferKey 
join [2am].dbo.Account life (nolock) on life.AccountKey = oar.AccountKey and life.RRR_ProductKey = 4 -- life account
join [2am].dbo.StageTransition st (nolock) on st.GenericKey = t.OfferKey and st.StageDefinitionStageDefinitionGroupKey=2142 -- Resubmit to Credit

---------------------------------------------------------------
-- Exclude loans that are currently 3 or more months in arrears 
---------------------------------------------------------------
delete t
from [dw].dwwarehousepre.Securitisation.FactAccountAttribute AS fa (nolock)
join [2am].dbo.FinancialService fs (nolock) on fa.AccountKey = fs.AccountKey and fs.FinancialServiceTypeKey in (1,2)
join #tmpfs t on fs.FinancialServiceKey = t.fskey
where fa.MonthsInArrears >= 3

---------------------------------------
-- Exclude loans under debt counciling.
---------------------------------------
delete t
from #tmpfs t
join 
(
	select financialservicekey as FSKey
	from 
	(
		select financialservicekey, StageDefinitionStageDefinitionGroupKey
		from [2am].dbo.StageTransitionComposite stc (nolock)
		join [2am].dbo.financialservice fs (nolock) on stc.generickey=fs.AccountKey
		join #tmpfs t on fs.financialservicekey=t.fskey
		where StageDefinitionStageDefinitionGroupKey in (3851, 3852)
	) p
	pivot
	(
		count(StageDefinitionStageDefinitionGroupKey) for StageDefinitionStageDefinitionGroupKey IN ( [3851], [3852])
	) as pvt
	where [3851] - [3852] <> 0
) x on x.FSKey=t.FSKey

------------------------------------------------------------------------------------------------
-- Exclude loans that have existing SA Life Policies unless they have been : 
-- 1. closed
-- 2. cancelled more than 12 months ago
-- 3. ntu'd more than 12 months ago
------------------------------------------------------------------------------------------------
delete t
from [2am].dbo.LifePolicy lp (nolock)
join [2am].dbo.FinancialService fsl (nolock) on lp.FinancialServiceKey = fsl.FinancialServiceKey 
join [2am].dbo.FinancialService fsVL (nolock) on fsl.ParentFinancialServiceKey=fsVL.FinancialServiceKey
join #TmpFS t on fsVL.FinancialServiceKey=t.fskey
where lp.PolicyStatusKey not in (4,5,12,14,15,16) -- Cancelled from Inception, Cancelled with Prorata, Closed, Closed - System Error, Cancelled – No Refund, Not Taken Up
or (lp.PolicyStatusKey in (4,5,15) and lp.DateOfCancellation >= dateadd(month, -12,getdate())) -- Cancelled within last 12 mnths
or (lp.PolicyStatusKey in (16) and lp.DateLastUpdated >= dateadd(month, -12,getdate())) -- NTU'd within last 12 mnths

-----------------------------------------------------------
-- Include Loan if there is at least role who is :
-- 1. a Main Applicant/Suretor 
-- 2. a Natural Person
-- 3. not overexposed in terms of the group exposure rules		
-- 4. between 18 & 65 years old
-----------------------------------------------------------
select distinct accountkey, idnumber, [2am].dbo.fagenextbirthday( le.idnumber ) as nextBirthday
into #LEAge 
from   [2am]..[Role] r with (nolock)
	JOIN [2am]..legalentity le (nolock) on r.legalentitykey = le.legalentitykey
where  r.roletypekey IN ( 2, 3 ) -- main applicant, suretor
	AND le.legalentitytypekey = 2 -- natural person
delete FROM #tmpfs where mlaccountkey not in ( SELECT accountkey FROM #LEAge WHERE nextBirthday > 18 and nextBirthday <= 65 ) 

------------------------------------------------------------
-- Create Test Table
------------------------------------------------------------
select * into test.QualifyingAccounts from #TmpFS

go

use [x2]
IF OBJECT_ID ('x2data.QualifyingAccountsMaintenance','TR') IS NOT NULL
    DROP TRIGGER x2data.QualifyingAccountsMaintenance;
GO

CREATE TRIGGER x2data.QualifyingAccountsMaintenance
ON x2.x2data.lifeorigination
AFTER INSERT
AS
IF OBJECT_ID ('[2am].test.QualifyingAccounts') IS NOT NULL
BEGIN
	IF OBJECT_ID ('[2am].test.CreatedLifeLeads') IS NOT NULL
		BEGIN
			insert into [2am].test.CreatedLifeLeads
			select InstanceID, LoanNumber, GETDATE() as CreationDate, AssignTo as AssignedConsultant from inserted
		END
	ELSE
		BEGIN
			select InstanceID, LoanNumber, GETDATE() as CreationDate, AssignTo as AssignedConsultant
			into [2am].test.CreatedLifeLeads 
			from inserted
		END
END
GO

ENABLE TRIGGER x2data.QualifyingAccountsMaintenance ON x2.x2data.lifeorigination