CREATE PROCEDURE test.CreateSwitchApplication

@CurrentLoan float,
@CashOut float,
@CapitaliseFees int,
@MarketValue float,
@EmploymentTypeKey int,
@Term int,
@HouseholdIncome float,
@InterestOnly int,
@fixedPercentage float

AS

BEGIN

	/*
	Switch
	New Variable
	Market Value 500 000
	Current Loan 100 000
	Cash Out 50 000
	Salaried
	Term 240
	Capitalise Fees = YES
	Income 25 000
	Interest Only = NO
	*/
	DECLARE @ProductKey int
	DECLARE @MortgageLoanPurposeKey int
	DECLARE @ReservedAccountKey int
	DECLARE @OfferInformationTypeKey int
	DECLARE @offerSourceKey int
	DECLARE @TotalLoanAmount float
	DECLARE @LoanAmountIncludingFees float
	DECLARE @LoanAmountExcludingFees float
	DECLARE @LTV float
	DECLARE @LoanAgreementAmount float
	DECLARE @OfferTypeKey int

	SET @OfferTypeKey = 6
	SET @MortgageLoanPurposeKey = 2
	SET @OfferInformationTypeKey = 1
	SET @offerSourceKey = 137
	SET @TotalLoanAmount = @CurrentLoan + @CashOut
	SET @LoanAmountExcludingFees = @TotalLoanAmount




	--insert into accountSequence
	INSERT INTO dbo.AccountSequence (IsUsed) VALUES (0) 
	select @ReservedAccountKey = SCOPE_IDENTITY()
	--we need to get fees
	declare @CancellationFee float --ExpenseType 5
	declare @IniationFee float -- ExpenseType 1
	declare @RegFee float --ExpenseType 4
	DECLARE @InterimInterest float
	DECLARE @BondAmount float

	SELECT @InterimInterest = InterimInterest , @CancellationFee = CancelFee, @IniationFee = CalculateInitiationFees,
	@BondAmount = BondToRegister, @RegFee = RegistrationFee
	FROM test.getFeesForApplicationCreate(@TotalLoanAmount,0,@OfferTypeKey,@CashOut,0,@CapitaliseFees,0,0,0)

	DECLARE @totalFees float

	set @totalFees = @CancellationFee + @IniationFee + @RegFee

	IF @CapitaliseFees = 1
		BEGIN
			SET @LoanAmountIncludingFees = @TotalLoanAmount + @TotalFees
			SET @LoanAgreementAmount = @LoanAmountIncludingFees
		END
	ELSE IF @CapitaliseFees = 0
		BEGIN
			SET @LoanAmountIncludingFees = @TotalLoanAmount + @TotalFees
			SET @LoanAgreementAmount = @LoanAmountExcludingFees
		END 
		
	SET @LoanAgreementAmount = @LoanAgreementAmount + @InterimInterest

	SET @LoanAmountExcludingFees = @LoanAmountExcludingFees + InterimInterest
		
	SET @LTV = @LoanAgreementAmount / @marketValue

	--GET THE CREDIT CRITERIA KEY
	DECLARE @CreditCriteriaKey int
	DECLARE @CategoryKey int
	DECLARE @CreditMatrixKey int
	DECLARE @MarginKey int
	select top 1 
	@CreditCriteriaKey = cc.CreditCriteriaKey , 
	@CategoryKey = cc.CategoryKey, 
	@CreditMatrixKey = cc.CreditMatrixKey, 
	@MarginKey = cc.MarginKey
	from dbo.CreditCriteria cc 
	inner join dbo.CreditMatrix cm on cc.CreditMatrixKey=cm.CreditMatrixKey 
	inner join dbo.OriginationSourceProductCreditMatrix ospcm on cm.CreditMatrixKey=ospcm.CreditMatrixKey 
	inner join dbo.OriginationSourceProduct osp on ospcm.OriginationSourceProductKey=osp.OriginationSourceProductKey
	inner join dbo.Margin m on cc.MarginKey=m.MarginKey 
	where 
	cc.MortgageLoanPurposeKey = @MortgageLoanPurposeKey 
	and cc.EmploymentTypeKey = @EmploymentTypeKey
	and cc.MaxLoanAmount >= @LoanAmountIncludingFees
	and cc.LTV >= @LTV 
	and cm.NewBusinessIndicator = 'Y'
	and osp.OriginationSourceKey = 1
	and osp.ProductKey = @ProductKey
	and cc.ExceptionCriteria=0 
	order by 
	cc.MaxLoanAmount asc, 
	m.Value asc, 
	cc.LTV asc
	--THEN WE NEED TO GET THE CREDIT MATRIX, CATEGORY, MARGIN, RATE CONFIGURATION
	declare @RateConfigurationKey int
	declare @MarketRateKey int
	set @MarketRateKey = 1
	select @RateConfigurationKey = RateConfigurationKey
	from RateConfiguration
	where marginKey = @MarginKey and marketRateKey = @MarketRateKey
	declare @MarketRateValue float
	select @MarketRateValue = value from MarketRate where marketRateKey = 1
	declare @MarginValue float
	select @MarginValue = value from Margin where marketRateKey = @MarginKey

	--DETERMINE RESET CONFIGURATION
	declare @spvKey int
	declare @resetConfigurationKey int
	if @LTV > 81
		begin
			set @spvKey = 16
		end
	else
		begin
			set @spvKey = 17
		end

	SELECT @resetConfigurationKey = [2AM].[dbo].[fResetConfigurationDetermine] (@spvKey, @productKey)

	DECLARE @MonthlyInstalment float
	--TODO: CALC

	SET @MonthlyInstalment = (@LoanAgreementAmount) * 
	 (Power(1 + ((@MarketRateValue+@MarginValue) / 12),@Term)) * 
	(((@MarketRateValue+@MarginValue) / 12) / (Power(1 + ((@MarketRateValue+@MarginValue) / 12),
	@Term) - 1))

	DECLARE @PTI float
	SET @PTI = @MonthlyInstalment / @HouseHoldIncome

	--THEN WE INSERT INTO LEGAL ENTITY
	declare @LegalEntityKey int

	insert into [2am].dbo.LegalEntity
	(LegalEntityTypeKey, MaritalStatusKey, GenderKey, PopulationGroupKey, IntroductionDate, Salutationkey, FirstNames, 
	Initials, Surname, PreferredName, DateOfBirth, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, 
	EmailAddress, CitizenTypeKey, LegalEntityStatusKey, DocumentLanguageKey, IDNumber, educationKey, homelanguagekey)
	VALUES
	(2, 2, 2, 2, GetDate()-5, 1, 'Test' + convert(varchar(2), datepart(dd, GetDate())) 
	+ convert(varchar(2), datepart(hh, GetDate())) 
	+ convert(varchar(2), datepart(mm, GetDate()))
	+ convert(varchar(2), datepart(ss, GetDate())) , 
	'T', 'Case', 'Test Case', '1973/10/10', '021', '5555555','021', '5555555', '0555555555', 'test@sahomeloans.com', 1, 1, 2, '', 5, 2)

	select @LegalEntityKey =  Scope_Identity()

	--THEN WE INSERT INTO LEGAL ENTITY MARKETING OPTION
	insert into [2am].dbo.legalEntityMarketingOption
	(LegalEntityKey, MarketingOptionKey)
	values
	(@LegalEntityKey, 6)

	--INSERT INTO OFFER
	declare @OfferKey int
	INSERT INTO dbo.Offer (
	OfferStartDate, 
	OfferEndDate, 
	EstimateNumberApplicants, 
	Reference, 
	ReservedAccountKey, 
	AccountKey, 
	OfferCampaignKey, 
	OfferStatusKey, 
	OriginationSourceKey, 
	OfferSourceKey, 
	OfferTypeKey
	) VALUES (
	getdate(), 
	null, 
	null, 
	null, 
	@ReservedAccountKey, 
	null, 
	null, 
	1, 
	1, 
	@OfferSourceKey, 
	@OfferTypeKey
	); 
	--INSERT INTO OFFER ATTRIBUTE
	if @CapitaliseFees = 1
		BEGIN
			INSERT INTO dbo.OfferAttribute (OfferKey, OfferAttributeTypeKey) 
			VALUES (@OfferKey, 3)
		END
	--INSERT INTO OFFER INFORMATION
	declare @OfferInformationKey int
	INSERT INTO dbo.OfferInformation 
	(
	OfferInsertDate, 
	OfferKey, 
	ProductKey, 
	OfferInformationTypeKey
	) VALUES 
	(
	getdate(), 
	@OfferKey, 
	@ProductKey,
	@OfferInformationTypeKey
	)
	set @OfferInformationKey = scope_identity()
	--OFFER INFORMATION QC
	INSERT INTO dbo.OfferInformationQuickCash 
	(CreditApprovedAmount, Term, CreditUpfrontApprovedAmount, OfferInformationKey) 
	VALUES (0, 0, 0, @OfferInformationKey)
	--OFFER INFORMATION VARIABLE LOAN
	INSERT INTO dbo.OfferInformationVariableLoan 
	(
	Term, 
	ExistingLoan, 
	CashDeposit, 
	PropertyValuation, 
	HouseholdIncome, 
	FeesTotal, 
	InterimInterest, 
	MonthlyInstalment, 
	LoanAgreementAmount, 
	BondToRegister, 
	LTV, 
	PTI, 
	MarketRate, 
	LoanAmountNoFees, 
	RequestedCashAmount, 
	CategoryKey, 
	SPVKey, 
	EmploymentTypeKey, 
	CreditMatrixKey, 
	CreditCriteriaKey, 
	RateConfigurationKey, 
	OfferInformationKey
	) VALUES (
	@Term, 
	@CurrentLoan, 
	0, 
	@MarketValue, 
	@HouseHoldIncome, 
	@totalFees, 
	@InterimInterest, 
	@MonthlyInstalment, 
	@LoanAgreementAmount, 
	@BondAmount, 
	@LTV, 
	@PTI, 
	@MarketRateValue, 
	@LoanAmountExcludingFees, 
	@CashOut, 
	@CategoryKey, 
	@SPVKey, 
	@EmploymentTypeKey, 
	@CreditMatrixKey, 
	@CreditCriteriaKey, 
	@RateConfigurationKey, 
	@OfferInformationKey
	)

	/*--OFFER INFORMATION INTEREST ONLY START---*/
	DECLARE @IntOnlyInstalment FLOAT
	DECLARE @MaturityDate DATETIME
	SET @IntOnlyInstalment = (@LoanAgreementAmount*(@MarginValue+@MarketValue))/365*31

	IF @InterestOnly = 0
		BEGIN
			INSERT INTO dbo.OfferInformationInterestOnly 
			(Installment, MaturityDate, OfferInformationKey)
			 VALUES (null, null, @OfferInformationKey)
		END
	ELSE IF (@InterestOnly = 1 AND @ProductKey = 11)
		BEGIN
			SET @MaturityDate = DATEADD(MM, 36, GETDATE())
			INSERT INTO dbo.OfferInformationInterestOnly 
			(Installment, MaturityDate, OfferInformationKey)
			 VALUES (@IntOnlyInstalment, @MaturityDate, @OfferInformationKey)		
		END
	ELSE IF (@InterestOnly = 1 AND @ProductKey <> 11)
		BEGIN
			SET @MaturityDate = DATEADD(MM, @Term+3, GETDATE())
			INSERT INTO dbo.OfferInformationInterestOnly 
			(Installment, MaturityDate, OfferInformationKey)
			 VALUES (@IntOnlyInstalment, @MaturityDate, @OfferInformationKey)
		END
	/*--OFFER INFORMATION INTEREST ONLY END---*/


	/*--OFFER INFORMATION FINANCIAL ADJUSTMENTS START---*/
	--edge
	IF (@ProductKey = 11)
		BEGIN
			INSERT INTO dbo.OfferInformationFinancialAdjustment 
			(Term, FixedRate, Discount, FromDate, OfferInformationKey, FinancialAdjustmentTypeSourceKey) 
			VALUES (36, null, 0, NULL, @OfferInformationKey, 5)
		END
	/*--OFFER INFORMATION FINANCIAL ADJUSTMENTS END---*/

	/*---OFFER INFORMATION EDGE START---*/
	IF (@ProductKey = 11)
		BEGIN
			INSERT INTO dbo.OfferInformationEdge (
			FullTermInstalment, AmortisationTermInstalment, InterestOnlyInstalment, InterestOnlyTerm, OfferInformationKey) 
			VALUES 	(0, @MonthlyInstalment, @IntOnlyInstalment, 36, @OfferInformationKey)
		END
	/*---OFFER INFORMATION EDGE END---*/

	/*---OFFER INFORMATION VARIFIX START---*/
	DECLARE @FixedRate float
	DECLARE @FixedLegTotalRate float
	DECLARE @FixedLegInstalment float
	DECLARE @FixedLeg float
	SET @FixedLeg = @LoanAgreementAmount * @FixedPercentage

	SELECT @FixedRate = value from dbo.MarketRate where marketRateKey=10
	SET @FixedLegTotalRate = @FixedRate + @MarginValue
	SET @FixedLegInstalment = (@FixedLeg) * (Power(1 + ((@FixedRate) / 12),@Term)) * (((@FixedRate) / 12) / (Power(1 + ((@FixedRate) / 12), @Term) - 1))

	INSERT INTO dbo.OfferInformationVarifixLoan 
	(FixedPercent, FixedInstallment, ElectionDate, ConversionStatus, MarketRateKey, OfferInformationKey) 
	VALUES (@FixedPercentage, @FixedLegInstalment, getdate(), 0, 10, @OfferInformationKey)

	/*---OFFER INFORMATION VARIFIX END---*/
		
	/*---OFFER EXPENSES START---*/
	INSERT INTO dbo.OfferExpense 
	(ExpenseAccountNumber, 
	ExpenseAccountName, 
	ExpenseReference, 
	TotalOutstandingAmount, 
	MonthlyPayment, 
	ToBeSettled, 
	OverRidden, 
	ExpenseTypeKey, 
	LegalEntityKey, 
	OfferKey) VALUES 
	(NULL, NULL, NULL, @CancellationFee, 0, 0, 0, 5, null, @OfferKey)


	INSERT INTO dbo.OfferExpense 
	(ExpenseAccountNumber, 
	ExpenseAccountName, 
	ExpenseReference, 
	TotalOutstandingAmount, 
	MonthlyPayment, 
	ToBeSettled, 
	OverRidden, 
	ExpenseTypeKey, 
	LegalEntityKey, 
	OfferKey) 
	VALUES (NULL, NULL, NULL, @IniationFee, 0, 0, 0, 1, null, @OfferKey)

	INSERT INTO dbo.OfferExpense 
	(ExpenseAccountNumber, 
	ExpenseAccountName, 
	ExpenseReference, 
	TotalOutstandingAmount, 
	MonthlyPayment, 
	ToBeSettled, 
	OverRidden, 
	ExpenseTypeKey, 
	LegalEntityKey, 
	OfferKey
	) 
	VALUES (NULL, NULL, NULL, @RegFee, 0, 0, 0, 4, null, @OfferKey)
	/*---OFFER EXPENSES END---*/

	--INSERT INTO OFFER MORTGAGE LOAN
	INSERT INTO dbo.OfferMortgageLoan 
	(NumApplicants, 
	TransferringAttorney, 
	ClientEstimatePropertyValuation, 
	PurchasePrice, 
	DependentsPerHousehold, 
	ContributingDependents, 
	MortgageLoanPurposeKey, 
	ApplicantTypeKey, 
	PropertyKey, 
	ResetConfigurationKey, 
	DocumentLanguageKey, 
	OfferKey
	) 
	VALUES 
	(NULL, NULL, @MarketValue, NULL, NULL, NULL, @MortgageLoanPurposeKey, 
	NULL, NULL, @ResetConfigurationKey, 2, @OfferKey)
	--INSERT INTO OFFER ROLE
	declare @OfferRoleKey int
	INSERT INTO dbo.OfferRole (
	StatusChangeDate, 
	LegalEntityKey, 
	OfferKey, 
	OfferRoleTypeKey, 
	GeneralStatusKey) 
	VALUES (getdate(), @LegalEntityKey, @OfferKey, 8, 1);
	select @OfferRoleKey = scope_identity()

	--OFFER ROLE ATTRIBUTE
	INSERT INTO dbo.OfferRoleAttribute (
	OfferRoleKey, 
	OfferRoleAttributeTypeKey
	) VALUES (@OfferRoleKey, 1)

	--UPDATE OFFER MORTGAGE LOAN
	UPDATE dbo.OfferMortgageLoan 
	SET NumApplicants = 1, ApplicantTypeKey = 1 
	WHERE OfferKey = @OfferKey
END