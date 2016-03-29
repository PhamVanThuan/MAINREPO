Use [2am]
go

-- Drop stored procedure if it already exists
IF EXISTS (
  SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES 
   WHERE SPECIFIC_SCHEMA = N'test'
     AND SPECIFIC_NAME = N'CreateNewPurchaseApplication' 
)
   DROP PROCEDURE test.CreateNewPurchaseApplication
GO

CREATE PROCEDURE test.CreateNewPurchaseApplication
		@ProductKey INT,
		@PurchasePrice FLOAT,
		@CashDeposit FLOAT,
		@EmploymentTypeKey INT,
		@HouseholdIncome FLOAT,
		@Term INT = 240, 
		@PercentageToFix FLOAT = 0,
		@isInterestOnly BIT = 0
AS
	DECLARE 
		@OfferTypeKey INT

	SELECT 
		@OfferTypeKey = 7 --New Purchase Loan
		--@ProductKey = 9, --New Variable Loan
		--@PurchasePrice = 1000000,
		--@CashDeposit = 500000,
		--@EmploymentTypeKey = 1, --Salaried
		--@Term = 240, 
		--@PercentageToFix = 0.5,
		--@isInterestOnly = 0,
		--@HouseholdIncome = 50000
		
	IF (@ProductKey = 2)
		BEGIN
			SET @isInterestOnly = 0	
		END
		
	IF (@ProductKey = 11)
		BEGIN
			select @Term = c.ControlNumeric
			from dbo.Control c 
			where c.ControlDescription='Edge Max Term'
		END
--------------------------------------------------
--Reserve AccountKey
--------------------------------------------------
	DECLARE @ReservedAccountKey INT

	INSERT INTO dbo.AccountSequence (IsUsed) VALUES (0); 

	select @ReservedAccountKey = SCOPE_IDENTITY()
--------------------------------------------------
--Calculate Fees
--------------------------------------------------	
	DECLARE @BondRequired float,@OverRideCancelFee float,@CashOut float,
	@CapitaliseFees bit,@NCACompliant bit,@IsBondExceptionAction bit,@QuickPay bit

	SELECT 
	@BondRequired=0,
	@OverRideCancelFee=0,
	@CapitaliseFees=0,
	@NCACompliant=0,
	@IsBondExceptionAction=0,
	@QuickPay=0
	
	DECLARE @InterimInterest FLOAT, @CancellationFee FLOAT, @InitiationFee FLOAT, @BondToRegister FLOAT, @RegFee FLOAT
	
	SELECT 
	@InterimInterest = InterimInterest, 
	@CancellationFee = CancelFee, 
	@InitiationFee = CalculateInitiationFees,
	@BondToRegister = BondToRegister, 
	@RegFee = RegistrationFee
	FROM test.getFeesForApplicationCreate(@PurchasePrice,0,@OfferTypeKey,@CashOut,0,@CapitaliseFees,0,0,0)
	
	--SELECT 
	--@InterimInterest = 0, 
	--@CancellationFee = 1000, 
	--@InitiationFee = 1000,
	--@BondToRegister = 840000, 
	--@RegFee = 1000
--------------------------------------------------
--Calculate Category
--------------------------------------------------
	--DECLARE @ProductKey INT --(Already declared)
	DECLARE @MortgageLoanPurposeKey INT, @MaxLoanAmount FLOAT, @LTV FLOAT, @OriginationSourceKey INT

	SELECT
	@MortgageLoanPurposeKey = 3, --New Purchase
	@MaxLoanAmount = @PurchasePrice - @CashDeposit, 
	@LTV = (@PurchasePrice - @CashDeposit)/@PurchasePrice, 
	@OriginationSourceKey = 1 --SA Home Loans
		
	DECLARE @CreditCriteriaKey int, @CategoryKey int, @CreditMatrixKey int, @MarginKey int
	
	select top 1 
	@CreditCriteriaKey = cc.CreditCriteriaKey , 
	@CategoryKey = cc.CategoryKey, 
	@CreditMatrixKey = cc.CreditMatrixKey, 
	@MarginKey = cc.MarginKey
	from 
	dbo.CreditCriteria cc 
	inner join dbo.CreditMatrix cm on cc.CreditMatrixKey=cm.CreditMatrixKey 
	inner join dbo.OriginationSourceProductCreditMatrix ospcm on cm.CreditMatrixKey=ospcm.CreditMatrixKey 
	inner join dbo.OriginationSourceProduct osp on ospcm.OriginationSourceProductKey=osp.OriginationSourceProductKey, 
	dbo.Margin m 
	where 
	cc.MarginKey=m.MarginKey 
	and cc.MortgageLoanPurposeKey=@MortgageLoanPurposeKey 
	and cc.EmploymentTypeKey=@EmploymentTypeKey 
	and cc.MaxLoanAmount>=@MaxLoanAmount 
	and cc.LTV>=@LTV 
	and cm.NewBusinessIndicator='Y' 
	and osp.OriginationSourceKey=@OriginationSourceKey 
	and osp.ProductKey=@ProductKey 
	and cc.ExceptionCriteria=0 
	order by 
	cc.MaxLoanAmount asc, 
	m.Value asc, 
	cc.LTV asc
