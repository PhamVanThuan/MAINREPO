//This class holds enumerations not read out from the database - all enumerations have been manually added.

namespace SAHL.Common.Globals
{
    /// <summary>
    /// Automatically generated enumeration for static values pertaining to AccountInformationType.
    /// </summary>
    public enum AccountInformationTypes
    {
        VariFixElected = 1,
        VariFixConversion = 2,
        VariFixOptedOut = 3,
        SuperLoElected = 4,
        SuperLoConversion = 5,
        SuperLoOptedOut = 6,
        DefendDiscountElected = 7,
        DefendDiscountConversion = 8,
        DefendDiscountOptedOut = 9,
        AccountLegalName = 10,
        InterestOnlyMaturityDate = 11,
        NotNCACompliant = 12,
        GovernmentEmployeePensionFund = 13,
        StopOrderDiscountEligible = 14
    }

    public enum AccountTypes
    {
        MortgageLoan = 1,
        Life = 2,
        HOC = 3,
        Regent = 4,
        Unsecured = 5,
        CreditProtectionPlan = 6
    }

    /// <summary>
    /// List of ways credit can grant
    /// </summary>
    public enum ApprovalTypes
    {
        None = 0,
        Approve,
        ApproveWithPricingChanges,
        DeclineWithOffer
    }

    /// <summary>
    /// Delimiters that can be used to split address component parts.
    /// </summary>
    public enum AddressDelimiters
    {
        CarriageReturn,
        Comma,
        Space,
        HtmlLineBreak
    }

    /// <summary>
    /// Property risk area : AreaClassification. Class1 = good, Class6 = bad
    /// </summary>
    public enum AreaClassifications
    {
        Unknown = 1,
        Class1 = 2,
        Class2 = 3,
        Class3 = 4,
        Class4 = 5,
        Class5 = 6,
        Class6 = 7
    }

    /// <summary>
    /// Parameter names for the BulkBatchParameter table - these correspond exactly to the names
    /// of the parameters allowed.
    /// </summary>
    public enum BulkBatchParameterNames
    {
        ArrearBalance,
        CapTypeConfigurationKey,
        Category,
        Consultant,
        DebitOrderDate,
        ExcludeOffersAfterDate,
        FileDate,
        FileName,
        Format,
        FromDate,
        IncludeArrears,
        MailAddress,
        MathematicalOperator,
        Month, NoInstalment,
        OriginationSourceKey,
        ReportKey,
        ReportType,
        ResetConfigurationKey,
        SampleList,
        SPV,
        StatementMonths,
        Subsidised,
        ToDate,
        TransType,
    }

    /// <summary>
    /// Automatically generated enumeration for static values pertaining to Category.
    /// </summary>
    public enum Categories
    {
        Category0 = 0,
        Category1 = 1,
        Category2 = 2,
        Category3 = 3,
        Category4 = 4,
        Category5 = 5,
        Category6 = 6,
        Category7 = 7,
        Category8 = 8,
        Category9 = 9,
        Category10 = 10,
        Category11 = 11,
        ExceptionCategory = 99
    }

    public enum Databases
    {
        Batch,
        EWork,
        ImageIndex,
        SAHLDB,
        TwoAM,
        Warehouse,
        X2
    }

    /// <summary>
    /// Products that apply to open switch loan applications.
    /// </summary>
    public enum ProductsSwitchLoan
    {
        VariableLoan = 1,
        VariFixLoan = 2,
        SuperLo = 5,
        DefendingDiscountRate = 6,
        NewVariableLoan = 9,
        Edge = 11
    }

    /// <summary>
    /// Products that apply to open new purchase applictions.
    /// </summary>
    public enum ProductsNewPurchase
    {
        VariableLoan = 1,
        VariFixLoan = 2,
        SuperLo = 5,
        DefendingDiscountRate = 6,
        NewVariableLoan = 9,
        Edge = 11
    }

