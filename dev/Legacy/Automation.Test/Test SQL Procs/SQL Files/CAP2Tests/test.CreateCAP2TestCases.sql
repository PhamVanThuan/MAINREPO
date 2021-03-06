USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [test].[CreateCAP2TestCases] 
IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[CreateCAP2TestCases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[CreateCAP2TestCases]
	Print 'Dropped Proc [test].[CreateCAP2TestCases]'
End
Go

CREATE PROCEDURE [test].[CreateCAP2TestCases] AS

CREATE TABLE #table (
TestIdentifier varchar(50),
TestType varchar(50),
AccountKey int ,
PTI float ,
LTV float ,
CurrentBalance float ,
BondAmount float ,
LoanAgreementAmount float ,
SPVKey int ,
SPVMaxLTV int ,
SPVMaxPTI int ,
NewBalanceOnePerc float ,
CapQualifyOnePerc varchar(25)  ,
NewBalanceTwoPerc float ,
CapQualifyTwoPerc varchar(25)  ,
NewBalanceThreePerc float ,
CapQualifyThreePerc varchar(25)  ,
CAPApplicationTypeOnePerc varchar(25)  ,
CAPApplicationTypeTwoPerc varchar(25)  ,
CAPApplicationTypeThreePerc varchar(25)  ,
MonthlyInstalmentOnePerc float ,
MonthlyInstalmentTwoPerc float ,
MonthlyInstalmentThreePerc float,
HouseholdIncome float,
ValuationAmount float,
UnderDebtCounselling bit,
CapTypeConfigurationKey INT,
Premium1 FLOAT,
Premium2 FLOAT,
Premium3 FLOAT,
CapOfferKey int,
ExpectedEndState varchar(250),
ScriptFile varchar(250),
ScriptToRun varchar(250)
)

--REMOVE TEST DATA
TRUNCATE TABLE test.AutomationCAP2TestCases

--we need a debt counselling test case for exclusions
CREATE TABLE #debt_counselling (   
  generickey     INT)   
--this will retrieve the debt counselling in/out transitions  
INSERT INTO #debt_counselling   
				SELECT distinct r.accountKey
				FROM [2am]..[Role] r (NOLOCK)
				JOIN [2am]..LegalEntity le (NOLOCK) 
					ON le.LegalEntityKey = r.LegalEntityKey
				JOIN [2am]..ExternalRole er (NOLOCK) 
					ON le.LegalEntityKey = er.LegalEntityKey and er.GenericKeyTypeKey = 27 --DebtCounselling
				JOIN [2am].debtcounselling.DebtCounselling dc (NOLOCK) 
					ON er.GenericKey = dc.DebtCounsellingKey
				WHERE
					er.GeneralStatusKey = 1
						AND 
					dc.DebtCounsellingStatusKey = 1
						AND 
					er.ExternalRoleTypeKey = 1;

insert into #table
select top 1 'ReadvanceGreaterThan80' as Identifier,'Create' as TestType,test.*, 0, '', 'CAP2Offers.xaml', 'ReadvanceRequiredDoNotIgnoreWarnings' 
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey 
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where test.ltv > 79
and test.capapplicationtypeoneperc = 'readv' and co.capofferkey is null
and debt.generickey is null and (test.ltv < test.spvmaxltv)
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 5 'FurtherAdvanceCAP'+cast(row_number() over (order by test.accountkey desc) as varchar(3)) as [TestIdentifier],
'Create' as TestType, test.*, 0, '', 'CAP2Offers.xaml', ''  
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey 
	and spat.spvattributetypekey=1
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where 
spat.value = 1
and test.capapplicationtypeoneperc = 'F.Adv > LAA' and test.capqualifyoneperc='Qualify'
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 5 'ReadvanceCAP'+cast(row_number() over (order by test.accountkey desc) as varchar(3)) as [TestIdentifier],
'Create' as TestType,test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.spvattributetypekey=2
and test.CAPApplicationTypeOnePerc='readv'
and test.currentbalance > 300000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and test.ltv < 75
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'RecalculateCAP2OfferPostTransaction','Create' as TestType,test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.spvattributetypekey=2
and test.CAPApplicationTypeOnePerc='readv'
and test.currentbalance > 300000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and test.ltv < 80
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'CAPExpiryTest','Create' as TestType,test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey
	and spvattributetypekey=1 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.value = 1