--------------------------------------------------
--Calculate RateConfiguration
--------------------------------------------------	
	--DECLARE  @MarginKey int --(Already declared)
	declare @RateConfigurationKey int, @MarketRateKey int, @MarketRate float, @Margin float
	
	set @MarketRateKey = 1
	
	select 
	@RateConfigurationKey = RateConfigurationKey,
	@Margin = m.Value,
	@MarketRate = mr.Value
	from 
	dbo.RateConfiguration rc
	join dbo.Margin m on rc.Marginkey = m.MarginKey
	join dbo.MarketRate mr on rc.MarketRateKey = mr.MarketRatekey
	where 
	rc.marginKey = @MarginKey and rc.marketRateKey = @MarketRateKey
--------------------------------------------------
--Calculate ResetConfiguration
--------------------------------------------------
	--DECLARE @ProductKey INT --(Already declared)
	DECLARE @SPVKey INT

	IF @LTV > 81
		BEGIN
			SET @SPVKey = 16
		END
	ELSE
		BEGIN
			SET @SPVKey = 17
		END
	
	DECLARE @ResetConfigurationKey INT

	SELECT @ResetConfigurationKey = [2AM].[dbo].[fResetConfigurationDetermine] (@SPVKey, @ProductKey)
--------------------------------------------------
--Insert LegalEntity
--------------------------------------------------
	DECLARE @LegalEntityKey INT, @IntroductionDate DATETIME, @TaxNumber VARCHAR(20), @HomePhoneCode VARCHAR(10), @HomePhoneNumber VARCHAR(15), 
	@WorkPhoneCode VARCHAR(10), @WorkPhoneNumber VARCHAR(15), @CellPhoneNumber VARCHAR(15), @EmailAddress VARCHAR(50), 
	@FaxCode VARCHAR(10), @FaxNumber VARCHAR(15), @Password VARCHAR(50), @Comments VARCHAR(225), @UserID VARCHAR(25), 
	@ChangeDate DATETIME, @DocumentLanguageKey INT, @ResidenceStatusKey INT, @LegalEntityExceptionStatusKey INT, 
	@LegalEntityStatusKey INT, @FirstNames VARCHAR(50), @Initials VARCHAR(5), @PreferredName VARCHAR(50), @IDNumber VARCHAR(20), 
	@PassportNumber VARCHAR(20), @DateOfBirth DATETIME, @Surname VARCHAR(50), @MaritalStatusKey INT, @GenderKey INT, 
	@PopulationGroupKey INT, @Salutationkey INT, @CitizenTypeKey INT, @EducationKey INT, @HomeLanguageKey INT, @LegalEntityTypeKey INT

	SELECT
	@IntroductionDate = GETDATE(), 
	@TaxNumber = '', 
	@HomePhoneCode = '123', 
	@HomePhoneNumber = '4567890', 
	@WorkPhoneCode = '', 
	@WorkPhoneNumber = '', 
	@CellPhoneNumber = '', 
	@EmailAddress = '', 
	@FaxCode = '', 
	@FaxNumber = '', 
	@Password = NULL, 
	@Comments = NULL, 
	@UserID = 'SAHL\BCUser', 
	@ChangeDate = NULL, 
	@DocumentLanguageKey = 2, 
	@ResidenceStatusKey = NULL, 
	@LegalEntityExceptionStatusKey = NULL, 
	@LegalEntityStatusKey = 1, 
	@FirstNames = 'Test', 
	@Initials = '', 
	@PreferredName = '', 
	@IDNumber = NULL, 
	@PassportNumber = NULL, 
	@DateOfBirth = NUll, 
	@Surname = 'Test', 
	@MaritalStatusKey = 2, --Single
	@GenderKey = 1, --Male
	@PopulationGroupKey = 1, --Unknown
	@Salutationkey = NULL, 
	@CitizenTypeKey = NULL, 
	@EducationKey = 1, --Unknown
	@HomeLanguageKey = 1, --Unknown
	@LegalEntityTypeKey = 2 --Natural Person

	INSERT INTO dbo.LegalEntity 
	(IntroductionDate, TaxNumber, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, 
	EmailAddress, FaxCode, FaxNumber, Password, Comments, UserID, ChangeDate, DocumentLanguageKey, ResidenceStatusKey, 
	LegalEntityExceptionStatusKey, LegalEntityStatusKey, FirstNames, Initials, PreferredName, IDNumber, PassportNumber, 
	DateOfBirth, Surname, MaritalStatusKey, GenderKey, PopulationGroupKey, Salutationkey, CitizenTypeKey, EducationKey, 
	HomeLanguageKey, LegalEntityTypeKey) 
	VALUES 
	(@IntroductionDate, @TaxNumber, @HomePhoneCode, @HomePhoneNumber, @WorkPhoneCode, @WorkPhoneNumber, @CellPhoneNumber, 
	@EmailAddress, @FaxCode, @FaxNumber, @Password, @Comments, @UserID, @ChangeDate, @DocumentLanguageKey, @ResidenceStatusKey, 
	@LegalEntityExceptionStatusKey, @LegalEntityStatusKey, @FirstNames, @Initials, @PreferredName, @IDNumber, @PassportNumber, 
	@DateOfBirth, @Surname, @MaritalStatusKey, @GenderKey, @PopulationGroupKey, @Salutationkey, @CitizenTypeKey, @EducationKey, 
	@HomeLanguageKey, @LegalEntityTypeKey); 

	SELECT @LegalEntityKey = SCOPE_IDENTITY()