    /// <summary>
    /// Products that apply to open refinance applications.
    /// </summary>
    public enum ProductsRefinance
    {
        VariableLoan = 1,
        VariFixLoan = 2,
        SuperLo = 5,
        DefendingDiscountRate = 6,
        NewVariableLoan = 9,
        Edge = 11
    }

    /// <summary>
    /// Products that apply to creation of switch loan applications.
    /// </summary>
    public enum ProductsSwitchLoanAtCreation
    {
        VariFixLoan = 2,
        SuperLo = 5,
        NewVariableLoan = 9,
        Edge = 11
    }

    /// <summary>
    /// Products that apply to creation of new purchase applictions.
    /// </summary>
    public enum ProductsNewPurchaseAtCreation
    {
        VariFixLoan = 2,
        SuperLo = 5,
        NewVariableLoan = 9,
        Edge = 11
    }

    /// <summary>
    /// Products that apply to creation of refinance applications.
    /// </summary>
    public enum ProductsRefinanceAtCreation
    {
        VariFixLoan = 2,
        SuperLo = 5,
        NewVariableLoan = 9,
        Edge = 11
    }

    public enum ProductsUnsecuredLending
    {
        PersonalLoan = 12
    }

    /// <summary>
    /// Remuneration types that can be used for EmploymentSalaried objects.  The value of these members must be the same as the
    /// corresponding items in the RemunerationTypes enumeration.
    /// </summary>
    public enum EmploymentSalariedRemunerationTypes
    {
        Salaried = RemunerationTypes.Salaried,
        BasicAndCommission = RemunerationTypes.BasicAndCommission,
        CommissionOnly = RemunerationTypes.CommissionOnly
    }

    /// <summary>
    /// Remuneration types that can be used for EmploymentSelfEmployed objects.  The value of these members must be the same as the
    /// corresponding items in the RemunerationTypes enumeration.
    /// </summary>
    public enum EmploymentSelfEmployedRemunerationTypes
    {
        CommissionOnly = RemunerationTypes.CommissionOnly,
        Drawings = RemunerationTypes.Drawings,
        RentalIncome = RemunerationTypes.RentalIncome,
        InvestmentIncome = RemunerationTypes.InvestmentIncome,
        Pension = RemunerationTypes.Pension,
        Maintenance = RemunerationTypes.Maintenance,
        BusinessProfits = RemunerationTypes.BusinessProfits
    }

    /// <summary>
    /// Remuneration types that can be used for EmploymentSubsidised objects.  The value of these members must be the same as the
    /// corresponding items in the RemunerationTypes enumeration.
    /// </summary>
    public enum EmploymentSubsidisedRemunerationTypes
    {
        Salaried = RemunerationTypes.Salaried,
        BasicAndCommission = RemunerationTypes.BasicAndCommission
    }

    /// <summary>
    /// Remuneration types that can be used for EmploymentUnemployed objects.  The value of these members must be the same as the
    /// corresponding items in the RemunerationTypes enumeration.
    /// </summary>
    public enum EmploymentUnemployedRemunerationTypes
    {
        RentalIncome = RemunerationTypes.RentalIncome,
        InvestmentIncome = RemunerationTypes.InvestmentIncome,
        Pension = RemunerationTypes.Pension,
        Maintenance = RemunerationTypes.Maintenance,
        Unknown = RemunerationTypes.Unknown
    }

    /// <summary>
    /// Remuneration types that can be used for EmploymentUnknown objects.  This is provided for legacy data only - going
    /// forward we should not get Unknown employment types persisted to the database.
    /// </summary>
    public enum EmploymentUnknownRemunerationTypes
    {
        Unknown = RemunerationTypes.Unknown
    }

    /// <summary>
    /// Regent Statuses (from SAHLDB..RegentStatus)
    /// </summary>
    public enum RegentStatus
    {
        NewBusiness = 1,
        Cancellation = 2,
        Suspended = 3,
        Underwriting = 4,
        Lapsed = 5,
        NTU = 6,
        Quote = 7,
        Declined = 8
    }