and test.CAPApplicationTypeOnePerc='readv'
and test.currentbalance > 300000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and test.ltv < 80
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0


insert into #table
select top 1 'CAP2PrintLetter', 'Create' as TestType,test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey
	and spvattributetypekey=1 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.value=1
and test.CAPQualifyThreePerc='Qualify'
and test.currentbalance > 300000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and test.ltv < 80
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'CAP2NTU', 'Create' as TestType,test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey
	and spvattributetypekey=1 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.value=1
and test.CAPQualifyThreePerc='Qualify'
and test.currentbalance > 300000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and test.ltv < 80
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'CAP2Decline','Create' as TestType,test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey
	and spvattributetypekey=1 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.value=1
and test.CAPQualifyThreePerc='Qualify'
and test.currentbalance > 300000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and test.ltv < 80
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0


insert into #table
select top 1 'VariFixCAP', 'Rule' as TestType,
a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','', dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', ''   
from account a join financialservice fs
on a.accountkey=fs.accountkey
left join #table t on a.accountkey=t.accountkey
left join #debt_counselling debt on a.accountkey=debt.generickey
left join detail d on a.accountkey=d.accountkey and d.detailtypekey in (11,150)
where a.accountstatuskey=1 and fs.accountstatuskey=1
and rrr_productkey=2
and t.accountkey is null
and debt.generickey is null
and d.accountkey is null


insert into #table
select top 1 'ExistingCAP', 'Rule' as TestType
, a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','', dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', ''   
from account a 
join financialservice fs on a.accountkey=fs.accountkey
	and fs.parentFinancialServiceKey is null
join fin.financialadjustment fa on fs.financialservicekey=fa.financialservicekey 
	and fa.financialadjustmentsourcekey=1
	and fa.financialadjustmentstatuskey=1
left join capoffer co on a.accountkey=co.accountkey 
	and co.capstatuskey in (1,5,6,7,8,9,10,11,12,13)
left join #table t on a.accountkey=t.accountkey
left join #debt_counselling debt on a.accountkey=debt.generickey
left join detail d on a.accountkey=d.accountkey and d.detailtypekey in (11,150)
where a.accountstatuskey=1 and fs.accountstatuskey=1
and datediff(mm, getdate(),dateadd(mm,24,fa.fromdate)) > 4
and co.cappaymentoptionkey is not null
and t.accountkey is null and co.capofferkey is not null
and debt.generickey is null
and d.accountkey is null


insert into #table
select top 1 'InterestOnlyCAP', 'Rule' as TestType
, a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','', dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', ''  
from account a 
join financialservice fs on a.accountkey=fs.accountkey
	and fs.parentFinancialServiceKey is null
join fin.mortgageloan ml on fs.financialservicekey=ml.financialservicekey
join bondmortgageloan bml on ml.financialservicekey=bml.financialservicekey
join fin.financialadjustment fa on fs.financialservicekey=fa.financialservicekey 
	and fa.financialadjustmentsourcekey=6 
	and fa.financialadjustmentstatuskey=1
join bond b on bml.bondkey=b.bondkey
left join #table t on a.accountkey=t.accountkey
left join #debt_counselling debt on a.accountkey=debt.generickey
left join detail d on a.accountkey=d.accountkey and d.detailtypekey in (11,150)
join fin.balance bal on bal.financialservicekey=ml.financialservicekey
where a.accountstatuskey=1 and fs.accountstatuskey=1
and bal.amount > 200000 and t.accountkey is null
and debt.generickey is null
and d.accountkey is null
group by a.AccountKey
having sum(bal.Amount) + 50000 < sum(BondLoanAgreementAmount)

insert into #table
select top 1 'ClosedAccountCAP', 'Rule' as TestType
, a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','' , dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', ''  
from account a
join financialservice fs on a.accountkey=fs.accountkey
	and fs.parentFinancialServiceKey is null