--------------------------------------------------
--Insert LegalEntityMarketingOption
--------------------------------------------------
	--DECLARE @ChangeDate DATETIME, @UserID VARCHAR(25), @LegalEntityKey INT --(Already declared)
	DECLARE @LegalEntityMarketingOptionKey INT, @MarketingOptionKey INT

	SELECT
	@ChangeDate = NULL, 
	@UserID = NULL, 
	@MarketingOptionKey = 4 --Telemarketing

	INSERT INTO dbo.LegalEntityMarketingOption 
	(ChangeDate, UserID, MarketingOptionKey, LegalEntityKey) 
	VALUES 
	(@ChangeDate, @UserID, @MarketingOptionKey, @LegalEntityKey);

	SELECT @LegalEntityMarketingOptionKey = SCOPE_IDENTITY()
--------------------------------------------------
--Insert Offer
--------------------------------------------------
	--DECLARE @OriginationSourceKey INT, @OfferTypeKey INT --(Already declared)
	DECLARE @OfferKey INT, @OfferStartDate DATETIME, @OfferEndDate DATETIME, @EstimateNumberApplicants INT, 
	@Reference VARCHAR(50), @AccountKey INT, @OfferCampaignKey INT, @OfferStatusKey INT, 
	@OfferSourceKey INT

	SELECT @OfferStartDate = GETDATE(), 
	@OfferEndDate = NULL, 
	@EstimateNumberApplicants = NULL, 
	@Reference = NULL, 
	@AccountKey = NULL, 
	@OfferCampaignKey = NULL, 
	@OfferStatusKey = 1, --Open
	@OfferSourceKey = 1 --Algoa

	INSERT INTO dbo.Offer 
	(OfferStartDate, OfferEndDate, EstimateNumberApplicants, Reference, ReservedAccountKey, AccountKey, 
	OfferCampaignKey, OfferStatusKey, OriginationSourceKey, OfferSourceKey, OfferTypeKey) 
	VALUES 
	(@OfferStartDate, @OfferEndDate, @EstimateNumberApplicants, @Reference, @ReservedAccountKey, @AccountKey, 
	@OfferCampaignKey, @OfferStatusKey, @OriginationSourceKey, @OfferSourceKey, @OfferTypeKey); 

	select @OfferKey = SCOPE_IDENTITY()
