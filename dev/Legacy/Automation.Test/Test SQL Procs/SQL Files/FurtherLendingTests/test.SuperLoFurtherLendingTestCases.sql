USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.SuperLoFurtherLendingTestCases') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.SuperLoFurtherLendingTestCases
	Print 'Dropped Proc test.SuperLoFurtherLendingTestCases'
End
Go

CREATE PROCEDURE test.SuperLoFurtherLendingTestCases

AS

if object_id('tempdb..#nonperforming','U') is not null
      drop table #nonperforming
if object_id('tempdb..#debt_counselling','U') is not null
      drop table #debt_counselling
if object_id('tempdb..#temp_account_info','U') is not null
      drop table #temp_account_info
if object_id('tempdb..#CommittedBalances','U') is not null
      drop table #CommittedBalances
if object_id('tempdb..#cte_fl_details','U') is not null
      drop table #cte_fl_details        
      
----remove existing test cases
delete from test.AutomationFLTestCases
where TestIdentifier like '%SuperLoNoOptOut%' 
delete from test.AutomationFLTestCases
where TestIdentifier like '%SuperLoSPVChange%'
delete from test.AutomationFLTestCases
where TestIdentifier like '%SuperLoGreaterThan85Percent%'

--we need a debt counselling test case for exclusions
CREATE TABLE #debt_counselling (   
  generickey     INT
  )   
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
where fa.financialAdjustmentStatusKey=1 and rrr_productkey=5
) as NonPerforming

SELECT *
INTO 
#CommittedBalances
FROM (
SELECT f.accountKey, f.commitedBalance, isnull(f.HasEverMovedFromCompany1,0) as HasEverMovedFromCompany1
from DW.DWWarehousePre.Securitisation.FactAccountAttribute f
join [2am].dbo.Account a on f.accountKey = a.accountKey
where a.accountStatusKey = 1 and rrr_productkey=5
) as WarehouseData  

--we need a table variable to store what we have already used  
declare @table table (AccountKey int)  

CREATE TABLE #temp_account_info (  
accountkey INT,  
currentbalance FLOAT,  
accruedinterestmtd FLOAT,  
rrr_originationsourcekey INT,  
rrr_productkey INT,  
financialservicekey INT,  
spvkey INT,
CLV FLOAT,
Readvance FLOAT,
HasEverMovedFromCompany1 BIT   
)  
  
INSERT INTO #temp_account_info  
SELECT 
a.accountkey,
max(b.amount) as CurrentBalance, 
max(lb.MTDInterest) as AccruedInterestMtd,  
rrr_originationsourcekey, 
rrr_productkey, 
MAX(fs.financialservicekey) financialservicekey, 
MAX(a.spvkey),
max(commitedBalance) as CLV,
max(isnull((CASE WHEN commitedBalance - (b.amount + lb.MTDInterest) < 0 THEN 0.00 ELSE commitedBalance - (b.amount + lb.MTDInterest) END),0))  AS [Readvance],
HasEverMovedFromCompany1
FROM [2am].[dbo].account a
join [2am].[dbo].role r on a.accountkey=r.accountkey
join legalentity le on r.legalentitykey=le.legalentitykey 
	and len(idnumber) = 13
JOIN [2am].[dbo].financialservice fs ON a.accountkey=fs.accountkey 
	and fs.parentFinancialServiceKey is null 
	and fs.financialservicetypeKey = 1
JOIN [2am].fin.mortgageloan ml ON fs.financialservicekey=ml.financialservicekey
join [2am].fin.LoanBalance lb on ml.financialServiceKey=lb.financialServiceKey
join [2am].fin.Balance b on fs.financialServiceKey = b.financialServiceKey 
	and b.balanceTypeKey=1
join [2am].spv.Spv s on a.spvkey=s.spvkey 
	and s.spvCompanyKey = 1
join spv.spvAttribute sa on s.spvkey=sa.spvkey 
	and sa.spvAttributeTypeKey=1 
	and sa.value = 1  --allowed further lending
JOIN #CommittedBalances dw on a.accountKey=dw.accountKey  
left join #NonPerforming np on a.accountkey=np.accountkey
left join #debt_counselling dc on a.accountkey=dc.genericKey
WHERE a.accountstatuskey=1 AND a.rrr_originationsourcekey=1 
and rrr_productkey=5
and dc.genericKey is null
and np.accountkey is null 
GROUP by a.accountkey, rrr_originationsourcekey, rrr_productkey,HasEverMovedFromCompany1;
  
