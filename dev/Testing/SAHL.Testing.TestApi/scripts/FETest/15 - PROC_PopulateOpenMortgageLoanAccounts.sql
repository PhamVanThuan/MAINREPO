use [FETest]

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateOpenMortgageLoanAccounts')
	DROP PROCEDURE dbo.PopulateOpenMortgageLoanAccounts
GO

CREATE PROCEDURE dbo.PopulateOpenMortgageLoanAccounts

AS

BEGIN

insert into FETest.dbo.OpenMortgageLoanAccounts
select a.AccountKey, a.RRR_ProductKey, 0
from [2AM].dbo.Account a
join [2AM].dbo.FinancialService fs on a.AccountKey = fs.AccountKey
where a.AccountStatusKey = 1
and fs.FinancialServiceTypeKey in (1,2)

Update a set HasThirdPartyInvoice = 1 
from FETest.dbo.OpenMortgageLoanAccounts a
join [2am].dbo.ThirdPartyInvoice t on a.AccountKey = t.AccountKey

End