--------------------------------------------------
--Insert OfferInformation
--------------------------------------------------
	--DECLARE @OfferKey INT, @ProductKey INT --(Already declared)
	DECLARE @OfferInformationKey INT, @OfferInsertDate DATETIME, @OfferInformationTypeKey INT

	SELECT
	@OfferInsertDate = GETDATE(), 
	@OfferInformationTypeKey = 1 --Original Offer

	INSERT INTO dbo.OfferInformation 
	(OfferInsertDate, OfferKey, ProductKey, OfferInformationTypeKey) 
	VALUES 
	(@OfferInsertDate, @OfferKey, @ProductKey, @OfferInformationTypeKey); 

	select @OfferInformationKey = SCOPE_IDENTITY()
--------------------------------------------------
--Insert OfferInformationVariableLoan
--------------------------------------------------
	--DECLARE @OfferInformationKey INT, @LTV FLOAT, @EmploymentTypeKey INT, @SPVKey INT, @InterimInterest FLOAT, @BondToRegister FLOAT, @CashDeposit FLOAT, @CategoryKey INT, @CreditMatrixKey INT, @CreditCriteriaKey INT, @RateConfigurationKey INT, @MarketRate FLOAT --(Already declared)
	DECLARE @ExistingLoan FLOAT, @PropertyValuation FLOAT, 
	@FeesTotal FLOAT, @MonthlyInstalment FLOAT, @LoanAgreementAmount FLOAT, 
	@PTI FLOAT, @LoanAmountNoFees FLOAT, @RequestedCashAmount FLOAT
	
	Set @LoanAgreementAmount = @PurchasePrice - @CashDeposit
	
	SELECT
	@ExistingLoan = 0,  
	@PropertyValuation = @PurchasePrice, 
	@FeesTotal = @CancellationFee + @InitiationFee + @RegFee, 
	@MonthlyInstalment = (@LoanAgreementAmount) * (Power(1 + ((@MarketRate+@Margin) / 12),@Term)) * 
	(((@MarketRate+@Margin) / 12) / (Power(1 + ((@MarketRate+@Margin) / 12),
	@Term) - 1)), 
	@PTI = @MonthlyInstalment/@HouseholdIncome, 
	@LoanAmountNoFees = @PurchasePrice - @CashDeposit, 
	@RequestedCashAmount = NULL

	INSERT INTO dbo.OfferInformationVariableLoan 
	(Term, ExistingLoan, CashDeposit, PropertyValuation, HouseholdIncome, FeesTotal, InterimInterest, 
	MonthlyInstalment, LoanAgreementAmount, BondToRegister, LTV, PTI, MarketRate, LoanAmountNoFees, 
	RequestedCashAmount, CategoryKey, SPVKey, EmploymentTypeKey, CreditMatrixKey, CreditCriteriaKey, 
	RateConfigurationKey, OfferInformationKey) 
	VALUES 
	(@Term, @ExistingLoan, @CashDeposit, @PropertyValuation, @HouseholdIncome, @FeesTotal, @InterimInterest, 
	@MonthlyInstalment, @LoanAgreementAmount, @BondToRegister, @LTV, @PTI, @MarketRate, @LoanAmountNoFees, 
	@RequestedCashAmount, @CategoryKey, @SPVKey, @EmploymentTypeKey, @CreditMatrixKey, @CreditCriteriaKey, 
	@RateConfigurationKey, @OfferInformationKey)