SELECT * INTO #cte_fl_details
FROM (  
SELECT 
temp.AccountKey,
temp.CurrentBalance,   
MaxValuation.ValDate AS [ValuationDate],  
CASE WHEN test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 = 0 THEN 0 ELSE  
temp.currentbalance/test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 END AS LTV,  
P.Description AS [Product],   
temp.Readvance  AS [Readvance], 
CASE WHEN bonds.BOND > ((CC.LTV)*test.getlatestvaluationamountbyaccountkey(temp.accountkey)) THEN
(CC.LTV)*(test.getlatestvaluationamountbyaccountkey(temp.accountkey)) - 
(temp.Readvance + temp.currentBalance + temp.accruedinterestMTD) ELSE 
bonds.BOND - (temp.Readvance + temp.currentBalance + temp.accruedinterestMTD) END AS [FurtherAdvance], 
(CC.LTV)*(test.getlatestvaluationamountbyaccountkey(temp.accountkey)) - (temp.CurrentBalance + temp.accruedinterestMTD) AS MaxFL,
CAST(s.spvkey AS char(3)) + ' - ' + s.description AS [SPV],  
CASE WHEN test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 = 0 THEN 0 ELSE  
((CASE WHEN bonds.LAA - (temp.currentbalance + temp.accruedinterestMTD) < 0 THEN 0.00 ELSE bonds.LAA - (temp.currentbalance + temp.accruedinterestMTD) END)+temp.CurrentBalance)/test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100   
END AS [RapidLTV],
CASE WHEN test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 = 0 THEN 0 ELSE  
(
(CASE WHEN bonds.BOND > ((CC.LTV)*test.getlatestvaluationamountbyaccountkey(temp.accountkey)) 
THEN
(CC.LTV)*test.getlatestvaluationamountbyaccountkey(temp.accountkey) - (temp.Readvance + temp.currentBalance + temp.accruedinterestMTD) 
ELSE 
bonds.BOND - (temp.Readvance + temp.currentBalance + temp.accruedinterestMTD) 
END
)
+temp.CurrentBalance)/test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 END
AS [FurtherAdvanceLTV],
test.GetLatestValuationAmountByAccountKey(temp.accountKey) as ValuationAmount,
HasEverMovedFromCompany1     
FROM       
#temp_account_info temp LEFT JOIN (  
SELECT accountkey FROM detailtype dt   
JOIN detail d ON dt.detailtypekey=d.detailtypekey  
WHERE dt.description like '%foreclosure%' or dt.description like '%suspended%'
or (dt.detailtypekey in (5, 9, 11, 14, 88, 99, 100, 104, 106, 117, 150, 180, 213, 214, 217, 227, 
241, 242, 251, 279, 293, 294, 296, 299, 302, 453, 454, 455, 456, 457, 459, 461, 464, 493)
)
) AS foreclosure_check ON temp.accountkey=foreclosure_check.accountkey  
JOIN product p ON temp.rrr_productkey=p.productkey     
JOIN spv.SPV s ON temp.spvkey=s.spvkey JOIN  
(  
SELECT fs.financialservicekey FSKey, SUM(bondloanagreementamount) AS LAA, SUM(bondregistrationamount) AS BOND  
FROM financialservice fs  
JOIN bondmortgageloan bml ON fs.financialservicekey=bml.financialservicekey  
JOIN bond b ON bml.bondkey=b.bondkey  
GROUP BY fs.financialservicekey  
) bonds ON temp.financialservicekey = bonds.FSKey  
INNER JOIN fin.MortgageLoan mlp ON  
temp.financialservicekey = mlp.financialservicekey JOIN (  
SELECT propertykey AS PropertyKey, MAX(valuationdate) ValDate FROM valuation   
GROUP by propertykey  
having MAX(valuationdate) > DATEADD( yy, -2, GETDATE())  
) MaxValuation ON mlp.propertykey = MaxValuation.propertykey  
LEFT JOIN offer o ON temp.accountkey = ISNULL(o.accountkey, o.reservedaccountkey) 
	AND o.offertypekey in (2,3,4) and o.offerStatusKey = 1
JOIN (
SELECT a.accountkey,CASE WHEN employmenttypekey = 5 THEN 1 ELSE employmenttypekey END AS EmploymentTypeKey, SUM(e.confirmedincome) AS confirmedincome,
ROW_NUMBER() OVER (PARTITION BY a.accountkey ORDER BY SUM(confirmedincome) DESC) AS ord
FROM [2am].[dbo].account a 
JOIN [2am].[dbo].role r ON a.accountkey=r.accountkey
JOIN [2am].[dbo].employment e ON r.legalentitykey=e.legalentitykey AND employmentstatuskey=1
WHERE rrr_productkey in (5)
GROUP BY a.accountkey,e.employmenttypekey
) AS emp_type ON temp.accountkey=emp_type.accountkey
	 AND ord=1	
