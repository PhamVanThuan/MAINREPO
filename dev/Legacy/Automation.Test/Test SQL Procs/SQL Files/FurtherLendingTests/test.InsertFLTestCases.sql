USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[InsertFLTestCases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[InsertFLTestCases] 
	Print 'Dropped procedure [test].[InsertFLTestCases]'
End

GO

CREATE PROCEDURE [test].[InsertFLTestCases]

@no_of_rows int

AS
BEGIN

--remove existing records
TRUNCATE TABLE test.FurtherLendingTestCases 

select distinct a.accountKey 
into #AccountsThatHaveBankAccounts
from [2am].dbo.Account a 
join [2am].dbo.Role r on a.accountKey = r.accountKey
join [2am].dbo.LegalEntityBankAccount leba on r.legalEntityKey =leba.legalEntityKey
	and leba.generalStatusKey=1
where AccountStatusKey = 1 and rrr_OriginationSourceKey=1

SELECT MAX(LTV/100) LTV,osp.ProductKey, cc.EmploymentTypeKey, osp.OriginationSourceKey
INTO #CreditCriteria  
FROM [2am].[dbo].CreditCriteria cc 
JOIN [2am].[dbo].CreditMatrix AS cm ON cc.CreditMatrixKey = cm.CreditMatrixKey
JOIN [2am].[dbo].OriginationSourceProductCreditMatrix ospcm ON ospcm.CreditMatrixKey = cm.CreditMatrixKey
JOIN [2am].[dbo].OriginationSourceProduct AS osp ON ospcm.OriginationSourceProductKey = osp.OriginationSourceProductKey
WHERE cc.MortgageLoanPurposeKey = 2 --always assessed AS switch for FL 
AND cc.MaxLoanAmount >= 0 
AND cm.NewBusinessIndicator = 'Y' 
AND cc.ExceptionCriteria = 0
GROUP BY osp.ProductKey, cc.EmploymentTypeKey, osp.OriginationSourceKey

SELECT 
a.accountkey,
rrr_originationsourcekey, 
rrr_productkey, 
MAX(fs_1.financialservicekey) financialservicekey, 
MAX(a.spvkey) as SPVKey,
max(isnull(commitedBalance,0)) as CLV,
SUM(b.Amount) as CurrentBalance,
SUM(lb.MTDInterest) as accruedinterestmtd,
max(fs.payment) as Payment,
0 as Readvance
INTO #temp_account_info
FROM [2am].dbo.account a
left JOIN DW.DWWarehousePre.Securitisation.FactAccountAttribute dw on a.accountKey=dw.accountKey
JOIN [2am].dbo.financialservice fs ON a.accountkey=fs.accountkey 
	and fs.parentFinancialServiceKey is null
JOIN #AccountsThatHaveBankAccounts on a.accountKey = #AccountsThatHaveBankAccounts.AccountKey
JOIN [2am].[fin].mortgageloan ml ON fs.financialservicekey=ml.financialservicekey
join [2am].[fin].loanBalance lb on ml.financialServiceKey=lb.financialServiceKey
join [2am].[fin].Balance b on ml.financialServiceKey=b.financialServiceKey 
	and b.balanceTypeKey=1
join [2am].spv.SPV s on a.spvkey=s.spvkey
join spv.spvAttribute sa on s.spvkey=sa.spvkey 
	and sa.spvAttributeTypeKey=1 
	and sa.value = 1  --allowed further lending
LEFT JOIN financialservice fs_1 ON fs.financialservicekey=fs_1.financialservicekey 
	AND fs_1.financialservicetypekey=1
LEFT JOIN [2am].dbo.Detail dt on a.accountKey = dt.accountKey
	and dt.detailTypeKey in (186,258,259,260, 261,262,263,264,265,266,267,335,381,422,469,540,541,575,576,577,578,580,585,598)
WHERE a.accountstatuskey=1 
AND a.rrr_originationsourcekey=1 
and lb.RemainingInstalments > 0
and dt.AccountKey is null
GROUP by a.accountkey, rrr_originationsourcekey, rrr_productkey

UPDATE #temp_account_info
set Readvance = isnull((CASE WHEN CLV - (CurrentBalance + accruedinterestmtd) < 0 THEN 0.00 ELSE CLV - (CurrentBalance + accruedinterestmtd) END),0);

SELECT a.accountkey, CASE WHEN employmenttypekey = 5 THEN 1 ELSE employmenttypekey END AS EmploymentTypeKey, SUM(e.confirmedBasicIncome) AS confirmedincome,
ROW_NUMBER() OVER (PARTITION BY a.accountkey ORDER BY SUM(confirmedBasicIncome) DESC) AS ord, SUM(e.confirmedIncome) as householdIncomeContribution
INTO #EmploymentType
FROM #temp_account_info temp
JOIN [2am].[dbo].account a on temp.accountKey = a.accountKey
JOIN [2am].[dbo].role r ON a.accountkey=r.accountkey
JOIN [2am].[dbo].employment e ON r.legalentitykey=e.legalentitykey 
	AND employmentstatuskey=1
WHERE a.rrr_productkey in (1,2,5,6,9,11)
GROUP BY a.accountkey, e.employmenttypekey;


WITH cte_fl_details AS (
SELECT TOP (@no_of_rows)
temp.AccountKey, 
v.ValuationDate AS [ValuationDate],
test.getlatestvaluationamountbyaccountkey(temp.accountkey) as [ValuationAmount],
CASE WHEN test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 = 0 THEN 0 ELSE 
temp.currentbalance/test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 END AS [LTV],
P.Description AS [Product], 
temp.Readvance  AS [Readvance], 
CASE WHEN bonds.BOND > ((CC.LTV)*(test.getlatestvaluationamountbyaccountkey(temp.accountkey))) THEN
(CC.LTV)*(test.getlatestvaluationamountbyaccountkey(temp.accountkey)) - 
(temp.Readvance + temp.currentBalance + temp.accruedinterestMTD) ELSE 
bonds.BOND - (temp.Readvance + temp.currentBalance + temp.accruedinterestMTD) END AS [FurtherAdvance], 
(CC.LTV)*(test.getlatestvaluationamountbyaccountkey(temp.accountkey)) - (temp.CurrentBalance+temp.accruedinterestMTD) AS [MaxFL],
CAST(s.spvkey AS char(3)) + ' - ' + s.description AS [SPV],
CASE WHEN test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 = 0 THEN 0 ELSE
(temp.Readvance+temp.CurrentBalance)/test.getlatestvaluationamountbyaccountkey(temp.accountkey)*100 END AS [RapidLTV], 
CC.LTV [OrigSPV_LTV], 
temp.CLV, 
bonds.BOND, 
bonds.LAA,
case when temp.currentBalance > temp.CLV then bonds.LAA - (temp.currentBalance+temp.accruedInterestMTD) else bonds.LAA - temp.CLV end as [FAdvLimit],
case when emp_type.confirmedIncome = 0 then 0 else (temp.payment/emp_type.confirmedincome)*100 end as [PTI],
temp.currentBalance as [CurrentBalance],
isnull(HouseholdIncome.Value,0) as [HouseholdIncome]
FROM     
#temp_account_info temp 
JOIN [2am].[dbo].product p ON temp.rrr_productkey=p.productkey 
JOIN [2am].spv.SPV s ON temp.spvkey=s.spvkey 
JOIN (
SELECT fs.financialservicekey FSKey, SUM(bondloanagreementamount) AS LAA, SUM(bondregistrationamount) AS BOND
FROM [2am].[dbo].financialservice fs
JOIN [2am].[dbo].bondmortgageloan bml ON fs.financialservicekey=bml.financialservicekey
JOIN [2am].[dbo].bond b ON bml.bondkey=b.bondkey
GROUP BY fs.financialservicekey
) bonds ON temp.financialservicekey = bonds.FSKey
INNER JOIN fin.MortgageLoan ml ON temp.financialservicekey = ml.financialservicekey 
LEFT JOIN [2am].[dbo].valuation v on ml.propertyKey=v.PropertyKey 
	and isActive = 1 
LEFT JOIN [2am].[dbo].offer o ON temp.accountkey = ISNULL(o.accountkey, o.reservedaccountkey) 
	AND o.offertypekey not in (6,7,8)
JOIN #EmploymentType AS emp_type ON temp.accountkey=emp_type.accountkey 
	AND ord=1
JOIN (SELECT Sum(householdIncomeContribution) as Value, AccountKey from #EmploymentType group by AccountKey) as HouseholdIncome on temp.accountKey=HouseholdIncome.accountKey
LEFT JOIN #CreditCriteria CC ON emp_type.employmenttypekey=CC.employmenttypekey 
	AND temp.rrr_productkey=CC.productkey 
	AND temp.rrr_originationsourcekey=CC.OriginationSourceKey
WHERE o.offerkey is null
)

INSERT INTO test.FurtherLendingTestCases 
(
AccountKey,
ValuationDate,
ValuationAmount,
LTV,
Product,
ReadvanceAmount,
FurtherAdvanceAmount,
FurtherLoanAmount,
MaxFurtherLending,
SPV,
RapidLTV,
CommittedLoanValue,
BondValue,
LoanAgreementAmount,
FurtherAdvanceLimit,
PTI ,
UnderDebtCounselling,
CurrentBalance,
HouseholdIncome
)
SELECT 
ISNULL(AccountKey, 0),
ISNULL(ValuationDate, getdate()), 
ISNULL(ValuationAmount, 0),
ISNULL(LTV, 0),
ISNULL(Product, '-'),
CASE WHEN Readvance < 0 THEN 0.00 ELSE Readvance END AS Readvance, 
CASE WHEN FurtherAdvance < 0 THEN 0.00 ELSE FurtherAdvance END AS FurtherAdvance,
0 AS [FurtherLoan],
CASE WHEN MaxFL < 0 THEN 0.00 ELSE MaxFL END AS MaxFL,
ISNULL(SPV, '-'),
ISNULL(RapidLTV, 0.00),
ISNULL(CLV, 0.00),
ISNULL(Bond, 0.00),
ISNULL(LAA, 0.00),
ISNULL(FAdvLimit, 0.00),
ISNULL(PTI, 0.00),
dbo.[fIsAccountUnderDebtCounselling](AccountKey),
CurrentBalance,
HouseholdIncome
FROM cte_fl_details
WHERE LTV <> 0

UPDATE test.FurtherLendingTestCases 
SET FurtherLoanAmount = 
CASE WHEN (MaxFurtherLending - ReadvanceAmount - FurtherAdvanceAmount) < 0.00 
THEN 
	0.00 
ELSE 
	MaxFurtherLending - ReadvanceAmount - FurtherAdvanceAmount 
END

DROP TABLE #temp_account_info
DROP TABLE #CreditCriteria
DROP TABLE #employmentType
DROP TABLE #AccountsThatHaveBankAccounts
END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

