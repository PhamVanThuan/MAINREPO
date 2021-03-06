USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[ConvertPersonalLoanLeadToApplication]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[ConvertPersonalLoanLeadToApplication]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].ConvertPersonalLoanLeadToApplication

GO
CREATE PROCEDURE test.ConvertPersonalLoanLeadToApplication  
  
@offerKey int,  
@hasSAHLLife bit = 1,
@hasExternalLife bit = 0
  
AS  
  
DECLARE @monthlyInstalment FLOAT  
DECLARE @ReservedAccountKey INT  
DECLARE @loanAmount FLOAT  
DECLARE @term INT  
DECLARE @CreditCriteriaUnsecuredLendingKey INT  
DECLARE @marginKey INT  
DECLARE @marginRate FLOAT  
DECLARE @marketRate FLOAT  
DECLARE @totalRate FLOAT  
DECLARE @lifePremium FLOAT  
DECLARE @lifeMultiplier FLOAT  
DECLARE @serviceFee FLOAT  
DECLARE @initFee FLOAT  
DECLARE @feesTotal FLOAT  
DECLARE @offerInformationKey INT  
DECLARE @externallifepolicykey INT
--set values  
SET @loanAmount = 0  
SET @term = 30  
  
WHILE @loanAmount < 10000  
BEGIN  
	SELECT @loanAmount = ROUND(RAND()*30000,2)  
END 
  
IF (@offerKey > 0)  
BEGIN  
 SELECT @CreditCriteriaUnsecuredLendingKey = CreditCriteriaUnsecuredLendingKey, @marginKey = marginKey   
 FROM [2am].dbo.creditcriteriaunsecuredlending  
 WHERE (@loanAmount >=  minLoanAmount and  @loanAmount <= maxLoanAmount) and term = @term  
  
 SELECT @marginRate = value FROM [2am].dbo.margin WHERE marginkey = @marginKey  
 SELECT @marketRate = value FROM [2am].dbo.marketRate WHERE marketRateKey = 6  
 SELECT @lifeMultiplier = controlNumeric FROM [2am].dbo.[control] WHERE controlDescription = 'PersonalLoanCreditLifePremium'  
 SELECT @serviceFee = controlNumeric FROM [2am].dbo.[control] WHERE controlDescription = 'PersonalLoanMonthlyFee'  
 SELECT @initFee = controlNumeric FROM [2am].dbo.[control] WHERE controlDescription = 'PersonalLoanInitiationFee'  
  
 SET @totalRate = @marginRate + @marketRate  
    
 SET @MonthlyInstalment = (@loanAmount+@initFee) *  (POWER(1 + ((@totalRate) / 12),@Term)) *   
(((@totalRate) / 12) / (POWER(1 + ((@totalRate) / 12), @Term) - 1))  
 
 
 SET @lifePremium = 0
 IF (@hasSAHLLife = 1) 
	BEGIN   
		SET @lifePremium = @lifeMultiplier*(@loanAmount+@initFee) --add attribute for life 
		IF NOT EXISTS (SELECT 1 FROM dbo.OfferAttribute WHERE OfferKey = @offerKey and OfferAttributeTypeKey = 12)
		BEGIN 
			INSERT INTO dbo.OfferAttribute(OfferKey, OfferAttributeTypeKey)  
			VALUES  (@offerKey, 12)
		END    	
	END
ELSE IF (@hasExternalLife = 1) 
 BEGIN
		BEGIN
			DECLARE @legalEntityKey INT
			SET @legalEntityKey = (SELECT legalEntityKey FROM [2am].dbo.ExternalRole WHERE genericKey = @offerKey and externalRoleTypeKey = 1)
			--zero out the premium
			UPDATE [2am].dbo.OfferInformationPersonalLoan
			SET LifePremium = 0
			WHERE offerInformationKey = @offerInformationKey
			--insert the external life policy information
			INSERT INTO [2am].dbo.ExternalLifePolicy
			(InsurerKey, PolicyNumber, CommencementDate, LifePolicyStatusKey, SumInsured, PolicyCeded, LegalEntityKey)
			VALUES ( 7, 'TestPolicyNumber', dateadd(mm, -6, GETDATE()), 3, @loanAmount*2, 1, @legalentityKey)
			SET @ExternalLifePolicyKey = SCOPE_IDENTITY()
			--insert the link
			INSERT INTO [2am].dbo.OfferExternalLife
			(offerKey, ExternalLifePolicyKey)
			VALUES (@OfferKey, @ExternalLifePolicyKey)
			--remove the offer attribute if it exists
			IF EXISTS (SELECT 1 FROM dbo.OfferAttribute WHERE OfferKey = @offerKey and OfferAttributeTypeKey = 12)
			BEGIN 
				DELETE FROM [2AM].dbo.OfferAttribute WHERE OfferKey = @offerKey and OfferAttributeTypeKey = 12
			END    	
		END
 END
 
 SET @feesTotal = @initFee   
  
 INSERT INTO [2am].dbo.OfferInformation  
 (offerInsertDate, offerKey, offerInformationTypeKey, userName, changeDate,  
 productKey)  
 VALUES  
 (GETDATE(), @offerKey, 1, 'SAHL\TestUser', GETDATE(), 12)  
  
 SELECT @offerInformationKey = SCOPE_IDENTITY()  
  
 INSERT INTO [2am].dbo.OfferInformationPersonalLoan   
 SELECT @offerInformationKey, @loanAmount, @term, @monthlyInstalment, @lifePremium, @feesTotal, @CreditCriteriaUnsecuredLendingKey, @marginKey, 6  
  
 DECLARE @expenseTypeKey INT  
 SELECT @expenseTypeKey = expenseTypeKey FROM [2am]..expenseType WHERE description = 'Personal Loan Initiation Fee'  
 --add the offerexpense  
 INSERT INTO [2am]..OfferExpense  
 (offerKey, expenseTypeKey, totalOutstandingAmount, tobeSettled)  
 VALUES  
 (@offerKey, @expenseTypeKey, @initFee, 1)  
           
END  


  
  
  