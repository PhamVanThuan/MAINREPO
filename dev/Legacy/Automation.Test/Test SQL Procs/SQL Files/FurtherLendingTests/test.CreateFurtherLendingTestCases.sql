USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[CreateFurtherLendingTestCases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[CreateFurtherLendingTestCases]
	Print 'Dropped Proc [test].[CreateFurtherLendingTestCases]'
End
Go

CREATE PROCEDURE [test].[CreateFurtherLendingTestCases]

AS
begin
declare @HighRisk int
declare @ModerateRisk int
declare @LowRisk int

set @HighRisk = 0
set @ModerateRisk = 0
set @LowRisk = 0
--we need to truncate the test.FurtherLendingTestCases table
IF OBJECT_ID('[2AM].test.FurtherLendingTestCases') IS NOT NULL
BEGIN
	TRUNCATE TABLE test.FurtherLendingTestCases
END

--insert new records
EXEC test.insertFLTestCases 20000

--we need a debt counselling test case for exclusions
CREATE TABLE #debt_counselling (   
  generickey     INT
  )   
--this will retrieve the debt counselling in/out transitions  
INSERT INTO #debt_counselling   
SELECT distinct r.accountKey
FROM [2am]..[Role] r (NOLOCK)
JOIN [2am]..LegalEntity le (NOLOCK) ON le.LegalEntityKey = r.LegalEntityKey
JOIN [2am]..ExternalRole er (NOLOCK) ON le.LegalEntityKey = er.LegalEntityKey 
	and er.GenericKeyTypeKey = 27 --DebtCounselling
JOIN [2am].debtcounselling.DebtCounselling dc (NOLOCK) ON er.GenericKey = dc.DebtCounsellingKey
WHERE er.GeneralStatusKey = 1
AND dc.DebtCounsellingStatusKey = 1
AND er.ExternalRoleTypeKey = 1;

--we need non performing loans for exclusions
SELECT NonPerforming.AccountKey
INTO
#NonPerforming
FROM
(  
select a.accountKey from [2am].dbo.Account a
join [2am].dbo.financialService fs on a.accountKey=fs.accountKey and parentFinancialServiceKey is null
join [2am].fin.financialAdjustment fa on fs.financialServiceKey = fa.financialServiceKey
and (financialadjustmentsourceKey=2 and financialAdjustmentTypeKey=5)
where fa.financialAdjustmentStatusKey=1
) as NonPerforming

SELECT DetailType.accountKey
INTO #DetailTypes
FROM 
(select distinct a.accountKey from [2am].dbo.Account a
join [2am].dbo.Detail d on a.accountKey=d.accountKey
where d.detailTypeKey in (
104,227,481,299,453,455,251,248,	
249,242,180,279,14, 294,295,597,
11,241)
) as DetailType

SELECT Offers.AccountKey
INTO #Offers
FROM
(SELECT distinct AccountKey
from [2am].dbo.Offer
where OfferTypeKey in (2,3,4) and OfferStatusKey = 1
)
as Offers

--filter out accounts that dont qualify
select test.* 
into #filteredTestCases
from test.furtherlendingtestcases test
	left join #Offers offer on test.accountkey=offer.accountkey 
	left join #DetailTypes d on test.accountkey = d.accountkey
	left join #debt_counselling debt on test.accountkey=debt.generickey
	left join #NonPerforming np on test.AccountKey=np.AccountKey 
where 
offer.accountKey is null
and d.accountKey is null
and np.AccountKey is null
and debt.genericKey is null
and test.Product <> 'Super Lo'

