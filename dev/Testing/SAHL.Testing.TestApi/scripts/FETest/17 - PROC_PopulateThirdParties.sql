use [FeTest]
go

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'PopulateThirdParties')
	DROP PROCEDURE dbo.PopulateThirdParties
GO

CREATE PROCEDURE dbo.PopulateThirdParties

AS

BEGIN

	IF(EXISTS(SELECT 1 FROM FETest.dbo.ThirdParties))
	BEGIN
		truncate table FETest.dbo.ThirdParties
	END

	insert into FETest.dbo.ThirdParties (Id, ThirdPartyKey, LegalEntityKey, TradingName, GeneralStatusKey,
		GenericKey, GenericKeyTypeKey, GenericKeyTypeDescription, HasBankAccount,
		PaymentEmailAddress, BankAccountKey, BankName, BranchCode, BranchName, AccountName, AccountNumber, AccountType)
	select tp.Id, tp.ThirdPartyKey, tp.LegalEntityKey, le.TradingName, tp.GeneralStatusKey, 
		tp.GenericKey, tp.GenericKeyTypeKey, gkt.Description as GenericKeyTypeDescription, case when ba.BankAccountKey is not null then 1 else 0 end,
		tppba.EmailAddress, ba.BankAccountKey, b.ACBBankDescription as BankName, ba.ACBBranchCode as BranchCode,
		br. ACBBranchDescription as BranchName, ba.AccountName, ba.AccountNumber, at.ACBTypeDescription as AccountType
	from [2AM].dbo.ThirdParty tp
		join [2AM].dbo.ThirdPartyType tpt on tp.ThirdPartyTypeKey = tpt.ThirdPartyTypeKey
		join [2AM].dbo.GenericKeyType gkt on tp.GenericKeyTypeKey = gkt.GenericKeyTypeKey
		join [2AM].dbo.LegalEntity le on tp.LegalEntityKey = le.LegalEntityKey
		left join [2AM].dbo.ThirdPartyPaymentBankAccount tppba
			join [2am].dbo.BankAccount ba on tppba.BankAccountKey = ba.BankAccountKey
			join [2AM].dbo.ACBType at on ba.ACBTypeNumber = at.ACBTypeNumber
			join [2AM].dbo.ACBBranch br on ba.ACBBranchCode = br.ACBBranchCode
			join [2AM].dbo.ACBBank b on br.ACBBankCode = b.ACBBankCode
		on tp.ThirdPartyKey = tppba.ThirdPartyKey

	update tp 
	set Contact = v.ValuatorContact
	from FETest.dbo.ThirdParties tp
		join [2AM].dbo.Valuator v on tp.GenericKey = v.ValuatorKey and tp.GenericKeyTypeKey = 59

	update tp 
	set Contact = a.AttorneyContact
	from FETest.dbo.ThirdParties tp
		join [2AM].dbo.Attorney a on tp.GenericKey = a.AttorneyKey and tp.GenericKeyTypeKey = 35

END