USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[InsertCAPTestCases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[InsertCAPTestCases]
	Print 'Dropped procedure [test].[InsertCAPTestCases]'
End
Go

/*
This procedure is used by the test team to insert a list of accounts and their 
associated CAP offers for testing purposes.
*/
CREATE PROCEDURE [test].[InsertCAPTestCases]

AS
BEGIN

IF (OBJECT_ID('test.CAPTestCases') IS NOT NULL)
	BEGIN
		DROP TABLE test.CAPTestCases
	END;

	CREATE TABLE test.CAPTestCases
		(	AccountKey INT,
			PTI FLOAT,
			LTV FLOAT,
			CurrentBalance FLOAT,
			BondAmount FLOAT,
			LoanAgreementAmount FLOAT,
			SPVKey INT,
			SPVMaxLTV INT,
			SPVMaxPTI INT,
			NewBalanceOnePerc FLOAT,
			CapQualifyOnePerc VARCHAR(25),
			NewBalanceTwoPerc FLOAT,
			CapQualifyTwoPerc VARCHAR(25),
			NewBalanceThreePerc FLOAT,
			CapQualifyThreePerc VARCHAR(25),
			CAPApplicationTypeOnePerc VARCHAR(25),
			CAPApplicationTypeTwoPerc VARCHAR(25),
			CAPApplicationTypeThreePerc VARCHAR(25),
			MonthlyInstalmentOnePerc FLOAT,
			MonthlyInstalmentTwoPerc FLOAT,
			MonthlyInstalmentThreePerc FLOAT,
			HouseholdIncome FLOAT,
			LatestValuation FLOAT,
			AccountUnderDebtCounselling BIT,
			CapTypeConfigurationKey INT,
			Premium1 FLOAT,
			Premium2 FLOAT,
			Premium3 FLOAT
		)			


if object_id('tempdb..#CommittedBalances','U') is not null
      drop table #CommittedBalances
      
declare @table table(Row int, CTCKey int, RCKey int)
declare @capTypeConfigurationKey int
declare @max_row int
declare @start int
declare @ResetConfigurationKey int

insert into @table
select row_number() over (order by CapTypeConfigurationKey), CapTypeConfigurationKey, ResetConfigurationKey 
from captypeconfiguration
where generalstatuskey=1
and (offerstartdate < getdate() and OfferEndDate > getdate())
order by 2 asc
                
select @max_row = max(row) from @table

set @start = 1

while (@start <= @max_row)

begin

	if object_id('tempdb..#ValidAccounts','U') is not null
      drop table #ValidAccounts;

	if object_id('tempdb..#captypeconfigurationdetails','U') is not null
      drop table #captypeconfigurationdetails;

	select @CapTypeConfigurationKey = CTCKey, @ResetConfigurationKey = RCKey 
	from @table where row = @start;
	
	SELECT * 
	INTO #captypeconfigurationdetails
	FROM (
	SELECT captypekey, 
	rate * 100 AS rate100, 
	premium 
	FROM   [2am]..captypeconfigurationdetail (nolock) 
	WHERE  captypeconfigurationkey = @CapTypeConfigurationKey
	AND generalstatuskey = 1
	) as CapTypeConfig
	
SELECT *
INTO 
#ValidAccounts
FROM (
select top 10000 a.accountKey, fs.payment, bal.currentBalance, bondamts.bondamt, bondamts.laamt, a.spvKey, spvm.maxLTV, spvm.maxPTI,
lbal.interestRate, lbal.remainingInstalments, commitedBalance
FROM     [2am]..ACCOUNT a (NOLOCK)
         JOIN [2am]..financialservice fs (NOLOCK) ON a.accountkey = fs.accountkey 
			and fs.parentFinancialServiceKey is null
         JOIN [2am].fin.mortgageloan ml (NOLOCK)  ON fs.financialservicekey = ml.financialservicekey 
         join [2am].fin.LoanBalance lbal (nolock) on fs.financialservicekey = lbal.financialservicekey 
         JOIN [2am].dbo.vMLFinancialServiceCurrentBalance bal (NOLOCK) on ml.financialServiceKey = bal.financialServiceKey
         JOIN DW.DWWarehousePre.Securitisation.FactAccountAttribute f (NOLOCK) on a.accountKey=f.accountKey
			and f.commitedBalance > 0
         LEFT JOIN [2am].fin.financialadjustment fa (NOLOCK) ON fs.financialservicekey = fa.financialservicekey 
              AND fa.financialadjustmentsourcekey IN (1,6) 
         LEFT JOIN [2am]..capoffer (NOLOCK) ON a.accountkey = capoffer.accountkey 
              AND capoffer.capstatuskey IN (1,2,3,5,6,7,8,9,10,11,12,13) 
         LEFT JOIN [2am].[spv].spvmandate spvm (NOLOCK) ON a.spvkey = spvm.spvkey 
         JOIN ( SELECT   fs.accountkey, 
                        Sum(bondregistrationamount)  AS bondamt, 
                        Sum(bondloanagreementamount) AS laamt 
               FROM     [2am]..financialservice fs (NOLOCK)
                        JOIN [2am]..bondmortgageloan bml (NOLOCK) ON fs.financialservicekey = bml.financialservicekey 
                        JOIN [2am]..bond (NOLOCK) ON bml.bondkey = bond.bondkey 
               GROUP BY fs.accountkey) AS bondamts 
           ON a.accountkey = bondamts.accountkey
WHERE    lbal.resetconfigurationkey = @ResetConfigurationKey
         AND a.accountstatuskey = 1  
         AND rrr_productkey NOT IN (2,3,4,11) 
         AND fa.financialadjustmentkey IS NULL 
         AND rrr_originationsourcekey = 1 
         AND capoffer.accountkey IS NULL 
         AND bal.CurrentBalance > 0 
         AND fs.accountstatuskey = 1 
         AND datediff(dd,a.opendate, fs.nextresetdate) > 93	 
		 AND lbal.remainingInstalments > 0
		 ) as AccountsValidForCap
		 
insert into test.captestcases
SELECT  top 5000 validacc.accountkey, 
                 CASE 
                   WHEN dbo.Gethouseholdincomebyaccountkey(validacc.accountkey) = 0 
                   THEN 0 
                   ELSE (validacc.payment / dbo.Gethouseholdincomebyaccountkey(validacc.accountkey)) * 100 
                 END AS pti, 
                 CASE 
                   WHEN test.Getlatestvaluationamountbyaccountkey(validacc.accountkey) = 0 
                   THEN 0 
                   ELSE (validacc.CurrentBalance / test.Getlatestvaluationamountbyaccountkey(validacc.accountkey)) * 100 
                 END AS ltv, 
                 validacc.CurrentBalance, 
                 validacc.bondamt, 
                 validacc.laamt, 
				 validacc.spvkey,
                 validacc.maxltv * 100                                AS [Max LTV], 
                 validacc.maxpti * 100                                AS [Max PTI], 
                 validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                      FROM   #captypeconfigurationdetails 
                                      WHERE  captypekey = 1) AS [New Balance 1%], 
                 CASE 
                   WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                              FROM   #captypeconfigurationdetails 
                                              WHERE  captypekey = 1)) > validacc.bondamt 
                   THEN 'DNQ' 
                   ELSE 'Qualify' 
                 END AS [Qualify 1%], 
                 validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                      FROM   #captypeconfigurationdetails WHERE  captypekey = 2) AS [New Balance 2%], 
                 CASE 
                   WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                              FROM   #captypeconfigurationdetails WHERE  captypekey = 2)) > validacc.bondamt 
                   THEN 'DNQ' 
                   ELSE 'Qualify' 
                 END AS [Qualify 2%], 
                 validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                      FROM   #captypeconfigurationdetails 
                                      WHERE  captypekey = 3) AS [New Balance 3%], 
                 CASE 
                   WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                              FROM   #captypeconfigurationdetails 
                                              WHERE  captypekey = 3)) > validacc.bondamt 
                   THEN 'DNQ' 
                   ELSE 'Qualify' 
                 END AS [Qualify 3%], 
                 CASE 
                   WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                              FROM   #captypeconfigurationdetails 
                                              WHERE  captypekey = 1)) <= commitedBalance
                   THEN 'Readv' 
				WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
												FROM   #captypeconfigurationdetails 
											WHERE  captypekey = 1)) > validacc.laamt                                          
                   THEN 'F.Adv > LAA' 
                   ELSE 'F.Adv < LAA' 
                 END AS [TYPE 1%], 
                 CASE 
                   WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                              FROM   #captypeconfigurationdetails 
                                              WHERE  captypekey = 2)) <=  commitedBalance
                   THEN 'Readv' 
				WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
											FROM   #captypeconfigurationdetails 
											WHERE  captypekey = 2)) > validacc.laamt                                          
                   THEN 'F.Adv > LAA' 
                   ELSE 'F.Adv < LAA' 
                 END AS [TYPE 2%], 
                 CASE 
                   WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
                                              FROM   #captypeconfigurationdetails 
                                              WHERE  captypekey = 3)) <=  commitedBalance
                   THEN 'Readv' 
				WHEN (validacc.CurrentBalance + (SELECT validacc.CurrentBalance * premium 
											FROM   #captypeconfigurationdetails 
											WHERE  captypekey = 3)) > validacc.laamt                                          
                   THEN 'F.Adv > LAA' 
                   ELSE 'F.Adv < LAA' 
                 END AS [TYPE 3%], 
                 Round(((validacc.CurrentBalance * (SELECT premium 
                                              FROM   #captypeconfigurationdetails 
                                              WHERE  captypekey = 1)) + validacc.CurrentBalance) * 
                                              (Power(1 + (validacc.interestrate / 12),validacc.remaininginstalments)) * 
                                              ((validacc.interestrate / 12) / 
                                              (Power(1 + (validacc.interestrate / 12),validacc.remaininginstalments) - 1)), 
                       2) AS [Instalment 1%], 
                 Round(((validacc.CurrentBalance * (SELECT premium 
                                              FROM   #captypeconfigurationdetails 
                                              WHERE  captypekey = 2)) + validacc.CurrentBalance) * 
                                              (Power(1 + (validacc.interestrate / 12),validacc.remaininginstalments)) * 
                                              ((validacc.interestrate / 12) / (Power(1 + (validacc.interestrate / 12),
                                              validacc.remaininginstalments) - 1)), 
                       2) AS [Instalment 2%], 
                 Round(((validacc.CurrentBalance * (SELECT premium 
                                              FROM   #captypeconfigurationdetails 
                                              WHERE  captypekey = 3)) + validacc.CurrentBalance) * 
                                              (Power(1 + (validacc.interestrate / 12),validacc.remaininginstalments)) * 
                                              ((validacc.interestrate / 12) / (Power(1 + (validacc.interestrate / 12),
                                              validacc.remaininginstalments) - 1)), 
                       2) AS [Instalment 3%],
		dbo.GetHouseholdIncomeByAccountKey(validacc.accountkey),
		test.Getlatestvaluationamountbyaccountkey(validacc.accountkey),
		case when isnull(dc.AccountKey,0) = 0 then 0 else 1 end,
		@CapTypeConfigurationKey,
		(SELECT validacc.currentbalance * premium 
        FROM   #captypeconfigurationdetails 
        WHERE  captypekey = 1) AS [Premium 1%],
		(SELECT validacc.currentbalance * premium 
        FROM   #captypeconfigurationdetails 
        WHERE  captypekey = 2) AS [Premium 2%],
		(SELECT validacc.currentbalance * premium 
		FROM   #captypeconfigurationdetails 
        WHERE  captypekey = 3) AS [Premium 3%]
FROM     #ValidAccounts validacc (NOLOCK)
		LEFT JOIN debtcounselling.debtCounselling dc on validacc.accountKey=dc.accountKey
			and dc.debtCounsellingStatusKey = 1	
			
	set @start = @start + 1
	
	if object_id('tempdb..#ValidAccounts','U') is not null
      drop table #ValidAccounts;

	if object_id('tempdb..#captypeconfigurationdetails','U') is not null
      drop table #captypeconfigurationdetails;
	
end



      

END


SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