    /// <summary>
    /// TransactionTypes (from SAHLDB..TransactionType)
    /// The table is relatively large, so feel free to add more as required.
    /// </summary>
    public enum TransactionTypes
    {
        Readvance = 140,
        ReadvanceCorrection = 1140,
        ReadvanceCAP = 141,
        ManualDebitOrderPayment = 710,
        MonthlyServiceFee = 485,
        MonthlyInterestDebit = 210,
        MonthlyInterestDebitCorrection = 1210,
        LoyaltyBenefitPayment = 237,
        LoyaltyBenefitPaymentCorrection = 1237,
        DebitNonPerformingInterest = 236,
        NonPerformingInterest = 966,
        ReverseNonPerformingInterest = 967,
        InstalmentPaymentSubsidy = 311,
        InstalmentPaymentDeposit = 312,
        DebtReviewArrangementCredit = 972,
        DebtReviewArrangementDebit = 973
    }

    public enum DisbursementLoanTransactionTypes
    {
        CancellationRefund = 160,
        ReAdvance = 140,
        ReadvanceCorrection = 1140,
        Refund = 170,
        CAP2ReAdvance = 141
    }

    public enum TransactionDisplayType
    {
        //Financial = 0,
        //Memo = 1,
        //All = 2

        Financial = 1,
        Memo = 2,
        All = 3
    }

    public enum Lead_MortgageLoan_Role : int
    {
        LEAD_MAIN_APPLICANT = 8,
        LEAD_SURETOR = 10
    };

    public enum Application_MortgageLoan_Role : int
    {
        APPLICATION_MAIN_APPLICANT = 11,
        APPLICATION_SURETOR = 12
    }

    public enum DataProviderDataServices
    {
        SAHLManualValuation = 1,
        SAHLClientEstimate = 2,
        LightstonePropertyIdentification = 3,
        LightstoneAutomatedValuation = 4,
        AdCheckPropertyIdentification = 5,
        AdCheckPhysicalValuation = 6,
        AdCheckDesktopValuation = 7,
        RCSPropertyIdentification = 8,
        RCSPhysicalValuation = 9,
        LightstoneInvalidData = 10,
        AdCheckInvalidData = 11,
        SublimeMarketingcampaign = 12,
        SantamMarketingcampaign = 13,
        RocketseedMarketingcampaign = 14,
        SAHLMarketingcampaign = 15,
        BlakesMarketingcampaign = 16,

        // 17 Used for Marketing campaign - Bryan Pietersen
        LightstonePhysicalValuation = 18
    }

    public enum ValuationDataProviderDataServices
    {
        SAHLManualValuation = 1,
        SAHLClientEstimate,
        LightstoneAutomatedValuation,
        AdCheckPhysicalValuation,
        AdCheckDesktopValuation,

        // 6 Used for RCS - Physical Valuation
        LightstonePhysicalValuation = 7
    }

    public enum PropertyDataProviderDataServices
    {
        LightstonePropertyIdentification = 1,
        AdCheckPropertyIdentification = 2,
        SAHLPropertyManualValuation = 3,
        LightstoneInvalidData = 4,
        AdCheckInvalidData = 5
    }

