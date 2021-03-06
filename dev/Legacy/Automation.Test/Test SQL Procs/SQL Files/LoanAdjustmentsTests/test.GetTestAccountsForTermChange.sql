USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[GetTestAccountsForTermChange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[GetTestAccountsForTermChange] 
	Print 'Dropped procedure [test].[GetTestAccountsForTermChange]'
End
Go

CREATE PROCEDURE [test].[GetTestAccountsForTermChange] 

AS
BEGIN

--CREATE TABLE #temp ( 
--  generickey     INT, 
--  sdsdg_in_3851  INT, 
--  count_in       INT, 
--  sdsdg_out_3852 INT, 
--  count_out      INT) 
----this will retrieve the debt counselling in/out transitions
--INSERT INTO #temp 
--SELECT   stc.generickey, 
--         stc.[StageDefinitionStageDefinitionGroupKey], 
--         Count(generickey) AS [Count_In], 
--         Max(Isnull(outs.[sdsdgkey],3852)), 
--         Max(Isnull(outs.[Count_Out],0)) 
--FROM     stagetransitioncomposite stc 
--         LEFT JOIN (SELECT   generickey                               AS gkey, 
--                             [StageDefinitionStageDefinitionGroupKey] AS sdsdgkey, 
--                             Count(generickey)                        AS [Count_Out] 
--                    FROM     stagetransitioncomposite stc_1 
--                    WHERE    stc_1.[StageDefinitionStageDefinitionGroupKey] IN (3852) 
--                    GROUP BY stc_1.[StageDefinitionStageDefinitionGroupKey], 
--                             stc_1.generickey) AS outs 
--           ON stc.generickey = outs.gkey 
--WHERE    stc.[StageDefinitionStageDefinitionGroupKey] IN (3851) 
--GROUP BY stc.[StageDefinitionStageDefinitionGroupKey], 
--         generickey 
--ORDER BY stc.generickey, 
--         stc.[StageDefinitionStageDefinitionGroupKey]; 

WITH accounts 
     AS (SELECT top 5000 a.[AccountKey], 
                ml.[FinancialServiceKey], 
                lb.[Term], 
                spvm.[SPVKey], 
                lb.[RemainingInstalments], 
                age.[AgeAtRepayment], 
                spa.[SPVattributeTypekey], 
                spvm.[SPVMaxTerm], 
                spv.[parentspvkey] AS [ParentSPV],
				p.Description AS [Product],
				CASE ISNULL(fa.financialadjustmentsourceKey ,0)
				WHEN 0 THEN 'None'
				WHEN 2 THEN 'Super Lo'
				WHEN 5 THEN 'Interest Only'
				END AS [RateOverride] 
         FROM   [Account] a 
				JOIN [Product] P 
				  ON a.RRR_ProductKey = P.ProductKey
                JOIN [FinancialService] fs 
                  ON fs.[AccountKey] = a.[AccountKey]
                JOIN fin.[MortgageLoan] ml 
                  ON ml.[FinancialServiceKey] = fs.[FinancialServiceKey] 
                JOIN fin.[LoanBalance] lb
                  ON lb.[FinancialServiceKey] = ml.[FinancialServiceKey]
                JOIN (SELECT   a.accountkey, 
                               Max(Datediff(yy,le.[DateOfBirth],Dateadd(mm,lb.[Term],a.[OpenDate]))) AS ageatrepayment 
                      FROM     [Role] r 
                               JOIN [LegalEntity] le 
                                 ON r.[LegalEntityKey] = le.[LegalEntityKey] 
                               JOIN [Account] a 
                                 ON r.[AccountKey] = a.[AccountKey] 
                                    AND rrr_productkey IN (1,2,5,9) 
                               JOIN [FinancialService] fs 
                                 ON fs.[AccountKey] = a.[AccountKey] 
                               JOIN fin.[MortgageLoan] ml 
                                 ON ml.[FinancialServiceKey] = fs.[FinancialServiceKey] 
                               JOIN fin.[LoanBalance] lb
                                 ON lb.[FinancialServiceKey] = ml.[FinancialServiceKey]
                                 
                      GROUP BY a.accountkey) AS age 
                  ON a.accountkey = age.accountkey 
                JOIN spv.[SPV] spv
                  ON a.[SPVKey] = spv.[SPVKey] 
                JOIN [SPVMandate] spvm 
                  ON spvm.[SPVKey] = spv.[SPVKey] 
                JOIN spv.[SPVattributes] spa
				  ON  spa.[SPVKey]=spvm.[SPVKey]
				LEFT JOIN fin.[FinancialAdjustment] fa 
				  ON fs.[FinancialServiceKey]=fa.[FinancialServiceKey] and fa.financialadjustmentsourceKey in (4,6)
         WHERE  fs.[AccountStatusKey] = 1 
         AND spa.[SPVattributeTypekey] = 2 
         AND [SPVMaxTerm] < 360)
	
SELECT a.accountkey												  AS [Account],
	   a.Product												  AS [Product],
	   a.RateOverride											  AS [Rate Override],	
       360 - (term - remaininginstalments)					  AS [SAHL Max Term], 
       spvmaxterm - (term - remaininginstalments)				  AS [SPV Max Term], 
       a.term													  AS [Initial Term], 
       remaininginstalments                                       AS [Remaining Term], 
       a.spvkey                                                   AS [Loan SPV], 
       ageatrepayment                                             AS [Age at Repayment], 
       spat.description                                    AS [SPV Allows Term Change], 
       spvmaxterm                                                 AS [SPV Max Term], 
       a.parentspv                                                AS [Parent SPV], 
       scop.[SpvKey]                                              AS [New SPV] 
FROM   accounts a 
--       JOIN #temp t 
--         ON a.accountkey = t.generickey AND t.count_in > t.count_out
       JOIN spv.spv sp 
         ON a.spvkey = sp.spvkey 
       JOIN spv.spvcompanyoriginatingspv scop 
         ON sp.spvcompanykey = scop.[SPVCompanyKey]
       JOIN spv.spvattributes spa
         ON spa.spvkey = sp.spvkey
       JOIN spv.spvattributetypes spat
         ON spat.spvattributetypekey=spa.spvattributetypekey
--WHERE  (t.generickey IS NOT NULL) 
--UNION ALL 
--SELECT a.accountkey												  AS [Account],
--	   a.Product												  AS [Product],
--	   a.RateOverride											  AS [Rate Override],	
--       360 - (initialinstallments - remaininginstallments)        AS [SAHL Max Term], 
--       spvmaxterm - (initialinstallments - remaininginstallments) AS [SPV Max Term], 
--       a.initialinstallments                                      AS [Initial Term], 
--       remaininginstallments                                      AS [Remaining Term], 
--       a.spvkey                                                   AS [Loan SPV], 
--       ageatrepayment                                             AS [Age at Repayment], 
--       a.allowtermchange                                          AS [SPV Allows Term Change], 
--       spvmaxterm                                                 AS [SPV Max Term], 
--       a.parentspv                                                AS [Parent SPV], 
--       'Foreclosure/Defending'                                    AS [Type], 
--       scop.[SpvKey]                                              AS [New SPV] 
--FROM   accounts a 
--       LEFT JOIN detail d 
--         ON a.accountkey = d.accountkey AND detailtypekey IN (186,454)  
--       JOIN spv sp 
--         ON a.parentspv = sp.spvkey 
--       JOIN dbo.spvcompanyoriginatingspv scop 
--         ON sp.company = scop.[SPVCompanyKey] 
--WHERE  (d.accountkey IS NOT NULL) 
--ORDER BY a.accountkey 

--DROP TABLE #temp

	END
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