--------------------------------------------------
--Insert OfferInformationVarifixLoan 
--------------------------------------------------
	--DECLARE @OfferInformationKey int, @PercentageToFix FLOAT --(Already declared)
	DECLARE @FixedInstallment float,@ElectionDate datetime,@ConversionStatus int,@FixedMarketRateKey int
	
	SELECT 	
	@FixedInstallment=(@LoanAgreementAmount * @PercentageToFix) * (Power(1 + ((mr.Value) / 12),@Term)) * (((mr.Value) / 12) / (Power(1 + ((mr.Value) / 12), @Term) - 1)),
	@ElectionDate=GETDATE(),
	@ConversionStatus=0,
	@FixedMarketRateKey = mr.MarketRateKey
	FROM
	dbo.MarketRate mr
	where 
	mr.MarketRateKey = 4
	
	IF (@ProductKey = 2)
		BEGIN
			INSERT INTO dbo.OfferInformationVarifixLoan 
			(FixedPercent, FixedInstallment, ElectionDate, ConversionStatus, MarketRateKey, OfferInformationKey) 
			VALUES 
			(@PercentageToFix, @FixedInstallment, @ElectionDate, @ConversionStatus, @FixedMarketRateKey, @OfferInformationKey)
		END
--------------------------------------------------
--Insert OfferInformationInterestOnly
--------------------------------------------------
	--DECLARE @OfferInformationKey INT --(Already declared)
	DECLARE @InterestOnlyTerm int,@InterestOnlyInstalment FLOAT,@MaturityDate datetime

	SELECT
	@InterestOnlyInstalment = NULL, 
	@MaturityDate = NULL
	
	IF (@ProductKey = 11)
		BEGIN
			SELECT 
			@InterestOnlyTerm=36,
			@MaturityDate = DATEADD(MM, 36, GETDATE()),
			@InterestOnlyInstalment = (@LoanAgreementAmount*(@Margin+@MarketRate))/365*31
		END
	ELSE IF (@isInterestOnly = 1)
		BEGIN
			SELECT 
			@InterestOnlyTerm=@Term+3,
			@MaturityDate = DATEADD(MM, @Term+3, GETDATE()),
			@InterestOnlyInstalment = (@LoanAgreementAmount*(@Margin+@MarketRate))/365*31
		END	
		
	INSERT INTO dbo.OfferInformationInterestOnly 
	(Installment, MaturityDate, OfferInformationKey) 
	VALUES 
	(@InterestOnlyInstalment, @MaturityDate, @OfferInformationKey)
--------------------------------------------------
--Insert OfferInformationFinancialAdjustment
--------------------------------------------------
	--DECLARE @OfferInformationKey int, @InterestOnlyTerm int --(Already declared)
	DECLARE @OfferInformationFinancialAdjustmentKey INT,@FixedRate float,@Discount float,@FromDate datetime,@FinancialAdjustmentTypeSourceKey int

	SELECT 
	@FixedRate=NULL,
	@Discount=0,
	@FromDate=NULL,
	@FinancialAdjustmentTypeSourceKey=5 --Type:InterestOnly Source: InterestOnly
			
	IF (@ProductKey = 11)
	BEGIN
		INSERT INTO dbo.OfferInformationFinancialAdjustment 
		(Term, FixedRate, Discount, FromDate, OfferInformationKey, FinancialAdjustmentTypeSourceKey) 
		VALUES 
		(@InterestOnlyTerm, @FixedRate, @Discount, @FromDate, @OfferInformationKey, @FinancialAdjustmentTypeSourceKey); 

		SELECT @OfferInformationFinancialAdjustmentKey = SCOPE_IDENTITY()
	END
--------------------------------------------------
--Insert OfferInformationEdge
--------------------------------------------------
	--DECLARE @OfferInformationKey int,@InterestOnlyTerm int,@InterestOnlyInstalment float --(Already declared)
	DECLARE @FullTermInstalment float,@amortisationTermInstalment float

	SELECT 
	@FullTermInstalment=@MonthlyInstalment,
	@AmortisationTermInstalment = (@LoanAgreementAmount) * (Power(1 + ((@MarketRate+@Margin) / 12),(@Term-@InterestOnlyTerm))) * 
	(((@MarketRate+@Margin) / 12) / (Power(1 + ((@MarketRate+@Margin) / 12),
	(@Term-@InterestOnlyTerm)) - 1))

	IF(@ProductKey = 11)
		BEGIN
			INSERT INTO dbo.OfferInformationEdge 
			(FullTermInstalment, AmortisationTermInstalment, InterestOnlyInstalment, InterestOnlyTerm, OfferInformationKey) 
			VALUES 
			(@FullTermInstalment, @AmortisationTermInstalment, @InterestOnlyInstalment, @InterestOnlyTerm, @OfferInformationKey)
		END