join fin.mortgageloan ml on fs.financialservicekey=ml.financialservicekey
left join #table t on a.accountkey=t.accountkey
left join #debt_counselling debt on a.accountkey=debt.generickey
left join detail d on a.accountkey=d.accountkey and d.detailtypekey in (11,150)
where rrr_productkey=9 and a.accountstatuskey=2
and t.accountkey is null
and debt.generickey is null
and d.accountkey is null

insert into #table
select top 1 'RCSAccountCAP', 'Rule' as TestType
, a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','', dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', '' 
from account a 
join financialservice fs on a.accountkey=fs.accountkey
	and fs.parentFinancialServiceKey is null
join fin.mortgageloan ml on fs.financialservicekey=ml.financialservicekey
left join #table t on a.accountkey=t.accountkey
left join #debt_counselling debt on a.accountkey=debt.generickey
left join detail d on a.accountkey=d.accountkey and d.detailtypekey in (11,150)
where a.spvkey=125 and a.accountstatuskey=1 and fs.accountstatuskey=1
and rrr_productkey=1
and t.accountkey is null
and debt.generickey is null
and d.accountkey is null

insert into #table
select top 1 'UnderCancellationCAP', 'Rule' as TestType
, a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','', dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', ''   
from account a 
join financialservice fs on a.accountkey=fs.accountkey
	and fs.parentFinancialServiceKey is null
join fin.mortgageloan ml on fs.financialservicekey=ml.financialservicekey
join bondmortgageloan bml on ml.financialservicekey=bml.financialservicekey
join bond b on bml.bondkey=b.bondkey
join detail d on a.accountkey=d.accountkey and detailtypekey in (11)
left join fin.financialadjustment fa on fs.financialservicekey=fa.financialservicekey 
	and fa.financialadjustmentsourcekey in (1,6) 
	and fa.financialadjustmentstatuskey=1
left join #table t on a.accountkey=t.accountkey
left join #debt_counselling debt on a.accountkey=debt.generickey
left join fin.balance bal on bal.financialservicekey=fs.financialservicekey
where a.accountstatuskey=1 and fs.accountstatuskey=1
and bal.amount > 200000 and t.accountkey is null
and fa.financialadjustmentkey is null and a.rrr_productkey <> 2
and debt.generickey is null
group by a.AccountKey
having sum(bal.amount) + 50000 < sum(BondLoanAgreementAmount)

insert into #table
select top 1 'DebtCounsellingCAP', 'Rule' as TestType
, a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','', dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', ''   
from account a 
join financialservice fs on a.accountkey=fs.accountkey
	and fs.parentFinancialServiceKey is null 
join fin.mortgageloan ml on fs.financialservicekey=ml.financialservicekey
join bondmortgageloan bml on ml.financialservicekey=bml.financialservicekey
join bond b on bml.bondkey=b.bondkey
join debtcounselling.debtcounselling dc on a.accountKey=dc.accountKey and dc.debtCounsellingStatusKey=1
left join detail d on a.accountkey=d.accountkey and d.detailtypekey in (11,150)
left join fin.financialadjustment fa on fs.financialservicekey=fa.financialservicekey 
	and fa.financialadjustmentsourcekey in (1,6) 
	and fa.financialadjustmentstatuskey=1
left join #table t on a.accountkey=t.accountkey
left join fin.balance bal on bal.financialservicekey=fs.financialservicekey
where a.accountstatuskey=1 and fs.accountstatuskey=1
and bal.amount > 200000 and t.accountkey is null
and fa.financialadjustmentkey is null and a.rrr_productkey <> 2
and d.accountkey is null
group by a.AccountKey
having sum(bal.amount) + 50000 < sum(BondLoanAgreementAmount)

insert into #table
select top 1 'MinBalanceCAP', 'Rule' as TestType
, a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','', dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', ''   
from account a 
join financialservice fs on a.accountkey=fs.accountkey
	and fs.parentFinancialServiceKey is null 
join fin.mortgageloan ml on fs.financialservicekey=ml.financialservicekey
join bondmortgageloan bml on ml.financialservicekey=bml.financialservicekey
join bond b on bml.bondkey=b.bondkey
left join #debt_counselling det on a.accountkey=det.generickey
left join fin.financialadjustment fa on fs.financialservicekey=fa.financialservicekey 
	and fa.financialadjustmentsourcekey in (1,6) 
	and fa.financialadjustmentstatuskey=1