    public enum DetailTypes
    {
        Scheduled = 1,
        InstructionNotSent = 2,
        InstructionSent = 3,
        InstructionReceived = 4,
        ReplyReceived = 5,
        RegistrationReceived = 6,
        Lodged = 9,
        LoanClosed = 10,
        UnderCancellation = 11,
        HOCNoHoc = 12,
        HOCCessionOfPolicy = 13,
        EstateLateUnsecured = 14,
        PaidUpWithNoHOC = 100,
        PaidUpWithHOC = 101,
        HOCCessionCommercialUse = 114,
        DebitOrderSuspended = 150,
        SequestrationOrLiquidation = 180,
        ForeclosureUnderway = 186,
        BankDetailsIncorrect = 217,
        UnderCancellationguaranteeRecieved = 227,
        StaffHomeLoan = 237,
        CancellationRegistered = 251,
        AuctionSold = 275,
        EstateLateSecured = 279,
        SuretyDeceased = 290,
        LegalActionStopped = 293,
        CancellationLodged = 299,
        ClientWantsToNTU = 412,
        ReadvanceInProgress = 456,
        FurtherLoanInProgress = 457,
        DeseasedEstatesLitigationVelile = 581,
        DeseasedEstatesLitigationRandles = 582,
        DeseasedEstatesLitigationManoghMaharaj = 583,
        DeseasedEstatesLitigationHaroldGie = 584,
        DeseasedEstatesLitigationStraussDaly = 590,
        DeceasedEstateInsolvent = 592,
        ReAppliedforLodgement = 175,
        ProceedWithLodgement = 213,
        UnableToLodge = 214,
        UpforFees = 343,
        ClientWonOver = 413,
        NewLegalAgreementSigned = 596,
        AlphaHousing = 599,
        ManualLifePolicyPayment = 600
    }

    public enum ResetConfigurations
    {
        TwentyFirst = 1,
        Eighteenth = 2,
        QuickCashPrimeRate = 3,
        Fifteenth = 4,
        TwentySecond = 7
    }

    public enum ResidenceStatus
    {
        NonResident = 1,
        Permanent,
        Temporary
    }

    /// <summary>
    /// Enumeration for Report Keys, add your own if required (I will)
    /// </summary>
    public enum ReportStatements
    {
        FurtherAdvanceApplication = 127,
        FurtherLoanApplication = 127,
        RapidReadvanceApplication = 128,
        NaedoDebitOrderAuthorization = 7067,
        ConfirmationOfEnquiry = 7084
    }

    public enum ReportFormats
    {
        Mail = 1,
        Email = 2,
        Fax = 3
    }


    /// <summary>
    /// Too many entries in the table. Will add as these we need.
    /// </summary>
    public enum StageDefinitions
    {
        DebtCounselling = 16,
        ApplicationCaptureSubmitted = 1121,
        ReleaseAndVariationRequest = 1264
    }

    /// <summary>
    /// Too many entries in the table. Will add as these we need.
    /// </summary>
    public enum StageDefinitionStageDefinitionGroups
    {
        GoToCredit = 2093,
        ReturnToProcessor = 2147,
        DebtCounsellingQuickCashIn = 2848,
        DebtCounsellingQuickCashOut = 3020,
        DisputesIn = 22,
        DebtCounsellingLossControlIn = 21,
        DebtCounsellingLossControlOut = 20,
        AwaitingSpouseConfirmationOut = 56,
        DebtCounsellingLossControlExternalIn = 2849,
        DebtCounsellingLossControlExternalOut = 2847,
        CreditApproveApplication = 2617,
        CreditApproveWithPricingChanges = 2625,
        CreditApproveDeclineWithOffer = 2623,
        CreditDecline = 2621,
        DeclinewithOffer = 2050,
        ApproveApplication = 2051,
        ApprovewithPricingChanges = 2054,
        DeclineApplication = 2016,
        NTUOffer = 110,
        DeclineOffer = 111,
        DebtCounsellingIN = 3851,
        DebtCounsellingOUT = 3852,
        DebtCounsellingProposalAccepted = 4405,
        DebtCounselling171ResponseSent = 4415,
        AdditionalSuretyOnFA = 4155,
        AdditionalSuretyOnRapid = 4156,
        InstructAttorney = 1694,
        InactivatedFinancialAdjustment = 4191,
        ChangeofDebtCounselor = 4420,
        ChangeOfPaymentDistributionAgent = 4431,
        Received17pt3 = 4438,
        Received17pt2 = 4444,
        Received17pt1 = 4445,
        RecoveriesProposalReceived = 4456,
        DeclineProposal = 4406,
        ConsultantDeclineReasons = 4465,
        FL45DayTimer = 1344,
        PersonalLoanCreditApprove = 4591,
        PersonalLoanCreditDecline = 4595,
        PersonalLoanChangeTerm = 4618,
        PersonalLoanChangeInstalment = 4620,
        PersonalLoanAlteredApproval = 4625,
        ApplicationEmploymentTypeCreditConfirmed = 4628,
        ApplicationEmploymentTypeManageApplicationConfirmed = 4627,
        CreateCapitecApplication = 4630,
        CapitecClientContacted = 4631,
        DisabilityClaimApproved = 4646,
        DisabilityClaimRepudiated = 4647,
        DisabilityClaimSettled = 4648,
        DisabilityClaimTerminated = 4649,
        CreditDisqualified30YearTerm = 4658
    }

