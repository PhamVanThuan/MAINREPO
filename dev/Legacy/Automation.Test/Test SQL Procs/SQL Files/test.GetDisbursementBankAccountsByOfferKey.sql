USE [2AM]
GO

/****** Object:  StoredProcedure [test].[GetDisbursementBankAccountsByOfferKey]    Script Date: 07/15/2010 15:20:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.GetDisbursementBankAccountsByOfferKey') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.GetDisbursementBankAccountsByOfferKey
	Print 'Dropped procedure test.GetDisbursementBankAccountsByOfferKey'
End
Go


CREATE PROCEDURE test.GetDisbursementBankAccountsByOfferKey

@OfferKey int

AS

BEGIN

	select LTRIM(RTRIM(bank.ACBBankDescription))+' - '+LTRIM(RTRIM(branch.ACBBranchCode))+' - '+LTRIM(rtrim(branch.ACBBranchDescription))
	+' - '+LTRIM(RTRIM(acbtype.ACBTypeDescription))+
	' - '+LTRIM(RTRIM(ba.AccountNumber))+' - '+LTRIM(RTRIM(ba.AccountName)) as [BankDetails], 'Client' as [AccountType] 
	from [2am].dbo.Offer o
	join [2am].dbo.OfferRole ofr on o.offerkey=ofr.offerkey
	join [2am].dbo.LegalEntity le on ofr.legalentitykey=le.legalentitykey
	join [2am].dbo.LegalEntityBankAccount leba on le.legalentitykey=leba.legalentitykey and leba.generalstatuskey=1
	join [2am].dbo.BankAccount ba on leba.bankaccountkey=ba.bankaccountkey
	join [2am].dbo.ACBBranch branch on ba.ACBBranchCode=branch.ACBBranchCode
	join [2am].dbo.ACBBank bank on branch.ACBBankCode=bank.ACBBankCode
	join [2am].dbo.ACBType acbtype on ba.ACBTypeNumber=acbtype.ACBTypeNumber
	where offerroletypekey in (8,10,11,12)
	and o.offerkey=@OfferKey
	union all
	select LTRIM(RTRIM(bank.ACBBankDescription))+' - '+LTRIM(RTRIM(branch.ACBBranchCode))+' - '+LTRIM(RTRIM(branch.ACBBranchDescription))+' - '+acbtype.ACBTypeDescription+
	' - '+LTRIM(RTRIM(ba.AccountNumber))+' - '+LTRIM(RTRIM(ba.AccountName)) as [BankDetails], 'SAHL Valuation' as AccountType 
	from [2am].dbo.BankAccount ba
	join [2am].dbo.ACBBranch branch on ba.ACBBranchCode=branch.ACBBranchCode
	join [2am].dbo.ACBBank bank on branch.ACBBankCode=bank.ACBBankCode
	join [2am].dbo.ACBType acbtype on ba.ACBTypeNumber=acbtype.ACBTypeNumber
	where ba.BankAccountKey = (Select ControlNumeric from [2am].dbo.Control
	where ControlDescription = 'SAHLValuationBankAccount')
	
END



GO