--we need a table variable to store what we have already used
declare @table table (AccountKey int)

	--remove the test data
	TRUNCATE TABLE test.AutomationFLTestCases

	insert into test.AutomationFLTestCases
	select top 10 'ReadvanceCreate'+cast(row_number() over (order by test.accountkey desc) as varchar(3)) as [TestIdentifier],
	'Readvances' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, round(ReadvanceAmount-1000,0) as ReadvanceAmount,
	-1 as FurtherAdvanceAmount, -1 as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0, 0
	from #filteredTestCases test
	where readvanceamount > 15000
	and test.rapidLTV < 79.5
	and valuationdate > DATEADD( yy, -2, GETDATE())
	order by test.accountKey desc
	
	insert into @table
	select AccountKey from test.AutomationFLTestCases

	insert into test.AutomationFLTestCases
	select top 5 'FurtherAdvanceCreate'+cast(row_number() over (order by test.accountkey desc) as varchar(3)) as [TestIdentifier],
	'FurtherAdvances' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, -1 as ReadvanceAmount,
	round(FurtherAdvanceAmount-1000,0) as FurtherAdvanceAmount, -1 as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0 , 0 
	from #filteredTestCases test
	where furtheradvanceamount > 15000
	and test.ltv < 75
	and test.AccountKey not in (select AccountKey from @table)
	and valuationdate > DATEADD( yy, -2, GETDATE())
	order by test.accountKey desc
	
	insert into @table
	select t.AccountKey from test.AutomationFLTestCases t
	where t.AccountKey not in (Select AccountKey from @table)

	insert into test.AutomationFLTestCases
	select top 5 'FurtherLoanCreate'+cast(row_number() over (order by test.accountkey desc) as varchar(3)) as [TestIdentifier],
	'FurtherLoans' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, -1 as ReadvanceAmount,
	-1 as FurtherAdvanceAmount, Round(FurtherLoanAmount-1000,0) as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0 , 0  
	from #filteredTestCases test
	where (furtherloanamount > 15000 and furtherloanamount < 500000)
	and test.ltv < 75
	and test.AccountKey not in (select AccountKey from @table)
	and valuationdate > DATEADD( yy, -2, GETDATE())
	and PTI < 20
	order by test.accountKey desc

	insert into @table
	select t.AccountKey from test.AutomationFLTestCases t
	where t.AccountKey not in (Select AccountKey from @table)

	insert into test.AutomationFLTestCases
	select top 1 'ReadvanceAndFAdvCreate'+cast(row_number() over (order by test.accountkey desc) as varchar(3)) as [TestIdentifier],
	'ReadvAndFAdv' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, round(ReadvanceAmount-1000,0) as ReadvanceAmount,
	round(FurtherAdvanceAmount-1000,0) as FurtherAdvanceAmount, Round(FurtherLoanAmount-1000,0) as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey,'' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0, 0   
	from #filteredTestCases test
	where (readvanceamount > 15000 and furtheradvanceamount > 15000) and furtherloanamount=0
	and test.ltv < 75
	and test.AccountKey not in (select AccountKey from @table)
	and valuationdate > DATEADD( yy, -2, GETDATE())
	order by test.accountKey desc

	insert into @table
	select t.AccountKey from test.AutomationFLTestCases t
	where t.AccountKey not in (Select AccountKey from @table)

	insert into test.AutomationFLTestCases
	select top 2 'ReadvFAdvAndFLCreate'+cast(row_number() over (order by test.accountkey desc) as varchar(3)) as [TestIdentifier],
	'All' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, round(ReadvanceAmount-1000,0) as ReadvanceAmount,
	round(FurtherAdvanceAmount-1000,0) as FurtherAdvanceAmount, -1 as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey, '' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0, 0   
	from #filteredTestCases test
	where (readvanceamount > 15000 and furtheradvanceamount > 15000 and FurtherLoanAmount > 15000)
	and test.ltv < 75
	and test.AccountKey not in (select AccountKey from @table)
	and valuationdate > DATEADD( yy, -2, GETDATE())
	order by test.accountKey desc

	insert into @table
	select t.AccountKey from test.AutomationFLTestCases t
	where t.AccountKey not in (Select AccountKey from @table)

	insert into test.AutomationFLTestCases
	select top 1 'ReadvanceOver80Percent' as [TestIdentifier],
	'Readvances' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, round(ReadvanceAmount-1000,0) as ReadvanceAmount,
	-1 as FurtherAdvanceAmount, -1 as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey, '' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0, 0   
	from #filteredTestCases test
	where (readvanceamount > 15000)
	and (test.RapidLTV between 80 and 95)
	and test.AccountKey not in (select AccountKey from @table)
	and valuationdate > DATEADD( yy, -2, GETDATE())
	order by test.accountKey desc
	
	insert into @table
	select t.AccountKey from test.AutomationFLTestCases t
	where t.AccountKey not in (Select AccountKey from @table)
	
	--this test case is used for the further advance application that is below the LAA 
	insert into test.AutomationFLTestCases
	select top 1 'FurtherAdvanceLessThanLAA' as [TestIdentifier],
	'All' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, round(ReadvanceAmount - 1000, 0) as ReadvanceAmount,
	round(furtherAdvanceLimit - 1000, 0) as FurtherAdvanceAmount, -1 as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey, '' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0  , 0 
	from #filteredTestCases test
	where furtherAdvanceLimit > 15000
	and (furtherAdvanceLimit <> FurtherAdvanceAmount)
	and CommittedLoanValue > 0
	and ReadvanceAmount > 15000
	and test.RapidLTV < 60
	and test.AccountKey not in (select AccountKey from @table)
	and valuationdate > DATEADD( yy, -2, GETDATE())
	order by test.accountKey desc
	
	insert into @table
	select t.AccountKey from test.AutomationFLTestCases t
	where t.AccountKey not in (Select AccountKey from @table)
	
	--we need to update some of the data to get separate cases for our readvance and further advance over LAA
	insert into test.AutomationFLTestCases
	select 'ReadvanceLessThanLAA', 'Readvances', test.AccountKey, ValuationDate,LTV,Product,ReadvanceAmount, -1, -1, 
	maxFurtherLending, SPV, OfferKey, ReadvOfferKey, 0, 0, AssignedFLAppProcUser,OrigConsultant, 
	Password, ContactOption,AssignedFLSupervisor,AssignedCreditUser,Credit,ReadvancePayments,CurrentState, ValuationAmount, CurrentBalance, 0, 0
	from test.automationfltestcases test 
	where testIdentifier='FurtherAdvanceLessThanLAA'
	--update the details
	update test.AutomationFLTestCases
	set TestGroup = 'Existing - Further Advance', ReadvanceAmount = -1
	where testIdentifier= 'FurtherAdvanceLessThanLAA'

	--this test case should get an account that qualifies for a further advance but has not had 
	--a valuation performed on the property in the last 24 months
	insert into test.AutomationFLTestCases
	select top 1 'FurtherAdvanceValRequired' as [TestIdentifier],
	'Existing - Further Advance' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, -1 as ReadvanceAmount,
	round(FurtherAdvanceAmount-1000,0) as FurtherAdvanceAmount, -1 as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance] , 0, 0 
	from #filteredTestCases test
	where valuationdate < dateadd(mm, -36, getdate())
	and FurtherAdvanceAmount > 15000
	and ReadvanceAmount = 0
	and test.AccountKey not in (select AccountKey from @table)
		order by test.accountKey desc
	
	insert into @table
	select t.AccountKey from test.AutomationFLTestCases t
	where t.AccountKey not in (Select AccountKey from @table)
	
	--this test case should get an account that qualifies for a further loan but has not had 
	--a valuation performed on the property in the last 36 months
	insert into test.AutomationFLTestCases
	select top 1 'FurtherLoanValRequired' as [TestIdentifier],
	'Existing - Further Loan' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, -1 as ReadvanceAmount,
	-1 as FurtherAdvanceAmount, round(FurtherLoanAmount-1000,0) as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0, 0  
	from #filteredTestCases test
	where valuationdate < dateadd(mm, -24, getdate())
	and FurtherAdvanceAmount = 0
	and ReadvanceAmount = 0
	and FurtherLoanAmount > 15000
	and test.AccountKey not in (select AccountKey from @table)
		order by test.LTV asc
	
	insert into @table
	select t.AccountKey from test.AutomationFLTestCases t
	where t.AccountKey not in (Select AccountKey from @table)
	