left join #table t on a.accountkey=t.accountkey
left join detail d on a.accountkey=d.accountkey and d.detailtypekey in (11,150)
left join fin.balance bal on bal.financialservicekey=fs.financialservicekey 
where a.accountstatuskey=1 and fs.accountstatuskey=1
and t.accountkey is null
and fa.financialadjustmentkey is null and a.rrr_productkey <> 2
and det.generickey is null and ((bal.amount) between 25000 and 60000)
and d.accountkey is null
group by a.AccountKey

insert into #table
select top 1 'OnePercDNQCAP', 'Rule' as TestType,
test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where test.CapQualifyOnePerc = 'DNQ' and test.CapQualifyTwoPerc = 'Qualify'
and test.CapQualifyThreePerc = 'Qualify'
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null

insert into #table
select top 1 'TwoPercDNQCAP', 'Rule' as TestType,
test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where test.CapQualifyOnePerc = 'DNQ' and test.CapQualifyTwoPerc = 'DNQ'
and test.CapQualifyThreePerc = 'Qualify'
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'ThreePercDNQCAP', 'Rule' as TestType,
test.*, 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where test.CapQualifyOnePerc = 'DNQ' and test.CapQualifyTwoPerc = 'DNQ'
and test.CapQualifyThreePerc = 'DNQ'
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null  and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'PTIExceededCAP', 'Create' as TestType, test.*, 0, '', 'CAP2Offers.xaml', ''  
from test.captestcases test
join spv.SPV on test.spvkey=spv.spvkey
join spv.spvattribute spat on spat.spvkey=spv.spvkey
	and spvattributetypekey=1 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where test.capqualifyoneperc='Qualify'
and spat.value=1
and ((test.monthlyinstalmentoneperc/test.householdincome)*100 > test.SPVMaxPTI) 
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null
and test.LTV < 100
and test.currentbalance > 75000  and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'LTVExceededCAP','Create' as TestType, test.*, 0, '', 'CAP2Offers.xaml', ''  
from test.captestcases test
join spv.SPV on test.spvkey=spv.spvkey
join spv.spvattribute spat on spat.spvkey=spv.spvkey
	and spvattributetypekey=1 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where test.capqualifyoneperc='Qualify' and test.CAPApplicationTypeOnePerc='readv'
and spat.value=1
and ((test.LTV > test.SPVMaxLTV) and test.LTV < 100) 
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null and test.LatestValuation > 0
and d.accountkey is null
and test.currentbalance > 75000  and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'CAP2ResetCheck', 'Rule' as TestType
, a.AccountKey,'','','','',''
,'','','','','','','','','','' ,'','','','','','','', dbo.[fIsAccountUnderDebtCounselling](a.accountKey),0,0,0,0, 0, '', 'CAP2Offers.xaml', ''   
from account a 
join financialservice fs on a.accountkey=fs.accountkey
	and fs.parentFinancialServiceKey is null 
join fin.mortgageloan ml on fs.financialservicekey=ml.financialservicekey
left join #debt_counselling debt on a.accountkey=debt.generickey
left join fin.financialadjustment fa on fs.financialservicekey=fa.financialservicekey and fa.financialadjustmentsourcekey in (1,6) and fa.financialadjustmentstatuskey=1
left join #table t on a.accountkey=t.accountkey
left join detail d on a.accountkey=d.accountkey and d.detailtypekey in (11,150)
where a.accountstatuskey=1 and fs.accountstatuskey=1
and t.accountkey is null
and fa.financialadjustmentkey is null and a.rrr_productkey <> 2
and debt.generickey is null
and d.accountkey is null
and datediff(dd, a.opendate, fs.nextresetdate) < 93
group by a.AccountKey

insert into #table
select top 1 'CAP2SPVNotAllowed','Create', test.*, 0, '', 'CAP2Offers.xaml', ''  
from test.captestcases test
join spv.SPV on test.spvkey=spv.spvkey
join spv.spvattribute spat on spat.spvkey=spv.spvkey
	and spvattributetypekey=1 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.value = 0
