namespace Common.Enums
{
    /// <summary>
    /// AccountStatus
    /// </summary>
    public enum AccountStatusEnum
    {
        Open = 1,
        Closed = 2,
        Application = 3,
        Locked = 4,
        Dormant = 5,
        ApplicationpriortoInstructAttorney = 6
    }

    /// <summary>
    /// GenericKeyType
    /// </summary>
    public enum GenericKeyTypeEnum
    {
        Account_AccountKey = 1,
        Offer_OfferKey = 2,
        LegalEntity_LegalEntityKey = 3,
        FinancialService_FinancialServiceKey = 4,
        OfferInformation_OfferInformationKey = 5,
        Property_PropertyKey = 6,
        Memo_MemoKey = 9,
        Losscontrol_LoanNumber = 12,
        DebtCounselling_LoanNumber = 15,
        Dispute_LoanNumber = 16,
        CapOffer_CapOfferKey = 17,
        Workflow_Case_ID = 18,
        ParentAccount_AccountKey = 19,
        RelatedLegalEntity_LegalEntityKey = 20,
        Valuation_ValuationKey = 21,
        SPV_SPVKey = 22,
        SPVTrancheDefinition_SPVTrancheDefinitionKey = 23,
        Trade_TradeKey = 24,
        OfferRoleTypeOrganisationStructureMapping_OfferRoleTypeOrganisationStructureMappingKey = 25,
        StageDefinitionStageDefinitionGroup_StageDefinitionStageDefinitionGroupKey = 26,
        debtCounselling_debtCounsellingKey = 27,
        Proposal_ProposalKey = 28,
        OfferRoleType_OfferRoleTypeKey = 33,
        WorkflowRoleType_WorkflowRoleTypeKey = 34,
        Attorney_AttorneyKey = 35,
        ExternalRoleType_ExternalRoleTypeKey = 41,
        AffordabilityAssessment_AffordabilityAssessmentKey = 58
    }

    public enum CapStatusEnum
    {
        Open = 1,
        TakenUp = 2,
        NotTakenUp = 3,
        Expired = 4,
        Recalculated = 5,
        ReadvanceRequired = 6,
        OfferDeclined = 7,
        PrepareForCredit = 8,
        CreditApproval = 9,
        CheckCashPayment = 10,
        GrantedCap2Offer = 11,
        AwaitingDocuments = 12,
        AwaitingLA = 13,
        CallbackHold = 14,
        ReadytoCallback = 15
    }

    public enum DisabilityClaimStatusEnum
    {
        Pending = 1,
        Repudiated = 2,
        Approved = 3,
        Terminated = 4,
        Settled = 5
    }

    public enum DisabilityPaymentStatusEnum
    {
        Active = 1,
        Paid = 2,
        Terminated = 3
    }

    public enum OfferTypeEnum
    {
        Readvance = 2,
        FurtherAdvance = 3,
        FurtherLoan = 4,
        SwitchLoan = 6,
        Refinance = 8,
        NewPurchase = 7,
        Unknown = -1,
        UnsecuredLending = 11
    }

    public enum CapOptionsEnum
    {
        OnePerc = 1,
        TwoPerc = 2,
        ThreePerc = 3,
    }

    public enum OfferStatusEnum
    {
        Open = 1,
        Closed = 2,
        Accepted = 3,
        NTU = 4,
        Declined = 5,
        Unknown = -1
    }

    public enum OfferRoleTypeGroupEnum
    {
        Operator = 1,
        NonOperator = 2,
        Client = 3
    }

    public enum OfferRoleTypeEnum
    {
        Consultant = 1,
        PreviousInsurer = 2,
        EstateAgentChannel = 3,
        ConveyanceAttorney = 4,
        Valuator = 5,
        Insurer = 7,
        LeadMainApplicant = 8,
        Seller = 9,
        LeadSuretor = 10,
        MainApplicant = 11,
        Suretor = 12,
        AssuredLife = 13,
        CAPConsultant = 14,
        CAPAdmin = 15,
        CAPManager = 16,
        ForensicManager = 25,
        ForensicAdmin = 26,
        HelpDeskManager = 27,
        HelpDeskSupervisor = 28,
        HelpDeskAdmin = 29,
        TranslateConditions = 30,
        CommissionableConsultant = 100,
        BranchConsultantD = 101,
        BranchAdminD = 102,
        BranchManagerD = 103,
        BranchSupervisorD = 104,
        QAAdministratorD = 689,
        QASupervisorD = 690,
        NewBusinessProcessorD = 694,
        RegistrationsAdministratorD = 698,
        RegistrationsLOAAdminD = 699,
        RegistrationsSupervisorD = 700,
        CreditExceptionsD = 805,
        CreditManagerD = 806,
        CreditSupervisorD = 807,
        CreditUnderwriterD = 808,
        FLProcessorD = 857,
        FLDisbersingD = 858,
        FLOriginatorD = 859,
        RVAdminD = 888,
        FLCollectionsAdminD = 890,
        ResubmissionAdminD = 891,
        RegistrationsManagerD = 892,
        EAAdminD = 901,
        EAManagerD = 902,
        EAConsultantD = 903,
        FLAdminD = 917,
        QCSupervisorD = 919,
        FLDisbursementD = 920,
        FLSupervisorD = 921,
        LossControlQCCollectionsAdminD = 922,
        QCAdminD = 923,
        QCCollectionsAdminD = 924,
        QCConsultantD = 925,
        QCManagerD = 926,
        ValuationsManagerD = 927,
        ValuationsAdministratorD = 928,
        LifeD = 929,
        ForensicManagerD = 930,
        ForensicAdminD = 931,
        HelpDeskManagerD = 932,
        HelpDeskSupervisorD = 933,
        HelpDeskAdminD = 934,
        NewBusinessSupervisorD = 935,
        TranslateConditionsD = 936,
        FLManagerD = 937,
        RVManagerD = 938,
        EstateAgent = 940,
        Unknown = -1
    }

    public enum RoleTypeEnum
    {
        AssuredLife = 1,
        MainApplicant = 2,
        Suretor = 3,
        Unknown = -1
    }

    public enum ProductEnum
    {
        None = 0,
        VariableLoan = 1,
        VariFixLoan = 2,
        HomeOwnersCover = 3,
        LifePolicy = 4,
        SuperLo = 5,
        DefendingDiscountRate = 6,
        NewVariableLoan = 9,
        QuickCash = 10,
        Edge = 11,
        Unknown = -1,
        PersonalLoan = 12,
        SAHLCreditProtectionPlan = 13
    }

    public enum ParameterTypeEnum
    {
        OfferKey = 1,
        OfferRoleTypeKey = 2,
        State = 3,
        ADUserName = 4,
        AccountKey = 5,
        OfferTypeKey = 6,
        InstanceID = 7,
        DebtCounsellingKey = 8,
        HearingType = 9,
        HearingAppearanceTypeKey = 10,
        Unknown = -1
    }

    public enum LegalEntityTypeEnum
    {
        Unknown = 1,
        NaturalPerson,
        Company,
        CloseCorporation,
        Trust
    }