insert into test.automationfltestcases
select 
top 1 'Basel2HighRisk' as [TestIdentifier],
'Existing - Readvance' as [TestGroup],
test.AccountKey, 
test.ValuationDate, 
round(test.LTV,2) as LTV,
test.Product, 
round(test.ReadvanceAmount-1,0) as ReadvanceAmount,
-1 as FurtherAdvanceAmount, 
-1 as FurtherLoanAmount, 
round(test.maxFurtherLending,0) as maxFurtherLending, 
test.SPV, 	
'' as OfferKey,
'' as ReadvOfferKey,
'' as FAdvOfferKey, 
'' as FLOfferKey, 
'' as AssignedFLAppProcUser, 
'SAHL\FLAppProcUser3' as OrigConsultant, 
'Natal1' as Password,
case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,
'',
'',
0,
0,
'' as CurrentState,
test.[ValuationAmount],
test.[CurrentBalance], 
0,
0
from #filteredTestCases test 
left join [2am].[test].automationFLTestCases aut on test.AccountKey=aut.AccountKey
where test.readvanceamount > 10000 --and test.accountkey = @HighRisk
and test.rapidLTV < 79.5
and test.valuationdate > DATEADD( yy, -2, GETDATE())
and aut.accountKey is null

set @HighRisk = (select AccountKey from test.automationfltestcases where testIdentifier = 'Basel2HighRisk')