and test.capapplicationtypethreeperc='F.Adv > LAA'
and test.capqualifythreeperc='Qualify'
and test.currentbalance > 100000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null  and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'CAP2SPVNotAllowedFAdvUnderLAA','Create', test.*, 0, '', 'CAP2Offers.xaml', ''  
from test.captestcases test
join spv.SPV on test.spvkey=spv.spvkey
join spv.spvattribute spat on spat.spvkey=spv.spvkey
	and spvattributetypekey=1
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.value=0
and test.capapplicationtypeoneperc='F.Adv < LAA'
and test.capqualifyoneperc='Qualify'
and test.currentbalance > 250000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null  and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'FurtherAdvanceCAPUnderLAA','Create' as TestType, test.*, 0, 'Readvance Required', 'CAP2Offers.xaml', 'CAP2OfferGranted'  
from test.captestcases test
join spv.spv spv on test.spvkey=spv.spvkey
join spv.spvattribute spat on spat.spvkey=spv.spvkey
	and spvattributetypekey=1  
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where 
spat.value=1 and test.currentbalance > 250000 and
test.capapplicationtypeoneperc = 'F.Adv < LAA' and test.capqualifyoneperc='Qualify'
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null  and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'CAP2SPVNotAllowedReadvance','Create', test.* , 0, '', 'CAP2Offers.xaml', '' 
from test.captestcases test
join spv.SPV on test.spvkey=spv.spvkey
join spv.spvattribute spat on spat.spvkey=spv.spvkey
	and spvattributetypekey=1 
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where spat.value=0
and test.capapplicationtypeoneperc='readv'
and test.capqualifyoneperc='Qualify'
and test.currentbalance > 250000
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'FurtherAdvanceCAPUnderLAA2','Create' as TestType, test.*, 0, '', 'CAP2Offers.xaml', ''  
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey
	and spvattributetypekey=1  
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where 
spat.value=1 and test.currentbalance > 250000 and
test.capapplicationtypeoneperc = 'F.Adv < LAA' and test.capqualifyoneperc='Qualify'
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null and test.AccountUnderDebtCounselling = 0

insert into #table
select top 1 'FurtherAdvanceCAPOverLAA','Create' as TestType, test.*, 0, 'Granted CAP2 Offer', 'CAP2Offers.xaml', 'CAP2OfferGranted'  
from test.captestcases test
join spv.SPV s on test.spvkey=s.spvkey
join spv.spvattribute spat on spat.spvkey=s.spvkey
	and spvattributetypekey=1
left join #table t on test.accountkey=t.accountkey
left join capoffer co on test.accountkey=co.accountkey 
and co.capstatuskey in (1,2,5,6,7,8,9,10,11,12,13)
left join #debt_counselling debt on test.accountkey=debt.generickey
left join detail d on test.accountkey=d.accountkey and d.detailtypekey in (11,150)
where 
spat.value=1 and test.currentbalance > 250000 and
test.capapplicationtypetwoperc = 'F.Adv > LAA' and test.capqualifyoneperc='Qualify'
and (test.ltv + 10 < test.spvmaxltv) and (test.pti+4 < test.spvmaxpti)
and t.accountkey is null and co.capofferkey is null
and debt.generickey is null
and d.accountkey is null  and test.AccountUnderDebtCounselling = 0

insert into test.AutomationCAP2TestCases
select * from #table

--set the automation script to run for those listed as create
update test.automationcap2testcases
set expectedEndState = 'Open CAP2 Offer', 
scriptToRun = 'CAPCaseCreate',
scriptFile = 'CAP2Offers.xaml'
where testType='Create'

--set the automation script to run for those listed as create
update test.automationcap2testcases
set expectedEndState = 'Awaiting Forms', 
scriptToRun = 'FormsSent',
scriptFile = 'CAP2Offers.xaml'
where testidentifier in ('ReadvanceCAP1', 'ReadvanceCAP2','ReadvanceCAP3','ReadvanceGreaterThan80',
'PTIExceededCAP','LTVExceededCAP','FurtherAdvanceCAPUnderLAA2')

drop table #table
drop table #debt_counselling

SET ANSI_NULLS OFF
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