    public enum DetailTypeEnum
    {
        None = 0,
        Scheduled = 1,
        InstructionNotSent = 2,
        InstructionSent = 3,
        InstructionReceived = 4,
        ReplyReceived = 5,
        RegistrationReceived = 6,
        UndeliverableInstruction = 7,
        InstructionNotRead = 8,
        Lodged = 9,
        LoanClosed = 10,
        UnderCancellation = 11,
        HOCNoHOC = 12,
        HOCCessionOfPolicy = 13,
        EstateLateUnsecured = 14,
        SubDivision = 15,
        Consolidation = 16,
        BondNetReference = 56,
        NoReadvancesorFurtherLoans = 88,
        SecondAccount = 91,
        ExchangeControlRegulation = 93,
        BondInFavourOfThirdParty = 99,
        PaidUpWithNoHOC = 100,
        PaidUpWithHOC = 101,
        AmendedCancellationFigures = 104,
        SpecialPowerOfAttorney = 105,
        NoFurtherAdvances = 106,
        PotentialBadDebt = 107,
        LetterofDemandSent = 108,
        BadDebt = 109,
        HOCCessionCommercialUse = 114,
        SAPODebitOrder = 115,
        ArrearsArrangement = 117,
        SubstitutionOfDebtors = 126,
        VoucherIntroduced = 139,
        SecurityDocumentCheckedOut = 140,
        CorrespondenceDocumentCheckedOut = 141,
        MWebIntroducedClient = 144,
        NoticeOfAttachmentServed = 148,
        CessionofLifeAssuranceCeded = 149,
        DebitOrderSuspended = 150,
        ReAppliedforLodgement = 175,
        DefaultJudgmentObtained = 176,
        SequestrationorLiquidation = 180,
        DevelopmentLoanUnit = 185,
        ForeclosureUnderway = 186,
        BlueBeanCreditCard = 189,
        EracticPayments = 190,
        ClientRetrenched = 191,
        RegentLifeAssuranceIssued = 192,
        RecommendationSubmitted = 193,
        InsuranceClaim = 194,
        CapitalGainsTaxTransfer = 198,
        AnnualPayment = 199,
        ExpropriationInProgress = 202,
        ProceedWithLodgement = 213,
        UnableToLodge = 214,
        BankDetailsIncorrect = 217,
        SuspensiveSale = 219,
        UnderCancellationGuaranteeReceived = 227,
        BadAddress = 228,
        StaffHomeLoan = 237,
        Encroachment = 238,
        ChangeOfName = 239,
        BondVariationInProgress = 241,
        UnderAdministration = 242,
        ArrearsRates = 243,
        RecommendationGranted = 244,
        RecommendationDeclined = 245,
        RecommendationOfferMade = 246,
        DeferredPayment = 247,
        LoanCancelledShortfallApproved = 248,
        CancellationRegistered = 251,
        UsufructRegistered = 253,
        MauritiusCompetitionFFWD = 254,
        ForeclosureRandlesInc = 258,
        ForeclosureBalsillies = 259,
        ForeclosureEdelsteinBosmanInc_Pretoria = 260,
        ForeclosureMoodieandRobertson = 261,
        ForeclosureHofmeyer_Johannesburg = 262,
        ForeclosureHofmeyrHerbsteinGihwalaInc_Cape = 263,
        ForeclosureWebbers = 264,
        ForeclosureKaplanBlumbergAttorneys = 265,
        ForeclosureVelileTintoAssPretoria = 266,
        ForeclosureDrakeFlemmerOrsmondIncorporated = 267,
        SummonsServed = 268,
        SummaryJudgementDeclined = 269,
        DefendedMatter = 270,
        SummaryJudgmentGranted = 271,
        WritandAttachment = 272,
        SalesDateObtained = 273,
        SaleStayed = 274,
        AuctionSold = 275,
        AuctionBoughtIn = 276,
        TransfertoSAHLEffected = 277,
        PropertyInPossession = 278,
        EstateLateSecured = 279,
        SecurityDocumentMissing = 281,
        BlueBannerSchedule = 286,
        SuretyDeceased = 290,
        CuratorAppointed = 291,
        LegalActionStopped = 293,
        DisabilityClaimLodged = 294,
        DisabilityClaimApproved = 295,
        Fraud = 296,
        SlowPayer = 298,
        CancellationLodged = 299,
        TelegramSent = 301,
        ArrearsRescheduled = 302,
        LossProvisionProjection = 303,
        NonResidentForeignNational = 305,
        DirectLoanSPVBlueBanner = 307,
        LitigationThekwini1 = 311,
        LitigationThekwini2 = 312,
        ArrearsLetter3 = 328,
        IneligibleLoans = 333,
        ForeclosureStraussDaly = 335,
        FeesPaid = 339,
        UpforFees = 343,
        QuickCash = 344,
        Audit = 355,
        TransnetNoSwitchingFees = 363,
        DirectloanCurrentSPV = 370,
        LitigationArrearsArrangement = 374,
        LitigationReschedule = 375,
        LossProvisionActual = 376,
        NoUnsolicitedCorrespondence = 377,
        ForeclosureStraussDaly_JohannesburgBranch = 381,
        RegentLifeAssurance = 385,
        SectionaltitlewithHOC = 387,
        ResubmissionLegal = 388,
        ShortfallApprovedSubjectToAOD = 395,
        ShortfallApproved = 396,
        DirectIndemnityBondsRegistered = 400,
        BuildingLoan = 401,
        ClientwantstoNTU = 412,
        ClientWonOver = 413,
        NinetyDayClauseApplies = 414,
        DirectMortgageBondRegistered = 417,
        ThirdPartyAuction = 421,
        ForeclosureStraussDaly_Bloemfontein = 422,
        HOCDebitOrderLetter = 423,
        LifeClientResubmitted = 424,
        UnabletoLodgeCessionofLife = 427,
        Guardplacedonproperty = 428,
        AdvancepaymentpriortoregistrationFL = 441,
        SAHLBarclayCardGoldCardApplication = 443,
        Preemptiveright_seeclientmemo = 446,
        SALifePolicy = 450,
        ConvertedtonewDatabase = 451,
        LossProvisionApproved = 452,
        Cancellation_HeldOver = 453,
        DefendingCancellations = 454,
        Cancellation_Upforfees = 455,
        ReadvanceinProgress = 456,
        FurtherLoaninProgress = 457,
        CapinProgress = 459,
        CapClient = 460,
        DefendingCancellation = 461,
        VariFixQuery_App, ChangesOptOut = 464,
        Foreclosure_StraussDaly_CapeTown = 469,
        Defended_6monthcondition = 474,
        Valuationfeewaived = 475,
        Maximumoffermade = 476,
        HSGLead = 477,
        VariFixNonConversionReason = 478,
        VariFixOpt_Out90DayPending = 479,
        SAHLPIP = 480,
        CancellationRefundFormreceived = 481,
        DevelopmentLoan_Pending = 484,
        DevelopmentLoan_DeedsReady = 485,
        RequireQuickCash = 487,
        SuspensiveSaleAgreement = 488,
        SuperLoelectedbyclient = 493,
        ExternalOriginator = 510,
        Eskom_doclient = 513,
        Subsidy_AuthorisedtoAdjustDebitOrder = 534,
        Foreclosure_HeroldGie = 540,
        Foreclosure_MaynardMenonGovender = 541,
        DisputeIndicatedonITC = 544,
        SAHLHOCPolicyDetails = 556,
        QuickCashUpfrontPaid = 559,
        QuickCashPaid = 560,
        ThirdPartyBondRegistered = 564,
        AccountDocumentLanguageAfrikaans = 566,
        RequiresEquityAccess = 569,
        EquityAccessPaid = 570,
        Resub_MonthlyFeenotapplicable = 574,
        Foreclosure_ShepstoneWylie_Durban = 575,
        Foreclosure_ShepstoneWylie_Gauteng = 576,
        Foreclosure_VelileTintoAss_Greenpoint = 577,
        Foreclosure_VelileTintoAss_El = 578,
        CessionofLifeAssuranceinprogress = 579,
        Foreclosure_ManoghMaharajAttorneys = 580,
        DeceasedEstates_LitigationVelile = 581,
        DeceasedEstates_LitigationRandles = 582,
        DeceasedEstates_LitigationManoghMaharaj = 583,
        DeceasedEstates_LitigationHeroldGie = 584,
        Foreclosure_McintyreVanDerPost = 585,
        AuctionCancelled = 586,
        Specialpowerofattorney = 587,
        Confessiontojudgement = 588,
        BondVariationRegistered = 589,
        DeceasedEstates_LitigationStraussDaly = 590,
        LetterofDemandPreLegal = 591,
        DeceasedEstateInsolvent = 592,
        SettlementAgreement = 593,
        Costsawardedinourfavour = 594,
        Summaryjudgmentadjournedsinedie = 595,
        Unknown = -1
    }

    public enum LifePolicyTypeEnum
    {
        None = 0,
        StandardCover,
        AccidentOnlyCover
    }

    public enum DisbursementStatusEnum
    {
        ReadyForDisbursement = 0,
        Disbursed = 1,
        Pending = 2,
        RolledBack = 3
    }