--we need to insert the data for this case into the AccountBaselII table
insert into AccountBaselII
(AccountKey, AccountingDate, ProcessDate, LGD, EAD, BehaviouralScore, PD, EL)
values
(@HighRisk, getdate(), getdate(),0.0910999998450279,1721945.5,671,0.144800007343292,22715) 

--MODERATE RISK TEST CASE
insert into test.automationfltestcases
select 
top 1 'Basel2ModerateRiskLowerBound' as [TestIdentifier],
'Existing - Readvance' as [TestGroup],
test.AccountKey, 
test.ValuationDate, 
round(test.LTV,2) as LTV,
test.Product, 
round(test.ReadvanceAmount-1,0) as ReadvanceAmount,
-1 as FurtherAdvanceAmount, 
-1 as FurtherLoanAmount,  
round(test.maxFurtherLending,0) as maxFurtherLending, 
test.SPV, 	
'' as OfferKey,
'' as ReadvOfferKey,
'' as FAdvOfferKey, 
'' as FLOfferKey, 
'' as AssignedFLAppProcUser, 
'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal1' as Password,
case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
'' as CurrentState,
test.[ValuationAmount],
test.[CurrentBalance], 0, 0
from #filteredTestCases test 
left join test.automationFLTestCases aut on test.AccountKey=aut.AccountKey
where test.readvanceamount > 10000
and test.rapidLTV < 79.5
and test.valuationdate > DATEADD( yy, -2, GETDATE())
and aut.accountKey is null

set @ModerateRisk = (select AccountKey from test.automationfltestcases
where testIdentifier = 'Basel2ModerateRiskLowerBound')

--we need to insert the data for this case into the AccountBaselII table
insert into AccountBaselII
(AccountKey, AccountingDate, ProcessDate, LGD, EAD, BehaviouralScore, PD, EL)
values
(@ModerateRisk, getdate(), getdate(),0.0910999998450279,945518.875,672,0.0255999993532896,2205) 

--MODERATE RISK TEST CASE
insert into test.automationfltestcases
select 
top 1 'Basel2ModerateRiskUpperBound' as [TestIdentifier],
'Existing - Readvance' as [TestGroup],
test.AccountKey, 
test.ValuationDate, 
round(test.LTV,2) as LTV,
test.Product, 
round(test.ReadvanceAmount-1,0) as ReadvanceAmount,
-1 as FurtherAdvanceAmount, 
-1 as FurtherLoanAmount,  
round(test.maxFurtherLending,0) as maxFurtherLending, 
test.SPV, 	
'' as OfferKey,
'' as ReadvOfferKey,
'' as FAdvOfferKey, 
'' as FLOfferKey, 
'' as AssignedFLAppProcUser, 
'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal1' as Password,
case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
'' as CurrentState,
test.[ValuationAmount],
test.[CurrentBalance], 0, 0
from #filteredTestCases test 
left join test.automationFLTestCases aut on test.AccountKey=aut.AccountKey
where test.readvanceamount > 10000
and test.rapidLTV < 79.5
and test.valuationdate > DATEADD( yy, -2, GETDATE())
and aut.accountKey is null

