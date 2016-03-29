USE [2AM]
GO

IF OBJECT_ID(N'test.GetLatestValuationAmountByAccountKey', N'FN') IS NULL

BEGIN

declare @sql nvarchar(max);

set @sql = 'CREATE FUNCTION test.GetLatestValuationAmountByAccountKey(
	@Accountkey int
)
RETURNS float
AS
BEGIN
	DECLARE @TotalValuationAmount float;

	SELECT @TotalValuationAmount = ISNULL(SUM(ISNULL(ValuationAmount, 0)), 0)
	FROM ( 	SELECT V.PropertyKey,V.ValuationAmount
			FROM [2AM].[dbo].[FinancialService] FS (nolock)
			JOIN [2AM].[fin].[MortgageLoan] ML (nolock)
			on FS.FinancialServiceKey = ML.FinancialServiceKey
			JOIN [2AM].[dbo].[Valuation] V (nolock)
			ON 	ML.PropertyKey = V.PropertyKey and isActive=1
			WHERE FS.AccountKey = @AccountKey
			AND FS.AccountStatusKey IN (1, 5)
			GROUP BY V.PropertyKey, V.ValuationAmount) AS T;
	RETURN (@TotalValuationAmount);
END'

EXECUTE sp_executesql @sql

END