LEFT JOIN (  
SELECT MAX(LTV/100) LTV,osp.ProductKey, cc.EmploymentTypeKey, osp.OriginationSourceKey    
FROM CreditCriteria cc   
JOIN CreditMatrix AS cm ON cc.CreditMatrixKey = cm.CreditMatrixKey  
JOIN OriginationSourceProductCreditMatrix ospcm ON ospcm.CreditMatrixKey = cm.CreditMatrixKey  
JOIN OriginationSourceProduct AS osp ON ospcm.OriginationSourceProductKey = osp.OriginationSourceProductKey  
WHERE cc.MortgageLoanPurposeKey = 2 --always assessed AS switch for FL   
AND cc.MaxLoanAmount >= 0   
AND cm.NewBusinessIndicator = 'Y'   
AND cc.ExceptionCriteria = 0  
GROUP BY osp.ProductKey, cc.EmploymentTypeKey, osp.OriginationSourceKey  
) AS CC ON emp_type.employmentTypeKey=CC.employmenttypekey   
AND temp.rrr_productkey=CC.productkey   
AND temp.rrr_originationsourcekey=CC.OriginationSourceKey  
WHERE  o.offerkey is null AND foreclosure_check.accountkey is null  
) as FLData

--super lo with effective LTV < 80
insert into test.AutomationFLTestCases
select top 2 'SuperLoNoOptOut'+cast(row_number() over (order by cte.accountkey) as varchar(3)) as [TestIdentifier],  
'FurtherAdvances' as [TestGroup],  
cte.AccountKey, 
cte.ValuationDate, 
round(cte.LTV,2) as LTV, 
cte.Product,
-1 as ReadvanceAmount,  
round(FurtherAdvance-1,0) as FurtherAdvance, 
-1 as FurtherLoanAmount, 
round(MaxFL,0) as maxFurtherLending, 
cte.SPV,   
'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 
'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal1' as Password,  
case when row_number() over (order by cte.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
'' as CurrentState,
cte.ValuationAmount,
cte.CurrentBalance, 0, 0       
FROM #cte_fl_details cte
left join test.automationfltestcases test on cte.accountKey = test.accountKey  
WHERE
(FurtherAdvance > 10000)
AND (FurtherAdvanceLTV < 79)
and test.accountKey is null
and HasEverMovedFromCompany1 = 0

--super lo with effective LTV between 80 and 85
insert into test.AutomationFLTestCases
select top 1 'SuperLoSPVChange'+cast(row_number() over (order by cte.accountkey) as varchar(3)) as [TestIdentifier],  
'FurtherAdvances' as [TestGroup],  
cte.AccountKey, 
cte.ValuationDate, 
round(cte.LTV,2) as LTV, 
cte.Product,
-1 as ReadvanceAmount,  
round(FurtherAdvance-1,0) as FurtherAdvance, 
-1 as FurtherLoanAmount, 
round(MaxFL,0) as maxFurtherLending, 
cte.SPV,   
'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 
'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal1' as Password,  
case when row_number() over (order by cte.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
'' as CurrentState, cte.ValuationAmount, cte.CurrentBalance, 0, 0      
FROM #cte_fl_details cte
left join test.automationfltestcases test on cte.accountKey = test.accountKey
WHERE
(FurtherAdvanceLTV > 81)
and FurtherAdvance > 0
and test.accountKey is null

insert into test.AutomationFLTestCases
select top 2 'SuperLoGreaterThan85Percent'+cast(row_number() over (order by cte.accountkey) as varchar(3)) as [TestIdentifier],  
'FurtherAdvances' as [TestGroup],  
cte.AccountKey, 
cte.ValuationDate, 
round(cte.LTV,2) as LTV, 
cte.Product,
-1 as ReadvanceAmount,  
round(FurtherAdvance-1,0) as FurtherAdvance, 
-1 as FurtherLoanAmount, 
round(MaxFL,0) as maxFurtherLending, 
cte.SPV,   
'' as OfferKey,'' as ReadvOfferKey,'' as FAdvOfferKey, '' as FLOfferKey, '' as AssignedFLAppProcUser, 
'SAHL\FLAppProcUser3' as OrigConsultant, 'Natal1' as Password,  
case when row_number() over (order by cte.accountkey) % 2 = 0 then 'Fax' else 'Email' end as ContactOption,'','',0,0,
'' as CurrentState,
cte.ValuationAmount,
cte.CurrentBalance, 0, 0       
FROM #cte_fl_details cte
left join test.automationfltestcases test on cte.accountKey = test.accountKey  
WHERE
(FurtherAdvance > 10000) AND (FurtherAdvanceLTV) > 85
and test.accountKey is null

DROP TABLE #temp_account_info
DROP TABLE #debt_counselling
DROP TABLE #nonperforming
DROP TABLE #CommittedBalances
DROP TABLE #cte_fl_details  