--------------------------------------------------
--Insert OfferExpenses
--------------------------------------------------
	--DECLARE @LegalEntityKey INT, @OfferKey INT --(Already declared)
	DECLARE @OfferExpenseKey INT, @ExpenseAccountNumber VARCHAR(50), @ExpenseAccountName VARCHAR(50), @ExpenseReference VARCHAR(50), 
	@TotalOutstandingAmount FLOAT, @MonthlyPayment FLOAT, @ToBeSettled BIT, @OverRidden BIT, @ExpenseTypeKey INT
	
	SELECT
	@ExpenseAccountNumber = NULL, 
	@ExpenseAccountName = NULL, 
	@ExpenseReference = NULL, 
	@TotalOutstandingAmount = @CancellationFee, 
	@MonthlyPayment = 0, 
	@ToBeSettled = 0, 
	@OverRidden = 0, 
	@ExpenseTypeKey = 5 --Cancellation Fee
	--@LegalEntityKey = NULL 	
		
	INSERT INTO dbo.OfferExpense 
	(ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, 
	ToBeSettled, OverRidden, ExpenseTypeKey, LegalEntityKey, OfferKey) 
	VALUES 
	(@ExpenseAccountNumber, @ExpenseAccountName, @ExpenseReference, @TotalOutstandingAmount, @MonthlyPayment, 
	@ToBeSettled, @OverRidden, @ExpenseTypeKey, null, @OfferKey); 

	select @OfferExpenseKey = SCOPE_IDENTITY()

	SELECT
	@ExpenseAccountNumber = NULL, 
	@ExpenseAccountName = NULL, 
	@ExpenseReference = NULL, 
	@TotalOutstandingAmount = @InitiationFee, 
	@MonthlyPayment = 0, 
	@ToBeSettled = 0, 
	@OverRidden = 0, 
	@ExpenseTypeKey = 1 --Initiation Fee – Bond Preparation Fee
	--@LegalEntityKey = NULL 

	INSERT INTO dbo.OfferExpense 
	(ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, 
	ToBeSettled, OverRidden, ExpenseTypeKey, LegalEntityKey, OfferKey) 
	VALUES 
	(@ExpenseAccountNumber, @ExpenseAccountName, @ExpenseReference, @TotalOutstandingAmount, @MonthlyPayment, 
	@ToBeSettled, @OverRidden, @ExpenseTypeKey, null, @OfferKey); 

	select @OfferExpenseKey = SCOPE_IDENTITY()

	SELECT
	@ExpenseAccountNumber = NULL, 
	@ExpenseAccountName = NULL, 
	@ExpenseReference = NULL, 
	@TotalOutstandingAmount = @RegFee, 
	@MonthlyPayment = 0, 
	@ToBeSettled = 0, 
	@OverRidden = 0, 
	@ExpenseTypeKey = 4 --Registration Fee
	--@LegalEntityKey = NULL 

	INSERT INTO dbo.OfferExpense 
	(ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, 
	ToBeSettled, OverRidden, ExpenseTypeKey, LegalEntityKey, OfferKey) 
	VALUES 
	(@ExpenseAccountNumber, @ExpenseAccountName, @ExpenseReference, @TotalOutstandingAmount, @MonthlyPayment, 
	@ToBeSettled, @OverRidden, @ExpenseTypeKey, null, @OfferKey); 

	select @OfferExpenseKey = SCOPE_IDENTITY()