    public enum DisbursementTransactionTypeEnum
    {
        Payment_NoInterest = 0,
        GuaranteePayment = 1,
        CashRequired = 2,
        OtherDisbursement = 3,
        CancellationRefund = 4,
        ReAdvance = 5,
        Refund = 6,
        CashRequired_Interest = 7,
        QuickCash = 8,
        CAP2ReAdvance = 9
    }

    public enum TransactionTypeEnum
    {
        None = 0,
        Loan = 110,
        CashDrawdown = 120,
        CashDeposit = 130,
        Readvance = 140,
        CAPReadvance = 141,
        FurtherLoan = 150,
        CancellationRefund = 160,
        CapitalMovement = 161,
        EarlyTerminationRefund = 165,
        Refund = 170,
        PromotionalDiscount = 180,
        Initialdrawdown_unsecured_ = 190,
        Initialdrawdown_secured_ = 191,
        Reductioninunsecuredportionofloan = 195,
        QuickCashInterestSettlement = 196,
        MonthlyInterestDebit = 210,
        MonthlyInterestDebit_unsecured_ = 211,
        InterestDebit = 220,
        AccruedInterestDebit_unsecured_ = 221,
        BackDatedInterestDebit = 225,
        InterestCredit230 = 230,
        InterestCredit231 = 231,
        BackDatedInterestCredit = 235,
        DebitNonPerformingInterest = 236,
        LoyaltyBenefitPayment = 237,
        NotUsed = 240,
        InterestCapPremium = 250,
        CapAdministrationFee = 255,
        CollectionFee = 265,
        DefaultAdministrationFee = 270,
        EarlyTerminationCharge = 275,
        InstalmentPaymentDebitOrder = 310,
        InstalmentPaymentSubsidy = 311,
        InstalmentPaymentDeposit = 312,
        InstalmentPaymentArrearSettlement = 313,
        PrePayment320 = 320,
        PrePayment321 = 321,
        RaiseInstalment = 330,
        RaiseHOCPremium = 340,
        RaiseInstalmentProtection = 350,
        RaiseRegentLifeAssurancePremium = 360,
        RaiseSALifeAssurancePremium = 370,
        FeesPaid = 399,
        DeedsFee = 400,
        TransferDuty = 410,
        CancellationFee = 415,
        StampDuty = 420,
        QuickCashProcessingFee = 425,
        InterestonQuickCashAdvance = 426,
        InitiationFeeQuickCash = 427,
        ConveyancingFee = 430,
        InitiationfeeHomeLoan = 440,
        InitiationFee = 441,
        BondPreparationFeeRegistration = 445,
        InitiationFeeBondPreparation = 446,
        ValuationFee = 450,
        ValuationArrears = 451,
        ValuationLitigation = 452,
        InitiationFeeValuation = 453,
        CancellationFee460 = 460,
        RegentLifeAssurancePremium = 465,
        BondProtectionPlanPremium = 466,
        HOCPremium = 470,
        ExternalHOCbondpreparationfee = 471,
        HOCExcess = 472,
        InstalmentProtectionPremium = 475,
        LegalFeeLitigation = 480,
        MonthlyServiceFee = 485,
        RaiseMonthlyServiceFee = 486,
        BondPreparationFee = 490,
        ShortProcessedLegalFees = 495,
        VATDebit = 510,
        ProductConversion = 550,
        ProductOpen = 560,
        ProductAccruedInterest = 561,
        ProductOldInstallment = 562,
        VariFixProcessingFee = 570,
        VariFixProcessingFeeReceived = 571,
        ReturnedDebitOrder = 600,
        MonthlyArrearInterestDebit = 610,
        ReturnedInstallment = 620,
        ProcessRDcheque = 621,
        ReturnedPrePayment = 630,
        ManualDebitOrderPayment = 710,
        InstallmentStatusCredit = 720,
        CapitalRepaymentCredit = 721,
        TechnicalArrearsCredit = 722,
        StopOrderEffectiveCredit = 723,
        StopOrderOtherPaymentCredit = 724,
        ArrearsArrangementCredit = 725,
        FurtherAdvanceRescheduleCredit = 726,
        InstallmentStatusDebit = 730,
        CapitalRepaymentDebit = 731,
        TechnicalArrearsDebit = 732,
        StopOrderEffectiveDebit = 733,
        StopOrderOtherPaymentDebit = 734,
        ArrearsArrangementDebit = 735,
        FurtherAdvanceRescheduleDebit = 736,
        RegentLifeAssurancePremiumRefunded = 737,
        SALifeAssurancePremiumRefunded = 738,
        HOCPremiumRefunded = 739,
        AttorneyCostsWaived = 760,
        QuickCashFeeWaived = 761,
        SAHLCostsWaived = 765,
        ProceedsFromSaleInExecution = 820,
        ForeclosureConveyancingFee = 830,
        ForeclosureTransferFee = 831,
        ForeclosureAcquisitionVAT = 832,
        Foreclosuresecurityguardfee = 833,
        ForeclosureMaintenanceCosts = 834,
        ForeclosureUtilityBills = 835,
        ForeclosureMaintenanceVAT = 836,
        RecoverableBadDebt = 840,
        PIPWriteOff = 841,
        BadDebt = 850,
        SPVTransfer = 900,
        SPVTransferCapital = 901,
        SPVTransferInterest = 902,
        SPVTransferCapital903 = 903,
        SPVTransferInterest904 = 904,
        SPVTransfer905 = 905,
        SPVTransferLoyaltybenfit906 = 906,
        SPVTransferLoyaltybenfit907 = 907,
        SPVTransferCAPAccrual908 = 908,
        SPVTransferCAPAccrual909 = 909,
        MonthEnd = 910,
        Void = 911,
        SPVTransferSuspendedInterest = 912,
        SPVTransferSuspendedInterest913 = 913,
        InterestRateChange = 920,
        AccrualofHedgecointerest = 921,
        SettlementofHedgecointerest = 922,
        CAP2DebitOrderRefund = 923,
        QuickCashProcessingfeeinclinInitiationfee = 925,
        Determination = 930,
        DeterminationLoyaltyBenefit = 931,
        InstallmentChange = 940,
        ProductConversion950 = 950,
        LoyaltyBenefitProvision960 = 960,
        LoyaltyBenefitProvision965 = 965,
        NonPerformingInterest = 966,
        ReverseNonPerformingInterest = 967,
        LoanCorrection = 1110,
        CashDrawdownCorrection = 1120,
        CashDepositCorrection = 1130,
        ReadvanceCorrection1140 = 1140,
        ReadvanceCorrection1141 = 1141,
        FurtherLoanCorrection = 1150,
        CancellationRefundCorrection = 1160,
        CapitalMovement1161 = 1161,
        EarlyTerminationRefundCorrection = 1165,
        RefundCorrection = 1170,
        PromotionalDiscountCorrection = 1180,
        QuickCashCorrection = 1190,
        Initialdrawdown_secured_Correction = 1191,
        QuickCashSettlementCorrection = 1195,
        QuickCashInterestSettlementCorrection = 1196,
        MonthlyInterestDebitCorrection = 1210,
        MonthlyInterestDebit_unsecured_Correction = 1211,
        InterestDebitCorrection = 1220,
        AccruedInterestDebit_unsecured_Correction = 1221,
        BackDatedInterestDebitCorrection = 1225,
        InterestCreditCorrection = 1230,
        BackDatedInterestCreditCorrection = 1235,
        DebitNonPerformingInterestCorrection = 1236,
        LoyaltyBenefitPayment1237 = 1237,
        NotUsed1240 = 1240,
        InterestCapPremiumCorrection = 1250,
        CapAdminFeeCorrection = 1255,
        CollectionFeeCorrection = 1265,
        DefaultAdministrationFeeCorrection = 1270,
        EarlyTerminationChargeCorrection = 1275,
        NotUsed1300 = 1300,
        InstalmentPaymentDebitOrderCorrection = 1310,
        InstalmentPaymentSubsidyCorrection = 1311,
        InstalmentPaymentDepositCorrection = 1312,
        InstalmentPaymentArrearSettlementCorrection = 1313,
        PrePaymentCorrection = 1320,
        PrePaymentCorrection1321 = 1321,
        RaiseInstalmentCorrection = 1330,
        RaiseHOCPremiumCorrection = 1340,
        RaiseInstalmentProtectionPremiumCorrection = 1350,
        RaiseRegentLifeAssurancePremiumCorrection = 1360,
        RaiseSALifeAssurancePremiumCorrection = 1370,
        FeesPaidCorrection = 1399,
        DeedsFeeCorrection = 1400,
        TransferDutyCorrection = 1410,
        CancellationFeeCorrection = 1415,
        StampDutyCorrection = 1420,
        QuickCashProcessingFeeCorrection = 1425,
        InterestonQuickCashAdvanceCorrection = 1426,
        InitiationFeeQuickCashCorrection = 1427,
        ConveyancingFeeCorrection = 1430,
        InitiationfeeCorrectionHomeLoan = 1440,
        InitiationFeeCorrection = 1441,
        BondPreparationFeeRegistrationCorrection = 1445,
        InitiationFeeBondPreparationCorrection = 1446,
        ValuationFeeCorrection = 1450,
        ValuationArrearsCorrection = 1451,
        ValuationsLitigationCorrection = 1452,
        InitiationFeeValuationCorrection = 1453,
        CancellationFeeCorrection1460 = 1460,
        RegentLifeAssurancePremiumCorrection = 1465,
        BondProtectionPlanPremiumCorrection = 1466,
        HOCPremiumCorrection = 1470,
        ExternalHOCbondpreparationfeeCorrection = 1471,
        HOCExcessCorrection = 1472,
        InstalmentProtectionpremiumCorrection = 1475,
        LegalFeeLitigationCorrection = 1480,
        MonthlyServiceFeeCorrection = 1485,
        RaiseMonthlyServiceFeeCorrection = 1486,
        BondPreparationFeeCorrection = 1490,
        ShortPaymentLegalFeesCorrection = 1495,
        VATCorrection = 1510,
        ProductConvertionCorrection = 1550,
        ProductOpenCorrection = 1560,
        ProductAccruedInterestCorrection = 1561,
        VariFixProcessingFeeCorrection = 1570,
        VariFixProcessingFeeReceivedCorrection = 1571,
        ReturnedDebitOrderCorrection = 1600,
        MonthlyArrearInterestCorrection = 1610,
        ReturnedInstallmentCorrection = 1620,
        ProcessRDchequeCorrection = 1621,
        ReturnedPrePaymentCorrection = 1630,
        ManualDebitOrderPaymentCorrection = 1710,
        InstallmentStatusCreditCorrection = 1720,
        TechnicalArrearsCreditCorrection = 1722,
        InstallmentStatusDebitCorrection = 1730,
        TechnicalArrearsDebitCorrection = 1732,
        RegentLifeAssurancePremiumRefundedCorrection = 1737,
        SALifeAssurancePremiumRefundedCorrection = 1738,
        HOCPremiumRefundedCorrection = 1739,
        AttorneyCostsWaivedCorrection = 1760,
        QuickCashFeeWaivedCorrection = 1761,
        SAHLCostsWaivedCorrection = 1765,
        ProceedsFromSaleInExecutionCorrection = 1820,
        ForeclosureConveyancingFeeCorrection = 1830,
        ForeclosureTransferFeeCorrection = 1831,
        ForeclosureAcquisitionVATCorrection = 1832,
        ForeclosuresecurityguardfeeCorrection = 1833,
        ForeclosureMaintenanceCostsCorrection = 1834,
        ForeclosureUtilityBillsCorrection = 1835,
        ForeclosureMaintenanceVATCorrection = 1836,
        RecoverableBadDebtCorrection = 1840,
        PIPWriteOffCorrection = 1841,
        BadDebtCorrection = 1850,
        QuickCashProcessingfeeinclCorrection = 1925,
        StaffOptIn = 1992,
        StaffOptOut = 1993,
        DebtReviewArrangementCredit = 972,
        DebtReviewArrangementDebit = 973,
        StartDebtCounselling = 970,
        StopDebtCounselling = 971,
        CloseAccount = 1985,
        ExpireReversalAdjustment = 1977,
        CAPOptOut = 2004,
        CancelFixedRateAdjustment = 1987,
        CancelReversalAdjustment = 2002,
        PersonalLoanInitiationFee = 2011,
        PersonalLoanInitiationFeeCorrection = 2012,
        PersonalLoanMonthlyServiceFee = 2013,
        PersonalLoanMonthlyServiceFeeCorrection = 2014,
        PersonalLoan = 2015,
        PersonalLoanCorrection = 2016,
        DebtCounselllingOptOut = 1998,
        TermChange = 1986,
        InitiationFeeDiscount_ReturningClient = 1442
    }

