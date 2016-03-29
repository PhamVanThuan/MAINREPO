USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[CreateCAP2Offer]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[CreateCAP2Offer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].CreateCAP2Offer

GO

CREATE PROCEDURE test.CreateCAP2Offer

@accountKey int

AS

declare @CapOfferKey int
declare @BrokerKey int

SELECT @BrokerKey = BrokerKey from [2am].dbo.Broker where ADUserName = 'SAHL\ClintonS'

INSERT INTO [2am].dbo.CapOffer 
(
AccountKey, CapTypeConfigurationKey, RemainingInstallments, 
CurrentBalance, CurrentInstallment, LinkRate, CapStatusKey, OfferDate, Promotion, BrokerKey,
CapitalisationDate, ChangeDate, CapPaymentOptionKey, UserID)
SELECT distinct @accountKey, test.CapTypeConfigurationKey, lb.RemainingInstalments, 
test.CurrentBalance, fs.payment,m.Value, 1, getdate(), 0, @BrokerKey,
null, getDate(), null, 'SAHL\ClintonS'
FROM test.AutomationCAP2TestCases aut 
join test.CapTestCases test on aut.accountKey = test.AccountKey
join [2am].dbo.financialservice fs on test.accountKey = fs.AccountKey
	and fs.financialserviceTypeKey = 1
	and fs.parentFinancialServiceKey is null
join [2am].fin.mortgageLoan ml on fs.financialServiceKey = ml.financialServiceKey
join [2am].fin.loanBalance lb on ml.financialServiceKey = lb.financialServiceKey
join [2am].fin.Balance bal on ml.financialServiceKey = bal.financialServiceKey
	and bal.[BalanceTypeKey] = 1 
join [2am].dbo.rateConfiguration rc on lb.rateConfigurationKey = rc.rateConfigurationKey
join [2am].dbo.margin m on rc.marginkey = m.marginkey
where aut.accountKey = @accountKey

set @CapOfferKey = scope_identity()

--variables
DECLARE @CapTypeConfigurationDetailKey int
DECLARE @CapTypeConfigurationKey int
DECLARE @EffectiveRate float
DECLARE @Fee float
DECLARE @Payment float
DECLARE @InterestRate float

SELECT @InterestRate = interestRate
from [2am].dbo.Account a
join [2am].dbo.FinancialService fs on a.accountKey = fs.AccountKey
	and fs.parentFinancialServiceKey is null 		
	and fs.financialservicetypekey = 1
join [2am].fin.loanBalance lb on fs.financialservicekey = lb.financialservicekey
where a.accountkey = @accountKey

SELECT @CapTypeConfigurationKey = CapTypeConfigurationKey FROM [2am].test.AutomationCap2TestCases WHERE accountKey = @accountKey 

--INSERT 1% CAP OPTION
IF (SELECT CapQualifyOnePerc FROM [2am].test.AutomationCap2TestCases WHERE accountKey = @accountKey) = 'Qualify'
	BEGIN
	
		SELECT @CapTypeConfigurationDetailKey = CapTypeConfigurationDetailKey FROM [2am].dbo.CapTypeConfigurationDetail ctcd
		WHERE ctcd.CapTypeConfigurationKey = @CapTypeConfigurationKey and ctcd.capTypeKey = 1
		
		SET  @EffectiveRate =  @InterestRate + 0.01 		
				
		SELECT @Fee = Premium1 from [2am].test.automationCap2TestCases where accountkey = @AccountKey
		
		SELECT @Payment = MonthlyInstalmentOnePerc from [2am].test.automationCap2TestCases where accountKey = @AccountKey
		
		INSERT INTO [2am].dbo.CapOfferDetail
		(CapOfferKey, CapTypeConfigurationDetailKey, EffectiveRate, Payment, Fee, CapStatusKey, AcceptanceDate, CapNTUReasonKey, CapNTUReasonDate,
		ChangeDate, UserID)
		SELECT
		 @CapOfferKey, @CapTypeConfigurationDetailKey, @EffectiveRate, @Payment, @Fee, 1, null, null, null, getdate(), 'SAHL\ClintonS'
	END

--INSERT 2% CAP OPTION
IF (SELECT CapQualifyTwoPerc FROM [2am].test.AutomationCap2TestCases WHERE accountKey = @accountKey) = 'Qualify'
	BEGIN
	
		SELECT @CapTypeConfigurationDetailKey = CapTypeConfigurationDetailKey FROM [2am].dbo.CapTypeConfigurationDetail ctcd
		WHERE ctcd.CapTypeConfigurationKey = @CapTypeConfigurationKey and ctcd.capTypeKey = 2
		
		SET  @EffectiveRate =  @InterestRate +  0.02 
						
		SELECT @Fee = Premium2 from [2am].test.automationCap2TestCases where accountkey = @AccountKey
		
		SELECT @Payment = MonthlyInstalmentTwoPerc from [2am].test.automationCap2TestCases where accountKey = @AccountKey
		
		INSERT INTO [2am].dbo.CapOfferDetail
		(CapOfferKey, CapTypeConfigurationDetailKey, EffectiveRate, Payment, Fee, CapStatusKey, AcceptanceDate, CapNTUReasonKey, CapNTUReasonDate,
		ChangeDate, UserID)
		SELECT
		 @CapOfferKey, @CapTypeConfigurationDetailKey, @EffectiveRate, @Payment, @Fee, 1, null, null, null, getdate(), 'SAHL\ClintonS'
	END

--INSERT 3% CAP OPTION
IF (SELECT CapQualifyThreePerc FROM [2am].test.AutomationCap2TestCases WHERE accountKey = @accountKey) = 'Qualify'
	BEGIN
	
		SELECT @CapTypeConfigurationDetailKey = CapTypeConfigurationDetailKey FROM [2am].dbo.CapTypeConfigurationDetail ctcd
		WHERE ctcd.CapTypeConfigurationKey = @CapTypeConfigurationKey and ctcd.capTypeKey = 3
		
		SET  @EffectiveRate =  @InterestRate +  0.03 
				
		SELECT @Fee = Premium3 from [2am].test.automationCap2TestCases where accountkey = @AccountKey
		
		SELECT @Payment = MonthlyInstalmentThreePerc from [2am].test.automationCap2TestCases where accountKey = @AccountKey
		
		INSERT INTO [2am].dbo.CapOfferDetail
		(CapOfferKey, CapTypeConfigurationDetailKey, EffectiveRate, Payment, Fee, CapStatusKey, AcceptanceDate, CapNTUReasonKey, CapNTUReasonDate,
		ChangeDate, UserID)
		SELECT
		 @CapOfferKey, @CapTypeConfigurationDetailKey, @EffectiveRate, @Payment, @Fee, 1, null, null, null, getdate(), 'SAHL\ClintonS'
	END
	
	
UPDATE test.AutomationCap2TestCases SET CapOfferKey = @CapOfferKey where AccountKey = @accountKey