    /// <summary>
    /// Lists the possible sources for an address object.
    /// </summary>
    public enum AddressSources
    {
        None,
        Address,
        LegalEntityAddress,
        FailedLegalEntityAddress
    }

    /// <summary>
    /// Automatically generated enumeration for static values pertaining to MarketRate.
    /// Not anymore. The 20YearFixedMortgageRate enum was failing obviously because of the
    /// </summary>
    public enum MarketRates
    {
        ThreeMonthJIBARRounded = 1,
        ThreeMonthJIBAR = 2,
        TwentyYearFixedMortgageRate = 3,
        FiveYearResetFixedMortgageRate = 4,
        RepoRate = 5,
        PrimeLendingRate = 6
    }

    /// <summary>
    /// X2 State Types.
    /// </summary>
    public enum X2StateTypes
    {
        User = 1,
        System = 2,
        SystemDecision = 3,
        StartingPoint = 4,
        Archive = 5
    }

    /// <summary>
    /// Attributes for the GenericDAOTest
    /// </summary>
    public enum TestType
    {
        None = 0,
        Find = 1,
        LoadSaveLoad = 2
    }

    /// <summary>
    /// ValuationBuildingType
    /// </summary>
    public enum ValuationBuildingType
    {
        Outbuilding = 1,
        Improvement = 2
    }

    /// <summary>
    /// Mapped to the table [2am].[dbo].[OfferDeclarationQuestion]
    /// Add the key value of the Question required.
    /// Other questions can be added as needed.
    /// The indentity column property is turned off so keys will remain the same.
    /// </summary>
    public enum OfferDeclarationQuestions
    {
        DeclaredInsolvent = 1,
        DateRehabilitatedFromInsolvency = 2,
        UnderAministrationOrder = 3,
        DateAdministrationOrderRescinded = 4,
        UnderDebtReview = 5,
        CurrentDebtRearrangement = 6,
        PerformITC = 7,
        UnderDebtCounselling = 8
    }

    //used in SAHL.Web.Services
    public enum OfferSources
    {
        InternetLead = 96,
        InternetApplication = 97,
        MobisiteLead = 174,
        MobisiteApplication = 175,
        CampaignLead = 176,
        Capitec = 177
    }

    /// <summary>
    /// Mapped to the table [2am].[dbo].[ReasonDescription]
    /// Add the key value of the ReasonDescription required.
    /// Other ReasonDescriptions can be added as needed.
    /// The indentity column property is turned off so keys will remain the same.
    /// </summary>
    public enum ReasonDescriptions
    {
        MiscellaneousReason = 11,
        ClientDoesNotWishToProceed = 25,
        DeedsInterdictAttachment = 45,
        DeedsOverBond = 46,
        DeedsExtent = 47,
        DeedsOwnership = 48,
        DeedsBondGrantRights = 49,
        ITCJudgement = 50,
        ITCDefault = 51,
        ITCNotices = 53,
        ITCPaymentProfileArrears = 55,
        AwaitingSpouseConfirmation = 92,
        ITCKnockoutRuleDefaults = 165,
        ITCKnockoutRuleWorstEverPaymentProfile = 166,
        ITCKnockoutRuleDebtReview = 167,
        CreditScoreDecline = 168,
        ITCKnockoutRuleJudgments = 169,
        ITCKnockoutRuleNotices = 170,
        ITCKnockoutRuleDisputes = 171,
        NocreditscoreEmpiricaScorenotavailable = 172,
        NocreditscoreMissingIncompleteSBCinformation = 173,
        NocreditscoreNoITCdata = 186,
        RiskMatrixRefer = 187,
        ReallocateToDC = 195,
        ManageDC = 196,
        ProposalAcceptance = 197,
        CourtOrderWithAcceptance = 198,
        CourtOrderWithAppeal = 199,
        NoArrangementNotReferredToCourtAndNoPaymentsInTermsOfProposal = 201,
        CaseCreatedInError = 214,
        ClientHasVoluntarilyCancelled = 215,
        DCCancelledDuetoConsumerNonCompliance = 216,
        DCCancelledDuetoNonPayment = 217,
        DCCancelledClientRehabilitated = 218,
        DCCancelledClientSequestrated = 219,
        DCCancelledClientDeceased = 220,
        BondExcludedInArrears = 221,
        NotificationofDeath = 235,
        NotificationofSequestration = 236,
        PersonalLoansNTUUnderDebtCounselling = 240,
        CapitecClientContacted = 249
    }