    /// <summary>
    /// Offer Information Types
    /// </summary>
    public enum OfferInformationTypeEnum
    {
        OriginalOffer = 1,
        RevisedOffer = 2,
        AcceptedOffer = 3
    }

    /// <summary>
    /// General Status
    /// </summary>
    public enum GeneralStatusEnum
    {
        None = 0,
        Active = 1,
        Inactive = 2,
        Pending = 3
    }

    /// <summary>
    ///
    /// </summary>
    public enum EmploymentTypeEnum
    {
        Salaried = 1,
        SelfEmployed = 2,
        SalariedWithDeductions = 3,
        Unemployed = 4,
        Unknown = 5
    }

    /// <summary>
    ///
    /// </summary>
    public enum EmploymentStatusEnum
    {
        Current = 1,
        Previous = 2
    }

    /// <summary>
    ///
    /// </summary>
    public enum RemunerationTypeEnum
    {
        Unknown = 1,
        Salaried = 2,
        HourlyRate = 3,
        BasicPlusCommission = 4,
        CommissionOnly = 5,
        RentalIncome = 6,
        InvestmentIncome = 7,
        Drawings = 8,
        Pension = 9,
        Maintenance = 1,
        BusinessProfits = 11
    }

    public enum WorkflowRoleTypeEnum
    {
        None = 0,
        DebtCounsellingAdminD = 1,
        DebtCounsellingConsultantD = 2,
        DebtCounsellingCourtConsultantD = 3,
        RecoveriesManagerD = 4,
        DebtCounsellingSupervisorD = 5,
        ForeclosureConsultantD = 6,
        ForeclosureSupportParalegalD = 7,
        PLConsultantD = 30,
        PLManagerD = 31,
        PLSupervisorD = 32,
        PLAdminD = 33,
        PLCreditAnalystD = 34,
        PLCreditExceptionsManagerD = 35
    }

    public enum ProposalTypeEnum
    {
        Proposal = 1,
        CounterProposal = 2
    }

    public enum ProposalStatusEnum
    {
        Active = 1,
        Inactive = 2,
        Draft = 3
    }

    public enum ProposalAcceptedEnum
    {
        False = 0,
        True = 1
    }

    public enum ExternalRoleTypeEnum
    {
        None = 0,
        Client = 1,
        DebtCounsellor = 2,
        PaymentDistributionAgent = 3,
        NationalCreditRegulator = 4,
        LitigationAttorney = 5,
        DebtCounselling = 6,
        DeceasedEstates = 7,
        Foreclosure = 8,
        Sequestrations = 9,
        WebAccess = 10
    }

    /// <summary>
    /// Origination Source Enum
    /// </summary>
    public enum OriginationSourceEnum
    {
        None = 0,
        SAHomeLoans = 1,
        Blakes,
        Imperial,
        RCS,
        SALife,
        AgencyChannel,
        MortgageOriginators
    }

    /// <summary>
    /// Citizen Type Enum
    /// </summary>
    public enum CitizenTypeEnum
    {
        None = 0,
        SACitizen = 1,
        SACitizenNonResident = 2,
        Foreigner = 3,
        NonResident = 4,
        NonResidentRefugee = 5,
        NonResidentConsulate = 6,
        NonResidentDiplomat = 7,
        NonResidentHighCommissioner = 8,
        NonResidentCMAResidentCitizen = 9,
        NonResidentContractWorker = 10
    }