set @ModerateRisk = (select AccountKey from test.automationfltestcases where testIdentifier = 'Basel2ModerateRiskUpperBound')

--we need to insert the data for this case into the AccountBaselII table
insert into AccountBaselII
(AccountKey, AccountingDate, ProcessDate, LGD, EAD, BehaviouralScore, PD, EL)
values
(@ModerateRisk, getdate(), getdate(),0.0910999998450279,945518.875,695,0.0255999993532896,2205) 

--LOW RISK TEST CASES
insert into test.automationfltestcases
select 
top 1 'Basel2LowRisk' as [TestIdentifier],
'Existing - Readvance' as [TestGroup],
test.AccountKey, 
test.ValuationDate, 
round(test.LTV,2) as LTV,
test.Product, 
round(test.ReadvanceAmount-1,0) as ReadvanceAmount,
-1 as FurtherAdvanceAmount, 
-1 as FurtherLoanAmount, 
round(test.maxFurtherLending,0) as maxFurtherLending, 
test.SPV, 	
'' as OfferKey,
'' as ReadvOfferKey,
'' as FAdvOfferKey, 
'' as FLOfferKey, 
'' as AssignedFLAppProcUser, 
'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal1' as Password,
case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
'' as CurrentState,
test.[ValuationAmount],
test.[CurrentBalance], 0, 0
from #filteredTestCases test 
left join test.automationFLTestCases aut on test.AccountKey=aut.AccountKey
where 
test.readvanceamount > 10000
and test.rapidLTV < 79.5
and test.valuationdate > DATEADD( yy, -2, GETDATE())
and aut.accountKey is null

set @LowRisk = (select AccountKey from test.automationfltestcases where testIdentifier = 'Basel2LowRisk')

--we need to insert the data for this case into the AccountBaselII table
insert into AccountBaselII
(AccountKey, AccountingDate, ProcessDate, LGD, EAD, BehaviouralScore, PD, EL)
values
(@LowRisk, getdate(), getdate(),0.0910999998450279,520346.09375,796,0.0511999987065792,2427) 	

--RETURNING CUSTOMER DISCOUNT NOT APPLIED FOR FURTHER LENDING
insert into test.AutomationFLTestCases
	select top 1 'ReturningCustomerDiscountNotApplied' as [TestIdentifier],
	'ReturningCustomerDiscountNotApplied' as [TestGroup],
	test.AccountKey, ValuationDate, round(LTV,2) as LTV, Product, -1 as ReadvanceAmount,
	-1 as FurtherAdvanceAmount, Round(FurtherLoanAmount-1000,0) as FurtherLoanAmount, round(maxFurtherLending,0) as maxFurtherLending, SPV, 
	'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal123' as Password,
	case when row_number() over (order by test.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
	'' as CurrentState, test.[ValuationAmount], test.[CurrentBalance], 0 , 0  
	from #filteredTestCases test
	join [2AM]..AccountInformation ai on test.AccountKey=ai.AccountKey
	where (furtherloanamount > 15000 and furtherloanamount < 500000)
	and ai.AccountInformationTypeKey=12
	and test.ltv < 75 
	and test.AccountKey not in (select AccountKey from @table)
	and valuationdate > DATEADD( yy, -2, GETDATE())
	and PTI < 20
	order by test.accountKey desc
	
insert into @table
select t.AccountKey from test.AutomationFLTestCases t
where t.AccountKey not in (Select AccountKey from @table)

drop table #debt_counselling
drop table #NonPerforming
drop table #filteredTestCases
drop table #detailtypes

end

--insert super lo test cases
exec test.SuperLoFurtherLendingTestCases
exec test.ExistingFLTestCases
--set the process via automation flag for all cases except for one of each application type
update test.automationfltestcases
set processviaworkflowautomation = 1
where testidentifier not in ('ReadvanceCreate2','FurtherAdvanceCreate2','FurtherLoanCreate2')


SET ANSI_NULLS OFF
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

--exec test.createfurtherlendingtestcases