--------------------------------------------------
--Insert OfferMortgageLoan
--------------------------------------------------
	--DECLARE @DocumentLanguageKey INT, @OfferKey INT, @MortgageLoanPurposeKey INT, @PurchasePrice FLOAT, @ResetConfigurationKey INT --(Already declared)
	DECLARE @NumApplicants INT, @TransferringAttorney VARCHAR(50), @ClientEstimatePropertyValuation FLOAT, 
	@DependentsPerHousehold INT, @ContributingDependents INT, @ApplicantTypeKey INT, @PropertyKey INT

	SELECT
	@NumApplicants = NULL, 
	@TransferringAttorney = NULL, 
	@ClientEstimatePropertyValuation = @PurchasePrice, 
	@DependentsPerHousehold = NULL, 
	@ContributingDependents = NULL, 
	@ApplicantTypeKey = NULL, 
	@PropertyKey = NULL

	INSERT INTO dbo.OfferMortgageLoan 
	(NumApplicants, TransferringAttorney, ClientEstimatePropertyValuation, PurchasePrice, DependentsPerHousehold, 
	ContributingDependents, MortgageLoanPurposeKey, ApplicantTypeKey, PropertyKey, ResetConfigurationKey, 
	DocumentLanguageKey, OfferKey) 
	VALUES 
	(@NumApplicants, @TransferringAttorney, @ClientEstimatePropertyValuation, @PurchasePrice, @DependentsPerHousehold, 
	@ContributingDependents, @MortgageLoanPurposeKey, @ApplicantTypeKey, @PropertyKey, @ResetConfigurationKey, 
	@DocumentLanguageKey, @OfferKey)
--------------------------------------------------
--Insert OfferRole
--------------------------------------------------
	--DECLARE @LegalEntityKey INT, @OfferKey INT --(Already declared)
	DECLARE @OfferRoleKey INT, @StatusChangeDate DATETIME, @OfferRoleTypeKey INT, @GeneralStatusKey INT

	Select 
	@StatusChangeDate = GETDATE(), 
	@OfferRoleTypeKey = 8, --Lead - Main Applicant
	@GeneralStatusKey = 1 --Active

	INSERT INTO dbo.OfferRole 
	(StatusChangeDate, LegalEntityKey, OfferKey, OfferRoleTypeKey, GeneralStatusKey) 
	VALUES 
	(@StatusChangeDate, @LegalEntityKey, @OfferKey, @OfferRoleTypeKey, @GeneralStatusKey); 

	select @OfferRoleKey = SCOPE_IDENTITY()
--------------------------------------------------
--Insert OfferRoleAttribute
--------------------------------------------------
	--DECLARE @OfferRoleKey INT --(Already declared)
	DECLARE @OfferRoleAttributeKey INT , @OfferRoleAttributeTypeKey INT

	Select @OfferRoleAttributeTypeKey = 1 --Income Contributor

	INSERT INTO dbo.OfferRoleAttribute 
	(OfferRoleKey, OfferRoleAttributeTypeKey) 
	VALUES 
	(@OfferRoleKey, @OfferRoleAttributeTypeKey); 

	select @OfferRoleAttributeKey = SCOPE_IDENTITY()
--------------------------------------------------
--Update OffermortgageLoan
--------------------------------------------------
	Select
	@NumApplicants = 1, 
	@ApplicantTypeKey = 1 --Single

	UPDATE dbo.OfferMortgageLoan 
	SET 
	NumApplicants = @NumApplicants, 
	ApplicantTypeKey = @ApplicantTypeKey
	WHERE OfferKey = @OfferKey
		
--select * from offer where offerkey = @offerkey

--Select * from 
--offermortgageloan oml
--where oml.offerkey = @offerkey

--Select * from 
--offerinformation oi
--where oi.offerkey = @offerkey

--Select oivl.* from 
--offerinformation oi
--join offerinformationvariableloan oivl on oi.offerinformationkey = oivl.offerinformationkey
--where oi.offerkey = @offerkey

--Select oivf.* from 
--offerinformation oi
--join offerinformationVarifixLoan oivf on oi.offerinformationkey = oivf.offerinformationkey
--where oi.offerkey = @offerkey

--Select oiio.* from 
--offerinformation oi
--join offerinformationInterestOnly oiio on oi.offerinformationkey = oiio.offerinformationkey
--where oi.offerkey = @offerkey

--Select oifadj.* from 
--offerinformation oi
--join offerinformationfinancialadjustment oifadj on oi.offerinformationkey = oifadj.offerinformationkey
--where oi.offerkey = @offerkey

--Select oie.* from 
--offerinformation oi
--join offerinformationEdge oie on oi.offerinformationkey = oie.offerinformationkey
--where oi.offerkey = @offerkey
go