    public enum MarketRateEnum
    {
        _3MonthJibar_Rounded = 1,
        _3MonthJibar = 2,
        _20YearFixedMortgageRate = 3,
        _5YearResetFixedMortgageRate = 4,
        RepoRate = 5,
        PrimeLendingRate = 6
    }

    public enum AssetLiabilityTypeEnum
    {
        None = 0,
        FixedProperty = 1,
        ListedInvestments,
        UnlistedInvestments,
        OtherAsset,
        LifeAssurance,
        LiabilityLoan,
        LiabilitySurety,
        FixedLongTermInvestment
    }

    public enum AssetLiabilitySubTypeEnum
    {
        None = 0,
        PersonalLoan = 1,
        StudentLoan = 2
    }

    public enum OfferDeclarationQuestionEnum
    {
        Insolvency = 1,
        Rehabilitation = 2,
        AdminOrder = 3,
        AdminOrderRescinded = 4,
        CurrentlyUnderDebtCounselling = 5,
        DebtRearrangement = 6,
        CreditCheck = 7
    }

    public enum SalutationTypeEnum
    {
        Mr = 1,
        Mrs = 2,
        Prof = 5,
        Dr = 6,
        Capt = 7,
        Past = 8,
        Miss = 9,
        Sir = 11,
        Ms = 12,
        Lord = 13,
        Rev = 14
    }

    public enum GenderTypeEnum
    {
        Male = 1,
        Female
    }

    public enum MaritalStatusEnum
    {
        Unknown = 0,
        Single = 2,
        MarriedCommunityOfProperty = 3,
        MarriedAnteNuptualContract = 4,
        Divorced = 5,
        Widowed = 6,
        MarriedForeign = 7
    }

    public enum PopulationGroupEnum
    {
        Unknown = 1,
        White = 2,
        Black = 3,
        Coloured = 4,
        Asian = 5
    }

    public enum EducationEnum
    {
        Unknown = 1,
        Matric = 2,
        UniversityDegree = 3,
        Diploma = 4,
        Other = 5
    }

    public enum LanguageEnum
    {
        Unknown = 1,
        English,
        Afrikaans,
        isiNdebele,
        isiXhosa,
        isiZulu,
        NorthernSotho,
        Sesotho,
        Setswana,
        siSwati,
        Tshivenda,
        Xitsonga,
        Other,
    }

    public enum HearingTypeEnum
    {
        Court = 1,
        Tribunal
    }

    public enum HearingAppearanceTypeEnum
    {
        CourtCourtApplication = 1,
        CourtCourtApplicationPostponed,
        CourtOrderGranted,
        CourtAppeal,
        CourtAppealPostponed,
        CourtAppealGranted,
        CourtAppealDeclined,
        TribunalCourtApplication,
        TribunalCourtApplicationPostponed,
        TribunalOrderGranted,
        TribunalAppeal,
        TribunalAppealPostponed,
        TribunalAppealGranted,
        TribunalAppealDeclined
    }

    public enum CorrespondenceMediumEnum
    {
        None = 0,
        Post = 1,
        Email,
        Fax,
        SMS,
    }

    public enum OnlineStatementFormatEnum
    {
        PDFFormat = 1,
        Text,
        NotApplicable,
        HTML,
    }

    public enum ReasonTypeGroupEnum
    {
        Decline = 2,
        NTU = 4,
        CancelPolicy = 5,
        Callback = 6,
        Query = 8,
        ApplicationRevision = 9,
        QAComplete = 11,
        Resubmission = 13,
        ClientNeedAnalysisCategory = 14,
        SwitchProduct = 15,
        DebtCounselling = 16
    }

    public enum ReasonTypeEnum
    {
        AdministrativeDecline = 3,
        ApplicationNTU = 11,
        BranchDecline = 35,
        CapCancellation = 24,
        ChangeFinancialInstitution = 31,
        CreditDecline = 14,
        CreditScoringDecline = 36,
        CreditScoringQuery = 37,
        DebtConsolidation = 26,
        DebtCounsellingCancelled = 44,
        DebtCounsellingTermination = 42,
        HomeRenovation = 28,
        Initiator = 39,
        InvestmentProperty = 29,
        LeadNTU = 32,
        LifeCallback = 16,
        LifeNTU = 17,
        LifeProductSwitch = 38,
        LifePolicyCancellation = 15,
        LitigationReallocation = 40,
        MovableAssetsFinance = 27,
        NewHomePurchase = 30,
        OriginationNTU = 34,
        PipelineNTU = 33,
        ProcessorQuery = 18,
        ProposalAccepted = 41,
        ProposalDeclined = 43,
        QAComplete = 23,
        QAQuery = 22,
        QuickCashDecline = 13,
        QuickCashNTU = 12,
        Resubmission = 25,
        RevisionCreditDecline = 19,
        RevisionInitial = 20,
        RevisionReSubmission = 21,
        DebtCounsellingNotification = 47
    }