    /// <summary>
    /// Mapped to the table [2am].[dbo].[ReasonDefinition]
    /// Add the key value of the ReasonDefinition required.
    /// Other ReasonDefinitions can be added as needed.
    /// </summary>
    public enum ReasonDefinitions
    {
        NotificationofDeath = 574,
        NotificationofSequestration = 575,
        DefaultInTermsOfCourtOrder = 531,
    }

    /// <summary>
    /// Used for Memo's
    /// </summary>
    public enum MemoStatus
    {
        All = 0,
        UnResolved = 1,
        Resolved = 2
    }

    public enum SPVs
    {
        MBT14TRUST = 114,
        BlueBannerAgencyAccount = 116,
        MainStreet65PtyLtd = 117,
        TheThekwiniWarehousingConduitPtyLtd = 122,
        BlueBannerSecuritisationVehicleRC1PtyLtd = 124,
        AlphaHousingBlueBanner = 157,
        AlphaHousingOldMutual = 160
    }

    /// <summary>
    /// Used By EmploymentDetails
    /// </summary>
    public enum ConfirmedIncome
    {
        No = 0,
        Yes = 1
    }

    /// <summary>
    /// Used By EmploymentDetails
    /// </summary>
    public enum ConfirmedEmployment
    {
        No = 0,
        Yes = 1
    }

    public enum CreditScoreDecisions : int
    {
        NoScore = 1,
        Accept = 2,
        Refer = 3,
        Decline = 4
    }

    public enum RiskMatrixDimensions : int
    {
        Empirica = 1,
        SBC = 2
    }

    public enum ClientSurveyStatus : int
    {
        All = 0,
        Unanswered = 1,
        Answered = 2,
        Rejected = 3
    }

    public enum ClientSurveyInternalEmails : int
    {
        ExternalMail = 0,
        InternalMail = 1
    }

    public enum CompcorpLiveReplyEventStatus : int
    {
        Submitted = 1,
        AIP = 2,
        Granted = 3,
        Registered = 4,
        Declined = 5,
        PropertyValuationCompleted = 6,
        BondAttorneyInstructed = 7,
        Referred = 8,
        Cancelled_Withdrawn = 9,
        NTU = 10,
        GuaranteesPaid = 11,
        OfferMade = 12,
        ApplicationIncomplete = 13,
        ApplicationInvalid = 14,
        WaitingForValuation = 15,
        WaitingForClientToAccept = 16,
        FinalPaidOut = 17,
        BankFeedback = 21,
        DocumentsReceived = 22,
        NoDocumentsReceived = 23,
        PropertyAssessmentCancelled = 24
    }

    public enum OrganisationStructureNodeTypes : int
    {
        EstateAgent = 0,
        NationalCreditRegulator = 1,
        PaymentDistributionAgency,
        DebtCounsellor,
        LegalEntity
    }

