using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.X2
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string loan_adjustmentsdatamodel_selectwhere = "SELECT InstanceID, AccountKey, SPVKey, RequestUser, ProcessUser, RequestApproved, LoanAdjustmentType, NewTerm, OldTerm, GenericKey FROM [x2].[x2data].[Loan_Adjustments] WHERE";
        public const string loan_adjustmentsdatamodel_selectbykey = "SELECT InstanceID, AccountKey, SPVKey, RequestUser, ProcessUser, RequestApproved, LoanAdjustmentType, NewTerm, OldTerm, GenericKey FROM [x2].[x2data].[Loan_Adjustments] WHERE InstanceID = @PrimaryKey";
        public const string loan_adjustmentsdatamodel_delete = "DELETE FROM [x2].[x2data].[Loan_Adjustments] WHERE InstanceID = @PrimaryKey";
        public const string loan_adjustmentsdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Loan_Adjustments] WHERE";
        public const string loan_adjustmentsdatamodel_insert = "INSERT INTO [x2].[x2data].[Loan_Adjustments] (InstanceID, AccountKey, SPVKey, RequestUser, ProcessUser, RequestApproved, LoanAdjustmentType, NewTerm, OldTerm, GenericKey) VALUES(@InstanceID, @AccountKey, @SPVKey, @RequestUser, @ProcessUser, @RequestApproved, @LoanAdjustmentType, @NewTerm, @OldTerm, @GenericKey); ";
        public const string loan_adjustmentsdatamodel_update = "UPDATE [x2].[x2data].[Loan_Adjustments] SET InstanceID = @InstanceID, AccountKey = @AccountKey, SPVKey = @SPVKey, RequestUser = @RequestUser, ProcessUser = @ProcessUser, RequestApproved = @RequestApproved, LoanAdjustmentType = @LoanAdjustmentType, NewTerm = @NewTerm, OldTerm = @OldTerm, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string delete_debit_orderdatamodel_selectwhere = "SELECT InstanceID, DebitOrderKey, RequestUser, AccountKey, ProcessUser, RequestApproved FROM [x2].[x2data].[Delete_Debit_Order] WHERE";
        public const string delete_debit_orderdatamodel_selectbykey = "SELECT InstanceID, DebitOrderKey, RequestUser, AccountKey, ProcessUser, RequestApproved FROM [x2].[x2data].[Delete_Debit_Order] WHERE InstanceID = @PrimaryKey";
        public const string delete_debit_orderdatamodel_delete = "DELETE FROM [x2].[x2data].[Delete_Debit_Order] WHERE InstanceID = @PrimaryKey";
        public const string delete_debit_orderdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Delete_Debit_Order] WHERE";
        public const string delete_debit_orderdatamodel_insert = "INSERT INTO [x2].[x2data].[Delete_Debit_Order] (InstanceID, DebitOrderKey, RequestUser, AccountKey, ProcessUser, RequestApproved) VALUES(@InstanceID, @DebitOrderKey, @RequestUser, @AccountKey, @ProcessUser, @RequestApproved); ";
        public const string delete_debit_orderdatamodel_update = "UPDATE [x2].[x2data].[Delete_Debit_Order] SET InstanceID = @InstanceID, DebitOrderKey = @DebitOrderKey, RequestUser = @RequestUser, AccountKey = @AccountKey, ProcessUser = @ProcessUser, RequestApproved = @RequestApproved WHERE InstanceID = @InstanceID";



        public const string debt_counsellingdatamodel_selectwhere = "SELECT InstanceID, DebtCounsellingKey, AccountKey, SentToLitigation, AssignADUserName, AssignWorkflowRoleTypeKey, PreviousState, CourtCase, LatestReasonDefinitionKey, ProductKey, GenericKey FROM [x2].[x2data].[Debt_Counselling] WHERE";
        public const string debt_counsellingdatamodel_selectbykey = "SELECT InstanceID, DebtCounsellingKey, AccountKey, SentToLitigation, AssignADUserName, AssignWorkflowRoleTypeKey, PreviousState, CourtCase, LatestReasonDefinitionKey, ProductKey, GenericKey FROM [x2].[x2data].[Debt_Counselling] WHERE InstanceID = @PrimaryKey";
        public const string debt_counsellingdatamodel_delete = "DELETE FROM [x2].[x2data].[Debt_Counselling] WHERE InstanceID = @PrimaryKey";
        public const string debt_counsellingdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Debt_Counselling] WHERE";
        public const string debt_counsellingdatamodel_insert = "INSERT INTO [x2].[x2data].[Debt_Counselling] (InstanceID, DebtCounsellingKey, AccountKey, SentToLitigation, AssignADUserName, AssignWorkflowRoleTypeKey, PreviousState, CourtCase, LatestReasonDefinitionKey, ProductKey, GenericKey) VALUES(@InstanceID, @DebtCounsellingKey, @AccountKey, @SentToLitigation, @AssignADUserName, @AssignWorkflowRoleTypeKey, @PreviousState, @CourtCase, @LatestReasonDefinitionKey, @ProductKey, @GenericKey); ";
        public const string debt_counsellingdatamodel_update = "UPDATE [x2].[x2data].[Debt_Counselling] SET InstanceID = @InstanceID, DebtCounsellingKey = @DebtCounsellingKey, AccountKey = @AccountKey, SentToLitigation = @SentToLitigation, AssignADUserName = @AssignADUserName, AssignWorkflowRoleTypeKey = @AssignWorkflowRoleTypeKey, PreviousState = @PreviousState, CourtCase = @CourtCase, LatestReasonDefinitionKey = @LatestReasonDefinitionKey, ProductKey = @ProductKey, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string personal_loansdatamodel_selectwhere = "SELECT InstanceID, ApplicationKey, PreviousState, GenericKey FROM [x2].[x2data].[Personal_Loans] WHERE";
        public const string personal_loansdatamodel_selectbykey = "SELECT InstanceID, ApplicationKey, PreviousState, GenericKey FROM [x2].[x2data].[Personal_Loans] WHERE InstanceID = @PrimaryKey";
        public const string personal_loansdatamodel_delete = "DELETE FROM [x2].[x2data].[Personal_Loans] WHERE InstanceID = @PrimaryKey";
        public const string personal_loansdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Personal_Loans] WHERE";
        public const string personal_loansdatamodel_insert = "INSERT INTO [x2].[x2data].[Personal_Loans] (InstanceID, ApplicationKey, PreviousState, GenericKey) VALUES(@InstanceID, @ApplicationKey, @PreviousState, @GenericKey); ";
        public const string personal_loansdatamodel_update = "UPDATE [x2].[x2data].[Personal_Loans] SET InstanceID = @InstanceID, ApplicationKey = @ApplicationKey, PreviousState = @PreviousState, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string cap2_offersdatamodel_selectwhere = "SELECT InstanceID, CapBroker, CapOfferKey, CapOfferDetailKey, LegalEntityName, AccountKey, CapNTUReasonKey, Promotion, CapExpireDate, CapCreditBroker, CapPaymentOptionKey, CapStatusKey, GenericKey, Last_State FROM [x2].[x2data].[CAP2_Offers] WHERE";
        public const string cap2_offersdatamodel_selectbykey = "SELECT InstanceID, CapBroker, CapOfferKey, CapOfferDetailKey, LegalEntityName, AccountKey, CapNTUReasonKey, Promotion, CapExpireDate, CapCreditBroker, CapPaymentOptionKey, CapStatusKey, GenericKey, Last_State FROM [x2].[x2data].[CAP2_Offers] WHERE InstanceID = @PrimaryKey";
        public const string cap2_offersdatamodel_delete = "DELETE FROM [x2].[x2data].[CAP2_Offers] WHERE InstanceID = @PrimaryKey";
        public const string cap2_offersdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[CAP2_Offers] WHERE";
        public const string cap2_offersdatamodel_insert = "INSERT INTO [x2].[x2data].[CAP2_Offers] (InstanceID, CapBroker, CapOfferKey, CapOfferDetailKey, LegalEntityName, AccountKey, CapNTUReasonKey, Promotion, CapExpireDate, CapCreditBroker, CapPaymentOptionKey, CapStatusKey, GenericKey, Last_State) VALUES(@InstanceID, @CapBroker, @CapOfferKey, @CapOfferDetailKey, @LegalEntityName, @AccountKey, @CapNTUReasonKey, @Promotion, @CapExpireDate, @CapCreditBroker, @CapPaymentOptionKey, @CapStatusKey, @GenericKey, @Last_State); ";
        public const string cap2_offersdatamodel_update = "UPDATE [x2].[x2data].[CAP2_Offers] SET InstanceID = @InstanceID, CapBroker = @CapBroker, CapOfferKey = @CapOfferKey, CapOfferDetailKey = @CapOfferDetailKey, LegalEntityName = @LegalEntityName, AccountKey = @AccountKey, CapNTUReasonKey = @CapNTUReasonKey, Promotion = @Promotion, CapExpireDate = @CapExpireDate, CapCreditBroker = @CapCreditBroker, CapPaymentOptionKey = @CapPaymentOptionKey, CapStatusKey = @CapStatusKey, GenericKey = @GenericKey, Last_State = @Last_State WHERE InstanceID = @InstanceID";



        public const string disability_claimdatamodel_selectwhere = "SELECT InstanceID, DisabilityClaimKey, MigrationTargetState, GenericKey FROM [x2].[x2data].[Disability_Claim] WHERE";
        public const string disability_claimdatamodel_selectbykey = "SELECT InstanceID, DisabilityClaimKey, MigrationTargetState, GenericKey FROM [x2].[x2data].[Disability_Claim] WHERE InstanceID = @PrimaryKey";
        public const string disability_claimdatamodel_delete = "DELETE FROM [x2].[x2data].[Disability_Claim] WHERE InstanceID = @PrimaryKey";
        public const string disability_claimdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Disability_Claim] WHERE";
        public const string disability_claimdatamodel_insert = "INSERT INTO [x2].[x2data].[Disability_Claim] (InstanceID, DisabilityClaimKey, MigrationTargetState, GenericKey) VALUES(@InstanceID, @DisabilityClaimKey, @MigrationTargetState, @GenericKey); ";
        public const string disability_claimdatamodel_update = "UPDATE [x2].[x2data].[Disability_Claim] SET InstanceID = @InstanceID, DisabilityClaimKey = @DisabilityClaimKey, MigrationTargetState = @MigrationTargetState, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string lifeoriginationdatamodel_selectwhere = "SELECT InstanceID, LastState, OfferKey, BenefitsDone, ExclusionsDone, RPARDone, DeclarationDone, FAISDone, RPARInsurer, LastNTUState, ConfirmationRequired, ExclusionsConfirmationDone, DeclarationConfirmationDone, FAISConfirmationDone, ContactNumber, LoanNumber, AssignTo, PolicyFinancialServiceKey, PolicyTypeKey, BenefitsConfirmProceed, BenefitsConfirmRefused, GenericKey FROM [x2].[x2data].[LifeOrigination] WHERE";
        public const string lifeoriginationdatamodel_selectbykey = "SELECT InstanceID, LastState, OfferKey, BenefitsDone, ExclusionsDone, RPARDone, DeclarationDone, FAISDone, RPARInsurer, LastNTUState, ConfirmationRequired, ExclusionsConfirmationDone, DeclarationConfirmationDone, FAISConfirmationDone, ContactNumber, LoanNumber, AssignTo, PolicyFinancialServiceKey, PolicyTypeKey, BenefitsConfirmProceed, BenefitsConfirmRefused, GenericKey FROM [x2].[x2data].[LifeOrigination] WHERE InstanceID = @PrimaryKey";
        public const string lifeoriginationdatamodel_delete = "DELETE FROM [x2].[x2data].[LifeOrigination] WHERE InstanceID = @PrimaryKey";
        public const string lifeoriginationdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[LifeOrigination] WHERE";
        public const string lifeoriginationdatamodel_insert = "INSERT INTO [x2].[x2data].[LifeOrigination] (InstanceID, LastState, OfferKey, BenefitsDone, ExclusionsDone, RPARDone, DeclarationDone, FAISDone, RPARInsurer, LastNTUState, ConfirmationRequired, ExclusionsConfirmationDone, DeclarationConfirmationDone, FAISConfirmationDone, ContactNumber, LoanNumber, AssignTo, PolicyFinancialServiceKey, PolicyTypeKey, BenefitsConfirmProceed, BenefitsConfirmRefused, GenericKey) VALUES(@InstanceID, @LastState, @OfferKey, @BenefitsDone, @ExclusionsDone, @RPARDone, @DeclarationDone, @FAISDone, @RPARInsurer, @LastNTUState, @ConfirmationRequired, @ExclusionsConfirmationDone, @DeclarationConfirmationDone, @FAISConfirmationDone, @ContactNumber, @LoanNumber, @AssignTo, @PolicyFinancialServiceKey, @PolicyTypeKey, @BenefitsConfirmProceed, @BenefitsConfirmRefused, @GenericKey); ";
        public const string lifeoriginationdatamodel_update = "UPDATE [x2].[x2data].[LifeOrigination] SET InstanceID = @InstanceID, LastState = @LastState, OfferKey = @OfferKey, BenefitsDone = @BenefitsDone, ExclusionsDone = @ExclusionsDone, RPARDone = @RPARDone, DeclarationDone = @DeclarationDone, FAISDone = @FAISDone, RPARInsurer = @RPARInsurer, LastNTUState = @LastNTUState, ConfirmationRequired = @ConfirmationRequired, ExclusionsConfirmationDone = @ExclusionsConfirmationDone, DeclarationConfirmationDone = @DeclarationConfirmationDone, FAISConfirmationDone = @FAISConfirmationDone, ContactNumber = @ContactNumber, LoanNumber = @LoanNumber, AssignTo = @AssignTo, PolicyFinancialServiceKey = @PolicyFinancialServiceKey, PolicyTypeKey = @PolicyTypeKey, BenefitsConfirmProceed = @BenefitsConfirmProceed, BenefitsConfirmRefused = @BenefitsConfirmRefused, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string readvance_paymentsdatamodel_selectwhere = "SELECT InstanceID, ApplicationKey, PreviousState, GenericKey, EntryPath FROM [x2].[x2data].[Readvance_Payments] WHERE";
        public const string readvance_paymentsdatamodel_selectbykey = "SELECT InstanceID, ApplicationKey, PreviousState, GenericKey, EntryPath FROM [x2].[x2data].[Readvance_Payments] WHERE InstanceID = @PrimaryKey";
        public const string readvance_paymentsdatamodel_delete = "DELETE FROM [x2].[x2data].[Readvance_Payments] WHERE InstanceID = @PrimaryKey";
        public const string readvance_paymentsdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Readvance_Payments] WHERE";
        public const string readvance_paymentsdatamodel_insert = "INSERT INTO [x2].[x2data].[Readvance_Payments] (InstanceID, ApplicationKey, PreviousState, GenericKey, EntryPath) VALUES(@InstanceID, @ApplicationKey, @PreviousState, @GenericKey, @EntryPath); ";
        public const string readvance_paymentsdatamodel_update = "UPDATE [x2].[x2data].[Readvance_Payments] SET InstanceID = @InstanceID, ApplicationKey = @ApplicationKey, PreviousState = @PreviousState, GenericKey = @GenericKey, EntryPath = @EntryPath WHERE InstanceID = @InstanceID";



        public const string quick_cashdatamodel_selectwhere = "SELECT InstanceID, ApplicationKey, PreviousState FROM [x2].[x2data].[Quick_Cash] WHERE";
        public const string quick_cashdatamodel_selectbykey = "SELECT InstanceID, ApplicationKey, PreviousState FROM [x2].[x2data].[Quick_Cash] WHERE InstanceID = @PrimaryKey";
        public const string quick_cashdatamodel_delete = "DELETE FROM [x2].[x2data].[Quick_Cash] WHERE InstanceID = @PrimaryKey";
        public const string quick_cashdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Quick_Cash] WHERE";
        public const string quick_cashdatamodel_insert = "INSERT INTO [x2].[x2data].[Quick_Cash] (InstanceID, ApplicationKey, PreviousState) VALUES(@InstanceID, @ApplicationKey, @PreviousState); ";
        public const string quick_cashdatamodel_update = "UPDATE [x2].[x2data].[Quick_Cash] SET InstanceID = @InstanceID, ApplicationKey = @ApplicationKey, PreviousState = @PreviousState WHERE InstanceID = @InstanceID";



        public const string creditdatamodel_selectwhere = "SELECT InstanceID, ApplicationKey, IsResub, ActionSource, PreviousState, ReviewRequired, StopRecursing, EntryPath, ExceptionsDeclineWithOffer, PolicyOverride, Is2ndPass, GenericKey FROM [x2].[x2data].[Credit] WHERE";
        public const string creditdatamodel_selectbykey = "SELECT InstanceID, ApplicationKey, IsResub, ActionSource, PreviousState, ReviewRequired, StopRecursing, EntryPath, ExceptionsDeclineWithOffer, PolicyOverride, Is2ndPass, GenericKey FROM [x2].[x2data].[Credit] WHERE InstanceID = @PrimaryKey";
        public const string creditdatamodel_delete = "DELETE FROM [x2].[x2data].[Credit] WHERE InstanceID = @PrimaryKey";
        public const string creditdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Credit] WHERE";
        public const string creditdatamodel_insert = "INSERT INTO [x2].[x2data].[Credit] (InstanceID, ApplicationKey, IsResub, ActionSource, PreviousState, ReviewRequired, StopRecursing, EntryPath, ExceptionsDeclineWithOffer, PolicyOverride, Is2ndPass, GenericKey) VALUES(@InstanceID, @ApplicationKey, @IsResub, @ActionSource, @PreviousState, @ReviewRequired, @StopRecursing, @EntryPath, @ExceptionsDeclineWithOffer, @PolicyOverride, @Is2ndPass, @GenericKey); ";
        public const string creditdatamodel_update = "UPDATE [x2].[x2data].[Credit] SET InstanceID = @InstanceID, ApplicationKey = @ApplicationKey, IsResub = @IsResub, ActionSource = @ActionSource, PreviousState = @PreviousState, ReviewRequired = @ReviewRequired, StopRecursing = @StopRecursing, EntryPath = @EntryPath, ExceptionsDeclineWithOffer = @ExceptionsDeclineWithOffer, PolicyOverride = @PolicyOverride, Is2ndPass = @Is2ndPass, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string valuationsdatamodel_selectwhere = "SELECT InstanceID, ApplicationKey, PropertyKey, Withdrawn, RequestingAdUser, AdcheckPropertyID, AdcheckValuationID, ValuationKey, AdCheckValuationIDStatus, EntryPath, OnManagerWorkList, nLoops, ValuationDataProviderDataServiceKey, LightstonePropertyID, IsReview, GenericKey FROM [x2].[x2data].[Valuations] WHERE";
        public const string valuationsdatamodel_selectbykey = "SELECT InstanceID, ApplicationKey, PropertyKey, Withdrawn, RequestingAdUser, AdcheckPropertyID, AdcheckValuationID, ValuationKey, AdCheckValuationIDStatus, EntryPath, OnManagerWorkList, nLoops, ValuationDataProviderDataServiceKey, LightstonePropertyID, IsReview, GenericKey FROM [x2].[x2data].[Valuations] WHERE InstanceID = @PrimaryKey";
        public const string valuationsdatamodel_delete = "DELETE FROM [x2].[x2data].[Valuations] WHERE InstanceID = @PrimaryKey";
        public const string valuationsdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Valuations] WHERE";
        public const string valuationsdatamodel_insert = "INSERT INTO [x2].[x2data].[Valuations] (InstanceID, ApplicationKey, PropertyKey, Withdrawn, RequestingAdUser, AdcheckPropertyID, AdcheckValuationID, ValuationKey, AdCheckValuationIDStatus, EntryPath, OnManagerWorkList, nLoops, ValuationDataProviderDataServiceKey, LightstonePropertyID, IsReview, GenericKey) VALUES(@InstanceID, @ApplicationKey, @PropertyKey, @Withdrawn, @RequestingAdUser, @AdcheckPropertyID, @AdcheckValuationID, @ValuationKey, @AdCheckValuationIDStatus, @EntryPath, @OnManagerWorkList, @nLoops, @ValuationDataProviderDataServiceKey, @LightstonePropertyID, @IsReview, @GenericKey); ";
        public const string valuationsdatamodel_update = "UPDATE [x2].[x2data].[Valuations] SET InstanceID = @InstanceID, ApplicationKey = @ApplicationKey, PropertyKey = @PropertyKey, Withdrawn = @Withdrawn, RequestingAdUser = @RequestingAdUser, AdcheckPropertyID = @AdcheckPropertyID, AdcheckValuationID = @AdcheckValuationID, ValuationKey = @ValuationKey, AdCheckValuationIDStatus = @AdCheckValuationIDStatus, EntryPath = @EntryPath, OnManagerWorkList = @OnManagerWorkList, nLoops = @nLoops, ValuationDataProviderDataServiceKey = @ValuationDataProviderDataServiceKey, LightstonePropertyID = @LightstonePropertyID, IsReview = @IsReview, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string release_and_variationsdatamodel_selectwhere = "SELECT InstanceID, ApplicationKey, IsFromFL, PreviousState FROM [x2].[x2data].[Release_And_Variations] WHERE";
        public const string release_and_variationsdatamodel_selectbykey = "SELECT InstanceID, ApplicationKey, IsFromFL, PreviousState FROM [x2].[x2data].[Release_And_Variations] WHERE InstanceID = @PrimaryKey";
        public const string release_and_variationsdatamodel_delete = "DELETE FROM [x2].[x2data].[Release_And_Variations] WHERE InstanceID = @PrimaryKey";
        public const string release_and_variationsdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Release_And_Variations] WHERE";
        public const string release_and_variationsdatamodel_insert = "INSERT INTO [x2].[x2data].[Release_And_Variations] (InstanceID, ApplicationKey, IsFromFL, PreviousState) VALUES(@InstanceID, @ApplicationKey, @IsFromFL, @PreviousState); ";
        public const string release_and_variationsdatamodel_update = "UPDATE [x2].[x2data].[Release_And_Variations] SET InstanceID = @InstanceID, ApplicationKey = @ApplicationKey, IsFromFL = @IsFromFL, PreviousState = @PreviousState WHERE InstanceID = @InstanceID";



        public const string application_managementdatamodel_selectwhere = "SELECT InstanceID, ApplicationKey, PreviousState, GenericKey, CaseOwnerName, IsFL, EWorkFolderID, IsResub, OfferTypeKey, AppCapIID, RequireValuation, AlphaHousingSurveyEmailSent FROM [x2].[x2data].[Application_Management] WHERE";
        public const string application_managementdatamodel_selectbykey = "SELECT InstanceID, ApplicationKey, PreviousState, GenericKey, CaseOwnerName, IsFL, EWorkFolderID, IsResub, OfferTypeKey, AppCapIID, RequireValuation, AlphaHousingSurveyEmailSent FROM [x2].[x2data].[Application_Management] WHERE InstanceID = @PrimaryKey";
        public const string application_managementdatamodel_delete = "DELETE FROM [x2].[x2data].[Application_Management] WHERE InstanceID = @PrimaryKey";
        public const string application_managementdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Application_Management] WHERE";
        public const string application_managementdatamodel_insert = "INSERT INTO [x2].[x2data].[Application_Management] (InstanceID, ApplicationKey, PreviousState, GenericKey, CaseOwnerName, IsFL, EWorkFolderID, IsResub, OfferTypeKey, AppCapIID, RequireValuation, AlphaHousingSurveyEmailSent) VALUES(@InstanceID, @ApplicationKey, @PreviousState, @GenericKey, @CaseOwnerName, @IsFL, @EWorkFolderID, @IsResub, @OfferTypeKey, @AppCapIID, @RequireValuation, @AlphaHousingSurveyEmailSent); ";
        public const string application_managementdatamodel_update = "UPDATE [x2].[x2data].[Application_Management] SET InstanceID = @InstanceID, ApplicationKey = @ApplicationKey, PreviousState = @PreviousState, GenericKey = @GenericKey, CaseOwnerName = @CaseOwnerName, IsFL = @IsFL, EWorkFolderID = @EWorkFolderID, IsResub = @IsResub, OfferTypeKey = @OfferTypeKey, AppCapIID = @AppCapIID, RequireValuation = @RequireValuation, AlphaHousingSurveyEmailSent = @AlphaHousingSurveyEmailSent WHERE InstanceID = @InstanceID";



        public const string application_capturedatamodel_selectwhere = "SELECT InstanceID, ApplicationKey, Last_State, Last_NTU_State, LeadType, GenericKey, CaseOwnerName, AdminUserName, IsEA, isEstateAgentApplication, NetLeadXML FROM [x2].[x2data].[Application_Capture] WHERE";
        public const string application_capturedatamodel_selectbykey = "SELECT InstanceID, ApplicationKey, Last_State, Last_NTU_State, LeadType, GenericKey, CaseOwnerName, AdminUserName, IsEA, isEstateAgentApplication, NetLeadXML FROM [x2].[x2data].[Application_Capture] WHERE InstanceID = @PrimaryKey";
        public const string application_capturedatamodel_delete = "DELETE FROM [x2].[x2data].[Application_Capture] WHERE InstanceID = @PrimaryKey";
        public const string application_capturedatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Application_Capture] WHERE";
        public const string application_capturedatamodel_insert = "INSERT INTO [x2].[x2data].[Application_Capture] (InstanceID, ApplicationKey, Last_State, Last_NTU_State, LeadType, GenericKey, CaseOwnerName, AdminUserName, IsEA, isEstateAgentApplication, NetLeadXML) VALUES(@InstanceID, @ApplicationKey, @Last_State, @Last_NTU_State, @LeadType, @GenericKey, @CaseOwnerName, @AdminUserName, @IsEA, @isEstateAgentApplication, @NetLeadXML); ";
        public const string application_capturedatamodel_update = "UPDATE [x2].[x2data].[Application_Capture] SET InstanceID = @InstanceID, ApplicationKey = @ApplicationKey, Last_State = @Last_State, Last_NTU_State = @Last_NTU_State, LeadType = @LeadType, GenericKey = @GenericKey, CaseOwnerName = @CaseOwnerName, AdminUserName = @AdminUserName, IsEA = @IsEA, isEstateAgentApplication = @isEstateAgentApplication, NetLeadXML = @NetLeadXML WHERE InstanceID = @InstanceID";



        public const string itdatamodel_selectwhere = "SELECT InstanceID, [Key], [User] FROM [x2].[x2data].[IT] WHERE";
        public const string itdatamodel_selectbykey = "SELECT InstanceID, [Key], [User] FROM [x2].[x2data].[IT] WHERE InstanceID = @PrimaryKey";
        public const string itdatamodel_delete = "DELETE FROM [x2].[x2data].[IT] WHERE InstanceID = @PrimaryKey";
        public const string itdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[IT] WHERE";
        public const string itdatamodel_insert = "INSERT INTO [x2].[x2data].[IT] (InstanceID, [Key], [User]) VALUES(@InstanceID, @Key, @User); ";
        public const string itdatamodel_update = "UPDATE [x2].[x2data].[IT] SET InstanceID = @InstanceID, [Key] = @Key, [User] = @User WHERE InstanceID = @InstanceID";



        public const string third_party_invoicesdatamodel_selectwhere = "SELECT InstanceID, ThirdPartyInvoiceKey, AccountKey, ThirdPartyTypeKey, GenericKey FROM [x2].[x2data].[Third_Party_Invoices] WHERE";
        public const string third_party_invoicesdatamodel_selectbykey = "SELECT InstanceID, ThirdPartyInvoiceKey, AccountKey, ThirdPartyTypeKey, GenericKey FROM [x2].[x2data].[Third_Party_Invoices] WHERE InstanceID = @PrimaryKey";
        public const string third_party_invoicesdatamodel_delete = "DELETE FROM [x2].[x2data].[Third_Party_Invoices] WHERE InstanceID = @PrimaryKey";
        public const string third_party_invoicesdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Third_Party_Invoices] WHERE";
        public const string third_party_invoicesdatamodel_insert = "INSERT INTO [x2].[x2data].[Third_Party_Invoices] (InstanceID, ThirdPartyInvoiceKey, AccountKey, ThirdPartyTypeKey, GenericKey) VALUES(@InstanceID, @ThirdPartyInvoiceKey, @AccountKey, @ThirdPartyTypeKey, @GenericKey); ";
        public const string third_party_invoicesdatamodel_update = "UPDATE [x2].[x2data].[Third_Party_Invoices] SET InstanceID = @InstanceID, ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey, AccountKey = @AccountKey, ThirdPartyTypeKey = @ThirdPartyTypeKey, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string help_deskdatamodel_selectwhere = "SELECT InstanceID, LegalEntityKey, CurrentConsultant, HelpDeskQueryKey, GenericKey FROM [x2].[x2data].[Help_Desk] WHERE";
        public const string help_deskdatamodel_selectbykey = "SELECT InstanceID, LegalEntityKey, CurrentConsultant, HelpDeskQueryKey, GenericKey FROM [x2].[x2data].[Help_Desk] WHERE InstanceID = @PrimaryKey";
        public const string help_deskdatamodel_delete = "DELETE FROM [x2].[x2data].[Help_Desk] WHERE InstanceID = @PrimaryKey";
        public const string help_deskdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[Help_Desk] WHERE";
        public const string help_deskdatamodel_insert = "INSERT INTO [x2].[x2data].[Help_Desk] (InstanceID, LegalEntityKey, CurrentConsultant, HelpDeskQueryKey, GenericKey) VALUES(@InstanceID, @LegalEntityKey, @CurrentConsultant, @HelpDeskQueryKey, @GenericKey); ";
        public const string help_deskdatamodel_update = "UPDATE [x2].[x2data].[Help_Desk] SET InstanceID = @InstanceID, LegalEntityKey = @LegalEntityKey, CurrentConsultant = @CurrentConsultant, HelpDeskQueryKey = @HelpDeskQueryKey, GenericKey = @GenericKey WHERE InstanceID = @InstanceID";



        public const string interestonlysmsdatamodel_selectwhere = "SELECT InstanceID, BondAmount, BondType, CallBackAt, CellNumber, Consultant, DeclineReason, EstimatedAMInstallment, EstimatedInstallment, ExistingClient, FirstNames, HomeAddressLine1, HomeAddressLine2, HomeAddressLine3, HomeAddressLine4, HomeContactNo, LastName, LoanTerm, OutstandingBalance, RequiredAmount, Salutation, TransferBranch, TransferRegion, TransferType, WorkContactNo FROM [x2].[x2data].[InterestOnlySMS] WHERE";
        public const string interestonlysmsdatamodel_selectbykey = "SELECT InstanceID, BondAmount, BondType, CallBackAt, CellNumber, Consultant, DeclineReason, EstimatedAMInstallment, EstimatedInstallment, ExistingClient, FirstNames, HomeAddressLine1, HomeAddressLine2, HomeAddressLine3, HomeAddressLine4, HomeContactNo, LastName, LoanTerm, OutstandingBalance, RequiredAmount, Salutation, TransferBranch, TransferRegion, TransferType, WorkContactNo FROM [x2].[x2data].[InterestOnlySMS] WHERE InstanceID = @PrimaryKey";
        public const string interestonlysmsdatamodel_delete = "DELETE FROM [x2].[x2data].[InterestOnlySMS] WHERE InstanceID = @PrimaryKey";
        public const string interestonlysmsdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[InterestOnlySMS] WHERE";
        public const string interestonlysmsdatamodel_insert = "INSERT INTO [x2].[x2data].[InterestOnlySMS] (InstanceID, BondAmount, BondType, CallBackAt, CellNumber, Consultant, DeclineReason, EstimatedAMInstallment, EstimatedInstallment, ExistingClient, FirstNames, HomeAddressLine1, HomeAddressLine2, HomeAddressLine3, HomeAddressLine4, HomeContactNo, LastName, LoanTerm, OutstandingBalance, RequiredAmount, Salutation, TransferBranch, TransferRegion, TransferType, WorkContactNo) VALUES(@InstanceID, @BondAmount, @BondType, @CallBackAt, @CellNumber, @Consultant, @DeclineReason, @EstimatedAMInstallment, @EstimatedInstallment, @ExistingClient, @FirstNames, @HomeAddressLine1, @HomeAddressLine2, @HomeAddressLine3, @HomeAddressLine4, @HomeContactNo, @LastName, @LoanTerm, @OutstandingBalance, @RequiredAmount, @Salutation, @TransferBranch, @TransferRegion, @TransferType, @WorkContactNo); ";
        public const string interestonlysmsdatamodel_update = "UPDATE [x2].[x2data].[InterestOnlySMS] SET InstanceID = @InstanceID, BondAmount = @BondAmount, BondType = @BondType, CallBackAt = @CallBackAt, CellNumber = @CellNumber, Consultant = @Consultant, DeclineReason = @DeclineReason, EstimatedAMInstallment = @EstimatedAMInstallment, EstimatedInstallment = @EstimatedInstallment, ExistingClient = @ExistingClient, FirstNames = @FirstNames, HomeAddressLine1 = @HomeAddressLine1, HomeAddressLine2 = @HomeAddressLine2, HomeAddressLine3 = @HomeAddressLine3, HomeAddressLine4 = @HomeAddressLine4, HomeContactNo = @HomeContactNo, LastName = @LastName, LoanTerm = @LoanTerm, OutstandingBalance = @OutstandingBalance, RequiredAmount = @RequiredAmount, Salutation = @Salutation, TransferBranch = @TransferBranch, TransferRegion = @TransferRegion, TransferType = @TransferType, WorkContactNo = @WorkContactNo WHERE InstanceID = @InstanceID";



        public const string rcsdatamodel_selectwhere = "SELECT InstanceID, FromStage, IncomeConfirmed, MoveToStage, Reference, RegistrationValidationDate, UserName, ValidDisbursement, ValuationReceived, offerKey, accountKey FROM [x2].[x2data].[RCS] WHERE";
        public const string rcsdatamodel_selectbykey = "SELECT InstanceID, FromStage, IncomeConfirmed, MoveToStage, Reference, RegistrationValidationDate, UserName, ValidDisbursement, ValuationReceived, offerKey, accountKey FROM [x2].[x2data].[RCS] WHERE InstanceID = @PrimaryKey";
        public const string rcsdatamodel_delete = "DELETE FROM [x2].[x2data].[RCS] WHERE InstanceID = @PrimaryKey";
        public const string rcsdatamodel_deletewhere = "DELETE FROM [x2].[x2data].[RCS] WHERE";
        public const string rcsdatamodel_insert = "INSERT INTO [x2].[x2data].[RCS] (InstanceID, FromStage, IncomeConfirmed, MoveToStage, Reference, RegistrationValidationDate, UserName, ValidDisbursement, ValuationReceived, offerKey, accountKey) VALUES(@InstanceID, @FromStage, @IncomeConfirmed, @MoveToStage, @Reference, @RegistrationValidationDate, @UserName, @ValidDisbursement, @ValuationReceived, @offerKey, @accountKey); ";
        public const string rcsdatamodel_update = "UPDATE [x2].[x2data].[RCS] SET InstanceID = @InstanceID, FromStage = @FromStage, IncomeConfirmed = @IncomeConfirmed, MoveToStage = @MoveToStage, Reference = @Reference, RegistrationValidationDate = @RegistrationValidationDate, UserName = @UserName, ValidDisbursement = @ValidDisbursement, ValuationReceived = @ValuationReceived, offerKey = @offerKey, accountKey = @accountKey WHERE InstanceID = @InstanceID";



    }
}