    public enum StageDefinitionStageDefinitionGroupEnum
    {
        DebtCounselling_AdvisedOfDebtCounselling = 4400,
        DebtCounselling_SendtoLitigation = 4402,
        DebtCounselling_ContinueLitigation = 4403,
        DebtCounselling_ManageLitigation = 4404,
        DebtCounselling_AcceptProposal = 4405,
        DebtCounselling_DeclineProposal = 4406,
        DebtCounselling_ProposalsentforApproval = 4407,
        DebtCounselling_RequestNewProposal = 4408,
        DebtCounselling_Proposal = 4409,
        DebtCounselling_CounterProposal = 4410,
        DebtCounselling_CourtOrderGranted = 4411,
        DebtCounselling_ChangeinCircumstances = 4412,
        DebtCounselling_AttorneytoOppose = 4413,
        DebtCounselling_ContinueDebtReview = 4414,
        DebtCounselling_17_1ResponseSent = 4415,
        DebtCounselling_DebtCounsellingCaseAssigned = 4417,
        DebtCounselling_InstructAttorneytoOppose = 4418,
        DebtCounselling_NotificationofApprovalDecision = 4419,
        DebtCounselling_ChangeofDebtCounselor = 4420,
        DebtCounselling_TerminateApplication = 4421,
        DebtCounselling_60DaysLapsed = 4423,
        DebtCounselling_GoToIntentToTerminate = 4425,
        DebtCounselling_GoToTerminate = 4424,
        DebtCounselling_45DayReminder = 4426,
        DebtCounselling_To60DaysReview = 4427,
        DebtCounselling_DebtReviewCancelled = 4428,
        DebtCounselling_PaymentReceived = 4430,
        DebtCounselling_ChangeofPaymentDistributionAgent = 4431,
        DebtCounselling_DefaultinPayment = 4432,
        DebtCounselling_ReallocateUser = 4433,
        DebtCounselling_PaymentinOrder = 4434,
        DebtCounselling_InadequatePayment = 4435,
        DebtCounselling_ReviewTerm = 4436,
        DebtCounselling_CurrentProposalTermExtended = 4437,
        DebtCounselling_17_3Received = 4438,
        Credit_ValuationApproved = 4399,
        ApplicationManagement_ReworkApplication = 1796,
        DebtCounselling_5DaysLapsed = 4422,
        DebtCounselling_DebtCounsellingTerminated = 4440,
        DebtCounselling_17_1Received = 4445,
        DebtCounselling_17_2Received = 4444,
        DebtCounselling_ConsultantDeclineReasons = 4465,
        DebtCounselling_NotifiedofDeath = 4449,
        DebtCounselling_NotifiedofSequestration = 4450,
        DebtCounselling_DebtCounsellingBondExclusion = 4567,
        DebtCounselling_BondExclusionArrearsIN = 4568,
        DebtCounselling_BondExclusionArrearsOUT = 4569,
        DebtCounselling_TerminationLetterSent = 4439,
        DebtCounselling_EscalateRecoveriesProposal = 4457,
        DebtCounselling_ApproveShortfall = 4570,
        DebtCounselling_DeclineShortfall = 4571,
        DebtCounselling_RecoveriesProposalReceived = 4456,
        DebtCounselling_UnderCancellation = 4561,
        DebtCounselling_UnderCancellationRemoved = 4562,
        CancellationRegistered = 4446,
        DebtCounselling_IntentToTerminate = 4572,
        DebtCounselling_SendDeclineLetter = 4574,
        DebtCounselling_SendCounterProposal = 4573,
        DebtCounselling_OptOutRequired = 4575,
        DebtCounselling_SendOptOutLegalDocuments = 4565,
        DebtCounselling_OptOutLegalDocumentsReceived = 4566,
        DebtCounselling_AccountAdjusted = 4582,
        DebtCounselling_CourtApplicationWithdrawn = 4621,
        LoanServicing_InactivatedRateOverride = 4191,
        RecalculatePremiums = 37,
        AddAssuredLife = 61,
        RemoveAssuredLife = 62,
        CreatePersonalLoanLead = 4602,
        PersonalLoans_CalculateApplication = 4604,
        PersonalLoans_Decline = 4595,
        PersonalLoans_ReturnToManageApplication = 4590,
        PersonalLoans_Approve = 4591,
        PersonalLoans_Altered_Approval = 4625,
        ApplicationInOrder = 4489,
        PersonalLoans_DeclineFinalised = 4596,
        PersonalLoans_DocumentsVerified = 4593,
        PersonalLoans_SendOffer = 4603,
        NTU = 4600,
        PersonalLoans_NTUFinalised = 4598,
        PersonalLoans_ReinstateNTU = 4599,
        PersonalLoans_NTUTimer = 4597,
        PersonalLoans_SendDocuments = 4592,
        PersonalLoans_ReworkApplication = 4601,
        PersonalLoans_DisburseFunds = 4594,
        PersonalLoans_DisbursedTimer = 4605,
        PersonalLoans_ExternalNTU = 4609,
        PersonalLoans_AutomatedCorrespondenceFailed = 4607,
        PersonalLoans_CorrespondenceSent = 4608,
        PersonalLoans_AutomatedCorrespondenceSent = 4606,
        PersonalLoans_RollbackDisbursement = 4616,
        PersonalLoans_NTULead = 4617,
        PersonalLoans_ChangeTerm = 4618,
        PersonalLoans_AdministrativeDecline = 4619,
        PersonalLoans_ChangeInstalment = 4620,
        PersonalLoans_ReturnToLegalAgreements = 4622,
        InstructEzValValuer = 4443,
        FurtherValuationRequired = 4584,
        ReviewValuationRequired = 4585,
        ReinstructValuer = 1890,
        RequestValuationReview = 1490,
        LightstoneValuationCompleted = 4580,
        LightstoneValuationAmended = 4581,
        LightstoneValuationRejected = 4578,
        ValuationInOrder = 2057,
        EscalateToManager = 2150,
        PersonalLoans_DocumentCheck = 4626,
        PerformManualValuation = 2174,
        Credit_ConfirmapplicationEmployment = 4628,
        ApplicationManagement_ConfirmapplicationEmployment = 4627,
        CreateCapitecApplication = 4630,
        DeclineCapitecApplication = 4633,
        CapitecClientContacted = 4631,
        DisabilityClaim_Created = 4644,
        DisabilityClaim_Migrated = 4644,
        DisabilityClaim_Captured = 4645,
        DisabilityClaim_Approved = 4646,
        DisabilityClaim_Repudiated = 4647,
        DisabilityClaim_Settled = 4648,
        DisabilityClaim_Terminated = 4649,
        DisabilityClaim_ApprovalLetterSent = 4652,
        DisabilityClaim_ManualApprovalLetter = 4653,
        DisabilityClaim_ContainsNoExclusions = 4654,
        PersonalLoans_AffordabilityAssessment_Confirm_Affordability = 4661,
        Origination_AffordabilityAssessment_Confirm_Affordability = 4662
    }

    public enum OrganisationTypeEnum
    {
        Company = 1,
        RegionOrChannel = 2,
        BranchOrOriginator = 3,
        Team = 4,
        Department = 5,
        SalesOffice = 6,
        Designation = 7,
        Division = 9,
        ExternalBranch = 10,
        SubDepartment = 11,
        None
    }

    public enum AddressTypeEnum
    {
        Residential = 1,
        Postal
    }

    public enum AddressFormatEnum
    {
        None = 0,
        Street = 1,
        Box,
        PostNetSuite,
        PrivateBag,
        FreeText,
        ClusterBox,
    }

    /// <summary>
    /// Debt Counselling Status
    /// </summary>
    public enum DebtCounsellingStatusEnum
    {
        None = 0,
        Open = 1,
        Closed = 2,
        Cancelled = 3,
        Terminated = 4
    }

    public enum FinancialServicePaymentTypeEnum
    {
        None = 0,
        DebitOrderPayment = 1,
        SubsidyPayment = 2,
        DirectPayment = 3
    }

    public enum ExternalRoleTypeGroupEnum
    {
        Client = 1,
        Operator = 2,
        LitigationAttorney = 3,
        Unknown = -1
    }

    public enum ContentTypeEnum
    {
        StandardText = 0,
        HTML = 1
    }

    /// <summary>
    /// FinancialServiceType
    /// </summary>
    public enum FinancialServiceTypeEnum
    {
        VariableLoan = 1,
        FixedLoan = 2,
        HomeOwnersCover = 4,
        LifePolicy = 5,
        LoyaltyBenefit = 6,
        SuspendedInterest = 7,
        ArrearBalance = 8,
        PersonalLoan = 10
    }

    public enum MortgageLoanPurposeEnum
    {
        Unknown = 1,
        Switchloan,
        Newpurchase,
        Refinance,
        FurtherLoan,
        ReAdvance,
        QuickCash,
        LifeLoan,
        CashUpfront,
        FurtherAdvance
    }

    public enum NotificationTypeEnum
    {
        Sequestration,
        Death
    }

    public enum ExpenseTypeEnum
    {
        None = 0,
        InitiationFeeBondPreparationFee = 1,
        TransferFee = 2,
        InitiationFeeValuationFee = 3,
        RegistrationFee = 4,
        CancellationFee = 5,
        InitiationFeeQuickCashProcessingFee = 6,
        LifeAdministrationFee = 7,
        ExistingMortgageAmount = 8,
        InterimInterest = 9,
        StoreCard = 11,
        VehicleFinance = 12,
        Other = 13,
        QuickCash = 14,
        QuickCashSettlement = 15,
        DeedsOfficeFee = 16
    }

    public enum HOCInsurerEnum
    {
        SAHLHOC = 2,
        ABSAInsuranceCompanyLtd = 3,
        ChartisAIGSouthAfricaLtd = 4,
        AllianzInsuranceLtd = 5,
        AutoGeneralInsuranceCompanyLtd = 6,
        InactiveBOEInsuranceCompanyLtd = 7,
        InactiveCommercialGuaranteeUnionLtd = 8,
        InactiveFedsureGeneralInsuranceLtd = 9,
        InactiveGlenrandMIBLtd = 10,
        MutualFederalInsuranceCoLtd = 11,
        NedgroupInsuranceCompanyLtd = 12,
        OutsuranceInsuranceCompanyltd = 13,
        RegentInsuranceCompanyLtd = 15,
        ZurichInsuranceCoSALtd = 16,
        SantamBeperk = 17,
        InactiveStPaulInsuranceCompanySALtd = 18,
        HollardInsuranceCompanyLtd = 19,
        SectionalTitle = 20,
        CompassInsuranceCompanyLtd = 21,
        InactiveStanbicInsurance = 22,
        PaidupwithnoHOC = 23,
        NewNationalAssuranceCompanyLtd = 24,
        LoanCancelledClosed = 25,
        InactivePriortoFormulationofAccreditedInsu = 27,
        GuardriskInsuranceCompanyLimited = 29,
        InactiveFirstforWomenInsuranceBrokersPty = 30,
        DialDirectInsuranceLtd = 32,
        LionofAfricaInsuranceCompanyLtd = 34,
        LloydsOfSouthAfricaPtyLtd = 35,
        StandardInsuranceLtd = 36,
        ConstantiaInsuranceCompanyLtd = 37,
        MomentumShortTermInsuranceCompanyLtd = 38,
        RenasaInsuranceCompanyLimited = 39,
        AlexanderForbesInsuranceCompanyLtd = 40,
        NatsureLimited = 41,
        IndequitySpecialisedInsuranceLtd = 42,
        CentriqInsuranceCompanyLimited = 43,
        ETANAInsuranceCompanyLtd = 44,
        MUAIMotorUnderwitingInsurance = 45,
        SaxumInsuranceLimited = 47,
        InfinitiInsuranceLimited = 48,
        SafireInsuranceCompanyLimited = 49,
        ResolutionInsuranceCompanyLimited = 50,
        UnityInsuranceLimited = 51,
        RMBSpecialisedLinesLimited = 52,
        WesternNationalInsuranceCompany = 53,
        MiWayInsuranceCompany = 54,
        DiscoveryInsure = 55,
        ACEInsuranceLimited = 56,
        KingPriceInsuranceCompanyLimited = 57,
        BudgetInsuranceCompanyLtd = 58,
        FirstforWomenInsuranceCompanyLimited = 59
    }

