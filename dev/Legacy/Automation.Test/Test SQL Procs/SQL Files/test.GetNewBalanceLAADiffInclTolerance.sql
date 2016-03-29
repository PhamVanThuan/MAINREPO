USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetNewBalanceLAADiffInclTolerance') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetNewBalanceLAADiffInclTolerance
	Print 'Dropped Proc test.GetNewBalanceLAADiffInclTolerance'
End
Go

CREATE PROCEDURE test.GetNewBalanceLAADiffInclTolerance

@OfferKey int

AS

begin

	declare @tolerance float
	select @tolerance = 1+(controlnumeric/100) from control where controldescription = 'Readvance to Loan Agreement Tolerance'

	select sum(BondLoanAgreementAmount) as LAA, sum(BondRegistrationAmount) as RegAmt, 
	round((max(bal.Amount) + max(oivl.LoanAgreementAmount)) - (sum(BondLoanAgreementAmount))*@tolerance,2) as Diff
	from 
	[2am].dbo.Offer o (nolock)
	join [2am].dbo.OfferInformation oi (nolock) on oi.OfferInformationKey=(select max(OfferInformationKey) from OfferInformation where offerkey=o.offerkey)
	join [2am].dbo.OfferInformationVariableLoan oivl (nolock) on oi.OfferInformationKey=oivl.OfferInformationKey
	join [2am].dbo.Account a (nolock) on o.ReservedAccountKey=a.AccountKey
	join [2am].dbo.FinancialService fs (nolock) on a.AccountKey=fs.AccountKey
	join [2am].fin.MortgageLoan ml (nolock) 
			on fs.FinancialServiceKey=ml.FinancialServiceKey
			join fin.balance bal (nolock) 
				on ml.financialservicekey=bal.financialservicekey
	join [2am].dbo.BondMortgageLoan bml (nolock) on fs.FinancialServiceKey=bml.FinancialServiceKey
	join [2am].dbo.Bond b (nolock) on bml.BondKey=b.BondKey
	where o.OfferKey=@OfferKey
	
end