    public enum Intervals : int
    {
        Monthly = 1,
        Quarterly = 4,
        Yearly = 12
    }

    public enum HearingAppearanceTypes
    {
        CourtApplication = 1,
        CourtApplicationPostponed = 2,
        OrderGranted = 3,
        Appeal = 4,
        AppealPostponed = 5,
        AppealGranted = 6,
        AppealDeclined = 7,
        TribunalCourtApplication = 8,
        TribunalCourtApplicationPostponed = 9,
        TribunalOrderGranted = 10,
        TribunalAppeal = 11,
        TribunalAppealPostponed = 12,
        TribunalAppealGranted = 13,
        TribunalAppealDeclined = 14,
        ConsentOrderGranted = 15
    }

    public enum EWorkLossControlCaseType
    {
        Collection = 0,
        Arrears = 1,
        Litigation = 2
    }

    public enum SMSTypes
    {
        Free_Format = 1,
        Banking_Details = 2
    }

    public enum FinancialAdjustmentTypeSources
    {
        CAP = 1,
        SuperLo = 2,
        SAHLStaff = 3,
        DefendingCancellations = 4,
        InterestOnly = 5,
        CollectNoPayment = 7,
        DiscountedLinkrate = 8,
        CollectPartialPayment = 9,
        NoMonthlyServiceFee = 10,
        CAP2 = 11,
        SuperLoBreachGracePeriod = 12,
        NonPerforming = 13,
        RateAdjustment = 14,
        DebtCounsellingFixedPaymentInclusiveHOC = 15,
        DebtCounsellingFixedPaymentInclusiveLife = 16,
        DebtCounsellingFixedPaymentInclusive = 17,
        DebtCounsellingFixedPaymentExclusive = 18,
        DebtCounsellingFixedRate = 19,
        DebtCounsellingDiscountRate = 20,
        CounterRate = 21,
        Edge = 31,
        Loanwith30YearTerm = 36
    }

    /// <summary>
    /// Automatically generated enumeration for static values pertaining to SPVAttributeTypes.
    /// </summary>
    public enum SPVAttributeTypes
    {
        AllowFurtherLending = 1,
        AllowTermChange = 2
    }

    public enum OrganisationStructure
    {
        ApplicationProcessor = 157,
        EstateAgent = 697,
        SAHLDirect = 93,
        CCC_Consultant = 900,
        Credit = 1005,
        CreditAdmin = 1007,
        CreditSupervisor = 1008
    }

    public enum Process
    {
        Origination,
        Life,
        RCS,
        HelpDesk,
        DeleteDebitOrder,
        DebtCounselling
    }

    public enum Workflow
    {
        ApplicationCapture,
        ApplicationManagement,
        Valuations,
        Credit,
        ReadvancePayments,
        LoanAdjustments,
        LifeOrigination,
        RCS,
        HelpDesk,
        DeleteDebitOrder,
        DebtCounselling
    }

    public enum QueryLanguages
    {
        Hql,
        Sql
    }

    public enum ReportGenericParameter
    {
        AccountKey,
        ParentAccountKey,
        GenericKey
    }

    public enum SPVDetermineSources
    {
        Offer = 1,
        Account = 2,
        Params = 3
    }

    public enum ApplicationAttributeTypeGroups
    {
        AlphaHousing = 1
    }

    public enum DomiciliumAddressTypes
    {
        Active = 1,
        Property = 2,
        LegalEntity = 3
    }

    public enum CreditCriteriaAttributeTypes
    {
        NewBusiness = 1,
        FurtherLendingAlphaHousing = 2,
        FurtherLendingNonAlphaHousing = 3,
        GovernmentEmployeePensionFund = 4
    }

    public enum BatchServiceTypes
    {
        PersonalLoanLeadImport = 1
    }

    public enum CacheTypes
    {
        Lookups,
        LookupItem,
        UIStatement,
        DomainService
    }

    public enum Providers
    {
        SAHL = 1,
        ConnectDirect = 2,
        Naedo = 3
    }
}