    public enum CalculatorNodeTypeEnum
    {
        ApplicationCalculator = 0,
        LeadCapture,
        ApplicationWizard,
        CreateApplication
    }

    public enum ReportTypeEnum
    {
        ITCDisputeIndicatedEng = 0,
        ITCDisputeIndicatedAfr,
    }

    public enum NodeTypeEnum
    {
        Update = 0,
        Add,
        Delete,
        View,
        Associate,
        Rollback,
        Maintain,
        Request,
    }

    public enum ACBTypeEnum
    {
        Unknown = 0,
        Current = 1,
        Savings = 2,
        Bond = 4,
        CreditCard = 5
    }

    public enum FutureDatedChangeTypeEnum
    {
        FixedDebitOrder = 1,
        NormalDebitOrder = 2
    }

    public enum ButtonTypeEnum
    {
        AddToMenu = 1,
        Add = 2,
        Remove = 3,
        Update = 4,
        View = 5,
        None = 6,
        Select = 7,
        Back = 8,
        Cancel = 9,
        Save = 10,
        Next = 11,
        AddEntry = 12,
        RemoveEntry = 13,
        Delete = 14,
        CreateApplication = 15,
        Calculate = 16,
        Finish = 17,
        CopytoDraft = 19,
        SetActive = 20,
        Done = 21,
        Preview = 22,
        SendCorrespondence = 23,
        Reasons = 24,
        Submit = 25,
        NextApplicant = 26,
        UpdateApplicant = 27,
        UpdateCalculator = 28,
        Search = 29,
        Use = 30,
        Post = 31,
        Rollback = 32,
        Proceed,
        CreateLead,
        Relate
    }

    public enum CorrespondenceTemplateEnum
    {
        MortgageLoanCancelledContinuePaying = 1,
        MortgageLoanCancelledDontContinuePaying = 2,
        DeceasedNotificationNoLiving = 3,
        DeceasedNotificationLivingExists = 4,
        SequestrationNotificationNoOthers = 5,
        SequestrationNotificationOthersExist = 6,
        AlphaHousingSurvey = 26
    }

    public enum DetailClassEnum
    {
        RegistrationProcess = 1,
        LoanManagement = 2,
        LoanIdentification = 3,
        CivilServants = 5,
        Parastatal = 6,
        Corporate = 9,
        LocalAuthorities = 11,
        CancellationFollowup = 13
    }

    public enum CancellationTypeEnum
    {
        Switch = 0,
        Sale = 1,
        Unkown = 2
    }

    public enum LegalEntityStatusEnum
    {
        Alive = 1,
        Deceased,
        Disabled
    }

    public enum RoundRobinPointerEnum
    {
        NewBusinessProcessor = 1,
        FLProcessor = 2,
        ValuationsAdministrator = 3,
        NewBusinessSupervisor = 4,
        FLSupervisor = 5,
        ValautionsManager = 6,
        QAAdministrator = 7,
        RegistrationsAdministrator = 8,
        RegistrationsLOAAdmin = 9,
        RegistrationsSupervisor = 10,
        RegistrationsManager = 11,
        FLDisbersing = 12,
        FLCollectionsAdmin = 13,
        FLAdmin = 14,
        LifeConsultant = 15,
        RVAdmin = 16,
        CreditSupervisor = 17,
        TranslateConditions = 18,
        DirectConsultant = 19,
        FLManager = 20,
        CreditUnderwritter = 21,
        CreditExceptions = 22,
        CreditManager = 23,
        FLSupervisorDisburseFunds = 24,
        PLCreditAnalyst = 25,
        PLConsultant = 26,
        PLAdmin = 27,
        PLSupervisor = 28,
        CapitecConsultant = 29
    }

    public enum LegalEntityExceptionStatusEnum
    {
        None = 0,
        Valid = 1,
        DuplicateIDNumbers,
        InvalidIDNumber
    }

    public enum FinancialAdjustmentTypeSourceEnum
    {
        CAP_FixedRateAdjustment = 1,
        SuperLo_DifferentialProvision = 2,
        Staff_InterestRateAdjustment = 3,
        DefendingCancellation_InterestRateAdjustment = 4,
        InterestOnly_InterestOnly = 5,
        DebtCounseling_PaymentAdjustment = 7,
        Origination_InterestRateAdjustment = 8,
        DebtCounseling_PaymentAdjustment_9 = 9,
        CAP2_FixedRateAdjustment = 11,
        SuspendedInterest_ReversalProvision_NonPerformingLoans = 13,
        PricingForRisk_InterestRateAdjustment = 14,
        DebtCounselling_PaymentAdjustment_15 = 15,
        DebtCounselling_PaymentAdjustment_16 = 16,
        DebtCounselling_PaymentAdjustment_17 = 17,
        DebtCounselling_PaymentAdjustment_18 = 18,
        DebtCounselling_FixedRateAdjustment = 19,
        DebtCounselling_InterestRateAdjustment = 20,
        ClientRetention_InterestRateAdjustment = 21,
        DebtCounselling_PaymentAdjustment_22 = 22,
        DebtCounselling_PaymentAdjustment_23 = 23,
        DebtCounselling_PaymentAdjustment_24 = 24,
        DebtCounselling_PaymentAdjustment_25 = 25,
        SuspendedInterest_PaymentAdjustment_25 = 26,
        AlphaHousingPricingForRisk = 33,
        SalaryDeductionPricingForRisk = 34,
        CapitecPricingForRisk = 35
    }

    public enum FinancialAdjustmentStatusEnum
    {
        Active = 1,
        Inactive = 2,
        Canceled = 3
    }

    public enum OccupancyTypeEnum
    {
        None = 0,
        OwnerOccupied,
        HolidayHome,
        Other,
        Rental,
        InvestmentProperty
    }

    public enum PropertyTypeEnum
    {
        Unknown = 1,
        House,
        Flat,
        Duplex,
        Simplex,
        ClusterHome,
        Maisonette
    }

    public enum TitleTypeEnum
    {
        Unknown = 1,
        Freehold = 2,
        SectionalTitle = 3,
        FreeholdEstate = 4,
        Shareblock = 5,
        Leasehold = 6,
        SectionalTitleWithHOC = 7
    }

    public enum AreaClassificationEnum
    {
        Unknown = 1,
        _1Class = 2,
        _2Class = 3,
        _3Class = 4,
        _4Class = 5,
        _5Class = 6,
        _6Class = 7
    }

    public enum DeedsPropertyTypeEnum
    {
        Erf = 1,
        Unit = 2
    }

    public enum DataProviderEnum
    {
        SAHL = 1,
        LightStone = 2,
        AdCheck = 3,
        RCS = 4,
        Sublime = 5,
        Santam = 6,
        Rocketseed = 7,
        Blakes = 8,
        AddedValue = 9
    }

    /// <summary>
    /// HOC Status of an HOC Account
    /// </summary>
    public enum HOCStatusEnum
    {
        Open = 1,
        PaidUpwithHOC = 2,
        PaidUpwithnoHOC = 3,
        Closed = 4
    }

    public enum ValuationClassificationEnum
    {
        None = 0,
        Budgetstandard,
        Normalstandard,
        Luxury,
        Luxurywithslatethatchroof,
        Exclusive
    }

    public enum ValuationImprovementTypeEnum
    {
        None = 0,
        Walls,
        SwimmingPool,
        TennisCourt,
        Lapa,
        Paving,
        Other
    }

    public enum ValuationRoofTypeEnum
    {
        None = 0,
        Conventional,
        Thatch,
        Other
    }

    public enum ValuationStatusEnum
    {
        None = 0,
        Pending,
        Complete,
        Withdrawn,
        Returned
    }

    public enum HOCRoofEnum
    {
        None = 0,
        Thatch,
        Conventional,
        Partial,
        Shingle
    }

    public enum RateOverrideTypeEnum
    {
        CAP = 1,
        SuperLo,
        SAHLStaff,
        DefendingCancellations,
        InterestOnly,
        CollectNoPayment,
        DiscountedLinkrate,
        CollectPartialPayment,
        NoMonthlyServiceFee,
        CAP2,
        SuperLoBreachGracePeriod,
        NonPerforming,
        RateAdjustment,
        DebtCounsellingFixedPaymentInclusiveHOC,
        DebtCounsellingFixedPaymentInclusiveLife,
        DebtCounsellingFixedPaymentInclusive,
        DebtCounsellingFixedPaymentExclusive,
        DebtCounsellingFixedRate,
        DebtCounsellingDiscountRate,
        CounterRate,
        DebtCounsellingFixedPaymentInclServiceFee,
        DebtCounsellingFixedPaymentInclHOCFees,
        DebtCounsellingFixedPaymentInclLifeFees,
        DebtCounsellingFixedPaymentInclHOCLife
    }

    public enum CorrespondenceSendMethodEnum
    {
        Fax = 0,
        Email,
        Post,
        SMS
    }

    public enum LifePolicyStatusEnum
    {
        Prospect = 1,
        Accepted = 2,
        Inforce = 3,
        CancelledfromInception = 4,
        CancelledwithProrata = 5,
        Lapsed = 11,
        Closed = 12,
        Acceptedtocommenceon1st = 13,
        ClosedSystemError = 14,
        CancelledNoRefund = 15,
        NotTakenUp = 16,
        ExternalInsurer = 17
    }

    public enum SubsidyProviderTypeEnum
    {
        CivilServants = 1,
        Parastatal,
        Corporate,
        LocalAuthorities
    }

    public enum ResidenceStatusEnum
    {
        NonResident = 1,
        Permanent,
        Temporary
    }

    public enum EmploymentSectorEnum
    {
        Construction = 5,
        FinancialServices = 7,
        PublicServices = 8,
        Health = 9,
        Legal = 10,
        Media = 11,
        RetailorWholesale = 14,
        Security = 16,
        ITandElectronics = 17,
        Transportation = 18,
        Other = 19,
        FoodandBeverage = 20,
        Manufacturing = 21,
        Property = 22,
        Tourism = 23,
        SocialServices = 24,
        PersonalServices = 25,
        Mining = 26,
        Trade = 27
    }

    public enum EmployerBusinessTypeEnum
    {
        Unknown = 1,
        Company,
        CloseCorporation,
        Trust,
        SoleProprietor
    }

    public enum AffordabilityTypeEnum
    {
        BasicSalary = 1,
        CommissionandOvertime = 2,
        Rental = 3,
        IncomefromInvestments = 4,
        OtherIncome1 = 5,
        OtherIncome2 = 6,
        StandardSalaryDeductions = 7,
        LivingExpenses = 8,
        BondPayments = 9,
        AllCarRepayments = 10,
        CreditCard = 11,
        OverdraftorPersonalLoans = 12,
        RetailAccounts = 13,
        CreditAccounts = 14,
        CommittedSavings = 15,
        OtherInstalments = 16,
        OtherExpenses = 17
    }

    public enum OfferAttributeTypeEnum
    {
        Life = 12,
        HOC = 13,
        StaffHomeLoan = 7,
        ManuallySelectedEmploymentType = 28,
        AlphaHousing = 26,
        DiscountedInitiationFee_ReturningClient = 29,
        OldMutualDeveloperLoan = 27,
        Capitec = 30,
        CapitaliseInitiationFee = 35,
        GovernmentEmployeePensionFund = 36,
        DisqualifiedForGEPF = 38
    }

    public enum BalanceTypeEnum
    {
        Loan = 1,
        LoyaltyBenefit = 2,
        SuspendedInterest = 3,
        Arrear = 4
    }

    public enum EmploymentConfirmationSourceEnum
    {
        PaperTelephoneDirectory = 1,
        TelkomEnquiries1023,
        TelkomEnquiries10118,
        ElectronicYellowPagesDirectory
    }

    public enum OriginationSourceProductEnum
    {
        SAHomeLoans_NewVariableLoan = 17,
        SAHomeLoans_PersonalLoan = 37
    }

    public enum ClaimTypeEnum
    {
        DeathClaim = 10,
        DisabilityClaim = 11,
        RetrenchmentClaim = 12,
    }

    public enum ClaimStatusEnum
    {
        Pending = 10,
        Settled = 11,
        Repudiated = 12,
        Invalid = 13
    }

    public enum ActivityTypeEnum
    {
        User = 1,
        Decision = 2,
        External = 3,
        Timed = 4,
        CallWorkFlow = 9,
        ReturnWorkflow = 10
    }

    public enum WorkflowEnum
    {
        CAP2Offers,
        DebtCounselling,
        PersonalLoans,
        ApplicationCapture,
        ApplicationManagement,
        Valuations,
        Credit,
        IT,
        ReadvancePayments,
        HelpDesk,
        LifeOrigination,
        LoanAdjustments,
        DisabilityClaim
    }

    public enum Insurer
    {
        SAHLLife = 1,
        OldMutual = 2,
        Sanlam = 3,
        ABSALife = 4,
        AfricanLife = 5,
        Aegis = 6,
        AIG = 7,
        AnchorLifeChannelLife = 8,
        AvbobLife = 9,
        BoELifeNedLife = 10,
        CapitalAllianceAALifeAGA = 11,
        CharterLife = 12,
        ClienteleLife = 13,
        CommercialUnion = 14,
        DiscoveryLife = 15,
        FNBLife = 16,
        Hollard = 17,
        KGALife = 18,
        LibertyLife = 19,
        MetropolitanLife = 20,
        MomentumLife = 21,
        PPS = 22,
        Rentmeester = 23,
        SageLife = 24,
        Samib = 25,
        StandardBankLife = 26,
        RegentLife = 27,
        Other = 28,
        CovisionLife = 31,
    }

    public enum OfferRoleAttributeTypeEnum
    {
        IncomeContributor = 1,
        ReturningClient = 2,
        MainContact = 3
    }

    public enum CacheTypes
    {
        Lookups,
        LookupItem,
        UIStatement,
        DomainService,
        Rules
    }

    public enum UserOrganisationStructureEnum
    {
        TeleConsultant = 1158
    }

    public enum AffordabilityAssessmentStressFactor
    {
        ZeroPercent = 1,
        ZeroPointFivePercent = 2,
        OnePercent = 3,
        TwoPercent = 4
    }

    public enum AffordabilityAssessmentStatusKey
    {
        Unconfirmed = 1,
        Confirmed = 2
    }

    public enum AssessmentItemTypes
    {
        BasicGrossSalary_Drawings = 1,
        Commission_Overtime = 2,
        NetRental = 3,
        Investments = 4,
        OtherIncome1 = 5,
        OtherIncome2 = 6,
        PayrollDeductions = 7,
        Accommodationexp_Rental = 8,
        Transport = 9,
        Food = 10,
        Education = 11,
        Medical = 12,
        Utilities = 13,
        ChildSupport = 14,
        OtherBonds = 15,
        Vehicle = 16,
        CreditCards = 17,
        PersonalLoans = 18,
        RetailAccounts = 19,
        OtherDebtExpenses = 20,
        SAHLBond = 21,
        HOC = 22,
        DomesticSalary = 23,
        InsurancePolicies = 24,
        CommittedSavings = 25,
        Security = 26,
        Telephone_TV = 27,
        Other = 28
    }
}