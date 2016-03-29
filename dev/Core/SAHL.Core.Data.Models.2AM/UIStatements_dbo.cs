using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string offerconditiontokendatamodel_selectwhere = "SELECT OfferConditionTokenKey, TokenKey, OfferConditionKey, TranslatableItemKey, TokenValue FROM [2am].[dbo].[OfferConditionToken] WHERE";
        public const string offerconditiontokendatamodel_selectbykey = "SELECT OfferConditionTokenKey, TokenKey, OfferConditionKey, TranslatableItemKey, TokenValue FROM [2am].[dbo].[OfferConditionToken] WHERE OfferConditionTokenKey = @PrimaryKey";
        public const string offerconditiontokendatamodel_delete = "DELETE FROM [2am].[dbo].[OfferConditionToken] WHERE OfferConditionTokenKey = @PrimaryKey";
        public const string offerconditiontokendatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferConditionToken] WHERE";
        public const string offerconditiontokendatamodel_insert = "INSERT INTO [2am].[dbo].[OfferConditionToken] (TokenKey, OfferConditionKey, TranslatableItemKey, TokenValue) VALUES(@TokenKey, @OfferConditionKey, @TranslatableItemKey, @TokenValue); select cast(scope_identity() as int)";
        public const string offerconditiontokendatamodel_update = "UPDATE [2am].[dbo].[OfferConditionToken] SET TokenKey = @TokenKey, OfferConditionKey = @OfferConditionKey, TranslatableItemKey = @TranslatableItemKey, TokenValue = @TokenValue WHERE OfferConditionTokenKey = @OfferConditionTokenKey";



        public const string futuredatedchangetypedatamodel_selectwhere = "SELECT FutureDatedChangeTypeKey, Description FROM [2am].[dbo].[FutureDatedChangeType] WHERE";
        public const string futuredatedchangetypedatamodel_selectbykey = "SELECT FutureDatedChangeTypeKey, Description FROM [2am].[dbo].[FutureDatedChangeType] WHERE FutureDatedChangeTypeKey = @PrimaryKey";
        public const string futuredatedchangetypedatamodel_delete = "DELETE FROM [2am].[dbo].[FutureDatedChangeType] WHERE FutureDatedChangeTypeKey = @PrimaryKey";
        public const string futuredatedchangetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FutureDatedChangeType] WHERE";
        public const string futuredatedchangetypedatamodel_insert = "INSERT INTO [2am].[dbo].[FutureDatedChangeType] (FutureDatedChangeTypeKey, Description) VALUES(@FutureDatedChangeTypeKey, @Description); ";
        public const string futuredatedchangetypedatamodel_update = "UPDATE [2am].[dbo].[FutureDatedChangeType] SET FutureDatedChangeTypeKey = @FutureDatedChangeTypeKey, Description = @Description WHERE FutureDatedChangeTypeKey = @FutureDatedChangeTypeKey";



        public const string helpdeskquerydatamodel_selectwhere = "SELECT HelpDeskQueryKey, HelpDeskCategoryKey, Description, InsertDate, MemoKey, ResolvedDate FROM [2am].[dbo].[HelpDeskQuery] WHERE";
        public const string helpdeskquerydatamodel_selectbykey = "SELECT HelpDeskQueryKey, HelpDeskCategoryKey, Description, InsertDate, MemoKey, ResolvedDate FROM [2am].[dbo].[HelpDeskQuery] WHERE HelpDeskQueryKey = @PrimaryKey";
        public const string helpdeskquerydatamodel_delete = "DELETE FROM [2am].[dbo].[HelpDeskQuery] WHERE HelpDeskQueryKey = @PrimaryKey";
        public const string helpdeskquerydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HelpDeskQuery] WHERE";
        public const string helpdeskquerydatamodel_insert = "INSERT INTO [2am].[dbo].[HelpDeskQuery] (HelpDeskCategoryKey, Description, InsertDate, MemoKey, ResolvedDate) VALUES(@HelpDeskCategoryKey, @Description, @InsertDate, @MemoKey, @ResolvedDate); select cast(scope_identity() as int)";
        public const string helpdeskquerydatamodel_update = "UPDATE [2am].[dbo].[HelpDeskQuery] SET HelpDeskCategoryKey = @HelpDeskCategoryKey, Description = @Description, InsertDate = @InsertDate, MemoKey = @MemoKey, ResolvedDate = @ResolvedDate WHERE HelpDeskQueryKey = @HelpDeskQueryKey";



        public const string legalentitymarketingoptionhistorydatamodel_selectwhere = "SELECT LegalEntityMarketingOptionHistoryKey, LegalEntityMarketingOptionKey, LegalEntityKey, MarketingOptionKey, ChangeAction, ChangeDate, UserID FROM [2am].[dbo].[LegalEntityMarketingOptionHistory] WHERE";
        public const string legalentitymarketingoptionhistorydatamodel_selectbykey = "SELECT LegalEntityMarketingOptionHistoryKey, LegalEntityMarketingOptionKey, LegalEntityKey, MarketingOptionKey, ChangeAction, ChangeDate, UserID FROM [2am].[dbo].[LegalEntityMarketingOptionHistory] WHERE LegalEntityMarketingOptionHistoryKey = @PrimaryKey";
        public const string legalentitymarketingoptionhistorydatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityMarketingOptionHistory] WHERE LegalEntityMarketingOptionHistoryKey = @PrimaryKey";
        public const string legalentitymarketingoptionhistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityMarketingOptionHistory] WHERE";
        public const string legalentitymarketingoptionhistorydatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityMarketingOptionHistory] (LegalEntityMarketingOptionKey, LegalEntityKey, MarketingOptionKey, ChangeAction, ChangeDate, UserID) VALUES(@LegalEntityMarketingOptionKey, @LegalEntityKey, @MarketingOptionKey, @ChangeAction, @ChangeDate, @UserID); select cast(scope_identity() as int)";
        public const string legalentitymarketingoptionhistorydatamodel_update = "UPDATE [2am].[dbo].[LegalEntityMarketingOptionHistory] SET LegalEntityMarketingOptionKey = @LegalEntityMarketingOptionKey, LegalEntityKey = @LegalEntityKey, MarketingOptionKey = @MarketingOptionKey, ChangeAction = @ChangeAction, ChangeDate = @ChangeDate, UserID = @UserID WHERE LegalEntityMarketingOptionHistoryKey = @LegalEntityMarketingOptionHistoryKey";



        public const string mailingaddressdatamodel_selectwhere = "SELECT MailingAddressAccountKey, AccountKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey FROM [2am].[dbo].[MailingAddress] WHERE";
        public const string mailingaddressdatamodel_selectbykey = "SELECT MailingAddressAccountKey, AccountKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey FROM [2am].[dbo].[MailingAddress] WHERE MailingAddressAccountKey = @PrimaryKey";
        public const string mailingaddressdatamodel_delete = "DELETE FROM [2am].[dbo].[MailingAddress] WHERE MailingAddressAccountKey = @PrimaryKey";
        public const string mailingaddressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MailingAddress] WHERE";
        public const string mailingaddressdatamodel_insert = "INSERT INTO [2am].[dbo].[MailingAddress] (AccountKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey) VALUES(@AccountKey, @AddressKey, @OnlineStatement, @OnlineStatementFormatKey, @LanguageKey, @LegalEntityKey, @CorrespondenceMediumKey); select cast(scope_identity() as int)";
        public const string mailingaddressdatamodel_update = "UPDATE [2am].[dbo].[MailingAddress] SET AccountKey = @AccountKey, AddressKey = @AddressKey, OnlineStatement = @OnlineStatement, OnlineStatementFormatKey = @OnlineStatementFormatKey, LanguageKey = @LanguageKey, LegalEntityKey = @LegalEntityKey, CorrespondenceMediumKey = @CorrespondenceMediumKey WHERE MailingAddressAccountKey = @MailingAddressAccountKey";



        public const string genericsetdefinitiondatamodel_selectwhere = "SELECT GenericSetDefinitionKey, Description, GenericSetTypeKey, Explanation FROM [2am].[dbo].[GenericSetDefinition] WHERE";
        public const string genericsetdefinitiondatamodel_selectbykey = "SELECT GenericSetDefinitionKey, Description, GenericSetTypeKey, Explanation FROM [2am].[dbo].[GenericSetDefinition] WHERE GenericSetDefinitionKey = @PrimaryKey";
        public const string genericsetdefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[GenericSetDefinition] WHERE GenericSetDefinitionKey = @PrimaryKey";
        public const string genericsetdefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[GenericSetDefinition] WHERE";
        public const string genericsetdefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[GenericSetDefinition] (Description, GenericSetTypeKey, Explanation) VALUES(@Description, @GenericSetTypeKey, @Explanation); select cast(scope_identity() as int)";
        public const string genericsetdefinitiondatamodel_update = "UPDATE [2am].[dbo].[GenericSetDefinition] SET Description = @Description, GenericSetTypeKey = @GenericSetTypeKey, Explanation = @Explanation WHERE GenericSetDefinitionKey = @GenericSetDefinitionKey";



        public const string domainprocessstatusdatamodel_selectwhere = "SELECT DomainProcessStatusKey, Description FROM [2am].[dbo].[DomainProcessStatus] WHERE";
        public const string domainprocessstatusdatamodel_selectbykey = "SELECT DomainProcessStatusKey, Description FROM [2am].[dbo].[DomainProcessStatus] WHERE DomainProcessStatusKey = @PrimaryKey";
        public const string domainprocessstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[DomainProcessStatus] WHERE DomainProcessStatusKey = @PrimaryKey";
        public const string domainprocessstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DomainProcessStatus] WHERE";
        public const string domainprocessstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[DomainProcessStatus] (DomainProcessStatusKey, Description) VALUES(@DomainProcessStatusKey, @Description); ";
        public const string domainprocessstatusdatamodel_update = "UPDATE [2am].[dbo].[DomainProcessStatus] SET DomainProcessStatusKey = @DomainProcessStatusKey, Description = @Description WHERE DomainProcessStatusKey = @DomainProcessStatusKey";



        public const string headericondatamodel_selectwhere = "SELECT HeaderIconKey, CoreBusinessObjectKey, HeaderIconTypeKey FROM [2am].[dbo].[HeaderIcon] WHERE";
        public const string headericondatamodel_selectbykey = "SELECT HeaderIconKey, CoreBusinessObjectKey, HeaderIconTypeKey FROM [2am].[dbo].[HeaderIcon] WHERE HeaderIconKey = @PrimaryKey";
        public const string headericondatamodel_delete = "DELETE FROM [2am].[dbo].[HeaderIcon] WHERE HeaderIconKey = @PrimaryKey";
        public const string headericondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HeaderIcon] WHERE";
        public const string headericondatamodel_insert = "INSERT INTO [2am].[dbo].[HeaderIcon] (CoreBusinessObjectKey, HeaderIconTypeKey) VALUES(@CoreBusinessObjectKey, @HeaderIconTypeKey); select cast(scope_identity() as int)";
        public const string headericondatamodel_update = "UPDATE [2am].[dbo].[HeaderIcon] SET CoreBusinessObjectKey = @CoreBusinessObjectKey, HeaderIconTypeKey = @HeaderIconTypeKey WHERE HeaderIconKey = @HeaderIconKey";



        public const string claimtypedatamodel_selectwhere = "SELECT ClaimTypeKey, Description FROM [2am].[dbo].[ClaimType] WHERE";
        public const string claimtypedatamodel_selectbykey = "SELECT ClaimTypeKey, Description FROM [2am].[dbo].[ClaimType] WHERE ClaimTypeKey = @PrimaryKey";
        public const string claimtypedatamodel_delete = "DELETE FROM [2am].[dbo].[ClaimType] WHERE ClaimTypeKey = @PrimaryKey";
        public const string claimtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ClaimType] WHERE";
        public const string claimtypedatamodel_insert = "INSERT INTO [2am].[dbo].[ClaimType] (ClaimTypeKey, Description) VALUES(@ClaimTypeKey, @Description); ";
        public const string claimtypedatamodel_update = "UPDATE [2am].[dbo].[ClaimType] SET ClaimTypeKey = @ClaimTypeKey, Description = @Description WHERE ClaimTypeKey = @ClaimTypeKey";



        public const string offerattributetypegroupdatamodel_selectwhere = "SELECT OfferAttributeTypeGroupKey, Description FROM [2am].[dbo].[OfferAttributeTypeGroup] WHERE";
        public const string offerattributetypegroupdatamodel_selectbykey = "SELECT OfferAttributeTypeGroupKey, Description FROM [2am].[dbo].[OfferAttributeTypeGroup] WHERE OfferAttributeTypeGroupKey = @PrimaryKey";
        public const string offerattributetypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferAttributeTypeGroup] WHERE OfferAttributeTypeGroupKey = @PrimaryKey";
        public const string offerattributetypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferAttributeTypeGroup] WHERE";
        public const string offerattributetypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferAttributeTypeGroup] (OfferAttributeTypeGroupKey, Description) VALUES(@OfferAttributeTypeGroupKey, @Description); ";
        public const string offerattributetypegroupdatamodel_update = "UPDATE [2am].[dbo].[OfferAttributeTypeGroup] SET OfferAttributeTypeGroupKey = @OfferAttributeTypeGroupKey, Description = @Description WHERE OfferAttributeTypeGroupKey = @OfferAttributeTypeGroupKey";



        public const string auditlegalentitybankaccountdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, BankAccountKey, LegalEntityBankAccountKey, GeneralStatusKey, UserID, ChangeDate FROM [2am].[dbo].[AuditLegalEntityBankAccount] WHERE";
        public const string auditlegalentitybankaccountdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, BankAccountKey, LegalEntityBankAccountKey, GeneralStatusKey, UserID, ChangeDate FROM [2am].[dbo].[AuditLegalEntityBankAccount] WHERE AuditNumber = @PrimaryKey";
        public const string auditlegalentitybankaccountdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditLegalEntityBankAccount] WHERE AuditNumber = @PrimaryKey";
        public const string auditlegalentitybankaccountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditLegalEntityBankAccount] WHERE";
        public const string auditlegalentitybankaccountdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditLegalEntityBankAccount] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, BankAccountKey, LegalEntityBankAccountKey, GeneralStatusKey, UserID, ChangeDate) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @LegalEntityKey, @BankAccountKey, @LegalEntityBankAccountKey, @GeneralStatusKey, @UserID, @ChangeDate); select cast(scope_identity() as int)";
        public const string auditlegalentitybankaccountdatamodel_update = "UPDATE [2am].[dbo].[AuditLegalEntityBankAccount] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, LegalEntityKey = @LegalEntityKey, BankAccountKey = @BankAccountKey, LegalEntityBankAccountKey = @LegalEntityBankAccountKey, GeneralStatusKey = @GeneralStatusKey, UserID = @UserID, ChangeDate = @ChangeDate WHERE AuditNumber = @AuditNumber";



        public const string valuationoutbuildingdatamodel_selectwhere = "SELECT ValuationOutbuildingKey, ValuationKey, ValuationRoofTypeKey, Extent, Rate FROM [2am].[dbo].[ValuationOutbuilding] WHERE";
        public const string valuationoutbuildingdatamodel_selectbykey = "SELECT ValuationOutbuildingKey, ValuationKey, ValuationRoofTypeKey, Extent, Rate FROM [2am].[dbo].[ValuationOutbuilding] WHERE ValuationOutbuildingKey = @PrimaryKey";
        public const string valuationoutbuildingdatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationOutbuilding] WHERE ValuationOutbuildingKey = @PrimaryKey";
        public const string valuationoutbuildingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationOutbuilding] WHERE";
        public const string valuationoutbuildingdatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationOutbuilding] (ValuationKey, ValuationRoofTypeKey, Extent, Rate) VALUES(@ValuationKey, @ValuationRoofTypeKey, @Extent, @Rate); select cast(scope_identity() as int)";
        public const string valuationoutbuildingdatamodel_update = "UPDATE [2am].[dbo].[ValuationOutbuilding] SET ValuationKey = @ValuationKey, ValuationRoofTypeKey = @ValuationRoofTypeKey, Extent = @Extent, Rate = @Rate WHERE ValuationOutbuildingKey = @ValuationOutbuildingKey";



        public const string organisationstructuredatamodel_selectwhere = "SELECT OrganisationStructureKey, ParentKey, Description, OrganisationTypeKey, GeneralStatusKey FROM [2am].[dbo].[OrganisationStructure] WHERE";
        public const string organisationstructuredatamodel_selectbykey = "SELECT OrganisationStructureKey, ParentKey, Description, OrganisationTypeKey, GeneralStatusKey FROM [2am].[dbo].[OrganisationStructure] WHERE OrganisationStructureKey = @PrimaryKey";
        public const string organisationstructuredatamodel_delete = "DELETE FROM [2am].[dbo].[OrganisationStructure] WHERE OrganisationStructureKey = @PrimaryKey";
        public const string organisationstructuredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OrganisationStructure] WHERE";
        public const string organisationstructuredatamodel_insert = "INSERT INTO [2am].[dbo].[OrganisationStructure] (ParentKey, Description, OrganisationTypeKey, GeneralStatusKey) VALUES(@ParentKey, @Description, @OrganisationTypeKey, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string organisationstructuredatamodel_update = "UPDATE [2am].[dbo].[OrganisationStructure] SET ParentKey = @ParentKey, Description = @Description, OrganisationTypeKey = @OrganisationTypeKey, GeneralStatusKey = @GeneralStatusKey WHERE OrganisationStructureKey = @OrganisationStructureKey";



        public const string lifecccoldcasesdatamodel_selectwhere = "SELECT loannumber FROM [2am].[dbo].[LifeCCCOldCases] WHERE";
        public const string lifecccoldcasesdatamodel_selectbykey = "SELECT loannumber FROM [2am].[dbo].[LifeCCCOldCases] WHERE  = @PrimaryKey";
        public const string lifecccoldcasesdatamodel_delete = "DELETE FROM [2am].[dbo].[LifeCCCOldCases] WHERE  = @PrimaryKey";
        public const string lifecccoldcasesdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifeCCCOldCases] WHERE";
        public const string lifecccoldcasesdatamodel_insert = "INSERT INTO [2am].[dbo].[LifeCCCOldCases] (loannumber) VALUES(@loannumber); ";
        public const string lifecccoldcasesdatamodel_update = "UPDATE [2am].[dbo].[LifeCCCOldCases] SET loannumber = @loannumber WHERE  = @";



        public const string lifepolicydatamodel_selectwhere = "SELECT FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, YearlyPremium, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, ClaimStatusDate, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, BrokerKey, DeathReassuranceRetention, IPBReassuranceRetention, LifePolicyTypeKey, AnniversaryDate, ClaimTypeKey FROM [2am].[dbo].[LifePolicy] WHERE";
        public const string lifepolicydatamodel_selectbykey = "SELECT FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, YearlyPremium, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, ClaimStatusDate, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, BrokerKey, DeathReassuranceRetention, IPBReassuranceRetention, LifePolicyTypeKey, AnniversaryDate, ClaimTypeKey FROM [2am].[dbo].[LifePolicy] WHERE FinancialServiceKey = @PrimaryKey";
        public const string lifepolicydatamodel_delete = "DELETE FROM [2am].[dbo].[LifePolicy] WHERE FinancialServiceKey = @PrimaryKey";
        public const string lifepolicydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifePolicy] WHERE";
        public const string lifepolicydatamodel_insert = "INSERT INTO [2am].[dbo].[LifePolicy] (FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, YearlyPremium, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, ClaimStatusDate, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, BrokerKey, DeathReassuranceRetention, IPBReassuranceRetention, LifePolicyTypeKey, AnniversaryDate, ClaimTypeKey) VALUES(@FinancialServiceKey, @DeathBenefit, @InstallmentProtectionBenefit, @DeathBenefitPremium, @InstallmentProtectionPremium, @PolicyStatusKey, @DateOfCommencement, @DateOfExpiry, @DeathRetentionLimit, @InstallmentProtectionRetentionLimit, @UpliftFactor, @JointDiscountFactor, @DateOfCancellation, @YearlyPremium, @DateOfAcceptance, @SumAssured, @DateLastUpdated, @Consultant, @ClaimStatusKey, @CurrentSumAssured, @PremiumShortfall, @InsurerKey, @ExternalPolicyNumber, @DateCeded, @PriorityKey, @ClaimStatusDate, @PolicyHolderLEKey, @RPARInsurer, @RPARPolicyNumber, @BrokerKey, @DeathReassuranceRetention, @IPBReassuranceRetention, @LifePolicyTypeKey, @AnniversaryDate, @ClaimTypeKey); ";
        public const string lifepolicydatamodel_update = "UPDATE [2am].[dbo].[LifePolicy] SET FinancialServiceKey = @FinancialServiceKey, DeathBenefit = @DeathBenefit, InstallmentProtectionBenefit = @InstallmentProtectionBenefit, DeathBenefitPremium = @DeathBenefitPremium, InstallmentProtectionPremium = @InstallmentProtectionPremium, PolicyStatusKey = @PolicyStatusKey, DateOfCommencement = @DateOfCommencement, DateOfExpiry = @DateOfExpiry, DeathRetentionLimit = @DeathRetentionLimit, InstallmentProtectionRetentionLimit = @InstallmentProtectionRetentionLimit, UpliftFactor = @UpliftFactor, JointDiscountFactor = @JointDiscountFactor, DateOfCancellation = @DateOfCancellation, YearlyPremium = @YearlyPremium, DateOfAcceptance = @DateOfAcceptance, SumAssured = @SumAssured, DateLastUpdated = @DateLastUpdated, Consultant = @Consultant, ClaimStatusKey = @ClaimStatusKey, CurrentSumAssured = @CurrentSumAssured, PremiumShortfall = @PremiumShortfall, InsurerKey = @InsurerKey, ExternalPolicyNumber = @ExternalPolicyNumber, DateCeded = @DateCeded, PriorityKey = @PriorityKey, ClaimStatusDate = @ClaimStatusDate, PolicyHolderLEKey = @PolicyHolderLEKey, RPARInsurer = @RPARInsurer, RPARPolicyNumber = @RPARPolicyNumber, BrokerKey = @BrokerKey, DeathReassuranceRetention = @DeathReassuranceRetention, IPBReassuranceRetention = @IPBReassuranceRetention, LifePolicyTypeKey = @LifePolicyTypeKey, AnniversaryDate = @AnniversaryDate, ClaimTypeKey = @ClaimTypeKey WHERE FinancialServiceKey = @FinancialServiceKey";



        public const string paymenttypedatamodel_selectwhere = "SELECT PaymentTypeKey, Description FROM [2am].[dbo].[PaymentType] WHERE";
        public const string paymenttypedatamodel_selectbykey = "SELECT PaymentTypeKey, Description FROM [2am].[dbo].[PaymentType] WHERE PaymentTypeKey = @PrimaryKey";
        public const string paymenttypedatamodel_delete = "DELETE FROM [2am].[dbo].[PaymentType] WHERE PaymentTypeKey = @PrimaryKey";
        public const string paymenttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PaymentType] WHERE";
        public const string paymenttypedatamodel_insert = "INSERT INTO [2am].[dbo].[PaymentType] (PaymentTypeKey, Description) VALUES(@PaymentTypeKey, @Description); ";
        public const string paymenttypedatamodel_update = "UPDATE [2am].[dbo].[PaymentType] SET PaymentTypeKey = @PaymentTypeKey, Description = @Description WHERE PaymentTypeKey = @PaymentTypeKey";



        public const string domainprocessdatamodel_selectwhere = "SELECT DomainProcessId, DomainProcessType, ProcessState, StartResultData, DomainProcessStatusKey, StatusReason, DataModel, DateCreated, DateModified FROM [2am].[dbo].[DomainProcess] WHERE";
        public const string domainprocessdatamodel_selectbykey = "SELECT DomainProcessId, DomainProcessType, ProcessState, StartResultData, DomainProcessStatusKey, StatusReason, DataModel, DateCreated, DateModified FROM [2am].[dbo].[DomainProcess] WHERE DomainProcessId = @PrimaryKey";
        public const string domainprocessdatamodel_delete = "DELETE FROM [2am].[dbo].[DomainProcess] WHERE DomainProcessId = @PrimaryKey";
        public const string domainprocessdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DomainProcess] WHERE";
        public const string domainprocessdatamodel_insert = "INSERT INTO [2am].[dbo].[DomainProcess] (DomainProcessId, DomainProcessType, ProcessState, StartResultData, DomainProcessStatusKey, StatusReason, DataModel, DateCreated, DateModified) VALUES(@DomainProcessId, @DomainProcessType, @ProcessState, @StartResultData, @DomainProcessStatusKey, @StatusReason, @DataModel, @DateCreated, @DateModified); ";
        public const string domainprocessdatamodel_update = "UPDATE [2am].[dbo].[DomainProcess] SET DomainProcessId = @DomainProcessId, DomainProcessType = @DomainProcessType, ProcessState = @ProcessState, StartResultData = @StartResultData, DomainProcessStatusKey = @DomainProcessStatusKey, StatusReason = @StatusReason, DataModel = @DataModel, DateCreated = @DateCreated, DateModified = @DateModified WHERE DomainProcessId = @DomainProcessId";



        public const string genderdatamodel_selectwhere = "SELECT GenderKey, Description FROM [2am].[dbo].[Gender] WHERE";
        public const string genderdatamodel_selectbykey = "SELECT GenderKey, Description FROM [2am].[dbo].[Gender] WHERE GenderKey = @PrimaryKey";
        public const string genderdatamodel_delete = "DELETE FROM [2am].[dbo].[Gender] WHERE GenderKey = @PrimaryKey";
        public const string genderdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Gender] WHERE";
        public const string genderdatamodel_insert = "INSERT INTO [2am].[dbo].[Gender] (GenderKey, Description) VALUES(@GenderKey, @Description); ";
        public const string genderdatamodel_update = "UPDATE [2am].[dbo].[Gender] SET GenderKey = @GenderKey, Description = @Description WHERE GenderKey = @GenderKey";



        public const string claimstatusdatamodel_selectwhere = "SELECT ClaimStatusKey, Description FROM [2am].[dbo].[ClaimStatus] WHERE";
        public const string claimstatusdatamodel_selectbykey = "SELECT ClaimStatusKey, Description FROM [2am].[dbo].[ClaimStatus] WHERE ClaimStatusKey = @PrimaryKey";
        public const string claimstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[ClaimStatus] WHERE ClaimStatusKey = @PrimaryKey";
        public const string claimstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ClaimStatus] WHERE";
        public const string claimstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[ClaimStatus] (ClaimStatusKey, Description) VALUES(@ClaimStatusKey, @Description); ";
        public const string claimstatusdatamodel_update = "UPDATE [2am].[dbo].[ClaimStatus] SET ClaimStatusKey = @ClaimStatusKey, Description = @Description WHERE ClaimStatusKey = @ClaimStatusKey";



        public const string catspaymentbatchitemdatamodel_selectwhere = "SELECT CATSPaymentBatchItemKey, GenericKey, GenericTypeKey, AccountKey, Amount, SourceBankAccountKey, TargetBankAccountKey, CATSPaymentBatchKey, SahlReferenceNumber, SourceReferenceNumber, TargetName, ExternalReference, EmailAddress, LegalEntityKey, Processed FROM [2am].[dbo].[CATSPaymentBatchItem] WHERE";
        public const string catspaymentbatchitemdatamodel_selectbykey = "SELECT CATSPaymentBatchItemKey, GenericKey, GenericTypeKey, AccountKey, Amount, SourceBankAccountKey, TargetBankAccountKey, CATSPaymentBatchKey, SahlReferenceNumber, SourceReferenceNumber, TargetName, ExternalReference, EmailAddress, LegalEntityKey, Processed FROM [2am].[dbo].[CATSPaymentBatchItem] WHERE CATSPaymentBatchItemKey = @PrimaryKey";
        public const string catspaymentbatchitemdatamodel_delete = "DELETE FROM [2am].[dbo].[CATSPaymentBatchItem] WHERE CATSPaymentBatchItemKey = @PrimaryKey";
        public const string catspaymentbatchitemdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CATSPaymentBatchItem] WHERE";
        public const string catspaymentbatchitemdatamodel_insert = "INSERT INTO [2am].[dbo].[CATSPaymentBatchItem] (GenericKey, GenericTypeKey, AccountKey, Amount, SourceBankAccountKey, TargetBankAccountKey, CATSPaymentBatchKey, SahlReferenceNumber, SourceReferenceNumber, TargetName, ExternalReference, EmailAddress, LegalEntityKey, Processed) VALUES(@GenericKey, @GenericTypeKey, @AccountKey, @Amount, @SourceBankAccountKey, @TargetBankAccountKey, @CATSPaymentBatchKey, @SahlReferenceNumber, @SourceReferenceNumber, @TargetName, @ExternalReference, @EmailAddress, @LegalEntityKey, @Processed); select cast(scope_identity() as int)";
        public const string catspaymentbatchitemdatamodel_update = "UPDATE [2am].[dbo].[CATSPaymentBatchItem] SET GenericKey = @GenericKey, GenericTypeKey = @GenericTypeKey, AccountKey = @AccountKey, Amount = @Amount, SourceBankAccountKey = @SourceBankAccountKey, TargetBankAccountKey = @TargetBankAccountKey, CATSPaymentBatchKey = @CATSPaymentBatchKey, SahlReferenceNumber = @SahlReferenceNumber, SourceReferenceNumber = @SourceReferenceNumber, TargetName = @TargetName, ExternalReference = @ExternalReference, EmailAddress = @EmailAddress, LegalEntityKey = @LegalEntityKey, Processed = @Processed WHERE CATSPaymentBatchItemKey = @CATSPaymentBatchItemKey";



        public const string offerlifedatamodel_selectwhere = "SELECT OfferKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, DateOfExpiry, UpliftFactor, JointDiscountFactor, MonthlyPremium, YearlyPremium, SumAssured, DateLastUpdated, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, DateOfAcceptance, Consultant, LifePolicyTypeKey FROM [2am].[dbo].[OfferLife] WHERE";
        public const string offerlifedatamodel_selectbykey = "SELECT OfferKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, DateOfExpiry, UpliftFactor, JointDiscountFactor, MonthlyPremium, YearlyPremium, SumAssured, DateLastUpdated, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, DateOfAcceptance, Consultant, LifePolicyTypeKey FROM [2am].[dbo].[OfferLife] WHERE OfferKey = @PrimaryKey";
        public const string offerlifedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferLife] WHERE OfferKey = @PrimaryKey";
        public const string offerlifedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferLife] WHERE";
        public const string offerlifedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferLife] (OfferKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, DateOfExpiry, UpliftFactor, JointDiscountFactor, MonthlyPremium, YearlyPremium, SumAssured, DateLastUpdated, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, DateOfAcceptance, Consultant, LifePolicyTypeKey) VALUES(@OfferKey, @DeathBenefit, @InstallmentProtectionBenefit, @DeathBenefitPremium, @InstallmentProtectionPremium, @DateOfExpiry, @UpliftFactor, @JointDiscountFactor, @MonthlyPremium, @YearlyPremium, @SumAssured, @DateLastUpdated, @CurrentSumAssured, @PremiumShortfall, @InsurerKey, @ExternalPolicyNumber, @DateCeded, @PriorityKey, @PolicyHolderLEKey, @RPARInsurer, @RPARPolicyNumber, @DateOfAcceptance, @Consultant, @LifePolicyTypeKey); ";
        public const string offerlifedatamodel_update = "UPDATE [2am].[dbo].[OfferLife] SET OfferKey = @OfferKey, DeathBenefit = @DeathBenefit, InstallmentProtectionBenefit = @InstallmentProtectionBenefit, DeathBenefitPremium = @DeathBenefitPremium, InstallmentProtectionPremium = @InstallmentProtectionPremium, DateOfExpiry = @DateOfExpiry, UpliftFactor = @UpliftFactor, JointDiscountFactor = @JointDiscountFactor, MonthlyPremium = @MonthlyPremium, YearlyPremium = @YearlyPremium, SumAssured = @SumAssured, DateLastUpdated = @DateLastUpdated, CurrentSumAssured = @CurrentSumAssured, PremiumShortfall = @PremiumShortfall, InsurerKey = @InsurerKey, ExternalPolicyNumber = @ExternalPolicyNumber, DateCeded = @DateCeded, PriorityKey = @PriorityKey, PolicyHolderLEKey = @PolicyHolderLEKey, RPARInsurer = @RPARInsurer, RPARPolicyNumber = @RPARPolicyNumber, DateOfAcceptance = @DateOfAcceptance, Consultant = @Consultant, LifePolicyTypeKey = @LifePolicyTypeKey WHERE OfferKey = @OfferKey";



        public const string watchlistconfigurationdatamodel_selectwhere = "SELECT WatchListConfigurationKey, ProcessName, WorkFlowName, StatementName FROM [2am].[dbo].[WatchListConfiguration] WHERE";
        public const string watchlistconfigurationdatamodel_selectbykey = "SELECT WatchListConfigurationKey, ProcessName, WorkFlowName, StatementName FROM [2am].[dbo].[WatchListConfiguration] WHERE WatchListConfigurationKey = @PrimaryKey";
        public const string watchlistconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[WatchListConfiguration] WHERE WatchListConfigurationKey = @PrimaryKey";
        public const string watchlistconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WatchListConfiguration] WHERE";
        public const string watchlistconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[WatchListConfiguration] (ProcessName, WorkFlowName, StatementName) VALUES(@ProcessName, @WorkFlowName, @StatementName); select cast(scope_identity() as int)";
        public const string watchlistconfigurationdatamodel_update = "UPDATE [2am].[dbo].[WatchListConfiguration] SET ProcessName = @ProcessName, WorkFlowName = @WorkFlowName, StatementName = @StatementName WHERE WatchListConfigurationKey = @WatchListConfigurationKey";



        public const string offerinformationinterestonlydatamodel_selectwhere = "SELECT OfferInformationKey, Installment, MaturityDate FROM [2am].[dbo].[OfferInformationInterestOnly] WHERE";
        public const string offerinformationinterestonlydatamodel_selectbykey = "SELECT OfferInformationKey, Installment, MaturityDate FROM [2am].[dbo].[OfferInformationInterestOnly] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationinterestonlydatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationInterestOnly] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationinterestonlydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationInterestOnly] WHERE";
        public const string offerinformationinterestonlydatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationInterestOnly] (OfferInformationKey, Installment, MaturityDate) VALUES(@OfferInformationKey, @Installment, @MaturityDate); ";
        public const string offerinformationinterestonlydatamodel_update = "UPDATE [2am].[dbo].[OfferInformationInterestOnly] SET OfferInformationKey = @OfferInformationKey, Installment = @Installment, MaturityDate = @MaturityDate WHERE OfferInformationKey = @OfferInformationKey";



        public const string transactionstatisticsdatamodel_selectwhere = "SELECT YearMonth, TransactionTypeKey, SPVKey, TransactionAmount, OldSPV, NewSPV FROM [2am].[dbo].[TransactionStatistics] WHERE";
        public const string transactionstatisticsdatamodel_selectbykey = "SELECT YearMonth, TransactionTypeKey, SPVKey, TransactionAmount, OldSPV, NewSPV FROM [2am].[dbo].[TransactionStatistics] WHERE  = @PrimaryKey";
        public const string transactionstatisticsdatamodel_delete = "DELETE FROM [2am].[dbo].[TransactionStatistics] WHERE  = @PrimaryKey";
        public const string transactionstatisticsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TransactionStatistics] WHERE";
        public const string transactionstatisticsdatamodel_insert = "INSERT INTO [2am].[dbo].[TransactionStatistics] (YearMonth, TransactionTypeKey, SPVKey, TransactionAmount, OldSPV, NewSPV) VALUES(@YearMonth, @TransactionTypeKey, @SPVKey, @TransactionAmount, @OldSPV, @NewSPV); ";
        public const string transactionstatisticsdatamodel_update = "UPDATE [2am].[dbo].[TransactionStatistics] SET YearMonth = @YearMonth, TransactionTypeKey = @TransactionTypeKey, SPVKey = @SPVKey, TransactionAmount = @TransactionAmount, OldSPV = @OldSPV, NewSPV = @NewSPV WHERE  = @";



        public const string documentsetdatamodel_selectwhere = "SELECT DocumentSetKey, Description, OriginationSourceProductKey, OfferTypeKey FROM [2am].[dbo].[DocumentSet] WHERE";
        public const string documentsetdatamodel_selectbykey = "SELECT DocumentSetKey, Description, OriginationSourceProductKey, OfferTypeKey FROM [2am].[dbo].[DocumentSet] WHERE DocumentSetKey = @PrimaryKey";
        public const string documentsetdatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentSet] WHERE DocumentSetKey = @PrimaryKey";
        public const string documentsetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentSet] WHERE";
        public const string documentsetdatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentSet] (Description, OriginationSourceProductKey, OfferTypeKey) VALUES(@Description, @OriginationSourceProductKey, @OfferTypeKey); select cast(scope_identity() as int)";
        public const string documentsetdatamodel_update = "UPDATE [2am].[dbo].[DocumentSet] SET Description = @Description, OriginationSourceProductKey = @OriginationSourceProductKey, OfferTypeKey = @OfferTypeKey WHERE DocumentSetKey = @DocumentSetKey";



        public const string detaildatamodel_selectwhere = "SELECT DetailKey, DetailTypeKey, AccountKey, DetailDate, Amount, Description, LinkID, UserID, ChangeDate FROM [2am].[dbo].[Detail] WHERE";
        public const string detaildatamodel_selectbykey = "SELECT DetailKey, DetailTypeKey, AccountKey, DetailDate, Amount, Description, LinkID, UserID, ChangeDate FROM [2am].[dbo].[Detail] WHERE DetailKey = @PrimaryKey";
        public const string detaildatamodel_delete = "DELETE FROM [2am].[dbo].[Detail] WHERE DetailKey = @PrimaryKey";
        public const string detaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Detail] WHERE";
        public const string detaildatamodel_insert = "INSERT INTO [2am].[dbo].[Detail] (DetailTypeKey, AccountKey, DetailDate, Amount, Description, LinkID, UserID, ChangeDate) VALUES(@DetailTypeKey, @AccountKey, @DetailDate, @Amount, @Description, @LinkID, @UserID, @ChangeDate); select cast(scope_identity() as int)";
        public const string detaildatamodel_update = "UPDATE [2am].[dbo].[Detail] SET DetailTypeKey = @DetailTypeKey, AccountKey = @AccountKey, DetailDate = @DetailDate, Amount = @Amount, Description = @Description, LinkID = @LinkID, UserID = @UserID, ChangeDate = @ChangeDate WHERE DetailKey = @DetailKey";



        public const string roledatamodel_selectwhere = "SELECT AccountRoleKey, LegalEntityKey, AccountKey, RoleTypeKey, GeneralStatusKey, StatusChangeDate FROM [2am].[dbo].[Role] WHERE";
        public const string roledatamodel_selectbykey = "SELECT AccountRoleKey, LegalEntityKey, AccountKey, RoleTypeKey, GeneralStatusKey, StatusChangeDate FROM [2am].[dbo].[Role] WHERE AccountRoleKey = @PrimaryKey";
        public const string roledatamodel_delete = "DELETE FROM [2am].[dbo].[Role] WHERE AccountRoleKey = @PrimaryKey";
        public const string roledatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Role] WHERE";
        public const string roledatamodel_insert = "INSERT INTO [2am].[dbo].[Role] (LegalEntityKey, AccountKey, RoleTypeKey, GeneralStatusKey, StatusChangeDate) VALUES(@LegalEntityKey, @AccountKey, @RoleTypeKey, @GeneralStatusKey, @StatusChangeDate); select cast(scope_identity() as int)";
        public const string roledatamodel_update = "UPDATE [2am].[dbo].[Role] SET LegalEntityKey = @LegalEntityKey, AccountKey = @AccountKey, RoleTypeKey = @RoleTypeKey, GeneralStatusKey = @GeneralStatusKey, StatusChangeDate = @StatusChangeDate WHERE AccountRoleKey = @AccountRoleKey";



        public const string headericondetailsdatamodel_selectwhere = "SELECT HeaderIconDetailsKey, GenericKeyTypeKey, GenericKey, HeaderIconTypeKey, Description FROM [2am].[dbo].[HeaderIconDetails] WHERE";
        public const string headericondetailsdatamodel_selectbykey = "SELECT HeaderIconDetailsKey, GenericKeyTypeKey, GenericKey, HeaderIconTypeKey, Description FROM [2am].[dbo].[HeaderIconDetails] WHERE HeaderIconDetailsKey = @PrimaryKey";
        public const string headericondetailsdatamodel_delete = "DELETE FROM [2am].[dbo].[HeaderIconDetails] WHERE HeaderIconDetailsKey = @PrimaryKey";
        public const string headericondetailsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HeaderIconDetails] WHERE";
        public const string headericondetailsdatamodel_insert = "INSERT INTO [2am].[dbo].[HeaderIconDetails] (GenericKeyTypeKey, GenericKey, HeaderIconTypeKey, Description) VALUES(@GenericKeyTypeKey, @GenericKey, @HeaderIconTypeKey, @Description); select cast(scope_identity() as int)";
        public const string headericondetailsdatamodel_update = "UPDATE [2am].[dbo].[HeaderIconDetails] SET GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey, HeaderIconTypeKey = @HeaderIconTypeKey, Description = @Description WHERE HeaderIconDetailsKey = @HeaderIconDetailsKey";



        public const string lifepolicyclaimdatamodel_selectwhere = "SELECT LifePolicyClaimKey, FinancialServiceKey, ClaimStatusKey, ClaimTypeKey, ClaimDate FROM [2am].[dbo].[LifePolicyClaim] WHERE";
        public const string lifepolicyclaimdatamodel_selectbykey = "SELECT LifePolicyClaimKey, FinancialServiceKey, ClaimStatusKey, ClaimTypeKey, ClaimDate FROM [2am].[dbo].[LifePolicyClaim] WHERE LifePolicyClaimKey = @PrimaryKey";
        public const string lifepolicyclaimdatamodel_delete = "DELETE FROM [2am].[dbo].[LifePolicyClaim] WHERE LifePolicyClaimKey = @PrimaryKey";
        public const string lifepolicyclaimdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifePolicyClaim] WHERE";
        public const string lifepolicyclaimdatamodel_insert = "INSERT INTO [2am].[dbo].[LifePolicyClaim] (FinancialServiceKey, ClaimStatusKey, ClaimTypeKey, ClaimDate) VALUES(@FinancialServiceKey, @ClaimStatusKey, @ClaimTypeKey, @ClaimDate); select cast(scope_identity() as int)";
        public const string lifepolicyclaimdatamodel_update = "UPDATE [2am].[dbo].[LifePolicyClaim] SET FinancialServiceKey = @FinancialServiceKey, ClaimStatusKey = @ClaimStatusKey, ClaimTypeKey = @ClaimTypeKey, ClaimDate = @ClaimDate WHERE LifePolicyClaimKey = @LifePolicyClaimKey";



        public const string valuationmainbuildingdatamodel_selectwhere = "SELECT ValuationKey, ValuationRoofTypeKey, Extent, Rate FROM [2am].[dbo].[ValuationMainBuilding] WHERE";
        public const string valuationmainbuildingdatamodel_selectbykey = "SELECT ValuationKey, ValuationRoofTypeKey, Extent, Rate FROM [2am].[dbo].[ValuationMainBuilding] WHERE ValuationKey = @PrimaryKey";
        public const string valuationmainbuildingdatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationMainBuilding] WHERE ValuationKey = @PrimaryKey";
        public const string valuationmainbuildingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationMainBuilding] WHERE";
        public const string valuationmainbuildingdatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationMainBuilding] (ValuationKey, ValuationRoofTypeKey, Extent, Rate) VALUES(@ValuationKey, @ValuationRoofTypeKey, @Extent, @Rate); ";
        public const string valuationmainbuildingdatamodel_update = "UPDATE [2am].[dbo].[ValuationMainBuilding] SET ValuationKey = @ValuationKey, ValuationRoofTypeKey = @ValuationRoofTypeKey, Extent = @Extent, Rate = @Rate WHERE ValuationKey = @ValuationKey";



        public const string populationgroupdatamodel_selectwhere = "SELECT PopulationGroupKey, Description FROM [2am].[dbo].[PopulationGroup] WHERE";
        public const string populationgroupdatamodel_selectbykey = "SELECT PopulationGroupKey, Description FROM [2am].[dbo].[PopulationGroup] WHERE PopulationGroupKey = @PrimaryKey";
        public const string populationgroupdatamodel_delete = "DELETE FROM [2am].[dbo].[PopulationGroup] WHERE PopulationGroupKey = @PrimaryKey";
        public const string populationgroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PopulationGroup] WHERE";
        public const string populationgroupdatamodel_insert = "INSERT INTO [2am].[dbo].[PopulationGroup] (PopulationGroupKey, Description) VALUES(@PopulationGroupKey, @Description); ";
        public const string populationgroupdatamodel_update = "UPDATE [2am].[dbo].[PopulationGroup] SET PopulationGroupKey = @PopulationGroupKey, Description = @Description WHERE PopulationGroupKey = @PopulationGroupKey";



        public const string hocconstructiondatamodel_selectwhere = "SELECT HOCConstructionKey, Description FROM [2am].[dbo].[HOCConstruction] WHERE";
        public const string hocconstructiondatamodel_selectbykey = "SELECT HOCConstructionKey, Description FROM [2am].[dbo].[HOCConstruction] WHERE HOCConstructionKey = @PrimaryKey";
        public const string hocconstructiondatamodel_delete = "DELETE FROM [2am].[dbo].[HOCConstruction] WHERE HOCConstructionKey = @PrimaryKey";
        public const string hocconstructiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCConstruction] WHERE";
        public const string hocconstructiondatamodel_insert = "INSERT INTO [2am].[dbo].[HOCConstruction] (HOCConstructionKey, Description) VALUES(@HOCConstructionKey, @Description); ";
        public const string hocconstructiondatamodel_update = "UPDATE [2am].[dbo].[HOCConstruction] SET HOCConstructionKey = @HOCConstructionKey, Description = @Description WHERE HOCConstructionKey = @HOCConstructionKey";



        public const string datagridconfigurationdatamodel_selectwhere = "SELECT DataGridConfigurationKey, StatementName, ColumnName, ColumnDescription, Sequence, Width, Visible, IndexIdentifier, FormatTypeKey, DataGridConfigurationTypeKey FROM [2am].[dbo].[DataGridConfiguration] WHERE";
        public const string datagridconfigurationdatamodel_selectbykey = "SELECT DataGridConfigurationKey, StatementName, ColumnName, ColumnDescription, Sequence, Width, Visible, IndexIdentifier, FormatTypeKey, DataGridConfigurationTypeKey FROM [2am].[dbo].[DataGridConfiguration] WHERE DataGridConfigurationKey = @PrimaryKey";
        public const string datagridconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[DataGridConfiguration] WHERE DataGridConfigurationKey = @PrimaryKey";
        public const string datagridconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DataGridConfiguration] WHERE";
        public const string datagridconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[DataGridConfiguration] (StatementName, ColumnName, ColumnDescription, Sequence, Width, Visible, IndexIdentifier, FormatTypeKey, DataGridConfigurationTypeKey) VALUES(@StatementName, @ColumnName, @ColumnDescription, @Sequence, @Width, @Visible, @IndexIdentifier, @FormatTypeKey, @DataGridConfigurationTypeKey); select cast(scope_identity() as int)";
        public const string datagridconfigurationdatamodel_update = "UPDATE [2am].[dbo].[DataGridConfiguration] SET StatementName = @StatementName, ColumnName = @ColumnName, ColumnDescription = @ColumnDescription, Sequence = @Sequence, Width = @Width, Visible = @Visible, IndexIdentifier = @IndexIdentifier, FormatTypeKey = @FormatTypeKey, DataGridConfigurationTypeKey = @DataGridConfigurationTypeKey WHERE DataGridConfigurationKey = @DataGridConfigurationKey";



        public const string accountinformationdatamodel_selectwhere = "SELECT AccountInformationKey, AccountInformationTypeKey, AccountKey, EntryDate, Amount, Information FROM [2am].[dbo].[AccountInformation] WHERE";
        public const string accountinformationdatamodel_selectbykey = "SELECT AccountInformationKey, AccountInformationTypeKey, AccountKey, EntryDate, Amount, Information FROM [2am].[dbo].[AccountInformation] WHERE AccountInformationKey = @PrimaryKey";
        public const string accountinformationdatamodel_delete = "DELETE FROM [2am].[dbo].[AccountInformation] WHERE AccountInformationKey = @PrimaryKey";
        public const string accountinformationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountInformation] WHERE";
        public const string accountinformationdatamodel_insert = "INSERT INTO [2am].[dbo].[AccountInformation] (AccountInformationTypeKey, AccountKey, EntryDate, Amount, Information) VALUES(@AccountInformationTypeKey, @AccountKey, @EntryDate, @Amount, @Information); select cast(scope_identity() as int)";
        public const string accountinformationdatamodel_update = "UPDATE [2am].[dbo].[AccountInformation] SET AccountInformationTypeKey = @AccountInformationTypeKey, AccountKey = @AccountKey, EntryDate = @EntryDate, Amount = @Amount, Information = @Information WHERE AccountInformationKey = @AccountInformationKey";



        public const string callbackdatamodel_selectwhere = "SELECT CallbackKey, GenericKeyTypeKey, GenericKey, ReasonKey, EntryDate, EntryUser, CallbackDate, CallbackUser, CompletedDate, CompletedUser FROM [2am].[dbo].[Callback] WHERE";
        public const string callbackdatamodel_selectbykey = "SELECT CallbackKey, GenericKeyTypeKey, GenericKey, ReasonKey, EntryDate, EntryUser, CallbackDate, CallbackUser, CompletedDate, CompletedUser FROM [2am].[dbo].[Callback] WHERE CallbackKey = @PrimaryKey";
        public const string callbackdatamodel_delete = "DELETE FROM [2am].[dbo].[Callback] WHERE CallbackKey = @PrimaryKey";
        public const string callbackdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Callback] WHERE";
        public const string callbackdatamodel_insert = "INSERT INTO [2am].[dbo].[Callback] (GenericKeyTypeKey, GenericKey, ReasonKey, EntryDate, EntryUser, CallbackDate, CallbackUser, CompletedDate, CompletedUser) VALUES(@GenericKeyTypeKey, @GenericKey, @ReasonKey, @EntryDate, @EntryUser, @CallbackDate, @CallbackUser, @CompletedDate, @CompletedUser); select cast(scope_identity() as int)";
        public const string callbackdatamodel_update = "UPDATE [2am].[dbo].[Callback] SET GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey, ReasonKey = @ReasonKey, EntryDate = @EntryDate, EntryUser = @EntryUser, CallbackDate = @CallbackDate, CallbackUser = @CallbackUser, CompletedDate = @CompletedDate, CompletedUser = @CompletedUser WHERE CallbackKey = @CallbackKey";



        public const string auditlifepolicydatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, YearlyPremium, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, ClaimStatusDate, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, BrokerKey, DeathReassuranceRetention, IPBReassuranceRetention, LifePolicyTypeKey, AnniversaryDate, ClaimTypeKey FROM [2am].[dbo].[AuditLifePolicy] WHERE";
        public const string auditlifepolicydatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, YearlyPremium, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, ClaimStatusDate, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, BrokerKey, DeathReassuranceRetention, IPBReassuranceRetention, LifePolicyTypeKey, AnniversaryDate, ClaimTypeKey FROM [2am].[dbo].[AuditLifePolicy] WHERE AuditNumber = @PrimaryKey";
        public const string auditlifepolicydatamodel_delete = "DELETE FROM [2am].[dbo].[AuditLifePolicy] WHERE AuditNumber = @PrimaryKey";
        public const string auditlifepolicydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditLifePolicy] WHERE";
        public const string auditlifepolicydatamodel_insert = "INSERT INTO [2am].[dbo].[AuditLifePolicy] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, YearlyPremium, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall, InsurerKey, ExternalPolicyNumber, DateCeded, PriorityKey, ClaimStatusDate, PolicyHolderLEKey, RPARInsurer, RPARPolicyNumber, BrokerKey, DeathReassuranceRetention, IPBReassuranceRetention, LifePolicyTypeKey, AnniversaryDate, ClaimTypeKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @FinancialServiceKey, @DeathBenefit, @InstallmentProtectionBenefit, @DeathBenefitPremium, @InstallmentProtectionPremium, @PolicyStatusKey, @DateOfCommencement, @DateOfExpiry, @DeathRetentionLimit, @InstallmentProtectionRetentionLimit, @UpliftFactor, @JointDiscountFactor, @DateOfCancellation, @YearlyPremium, @DateOfAcceptance, @SumAssured, @DateLastUpdated, @Consultant, @ClaimStatusKey, @CurrentSumAssured, @PremiumShortfall, @InsurerKey, @ExternalPolicyNumber, @DateCeded, @PriorityKey, @ClaimStatusDate, @PolicyHolderLEKey, @RPARInsurer, @RPARPolicyNumber, @BrokerKey, @DeathReassuranceRetention, @IPBReassuranceRetention, @LifePolicyTypeKey, @AnniversaryDate, @ClaimTypeKey); select cast(scope_identity() as int)";
        public const string auditlifepolicydatamodel_update = "UPDATE [2am].[dbo].[AuditLifePolicy] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, FinancialServiceKey = @FinancialServiceKey, DeathBenefit = @DeathBenefit, InstallmentProtectionBenefit = @InstallmentProtectionBenefit, DeathBenefitPremium = @DeathBenefitPremium, InstallmentProtectionPremium = @InstallmentProtectionPremium, PolicyStatusKey = @PolicyStatusKey, DateOfCommencement = @DateOfCommencement, DateOfExpiry = @DateOfExpiry, DeathRetentionLimit = @DeathRetentionLimit, InstallmentProtectionRetentionLimit = @InstallmentProtectionRetentionLimit, UpliftFactor = @UpliftFactor, JointDiscountFactor = @JointDiscountFactor, DateOfCancellation = @DateOfCancellation, YearlyPremium = @YearlyPremium, DateOfAcceptance = @DateOfAcceptance, SumAssured = @SumAssured, DateLastUpdated = @DateLastUpdated, Consultant = @Consultant, ClaimStatusKey = @ClaimStatusKey, CurrentSumAssured = @CurrentSumAssured, PremiumShortfall = @PremiumShortfall, InsurerKey = @InsurerKey, ExternalPolicyNumber = @ExternalPolicyNumber, DateCeded = @DateCeded, PriorityKey = @PriorityKey, ClaimStatusDate = @ClaimStatusDate, PolicyHolderLEKey = @PolicyHolderLEKey, RPARInsurer = @RPARInsurer, RPARPolicyNumber = @RPARPolicyNumber, BrokerKey = @BrokerKey, DeathReassuranceRetention = @DeathReassuranceRetention, IPBReassuranceRetention = @IPBReassuranceRetention, LifePolicyTypeKey = @LifePolicyTypeKey, AnniversaryDate = @AnniversaryDate, ClaimTypeKey = @ClaimTypeKey WHERE AuditNumber = @AuditNumber";



        public const string comcorpofferpropertydetailsdatamodel_selectwhere = "SELECT OfferKey, SellerIDNo, SAHLOccupancyType, SAHLPropertyType, SAHLTitleType, SectionalTitleUnitNo, ComplexName, StreetNo, StreetName, Suburb, City, Province, PostalCode, ContactCellphone, ContactName, NamePropertyRegistered, StandErfNo, PortionNo, InsertDate, ChangeDate FROM [2am].[dbo].[ComcorpOfferPropertyDetails] WHERE";
        public const string comcorpofferpropertydetailsdatamodel_selectbykey = "SELECT OfferKey, SellerIDNo, SAHLOccupancyType, SAHLPropertyType, SAHLTitleType, SectionalTitleUnitNo, ComplexName, StreetNo, StreetName, Suburb, City, Province, PostalCode, ContactCellphone, ContactName, NamePropertyRegistered, StandErfNo, PortionNo, InsertDate, ChangeDate FROM [2am].[dbo].[ComcorpOfferPropertyDetails] WHERE OfferKey = @PrimaryKey";
        public const string comcorpofferpropertydetailsdatamodel_delete = "DELETE FROM [2am].[dbo].[ComcorpOfferPropertyDetails] WHERE OfferKey = @PrimaryKey";
        public const string comcorpofferpropertydetailsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ComcorpOfferPropertyDetails] WHERE";
        public const string comcorpofferpropertydetailsdatamodel_insert = "INSERT INTO [2am].[dbo].[ComcorpOfferPropertyDetails] (OfferKey, SellerIDNo, SAHLOccupancyType, SAHLPropertyType, SAHLTitleType, SectionalTitleUnitNo, ComplexName, StreetNo, StreetName, Suburb, City, Province, PostalCode, ContactCellphone, ContactName, NamePropertyRegistered, StandErfNo, PortionNo, InsertDate, ChangeDate) VALUES(@OfferKey, @SellerIDNo, @SAHLOccupancyType, @SAHLPropertyType, @SAHLTitleType, @SectionalTitleUnitNo, @ComplexName, @StreetNo, @StreetName, @Suburb, @City, @Province, @PostalCode, @ContactCellphone, @ContactName, @NamePropertyRegistered, @StandErfNo, @PortionNo, @InsertDate, @ChangeDate); ";
        public const string comcorpofferpropertydetailsdatamodel_update = "UPDATE [2am].[dbo].[ComcorpOfferPropertyDetails] SET OfferKey = @OfferKey, SellerIDNo = @SellerIDNo, SAHLOccupancyType = @SAHLOccupancyType, SAHLPropertyType = @SAHLPropertyType, SAHLTitleType = @SAHLTitleType, SectionalTitleUnitNo = @SectionalTitleUnitNo, ComplexName = @ComplexName, StreetNo = @StreetNo, StreetName = @StreetName, Suburb = @Suburb, City = @City, Province = @Province, PostalCode = @PostalCode, ContactCellphone = @ContactCellphone, ContactName = @ContactName, NamePropertyRegistered = @NamePropertyRegistered, StandErfNo = @StandErfNo, PortionNo = @PortionNo, InsertDate = @InsertDate, ChangeDate = @ChangeDate WHERE OfferKey = @OfferKey";



        public const string propertyaccessdetailsdatamodel_selectwhere = "SELECT PropertyAccessDetailsKey, PropertyKey, Contact1, Contact1Phone, Contact1WorkPhone, Contact1MobilePhone, Contact2, Contact2Phone FROM [2am].[dbo].[PropertyAccessDetails] WHERE";
        public const string propertyaccessdetailsdatamodel_selectbykey = "SELECT PropertyAccessDetailsKey, PropertyKey, Contact1, Contact1Phone, Contact1WorkPhone, Contact1MobilePhone, Contact2, Contact2Phone FROM [2am].[dbo].[PropertyAccessDetails] WHERE PropertyAccessDetailsKey = @PrimaryKey";
        public const string propertyaccessdetailsdatamodel_delete = "DELETE FROM [2am].[dbo].[PropertyAccessDetails] WHERE PropertyAccessDetailsKey = @PrimaryKey";
        public const string propertyaccessdetailsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PropertyAccessDetails] WHERE";
        public const string propertyaccessdetailsdatamodel_insert = "INSERT INTO [2am].[dbo].[PropertyAccessDetails] (PropertyKey, Contact1, Contact1Phone, Contact1WorkPhone, Contact1MobilePhone, Contact2, Contact2Phone) VALUES(@PropertyKey, @Contact1, @Contact1Phone, @Contact1WorkPhone, @Contact1MobilePhone, @Contact2, @Contact2Phone); select cast(scope_identity() as int)";
        public const string propertyaccessdetailsdatamodel_update = "UPDATE [2am].[dbo].[PropertyAccessDetails] SET PropertyKey = @PropertyKey, Contact1 = @Contact1, Contact1Phone = @Contact1Phone, Contact1WorkPhone = @Contact1WorkPhone, Contact1MobilePhone = @Contact1MobilePhone, Contact2 = @Contact2, Contact2Phone = @Contact2Phone WHERE PropertyAccessDetailsKey = @PropertyAccessDetailsKey";



        public const string calltypedatamodel_selectwhere = "SELECT CallTypeKey, Description, InBound FROM [2am].[dbo].[CallType] WHERE";
        public const string calltypedatamodel_selectbykey = "SELECT CallTypeKey, Description, InBound FROM [2am].[dbo].[CallType] WHERE CallTypeKey = @PrimaryKey";
        public const string calltypedatamodel_delete = "DELETE FROM [2am].[dbo].[CallType] WHERE CallTypeKey = @PrimaryKey";
        public const string calltypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CallType] WHERE";
        public const string calltypedatamodel_insert = "INSERT INTO [2am].[dbo].[CallType] (Description, InBound) VALUES(@Description, @InBound); select cast(scope_identity() as int)";
        public const string calltypedatamodel_update = "UPDATE [2am].[dbo].[CallType] SET Description = @Description, InBound = @InBound WHERE CallTypeKey = @CallTypeKey";



        public const string documenttypegroupconfigurationdatamodel_selectwhere = "SELECT DocumentTypeGroupConfigurationKey, DocumentTypeKey, DocumentGroupKey, OriginationSourceProductKey FROM [2am].[dbo].[DocumentTypeGroupConfiguration] WHERE";
        public const string documenttypegroupconfigurationdatamodel_selectbykey = "SELECT DocumentTypeGroupConfigurationKey, DocumentTypeKey, DocumentGroupKey, OriginationSourceProductKey FROM [2am].[dbo].[DocumentTypeGroupConfiguration] WHERE DocumentTypeGroupConfigurationKey = @PrimaryKey";
        public const string documenttypegroupconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentTypeGroupConfiguration] WHERE DocumentTypeGroupConfigurationKey = @PrimaryKey";
        public const string documenttypegroupconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentTypeGroupConfiguration] WHERE";
        public const string documenttypegroupconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentTypeGroupConfiguration] (DocumentTypeKey, DocumentGroupKey, OriginationSourceProductKey) VALUES(@DocumentTypeKey, @DocumentGroupKey, @OriginationSourceProductKey); select cast(scope_identity() as int)";
        public const string documenttypegroupconfigurationdatamodel_update = "UPDATE [2am].[dbo].[DocumentTypeGroupConfiguration] SET DocumentTypeKey = @DocumentTypeKey, DocumentGroupKey = @DocumentGroupKey, OriginationSourceProductKey = @OriginationSourceProductKey WHERE DocumentTypeGroupConfigurationKey = @DocumentTypeGroupConfigurationKey";



        public const string documenttypedatamodel_selectwhere = "SELECT DocumentTypeKey, Description, LegalEntity, GenericKeyTypeKey FROM [2am].[dbo].[DocumentType] WHERE";
        public const string documenttypedatamodel_selectbykey = "SELECT DocumentTypeKey, Description, LegalEntity, GenericKeyTypeKey FROM [2am].[dbo].[DocumentType] WHERE DocumentTypeKey = @PrimaryKey";
        public const string documenttypedatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentType] WHERE DocumentTypeKey = @PrimaryKey";
        public const string documenttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentType] WHERE";
        public const string documenttypedatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentType] (Description, LegalEntity, GenericKeyTypeKey) VALUES(@Description, @LegalEntity, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string documenttypedatamodel_update = "UPDATE [2am].[dbo].[DocumentType] SET Description = @Description, LegalEntity = @LegalEntity, GenericKeyTypeKey = @GenericKeyTypeKey WHERE DocumentTypeKey = @DocumentTypeKey";



        public const string productdatamodel_selectwhere = "SELECT ProductKey, Description, OriginateYN FROM [2am].[dbo].[Product] WHERE";
        public const string productdatamodel_selectbykey = "SELECT ProductKey, Description, OriginateYN FROM [2am].[dbo].[Product] WHERE ProductKey = @PrimaryKey";
        public const string productdatamodel_delete = "DELETE FROM [2am].[dbo].[Product] WHERE ProductKey = @PrimaryKey";
        public const string productdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Product] WHERE";
        public const string productdatamodel_insert = "INSERT INTO [2am].[dbo].[Product] (ProductKey, Description, OriginateYN) VALUES(@ProductKey, @Description, @OriginateYN); ";
        public const string productdatamodel_update = "UPDATE [2am].[dbo].[Product] SET ProductKey = @ProductKey, Description = @Description, OriginateYN = @OriginateYN WHERE ProductKey = @ProductKey";



        public const string profiletypedatamodel_selectwhere = "SELECT ProfileTypeKey, Description FROM [2am].[dbo].[ProfileType] WHERE";
        public const string profiletypedatamodel_selectbykey = "SELECT ProfileTypeKey, Description FROM [2am].[dbo].[ProfileType] WHERE ProfileTypeKey = @PrimaryKey";
        public const string profiletypedatamodel_delete = "DELETE FROM [2am].[dbo].[ProfileType] WHERE ProfileTypeKey = @PrimaryKey";
        public const string profiletypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ProfileType] WHERE";
        public const string profiletypedatamodel_insert = "INSERT INTO [2am].[dbo].[ProfileType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string profiletypedatamodel_update = "UPDATE [2am].[dbo].[ProfileType] SET Description = @Description WHERE ProfileTypeKey = @ProfileTypeKey";



        public const string offercalldatamodel_selectwhere = "SELECT OfferCallKey, OfferKey, CallTypeKey, CallDate FROM [2am].[dbo].[OfferCall] WHERE";
        public const string offercalldatamodel_selectbykey = "SELECT OfferCallKey, OfferKey, CallTypeKey, CallDate FROM [2am].[dbo].[OfferCall] WHERE OfferCallKey = @PrimaryKey";
        public const string offercalldatamodel_delete = "DELETE FROM [2am].[dbo].[OfferCall] WHERE OfferCallKey = @PrimaryKey";
        public const string offercalldatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferCall] WHERE";
        public const string offercalldatamodel_insert = "INSERT INTO [2am].[dbo].[OfferCall] (OfferKey, CallTypeKey, CallDate) VALUES(@OfferKey, @CallTypeKey, @CallDate); select cast(scope_identity() as int)";
        public const string offercalldatamodel_update = "UPDATE [2am].[dbo].[OfferCall] SET OfferKey = @OfferKey, CallTypeKey = @CallTypeKey, CallDate = @CallDate WHERE OfferCallKey = @OfferCallKey";



        public const string valuationcombinedthatchdatamodel_selectwhere = "SELECT ValuationKey, Value FROM [2am].[dbo].[ValuationCombinedThatch] WHERE";
        public const string valuationcombinedthatchdatamodel_selectbykey = "SELECT ValuationKey, Value FROM [2am].[dbo].[ValuationCombinedThatch] WHERE ValuationKey = @PrimaryKey";
        public const string valuationcombinedthatchdatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationCombinedThatch] WHERE ValuationKey = @PrimaryKey";
        public const string valuationcombinedthatchdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationCombinedThatch] WHERE";
        public const string valuationcombinedthatchdatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationCombinedThatch] (ValuationKey, Value) VALUES(@ValuationKey, @Value); ";
        public const string valuationcombinedthatchdatamodel_update = "UPDATE [2am].[dbo].[ValuationCombinedThatch] SET ValuationKey = @ValuationKey, Value = @Value WHERE ValuationKey = @ValuationKey";



        public const string hocinsurerdatamodel_selectwhere = "SELECT HOCInsurerKey, Description, HOCInsurerStatus FROM [2am].[dbo].[HOCInsurer] WHERE";
        public const string hocinsurerdatamodel_selectbykey = "SELECT HOCInsurerKey, Description, HOCInsurerStatus FROM [2am].[dbo].[HOCInsurer] WHERE HOCInsurerKey = @PrimaryKey";
        public const string hocinsurerdatamodel_delete = "DELETE FROM [2am].[dbo].[HOCInsurer] WHERE HOCInsurerKey = @PrimaryKey";
        public const string hocinsurerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCInsurer] WHERE";
        public const string hocinsurerdatamodel_insert = "INSERT INTO [2am].[dbo].[HOCInsurer] (HOCInsurerKey, Description, HOCInsurerStatus) VALUES(@HOCInsurerKey, @Description, @HOCInsurerStatus); ";
        public const string hocinsurerdatamodel_update = "UPDATE [2am].[dbo].[HOCInsurer] SET HOCInsurerKey = @HOCInsurerKey, Description = @Description, HOCInsurerStatus = @HOCInsurerStatus WHERE HOCInsurerKey = @HOCInsurerKey";



        public const string originationsourceproductdatamodel_selectwhere = "SELECT OriginationSourceProductKey, OriginationSourceKey, ProductKey FROM [2am].[dbo].[OriginationSourceProduct] WHERE";
        public const string originationsourceproductdatamodel_selectbykey = "SELECT OriginationSourceProductKey, OriginationSourceKey, ProductKey FROM [2am].[dbo].[OriginationSourceProduct] WHERE OriginationSourceProductKey = @PrimaryKey";
        public const string originationsourceproductdatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceProduct] WHERE OriginationSourceProductKey = @PrimaryKey";
        public const string originationsourceproductdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceProduct] WHERE";
        public const string originationsourceproductdatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceProduct] (OriginationSourceKey, ProductKey) VALUES(@OriginationSourceKey, @ProductKey); select cast(scope_identity() as int)";
        public const string originationsourceproductdatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceProduct] SET OriginationSourceKey = @OriginationSourceKey, ProductKey = @ProductKey WHERE OriginationSourceProductKey = @OriginationSourceProductKey";



        public const string legalentityexceptionreasondatamodel_selectwhere = "SELECT LegalEntityExceptionReasonKey, Description, Priority FROM [2am].[dbo].[LegalEntityExceptionReason] WHERE";
        public const string legalentityexceptionreasondatamodel_selectbykey = "SELECT LegalEntityExceptionReasonKey, Description, Priority FROM [2am].[dbo].[LegalEntityExceptionReason] WHERE LegalEntityExceptionReasonKey = @PrimaryKey";
        public const string legalentityexceptionreasondatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityExceptionReason] WHERE LegalEntityExceptionReasonKey = @PrimaryKey";
        public const string legalentityexceptionreasondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityExceptionReason] WHERE";
        public const string legalentityexceptionreasondatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityExceptionReason] (Description, Priority) VALUES(@Description, @Priority); select cast(scope_identity() as int)";
        public const string legalentityexceptionreasondatamodel_update = "UPDATE [2am].[dbo].[LegalEntityExceptionReason] SET Description = @Description, Priority = @Priority WHERE LegalEntityExceptionReasonKey = @LegalEntityExceptionReasonKey";



        public const string userprofiledatamodel_selectwhere = "SELECT UserProfileKey, ADUserName, ProfileTypeKey, Value FROM [2am].[dbo].[UserProfile] WHERE";
        public const string userprofiledatamodel_selectbykey = "SELECT UserProfileKey, ADUserName, ProfileTypeKey, Value FROM [2am].[dbo].[UserProfile] WHERE UserProfileKey = @PrimaryKey";
        public const string userprofiledatamodel_delete = "DELETE FROM [2am].[dbo].[UserProfile] WHERE UserProfileKey = @PrimaryKey";
        public const string userprofiledatamodel_deletewhere = "DELETE FROM [2am].[dbo].[UserProfile] WHERE";
        public const string userprofiledatamodel_insert = "INSERT INTO [2am].[dbo].[UserProfile] (ADUserName, ProfileTypeKey, Value) VALUES(@ADUserName, @ProfileTypeKey, @Value); select cast(scope_identity() as int)";
        public const string userprofiledatamodel_update = "UPDATE [2am].[dbo].[UserProfile] SET ADUserName = @ADUserName, ProfileTypeKey = @ProfileTypeKey, Value = @Value WHERE UserProfileKey = @UserProfileKey";



        public const string historynotmigrated_originationcleanupdatamodel_selectwhere = "SELECT HistoryKey, AccountKey, ADUserKey, UserName, date, Action, Comments FROM [2am].[dbo].[HistoryNotMigrated_OriginationCleanUp] WHERE";
        public const string historynotmigrated_originationcleanupdatamodel_selectbykey = "SELECT HistoryKey, AccountKey, ADUserKey, UserName, date, Action, Comments FROM [2am].[dbo].[HistoryNotMigrated_OriginationCleanUp] WHERE  = @PrimaryKey";
        public const string historynotmigrated_originationcleanupdatamodel_delete = "DELETE FROM [2am].[dbo].[HistoryNotMigrated_OriginationCleanUp] WHERE  = @PrimaryKey";
        public const string historynotmigrated_originationcleanupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HistoryNotMigrated_OriginationCleanUp] WHERE";
        public const string historynotmigrated_originationcleanupdatamodel_insert = "INSERT INTO [2am].[dbo].[HistoryNotMigrated_OriginationCleanUp] (HistoryKey, AccountKey, ADUserKey, UserName, date, Action, Comments) VALUES(@HistoryKey, @AccountKey, @ADUserKey, @UserName, @date, @Action, @Comments); ";
        public const string historynotmigrated_originationcleanupdatamodel_update = "UPDATE [2am].[dbo].[HistoryNotMigrated_OriginationCleanUp] SET HistoryKey = @HistoryKey, AccountKey = @AccountKey, ADUserKey = @ADUserKey, UserName = @UserName, date = @date, Action = @Action, Comments = @Comments WHERE  = @";



        public const string suburbdatamodel_selectwhere = "SELECT SuburbKey, Description, CityKey, PostalCode FROM [2am].[dbo].[Suburb] WHERE";
        public const string suburbdatamodel_selectbykey = "SELECT SuburbKey, Description, CityKey, PostalCode FROM [2am].[dbo].[Suburb] WHERE SuburbKey = @PrimaryKey";
        public const string suburbdatamodel_delete = "DELETE FROM [2am].[dbo].[Suburb] WHERE SuburbKey = @PrimaryKey";
        public const string suburbdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Suburb] WHERE";
        public const string suburbdatamodel_insert = "INSERT INTO [2am].[dbo].[Suburb] (Description, CityKey, PostalCode) VALUES(@Description, @CityKey, @PostalCode); select cast(scope_identity() as int)";
        public const string suburbdatamodel_update = "UPDATE [2am].[dbo].[Suburb] SET Description = @Description, CityKey = @CityKey, PostalCode = @PostalCode WHERE SuburbKey = @SuburbKey";



        public const string offermailingaddressdatamodel_selectwhere = "SELECT OfferMailingAddressKey, OfferKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey FROM [2am].[dbo].[OfferMailingAddress] WHERE";
        public const string offermailingaddressdatamodel_selectbykey = "SELECT OfferMailingAddressKey, OfferKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey FROM [2am].[dbo].[OfferMailingAddress] WHERE OfferMailingAddressKey = @PrimaryKey";
        public const string offermailingaddressdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferMailingAddress] WHERE OfferMailingAddressKey = @PrimaryKey";
        public const string offermailingaddressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferMailingAddress] WHERE";
        public const string offermailingaddressdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferMailingAddress] (OfferKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey) VALUES(@OfferKey, @AddressKey, @OnlineStatement, @OnlineStatementFormatKey, @LanguageKey, @LegalEntityKey, @CorrespondenceMediumKey); select cast(scope_identity() as int)";
        public const string offermailingaddressdatamodel_update = "UPDATE [2am].[dbo].[OfferMailingAddress] SET OfferKey = @OfferKey, AddressKey = @AddressKey, OnlineStatement = @OnlineStatement, OnlineStatementFormatKey = @OnlineStatementFormatKey, LanguageKey = @LanguageKey, LegalEntityKey = @LegalEntityKey, CorrespondenceMediumKey = @CorrespondenceMediumKey WHERE OfferMailingAddressKey = @OfferMailingAddressKey";



        public const string vendordatamodel_selectwhere = "SELECT VendorKey, ParentKey, VendorCode, OrganisationStructureKey, LegalEntityKey, GeneralStatusKey FROM [2am].[dbo].[Vendor] WHERE";
        public const string vendordatamodel_selectbykey = "SELECT VendorKey, ParentKey, VendorCode, OrganisationStructureKey, LegalEntityKey, GeneralStatusKey FROM [2am].[dbo].[Vendor] WHERE VendorKey = @PrimaryKey";
        public const string vendordatamodel_delete = "DELETE FROM [2am].[dbo].[Vendor] WHERE VendorKey = @PrimaryKey";
        public const string vendordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Vendor] WHERE";
        public const string vendordatamodel_insert = "INSERT INTO [2am].[dbo].[Vendor] (ParentKey, VendorCode, OrganisationStructureKey, LegalEntityKey, GeneralStatusKey) VALUES(@ParentKey, @VendorCode, @OrganisationStructureKey, @LegalEntityKey, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string vendordatamodel_update = "UPDATE [2am].[dbo].[Vendor] SET ParentKey = @ParentKey, VendorCode = @VendorCode, OrganisationStructureKey = @OrganisationStructureKey, LegalEntityKey = @LegalEntityKey, GeneralStatusKey = @GeneralStatusKey WHERE VendorKey = @VendorKey";



        public const string genericsetdatamodel_selectwhere = "SELECT GenericSetKey, GenericSetDefinitionKey, GenericKey FROM [2am].[dbo].[GenericSet] WHERE";
        public const string genericsetdatamodel_selectbykey = "SELECT GenericSetKey, GenericSetDefinitionKey, GenericKey FROM [2am].[dbo].[GenericSet] WHERE GenericSetKey = @PrimaryKey";
        public const string genericsetdatamodel_delete = "DELETE FROM [2am].[dbo].[GenericSet] WHERE GenericSetKey = @PrimaryKey";
        public const string genericsetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[GenericSet] WHERE";
        public const string genericsetdatamodel_insert = "INSERT INTO [2am].[dbo].[GenericSet] (GenericSetDefinitionKey, GenericKey) VALUES(@GenericSetDefinitionKey, @GenericKey); select cast(scope_identity() as int)";
        public const string genericsetdatamodel_update = "UPDATE [2am].[dbo].[GenericSet] SET GenericSetDefinitionKey = @GenericSetDefinitionKey, GenericKey = @GenericKey WHERE GenericSetKey = @GenericSetKey";



        public const string tmp_failedlegalentitystreetaddressdatamodel_selectwhere = "SELECT LegalEntityKey, FailedStreetMigrationKey, FailedPostalMigrationKey, IsCleaned FROM [2am].[dbo].[tmp_FailedLegalEntityStreetAddress] WHERE";
        public const string tmp_failedlegalentitystreetaddressdatamodel_selectbykey = "SELECT LegalEntityKey, FailedStreetMigrationKey, FailedPostalMigrationKey, IsCleaned FROM [2am].[dbo].[tmp_FailedLegalEntityStreetAddress] WHERE  = @PrimaryKey";
        public const string tmp_failedlegalentitystreetaddressdatamodel_delete = "DELETE FROM [2am].[dbo].[tmp_FailedLegalEntityStreetAddress] WHERE  = @PrimaryKey";
        public const string tmp_failedlegalentitystreetaddressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[tmp_FailedLegalEntityStreetAddress] WHERE";
        public const string tmp_failedlegalentitystreetaddressdatamodel_insert = "INSERT INTO [2am].[dbo].[tmp_FailedLegalEntityStreetAddress] (LegalEntityKey, FailedStreetMigrationKey, FailedPostalMigrationKey, IsCleaned) VALUES(@LegalEntityKey, @FailedStreetMigrationKey, @FailedPostalMigrationKey, @IsCleaned); ";
        public const string tmp_failedlegalentitystreetaddressdatamodel_update = "UPDATE [2am].[dbo].[tmp_FailedLegalEntityStreetAddress] SET LegalEntityKey = @LegalEntityKey, FailedStreetMigrationKey = @FailedStreetMigrationKey, FailedPostalMigrationKey = @FailedPostalMigrationKey, IsCleaned = @IsCleaned WHERE  = @";



        public const string audit_checkdatamodel_selectwhere = "SELECT TableName, Count FROM [2am].[dbo].[Audit_Check] WHERE";
        public const string audit_checkdatamodel_selectbykey = "SELECT TableName, Count FROM [2am].[dbo].[Audit_Check] WHERE  = @PrimaryKey";
        public const string audit_checkdatamodel_delete = "DELETE FROM [2am].[dbo].[Audit_Check] WHERE  = @PrimaryKey";
        public const string audit_checkdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Audit_Check] WHERE";
        public const string audit_checkdatamodel_insert = "INSERT INTO [2am].[dbo].[Audit_Check] (TableName, Count) VALUES(@TableName, @Count); ";
        public const string audit_checkdatamodel_update = "UPDATE [2am].[dbo].[Audit_Check] SET TableName = @TableName, Count = @Count WHERE  = @";



        public const string auditloanagreementdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LoanAgreementKey, AgreementDate, Amount, UserName, BondKey, ChangeDate FROM [2am].[dbo].[AuditLoanAgreement] WHERE";
        public const string auditloanagreementdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LoanAgreementKey, AgreementDate, Amount, UserName, BondKey, ChangeDate FROM [2am].[dbo].[AuditLoanAgreement] WHERE AuditNumber = @PrimaryKey";
        public const string auditloanagreementdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditLoanAgreement] WHERE AuditNumber = @PrimaryKey";
        public const string auditloanagreementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditLoanAgreement] WHERE";
        public const string auditloanagreementdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditLoanAgreement] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LoanAgreementKey, AgreementDate, Amount, UserName, BondKey, ChangeDate) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @LoanAgreementKey, @AgreementDate, @Amount, @UserName, @BondKey, @ChangeDate); select cast(scope_identity() as int)";
        public const string auditloanagreementdatamodel_update = "UPDATE [2am].[dbo].[AuditLoanAgreement] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, LoanAgreementKey = @LoanAgreementKey, AgreementDate = @AgreementDate, Amount = @Amount, UserName = @UserName, BondKey = @BondKey, ChangeDate = @ChangeDate WHERE AuditNumber = @AuditNumber";



        public const string bulkbatchdatamodel_selectwhere = "SELECT BulkBatchKey, BulkBatchStatusKey, Description, BulkBatchTypeKey, IdentifierReferenceKey, EffectiveDate, StartDateTime, CompletedDateTime, FileName, UserID, ChangeDate FROM [2am].[dbo].[BulkBatch] WHERE";
        public const string bulkbatchdatamodel_selectbykey = "SELECT BulkBatchKey, BulkBatchStatusKey, Description, BulkBatchTypeKey, IdentifierReferenceKey, EffectiveDate, StartDateTime, CompletedDateTime, FileName, UserID, ChangeDate FROM [2am].[dbo].[BulkBatch] WHERE BulkBatchKey = @PrimaryKey";
        public const string bulkbatchdatamodel_delete = "DELETE FROM [2am].[dbo].[BulkBatch] WHERE BulkBatchKey = @PrimaryKey";
        public const string bulkbatchdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BulkBatch] WHERE";
        public const string bulkbatchdatamodel_insert = "INSERT INTO [2am].[dbo].[BulkBatch] (BulkBatchStatusKey, Description, BulkBatchTypeKey, IdentifierReferenceKey, EffectiveDate, StartDateTime, CompletedDateTime, FileName, UserID, ChangeDate) VALUES(@BulkBatchStatusKey, @Description, @BulkBatchTypeKey, @IdentifierReferenceKey, @EffectiveDate, @StartDateTime, @CompletedDateTime, @FileName, @UserID, @ChangeDate); select cast(scope_identity() as int)";
        public const string bulkbatchdatamodel_update = "UPDATE [2am].[dbo].[BulkBatch] SET BulkBatchStatusKey = @BulkBatchStatusKey, Description = @Description, BulkBatchTypeKey = @BulkBatchTypeKey, IdentifierReferenceKey = @IdentifierReferenceKey, EffectiveDate = @EffectiveDate, StartDateTime = @StartDateTime, CompletedDateTime = @CompletedDateTime, FileName = @FileName, UserID = @UserID, ChangeDate = @ChangeDate WHERE BulkBatchKey = @BulkBatchKey";



        public const string tmp_failedlegalentitypostaladdressdatamodel_selectwhere = "SELECT LegalEntityKey, FailedStreetMigrationKey, FailedPostalMigrationKey, IsCleaned FROM [2am].[dbo].[tmp_FailedLegalEntityPostalAddress] WHERE";
        public const string tmp_failedlegalentitypostaladdressdatamodel_selectbykey = "SELECT LegalEntityKey, FailedStreetMigrationKey, FailedPostalMigrationKey, IsCleaned FROM [2am].[dbo].[tmp_FailedLegalEntityPostalAddress] WHERE  = @PrimaryKey";
        public const string tmp_failedlegalentitypostaladdressdatamodel_delete = "DELETE FROM [2am].[dbo].[tmp_FailedLegalEntityPostalAddress] WHERE  = @PrimaryKey";
        public const string tmp_failedlegalentitypostaladdressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[tmp_FailedLegalEntityPostalAddress] WHERE";
        public const string tmp_failedlegalentitypostaladdressdatamodel_insert = "INSERT INTO [2am].[dbo].[tmp_FailedLegalEntityPostalAddress] (LegalEntityKey, FailedStreetMigrationKey, FailedPostalMigrationKey, IsCleaned) VALUES(@LegalEntityKey, @FailedStreetMigrationKey, @FailedPostalMigrationKey, @IsCleaned); ";
        public const string tmp_failedlegalentitypostaladdressdatamodel_update = "UPDATE [2am].[dbo].[tmp_FailedLegalEntityPostalAddress] SET LegalEntityKey = @LegalEntityKey, FailedStreetMigrationKey = @FailedStreetMigrationKey, FailedPostalMigrationKey = @FailedPostalMigrationKey, IsCleaned = @IsCleaned WHERE  = @";



        public const string aduserdatamodel_selectwhere = "SELECT ADUserKey, ADUserName, GeneralStatusKey, Password, PasswordQuestion, PasswordAnswer, LegalEntityKey FROM [2am].[dbo].[ADUser] WHERE";
        public const string aduserdatamodel_selectbykey = "SELECT ADUserKey, ADUserName, GeneralStatusKey, Password, PasswordQuestion, PasswordAnswer, LegalEntityKey FROM [2am].[dbo].[ADUser] WHERE ADUserKey = @PrimaryKey";
        public const string aduserdatamodel_delete = "DELETE FROM [2am].[dbo].[ADUser] WHERE ADUserKey = @PrimaryKey";
        public const string aduserdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ADUser] WHERE";
        public const string aduserdatamodel_insert = "INSERT INTO [2am].[dbo].[ADUser] (ADUserName, GeneralStatusKey, Password, PasswordQuestion, PasswordAnswer, LegalEntityKey) VALUES(@ADUserName, @GeneralStatusKey, @Password, @PasswordQuestion, @PasswordAnswer, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string aduserdatamodel_update = "UPDATE [2am].[dbo].[ADUser] SET ADUserName = @ADUserName, GeneralStatusKey = @GeneralStatusKey, Password = @Password, PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer, LegalEntityKey = @LegalEntityKey WHERE ADUserKey = @ADUserKey";



        public const string areaclassificationdatamodel_selectwhere = "SELECT AreaClassificationKey, Description FROM [2am].[dbo].[AreaClassification] WHERE";
        public const string areaclassificationdatamodel_selectbykey = "SELECT AreaClassificationKey, Description FROM [2am].[dbo].[AreaClassification] WHERE AreaClassificationKey = @PrimaryKey";
        public const string areaclassificationdatamodel_delete = "DELETE FROM [2am].[dbo].[AreaClassification] WHERE AreaClassificationKey = @PrimaryKey";
        public const string areaclassificationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AreaClassification] WHERE";
        public const string areaclassificationdatamodel_insert = "INSERT INTO [2am].[dbo].[AreaClassification] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string areaclassificationdatamodel_update = "UPDATE [2am].[dbo].[AreaClassification] SET Description = @Description WHERE AreaClassificationKey = @AreaClassificationKey";



        public const string hocroofdatamodel_selectwhere = "SELECT HOCRoofKey, Description FROM [2am].[dbo].[HOCRoof] WHERE";
        public const string hocroofdatamodel_selectbykey = "SELECT HOCRoofKey, Description FROM [2am].[dbo].[HOCRoof] WHERE HOCRoofKey = @PrimaryKey";
        public const string hocroofdatamodel_delete = "DELETE FROM [2am].[dbo].[HOCRoof] WHERE HOCRoofKey = @PrimaryKey";
        public const string hocroofdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCRoof] WHERE";
        public const string hocroofdatamodel_insert = "INSERT INTO [2am].[dbo].[HOCRoof] (HOCRoofKey, Description) VALUES(@HOCRoofKey, @Description); ";
        public const string hocroofdatamodel_update = "UPDATE [2am].[dbo].[HOCRoof] SET HOCRoofKey = @HOCRoofKey, Description = @Description WHERE HOCRoofKey = @HOCRoofKey";



        public const string importfiledatamodel_selectwhere = "SELECT FileKey, FileName, FileType, DateImported, Status, UserID, XML_Data FROM [2am].[dbo].[ImportFile] WHERE";
        public const string importfiledatamodel_selectbykey = "SELECT FileKey, FileName, FileType, DateImported, Status, UserID, XML_Data FROM [2am].[dbo].[ImportFile] WHERE FileKey = @PrimaryKey";
        public const string importfiledatamodel_delete = "DELETE FROM [2am].[dbo].[ImportFile] WHERE FileKey = @PrimaryKey";
        public const string importfiledatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportFile] WHERE";
        public const string importfiledatamodel_insert = "INSERT INTO [2am].[dbo].[ImportFile] (FileName, FileType, DateImported, Status, UserID, XML_Data) VALUES(@FileName, @FileType, @DateImported, @Status, @UserID, @XML_Data); select cast(scope_identity() as int)";
        public const string importfiledatamodel_update = "UPDATE [2am].[dbo].[ImportFile] SET FileName = @FileName, FileType = @FileType, DateImported = @DateImported, Status = @Status, UserID = @UserID, XML_Data = @XML_Data WHERE FileKey = @FileKey";



        public const string superlorecalcdatamodel_selectwhere = "SELECT FinancialServiceKey, ConvertedDate, AccumulatedLoyaltyBenefit, RecalcAccumLB FROM [2am].[dbo].[SuperLoRecalc] WHERE";
        public const string superlorecalcdatamodel_selectbykey = "SELECT FinancialServiceKey, ConvertedDate, AccumulatedLoyaltyBenefit, RecalcAccumLB FROM [2am].[dbo].[SuperLoRecalc] WHERE  = @PrimaryKey";
        public const string superlorecalcdatamodel_delete = "DELETE FROM [2am].[dbo].[SuperLoRecalc] WHERE  = @PrimaryKey";
        public const string superlorecalcdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[SuperLoRecalc] WHERE";
        public const string superlorecalcdatamodel_insert = "INSERT INTO [2am].[dbo].[SuperLoRecalc] (FinancialServiceKey, ConvertedDate, AccumulatedLoyaltyBenefit, RecalcAccumLB) VALUES(@FinancialServiceKey, @ConvertedDate, @AccumulatedLoyaltyBenefit, @RecalcAccumLB); ";
        public const string superlorecalcdatamodel_update = "UPDATE [2am].[dbo].[SuperLoRecalc] SET FinancialServiceKey = @FinancialServiceKey, ConvertedDate = @ConvertedDate, AccumulatedLoyaltyBenefit = @AccumulatedLoyaltyBenefit, RecalcAccumLB = @RecalcAccumLB WHERE  = @";



        public const string capcreditbrokertokendatamodel_selectwhere = "SELECT CapCreditBrokerKey, BrokerKey, LastAssigned FROM [2am].[dbo].[CapCreditBrokerToken] WHERE";
        public const string capcreditbrokertokendatamodel_selectbykey = "SELECT CapCreditBrokerKey, BrokerKey, LastAssigned FROM [2am].[dbo].[CapCreditBrokerToken] WHERE CapCreditBrokerKey = @PrimaryKey";
        public const string capcreditbrokertokendatamodel_delete = "DELETE FROM [2am].[dbo].[CapCreditBrokerToken] WHERE CapCreditBrokerKey = @PrimaryKey";
        public const string capcreditbrokertokendatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CapCreditBrokerToken] WHERE";
        public const string capcreditbrokertokendatamodel_insert = "INSERT INTO [2am].[dbo].[CapCreditBrokerToken] (BrokerKey, LastAssigned) VALUES(@BrokerKey, @LastAssigned); select cast(scope_identity() as int)";
        public const string capcreditbrokertokendatamodel_update = "UPDATE [2am].[dbo].[CapCreditBrokerToken] SET BrokerKey = @BrokerKey, LastAssigned = @LastAssigned WHERE CapCreditBrokerKey = @CapCreditBrokerKey";



        public const string documentsetconfigdatamodel_selectwhere = "SELECT DocumentSetConfigKey, DocumentSetKey, DocumentTypeKey, RuleItemKey FROM [2am].[dbo].[DocumentSetConfig] WHERE";
        public const string documentsetconfigdatamodel_selectbykey = "SELECT DocumentSetConfigKey, DocumentSetKey, DocumentTypeKey, RuleItemKey FROM [2am].[dbo].[DocumentSetConfig] WHERE DocumentSetConfigKey = @PrimaryKey";
        public const string documentsetconfigdatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentSetConfig] WHERE DocumentSetConfigKey = @PrimaryKey";
        public const string documentsetconfigdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentSetConfig] WHERE";
        public const string documentsetconfigdatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentSetConfig] (DocumentSetKey, DocumentTypeKey, RuleItemKey) VALUES(@DocumentSetKey, @DocumentTypeKey, @RuleItemKey); select cast(scope_identity() as int)";
        public const string documentsetconfigdatamodel_update = "UPDATE [2am].[dbo].[DocumentSetConfig] SET DocumentSetKey = @DocumentSetKey, DocumentTypeKey = @DocumentTypeKey, RuleItemKey = @RuleItemKey WHERE DocumentSetConfigKey = @DocumentSetConfigKey";



        public const string propertydataproviderdataservicedatamodel_selectwhere = "SELECT PropertyDataProviderDataServiceKey, DataProviderDataServiceKey FROM [2am].[dbo].[PropertyDataProviderDataService] WHERE";
        public const string propertydataproviderdataservicedatamodel_selectbykey = "SELECT PropertyDataProviderDataServiceKey, DataProviderDataServiceKey FROM [2am].[dbo].[PropertyDataProviderDataService] WHERE PropertyDataProviderDataServiceKey = @PrimaryKey";
        public const string propertydataproviderdataservicedatamodel_delete = "DELETE FROM [2am].[dbo].[PropertyDataProviderDataService] WHERE PropertyDataProviderDataServiceKey = @PrimaryKey";
        public const string propertydataproviderdataservicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PropertyDataProviderDataService] WHERE";
        public const string propertydataproviderdataservicedatamodel_insert = "INSERT INTO [2am].[dbo].[PropertyDataProviderDataService] (PropertyDataProviderDataServiceKey, DataProviderDataServiceKey) VALUES(@PropertyDataProviderDataServiceKey, @DataProviderDataServiceKey); ";
        public const string propertydataproviderdataservicedatamodel_update = "UPDATE [2am].[dbo].[PropertyDataProviderDataService] SET PropertyDataProviderDataServiceKey = @PropertyDataProviderDataServiceKey, DataProviderDataServiceKey = @DataProviderDataServiceKey WHERE PropertyDataProviderDataServiceKey = @PropertyDataProviderDataServiceKey";



        public const string tempcreditcriteriadatamodel_selectwhere = "SELECT CreditCriteriaKey, CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria FROM [2am].[dbo].[TempCreditCriteria] WHERE";
        public const string tempcreditcriteriadatamodel_selectbykey = "SELECT CreditCriteriaKey, CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria FROM [2am].[dbo].[TempCreditCriteria] WHERE CreditCriteriaKey = @PrimaryKey";
        public const string tempcreditcriteriadatamodel_delete = "DELETE FROM [2am].[dbo].[TempCreditCriteria] WHERE CreditCriteriaKey = @PrimaryKey";
        public const string tempcreditcriteriadatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TempCreditCriteria] WHERE";
        public const string tempcreditcriteriadatamodel_insert = "INSERT INTO [2am].[dbo].[TempCreditCriteria] (CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria) VALUES(@CreditMatrixKey, @MarginKey, @CategoryKey, @EmploymentTypeKey, @MortgageLoanPurposeKey, @MinLoanAmount, @MaxLoanAmount, @MinPropertyValue, @MaxPropertyValue, @LTV, @PTI, @MinIncomeAmount, @ExceptionCriteria); select cast(scope_identity() as int)";
        public const string tempcreditcriteriadatamodel_update = "UPDATE [2am].[dbo].[TempCreditCriteria] SET CreditMatrixKey = @CreditMatrixKey, MarginKey = @MarginKey, CategoryKey = @CategoryKey, EmploymentTypeKey = @EmploymentTypeKey, MortgageLoanPurposeKey = @MortgageLoanPurposeKey, MinLoanAmount = @MinLoanAmount, MaxLoanAmount = @MaxLoanAmount, MinPropertyValue = @MinPropertyValue, MaxPropertyValue = @MaxPropertyValue, LTV = @LTV, PTI = @PTI, MinIncomeAmount = @MinIncomeAmount, ExceptionCriteria = @ExceptionCriteria WHERE CreditCriteriaKey = @CreditCriteriaKey";



        public const string feesdatamodel_selectwhere = "SELECT FeeRange, FeeNaturalTransferDuty, FeeNaturalConveyancing, FeeNaturalVAT, FeeLegalTransferDuty, FeeLegalConveyancing, FeeLegalVAT, FeeBondStamps, FeeBondConveyancing, FeeBondVAT, FeeAdmin, FeeValuation, FeeCancelDuty, FeeCancelConveyancing, FeeCancelVAT, FeeFlexiSwitch, FeeRCSBondConveyancing, FeeRCSBondVAT, FeeDeedsOffice, FeeRCSBondPreparation, FeeBondConveyancing80Pct, FeeBondVAT80Pct, FeeBondConveyancingNoFICA, FeeBondNoFICAVAT FROM [2am].[dbo].[Fees] WHERE";
        public const string feesdatamodel_selectbykey = "SELECT FeeRange, FeeNaturalTransferDuty, FeeNaturalConveyancing, FeeNaturalVAT, FeeLegalTransferDuty, FeeLegalConveyancing, FeeLegalVAT, FeeBondStamps, FeeBondConveyancing, FeeBondVAT, FeeAdmin, FeeValuation, FeeCancelDuty, FeeCancelConveyancing, FeeCancelVAT, FeeFlexiSwitch, FeeRCSBondConveyancing, FeeRCSBondVAT, FeeDeedsOffice, FeeRCSBondPreparation, FeeBondConveyancing80Pct, FeeBondVAT80Pct, FeeBondConveyancingNoFICA, FeeBondNoFICAVAT FROM [2am].[dbo].[Fees] WHERE FeeRange = @PrimaryKey";
        public const string feesdatamodel_delete = "DELETE FROM [2am].[dbo].[Fees] WHERE FeeRange = @PrimaryKey";
        public const string feesdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Fees] WHERE";
        public const string feesdatamodel_insert = "INSERT INTO [2am].[dbo].[Fees] (FeeRange, FeeNaturalTransferDuty, FeeNaturalConveyancing, FeeNaturalVAT, FeeLegalTransferDuty, FeeLegalConveyancing, FeeLegalVAT, FeeBondStamps, FeeBondConveyancing, FeeBondVAT, FeeAdmin, FeeValuation, FeeCancelDuty, FeeCancelConveyancing, FeeCancelVAT, FeeFlexiSwitch, FeeRCSBondConveyancing, FeeRCSBondVAT, FeeDeedsOffice, FeeRCSBondPreparation, FeeBondConveyancing80Pct, FeeBondVAT80Pct, FeeBondConveyancingNoFICA, FeeBondNoFICAVAT) VALUES(@FeeRange, @FeeNaturalTransferDuty, @FeeNaturalConveyancing, @FeeNaturalVAT, @FeeLegalTransferDuty, @FeeLegalConveyancing, @FeeLegalVAT, @FeeBondStamps, @FeeBondConveyancing, @FeeBondVAT, @FeeAdmin, @FeeValuation, @FeeCancelDuty, @FeeCancelConveyancing, @FeeCancelVAT, @FeeFlexiSwitch, @FeeRCSBondConveyancing, @FeeRCSBondVAT, @FeeDeedsOffice, @FeeRCSBondPreparation, @FeeBondConveyancing80Pct, @FeeBondVAT80Pct, @FeeBondConveyancingNoFICA, @FeeBondNoFICAVAT); ";
        public const string feesdatamodel_update = "UPDATE [2am].[dbo].[Fees] SET FeeRange = @FeeRange, FeeNaturalTransferDuty = @FeeNaturalTransferDuty, FeeNaturalConveyancing = @FeeNaturalConveyancing, FeeNaturalVAT = @FeeNaturalVAT, FeeLegalTransferDuty = @FeeLegalTransferDuty, FeeLegalConveyancing = @FeeLegalConveyancing, FeeLegalVAT = @FeeLegalVAT, FeeBondStamps = @FeeBondStamps, FeeBondConveyancing = @FeeBondConveyancing, FeeBondVAT = @FeeBondVAT, FeeAdmin = @FeeAdmin, FeeValuation = @FeeValuation, FeeCancelDuty = @FeeCancelDuty, FeeCancelConveyancing = @FeeCancelConveyancing, FeeCancelVAT = @FeeCancelVAT, FeeFlexiSwitch = @FeeFlexiSwitch, FeeRCSBondConveyancing = @FeeRCSBondConveyancing, FeeRCSBondVAT = @FeeRCSBondVAT, FeeDeedsOffice = @FeeDeedsOffice, FeeRCSBondPreparation = @FeeRCSBondPreparation, FeeBondConveyancing80Pct = @FeeBondConveyancing80Pct, FeeBondVAT80Pct = @FeeBondVAT80Pct, FeeBondConveyancingNoFICA = @FeeBondConveyancingNoFICA, FeeBondNoFICAVAT = @FeeBondNoFICAVAT WHERE FeeRange = @FeeRange";



        public const string bulkbatchparameterdatamodel_selectwhere = "SELECT BulkBatchParameterKey, BulkBatchKey, ParameterName, ParameterValue FROM [2am].[dbo].[BulkBatchParameter] WHERE";
        public const string bulkbatchparameterdatamodel_selectbykey = "SELECT BulkBatchParameterKey, BulkBatchKey, ParameterName, ParameterValue FROM [2am].[dbo].[BulkBatchParameter] WHERE BulkBatchParameterKey = @PrimaryKey";
        public const string bulkbatchparameterdatamodel_delete = "DELETE FROM [2am].[dbo].[BulkBatchParameter] WHERE BulkBatchParameterKey = @PrimaryKey";
        public const string bulkbatchparameterdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BulkBatchParameter] WHERE";
        public const string bulkbatchparameterdatamodel_insert = "INSERT INTO [2am].[dbo].[BulkBatchParameter] (BulkBatchKey, ParameterName, ParameterValue) VALUES(@BulkBatchKey, @ParameterName, @ParameterValue); select cast(scope_identity() as int)";
        public const string bulkbatchparameterdatamodel_update = "UPDATE [2am].[dbo].[BulkBatchParameter] SET BulkBatchKey = @BulkBatchKey, ParameterName = @ParameterName, ParameterValue = @ParameterValue WHERE BulkBatchParameterKey = @BulkBatchParameterKey";



        public const string spvrollbackdatamodel_selectwhere = "SELECT Loannumber FROM [2am].[dbo].[SPVRollback] WHERE";
        public const string spvrollbackdatamodel_selectbykey = "SELECT Loannumber FROM [2am].[dbo].[SPVRollback] WHERE  = @PrimaryKey";
        public const string spvrollbackdatamodel_delete = "DELETE FROM [2am].[dbo].[SPVRollback] WHERE  = @PrimaryKey";
        public const string spvrollbackdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[SPVRollback] WHERE";
        public const string spvrollbackdatamodel_insert = "INSERT INTO [2am].[dbo].[SPVRollback] (Loannumber) VALUES(@Loannumber); ";
        public const string spvrollbackdatamodel_update = "UPDATE [2am].[dbo].[SPVRollback] SET Loannumber = @Loannumber WHERE  = @";



        public const string userorganisationstructuredatamodel_selectwhere = "SELECT UserOrganisationStructureKey, ADUserKey, OrganisationStructureKey, GenericKey, GenericKeyTypeKey, GeneralStatusKey FROM [2am].[dbo].[UserOrganisationStructure] WHERE";
        public const string userorganisationstructuredatamodel_selectbykey = "SELECT UserOrganisationStructureKey, ADUserKey, OrganisationStructureKey, GenericKey, GenericKeyTypeKey, GeneralStatusKey FROM [2am].[dbo].[UserOrganisationStructure] WHERE UserOrganisationStructureKey = @PrimaryKey";
        public const string userorganisationstructuredatamodel_delete = "DELETE FROM [2am].[dbo].[UserOrganisationStructure] WHERE UserOrganisationStructureKey = @PrimaryKey";
        public const string userorganisationstructuredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[UserOrganisationStructure] WHERE";
        public const string userorganisationstructuredatamodel_insert = "INSERT INTO [2am].[dbo].[UserOrganisationStructure] (ADUserKey, OrganisationStructureKey, GenericKey, GenericKeyTypeKey, GeneralStatusKey) VALUES(@ADUserKey, @OrganisationStructureKey, @GenericKey, @GenericKeyTypeKey, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string userorganisationstructuredatamodel_update = "UPDATE [2am].[dbo].[UserOrganisationStructure] SET ADUserKey = @ADUserKey, OrganisationStructureKey = @OrganisationStructureKey, GenericKey = @GenericKey, GenericKeyTypeKey = @GenericKeyTypeKey, GeneralStatusKey = @GeneralStatusKey WHERE UserOrganisationStructureKey = @UserOrganisationStructureKey";



        public const string batchtypedatamodel_selectwhere = "SELECT BatchTypeKey, CalculationFormula, Description, ChangeDate, ChangeUser FROM [2am].[dbo].[BatchType] WHERE";
        public const string batchtypedatamodel_selectbykey = "SELECT BatchTypeKey, CalculationFormula, Description, ChangeDate, ChangeUser FROM [2am].[dbo].[BatchType] WHERE BatchTypeKey = @PrimaryKey";
        public const string batchtypedatamodel_delete = "DELETE FROM [2am].[dbo].[BatchType] WHERE BatchTypeKey = @PrimaryKey";
        public const string batchtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchType] WHERE";
        public const string batchtypedatamodel_insert = "INSERT INTO [2am].[dbo].[BatchType] (CalculationFormula, Description, ChangeDate, ChangeUser) VALUES(@CalculationFormula, @Description, @ChangeDate, @ChangeUser); select cast(scope_identity() as int)";
        public const string batchtypedatamodel_update = "UPDATE [2am].[dbo].[BatchType] SET CalculationFormula = @CalculationFormula, Description = @Description, ChangeDate = @ChangeDate, ChangeUser = @ChangeUser WHERE BatchTypeKey = @BatchTypeKey";



        public const string disbursementfinancialtransactiondatamodel_selectwhere = "SELECT DisbursementFinancialTransactionKey, DisbursementKey, FinancialTransactionKey FROM [2am].[dbo].[DisbursementFinancialTransaction] WHERE";
        public const string disbursementfinancialtransactiondatamodel_selectbykey = "SELECT DisbursementFinancialTransactionKey, DisbursementKey, FinancialTransactionKey FROM [2am].[dbo].[DisbursementFinancialTransaction] WHERE DisbursementFinancialTransactionKey = @PrimaryKey";
        public const string disbursementfinancialtransactiondatamodel_delete = "DELETE FROM [2am].[dbo].[DisbursementFinancialTransaction] WHERE DisbursementFinancialTransactionKey = @PrimaryKey";
        public const string disbursementfinancialtransactiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisbursementFinancialTransaction] WHERE";
        public const string disbursementfinancialtransactiondatamodel_insert = "INSERT INTO [2am].[dbo].[DisbursementFinancialTransaction] (DisbursementKey, FinancialTransactionKey) VALUES(@DisbursementKey, @FinancialTransactionKey); select cast(scope_identity() as int)";
        public const string disbursementfinancialtransactiondatamodel_update = "UPDATE [2am].[dbo].[DisbursementFinancialTransaction] SET DisbursementKey = @DisbursementKey, FinancialTransactionKey = @FinancialTransactionKey WHERE DisbursementFinancialTransactionKey = @DisbursementFinancialTransactionKey";



        public const string interestrecalc1augdatamodel_selectwhere = "SELECT FinancialServiceKey, MayInterest, JuneInterest, LBMay, LBJune, Junediff FROM [2am].[dbo].[InterestRecalc1Aug] WHERE";
        public const string interestrecalc1augdatamodel_selectbykey = "SELECT FinancialServiceKey, MayInterest, JuneInterest, LBMay, LBJune, Junediff FROM [2am].[dbo].[InterestRecalc1Aug] WHERE  = @PrimaryKey";
        public const string interestrecalc1augdatamodel_delete = "DELETE FROM [2am].[dbo].[InterestRecalc1Aug] WHERE  = @PrimaryKey";
        public const string interestrecalc1augdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[InterestRecalc1Aug] WHERE";
        public const string interestrecalc1augdatamodel_insert = "INSERT INTO [2am].[dbo].[InterestRecalc1Aug] (FinancialServiceKey, MayInterest, JuneInterest, LBMay, LBJune, Junediff) VALUES(@FinancialServiceKey, @MayInterest, @JuneInterest, @LBMay, @LBJune, @Junediff); ";
        public const string interestrecalc1augdatamodel_update = "UPDATE [2am].[dbo].[InterestRecalc1Aug] SET FinancialServiceKey = @FinancialServiceKey, MayInterest = @MayInterest, JuneInterest = @JuneInterest, LBMay = @LBMay, LBJune = @LBJune, Junediff = @Junediff WHERE  = @";



        public const string conditiondatamodel_selectwhere = "SELECT ConditionKey, ConditionTypeKey, ConditionPhrase, TokenDescriptions, TranslatableItemKey, ConditionName FROM [2am].[dbo].[Condition] WHERE";
        public const string conditiondatamodel_selectbykey = "SELECT ConditionKey, ConditionTypeKey, ConditionPhrase, TokenDescriptions, TranslatableItemKey, ConditionName FROM [2am].[dbo].[Condition] WHERE ConditionKey = @PrimaryKey";
        public const string conditiondatamodel_delete = "DELETE FROM [2am].[dbo].[Condition] WHERE ConditionKey = @PrimaryKey";
        public const string conditiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Condition] WHERE";
        public const string conditiondatamodel_insert = "INSERT INTO [2am].[dbo].[Condition] (ConditionTypeKey, ConditionPhrase, TokenDescriptions, TranslatableItemKey, ConditionName) VALUES(@ConditionTypeKey, @ConditionPhrase, @TokenDescriptions, @TranslatableItemKey, @ConditionName); select cast(scope_identity() as int)";
        public const string conditiondatamodel_update = "UPDATE [2am].[dbo].[Condition] SET ConditionTypeKey = @ConditionTypeKey, ConditionPhrase = @ConditionPhrase, TokenDescriptions = @TokenDescriptions, TranslatableItemKey = @TranslatableItemKey, ConditionName = @ConditionName WHERE ConditionKey = @ConditionKey";



        public const string propertytitledeeddatamodel_selectwhere = "SELECT PropertyTitleDeedKey, PropertyKey, TitleDeedNumber, DeedsOfficeKey FROM [2am].[dbo].[PropertyTitleDeed] WHERE";
        public const string propertytitledeeddatamodel_selectbykey = "SELECT PropertyTitleDeedKey, PropertyKey, TitleDeedNumber, DeedsOfficeKey FROM [2am].[dbo].[PropertyTitleDeed] WHERE PropertyTitleDeedKey = @PrimaryKey";
        public const string propertytitledeeddatamodel_delete = "DELETE FROM [2am].[dbo].[PropertyTitleDeed] WHERE PropertyTitleDeedKey = @PrimaryKey";
        public const string propertytitledeeddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PropertyTitleDeed] WHERE";
        public const string propertytitledeeddatamodel_insert = "INSERT INTO [2am].[dbo].[PropertyTitleDeed] (PropertyKey, TitleDeedNumber, DeedsOfficeKey) VALUES(@PropertyKey, @TitleDeedNumber, @DeedsOfficeKey); select cast(scope_identity() as int)";
        public const string propertytitledeeddatamodel_update = "UPDATE [2am].[dbo].[PropertyTitleDeed] SET PropertyKey = @PropertyKey, TitleDeedNumber = @TitleDeedNumber, DeedsOfficeKey = @DeedsOfficeKey WHERE PropertyTitleDeedKey = @PropertyTitleDeedKey";



        public const string auditmailingaddressdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MailingAddressAccountKey, AccountKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey FROM [2am].[dbo].[AuditMailingAddress] WHERE";
        public const string auditmailingaddressdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MailingAddressAccountKey, AccountKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey FROM [2am].[dbo].[AuditMailingAddress] WHERE AuditNumber = @PrimaryKey";
        public const string auditmailingaddressdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditMailingAddress] WHERE AuditNumber = @PrimaryKey";
        public const string auditmailingaddressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditMailingAddress] WHERE";
        public const string auditmailingaddressdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditMailingAddress] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MailingAddressAccountKey, AccountKey, AddressKey, OnlineStatement, OnlineStatementFormatKey, LanguageKey, LegalEntityKey, CorrespondenceMediumKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @MailingAddressAccountKey, @AccountKey, @AddressKey, @OnlineStatement, @OnlineStatementFormatKey, @LanguageKey, @LegalEntityKey, @CorrespondenceMediumKey); select cast(scope_identity() as int)";
        public const string auditmailingaddressdatamodel_update = "UPDATE [2am].[dbo].[AuditMailingAddress] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, MailingAddressAccountKey = @MailingAddressAccountKey, AccountKey = @AccountKey, AddressKey = @AddressKey, OnlineStatement = @OnlineStatement, OnlineStatementFormatKey = @OnlineStatementFormatKey, LanguageKey = @LanguageKey, LegalEntityKey = @LegalEntityKey, CorrespondenceMediumKey = @CorrespondenceMediumKey WHERE AuditNumber = @AuditNumber";



        public const string subsidyproviderdatamodel_selectwhere = "SELECT SubsidyProviderKey, SubsidyProviderTypeKey, PersalOrganisationCode, ContactPerson, UserID, ChangeDate, LegalEntityKey, GEPFAffiliate FROM [2am].[dbo].[SubsidyProvider] WHERE";
        public const string subsidyproviderdatamodel_selectbykey = "SELECT SubsidyProviderKey, SubsidyProviderTypeKey, PersalOrganisationCode, ContactPerson, UserID, ChangeDate, LegalEntityKey, GEPFAffiliate FROM [2am].[dbo].[SubsidyProvider] WHERE SubsidyProviderKey = @PrimaryKey";
        public const string subsidyproviderdatamodel_delete = "DELETE FROM [2am].[dbo].[SubsidyProvider] WHERE SubsidyProviderKey = @PrimaryKey";
        public const string subsidyproviderdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[SubsidyProvider] WHERE";
        public const string subsidyproviderdatamodel_insert = "INSERT INTO [2am].[dbo].[SubsidyProvider] (SubsidyProviderTypeKey, PersalOrganisationCode, ContactPerson, UserID, ChangeDate, LegalEntityKey, GEPFAffiliate) VALUES(@SubsidyProviderTypeKey, @PersalOrganisationCode, @ContactPerson, @UserID, @ChangeDate, @LegalEntityKey, @GEPFAffiliate); select cast(scope_identity() as int)";
        public const string subsidyproviderdatamodel_update = "UPDATE [2am].[dbo].[SubsidyProvider] SET SubsidyProviderTypeKey = @SubsidyProviderTypeKey, PersalOrganisationCode = @PersalOrganisationCode, ContactPerson = @ContactPerson, UserID = @UserID, ChangeDate = @ChangeDate, LegalEntityKey = @LegalEntityKey, GEPFAffiliate = @GEPFAffiliate WHERE SubsidyProviderKey = @SubsidyProviderKey";



        public const string batchamountsdatamodel_selectwhere = "SELECT DateToday, FinancialServiceKey, InstallmentAmount, FixedDebitOrderAmount, HOC, ProRataHOC, Regent, Life, Other, CurrentBalance, UnderCancellation, LoanOpendate, BadBankDetail FROM [2am].[dbo].[BatchAmounts] WHERE";
        public const string batchamountsdatamodel_selectbykey = "SELECT DateToday, FinancialServiceKey, InstallmentAmount, FixedDebitOrderAmount, HOC, ProRataHOC, Regent, Life, Other, CurrentBalance, UnderCancellation, LoanOpendate, BadBankDetail FROM [2am].[dbo].[BatchAmounts] WHERE  = @PrimaryKey";
        public const string batchamountsdatamodel_delete = "DELETE FROM [2am].[dbo].[BatchAmounts] WHERE  = @PrimaryKey";
        public const string batchamountsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchAmounts] WHERE";
        public const string batchamountsdatamodel_insert = "INSERT INTO [2am].[dbo].[BatchAmounts] (DateToday, FinancialServiceKey, InstallmentAmount, FixedDebitOrderAmount, HOC, ProRataHOC, Regent, Life, Other, CurrentBalance, UnderCancellation, LoanOpendate, BadBankDetail) VALUES(@DateToday, @FinancialServiceKey, @InstallmentAmount, @FixedDebitOrderAmount, @HOC, @ProRataHOC, @Regent, @Life, @Other, @CurrentBalance, @UnderCancellation, @LoanOpendate, @BadBankDetail); ";
        public const string batchamountsdatamodel_update = "UPDATE [2am].[dbo].[BatchAmounts] SET DateToday = @DateToday, FinancialServiceKey = @FinancialServiceKey, InstallmentAmount = @InstallmentAmount, FixedDebitOrderAmount = @FixedDebitOrderAmount, HOC = @HOC, ProRataHOC = @ProRataHOC, Regent = @Regent, Life = @Life, Other = @Other, CurrentBalance = @CurrentBalance, UnderCancellation = @UnderCancellation, LoanOpendate = @LoanOpendate, BadBankDetail = @BadBankDetail WHERE  = @";



        public const string thirdpartytypedatamodel_selectwhere = "SELECT ThirdPartyTypeKey, Description FROM [2am].[dbo].[ThirdPartyType] WHERE";
        public const string thirdpartytypedatamodel_selectbykey = "SELECT ThirdPartyTypeKey, Description FROM [2am].[dbo].[ThirdPartyType] WHERE ThirdPartyTypeKey = @PrimaryKey";
        public const string thirdpartytypedatamodel_delete = "DELETE FROM [2am].[dbo].[ThirdPartyType] WHERE ThirdPartyTypeKey = @PrimaryKey";
        public const string thirdpartytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ThirdPartyType] WHERE";
        public const string thirdpartytypedatamodel_insert = "INSERT INTO [2am].[dbo].[ThirdPartyType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string thirdpartytypedatamodel_update = "UPDATE [2am].[dbo].[ThirdPartyType] SET Description = @Description WHERE ThirdPartyTypeKey = @ThirdPartyTypeKey";



        public const string residencestatusdatamodel_selectwhere = "SELECT ResidenceStatusKey, Description FROM [2am].[dbo].[ResidenceStatus] WHERE";
        public const string residencestatusdatamodel_selectbykey = "SELECT ResidenceStatusKey, Description FROM [2am].[dbo].[ResidenceStatus] WHERE ResidenceStatusKey = @PrimaryKey";
        public const string residencestatusdatamodel_delete = "DELETE FROM [2am].[dbo].[ResidenceStatus] WHERE ResidenceStatusKey = @PrimaryKey";
        public const string residencestatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ResidenceStatus] WHERE";
        public const string residencestatusdatamodel_insert = "INSERT INTO [2am].[dbo].[ResidenceStatus] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string residencestatusdatamodel_update = "UPDATE [2am].[dbo].[ResidenceStatus] SET Description = @Description WHERE ResidenceStatusKey = @ResidenceStatusKey";



        public const string offersubsidydatamodel_selectwhere = "SELECT OfferSubsidyKey, OfferKey, SubsidyKey FROM [2am].[dbo].[OfferSubsidy] WHERE";
        public const string offersubsidydatamodel_selectbykey = "SELECT OfferSubsidyKey, OfferKey, SubsidyKey FROM [2am].[dbo].[OfferSubsidy] WHERE OfferSubsidyKey = @PrimaryKey";
        public const string offersubsidydatamodel_delete = "DELETE FROM [2am].[dbo].[OfferSubsidy] WHERE OfferSubsidyKey = @PrimaryKey";
        public const string offersubsidydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferSubsidy] WHERE";
        public const string offersubsidydatamodel_insert = "INSERT INTO [2am].[dbo].[OfferSubsidy] (OfferKey, SubsidyKey) VALUES(@OfferKey, @SubsidyKey); select cast(scope_identity() as int)";
        public const string offersubsidydatamodel_update = "UPDATE [2am].[dbo].[OfferSubsidy] SET OfferKey = @OfferKey, SubsidyKey = @SubsidyKey WHERE OfferSubsidyKey = @OfferSubsidyKey";



        public const string quickcashpaymenttypedatamodel_selectwhere = "SELECT QuickCashPaymentTypeKey, Description FROM [2am].[dbo].[QuickCashPaymentType] WHERE";
        public const string quickcashpaymenttypedatamodel_selectbykey = "SELECT QuickCashPaymentTypeKey, Description FROM [2am].[dbo].[QuickCashPaymentType] WHERE QuickCashPaymentTypeKey = @PrimaryKey";
        public const string quickcashpaymenttypedatamodel_delete = "DELETE FROM [2am].[dbo].[QuickCashPaymentType] WHERE QuickCashPaymentTypeKey = @PrimaryKey";
        public const string quickcashpaymenttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[QuickCashPaymentType] WHERE";
        public const string quickcashpaymenttypedatamodel_insert = "INSERT INTO [2am].[dbo].[QuickCashPaymentType] (QuickCashPaymentTypeKey, Description) VALUES(@QuickCashPaymentTypeKey, @Description); ";
        public const string quickcashpaymenttypedatamodel_update = "UPDATE [2am].[dbo].[QuickCashPaymentType] SET QuickCashPaymentTypeKey = @QuickCashPaymentTypeKey, Description = @Description WHERE QuickCashPaymentTypeKey = @QuickCashPaymentTypeKey";



        public const string comcorpimagingrequestdatamodel_selectwhere = "SELECT ComcorpImagingRequestKey, OfferKey, ImagingReference, ExpectedDocuments, ReceivedDocuments FROM [2am].[dbo].[ComcorpImagingRequest] WHERE";
        public const string comcorpimagingrequestdatamodel_selectbykey = "SELECT ComcorpImagingRequestKey, OfferKey, ImagingReference, ExpectedDocuments, ReceivedDocuments FROM [2am].[dbo].[ComcorpImagingRequest] WHERE ComcorpImagingRequestKey = @PrimaryKey";
        public const string comcorpimagingrequestdatamodel_delete = "DELETE FROM [2am].[dbo].[ComcorpImagingRequest] WHERE ComcorpImagingRequestKey = @PrimaryKey";
        public const string comcorpimagingrequestdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ComcorpImagingRequest] WHERE";
        public const string comcorpimagingrequestdatamodel_insert = "INSERT INTO [2am].[dbo].[ComcorpImagingRequest] (OfferKey, ImagingReference, ExpectedDocuments, ReceivedDocuments) VALUES(@OfferKey, @ImagingReference, @ExpectedDocuments, @ReceivedDocuments); select cast(scope_identity() as int)";
        public const string comcorpimagingrequestdatamodel_update = "UPDATE [2am].[dbo].[ComcorpImagingRequest] SET OfferKey = @OfferKey, ImagingReference = @ImagingReference, ExpectedDocuments = @ExpectedDocuments, ReceivedDocuments = @ReceivedDocuments WHERE ComcorpImagingRequestKey = @ComcorpImagingRequestKey";



        public const string hocstatusdatamodel_selectwhere = "SELECT HOCStatusKey, Description FROM [2am].[dbo].[HOCStatus] WHERE";
        public const string hocstatusdatamodel_selectbykey = "SELECT HOCStatusKey, Description FROM [2am].[dbo].[HOCStatus] WHERE HOCStatusKey = @PrimaryKey";
        public const string hocstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[HOCStatus] WHERE HOCStatusKey = @PrimaryKey";
        public const string hocstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCStatus] WHERE";
        public const string hocstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[HOCStatus] (HOCStatusKey, Description) VALUES(@HOCStatusKey, @Description); ";
        public const string hocstatusdatamodel_update = "UPDATE [2am].[dbo].[HOCStatus] SET HOCStatusKey = @HOCStatusKey, Description = @Description WHERE HOCStatusKey = @HOCStatusKey";



        public const string foreclosureattorneydetailtypemappingdatamodel_selectwhere = "SELECT ForeclosureAttorneyDetailTypeMappingKey, LegalEntityKey, DetailTypeKey, GeneralStatusKey FROM [2am].[dbo].[ForeclosureAttorneyDetailTypeMapping] WHERE";
        public const string foreclosureattorneydetailtypemappingdatamodel_selectbykey = "SELECT ForeclosureAttorneyDetailTypeMappingKey, LegalEntityKey, DetailTypeKey, GeneralStatusKey FROM [2am].[dbo].[ForeclosureAttorneyDetailTypeMapping] WHERE ForeclosureAttorneyDetailTypeMappingKey = @PrimaryKey";
        public const string foreclosureattorneydetailtypemappingdatamodel_delete = "DELETE FROM [2am].[dbo].[ForeclosureAttorneyDetailTypeMapping] WHERE ForeclosureAttorneyDetailTypeMappingKey = @PrimaryKey";
        public const string foreclosureattorneydetailtypemappingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ForeclosureAttorneyDetailTypeMapping] WHERE";
        public const string foreclosureattorneydetailtypemappingdatamodel_insert = "INSERT INTO [2am].[dbo].[ForeclosureAttorneyDetailTypeMapping] (LegalEntityKey, DetailTypeKey, GeneralStatusKey) VALUES(@LegalEntityKey, @DetailTypeKey, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string foreclosureattorneydetailtypemappingdatamodel_update = "UPDATE [2am].[dbo].[ForeclosureAttorneyDetailTypeMapping] SET LegalEntityKey = @LegalEntityKey, DetailTypeKey = @DetailTypeKey, GeneralStatusKey = @GeneralStatusKey WHERE ForeclosureAttorneyDetailTypeMappingKey = @ForeclosureAttorneyDetailTypeMappingKey";



        public const string importofferdatamodel_selectwhere = "SELECT OfferKey, FileKey, ImportStatusKey, OfferAmount, OfferStartDate, OfferEndDate, MortgageLoanPurposeKey, ApplicantTypeKey, NumberApplicants, HomePurchaseDate, BondRegistrationDate, CurrentBondValue, DeedsOfficeDate, BondFinancialInstitution, ExistingLoan, PurchasePrice, Reference, ErrorMsg, ImportID FROM [2am].[dbo].[ImportOffer] WHERE";
        public const string importofferdatamodel_selectbykey = "SELECT OfferKey, FileKey, ImportStatusKey, OfferAmount, OfferStartDate, OfferEndDate, MortgageLoanPurposeKey, ApplicantTypeKey, NumberApplicants, HomePurchaseDate, BondRegistrationDate, CurrentBondValue, DeedsOfficeDate, BondFinancialInstitution, ExistingLoan, PurchasePrice, Reference, ErrorMsg, ImportID FROM [2am].[dbo].[ImportOffer] WHERE OfferKey = @PrimaryKey";
        public const string importofferdatamodel_delete = "DELETE FROM [2am].[dbo].[ImportOffer] WHERE OfferKey = @PrimaryKey";
        public const string importofferdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportOffer] WHERE";
        public const string importofferdatamodel_insert = "INSERT INTO [2am].[dbo].[ImportOffer] (FileKey, ImportStatusKey, OfferAmount, OfferStartDate, OfferEndDate, MortgageLoanPurposeKey, ApplicantTypeKey, NumberApplicants, HomePurchaseDate, BondRegistrationDate, CurrentBondValue, DeedsOfficeDate, BondFinancialInstitution, ExistingLoan, PurchasePrice, Reference, ErrorMsg, ImportID) VALUES(@FileKey, @ImportStatusKey, @OfferAmount, @OfferStartDate, @OfferEndDate, @MortgageLoanPurposeKey, @ApplicantTypeKey, @NumberApplicants, @HomePurchaseDate, @BondRegistrationDate, @CurrentBondValue, @DeedsOfficeDate, @BondFinancialInstitution, @ExistingLoan, @PurchasePrice, @Reference, @ErrorMsg, @ImportID); select cast(scope_identity() as int)";
        public const string importofferdatamodel_update = "UPDATE [2am].[dbo].[ImportOffer] SET FileKey = @FileKey, ImportStatusKey = @ImportStatusKey, OfferAmount = @OfferAmount, OfferStartDate = @OfferStartDate, OfferEndDate = @OfferEndDate, MortgageLoanPurposeKey = @MortgageLoanPurposeKey, ApplicantTypeKey = @ApplicantTypeKey, NumberApplicants = @NumberApplicants, HomePurchaseDate = @HomePurchaseDate, BondRegistrationDate = @BondRegistrationDate, CurrentBondValue = @CurrentBondValue, DeedsOfficeDate = @DeedsOfficeDate, BondFinancialInstitution = @BondFinancialInstitution, ExistingLoan = @ExistingLoan, PurchasePrice = @PurchasePrice, Reference = @Reference, ErrorMsg = @ErrorMsg, ImportID = @ImportID WHERE OfferKey = @OfferKey";



        public const string offerdocumentdatamodel_selectwhere = "SELECT OfferDocumentKey, OfferKey, DocumentTypeKey, DocumentReceivedDate, DocumentReceivedBy, Description, GenericKey FROM [2am].[dbo].[OfferDocument] WHERE";
        public const string offerdocumentdatamodel_selectbykey = "SELECT OfferDocumentKey, OfferKey, DocumentTypeKey, DocumentReceivedDate, DocumentReceivedBy, Description, GenericKey FROM [2am].[dbo].[OfferDocument] WHERE OfferDocumentKey = @PrimaryKey";
        public const string offerdocumentdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDocument] WHERE OfferDocumentKey = @PrimaryKey";
        public const string offerdocumentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDocument] WHERE";
        public const string offerdocumentdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDocument] (OfferKey, DocumentTypeKey, DocumentReceivedDate, DocumentReceivedBy, Description, GenericKey) VALUES(@OfferKey, @DocumentTypeKey, @DocumentReceivedDate, @DocumentReceivedBy, @Description, @GenericKey); select cast(scope_identity() as int)";
        public const string offerdocumentdatamodel_update = "UPDATE [2am].[dbo].[OfferDocument] SET OfferKey = @OfferKey, DocumentTypeKey = @DocumentTypeKey, DocumentReceivedDate = @DocumentReceivedDate, DocumentReceivedBy = @DocumentReceivedBy, Description = @Description, GenericKey = @GenericKey WHERE OfferDocumentKey = @OfferDocumentKey";



        public const string disabilityclaimstatusdatamodel_selectwhere = "SELECT DisabilityClaimStatusKey, Description FROM [2am].[dbo].[DisabilityClaimStatus] WHERE";
        public const string disabilityclaimstatusdatamodel_selectbykey = "SELECT DisabilityClaimStatusKey, Description FROM [2am].[dbo].[DisabilityClaimStatus] WHERE DisabilityClaimStatusKey = @PrimaryKey";
        public const string disabilityclaimstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[DisabilityClaimStatus] WHERE DisabilityClaimStatusKey = @PrimaryKey";
        public const string disabilityclaimstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisabilityClaimStatus] WHERE";
        public const string disabilityclaimstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[DisabilityClaimStatus] (DisabilityClaimStatusKey, Description) VALUES(@DisabilityClaimStatusKey, @Description); ";
        public const string disabilityclaimstatusdatamodel_update = "UPDATE [2am].[dbo].[DisabilityClaimStatus] SET DisabilityClaimStatusKey = @DisabilityClaimStatusKey, Description = @Description WHERE DisabilityClaimStatusKey = @DisabilityClaimStatusKey";



        public const string lifecommissionconfigurationdatamodel_selectwhere = "SELECT CommissionConfigurationKey, MaxCalls, MinCalls, MaxHits, MinHits, TargetCalls, TargetHits, FloorPrice, CommRate, PerScaleY1, PerScaleY2, PerScaleY3, ChangeDate, ChangeUser, ADUserKey FROM [2am].[dbo].[LifeCommissionConfiguration] WHERE";
        public const string lifecommissionconfigurationdatamodel_selectbykey = "SELECT CommissionConfigurationKey, MaxCalls, MinCalls, MaxHits, MinHits, TargetCalls, TargetHits, FloorPrice, CommRate, PerScaleY1, PerScaleY2, PerScaleY3, ChangeDate, ChangeUser, ADUserKey FROM [2am].[dbo].[LifeCommissionConfiguration] WHERE CommissionConfigurationKey = @PrimaryKey";
        public const string lifecommissionconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[LifeCommissionConfiguration] WHERE CommissionConfigurationKey = @PrimaryKey";
        public const string lifecommissionconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifeCommissionConfiguration] WHERE";
        public const string lifecommissionconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[LifeCommissionConfiguration] (MaxCalls, MinCalls, MaxHits, MinHits, TargetCalls, TargetHits, FloorPrice, CommRate, PerScaleY1, PerScaleY2, PerScaleY3, ChangeDate, ChangeUser, ADUserKey) VALUES(@MaxCalls, @MinCalls, @MaxHits, @MinHits, @TargetCalls, @TargetHits, @FloorPrice, @CommRate, @PerScaleY1, @PerScaleY2, @PerScaleY3, @ChangeDate, @ChangeUser, @ADUserKey); select cast(scope_identity() as int)";
        public const string lifecommissionconfigurationdatamodel_update = "UPDATE [2am].[dbo].[LifeCommissionConfiguration] SET MaxCalls = @MaxCalls, MinCalls = @MinCalls, MaxHits = @MaxHits, MinHits = @MinHits, TargetCalls = @TargetCalls, TargetHits = @TargetHits, FloorPrice = @FloorPrice, CommRate = @CommRate, PerScaleY1 = @PerScaleY1, PerScaleY2 = @PerScaleY2, PerScaleY3 = @PerScaleY3, ChangeDate = @ChangeDate, ChangeUser = @ChangeUser, ADUserKey = @ADUserKey WHERE CommissionConfigurationKey = @CommissionConfigurationKey";



        public const string margindatamodel_selectwhere = "SELECT MarginKey, Value, Description FROM [2am].[dbo].[Margin] WHERE";
        public const string margindatamodel_selectbykey = "SELECT MarginKey, Value, Description FROM [2am].[dbo].[Margin] WHERE MarginKey = @PrimaryKey";
        public const string margindatamodel_delete = "DELETE FROM [2am].[dbo].[Margin] WHERE MarginKey = @PrimaryKey";
        public const string margindatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Margin] WHERE";
        public const string margindatamodel_insert = "INSERT INTO [2am].[dbo].[Margin] (Value, Description) VALUES(@Value, @Description); select cast(scope_identity() as int)";
        public const string margindatamodel_update = "UPDATE [2am].[dbo].[Margin] SET Value = @Value, Description = @Description WHERE MarginKey = @MarginKey";



        public const string provincedatamodel_selectwhere = "SELECT ProvinceKey, Description, CountryKey FROM [2am].[dbo].[Province] WHERE";
        public const string provincedatamodel_selectbykey = "SELECT ProvinceKey, Description, CountryKey FROM [2am].[dbo].[Province] WHERE ProvinceKey = @PrimaryKey";
        public const string provincedatamodel_delete = "DELETE FROM [2am].[dbo].[Province] WHERE ProvinceKey = @PrimaryKey";
        public const string provincedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Province] WHERE";
        public const string provincedatamodel_insert = "INSERT INTO [2am].[dbo].[Province] (Description, CountryKey) VALUES(@Description, @CountryKey); select cast(scope_identity() as int)";
        public const string provincedatamodel_update = "UPDATE [2am].[dbo].[Province] SET Description = @Description, CountryKey = @CountryKey WHERE ProvinceKey = @ProvinceKey";



        public const string bankaccount_backupdatamodel_selectwhere = "SELECT BankAccountKey, ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate FROM [2am].[dbo].[BankAccount_Backup] WHERE";
        public const string bankaccount_backupdatamodel_selectbykey = "SELECT BankAccountKey, ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate FROM [2am].[dbo].[BankAccount_Backup] WHERE  = @PrimaryKey";
        public const string bankaccount_backupdatamodel_delete = "DELETE FROM [2am].[dbo].[BankAccount_Backup] WHERE  = @PrimaryKey";
        public const string bankaccount_backupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BankAccount_Backup] WHERE";
        public const string bankaccount_backupdatamodel_insert = "INSERT INTO [2am].[dbo].[BankAccount_Backup] (BankAccountKey, ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate) VALUES(@BankAccountKey, @ACBBranchCode, @AccountNumber, @ACBTypeNumber, @AccountName, @UserID, @ChangeDate); ";
        public const string bankaccount_backupdatamodel_update = "UPDATE [2am].[dbo].[BankAccount_Backup] SET BankAccountKey = @BankAccountKey, ACBBranchCode = @ACBBranchCode, AccountNumber = @AccountNumber, ACBTypeNumber = @ACBTypeNumber, AccountName = @AccountName, UserID = @UserID, ChangeDate = @ChangeDate WHERE  = @";



        public const string bulkbatchlogdatamodel_selectwhere = "SELECT BulkBatchLogKey, BulkBatchKey, Description, MessageTypeKey, MessageReference, MessageReferenceKey FROM [2am].[dbo].[BulkBatchLog] WHERE";
        public const string bulkbatchlogdatamodel_selectbykey = "SELECT BulkBatchLogKey, BulkBatchKey, Description, MessageTypeKey, MessageReference, MessageReferenceKey FROM [2am].[dbo].[BulkBatchLog] WHERE BulkBatchLogKey = @PrimaryKey";
        public const string bulkbatchlogdatamodel_delete = "DELETE FROM [2am].[dbo].[BulkBatchLog] WHERE BulkBatchLogKey = @PrimaryKey";
        public const string bulkbatchlogdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BulkBatchLog] WHERE";
        public const string bulkbatchlogdatamodel_insert = "INSERT INTO [2am].[dbo].[BulkBatchLog] (BulkBatchKey, Description, MessageTypeKey, MessageReference, MessageReferenceKey) VALUES(@BulkBatchKey, @Description, @MessageTypeKey, @MessageReference, @MessageReferenceKey); select cast(scope_identity() as int)";
        public const string bulkbatchlogdatamodel_update = "UPDATE [2am].[dbo].[BulkBatchLog] SET BulkBatchKey = @BulkBatchKey, Description = @Description, MessageTypeKey = @MessageTypeKey, MessageReference = @MessageReference, MessageReferenceKey = @MessageReferenceKey WHERE BulkBatchLogKey = @BulkBatchLogKey";



        public const string teamdatamodel_selectwhere = "SELECT TeamKey, Description, ChangeDate, ChangeUser FROM [2am].[dbo].[Team] WHERE";
        public const string teamdatamodel_selectbykey = "SELECT TeamKey, Description, ChangeDate, ChangeUser FROM [2am].[dbo].[Team] WHERE TeamKey = @PrimaryKey";
        public const string teamdatamodel_delete = "DELETE FROM [2am].[dbo].[Team] WHERE TeamKey = @PrimaryKey";
        public const string teamdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Team] WHERE";
        public const string teamdatamodel_insert = "INSERT INTO [2am].[dbo].[Team] (Description, ChangeDate, ChangeUser) VALUES(@Description, @ChangeDate, @ChangeUser); select cast(scope_identity() as int)";
        public const string teamdatamodel_update = "UPDATE [2am].[dbo].[Team] SET Description = @Description, ChangeDate = @ChangeDate, ChangeUser = @ChangeUser WHERE TeamKey = @TeamKey";



        public const string financialservicebankaccountdatamodel_selectwhere = "SELECT FinancialServiceBankAccountKey, FinancialServiceKey, BankAccountKey, Percentage, DebitOrderDay, GeneralStatusKey, UserID, ChangeDate, FinancialServicePaymentTypeKey, PaymentSplitTypeKey, ProviderKey, IsNaedoCompliant FROM [2am].[dbo].[FinancialServiceBankAccount] WHERE";
        public const string financialservicebankaccountdatamodel_selectbykey = "SELECT FinancialServiceBankAccountKey, FinancialServiceKey, BankAccountKey, Percentage, DebitOrderDay, GeneralStatusKey, UserID, ChangeDate, FinancialServicePaymentTypeKey, PaymentSplitTypeKey, ProviderKey, IsNaedoCompliant FROM [2am].[dbo].[FinancialServiceBankAccount] WHERE FinancialServiceBankAccountKey = @PrimaryKey";
        public const string financialservicebankaccountdatamodel_delete = "DELETE FROM [2am].[dbo].[FinancialServiceBankAccount] WHERE FinancialServiceBankAccountKey = @PrimaryKey";
        public const string financialservicebankaccountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FinancialServiceBankAccount] WHERE";
        public const string financialservicebankaccountdatamodel_insert = "INSERT INTO [2am].[dbo].[FinancialServiceBankAccount] (FinancialServiceKey, BankAccountKey, Percentage, DebitOrderDay, GeneralStatusKey, UserID, ChangeDate, FinancialServicePaymentTypeKey, PaymentSplitTypeKey, ProviderKey, IsNaedoCompliant) VALUES(@FinancialServiceKey, @BankAccountKey, @Percentage, @DebitOrderDay, @GeneralStatusKey, @UserID, @ChangeDate, @FinancialServicePaymentTypeKey, @PaymentSplitTypeKey, @ProviderKey, @IsNaedoCompliant); select cast(scope_identity() as int)";
        public const string financialservicebankaccountdatamodel_update = "UPDATE [2am].[dbo].[FinancialServiceBankAccount] SET FinancialServiceKey = @FinancialServiceKey, BankAccountKey = @BankAccountKey, Percentage = @Percentage, DebitOrderDay = @DebitOrderDay, GeneralStatusKey = @GeneralStatusKey, UserID = @UserID, ChangeDate = @ChangeDate, FinancialServicePaymentTypeKey = @FinancialServicePaymentTypeKey, PaymentSplitTypeKey = @PaymentSplitTypeKey, ProviderKey = @ProviderKey, IsNaedoCompliant = @IsNaedoCompliant WHERE FinancialServiceBankAccountKey = @FinancialServiceBankAccountKey";



        public const string recurringtransactiontypedatamodel_selectwhere = "SELECT RecurringTransactionTypeKey, Description FROM [2am].[dbo].[RecurringTransactionType] WHERE";
        public const string recurringtransactiontypedatamodel_selectbykey = "SELECT RecurringTransactionTypeKey, Description FROM [2am].[dbo].[RecurringTransactionType] WHERE RecurringTransactionTypeKey = @PrimaryKey";
        public const string recurringtransactiontypedatamodel_delete = "DELETE FROM [2am].[dbo].[RecurringTransactionType] WHERE RecurringTransactionTypeKey = @PrimaryKey";
        public const string recurringtransactiontypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RecurringTransactionType] WHERE";
        public const string recurringtransactiontypedatamodel_insert = "INSERT INTO [2am].[dbo].[RecurringTransactionType] (RecurringTransactionTypeKey, Description) VALUES(@RecurringTransactionTypeKey, @Description); ";
        public const string recurringtransactiontypedatamodel_update = "UPDATE [2am].[dbo].[RecurringTransactionType] SET RecurringTransactionTypeKey = @RecurringTransactionTypeKey, Description = @Description WHERE RecurringTransactionTypeKey = @RecurringTransactionTypeKey";



        public const string hocsubsidencedatamodel_selectwhere = "SELECT HOCSubsidenceKey, Description FROM [2am].[dbo].[HOCSubsidence] WHERE";
        public const string hocsubsidencedatamodel_selectbykey = "SELECT HOCSubsidenceKey, Description FROM [2am].[dbo].[HOCSubsidence] WHERE HOCSubsidenceKey = @PrimaryKey";
        public const string hocsubsidencedatamodel_delete = "DELETE FROM [2am].[dbo].[HOCSubsidence] WHERE HOCSubsidenceKey = @PrimaryKey";
        public const string hocsubsidencedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCSubsidence] WHERE";
        public const string hocsubsidencedatamodel_insert = "INSERT INTO [2am].[dbo].[HOCSubsidence] (HOCSubsidenceKey, Description) VALUES(@HOCSubsidenceKey, @Description); ";
        public const string hocsubsidencedatamodel_update = "UPDATE [2am].[dbo].[HOCSubsidence] SET HOCSubsidenceKey = @HOCSubsidenceKey, Description = @Description WHERE HOCSubsidenceKey = @HOCSubsidenceKey";



        public const string userorganisationstructurehistorydatamodel_selectwhere = "SELECT UserOrganisationStructureHistoryKey, UserOrganisationStructureKey, ADUserKey, OrganisationStructureKey, ChangeDate, Action, GenericKey, GenericKeyTypeKey, GeneralStatusKey FROM [2am].[dbo].[UserOrganisationStructureHistory] WHERE";
        public const string userorganisationstructurehistorydatamodel_selectbykey = "SELECT UserOrganisationStructureHistoryKey, UserOrganisationStructureKey, ADUserKey, OrganisationStructureKey, ChangeDate, Action, GenericKey, GenericKeyTypeKey, GeneralStatusKey FROM [2am].[dbo].[UserOrganisationStructureHistory] WHERE UserOrganisationStructureHistoryKey = @PrimaryKey";
        public const string userorganisationstructurehistorydatamodel_delete = "DELETE FROM [2am].[dbo].[UserOrganisationStructureHistory] WHERE UserOrganisationStructureHistoryKey = @PrimaryKey";
        public const string userorganisationstructurehistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[UserOrganisationStructureHistory] WHERE";
        public const string userorganisationstructurehistorydatamodel_insert = "INSERT INTO [2am].[dbo].[UserOrganisationStructureHistory] (UserOrganisationStructureKey, ADUserKey, OrganisationStructureKey, ChangeDate, Action, GenericKey, GenericKeyTypeKey, GeneralStatusKey) VALUES(@UserOrganisationStructureKey, @ADUserKey, @OrganisationStructureKey, @ChangeDate, @Action, @GenericKey, @GenericKeyTypeKey, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string userorganisationstructurehistorydatamodel_update = "UPDATE [2am].[dbo].[UserOrganisationStructureHistory] SET UserOrganisationStructureKey = @UserOrganisationStructureKey, ADUserKey = @ADUserKey, OrganisationStructureKey = @OrganisationStructureKey, ChangeDate = @ChangeDate, Action = @Action, GenericKey = @GenericKey, GenericKeyTypeKey = @GenericKeyTypeKey, GeneralStatusKey = @GeneralStatusKey WHERE UserOrganisationStructureHistoryKey = @UserOrganisationStructureHistoryKey";



        public const string disabilitytypedatamodel_selectwhere = "SELECT DisabilityTypeKey, Description FROM [2am].[dbo].[DisabilityType] WHERE";
        public const string disabilitytypedatamodel_selectbykey = "SELECT DisabilityTypeKey, Description FROM [2am].[dbo].[DisabilityType] WHERE DisabilityTypeKey = @PrimaryKey";
        public const string disabilitytypedatamodel_delete = "DELETE FROM [2am].[dbo].[DisabilityType] WHERE DisabilityTypeKey = @PrimaryKey";
        public const string disabilitytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisabilityType] WHERE";
        public const string disabilitytypedatamodel_insert = "INSERT INTO [2am].[dbo].[DisabilityType] (DisabilityTypeKey, Description) VALUES(@DisabilityTypeKey, @Description); ";
        public const string disabilitytypedatamodel_update = "UPDATE [2am].[dbo].[DisabilityType] SET DisabilityTypeKey = @DisabilityTypeKey, Description = @Description WHERE DisabilityTypeKey = @DisabilityTypeKey";



        public const string textstatementdatamodel_selectwhere = "SELECT TextStatementKey, TextStatementTypeKey, StatementTitle, Statement FROM [2am].[dbo].[TextStatement] WHERE";
        public const string textstatementdatamodel_selectbykey = "SELECT TextStatementKey, TextStatementTypeKey, StatementTitle, Statement FROM [2am].[dbo].[TextStatement] WHERE TextStatementKey = @PrimaryKey";
        public const string textstatementdatamodel_delete = "DELETE FROM [2am].[dbo].[TextStatement] WHERE TextStatementKey = @PrimaryKey";
        public const string textstatementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TextStatement] WHERE";
        public const string textstatementdatamodel_insert = "INSERT INTO [2am].[dbo].[TextStatement] (TextStatementTypeKey, StatementTitle, Statement) VALUES(@TextStatementTypeKey, @StatementTitle, @Statement); select cast(scope_identity() as int)";
        public const string textstatementdatamodel_update = "UPDATE [2am].[dbo].[TextStatement] SET TextStatementTypeKey = @TextStatementTypeKey, StatementTitle = @StatementTitle, Statement = @Statement WHERE TextStatementKey = @TextStatementKey";



        public const string citydatamodel_selectwhere = "SELECT CityKey, Description, ProvinceKey FROM [2am].[dbo].[City] WHERE";
        public const string citydatamodel_selectbykey = "SELECT CityKey, Description, ProvinceKey FROM [2am].[dbo].[City] WHERE CityKey = @PrimaryKey";
        public const string citydatamodel_delete = "DELETE FROM [2am].[dbo].[City] WHERE CityKey = @PrimaryKey";
        public const string citydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[City] WHERE";
        public const string citydatamodel_insert = "INSERT INTO [2am].[dbo].[City] (Description, ProvinceKey) VALUES(@Description, @ProvinceKey); select cast(scope_identity() as int)";
        public const string citydatamodel_update = "UPDATE [2am].[dbo].[City] SET Description = @Description, ProvinceKey = @ProvinceKey WHERE CityKey = @CityKey";



        public const string comcorprequestdatamodel_selectwhere = "SELECT ComcorpRequestKey, XmlRequest, RequestDate, XmlResponse, ResponseDate FROM [2am].[dbo].[ComcorpRequest] WHERE";
        public const string comcorprequestdatamodel_selectbykey = "SELECT ComcorpRequestKey, XmlRequest, RequestDate, XmlResponse, ResponseDate FROM [2am].[dbo].[ComcorpRequest] WHERE ComcorpRequestKey = @PrimaryKey";
        public const string comcorprequestdatamodel_delete = "DELETE FROM [2am].[dbo].[ComcorpRequest] WHERE ComcorpRequestKey = @PrimaryKey";
        public const string comcorprequestdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ComcorpRequest] WHERE";
        public const string comcorprequestdatamodel_insert = "INSERT INTO [2am].[dbo].[ComcorpRequest] (XmlRequest, RequestDate, XmlResponse, ResponseDate) VALUES(@XmlRequest, @RequestDate, @XmlResponse, @ResponseDate); select cast(scope_identity() as int)";
        public const string comcorprequestdatamodel_update = "UPDATE [2am].[dbo].[ComcorpRequest] SET XmlRequest = @XmlRequest, RequestDate = @RequestDate, XmlResponse = @XmlResponse, ResponseDate = @ResponseDate WHERE ComcorpRequestKey = @ComcorpRequestKey";



        public const string importlegalentitydatamodel_selectwhere = "SELECT LegalEntityKey, OfferKey, MaritalStatusKey, GenderKey, CitizenTypeKey, SalutationKey, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, ImportID FROM [2am].[dbo].[ImportLegalEntity] WHERE";
        public const string importlegalentitydatamodel_selectbykey = "SELECT LegalEntityKey, OfferKey, MaritalStatusKey, GenderKey, CitizenTypeKey, SalutationKey, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, ImportID FROM [2am].[dbo].[ImportLegalEntity] WHERE LegalEntityKey = @PrimaryKey";
        public const string importlegalentitydatamodel_delete = "DELETE FROM [2am].[dbo].[ImportLegalEntity] WHERE LegalEntityKey = @PrimaryKey";
        public const string importlegalentitydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportLegalEntity] WHERE";
        public const string importlegalentitydatamodel_insert = "INSERT INTO [2am].[dbo].[ImportLegalEntity] (OfferKey, MaritalStatusKey, GenderKey, CitizenTypeKey, SalutationKey, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, ImportID) VALUES(@OfferKey, @MaritalStatusKey, @GenderKey, @CitizenTypeKey, @SalutationKey, @FirstNames, @Initials, @Surname, @PreferredName, @IDNumber, @PassportNumber, @TaxNumber, @HomePhoneCode, @HomePhoneNumber, @WorkPhoneCode, @WorkPhoneNumber, @CellPhoneNumber, @EmailAddress, @FaxCode, @FaxNumber, @ImportID); select cast(scope_identity() as int)";
        public const string importlegalentitydatamodel_update = "UPDATE [2am].[dbo].[ImportLegalEntity] SET OfferKey = @OfferKey, MaritalStatusKey = @MaritalStatusKey, GenderKey = @GenderKey, CitizenTypeKey = @CitizenTypeKey, SalutationKey = @SalutationKey, FirstNames = @FirstNames, Initials = @Initials, Surname = @Surname, PreferredName = @PreferredName, IDNumber = @IDNumber, PassportNumber = @PassportNumber, TaxNumber = @TaxNumber, HomePhoneCode = @HomePhoneCode, HomePhoneNumber = @HomePhoneNumber, WorkPhoneCode = @WorkPhoneCode, WorkPhoneNumber = @WorkPhoneNumber, CellPhoneNumber = @CellPhoneNumber, EmailAddress = @EmailAddress, FaxCode = @FaxCode, FaxNumber = @FaxNumber, ImportID = @ImportID WHERE LegalEntityKey = @LegalEntityKey";



        public const string rateconfigurationdatamodel_selectwhere = "SELECT RateConfigurationKey, MarketRateKey, MarginKey FROM [2am].[dbo].[RateConfiguration] WHERE";
        public const string rateconfigurationdatamodel_selectbykey = "SELECT RateConfigurationKey, MarketRateKey, MarginKey FROM [2am].[dbo].[RateConfiguration] WHERE RateConfigurationKey = @PrimaryKey";
        public const string rateconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[RateConfiguration] WHERE RateConfigurationKey = @PrimaryKey";
        public const string rateconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RateConfiguration] WHERE";
        public const string rateconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[RateConfiguration] (MarketRateKey, MarginKey) VALUES(@MarketRateKey, @MarginKey); select cast(scope_identity() as int)";
        public const string rateconfigurationdatamodel_update = "UPDATE [2am].[dbo].[RateConfiguration] SET MarketRateKey = @MarketRateKey, MarginKey = @MarginKey WHERE RateConfigurationKey = @RateConfigurationKey";



        public const string offerdocumentreferencedatamodel_selectwhere = "SELECT OfferDocumentReferenceKey, OfferDocumentKey, DocumentTypeReferenceObjectKey, GenericKey FROM [2am].[dbo].[OfferDocumentReference] WHERE";
        public const string offerdocumentreferencedatamodel_selectbykey = "SELECT OfferDocumentReferenceKey, OfferDocumentKey, DocumentTypeReferenceObjectKey, GenericKey FROM [2am].[dbo].[OfferDocumentReference] WHERE OfferDocumentReferenceKey = @PrimaryKey";
        public const string offerdocumentreferencedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDocumentReference] WHERE OfferDocumentReferenceKey = @PrimaryKey";
        public const string offerdocumentreferencedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDocumentReference] WHERE";
        public const string offerdocumentreferencedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDocumentReference] (OfferDocumentKey, DocumentTypeReferenceObjectKey, GenericKey) VALUES(@OfferDocumentKey, @DocumentTypeReferenceObjectKey, @GenericKey); select cast(scope_identity() as int)";
        public const string offerdocumentreferencedatamodel_update = "UPDATE [2am].[dbo].[OfferDocumentReference] SET OfferDocumentKey = @OfferDocumentKey, DocumentTypeReferenceObjectKey = @DocumentTypeReferenceObjectKey, GenericKey = @GenericKey WHERE OfferDocumentReferenceKey = @OfferDocumentReferenceKey";



        public const string auditmarginproductdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MarginProductKey, MarginKey, OriginationSourceProductKey, Discount FROM [2am].[dbo].[AuditMarginProduct] WHERE";
        public const string auditmarginproductdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MarginProductKey, MarginKey, OriginationSourceProductKey, Discount FROM [2am].[dbo].[AuditMarginProduct] WHERE AuditNumber = @PrimaryKey";
        public const string auditmarginproductdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditMarginProduct] WHERE AuditNumber = @PrimaryKey";
        public const string auditmarginproductdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditMarginProduct] WHERE";
        public const string auditmarginproductdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditMarginProduct] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MarginProductKey, MarginKey, OriginationSourceProductKey, Discount) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @MarginProductKey, @MarginKey, @OriginationSourceProductKey, @Discount); select cast(scope_identity() as int)";
        public const string auditmarginproductdatamodel_update = "UPDATE [2am].[dbo].[AuditMarginProduct] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, MarginProductKey = @MarginProductKey, MarginKey = @MarginKey, OriginationSourceProductKey = @OriginationSourceProductKey, Discount = @Discount WHERE AuditNumber = @AuditNumber";



        public const string accountstatushistorydatamodel_selectwhere = "SELECT AccountStatusHistoryKey, AccountStatusKey, AccountKey, DateAssumed, Comment FROM [2am].[dbo].[AccountStatusHistory] WHERE";
        public const string accountstatushistorydatamodel_selectbykey = "SELECT AccountStatusHistoryKey, AccountStatusKey, AccountKey, DateAssumed, Comment FROM [2am].[dbo].[AccountStatusHistory] WHERE AccountStatusHistoryKey = @PrimaryKey";
        public const string accountstatushistorydatamodel_delete = "DELETE FROM [2am].[dbo].[AccountStatusHistory] WHERE AccountStatusHistoryKey = @PrimaryKey";
        public const string accountstatushistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountStatusHistory] WHERE";
        public const string accountstatushistorydatamodel_insert = "INSERT INTO [2am].[dbo].[AccountStatusHistory] (AccountStatusKey, AccountKey, DateAssumed, Comment) VALUES(@AccountStatusKey, @AccountKey, @DateAssumed, @Comment); select cast(scope_identity() as int)";
        public const string accountstatushistorydatamodel_update = "UPDATE [2am].[dbo].[AccountStatusHistory] SET AccountStatusKey = @AccountStatusKey, AccountKey = @AccountKey, DateAssumed = @DateAssumed, Comment = @Comment WHERE AccountStatusHistoryKey = @AccountStatusHistoryKey";



        public const string offeraccountrelationshipdatamodel_selectwhere = "SELECT OfferAccountRelationshipKey, AccountKey, OfferKey FROM [2am].[dbo].[OfferAccountRelationship] WHERE";
        public const string offeraccountrelationshipdatamodel_selectbykey = "SELECT OfferAccountRelationshipKey, AccountKey, OfferKey FROM [2am].[dbo].[OfferAccountRelationship] WHERE OfferAccountRelationshipKey = @PrimaryKey";
        public const string offeraccountrelationshipdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferAccountRelationship] WHERE OfferAccountRelationshipKey = @PrimaryKey";
        public const string offeraccountrelationshipdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferAccountRelationship] WHERE";
        public const string offeraccountrelationshipdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferAccountRelationship] (AccountKey, OfferKey) VALUES(@AccountKey, @OfferKey); select cast(scope_identity() as int)";
        public const string offeraccountrelationshipdatamodel_update = "UPDATE [2am].[dbo].[OfferAccountRelationship] SET AccountKey = @AccountKey, OfferKey = @OfferKey WHERE OfferAccountRelationshipKey = @OfferAccountRelationshipKey";



        public const string subsidydatamodel_selectwhere = "SELECT SubsidyKey, SubsidyProviderKey, EmploymentKey, LegalEntityKey, SalaryNumber, Paypoint, Notch, Rank, GeneralStatusKey, StopOrderAmount, GEPFMember FROM [2am].[dbo].[Subsidy] WHERE";
        public const string subsidydatamodel_selectbykey = "SELECT SubsidyKey, SubsidyProviderKey, EmploymentKey, LegalEntityKey, SalaryNumber, Paypoint, Notch, Rank, GeneralStatusKey, StopOrderAmount, GEPFMember FROM [2am].[dbo].[Subsidy] WHERE SubsidyKey = @PrimaryKey";
        public const string subsidydatamodel_delete = "DELETE FROM [2am].[dbo].[Subsidy] WHERE SubsidyKey = @PrimaryKey";
        public const string subsidydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Subsidy] WHERE";
        public const string subsidydatamodel_insert = "INSERT INTO [2am].[dbo].[Subsidy] (SubsidyProviderKey, EmploymentKey, LegalEntityKey, SalaryNumber, Paypoint, Notch, Rank, GeneralStatusKey, StopOrderAmount, GEPFMember) VALUES(@SubsidyProviderKey, @EmploymentKey, @LegalEntityKey, @SalaryNumber, @Paypoint, @Notch, @Rank, @GeneralStatusKey, @StopOrderAmount, @GEPFMember); select cast(scope_identity() as int)";
        public const string subsidydatamodel_update = "UPDATE [2am].[dbo].[Subsidy] SET SubsidyProviderKey = @SubsidyProviderKey, EmploymentKey = @EmploymentKey, LegalEntityKey = @LegalEntityKey, SalaryNumber = @SalaryNumber, Paypoint = @Paypoint, Notch = @Notch, Rank = @Rank, GeneralStatusKey = @GeneralStatusKey, StopOrderAmount = @StopOrderAmount, GEPFMember = @GEPFMember WHERE SubsidyKey = @SubsidyKey";



        public const string disabilityclaimdatamodel_selectwhere = "SELECT DisabilityClaimKey, AccountKey, LegalEntityKey, DateClaimReceived, LastDateWorked, DateOfDiagnosis, ClaimantOccupation, DisabilityTypeKey, OtherDisabilityComments, ExpectedReturnToWorkDate, DisabilityClaimStatusKey, PaymentStartDate, NumberOfInstalmentsAuthorised, PaymentEndDate FROM [2am].[dbo].[DisabilityClaim] WHERE";
        public const string disabilityclaimdatamodel_selectbykey = "SELECT DisabilityClaimKey, AccountKey, LegalEntityKey, DateClaimReceived, LastDateWorked, DateOfDiagnosis, ClaimantOccupation, DisabilityTypeKey, OtherDisabilityComments, ExpectedReturnToWorkDate, DisabilityClaimStatusKey, PaymentStartDate, NumberOfInstalmentsAuthorised, PaymentEndDate FROM [2am].[dbo].[DisabilityClaim] WHERE DisabilityClaimKey = @PrimaryKey";
        public const string disabilityclaimdatamodel_delete = "DELETE FROM [2am].[dbo].[DisabilityClaim] WHERE DisabilityClaimKey = @PrimaryKey";
        public const string disabilityclaimdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisabilityClaim] WHERE";
        public const string disabilityclaimdatamodel_insert = "INSERT INTO [2am].[dbo].[DisabilityClaim] (AccountKey, LegalEntityKey, DateClaimReceived, LastDateWorked, DateOfDiagnosis, ClaimantOccupation, DisabilityTypeKey, OtherDisabilityComments, ExpectedReturnToWorkDate, DisabilityClaimStatusKey, PaymentStartDate, NumberOfInstalmentsAuthorised, PaymentEndDate) VALUES(@AccountKey, @LegalEntityKey, @DateClaimReceived, @LastDateWorked, @DateOfDiagnosis, @ClaimantOccupation, @DisabilityTypeKey, @OtherDisabilityComments, @ExpectedReturnToWorkDate, @DisabilityClaimStatusKey, @PaymentStartDate, @NumberOfInstalmentsAuthorised, @PaymentEndDate); select cast(scope_identity() as int)";
        public const string disabilityclaimdatamodel_update = "UPDATE [2am].[dbo].[DisabilityClaim] SET AccountKey = @AccountKey, LegalEntityKey = @LegalEntityKey, DateClaimReceived = @DateClaimReceived, LastDateWorked = @LastDateWorked, DateOfDiagnosis = @DateOfDiagnosis, ClaimantOccupation = @ClaimantOccupation, DisabilityTypeKey = @DisabilityTypeKey, OtherDisabilityComments = @OtherDisabilityComments, ExpectedReturnToWorkDate = @ExpectedReturnToWorkDate, DisabilityClaimStatusKey = @DisabilityClaimStatusKey, PaymentStartDate = @PaymentStartDate, NumberOfInstalmentsAuthorised = @NumberOfInstalmentsAuthorised, PaymentEndDate = @PaymentEndDate WHERE DisabilityClaimKey = @DisabilityClaimKey";



        public const string remunerationtypedatamodel_selectwhere = "SELECT RemunerationTypeKey, Description FROM [2am].[dbo].[RemunerationType] WHERE";
        public const string remunerationtypedatamodel_selectbykey = "SELECT RemunerationTypeKey, Description FROM [2am].[dbo].[RemunerationType] WHERE RemunerationTypeKey = @PrimaryKey";
        public const string remunerationtypedatamodel_delete = "DELETE FROM [2am].[dbo].[RemunerationType] WHERE RemunerationTypeKey = @PrimaryKey";
        public const string remunerationtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RemunerationType] WHERE";
        public const string remunerationtypedatamodel_insert = "INSERT INTO [2am].[dbo].[RemunerationType] (RemunerationTypeKey, Description) VALUES(@RemunerationTypeKey, @Description); ";
        public const string remunerationtypedatamodel_update = "UPDATE [2am].[dbo].[RemunerationType] SET RemunerationTypeKey = @RemunerationTypeKey, Description = @Description WHERE RemunerationTypeKey = @RemunerationTypeKey";



        public const string marginproductdatamodel_selectwhere = "SELECT MarginProductKey, MarginKey, OriginationSourceProductKey, Discount FROM [2am].[dbo].[MarginProduct] WHERE";
        public const string marginproductdatamodel_selectbykey = "SELECT MarginProductKey, MarginKey, OriginationSourceProductKey, Discount FROM [2am].[dbo].[MarginProduct] WHERE MarginProductKey = @PrimaryKey";
        public const string marginproductdatamodel_delete = "DELETE FROM [2am].[dbo].[MarginProduct] WHERE MarginProductKey = @PrimaryKey";
        public const string marginproductdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MarginProduct] WHERE";
        public const string marginproductdatamodel_insert = "INSERT INTO [2am].[dbo].[MarginProduct] (MarginKey, OriginationSourceProductKey, Discount) VALUES(@MarginKey, @OriginationSourceProductKey, @Discount); select cast(scope_identity() as int)";
        public const string marginproductdatamodel_update = "UPDATE [2am].[dbo].[MarginProduct] SET MarginKey = @MarginKey, OriginationSourceProductKey = @OriginationSourceProductKey, Discount = @Discount WHERE MarginProductKey = @MarginProductKey";



        public const string offeroriginatordatamodel_selectwhere = "SELECT OfferOriginatorKey, LegalEntityKey, Contact, GeneralStatusKey, OriginationSourceKey FROM [2am].[dbo].[OfferOriginator] WHERE";
        public const string offeroriginatordatamodel_selectbykey = "SELECT OfferOriginatorKey, LegalEntityKey, Contact, GeneralStatusKey, OriginationSourceKey FROM [2am].[dbo].[OfferOriginator] WHERE OfferOriginatorKey = @PrimaryKey";
        public const string offeroriginatordatamodel_delete = "DELETE FROM [2am].[dbo].[OfferOriginator] WHERE OfferOriginatorKey = @PrimaryKey";
        public const string offeroriginatordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferOriginator] WHERE";
        public const string offeroriginatordatamodel_insert = "INSERT INTO [2am].[dbo].[OfferOriginator] (LegalEntityKey, Contact, GeneralStatusKey, OriginationSourceKey) VALUES(@LegalEntityKey, @Contact, @GeneralStatusKey, @OriginationSourceKey); select cast(scope_identity() as int)";
        public const string offeroriginatordatamodel_update = "UPDATE [2am].[dbo].[OfferOriginator] SET LegalEntityKey = @LegalEntityKey, Contact = @Contact, GeneralStatusKey = @GeneralStatusKey, OriginationSourceKey = @OriginationSourceKey WHERE OfferOriginatorKey = @OfferOriginatorKey";



        public const string importemploymentdatamodel_selectwhere = "SELECT EmploymentKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmployerName, EmployerContactPerson, EmployerPhoneCode, EmployerPhoneNumber, EmploymentStartDate, EmploymentEndDate, MonthlyIncome FROM [2am].[dbo].[ImportEmployment] WHERE";
        public const string importemploymentdatamodel_selectbykey = "SELECT EmploymentKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmployerName, EmployerContactPerson, EmployerPhoneCode, EmployerPhoneNumber, EmploymentStartDate, EmploymentEndDate, MonthlyIncome FROM [2am].[dbo].[ImportEmployment] WHERE EmploymentKey = @PrimaryKey";
        public const string importemploymentdatamodel_delete = "DELETE FROM [2am].[dbo].[ImportEmployment] WHERE EmploymentKey = @PrimaryKey";
        public const string importemploymentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportEmployment] WHERE";
        public const string importemploymentdatamodel_insert = "INSERT INTO [2am].[dbo].[ImportEmployment] (EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmployerName, EmployerContactPerson, EmployerPhoneCode, EmployerPhoneNumber, EmploymentStartDate, EmploymentEndDate, MonthlyIncome) VALUES(@EmploymentTypeKey, @RemunerationTypeKey, @EmploymentStatusKey, @LegalEntityKey, @EmployerName, @EmployerContactPerson, @EmployerPhoneCode, @EmployerPhoneNumber, @EmploymentStartDate, @EmploymentEndDate, @MonthlyIncome); select cast(scope_identity() as int)";
        public const string importemploymentdatamodel_update = "UPDATE [2am].[dbo].[ImportEmployment] SET EmploymentTypeKey = @EmploymentTypeKey, RemunerationTypeKey = @RemunerationTypeKey, EmploymentStatusKey = @EmploymentStatusKey, LegalEntityKey = @LegalEntityKey, EmployerName = @EmployerName, EmployerContactPerson = @EmployerContactPerson, EmployerPhoneCode = @EmployerPhoneCode, EmployerPhoneNumber = @EmployerPhoneNumber, EmploymentStartDate = @EmploymentStartDate, EmploymentEndDate = @EmploymentEndDate, MonthlyIncome = @MonthlyIncome WHERE EmploymentKey = @EmploymentKey";



        public const string onlinestatementformatdatamodel_selectwhere = "SELECT OnlineStatementFormatKey, Description FROM [2am].[dbo].[OnlineStatementFormat] WHERE";
        public const string onlinestatementformatdatamodel_selectbykey = "SELECT OnlineStatementFormatKey, Description FROM [2am].[dbo].[OnlineStatementFormat] WHERE OnlineStatementFormatKey = @PrimaryKey";
        public const string onlinestatementformatdatamodel_delete = "DELETE FROM [2am].[dbo].[OnlineStatementFormat] WHERE OnlineStatementFormatKey = @PrimaryKey";
        public const string onlinestatementformatdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OnlineStatementFormat] WHERE";
        public const string onlinestatementformatdatamodel_insert = "INSERT INTO [2am].[dbo].[OnlineStatementFormat] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string onlinestatementformatdatamodel_update = "UPDATE [2am].[dbo].[OnlineStatementFormat] SET Description = @Description WHERE OnlineStatementFormatKey = @OnlineStatementFormatKey";



        public const string productconditiondatamodel_selectwhere = "SELECT ProductConditionKey, ProductConditionTypeKey, ConditionKey, OriginationSourceProductKey, FinancialServiceTypeKey, PurposeKey, ApplicationName FROM [2am].[dbo].[ProductCondition] WHERE";
        public const string productconditiondatamodel_selectbykey = "SELECT ProductConditionKey, ProductConditionTypeKey, ConditionKey, OriginationSourceProductKey, FinancialServiceTypeKey, PurposeKey, ApplicationName FROM [2am].[dbo].[ProductCondition] WHERE ProductConditionKey = @PrimaryKey";
        public const string productconditiondatamodel_delete = "DELETE FROM [2am].[dbo].[ProductCondition] WHERE ProductConditionKey = @PrimaryKey";
        public const string productconditiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ProductCondition] WHERE";
        public const string productconditiondatamodel_insert = "INSERT INTO [2am].[dbo].[ProductCondition] (ProductConditionTypeKey, ConditionKey, OriginationSourceProductKey, FinancialServiceTypeKey, PurposeKey, ApplicationName) VALUES(@ProductConditionTypeKey, @ConditionKey, @OriginationSourceProductKey, @FinancialServiceTypeKey, @PurposeKey, @ApplicationName); select cast(scope_identity() as int)";
        public const string productconditiondatamodel_update = "UPDATE [2am].[dbo].[ProductCondition] SET ProductConditionTypeKey = @ProductConditionTypeKey, ConditionKey = @ConditionKey, OriginationSourceProductKey = @OriginationSourceProductKey, FinancialServiceTypeKey = @FinancialServiceTypeKey, PurposeKey = @PurposeKey, ApplicationName = @ApplicationName WHERE ProductConditionKey = @ProductConditionKey";



        public const string batchtransactiondatamodel_selectwhere = "SELECT BatchTransactionKey, BulkBatchKey, AccountKey, LegalEntityKey, TransactionTypeNumber, EffectiveDate, Amount, Reference, UserID, BatchTransactionStatusKey FROM [2am].[dbo].[BatchTransaction] WHERE";
        public const string batchtransactiondatamodel_selectbykey = "SELECT BatchTransactionKey, BulkBatchKey, AccountKey, LegalEntityKey, TransactionTypeNumber, EffectiveDate, Amount, Reference, UserID, BatchTransactionStatusKey FROM [2am].[dbo].[BatchTransaction] WHERE BatchTransactionKey = @PrimaryKey";
        public const string batchtransactiondatamodel_delete = "DELETE FROM [2am].[dbo].[BatchTransaction] WHERE BatchTransactionKey = @PrimaryKey";
        public const string batchtransactiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchTransaction] WHERE";
        public const string batchtransactiondatamodel_insert = "INSERT INTO [2am].[dbo].[BatchTransaction] (BulkBatchKey, AccountKey, LegalEntityKey, TransactionTypeNumber, EffectiveDate, Amount, Reference, UserID, BatchTransactionStatusKey) VALUES(@BulkBatchKey, @AccountKey, @LegalEntityKey, @TransactionTypeNumber, @EffectiveDate, @Amount, @Reference, @UserID, @BatchTransactionStatusKey); select cast(scope_identity() as int)";
        public const string batchtransactiondatamodel_update = "UPDATE [2am].[dbo].[BatchTransaction] SET BulkBatchKey = @BulkBatchKey, AccountKey = @AccountKey, LegalEntityKey = @LegalEntityKey, TransactionTypeNumber = @TransactionTypeNumber, EffectiveDate = @EffectiveDate, Amount = @Amount, Reference = @Reference, UserID = @UserID, BatchTransactionStatusKey = @BatchTransactionStatusKey WHERE BatchTransactionKey = @BatchTransactionKey";



        public const string importstatusdatamodel_selectwhere = "SELECT ImportStatusKey, Description FROM [2am].[dbo].[ImportStatus] WHERE";
        public const string importstatusdatamodel_selectbykey = "SELECT ImportStatusKey, Description FROM [2am].[dbo].[ImportStatus] WHERE ImportStatusKey = @PrimaryKey";
        public const string importstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[ImportStatus] WHERE ImportStatusKey = @PrimaryKey";
        public const string importstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportStatus] WHERE";
        public const string importstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[ImportStatus] (ImportStatusKey, Description) VALUES(@ImportStatusKey, @Description); ";
        public const string importstatusdatamodel_update = "UPDATE [2am].[dbo].[ImportStatus] SET ImportStatusKey = @ImportStatusKey, Description = @Description WHERE ImportStatusKey = @ImportStatusKey";



        public const string datatypedatamodel_selectwhere = "SELECT DataTypeKey, Description FROM [2am].[dbo].[DataType] WHERE";
        public const string datatypedatamodel_selectbykey = "SELECT DataTypeKey, Description FROM [2am].[dbo].[DataType] WHERE DataTypeKey = @PrimaryKey";
        public const string datatypedatamodel_delete = "DELETE FROM [2am].[dbo].[DataType] WHERE DataTypeKey = @PrimaryKey";
        public const string datatypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DataType] WHERE";
        public const string datatypedatamodel_insert = "INSERT INTO [2am].[dbo].[DataType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string datatypedatamodel_update = "UPDATE [2am].[dbo].[DataType] SET Description = @Description WHERE DataTypeKey = @DataTypeKey";



        public const string offersourcedatamodel_selectwhere = "SELECT OfferSourceKey, Description, GeneralStatusKey FROM [2am].[dbo].[OfferSource] WHERE";
        public const string offersourcedatamodel_selectbykey = "SELECT OfferSourceKey, Description, GeneralStatusKey FROM [2am].[dbo].[OfferSource] WHERE OfferSourceKey = @PrimaryKey";
        public const string offersourcedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferSource] WHERE OfferSourceKey = @PrimaryKey";
        public const string offersourcedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferSource] WHERE";
        public const string offersourcedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferSource] (Description, GeneralStatusKey) VALUES(@Description, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string offersourcedatamodel_update = "UPDATE [2am].[dbo].[OfferSource] SET Description = @Description, GeneralStatusKey = @GeneralStatusKey WHERE OfferSourceKey = @OfferSourceKey";



        public const string offerdebitorderdatamodel_selectwhere = "SELECT OfferDebitOrderKey, OfferKey, BankAccountKey, Percentage, DebitOrderDay, FinancialServicePaymentTypeKey FROM [2am].[dbo].[OfferDebitOrder] WHERE";
        public const string offerdebitorderdatamodel_selectbykey = "SELECT OfferDebitOrderKey, OfferKey, BankAccountKey, Percentage, DebitOrderDay, FinancialServicePaymentTypeKey FROM [2am].[dbo].[OfferDebitOrder] WHERE OfferDebitOrderKey = @PrimaryKey";
        public const string offerdebitorderdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDebitOrder] WHERE OfferDebitOrderKey = @PrimaryKey";
        public const string offerdebitorderdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDebitOrder] WHERE";
        public const string offerdebitorderdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDebitOrder] (OfferKey, BankAccountKey, Percentage, DebitOrderDay, FinancialServicePaymentTypeKey) VALUES(@OfferKey, @BankAccountKey, @Percentage, @DebitOrderDay, @FinancialServicePaymentTypeKey); select cast(scope_identity() as int)";
        public const string offerdebitorderdatamodel_update = "UPDATE [2am].[dbo].[OfferDebitOrder] SET OfferKey = @OfferKey, BankAccountKey = @BankAccountKey, Percentage = @Percentage, DebitOrderDay = @DebitOrderDay, FinancialServicePaymentTypeKey = @FinancialServicePaymentTypeKey WHERE OfferDebitOrderKey = @OfferDebitOrderKey";



        public const string reporttypedatamodel_selectwhere = "SELECT ReportTypeKey, Description FROM [2am].[dbo].[ReportType] WHERE";
        public const string reporttypedatamodel_selectbykey = "SELECT ReportTypeKey, Description FROM [2am].[dbo].[ReportType] WHERE ReportTypeKey = @PrimaryKey";
        public const string reporttypedatamodel_delete = "DELETE FROM [2am].[dbo].[ReportType] WHERE ReportTypeKey = @PrimaryKey";
        public const string reporttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReportType] WHERE";
        public const string reporttypedatamodel_insert = "INSERT INTO [2am].[dbo].[ReportType] (ReportTypeKey, Description) VALUES(@ReportTypeKey, @Description); ";
        public const string reporttypedatamodel_update = "UPDATE [2am].[dbo].[ReportType] SET ReportTypeKey = @ReportTypeKey, Description = @Description WHERE ReportTypeKey = @ReportTypeKey";



        public const string ruleexclusionsetdatamodel_selectwhere = "SELECT RuleExclusionSetKey, Description, Comment FROM [2am].[dbo].[RuleExclusionSet] WHERE";
        public const string ruleexclusionsetdatamodel_selectbykey = "SELECT RuleExclusionSetKey, Description, Comment FROM [2am].[dbo].[RuleExclusionSet] WHERE RuleExclusionSetKey = @PrimaryKey";
        public const string ruleexclusionsetdatamodel_delete = "DELETE FROM [2am].[dbo].[RuleExclusionSet] WHERE RuleExclusionSetKey = @PrimaryKey";
        public const string ruleexclusionsetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RuleExclusionSet] WHERE";
        public const string ruleexclusionsetdatamodel_insert = "INSERT INTO [2am].[dbo].[RuleExclusionSet] (RuleExclusionSetKey, Description, Comment) VALUES(@RuleExclusionSetKey, @Description, @Comment); ";
        public const string ruleexclusionsetdatamodel_update = "UPDATE [2am].[dbo].[RuleExclusionSet] SET RuleExclusionSetKey = @RuleExclusionSetKey, Description = @Description, Comment = @Comment WHERE RuleExclusionSetKey = @RuleExclusionSetKey";



        public const string postofficedatamodel_selectwhere = "SELECT PostOfficeKey, Description, PostalCode, CityKey FROM [2am].[dbo].[PostOffice] WHERE";
        public const string postofficedatamodel_selectbykey = "SELECT PostOfficeKey, Description, PostalCode, CityKey FROM [2am].[dbo].[PostOffice] WHERE PostOfficeKey = @PrimaryKey";
        public const string postofficedatamodel_delete = "DELETE FROM [2am].[dbo].[PostOffice] WHERE PostOfficeKey = @PrimaryKey";
        public const string postofficedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PostOffice] WHERE";
        public const string postofficedatamodel_insert = "INSERT INTO [2am].[dbo].[PostOffice] (Description, PostalCode, CityKey) VALUES(@Description, @PostalCode, @CityKey); select cast(scope_identity() as int)";
        public const string postofficedatamodel_update = "UPDATE [2am].[dbo].[PostOffice] SET Description = @Description, PostalCode = @PostalCode, CityKey = @CityKey WHERE PostOfficeKey = @PostOfficeKey";



        public const string futuredatedchangedatamodel_selectwhere = "SELECT FutureDatedChangeKey, FutureDatedChangeTypeKey, IdentifierReferenceKey, EffectiveDate, NotificationRequired, UserID, InsertDate, ChangeDate FROM [2am].[dbo].[FutureDatedChange] WHERE";
        public const string futuredatedchangedatamodel_selectbykey = "SELECT FutureDatedChangeKey, FutureDatedChangeTypeKey, IdentifierReferenceKey, EffectiveDate, NotificationRequired, UserID, InsertDate, ChangeDate FROM [2am].[dbo].[FutureDatedChange] WHERE FutureDatedChangeKey = @PrimaryKey";
        public const string futuredatedchangedatamodel_delete = "DELETE FROM [2am].[dbo].[FutureDatedChange] WHERE FutureDatedChangeKey = @PrimaryKey";
        public const string futuredatedchangedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FutureDatedChange] WHERE";
        public const string futuredatedchangedatamodel_insert = "INSERT INTO [2am].[dbo].[FutureDatedChange] (FutureDatedChangeTypeKey, IdentifierReferenceKey, EffectiveDate, NotificationRequired, UserID, InsertDate, ChangeDate) VALUES(@FutureDatedChangeTypeKey, @IdentifierReferenceKey, @EffectiveDate, @NotificationRequired, @UserID, @InsertDate, @ChangeDate); select cast(scope_identity() as int)";
        public const string futuredatedchangedatamodel_update = "UPDATE [2am].[dbo].[FutureDatedChange] SET FutureDatedChangeTypeKey = @FutureDatedChangeTypeKey, IdentifierReferenceKey = @IdentifierReferenceKey, EffectiveDate = @EffectiveDate, NotificationRequired = @NotificationRequired, UserID = @UserID, InsertDate = @InsertDate, ChangeDate = @ChangeDate WHERE FutureDatedChangeKey = @FutureDatedChangeKey";



        public const string organisationstructureattributetypedatamodel_selectwhere = "SELECT OrganisationStructureAttributeTypeKey, Description, DataTypeKey, Length FROM [2am].[dbo].[OrganisationStructureAttributeType] WHERE";
        public const string organisationstructureattributetypedatamodel_selectbykey = "SELECT OrganisationStructureAttributeTypeKey, Description, DataTypeKey, Length FROM [2am].[dbo].[OrganisationStructureAttributeType] WHERE OrganisationStructureAttributeTypeKey = @PrimaryKey";
        public const string organisationstructureattributetypedatamodel_delete = "DELETE FROM [2am].[dbo].[OrganisationStructureAttributeType] WHERE OrganisationStructureAttributeTypeKey = @PrimaryKey";
        public const string organisationstructureattributetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OrganisationStructureAttributeType] WHERE";
        public const string organisationstructureattributetypedatamodel_insert = "INSERT INTO [2am].[dbo].[OrganisationStructureAttributeType] (Description, DataTypeKey, Length) VALUES(@Description, @DataTypeKey, @Length); select cast(scope_identity() as int)";
        public const string organisationstructureattributetypedatamodel_update = "UPDATE [2am].[dbo].[OrganisationStructureAttributeType] SET Description = @Description, DataTypeKey = @DataTypeKey, Length = @Length WHERE OrganisationStructureAttributeTypeKey = @OrganisationStructureAttributeTypeKey";



        public const string importaccountexpensedatamodel_selectwhere = "SELECT AccountExpenseKey, OfferKey, ExpenseTypeKey, ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, ToBeSettled FROM [2am].[dbo].[ImportAccountExpense] WHERE";
        public const string importaccountexpensedatamodel_selectbykey = "SELECT AccountExpenseKey, OfferKey, ExpenseTypeKey, ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, ToBeSettled FROM [2am].[dbo].[ImportAccountExpense] WHERE AccountExpenseKey = @PrimaryKey";
        public const string importaccountexpensedatamodel_delete = "DELETE FROM [2am].[dbo].[ImportAccountExpense] WHERE AccountExpenseKey = @PrimaryKey";
        public const string importaccountexpensedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportAccountExpense] WHERE";
        public const string importaccountexpensedatamodel_insert = "INSERT INTO [2am].[dbo].[ImportAccountExpense] (OfferKey, ExpenseTypeKey, ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, ToBeSettled) VALUES(@OfferKey, @ExpenseTypeKey, @ExpenseAccountNumber, @ExpenseAccountName, @ExpenseReference, @TotalOutstandingAmount, @MonthlyPayment, @ToBeSettled); select cast(scope_identity() as int)";
        public const string importaccountexpensedatamodel_update = "UPDATE [2am].[dbo].[ImportAccountExpense] SET OfferKey = @OfferKey, ExpenseTypeKey = @ExpenseTypeKey, ExpenseAccountNumber = @ExpenseAccountNumber, ExpenseAccountName = @ExpenseAccountName, ExpenseReference = @ExpenseReference, TotalOutstandingAmount = @TotalOutstandingAmount, MonthlyPayment = @MonthlyPayment, ToBeSettled = @ToBeSettled WHERE AccountExpenseKey = @AccountExpenseKey";



        public const string prioritydatamodel_selectwhere = "SELECT PriorityKey, OriginationSourceProductKey, Description FROM [2am].[dbo].[Priority] WHERE";
        public const string prioritydatamodel_selectbykey = "SELECT PriorityKey, OriginationSourceProductKey, Description FROM [2am].[dbo].[Priority] WHERE PriorityKey = @PrimaryKey";
        public const string prioritydatamodel_delete = "DELETE FROM [2am].[dbo].[Priority] WHERE PriorityKey = @PrimaryKey";
        public const string prioritydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Priority] WHERE";
        public const string prioritydatamodel_insert = "INSERT INTO [2am].[dbo].[Priority] (OriginationSourceProductKey, Description) VALUES(@OriginationSourceProductKey, @Description); select cast(scope_identity() as int)";
        public const string prioritydatamodel_update = "UPDATE [2am].[dbo].[Priority] SET OriginationSourceProductKey = @OriginationSourceProductKey, Description = @Description WHERE PriorityKey = @PriorityKey";



        public const string monthlyfeetoberaiseddatamodel_selectwhere = "SELECT AccountKey, FinancialServiceKey, GrantedDate, DateDisbursed FROM [2am].[dbo].[MonthlyFeeToBeRaised] WHERE";
        public const string monthlyfeetoberaiseddatamodel_selectbykey = "SELECT AccountKey, FinancialServiceKey, GrantedDate, DateDisbursed FROM [2am].[dbo].[MonthlyFeeToBeRaised] WHERE  = @PrimaryKey";
        public const string monthlyfeetoberaiseddatamodel_delete = "DELETE FROM [2am].[dbo].[MonthlyFeeToBeRaised] WHERE  = @PrimaryKey";
        public const string monthlyfeetoberaiseddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MonthlyFeeToBeRaised] WHERE";
        public const string monthlyfeetoberaiseddatamodel_insert = "INSERT INTO [2am].[dbo].[MonthlyFeeToBeRaised] (AccountKey, FinancialServiceKey, GrantedDate, DateDisbursed) VALUES(@AccountKey, @FinancialServiceKey, @GrantedDate, @DateDisbursed); ";
        public const string monthlyfeetoberaiseddatamodel_update = "UPDATE [2am].[dbo].[MonthlyFeeToBeRaised] SET AccountKey = @AccountKey, FinancialServiceKey = @FinancialServiceKey, GrantedDate = @GrantedDate, DateDisbursed = @DateDisbursed WHERE  = @";



        public const string auditmortgageloandatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, InitialInstallments, InitialBalance, RemainingInstallments, CurrentBalance, ArrearBalance, CloseDate, OpenDate, InterestRate, SPVKey, RateConfigurationKey, ResetConfigurationKey, AccruedInterestMTD, MortgageLoanPurposeKey, CreditMatrixKey, PreApproved, Discount, BaseRate, UserID, ChangeDate, ActiveMarketRate, AccumulatedCoPayment, MTDCoPayment FROM [2am].[dbo].[AuditMortgageLoan] WHERE";
        public const string auditmortgageloandatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, InitialInstallments, InitialBalance, RemainingInstallments, CurrentBalance, ArrearBalance, CloseDate, OpenDate, InterestRate, SPVKey, RateConfigurationKey, ResetConfigurationKey, AccruedInterestMTD, MortgageLoanPurposeKey, CreditMatrixKey, PreApproved, Discount, BaseRate, UserID, ChangeDate, ActiveMarketRate, AccumulatedCoPayment, MTDCoPayment FROM [2am].[dbo].[AuditMortgageLoan] WHERE AuditNumber = @PrimaryKey";
        public const string auditmortgageloandatamodel_delete = "DELETE FROM [2am].[dbo].[AuditMortgageLoan] WHERE AuditNumber = @PrimaryKey";
        public const string auditmortgageloandatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditMortgageLoan] WHERE";
        public const string auditmortgageloandatamodel_insert = "INSERT INTO [2am].[dbo].[AuditMortgageLoan] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, InitialInstallments, InitialBalance, RemainingInstallments, CurrentBalance, ArrearBalance, CloseDate, OpenDate, InterestRate, SPVKey, RateConfigurationKey, ResetConfigurationKey, AccruedInterestMTD, MortgageLoanPurposeKey, CreditMatrixKey, PreApproved, Discount, BaseRate, UserID, ChangeDate, ActiveMarketRate, AccumulatedCoPayment, MTDCoPayment) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @FinancialServiceKey, @InitialInstallments, @InitialBalance, @RemainingInstallments, @CurrentBalance, @ArrearBalance, @CloseDate, @OpenDate, @InterestRate, @SPVKey, @RateConfigurationKey, @ResetConfigurationKey, @AccruedInterestMTD, @MortgageLoanPurposeKey, @CreditMatrixKey, @PreApproved, @Discount, @BaseRate, @UserID, @ChangeDate, @ActiveMarketRate, @AccumulatedCoPayment, @MTDCoPayment); select cast(scope_identity() as int)";
        public const string auditmortgageloandatamodel_update = "UPDATE [2am].[dbo].[AuditMortgageLoan] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, FinancialServiceKey = @FinancialServiceKey, InitialInstallments = @InitialInstallments, InitialBalance = @InitialBalance, RemainingInstallments = @RemainingInstallments, CurrentBalance = @CurrentBalance, ArrearBalance = @ArrearBalance, CloseDate = @CloseDate, OpenDate = @OpenDate, InterestRate = @InterestRate, SPVKey = @SPVKey, RateConfigurationKey = @RateConfigurationKey, ResetConfigurationKey = @ResetConfigurationKey, AccruedInterestMTD = @AccruedInterestMTD, MortgageLoanPurposeKey = @MortgageLoanPurposeKey, CreditMatrixKey = @CreditMatrixKey, PreApproved = @PreApproved, Discount = @Discount, BaseRate = @BaseRate, UserID = @UserID, ChangeDate = @ChangeDate, ActiveMarketRate = @ActiveMarketRate, AccumulatedCoPayment = @AccumulatedCoPayment, MTDCoPayment = @MTDCoPayment WHERE AuditNumber = @AuditNumber";



        public const string externalroledatamodel_selectwhere = "SELECT ExternalRoleKey, GenericKey, GenericKeyTypeKey, LegalEntityKey, ExternalRoleTypeKey, GeneralStatusKey, ChangeDate FROM [2am].[dbo].[ExternalRole] WHERE";
        public const string externalroledatamodel_selectbykey = "SELECT ExternalRoleKey, GenericKey, GenericKeyTypeKey, LegalEntityKey, ExternalRoleTypeKey, GeneralStatusKey, ChangeDate FROM [2am].[dbo].[ExternalRole] WHERE ExternalRoleKey = @PrimaryKey";
        public const string externalroledatamodel_delete = "DELETE FROM [2am].[dbo].[ExternalRole] WHERE ExternalRoleKey = @PrimaryKey";
        public const string externalroledatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ExternalRole] WHERE";
        public const string externalroledatamodel_insert = "INSERT INTO [2am].[dbo].[ExternalRole] (GenericKey, GenericKeyTypeKey, LegalEntityKey, ExternalRoleTypeKey, GeneralStatusKey, ChangeDate) VALUES(@GenericKey, @GenericKeyTypeKey, @LegalEntityKey, @ExternalRoleTypeKey, @GeneralStatusKey, @ChangeDate); select cast(scope_identity() as int)";
        public const string externalroledatamodel_update = "UPDATE [2am].[dbo].[ExternalRole] SET GenericKey = @GenericKey, GenericKeyTypeKey = @GenericKeyTypeKey, LegalEntityKey = @LegalEntityKey, ExternalRoleTypeKey = @ExternalRoleTypeKey, GeneralStatusKey = @GeneralStatusKey, ChangeDate = @ChangeDate WHERE ExternalRoleKey = @ExternalRoleKey";



        public const string disabilitypaymentdatamodel_selectwhere = "SELECT DisabilityPaymentKey, DisabilityClaimKey, PaymentDate, Amount, DisabilityPaymentStatusKey FROM [2am].[dbo].[DisabilityPayment] WHERE";
        public const string disabilitypaymentdatamodel_selectbykey = "SELECT DisabilityPaymentKey, DisabilityClaimKey, PaymentDate, Amount, DisabilityPaymentStatusKey FROM [2am].[dbo].[DisabilityPayment] WHERE DisabilityPaymentKey = @PrimaryKey";
        public const string disabilitypaymentdatamodel_delete = "DELETE FROM [2am].[dbo].[DisabilityPayment] WHERE DisabilityPaymentKey = @PrimaryKey";
        public const string disabilitypaymentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisabilityPayment] WHERE";
        public const string disabilitypaymentdatamodel_insert = "INSERT INTO [2am].[dbo].[DisabilityPayment] (DisabilityClaimKey, PaymentDate, Amount, DisabilityPaymentStatusKey) VALUES(@DisabilityClaimKey, @PaymentDate, @Amount, @DisabilityPaymentStatusKey); select cast(scope_identity() as int)";
        public const string disabilitypaymentdatamodel_update = "UPDATE [2am].[dbo].[DisabilityPayment] SET DisabilityClaimKey = @DisabilityClaimKey, PaymentDate = @PaymentDate, Amount = @Amount, DisabilityPaymentStatusKey = @DisabilityPaymentStatusKey WHERE DisabilityPaymentKey = @DisabilityPaymentKey";



        public const string roletypedatamodel_selectwhere = "SELECT RoleTypeKey, Description FROM [2am].[dbo].[RoleType] WHERE";
        public const string roletypedatamodel_selectbykey = "SELECT RoleTypeKey, Description FROM [2am].[dbo].[RoleType] WHERE RoleTypeKey = @PrimaryKey";
        public const string roletypedatamodel_delete = "DELETE FROM [2am].[dbo].[RoleType] WHERE RoleTypeKey = @PrimaryKey";
        public const string roletypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RoleType] WHERE";
        public const string roletypedatamodel_insert = "INSERT INTO [2am].[dbo].[RoleType] (RoleTypeKey, Description) VALUES(@RoleTypeKey, @Description); ";
        public const string roletypedatamodel_update = "UPDATE [2am].[dbo].[RoleType] SET RoleTypeKey = @RoleTypeKey, Description = @Description WHERE RoleTypeKey = @RoleTypeKey";



        public const string insurerdatamodel_selectwhere = "SELECT InsurerKey, Description FROM [2am].[dbo].[Insurer] WHERE";
        public const string insurerdatamodel_selectbykey = "SELECT InsurerKey, Description FROM [2am].[dbo].[Insurer] WHERE InsurerKey = @PrimaryKey";
        public const string insurerdatamodel_delete = "DELETE FROM [2am].[dbo].[Insurer] WHERE InsurerKey = @PrimaryKey";
        public const string insurerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Insurer] WHERE";
        public const string insurerdatamodel_insert = "INSERT INTO [2am].[dbo].[Insurer] (InsurerKey, Description) VALUES(@InsurerKey, @Description); ";
        public const string insurerdatamodel_update = "UPDATE [2am].[dbo].[Insurer] SET InsurerKey = @InsurerKey, Description = @Description WHERE InsurerKey = @InsurerKey";



        public const string accountsubsidydatamodel_selectwhere = "SELECT AccountSubsidyKey, AccountKey, SubsidyKey FROM [2am].[dbo].[AccountSubsidy] WHERE";
        public const string accountsubsidydatamodel_selectbykey = "SELECT AccountSubsidyKey, AccountKey, SubsidyKey FROM [2am].[dbo].[AccountSubsidy] WHERE AccountSubsidyKey = @PrimaryKey";
        public const string accountsubsidydatamodel_delete = "DELETE FROM [2am].[dbo].[AccountSubsidy] WHERE AccountSubsidyKey = @PrimaryKey";
        public const string accountsubsidydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountSubsidy] WHERE";
        public const string accountsubsidydatamodel_insert = "INSERT INTO [2am].[dbo].[AccountSubsidy] (AccountKey, SubsidyKey) VALUES(@AccountKey, @SubsidyKey); select cast(scope_identity() as int)";
        public const string accountsubsidydatamodel_update = "UPDATE [2am].[dbo].[AccountSubsidy] SET AccountKey = @AccountKey, SubsidyKey = @SubsidyKey WHERE AccountSubsidyKey = @AccountSubsidyKey";



        public const string ruleexclusiondatamodel_selectwhere = "SELECT RuleExclusionKey, RuleExclusionSetKey, RuleItemKey FROM [2am].[dbo].[RuleExclusion] WHERE";
        public const string ruleexclusiondatamodel_selectbykey = "SELECT RuleExclusionKey, RuleExclusionSetKey, RuleItemKey FROM [2am].[dbo].[RuleExclusion] WHERE RuleExclusionKey = @PrimaryKey";
        public const string ruleexclusiondatamodel_delete = "DELETE FROM [2am].[dbo].[RuleExclusion] WHERE RuleExclusionKey = @PrimaryKey";
        public const string ruleexclusiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RuleExclusion] WHERE";
        public const string ruleexclusiondatamodel_insert = "INSERT INTO [2am].[dbo].[RuleExclusion] (RuleExclusionSetKey, RuleItemKey) VALUES(@RuleExclusionSetKey, @RuleItemKey); select cast(scope_identity() as int)";
        public const string ruleexclusiondatamodel_update = "UPDATE [2am].[dbo].[RuleExclusion] SET RuleExclusionSetKey = @RuleExclusionSetKey, RuleItemKey = @RuleItemKey WHERE RuleExclusionKey = @RuleExclusionKey";



        public const string futuredatedchangedetaildatamodel_selectwhere = "SELECT FutureDatedChangeDetailKey, FutureDatedChangeKey, ReferenceKey, Action, TableName, ColumnName, Value, UserID, ChangeDate FROM [2am].[dbo].[FutureDatedChangeDetail] WHERE";
        public const string futuredatedchangedetaildatamodel_selectbykey = "SELECT FutureDatedChangeDetailKey, FutureDatedChangeKey, ReferenceKey, Action, TableName, ColumnName, Value, UserID, ChangeDate FROM [2am].[dbo].[FutureDatedChangeDetail] WHERE FutureDatedChangeDetailKey = @PrimaryKey";
        public const string futuredatedchangedetaildatamodel_delete = "DELETE FROM [2am].[dbo].[FutureDatedChangeDetail] WHERE FutureDatedChangeDetailKey = @PrimaryKey";
        public const string futuredatedchangedetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FutureDatedChangeDetail] WHERE";
        public const string futuredatedchangedetaildatamodel_insert = "INSERT INTO [2am].[dbo].[FutureDatedChangeDetail] (FutureDatedChangeKey, ReferenceKey, Action, TableName, ColumnName, Value, UserID, ChangeDate) VALUES(@FutureDatedChangeKey, @ReferenceKey, @Action, @TableName, @ColumnName, @Value, @UserID, @ChangeDate); select cast(scope_identity() as int)";
        public const string futuredatedchangedetaildatamodel_update = "UPDATE [2am].[dbo].[FutureDatedChangeDetail] SET FutureDatedChangeKey = @FutureDatedChangeKey, ReferenceKey = @ReferenceKey, Action = @Action, TableName = @TableName, ColumnName = @ColumnName, Value = @Value, UserID = @UserID, ChangeDate = @ChangeDate WHERE FutureDatedChangeDetailKey = @FutureDatedChangeDetailKey";



        public const string externalroletypedatamodel_selectwhere = "SELECT ExternalRoleTypeKey, Description, ExternalRoleTypeGroupKey FROM [2am].[dbo].[ExternalRoleType] WHERE";
        public const string externalroletypedatamodel_selectbykey = "SELECT ExternalRoleTypeKey, Description, ExternalRoleTypeGroupKey FROM [2am].[dbo].[ExternalRoleType] WHERE ExternalRoleTypeKey = @PrimaryKey";
        public const string externalroletypedatamodel_delete = "DELETE FROM [2am].[dbo].[ExternalRoleType] WHERE ExternalRoleTypeKey = @PrimaryKey";
        public const string externalroletypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ExternalRoleType] WHERE";
        public const string externalroletypedatamodel_insert = "INSERT INTO [2am].[dbo].[ExternalRoleType] (ExternalRoleTypeKey, Description, ExternalRoleTypeGroupKey) VALUES(@ExternalRoleTypeKey, @Description, @ExternalRoleTypeGroupKey); ";
        public const string externalroletypedatamodel_update = "UPDATE [2am].[dbo].[ExternalRoleType] SET ExternalRoleTypeKey = @ExternalRoleTypeKey, Description = @Description, ExternalRoleTypeGroupKey = @ExternalRoleTypeGroupKey WHERE ExternalRoleTypeKey = @ExternalRoleTypeKey";



        public const string organisationstructureattributedatamodel_selectwhere = "SELECT OrganisationStructureAttributeKey, OrganisationStructureAttributeTypeKey, AttributeValue, OrganisationStructureKey FROM [2am].[dbo].[OrganisationStructureAttribute] WHERE";
        public const string organisationstructureattributedatamodel_selectbykey = "SELECT OrganisationStructureAttributeKey, OrganisationStructureAttributeTypeKey, AttributeValue, OrganisationStructureKey FROM [2am].[dbo].[OrganisationStructureAttribute] WHERE OrganisationStructureAttributeKey = @PrimaryKey";
        public const string organisationstructureattributedatamodel_delete = "DELETE FROM [2am].[dbo].[OrganisationStructureAttribute] WHERE OrganisationStructureAttributeKey = @PrimaryKey";
        public const string organisationstructureattributedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OrganisationStructureAttribute] WHERE";
        public const string organisationstructureattributedatamodel_insert = "INSERT INTO [2am].[dbo].[OrganisationStructureAttribute] (OrganisationStructureAttributeTypeKey, AttributeValue, OrganisationStructureKey) VALUES(@OrganisationStructureAttributeTypeKey, @AttributeValue, @OrganisationStructureKey); select cast(scope_identity() as int)";
        public const string organisationstructureattributedatamodel_update = "UPDATE [2am].[dbo].[OrganisationStructureAttribute] SET OrganisationStructureAttributeTypeKey = @OrganisationStructureAttributeTypeKey, AttributeValue = @AttributeValue, OrganisationStructureKey = @OrganisationStructureKey WHERE OrganisationStructureAttributeKey = @OrganisationStructureAttributeKey";



        public const string importpropertydatamodel_selectwhere = "SELECT PropertyKey, OfferKey, PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, TitleDeedNumber FROM [2am].[dbo].[ImportProperty] WHERE";
        public const string importpropertydatamodel_selectbykey = "SELECT PropertyKey, OfferKey, PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, TitleDeedNumber FROM [2am].[dbo].[ImportProperty] WHERE PropertyKey = @PrimaryKey";
        public const string importpropertydatamodel_delete = "DELETE FROM [2am].[dbo].[ImportProperty] WHERE PropertyKey = @PrimaryKey";
        public const string importpropertydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportProperty] WHERE";
        public const string importpropertydatamodel_insert = "INSERT INTO [2am].[dbo].[ImportProperty] (OfferKey, PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, TitleDeedNumber) VALUES(@OfferKey, @PropertyTypeKey, @TitleTypeKey, @AreaClassificationKey, @OccupancyTypeKey, @PropertyDescription1, @PropertyDescription2, @PropertyDescription3, @DeedsOfficeValue, @CurrentBondDate, @ErfNumber, @ErfPortionNumber, @SectionalSchemeName, @SectionalUnitNumber, @DeedsPropertyTypeKey, @ErfSuburbDescription, @ErfMetroDescription, @TitleDeedNumber); select cast(scope_identity() as int)";
        public const string importpropertydatamodel_update = "UPDATE [2am].[dbo].[ImportProperty] SET OfferKey = @OfferKey, PropertyTypeKey = @PropertyTypeKey, TitleTypeKey = @TitleTypeKey, AreaClassificationKey = @AreaClassificationKey, OccupancyTypeKey = @OccupancyTypeKey, PropertyDescription1 = @PropertyDescription1, PropertyDescription2 = @PropertyDescription2, PropertyDescription3 = @PropertyDescription3, DeedsOfficeValue = @DeedsOfficeValue, CurrentBondDate = @CurrentBondDate, ErfNumber = @ErfNumber, ErfPortionNumber = @ErfPortionNumber, SectionalSchemeName = @SectionalSchemeName, SectionalUnitNumber = @SectionalUnitNumber, DeedsPropertyTypeKey = @DeedsPropertyTypeKey, ErfSuburbDescription = @ErfSuburbDescription, ErfMetroDescription = @ErfMetroDescription, TitleDeedNumber = @TitleDeedNumber WHERE PropertyKey = @PropertyKey";



        public const string disabilitypaymentstatusdatamodel_selectwhere = "SELECT DisabilityPaymentStatusKey, Description FROM [2am].[dbo].[DisabilityPaymentStatus] WHERE";
        public const string disabilitypaymentstatusdatamodel_selectbykey = "SELECT DisabilityPaymentStatusKey, Description FROM [2am].[dbo].[DisabilityPaymentStatus] WHERE DisabilityPaymentStatusKey = @PrimaryKey";
        public const string disabilitypaymentstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[DisabilityPaymentStatus] WHERE DisabilityPaymentStatusKey = @PrimaryKey";
        public const string disabilitypaymentstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisabilityPaymentStatus] WHERE";
        public const string disabilitypaymentstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[DisabilityPaymentStatus] (DisabilityPaymentStatusKey, Description) VALUES(@DisabilityPaymentStatusKey, @Description); ";
        public const string disabilitypaymentstatusdatamodel_update = "UPDATE [2am].[dbo].[DisabilityPaymentStatus] SET DisabilityPaymentStatusKey = @DisabilityPaymentStatusKey, Description = @Description WHERE DisabilityPaymentStatusKey = @DisabilityPaymentStatusKey";



        public const string productconditiontypedatamodel_selectwhere = "SELECT ProductConditionTypeKey, Description FROM [2am].[dbo].[ProductConditionType] WHERE";
        public const string productconditiontypedatamodel_selectbykey = "SELECT ProductConditionTypeKey, Description FROM [2am].[dbo].[ProductConditionType] WHERE ProductConditionTypeKey = @PrimaryKey";
        public const string productconditiontypedatamodel_delete = "DELETE FROM [2am].[dbo].[ProductConditionType] WHERE ProductConditionTypeKey = @PrimaryKey";
        public const string productconditiontypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ProductConditionType] WHERE";
        public const string productconditiontypedatamodel_insert = "INSERT INTO [2am].[dbo].[ProductConditionType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string productconditiontypedatamodel_update = "UPDATE [2am].[dbo].[ProductConditionType] SET Description = @Description WHERE ProductConditionTypeKey = @ProductConditionTypeKey";



        public const string persalorganisationcodesdatamodel_selectwhere = "SELECT PersalOrganisationKey, PersalOrganisationName FROM [2am].[dbo].[PersalOrganisationCodes] WHERE";
        public const string persalorganisationcodesdatamodel_selectbykey = "SELECT PersalOrganisationKey, PersalOrganisationName FROM [2am].[dbo].[PersalOrganisationCodes] WHERE PersalOrganisationKey = @PrimaryKey";
        public const string persalorganisationcodesdatamodel_delete = "DELETE FROM [2am].[dbo].[PersalOrganisationCodes] WHERE PersalOrganisationKey = @PrimaryKey";
        public const string persalorganisationcodesdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PersalOrganisationCodes] WHERE";
        public const string persalorganisationcodesdatamodel_insert = "INSERT INTO [2am].[dbo].[PersalOrganisationCodes] (PersalOrganisationKey, PersalOrganisationName) VALUES(@PersalOrganisationKey, @PersalOrganisationName); ";
        public const string persalorganisationcodesdatamodel_update = "UPDATE [2am].[dbo].[PersalOrganisationCodes] SET PersalOrganisationKey = @PersalOrganisationKey, PersalOrganisationName = @PersalOrganisationName WHERE PersalOrganisationKey = @PersalOrganisationKey";



        public const string offerinternetreferrerdatamodel_selectwhere = "SELECT OfferInternetReferrerKey, OfferKey, UserURL, ReferringServerURL, Parameters FROM [2am].[dbo].[OfferInternetReferrer] WHERE";
        public const string offerinternetreferrerdatamodel_selectbykey = "SELECT OfferInternetReferrerKey, OfferKey, UserURL, ReferringServerURL, Parameters FROM [2am].[dbo].[OfferInternetReferrer] WHERE OfferInternetReferrerKey = @PrimaryKey";
        public const string offerinternetreferrerdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInternetReferrer] WHERE OfferInternetReferrerKey = @PrimaryKey";
        public const string offerinternetreferrerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInternetReferrer] WHERE";
        public const string offerinternetreferrerdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInternetReferrer] (OfferKey, UserURL, ReferringServerURL, Parameters) VALUES(@OfferKey, @UserURL, @ReferringServerURL, @Parameters); select cast(scope_identity() as int)";
        public const string offerinternetreferrerdatamodel_update = "UPDATE [2am].[dbo].[OfferInternetReferrer] SET OfferKey = @OfferKey, UserURL = @UserURL, ReferringServerURL = @ReferringServerURL, Parameters = @Parameters WHERE OfferInternetReferrerKey = @OfferInternetReferrerKey";



        public const string batchloantransactiondatamodel_selectwhere = "SELECT BatchLoanTransactionKey, BatchTransactionKey, LoanTransactionNumber FROM [2am].[dbo].[BatchLoanTransaction] WHERE";
        public const string batchloantransactiondatamodel_selectbykey = "SELECT BatchLoanTransactionKey, BatchTransactionKey, LoanTransactionNumber FROM [2am].[dbo].[BatchLoanTransaction] WHERE BatchLoanTransactionKey = @PrimaryKey";
        public const string batchloantransactiondatamodel_delete = "DELETE FROM [2am].[dbo].[BatchLoanTransaction] WHERE BatchLoanTransactionKey = @PrimaryKey";
        public const string batchloantransactiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchLoanTransaction] WHERE";
        public const string batchloantransactiondatamodel_insert = "INSERT INTO [2am].[dbo].[BatchLoanTransaction] (BatchTransactionKey, LoanTransactionNumber) VALUES(@BatchTransactionKey, @LoanTransactionNumber); select cast(scope_identity() as int)";
        public const string batchloantransactiondatamodel_update = "UPDATE [2am].[dbo].[BatchLoanTransaction] SET BatchTransactionKey = @BatchTransactionKey, LoanTransactionNumber = @LoanTransactionNumber WHERE BatchLoanTransactionKey = @BatchLoanTransactionKey";



        public const string cdvdatamodel_selectwhere = "SELECT CDVKey, ACBBankCode, ACBBranchCode, ACBTypeNumber, StreamCode, ExceptionStreamCode, Weightings, Modulus, FudgeFactor, ExceptionCode, AccountIndicator, UserID, DateChange FROM [2am].[dbo].[CDV] WHERE";
        public const string cdvdatamodel_selectbykey = "SELECT CDVKey, ACBBankCode, ACBBranchCode, ACBTypeNumber, StreamCode, ExceptionStreamCode, Weightings, Modulus, FudgeFactor, ExceptionCode, AccountIndicator, UserID, DateChange FROM [2am].[dbo].[CDV] WHERE CDVKey = @PrimaryKey";
        public const string cdvdatamodel_delete = "DELETE FROM [2am].[dbo].[CDV] WHERE CDVKey = @PrimaryKey";
        public const string cdvdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CDV] WHERE";
        public const string cdvdatamodel_insert = "INSERT INTO [2am].[dbo].[CDV] (ACBBankCode, ACBBranchCode, ACBTypeNumber, StreamCode, ExceptionStreamCode, Weightings, Modulus, FudgeFactor, ExceptionCode, AccountIndicator, UserID, DateChange) VALUES(@ACBBankCode, @ACBBranchCode, @ACBTypeNumber, @StreamCode, @ExceptionStreamCode, @Weightings, @Modulus, @FudgeFactor, @ExceptionCode, @AccountIndicator, @UserID, @DateChange); select cast(scope_identity() as int)";
        public const string cdvdatamodel_update = "UPDATE [2am].[dbo].[CDV] SET ACBBankCode = @ACBBankCode, ACBBranchCode = @ACBBranchCode, ACBTypeNumber = @ACBTypeNumber, StreamCode = @StreamCode, ExceptionStreamCode = @ExceptionStreamCode, Weightings = @Weightings, Modulus = @Modulus, FudgeFactor = @FudgeFactor, ExceptionCode = @ExceptionCode, AccountIndicator = @AccountIndicator, UserID = @UserID, DateChange = @DateChange WHERE CDVKey = @CDVKey";



        public const string auditmargindatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MarginKey, Value, Description FROM [2am].[dbo].[AuditMargin] WHERE";
        public const string auditmargindatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MarginKey, Value, Description FROM [2am].[dbo].[AuditMargin] WHERE AuditNumber = @PrimaryKey";
        public const string auditmargindatamodel_delete = "DELETE FROM [2am].[dbo].[AuditMargin] WHERE AuditNumber = @PrimaryKey";
        public const string auditmargindatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditMargin] WHERE";
        public const string auditmargindatamodel_insert = "INSERT INTO [2am].[dbo].[AuditMargin] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MarginKey, Value, Description) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @MarginKey, @Value, @Description); select cast(scope_identity() as int)";
        public const string auditmargindatamodel_update = "UPDATE [2am].[dbo].[AuditMargin] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, MarginKey = @MarginKey, Value = @Value, Description = @Description WHERE AuditNumber = @AuditNumber";



        public const string roundrobinpointerdatamodel_selectwhere = "SELECT RoundRobinPointerKey, RoundRobinPointerIndexID, Description, GeneralStatusKey FROM [2am].[dbo].[RoundRobinPointer] WHERE";
        public const string roundrobinpointerdatamodel_selectbykey = "SELECT RoundRobinPointerKey, RoundRobinPointerIndexID, Description, GeneralStatusKey FROM [2am].[dbo].[RoundRobinPointer] WHERE RoundRobinPointerKey = @PrimaryKey";
        public const string roundrobinpointerdatamodel_delete = "DELETE FROM [2am].[dbo].[RoundRobinPointer] WHERE RoundRobinPointerKey = @PrimaryKey";
        public const string roundrobinpointerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RoundRobinPointer] WHERE";
        public const string roundrobinpointerdatamodel_insert = "INSERT INTO [2am].[dbo].[RoundRobinPointer] (RoundRobinPointerKey, RoundRobinPointerIndexID, Description, GeneralStatusKey) VALUES(@RoundRobinPointerKey, @RoundRobinPointerIndexID, @Description, @GeneralStatusKey); ";
        public const string roundrobinpointerdatamodel_update = "UPDATE [2am].[dbo].[RoundRobinPointer] SET RoundRobinPointerKey = @RoundRobinPointerKey, RoundRobinPointerIndexID = @RoundRobinPointerIndexID, Description = @Description, GeneralStatusKey = @GeneralStatusKey WHERE RoundRobinPointerKey = @RoundRobinPointerKey";



        public const string languagedatamodel_selectwhere = "SELECT LanguageKey, Description, Translatable FROM [2am].[dbo].[Language] WHERE";
        public const string languagedatamodel_selectbykey = "SELECT LanguageKey, Description, Translatable FROM [2am].[dbo].[Language] WHERE LanguageKey = @PrimaryKey";
        public const string languagedatamodel_delete = "DELETE FROM [2am].[dbo].[Language] WHERE LanguageKey = @PrimaryKey";
        public const string languagedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Language] WHERE";
        public const string languagedatamodel_insert = "INSERT INTO [2am].[dbo].[Language] (LanguageKey, Description, Translatable) VALUES(@LanguageKey, @Description, @Translatable); ";
        public const string languagedatamodel_update = "UPDATE [2am].[dbo].[Language] SET LanguageKey = @LanguageKey, Description = @Description, Translatable = @Translatable WHERE LanguageKey = @LanguageKey";



        public const string financialservicegroupdatamodel_selectwhere = "SELECT FinancialServiceGroupKey, Description FROM [2am].[dbo].[FinancialServiceGroup] WHERE";
        public const string financialservicegroupdatamodel_selectbykey = "SELECT FinancialServiceGroupKey, Description FROM [2am].[dbo].[FinancialServiceGroup] WHERE FinancialServiceGroupKey = @PrimaryKey";
        public const string financialservicegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[FinancialServiceGroup] WHERE FinancialServiceGroupKey = @PrimaryKey";
        public const string financialservicegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FinancialServiceGroup] WHERE";
        public const string financialservicegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[FinancialServiceGroup] (FinancialServiceGroupKey, Description) VALUES(@FinancialServiceGroupKey, @Description); ";
        public const string financialservicegroupdatamodel_update = "UPDATE [2am].[dbo].[FinancialServiceGroup] SET FinancialServiceGroupKey = @FinancialServiceGroupKey, Description = @Description WHERE FinancialServiceGroupKey = @FinancialServiceGroupKey";



        public const string employmentconfirmationsourcedatamodel_selectwhere = "SELECT EmploymentConfirmationSourceKey, Description, GeneralStatusKey FROM [2am].[dbo].[EmploymentConfirmationSource] WHERE";
        public const string employmentconfirmationsourcedatamodel_selectbykey = "SELECT EmploymentConfirmationSourceKey, Description, GeneralStatusKey FROM [2am].[dbo].[EmploymentConfirmationSource] WHERE EmploymentConfirmationSourceKey = @PrimaryKey";
        public const string employmentconfirmationsourcedatamodel_delete = "DELETE FROM [2am].[dbo].[EmploymentConfirmationSource] WHERE EmploymentConfirmationSourceKey = @PrimaryKey";
        public const string employmentconfirmationsourcedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[EmploymentConfirmationSource] WHERE";
        public const string employmentconfirmationsourcedatamodel_insert = "INSERT INTO [2am].[dbo].[EmploymentConfirmationSource] (EmploymentConfirmationSourceKey, Description, GeneralStatusKey) VALUES(@EmploymentConfirmationSourceKey, @Description, @GeneralStatusKey); ";
        public const string employmentconfirmationsourcedatamodel_update = "UPDATE [2am].[dbo].[EmploymentConfirmationSource] SET EmploymentConfirmationSourceKey = @EmploymentConfirmationSourceKey, Description = @Description, GeneralStatusKey = @GeneralStatusKey WHERE EmploymentConfirmationSourceKey = @EmploymentConfirmationSourceKey";



        public const string lifepremiumforecastdatamodel_selectwhere = "SELECT LifePremiumForecastKey, LoanYear, Age, SumAssured, MonthlyPremium, MonthlyComm, EntryDate, AccountKey, YearlyPremium, IPBPremium FROM [2am].[dbo].[LifePremiumForecast] WHERE";
        public const string lifepremiumforecastdatamodel_selectbykey = "SELECT LifePremiumForecastKey, LoanYear, Age, SumAssured, MonthlyPremium, MonthlyComm, EntryDate, AccountKey, YearlyPremium, IPBPremium FROM [2am].[dbo].[LifePremiumForecast] WHERE LifePremiumForecastKey = @PrimaryKey";
        public const string lifepremiumforecastdatamodel_delete = "DELETE FROM [2am].[dbo].[LifePremiumForecast] WHERE LifePremiumForecastKey = @PrimaryKey";
        public const string lifepremiumforecastdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifePremiumForecast] WHERE";
        public const string lifepremiumforecastdatamodel_insert = "INSERT INTO [2am].[dbo].[LifePremiumForecast] (LoanYear, Age, SumAssured, MonthlyPremium, MonthlyComm, EntryDate, AccountKey, YearlyPremium, IPBPremium) VALUES(@LoanYear, @Age, @SumAssured, @MonthlyPremium, @MonthlyComm, @EntryDate, @AccountKey, @YearlyPremium, @IPBPremium); select cast(scope_identity() as int)";
        public const string lifepremiumforecastdatamodel_update = "UPDATE [2am].[dbo].[LifePremiumForecast] SET LoanYear = @LoanYear, Age = @Age, SumAssured = @SumAssured, MonthlyPremium = @MonthlyPremium, MonthlyComm = @MonthlyComm, EntryDate = @EntryDate, AccountKey = @AccountKey, YearlyPremium = @YearlyPremium, IPBPremium = @IPBPremium WHERE LifePremiumForecastKey = @LifePremiumForecastKey";



        public const string transactiontypedataaccessdatamodel_selectwhere = "SELECT TransactionTypeDataAccessKey, ADCredentials, TransactionTypeKey FROM [2am].[dbo].[TransactionTypeDataAccess] WHERE";
        public const string transactiontypedataaccessdatamodel_selectbykey = "SELECT TransactionTypeDataAccessKey, ADCredentials, TransactionTypeKey FROM [2am].[dbo].[TransactionTypeDataAccess] WHERE TransactionTypeDataAccessKey = @PrimaryKey";
        public const string transactiontypedataaccessdatamodel_delete = "DELETE FROM [2am].[dbo].[TransactionTypeDataAccess] WHERE TransactionTypeDataAccessKey = @PrimaryKey";
        public const string transactiontypedataaccessdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TransactionTypeDataAccess] WHERE";
        public const string transactiontypedataaccessdatamodel_insert = "INSERT INTO [2am].[dbo].[TransactionTypeDataAccess] (ADCredentials, TransactionTypeKey) VALUES(@ADCredentials, @TransactionTypeKey); select cast(scope_identity() as int)";
        public const string transactiontypedataaccessdatamodel_update = "UPDATE [2am].[dbo].[TransactionTypeDataAccess] SET ADCredentials = @ADCredentials, TransactionTypeKey = @TransactionTypeKey WHERE TransactionTypeDataAccessKey = @TransactionTypeDataAccessKey";



        public const string capofferdatamodel_selectwhere = "SELECT CapOfferKey, AccountKey, CapTypeConfigurationKey, RemainingInstallments, CurrentBalance, CurrentInstallment, LinkRate, CapStatusKey, OfferDate, Promotion, BrokerKey, CapitalisationDate, ChangeDate, CAPPaymentOptionKey, UserID FROM [2am].[dbo].[CapOffer] WHERE";
        public const string capofferdatamodel_selectbykey = "SELECT CapOfferKey, AccountKey, CapTypeConfigurationKey, RemainingInstallments, CurrentBalance, CurrentInstallment, LinkRate, CapStatusKey, OfferDate, Promotion, BrokerKey, CapitalisationDate, ChangeDate, CAPPaymentOptionKey, UserID FROM [2am].[dbo].[CapOffer] WHERE CapOfferKey = @PrimaryKey";
        public const string capofferdatamodel_delete = "DELETE FROM [2am].[dbo].[CapOffer] WHERE CapOfferKey = @PrimaryKey";
        public const string capofferdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CapOffer] WHERE";
        public const string capofferdatamodel_insert = "INSERT INTO [2am].[dbo].[CapOffer] (AccountKey, CapTypeConfigurationKey, RemainingInstallments, CurrentBalance, CurrentInstallment, LinkRate, CapStatusKey, OfferDate, Promotion, BrokerKey, CapitalisationDate, ChangeDate, CAPPaymentOptionKey, UserID) VALUES(@AccountKey, @CapTypeConfigurationKey, @RemainingInstallments, @CurrentBalance, @CurrentInstallment, @LinkRate, @CapStatusKey, @OfferDate, @Promotion, @BrokerKey, @CapitalisationDate, @ChangeDate, @CAPPaymentOptionKey, @UserID); select cast(scope_identity() as int)";
        public const string capofferdatamodel_update = "UPDATE [2am].[dbo].[CapOffer] SET AccountKey = @AccountKey, CapTypeConfigurationKey = @CapTypeConfigurationKey, RemainingInstallments = @RemainingInstallments, CurrentBalance = @CurrentBalance, CurrentInstallment = @CurrentInstallment, LinkRate = @LinkRate, CapStatusKey = @CapStatusKey, OfferDate = @OfferDate, Promotion = @Promotion, BrokerKey = @BrokerKey, CapitalisationDate = @CapitalisationDate, ChangeDate = @ChangeDate, CAPPaymentOptionKey = @CAPPaymentOptionKey, UserID = @UserID WHERE CapOfferKey = @CapOfferKey";



        public const string importofferinformationdatamodel_selectwhere = "SELECT OfferInformationKey, OfferKey, OfferTerm, CashDeposit, PropertyValuation, FeesTotal, InterimInterest, MonthlyInstalment, HOCPremium, LifePremium, PreApprovedAmount, MaxCashAllowed, MaxQuickCashAllowed, RequestedQuickCashAmount, BondToRegister, LTV, PTI FROM [2am].[dbo].[ImportOfferInformation] WHERE";
        public const string importofferinformationdatamodel_selectbykey = "SELECT OfferInformationKey, OfferKey, OfferTerm, CashDeposit, PropertyValuation, FeesTotal, InterimInterest, MonthlyInstalment, HOCPremium, LifePremium, PreApprovedAmount, MaxCashAllowed, MaxQuickCashAllowed, RequestedQuickCashAmount, BondToRegister, LTV, PTI FROM [2am].[dbo].[ImportOfferInformation] WHERE OfferInformationKey = @PrimaryKey";
        public const string importofferinformationdatamodel_delete = "DELETE FROM [2am].[dbo].[ImportOfferInformation] WHERE OfferInformationKey = @PrimaryKey";
        public const string importofferinformationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportOfferInformation] WHERE";
        public const string importofferinformationdatamodel_insert = "INSERT INTO [2am].[dbo].[ImportOfferInformation] (OfferKey, OfferTerm, CashDeposit, PropertyValuation, FeesTotal, InterimInterest, MonthlyInstalment, HOCPremium, LifePremium, PreApprovedAmount, MaxCashAllowed, MaxQuickCashAllowed, RequestedQuickCashAmount, BondToRegister, LTV, PTI) VALUES(@OfferKey, @OfferTerm, @CashDeposit, @PropertyValuation, @FeesTotal, @InterimInterest, @MonthlyInstalment, @HOCPremium, @LifePremium, @PreApprovedAmount, @MaxCashAllowed, @MaxQuickCashAllowed, @RequestedQuickCashAmount, @BondToRegister, @LTV, @PTI); select cast(scope_identity() as int)";
        public const string importofferinformationdatamodel_update = "UPDATE [2am].[dbo].[ImportOfferInformation] SET OfferKey = @OfferKey, OfferTerm = @OfferTerm, CashDeposit = @CashDeposit, PropertyValuation = @PropertyValuation, FeesTotal = @FeesTotal, InterimInterest = @InterimInterest, MonthlyInstalment = @MonthlyInstalment, HOCPremium = @HOCPremium, LifePremium = @LifePremium, PreApprovedAmount = @PreApprovedAmount, MaxCashAllowed = @MaxCashAllowed, MaxQuickCashAllowed = @MaxQuickCashAllowed, RequestedQuickCashAmount = @RequestedQuickCashAmount, BondToRegister = @BondToRegister, LTV = @LTV, PTI = @PTI WHERE OfferInformationKey = @OfferInformationKey";



        public const string accountinformationtypedatamodel_selectwhere = "SELECT AccountInformationTypeKey, Description FROM [2am].[dbo].[AccountInformationType] WHERE";
        public const string accountinformationtypedatamodel_selectbykey = "SELECT AccountInformationTypeKey, Description FROM [2am].[dbo].[AccountInformationType] WHERE AccountInformationTypeKey = @PrimaryKey";
        public const string accountinformationtypedatamodel_delete = "DELETE FROM [2am].[dbo].[AccountInformationType] WHERE AccountInformationTypeKey = @PrimaryKey";
        public const string accountinformationtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountInformationType] WHERE";
        public const string accountinformationtypedatamodel_insert = "INSERT INTO [2am].[dbo].[AccountInformationType] (AccountInformationTypeKey, Description) VALUES(@AccountInformationTypeKey, @Description); ";
        public const string accountinformationtypedatamodel_update = "UPDATE [2am].[dbo].[AccountInformationType] SET AccountInformationTypeKey = @AccountInformationTypeKey, Description = @Description WHERE AccountInformationTypeKey = @AccountInformationTypeKey";



        public const string auditmortgageloaninfodatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MortgageLoanInfoKey, FinancialServiceKey, ElectionDate, ConvertedDate, AccumulatedLoyaltyBenefit, NextPaymentDate, DiscountRate, PPThresholdYr1, PPThresholdYr2, PPThresholdYr3, PPThresholdYr4, PPThresholdYr5, MTDLoyaltyBenefit, PPAllowed, GeneralStatusKey, Exclusion, ExclusionEndDate, ExclusionReason, OverPaymentAmount FROM [2am].[dbo].[AuditMortgageLoanInfo] WHERE";
        public const string auditmortgageloaninfodatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MortgageLoanInfoKey, FinancialServiceKey, ElectionDate, ConvertedDate, AccumulatedLoyaltyBenefit, NextPaymentDate, DiscountRate, PPThresholdYr1, PPThresholdYr2, PPThresholdYr3, PPThresholdYr4, PPThresholdYr5, MTDLoyaltyBenefit, PPAllowed, GeneralStatusKey, Exclusion, ExclusionEndDate, ExclusionReason, OverPaymentAmount FROM [2am].[dbo].[AuditMortgageLoanInfo] WHERE AuditNumber = @PrimaryKey";
        public const string auditmortgageloaninfodatamodel_delete = "DELETE FROM [2am].[dbo].[AuditMortgageLoanInfo] WHERE AuditNumber = @PrimaryKey";
        public const string auditmortgageloaninfodatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditMortgageLoanInfo] WHERE";
        public const string auditmortgageloaninfodatamodel_insert = "INSERT INTO [2am].[dbo].[AuditMortgageLoanInfo] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, MortgageLoanInfoKey, FinancialServiceKey, ElectionDate, ConvertedDate, AccumulatedLoyaltyBenefit, NextPaymentDate, DiscountRate, PPThresholdYr1, PPThresholdYr2, PPThresholdYr3, PPThresholdYr4, PPThresholdYr5, MTDLoyaltyBenefit, PPAllowed, GeneralStatusKey, Exclusion, ExclusionEndDate, ExclusionReason, OverPaymentAmount) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @MortgageLoanInfoKey, @FinancialServiceKey, @ElectionDate, @ConvertedDate, @AccumulatedLoyaltyBenefit, @NextPaymentDate, @DiscountRate, @PPThresholdYr1, @PPThresholdYr2, @PPThresholdYr3, @PPThresholdYr4, @PPThresholdYr5, @MTDLoyaltyBenefit, @PPAllowed, @GeneralStatusKey, @Exclusion, @ExclusionEndDate, @ExclusionReason, @OverPaymentAmount); select cast(scope_identity() as int)";
        public const string auditmortgageloaninfodatamodel_update = "UPDATE [2am].[dbo].[AuditMortgageLoanInfo] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, MortgageLoanInfoKey = @MortgageLoanInfoKey, FinancialServiceKey = @FinancialServiceKey, ElectionDate = @ElectionDate, ConvertedDate = @ConvertedDate, AccumulatedLoyaltyBenefit = @AccumulatedLoyaltyBenefit, NextPaymentDate = @NextPaymentDate, DiscountRate = @DiscountRate, PPThresholdYr1 = @PPThresholdYr1, PPThresholdYr2 = @PPThresholdYr2, PPThresholdYr3 = @PPThresholdYr3, PPThresholdYr4 = @PPThresholdYr4, PPThresholdYr5 = @PPThresholdYr5, MTDLoyaltyBenefit = @MTDLoyaltyBenefit, PPAllowed = @PPAllowed, GeneralStatusKey = @GeneralStatusKey, Exclusion = @Exclusion, ExclusionEndDate = @ExclusionEndDate, ExclusionReason = @ExclusionReason, OverPaymentAmount = @OverPaymentAmount WHERE AuditNumber = @AuditNumber";



        public const string reportparametertypedatamodel_selectwhere = "SELECT ReportParameterTypeKey, Description FROM [2am].[dbo].[ReportParameterType] WHERE";
        public const string reportparametertypedatamodel_selectbykey = "SELECT ReportParameterTypeKey, Description FROM [2am].[dbo].[ReportParameterType] WHERE ReportParameterTypeKey = @PrimaryKey";
        public const string reportparametertypedatamodel_delete = "DELETE FROM [2am].[dbo].[ReportParameterType] WHERE ReportParameterTypeKey = @PrimaryKey";
        public const string reportparametertypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReportParameterType] WHERE";
        public const string reportparametertypedatamodel_insert = "INSERT INTO [2am].[dbo].[ReportParameterType] (ReportParameterTypeKey, Description) VALUES(@ReportParameterTypeKey, @Description); ";
        public const string reportparametertypedatamodel_update = "UPDATE [2am].[dbo].[ReportParameterType] SET ReportParameterTypeKey = @ReportParameterTypeKey, Description = @Description WHERE ReportParameterTypeKey = @ReportParameterTypeKey";



        public const string batchamounts_tempdatamodel_selectwhere = "SELECT DateToday, FinancialServiceKey, InstallmentAmount, FixedDebitOrderAmount, HOC, ProRataHOC, Regent, Life, Other, CurrentBalance, UnderCancellation, LoanOpendate, BadBankDetail FROM [2am].[dbo].[BatchAmounts_Temp] WHERE";
        public const string batchamounts_tempdatamodel_selectbykey = "SELECT DateToday, FinancialServiceKey, InstallmentAmount, FixedDebitOrderAmount, HOC, ProRataHOC, Regent, Life, Other, CurrentBalance, UnderCancellation, LoanOpendate, BadBankDetail FROM [2am].[dbo].[BatchAmounts_Temp] WHERE  = @PrimaryKey";
        public const string batchamounts_tempdatamodel_delete = "DELETE FROM [2am].[dbo].[BatchAmounts_Temp] WHERE  = @PrimaryKey";
        public const string batchamounts_tempdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchAmounts_Temp] WHERE";
        public const string batchamounts_tempdatamodel_insert = "INSERT INTO [2am].[dbo].[BatchAmounts_Temp] (DateToday, FinancialServiceKey, InstallmentAmount, FixedDebitOrderAmount, HOC, ProRataHOC, Regent, Life, Other, CurrentBalance, UnderCancellation, LoanOpendate, BadBankDetail) VALUES(@DateToday, @FinancialServiceKey, @InstallmentAmount, @FixedDebitOrderAmount, @HOC, @ProRataHOC, @Regent, @Life, @Other, @CurrentBalance, @UnderCancellation, @LoanOpendate, @BadBankDetail); ";
        public const string batchamounts_tempdatamodel_update = "UPDATE [2am].[dbo].[BatchAmounts_Temp] SET DateToday = @DateToday, FinancialServiceKey = @FinancialServiceKey, InstallmentAmount = @InstallmentAmount, FixedDebitOrderAmount = @FixedDebitOrderAmount, HOC = @HOC, ProRataHOC = @ProRataHOC, Regent = @Regent, Life = @Life, Other = @Other, CurrentBalance = @CurrentBalance, UnderCancellation = @UnderCancellation, LoanOpendate = @LoanOpendate, BadBankDetail = @BadBankDetail WHERE  = @";



        public const string controldatamodel_selectwhere = "SELECT ControlNumber, ControlDescription, ControlNumeric, ControlText, ControlGroupKey FROM [2am].[dbo].[Control] WHERE";
        public const string controldatamodel_selectbykey = "SELECT ControlNumber, ControlDescription, ControlNumeric, ControlText, ControlGroupKey FROM [2am].[dbo].[Control] WHERE ControlNumber = @PrimaryKey";
        public const string controldatamodel_delete = "DELETE FROM [2am].[dbo].[Control] WHERE ControlNumber = @PrimaryKey";
        public const string controldatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Control] WHERE";
        public const string controldatamodel_insert = "INSERT INTO [2am].[dbo].[Control] (ControlDescription, ControlNumeric, ControlText, ControlGroupKey) VALUES(@ControlDescription, @ControlNumeric, @ControlText, @ControlGroupKey); select cast(scope_identity() as int)";
        public const string controldatamodel_update = "UPDATE [2am].[dbo].[Control] SET ControlDescription = @ControlDescription, ControlNumeric = @ControlNumeric, ControlText = @ControlText, ControlGroupKey = @ControlGroupKey WHERE ControlNumber = @ControlNumber";



        public const string lifepolicytypedatamodel_selectwhere = "SELECT LifePolicyTypeKey, Description FROM [2am].[dbo].[LifePolicyType] WHERE";
        public const string lifepolicytypedatamodel_selectbykey = "SELECT LifePolicyTypeKey, Description FROM [2am].[dbo].[LifePolicyType] WHERE LifePolicyTypeKey = @PrimaryKey";
        public const string lifepolicytypedatamodel_delete = "DELETE FROM [2am].[dbo].[LifePolicyType] WHERE LifePolicyTypeKey = @PrimaryKey";
        public const string lifepolicytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifePolicyType] WHERE";
        public const string lifepolicytypedatamodel_insert = "INSERT INTO [2am].[dbo].[LifePolicyType] (LifePolicyTypeKey, Description) VALUES(@LifePolicyTypeKey, @Description); ";
        public const string lifepolicytypedatamodel_update = "UPDATE [2am].[dbo].[LifePolicyType] SET LifePolicyTypeKey = @LifePolicyTypeKey, Description = @Description WHERE LifePolicyTypeKey = @LifePolicyTypeKey";



        public const string offermortgageloandatamodel_selectwhere = "SELECT OfferKey, OfferAmount, MortgageLoanPurposeKey, ApplicantTypeKey, NumApplicants, HomePurchaseDate, BondRegistrationDate, CurrentBondValue, DeedsOfficeDate, BondFinancialInstitution, PurchasePrice, ResetConfigurationKey, TransferringAttorney, ClientEstimatePropertyValuation, PropertyKey, DependentsPerHousehold, ContributingDependents, DocumentLanguageKey FROM [2am].[dbo].[OfferMortgageLoan] WHERE";
        public const string offermortgageloandatamodel_selectbykey = "SELECT OfferKey, OfferAmount, MortgageLoanPurposeKey, ApplicantTypeKey, NumApplicants, HomePurchaseDate, BondRegistrationDate, CurrentBondValue, DeedsOfficeDate, BondFinancialInstitution, PurchasePrice, ResetConfigurationKey, TransferringAttorney, ClientEstimatePropertyValuation, PropertyKey, DependentsPerHousehold, ContributingDependents, DocumentLanguageKey FROM [2am].[dbo].[OfferMortgageLoan] WHERE OfferKey = @PrimaryKey";
        public const string offermortgageloandatamodel_delete = "DELETE FROM [2am].[dbo].[OfferMortgageLoan] WHERE OfferKey = @PrimaryKey";
        public const string offermortgageloandatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferMortgageLoan] WHERE";
        public const string offermortgageloandatamodel_insert = "INSERT INTO [2am].[dbo].[OfferMortgageLoan] (OfferKey, OfferAmount, MortgageLoanPurposeKey, ApplicantTypeKey, NumApplicants, HomePurchaseDate, BondRegistrationDate, CurrentBondValue, DeedsOfficeDate, BondFinancialInstitution, PurchasePrice, ResetConfigurationKey, TransferringAttorney, ClientEstimatePropertyValuation, PropertyKey, DependentsPerHousehold, ContributingDependents, DocumentLanguageKey) VALUES(@OfferKey, @OfferAmount, @MortgageLoanPurposeKey, @ApplicantTypeKey, @NumApplicants, @HomePurchaseDate, @BondRegistrationDate, @CurrentBondValue, @DeedsOfficeDate, @BondFinancialInstitution, @PurchasePrice, @ResetConfigurationKey, @TransferringAttorney, @ClientEstimatePropertyValuation, @PropertyKey, @DependentsPerHousehold, @ContributingDependents, @DocumentLanguageKey); ";
        public const string offermortgageloandatamodel_update = "UPDATE [2am].[dbo].[OfferMortgageLoan] SET OfferKey = @OfferKey, OfferAmount = @OfferAmount, MortgageLoanPurposeKey = @MortgageLoanPurposeKey, ApplicantTypeKey = @ApplicantTypeKey, NumApplicants = @NumApplicants, HomePurchaseDate = @HomePurchaseDate, BondRegistrationDate = @BondRegistrationDate, CurrentBondValue = @CurrentBondValue, DeedsOfficeDate = @DeedsOfficeDate, BondFinancialInstitution = @BondFinancialInstitution, PurchasePrice = @PurchasePrice, ResetConfigurationKey = @ResetConfigurationKey, TransferringAttorney = @TransferringAttorney, ClientEstimatePropertyValuation = @ClientEstimatePropertyValuation, PropertyKey = @PropertyKey, DependentsPerHousehold = @DependentsPerHousehold, ContributingDependents = @ContributingDependents, DocumentLanguageKey = @DocumentLanguageKey WHERE OfferKey = @OfferKey";



        public const string affordabilityassessmentstatusdatamodel_selectwhere = "SELECT AffordabilityAssessmentStatusKey, Description FROM [2am].[dbo].[AffordabilityAssessmentStatus] WHERE";
        public const string affordabilityassessmentstatusdatamodel_selectbykey = "SELECT AffordabilityAssessmentStatusKey, Description FROM [2am].[dbo].[AffordabilityAssessmentStatus] WHERE AffordabilityAssessmentStatusKey = @PrimaryKey";
        public const string affordabilityassessmentstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentStatus] WHERE AffordabilityAssessmentStatusKey = @PrimaryKey";
        public const string affordabilityassessmentstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentStatus] WHERE";
        public const string affordabilityassessmentstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityAssessmentStatus] (AffordabilityAssessmentStatusKey, Description) VALUES(@AffordabilityAssessmentStatusKey, @Description); ";
        public const string affordabilityassessmentstatusdatamodel_update = "UPDATE [2am].[dbo].[AffordabilityAssessmentStatus] SET AffordabilityAssessmentStatusKey = @AffordabilityAssessmentStatusKey, Description = @Description WHERE AffordabilityAssessmentStatusKey = @AffordabilityAssessmentStatusKey";



        public const string generickeytypedatamodel_selectwhere = "SELECT GenericKeyTypeKey, Description, TableName, PrimaryKeyColumn FROM [2am].[dbo].[GenericKeyType] WHERE";
        public const string generickeytypedatamodel_selectbykey = "SELECT GenericKeyTypeKey, Description, TableName, PrimaryKeyColumn FROM [2am].[dbo].[GenericKeyType] WHERE GenericKeyTypeKey = @PrimaryKey";
        public const string generickeytypedatamodel_delete = "DELETE FROM [2am].[dbo].[GenericKeyType] WHERE GenericKeyTypeKey = @PrimaryKey";
        public const string generickeytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[GenericKeyType] WHERE";
        public const string generickeytypedatamodel_insert = "INSERT INTO [2am].[dbo].[GenericKeyType] (GenericKeyTypeKey, Description, TableName, PrimaryKeyColumn) VALUES(@GenericKeyTypeKey, @Description, @TableName, @PrimaryKeyColumn); ";
        public const string generickeytypedatamodel_update = "UPDATE [2am].[dbo].[GenericKeyType] SET GenericKeyTypeKey = @GenericKeyTypeKey, Description = @Description, TableName = @TableName, PrimaryKeyColumn = @PrimaryKeyColumn WHERE GenericKeyTypeKey = @GenericKeyTypeKey";



        public const string salutationtypedatamodel_selectwhere = "SELECT SalutationKey, Description, TranslatableItemKey FROM [2am].[dbo].[SalutationType] WHERE";
        public const string salutationtypedatamodel_selectbykey = "SELECT SalutationKey, Description, TranslatableItemKey FROM [2am].[dbo].[SalutationType] WHERE SalutationKey = @PrimaryKey";
        public const string salutationtypedatamodel_delete = "DELETE FROM [2am].[dbo].[SalutationType] WHERE SalutationKey = @PrimaryKey";
        public const string salutationtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[SalutationType] WHERE";
        public const string salutationtypedatamodel_insert = "INSERT INTO [2am].[dbo].[SalutationType] (SalutationKey, Description, TranslatableItemKey) VALUES(@SalutationKey, @Description, @TranslatableItemKey); ";
        public const string salutationtypedatamodel_update = "UPDATE [2am].[dbo].[SalutationType] SET SalutationKey = @SalutationKey, Description = @Description, TranslatableItemKey = @TranslatableItemKey WHERE SalutationKey = @SalutationKey";



        public const string workflowroletypegroupdatamodel_selectwhere = "SELECT WorkflowRoleTypeGroupKey, Description, GenericKeyTypeKey, WorkflowOrganisationStructureKey FROM [2am].[dbo].[WorkflowRoleTypeGroup] WHERE";
        public const string workflowroletypegroupdatamodel_selectbykey = "SELECT WorkflowRoleTypeGroupKey, Description, GenericKeyTypeKey, WorkflowOrganisationStructureKey FROM [2am].[dbo].[WorkflowRoleTypeGroup] WHERE WorkflowRoleTypeGroupKey = @PrimaryKey";
        public const string workflowroletypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowRoleTypeGroup] WHERE WorkflowRoleTypeGroupKey = @PrimaryKey";
        public const string workflowroletypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowRoleTypeGroup] WHERE";
        public const string workflowroletypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowRoleTypeGroup] (WorkflowRoleTypeGroupKey, Description, GenericKeyTypeKey, WorkflowOrganisationStructureKey) VALUES(@WorkflowRoleTypeGroupKey, @Description, @GenericKeyTypeKey, @WorkflowOrganisationStructureKey); ";
        public const string workflowroletypegroupdatamodel_update = "UPDATE [2am].[dbo].[WorkflowRoleTypeGroup] SET WorkflowRoleTypeGroupKey = @WorkflowRoleTypeGroupKey, Description = @Description, GenericKeyTypeKey = @GenericKeyTypeKey, WorkflowOrganisationStructureKey = @WorkflowOrganisationStructureKey WHERE WorkflowRoleTypeGroupKey = @WorkflowRoleTypeGroupKey";



        public const string offerdatamodel_selectwhere = "SELECT OfferKey, OfferTypeKey, OfferStatusKey, OfferStartDate, OfferEndDate, AccountKey, Reference, OfferCampaignKey, OfferSourceKey, ReservedAccountKey, OriginationSourceKey, EstimateNumberApplicants FROM [2am].[dbo].[Offer] WHERE";
        public const string offerdatamodel_selectbykey = "SELECT OfferKey, OfferTypeKey, OfferStatusKey, OfferStartDate, OfferEndDate, AccountKey, Reference, OfferCampaignKey, OfferSourceKey, ReservedAccountKey, OriginationSourceKey, EstimateNumberApplicants FROM [2am].[dbo].[Offer] WHERE OfferKey = @PrimaryKey";
        public const string offerdatamodel_delete = "DELETE FROM [2am].[dbo].[Offer] WHERE OfferKey = @PrimaryKey";
        public const string offerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Offer] WHERE";
        public const string offerdatamodel_insert = "INSERT INTO [2am].[dbo].[Offer] (OfferTypeKey, OfferStatusKey, OfferStartDate, OfferEndDate, AccountKey, Reference, OfferCampaignKey, OfferSourceKey, ReservedAccountKey, OriginationSourceKey, EstimateNumberApplicants) VALUES(@OfferTypeKey, @OfferStatusKey, @OfferStartDate, @OfferEndDate, @AccountKey, @Reference, @OfferCampaignKey, @OfferSourceKey, @ReservedAccountKey, @OriginationSourceKey, @EstimateNumberApplicants); select cast(scope_identity() as int)";
        public const string offerdatamodel_update = "UPDATE [2am].[dbo].[Offer] SET OfferTypeKey = @OfferTypeKey, OfferStatusKey = @OfferStatusKey, OfferStartDate = @OfferStartDate, OfferEndDate = @OfferEndDate, AccountKey = @AccountKey, Reference = @Reference, OfferCampaignKey = @OfferCampaignKey, OfferSourceKey = @OfferSourceKey, ReservedAccountKey = @ReservedAccountKey, OriginationSourceKey = @OriginationSourceKey, EstimateNumberApplicants = @EstimateNumberApplicants WHERE OfferKey = @OfferKey";



        public const string usergroupmappingdatamodel_selectwhere = "SELECT UserGroupMappingKey, OrganisationStructureKey, FunctionalGroupDefinitionKey FROM [2am].[dbo].[UserGroupMapping] WHERE";
        public const string usergroupmappingdatamodel_selectbykey = "SELECT UserGroupMappingKey, OrganisationStructureKey, FunctionalGroupDefinitionKey FROM [2am].[dbo].[UserGroupMapping] WHERE UserGroupMappingKey = @PrimaryKey";
        public const string usergroupmappingdatamodel_delete = "DELETE FROM [2am].[dbo].[UserGroupMapping] WHERE UserGroupMappingKey = @PrimaryKey";
        public const string usergroupmappingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[UserGroupMapping] WHERE";
        public const string usergroupmappingdatamodel_insert = "INSERT INTO [2am].[dbo].[UserGroupMapping] (OrganisationStructureKey, FunctionalGroupDefinitionKey) VALUES(@OrganisationStructureKey, @FunctionalGroupDefinitionKey); select cast(scope_identity() as int)";
        public const string usergroupmappingdatamodel_update = "UPDATE [2am].[dbo].[UserGroupMapping] SET OrganisationStructureKey = @OrganisationStructureKey, FunctionalGroupDefinitionKey = @FunctionalGroupDefinitionKey WHERE UserGroupMappingKey = @UserGroupMappingKey";



        public const string importroledatamodel_selectwhere = "SELECT RoleKey, LegalEntityKey, RoleTypeKey FROM [2am].[dbo].[ImportRole] WHERE";
        public const string importroledatamodel_selectbykey = "SELECT RoleKey, LegalEntityKey, RoleTypeKey FROM [2am].[dbo].[ImportRole] WHERE RoleKey = @PrimaryKey";
        public const string importroledatamodel_delete = "DELETE FROM [2am].[dbo].[ImportRole] WHERE RoleKey = @PrimaryKey";
        public const string importroledatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportRole] WHERE";
        public const string importroledatamodel_insert = "INSERT INTO [2am].[dbo].[ImportRole] (LegalEntityKey, RoleTypeKey) VALUES(@LegalEntityKey, @RoleTypeKey); select cast(scope_identity() as int)";
        public const string importroledatamodel_update = "UPDATE [2am].[dbo].[ImportRole] SET LegalEntityKey = @LegalEntityKey, RoleTypeKey = @RoleTypeKey WHERE RoleKey = @RoleKey";



        public const string accountstatusdatamodel_selectwhere = "SELECT AccountStatusKey, Description FROM [2am].[dbo].[AccountStatus] WHERE";
        public const string accountstatusdatamodel_selectbykey = "SELECT AccountStatusKey, Description FROM [2am].[dbo].[AccountStatus] WHERE AccountStatusKey = @PrimaryKey";
        public const string accountstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[AccountStatus] WHERE AccountStatusKey = @PrimaryKey";
        public const string accountstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountStatus] WHERE";
        public const string accountstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[AccountStatus] (AccountStatusKey, Description) VALUES(@AccountStatusKey, @Description); ";
        public const string accountstatusdatamodel_update = "UPDATE [2am].[dbo].[AccountStatus] SET AccountStatusKey = @AccountStatusKey, Description = @Description WHERE AccountStatusKey = @AccountStatusKey";



        public const string employmentverificationprocesstypedatamodel_selectwhere = "SELECT EmploymentVerificationProcessTypeKey, Description, GeneralStatuskey FROM [2am].[dbo].[EmploymentVerificationProcessType] WHERE";
        public const string employmentverificationprocesstypedatamodel_selectbykey = "SELECT EmploymentVerificationProcessTypeKey, Description, GeneralStatuskey FROM [2am].[dbo].[EmploymentVerificationProcessType] WHERE EmploymentVerificationProcessTypeKey = @PrimaryKey";
        public const string employmentverificationprocesstypedatamodel_delete = "DELETE FROM [2am].[dbo].[EmploymentVerificationProcessType] WHERE EmploymentVerificationProcessTypeKey = @PrimaryKey";
        public const string employmentverificationprocesstypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[EmploymentVerificationProcessType] WHERE";
        public const string employmentverificationprocesstypedatamodel_insert = "INSERT INTO [2am].[dbo].[EmploymentVerificationProcessType] (EmploymentVerificationProcessTypeKey, Description, GeneralStatuskey) VALUES(@EmploymentVerificationProcessTypeKey, @Description, @GeneralStatuskey); ";
        public const string employmentverificationprocesstypedatamodel_update = "UPDATE [2am].[dbo].[EmploymentVerificationProcessType] SET EmploymentVerificationProcessTypeKey = @EmploymentVerificationProcessTypeKey, Description = @Description, GeneralStatuskey = @GeneralStatuskey WHERE EmploymentVerificationProcessTypeKey = @EmploymentVerificationProcessTypeKey";



        public const string offerinformationvariableloandatamodel_selectwhere = "SELECT OfferInformationKey, CategoryKey, Term, ExistingLoan, CashDeposit, PropertyValuation, HouseholdIncome, FeesTotal, InterimInterest, MonthlyInstalment, LifePremium, HOCPremium, MinLoanRequired, MinBondRequired, PreApprovedAmount, MinCashAllowed, MaxCashAllowed, LoanAmountNoFees, RequestedCashAmount, LoanAgreementAmount, BondToRegister, LTV, PTI, MarketRate, SPVKey, EmploymentTypeKey, RateConfigurationKey, CreditMatrixKey, CreditCriteriaKey, AppliedInitiationFeeDiscount FROM [2am].[dbo].[OfferInformationVariableLoan] WHERE";
        public const string offerinformationvariableloandatamodel_selectbykey = "SELECT OfferInformationKey, CategoryKey, Term, ExistingLoan, CashDeposit, PropertyValuation, HouseholdIncome, FeesTotal, InterimInterest, MonthlyInstalment, LifePremium, HOCPremium, MinLoanRequired, MinBondRequired, PreApprovedAmount, MinCashAllowed, MaxCashAllowed, LoanAmountNoFees, RequestedCashAmount, LoanAgreementAmount, BondToRegister, LTV, PTI, MarketRate, SPVKey, EmploymentTypeKey, RateConfigurationKey, CreditMatrixKey, CreditCriteriaKey, AppliedInitiationFeeDiscount FROM [2am].[dbo].[OfferInformationVariableLoan] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationvariableloandatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationVariableLoan] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationvariableloandatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationVariableLoan] WHERE";
        public const string offerinformationvariableloandatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationVariableLoan] (OfferInformationKey, CategoryKey, Term, ExistingLoan, CashDeposit, PropertyValuation, HouseholdIncome, FeesTotal, InterimInterest, MonthlyInstalment, LifePremium, HOCPremium, MinLoanRequired, MinBondRequired, PreApprovedAmount, MinCashAllowed, MaxCashAllowed, LoanAmountNoFees, RequestedCashAmount, LoanAgreementAmount, BondToRegister, LTV, PTI, MarketRate, SPVKey, EmploymentTypeKey, RateConfigurationKey, CreditMatrixKey, CreditCriteriaKey, AppliedInitiationFeeDiscount) VALUES(@OfferInformationKey, @CategoryKey, @Term, @ExistingLoan, @CashDeposit, @PropertyValuation, @HouseholdIncome, @FeesTotal, @InterimInterest, @MonthlyInstalment, @LifePremium, @HOCPremium, @MinLoanRequired, @MinBondRequired, @PreApprovedAmount, @MinCashAllowed, @MaxCashAllowed, @LoanAmountNoFees, @RequestedCashAmount, @LoanAgreementAmount, @BondToRegister, @LTV, @PTI, @MarketRate, @SPVKey, @EmploymentTypeKey, @RateConfigurationKey, @CreditMatrixKey, @CreditCriteriaKey, @AppliedInitiationFeeDiscount); ";
        public const string offerinformationvariableloandatamodel_update = "UPDATE [2am].[dbo].[OfferInformationVariableLoan] SET OfferInformationKey = @OfferInformationKey, CategoryKey = @CategoryKey, Term = @Term, ExistingLoan = @ExistingLoan, CashDeposit = @CashDeposit, PropertyValuation = @PropertyValuation, HouseholdIncome = @HouseholdIncome, FeesTotal = @FeesTotal, InterimInterest = @InterimInterest, MonthlyInstalment = @MonthlyInstalment, LifePremium = @LifePremium, HOCPremium = @HOCPremium, MinLoanRequired = @MinLoanRequired, MinBondRequired = @MinBondRequired, PreApprovedAmount = @PreApprovedAmount, MinCashAllowed = @MinCashAllowed, MaxCashAllowed = @MaxCashAllowed, LoanAmountNoFees = @LoanAmountNoFees, RequestedCashAmount = @RequestedCashAmount, LoanAgreementAmount = @LoanAgreementAmount, BondToRegister = @BondToRegister, LTV = @LTV, PTI = @PTI, MarketRate = @MarketRate, SPVKey = @SPVKey, EmploymentTypeKey = @EmploymentTypeKey, RateConfigurationKey = @RateConfigurationKey, CreditMatrixKey = @CreditMatrixKey, CreditCriteriaKey = @CreditCriteriaKey, AppliedInitiationFeeDiscount = @AppliedInitiationFeeDiscount WHERE OfferInformationKey = @OfferInformationKey";



        public const string affordabilityassessmentstressfactordatamodel_selectwhere = "SELECT AffordabilityAssessmentStressFactorKey, StressFactorPercentage, PercentageIncreaseOnRepayments FROM [2am].[dbo].[AffordabilityAssessmentStressFactor] WHERE";
        public const string affordabilityassessmentstressfactordatamodel_selectbykey = "SELECT AffordabilityAssessmentStressFactorKey, StressFactorPercentage, PercentageIncreaseOnRepayments FROM [2am].[dbo].[AffordabilityAssessmentStressFactor] WHERE AffordabilityAssessmentStressFactorKey = @PrimaryKey";
        public const string affordabilityassessmentstressfactordatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentStressFactor] WHERE AffordabilityAssessmentStressFactorKey = @PrimaryKey";
        public const string affordabilityassessmentstressfactordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentStressFactor] WHERE";
        public const string affordabilityassessmentstressfactordatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityAssessmentStressFactor] (AffordabilityAssessmentStressFactorKey, StressFactorPercentage, PercentageIncreaseOnRepayments) VALUES(@AffordabilityAssessmentStressFactorKey, @StressFactorPercentage, @PercentageIncreaseOnRepayments); ";
        public const string affordabilityassessmentstressfactordatamodel_update = "UPDATE [2am].[dbo].[AffordabilityAssessmentStressFactor] SET AffordabilityAssessmentStressFactorKey = @AffordabilityAssessmentStressFactorKey, StressFactorPercentage = @StressFactorPercentage, PercentageIncreaseOnRepayments = @PercentageIncreaseOnRepayments WHERE AffordabilityAssessmentStressFactorKey = @AffordabilityAssessmentStressFactorKey";



        public const string documentsourcetypedatamodel_selectwhere = "SELECT DocumentSourceTypeKey, Description, GenericKeyTypeKey FROM [2am].[dbo].[DocumentSourceType] WHERE";
        public const string documentsourcetypedatamodel_selectbykey = "SELECT DocumentSourceTypeKey, Description, GenericKeyTypeKey FROM [2am].[dbo].[DocumentSourceType] WHERE DocumentSourceTypeKey = @PrimaryKey";
        public const string documentsourcetypedatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentSourceType] WHERE DocumentSourceTypeKey = @PrimaryKey";
        public const string documentsourcetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentSourceType] WHERE";
        public const string documentsourcetypedatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentSourceType] (Description, GenericKeyTypeKey) VALUES(@Description, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string documentsourcetypedatamodel_update = "UPDATE [2am].[dbo].[DocumentSourceType] SET Description = @Description, GenericKeyTypeKey = @GenericKeyTypeKey WHERE DocumentSourceTypeKey = @DocumentSourceTypeKey";



        public const string reportgroupdatamodel_selectwhere = "SELECT ReportGroupKey, Description, FeatureKey FROM [2am].[dbo].[ReportGroup] WHERE";
        public const string reportgroupdatamodel_selectbykey = "SELECT ReportGroupKey, Description, FeatureKey FROM [2am].[dbo].[ReportGroup] WHERE ReportGroupKey = @PrimaryKey";
        public const string reportgroupdatamodel_delete = "DELETE FROM [2am].[dbo].[ReportGroup] WHERE ReportGroupKey = @PrimaryKey";
        public const string reportgroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReportGroup] WHERE";
        public const string reportgroupdatamodel_insert = "INSERT INTO [2am].[dbo].[ReportGroup] (ReportGroupKey, Description, FeatureKey) VALUES(@ReportGroupKey, @Description, @FeatureKey); ";
        public const string reportgroupdatamodel_update = "UPDATE [2am].[dbo].[ReportGroup] SET ReportGroupKey = @ReportGroupKey, Description = @Description, FeatureKey = @FeatureKey WHERE ReportGroupKey = @ReportGroupKey";



        public const string accountindicationtypedatamodel_selectwhere = "SELECT AccountIndicationTypeKey, Description FROM [2am].[dbo].[AccountIndicationType] WHERE";
        public const string accountindicationtypedatamodel_selectbykey = "SELECT AccountIndicationTypeKey, Description FROM [2am].[dbo].[AccountIndicationType] WHERE AccountIndicationTypeKey = @PrimaryKey";
        public const string accountindicationtypedatamodel_delete = "DELETE FROM [2am].[dbo].[AccountIndicationType] WHERE AccountIndicationTypeKey = @PrimaryKey";
        public const string accountindicationtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountIndicationType] WHERE";
        public const string accountindicationtypedatamodel_insert = "INSERT INTO [2am].[dbo].[AccountIndicationType] (AccountIndicationTypeKey, Description) VALUES(@AccountIndicationTypeKey, @Description); ";
        public const string accountindicationtypedatamodel_update = "UPDATE [2am].[dbo].[AccountIndicationType] SET AccountIndicationTypeKey = @AccountIndicationTypeKey, Description = @Description WHERE AccountIndicationTypeKey = @AccountIndicationTypeKey";



        public const string auditaccountdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountKey, FixedPayment, AccountStatusKey, InsertedDate, OriginationSourceProductKey, OpenDate, CloseDate, RRR_ProductKey, RRR_OriginationSourceKey, UserID, ChangeDate, SPVKey, ParentAccountKey FROM [2am].[dbo].[AuditAccount] WHERE";
        public const string auditaccountdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountKey, FixedPayment, AccountStatusKey, InsertedDate, OriginationSourceProductKey, OpenDate, CloseDate, RRR_ProductKey, RRR_OriginationSourceKey, UserID, ChangeDate, SPVKey, ParentAccountKey FROM [2am].[dbo].[AuditAccount] WHERE AuditNumber = @PrimaryKey";
        public const string auditaccountdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditAccount] WHERE AuditNumber = @PrimaryKey";
        public const string auditaccountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditAccount] WHERE";
        public const string auditaccountdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditAccount] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountKey, FixedPayment, AccountStatusKey, InsertedDate, OriginationSourceProductKey, OpenDate, CloseDate, RRR_ProductKey, RRR_OriginationSourceKey, UserID, ChangeDate, SPVKey, ParentAccountKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @AccountKey, @FixedPayment, @AccountStatusKey, @InsertedDate, @OriginationSourceProductKey, @OpenDate, @CloseDate, @RRR_ProductKey, @RRR_OriginationSourceKey, @UserID, @ChangeDate, @SPVKey, @ParentAccountKey); select cast(scope_identity() as int)";
        public const string auditaccountdatamodel_update = "UPDATE [2am].[dbo].[AuditAccount] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, AccountKey = @AccountKey, FixedPayment = @FixedPayment, AccountStatusKey = @AccountStatusKey, InsertedDate = @InsertedDate, OriginationSourceProductKey = @OriginationSourceProductKey, OpenDate = @OpenDate, CloseDate = @CloseDate, RRR_ProductKey = @RRR_ProductKey, RRR_OriginationSourceKey = @RRR_OriginationSourceKey, UserID = @UserID, ChangeDate = @ChangeDate, SPVKey = @SPVKey, ParentAccountKey = @ParentAccountKey WHERE AuditNumber = @AuditNumber";



        public const string ruleitemdatamodel_selectwhere = "SELECT RuleItemKey, Name, Description, AssemblyName, TypeName, EnforceRule, GeneralStatusKey, GeneralStatusReasonDescription FROM [2am].[dbo].[RuleItem] WHERE";
        public const string ruleitemdatamodel_selectbykey = "SELECT RuleItemKey, Name, Description, AssemblyName, TypeName, EnforceRule, GeneralStatusKey, GeneralStatusReasonDescription FROM [2am].[dbo].[RuleItem] WHERE RuleItemKey = @PrimaryKey";
        public const string ruleitemdatamodel_delete = "DELETE FROM [2am].[dbo].[RuleItem] WHERE RuleItemKey = @PrimaryKey";
        public const string ruleitemdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RuleItem] WHERE";
        public const string ruleitemdatamodel_insert = "INSERT INTO [2am].[dbo].[RuleItem] (RuleItemKey, Name, Description, AssemblyName, TypeName, EnforceRule, GeneralStatusKey, GeneralStatusReasonDescription) VALUES(@RuleItemKey, @Name, @Description, @AssemblyName, @TypeName, @EnforceRule, @GeneralStatusKey, @GeneralStatusReasonDescription); ";
        public const string ruleitemdatamodel_update = "UPDATE [2am].[dbo].[RuleItem] SET RuleItemKey = @RuleItemKey, Name = @Name, Description = @Description, AssemblyName = @AssemblyName, TypeName = @TypeName, EnforceRule = @EnforceRule, GeneralStatusKey = @GeneralStatusKey, GeneralStatusReasonDescription = @GeneralStatusReasonDescription WHERE RuleItemKey = @RuleItemKey";



        public const string offercampaigndatamodel_selectwhere = "SELECT OfferCampaignKey, Description, StartDate FROM [2am].[dbo].[OfferCampaign] WHERE";
        public const string offercampaigndatamodel_selectbykey = "SELECT OfferCampaignKey, Description, StartDate FROM [2am].[dbo].[OfferCampaign] WHERE OfferCampaignKey = @PrimaryKey";
        public const string offercampaigndatamodel_delete = "DELETE FROM [2am].[dbo].[OfferCampaign] WHERE OfferCampaignKey = @PrimaryKey";
        public const string offercampaigndatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferCampaign] WHERE";
        public const string offercampaigndatamodel_insert = "INSERT INTO [2am].[dbo].[OfferCampaign] (Description, StartDate) VALUES(@Description, @StartDate); select cast(scope_identity() as int)";
        public const string offercampaigndatamodel_update = "UPDATE [2am].[dbo].[OfferCampaign] SET Description = @Description, StartDate = @StartDate WHERE OfferCampaignKey = @OfferCampaignKey";



        public const string affordabilityassessmentitemcategorydatamodel_selectwhere = "SELECT AffordabilityAssessmentItemCategoryKey, Description FROM [2am].[dbo].[AffordabilityAssessmentItemCategory] WHERE";
        public const string affordabilityassessmentitemcategorydatamodel_selectbykey = "SELECT AffordabilityAssessmentItemCategoryKey, Description FROM [2am].[dbo].[AffordabilityAssessmentItemCategory] WHERE AffordabilityAssessmentItemCategoryKey = @PrimaryKey";
        public const string affordabilityassessmentitemcategorydatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentItemCategory] WHERE AffordabilityAssessmentItemCategoryKey = @PrimaryKey";
        public const string affordabilityassessmentitemcategorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentItemCategory] WHERE";
        public const string affordabilityassessmentitemcategorydatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityAssessmentItemCategory] (AffordabilityAssessmentItemCategoryKey, Description) VALUES(@AffordabilityAssessmentItemCategoryKey, @Description); ";
        public const string affordabilityassessmentitemcategorydatamodel_update = "UPDATE [2am].[dbo].[AffordabilityAssessmentItemCategory] SET AffordabilityAssessmentItemCategoryKey = @AffordabilityAssessmentItemCategoryKey, Description = @Description WHERE AffordabilityAssessmentItemCategoryKey = @AffordabilityAssessmentItemCategoryKey";



        public const string guaranteedatamodel_selectwhere = "SELECT GuaranteeKey, AccountKey, LimitedAmount, IssueDate, StatusNumber, CancelledDate FROM [2am].[dbo].[Guarantee] WHERE";
        public const string guaranteedatamodel_selectbykey = "SELECT GuaranteeKey, AccountKey, LimitedAmount, IssueDate, StatusNumber, CancelledDate FROM [2am].[dbo].[Guarantee] WHERE GuaranteeKey = @PrimaryKey";
        public const string guaranteedatamodel_delete = "DELETE FROM [2am].[dbo].[Guarantee] WHERE GuaranteeKey = @PrimaryKey";
        public const string guaranteedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Guarantee] WHERE";
        public const string guaranteedatamodel_insert = "INSERT INTO [2am].[dbo].[Guarantee] (AccountKey, LimitedAmount, IssueDate, StatusNumber, CancelledDate) VALUES(@AccountKey, @LimitedAmount, @IssueDate, @StatusNumber, @CancelledDate); select cast(scope_identity() as int)";
        public const string guaranteedatamodel_update = "UPDATE [2am].[dbo].[Guarantee] SET AccountKey = @AccountKey, LimitedAmount = @LimitedAmount, IssueDate = @IssueDate, StatusNumber = @StatusNumber, CancelledDate = @CancelledDate WHERE GuaranteeKey = @GuaranteeKey";



        public const string workflowroletypedatamodel_selectwhere = "SELECT WorkflowRoleTypeKey, Description, WorkflowRoleTypeGroupKey FROM [2am].[dbo].[WorkflowRoleType] WHERE";
        public const string workflowroletypedatamodel_selectbykey = "SELECT WorkflowRoleTypeKey, Description, WorkflowRoleTypeGroupKey FROM [2am].[dbo].[WorkflowRoleType] WHERE WorkflowRoleTypeKey = @PrimaryKey";
        public const string workflowroletypedatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowRoleType] WHERE WorkflowRoleTypeKey = @PrimaryKey";
        public const string workflowroletypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowRoleType] WHERE";
        public const string workflowroletypedatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowRoleType] (WorkflowRoleTypeKey, Description, WorkflowRoleTypeGroupKey) VALUES(@WorkflowRoleTypeKey, @Description, @WorkflowRoleTypeGroupKey); ";
        public const string workflowroletypedatamodel_update = "UPDATE [2am].[dbo].[WorkflowRoleType] SET WorkflowRoleTypeKey = @WorkflowRoleTypeKey, Description = @Description, WorkflowRoleTypeGroupKey = @WorkflowRoleTypeGroupKey WHERE WorkflowRoleTypeKey = @WorkflowRoleTypeKey";



        public const string linkratemapdatamodel_selectwhere = "SELECT LinkRateMapKey, ExistingLinkRate, NewLinkRate, ExistingCategory FROM [2am].[dbo].[LinkRateMap] WHERE";
        public const string linkratemapdatamodel_selectbykey = "SELECT LinkRateMapKey, ExistingLinkRate, NewLinkRate, ExistingCategory FROM [2am].[dbo].[LinkRateMap] WHERE LinkRateMapKey = @PrimaryKey";
        public const string linkratemapdatamodel_delete = "DELETE FROM [2am].[dbo].[LinkRateMap] WHERE LinkRateMapKey = @PrimaryKey";
        public const string linkratemapdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LinkRateMap] WHERE";
        public const string linkratemapdatamodel_insert = "INSERT INTO [2am].[dbo].[LinkRateMap] (ExistingLinkRate, NewLinkRate, ExistingCategory) VALUES(@ExistingLinkRate, @NewLinkRate, @ExistingCategory); select cast(scope_identity() as int)";
        public const string linkratemapdatamodel_update = "UPDATE [2am].[dbo].[LinkRateMap] SET ExistingLinkRate = @ExistingLinkRate, NewLinkRate = @NewLinkRate, ExistingCategory = @ExistingCategory WHERE LinkRateMapKey = @LinkRateMapKey";



        public const string legalentityexceptionstatusdatamodel_selectwhere = "SELECT LegalEntityExceptionStatusKey, Description FROM [2am].[dbo].[LegalEntityExceptionStatus] WHERE";
        public const string legalentityexceptionstatusdatamodel_selectbykey = "SELECT LegalEntityExceptionStatusKey, Description FROM [2am].[dbo].[LegalEntityExceptionStatus] WHERE LegalEntityExceptionStatusKey = @PrimaryKey";
        public const string legalentityexceptionstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityExceptionStatus] WHERE LegalEntityExceptionStatusKey = @PrimaryKey";
        public const string legalentityexceptionstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityExceptionStatus] WHERE";
        public const string legalentityexceptionstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityExceptionStatus] (LegalEntityExceptionStatusKey, Description) VALUES(@LegalEntityExceptionStatusKey, @Description); ";
        public const string legalentityexceptionstatusdatamodel_update = "UPDATE [2am].[dbo].[LegalEntityExceptionStatus] SET LegalEntityExceptionStatusKey = @LegalEntityExceptionStatusKey, Description = @Description WHERE LegalEntityExceptionStatusKey = @LegalEntityExceptionStatusKey";



        public const string usergroupassignmentdatamodel_selectwhere = "SELECT UserGroupAssignmentKey, UserGroupMappingKey, ADUserKey, GenericKey, ChangeDate, InsertedDate FROM [2am].[dbo].[UserGroupAssignment] WHERE";
        public const string usergroupassignmentdatamodel_selectbykey = "SELECT UserGroupAssignmentKey, UserGroupMappingKey, ADUserKey, GenericKey, ChangeDate, InsertedDate FROM [2am].[dbo].[UserGroupAssignment] WHERE UserGroupAssignmentKey = @PrimaryKey";
        public const string usergroupassignmentdatamodel_delete = "DELETE FROM [2am].[dbo].[UserGroupAssignment] WHERE UserGroupAssignmentKey = @PrimaryKey";
        public const string usergroupassignmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[UserGroupAssignment] WHERE";
        public const string usergroupassignmentdatamodel_insert = "INSERT INTO [2am].[dbo].[UserGroupAssignment] (UserGroupMappingKey, ADUserKey, GenericKey, ChangeDate, InsertedDate) VALUES(@UserGroupMappingKey, @ADUserKey, @GenericKey, @ChangeDate, @InsertedDate); select cast(scope_identity() as int)";
        public const string usergroupassignmentdatamodel_update = "UPDATE [2am].[dbo].[UserGroupAssignment] SET UserGroupMappingKey = @UserGroupMappingKey, ADUserKey = @ADUserKey, GenericKey = @GenericKey, ChangeDate = @ChangeDate, InsertedDate = @InsertedDate WHERE UserGroupAssignmentKey = @UserGroupAssignmentKey";



        public const string roundrobinpointerdefinitiondatamodel_selectwhere = "SELECT RoundRobinPointerDefinitionKey, RoundRobinPointerKey, GenericKeyTypeKey, GenericKey, ApplicationName, StatementName, GeneralStatusKey FROM [2am].[dbo].[RoundRobinPointerDefinition] WHERE";
        public const string roundrobinpointerdefinitiondatamodel_selectbykey = "SELECT RoundRobinPointerDefinitionKey, RoundRobinPointerKey, GenericKeyTypeKey, GenericKey, ApplicationName, StatementName, GeneralStatusKey FROM [2am].[dbo].[RoundRobinPointerDefinition] WHERE RoundRobinPointerDefinitionKey = @PrimaryKey";
        public const string roundrobinpointerdefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[RoundRobinPointerDefinition] WHERE RoundRobinPointerDefinitionKey = @PrimaryKey";
        public const string roundrobinpointerdefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RoundRobinPointerDefinition] WHERE";
        public const string roundrobinpointerdefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[RoundRobinPointerDefinition] (RoundRobinPointerKey, GenericKeyTypeKey, GenericKey, ApplicationName, StatementName, GeneralStatusKey) VALUES(@RoundRobinPointerKey, @GenericKeyTypeKey, @GenericKey, @ApplicationName, @StatementName, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string roundrobinpointerdefinitiondatamodel_update = "UPDATE [2am].[dbo].[RoundRobinPointerDefinition] SET RoundRobinPointerKey = @RoundRobinPointerKey, GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey, ApplicationName = @ApplicationName, StatementName = @StatementName, GeneralStatusKey = @GeneralStatusKey WHERE RoundRobinPointerDefinitionKey = @RoundRobinPointerDefinitionKey";



        public const string employmentverificationprocessdatamodel_selectwhere = "SELECT EmploymentVerificationProcessKey, EmploymentKey, EmploymentVerificationProcessTypeKey, UserID, ChangeDate FROM [2am].[dbo].[EmploymentVerificationProcess] WHERE";
        public const string employmentverificationprocessdatamodel_selectbykey = "SELECT EmploymentVerificationProcessKey, EmploymentKey, EmploymentVerificationProcessTypeKey, UserID, ChangeDate FROM [2am].[dbo].[EmploymentVerificationProcess] WHERE EmploymentVerificationProcessKey = @PrimaryKey";
        public const string employmentverificationprocessdatamodel_delete = "DELETE FROM [2am].[dbo].[EmploymentVerificationProcess] WHERE EmploymentVerificationProcessKey = @PrimaryKey";
        public const string employmentverificationprocessdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[EmploymentVerificationProcess] WHERE";
        public const string employmentverificationprocessdatamodel_insert = "INSERT INTO [2am].[dbo].[EmploymentVerificationProcess] (EmploymentKey, EmploymentVerificationProcessTypeKey, UserID, ChangeDate) VALUES(@EmploymentKey, @EmploymentVerificationProcessTypeKey, @UserID, @ChangeDate); select cast(scope_identity() as int)";
        public const string employmentverificationprocessdatamodel_update = "UPDATE [2am].[dbo].[EmploymentVerificationProcess] SET EmploymentKey = @EmploymentKey, EmploymentVerificationProcessTypeKey = @EmploymentVerificationProcessTypeKey, UserID = @UserID, ChangeDate = @ChangeDate WHERE EmploymentVerificationProcessKey = @EmploymentVerificationProcessKey";



        public const string disbursementinterestapplieddatamodel_selectwhere = "SELECT InterestAppliedTypeKey, Description FROM [2am].[dbo].[DisbursementInterestApplied] WHERE";
        public const string disbursementinterestapplieddatamodel_selectbykey = "SELECT InterestAppliedTypeKey, Description FROM [2am].[dbo].[DisbursementInterestApplied] WHERE InterestAppliedTypeKey = @PrimaryKey";
        public const string disbursementinterestapplieddatamodel_delete = "DELETE FROM [2am].[dbo].[DisbursementInterestApplied] WHERE InterestAppliedTypeKey = @PrimaryKey";
        public const string disbursementinterestapplieddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisbursementInterestApplied] WHERE";
        public const string disbursementinterestapplieddatamodel_insert = "INSERT INTO [2am].[dbo].[DisbursementInterestApplied] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string disbursementinterestapplieddatamodel_update = "UPDATE [2am].[dbo].[DisbursementInterestApplied] SET Description = @Description WHERE InterestAppliedTypeKey = @InterestAppliedTypeKey";



        public const string accounttyperecognitiondatamodel_selectwhere = "SELECT AccountTypeRecognitionKey, ACBBankCode, ACBTypeNumber, RangeStart, RangeEnd, NoOfDigits1, NoOfDigits2, DigitNo1, MustEqual1, DigitNo2, MustEqual2, DropDigits, StartDropDigits, EndDropDigits, UserID, DateChange FROM [2am].[dbo].[AccountTypeRecognition] WHERE";
        public const string accounttyperecognitiondatamodel_selectbykey = "SELECT AccountTypeRecognitionKey, ACBBankCode, ACBTypeNumber, RangeStart, RangeEnd, NoOfDigits1, NoOfDigits2, DigitNo1, MustEqual1, DigitNo2, MustEqual2, DropDigits, StartDropDigits, EndDropDigits, UserID, DateChange FROM [2am].[dbo].[AccountTypeRecognition] WHERE AccountTypeRecognitionKey = @PrimaryKey";
        public const string accounttyperecognitiondatamodel_delete = "DELETE FROM [2am].[dbo].[AccountTypeRecognition] WHERE AccountTypeRecognitionKey = @PrimaryKey";
        public const string accounttyperecognitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountTypeRecognition] WHERE";
        public const string accounttyperecognitiondatamodel_insert = "INSERT INTO [2am].[dbo].[AccountTypeRecognition] (ACBBankCode, ACBTypeNumber, RangeStart, RangeEnd, NoOfDigits1, NoOfDigits2, DigitNo1, MustEqual1, DigitNo2, MustEqual2, DropDigits, StartDropDigits, EndDropDigits, UserID, DateChange) VALUES(@ACBBankCode, @ACBTypeNumber, @RangeStart, @RangeEnd, @NoOfDigits1, @NoOfDigits2, @DigitNo1, @MustEqual1, @DigitNo2, @MustEqual2, @DropDigits, @StartDropDigits, @EndDropDigits, @UserID, @DateChange); select cast(scope_identity() as int)";
        public const string accounttyperecognitiondatamodel_update = "UPDATE [2am].[dbo].[AccountTypeRecognition] SET ACBBankCode = @ACBBankCode, ACBTypeNumber = @ACBTypeNumber, RangeStart = @RangeStart, RangeEnd = @RangeEnd, NoOfDigits1 = @NoOfDigits1, NoOfDigits2 = @NoOfDigits2, DigitNo1 = @DigitNo1, MustEqual1 = @MustEqual1, DigitNo2 = @DigitNo2, MustEqual2 = @MustEqual2, DropDigits = @DropDigits, StartDropDigits = @StartDropDigits, EndDropDigits = @EndDropDigits, UserID = @UserID, DateChange = @DateChange WHERE AccountTypeRecognitionKey = @AccountTypeRecognitionKey";



        public const string offerdeclarationquestionanswerdatamodel_selectwhere = "SELECT OfferDeclarationQuestionAnswerKey, OfferDeclarationQuestionKey, OfferDeclarationAnswerKey FROM [2am].[dbo].[OfferDeclarationQuestionAnswer] WHERE";
        public const string offerdeclarationquestionanswerdatamodel_selectbykey = "SELECT OfferDeclarationQuestionAnswerKey, OfferDeclarationQuestionKey, OfferDeclarationAnswerKey FROM [2am].[dbo].[OfferDeclarationQuestionAnswer] WHERE OfferDeclarationQuestionAnswerKey = @PrimaryKey";
        public const string offerdeclarationquestionanswerdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDeclarationQuestionAnswer] WHERE OfferDeclarationQuestionAnswerKey = @PrimaryKey";
        public const string offerdeclarationquestionanswerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDeclarationQuestionAnswer] WHERE";
        public const string offerdeclarationquestionanswerdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDeclarationQuestionAnswer] (OfferDeclarationQuestionKey, OfferDeclarationAnswerKey) VALUES(@OfferDeclarationQuestionKey, @OfferDeclarationAnswerKey); select cast(scope_identity() as int)";
        public const string offerdeclarationquestionanswerdatamodel_update = "UPDATE [2am].[dbo].[OfferDeclarationQuestionAnswer] SET OfferDeclarationQuestionKey = @OfferDeclarationQuestionKey, OfferDeclarationAnswerKey = @OfferDeclarationAnswerKey WHERE OfferDeclarationQuestionAnswerKey = @OfferDeclarationQuestionAnswerKey";



        public const string subsidyprovidertypedatamodel_selectwhere = "SELECT SubsidyProviderTypeKey, Description FROM [2am].[dbo].[SubsidyProviderType] WHERE";
        public const string subsidyprovidertypedatamodel_selectbykey = "SELECT SubsidyProviderTypeKey, Description FROM [2am].[dbo].[SubsidyProviderType] WHERE SubsidyProviderTypeKey = @PrimaryKey";
        public const string subsidyprovidertypedatamodel_delete = "DELETE FROM [2am].[dbo].[SubsidyProviderType] WHERE SubsidyProviderTypeKey = @PrimaryKey";
        public const string subsidyprovidertypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[SubsidyProviderType] WHERE";
        public const string subsidyprovidertypedatamodel_insert = "INSERT INTO [2am].[dbo].[SubsidyProviderType] (SubsidyProviderTypeKey, Description) VALUES(@SubsidyProviderTypeKey, @Description); ";
        public const string subsidyprovidertypedatamodel_update = "UPDATE [2am].[dbo].[SubsidyProviderType] SET SubsidyProviderTypeKey = @SubsidyProviderTypeKey, Description = @Description WHERE SubsidyProviderTypeKey = @SubsidyProviderTypeKey";



        public const string valuatordatamodel_selectwhere = "SELECT ValuatorKey, ValuatorContact, ValuatorPassword, LimitedUserGroup, GeneralStatusKey, LegalEntityKey FROM [2am].[dbo].[Valuator] WHERE";
        public const string valuatordatamodel_selectbykey = "SELECT ValuatorKey, ValuatorContact, ValuatorPassword, LimitedUserGroup, GeneralStatusKey, LegalEntityKey FROM [2am].[dbo].[Valuator] WHERE ValuatorKey = @PrimaryKey";
        public const string valuatordatamodel_delete = "DELETE FROM [2am].[dbo].[Valuator] WHERE ValuatorKey = @PrimaryKey";
        public const string valuatordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Valuator] WHERE";
        public const string valuatordatamodel_insert = "INSERT INTO [2am].[dbo].[Valuator] (ValuatorContact, ValuatorPassword, LimitedUserGroup, GeneralStatusKey, LegalEntityKey) VALUES(@ValuatorContact, @ValuatorPassword, @LimitedUserGroup, @GeneralStatusKey, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string valuatordatamodel_update = "UPDATE [2am].[dbo].[Valuator] SET ValuatorContact = @ValuatorContact, ValuatorPassword = @ValuatorPassword, LimitedUserGroup = @LimitedUserGroup, GeneralStatusKey = @GeneralStatusKey, LegalEntityKey = @LegalEntityKey WHERE ValuatorKey = @ValuatorKey";



        public const string affordabilityassessmentitemtypedatamodel_selectwhere = "SELECT AffordabilityAssessmentItemTypeKey, AffordabilityAssessmentItemCategoryKey, Description, ApplyStressFactor, Consolidatable FROM [2am].[dbo].[AffordabilityAssessmentItemType] WHERE";
        public const string affordabilityassessmentitemtypedatamodel_selectbykey = "SELECT AffordabilityAssessmentItemTypeKey, AffordabilityAssessmentItemCategoryKey, Description, ApplyStressFactor, Consolidatable FROM [2am].[dbo].[AffordabilityAssessmentItemType] WHERE AffordabilityAssessmentItemTypeKey = @PrimaryKey";
        public const string affordabilityassessmentitemtypedatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentItemType] WHERE AffordabilityAssessmentItemTypeKey = @PrimaryKey";
        public const string affordabilityassessmentitemtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentItemType] WHERE";
        public const string affordabilityassessmentitemtypedatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityAssessmentItemType] (AffordabilityAssessmentItemTypeKey, AffordabilityAssessmentItemCategoryKey, Description, ApplyStressFactor, Consolidatable) VALUES(@AffordabilityAssessmentItemTypeKey, @AffordabilityAssessmentItemCategoryKey, @Description, @ApplyStressFactor, @Consolidatable); ";
        public const string affordabilityassessmentitemtypedatamodel_update = "UPDATE [2am].[dbo].[AffordabilityAssessmentItemType] SET AffordabilityAssessmentItemTypeKey = @AffordabilityAssessmentItemTypeKey, AffordabilityAssessmentItemCategoryKey = @AffordabilityAssessmentItemCategoryKey, Description = @Description, ApplyStressFactor = @ApplyStressFactor, Consolidatable = @Consolidatable WHERE AffordabilityAssessmentItemTypeKey = @AffordabilityAssessmentItemTypeKey";



        public const string addressformatdatamodel_selectwhere = "SELECT AddressFormatKey, Description FROM [2am].[dbo].[AddressFormat] WHERE";
        public const string addressformatdatamodel_selectbykey = "SELECT AddressFormatKey, Description FROM [2am].[dbo].[AddressFormat] WHERE AddressFormatKey = @PrimaryKey";
        public const string addressformatdatamodel_delete = "DELETE FROM [2am].[dbo].[AddressFormat] WHERE AddressFormatKey = @PrimaryKey";
        public const string addressformatdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AddressFormat] WHERE";
        public const string addressformatdatamodel_insert = "INSERT INTO [2am].[dbo].[AddressFormat] (AddressFormatKey, Description) VALUES(@AddressFormatKey, @Description); ";
        public const string addressformatdatamodel_update = "UPDATE [2am].[dbo].[AddressFormat] SET AddressFormatKey = @AddressFormatKey, Description = @Description WHERE AddressFormatKey = @AddressFormatKey";



        public const string reportparameterdatamodel_selectwhere = "SELECT ReportParameterKey, ReportStatementKey, ParameterName, ParameterType, ParameterLength, DisplayName, Required, ParameterTypeKey, DomainFieldKey, StatementName FROM [2am].[dbo].[ReportParameter] WHERE";
        public const string reportparameterdatamodel_selectbykey = "SELECT ReportParameterKey, ReportStatementKey, ParameterName, ParameterType, ParameterLength, DisplayName, Required, ParameterTypeKey, DomainFieldKey, StatementName FROM [2am].[dbo].[ReportParameter] WHERE ReportParameterKey = @PrimaryKey";
        public const string reportparameterdatamodel_delete = "DELETE FROM [2am].[dbo].[ReportParameter] WHERE ReportParameterKey = @PrimaryKey";
        public const string reportparameterdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReportParameter] WHERE";
        public const string reportparameterdatamodel_insert = "INSERT INTO [2am].[dbo].[ReportParameter] (ReportStatementKey, ParameterName, ParameterType, ParameterLength, DisplayName, Required, ParameterTypeKey, DomainFieldKey, StatementName) VALUES(@ReportStatementKey, @ParameterName, @ParameterType, @ParameterLength, @DisplayName, @Required, @ParameterTypeKey, @DomainFieldKey, @StatementName); select cast(scope_identity() as int)";
        public const string reportparameterdatamodel_update = "UPDATE [2am].[dbo].[ReportParameter] SET ReportStatementKey = @ReportStatementKey, ParameterName = @ParameterName, ParameterType = @ParameterType, ParameterLength = @ParameterLength, DisplayName = @DisplayName, Required = @Required, ParameterTypeKey = @ParameterTypeKey, DomainFieldKey = @DomainFieldKey, StatementName = @StatementName WHERE ReportParameterKey = @ReportParameterKey";



        public const string uiwsmenudatamodel_selectwhere = "SELECT MenuKey, Name, Link, Active, Parent, DisplayOrder FROM [2am].[dbo].[uiWsMenu] WHERE";
        public const string uiwsmenudatamodel_selectbykey = "SELECT MenuKey, Name, Link, Active, Parent, DisplayOrder FROM [2am].[dbo].[uiWsMenu] WHERE  = @PrimaryKey";
        public const string uiwsmenudatamodel_delete = "DELETE FROM [2am].[dbo].[uiWsMenu] WHERE  = @PrimaryKey";
        public const string uiwsmenudatamodel_deletewhere = "DELETE FROM [2am].[dbo].[uiWsMenu] WHERE";
        public const string uiwsmenudatamodel_insert = "INSERT INTO [2am].[dbo].[uiWsMenu] (MenuKey, Name, Link, Active, Parent, DisplayOrder) VALUES(@MenuKey, @Name, @Link, @Active, @Parent, @DisplayOrder); ";
        public const string uiwsmenudatamodel_update = "UPDATE [2am].[dbo].[uiWsMenu] SET MenuKey = @MenuKey, Name = @Name, Link = @Link, Active = @Active, Parent = @Parent, DisplayOrder = @DisplayOrder WHERE  = @";



        public const string linkedkeysdatamodel_selectwhere = "SELECT LinkedKey, GuidKey FROM [2am].[dbo].[LinkedKeys] WHERE";
        public const string linkedkeysdatamodel_selectbykey = "SELECT LinkedKey, GuidKey FROM [2am].[dbo].[LinkedKeys] WHERE  = @PrimaryKey";
        public const string linkedkeysdatamodel_delete = "DELETE FROM [2am].[dbo].[LinkedKeys] WHERE  = @PrimaryKey";
        public const string linkedkeysdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LinkedKeys] WHERE";
        public const string linkedkeysdatamodel_insert = "INSERT INTO [2am].[dbo].[LinkedKeys] (LinkedKey, GuidKey) VALUES(@LinkedKey, @GuidKey); ";
        public const string linkedkeysdatamodel_update = "UPDATE [2am].[dbo].[LinkedKeys] SET LinkedKey = @LinkedKey, GuidKey = @GuidKey WHERE  = @";



        public const string legalentityrelationshiptypedatamodel_selectwhere = "SELECT RelationshipTypeKey, Description FROM [2am].[dbo].[LegalEntityRelationshipType] WHERE";
        public const string legalentityrelationshiptypedatamodel_selectbykey = "SELECT RelationshipTypeKey, Description FROM [2am].[dbo].[LegalEntityRelationshipType] WHERE RelationshipTypeKey = @PrimaryKey";
        public const string legalentityrelationshiptypedatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityRelationshipType] WHERE RelationshipTypeKey = @PrimaryKey";
        public const string legalentityrelationshiptypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityRelationshipType] WHERE";
        public const string legalentityrelationshiptypedatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityRelationshipType] (RelationshipTypeKey, Description) VALUES(@RelationshipTypeKey, @Description); ";
        public const string legalentityrelationshiptypedatamodel_update = "UPDATE [2am].[dbo].[LegalEntityRelationshipType] SET RelationshipTypeKey = @RelationshipTypeKey, Description = @Description WHERE RelationshipTypeKey = @RelationshipTypeKey";



        public const string textstatementtypedatamodel_selectwhere = "SELECT TextStatementTypeKey, OriginationSourceProductKey, Description FROM [2am].[dbo].[TextStatementType] WHERE";
        public const string textstatementtypedatamodel_selectbykey = "SELECT TextStatementTypeKey, OriginationSourceProductKey, Description FROM [2am].[dbo].[TextStatementType] WHERE TextStatementTypeKey = @PrimaryKey";
        public const string textstatementtypedatamodel_delete = "DELETE FROM [2am].[dbo].[TextStatementType] WHERE TextStatementTypeKey = @PrimaryKey";
        public const string textstatementtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TextStatementType] WHERE";
        public const string textstatementtypedatamodel_insert = "INSERT INTO [2am].[dbo].[TextStatementType] (TextStatementTypeKey, OriginationSourceProductKey, Description) VALUES(@TextStatementTypeKey, @OriginationSourceProductKey, @Description); ";
        public const string textstatementtypedatamodel_update = "UPDATE [2am].[dbo].[TextStatementType] SET TextStatementTypeKey = @TextStatementTypeKey, OriginationSourceProductKey = @OriginationSourceProductKey, Description = @Description WHERE TextStatementTypeKey = @TextStatementTypeKey";



        public const string workflowroledatamodel_selectwhere = "SELECT WorkflowRoleKey, LegalEntityKey, GenericKey, WorkflowRoleTypeKey, GeneralStatusKey, StatusChangeDate FROM [2am].[dbo].[WorkflowRole] WHERE";
        public const string workflowroledatamodel_selectbykey = "SELECT WorkflowRoleKey, LegalEntityKey, GenericKey, WorkflowRoleTypeKey, GeneralStatusKey, StatusChangeDate FROM [2am].[dbo].[WorkflowRole] WHERE WorkflowRoleKey = @PrimaryKey";
        public const string workflowroledatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowRole] WHERE WorkflowRoleKey = @PrimaryKey";
        public const string workflowroledatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowRole] WHERE";
        public const string workflowroledatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowRole] (LegalEntityKey, GenericKey, WorkflowRoleTypeKey, GeneralStatusKey, StatusChangeDate) VALUES(@LegalEntityKey, @GenericKey, @WorkflowRoleTypeKey, @GeneralStatusKey, @StatusChangeDate); select cast(scope_identity() as int)";
        public const string workflowroledatamodel_update = "UPDATE [2am].[dbo].[WorkflowRole] SET LegalEntityKey = @LegalEntityKey, GenericKey = @GenericKey, WorkflowRoleTypeKey = @WorkflowRoleTypeKey, GeneralStatusKey = @GeneralStatusKey, StatusChangeDate = @StatusChangeDate WHERE WorkflowRoleKey = @WorkflowRoleKey";



        public const string legalentityorganisationstructuredatamodel_selectwhere = "SELECT LegalEntityOrganisationStructureKey, LegalEntityKey, OrganisationStructureKey FROM [2am].[dbo].[LegalEntityOrganisationStructure] WHERE";
        public const string legalentityorganisationstructuredatamodel_selectbykey = "SELECT LegalEntityOrganisationStructureKey, LegalEntityKey, OrganisationStructureKey FROM [2am].[dbo].[LegalEntityOrganisationStructure] WHERE LegalEntityOrganisationStructureKey = @PrimaryKey";
        public const string legalentityorganisationstructuredatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityOrganisationStructure] WHERE LegalEntityOrganisationStructureKey = @PrimaryKey";
        public const string legalentityorganisationstructuredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityOrganisationStructure] WHERE";
        public const string legalentityorganisationstructuredatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityOrganisationStructure] (LegalEntityKey, OrganisationStructureKey) VALUES(@LegalEntityKey, @OrganisationStructureKey); select cast(scope_identity() as int)";
        public const string legalentityorganisationstructuredatamodel_update = "UPDATE [2am].[dbo].[LegalEntityOrganisationStructure] SET LegalEntityKey = @LegalEntityKey, OrganisationStructureKey = @OrganisationStructureKey WHERE LegalEntityOrganisationStructureKey = @LegalEntityOrganisationStructureKey";



        public const string addresstypedatamodel_selectwhere = "SELECT AddressTypeKey, Description FROM [2am].[dbo].[AddressType] WHERE";
        public const string addresstypedatamodel_selectbykey = "SELECT AddressTypeKey, Description FROM [2am].[dbo].[AddressType] WHERE AddressTypeKey = @PrimaryKey";
        public const string addresstypedatamodel_delete = "DELETE FROM [2am].[dbo].[AddressType] WHERE AddressTypeKey = @PrimaryKey";
        public const string addresstypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AddressType] WHERE";
        public const string addresstypedatamodel_insert = "INSERT INTO [2am].[dbo].[AddressType] (AddressTypeKey, Description) VALUES(@AddressTypeKey, @Description); ";
        public const string addresstypedatamodel_update = "UPDATE [2am].[dbo].[AddressType] SET AddressTypeKey = @AddressTypeKey, Description = @Description WHERE AddressTypeKey = @AddressTypeKey";



        public const string offerdeclarationdatamodel_selectwhere = "SELECT OfferDeclarationKey, OfferRoleKey, OfferDeclarationQuestionKey, OfferDeclarationAnswerKey, OfferDeclarationDate FROM [2am].[dbo].[OfferDeclaration] WHERE";
        public const string offerdeclarationdatamodel_selectbykey = "SELECT OfferDeclarationKey, OfferRoleKey, OfferDeclarationQuestionKey, OfferDeclarationAnswerKey, OfferDeclarationDate FROM [2am].[dbo].[OfferDeclaration] WHERE OfferDeclarationKey = @PrimaryKey";
        public const string offerdeclarationdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDeclaration] WHERE OfferDeclarationKey = @PrimaryKey";
        public const string offerdeclarationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDeclaration] WHERE";
        public const string offerdeclarationdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDeclaration] (OfferRoleKey, OfferDeclarationQuestionKey, OfferDeclarationAnswerKey, OfferDeclarationDate) VALUES(@OfferRoleKey, @OfferDeclarationQuestionKey, @OfferDeclarationAnswerKey, @OfferDeclarationDate); select cast(scope_identity() as int)";
        public const string offerdeclarationdatamodel_update = "UPDATE [2am].[dbo].[OfferDeclaration] SET OfferRoleKey = @OfferRoleKey, OfferDeclarationQuestionKey = @OfferDeclarationQuestionKey, OfferDeclarationAnswerKey = @OfferDeclarationAnswerKey, OfferDeclarationDate = @OfferDeclarationDate WHERE OfferDeclarationKey = @OfferDeclarationKey";



        public const string auditaccountpropertydatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountPropertyKey, AccountKey, PropertyKey FROM [2am].[dbo].[AuditAccountProperty] WHERE";
        public const string auditaccountpropertydatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountPropertyKey, AccountKey, PropertyKey FROM [2am].[dbo].[AuditAccountProperty] WHERE AuditNumber = @PrimaryKey";
        public const string auditaccountpropertydatamodel_delete = "DELETE FROM [2am].[dbo].[AuditAccountProperty] WHERE AuditNumber = @PrimaryKey";
        public const string auditaccountpropertydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditAccountProperty] WHERE";
        public const string auditaccountpropertydatamodel_insert = "INSERT INTO [2am].[dbo].[AuditAccountProperty] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountPropertyKey, AccountKey, PropertyKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @AccountPropertyKey, @AccountKey, @PropertyKey); select cast(scope_identity() as int)";
        public const string auditaccountpropertydatamodel_update = "UPDATE [2am].[dbo].[AuditAccountProperty] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, AccountPropertyKey = @AccountPropertyKey, AccountKey = @AccountKey, PropertyKey = @PropertyKey WHERE AuditNumber = @AuditNumber";



        public const string userorganisationstructureroundrobinstatusdatamodel_selectwhere = "SELECT UserOrganisationStructureRoundRobinStatusKey, UserOrganisationStructureKey, GeneralStatusKey, CapitecGeneralStatusKey FROM [2am].[dbo].[UserOrganisationStructureRoundRobinStatus] WHERE";
        public const string userorganisationstructureroundrobinstatusdatamodel_selectbykey = "SELECT UserOrganisationStructureRoundRobinStatusKey, UserOrganisationStructureKey, GeneralStatusKey, CapitecGeneralStatusKey FROM [2am].[dbo].[UserOrganisationStructureRoundRobinStatus] WHERE UserOrganisationStructureRoundRobinStatusKey = @PrimaryKey";
        public const string userorganisationstructureroundrobinstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[UserOrganisationStructureRoundRobinStatus] WHERE UserOrganisationStructureRoundRobinStatusKey = @PrimaryKey";
        public const string userorganisationstructureroundrobinstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[UserOrganisationStructureRoundRobinStatus] WHERE";
        public const string userorganisationstructureroundrobinstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[UserOrganisationStructureRoundRobinStatus] (UserOrganisationStructureKey, GeneralStatusKey, CapitecGeneralStatusKey) VALUES(@UserOrganisationStructureKey, @GeneralStatusKey, @CapitecGeneralStatusKey); select cast(scope_identity() as int)";
        public const string userorganisationstructureroundrobinstatusdatamodel_update = "UPDATE [2am].[dbo].[UserOrganisationStructureRoundRobinStatus] SET UserOrganisationStructureKey = @UserOrganisationStructureKey, GeneralStatusKey = @GeneralStatusKey, CapitecGeneralStatusKey = @CapitecGeneralStatusKey WHERE UserOrganisationStructureRoundRobinStatusKey = @UserOrganisationStructureRoundRobinStatusKey";



        public const string offerinformationedgedatamodel_selectwhere = "SELECT OfferInformationKey, FullTermInstalment, AmortisationTermInstalment, InterestOnlyInstalment, InterestOnlyTerm FROM [2am].[dbo].[OfferInformationEdge] WHERE";
        public const string offerinformationedgedatamodel_selectbykey = "SELECT OfferInformationKey, FullTermInstalment, AmortisationTermInstalment, InterestOnlyInstalment, InterestOnlyTerm FROM [2am].[dbo].[OfferInformationEdge] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationedgedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationEdge] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationedgedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationEdge] WHERE";
        public const string offerinformationedgedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationEdge] (OfferInformationKey, FullTermInstalment, AmortisationTermInstalment, InterestOnlyInstalment, InterestOnlyTerm) VALUES(@OfferInformationKey, @FullTermInstalment, @AmortisationTermInstalment, @InterestOnlyInstalment, @InterestOnlyTerm); ";
        public const string offerinformationedgedatamodel_update = "UPDATE [2am].[dbo].[OfferInformationEdge] SET OfferInformationKey = @OfferInformationKey, FullTermInstalment = @FullTermInstalment, AmortisationTermInstalment = @AmortisationTermInstalment, InterestOnlyInstalment = @InterestOnlyInstalment, InterestOnlyTerm = @InterestOnlyTerm WHERE OfferInformationKey = @OfferInformationKey";



        public const string auditoriginationsourceproductdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, OriginationSourceProductKey, OriginationSourceKey, ProductKey FROM [2am].[dbo].[AuditOriginationSourceProduct] WHERE";
        public const string auditoriginationsourceproductdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, OriginationSourceProductKey, OriginationSourceKey, ProductKey FROM [2am].[dbo].[AuditOriginationSourceProduct] WHERE AuditNumber = @PrimaryKey";
        public const string auditoriginationsourceproductdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditOriginationSourceProduct] WHERE AuditNumber = @PrimaryKey";
        public const string auditoriginationsourceproductdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditOriginationSourceProduct] WHERE";
        public const string auditoriginationsourceproductdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditOriginationSourceProduct] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, OriginationSourceProductKey, OriginationSourceKey, ProductKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @OriginationSourceProductKey, @OriginationSourceKey, @ProductKey); select cast(scope_identity() as int)";
        public const string auditoriginationsourceproductdatamodel_update = "UPDATE [2am].[dbo].[AuditOriginationSourceProduct] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, OriginationSourceProductKey = @OriginationSourceProductKey, OriginationSourceKey = @OriginationSourceKey, ProductKey = @ProductKey WHERE AuditNumber = @AuditNumber";



        public const string accountindicationdatamodel_selectwhere = "SELECT AccountIndicationKey, AccountIndicator, AccountIndicationTypeKey, Indicator, UserID, DateChange FROM [2am].[dbo].[AccountIndication] WHERE";
        public const string accountindicationdatamodel_selectbykey = "SELECT AccountIndicationKey, AccountIndicator, AccountIndicationTypeKey, Indicator, UserID, DateChange FROM [2am].[dbo].[AccountIndication] WHERE AccountIndicationKey = @PrimaryKey";
        public const string accountindicationdatamodel_delete = "DELETE FROM [2am].[dbo].[AccountIndication] WHERE AccountIndicationKey = @PrimaryKey";
        public const string accountindicationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountIndication] WHERE";
        public const string accountindicationdatamodel_insert = "INSERT INTO [2am].[dbo].[AccountIndication] (AccountIndicator, AccountIndicationTypeKey, Indicator, UserID, DateChange) VALUES(@AccountIndicator, @AccountIndicationTypeKey, @Indicator, @UserID, @DateChange); select cast(scope_identity() as int)";
        public const string accountindicationdatamodel_update = "UPDATE [2am].[dbo].[AccountIndication] SET AccountIndicator = @AccountIndicator, AccountIndicationTypeKey = @AccountIndicationTypeKey, Indicator = @Indicator, UserID = @UserID, DateChange = @DateChange WHERE AccountIndicationKey = @AccountIndicationKey";



        public const string affordabilityassessmentdatamodel_selectwhere = "SELECT AffordabilityAssessmentKey, GenericKey, GenericKeyTypeKey, AffordabilityAssessmentStatusKey, GeneralStatusKey, AffordabilityAssessmentStressFactorKey, ModifiedDate, ModifiedByUserId, NumberOfContributingApplicants, NumberOfHouseholdDependants, MinimumMonthlyFixedExpenses, ConfirmedDate, Notes FROM [2am].[dbo].[AffordabilityAssessment] WHERE";
        public const string affordabilityassessmentdatamodel_selectbykey = "SELECT AffordabilityAssessmentKey, GenericKey, GenericKeyTypeKey, AffordabilityAssessmentStatusKey, GeneralStatusKey, AffordabilityAssessmentStressFactorKey, ModifiedDate, ModifiedByUserId, NumberOfContributingApplicants, NumberOfHouseholdDependants, MinimumMonthlyFixedExpenses, ConfirmedDate, Notes FROM [2am].[dbo].[AffordabilityAssessment] WHERE AffordabilityAssessmentKey = @PrimaryKey";
        public const string affordabilityassessmentdatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityAssessment] WHERE AffordabilityAssessmentKey = @PrimaryKey";
        public const string affordabilityassessmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityAssessment] WHERE";
        public const string affordabilityassessmentdatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityAssessment] (GenericKey, GenericKeyTypeKey, AffordabilityAssessmentStatusKey, GeneralStatusKey, AffordabilityAssessmentStressFactorKey, ModifiedDate, ModifiedByUserId, NumberOfContributingApplicants, NumberOfHouseholdDependants, MinimumMonthlyFixedExpenses, ConfirmedDate, Notes) VALUES(@GenericKey, @GenericKeyTypeKey, @AffordabilityAssessmentStatusKey, @GeneralStatusKey, @AffordabilityAssessmentStressFactorKey, @ModifiedDate, @ModifiedByUserId, @NumberOfContributingApplicants, @NumberOfHouseholdDependants, @MinimumMonthlyFixedExpenses, @ConfirmedDate, @Notes); select cast(scope_identity() as int)";
        public const string affordabilityassessmentdatamodel_update = "UPDATE [2am].[dbo].[AffordabilityAssessment] SET GenericKey = @GenericKey, GenericKeyTypeKey = @GenericKeyTypeKey, AffordabilityAssessmentStatusKey = @AffordabilityAssessmentStatusKey, GeneralStatusKey = @GeneralStatusKey, AffordabilityAssessmentStressFactorKey = @AffordabilityAssessmentStressFactorKey, ModifiedDate = @ModifiedDate, ModifiedByUserId = @ModifiedByUserId, NumberOfContributingApplicants = @NumberOfContributingApplicants, NumberOfHouseholdDependants = @NumberOfHouseholdDependants, MinimumMonthlyFixedExpenses = @MinimumMonthlyFixedExpenses, ConfirmedDate = @ConfirmedDate, Notes = @Notes WHERE AffordabilityAssessmentKey = @AffordabilityAssessmentKey";



        public const string legalentitystatusdatamodel_selectwhere = "SELECT LegalEntityStatusKey, Description FROM [2am].[dbo].[LegalEntityStatus] WHERE";
        public const string legalentitystatusdatamodel_selectbykey = "SELECT LegalEntityStatusKey, Description FROM [2am].[dbo].[LegalEntityStatus] WHERE LegalEntityStatusKey = @PrimaryKey";
        public const string legalentitystatusdatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityStatus] WHERE LegalEntityStatusKey = @PrimaryKey";
        public const string legalentitystatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityStatus] WHERE";
        public const string legalentitystatusdatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityStatus] (LegalEntityStatusKey, Description) VALUES(@LegalEntityStatusKey, @Description); ";
        public const string legalentitystatusdatamodel_update = "UPDATE [2am].[dbo].[LegalEntityStatus] SET LegalEntityStatusKey = @LegalEntityStatusKey, Description = @Description WHERE LegalEntityStatusKey = @LegalEntityStatusKey";



        public const string lifepremiumhistorydatamodel_selectwhere = "SELECT LifePremiumHistoryKey, ChangeDate, DeathPremium, IPBPremium, SumAssured, YearlyPremium, PolicyFactor, DiscountFactor, UserName, AccountKey, MonthlyPremium FROM [2am].[dbo].[LifePremiumHistory] WHERE";
        public const string lifepremiumhistorydatamodel_selectbykey = "SELECT LifePremiumHistoryKey, ChangeDate, DeathPremium, IPBPremium, SumAssured, YearlyPremium, PolicyFactor, DiscountFactor, UserName, AccountKey, MonthlyPremium FROM [2am].[dbo].[LifePremiumHistory] WHERE LifePremiumHistoryKey = @PrimaryKey";
        public const string lifepremiumhistorydatamodel_delete = "DELETE FROM [2am].[dbo].[LifePremiumHistory] WHERE LifePremiumHistoryKey = @PrimaryKey";
        public const string lifepremiumhistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifePremiumHistory] WHERE";
        public const string lifepremiumhistorydatamodel_insert = "INSERT INTO [2am].[dbo].[LifePremiumHistory] (ChangeDate, DeathPremium, IPBPremium, SumAssured, YearlyPremium, PolicyFactor, DiscountFactor, UserName, AccountKey, MonthlyPremium) VALUES(@ChangeDate, @DeathPremium, @IPBPremium, @SumAssured, @YearlyPremium, @PolicyFactor, @DiscountFactor, @UserName, @AccountKey, @MonthlyPremium); select cast(scope_identity() as int)";
        public const string lifepremiumhistorydatamodel_update = "UPDATE [2am].[dbo].[LifePremiumHistory] SET ChangeDate = @ChangeDate, DeathPremium = @DeathPremium, IPBPremium = @IPBPremium, SumAssured = @SumAssured, YearlyPremium = @YearlyPremium, PolicyFactor = @PolicyFactor, DiscountFactor = @DiscountFactor, UserName = @UserName, AccountKey = @AccountKey, MonthlyPremium = @MonthlyPremium WHERE LifePremiumHistoryKey = @LifePremiumHistoryKey";



        public const string clientofferstatusdatamodel_selectwhere = "SELECT ClientOfferStatusKey, Description FROM [2am].[dbo].[ClientOfferStatus] WHERE";
        public const string clientofferstatusdatamodel_selectbykey = "SELECT ClientOfferStatusKey, Description FROM [2am].[dbo].[ClientOfferStatus] WHERE ClientOfferStatusKey = @PrimaryKey";
        public const string clientofferstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[ClientOfferStatus] WHERE ClientOfferStatusKey = @PrimaryKey";
        public const string clientofferstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ClientOfferStatus] WHERE";
        public const string clientofferstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[ClientOfferStatus] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string clientofferstatusdatamodel_update = "UPDATE [2am].[dbo].[ClientOfferStatus] SET Description = @Description WHERE ClientOfferStatusKey = @ClientOfferStatusKey";



        public const string reportstatementdatamodel_selectwhere = "SELECT ReportStatementKey, OriginationSourceProductKey, ReportName, Description, StatementName, GroupBy, OrderBy, ReportGroupKey, FeatureKey, ReportTypeKey, ReportOutputPath FROM [2am].[dbo].[ReportStatement] WHERE";
        public const string reportstatementdatamodel_selectbykey = "SELECT ReportStatementKey, OriginationSourceProductKey, ReportName, Description, StatementName, GroupBy, OrderBy, ReportGroupKey, FeatureKey, ReportTypeKey, ReportOutputPath FROM [2am].[dbo].[ReportStatement] WHERE ReportStatementKey = @PrimaryKey";
        public const string reportstatementdatamodel_delete = "DELETE FROM [2am].[dbo].[ReportStatement] WHERE ReportStatementKey = @PrimaryKey";
        public const string reportstatementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReportStatement] WHERE";
        public const string reportstatementdatamodel_insert = "INSERT INTO [2am].[dbo].[ReportStatement] (ReportStatementKey, OriginationSourceProductKey, ReportName, Description, StatementName, GroupBy, OrderBy, ReportGroupKey, FeatureKey, ReportTypeKey, ReportOutputPath) VALUES(@ReportStatementKey, @OriginationSourceProductKey, @ReportName, @Description, @StatementName, @GroupBy, @OrderBy, @ReportGroupKey, @FeatureKey, @ReportTypeKey, @ReportOutputPath); ";
        public const string reportstatementdatamodel_update = "UPDATE [2am].[dbo].[ReportStatement] SET ReportStatementKey = @ReportStatementKey, OriginationSourceProductKey = @OriginationSourceProductKey, ReportName = @ReportName, Description = @Description, StatementName = @StatementName, GroupBy = @GroupBy, OrderBy = @OrderBy, ReportGroupKey = @ReportGroupKey, FeatureKey = @FeatureKey, ReportTypeKey = @ReportTypeKey, ReportOutputPath = @ReportOutputPath WHERE ReportStatementKey = @ReportStatementKey";



        public const string organisationstructureoriginationsourcedatamodel_selectwhere = "SELECT OrganisationStructureOriginationSourceKey, OrganisationStructureKey, OriginationSourceKey FROM [2am].[dbo].[OrganisationStructureOriginationSource] WHERE";
        public const string organisationstructureoriginationsourcedatamodel_selectbykey = "SELECT OrganisationStructureOriginationSourceKey, OrganisationStructureKey, OriginationSourceKey FROM [2am].[dbo].[OrganisationStructureOriginationSource] WHERE OrganisationStructureOriginationSourceKey = @PrimaryKey";
        public const string organisationstructureoriginationsourcedatamodel_delete = "DELETE FROM [2am].[dbo].[OrganisationStructureOriginationSource] WHERE OrganisationStructureOriginationSourceKey = @PrimaryKey";
        public const string organisationstructureoriginationsourcedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OrganisationStructureOriginationSource] WHERE";
        public const string organisationstructureoriginationsourcedatamodel_insert = "INSERT INTO [2am].[dbo].[OrganisationStructureOriginationSource] (OrganisationStructureKey, OriginationSourceKey) VALUES(@OrganisationStructureKey, @OriginationSourceKey); select cast(scope_identity() as int)";
        public const string organisationstructureoriginationsourcedatamodel_update = "UPDATE [2am].[dbo].[OrganisationStructureOriginationSource] SET OrganisationStructureKey = @OrganisationStructureKey, OriginationSourceKey = @OriginationSourceKey WHERE OrganisationStructureOriginationSourceKey = @OrganisationStructureOriginationSourceKey";



        public const string affordabilitytypedatamodel_selectwhere = "SELECT AffordabilityTypeKey, Description, IsExpense, AffordabilityTypeGroupKey, DescriptionRequired, Sequence FROM [2am].[dbo].[AffordabilityType] WHERE";
        public const string affordabilitytypedatamodel_selectbykey = "SELECT AffordabilityTypeKey, Description, IsExpense, AffordabilityTypeGroupKey, DescriptionRequired, Sequence FROM [2am].[dbo].[AffordabilityType] WHERE AffordabilityTypeKey = @PrimaryKey";
        public const string affordabilitytypedatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityType] WHERE AffordabilityTypeKey = @PrimaryKey";
        public const string affordabilitytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityType] WHERE";
        public const string affordabilitytypedatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityType] (AffordabilityTypeKey, Description, IsExpense, AffordabilityTypeGroupKey, DescriptionRequired, Sequence) VALUES(@AffordabilityTypeKey, @Description, @IsExpense, @AffordabilityTypeGroupKey, @DescriptionRequired, @Sequence); ";
        public const string affordabilitytypedatamodel_update = "UPDATE [2am].[dbo].[AffordabilityType] SET AffordabilityTypeKey = @AffordabilityTypeKey, Description = @Description, IsExpense = @IsExpense, AffordabilityTypeGroupKey = @AffordabilityTypeGroupKey, DescriptionRequired = @DescriptionRequired, Sequence = @Sequence WHERE AffordabilityTypeKey = @AffordabilityTypeKey";



        public const string titletypedatamodel_selectwhere = "SELECT TitleTypeKey, Description FROM [2am].[dbo].[TitleType] WHERE";
        public const string titletypedatamodel_selectbykey = "SELECT TitleTypeKey, Description FROM [2am].[dbo].[TitleType] WHERE TitleTypeKey = @PrimaryKey";
        public const string titletypedatamodel_delete = "DELETE FROM [2am].[dbo].[TitleType] WHERE TitleTypeKey = @PrimaryKey";
        public const string titletypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TitleType] WHERE";
        public const string titletypedatamodel_insert = "INSERT INTO [2am].[dbo].[TitleType] (TitleTypeKey, Description) VALUES(@TitleTypeKey, @Description); ";
        public const string titletypedatamodel_update = "UPDATE [2am].[dbo].[TitleType] SET TitleTypeKey = @TitleTypeKey, Description = @Description WHERE TitleTypeKey = @TitleTypeKey";



        public const string legalentityorganisationstructurehistorydatamodel_selectwhere = "SELECT LegalEntityOrganisationStructureHistoryKey, LegalEntityOrganisationStructureKey, LegalEntityKey, OrganisationStructureKey, ChangeDate, Action FROM [2am].[dbo].[LegalEntityOrganisationStructureHistory] WHERE";
        public const string legalentityorganisationstructurehistorydatamodel_selectbykey = "SELECT LegalEntityOrganisationStructureHistoryKey, LegalEntityOrganisationStructureKey, LegalEntityKey, OrganisationStructureKey, ChangeDate, Action FROM [2am].[dbo].[LegalEntityOrganisationStructureHistory] WHERE LegalEntityOrganisationStructureHistoryKey = @PrimaryKey";
        public const string legalentityorganisationstructurehistorydatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityOrganisationStructureHistory] WHERE LegalEntityOrganisationStructureHistoryKey = @PrimaryKey";
        public const string legalentityorganisationstructurehistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityOrganisationStructureHistory] WHERE";
        public const string legalentityorganisationstructurehistorydatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityOrganisationStructureHistory] (LegalEntityOrganisationStructureKey, LegalEntityKey, OrganisationStructureKey, ChangeDate, Action) VALUES(@LegalEntityOrganisationStructureKey, @LegalEntityKey, @OrganisationStructureKey, @ChangeDate, @Action); select cast(scope_identity() as int)";
        public const string legalentityorganisationstructurehistorydatamodel_update = "UPDATE [2am].[dbo].[LegalEntityOrganisationStructureHistory] SET LegalEntityOrganisationStructureKey = @LegalEntityOrganisationStructureKey, LegalEntityKey = @LegalEntityKey, OrganisationStructureKey = @OrganisationStructureKey, ChangeDate = @ChangeDate, Action = @Action WHERE LegalEntityOrganisationStructureHistoryKey = @LegalEntityOrganisationStructureHistoryKey";



        public const string legalentitytypedatamodel_selectwhere = "SELECT LegalEntityTypeKey, Description FROM [2am].[dbo].[LegalEntityType] WHERE";
        public const string legalentitytypedatamodel_selectbykey = "SELECT LegalEntityTypeKey, Description FROM [2am].[dbo].[LegalEntityType] WHERE LegalEntityTypeKey = @PrimaryKey";
        public const string legalentitytypedatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityType] WHERE LegalEntityTypeKey = @PrimaryKey";
        public const string legalentitytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityType] WHERE";
        public const string legalentitytypedatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityType] (LegalEntityTypeKey, Description) VALUES(@LegalEntityTypeKey, @Description); ";
        public const string legalentitytypedatamodel_update = "UPDATE [2am].[dbo].[LegalEntityType] SET LegalEntityTypeKey = @LegalEntityTypeKey, Description = @Description WHERE LegalEntityTypeKey = @LegalEntityTypeKey";



        public const string originationsourceproductconfigurationdatamodel_selectwhere = "SELECT OSPConfigurationKey, OriginationSourceProductKey, FinancialServiceTypeKey, MarketRateKey, ResetConfigurationKey FROM [2am].[dbo].[OriginationSourceProductConfiguration] WHERE";
        public const string originationsourceproductconfigurationdatamodel_selectbykey = "SELECT OSPConfigurationKey, OriginationSourceProductKey, FinancialServiceTypeKey, MarketRateKey, ResetConfigurationKey FROM [2am].[dbo].[OriginationSourceProductConfiguration] WHERE OSPConfigurationKey = @PrimaryKey";
        public const string originationsourceproductconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceProductConfiguration] WHERE OSPConfigurationKey = @PrimaryKey";
        public const string originationsourceproductconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceProductConfiguration] WHERE";
        public const string originationsourceproductconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceProductConfiguration] (OriginationSourceProductKey, FinancialServiceTypeKey, MarketRateKey, ResetConfigurationKey) VALUES(@OriginationSourceProductKey, @FinancialServiceTypeKey, @MarketRateKey, @ResetConfigurationKey); select cast(scope_identity() as int)";
        public const string originationsourceproductconfigurationdatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceProductConfiguration] SET OriginationSourceProductKey = @OriginationSourceProductKey, FinancialServiceTypeKey = @FinancialServiceTypeKey, MarketRateKey = @MarketRateKey, ResetConfigurationKey = @ResetConfigurationKey WHERE OSPConfigurationKey = @OSPConfigurationKey";



        public const string cdvexceptionsdatamodel_selectwhere = "SELECT CDVExceptionsKey, ACBBankCode, ACBTypeNumber, NoOfDigits, Weightings, Modulus, FudgeFactor, ExceptionCode, UserID, DateChange FROM [2am].[dbo].[CDVExceptions] WHERE";
        public const string cdvexceptionsdatamodel_selectbykey = "SELECT CDVExceptionsKey, ACBBankCode, ACBTypeNumber, NoOfDigits, Weightings, Modulus, FudgeFactor, ExceptionCode, UserID, DateChange FROM [2am].[dbo].[CDVExceptions] WHERE CDVExceptionsKey = @PrimaryKey";
        public const string cdvexceptionsdatamodel_delete = "DELETE FROM [2am].[dbo].[CDVExceptions] WHERE CDVExceptionsKey = @PrimaryKey";
        public const string cdvexceptionsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CDVExceptions] WHERE";
        public const string cdvexceptionsdatamodel_insert = "INSERT INTO [2am].[dbo].[CDVExceptions] (ACBBankCode, ACBTypeNumber, NoOfDigits, Weightings, Modulus, FudgeFactor, ExceptionCode, UserID, DateChange) VALUES(@ACBBankCode, @ACBTypeNumber, @NoOfDigits, @Weightings, @Modulus, @FudgeFactor, @ExceptionCode, @UserID, @DateChange); select cast(scope_identity() as int)";
        public const string cdvexceptionsdatamodel_update = "UPDATE [2am].[dbo].[CDVExceptions] SET ACBBankCode = @ACBBankCode, ACBTypeNumber = @ACBTypeNumber, NoOfDigits = @NoOfDigits, Weightings = @Weightings, Modulus = @Modulus, FudgeFactor = @FudgeFactor, ExceptionCode = @ExceptionCode, UserID = @UserID, DateChange = @DateChange WHERE CDVExceptionsKey = @CDVExceptionsKey";



        public const string offerdeclarationquestionanswerconfigurationdatamodel_selectwhere = "SELECT OfferDeclarationQuestionAnswerConfigurationKey, OfferDeclarationQuestionKey, LegalEntityTypeKey, GenericKey, OriginationSourceProductKey, GenericKeyTypeKey FROM [2am].[dbo].[OfferDeclarationQuestionAnswerConfiguration] WHERE";
        public const string offerdeclarationquestionanswerconfigurationdatamodel_selectbykey = "SELECT OfferDeclarationQuestionAnswerConfigurationKey, OfferDeclarationQuestionKey, LegalEntityTypeKey, GenericKey, OriginationSourceProductKey, GenericKeyTypeKey FROM [2am].[dbo].[OfferDeclarationQuestionAnswerConfiguration] WHERE OfferDeclarationQuestionAnswerConfigurationKey = @PrimaryKey";
        public const string offerdeclarationquestionanswerconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDeclarationQuestionAnswerConfiguration] WHERE OfferDeclarationQuestionAnswerConfigurationKey = @PrimaryKey";
        public const string offerdeclarationquestionanswerconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDeclarationQuestionAnswerConfiguration] WHERE";
        public const string offerdeclarationquestionanswerconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDeclarationQuestionAnswerConfiguration] (OfferDeclarationQuestionKey, LegalEntityTypeKey, GenericKey, OriginationSourceProductKey, GenericKeyTypeKey) VALUES(@OfferDeclarationQuestionKey, @LegalEntityTypeKey, @GenericKey, @OriginationSourceProductKey, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string offerdeclarationquestionanswerconfigurationdatamodel_update = "UPDATE [2am].[dbo].[OfferDeclarationQuestionAnswerConfiguration] SET OfferDeclarationQuestionKey = @OfferDeclarationQuestionKey, LegalEntityTypeKey = @LegalEntityTypeKey, GenericKey = @GenericKey, OriginationSourceProductKey = @OriginationSourceProductKey, GenericKeyTypeKey = @GenericKeyTypeKey WHERE OfferDeclarationQuestionAnswerConfigurationKey = @OfferDeclarationQuestionAnswerConfigurationKey";



        public const string brokerdatamodel_selectwhere = "SELECT BrokerKey, ADUserName, FullName, Initials, TelephoneNumber, FaxNumber, EmailAddress, Password, PasswordQuestion, PasswordAnswer, GeneralStatusKey, ADUserKey, BrokerStatusNumber, BrokerTypeNumber, BrokerCommissionTrigger, BrokerMinimumSAHL, BrokerMinimumSCMB, BrokerPercentageSAHL, BrokerPercentageSCMB, BrokerTarget FROM [2am].[dbo].[Broker] WHERE";
        public const string brokerdatamodel_selectbykey = "SELECT BrokerKey, ADUserName, FullName, Initials, TelephoneNumber, FaxNumber, EmailAddress, Password, PasswordQuestion, PasswordAnswer, GeneralStatusKey, ADUserKey, BrokerStatusNumber, BrokerTypeNumber, BrokerCommissionTrigger, BrokerMinimumSAHL, BrokerMinimumSCMB, BrokerPercentageSAHL, BrokerPercentageSCMB, BrokerTarget FROM [2am].[dbo].[Broker] WHERE BrokerKey = @PrimaryKey";
        public const string brokerdatamodel_delete = "DELETE FROM [2am].[dbo].[Broker] WHERE BrokerKey = @PrimaryKey";
        public const string brokerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Broker] WHERE";
        public const string brokerdatamodel_insert = "INSERT INTO [2am].[dbo].[Broker] (ADUserName, FullName, Initials, TelephoneNumber, FaxNumber, EmailAddress, Password, PasswordQuestion, PasswordAnswer, GeneralStatusKey, ADUserKey, BrokerStatusNumber, BrokerTypeNumber, BrokerCommissionTrigger, BrokerMinimumSAHL, BrokerMinimumSCMB, BrokerPercentageSAHL, BrokerPercentageSCMB, BrokerTarget) VALUES(@ADUserName, @FullName, @Initials, @TelephoneNumber, @FaxNumber, @EmailAddress, @Password, @PasswordQuestion, @PasswordAnswer, @GeneralStatusKey, @ADUserKey, @BrokerStatusNumber, @BrokerTypeNumber, @BrokerCommissionTrigger, @BrokerMinimumSAHL, @BrokerMinimumSCMB, @BrokerPercentageSAHL, @BrokerPercentageSCMB, @BrokerTarget); select cast(scope_identity() as int)";
        public const string brokerdatamodel_update = "UPDATE [2am].[dbo].[Broker] SET ADUserName = @ADUserName, FullName = @FullName, Initials = @Initials, TelephoneNumber = @TelephoneNumber, FaxNumber = @FaxNumber, EmailAddress = @EmailAddress, Password = @Password, PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer, GeneralStatusKey = @GeneralStatusKey, ADUserKey = @ADUserKey, BrokerStatusNumber = @BrokerStatusNumber, BrokerTypeNumber = @BrokerTypeNumber, BrokerCommissionTrigger = @BrokerCommissionTrigger, BrokerMinimumSAHL = @BrokerMinimumSAHL, BrokerMinimumSCMB = @BrokerMinimumSCMB, BrokerPercentageSAHL = @BrokerPercentageSAHL, BrokerPercentageSCMB = @BrokerPercentageSCMB, BrokerTarget = @BrokerTarget WHERE BrokerKey = @BrokerKey";



        public const string applicanttypedatamodel_selectwhere = "SELECT ApplicantTypeKey, Description FROM [2am].[dbo].[ApplicantType] WHERE";
        public const string applicanttypedatamodel_selectbykey = "SELECT ApplicantTypeKey, Description FROM [2am].[dbo].[ApplicantType] WHERE ApplicantTypeKey = @PrimaryKey";
        public const string applicanttypedatamodel_delete = "DELETE FROM [2am].[dbo].[ApplicantType] WHERE ApplicantTypeKey = @PrimaryKey";
        public const string applicanttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ApplicantType] WHERE";
        public const string applicanttypedatamodel_insert = "INSERT INTO [2am].[dbo].[ApplicantType] (ApplicantTypeKey, Description) VALUES(@ApplicantTypeKey, @Description); ";
        public const string applicanttypedatamodel_update = "UPDATE [2am].[dbo].[ApplicantType] SET ApplicantTypeKey = @ApplicantTypeKey, Description = @Description WHERE ApplicantTypeKey = @ApplicantTypeKey";



        public const string santampolicytracking_tempdatamodel_selectwhere = "SELECT SANTAMPolicyTrackingKey, QuoteNumber, PolicyNumber, IDNumber, ActiveDate, CancelDate, MonthlyPremium, CollectionDay, SANTAMPolicyStatus, CampaignTargetContactKey FROM [2am].[dbo].[SANTAMPolicyTracking_TEMP] WHERE";
        public const string santampolicytracking_tempdatamodel_selectbykey = "SELECT SANTAMPolicyTrackingKey, QuoteNumber, PolicyNumber, IDNumber, ActiveDate, CancelDate, MonthlyPremium, CollectionDay, SANTAMPolicyStatus, CampaignTargetContactKey FROM [2am].[dbo].[SANTAMPolicyTracking_TEMP] WHERE  = @PrimaryKey";
        public const string santampolicytracking_tempdatamodel_delete = "DELETE FROM [2am].[dbo].[SANTAMPolicyTracking_TEMP] WHERE  = @PrimaryKey";
        public const string santampolicytracking_tempdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[SANTAMPolicyTracking_TEMP] WHERE";
        public const string santampolicytracking_tempdatamodel_insert = "INSERT INTO [2am].[dbo].[SANTAMPolicyTracking_TEMP] (SANTAMPolicyTrackingKey, QuoteNumber, PolicyNumber, IDNumber, ActiveDate, CancelDate, MonthlyPremium, CollectionDay, SANTAMPolicyStatus, CampaignTargetContactKey) VALUES(@SANTAMPolicyTrackingKey, @QuoteNumber, @PolicyNumber, @IDNumber, @ActiveDate, @CancelDate, @MonthlyPremium, @CollectionDay, @SANTAMPolicyStatus, @CampaignTargetContactKey); ";
        public const string santampolicytracking_tempdatamodel_update = "UPDATE [2am].[dbo].[SANTAMPolicyTracking_TEMP] SET SANTAMPolicyTrackingKey = @SANTAMPolicyTrackingKey, QuoteNumber = @QuoteNumber, PolicyNumber = @PolicyNumber, IDNumber = @IDNumber, ActiveDate = @ActiveDate, CancelDate = @CancelDate, MonthlyPremium = @MonthlyPremium, CollectionDay = @CollectionDay, SANTAMPolicyStatus = @SANTAMPolicyStatus, CampaignTargetContactKey = @CampaignTargetContactKey WHERE  = @";



        public const string loanagreementdatamodel_selectwhere = "SELECT LoanAgreementKey, AgreementDate, Amount, UserName, BondKey, ChangeDate FROM [2am].[dbo].[LoanAgreement] WHERE";
        public const string loanagreementdatamodel_selectbykey = "SELECT LoanAgreementKey, AgreementDate, Amount, UserName, BondKey, ChangeDate FROM [2am].[dbo].[LoanAgreement] WHERE LoanAgreementKey = @PrimaryKey";
        public const string loanagreementdatamodel_delete = "DELETE FROM [2am].[dbo].[LoanAgreement] WHERE LoanAgreementKey = @PrimaryKey";
        public const string loanagreementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LoanAgreement] WHERE";
        public const string loanagreementdatamodel_insert = "INSERT INTO [2am].[dbo].[LoanAgreement] (AgreementDate, Amount, UserName, BondKey, ChangeDate) VALUES(@AgreementDate, @Amount, @UserName, @BondKey, @ChangeDate); select cast(scope_identity() as int)";
        public const string loanagreementdatamodel_update = "UPDATE [2am].[dbo].[LoanAgreement] SET AgreementDate = @AgreementDate, Amount = @Amount, UserName = @UserName, BondKey = @BondKey, ChangeDate = @ChangeDate WHERE LoanAgreementKey = @LoanAgreementKey";



        public const string valuationrooftypedatamodel_selectwhere = "SELECT ValuationRoofTypeKey, Description FROM [2am].[dbo].[ValuationRoofType] WHERE";
        public const string valuationrooftypedatamodel_selectbykey = "SELECT ValuationRoofTypeKey, Description FROM [2am].[dbo].[ValuationRoofType] WHERE ValuationRoofTypeKey = @PrimaryKey";
        public const string valuationrooftypedatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationRoofType] WHERE ValuationRoofTypeKey = @PrimaryKey";
        public const string valuationrooftypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationRoofType] WHERE";
        public const string valuationrooftypedatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationRoofType] (ValuationRoofTypeKey, Description) VALUES(@ValuationRoofTypeKey, @Description); ";
        public const string valuationrooftypedatamodel_update = "UPDATE [2am].[dbo].[ValuationRoofType] SET ValuationRoofTypeKey = @ValuationRoofTypeKey, Description = @Description WHERE ValuationRoofTypeKey = @ValuationRoofTypeKey";



        public const string workflowroletypeorganisationstructuremappingdatamodel_selectwhere = "SELECT WorkflowRoleTypeOrganisationStructureMappingKey, WorkflowRoleTypeKey, OrganisationStructureKey FROM [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] WHERE";
        public const string workflowroletypeorganisationstructuremappingdatamodel_selectbykey = "SELECT WorkflowRoleTypeOrganisationStructureMappingKey, WorkflowRoleTypeKey, OrganisationStructureKey FROM [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] WHERE WorkflowRoleTypeOrganisationStructureMappingKey = @PrimaryKey";
        public const string workflowroletypeorganisationstructuremappingdatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] WHERE WorkflowRoleTypeOrganisationStructureMappingKey = @PrimaryKey";
        public const string workflowroletypeorganisationstructuremappingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] WHERE";
        public const string workflowroletypeorganisationstructuremappingdatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] (WorkflowRoleTypeKey, OrganisationStructureKey) VALUES(@WorkflowRoleTypeKey, @OrganisationStructureKey); select cast(scope_identity() as int)";
        public const string workflowroletypeorganisationstructuremappingdatamodel_update = "UPDATE [2am].[dbo].[WorkflowRoleTypeOrganisationStructureMapping] SET WorkflowRoleTypeKey = @WorkflowRoleTypeKey, OrganisationStructureKey = @OrganisationStructureKey WHERE WorkflowRoleTypeOrganisationStructureMappingKey = @WorkflowRoleTypeOrganisationStructureMappingKey";



        public const string auditoriginationsourceproductconfigurationdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, OSPConfigurationKey, OriginationSourceProductKey, FinancialServiceTypeKey, MarketRateKey, ResetConfigurationKey, DiscountPercentage FROM [2am].[dbo].[AuditOriginationSourceProductConfiguration] WHERE";
        public const string auditoriginationsourceproductconfigurationdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, OSPConfigurationKey, OriginationSourceProductKey, FinancialServiceTypeKey, MarketRateKey, ResetConfigurationKey, DiscountPercentage FROM [2am].[dbo].[AuditOriginationSourceProductConfiguration] WHERE AuditNumber = @PrimaryKey";
        public const string auditoriginationsourceproductconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditOriginationSourceProductConfiguration] WHERE AuditNumber = @PrimaryKey";
        public const string auditoriginationsourceproductconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditOriginationSourceProductConfiguration] WHERE";
        public const string auditoriginationsourceproductconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditOriginationSourceProductConfiguration] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, OSPConfigurationKey, OriginationSourceProductKey, FinancialServiceTypeKey, MarketRateKey, ResetConfigurationKey, DiscountPercentage) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @OSPConfigurationKey, @OriginationSourceProductKey, @FinancialServiceTypeKey, @MarketRateKey, @ResetConfigurationKey, @DiscountPercentage); select cast(scope_identity() as int)";
        public const string auditoriginationsourceproductconfigurationdatamodel_update = "UPDATE [2am].[dbo].[AuditOriginationSourceProductConfiguration] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, OSPConfigurationKey = @OSPConfigurationKey, OriginationSourceProductKey = @OriginationSourceProductKey, FinancialServiceTypeKey = @FinancialServiceTypeKey, MarketRateKey = @MarketRateKey, ResetConfigurationKey = @ResetConfigurationKey, DiscountPercentage = @DiscountPercentage WHERE AuditNumber = @AuditNumber";



        public const string santampolicystatusdatamodel_selectwhere = "SELECT SANTAMPolicyStatusKey, Description FROM [2am].[dbo].[SANTAMPolicyStatus] WHERE";
        public const string santampolicystatusdatamodel_selectbykey = "SELECT SANTAMPolicyStatusKey, Description FROM [2am].[dbo].[SANTAMPolicyStatus] WHERE SANTAMPolicyStatusKey = @PrimaryKey";
        public const string santampolicystatusdatamodel_delete = "DELETE FROM [2am].[dbo].[SANTAMPolicyStatus] WHERE SANTAMPolicyStatusKey = @PrimaryKey";
        public const string santampolicystatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[SANTAMPolicyStatus] WHERE";
        public const string santampolicystatusdatamodel_insert = "INSERT INTO [2am].[dbo].[SANTAMPolicyStatus] (SANTAMPolicyStatusKey, Description) VALUES(@SANTAMPolicyStatusKey, @Description); ";
        public const string santampolicystatusdatamodel_update = "UPDATE [2am].[dbo].[SANTAMPolicyStatus] SET SANTAMPolicyStatusKey = @SANTAMPolicyStatusKey, Description = @Description WHERE SANTAMPolicyStatusKey = @SANTAMPolicyStatusKey";



        public const string financialserviceconditiondatamodel_selectwhere = "SELECT FinancialServiceConditionKey, FinancialServiceKey, UserDefinedConditionText, ConditionTypeKey, ConditionKey FROM [2am].[dbo].[FinancialServiceCondition] WHERE";
        public const string financialserviceconditiondatamodel_selectbykey = "SELECT FinancialServiceConditionKey, FinancialServiceKey, UserDefinedConditionText, ConditionTypeKey, ConditionKey FROM [2am].[dbo].[FinancialServiceCondition] WHERE FinancialServiceConditionKey = @PrimaryKey";
        public const string financialserviceconditiondatamodel_delete = "DELETE FROM [2am].[dbo].[FinancialServiceCondition] WHERE FinancialServiceConditionKey = @PrimaryKey";
        public const string financialserviceconditiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FinancialServiceCondition] WHERE";
        public const string financialserviceconditiondatamodel_insert = "INSERT INTO [2am].[dbo].[FinancialServiceCondition] (FinancialServiceKey, UserDefinedConditionText, ConditionTypeKey, ConditionKey) VALUES(@FinancialServiceKey, @UserDefinedConditionText, @ConditionTypeKey, @ConditionKey); select cast(scope_identity() as int)";
        public const string financialserviceconditiondatamodel_update = "UPDATE [2am].[dbo].[FinancialServiceCondition] SET FinancialServiceKey = @FinancialServiceKey, UserDefinedConditionText = @UserDefinedConditionText, ConditionTypeKey = @ConditionTypeKey, ConditionKey = @ConditionKey WHERE FinancialServiceConditionKey = @FinancialServiceConditionKey";



        public const string originationsourceproductfundingdatamodel_selectwhere = "SELECT OriginationSourceKey, ProductKey, OriginationSourceProductKey, LTV, LinkRate, FundingWarehouse FROM [2am].[dbo].[OriginationSourceProductFunding] WHERE";
        public const string originationsourceproductfundingdatamodel_selectbykey = "SELECT OriginationSourceKey, ProductKey, OriginationSourceProductKey, LTV, LinkRate, FundingWarehouse FROM [2am].[dbo].[OriginationSourceProductFunding] WHERE  = @PrimaryKey";
        public const string originationsourceproductfundingdatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceProductFunding] WHERE  = @PrimaryKey";
        public const string originationsourceproductfundingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceProductFunding] WHERE";
        public const string originationsourceproductfundingdatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceProductFunding] (OriginationSourceKey, ProductKey, OriginationSourceProductKey, LTV, LinkRate, FundingWarehouse) VALUES(@OriginationSourceKey, @ProductKey, @OriginationSourceProductKey, @LTV, @LinkRate, @FundingWarehouse); ";
        public const string originationsourceproductfundingdatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceProductFunding] SET OriginationSourceKey = @OriginationSourceKey, ProductKey = @ProductKey, OriginationSourceProductKey = @OriginationSourceProductKey, LTV = @LTV, LinkRate = @LinkRate, FundingWarehouse = @FundingWarehouse WHERE  = @";



        public const string assetliabilitytypedatamodel_selectwhere = "SELECT AssetLiabilityTypeKey, Description FROM [2am].[dbo].[AssetLiabilityType] WHERE";
        public const string assetliabilitytypedatamodel_selectbykey = "SELECT AssetLiabilityTypeKey, Description FROM [2am].[dbo].[AssetLiabilityType] WHERE AssetLiabilityTypeKey = @PrimaryKey";
        public const string assetliabilitytypedatamodel_delete = "DELETE FROM [2am].[dbo].[AssetLiabilityType] WHERE AssetLiabilityTypeKey = @PrimaryKey";
        public const string assetliabilitytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AssetLiabilityType] WHERE";
        public const string assetliabilitytypedatamodel_insert = "INSERT INTO [2am].[dbo].[AssetLiabilityType] (AssetLiabilityTypeKey, Description) VALUES(@AssetLiabilityTypeKey, @Description); ";
        public const string assetliabilitytypedatamodel_update = "UPDATE [2am].[dbo].[AssetLiabilityType] SET AssetLiabilityTypeKey = @AssetLiabilityTypeKey, Description = @Description WHERE AssetLiabilityTypeKey = @AssetLiabilityTypeKey";



        public const string commissiontransactiondatamodel_selectwhere = "SELECT CommissionTransactionKey, FinancialServiceKey, FinancialServiceTypeKey, CommissionCalcAmount, CommissionAmount, CommissionFactor, CommissionType, KickerCalcAmount, KickerAmount, TransactionDate, BatchTypeKey, BatchRunDate, ADUserKey FROM [2am].[dbo].[CommissionTransaction] WHERE";
        public const string commissiontransactiondatamodel_selectbykey = "SELECT CommissionTransactionKey, FinancialServiceKey, FinancialServiceTypeKey, CommissionCalcAmount, CommissionAmount, CommissionFactor, CommissionType, KickerCalcAmount, KickerAmount, TransactionDate, BatchTypeKey, BatchRunDate, ADUserKey FROM [2am].[dbo].[CommissionTransaction] WHERE CommissionTransactionKey = @PrimaryKey";
        public const string commissiontransactiondatamodel_delete = "DELETE FROM [2am].[dbo].[CommissionTransaction] WHERE CommissionTransactionKey = @PrimaryKey";
        public const string commissiontransactiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CommissionTransaction] WHERE";
        public const string commissiontransactiondatamodel_insert = "INSERT INTO [2am].[dbo].[CommissionTransaction] (FinancialServiceKey, FinancialServiceTypeKey, CommissionCalcAmount, CommissionAmount, CommissionFactor, CommissionType, KickerCalcAmount, KickerAmount, TransactionDate, BatchTypeKey, BatchRunDate, ADUserKey) VALUES(@FinancialServiceKey, @FinancialServiceTypeKey, @CommissionCalcAmount, @CommissionAmount, @CommissionFactor, @CommissionType, @KickerCalcAmount, @KickerAmount, @TransactionDate, @BatchTypeKey, @BatchRunDate, @ADUserKey); select cast(scope_identity() as int)";
        public const string commissiontransactiondatamodel_update = "UPDATE [2am].[dbo].[CommissionTransaction] SET FinancialServiceKey = @FinancialServiceKey, FinancialServiceTypeKey = @FinancialServiceTypeKey, CommissionCalcAmount = @CommissionCalcAmount, CommissionAmount = @CommissionAmount, CommissionFactor = @CommissionFactor, CommissionType = @CommissionType, KickerCalcAmount = @KickerCalcAmount, KickerAmount = @KickerAmount, TransactionDate = @TransactionDate, BatchTypeKey = @BatchTypeKey, BatchRunDate = @BatchRunDate, ADUserKey = @ADUserKey WHERE CommissionTransactionKey = @CommissionTransactionKey";



        public const string creditscoredecisiondatamodel_selectwhere = "SELECT CreditScoreDecisionKey, Description FROM [2am].[dbo].[CreditScoreDecision] WHERE";
        public const string creditscoredecisiondatamodel_selectbykey = "SELECT CreditScoreDecisionKey, Description FROM [2am].[dbo].[CreditScoreDecision] WHERE CreditScoreDecisionKey = @PrimaryKey";
        public const string creditscoredecisiondatamodel_delete = "DELETE FROM [2am].[dbo].[CreditScoreDecision] WHERE CreditScoreDecisionKey = @PrimaryKey";
        public const string creditscoredecisiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CreditScoreDecision] WHERE";
        public const string creditscoredecisiondatamodel_insert = "INSERT INTO [2am].[dbo].[CreditScoreDecision] (CreditScoreDecisionKey, Description) VALUES(@CreditScoreDecisionKey, @Description); ";
        public const string creditscoredecisiondatamodel_update = "UPDATE [2am].[dbo].[CreditScoreDecision] SET CreditScoreDecisionKey = @CreditScoreDecisionKey, Description = @Description WHERE CreditScoreDecisionKey = @CreditScoreDecisionKey";



        public const string courierdatamodel_selectwhere = "SELECT CourierKey, CourierName, EmailAddress FROM [2am].[dbo].[Courier] WHERE";
        public const string courierdatamodel_selectbykey = "SELECT CourierKey, CourierName, EmailAddress FROM [2am].[dbo].[Courier] WHERE CourierKey = @PrimaryKey";
        public const string courierdatamodel_delete = "DELETE FROM [2am].[dbo].[Courier] WHERE CourierKey = @PrimaryKey";
        public const string courierdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Courier] WHERE";
        public const string courierdatamodel_insert = "INSERT INTO [2am].[dbo].[Courier] (CourierName, EmailAddress) VALUES(@CourierName, @EmailAddress); select cast(scope_identity() as int)";
        public const string courierdatamodel_update = "UPDATE [2am].[dbo].[Courier] SET CourierName = @CourierName, EmailAddress = @EmailAddress WHERE CourierKey = @CourierKey";



        public const string affordabilityassessmentitemdatamodel_selectwhere = "SELECT AffordabilityAssessmentItemKey, AffordabilityAssessmentKey, AffordabilityAssessmentItemTypeKey, ModifiedDate, ModifiedByUserId, ClientValue, CreditValue, DebtToConsolidateValue, ItemNotes FROM [2am].[dbo].[AffordabilityAssessmentItem] WHERE";
        public const string affordabilityassessmentitemdatamodel_selectbykey = "SELECT AffordabilityAssessmentItemKey, AffordabilityAssessmentKey, AffordabilityAssessmentItemTypeKey, ModifiedDate, ModifiedByUserId, ClientValue, CreditValue, DebtToConsolidateValue, ItemNotes FROM [2am].[dbo].[AffordabilityAssessmentItem] WHERE AffordabilityAssessmentItemKey = @PrimaryKey";
        public const string affordabilityassessmentitemdatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentItem] WHERE AffordabilityAssessmentItemKey = @PrimaryKey";
        public const string affordabilityassessmentitemdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentItem] WHERE";
        public const string affordabilityassessmentitemdatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityAssessmentItem] (AffordabilityAssessmentKey, AffordabilityAssessmentItemTypeKey, ModifiedDate, ModifiedByUserId, ClientValue, CreditValue, DebtToConsolidateValue, ItemNotes) VALUES(@AffordabilityAssessmentKey, @AffordabilityAssessmentItemTypeKey, @ModifiedDate, @ModifiedByUserId, @ClientValue, @CreditValue, @DebtToConsolidateValue, @ItemNotes); select cast(scope_identity() as int)";
        public const string affordabilityassessmentitemdatamodel_update = "UPDATE [2am].[dbo].[AffordabilityAssessmentItem] SET AffordabilityAssessmentKey = @AffordabilityAssessmentKey, AffordabilityAssessmentItemTypeKey = @AffordabilityAssessmentItemTypeKey, ModifiedDate = @ModifiedDate, ModifiedByUserId = @ModifiedByUserId, ClientValue = @ClientValue, CreditValue = @CreditValue, DebtToConsolidateValue = @DebtToConsolidateValue, ItemNotes = @ItemNotes WHERE AffordabilityAssessmentItemKey = @AffordabilityAssessmentItemKey";



        public const string lifeinsurableinteresttypedatamodel_selectwhere = "SELECT LifeInsurableInterestTypeKey, Description FROM [2am].[dbo].[LifeInsurableInterestType] WHERE";
        public const string lifeinsurableinteresttypedatamodel_selectbykey = "SELECT LifeInsurableInterestTypeKey, Description FROM [2am].[dbo].[LifeInsurableInterestType] WHERE LifeInsurableInterestTypeKey = @PrimaryKey";
        public const string lifeinsurableinteresttypedatamodel_delete = "DELETE FROM [2am].[dbo].[LifeInsurableInterestType] WHERE LifeInsurableInterestTypeKey = @PrimaryKey";
        public const string lifeinsurableinteresttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifeInsurableInterestType] WHERE";
        public const string lifeinsurableinteresttypedatamodel_insert = "INSERT INTO [2am].[dbo].[LifeInsurableInterestType] (LifeInsurableInterestTypeKey, Description) VALUES(@LifeInsurableInterestTypeKey, @Description); ";
        public const string lifeinsurableinteresttypedatamodel_update = "UPDATE [2am].[dbo].[LifeInsurableInterestType] SET LifeInsurableInterestTypeKey = @LifeInsurableInterestTypeKey, Description = @Description WHERE LifeInsurableInterestTypeKey = @LifeInsurableInterestTypeKey";



        public const string functionalgroupdefinitiondatamodel_selectwhere = "SELECT FunctionalGroupDefinitionKey, FunctionalGroupName, GenericKeyTypeKey, AllowMany FROM [2am].[dbo].[FunctionalGroupDefinition] WHERE";
        public const string functionalgroupdefinitiondatamodel_selectbykey = "SELECT FunctionalGroupDefinitionKey, FunctionalGroupName, GenericKeyTypeKey, AllowMany FROM [2am].[dbo].[FunctionalGroupDefinition] WHERE FunctionalGroupDefinitionKey = @PrimaryKey";
        public const string functionalgroupdefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[FunctionalGroupDefinition] WHERE FunctionalGroupDefinitionKey = @PrimaryKey";
        public const string functionalgroupdefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FunctionalGroupDefinition] WHERE";
        public const string functionalgroupdefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[FunctionalGroupDefinition] (FunctionalGroupName, GenericKeyTypeKey, AllowMany) VALUES(@FunctionalGroupName, @GenericKeyTypeKey, @AllowMany); select cast(scope_identity() as int)";
        public const string functionalgroupdefinitiondatamodel_update = "UPDATE [2am].[dbo].[FunctionalGroupDefinition] SET FunctionalGroupName = @FunctionalGroupName, GenericKeyTypeKey = @GenericKeyTypeKey, AllowMany = @AllowMany WHERE FunctionalGroupDefinitionKey = @FunctionalGroupDefinitionKey";



        public const string lifecommissiontargetsdatamodel_selectwhere = "SELECT TargetKey, Consultant, EffectiveYear, EffectiveMonth, TargetPolicies, MinPoliciesToQualify FROM [2am].[dbo].[LifeCommissionTargets] WHERE";
        public const string lifecommissiontargetsdatamodel_selectbykey = "SELECT TargetKey, Consultant, EffectiveYear, EffectiveMonth, TargetPolicies, MinPoliciesToQualify FROM [2am].[dbo].[LifeCommissionTargets] WHERE TargetKey = @PrimaryKey";
        public const string lifecommissiontargetsdatamodel_delete = "DELETE FROM [2am].[dbo].[LifeCommissionTargets] WHERE TargetKey = @PrimaryKey";
        public const string lifecommissiontargetsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifeCommissionTargets] WHERE";
        public const string lifecommissiontargetsdatamodel_insert = "INSERT INTO [2am].[dbo].[LifeCommissionTargets] (Consultant, EffectiveYear, EffectiveMonth, TargetPolicies, MinPoliciesToQualify) VALUES(@Consultant, @EffectiveYear, @EffectiveMonth, @TargetPolicies, @MinPoliciesToQualify); select cast(scope_identity() as int)";
        public const string lifecommissiontargetsdatamodel_update = "UPDATE [2am].[dbo].[LifeCommissionTargets] SET Consultant = @Consultant, EffectiveYear = @EffectiveYear, EffectiveMonth = @EffectiveMonth, TargetPolicies = @TargetPolicies, MinPoliciesToQualify = @MinPoliciesToQualify WHERE TargetKey = @TargetKey";



        public const string methodprofilerdatamodel_selectwhere = "SELECT id, CallingTime, MethodName, ClassName, TotalSeconds, InnerMethods, ThreadID, CallingApp FROM [2am].[dbo].[MethodProfiler] WHERE";
        public const string methodprofilerdatamodel_selectbykey = "SELECT id, CallingTime, MethodName, ClassName, TotalSeconds, InnerMethods, ThreadID, CallingApp FROM [2am].[dbo].[MethodProfiler] WHERE id = @PrimaryKey";
        public const string methodprofilerdatamodel_delete = "DELETE FROM [2am].[dbo].[MethodProfiler] WHERE id = @PrimaryKey";
        public const string methodprofilerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MethodProfiler] WHERE";
        public const string methodprofilerdatamodel_insert = "INSERT INTO [2am].[dbo].[MethodProfiler] (CallingTime, MethodName, ClassName, TotalSeconds, InnerMethods, ThreadID, CallingApp) VALUES(@CallingTime, @MethodName, @ClassName, @TotalSeconds, @InnerMethods, @ThreadID, @CallingApp); select cast(scope_identity() as int)";
        public const string methodprofilerdatamodel_update = "UPDATE [2am].[dbo].[MethodProfiler] SET CallingTime = @CallingTime, MethodName = @MethodName, ClassName = @ClassName, TotalSeconds = @TotalSeconds, InnerMethods = @InnerMethods, ThreadID = @ThreadID, CallingApp = @CallingApp WHERE id = @id";



        public const string hoctransactionsdatamodel_selectwhere = "SELECT TransactionKey, AccountKey, Action, UploadDate, Reason, InsertDate FROM [2am].[dbo].[HOCTransactions] WHERE";
        public const string hoctransactionsdatamodel_selectbykey = "SELECT TransactionKey, AccountKey, Action, UploadDate, Reason, InsertDate FROM [2am].[dbo].[HOCTransactions] WHERE TransactionKey = @PrimaryKey";
        public const string hoctransactionsdatamodel_delete = "DELETE FROM [2am].[dbo].[HOCTransactions] WHERE TransactionKey = @PrimaryKey";
        public const string hoctransactionsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCTransactions] WHERE";
        public const string hoctransactionsdatamodel_insert = "INSERT INTO [2am].[dbo].[HOCTransactions] (AccountKey, Action, UploadDate, Reason, InsertDate) VALUES(@AccountKey, @Action, @UploadDate, @Reason, @InsertDate); select cast(scope_identity() as int)";
        public const string hoctransactionsdatamodel_update = "UPDATE [2am].[dbo].[HOCTransactions] SET AccountKey = @AccountKey, Action = @Action, UploadDate = @UploadDate, Reason = @Reason, InsertDate = @InsertDate WHERE TransactionKey = @TransactionKey";



        public const string offerroletypeorganisationstructuremappingdatamodel_selectwhere = "SELECT OfferRoleTypeOrganisationStructureMappingKey, OfferRoleTypeKey, OrganisationStructureKey FROM [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] WHERE";
        public const string offerroletypeorganisationstructuremappingdatamodel_selectbykey = "SELECT OfferRoleTypeOrganisationStructureMappingKey, OfferRoleTypeKey, OrganisationStructureKey FROM [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] WHERE OfferRoleTypeOrganisationStructureMappingKey = @PrimaryKey";
        public const string offerroletypeorganisationstructuremappingdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] WHERE OfferRoleTypeOrganisationStructureMappingKey = @PrimaryKey";
        public const string offerroletypeorganisationstructuremappingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] WHERE";
        public const string offerroletypeorganisationstructuremappingdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] (OfferRoleTypeKey, OrganisationStructureKey) VALUES(@OfferRoleTypeKey, @OrganisationStructureKey); select cast(scope_identity() as int)";
        public const string offerroletypeorganisationstructuremappingdatamodel_update = "UPDATE [2am].[dbo].[OfferRoleTypeOrganisationStructureMapping] SET OfferRoleTypeKey = @OfferRoleTypeKey, OrganisationStructureKey = @OrganisationStructureKey WHERE OfferRoleTypeOrganisationStructureMappingKey = @OfferRoleTypeOrganisationStructureMappingKey";



        public const string offeraggregatedecisiondatamodel_selectwhere = "SELECT OfferAggregateDecisionKey, PrimaryDecision, SecondaryDecision, AggregateDecision FROM [2am].[dbo].[OfferAggregateDecision] WHERE";
        public const string offeraggregatedecisiondatamodel_selectbykey = "SELECT OfferAggregateDecisionKey, PrimaryDecision, SecondaryDecision, AggregateDecision FROM [2am].[dbo].[OfferAggregateDecision] WHERE OfferAggregateDecisionKey = @PrimaryKey";
        public const string offeraggregatedecisiondatamodel_delete = "DELETE FROM [2am].[dbo].[OfferAggregateDecision] WHERE OfferAggregateDecisionKey = @PrimaryKey";
        public const string offeraggregatedecisiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferAggregateDecision] WHERE";
        public const string offeraggregatedecisiondatamodel_insert = "INSERT INTO [2am].[dbo].[OfferAggregateDecision] (OfferAggregateDecisionKey, PrimaryDecision, SecondaryDecision, AggregateDecision) VALUES(@OfferAggregateDecisionKey, @PrimaryDecision, @SecondaryDecision, @AggregateDecision); ";
        public const string offeraggregatedecisiondatamodel_update = "UPDATE [2am].[dbo].[OfferAggregateDecision] SET OfferAggregateDecisionKey = @OfferAggregateDecisionKey, PrimaryDecision = @PrimaryDecision, SecondaryDecision = @SecondaryDecision, AggregateDecision = @AggregateDecision WHERE OfferAggregateDecisionKey = @OfferAggregateDecisionKey";



        public const string santampolicytrackingdatamodel_selectwhere = "SELECT SANTAMPolicyTrackingPrimaryKey, SANTAMPolicyTrackingKey, PolicyNumber, QuoteNumber, CampaignTargetContactKey, LegalEntityKey, AccountKey, ActiveDate, CancelDate, MonthlyPremium, CollectionDay, SANTAMPolicyStatusKey FROM [2am].[dbo].[SANTAMPolicyTracking] WHERE";
        public const string santampolicytrackingdatamodel_selectbykey = "SELECT SANTAMPolicyTrackingPrimaryKey, SANTAMPolicyTrackingKey, PolicyNumber, QuoteNumber, CampaignTargetContactKey, LegalEntityKey, AccountKey, ActiveDate, CancelDate, MonthlyPremium, CollectionDay, SANTAMPolicyStatusKey FROM [2am].[dbo].[SANTAMPolicyTracking] WHERE SANTAMPolicyTrackingPrimaryKey = @PrimaryKey";
        public const string santampolicytrackingdatamodel_delete = "DELETE FROM [2am].[dbo].[SANTAMPolicyTracking] WHERE SANTAMPolicyTrackingPrimaryKey = @PrimaryKey";
        public const string santampolicytrackingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[SANTAMPolicyTracking] WHERE";
        public const string santampolicytrackingdatamodel_insert = "INSERT INTO [2am].[dbo].[SANTAMPolicyTracking] (SANTAMPolicyTrackingKey, PolicyNumber, QuoteNumber, CampaignTargetContactKey, LegalEntityKey, AccountKey, ActiveDate, CancelDate, MonthlyPremium, CollectionDay, SANTAMPolicyStatusKey) VALUES(@SANTAMPolicyTrackingKey, @PolicyNumber, @QuoteNumber, @CampaignTargetContactKey, @LegalEntityKey, @AccountKey, @ActiveDate, @CancelDate, @MonthlyPremium, @CollectionDay, @SANTAMPolicyStatusKey); select cast(scope_identity() as int)";
        public const string santampolicytrackingdatamodel_update = "UPDATE [2am].[dbo].[SANTAMPolicyTracking] SET SANTAMPolicyTrackingKey = @SANTAMPolicyTrackingKey, PolicyNumber = @PolicyNumber, QuoteNumber = @QuoteNumber, CampaignTargetContactKey = @CampaignTargetContactKey, LegalEntityKey = @LegalEntityKey, AccountKey = @AccountKey, ActiveDate = @ActiveDate, CancelDate = @CancelDate, MonthlyPremium = @MonthlyPremium, CollectionDay = @CollectionDay, SANTAMPolicyStatusKey = @SANTAMPolicyStatusKey WHERE SANTAMPolicyTrackingPrimaryKey = @SANTAMPolicyTrackingPrimaryKey";



        public const string courierappointmentdatamodel_selectwhere = "SELECT CourierAppointmentKey, CourierKey, AccountKey, AppointmentDate, Notes FROM [2am].[dbo].[CourierAppointment] WHERE";
        public const string courierappointmentdatamodel_selectbykey = "SELECT CourierAppointmentKey, CourierKey, AccountKey, AppointmentDate, Notes FROM [2am].[dbo].[CourierAppointment] WHERE CourierAppointmentKey = @PrimaryKey";
        public const string courierappointmentdatamodel_delete = "DELETE FROM [2am].[dbo].[CourierAppointment] WHERE CourierAppointmentKey = @PrimaryKey";
        public const string courierappointmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CourierAppointment] WHERE";
        public const string courierappointmentdatamodel_insert = "INSERT INTO [2am].[dbo].[CourierAppointment] (CourierKey, AccountKey, AppointmentDate, Notes) VALUES(@CourierKey, @AccountKey, @AppointmentDate, @Notes); select cast(scope_identity() as int)";
        public const string courierappointmentdatamodel_update = "UPDATE [2am].[dbo].[CourierAppointment] SET CourierKey = @CourierKey, AccountKey = @AccountKey, AppointmentDate = @AppointmentDate, Notes = @Notes WHERE CourierAppointmentKey = @CourierAppointmentKey";



        public const string creditcriteriadatamodel_selectwhere = "SELECT CreditCriteriaKey, CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria, MaxIncomeAmount, MinEmpiricaScore FROM [2am].[dbo].[CreditCriteria] WHERE";
        public const string creditcriteriadatamodel_selectbykey = "SELECT CreditCriteriaKey, CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria, MaxIncomeAmount, MinEmpiricaScore FROM [2am].[dbo].[CreditCriteria] WHERE CreditCriteriaKey = @PrimaryKey";
        public const string creditcriteriadatamodel_delete = "DELETE FROM [2am].[dbo].[CreditCriteria] WHERE CreditCriteriaKey = @PrimaryKey";
        public const string creditcriteriadatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CreditCriteria] WHERE";
        public const string creditcriteriadatamodel_insert = "INSERT INTO [2am].[dbo].[CreditCriteria] (CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria, MaxIncomeAmount, MinEmpiricaScore) VALUES(@CreditMatrixKey, @MarginKey, @CategoryKey, @EmploymentTypeKey, @MortgageLoanPurposeKey, @MinLoanAmount, @MaxLoanAmount, @MinPropertyValue, @MaxPropertyValue, @LTV, @PTI, @MinIncomeAmount, @ExceptionCriteria, @MaxIncomeAmount, @MinEmpiricaScore); select cast(scope_identity() as int)";
        public const string creditcriteriadatamodel_update = "UPDATE [2am].[dbo].[CreditCriteria] SET CreditMatrixKey = @CreditMatrixKey, MarginKey = @MarginKey, CategoryKey = @CategoryKey, EmploymentTypeKey = @EmploymentTypeKey, MortgageLoanPurposeKey = @MortgageLoanPurposeKey, MinLoanAmount = @MinLoanAmount, MaxLoanAmount = @MaxLoanAmount, MinPropertyValue = @MinPropertyValue, MaxPropertyValue = @MaxPropertyValue, LTV = @LTV, PTI = @PTI, MinIncomeAmount = @MinIncomeAmount, ExceptionCriteria = @ExceptionCriteria, MaxIncomeAmount = @MaxIncomeAmount, MinEmpiricaScore = @MinEmpiricaScore WHERE CreditCriteriaKey = @CreditCriteriaKey";



        public const string lifecommissionsecuritydatamodel_selectwhere = "SELECT SecurityKey, UserID, Administrator FROM [2am].[dbo].[LifeCommissionSecurity] WHERE";
        public const string lifecommissionsecuritydatamodel_selectbykey = "SELECT SecurityKey, UserID, Administrator FROM [2am].[dbo].[LifeCommissionSecurity] WHERE SecurityKey = @PrimaryKey";
        public const string lifecommissionsecuritydatamodel_delete = "DELETE FROM [2am].[dbo].[LifeCommissionSecurity] WHERE SecurityKey = @PrimaryKey";
        public const string lifecommissionsecuritydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifeCommissionSecurity] WHERE";
        public const string lifecommissionsecuritydatamodel_insert = "INSERT INTO [2am].[dbo].[LifeCommissionSecurity] (UserID, Administrator) VALUES(@UserID, @Administrator); select cast(scope_identity() as int)";
        public const string lifecommissionsecuritydatamodel_update = "UPDATE [2am].[dbo].[LifeCommissionSecurity] SET UserID = @UserID, Administrator = @Administrator WHERE SecurityKey = @SecurityKey";



        public const string reasondescriptiondatamodel_selectwhere = "SELECT ReasonDescriptionKey, Description, TranslatableItemKey FROM [2am].[dbo].[ReasonDescription] WHERE";
        public const string reasondescriptiondatamodel_selectbykey = "SELECT ReasonDescriptionKey, Description, TranslatableItemKey FROM [2am].[dbo].[ReasonDescription] WHERE ReasonDescriptionKey = @PrimaryKey";
        public const string reasondescriptiondatamodel_delete = "DELETE FROM [2am].[dbo].[ReasonDescription] WHERE ReasonDescriptionKey = @PrimaryKey";
        public const string reasondescriptiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReasonDescription] WHERE";
        public const string reasondescriptiondatamodel_insert = "INSERT INTO [2am].[dbo].[ReasonDescription] (ReasonDescriptionKey, Description, TranslatableItemKey) VALUES(@ReasonDescriptionKey, @Description, @TranslatableItemKey); ";
        public const string reasondescriptiondatamodel_update = "UPDATE [2am].[dbo].[ReasonDescription] SET ReasonDescriptionKey = @ReasonDescriptionKey, Description = @Description, TranslatableItemKey = @TranslatableItemKey WHERE ReasonDescriptionKey = @ReasonDescriptionKey";



        public const string creditmatrixunsecuredlendingdatamodel_selectwhere = "SELECT CreditMatrixUnsecuredLendingKey, NewBusinessIndicator, ImplementationDate FROM [2am].[dbo].[CreditMatrixUnsecuredLending] WHERE";
        public const string creditmatrixunsecuredlendingdatamodel_selectbykey = "SELECT CreditMatrixUnsecuredLendingKey, NewBusinessIndicator, ImplementationDate FROM [2am].[dbo].[CreditMatrixUnsecuredLending] WHERE CreditMatrixUnsecuredLendingKey = @PrimaryKey";
        public const string creditmatrixunsecuredlendingdatamodel_delete = "DELETE FROM [2am].[dbo].[CreditMatrixUnsecuredLending] WHERE CreditMatrixUnsecuredLendingKey = @PrimaryKey";
        public const string creditmatrixunsecuredlendingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CreditMatrixUnsecuredLending] WHERE";
        public const string creditmatrixunsecuredlendingdatamodel_insert = "INSERT INTO [2am].[dbo].[CreditMatrixUnsecuredLending] (NewBusinessIndicator, ImplementationDate) VALUES(@NewBusinessIndicator, @ImplementationDate); select cast(scope_identity() as int)";
        public const string creditmatrixunsecuredlendingdatamodel_update = "UPDATE [2am].[dbo].[CreditMatrixUnsecuredLending] SET NewBusinessIndicator = @NewBusinessIndicator, ImplementationDate = @ImplementationDate WHERE CreditMatrixUnsecuredLendingKey = @CreditMatrixUnsecuredLendingKey";



        public const string auditaccountsubsidydatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountSubsidyKey, AccountKey, SubsidyKey FROM [2am].[dbo].[AuditAccountSubsidy] WHERE";
        public const string auditaccountsubsidydatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountSubsidyKey, AccountKey, SubsidyKey FROM [2am].[dbo].[AuditAccountSubsidy] WHERE AuditNumber = @PrimaryKey";
        public const string auditaccountsubsidydatamodel_delete = "DELETE FROM [2am].[dbo].[AuditAccountSubsidy] WHERE AuditNumber = @PrimaryKey";
        public const string auditaccountsubsidydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditAccountSubsidy] WHERE";
        public const string auditaccountsubsidydatamodel_insert = "INSERT INTO [2am].[dbo].[AuditAccountSubsidy] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AccountSubsidyKey, AccountKey, SubsidyKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @AccountSubsidyKey, @AccountKey, @SubsidyKey); select cast(scope_identity() as int)";
        public const string auditaccountsubsidydatamodel_update = "UPDATE [2am].[dbo].[AuditAccountSubsidy] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, AccountSubsidyKey = @AccountSubsidyKey, AccountKey = @AccountKey, SubsidyKey = @SubsidyKey WHERE AuditNumber = @AuditNumber";



        public const string valuationstatusdatamodel_selectwhere = "SELECT ValuationStatusKey, Description FROM [2am].[dbo].[ValuationStatus] WHERE";
        public const string valuationstatusdatamodel_selectbykey = "SELECT ValuationStatusKey, Description FROM [2am].[dbo].[ValuationStatus] WHERE ValuationStatusKey = @PrimaryKey";
        public const string valuationstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationStatus] WHERE ValuationStatusKey = @PrimaryKey";
        public const string valuationstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationStatus] WHERE";
        public const string valuationstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationStatus] (ValuationStatusKey, Description) VALUES(@ValuationStatusKey, @Description); ";
        public const string valuationstatusdatamodel_update = "UPDATE [2am].[dbo].[ValuationStatus] SET ValuationStatusKey = @ValuationStatusKey, Description = @Description WHERE ValuationStatusKey = @ValuationStatusKey";



        public const string lifepolicystatusdatamodel_selectwhere = "SELECT PolicyStatusKey, Description FROM [2am].[dbo].[LifePolicyStatus] WHERE";
        public const string lifepolicystatusdatamodel_selectbykey = "SELECT PolicyStatusKey, Description FROM [2am].[dbo].[LifePolicyStatus] WHERE PolicyStatusKey = @PrimaryKey";
        public const string lifepolicystatusdatamodel_delete = "DELETE FROM [2am].[dbo].[LifePolicyStatus] WHERE PolicyStatusKey = @PrimaryKey";
        public const string lifepolicystatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifePolicyStatus] WHERE";
        public const string lifepolicystatusdatamodel_insert = "INSERT INTO [2am].[dbo].[LifePolicyStatus] (PolicyStatusKey, Description) VALUES(@PolicyStatusKey, @Description); ";
        public const string lifepolicystatusdatamodel_update = "UPDATE [2am].[dbo].[LifePolicyStatus] SET PolicyStatusKey = @PolicyStatusKey, Description = @Description WHERE PolicyStatusKey = @PolicyStatusKey";



        public const string captypeconfigurationdatamodel_selectwhere = "SELECT CapTypeConfigurationKey, OfferStartDate, OfferEndDate, GeneralStatusKey, CapEffectiveDate, CapClosureDate, ResetConfigurationKey, ResetDate, Term, ChangeDate, UserID, NACQDiscount FROM [2am].[dbo].[CapTypeConfiguration] WHERE";
        public const string captypeconfigurationdatamodel_selectbykey = "SELECT CapTypeConfigurationKey, OfferStartDate, OfferEndDate, GeneralStatusKey, CapEffectiveDate, CapClosureDate, ResetConfigurationKey, ResetDate, Term, ChangeDate, UserID, NACQDiscount FROM [2am].[dbo].[CapTypeConfiguration] WHERE CapTypeConfigurationKey = @PrimaryKey";
        public const string captypeconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[CapTypeConfiguration] WHERE CapTypeConfigurationKey = @PrimaryKey";
        public const string captypeconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CapTypeConfiguration] WHERE";
        public const string captypeconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[CapTypeConfiguration] (OfferStartDate, OfferEndDate, GeneralStatusKey, CapEffectiveDate, CapClosureDate, ResetConfigurationKey, ResetDate, Term, ChangeDate, UserID, NACQDiscount) VALUES(@OfferStartDate, @OfferEndDate, @GeneralStatusKey, @CapEffectiveDate, @CapClosureDate, @ResetConfigurationKey, @ResetDate, @Term, @ChangeDate, @UserID, @NACQDiscount); select cast(scope_identity() as int)";
        public const string captypeconfigurationdatamodel_update = "UPDATE [2am].[dbo].[CapTypeConfiguration] SET OfferStartDate = @OfferStartDate, OfferEndDate = @OfferEndDate, GeneralStatusKey = @GeneralStatusKey, CapEffectiveDate = @CapEffectiveDate, CapClosureDate = @CapClosureDate, ResetConfigurationKey = @ResetConfigurationKey, ResetDate = @ResetDate, Term = @Term, ChangeDate = @ChangeDate, UserID = @UserID, NACQDiscount = @NACQDiscount WHERE CapTypeConfigurationKey = @CapTypeConfigurationKey";



        public const string reportreferencedatamodel_selectwhere = "SELECT ReportReferenceKey, ReportStatementKey, Description FROM [2am].[dbo].[ReportReference] WHERE";
        public const string reportreferencedatamodel_selectbykey = "SELECT ReportReferenceKey, ReportStatementKey, Description FROM [2am].[dbo].[ReportReference] WHERE ReportReferenceKey = @PrimaryKey";
        public const string reportreferencedatamodel_delete = "DELETE FROM [2am].[dbo].[ReportReference] WHERE ReportReferenceKey = @PrimaryKey";
        public const string reportreferencedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReportReference] WHERE";
        public const string reportreferencedatamodel_insert = "INSERT INTO [2am].[dbo].[ReportReference] (ReportStatementKey, Description) VALUES(@ReportStatementKey, @Description); select cast(scope_identity() as int)";
        public const string reportreferencedatamodel_update = "UPDATE [2am].[dbo].[ReportReference] SET ReportStatementKey = @ReportStatementKey, Description = @Description WHERE ReportReferenceKey = @ReportReferenceKey";



        public const string batchtransactionstatusdatamodel_selectwhere = "SELECT BatchTransactionStatusKey, Description FROM [2am].[dbo].[BatchTransactionStatus] WHERE";
        public const string batchtransactionstatusdatamodel_selectbykey = "SELECT BatchTransactionStatusKey, Description FROM [2am].[dbo].[BatchTransactionStatus] WHERE BatchTransactionStatusKey = @PrimaryKey";
        public const string batchtransactionstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[BatchTransactionStatus] WHERE BatchTransactionStatusKey = @PrimaryKey";
        public const string batchtransactionstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchTransactionStatus] WHERE";
        public const string batchtransactionstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[BatchTransactionStatus] (BatchTransactionStatusKey, Description) VALUES(@BatchTransactionStatusKey, @Description); ";
        public const string batchtransactionstatusdatamodel_update = "UPDATE [2am].[dbo].[BatchTransactionStatus] SET BatchTransactionStatusKey = @BatchTransactionStatusKey, Description = @Description WHERE BatchTransactionStatusKey = @BatchTransactionStatusKey";



        public const string offerdebtsettlementdatamodel_selectwhere = "SELECT OfferDebtSettlementKey, OfferExpenseKey, SettlementAmount, SettlementDate, DisbursementTypeKey, BankAccountKey, InterestAppliedTypeKey, RateApplied, InterestStartDate, CapitalAmount, GuaranteeAmount, DisbursementKey FROM [2am].[dbo].[OfferDebtSettlement] WHERE";
        public const string offerdebtsettlementdatamodel_selectbykey = "SELECT OfferDebtSettlementKey, OfferExpenseKey, SettlementAmount, SettlementDate, DisbursementTypeKey, BankAccountKey, InterestAppliedTypeKey, RateApplied, InterestStartDate, CapitalAmount, GuaranteeAmount, DisbursementKey FROM [2am].[dbo].[OfferDebtSettlement] WHERE OfferDebtSettlementKey = @PrimaryKey";
        public const string offerdebtsettlementdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDebtSettlement] WHERE OfferDebtSettlementKey = @PrimaryKey";
        public const string offerdebtsettlementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDebtSettlement] WHERE";
        public const string offerdebtsettlementdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDebtSettlement] (OfferExpenseKey, SettlementAmount, SettlementDate, DisbursementTypeKey, BankAccountKey, InterestAppliedTypeKey, RateApplied, InterestStartDate, CapitalAmount, GuaranteeAmount, DisbursementKey) VALUES(@OfferExpenseKey, @SettlementAmount, @SettlementDate, @DisbursementTypeKey, @BankAccountKey, @InterestAppliedTypeKey, @RateApplied, @InterestStartDate, @CapitalAmount, @GuaranteeAmount, @DisbursementKey); select cast(scope_identity() as int)";
        public const string offerdebtsettlementdatamodel_update = "UPDATE [2am].[dbo].[OfferDebtSettlement] SET OfferExpenseKey = @OfferExpenseKey, SettlementAmount = @SettlementAmount, SettlementDate = @SettlementDate, DisbursementTypeKey = @DisbursementTypeKey, BankAccountKey = @BankAccountKey, InterestAppliedTypeKey = @InterestAppliedTypeKey, RateApplied = @RateApplied, InterestStartDate = @InterestStartDate, CapitalAmount = @CapitalAmount, GuaranteeAmount = @GuaranteeAmount, DisbursementKey = @DisbursementKey WHERE OfferDebtSettlementKey = @OfferDebtSettlementKey";



        public const string documenttemplatedatamodel_selectwhere = "SELECT DocumentTemplateKey, Description, Path, ApplicationName, StatementName FROM [2am].[dbo].[DocumentTemplate] WHERE";
        public const string documenttemplatedatamodel_selectbykey = "SELECT DocumentTemplateKey, Description, Path, ApplicationName, StatementName FROM [2am].[dbo].[DocumentTemplate] WHERE DocumentTemplateKey = @PrimaryKey";
        public const string documenttemplatedatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentTemplate] WHERE DocumentTemplateKey = @PrimaryKey";
        public const string documenttemplatedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentTemplate] WHERE";
        public const string documenttemplatedatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentTemplate] (DocumentTemplateKey, Description, Path, ApplicationName, StatementName) VALUES(@DocumentTemplateKey, @Description, @Path, @ApplicationName, @StatementName); ";
        public const string documenttemplatedatamodel_update = "UPDATE [2am].[dbo].[DocumentTemplate] SET DocumentTemplateKey = @DocumentTemplateKey, Description = @Description, Path = @Path, ApplicationName = @ApplicationName, StatementName = @StatementName WHERE DocumentTemplateKey = @DocumentTemplateKey";



        public const string lifecommissionratesdatamodel_selectwhere = "SELECT RatesKey, Entity, Percentage, EffectiveDate FROM [2am].[dbo].[LifeCommissionRates] WHERE";
        public const string lifecommissionratesdatamodel_selectbykey = "SELECT RatesKey, Entity, Percentage, EffectiveDate FROM [2am].[dbo].[LifeCommissionRates] WHERE RatesKey = @PrimaryKey";
        public const string lifecommissionratesdatamodel_delete = "DELETE FROM [2am].[dbo].[LifeCommissionRates] WHERE RatesKey = @PrimaryKey";
        public const string lifecommissionratesdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifeCommissionRates] WHERE";
        public const string lifecommissionratesdatamodel_insert = "INSERT INTO [2am].[dbo].[LifeCommissionRates] (Entity, Percentage, EffectiveDate) VALUES(@Entity, @Percentage, @EffectiveDate); select cast(scope_identity() as int)";
        public const string lifecommissionratesdatamodel_update = "UPDATE [2am].[dbo].[LifeCommissionRates] SET Entity = @Entity, Percentage = @Percentage, EffectiveDate = @EffectiveDate WHERE RatesKey = @RatesKey";



        public const string stagetransitiondatamodel_selectwhere = "SELECT StageTransitionKey, GenericKey, ADUserKey, TransitionDate, Comments, StageDefinitionStageDefinitionGroupKey, EndTransitionDate FROM [2am].[dbo].[StageTransition] WHERE";
        public const string stagetransitiondatamodel_selectbykey = "SELECT StageTransitionKey, GenericKey, ADUserKey, TransitionDate, Comments, StageDefinitionStageDefinitionGroupKey, EndTransitionDate FROM [2am].[dbo].[StageTransition] WHERE StageTransitionKey = @PrimaryKey";
        public const string stagetransitiondatamodel_delete = "DELETE FROM [2am].[dbo].[StageTransition] WHERE StageTransitionKey = @PrimaryKey";
        public const string stagetransitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StageTransition] WHERE";
        public const string stagetransitiondatamodel_insert = "INSERT INTO [2am].[dbo].[StageTransition] (GenericKey, ADUserKey, TransitionDate, Comments, StageDefinitionStageDefinitionGroupKey, EndTransitionDate) VALUES(@GenericKey, @ADUserKey, @TransitionDate, @Comments, @StageDefinitionStageDefinitionGroupKey, @EndTransitionDate); select cast(scope_identity() as int)";
        public const string stagetransitiondatamodel_update = "UPDATE [2am].[dbo].[StageTransition] SET GenericKey = @GenericKey, ADUserKey = @ADUserKey, TransitionDate = @TransitionDate, Comments = @Comments, StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey, EndTransitionDate = @EndTransitionDate WHERE StageTransitionKey = @StageTransitionKey";



        public const string auditproductdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ProductKey, Description, OriginateYN FROM [2am].[dbo].[AuditProduct] WHERE";
        public const string auditproductdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ProductKey, Description, OriginateYN FROM [2am].[dbo].[AuditProduct] WHERE AuditNumber = @PrimaryKey";
        public const string auditproductdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditProduct] WHERE AuditNumber = @PrimaryKey";
        public const string auditproductdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditProduct] WHERE";
        public const string auditproductdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditProduct] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ProductKey, Description, OriginateYN) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @ProductKey, @Description, @OriginateYN); select cast(scope_identity() as int)";
        public const string auditproductdatamodel_update = "UPDATE [2am].[dbo].[AuditProduct] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, ProductKey = @ProductKey, Description = @Description, OriginateYN = @OriginateYN WHERE AuditNumber = @AuditNumber";



        public const string invoicestatusdatamodel_selectwhere = "SELECT InvoiceStatusKey, Description FROM [2am].[dbo].[InvoiceStatus] WHERE";
        public const string invoicestatusdatamodel_selectbykey = "SELECT InvoiceStatusKey, Description FROM [2am].[dbo].[InvoiceStatus] WHERE InvoiceStatusKey = @PrimaryKey";
        public const string invoicestatusdatamodel_delete = "DELETE FROM [2am].[dbo].[InvoiceStatus] WHERE InvoiceStatusKey = @PrimaryKey";
        public const string invoicestatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[InvoiceStatus] WHERE";
        public const string invoicestatusdatamodel_insert = "INSERT INTO [2am].[dbo].[InvoiceStatus] (InvoiceStatusKey, Description) VALUES(@InvoiceStatusKey, @Description); ";
        public const string invoicestatusdatamodel_update = "UPDATE [2am].[dbo].[InvoiceStatus] SET InvoiceStatusKey = @InvoiceStatusKey, Description = @Description WHERE InvoiceStatusKey = @InvoiceStatusKey";



        public const string addressdatamodel_selectwhere = "SELECT AddressKey, AddressFormatKey, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbKey, PostOfficeKey, RRR_CountryDescription, RRR_ProvinceDescription, RRR_CityDescription, RRR_SuburbDescription, RRR_PostalCode, UserID, ChangeDate, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5 FROM [2am].[dbo].[Address] WHERE";
        public const string addressdatamodel_selectbykey = "SELECT AddressKey, AddressFormatKey, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbKey, PostOfficeKey, RRR_CountryDescription, RRR_ProvinceDescription, RRR_CityDescription, RRR_SuburbDescription, RRR_PostalCode, UserID, ChangeDate, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5 FROM [2am].[dbo].[Address] WHERE AddressKey = @PrimaryKey";
        public const string addressdatamodel_delete = "DELETE FROM [2am].[dbo].[Address] WHERE AddressKey = @PrimaryKey";
        public const string addressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Address] WHERE";
        public const string addressdatamodel_insert = "INSERT INTO [2am].[dbo].[Address] (AddressFormatKey, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbKey, PostOfficeKey, RRR_CountryDescription, RRR_ProvinceDescription, RRR_CityDescription, RRR_SuburbDescription, RRR_PostalCode, UserID, ChangeDate, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5) VALUES(@AddressFormatKey, @BoxNumber, @UnitNumber, @BuildingNumber, @BuildingName, @StreetNumber, @StreetName, @SuburbKey, @PostOfficeKey, @RRR_CountryDescription, @RRR_ProvinceDescription, @RRR_CityDescription, @RRR_SuburbDescription, @RRR_PostalCode, @UserID, @ChangeDate, @SuiteNumber, @FreeText1, @FreeText2, @FreeText3, @FreeText4, @FreeText5); select cast(scope_identity() as int)";
        public const string addressdatamodel_update = "UPDATE [2am].[dbo].[Address] SET AddressFormatKey = @AddressFormatKey, BoxNumber = @BoxNumber, UnitNumber = @UnitNumber, BuildingNumber = @BuildingNumber, BuildingName = @BuildingName, StreetNumber = @StreetNumber, StreetName = @StreetName, SuburbKey = @SuburbKey, PostOfficeKey = @PostOfficeKey, RRR_CountryDescription = @RRR_CountryDescription, RRR_ProvinceDescription = @RRR_ProvinceDescription, RRR_CityDescription = @RRR_CityDescription, RRR_SuburbDescription = @RRR_SuburbDescription, RRR_PostalCode = @RRR_PostalCode, UserID = @UserID, ChangeDate = @ChangeDate, SuiteNumber = @SuiteNumber, FreeText1 = @FreeText1, FreeText2 = @FreeText2, FreeText3 = @FreeText3, FreeText4 = @FreeText4, FreeText5 = @FreeText5 WHERE AddressKey = @AddressKey";



        public const string correspondenceparametersdatamodel_selectwhere = "SELECT CorrespondenceParameterKey, CorrespondenceKey, ReportParameterKey, ReportParameterValue FROM [2am].[dbo].[CorrespondenceParameters] WHERE";
        public const string correspondenceparametersdatamodel_selectbykey = "SELECT CorrespondenceParameterKey, CorrespondenceKey, ReportParameterKey, ReportParameterValue FROM [2am].[dbo].[CorrespondenceParameters] WHERE CorrespondenceParameterKey = @PrimaryKey";
        public const string correspondenceparametersdatamodel_delete = "DELETE FROM [2am].[dbo].[CorrespondenceParameters] WHERE CorrespondenceParameterKey = @PrimaryKey";
        public const string correspondenceparametersdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CorrespondenceParameters] WHERE";
        public const string correspondenceparametersdatamodel_insert = "INSERT INTO [2am].[dbo].[CorrespondenceParameters] (CorrespondenceKey, ReportParameterKey, ReportParameterValue) VALUES(@CorrespondenceKey, @ReportParameterKey, @ReportParameterValue); select cast(scope_identity() as int)";
        public const string correspondenceparametersdatamodel_update = "UPDATE [2am].[dbo].[CorrespondenceParameters] SET CorrespondenceKey = @CorrespondenceKey, ReportParameterKey = @ReportParameterKey, ReportParameterValue = @ReportParameterValue WHERE CorrespondenceParameterKey = @CorrespondenceParameterKey";



        public const string affordabilityassessmentlegalentitydatamodel_selectwhere = "SELECT AffordabilityAssessmentLegalEntityKey, AffordabilityAssessmentKey, LegalEntityKey FROM [2am].[dbo].[AffordabilityAssessmentLegalEntity] WHERE";
        public const string affordabilityassessmentlegalentitydatamodel_selectbykey = "SELECT AffordabilityAssessmentLegalEntityKey, AffordabilityAssessmentKey, LegalEntityKey FROM [2am].[dbo].[AffordabilityAssessmentLegalEntity] WHERE AffordabilityAssessmentLegalEntityKey = @PrimaryKey";
        public const string affordabilityassessmentlegalentitydatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentLegalEntity] WHERE AffordabilityAssessmentLegalEntityKey = @PrimaryKey";
        public const string affordabilityassessmentlegalentitydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityAssessmentLegalEntity] WHERE";
        public const string affordabilityassessmentlegalentitydatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityAssessmentLegalEntity] (AffordabilityAssessmentKey, LegalEntityKey) VALUES(@AffordabilityAssessmentKey, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string affordabilityassessmentlegalentitydatamodel_update = "UPDATE [2am].[dbo].[AffordabilityAssessmentLegalEntity] SET AffordabilityAssessmentKey = @AffordabilityAssessmentKey, LegalEntityKey = @LegalEntityKey WHERE AffordabilityAssessmentLegalEntityKey = @AffordabilityAssessmentLegalEntityKey";



        public const string captypeconfigurationdetaildatamodel_selectwhere = "SELECT CapTypeConfigurationDetailKey, CapTypeConfigurationKey, CapTypeKey, Rate, GeneralStatusKey, Premium, FeePremium, FeeAdmin, RateFinance, ChangeDate, UserID FROM [2am].[dbo].[CapTypeConfigurationDetail] WHERE";
        public const string captypeconfigurationdetaildatamodel_selectbykey = "SELECT CapTypeConfigurationDetailKey, CapTypeConfigurationKey, CapTypeKey, Rate, GeneralStatusKey, Premium, FeePremium, FeeAdmin, RateFinance, ChangeDate, UserID FROM [2am].[dbo].[CapTypeConfigurationDetail] WHERE CapTypeConfigurationDetailKey = @PrimaryKey";
        public const string captypeconfigurationdetaildatamodel_delete = "DELETE FROM [2am].[dbo].[CapTypeConfigurationDetail] WHERE CapTypeConfigurationDetailKey = @PrimaryKey";
        public const string captypeconfigurationdetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CapTypeConfigurationDetail] WHERE";
        public const string captypeconfigurationdetaildatamodel_insert = "INSERT INTO [2am].[dbo].[CapTypeConfigurationDetail] (CapTypeConfigurationKey, CapTypeKey, Rate, GeneralStatusKey, Premium, FeePremium, FeeAdmin, RateFinance, ChangeDate, UserID) VALUES(@CapTypeConfigurationKey, @CapTypeKey, @Rate, @GeneralStatusKey, @Premium, @FeePremium, @FeeAdmin, @RateFinance, @ChangeDate, @UserID); select cast(scope_identity() as int)";
        public const string captypeconfigurationdetaildatamodel_update = "UPDATE [2am].[dbo].[CapTypeConfigurationDetail] SET CapTypeConfigurationKey = @CapTypeConfigurationKey, CapTypeKey = @CapTypeKey, Rate = @Rate, GeneralStatusKey = @GeneralStatusKey, Premium = @Premium, FeePremium = @FeePremium, FeeAdmin = @FeeAdmin, RateFinance = @RateFinance, ChangeDate = @ChangeDate, UserID = @UserID WHERE CapTypeConfigurationDetailKey = @CapTypeConfigurationDetailKey";



        public const string assettransferdatamodel_selectwhere = "SELECT AccountKey, ClientSurname, SPVKey, LoanTotalBondAmount, LoanCurrentBalance, UserName, TransferedYN FROM [2am].[dbo].[AssetTransfer] WHERE";
        public const string assettransferdatamodel_selectbykey = "SELECT AccountKey, ClientSurname, SPVKey, LoanTotalBondAmount, LoanCurrentBalance, UserName, TransferedYN FROM [2am].[dbo].[AssetTransfer] WHERE AccountKey = @PrimaryKey";
        public const string assettransferdatamodel_delete = "DELETE FROM [2am].[dbo].[AssetTransfer] WHERE AccountKey = @PrimaryKey";
        public const string assettransferdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AssetTransfer] WHERE";
        public const string assettransferdatamodel_insert = "INSERT INTO [2am].[dbo].[AssetTransfer] (AccountKey, ClientSurname, SPVKey, LoanTotalBondAmount, LoanCurrentBalance, UserName, TransferedYN) VALUES(@AccountKey, @ClientSurname, @SPVKey, @LoanTotalBondAmount, @LoanCurrentBalance, @UserName, @TransferedYN); ";
        public const string assettransferdatamodel_update = "UPDATE [2am].[dbo].[AssetTransfer] SET AccountKey = @AccountKey, ClientSurname = @ClientSurname, SPVKey = @SPVKey, LoanTotalBondAmount = @LoanTotalBondAmount, LoanCurrentBalance = @LoanCurrentBalance, UserName = @UserName, TransferedYN = @TransferedYN WHERE AccountKey = @AccountKey";



        public const string creditcriteriaunsecuredlendingdatamodel_selectwhere = "SELECT CreditCriteriaUnsecuredLendingKey, CreditMatrixUnsecuredLendingKey, MarginKey, MinLoanAmount, MaxLoanAmount, Term FROM [2am].[dbo].[CreditCriteriaUnsecuredLending] WHERE";
        public const string creditcriteriaunsecuredlendingdatamodel_selectbykey = "SELECT CreditCriteriaUnsecuredLendingKey, CreditMatrixUnsecuredLendingKey, MarginKey, MinLoanAmount, MaxLoanAmount, Term FROM [2am].[dbo].[CreditCriteriaUnsecuredLending] WHERE CreditCriteriaUnsecuredLendingKey = @PrimaryKey";
        public const string creditcriteriaunsecuredlendingdatamodel_delete = "DELETE FROM [2am].[dbo].[CreditCriteriaUnsecuredLending] WHERE CreditCriteriaUnsecuredLendingKey = @PrimaryKey";
        public const string creditcriteriaunsecuredlendingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CreditCriteriaUnsecuredLending] WHERE";
        public const string creditcriteriaunsecuredlendingdatamodel_insert = "INSERT INTO [2am].[dbo].[CreditCriteriaUnsecuredLending] (CreditMatrixUnsecuredLendingKey, MarginKey, MinLoanAmount, MaxLoanAmount, Term) VALUES(@CreditMatrixUnsecuredLendingKey, @MarginKey, @MinLoanAmount, @MaxLoanAmount, @Term); select cast(scope_identity() as int)";
        public const string creditcriteriaunsecuredlendingdatamodel_update = "UPDATE [2am].[dbo].[CreditCriteriaUnsecuredLending] SET CreditMatrixUnsecuredLendingKey = @CreditMatrixUnsecuredLendingKey, MarginKey = @MarginKey, MinLoanAmount = @MinLoanAmount, MaxLoanAmount = @MaxLoanAmount, Term = @Term WHERE CreditCriteriaUnsecuredLendingKey = @CreditCriteriaUnsecuredLendingKey";



        public const string valuationdataproviderdataservicedatamodel_selectwhere = "SELECT ValuationDataProviderDataServiceKey, DataProviderDataServiceKey FROM [2am].[dbo].[ValuationDataProviderDataService] WHERE";
        public const string valuationdataproviderdataservicedatamodel_selectbykey = "SELECT ValuationDataProviderDataServiceKey, DataProviderDataServiceKey FROM [2am].[dbo].[ValuationDataProviderDataService] WHERE ValuationDataProviderDataServiceKey = @PrimaryKey";
        public const string valuationdataproviderdataservicedatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationDataProviderDataService] WHERE ValuationDataProviderDataServiceKey = @PrimaryKey";
        public const string valuationdataproviderdataservicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationDataProviderDataService] WHERE";
        public const string valuationdataproviderdataservicedatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationDataProviderDataService] (ValuationDataProviderDataServiceKey, DataProviderDataServiceKey) VALUES(@ValuationDataProviderDataServiceKey, @DataProviderDataServiceKey); ";
        public const string valuationdataproviderdataservicedatamodel_update = "UPDATE [2am].[dbo].[ValuationDataProviderDataService] SET ValuationDataProviderDataServiceKey = @ValuationDataProviderDataServiceKey, DataProviderDataServiceKey = @DataProviderDataServiceKey WHERE ValuationDataProviderDataServiceKey = @ValuationDataProviderDataServiceKey";



        public const string maritalstatusdatamodel_selectwhere = "SELECT MaritalStatusKey, Description FROM [2am].[dbo].[MaritalStatus] WHERE";
        public const string maritalstatusdatamodel_selectbykey = "SELECT MaritalStatusKey, Description FROM [2am].[dbo].[MaritalStatus] WHERE MaritalStatusKey = @PrimaryKey";
        public const string maritalstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[MaritalStatus] WHERE MaritalStatusKey = @PrimaryKey";
        public const string maritalstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MaritalStatus] WHERE";
        public const string maritalstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[MaritalStatus] (MaritalStatusKey, Description) VALUES(@MaritalStatusKey, @Description); ";
        public const string maritalstatusdatamodel_update = "UPDATE [2am].[dbo].[MaritalStatus] SET MaritalStatusKey = @MaritalStatusKey, Description = @Description WHERE MaritalStatusKey = @MaritalStatusKey";



        public const string reportstatementdocumenttemplatedatamodel_selectwhere = "SELECT ReportStatementDocumentTemplateKey, ReportStatementKey, DocumentTemplateKey, TemplateGenerationOrder FROM [2am].[dbo].[ReportStatementDocumentTemplate] WHERE";
        public const string reportstatementdocumenttemplatedatamodel_selectbykey = "SELECT ReportStatementDocumentTemplateKey, ReportStatementKey, DocumentTemplateKey, TemplateGenerationOrder FROM [2am].[dbo].[ReportStatementDocumentTemplate] WHERE ReportStatementDocumentTemplateKey = @PrimaryKey";
        public const string reportstatementdocumenttemplatedatamodel_delete = "DELETE FROM [2am].[dbo].[ReportStatementDocumentTemplate] WHERE ReportStatementDocumentTemplateKey = @PrimaryKey";
        public const string reportstatementdocumenttemplatedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReportStatementDocumentTemplate] WHERE";
        public const string reportstatementdocumenttemplatedatamodel_insert = "INSERT INTO [2am].[dbo].[ReportStatementDocumentTemplate] (ReportStatementKey, DocumentTemplateKey, TemplateGenerationOrder) VALUES(@ReportStatementKey, @DocumentTemplateKey, @TemplateGenerationOrder); select cast(scope_identity() as int)";
        public const string reportstatementdocumenttemplatedatamodel_update = "UPDATE [2am].[dbo].[ReportStatementDocumentTemplate] SET ReportStatementKey = @ReportStatementKey, DocumentTemplateKey = @DocumentTemplateKey, TemplateGenerationOrder = @TemplateGenerationOrder WHERE ReportStatementDocumentTemplateKey = @ReportStatementDocumentTemplateKey";



        public const string bulkbatchstatusdatamodel_selectwhere = "SELECT BulkBatchStatusKey, Description FROM [2am].[dbo].[BulkBatchStatus] WHERE";
        public const string bulkbatchstatusdatamodel_selectbykey = "SELECT BulkBatchStatusKey, Description FROM [2am].[dbo].[BulkBatchStatus] WHERE BulkBatchStatusKey = @PrimaryKey";
        public const string bulkbatchstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[BulkBatchStatus] WHERE BulkBatchStatusKey = @PrimaryKey";
        public const string bulkbatchstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BulkBatchStatus] WHERE";
        public const string bulkbatchstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[BulkBatchStatus] (BulkBatchStatusKey, Description) VALUES(@BulkBatchStatusKey, @Description); ";
        public const string bulkbatchstatusdatamodel_update = "UPDATE [2am].[dbo].[BulkBatchStatus] SET BulkBatchStatusKey = @BulkBatchStatusKey, Description = @Description WHERE BulkBatchStatusKey = @BulkBatchStatusKey";



        public const string x2assignmentmigdatamodel_selectwhere = "SELECT ID, InstanceId, UserOrganisationStructureKey, capabilityKey FROM [2am].[dbo].[x2AssignmentMig] WHERE";
        public const string x2assignmentmigdatamodel_selectbykey = "SELECT ID, InstanceId, UserOrganisationStructureKey, capabilityKey FROM [2am].[dbo].[x2AssignmentMig] WHERE ID = @PrimaryKey";
        public const string x2assignmentmigdatamodel_delete = "DELETE FROM [2am].[dbo].[x2AssignmentMig] WHERE ID = @PrimaryKey";
        public const string x2assignmentmigdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[x2AssignmentMig] WHERE";
        public const string x2assignmentmigdatamodel_insert = "INSERT INTO [2am].[dbo].[x2AssignmentMig] (ID, InstanceId, UserOrganisationStructureKey, capabilityKey) VALUES(@ID, @InstanceId, @UserOrganisationStructureKey, @capabilityKey); ";
        public const string x2assignmentmigdatamodel_update = "UPDATE [2am].[dbo].[x2AssignmentMig] SET ID = @ID, InstanceId = @InstanceId, UserOrganisationStructureKey = @UserOrganisationStructureKey, capabilityKey = @capabilityKey WHERE ID = @ID";



        public const string riskmatrixdatamodel_selectwhere = "SELECT RiskMatrixKey, Description FROM [2am].[dbo].[RiskMatrix] WHERE";
        public const string riskmatrixdatamodel_selectbykey = "SELECT RiskMatrixKey, Description FROM [2am].[dbo].[RiskMatrix] WHERE RiskMatrixKey = @PrimaryKey";
        public const string riskmatrixdatamodel_delete = "DELETE FROM [2am].[dbo].[RiskMatrix] WHERE RiskMatrixKey = @PrimaryKey";
        public const string riskmatrixdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RiskMatrix] WHERE";
        public const string riskmatrixdatamodel_insert = "INSERT INTO [2am].[dbo].[RiskMatrix] (RiskMatrixKey, Description) VALUES(@RiskMatrixKey, @Description); ";
        public const string riskmatrixdatamodel_update = "UPDATE [2am].[dbo].[RiskMatrix] SET RiskMatrixKey = @RiskMatrixKey, Description = @Description WHERE RiskMatrixKey = @RiskMatrixKey";



        public const string accountsequencedatamodel_selectwhere = "SELECT AccountKey, IsUsed FROM [2am].[dbo].[AccountSequence] WHERE";
        public const string accountsequencedatamodel_selectbykey = "SELECT AccountKey, IsUsed FROM [2am].[dbo].[AccountSequence] WHERE AccountKey = @PrimaryKey";
        public const string accountsequencedatamodel_delete = "DELETE FROM [2am].[dbo].[AccountSequence] WHERE AccountKey = @PrimaryKey";
        public const string accountsequencedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountSequence] WHERE";
        public const string accountsequencedatamodel_insert = "INSERT INTO [2am].[dbo].[AccountSequence] (IsUsed) VALUES(@IsUsed); select cast(scope_identity() as int)";
        public const string accountsequencedatamodel_update = "UPDATE [2am].[dbo].[AccountSequence] SET IsUsed = @IsUsed WHERE AccountKey = @AccountKey";



        public const string bankrangedatamodel_selectwhere = "SELECT BankRangeKey, ACBBankCode, RangeStart, RangeEnd, UserID, DateChange FROM [2am].[dbo].[BankRange] WHERE";
        public const string bankrangedatamodel_selectbykey = "SELECT BankRangeKey, ACBBankCode, RangeStart, RangeEnd, UserID, DateChange FROM [2am].[dbo].[BankRange] WHERE BankRangeKey = @PrimaryKey";
        public const string bankrangedatamodel_delete = "DELETE FROM [2am].[dbo].[BankRange] WHERE BankRangeKey = @PrimaryKey";
        public const string bankrangedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BankRange] WHERE";
        public const string bankrangedatamodel_insert = "INSERT INTO [2am].[dbo].[BankRange] (ACBBankCode, RangeStart, RangeEnd, UserID, DateChange) VALUES(@ACBBankCode, @RangeStart, @RangeEnd, @UserID, @DateChange); select cast(scope_identity() as int)";
        public const string bankrangedatamodel_update = "UPDATE [2am].[dbo].[BankRange] SET ACBBankCode = @ACBBankCode, RangeStart = @RangeStart, RangeEnd = @RangeEnd, UserID = @UserID, DateChange = @DateChange WHERE BankRangeKey = @BankRangeKey";



        public const string thirdpartyinvoicedatamodel_selectwhere = "SELECT ThirdPartyInvoiceKey, SahlReference, InvoiceStatusKey, AccountKey, ThirdPartyId, InvoiceNumber, InvoiceDate, ReceivedFromEmailAddress, AmountExcludingVAT, VATAmount, TotalAmountIncludingVAT, CapitaliseInvoice, ReceivedDate, PaymentReference FROM [2am].[dbo].[ThirdPartyInvoice] WHERE";
        public const string thirdpartyinvoicedatamodel_selectbykey = "SELECT ThirdPartyInvoiceKey, SahlReference, InvoiceStatusKey, AccountKey, ThirdPartyId, InvoiceNumber, InvoiceDate, ReceivedFromEmailAddress, AmountExcludingVAT, VATAmount, TotalAmountIncludingVAT, CapitaliseInvoice, ReceivedDate, PaymentReference FROM [2am].[dbo].[ThirdPartyInvoice] WHERE ThirdPartyInvoiceKey = @PrimaryKey";
        public const string thirdpartyinvoicedatamodel_delete = "DELETE FROM [2am].[dbo].[ThirdPartyInvoice] WHERE ThirdPartyInvoiceKey = @PrimaryKey";
        public const string thirdpartyinvoicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ThirdPartyInvoice] WHERE";
        public const string thirdpartyinvoicedatamodel_insert = "INSERT INTO [2am].[dbo].[ThirdPartyInvoice] (SahlReference, InvoiceStatusKey, AccountKey, ThirdPartyId, InvoiceNumber, InvoiceDate, ReceivedFromEmailAddress, AmountExcludingVAT, VATAmount, TotalAmountIncludingVAT, CapitaliseInvoice, ReceivedDate, PaymentReference) VALUES(@SahlReference, @InvoiceStatusKey, @AccountKey, @ThirdPartyId, @InvoiceNumber, @InvoiceDate, @ReceivedFromEmailAddress, @AmountExcludingVAT, @VATAmount, @TotalAmountIncludingVAT, @CapitaliseInvoice, @ReceivedDate, @PaymentReference); select cast(scope_identity() as int)";
        public const string thirdpartyinvoicedatamodel_update = "UPDATE [2am].[dbo].[ThirdPartyInvoice] SET SahlReference = @SahlReference, InvoiceStatusKey = @InvoiceStatusKey, AccountKey = @AccountKey, ThirdPartyId = @ThirdPartyId, InvoiceNumber = @InvoiceNumber, InvoiceDate = @InvoiceDate, ReceivedFromEmailAddress = @ReceivedFromEmailAddress, AmountExcludingVAT = @AmountExcludingVAT, VATAmount = @VATAmount, TotalAmountIncludingVAT = @TotalAmountIncludingVAT, CapitaliseInvoice = @CapitaliseInvoice, ReceivedDate = @ReceivedDate, PaymentReference = @PaymentReference WHERE ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey";



        public const string legalentityaddressdatamodel_selectwhere = "SELECT LegalEntityAddressKey, LegalEntityKey, AddressKey, AddressTypeKey, EffectiveDate, GeneralStatusKey FROM [2am].[dbo].[LegalEntityAddress] WHERE";
        public const string legalentityaddressdatamodel_selectbykey = "SELECT LegalEntityAddressKey, LegalEntityKey, AddressKey, AddressTypeKey, EffectiveDate, GeneralStatusKey FROM [2am].[dbo].[LegalEntityAddress] WHERE LegalEntityAddressKey = @PrimaryKey";
        public const string legalentityaddressdatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityAddress] WHERE LegalEntityAddressKey = @PrimaryKey";
        public const string legalentityaddressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityAddress] WHERE";
        public const string legalentityaddressdatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityAddress] (LegalEntityKey, AddressKey, AddressTypeKey, EffectiveDate, GeneralStatusKey) VALUES(@LegalEntityKey, @AddressKey, @AddressTypeKey, @EffectiveDate, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string legalentityaddressdatamodel_update = "UPDATE [2am].[dbo].[LegalEntityAddress] SET LegalEntityKey = @LegalEntityKey, AddressKey = @AddressKey, AddressTypeKey = @AddressTypeKey, EffectiveDate = @EffectiveDate, GeneralStatusKey = @GeneralStatusKey WHERE LegalEntityAddressKey = @LegalEntityAddressKey";



        public const string userassignmentdatamodel_selectwhere = "SELECT UserAssignmentKey, FinancialServiceKey, OriginationSourceProductKey, AssignmentDate, AssigningUser, AssignedUser FROM [2am].[dbo].[UserAssignment] WHERE";
        public const string userassignmentdatamodel_selectbykey = "SELECT UserAssignmentKey, FinancialServiceKey, OriginationSourceProductKey, AssignmentDate, AssigningUser, AssignedUser FROM [2am].[dbo].[UserAssignment] WHERE UserAssignmentKey = @PrimaryKey";
        public const string userassignmentdatamodel_delete = "DELETE FROM [2am].[dbo].[UserAssignment] WHERE UserAssignmentKey = @PrimaryKey";
        public const string userassignmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[UserAssignment] WHERE";
        public const string userassignmentdatamodel_insert = "INSERT INTO [2am].[dbo].[UserAssignment] (FinancialServiceKey, OriginationSourceProductKey, AssignmentDate, AssigningUser, AssignedUser) VALUES(@FinancialServiceKey, @OriginationSourceProductKey, @AssignmentDate, @AssigningUser, @AssignedUser); select cast(scope_identity() as int)";
        public const string userassignmentdatamodel_update = "UPDATE [2am].[dbo].[UserAssignment] SET FinancialServiceKey = @FinancialServiceKey, OriginationSourceProductKey = @OriginationSourceProductKey, AssignmentDate = @AssignmentDate, AssigningUser = @AssigningUser, AssignedUser = @AssignedUser WHERE UserAssignmentKey = @UserAssignmentKey";



        public const string organisationstructurehistorydatamodel_selectwhere = "SELECT OrganisationStructureHistoryKey, OrganisationStructureKey, ParentKey, Description, OrganisationTypeKey, GeneralStatusKey, ChangeDate, Action FROM [2am].[dbo].[OrganisationStructureHistory] WHERE";
        public const string organisationstructurehistorydatamodel_selectbykey = "SELECT OrganisationStructureHistoryKey, OrganisationStructureKey, ParentKey, Description, OrganisationTypeKey, GeneralStatusKey, ChangeDate, Action FROM [2am].[dbo].[OrganisationStructureHistory] WHERE OrganisationStructureHistoryKey = @PrimaryKey";
        public const string organisationstructurehistorydatamodel_delete = "DELETE FROM [2am].[dbo].[OrganisationStructureHistory] WHERE OrganisationStructureHistoryKey = @PrimaryKey";
        public const string organisationstructurehistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OrganisationStructureHistory] WHERE";
        public const string organisationstructurehistorydatamodel_insert = "INSERT INTO [2am].[dbo].[OrganisationStructureHistory] (OrganisationStructureKey, ParentKey, Description, OrganisationTypeKey, GeneralStatusKey, ChangeDate, Action) VALUES(@OrganisationStructureKey, @ParentKey, @Description, @OrganisationTypeKey, @GeneralStatusKey, @ChangeDate, @Action); select cast(scope_identity() as int)";
        public const string organisationstructurehistorydatamodel_update = "UPDATE [2am].[dbo].[OrganisationStructureHistory] SET OrganisationStructureKey = @OrganisationStructureKey, ParentKey = @ParentKey, Description = @Description, OrganisationTypeKey = @OrganisationTypeKey, GeneralStatusKey = @GeneralStatusKey, ChangeDate = @ChangeDate, Action = @Action WHERE OrganisationStructureHistoryKey = @OrganisationStructureHistoryKey";



        public const string creditcriteria_backupdatamodel_selectwhere = "SELECT CreditCriteriaKey, CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria FROM [2am].[dbo].[CreditCriteria_Backup] WHERE";
        public const string creditcriteria_backupdatamodel_selectbykey = "SELECT CreditCriteriaKey, CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria FROM [2am].[dbo].[CreditCriteria_Backup] WHERE  = @PrimaryKey";
        public const string creditcriteria_backupdatamodel_delete = "DELETE FROM [2am].[dbo].[CreditCriteria_Backup] WHERE  = @PrimaryKey";
        public const string creditcriteria_backupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CreditCriteria_Backup] WHERE";
        public const string creditcriteria_backupdatamodel_insert = "INSERT INTO [2am].[dbo].[CreditCriteria_Backup] (CreditCriteriaKey, CreditMatrixKey, MarginKey, CategoryKey, EmploymentTypeKey, MortgageLoanPurposeKey, MinLoanAmount, MaxLoanAmount, MinPropertyValue, MaxPropertyValue, LTV, PTI, MinIncomeAmount, ExceptionCriteria) VALUES(@CreditCriteriaKey, @CreditMatrixKey, @MarginKey, @CategoryKey, @EmploymentTypeKey, @MortgageLoanPurposeKey, @MinLoanAmount, @MaxLoanAmount, @MinPropertyValue, @MaxPropertyValue, @LTV, @PTI, @MinIncomeAmount, @ExceptionCriteria); ";
        public const string creditcriteria_backupdatamodel_update = "UPDATE [2am].[dbo].[CreditCriteria_Backup] SET CreditCriteriaKey = @CreditCriteriaKey, CreditMatrixKey = @CreditMatrixKey, MarginKey = @MarginKey, CategoryKey = @CategoryKey, EmploymentTypeKey = @EmploymentTypeKey, MortgageLoanPurposeKey = @MortgageLoanPurposeKey, MinLoanAmount = @MinLoanAmount, MaxLoanAmount = @MaxLoanAmount, MinPropertyValue = @MinPropertyValue, MaxPropertyValue = @MaxPropertyValue, LTV = @LTV, PTI = @PTI, MinIncomeAmount = @MinIncomeAmount, ExceptionCriteria = @ExceptionCriteria WHERE  = @";



        public const string riskmatrixdimensiondatamodel_selectwhere = "SELECT RiskMatrixDimensionKey, Description FROM [2am].[dbo].[RiskMatrixDimension] WHERE";
        public const string riskmatrixdimensiondatamodel_selectbykey = "SELECT RiskMatrixDimensionKey, Description FROM [2am].[dbo].[RiskMatrixDimension] WHERE RiskMatrixDimensionKey = @PrimaryKey";
        public const string riskmatrixdimensiondatamodel_delete = "DELETE FROM [2am].[dbo].[RiskMatrixDimension] WHERE RiskMatrixDimensionKey = @PrimaryKey";
        public const string riskmatrixdimensiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RiskMatrixDimension] WHERE";
        public const string riskmatrixdimensiondatamodel_insert = "INSERT INTO [2am].[dbo].[RiskMatrixDimension] (RiskMatrixDimensionKey, Description) VALUES(@RiskMatrixDimensionKey, @Description); ";
        public const string riskmatrixdimensiondatamodel_update = "UPDATE [2am].[dbo].[RiskMatrixDimension] SET RiskMatrixDimensionKey = @RiskMatrixDimensionKey, Description = @Description WHERE RiskMatrixDimensionKey = @RiskMatrixDimensionKey";



        public const string auditaddressdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AddressKey, AddressFormatKey, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbKey, PostOfficeKey, RRR_CountryDescription, RRR_ProvinceDescription, RRR_CityDescription, RRR_SuburbDescription, RRR_PostalCode, UserID, ChangeDate, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5 FROM [2am].[dbo].[AuditAddress] WHERE";
        public const string auditaddressdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AddressKey, AddressFormatKey, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbKey, PostOfficeKey, RRR_CountryDescription, RRR_ProvinceDescription, RRR_CityDescription, RRR_SuburbDescription, RRR_PostalCode, UserID, ChangeDate, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5 FROM [2am].[dbo].[AuditAddress] WHERE AuditNumber = @PrimaryKey";
        public const string auditaddressdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditAddress] WHERE AuditNumber = @PrimaryKey";
        public const string auditaddressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditAddress] WHERE";
        public const string auditaddressdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditAddress] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AddressKey, AddressFormatKey, BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, SuburbKey, PostOfficeKey, RRR_CountryDescription, RRR_ProvinceDescription, RRR_CityDescription, RRR_SuburbDescription, RRR_PostalCode, UserID, ChangeDate, SuiteNumber, FreeText1, FreeText2, FreeText3, FreeText4, FreeText5) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @AddressKey, @AddressFormatKey, @BoxNumber, @UnitNumber, @BuildingNumber, @BuildingName, @StreetNumber, @StreetName, @SuburbKey, @PostOfficeKey, @RRR_CountryDescription, @RRR_ProvinceDescription, @RRR_CityDescription, @RRR_SuburbDescription, @RRR_PostalCode, @UserID, @ChangeDate, @SuiteNumber, @FreeText1, @FreeText2, @FreeText3, @FreeText4, @FreeText5); select cast(scope_identity() as int)";
        public const string auditaddressdatamodel_update = "UPDATE [2am].[dbo].[AuditAddress] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, AddressKey = @AddressKey, AddressFormatKey = @AddressFormatKey, BoxNumber = @BoxNumber, UnitNumber = @UnitNumber, BuildingNumber = @BuildingNumber, BuildingName = @BuildingName, StreetNumber = @StreetNumber, StreetName = @StreetName, SuburbKey = @SuburbKey, PostOfficeKey = @PostOfficeKey, RRR_CountryDescription = @RRR_CountryDescription, RRR_ProvinceDescription = @RRR_ProvinceDescription, RRR_CityDescription = @RRR_CityDescription, RRR_SuburbDescription = @RRR_SuburbDescription, RRR_PostalCode = @RRR_PostalCode, UserID = @UserID, ChangeDate = @ChangeDate, SuiteNumber = @SuiteNumber, FreeText1 = @FreeText1, FreeText2 = @FreeText2, FreeText3 = @FreeText3, FreeText4 = @FreeText4, FreeText5 = @FreeText5 WHERE AuditNumber = @AuditNumber";



        public const string rateadjustmentgroupdatamodel_selectwhere = "SELECT RateAdjustmentGroupKey, Description FROM [2am].[dbo].[RateAdjustmentGroup] WHERE";
        public const string rateadjustmentgroupdatamodel_selectbykey = "SELECT RateAdjustmentGroupKey, Description FROM [2am].[dbo].[RateAdjustmentGroup] WHERE RateAdjustmentGroupKey = @PrimaryKey";
        public const string rateadjustmentgroupdatamodel_delete = "DELETE FROM [2am].[dbo].[RateAdjustmentGroup] WHERE RateAdjustmentGroupKey = @PrimaryKey";
        public const string rateadjustmentgroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RateAdjustmentGroup] WHERE";
        public const string rateadjustmentgroupdatamodel_insert = "INSERT INTO [2am].[dbo].[RateAdjustmentGroup] (RateAdjustmentGroupKey, Description) VALUES(@RateAdjustmentGroupKey, @Description); ";
        public const string rateadjustmentgroupdatamodel_update = "UPDATE [2am].[dbo].[RateAdjustmentGroup] SET RateAdjustmentGroupKey = @RateAdjustmentGroupKey, Description = @Description WHERE RateAdjustmentGroupKey = @RateAdjustmentGroupKey";



        public const string expensetypedatamodel_selectwhere = "SELECT ExpenseTypeKey, ExpenseTypeGroupKey, PaymentTypeKey, Description FROM [2am].[dbo].[ExpenseType] WHERE";
        public const string expensetypedatamodel_selectbykey = "SELECT ExpenseTypeKey, ExpenseTypeGroupKey, PaymentTypeKey, Description FROM [2am].[dbo].[ExpenseType] WHERE ExpenseTypeKey = @PrimaryKey";
        public const string expensetypedatamodel_delete = "DELETE FROM [2am].[dbo].[ExpenseType] WHERE ExpenseTypeKey = @PrimaryKey";
        public const string expensetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ExpenseType] WHERE";
        public const string expensetypedatamodel_insert = "INSERT INTO [2am].[dbo].[ExpenseType] (ExpenseTypeGroupKey, PaymentTypeKey, Description) VALUES(@ExpenseTypeGroupKey, @PaymentTypeKey, @Description); select cast(scope_identity() as int)";
        public const string expensetypedatamodel_update = "UPDATE [2am].[dbo].[ExpenseType] SET ExpenseTypeGroupKey = @ExpenseTypeGroupKey, PaymentTypeKey = @PaymentTypeKey, Description = @Description WHERE ExpenseTypeKey = @ExpenseTypeKey";



        public const string cancellationtypedatamodel_selectwhere = "SELECT CancellationTypeKey, Description, CancellationWebCode FROM [2am].[dbo].[CancellationType] WHERE";
        public const string cancellationtypedatamodel_selectbykey = "SELECT CancellationTypeKey, Description, CancellationWebCode FROM [2am].[dbo].[CancellationType] WHERE CancellationTypeKey = @PrimaryKey";
        public const string cancellationtypedatamodel_delete = "DELETE FROM [2am].[dbo].[CancellationType] WHERE CancellationTypeKey = @PrimaryKey";
        public const string cancellationtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CancellationType] WHERE";
        public const string cancellationtypedatamodel_insert = "INSERT INTO [2am].[dbo].[CancellationType] (CancellationTypeKey, Description, CancellationWebCode) VALUES(@CancellationTypeKey, @Description, @CancellationWebCode); ";
        public const string cancellationtypedatamodel_update = "UPDATE [2am].[dbo].[CancellationType] SET CancellationTypeKey = @CancellationTypeKey, Description = @Description, CancellationWebCode = @CancellationWebCode WHERE CancellationTypeKey = @CancellationTypeKey";



        public const string marketingoptiondatamodel_selectwhere = "SELECT MarketingOptionKey, Description, GeneralStatusKey FROM [2am].[dbo].[MarketingOption] WHERE";
        public const string marketingoptiondatamodel_selectbykey = "SELECT MarketingOptionKey, Description, GeneralStatusKey FROM [2am].[dbo].[MarketingOption] WHERE MarketingOptionKey = @PrimaryKey";
        public const string marketingoptiondatamodel_delete = "DELETE FROM [2am].[dbo].[MarketingOption] WHERE MarketingOptionKey = @PrimaryKey";
        public const string marketingoptiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MarketingOption] WHERE";
        public const string marketingoptiondatamodel_insert = "INSERT INTO [2am].[dbo].[MarketingOption] (MarketingOptionKey, Description, GeneralStatusKey) VALUES(@MarketingOptionKey, @Description, @GeneralStatusKey); ";
        public const string marketingoptiondatamodel_update = "UPDATE [2am].[dbo].[MarketingOption] SET MarketingOptionKey = @MarketingOptionKey, Description = @Description, GeneralStatusKey = @GeneralStatusKey WHERE MarketingOptionKey = @MarketingOptionKey";



        public const string documentgroupdatamodel_selectwhere = "SELECT DocumentGroupKey, Description FROM [2am].[dbo].[DocumentGroup] WHERE";
        public const string documentgroupdatamodel_selectbykey = "SELECT DocumentGroupKey, Description FROM [2am].[dbo].[DocumentGroup] WHERE DocumentGroupKey = @PrimaryKey";
        public const string documentgroupdatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentGroup] WHERE DocumentGroupKey = @PrimaryKey";
        public const string documentgroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentGroup] WHERE";
        public const string documentgroupdatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentGroup] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string documentgroupdatamodel_update = "UPDATE [2am].[dbo].[DocumentGroup] SET Description = @Description WHERE DocumentGroupKey = @DocumentGroupKey";



        public const string bulkbatchtypedatamodel_selectwhere = "SELECT BulkBatchTypeKey, Description, FilePath FROM [2am].[dbo].[BulkBatchType] WHERE";
        public const string bulkbatchtypedatamodel_selectbykey = "SELECT BulkBatchTypeKey, Description, FilePath FROM [2am].[dbo].[BulkBatchType] WHERE BulkBatchTypeKey = @PrimaryKey";
        public const string bulkbatchtypedatamodel_delete = "DELETE FROM [2am].[dbo].[BulkBatchType] WHERE BulkBatchTypeKey = @PrimaryKey";
        public const string bulkbatchtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BulkBatchType] WHERE";
        public const string bulkbatchtypedatamodel_insert = "INSERT INTO [2am].[dbo].[BulkBatchType] (BulkBatchTypeKey, Description, FilePath) VALUES(@BulkBatchTypeKey, @Description, @FilePath); ";
        public const string bulkbatchtypedatamodel_update = "UPDATE [2am].[dbo].[BulkBatchType] SET BulkBatchTypeKey = @BulkBatchTypeKey, Description = @Description, FilePath = @FilePath WHERE BulkBatchTypeKey = @BulkBatchTypeKey";



        public const string lifeofferassignmentdatamodel_selectwhere = "SELECT LifeOfferAssignmentKey, OfferKey, LoanAccountKey, LoanOfferKey, LoanOfferTypeKey, DateAssigned, ADUserName FROM [2am].[dbo].[LifeOfferAssignment] WHERE";
        public const string lifeofferassignmentdatamodel_selectbykey = "SELECT LifeOfferAssignmentKey, OfferKey, LoanAccountKey, LoanOfferKey, LoanOfferTypeKey, DateAssigned, ADUserName FROM [2am].[dbo].[LifeOfferAssignment] WHERE LifeOfferAssignmentKey = @PrimaryKey";
        public const string lifeofferassignmentdatamodel_delete = "DELETE FROM [2am].[dbo].[LifeOfferAssignment] WHERE LifeOfferAssignmentKey = @PrimaryKey";
        public const string lifeofferassignmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifeOfferAssignment] WHERE";
        public const string lifeofferassignmentdatamodel_insert = "INSERT INTO [2am].[dbo].[LifeOfferAssignment] (OfferKey, LoanAccountKey, LoanOfferKey, LoanOfferTypeKey, DateAssigned, ADUserName) VALUES(@OfferKey, @LoanAccountKey, @LoanOfferKey, @LoanOfferTypeKey, @DateAssigned, @ADUserName); select cast(scope_identity() as int)";
        public const string lifeofferassignmentdatamodel_update = "UPDATE [2am].[dbo].[LifeOfferAssignment] SET OfferKey = @OfferKey, LoanAccountKey = @LoanAccountKey, LoanOfferKey = @LoanOfferKey, LoanOfferTypeKey = @LoanOfferTypeKey, DateAssigned = @DateAssigned, ADUserName = @ADUserName WHERE LifeOfferAssignmentKey = @LifeOfferAssignmentKey";



        public const string offerinformationpersonalloandatamodel_selectwhere = "SELECT OfferInformationKey, LoanAmount, Term, MonthlyInstalment, LifePremium, FeesTotal, CreditCriteriaUnsecuredLendingKey, MarginKey, MarketRateKey FROM [2am].[dbo].[OfferInformationPersonalLoan] WHERE";
        public const string offerinformationpersonalloandatamodel_selectbykey = "SELECT OfferInformationKey, LoanAmount, Term, MonthlyInstalment, LifePremium, FeesTotal, CreditCriteriaUnsecuredLendingKey, MarginKey, MarketRateKey FROM [2am].[dbo].[OfferInformationPersonalLoan] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationpersonalloandatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationPersonalLoan] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationpersonalloandatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationPersonalLoan] WHERE";
        public const string offerinformationpersonalloandatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationPersonalLoan] (OfferInformationKey, LoanAmount, Term, MonthlyInstalment, LifePremium, FeesTotal, CreditCriteriaUnsecuredLendingKey, MarginKey, MarketRateKey) VALUES(@OfferInformationKey, @LoanAmount, @Term, @MonthlyInstalment, @LifePremium, @FeesTotal, @CreditCriteriaUnsecuredLendingKey, @MarginKey, @MarketRateKey); ";
        public const string offerinformationpersonalloandatamodel_update = "UPDATE [2am].[dbo].[OfferInformationPersonalLoan] SET OfferInformationKey = @OfferInformationKey, LoanAmount = @LoanAmount, Term = @Term, MonthlyInstalment = @MonthlyInstalment, LifePremium = @LifePremium, FeesTotal = @FeesTotal, CreditCriteriaUnsecuredLendingKey = @CreditCriteriaUnsecuredLendingKey, MarginKey = @MarginKey, MarketRateKey = @MarketRateKey WHERE OfferInformationKey = @OfferInformationKey";



        public const string dataproviderdataservicedatamodel_selectwhere = "SELECT DataProviderDataServiceKey, DataProviderKey, DataServiceKey FROM [2am].[dbo].[DataProviderDataService] WHERE";
        public const string dataproviderdataservicedatamodel_selectbykey = "SELECT DataProviderDataServiceKey, DataProviderKey, DataServiceKey FROM [2am].[dbo].[DataProviderDataService] WHERE DataProviderDataServiceKey = @PrimaryKey";
        public const string dataproviderdataservicedatamodel_delete = "DELETE FROM [2am].[dbo].[DataProviderDataService] WHERE DataProviderDataServiceKey = @PrimaryKey";
        public const string dataproviderdataservicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DataProviderDataService] WHERE";
        public const string dataproviderdataservicedatamodel_insert = "INSERT INTO [2am].[dbo].[DataProviderDataService] (DataProviderDataServiceKey, DataProviderKey, DataServiceKey) VALUES(@DataProviderDataServiceKey, @DataProviderKey, @DataServiceKey); ";
        public const string dataproviderdataservicedatamodel_update = "UPDATE [2am].[dbo].[DataProviderDataService] SET DataProviderDataServiceKey = @DataProviderDataServiceKey, DataProviderKey = @DataProviderKey, DataServiceKey = @DataServiceKey WHERE DataProviderDataServiceKey = @DataProviderDataServiceKey";



        public const string auditaffordabilityassessmentdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentKey, GenericKey, GenericKeyTypeKey, AffordabilityAssessmentStatusKey, GeneralStatusKey, AffordabilityAssessmentStressFactorKey, ModifiedDate, ModifiedByUserId, NumberOfContributingApplicants, NumberOfHouseholdDependants, MinimumMonthlyFixedExpenses, ConfirmedDate, Notes FROM [2am].[dbo].[AuditAffordabilityAssessment] WHERE";
        public const string auditaffordabilityassessmentdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentKey, GenericKey, GenericKeyTypeKey, AffordabilityAssessmentStatusKey, GeneralStatusKey, AffordabilityAssessmentStressFactorKey, ModifiedDate, ModifiedByUserId, NumberOfContributingApplicants, NumberOfHouseholdDependants, MinimumMonthlyFixedExpenses, ConfirmedDate, Notes FROM [2am].[dbo].[AuditAffordabilityAssessment] WHERE AuditNumber = @PrimaryKey";
        public const string auditaffordabilityassessmentdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditAffordabilityAssessment] WHERE AuditNumber = @PrimaryKey";
        public const string auditaffordabilityassessmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditAffordabilityAssessment] WHERE";
        public const string auditaffordabilityassessmentdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditAffordabilityAssessment] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentKey, GenericKey, GenericKeyTypeKey, AffordabilityAssessmentStatusKey, GeneralStatusKey, AffordabilityAssessmentStressFactorKey, ModifiedDate, ModifiedByUserId, NumberOfContributingApplicants, NumberOfHouseholdDependants, MinimumMonthlyFixedExpenses, ConfirmedDate, Notes) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @AffordabilityAssessmentKey, @GenericKey, @GenericKeyTypeKey, @AffordabilityAssessmentStatusKey, @GeneralStatusKey, @AffordabilityAssessmentStressFactorKey, @ModifiedDate, @ModifiedByUserId, @NumberOfContributingApplicants, @NumberOfHouseholdDependants, @MinimumMonthlyFixedExpenses, @ConfirmedDate, @Notes); select cast(scope_identity() as int)";
        public const string auditaffordabilityassessmentdatamodel_update = "UPDATE [2am].[dbo].[AuditAffordabilityAssessment] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, AffordabilityAssessmentKey = @AffordabilityAssessmentKey, GenericKey = @GenericKey, GenericKeyTypeKey = @GenericKeyTypeKey, AffordabilityAssessmentStatusKey = @AffordabilityAssessmentStatusKey, GeneralStatusKey = @GeneralStatusKey, AffordabilityAssessmentStressFactorKey = @AffordabilityAssessmentStressFactorKey, ModifiedDate = @ModifiedDate, ModifiedByUserId = @ModifiedByUserId, NumberOfContributingApplicants = @NumberOfContributingApplicants, NumberOfHouseholdDependants = @NumberOfHouseholdDependants, MinimumMonthlyFixedExpenses = @MinimumMonthlyFixedExpenses, ConfirmedDate = @ConfirmedDate, Notes = @Notes WHERE AuditNumber = @AuditNumber";



        public const string riskmatrixrangedatamodel_selectwhere = "SELECT RiskMatrixRangeKey, Min, Max, Designation FROM [2am].[dbo].[RiskMatrixRange] WHERE";
        public const string riskmatrixrangedatamodel_selectbykey = "SELECT RiskMatrixRangeKey, Min, Max, Designation FROM [2am].[dbo].[RiskMatrixRange] WHERE RiskMatrixRangeKey = @PrimaryKey";
        public const string riskmatrixrangedatamodel_delete = "DELETE FROM [2am].[dbo].[RiskMatrixRange] WHERE RiskMatrixRangeKey = @PrimaryKey";
        public const string riskmatrixrangedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RiskMatrixRange] WHERE";
        public const string riskmatrixrangedatamodel_insert = "INSERT INTO [2am].[dbo].[RiskMatrixRange] (RiskMatrixRangeKey, Min, Max, Designation) VALUES(@RiskMatrixRangeKey, @Min, @Max, @Designation); ";
        public const string riskmatrixrangedatamodel_update = "UPDATE [2am].[dbo].[RiskMatrixRange] SET RiskMatrixRangeKey = @RiskMatrixRangeKey, Min = @Min, Max = @Max, Designation = @Designation WHERE RiskMatrixRangeKey = @RiskMatrixRangeKey";



        public const string auditpropertydatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, PropertyKey, PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, AddressKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, FinancialServiceKey, DataProviderKey FROM [2am].[dbo].[AuditProperty] WHERE";
        public const string auditpropertydatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, PropertyKey, PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, AddressKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, FinancialServiceKey, DataProviderKey FROM [2am].[dbo].[AuditProperty] WHERE AuditNumber = @PrimaryKey";
        public const string auditpropertydatamodel_delete = "DELETE FROM [2am].[dbo].[AuditProperty] WHERE AuditNumber = @PrimaryKey";
        public const string auditpropertydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditProperty] WHERE";
        public const string auditpropertydatamodel_insert = "INSERT INTO [2am].[dbo].[AuditProperty] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, PropertyKey, PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, AddressKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, FinancialServiceKey, DataProviderKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @PropertyKey, @PropertyTypeKey, @TitleTypeKey, @AreaClassificationKey, @OccupancyTypeKey, @AddressKey, @PropertyDescription1, @PropertyDescription2, @PropertyDescription3, @DeedsOfficeValue, @CurrentBondDate, @ErfNumber, @ErfPortionNumber, @SectionalSchemeName, @SectionalUnitNumber, @DeedsPropertyTypeKey, @ErfSuburbDescription, @ErfMetroDescription, @FinancialServiceKey, @DataProviderKey); select cast(scope_identity() as int)";
        public const string auditpropertydatamodel_update = "UPDATE [2am].[dbo].[AuditProperty] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, PropertyKey = @PropertyKey, PropertyTypeKey = @PropertyTypeKey, TitleTypeKey = @TitleTypeKey, AreaClassificationKey = @AreaClassificationKey, OccupancyTypeKey = @OccupancyTypeKey, AddressKey = @AddressKey, PropertyDescription1 = @PropertyDescription1, PropertyDescription2 = @PropertyDescription2, PropertyDescription3 = @PropertyDescription3, DeedsOfficeValue = @DeedsOfficeValue, CurrentBondDate = @CurrentBondDate, ErfNumber = @ErfNumber, ErfPortionNumber = @ErfPortionNumber, SectionalSchemeName = @SectionalSchemeName, SectionalUnitNumber = @SectionalUnitNumber, DeedsPropertyTypeKey = @DeedsPropertyTypeKey, ErfSuburbDescription = @ErfSuburbDescription, ErfMetroDescription = @ErfMetroDescription, FinancialServiceKey = @FinancialServiceKey, DataProviderKey = @DataProviderKey WHERE AuditNumber = @AuditNumber";



        public const string notedatamodel_selectwhere = "SELECT NoteKey, GenericKeyTypeKey, GenericKey, DiaryDate FROM [2am].[dbo].[Note] WHERE";
        public const string notedatamodel_selectbykey = "SELECT NoteKey, GenericKeyTypeKey, GenericKey, DiaryDate FROM [2am].[dbo].[Note] WHERE NoteKey = @PrimaryKey";
        public const string notedatamodel_delete = "DELETE FROM [2am].[dbo].[Note] WHERE NoteKey = @PrimaryKey";
        public const string notedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Note] WHERE";
        public const string notedatamodel_insert = "INSERT INTO [2am].[dbo].[Note] (GenericKeyTypeKey, GenericKey, DiaryDate) VALUES(@GenericKeyTypeKey, @GenericKey, @DiaryDate); select cast(scope_identity() as int)";
        public const string notedatamodel_update = "UPDATE [2am].[dbo].[Note] SET GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey, DiaryDate = @DiaryDate WHERE NoteKey = @NoteKey";



        public const string cdcim900exceptionsdatamodel_selectwhere = "SELECT CDCIM900ExceptionsKey, FileDate, RecordNumber, BranchNumber, Exception, ActionDate FROM [2am].[dbo].[CDCIM900Exceptions] WHERE";
        public const string cdcim900exceptionsdatamodel_selectbykey = "SELECT CDCIM900ExceptionsKey, FileDate, RecordNumber, BranchNumber, Exception, ActionDate FROM [2am].[dbo].[CDCIM900Exceptions] WHERE CDCIM900ExceptionsKey = @PrimaryKey";
        public const string cdcim900exceptionsdatamodel_delete = "DELETE FROM [2am].[dbo].[CDCIM900Exceptions] WHERE CDCIM900ExceptionsKey = @PrimaryKey";
        public const string cdcim900exceptionsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CDCIM900Exceptions] WHERE";
        public const string cdcim900exceptionsdatamodel_insert = "INSERT INTO [2am].[dbo].[CDCIM900Exceptions] (FileDate, RecordNumber, BranchNumber, Exception, ActionDate) VALUES(@FileDate, @RecordNumber, @BranchNumber, @Exception, @ActionDate); select cast(scope_identity() as int)";
        public const string cdcim900exceptionsdatamodel_update = "UPDATE [2am].[dbo].[CDCIM900Exceptions] SET FileDate = @FileDate, RecordNumber = @RecordNumber, BranchNumber = @BranchNumber, Exception = @Exception, ActionDate = @ActionDate WHERE CDCIM900ExceptionsKey = @CDCIM900ExceptionsKey";



        public const string datagridconfigurationtypedatamodel_selectwhere = "SELECT DataGridConfigurationTypeKey, Description FROM [2am].[dbo].[DataGridConfigurationType] WHERE";
        public const string datagridconfigurationtypedatamodel_selectbykey = "SELECT DataGridConfigurationTypeKey, Description FROM [2am].[dbo].[DataGridConfigurationType] WHERE DataGridConfigurationTypeKey = @PrimaryKey";
        public const string datagridconfigurationtypedatamodel_delete = "DELETE FROM [2am].[dbo].[DataGridConfigurationType] WHERE DataGridConfigurationTypeKey = @PrimaryKey";
        public const string datagridconfigurationtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DataGridConfigurationType] WHERE";
        public const string datagridconfigurationtypedatamodel_insert = "INSERT INTO [2am].[dbo].[DataGridConfigurationType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string datagridconfigurationtypedatamodel_update = "UPDATE [2am].[dbo].[DataGridConfigurationType] SET Description = @Description WHERE DataGridConfigurationTypeKey = @DataGridConfigurationTypeKey";



        public const string tradedatamodel_selectwhere = "SELECT TradeKey, TradeType, Company, TradeDate, StartDate, EndDate, ResetConfigurationKey, StrikeRate, TradeBalance, CapBalance, Premium, CapTypeKey, TrancheTypeKey FROM [2am].[dbo].[Trade] WHERE";
        public const string tradedatamodel_selectbykey = "SELECT TradeKey, TradeType, Company, TradeDate, StartDate, EndDate, ResetConfigurationKey, StrikeRate, TradeBalance, CapBalance, Premium, CapTypeKey, TrancheTypeKey FROM [2am].[dbo].[Trade] WHERE TradeKey = @PrimaryKey";
        public const string tradedatamodel_delete = "DELETE FROM [2am].[dbo].[Trade] WHERE TradeKey = @PrimaryKey";
        public const string tradedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Trade] WHERE";
        public const string tradedatamodel_insert = "INSERT INTO [2am].[dbo].[Trade] (TradeType, Company, TradeDate, StartDate, EndDate, ResetConfigurationKey, StrikeRate, TradeBalance, CapBalance, Premium, CapTypeKey, TrancheTypeKey) VALUES(@TradeType, @Company, @TradeDate, @StartDate, @EndDate, @ResetConfigurationKey, @StrikeRate, @TradeBalance, @CapBalance, @Premium, @CapTypeKey, @TrancheTypeKey); select cast(scope_identity() as int)";
        public const string tradedatamodel_update = "UPDATE [2am].[dbo].[Trade] SET TradeType = @TradeType, Company = @Company, TradeDate = @TradeDate, StartDate = @StartDate, EndDate = @EndDate, ResetConfigurationKey = @ResetConfigurationKey, StrikeRate = @StrikeRate, TradeBalance = @TradeBalance, CapBalance = @CapBalance, Premium = @Premium, CapTypeKey = @CapTypeKey, TrancheTypeKey = @TrancheTypeKey WHERE TradeKey = @TradeKey";



        public const string externalroletypegroupdatamodel_selectwhere = "SELECT ExternalRoleTypeGroupKey, Description FROM [2am].[dbo].[ExternalRoleTypeGroup] WHERE";
        public const string externalroletypegroupdatamodel_selectbykey = "SELECT ExternalRoleTypeGroupKey, Description FROM [2am].[dbo].[ExternalRoleTypeGroup] WHERE ExternalRoleTypeGroupKey = @PrimaryKey";
        public const string externalroletypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[ExternalRoleTypeGroup] WHERE ExternalRoleTypeGroupKey = @PrimaryKey";
        public const string externalroletypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ExternalRoleTypeGroup] WHERE";
        public const string externalroletypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[ExternalRoleTypeGroup] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string externalroletypegroupdatamodel_update = "UPDATE [2am].[dbo].[ExternalRoleTypeGroup] SET Description = @Description WHERE ExternalRoleTypeGroupKey = @ExternalRoleTypeGroupKey";



        public const string rateadjustmentelementtypedatamodel_selectwhere = "SELECT RateAdjustmentElementTypeKey, Description, StatementName FROM [2am].[dbo].[RateAdjustmentElementType] WHERE";
        public const string rateadjustmentelementtypedatamodel_selectbykey = "SELECT RateAdjustmentElementTypeKey, Description, StatementName FROM [2am].[dbo].[RateAdjustmentElementType] WHERE RateAdjustmentElementTypeKey = @PrimaryKey";
        public const string rateadjustmentelementtypedatamodel_delete = "DELETE FROM [2am].[dbo].[RateAdjustmentElementType] WHERE RateAdjustmentElementTypeKey = @PrimaryKey";
        public const string rateadjustmentelementtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RateAdjustmentElementType] WHERE";
        public const string rateadjustmentelementtypedatamodel_insert = "INSERT INTO [2am].[dbo].[RateAdjustmentElementType] (RateAdjustmentElementTypeKey, Description, StatementName) VALUES(@RateAdjustmentElementTypeKey, @Description, @StatementName); ";
        public const string rateadjustmentelementtypedatamodel_update = "UPDATE [2am].[dbo].[RateAdjustmentElementType] SET RateAdjustmentElementTypeKey = @RateAdjustmentElementTypeKey, Description = @Description, StatementName = @StatementName WHERE RateAdjustmentElementTypeKey = @RateAdjustmentElementTypeKey";



        public const string cappaymentoptiondatamodel_selectwhere = "SELECT CAPPaymentOptionKey, Description FROM [2am].[dbo].[CAPPaymentOption] WHERE";
        public const string cappaymentoptiondatamodel_selectbykey = "SELECT CAPPaymentOptionKey, Description FROM [2am].[dbo].[CAPPaymentOption] WHERE CAPPaymentOptionKey = @PrimaryKey";
        public const string cappaymentoptiondatamodel_delete = "DELETE FROM [2am].[dbo].[CAPPaymentOption] WHERE CAPPaymentOptionKey = @PrimaryKey";
        public const string cappaymentoptiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CAPPaymentOption] WHERE";
        public const string cappaymentoptiondatamodel_insert = "INSERT INTO [2am].[dbo].[CAPPaymentOption] (CAPPaymentOptionKey, Description) VALUES(@CAPPaymentOptionKey, @Description); ";
        public const string cappaymentoptiondatamodel_update = "UPDATE [2am].[dbo].[CAPPaymentOption] SET CAPPaymentOptionKey = @CAPPaymentOptionKey, Description = @Description WHERE CAPPaymentOptionKey = @CAPPaymentOptionKey";



        public const string cap_newinterestprogramdatamodel_selectwhere = "SELECT FinancialServiceKey FROM [2am].[dbo].[CAP_NewInterestProgram] WHERE";
        public const string cap_newinterestprogramdatamodel_selectbykey = "SELECT FinancialServiceKey FROM [2am].[dbo].[CAP_NewInterestProgram] WHERE  = @PrimaryKey";
        public const string cap_newinterestprogramdatamodel_delete = "DELETE FROM [2am].[dbo].[CAP_NewInterestProgram] WHERE  = @PrimaryKey";
        public const string cap_newinterestprogramdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CAP_NewInterestProgram] WHERE";
        public const string cap_newinterestprogramdatamodel_insert = "INSERT INTO [2am].[dbo].[CAP_NewInterestProgram] (FinancialServiceKey) VALUES(@FinancialServiceKey); ";
        public const string cap_newinterestprogramdatamodel_update = "UPDATE [2am].[dbo].[CAP_NewInterestProgram] SET FinancialServiceKey = @FinancialServiceKey WHERE  = @";



        public const string riskmatrixrevisiondatamodel_selectwhere = "SELECT RiskMatrixRevisionKey, RiskMatrixKey, Description, RevisionDate FROM [2am].[dbo].[RiskMatrixRevision] WHERE";
        public const string riskmatrixrevisiondatamodel_selectbykey = "SELECT RiskMatrixRevisionKey, RiskMatrixKey, Description, RevisionDate FROM [2am].[dbo].[RiskMatrixRevision] WHERE RiskMatrixRevisionKey = @PrimaryKey";
        public const string riskmatrixrevisiondatamodel_delete = "DELETE FROM [2am].[dbo].[RiskMatrixRevision] WHERE RiskMatrixRevisionKey = @PrimaryKey";
        public const string riskmatrixrevisiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RiskMatrixRevision] WHERE";
        public const string riskmatrixrevisiondatamodel_insert = "INSERT INTO [2am].[dbo].[RiskMatrixRevision] (RiskMatrixRevisionKey, RiskMatrixKey, Description, RevisionDate) VALUES(@RiskMatrixRevisionKey, @RiskMatrixKey, @Description, @RevisionDate); ";
        public const string riskmatrixrevisiondatamodel_update = "UPDATE [2am].[dbo].[RiskMatrixRevision] SET RiskMatrixRevisionKey = @RiskMatrixRevisionKey, RiskMatrixKey = @RiskMatrixKey, Description = @Description, RevisionDate = @RevisionDate WHERE RiskMatrixRevisionKey = @RiskMatrixRevisionKey";



        public const string cdctablelistdatamodel_selectwhere = "SELECT Name, [Schema] FROM [2am].[dbo].[cdctablelist] WHERE";
        public const string cdctablelistdatamodel_selectbykey = "SELECT Name, [Schema] FROM [2am].[dbo].[cdctablelist] WHERE  = @PrimaryKey";
        public const string cdctablelistdatamodel_delete = "DELETE FROM [2am].[dbo].[cdctablelist] WHERE  = @PrimaryKey";
        public const string cdctablelistdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[cdctablelist] WHERE";
        public const string cdctablelistdatamodel_insert = "INSERT INTO [2am].[dbo].[cdctablelist] (Name, [Schema]) VALUES(@Name, @Schema); ";
        public const string cdctablelistdatamodel_update = "UPDATE [2am].[dbo].[cdctablelist] SET Name = @Name, [Schema] = @Schema WHERE  = @";



        public const string correspondencedatamodel_selectwhere = "SELECT CorrespondenceKey, GenericKey, ReportStatementKey, CorrespondenceMediumKey, DestinationValue, DueDate, CompletedDate, UserID, ChangeDate, OutputFile, GenericKeyTypeKey, LegalEntityKey FROM [2am].[dbo].[Correspondence] WHERE";
        public const string correspondencedatamodel_selectbykey = "SELECT CorrespondenceKey, GenericKey, ReportStatementKey, CorrespondenceMediumKey, DestinationValue, DueDate, CompletedDate, UserID, ChangeDate, OutputFile, GenericKeyTypeKey, LegalEntityKey FROM [2am].[dbo].[Correspondence] WHERE CorrespondenceKey = @PrimaryKey";
        public const string correspondencedatamodel_delete = "DELETE FROM [2am].[dbo].[Correspondence] WHERE CorrespondenceKey = @PrimaryKey";
        public const string correspondencedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Correspondence] WHERE";
        public const string correspondencedatamodel_insert = "INSERT INTO [2am].[dbo].[Correspondence] (GenericKey, ReportStatementKey, CorrespondenceMediumKey, DestinationValue, DueDate, CompletedDate, UserID, ChangeDate, OutputFile, GenericKeyTypeKey, LegalEntityKey) VALUES(@GenericKey, @ReportStatementKey, @CorrespondenceMediumKey, @DestinationValue, @DueDate, @CompletedDate, @UserID, @ChangeDate, @OutputFile, @GenericKeyTypeKey, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string correspondencedatamodel_update = "UPDATE [2am].[dbo].[Correspondence] SET GenericKey = @GenericKey, ReportStatementKey = @ReportStatementKey, CorrespondenceMediumKey = @CorrespondenceMediumKey, DestinationValue = @DestinationValue, DueDate = @DueDate, CompletedDate = @CompletedDate, UserID = @UserID, ChangeDate = @ChangeDate, OutputFile = @OutputFile, GenericKeyTypeKey = @GenericKeyTypeKey, LegalEntityKey = @LegalEntityKey WHERE CorrespondenceKey = @CorrespondenceKey";



        public const string notedetaildatamodel_selectwhere = "SELECT NoteDetailKey, NoteKey, Tag, WorkflowState, InsertedDate, NoteText, LegalEntityKey FROM [2am].[dbo].[NoteDetail] WHERE";
        public const string notedetaildatamodel_selectbykey = "SELECT NoteDetailKey, NoteKey, Tag, WorkflowState, InsertedDate, NoteText, LegalEntityKey FROM [2am].[dbo].[NoteDetail] WHERE NoteDetailKey = @PrimaryKey";
        public const string notedetaildatamodel_delete = "DELETE FROM [2am].[dbo].[NoteDetail] WHERE NoteDetailKey = @PrimaryKey";
        public const string notedetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[NoteDetail] WHERE";
        public const string notedetaildatamodel_insert = "INSERT INTO [2am].[dbo].[NoteDetail] (NoteKey, Tag, WorkflowState, InsertedDate, NoteText, LegalEntityKey) VALUES(@NoteKey, @Tag, @WorkflowState, @InsertedDate, @NoteText, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string notedetaildatamodel_update = "UPDATE [2am].[dbo].[NoteDetail] SET NoteKey = @NoteKey, Tag = @Tag, WorkflowState = @WorkflowState, InsertedDate = @InsertedDate, NoteText = @NoteText, LegalEntityKey = @LegalEntityKey WHERE NoteDetailKey = @NoteDetailKey";



        public const string originationsourceproductexpensetypedatamodel_selectwhere = "SELECT OriginationSourceProductExpenseTypeKey, OriginationSourceProductKey, ExpenseTypeKey FROM [2am].[dbo].[OriginationSourceProductExpenseType] WHERE";
        public const string originationsourceproductexpensetypedatamodel_selectbykey = "SELECT OriginationSourceProductExpenseTypeKey, OriginationSourceProductKey, ExpenseTypeKey FROM [2am].[dbo].[OriginationSourceProductExpenseType] WHERE OriginationSourceProductExpenseTypeKey = @PrimaryKey";
        public const string originationsourceproductexpensetypedatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceProductExpenseType] WHERE OriginationSourceProductExpenseTypeKey = @PrimaryKey";
        public const string originationsourceproductexpensetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceProductExpenseType] WHERE";
        public const string originationsourceproductexpensetypedatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceProductExpenseType] (OriginationSourceProductKey, ExpenseTypeKey) VALUES(@OriginationSourceProductKey, @ExpenseTypeKey); select cast(scope_identity() as int)";
        public const string originationsourceproductexpensetypedatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceProductExpenseType] SET OriginationSourceProductKey = @OriginationSourceProductKey, ExpenseTypeKey = @ExpenseTypeKey WHERE OriginationSourceProductExpenseTypeKey = @OriginationSourceProductExpenseTypeKey";



        public const string messagetypedatamodel_selectwhere = "SELECT MessageTypeKey, Description FROM [2am].[dbo].[MessageType] WHERE";
        public const string messagetypedatamodel_selectbykey = "SELECT MessageTypeKey, Description FROM [2am].[dbo].[MessageType] WHERE MessageTypeKey = @PrimaryKey";
        public const string messagetypedatamodel_delete = "DELETE FROM [2am].[dbo].[MessageType] WHERE MessageTypeKey = @PrimaryKey";
        public const string messagetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MessageType] WHERE";
        public const string messagetypedatamodel_insert = "INSERT INTO [2am].[dbo].[MessageType] (MessageTypeKey, Description) VALUES(@MessageTypeKey, @Description); ";
        public const string messagetypedatamodel_update = "UPDATE [2am].[dbo].[MessageType] SET MessageTypeKey = @MessageTypeKey, Description = @Description WHERE MessageTypeKey = @MessageTypeKey";



        public const string legalentitylogindatamodel_selectwhere = "SELECT LegalEntityLoginKey, Username, Password, LastLoginDate, GeneralStatusKey, LegalEntityKey FROM [2am].[dbo].[LegalEntityLogin] WHERE";
        public const string legalentitylogindatamodel_selectbykey = "SELECT LegalEntityLoginKey, Username, Password, LastLoginDate, GeneralStatusKey, LegalEntityKey FROM [2am].[dbo].[LegalEntityLogin] WHERE LegalEntityLoginKey = @PrimaryKey";
        public const string legalentitylogindatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityLogin] WHERE LegalEntityLoginKey = @PrimaryKey";
        public const string legalentitylogindatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityLogin] WHERE";
        public const string legalentitylogindatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityLogin] (Username, Password, LastLoginDate, GeneralStatusKey, LegalEntityKey) VALUES(@Username, @Password, @LastLoginDate, @GeneralStatusKey, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string legalentitylogindatamodel_update = "UPDATE [2am].[dbo].[LegalEntityLogin] SET Username = @Username, Password = @Password, LastLoginDate = @LastLoginDate, GeneralStatusKey = @GeneralStatusKey, LegalEntityKey = @LegalEntityKey WHERE LegalEntityLoginKey = @LegalEntityLoginKey";



        public const string marketratehistorydatamodel_selectwhere = "SELECT MarketRateHistoryKey, ChangeDate, RateBefore, RateAfter, MarketRateKey, ChangedBy, ChangedByHost, ChangedByApp FROM [2am].[dbo].[MarketRateHistory] WHERE";
        public const string marketratehistorydatamodel_selectbykey = "SELECT MarketRateHistoryKey, ChangeDate, RateBefore, RateAfter, MarketRateKey, ChangedBy, ChangedByHost, ChangedByApp FROM [2am].[dbo].[MarketRateHistory] WHERE MarketRateHistoryKey = @PrimaryKey";
        public const string marketratehistorydatamodel_delete = "DELETE FROM [2am].[dbo].[MarketRateHistory] WHERE MarketRateHistoryKey = @PrimaryKey";
        public const string marketratehistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MarketRateHistory] WHERE";
        public const string marketratehistorydatamodel_insert = "INSERT INTO [2am].[dbo].[MarketRateHistory] (ChangeDate, RateBefore, RateAfter, MarketRateKey, ChangedBy, ChangedByHost, ChangedByApp) VALUES(@ChangeDate, @RateBefore, @RateAfter, @MarketRateKey, @ChangedBy, @ChangedByHost, @ChangedByApp); select cast(scope_identity() as int)";
        public const string marketratehistorydatamodel_update = "UPDATE [2am].[dbo].[MarketRateHistory] SET ChangeDate = @ChangeDate, RateBefore = @RateBefore, RateAfter = @RateAfter, MarketRateKey = @MarketRateKey, ChangedBy = @ChangedBy, ChangedByHost = @ChangedByHost, ChangedByApp = @ChangedByApp WHERE MarketRateHistoryKey = @MarketRateHistoryKey";



        public const string auditbankaccountdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BankAccountKey, ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate FROM [2am].[dbo].[AuditBankAccount] WHERE";
        public const string auditbankaccountdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BankAccountKey, ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate FROM [2am].[dbo].[AuditBankAccount] WHERE AuditNumber = @PrimaryKey";
        public const string auditbankaccountdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditBankAccount] WHERE AuditNumber = @PrimaryKey";
        public const string auditbankaccountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditBankAccount] WHERE";
        public const string auditbankaccountdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditBankAccount] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BankAccountKey, ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @BankAccountKey, @ACBBranchCode, @AccountNumber, @ACBTypeNumber, @AccountName, @UserID, @ChangeDate); select cast(scope_identity() as int)";
        public const string auditbankaccountdatamodel_update = "UPDATE [2am].[dbo].[AuditBankAccount] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, BankAccountKey = @BankAccountKey, ACBBranchCode = @ACBBranchCode, AccountNumber = @AccountNumber, ACBTypeNumber = @ACBTypeNumber, AccountName = @AccountName, UserID = @UserID, ChangeDate = @ChangeDate WHERE AuditNumber = @AuditNumber";



        public const string rateadjustmentelementdatamodel_selectwhere = "SELECT RateAdjustmentElementKey, ElementMinValue, ElementMaxValue, ElementText, RateAdjustmentValue, EffectiveDate, Description, RateAdjustmentGroupKey, RateAdjustmentElementTypeKey, GenericKeyTypeKey, GeneralStatusKey, RuleItemKey, FinancialAdjustmentTypeSourceKey FROM [2am].[dbo].[RateAdjustmentElement] WHERE";
        public const string rateadjustmentelementdatamodel_selectbykey = "SELECT RateAdjustmentElementKey, ElementMinValue, ElementMaxValue, ElementText, RateAdjustmentValue, EffectiveDate, Description, RateAdjustmentGroupKey, RateAdjustmentElementTypeKey, GenericKeyTypeKey, GeneralStatusKey, RuleItemKey, FinancialAdjustmentTypeSourceKey FROM [2am].[dbo].[RateAdjustmentElement] WHERE RateAdjustmentElementKey = @PrimaryKey";
        public const string rateadjustmentelementdatamodel_delete = "DELETE FROM [2am].[dbo].[RateAdjustmentElement] WHERE RateAdjustmentElementKey = @PrimaryKey";
        public const string rateadjustmentelementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RateAdjustmentElement] WHERE";
        public const string rateadjustmentelementdatamodel_insert = "INSERT INTO [2am].[dbo].[RateAdjustmentElement] (ElementMinValue, ElementMaxValue, ElementText, RateAdjustmentValue, EffectiveDate, Description, RateAdjustmentGroupKey, RateAdjustmentElementTypeKey, GenericKeyTypeKey, GeneralStatusKey, RuleItemKey, FinancialAdjustmentTypeSourceKey) VALUES(@ElementMinValue, @ElementMaxValue, @ElementText, @RateAdjustmentValue, @EffectiveDate, @Description, @RateAdjustmentGroupKey, @RateAdjustmentElementTypeKey, @GenericKeyTypeKey, @GeneralStatusKey, @RuleItemKey, @FinancialAdjustmentTypeSourceKey); select cast(scope_identity() as int)";
        public const string rateadjustmentelementdatamodel_update = "UPDATE [2am].[dbo].[RateAdjustmentElement] SET ElementMinValue = @ElementMinValue, ElementMaxValue = @ElementMaxValue, ElementText = @ElementText, RateAdjustmentValue = @RateAdjustmentValue, EffectiveDate = @EffectiveDate, Description = @Description, RateAdjustmentGroupKey = @RateAdjustmentGroupKey, RateAdjustmentElementTypeKey = @RateAdjustmentElementTypeKey, GenericKeyTypeKey = @GenericKeyTypeKey, GeneralStatusKey = @GeneralStatusKey, RuleItemKey = @RuleItemKey, FinancialAdjustmentTypeSourceKey = @FinancialAdjustmentTypeSourceKey WHERE RateAdjustmentElementKey = @RateAdjustmentElementKey";



        public const string calendardatamodel_selectwhere = "SELECT CalendarKey, CalendarDate, IsSaturday, IsSunday, IsHoliday FROM [2am].[dbo].[Calendar] WHERE";
        public const string calendardatamodel_selectbykey = "SELECT CalendarKey, CalendarDate, IsSaturday, IsSunday, IsHoliday FROM [2am].[dbo].[Calendar] WHERE CalendarKey = @PrimaryKey";
        public const string calendardatamodel_delete = "DELETE FROM [2am].[dbo].[Calendar] WHERE CalendarKey = @PrimaryKey";
        public const string calendardatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Calendar] WHERE";
        public const string calendardatamodel_insert = "INSERT INTO [2am].[dbo].[Calendar] (CalendarDate, IsSaturday, IsSunday, IsHoliday) VALUES(@CalendarDate, @IsSaturday, @IsSunday, @IsHoliday); select cast(scope_identity() as int)";
        public const string calendardatamodel_update = "UPDATE [2am].[dbo].[Calendar] SET CalendarDate = @CalendarDate, IsSaturday = @IsSaturday, IsSunday = @IsSunday, IsHoliday = @IsHoliday WHERE CalendarKey = @CalendarKey";



        public const string audithocdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, HOCInsurerKey, HOCPolicyNumber, HOCProrataPremium, HOCMonthlyPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, HOCSubsidenceKey, HOCConstructionKey, HOCRoofKey, HOCStatusID, HOCSBICFlag, CapitalizedMonthlyBalance, CommencementDate, AnniversaryDate, UserID, ChangeDate, HOCStatusKey, Ceded, SAHLPolicyNumber, CancellationDate, HOCHistoryKey, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey, HOCBaseConventional, HOCBaseThatch, HOCBaseShingle FROM [2am].[dbo].[AuditHOC] WHERE";
        public const string audithocdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, HOCInsurerKey, HOCPolicyNumber, HOCProrataPremium, HOCMonthlyPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, HOCSubsidenceKey, HOCConstructionKey, HOCRoofKey, HOCStatusID, HOCSBICFlag, CapitalizedMonthlyBalance, CommencementDate, AnniversaryDate, UserID, ChangeDate, HOCStatusKey, Ceded, SAHLPolicyNumber, CancellationDate, HOCHistoryKey, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey, HOCBaseConventional, HOCBaseThatch, HOCBaseShingle FROM [2am].[dbo].[AuditHOC] WHERE AuditNumber = @PrimaryKey";
        public const string audithocdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditHOC] WHERE AuditNumber = @PrimaryKey";
        public const string audithocdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditHOC] WHERE";
        public const string audithocdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditHOC] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, HOCInsurerKey, HOCPolicyNumber, HOCProrataPremium, HOCMonthlyPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, HOCSubsidenceKey, HOCConstructionKey, HOCRoofKey, HOCStatusID, HOCSBICFlag, CapitalizedMonthlyBalance, CommencementDate, AnniversaryDate, UserID, ChangeDate, HOCStatusKey, Ceded, SAHLPolicyNumber, CancellationDate, HOCHistoryKey, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey, HOCBaseConventional, HOCBaseThatch, HOCBaseShingle) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @FinancialServiceKey, @HOCInsurerKey, @HOCPolicyNumber, @HOCProrataPremium, @HOCMonthlyPremium, @HOCThatchAmount, @HOCConventionalAmount, @HOCShingleAmount, @HOCTotalSumInsured, @HOCSubsidenceKey, @HOCConstructionKey, @HOCRoofKey, @HOCStatusID, @HOCSBICFlag, @CapitalizedMonthlyBalance, @CommencementDate, @AnniversaryDate, @UserID, @ChangeDate, @HOCStatusKey, @Ceded, @SAHLPolicyNumber, @CancellationDate, @HOCHistoryKey, @HOCAdministrationFee, @HOCBasePremium, @SASRIAAmount, @HOCRatesKey, @HOCBaseConventional, @HOCBaseThatch, @HOCBaseShingle); select cast(scope_identity() as int)";
        public const string audithocdatamodel_update = "UPDATE [2am].[dbo].[AuditHOC] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, FinancialServiceKey = @FinancialServiceKey, HOCInsurerKey = @HOCInsurerKey, HOCPolicyNumber = @HOCPolicyNumber, HOCProrataPremium = @HOCProrataPremium, HOCMonthlyPremium = @HOCMonthlyPremium, HOCThatchAmount = @HOCThatchAmount, HOCConventionalAmount = @HOCConventionalAmount, HOCShingleAmount = @HOCShingleAmount, HOCTotalSumInsured = @HOCTotalSumInsured, HOCSubsidenceKey = @HOCSubsidenceKey, HOCConstructionKey = @HOCConstructionKey, HOCRoofKey = @HOCRoofKey, HOCStatusID = @HOCStatusID, HOCSBICFlag = @HOCSBICFlag, CapitalizedMonthlyBalance = @CapitalizedMonthlyBalance, CommencementDate = @CommencementDate, AnniversaryDate = @AnniversaryDate, UserID = @UserID, ChangeDate = @ChangeDate, HOCStatusKey = @HOCStatusKey, Ceded = @Ceded, SAHLPolicyNumber = @SAHLPolicyNumber, CancellationDate = @CancellationDate, HOCHistoryKey = @HOCHistoryKey, HOCAdministrationFee = @HOCAdministrationFee, HOCBasePremium = @HOCBasePremium, SASRIAAmount = @SASRIAAmount, HOCRatesKey = @HOCRatesKey, HOCBaseConventional = @HOCBaseConventional, HOCBaseThatch = @HOCBaseThatch, HOCBaseShingle = @HOCBaseShingle WHERE AuditNumber = @AuditNumber";



        public const string riskmatrixcelldatamodel_selectwhere = "SELECT RiskMatrixCellKey, RiskMatrixRevisionKey, CreditScoreDecisionKey, RuleItemKey, GeneralStatusKey, Designation FROM [2am].[dbo].[RiskMatrixCell] WHERE";
        public const string riskmatrixcelldatamodel_selectbykey = "SELECT RiskMatrixCellKey, RiskMatrixRevisionKey, CreditScoreDecisionKey, RuleItemKey, GeneralStatusKey, Designation FROM [2am].[dbo].[RiskMatrixCell] WHERE RiskMatrixCellKey = @PrimaryKey";
        public const string riskmatrixcelldatamodel_delete = "DELETE FROM [2am].[dbo].[RiskMatrixCell] WHERE RiskMatrixCellKey = @PrimaryKey";
        public const string riskmatrixcelldatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RiskMatrixCell] WHERE";
        public const string riskmatrixcelldatamodel_insert = "INSERT INTO [2am].[dbo].[RiskMatrixCell] (RiskMatrixCellKey, RiskMatrixRevisionKey, CreditScoreDecisionKey, RuleItemKey, GeneralStatusKey, Designation) VALUES(@RiskMatrixCellKey, @RiskMatrixRevisionKey, @CreditScoreDecisionKey, @RuleItemKey, @GeneralStatusKey, @Designation); ";
        public const string riskmatrixcelldatamodel_update = "UPDATE [2am].[dbo].[RiskMatrixCell] SET RiskMatrixCellKey = @RiskMatrixCellKey, RiskMatrixRevisionKey = @RiskMatrixRevisionKey, CreditScoreDecisionKey = @CreditScoreDecisionKey, RuleItemKey = @RuleItemKey, GeneralStatusKey = @GeneralStatusKey, Designation = @Designation WHERE RiskMatrixCellKey = @RiskMatrixCellKey";



        public const string rateoverridetypedatamodel_selectwhere = "SELECT RateOverrideTypeKey, Description, RateOverrideTypeGroupKey FROM [2am].[dbo].[RateOverrideType] WHERE";
        public const string rateoverridetypedatamodel_selectbykey = "SELECT RateOverrideTypeKey, Description, RateOverrideTypeGroupKey FROM [2am].[dbo].[RateOverrideType] WHERE RateOverrideTypeKey = @PrimaryKey";
        public const string rateoverridetypedatamodel_delete = "DELETE FROM [2am].[dbo].[RateOverrideType] WHERE RateOverrideTypeKey = @PrimaryKey";
        public const string rateoverridetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RateOverrideType] WHERE";
        public const string rateoverridetypedatamodel_insert = "INSERT INTO [2am].[dbo].[RateOverrideType] (RateOverrideTypeKey, Description, RateOverrideTypeGroupKey) VALUES(@RateOverrideTypeKey, @Description, @RateOverrideTypeGroupKey); ";
        public const string rateoverridetypedatamodel_update = "UPDATE [2am].[dbo].[RateOverrideType] SET RateOverrideTypeKey = @RateOverrideTypeKey, Description = @Description, RateOverrideTypeGroupKey = @RateOverrideTypeGroupKey WHERE RateOverrideTypeKey = @RateOverrideTypeKey";



        public const string auditaffordabilityassessmentitemdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentItemKey, AffordabilityAssessmentKey, AffordabilityAssessmentItemTypeKey, ModifiedDate, ModifiedByUserId, ClientValue, CreditValue, DebtToConsolidateValue, ItemNotes FROM [2am].[dbo].[AuditAffordabilityAssessmentItem] WHERE";
        public const string auditaffordabilityassessmentitemdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentItemKey, AffordabilityAssessmentKey, AffordabilityAssessmentItemTypeKey, ModifiedDate, ModifiedByUserId, ClientValue, CreditValue, DebtToConsolidateValue, ItemNotes FROM [2am].[dbo].[AuditAffordabilityAssessmentItem] WHERE AuditNumber = @PrimaryKey";
        public const string auditaffordabilityassessmentitemdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditAffordabilityAssessmentItem] WHERE AuditNumber = @PrimaryKey";
        public const string auditaffordabilityassessmentitemdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditAffordabilityAssessmentItem] WHERE";
        public const string auditaffordabilityassessmentitemdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditAffordabilityAssessmentItem] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentItemKey, AffordabilityAssessmentKey, AffordabilityAssessmentItemTypeKey, ModifiedDate, ModifiedByUserId, ClientValue, CreditValue, DebtToConsolidateValue, ItemNotes) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @AffordabilityAssessmentItemKey, @AffordabilityAssessmentKey, @AffordabilityAssessmentItemTypeKey, @ModifiedDate, @ModifiedByUserId, @ClientValue, @CreditValue, @DebtToConsolidateValue, @ItemNotes); select cast(scope_identity() as int)";
        public const string auditaffordabilityassessmentitemdatamodel_update = "UPDATE [2am].[dbo].[AuditAffordabilityAssessmentItem] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, AffordabilityAssessmentItemKey = @AffordabilityAssessmentItemKey, AffordabilityAssessmentKey = @AffordabilityAssessmentKey, AffordabilityAssessmentItemTypeKey = @AffordabilityAssessmentItemTypeKey, ModifiedDate = @ModifiedDate, ModifiedByUserId = @ModifiedByUserId, ClientValue = @ClientValue, CreditValue = @CreditValue, DebtToConsolidateValue = @DebtToConsolidateValue, ItemNotes = @ItemNotes WHERE AuditNumber = @AuditNumber";



        public const string legalentitycleanedupdatamodel_selectwhere = "SELECT cleanupkey, LegalEntityKey, LegalEntityExceptionReasonKey, Description, Surname, Firstnames, IDNumber, Accounts FROM [2am].[dbo].[LegalEntityCleanedup] WHERE";
        public const string legalentitycleanedupdatamodel_selectbykey = "SELECT cleanupkey, LegalEntityKey, LegalEntityExceptionReasonKey, Description, Surname, Firstnames, IDNumber, Accounts FROM [2am].[dbo].[LegalEntityCleanedup] WHERE cleanupkey = @PrimaryKey";
        public const string legalentitycleanedupdatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityCleanedup] WHERE cleanupkey = @PrimaryKey";
        public const string legalentitycleanedupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityCleanedup] WHERE";
        public const string legalentitycleanedupdatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityCleanedup] (LegalEntityKey, LegalEntityExceptionReasonKey, Description, Surname, Firstnames, IDNumber, Accounts) VALUES(@LegalEntityKey, @LegalEntityExceptionReasonKey, @Description, @Surname, @Firstnames, @IDNumber, @Accounts); select cast(scope_identity() as int)";
        public const string legalentitycleanedupdatamodel_update = "UPDATE [2am].[dbo].[LegalEntityCleanedup] SET LegalEntityKey = @LegalEntityKey, LegalEntityExceptionReasonKey = @LegalEntityExceptionReasonKey, Description = @Description, Surname = @Surname, Firstnames = @Firstnames, IDNumber = @IDNumber, Accounts = @Accounts WHERE cleanupkey = @cleanupkey";



        public const string transactiontypeuidatamodel_selectwhere = "SELECT TransactionTypeUIKey, TransactionTypeKey, ScreenBatch, HTMLColour, Memo FROM [2am].[dbo].[TransactionTypeUI] WHERE";
        public const string transactiontypeuidatamodel_selectbykey = "SELECT TransactionTypeUIKey, TransactionTypeKey, ScreenBatch, HTMLColour, Memo FROM [2am].[dbo].[TransactionTypeUI] WHERE TransactionTypeUIKey = @PrimaryKey";
        public const string transactiontypeuidatamodel_delete = "DELETE FROM [2am].[dbo].[TransactionTypeUI] WHERE TransactionTypeUIKey = @PrimaryKey";
        public const string transactiontypeuidatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TransactionTypeUI] WHERE";
        public const string transactiontypeuidatamodel_insert = "INSERT INTO [2am].[dbo].[TransactionTypeUI] (TransactionTypeKey, ScreenBatch, HTMLColour, Memo) VALUES(@TransactionTypeKey, @ScreenBatch, @HTMLColour, @Memo); select cast(scope_identity() as int)";
        public const string transactiontypeuidatamodel_update = "UPDATE [2am].[dbo].[TransactionTypeUI] SET TransactionTypeKey = @TransactionTypeKey, ScreenBatch = @ScreenBatch, HTMLColour = @HTMLColour, Memo = @Memo WHERE TransactionTypeUIKey = @TransactionTypeUIKey";



        public const string auditpropertytitledeeddatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, PropertyTitleDeedKey, PropertyKey, TitleDeedNumber, DeedsOfficeKey FROM [2am].[dbo].[AuditPropertyTitleDeed] WHERE";
        public const string auditpropertytitledeeddatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, PropertyTitleDeedKey, PropertyKey, TitleDeedNumber, DeedsOfficeKey FROM [2am].[dbo].[AuditPropertyTitleDeed] WHERE AuditNumber = @PrimaryKey";
        public const string auditpropertytitledeeddatamodel_delete = "DELETE FROM [2am].[dbo].[AuditPropertyTitleDeed] WHERE AuditNumber = @PrimaryKey";
        public const string auditpropertytitledeeddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditPropertyTitleDeed] WHERE";
        public const string auditpropertytitledeeddatamodel_insert = "INSERT INTO [2am].[dbo].[AuditPropertyTitleDeed] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, PropertyTitleDeedKey, PropertyKey, TitleDeedNumber, DeedsOfficeKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @PropertyTitleDeedKey, @PropertyKey, @TitleDeedNumber, @DeedsOfficeKey); select cast(scope_identity() as int)";
        public const string auditpropertytitledeeddatamodel_update = "UPDATE [2am].[dbo].[AuditPropertyTitleDeed] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, PropertyTitleDeedKey = @PropertyTitleDeedKey, PropertyKey = @PropertyKey, TitleDeedNumber = @TitleDeedNumber, DeedsOfficeKey = @DeedsOfficeKey WHERE AuditNumber = @AuditNumber";



        public const string dataservicedatamodel_selectwhere = "SELECT DataServiceKey, Description FROM [2am].[dbo].[DataService] WHERE";
        public const string dataservicedatamodel_selectbykey = "SELECT DataServiceKey, Description FROM [2am].[dbo].[DataService] WHERE DataServiceKey = @PrimaryKey";
        public const string dataservicedatamodel_delete = "DELETE FROM [2am].[dbo].[DataService] WHERE DataServiceKey = @PrimaryKey";
        public const string dataservicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DataService] WHERE";
        public const string dataservicedatamodel_insert = "INSERT INTO [2am].[dbo].[DataService] (DataServiceKey, Description) VALUES(@DataServiceKey, @Description); ";
        public const string dataservicedatamodel_update = "UPDATE [2am].[dbo].[DataService] SET DataServiceKey = @DataServiceKey, Description = @Description WHERE DataServiceKey = @DataServiceKey";



        public const string importdedupedatamodel_selectwhere = "SELECT DedupeKey, FileKey, IDNumber, ImportStatusKey, ErrorMsg FROM [2am].[dbo].[ImportDedupe] WHERE";
        public const string importdedupedatamodel_selectbykey = "SELECT DedupeKey, FileKey, IDNumber, ImportStatusKey, ErrorMsg FROM [2am].[dbo].[ImportDedupe] WHERE DedupeKey = @PrimaryKey";
        public const string importdedupedatamodel_delete = "DELETE FROM [2am].[dbo].[ImportDedupe] WHERE DedupeKey = @PrimaryKey";
        public const string importdedupedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ImportDedupe] WHERE";
        public const string importdedupedatamodel_insert = "INSERT INTO [2am].[dbo].[ImportDedupe] (FileKey, IDNumber, ImportStatusKey, ErrorMsg) VALUES(@FileKey, @IDNumber, @ImportStatusKey, @ErrorMsg); select cast(scope_identity() as int)";
        public const string importdedupedatamodel_update = "UPDATE [2am].[dbo].[ImportDedupe] SET FileKey = @FileKey, IDNumber = @IDNumber, ImportStatusKey = @ImportStatusKey, ErrorMsg = @ErrorMsg WHERE DedupeKey = @DedupeKey";



        public const string offerattributedatamodel_selectwhere = "SELECT OfferAttributeKey, OfferKey, OfferAttributeTypeKey FROM [2am].[dbo].[OfferAttribute] WHERE";
        public const string offerattributedatamodel_selectbykey = "SELECT OfferAttributeKey, OfferKey, OfferAttributeTypeKey FROM [2am].[dbo].[OfferAttribute] WHERE OfferAttributeKey = @PrimaryKey";
        public const string offerattributedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferAttribute] WHERE OfferAttributeKey = @PrimaryKey";
        public const string offerattributedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferAttribute] WHERE";
        public const string offerattributedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferAttribute] (OfferKey, OfferAttributeTypeKey) VALUES(@OfferKey, @OfferAttributeTypeKey); select cast(scope_identity() as int)";
        public const string offerattributedatamodel_update = "UPDATE [2am].[dbo].[OfferAttribute] SET OfferKey = @OfferKey, OfferAttributeTypeKey = @OfferAttributeTypeKey WHERE OfferAttributeKey = @OfferAttributeKey";



        public const string mortgageloanpurposedatamodel_selectwhere = "SELECT MortgageLoanPurposeKey, Description, MortgageLoanPurposeGroupKey FROM [2am].[dbo].[MortgageLoanPurpose] WHERE";
        public const string mortgageloanpurposedatamodel_selectbykey = "SELECT MortgageLoanPurposeKey, Description, MortgageLoanPurposeGroupKey FROM [2am].[dbo].[MortgageLoanPurpose] WHERE MortgageLoanPurposeKey = @PrimaryKey";
        public const string mortgageloanpurposedatamodel_delete = "DELETE FROM [2am].[dbo].[MortgageLoanPurpose] WHERE MortgageLoanPurposeKey = @PrimaryKey";
        public const string mortgageloanpurposedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MortgageLoanPurpose] WHERE";
        public const string mortgageloanpurposedatamodel_insert = "INSERT INTO [2am].[dbo].[MortgageLoanPurpose] (MortgageLoanPurposeKey, Description, MortgageLoanPurposeGroupKey) VALUES(@MortgageLoanPurposeKey, @Description, @MortgageLoanPurposeGroupKey); ";
        public const string mortgageloanpurposedatamodel_update = "UPDATE [2am].[dbo].[MortgageLoanPurpose] SET MortgageLoanPurposeKey = @MortgageLoanPurposeKey, Description = @Description, MortgageLoanPurposeGroupKey = @MortgageLoanPurposeGroupKey WHERE MortgageLoanPurposeKey = @MortgageLoanPurposeKey";



        public const string hocfuturepremiumsdatamodel_selectwhere = "SELECT HOCFinancialservicekey, HOCPolicyNumber, HOCMonthlyPremium, HOCConventionalAmount, HOCThatchAmount, MortgageLoanNumber, CurrentAnniversaryDate, Effectivedate, InsertDate, HocAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey FROM [2am].[dbo].[HocFuturePremiums] WHERE";
        public const string hocfuturepremiumsdatamodel_selectbykey = "SELECT HOCFinancialservicekey, HOCPolicyNumber, HOCMonthlyPremium, HOCConventionalAmount, HOCThatchAmount, MortgageLoanNumber, CurrentAnniversaryDate, Effectivedate, InsertDate, HocAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey FROM [2am].[dbo].[HocFuturePremiums] WHERE  = @PrimaryKey";
        public const string hocfuturepremiumsdatamodel_delete = "DELETE FROM [2am].[dbo].[HocFuturePremiums] WHERE  = @PrimaryKey";
        public const string hocfuturepremiumsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HocFuturePremiums] WHERE";
        public const string hocfuturepremiumsdatamodel_insert = "INSERT INTO [2am].[dbo].[HocFuturePremiums] (HOCFinancialservicekey, HOCPolicyNumber, HOCMonthlyPremium, HOCConventionalAmount, HOCThatchAmount, MortgageLoanNumber, CurrentAnniversaryDate, Effectivedate, InsertDate, HocAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey) VALUES(@HOCFinancialservicekey, @HOCPolicyNumber, @HOCMonthlyPremium, @HOCConventionalAmount, @HOCThatchAmount, @MortgageLoanNumber, @CurrentAnniversaryDate, @Effectivedate, @InsertDate, @HocAdministrationFee, @HOCBasePremium, @SASRIAAmount, @HOCRatesKey); ";
        public const string hocfuturepremiumsdatamodel_update = "UPDATE [2am].[dbo].[HocFuturePremiums] SET HOCFinancialservicekey = @HOCFinancialservicekey, HOCPolicyNumber = @HOCPolicyNumber, HOCMonthlyPremium = @HOCMonthlyPremium, HOCConventionalAmount = @HOCConventionalAmount, HOCThatchAmount = @HOCThatchAmount, MortgageLoanNumber = @MortgageLoanNumber, CurrentAnniversaryDate = @CurrentAnniversaryDate, Effectivedate = @Effectivedate, InsertDate = @InsertDate, HocAdministrationFee = @HocAdministrationFee, HOCBasePremium = @HOCBasePremium, SASRIAAmount = @SASRIAAmount, HOCRatesKey = @HOCRatesKey WHERE  = @";



        public const string capstatusdatamodel_selectwhere = "SELECT CapStatusKey, Description FROM [2am].[dbo].[CapStatus] WHERE";
        public const string capstatusdatamodel_selectbykey = "SELECT CapStatusKey, Description FROM [2am].[dbo].[CapStatus] WHERE CapStatusKey = @PrimaryKey";
        public const string capstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[CapStatus] WHERE CapStatusKey = @PrimaryKey";
        public const string capstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CapStatus] WHERE";
        public const string capstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[CapStatus] (CapStatusKey, Description) VALUES(@CapStatusKey, @Description); ";
        public const string capstatusdatamodel_update = "UPDATE [2am].[dbo].[CapStatus] SET CapStatusKey = @CapStatusKey, Description = @Description WHERE CapStatusKey = @CapStatusKey";



        public const string accountattorneyinvoicedatamodel_selectwhere = "SELECT AccountAttorneyInvoiceKey, AccountKey, AttorneyKey, InvoiceNumber, Amount, Comment, InvoiceDate, VatAmount, TotalAmount, ChangeDate FROM [2am].[dbo].[AccountAttorneyInvoice] WHERE";
        public const string accountattorneyinvoicedatamodel_selectbykey = "SELECT AccountAttorneyInvoiceKey, AccountKey, AttorneyKey, InvoiceNumber, Amount, Comment, InvoiceDate, VatAmount, TotalAmount, ChangeDate FROM [2am].[dbo].[AccountAttorneyInvoice] WHERE AccountAttorneyInvoiceKey = @PrimaryKey";
        public const string accountattorneyinvoicedatamodel_delete = "DELETE FROM [2am].[dbo].[AccountAttorneyInvoice] WHERE AccountAttorneyInvoiceKey = @PrimaryKey";
        public const string accountattorneyinvoicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountAttorneyInvoice] WHERE";
        public const string accountattorneyinvoicedatamodel_insert = "INSERT INTO [2am].[dbo].[AccountAttorneyInvoice] (AccountKey, AttorneyKey, InvoiceNumber, Amount, Comment, InvoiceDate, VatAmount, TotalAmount, ChangeDate) VALUES(@AccountKey, @AttorneyKey, @InvoiceNumber, @Amount, @Comment, @InvoiceDate, @VatAmount, @TotalAmount, @ChangeDate); select cast(scope_identity() as int)";
        public const string accountattorneyinvoicedatamodel_update = "UPDATE [2am].[dbo].[AccountAttorneyInvoice] SET AccountKey = @AccountKey, AttorneyKey = @AttorneyKey, InvoiceNumber = @InvoiceNumber, Amount = @Amount, Comment = @Comment, InvoiceDate = @InvoiceDate, VatAmount = @VatAmount, TotalAmount = @TotalAmount, ChangeDate = @ChangeDate WHERE AccountAttorneyInvoiceKey = @AccountAttorneyInvoiceKey";



        public const string cancellationreasondatamodel_selectwhere = "SELECT CancellationReasonKey, Description FROM [2am].[dbo].[CancellationReason] WHERE";
        public const string cancellationreasondatamodel_selectbykey = "SELECT CancellationReasonKey, Description FROM [2am].[dbo].[CancellationReason] WHERE CancellationReasonKey = @PrimaryKey";
        public const string cancellationreasondatamodel_delete = "DELETE FROM [2am].[dbo].[CancellationReason] WHERE CancellationReasonKey = @PrimaryKey";
        public const string cancellationreasondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CancellationReason] WHERE";
        public const string cancellationreasondatamodel_insert = "INSERT INTO [2am].[dbo].[CancellationReason] (CancellationReasonKey, Description) VALUES(@CancellationReasonKey, @Description); ";
        public const string cancellationreasondatamodel_update = "UPDATE [2am].[dbo].[CancellationReason] SET CancellationReasonKey = @CancellationReasonKey, Description = @Description WHERE CancellationReasonKey = @CancellationReasonKey";



        public const string registrationletterinfodatamodel_selectwhere = "SELECT AccountKey, OpenDate, Description, HOCPolicyNumber, MonthlyInstallment, MonthlyPremium, HOCProRataPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, Nextinstalmentdate, InitialBalance FROM [2am].[dbo].[RegistrationLetterInfo] WHERE";
        public const string registrationletterinfodatamodel_selectbykey = "SELECT AccountKey, OpenDate, Description, HOCPolicyNumber, MonthlyInstallment, MonthlyPremium, HOCProRataPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, Nextinstalmentdate, InitialBalance FROM [2am].[dbo].[RegistrationLetterInfo] WHERE  = @PrimaryKey";
        public const string registrationletterinfodatamodel_delete = "DELETE FROM [2am].[dbo].[RegistrationLetterInfo] WHERE  = @PrimaryKey";
        public const string registrationletterinfodatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RegistrationLetterInfo] WHERE";
        public const string registrationletterinfodatamodel_insert = "INSERT INTO [2am].[dbo].[RegistrationLetterInfo] (AccountKey, OpenDate, Description, HOCPolicyNumber, MonthlyInstallment, MonthlyPremium, HOCProRataPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, Nextinstalmentdate, InitialBalance) VALUES(@AccountKey, @OpenDate, @Description, @HOCPolicyNumber, @MonthlyInstallment, @MonthlyPremium, @HOCProRataPremium, @HOCThatchAmount, @HOCConventionalAmount, @HOCShingleAmount, @HOCTotalSumInsured, @Nextinstalmentdate, @InitialBalance); ";
        public const string registrationletterinfodatamodel_update = "UPDATE [2am].[dbo].[RegistrationLetterInfo] SET AccountKey = @AccountKey, OpenDate = @OpenDate, Description = @Description, HOCPolicyNumber = @HOCPolicyNumber, MonthlyInstallment = @MonthlyInstallment, MonthlyPremium = @MonthlyPremium, HOCProRataPremium = @HOCProRataPremium, HOCThatchAmount = @HOCThatchAmount, HOCConventionalAmount = @HOCConventionalAmount, HOCShingleAmount = @HOCShingleAmount, HOCTotalSumInsured = @HOCTotalSumInsured, Nextinstalmentdate = @Nextinstalmentdate, InitialBalance = @InitialBalance WHERE  = @";



        public const string cdctablelist_adhocdatamodel_selectwhere = "SELECT Name, [Schema] FROM [2am].[dbo].[cdctablelist_adhoc] WHERE";
        public const string cdctablelist_adhocdatamodel_selectbykey = "SELECT Name, [Schema] FROM [2am].[dbo].[cdctablelist_adhoc] WHERE  = @PrimaryKey";
        public const string cdctablelist_adhocdatamodel_delete = "DELETE FROM [2am].[dbo].[cdctablelist_adhoc] WHERE  = @PrimaryKey";
        public const string cdctablelist_adhocdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[cdctablelist_adhoc] WHERE";
        public const string cdctablelist_adhocdatamodel_insert = "INSERT INTO [2am].[dbo].[cdctablelist_adhoc] (Name, [Schema]) VALUES(@Name, @Schema); ";
        public const string cdctablelist_adhocdatamodel_update = "UPDATE [2am].[dbo].[cdctablelist_adhoc] SET Name = @Name, [Schema] = @Schema WHERE  = @";



        public const string accountbaseliidatamodel_selectwhere = "SELECT AccountBaselIIKey, AccountKey, AccountingDate, ProcessDate, LGD, EAD, BehaviouralScore, PD, EL FROM [2am].[dbo].[AccountBaselII] WHERE";
        public const string accountbaseliidatamodel_selectbykey = "SELECT AccountBaselIIKey, AccountKey, AccountingDate, ProcessDate, LGD, EAD, BehaviouralScore, PD, EL FROM [2am].[dbo].[AccountBaselII] WHERE AccountBaselIIKey = @PrimaryKey";
        public const string accountbaseliidatamodel_delete = "DELETE FROM [2am].[dbo].[AccountBaselII] WHERE AccountBaselIIKey = @PrimaryKey";
        public const string accountbaseliidatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountBaselII] WHERE";
        public const string accountbaseliidatamodel_insert = "INSERT INTO [2am].[dbo].[AccountBaselII] (AccountKey, AccountingDate, ProcessDate, LGD, EAD, BehaviouralScore, PD, EL) VALUES(@AccountKey, @AccountingDate, @ProcessDate, @LGD, @EAD, @BehaviouralScore, @PD, @EL); select cast(scope_identity() as int)";
        public const string accountbaseliidatamodel_update = "UPDATE [2am].[dbo].[AccountBaselII] SET AccountKey = @AccountKey, AccountingDate = @AccountingDate, ProcessDate = @ProcessDate, LGD = @LGD, EAD = @EAD, BehaviouralScore = @BehaviouralScore, PD = @PD, EL = @EL WHERE AccountBaselIIKey = @AccountBaselIIKey";



        public const string originationsourceicondatamodel_selectwhere = "SELECT OriginationSourceIconKey, OriginationSourceKey, Icon FROM [2am].[dbo].[OriginationSourceIcon] WHERE";
        public const string originationsourceicondatamodel_selectbykey = "SELECT OriginationSourceIconKey, OriginationSourceKey, Icon FROM [2am].[dbo].[OriginationSourceIcon] WHERE OriginationSourceIconKey = @PrimaryKey";
        public const string originationsourceicondatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceIcon] WHERE OriginationSourceIconKey = @PrimaryKey";
        public const string originationsourceicondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceIcon] WHERE";
        public const string originationsourceicondatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceIcon] (OriginationSourceKey, Icon) VALUES(@OriginationSourceKey, @Icon); select cast(scope_identity() as int)";
        public const string originationsourceicondatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceIcon] SET OriginationSourceKey = @OriginationSourceKey, Icon = @Icon WHERE OriginationSourceIconKey = @OriginationSourceIconKey";



        public const string auditfinancialtranposteddatamodel_selectwhere = "SELECT AuditNumber, LoanNumber, TransactionTypeNumber, TransactionEffectiveDate, TransactionInsertDate, TransactionAmount, TransactionReference, UserID FROM [2am].[dbo].[AuditFinancialTranPosted] WHERE";
        public const string auditfinancialtranposteddatamodel_selectbykey = "SELECT AuditNumber, LoanNumber, TransactionTypeNumber, TransactionEffectiveDate, TransactionInsertDate, TransactionAmount, TransactionReference, UserID FROM [2am].[dbo].[AuditFinancialTranPosted] WHERE AuditNumber = @PrimaryKey";
        public const string auditfinancialtranposteddatamodel_delete = "DELETE FROM [2am].[dbo].[AuditFinancialTranPosted] WHERE AuditNumber = @PrimaryKey";
        public const string auditfinancialtranposteddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditFinancialTranPosted] WHERE";
        public const string auditfinancialtranposteddatamodel_insert = "INSERT INTO [2am].[dbo].[AuditFinancialTranPosted] (LoanNumber, TransactionTypeNumber, TransactionEffectiveDate, TransactionInsertDate, TransactionAmount, TransactionReference, UserID) VALUES(@LoanNumber, @TransactionTypeNumber, @TransactionEffectiveDate, @TransactionInsertDate, @TransactionAmount, @TransactionReference, @UserID); select cast(scope_identity() as int)";
        public const string auditfinancialtranposteddatamodel_update = "UPDATE [2am].[dbo].[AuditFinancialTranPosted] SET LoanNumber = @LoanNumber, TransactionTypeNumber = @TransactionTypeNumber, TransactionEffectiveDate = @TransactionEffectiveDate, TransactionInsertDate = @TransactionInsertDate, TransactionAmount = @TransactionAmount, TransactionReference = @TransactionReference, UserID = @UserID WHERE AuditNumber = @AuditNumber";



        public const string ospfinancialadjustmenttypesourcedatamodel_selectwhere = "SELECT OSPFinancialAdjustmentTypeSourceKey, FinancialAdjustmentTypeSourceKey, OriginationSourceProductKey FROM [2am].[dbo].[OSPFinancialAdjustmentTypeSource] WHERE";
        public const string ospfinancialadjustmenttypesourcedatamodel_selectbykey = "SELECT OSPFinancialAdjustmentTypeSourceKey, FinancialAdjustmentTypeSourceKey, OriginationSourceProductKey FROM [2am].[dbo].[OSPFinancialAdjustmentTypeSource] WHERE OSPFinancialAdjustmentTypeSourceKey = @PrimaryKey";
        public const string ospfinancialadjustmenttypesourcedatamodel_delete = "DELETE FROM [2am].[dbo].[OSPFinancialAdjustmentTypeSource] WHERE OSPFinancialAdjustmentTypeSourceKey = @PrimaryKey";
        public const string ospfinancialadjustmenttypesourcedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OSPFinancialAdjustmentTypeSource] WHERE";
        public const string ospfinancialadjustmenttypesourcedatamodel_insert = "INSERT INTO [2am].[dbo].[OSPFinancialAdjustmentTypeSource] (FinancialAdjustmentTypeSourceKey, OriginationSourceProductKey) VALUES(@FinancialAdjustmentTypeSourceKey, @OriginationSourceProductKey); select cast(scope_identity() as int)";
        public const string ospfinancialadjustmenttypesourcedatamodel_update = "UPDATE [2am].[dbo].[OSPFinancialAdjustmentTypeSource] SET FinancialAdjustmentTypeSourceKey = @FinancialAdjustmentTypeSourceKey, OriginationSourceProductKey = @OriginationSourceProductKey WHERE OSPFinancialAdjustmentTypeSourceKey = @OSPFinancialAdjustmentTypeSourceKey";



        public const string financialservicetypedatamodel_selectwhere = "SELECT FinancialServiceTypeKey, Description, ResetConfigurationKey, BalanceTypeKey, FinancialServiceGroupKey FROM [2am].[dbo].[FinancialServiceType] WHERE";
        public const string financialservicetypedatamodel_selectbykey = "SELECT FinancialServiceTypeKey, Description, ResetConfigurationKey, BalanceTypeKey, FinancialServiceGroupKey FROM [2am].[dbo].[FinancialServiceType] WHERE FinancialServiceTypeKey = @PrimaryKey";
        public const string financialservicetypedatamodel_delete = "DELETE FROM [2am].[dbo].[FinancialServiceType] WHERE FinancialServiceTypeKey = @PrimaryKey";
        public const string financialservicetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FinancialServiceType] WHERE";
        public const string financialservicetypedatamodel_insert = "INSERT INTO [2am].[dbo].[FinancialServiceType] (FinancialServiceTypeKey, Description, ResetConfigurationKey, BalanceTypeKey, FinancialServiceGroupKey) VALUES(@FinancialServiceTypeKey, @Description, @ResetConfigurationKey, @BalanceTypeKey, @FinancialServiceGroupKey); ";
        public const string financialservicetypedatamodel_update = "UPDATE [2am].[dbo].[FinancialServiceType] SET FinancialServiceTypeKey = @FinancialServiceTypeKey, Description = @Description, ResetConfigurationKey = @ResetConfigurationKey, BalanceTypeKey = @BalanceTypeKey, FinancialServiceGroupKey = @FinancialServiceGroupKey WHERE FinancialServiceTypeKey = @FinancialServiceTypeKey";



        public const string correspondencemediumreportstatementdatamodel_selectwhere = "SELECT CorrespondenceMediumReportStatementKey, ReportStatementKey, CorrespondenceMediumKey FROM [2am].[dbo].[CorrespondenceMediumReportStatement] WHERE";
        public const string correspondencemediumreportstatementdatamodel_selectbykey = "SELECT CorrespondenceMediumReportStatementKey, ReportStatementKey, CorrespondenceMediumKey FROM [2am].[dbo].[CorrespondenceMediumReportStatement] WHERE CorrespondenceMediumReportStatementKey = @PrimaryKey";
        public const string correspondencemediumreportstatementdatamodel_delete = "DELETE FROM [2am].[dbo].[CorrespondenceMediumReportStatement] WHERE CorrespondenceMediumReportStatementKey = @PrimaryKey";
        public const string correspondencemediumreportstatementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CorrespondenceMediumReportStatement] WHERE";
        public const string correspondencemediumreportstatementdatamodel_insert = "INSERT INTO [2am].[dbo].[CorrespondenceMediumReportStatement] (ReportStatementKey, CorrespondenceMediumKey) VALUES(@ReportStatementKey, @CorrespondenceMediumKey); select cast(scope_identity() as int)";
        public const string correspondencemediumreportstatementdatamodel_update = "UPDATE [2am].[dbo].[CorrespondenceMediumReportStatement] SET ReportStatementKey = @ReportStatementKey, CorrespondenceMediumKey = @CorrespondenceMediumKey WHERE CorrespondenceMediumReportStatementKey = @CorrespondenceMediumReportStatementKey";



        public const string tmp_life_bulkleaduploaddatamodel_selectwhere = "SELECT LoanNumber, Consultant, Result, Date FROM [2am].[dbo].[tmp_Life_BulkLeadUpload] WHERE";
        public const string tmp_life_bulkleaduploaddatamodel_selectbykey = "SELECT LoanNumber, Consultant, Result, Date FROM [2am].[dbo].[tmp_Life_BulkLeadUpload] WHERE  = @PrimaryKey";
        public const string tmp_life_bulkleaduploaddatamodel_delete = "DELETE FROM [2am].[dbo].[tmp_Life_BulkLeadUpload] WHERE  = @PrimaryKey";
        public const string tmp_life_bulkleaduploaddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[tmp_Life_BulkLeadUpload] WHERE";
        public const string tmp_life_bulkleaduploaddatamodel_insert = "INSERT INTO [2am].[dbo].[tmp_Life_BulkLeadUpload] (LoanNumber, Consultant, Result, Date) VALUES(@LoanNumber, @Consultant, @Result, @Date); ";
        public const string tmp_life_bulkleaduploaddatamodel_update = "UPDATE [2am].[dbo].[tmp_Life_BulkLeadUpload] SET LoanNumber = @LoanNumber, Consultant = @Consultant, Result = @Result, Date = @Date WHERE  = @";



        public const string categorydatamodel_selectwhere = "SELECT CategoryKey, Value, Description FROM [2am].[dbo].[Category] WHERE";
        public const string categorydatamodel_selectbykey = "SELECT CategoryKey, Value, Description FROM [2am].[dbo].[Category] WHERE CategoryKey = @PrimaryKey";
        public const string categorydatamodel_delete = "DELETE FROM [2am].[dbo].[Category] WHERE CategoryKey = @PrimaryKey";
        public const string categorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Category] WHERE";
        public const string categorydatamodel_insert = "INSERT INTO [2am].[dbo].[Category] (CategoryKey, Value, Description) VALUES(@CategoryKey, @Value, @Description); ";
        public const string categorydatamodel_update = "UPDATE [2am].[dbo].[Category] SET CategoryKey = @CategoryKey, Value = @Value, Description = @Description WHERE CategoryKey = @CategoryKey";



        public const string paymentsplittypedatamodel_selectwhere = "SELECT PaymentSplitTypeKey, Description FROM [2am].[dbo].[PaymentSplitType] WHERE";
        public const string paymentsplittypedatamodel_selectbykey = "SELECT PaymentSplitTypeKey, Description FROM [2am].[dbo].[PaymentSplitType] WHERE PaymentSplitTypeKey = @PrimaryKey";
        public const string paymentsplittypedatamodel_delete = "DELETE FROM [2am].[dbo].[PaymentSplitType] WHERE PaymentSplitTypeKey = @PrimaryKey";
        public const string paymentsplittypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PaymentSplitType] WHERE";
        public const string paymentsplittypedatamodel_insert = "INSERT INTO [2am].[dbo].[PaymentSplitType] (PaymentSplitTypeKey, Description) VALUES(@PaymentSplitTypeKey, @Description); ";
        public const string paymentsplittypedatamodel_update = "UPDATE [2am].[dbo].[PaymentSplitType] SET PaymentSplitTypeKey = @PaymentSplitTypeKey, Description = @Description WHERE PaymentSplitTypeKey = @PaymentSplitTypeKey";



        public const string countrydatamodel_selectwhere = "SELECT CountryKey, Description, AllowFreeTextFormat FROM [2am].[dbo].[Country] WHERE";
        public const string countrydatamodel_selectbykey = "SELECT CountryKey, Description, AllowFreeTextFormat FROM [2am].[dbo].[Country] WHERE CountryKey = @PrimaryKey";
        public const string countrydatamodel_delete = "DELETE FROM [2am].[dbo].[Country] WHERE CountryKey = @PrimaryKey";
        public const string countrydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Country] WHERE";
        public const string countrydatamodel_insert = "INSERT INTO [2am].[dbo].[Country] (Description, AllowFreeTextFormat) VALUES(@Description, @AllowFreeTextFormat); select cast(scope_identity() as int)";
        public const string countrydatamodel_update = "UPDATE [2am].[dbo].[Country] SET Description = @Description, AllowFreeTextFormat = @AllowFreeTextFormat WHERE CountryKey = @CountryKey";



        public const string auditbonddatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BondKey, DeedsOfficeKey, AttorneyKey, BondRegistrationNumber, BondRegistrationDate, BondRegistrationAmount, BondLoanAgreementAmount, UserID, ChangeDate, OfferKey FROM [2am].[dbo].[AuditBond] WHERE";
        public const string auditbonddatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BondKey, DeedsOfficeKey, AttorneyKey, BondRegistrationNumber, BondRegistrationDate, BondRegistrationAmount, BondLoanAgreementAmount, UserID, ChangeDate, OfferKey FROM [2am].[dbo].[AuditBond] WHERE AuditNumber = @PrimaryKey";
        public const string auditbonddatamodel_delete = "DELETE FROM [2am].[dbo].[AuditBond] WHERE AuditNumber = @PrimaryKey";
        public const string auditbonddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditBond] WHERE";
        public const string auditbonddatamodel_insert = "INSERT INTO [2am].[dbo].[AuditBond] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BondKey, DeedsOfficeKey, AttorneyKey, BondRegistrationNumber, BondRegistrationDate, BondRegistrationAmount, BondLoanAgreementAmount, UserID, ChangeDate, OfferKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @BondKey, @DeedsOfficeKey, @AttorneyKey, @BondRegistrationNumber, @BondRegistrationDate, @BondRegistrationAmount, @BondLoanAgreementAmount, @UserID, @ChangeDate, @OfferKey); select cast(scope_identity() as int)";
        public const string auditbonddatamodel_update = "UPDATE [2am].[dbo].[AuditBond] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, BondKey = @BondKey, DeedsOfficeKey = @DeedsOfficeKey, AttorneyKey = @AttorneyKey, BondRegistrationNumber = @BondRegistrationNumber, BondRegistrationDate = @BondRegistrationDate, BondRegistrationAmount = @BondRegistrationAmount, BondLoanAgreementAmount = @BondLoanAgreementAmount, UserID = @UserID, ChangeDate = @ChangeDate, OfferKey = @OfferKey WHERE AuditNumber = @AuditNumber";



        public const string monthendinterestdatamodel_selectwhere = "SELECT FinancialServiceKey, AccruedInterest, LoyaltyBenifit, CoPayment, AccruedInterestNew, LoyaltyBenefitNew, CoPaymentNew, AcruedInterestMonthEnd, CoPaymentMonthend FROM [2am].[dbo].[MonthEndInterest] WHERE";
        public const string monthendinterestdatamodel_selectbykey = "SELECT FinancialServiceKey, AccruedInterest, LoyaltyBenifit, CoPayment, AccruedInterestNew, LoyaltyBenefitNew, CoPaymentNew, AcruedInterestMonthEnd, CoPaymentMonthend FROM [2am].[dbo].[MonthEndInterest] WHERE FinancialServiceKey = @PrimaryKey";
        public const string monthendinterestdatamodel_delete = "DELETE FROM [2am].[dbo].[MonthEndInterest] WHERE FinancialServiceKey = @PrimaryKey";
        public const string monthendinterestdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MonthEndInterest] WHERE";
        public const string monthendinterestdatamodel_insert = "INSERT INTO [2am].[dbo].[MonthEndInterest] (FinancialServiceKey, AccruedInterest, LoyaltyBenifit, CoPayment, AccruedInterestNew, LoyaltyBenefitNew, CoPaymentNew, AcruedInterestMonthEnd, CoPaymentMonthend) VALUES(@FinancialServiceKey, @AccruedInterest, @LoyaltyBenifit, @CoPayment, @AccruedInterestNew, @LoyaltyBenefitNew, @CoPaymentNew, @AcruedInterestMonthEnd, @CoPaymentMonthend); ";
        public const string monthendinterestdatamodel_update = "UPDATE [2am].[dbo].[MonthEndInterest] SET FinancialServiceKey = @FinancialServiceKey, AccruedInterest = @AccruedInterest, LoyaltyBenifit = @LoyaltyBenifit, CoPayment = @CoPayment, AccruedInterestNew = @AccruedInterestNew, LoyaltyBenefitNew = @LoyaltyBenefitNew, CoPaymentNew = @CoPaymentNew, AcruedInterestMonthEnd = @AcruedInterestMonthEnd, CoPaymentMonthend = @CoPaymentMonthend WHERE FinancialServiceKey = @FinancialServiceKey";



        public const string originationsourceproductcreditmatrixdatamodel_selectwhere = "SELECT OriginationSourceProductCreditMatrixKey, CreditMatrixKey, OriginationSourceProductKey FROM [2am].[dbo].[OriginationSourceProductCreditMatrix] WHERE";
        public const string originationsourceproductcreditmatrixdatamodel_selectbykey = "SELECT OriginationSourceProductCreditMatrixKey, CreditMatrixKey, OriginationSourceProductKey FROM [2am].[dbo].[OriginationSourceProductCreditMatrix] WHERE OriginationSourceProductCreditMatrixKey = @PrimaryKey";
        public const string originationsourceproductcreditmatrixdatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceProductCreditMatrix] WHERE OriginationSourceProductCreditMatrixKey = @PrimaryKey";
        public const string originationsourceproductcreditmatrixdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceProductCreditMatrix] WHERE";
        public const string originationsourceproductcreditmatrixdatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceProductCreditMatrix] (CreditMatrixKey, OriginationSourceProductKey) VALUES(@CreditMatrixKey, @OriginationSourceProductKey); select cast(scope_identity() as int)";
        public const string originationsourceproductcreditmatrixdatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceProductCreditMatrix] SET CreditMatrixKey = @CreditMatrixKey, OriginationSourceProductKey = @OriginationSourceProductKey WHERE OriginationSourceProductCreditMatrixKey = @OriginationSourceProductCreditMatrixKey";



        public const string applicationdocumenttypedatamodel_selectwhere = "SELECT ApplicationDocumentTypeKey, Description FROM [2am].[dbo].[ApplicationDocumentType] WHERE";
        public const string applicationdocumenttypedatamodel_selectbykey = "SELECT ApplicationDocumentTypeKey, Description FROM [2am].[dbo].[ApplicationDocumentType] WHERE ApplicationDocumentTypeKey = @PrimaryKey";
        public const string applicationdocumenttypedatamodel_delete = "DELETE FROM [2am].[dbo].[ApplicationDocumentType] WHERE ApplicationDocumentTypeKey = @PrimaryKey";
        public const string applicationdocumenttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ApplicationDocumentType] WHERE";
        public const string applicationdocumenttypedatamodel_insert = "INSERT INTO [2am].[dbo].[ApplicationDocumentType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string applicationdocumenttypedatamodel_update = "UPDATE [2am].[dbo].[ApplicationDocumentType] SET Description = @Description WHERE ApplicationDocumentTypeKey = @ApplicationDocumentTypeKey";



        public const string riskmatrixcelldimensiondatamodel_selectwhere = "SELECT RiskMatrixCellDimensionKey, RiskMatrixCellKey, RiskMatrixDimensionKey, RiskMatrixRangeKey FROM [2am].[dbo].[RiskMatrixCellDimension] WHERE";
        public const string riskmatrixcelldimensiondatamodel_selectbykey = "SELECT RiskMatrixCellDimensionKey, RiskMatrixCellKey, RiskMatrixDimensionKey, RiskMatrixRangeKey FROM [2am].[dbo].[RiskMatrixCellDimension] WHERE RiskMatrixCellDimensionKey = @PrimaryKey";
        public const string riskmatrixcelldimensiondatamodel_delete = "DELETE FROM [2am].[dbo].[RiskMatrixCellDimension] WHERE RiskMatrixCellDimensionKey = @PrimaryKey";
        public const string riskmatrixcelldimensiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RiskMatrixCellDimension] WHERE";
        public const string riskmatrixcelldimensiondatamodel_insert = "INSERT INTO [2am].[dbo].[RiskMatrixCellDimension] (RiskMatrixCellDimensionKey, RiskMatrixCellKey, RiskMatrixDimensionKey, RiskMatrixRangeKey) VALUES(@RiskMatrixCellDimensionKey, @RiskMatrixCellKey, @RiskMatrixDimensionKey, @RiskMatrixRangeKey); ";
        public const string riskmatrixcelldimensiondatamodel_update = "UPDATE [2am].[dbo].[RiskMatrixCellDimension] SET RiskMatrixCellDimensionKey = @RiskMatrixCellDimensionKey, RiskMatrixCellKey = @RiskMatrixCellKey, RiskMatrixDimensionKey = @RiskMatrixDimensionKey, RiskMatrixRangeKey = @RiskMatrixRangeKey WHERE RiskMatrixCellDimensionKey = @RiskMatrixCellDimensionKey";



        public const string behaviouralscorecategorydatamodel_selectwhere = "SELECT BehaviouralScoreCategoryKey, Description, BehaviouralScore, ThresholdColour FROM [2am].[dbo].[BehaviouralScoreCategory] WHERE";
        public const string behaviouralscorecategorydatamodel_selectbykey = "SELECT BehaviouralScoreCategoryKey, Description, BehaviouralScore, ThresholdColour FROM [2am].[dbo].[BehaviouralScoreCategory] WHERE BehaviouralScoreCategoryKey = @PrimaryKey";
        public const string behaviouralscorecategorydatamodel_delete = "DELETE FROM [2am].[dbo].[BehaviouralScoreCategory] WHERE BehaviouralScoreCategoryKey = @PrimaryKey";
        public const string behaviouralscorecategorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BehaviouralScoreCategory] WHERE";
        public const string behaviouralscorecategorydatamodel_insert = "INSERT INTO [2am].[dbo].[BehaviouralScoreCategory] (BehaviouralScoreCategoryKey, Description, BehaviouralScore, ThresholdColour) VALUES(@BehaviouralScoreCategoryKey, @Description, @BehaviouralScore, @ThresholdColour); ";
        public const string behaviouralscorecategorydatamodel_update = "UPDATE [2am].[dbo].[BehaviouralScoreCategory] SET BehaviouralScoreCategoryKey = @BehaviouralScoreCategoryKey, Description = @Description, BehaviouralScore = @BehaviouralScore, ThresholdColour = @ThresholdColour WHERE BehaviouralScoreCategoryKey = @BehaviouralScoreCategoryKey";



        public const string offerinformationfinancialadjustmentdatamodel_selectwhere = "SELECT OfferInformationFinancialAdjustmentKey, OfferInformationKey, FinancialAdjustmentTypeSourceKey, Term, CapRate, CAPBalance, FloorRate, FixedRate, Discount, FromDate FROM [2am].[dbo].[OfferInformationFinancialAdjustment] WHERE";
        public const string offerinformationfinancialadjustmentdatamodel_selectbykey = "SELECT OfferInformationFinancialAdjustmentKey, OfferInformationKey, FinancialAdjustmentTypeSourceKey, Term, CapRate, CAPBalance, FloorRate, FixedRate, Discount, FromDate FROM [2am].[dbo].[OfferInformationFinancialAdjustment] WHERE OfferInformationFinancialAdjustmentKey = @PrimaryKey";
        public const string offerinformationfinancialadjustmentdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationFinancialAdjustment] WHERE OfferInformationFinancialAdjustmentKey = @PrimaryKey";
        public const string offerinformationfinancialadjustmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationFinancialAdjustment] WHERE";
        public const string offerinformationfinancialadjustmentdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationFinancialAdjustment] (OfferInformationKey, FinancialAdjustmentTypeSourceKey, Term, CapRate, CAPBalance, FloorRate, FixedRate, Discount, FromDate) VALUES(@OfferInformationKey, @FinancialAdjustmentTypeSourceKey, @Term, @CapRate, @CAPBalance, @FloorRate, @FixedRate, @Discount, @FromDate); select cast(scope_identity() as int)";
        public const string offerinformationfinancialadjustmentdatamodel_update = "UPDATE [2am].[dbo].[OfferInformationFinancialAdjustment] SET OfferInformationKey = @OfferInformationKey, FinancialAdjustmentTypeSourceKey = @FinancialAdjustmentTypeSourceKey, Term = @Term, CapRate = @CapRate, CAPBalance = @CAPBalance, FloorRate = @FloorRate, FixedRate = @FixedRate, Discount = @Discount, FromDate = @FromDate WHERE OfferInformationFinancialAdjustmentKey = @OfferInformationFinancialAdjustmentKey";



        public const string originationsourceattorneydatamodel_selectwhere = "SELECT OriginationSourceAttorneyKey, OriginationSourceKey, AttorneyKey FROM [2am].[dbo].[OriginationSourceAttorney] WHERE";
        public const string originationsourceattorneydatamodel_selectbykey = "SELECT OriginationSourceAttorneyKey, OriginationSourceKey, AttorneyKey FROM [2am].[dbo].[OriginationSourceAttorney] WHERE OriginationSourceAttorneyKey = @PrimaryKey";
        public const string originationsourceattorneydatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceAttorney] WHERE OriginationSourceAttorneyKey = @PrimaryKey";
        public const string originationsourceattorneydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceAttorney] WHERE";
        public const string originationsourceattorneydatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceAttorney] (OriginationSourceKey, AttorneyKey) VALUES(@OriginationSourceKey, @AttorneyKey); select cast(scope_identity() as int)";
        public const string originationsourceattorneydatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceAttorney] SET OriginationSourceKey = @OriginationSourceKey, AttorneyKey = @AttorneyKey WHERE OriginationSourceAttorneyKey = @OriginationSourceAttorneyKey";



        public const string auditaffordabilityassessmentlegalentitydatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentLegalEntityKey, AffordabilityAssessmentKey, LegalEntityKey FROM [2am].[dbo].[AuditAffordabilityAssessmentLegalEntity] WHERE";
        public const string auditaffordabilityassessmentlegalentitydatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentLegalEntityKey, AffordabilityAssessmentKey, LegalEntityKey FROM [2am].[dbo].[AuditAffordabilityAssessmentLegalEntity] WHERE AuditNumber = @PrimaryKey";
        public const string auditaffordabilityassessmentlegalentitydatamodel_delete = "DELETE FROM [2am].[dbo].[AuditAffordabilityAssessmentLegalEntity] WHERE AuditNumber = @PrimaryKey";
        public const string auditaffordabilityassessmentlegalentitydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditAffordabilityAssessmentLegalEntity] WHERE";
        public const string auditaffordabilityassessmentlegalentitydatamodel_insert = "INSERT INTO [2am].[dbo].[AuditAffordabilityAssessmentLegalEntity] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, AffordabilityAssessmentLegalEntityKey, AffordabilityAssessmentKey, LegalEntityKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @AffordabilityAssessmentLegalEntityKey, @AffordabilityAssessmentKey, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string auditaffordabilityassessmentlegalentitydatamodel_update = "UPDATE [2am].[dbo].[AuditAffordabilityAssessmentLegalEntity] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, AffordabilityAssessmentLegalEntityKey = @AffordabilityAssessmentLegalEntityKey, AffordabilityAssessmentKey = @AffordabilityAssessmentKey, LegalEntityKey = @LegalEntityKey WHERE AuditNumber = @AuditNumber";



        public const string marketingoptionrelevancedatamodel_selectwhere = "SELECT MarketingOptionRelevanceKey, Description FROM [2am].[dbo].[MarketingOptionRelevance] WHERE";
        public const string marketingoptionrelevancedatamodel_selectbykey = "SELECT MarketingOptionRelevanceKey, Description FROM [2am].[dbo].[MarketingOptionRelevance] WHERE MarketingOptionRelevanceKey = @PrimaryKey";
        public const string marketingoptionrelevancedatamodel_delete = "DELETE FROM [2am].[dbo].[MarketingOptionRelevance] WHERE MarketingOptionRelevanceKey = @PrimaryKey";
        public const string marketingoptionrelevancedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MarketingOptionRelevance] WHERE";
        public const string marketingoptionrelevancedatamodel_insert = "INSERT INTO [2am].[dbo].[MarketingOptionRelevance] (MarketingOptionRelevanceKey, Description) VALUES(@MarketingOptionRelevanceKey, @Description); ";
        public const string marketingoptionrelevancedatamodel_update = "UPDATE [2am].[dbo].[MarketingOptionRelevance] SET MarketingOptionRelevanceKey = @MarketingOptionRelevanceKey, Description = @Description WHERE MarketingOptionRelevanceKey = @MarketingOptionRelevanceKey";



        public const string tranchetypedatamodel_selectwhere = "SELECT TrancheTypeKey, Description FROM [2am].[dbo].[TrancheType] WHERE";
        public const string tranchetypedatamodel_selectbykey = "SELECT TrancheTypeKey, Description FROM [2am].[dbo].[TrancheType] WHERE TrancheTypeKey = @PrimaryKey";
        public const string tranchetypedatamodel_delete = "DELETE FROM [2am].[dbo].[TrancheType] WHERE TrancheTypeKey = @PrimaryKey";
        public const string tranchetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TrancheType] WHERE";
        public const string tranchetypedatamodel_insert = "INSERT INTO [2am].[dbo].[TrancheType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string tranchetypedatamodel_update = "UPDATE [2am].[dbo].[TrancheType] SET Description = @Description WHERE TrancheTypeKey = @TrancheTypeKey";



        public const string auditrateconfigurationdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, RateConfigurationKey, MarketRateKey, MarginKey FROM [2am].[dbo].[AuditRateConfiguration] WHERE";
        public const string auditrateconfigurationdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, RateConfigurationKey, MarketRateKey, MarginKey FROM [2am].[dbo].[AuditRateConfiguration] WHERE AuditNumber = @PrimaryKey";
        public const string auditrateconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditRateConfiguration] WHERE AuditNumber = @PrimaryKey";
        public const string auditrateconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditRateConfiguration] WHERE";
        public const string auditrateconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditRateConfiguration] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, RateConfigurationKey, MarketRateKey, MarginKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @RateConfigurationKey, @MarketRateKey, @MarginKey); select cast(scope_identity() as int)";
        public const string auditrateconfigurationdatamodel_update = "UPDATE [2am].[dbo].[AuditRateConfiguration] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, RateConfigurationKey = @RateConfigurationKey, MarketRateKey = @MarketRateKey, MarginKey = @MarginKey WHERE AuditNumber = @AuditNumber";



        public const string originationsourceproductpurposedatamodel_selectwhere = "SELECT OriginationSourceProductPurposeKey, OriginationSourceProductKey, MortgageLoanPurposeKey FROM [2am].[dbo].[OriginationSourceProductPurpose] WHERE";
        public const string originationsourceproductpurposedatamodel_selectbykey = "SELECT OriginationSourceProductPurposeKey, OriginationSourceProductKey, MortgageLoanPurposeKey FROM [2am].[dbo].[OriginationSourceProductPurpose] WHERE OriginationSourceProductPurposeKey = @PrimaryKey";
        public const string originationsourceproductpurposedatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceProductPurpose] WHERE OriginationSourceProductPurposeKey = @PrimaryKey";
        public const string originationsourceproductpurposedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceProductPurpose] WHERE";
        public const string originationsourceproductpurposedatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceProductPurpose] (OriginationSourceProductKey, MortgageLoanPurposeKey) VALUES(@OriginationSourceProductKey, @MortgageLoanPurposeKey); select cast(scope_identity() as int)";
        public const string originationsourceproductpurposedatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceProductPurpose] SET OriginationSourceProductKey = @OriginationSourceProductKey, MortgageLoanPurposeKey = @MortgageLoanPurposeKey WHERE OriginationSourceProductPurposeKey = @OriginationSourceProductPurposeKey";



        public const string offerinformationappliedrateadjustmentdatamodel_selectwhere = "SELECT OfferInformationAppliedRateAdjustmentKey, OfferElementValue, RateAdjustmentElementKey, ADUserKey, OfferInformationFinancialAdjustmentKey, ChangeDate FROM [2am].[dbo].[OfferInformationAppliedRateAdjustment] WHERE";
        public const string offerinformationappliedrateadjustmentdatamodel_selectbykey = "SELECT OfferInformationAppliedRateAdjustmentKey, OfferElementValue, RateAdjustmentElementKey, ADUserKey, OfferInformationFinancialAdjustmentKey, ChangeDate FROM [2am].[dbo].[OfferInformationAppliedRateAdjustment] WHERE OfferInformationAppliedRateAdjustmentKey = @PrimaryKey";
        public const string offerinformationappliedrateadjustmentdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationAppliedRateAdjustment] WHERE OfferInformationAppliedRateAdjustmentKey = @PrimaryKey";
        public const string offerinformationappliedrateadjustmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationAppliedRateAdjustment] WHERE";
        public const string offerinformationappliedrateadjustmentdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationAppliedRateAdjustment] (OfferElementValue, RateAdjustmentElementKey, ADUserKey, OfferInformationFinancialAdjustmentKey, ChangeDate) VALUES(@OfferElementValue, @RateAdjustmentElementKey, @ADUserKey, @OfferInformationFinancialAdjustmentKey, @ChangeDate); select cast(scope_identity() as int)";
        public const string offerinformationappliedrateadjustmentdatamodel_update = "UPDATE [2am].[dbo].[OfferInformationAppliedRateAdjustment] SET OfferElementValue = @OfferElementValue, RateAdjustmentElementKey = @RateAdjustmentElementKey, ADUserKey = @ADUserKey, OfferInformationFinancialAdjustmentKey = @OfferInformationFinancialAdjustmentKey, ChangeDate = @ChangeDate WHERE OfferInformationAppliedRateAdjustmentKey = @OfferInformationAppliedRateAdjustmentKey";



        public const string externalroledomiciliumdatamodel_selectwhere = "SELECT ExternalRoleDomiciliumKey, LegalEntityDomiciliumKey, ExternalRoleKey, ChangeDate, ADUserKey FROM [2am].[dbo].[ExternalRoleDomicilium] WHERE";
        public const string externalroledomiciliumdatamodel_selectbykey = "SELECT ExternalRoleDomiciliumKey, LegalEntityDomiciliumKey, ExternalRoleKey, ChangeDate, ADUserKey FROM [2am].[dbo].[ExternalRoleDomicilium] WHERE ExternalRoleDomiciliumKey = @PrimaryKey";
        public const string externalroledomiciliumdatamodel_delete = "DELETE FROM [2am].[dbo].[ExternalRoleDomicilium] WHERE ExternalRoleDomiciliumKey = @PrimaryKey";
        public const string externalroledomiciliumdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ExternalRoleDomicilium] WHERE";
        public const string externalroledomiciliumdatamodel_insert = "INSERT INTO [2am].[dbo].[ExternalRoleDomicilium] (LegalEntityDomiciliumKey, ExternalRoleKey, ChangeDate, ADUserKey) VALUES(@LegalEntityDomiciliumKey, @ExternalRoleKey, @ChangeDate, @ADUserKey); select cast(scope_identity() as int)";
        public const string externalroledomiciliumdatamodel_update = "UPDATE [2am].[dbo].[ExternalRoleDomicilium] SET LegalEntityDomiciliumKey = @LegalEntityDomiciliumKey, ExternalRoleKey = @ExternalRoleKey, ChangeDate = @ChangeDate, ADUserKey = @ADUserKey WHERE ExternalRoleDomiciliumKey = @ExternalRoleDomiciliumKey";



        public const string originationsourceproductofferattributetypedatamodel_selectwhere = "SELECT OriginationSourceProductOfferAttributeTypeKey, OriginationSourceProductKey, OfferAttributeTypeKey FROM [2am].[dbo].[OriginationSourceProductOfferAttributeType] WHERE";
        public const string originationsourceproductofferattributetypedatamodel_selectbykey = "SELECT OriginationSourceProductOfferAttributeTypeKey, OriginationSourceProductKey, OfferAttributeTypeKey FROM [2am].[dbo].[OriginationSourceProductOfferAttributeType] WHERE OriginationSourceProductOfferAttributeTypeKey = @PrimaryKey";
        public const string originationsourceproductofferattributetypedatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceProductOfferAttributeType] WHERE OriginationSourceProductOfferAttributeTypeKey = @PrimaryKey";
        public const string originationsourceproductofferattributetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceProductOfferAttributeType] WHERE";
        public const string originationsourceproductofferattributetypedatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceProductOfferAttributeType] (OriginationSourceProductKey, OfferAttributeTypeKey) VALUES(@OriginationSourceProductKey, @OfferAttributeTypeKey); select cast(scope_identity() as int)";
        public const string originationsourceproductofferattributetypedatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceProductOfferAttributeType] SET OriginationSourceProductKey = @OriginationSourceProductKey, OfferAttributeTypeKey = @OfferAttributeTypeKey WHERE OriginationSourceProductOfferAttributeTypeKey = @OriginationSourceProductOfferAttributeTypeKey";



        public const string campaigntargetresponsedatamodel_selectwhere = "SELECT CampaignTargetResponseKey, Description FROM [2am].[dbo].[CampaignTargetResponse] WHERE";
        public const string campaigntargetresponsedatamodel_selectbykey = "SELECT CampaignTargetResponseKey, Description FROM [2am].[dbo].[CampaignTargetResponse] WHERE CampaignTargetResponseKey = @PrimaryKey";
        public const string campaigntargetresponsedatamodel_delete = "DELETE FROM [2am].[dbo].[CampaignTargetResponse] WHERE CampaignTargetResponseKey = @PrimaryKey";
        public const string campaigntargetresponsedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CampaignTargetResponse] WHERE";
        public const string campaigntargetresponsedatamodel_insert = "INSERT INTO [2am].[dbo].[CampaignTargetResponse] (CampaignTargetResponseKey, Description) VALUES(@CampaignTargetResponseKey, @Description); ";
        public const string campaigntargetresponsedatamodel_update = "UPDATE [2am].[dbo].[CampaignTargetResponse] SET CampaignTargetResponseKey = @CampaignTargetResponseKey, Description = @Description WHERE CampaignTargetResponseKey = @CampaignTargetResponseKey";



        public const string mortgageloanpurposegroupdatamodel_selectwhere = "SELECT MortgageLoanPurposeGroupKey, Description FROM [2am].[dbo].[MortgageLoanPurposeGroup] WHERE";
        public const string mortgageloanpurposegroupdatamodel_selectbykey = "SELECT MortgageLoanPurposeGroupKey, Description FROM [2am].[dbo].[MortgageLoanPurposeGroup] WHERE MortgageLoanPurposeGroupKey = @PrimaryKey";
        public const string mortgageloanpurposegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[MortgageLoanPurposeGroup] WHERE MortgageLoanPurposeGroupKey = @PrimaryKey";
        public const string mortgageloanpurposegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MortgageLoanPurposeGroup] WHERE";
        public const string mortgageloanpurposegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[MortgageLoanPurposeGroup] (MortgageLoanPurposeGroupKey, Description) VALUES(@MortgageLoanPurposeGroupKey, @Description); ";
        public const string mortgageloanpurposegroupdatamodel_update = "UPDATE [2am].[dbo].[MortgageLoanPurposeGroup] SET MortgageLoanPurposeGroupKey = @MortgageLoanPurposeGroupKey, Description = @Description WHERE MortgageLoanPurposeGroupKey = @MortgageLoanPurposeGroupKey";



        public const string creditmatrixdatamodel_selectwhere = "SELECT CreditMatrixKey, NewBusinessIndicator, ImplementationDate FROM [2am].[dbo].[CreditMatrix] WHERE";
        public const string creditmatrixdatamodel_selectbykey = "SELECT CreditMatrixKey, NewBusinessIndicator, ImplementationDate FROM [2am].[dbo].[CreditMatrix] WHERE CreditMatrixKey = @PrimaryKey";
        public const string creditmatrixdatamodel_delete = "DELETE FROM [2am].[dbo].[CreditMatrix] WHERE CreditMatrixKey = @PrimaryKey";
        public const string creditmatrixdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CreditMatrix] WHERE";
        public const string creditmatrixdatamodel_insert = "INSERT INTO [2am].[dbo].[CreditMatrix] (NewBusinessIndicator, ImplementationDate) VALUES(@NewBusinessIndicator, @ImplementationDate); select cast(scope_identity() as int)";
        public const string creditmatrixdatamodel_update = "UPDATE [2am].[dbo].[CreditMatrix] SET NewBusinessIndicator = @NewBusinessIndicator, ImplementationDate = @ImplementationDate WHERE CreditMatrixKey = @CreditMatrixKey";



        public const string applicationdocumentdatamodel_selectwhere = "SELECT ApplicationDocumentKey, ApplicationDocumentTypeKey, Received, Required, ADUserKey, ReceivedDate FROM [2am].[dbo].[ApplicationDocument] WHERE";
        public const string applicationdocumentdatamodel_selectbykey = "SELECT ApplicationDocumentKey, ApplicationDocumentTypeKey, Received, Required, ADUserKey, ReceivedDate FROM [2am].[dbo].[ApplicationDocument] WHERE ApplicationDocumentKey = @PrimaryKey";
        public const string applicationdocumentdatamodel_delete = "DELETE FROM [2am].[dbo].[ApplicationDocument] WHERE ApplicationDocumentKey = @PrimaryKey";
        public const string applicationdocumentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ApplicationDocument] WHERE";
        public const string applicationdocumentdatamodel_insert = "INSERT INTO [2am].[dbo].[ApplicationDocument] (ApplicationDocumentTypeKey, Received, Required, ADUserKey, ReceivedDate) VALUES(@ApplicationDocumentTypeKey, @Received, @Required, @ADUserKey, @ReceivedDate); select cast(scope_identity() as int)";
        public const string applicationdocumentdatamodel_update = "UPDATE [2am].[dbo].[ApplicationDocument] SET ApplicationDocumentTypeKey = @ApplicationDocumentTypeKey, Received = @Received, Required = @Required, ADUserKey = @ADUserKey, ReceivedDate = @ReceivedDate WHERE ApplicationDocumentKey = @ApplicationDocumentKey";



        public const string userprofilesettingdatamodel_selectwhere = "SELECT UserProfileSettingKey, ADUserKey, SettingName, SettingValue, SettingType FROM [2am].[dbo].[UserProfileSetting] WHERE";
        public const string userprofilesettingdatamodel_selectbykey = "SELECT UserProfileSettingKey, ADUserKey, SettingName, SettingValue, SettingType FROM [2am].[dbo].[UserProfileSetting] WHERE UserProfileSettingKey = @PrimaryKey";
        public const string userprofilesettingdatamodel_delete = "DELETE FROM [2am].[dbo].[UserProfileSetting] WHERE UserProfileSettingKey = @PrimaryKey";
        public const string userprofilesettingdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[UserProfileSetting] WHERE";
        public const string userprofilesettingdatamodel_insert = "INSERT INTO [2am].[dbo].[UserProfileSetting] (ADUserKey, SettingName, SettingValue, SettingType) VALUES(@ADUserKey, @SettingName, @SettingValue, @SettingType); select cast(scope_identity() as int)";
        public const string userprofilesettingdatamodel_update = "UPDATE [2am].[dbo].[UserProfileSetting] SET ADUserKey = @ADUserKey, SettingName = @SettingName, SettingValue = @SettingValue, SettingType = @SettingType WHERE UserProfileSettingKey = @UserProfileSettingKey";



        public const string originationsourcevaluatordatamodel_selectwhere = "SELECT OriginationSourceValuatorKey, OriginationSourceKey, ValuatorKey FROM [2am].[dbo].[OriginationSourceValuator] WHERE";
        public const string originationsourcevaluatordatamodel_selectbykey = "SELECT OriginationSourceValuatorKey, OriginationSourceKey, ValuatorKey FROM [2am].[dbo].[OriginationSourceValuator] WHERE OriginationSourceValuatorKey = @PrimaryKey";
        public const string originationsourcevaluatordatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceValuator] WHERE OriginationSourceValuatorKey = @PrimaryKey";
        public const string originationsourcevaluatordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceValuator] WHERE";
        public const string originationsourcevaluatordatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceValuator] (OriginationSourceKey, ValuatorKey) VALUES(@OriginationSourceKey, @ValuatorKey); select cast(scope_identity() as int)";
        public const string originationsourcevaluatordatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceValuator] SET OriginationSourceKey = @OriginationSourceKey, ValuatorKey = @ValuatorKey WHERE OriginationSourceValuatorKey = @OriginationSourceValuatorKey";



        public const string resetconfigurationdatamodel_selectwhere = "SELECT ResetConfigurationKey, IntervalType, IntervalDuration, ResetDate, ActionDate, BusinessDayIndicator, Description FROM [2am].[dbo].[ResetConfiguration] WHERE";
        public const string resetconfigurationdatamodel_selectbykey = "SELECT ResetConfigurationKey, IntervalType, IntervalDuration, ResetDate, ActionDate, BusinessDayIndicator, Description FROM [2am].[dbo].[ResetConfiguration] WHERE ResetConfigurationKey = @PrimaryKey";
        public const string resetconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[ResetConfiguration] WHERE ResetConfigurationKey = @PrimaryKey";
        public const string resetconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ResetConfiguration] WHERE";
        public const string resetconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[ResetConfiguration] (IntervalType, IntervalDuration, ResetDate, ActionDate, BusinessDayIndicator, Description) VALUES(@IntervalType, @IntervalDuration, @ResetDate, @ActionDate, @BusinessDayIndicator, @Description); select cast(scope_identity() as int)";
        public const string resetconfigurationdatamodel_update = "UPDATE [2am].[dbo].[ResetConfiguration] SET IntervalType = @IntervalType, IntervalDuration = @IntervalDuration, ResetDate = @ResetDate, ActionDate = @ActionDate, BusinessDayIndicator = @BusinessDayIndicator, Description = @Description WHERE ResetConfigurationKey = @ResetConfigurationKey";



        public const string hochistorydatamodel_selectwhere = "SELECT HOCHistoryKey, FinancialServiceKey, HOCInsurerKey, CommencementDate, CancellationDate, ChangeDate, UserID FROM [2am].[dbo].[HOCHistory] WHERE";
        public const string hochistorydatamodel_selectbykey = "SELECT HOCHistoryKey, FinancialServiceKey, HOCInsurerKey, CommencementDate, CancellationDate, ChangeDate, UserID FROM [2am].[dbo].[HOCHistory] WHERE HOCHistoryKey = @PrimaryKey";
        public const string hochistorydatamodel_delete = "DELETE FROM [2am].[dbo].[HOCHistory] WHERE HOCHistoryKey = @PrimaryKey";
        public const string hochistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCHistory] WHERE";
        public const string hochistorydatamodel_insert = "INSERT INTO [2am].[dbo].[HOCHistory] (FinancialServiceKey, HOCInsurerKey, CommencementDate, CancellationDate, ChangeDate, UserID) VALUES(@FinancialServiceKey, @HOCInsurerKey, @CommencementDate, @CancellationDate, @ChangeDate, @UserID); select cast(scope_identity() as int)";
        public const string hochistorydatamodel_update = "UPDATE [2am].[dbo].[HOCHistory] SET FinancialServiceKey = @FinancialServiceKey, HOCInsurerKey = @HOCInsurerKey, CommencementDate = @CommencementDate, CancellationDate = @CancellationDate, ChangeDate = @ChangeDate, UserID = @UserID WHERE HOCHistoryKey = @HOCHistoryKey";



        public const string campaigndefinitiondatamodel_selectwhere = "SELECT CampaignDefinitionKey, CampaignName, CampaignReference, Startdate, EndDate, MarketingOptionKey, OrganisationStructureKey, GeneralStatusKey, CampaignDefinitionParentKey, ReportStatementKey, ADUserKey, DataProviderDataServiceKey, MarketingOptionRelevanceKey FROM [2am].[dbo].[CampaignDefinition] WHERE";
        public const string campaigndefinitiondatamodel_selectbykey = "SELECT CampaignDefinitionKey, CampaignName, CampaignReference, Startdate, EndDate, MarketingOptionKey, OrganisationStructureKey, GeneralStatusKey, CampaignDefinitionParentKey, ReportStatementKey, ADUserKey, DataProviderDataServiceKey, MarketingOptionRelevanceKey FROM [2am].[dbo].[CampaignDefinition] WHERE CampaignDefinitionKey = @PrimaryKey";
        public const string campaigndefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[CampaignDefinition] WHERE CampaignDefinitionKey = @PrimaryKey";
        public const string campaigndefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CampaignDefinition] WHERE";
        public const string campaigndefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[CampaignDefinition] (CampaignName, CampaignReference, Startdate, EndDate, MarketingOptionKey, OrganisationStructureKey, GeneralStatusKey, CampaignDefinitionParentKey, ReportStatementKey, ADUserKey, DataProviderDataServiceKey, MarketingOptionRelevanceKey) VALUES(@CampaignName, @CampaignReference, @Startdate, @EndDate, @MarketingOptionKey, @OrganisationStructureKey, @GeneralStatusKey, @CampaignDefinitionParentKey, @ReportStatementKey, @ADUserKey, @DataProviderDataServiceKey, @MarketingOptionRelevanceKey); select cast(scope_identity() as int)";
        public const string campaigndefinitiondatamodel_update = "UPDATE [2am].[dbo].[CampaignDefinition] SET CampaignName = @CampaignName, CampaignReference = @CampaignReference, Startdate = @Startdate, EndDate = @EndDate, MarketingOptionKey = @MarketingOptionKey, OrganisationStructureKey = @OrganisationStructureKey, GeneralStatusKey = @GeneralStatusKey, CampaignDefinitionParentKey = @CampaignDefinitionParentKey, ReportStatementKey = @ReportStatementKey, ADUserKey = @ADUserKey, DataProviderDataServiceKey = @DataProviderDataServiceKey, MarketingOptionRelevanceKey = @MarketingOptionRelevanceKey WHERE CampaignDefinitionKey = @CampaignDefinitionKey";



        public const string citizentypedatamodel_selectwhere = "SELECT CitizenTypeKey, Description FROM [2am].[dbo].[CitizenType] WHERE";
        public const string citizentypedatamodel_selectbykey = "SELECT CitizenTypeKey, Description FROM [2am].[dbo].[CitizenType] WHERE CitizenTypeKey = @PrimaryKey";
        public const string citizentypedatamodel_delete = "DELETE FROM [2am].[dbo].[CitizenType] WHERE CitizenTypeKey = @PrimaryKey";
        public const string citizentypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CitizenType] WHERE";
        public const string citizentypedatamodel_insert = "INSERT INTO [2am].[dbo].[CitizenType] (CitizenTypeKey, Description) VALUES(@CitizenTypeKey, @Description); ";
        public const string citizentypedatamodel_update = "UPDATE [2am].[dbo].[CitizenType] SET CitizenTypeKey = @CitizenTypeKey, Description = @Description WHERE CitizenTypeKey = @CitizenTypeKey";



        public const string scorecarddatamodel_selectwhere = "SELECT ScoreCardKey, Description, BasePoints, RevisionDate, GeneralStatusKey FROM [2am].[dbo].[ScoreCard] WHERE";
        public const string scorecarddatamodel_selectbykey = "SELECT ScoreCardKey, Description, BasePoints, RevisionDate, GeneralStatusKey FROM [2am].[dbo].[ScoreCard] WHERE ScoreCardKey = @PrimaryKey";
        public const string scorecarddatamodel_delete = "DELETE FROM [2am].[dbo].[ScoreCard] WHERE ScoreCardKey = @PrimaryKey";
        public const string scorecarddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ScoreCard] WHERE";
        public const string scorecarddatamodel_insert = "INSERT INTO [2am].[dbo].[ScoreCard] (ScoreCardKey, Description, BasePoints, RevisionDate, GeneralStatusKey) VALUES(@ScoreCardKey, @Description, @BasePoints, @RevisionDate, @GeneralStatusKey); ";
        public const string scorecarddatamodel_update = "UPDATE [2am].[dbo].[ScoreCard] SET ScoreCardKey = @ScoreCardKey, Description = @Description, BasePoints = @BasePoints, RevisionDate = @RevisionDate, GeneralStatusKey = @GeneralStatusKey WHERE ScoreCardKey = @ScoreCardKey";



        public const string xsltransformationdatamodel_selectwhere = "SELECT XSLTransformationKey, GenericKeyTypeKey, StyleSheet, Version FROM [2am].[dbo].[XSLTransformation] WHERE";
        public const string xsltransformationdatamodel_selectbykey = "SELECT XSLTransformationKey, GenericKeyTypeKey, StyleSheet, Version FROM [2am].[dbo].[XSLTransformation] WHERE XSLTransformationKey = @PrimaryKey";
        public const string xsltransformationdatamodel_delete = "DELETE FROM [2am].[dbo].[XSLTransformation] WHERE XSLTransformationKey = @PrimaryKey";
        public const string xsltransformationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[XSLTransformation] WHERE";
        public const string xsltransformationdatamodel_insert = "INSERT INTO [2am].[dbo].[XSLTransformation] (GenericKeyTypeKey, StyleSheet, Version) VALUES(@GenericKeyTypeKey, @StyleSheet, @Version); select cast(scope_identity() as int)";
        public const string xsltransformationdatamodel_update = "UPDATE [2am].[dbo].[XSLTransformation] SET GenericKeyTypeKey = @GenericKeyTypeKey, StyleSheet = @StyleSheet, Version = @Version WHERE XSLTransformationKey = @XSLTransformationKey";



        public const string attorneydatamodel_selectwhere = "SELECT AttorneyKey, DeedsOfficeKey, AttorneyContact, AttorneyMandate, AttorneyWorkFlowEnabled, AttorneyLoanTarget, AttorneyFurtherLoanTarget, AttorneyLitigationInd, LegalEntityKey, AttorneyRegistrationInd, GeneralStatusKey FROM [2am].[dbo].[Attorney] WHERE";
        public const string attorneydatamodel_selectbykey = "SELECT AttorneyKey, DeedsOfficeKey, AttorneyContact, AttorneyMandate, AttorneyWorkFlowEnabled, AttorneyLoanTarget, AttorneyFurtherLoanTarget, AttorneyLitigationInd, LegalEntityKey, AttorneyRegistrationInd, GeneralStatusKey FROM [2am].[dbo].[Attorney] WHERE AttorneyKey = @PrimaryKey";
        public const string attorneydatamodel_delete = "DELETE FROM [2am].[dbo].[Attorney] WHERE AttorneyKey = @PrimaryKey";
        public const string attorneydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Attorney] WHERE";
        public const string attorneydatamodel_insert = "INSERT INTO [2am].[dbo].[Attorney] (DeedsOfficeKey, AttorneyContact, AttorneyMandate, AttorneyWorkFlowEnabled, AttorneyLoanTarget, AttorneyFurtherLoanTarget, AttorneyLitigationInd, LegalEntityKey, AttorneyRegistrationInd, GeneralStatusKey) VALUES(@DeedsOfficeKey, @AttorneyContact, @AttorneyMandate, @AttorneyWorkFlowEnabled, @AttorneyLoanTarget, @AttorneyFurtherLoanTarget, @AttorneyLitigationInd, @LegalEntityKey, @AttorneyRegistrationInd, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string attorneydatamodel_update = "UPDATE [2am].[dbo].[Attorney] SET DeedsOfficeKey = @DeedsOfficeKey, AttorneyContact = @AttorneyContact, AttorneyMandate = @AttorneyMandate, AttorneyWorkFlowEnabled = @AttorneyWorkFlowEnabled, AttorneyLoanTarget = @AttorneyLoanTarget, AttorneyFurtherLoanTarget = @AttorneyFurtherLoanTarget, AttorneyLitigationInd = @AttorneyLitigationInd, LegalEntityKey = @LegalEntityKey, AttorneyRegistrationInd = @AttorneyRegistrationInd, GeneralStatusKey = @GeneralStatusKey WHERE AttorneyKey = @AttorneyKey";



        public const string auditlegalentitydatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, LegalEntityTypeKey, MaritalStatusKey, GenderKey, PopulationGroupKey, IntroductionDate, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, RegistrationNumber, RegisteredName, TradingName, DateOfBirth, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, Password, SalutationKey, CitizenTypeKey, LegalEntityStatusKey, Comments, LegalEntityExceptionStatusKey, UserID, ChangeDate, EducationKey, HomeLanguageKey, DocumentLanguageKey, ResidenceStatusKey FROM [2am].[dbo].[AuditLegalEntity] WHERE";
        public const string auditlegalentitydatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, LegalEntityTypeKey, MaritalStatusKey, GenderKey, PopulationGroupKey, IntroductionDate, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, RegistrationNumber, RegisteredName, TradingName, DateOfBirth, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, Password, SalutationKey, CitizenTypeKey, LegalEntityStatusKey, Comments, LegalEntityExceptionStatusKey, UserID, ChangeDate, EducationKey, HomeLanguageKey, DocumentLanguageKey, ResidenceStatusKey FROM [2am].[dbo].[AuditLegalEntity] WHERE AuditNumber = @PrimaryKey";
        public const string auditlegalentitydatamodel_delete = "DELETE FROM [2am].[dbo].[AuditLegalEntity] WHERE AuditNumber = @PrimaryKey";
        public const string auditlegalentitydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditLegalEntity] WHERE";
        public const string auditlegalentitydatamodel_insert = "INSERT INTO [2am].[dbo].[AuditLegalEntity] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, LegalEntityTypeKey, MaritalStatusKey, GenderKey, PopulationGroupKey, IntroductionDate, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, RegistrationNumber, RegisteredName, TradingName, DateOfBirth, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, Password, SalutationKey, CitizenTypeKey, LegalEntityStatusKey, Comments, LegalEntityExceptionStatusKey, UserID, ChangeDate, EducationKey, HomeLanguageKey, DocumentLanguageKey, ResidenceStatusKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @LegalEntityKey, @LegalEntityTypeKey, @MaritalStatusKey, @GenderKey, @PopulationGroupKey, @IntroductionDate, @FirstNames, @Initials, @Surname, @PreferredName, @IDNumber, @PassportNumber, @TaxNumber, @RegistrationNumber, @RegisteredName, @TradingName, @DateOfBirth, @HomePhoneCode, @HomePhoneNumber, @WorkPhoneCode, @WorkPhoneNumber, @CellPhoneNumber, @EmailAddress, @FaxCode, @FaxNumber, @Password, @SalutationKey, @CitizenTypeKey, @LegalEntityStatusKey, @Comments, @LegalEntityExceptionStatusKey, @UserID, @ChangeDate, @EducationKey, @HomeLanguageKey, @DocumentLanguageKey, @ResidenceStatusKey); select cast(scope_identity() as int)";
        public const string auditlegalentitydatamodel_update = "UPDATE [2am].[dbo].[AuditLegalEntity] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, LegalEntityKey = @LegalEntityKey, LegalEntityTypeKey = @LegalEntityTypeKey, MaritalStatusKey = @MaritalStatusKey, GenderKey = @GenderKey, PopulationGroupKey = @PopulationGroupKey, IntroductionDate = @IntroductionDate, FirstNames = @FirstNames, Initials = @Initials, Surname = @Surname, PreferredName = @PreferredName, IDNumber = @IDNumber, PassportNumber = @PassportNumber, TaxNumber = @TaxNumber, RegistrationNumber = @RegistrationNumber, RegisteredName = @RegisteredName, TradingName = @TradingName, DateOfBirth = @DateOfBirth, HomePhoneCode = @HomePhoneCode, HomePhoneNumber = @HomePhoneNumber, WorkPhoneCode = @WorkPhoneCode, WorkPhoneNumber = @WorkPhoneNumber, CellPhoneNumber = @CellPhoneNumber, EmailAddress = @EmailAddress, FaxCode = @FaxCode, FaxNumber = @FaxNumber, Password = @Password, SalutationKey = @SalutationKey, CitizenTypeKey = @CitizenTypeKey, LegalEntityStatusKey = @LegalEntityStatusKey, Comments = @Comments, LegalEntityExceptionStatusKey = @LegalEntityExceptionStatusKey, UserID = @UserID, ChangeDate = @ChangeDate, EducationKey = @EducationKey, HomeLanguageKey = @HomeLanguageKey, DocumentLanguageKey = @DocumentLanguageKey, ResidenceStatusKey = @ResidenceStatusKey WHERE AuditNumber = @AuditNumber";



        public const string auditbondmortgageloandatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BondMortgageLoanKey, FinancialServiceKey, BondKey FROM [2am].[dbo].[AuditBondMortgageLoan] WHERE";
        public const string auditbondmortgageloandatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BondMortgageLoanKey, FinancialServiceKey, BondKey FROM [2am].[dbo].[AuditBondMortgageLoan] WHERE AuditNumber = @PrimaryKey";
        public const string auditbondmortgageloandatamodel_delete = "DELETE FROM [2am].[dbo].[AuditBondMortgageLoan] WHERE AuditNumber = @PrimaryKey";
        public const string auditbondmortgageloandatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditBondMortgageLoan] WHERE";
        public const string auditbondmortgageloandatamodel_insert = "INSERT INTO [2am].[dbo].[AuditBondMortgageLoan] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, BondMortgageLoanKey, FinancialServiceKey, BondKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @BondMortgageLoanKey, @FinancialServiceKey, @BondKey); select cast(scope_identity() as int)";
        public const string auditbondmortgageloandatamodel_update = "UPDATE [2am].[dbo].[AuditBondMortgageLoan] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, BondMortgageLoanKey = @BondMortgageLoanKey, FinancialServiceKey = @FinancialServiceKey, BondKey = @BondKey WHERE AuditNumber = @AuditNumber";



        public const string occupancytypedatamodel_selectwhere = "SELECT OccupancyTypeKey, Description FROM [2am].[dbo].[OccupancyType] WHERE";
        public const string occupancytypedatamodel_selectbykey = "SELECT OccupancyTypeKey, Description FROM [2am].[dbo].[OccupancyType] WHERE OccupancyTypeKey = @PrimaryKey";
        public const string occupancytypedatamodel_delete = "DELETE FROM [2am].[dbo].[OccupancyType] WHERE OccupancyTypeKey = @PrimaryKey";
        public const string occupancytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OccupancyType] WHERE";
        public const string occupancytypedatamodel_insert = "INSERT INTO [2am].[dbo].[OccupancyType] (OccupancyTypeKey, Description) VALUES(@OccupancyTypeKey, @Description); ";
        public const string occupancytypedatamodel_update = "UPDATE [2am].[dbo].[OccupancyType] SET OccupancyTypeKey = @OccupancyTypeKey, Description = @Description WHERE OccupancyTypeKey = @OccupancyTypeKey";



        public const string allocationmandatedatamodel_selectwhere = "SELECT AllocationMandateKey, Name, Description, TypeName, ParameterTypeKey, ParameterValue FROM [2am].[dbo].[AllocationMandate] WHERE";
        public const string allocationmandatedatamodel_selectbykey = "SELECT AllocationMandateKey, Name, Description, TypeName, ParameterTypeKey, ParameterValue FROM [2am].[dbo].[AllocationMandate] WHERE AllocationMandateKey = @PrimaryKey";
        public const string allocationmandatedatamodel_delete = "DELETE FROM [2am].[dbo].[AllocationMandate] WHERE AllocationMandateKey = @PrimaryKey";
        public const string allocationmandatedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AllocationMandate] WHERE";
        public const string allocationmandatedatamodel_insert = "INSERT INTO [2am].[dbo].[AllocationMandate] (Name, Description, TypeName, ParameterTypeKey, ParameterValue) VALUES(@Name, @Description, @TypeName, @ParameterTypeKey, @ParameterValue); select cast(scope_identity() as int)";
        public const string allocationmandatedatamodel_update = "UPDATE [2am].[dbo].[AllocationMandate] SET Name = @Name, Description = @Description, TypeName = @TypeName, ParameterTypeKey = @ParameterTypeKey, ParameterValue = @ParameterValue WHERE AllocationMandateKey = @AllocationMandateKey";



        public const string employerbusinesstypedatamodel_selectwhere = "SELECT EmployerBusinessTypeKey, Description FROM [2am].[dbo].[EmployerBusinessType] WHERE";
        public const string employerbusinesstypedatamodel_selectbykey = "SELECT EmployerBusinessTypeKey, Description FROM [2am].[dbo].[EmployerBusinessType] WHERE EmployerBusinessTypeKey = @PrimaryKey";
        public const string employerbusinesstypedatamodel_delete = "DELETE FROM [2am].[dbo].[EmployerBusinessType] WHERE EmployerBusinessTypeKey = @PrimaryKey";
        public const string employerbusinesstypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[EmployerBusinessType] WHERE";
        public const string employerbusinesstypedatamodel_insert = "INSERT INTO [2am].[dbo].[EmployerBusinessType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string employerbusinesstypedatamodel_update = "UPDATE [2am].[dbo].[EmployerBusinessType] SET Description = @Description WHERE EmployerBusinessTypeKey = @EmployerBusinessTypeKey";



        public const string offercorrespondencedatamodel_selectwhere = "SELECT OfferCorrespondenceKey, OfferKey, CorrespondenceKey FROM [2am].[dbo].[OfferCorrespondence] WHERE";
        public const string offercorrespondencedatamodel_selectbykey = "SELECT OfferCorrespondenceKey, OfferKey, CorrespondenceKey FROM [2am].[dbo].[OfferCorrespondence] WHERE OfferCorrespondenceKey = @PrimaryKey";
        public const string offercorrespondencedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferCorrespondence] WHERE OfferCorrespondenceKey = @PrimaryKey";
        public const string offercorrespondencedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferCorrespondence] WHERE";
        public const string offercorrespondencedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferCorrespondence] (OfferKey, CorrespondenceKey) VALUES(@OfferKey, @CorrespondenceKey); select cast(scope_identity() as int)";
        public const string offercorrespondencedatamodel_update = "UPDATE [2am].[dbo].[OfferCorrespondence] SET OfferKey = @OfferKey, CorrespondenceKey = @CorrespondenceKey WHERE OfferCorrespondenceKey = @OfferCorrespondenceKey";



        public const string offerinformationquickcashdatamodel_selectwhere = "SELECT OfferInformationKey, CreditApprovedAmount, Term, CreditUpfrontApprovedAmount FROM [2am].[dbo].[OfferInformationQuickCash] WHERE";
        public const string offerinformationquickcashdatamodel_selectbykey = "SELECT OfferInformationKey, CreditApprovedAmount, Term, CreditUpfrontApprovedAmount FROM [2am].[dbo].[OfferInformationQuickCash] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationquickcashdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationQuickCash] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationquickcashdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationQuickCash] WHERE";
        public const string offerinformationquickcashdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationQuickCash] (OfferInformationKey, CreditApprovedAmount, Term, CreditUpfrontApprovedAmount) VALUES(@OfferInformationKey, @CreditApprovedAmount, @Term, @CreditUpfrontApprovedAmount); ";
        public const string offerinformationquickcashdatamodel_update = "UPDATE [2am].[dbo].[OfferInformationQuickCash] SET OfferInformationKey = @OfferInformationKey, CreditApprovedAmount = @CreditApprovedAmount, Term = @Term, CreditUpfrontApprovedAmount = @CreditUpfrontApprovedAmount WHERE OfferInformationKey = @OfferInformationKey";



        public const string capofferdetaildatamodel_selectwhere = "SELECT CapOfferDetailKey, CapOfferKey, CapTypeConfigurationDetailKey, EffectiveRate, Payment, Fee, CapStatusKey, AcceptanceDate, CapNTUReasonKey, CapNTUReasonDate, ChangeDate, UserID FROM [2am].[dbo].[CapOfferDetail] WHERE";
        public const string capofferdetaildatamodel_selectbykey = "SELECT CapOfferDetailKey, CapOfferKey, CapTypeConfigurationDetailKey, EffectiveRate, Payment, Fee, CapStatusKey, AcceptanceDate, CapNTUReasonKey, CapNTUReasonDate, ChangeDate, UserID FROM [2am].[dbo].[CapOfferDetail] WHERE CapOfferDetailKey = @PrimaryKey";
        public const string capofferdetaildatamodel_delete = "DELETE FROM [2am].[dbo].[CapOfferDetail] WHERE CapOfferDetailKey = @PrimaryKey";
        public const string capofferdetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CapOfferDetail] WHERE";
        public const string capofferdetaildatamodel_insert = "INSERT INTO [2am].[dbo].[CapOfferDetail] (CapOfferKey, CapTypeConfigurationDetailKey, EffectiveRate, Payment, Fee, CapStatusKey, AcceptanceDate, CapNTUReasonKey, CapNTUReasonDate, ChangeDate, UserID) VALUES(@CapOfferKey, @CapTypeConfigurationDetailKey, @EffectiveRate, @Payment, @Fee, @CapStatusKey, @AcceptanceDate, @CapNTUReasonKey, @CapNTUReasonDate, @ChangeDate, @UserID); select cast(scope_identity() as int)";
        public const string capofferdetaildatamodel_update = "UPDATE [2am].[dbo].[CapOfferDetail] SET CapOfferKey = @CapOfferKey, CapTypeConfigurationDetailKey = @CapTypeConfigurationDetailKey, EffectiveRate = @EffectiveRate, Payment = @Payment, Fee = @Fee, CapStatusKey = @CapStatusKey, AcceptanceDate = @AcceptanceDate, CapNTUReasonKey = @CapNTUReasonKey, CapNTUReasonDate = @CapNTUReasonDate, ChangeDate = @ChangeDate, UserID = @UserID WHERE CapOfferDetailKey = @CapOfferDetailKey";



        public const string workflowrulesetdatamodel_selectwhere = "SELECT WorkflowRuleSetKey, Name FROM [2am].[dbo].[WorkflowRuleSet] WHERE";
        public const string workflowrulesetdatamodel_selectbykey = "SELECT WorkflowRuleSetKey, Name FROM [2am].[dbo].[WorkflowRuleSet] WHERE WorkflowRuleSetKey = @PrimaryKey";
        public const string workflowrulesetdatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowRuleSet] WHERE WorkflowRuleSetKey = @PrimaryKey";
        public const string workflowrulesetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowRuleSet] WHERE";
        public const string workflowrulesetdatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowRuleSet] (Name) VALUES(@Name); select cast(scope_identity() as int)";
        public const string workflowrulesetdatamodel_update = "UPDATE [2am].[dbo].[WorkflowRuleSet] SET Name = @Name WHERE WorkflowRuleSetKey = @WorkflowRuleSetKey";



        public const string hocdatamodel_selectwhere = "SELECT FinancialServiceKey, HOCInsurerKey, HOCPolicyNumber, HOCProrataPremium, HOCMonthlyPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, HOCSubsidenceKey, HOCConstructionKey, HOCRoofKey, HOCStatusID, HOCSBICFlag, CapitalizedMonthlyBalance, CommencementDate, AnniversaryDate, UserID, ChangeDate, HOCStatusKey, Ceded, SAHLPolicyNumber, CancellationDate, HOCHistoryKey, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey, HOCBaseConventional, HOCBaseThatch, HOCBaseShingle FROM [2am].[dbo].[HOC] WHERE";
        public const string hocdatamodel_selectbykey = "SELECT FinancialServiceKey, HOCInsurerKey, HOCPolicyNumber, HOCProrataPremium, HOCMonthlyPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, HOCSubsidenceKey, HOCConstructionKey, HOCRoofKey, HOCStatusID, HOCSBICFlag, CapitalizedMonthlyBalance, CommencementDate, AnniversaryDate, UserID, ChangeDate, HOCStatusKey, Ceded, SAHLPolicyNumber, CancellationDate, HOCHistoryKey, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey, HOCBaseConventional, HOCBaseThatch, HOCBaseShingle FROM [2am].[dbo].[HOC] WHERE FinancialServiceKey = @PrimaryKey";
        public const string hocdatamodel_delete = "DELETE FROM [2am].[dbo].[HOC] WHERE FinancialServiceKey = @PrimaryKey";
        public const string hocdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOC] WHERE";
        public const string hocdatamodel_insert = "INSERT INTO [2am].[dbo].[HOC] (FinancialServiceKey, HOCInsurerKey, HOCPolicyNumber, HOCProrataPremium, HOCMonthlyPremium, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCTotalSumInsured, HOCSubsidenceKey, HOCConstructionKey, HOCRoofKey, HOCStatusID, HOCSBICFlag, CapitalizedMonthlyBalance, CommencementDate, AnniversaryDate, UserID, ChangeDate, HOCStatusKey, Ceded, SAHLPolicyNumber, CancellationDate, HOCHistoryKey, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey, HOCBaseConventional, HOCBaseThatch, HOCBaseShingle) VALUES(@FinancialServiceKey, @HOCInsurerKey, @HOCPolicyNumber, @HOCProrataPremium, @HOCMonthlyPremium, @HOCThatchAmount, @HOCConventionalAmount, @HOCShingleAmount, @HOCTotalSumInsured, @HOCSubsidenceKey, @HOCConstructionKey, @HOCRoofKey, @HOCStatusID, @HOCSBICFlag, @CapitalizedMonthlyBalance, @CommencementDate, @AnniversaryDate, @UserID, @ChangeDate, @HOCStatusKey, @Ceded, @SAHLPolicyNumber, @CancellationDate, @HOCHistoryKey, @HOCAdministrationFee, @HOCBasePremium, @SASRIAAmount, @HOCRatesKey, @HOCBaseConventional, @HOCBaseThatch, @HOCBaseShingle); ";
        public const string hocdatamodel_update = "UPDATE [2am].[dbo].[HOC] SET FinancialServiceKey = @FinancialServiceKey, HOCInsurerKey = @HOCInsurerKey, HOCPolicyNumber = @HOCPolicyNumber, HOCProrataPremium = @HOCProrataPremium, HOCMonthlyPremium = @HOCMonthlyPremium, HOCThatchAmount = @HOCThatchAmount, HOCConventionalAmount = @HOCConventionalAmount, HOCShingleAmount = @HOCShingleAmount, HOCTotalSumInsured = @HOCTotalSumInsured, HOCSubsidenceKey = @HOCSubsidenceKey, HOCConstructionKey = @HOCConstructionKey, HOCRoofKey = @HOCRoofKey, HOCStatusID = @HOCStatusID, HOCSBICFlag = @HOCSBICFlag, CapitalizedMonthlyBalance = @CapitalizedMonthlyBalance, CommencementDate = @CommencementDate, AnniversaryDate = @AnniversaryDate, UserID = @UserID, ChangeDate = @ChangeDate, HOCStatusKey = @HOCStatusKey, Ceded = @Ceded, SAHLPolicyNumber = @SAHLPolicyNumber, CancellationDate = @CancellationDate, HOCHistoryKey = @HOCHistoryKey, HOCAdministrationFee = @HOCAdministrationFee, HOCBasePremium = @HOCBasePremium, SASRIAAmount = @SASRIAAmount, HOCRatesKey = @HOCRatesKey, HOCBaseConventional = @HOCBaseConventional, HOCBaseThatch = @HOCBaseThatch, HOCBaseShingle = @HOCBaseShingle WHERE FinancialServiceKey = @FinancialServiceKey";



        public const string marketratedatamodel_selectwhere = "SELECT MarketRateKey, Value, Description FROM [2am].[dbo].[MarketRate] WHERE";
        public const string marketratedatamodel_selectbykey = "SELECT MarketRateKey, Value, Description FROM [2am].[dbo].[MarketRate] WHERE MarketRateKey = @PrimaryKey";
        public const string marketratedatamodel_delete = "DELETE FROM [2am].[dbo].[MarketRate] WHERE MarketRateKey = @PrimaryKey";
        public const string marketratedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MarketRate] WHERE";
        public const string marketratedatamodel_insert = "INSERT INTO [2am].[dbo].[MarketRate] (MarketRateKey, Value, Description) VALUES(@MarketRateKey, @Value, @Description); ";
        public const string marketratedatamodel_update = "UPDATE [2am].[dbo].[MarketRate] SET MarketRateKey = @MarketRateKey, Value = @Value, Description = @Description WHERE MarketRateKey = @MarketRateKey";



        public const string conditionsetdatamodel_selectwhere = "SELECT ConditionSetKey, Description FROM [2am].[dbo].[ConditionSet] WHERE";
        public const string conditionsetdatamodel_selectbykey = "SELECT ConditionSetKey, Description FROM [2am].[dbo].[ConditionSet] WHERE ConditionSetKey = @PrimaryKey";
        public const string conditionsetdatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionSet] WHERE ConditionSetKey = @PrimaryKey";
        public const string conditionsetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionSet] WHERE";
        public const string conditionsetdatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionSet] (ConditionSetKey, Description) VALUES(@ConditionSetKey, @Description); ";
        public const string conditionsetdatamodel_update = "UPDATE [2am].[dbo].[ConditionSet] SET ConditionSetKey = @ConditionSetKey, Description = @Description WHERE ConditionSetKey = @ConditionSetKey";



        public const string scorecardattributedatamodel_selectwhere = "SELECT ScoreCardAttributeKey, ScoreCardKey, Code, Description FROM [2am].[dbo].[ScoreCardAttribute] WHERE";
        public const string scorecardattributedatamodel_selectbykey = "SELECT ScoreCardAttributeKey, ScoreCardKey, Code, Description FROM [2am].[dbo].[ScoreCardAttribute] WHERE ScoreCardAttributeKey = @PrimaryKey";
        public const string scorecardattributedatamodel_delete = "DELETE FROM [2am].[dbo].[ScoreCardAttribute] WHERE ScoreCardAttributeKey = @PrimaryKey";
        public const string scorecardattributedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ScoreCardAttribute] WHERE";
        public const string scorecardattributedatamodel_insert = "INSERT INTO [2am].[dbo].[ScoreCardAttribute] (ScoreCardAttributeKey, ScoreCardKey, Code, Description) VALUES(@ScoreCardAttributeKey, @ScoreCardKey, @Code, @Description); ";
        public const string scorecardattributedatamodel_update = "UPDATE [2am].[dbo].[ScoreCardAttribute] SET ScoreCardAttributeKey = @ScoreCardAttributeKey, ScoreCardKey = @ScoreCardKey, Code = @Code, Description = @Description WHERE ScoreCardAttributeKey = @ScoreCardAttributeKey";



        public const string legalentitydatamodel_selectwhere = "SELECT LegalEntityKey, LegalEntityTypeKey, MaritalStatusKey, GenderKey, PopulationGroupKey, IntroductionDate, Salutationkey, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, RegistrationNumber, RegisteredName, TradingName, DateOfBirth, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, Password, CitizenTypeKey, LegalEntityStatusKey, Comments, LegalEntityExceptionStatusKey, UserID, ChangeDate, EducationKey, HomeLanguageKey, DocumentLanguageKey, ResidenceStatusKey FROM [2am].[dbo].[LegalEntity] WHERE";
        public const string legalentitydatamodel_selectbykey = "SELECT LegalEntityKey, LegalEntityTypeKey, MaritalStatusKey, GenderKey, PopulationGroupKey, IntroductionDate, Salutationkey, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, RegistrationNumber, RegisteredName, TradingName, DateOfBirth, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, Password, CitizenTypeKey, LegalEntityStatusKey, Comments, LegalEntityExceptionStatusKey, UserID, ChangeDate, EducationKey, HomeLanguageKey, DocumentLanguageKey, ResidenceStatusKey FROM [2am].[dbo].[LegalEntity] WHERE LegalEntityKey = @PrimaryKey";
        public const string legalentitydatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntity] WHERE LegalEntityKey = @PrimaryKey";
        public const string legalentitydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntity] WHERE";
        public const string legalentitydatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntity] (LegalEntityTypeKey, MaritalStatusKey, GenderKey, PopulationGroupKey, IntroductionDate, Salutationkey, FirstNames, Initials, Surname, PreferredName, IDNumber, PassportNumber, TaxNumber, RegistrationNumber, RegisteredName, TradingName, DateOfBirth, HomePhoneCode, HomePhoneNumber, WorkPhoneCode, WorkPhoneNumber, CellPhoneNumber, EmailAddress, FaxCode, FaxNumber, Password, CitizenTypeKey, LegalEntityStatusKey, Comments, LegalEntityExceptionStatusKey, UserID, ChangeDate, EducationKey, HomeLanguageKey, DocumentLanguageKey, ResidenceStatusKey) VALUES(@LegalEntityTypeKey, @MaritalStatusKey, @GenderKey, @PopulationGroupKey, @IntroductionDate, @Salutationkey, @FirstNames, @Initials, @Surname, @PreferredName, @IDNumber, @PassportNumber, @TaxNumber, @RegistrationNumber, @RegisteredName, @TradingName, @DateOfBirth, @HomePhoneCode, @HomePhoneNumber, @WorkPhoneCode, @WorkPhoneNumber, @CellPhoneNumber, @EmailAddress, @FaxCode, @FaxNumber, @Password, @CitizenTypeKey, @LegalEntityStatusKey, @Comments, @LegalEntityExceptionStatusKey, @UserID, @ChangeDate, @EducationKey, @HomeLanguageKey, @DocumentLanguageKey, @ResidenceStatusKey); select cast(scope_identity() as int)";
        public const string legalentitydatamodel_update = "UPDATE [2am].[dbo].[LegalEntity] SET LegalEntityTypeKey = @LegalEntityTypeKey, MaritalStatusKey = @MaritalStatusKey, GenderKey = @GenderKey, PopulationGroupKey = @PopulationGroupKey, IntroductionDate = @IntroductionDate, Salutationkey = @Salutationkey, FirstNames = @FirstNames, Initials = @Initials, Surname = @Surname, PreferredName = @PreferredName, IDNumber = @IDNumber, PassportNumber = @PassportNumber, TaxNumber = @TaxNumber, RegistrationNumber = @RegistrationNumber, RegisteredName = @RegisteredName, TradingName = @TradingName, DateOfBirth = @DateOfBirth, HomePhoneCode = @HomePhoneCode, HomePhoneNumber = @HomePhoneNumber, WorkPhoneCode = @WorkPhoneCode, WorkPhoneNumber = @WorkPhoneNumber, CellPhoneNumber = @CellPhoneNumber, EmailAddress = @EmailAddress, FaxCode = @FaxCode, FaxNumber = @FaxNumber, Password = @Password, CitizenTypeKey = @CitizenTypeKey, LegalEntityStatusKey = @LegalEntityStatusKey, Comments = @Comments, LegalEntityExceptionStatusKey = @LegalEntityExceptionStatusKey, UserID = @UserID, ChangeDate = @ChangeDate, EducationKey = @EducationKey, HomeLanguageKey = @HomeLanguageKey, DocumentLanguageKey = @DocumentLanguageKey, ResidenceStatusKey = @ResidenceStatusKey WHERE LegalEntityKey = @LegalEntityKey";



        public const string legalentitydomiciliumdatamodel_selectwhere = "SELECT LegalEntityDomiciliumKey, LegalEntityAddressKey, GeneralStatusKey, ChangeDate, ADUserKey FROM [2am].[dbo].[LegalEntityDomicilium] WHERE";
        public const string legalentitydomiciliumdatamodel_selectbykey = "SELECT LegalEntityDomiciliumKey, LegalEntityAddressKey, GeneralStatusKey, ChangeDate, ADUserKey FROM [2am].[dbo].[LegalEntityDomicilium] WHERE LegalEntityDomiciliumKey = @PrimaryKey";
        public const string legalentitydomiciliumdatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityDomicilium] WHERE LegalEntityDomiciliumKey = @PrimaryKey";
        public const string legalentitydomiciliumdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityDomicilium] WHERE";
        public const string legalentitydomiciliumdatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityDomicilium] (LegalEntityAddressKey, GeneralStatusKey, ChangeDate, ADUserKey) VALUES(@LegalEntityAddressKey, @GeneralStatusKey, @ChangeDate, @ADUserKey); select cast(scope_identity() as int)";
        public const string legalentitydomiciliumdatamodel_update = "UPDATE [2am].[dbo].[LegalEntityDomicilium] SET LegalEntityAddressKey = @LegalEntityAddressKey, GeneralStatusKey = @GeneralStatusKey, ChangeDate = @ChangeDate, ADUserKey = @ADUserKey WHERE LegalEntityDomiciliumKey = @LegalEntityDomiciliumKey";



        public const string uistatementdatamodel_selectwhere = "SELECT StatementKey, ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate FROM [2am].[dbo].[uiStatement] WHERE";
        public const string uistatementdatamodel_selectbykey = "SELECT StatementKey, ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate FROM [2am].[dbo].[uiStatement] WHERE StatementKey = @PrimaryKey";
        public const string uistatementdatamodel_delete = "DELETE FROM [2am].[dbo].[uiStatement] WHERE StatementKey = @PrimaryKey";
        public const string uistatementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[uiStatement] WHERE";
        public const string uistatementdatamodel_insert = "INSERT INTO [2am].[dbo].[uiStatement] (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate) VALUES(@ApplicationName, @StatementName, @ModifyDate, @Version, @ModifyUser, @Statement, @Type, @LastAccessedDate); select cast(scope_identity() as int)";
        public const string uistatementdatamodel_update = "UPDATE [2am].[dbo].[uiStatement] SET ApplicationName = @ApplicationName, StatementName = @StatementName, ModifyDate = @ModifyDate, Version = @Version, ModifyUser = @ModifyUser, Statement = @Statement, Type = @Type, LastAccessedDate = @LastAccessedDate WHERE StatementKey = @StatementKey";



        public const string offerinformationquickcashdetaildatamodel_selectwhere = "SELECT OfferInformationQuickCashDetailKey, OfferInformationKey, InterestRate, RequestedAmount, RateConfigurationKey, Disbursed, QuickCashPaymentTypeKey FROM [2am].[dbo].[OfferInformationQuickCashDetail] WHERE";
        public const string offerinformationquickcashdetaildatamodel_selectbykey = "SELECT OfferInformationQuickCashDetailKey, OfferInformationKey, InterestRate, RequestedAmount, RateConfigurationKey, Disbursed, QuickCashPaymentTypeKey FROM [2am].[dbo].[OfferInformationQuickCashDetail] WHERE OfferInformationQuickCashDetailKey = @PrimaryKey";
        public const string offerinformationquickcashdetaildatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationQuickCashDetail] WHERE OfferInformationQuickCashDetailKey = @PrimaryKey";
        public const string offerinformationquickcashdetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationQuickCashDetail] WHERE";
        public const string offerinformationquickcashdetaildatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationQuickCashDetail] (OfferInformationKey, InterestRate, RequestedAmount, RateConfigurationKey, Disbursed, QuickCashPaymentTypeKey) VALUES(@OfferInformationKey, @InterestRate, @RequestedAmount, @RateConfigurationKey, @Disbursed, @QuickCashPaymentTypeKey); select cast(scope_identity() as int)";
        public const string offerinformationquickcashdetaildatamodel_update = "UPDATE [2am].[dbo].[OfferInformationQuickCashDetail] SET OfferInformationKey = @OfferInformationKey, InterestRate = @InterestRate, RequestedAmount = @RequestedAmount, RateConfigurationKey = @RateConfigurationKey, Disbursed = @Disbursed, QuickCashPaymentTypeKey = @QuickCashPaymentTypeKey WHERE OfferInformationQuickCashDetailKey = @OfferInformationQuickCashDetailKey";



        public const string offerattributetypedatamodel_selectwhere = "SELECT OfferAttributeTypeKey, Description, ISGeneric, OfferAttributeTypeGroupKey, UserEditable FROM [2am].[dbo].[OfferAttributeType] WHERE";
        public const string offerattributetypedatamodel_selectbykey = "SELECT OfferAttributeTypeKey, Description, ISGeneric, OfferAttributeTypeGroupKey, UserEditable FROM [2am].[dbo].[OfferAttributeType] WHERE OfferAttributeTypeKey = @PrimaryKey";
        public const string offerattributetypedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferAttributeType] WHERE OfferAttributeTypeKey = @PrimaryKey";
        public const string offerattributetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferAttributeType] WHERE";
        public const string offerattributetypedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferAttributeType] (OfferAttributeTypeKey, Description, ISGeneric, OfferAttributeTypeGroupKey, UserEditable) VALUES(@OfferAttributeTypeKey, @Description, @ISGeneric, @OfferAttributeTypeGroupKey, @UserEditable); ";
        public const string offerattributetypedatamodel_update = "UPDATE [2am].[dbo].[OfferAttributeType] SET OfferAttributeTypeKey = @OfferAttributeTypeKey, Description = @Description, ISGeneric = @ISGeneric, OfferAttributeTypeGroupKey = @OfferAttributeTypeGroupKey, UserEditable = @UserEditable WHERE OfferAttributeTypeKey = @OfferAttributeTypeKey";



        public const string hochistorydetaildatamodel_selectwhere = "SELECT HOCHistoryDetailKey, HOCHistoryKey, EffectiveDate, UpdateType, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCProrataPremium, HOCMonthlyPremium, PrintDate, SendFileDate, ChangeDate, UserID, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey FROM [2am].[dbo].[HOCHistoryDetail] WHERE";
        public const string hochistorydetaildatamodel_selectbykey = "SELECT HOCHistoryDetailKey, HOCHistoryKey, EffectiveDate, UpdateType, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCProrataPremium, HOCMonthlyPremium, PrintDate, SendFileDate, ChangeDate, UserID, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey FROM [2am].[dbo].[HOCHistoryDetail] WHERE HOCHistoryDetailKey = @PrimaryKey";
        public const string hochistorydetaildatamodel_delete = "DELETE FROM [2am].[dbo].[HOCHistoryDetail] WHERE HOCHistoryDetailKey = @PrimaryKey";
        public const string hochistorydetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCHistoryDetail] WHERE";
        public const string hochistorydetaildatamodel_insert = "INSERT INTO [2am].[dbo].[HOCHistoryDetail] (HOCHistoryKey, EffectiveDate, UpdateType, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, HOCProrataPremium, HOCMonthlyPremium, PrintDate, SendFileDate, ChangeDate, UserID, HOCAdministrationFee, HOCBasePremium, SASRIAAmount, HOCRatesKey) VALUES(@HOCHistoryKey, @EffectiveDate, @UpdateType, @HOCThatchAmount, @HOCConventionalAmount, @HOCShingleAmount, @HOCProrataPremium, @HOCMonthlyPremium, @PrintDate, @SendFileDate, @ChangeDate, @UserID, @HOCAdministrationFee, @HOCBasePremium, @SASRIAAmount, @HOCRatesKey); select cast(scope_identity() as int)";
        public const string hochistorydetaildatamodel_update = "UPDATE [2am].[dbo].[HOCHistoryDetail] SET HOCHistoryKey = @HOCHistoryKey, EffectiveDate = @EffectiveDate, UpdateType = @UpdateType, HOCThatchAmount = @HOCThatchAmount, HOCConventionalAmount = @HOCConventionalAmount, HOCShingleAmount = @HOCShingleAmount, HOCProrataPremium = @HOCProrataPremium, HOCMonthlyPremium = @HOCMonthlyPremium, PrintDate = @PrintDate, SendFileDate = @SendFileDate, ChangeDate = @ChangeDate, UserID = @UserID, HOCAdministrationFee = @HOCAdministrationFee, HOCBasePremium = @HOCBasePremium, SASRIAAmount = @SASRIAAmount, HOCRatesKey = @HOCRatesKey WHERE HOCHistoryDetailKey = @HOCHistoryDetailKey";



        public const string dataproviderdatamodel_selectwhere = "SELECT DataProviderKey, Description FROM [2am].[dbo].[DataProvider] WHERE";
        public const string dataproviderdatamodel_selectbykey = "SELECT DataProviderKey, Description FROM [2am].[dbo].[DataProvider] WHERE DataProviderKey = @PrimaryKey";
        public const string dataproviderdatamodel_delete = "DELETE FROM [2am].[dbo].[DataProvider] WHERE DataProviderKey = @PrimaryKey";
        public const string dataproviderdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DataProvider] WHERE";
        public const string dataproviderdatamodel_insert = "INSERT INTO [2am].[dbo].[DataProvider] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string dataproviderdatamodel_update = "UPDATE [2am].[dbo].[DataProvider] SET Description = @Description WHERE DataProviderKey = @DataProviderKey";



        public const string allocationmandatesetgroupdatamodel_selectwhere = "SELECT AllocationMandateSetGroupKey, AllocationGroupName, OrganisationStructureKey FROM [2am].[dbo].[AllocationMandateSetGroup] WHERE";
        public const string allocationmandatesetgroupdatamodel_selectbykey = "SELECT AllocationMandateSetGroupKey, AllocationGroupName, OrganisationStructureKey FROM [2am].[dbo].[AllocationMandateSetGroup] WHERE AllocationMandateSetGroupKey = @PrimaryKey";
        public const string allocationmandatesetgroupdatamodel_delete = "DELETE FROM [2am].[dbo].[AllocationMandateSetGroup] WHERE AllocationMandateSetGroupKey = @PrimaryKey";
        public const string allocationmandatesetgroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AllocationMandateSetGroup] WHERE";
        public const string allocationmandatesetgroupdatamodel_insert = "INSERT INTO [2am].[dbo].[AllocationMandateSetGroup] (AllocationGroupName, OrganisationStructureKey) VALUES(@AllocationGroupName, @OrganisationStructureKey); select cast(scope_identity() as int)";
        public const string allocationmandatesetgroupdatamodel_update = "UPDATE [2am].[dbo].[AllocationMandateSetGroup] SET AllocationGroupName = @AllocationGroupName, OrganisationStructureKey = @OrganisationStructureKey WHERE AllocationMandateSetGroupKey = @AllocationMandateSetGroupKey";



        public const string contenttypedatamodel_selectwhere = "SELECT ContentTypeKey, Description FROM [2am].[dbo].[ContentType] WHERE";
        public const string contenttypedatamodel_selectbykey = "SELECT ContentTypeKey, Description FROM [2am].[dbo].[ContentType] WHERE ContentTypeKey = @PrimaryKey";
        public const string contenttypedatamodel_delete = "DELETE FROM [2am].[dbo].[ContentType] WHERE ContentTypeKey = @PrimaryKey";
        public const string contenttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ContentType] WHERE";
        public const string contenttypedatamodel_insert = "INSERT INTO [2am].[dbo].[ContentType] (ContentTypeKey, Description) VALUES(@ContentTypeKey, @Description); ";
        public const string contenttypedatamodel_update = "UPDATE [2am].[dbo].[ContentType] SET ContentTypeKey = @ContentTypeKey, Description = @Description WHERE ContentTypeKey = @ContentTypeKey";



        public const string batchservicetypedatamodel_selectwhere = "SELECT BatchServiceTypeKey, Description FROM [2am].[dbo].[BatchServiceType] WHERE";
        public const string batchservicetypedatamodel_selectbykey = "SELECT BatchServiceTypeKey, Description FROM [2am].[dbo].[BatchServiceType] WHERE BatchServiceTypeKey = @PrimaryKey";
        public const string batchservicetypedatamodel_delete = "DELETE FROM [2am].[dbo].[BatchServiceType] WHERE BatchServiceTypeKey = @PrimaryKey";
        public const string batchservicetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchServiceType] WHERE";
        public const string batchservicetypedatamodel_insert = "INSERT INTO [2am].[dbo].[BatchServiceType] (BatchServiceTypeKey, Description) VALUES(@BatchServiceTypeKey, @Description); ";
        public const string batchservicetypedatamodel_update = "UPDATE [2am].[dbo].[BatchServiceType] SET BatchServiceTypeKey = @BatchServiceTypeKey, Description = @Description WHERE BatchServiceTypeKey = @BatchServiceTypeKey";



        public const string workflowrulesetitemdatamodel_selectwhere = "SELECT WorkflowRuleSetItemKey, WorkflowRuleSetKey, RuleItemKey FROM [2am].[dbo].[WorkflowRuleSetItem] WHERE";
        public const string workflowrulesetitemdatamodel_selectbykey = "SELECT WorkflowRuleSetItemKey, WorkflowRuleSetKey, RuleItemKey FROM [2am].[dbo].[WorkflowRuleSetItem] WHERE WorkflowRuleSetItemKey = @PrimaryKey";
        public const string workflowrulesetitemdatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowRuleSetItem] WHERE WorkflowRuleSetItemKey = @PrimaryKey";
        public const string workflowrulesetitemdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowRuleSetItem] WHERE";
        public const string workflowrulesetitemdatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowRuleSetItem] (WorkflowRuleSetKey, RuleItemKey) VALUES(@WorkflowRuleSetKey, @RuleItemKey); select cast(scope_identity() as int)";
        public const string workflowrulesetitemdatamodel_update = "UPDATE [2am].[dbo].[WorkflowRuleSetItem] SET WorkflowRuleSetKey = @WorkflowRuleSetKey, RuleItemKey = @RuleItemKey WHERE WorkflowRuleSetItemKey = @WorkflowRuleSetItemKey";



        public const string thirdpartydatamodel_selectwhere = "SELECT ThirdPartyKey, Id, LegalEntityKey, IsPanel, GeneralStatusKey, GenericKey, ThirdPartyTypeKey, GenericKeyTypeKey FROM [2am].[dbo].[ThirdParty] WHERE";
        public const string thirdpartydatamodel_selectbykey = "SELECT ThirdPartyKey, Id, LegalEntityKey, IsPanel, GeneralStatusKey, GenericKey, ThirdPartyTypeKey, GenericKeyTypeKey FROM [2am].[dbo].[ThirdParty] WHERE ThirdPartyKey = @PrimaryKey";
        public const string thirdpartydatamodel_delete = "DELETE FROM [2am].[dbo].[ThirdParty] WHERE ThirdPartyKey = @PrimaryKey";
        public const string thirdpartydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ThirdParty] WHERE";
        public const string thirdpartydatamodel_insert = "INSERT INTO [2am].[dbo].[ThirdParty] (Id, LegalEntityKey, IsPanel, GeneralStatusKey, GenericKey, ThirdPartyTypeKey, GenericKeyTypeKey) VALUES(@Id, @LegalEntityKey, @IsPanel, @GeneralStatusKey, @GenericKey, @ThirdPartyTypeKey, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string thirdpartydatamodel_update = "UPDATE [2am].[dbo].[ThirdParty] SET Id = @Id, LegalEntityKey = @LegalEntityKey, IsPanel = @IsPanel, GeneralStatusKey = @GeneralStatusKey, GenericKey = @GenericKey, ThirdPartyTypeKey = @ThirdPartyTypeKey, GenericKeyTypeKey = @GenericKeyTypeKey WHERE ThirdPartyKey = @ThirdPartyKey";



        public const string hocratesdatamodel_selectwhere = "SELECT HOCRatesKey, HOCInsurerKey, HOCSubsidenceKey, ThatchPremium, ConventionalPremium, ShinglePremium, SASRIAFactor, MinSASRIAAmount, AdminFee, AnnualEscalation, IsActive FROM [2am].[dbo].[HOCRates] WHERE";
        public const string hocratesdatamodel_selectbykey = "SELECT HOCRatesKey, HOCInsurerKey, HOCSubsidenceKey, ThatchPremium, ConventionalPremium, ShinglePremium, SASRIAFactor, MinSASRIAAmount, AdminFee, AnnualEscalation, IsActive FROM [2am].[dbo].[HOCRates] WHERE HOCRatesKey = @PrimaryKey";
        public const string hocratesdatamodel_delete = "DELETE FROM [2am].[dbo].[HOCRates] WHERE HOCRatesKey = @PrimaryKey";
        public const string hocratesdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCRates] WHERE";
        public const string hocratesdatamodel_insert = "INSERT INTO [2am].[dbo].[HOCRates] (HOCInsurerKey, HOCSubsidenceKey, ThatchPremium, ConventionalPremium, ShinglePremium, SASRIAFactor, MinSASRIAAmount, AdminFee, AnnualEscalation, IsActive) VALUES(@HOCInsurerKey, @HOCSubsidenceKey, @ThatchPremium, @ConventionalPremium, @ShinglePremium, @SASRIAFactor, @MinSASRIAAmount, @AdminFee, @AnnualEscalation, @IsActive); select cast(scope_identity() as int)";
        public const string hocratesdatamodel_update = "UPDATE [2am].[dbo].[HOCRates] SET HOCInsurerKey = @HOCInsurerKey, HOCSubsidenceKey = @HOCSubsidenceKey, ThatchPremium = @ThatchPremium, ConventionalPremium = @ConventionalPremium, ShinglePremium = @ShinglePremium, SASRIAFactor = @SASRIAFactor, MinSASRIAAmount = @MinSASRIAAmount, AdminFee = @AdminFee, AnnualEscalation = @AnnualEscalation, IsActive = @IsActive WHERE HOCRatesKey = @HOCRatesKey";



        public const string lifeinsurableinterestdatamodel_selectwhere = "SELECT LifeInsurableInterestKey, LegalEntityKey, AccountKey, LifeInsurableInterestTypeKey FROM [2am].[dbo].[LifeInsurableInterest] WHERE";
        public const string lifeinsurableinterestdatamodel_selectbykey = "SELECT LifeInsurableInterestKey, LegalEntityKey, AccountKey, LifeInsurableInterestTypeKey FROM [2am].[dbo].[LifeInsurableInterest] WHERE LifeInsurableInterestKey = @PrimaryKey";
        public const string lifeinsurableinterestdatamodel_delete = "DELETE FROM [2am].[dbo].[LifeInsurableInterest] WHERE LifeInsurableInterestKey = @PrimaryKey";
        public const string lifeinsurableinterestdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifeInsurableInterest] WHERE";
        public const string lifeinsurableinterestdatamodel_insert = "INSERT INTO [2am].[dbo].[LifeInsurableInterest] (LegalEntityKey, AccountKey, LifeInsurableInterestTypeKey) VALUES(@LegalEntityKey, @AccountKey, @LifeInsurableInterestTypeKey); select cast(scope_identity() as int)";
        public const string lifeinsurableinterestdatamodel_update = "UPDATE [2am].[dbo].[LifeInsurableInterest] SET LegalEntityKey = @LegalEntityKey, AccountKey = @AccountKey, LifeInsurableInterestTypeKey = @LifeInsurableInterestTypeKey WHERE LifeInsurableInterestKey = @LifeInsurableInterestKey";



        public const string parametertypedatamodel_selectwhere = "SELECT ParameterTypeKey, Description, SQLDataType, CSharpDataType FROM [2am].[dbo].[ParameterType] WHERE";
        public const string parametertypedatamodel_selectbykey = "SELECT ParameterTypeKey, Description, SQLDataType, CSharpDataType FROM [2am].[dbo].[ParameterType] WHERE ParameterTypeKey = @PrimaryKey";
        public const string parametertypedatamodel_delete = "DELETE FROM [2am].[dbo].[ParameterType] WHERE ParameterTypeKey = @PrimaryKey";
        public const string parametertypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ParameterType] WHERE";
        public const string parametertypedatamodel_insert = "INSERT INTO [2am].[dbo].[ParameterType] (Description, SQLDataType, CSharpDataType) VALUES(@Description, @SQLDataType, @CSharpDataType); select cast(scope_identity() as int)";
        public const string parametertypedatamodel_update = "UPDATE [2am].[dbo].[ParameterType] SET Description = @Description, SQLDataType = @SQLDataType, CSharpDataType = @CSharpDataType WHERE ParameterTypeKey = @ParameterTypeKey";



        public const string scorecardattributerangedatamodel_selectwhere = "SELECT ScoreCardAttributeRangeKey, ScoreCardAttributeKey, Min, Max, Points FROM [2am].[dbo].[ScoreCardAttributeRange] WHERE";
        public const string scorecardattributerangedatamodel_selectbykey = "SELECT ScoreCardAttributeRangeKey, ScoreCardAttributeKey, Min, Max, Points FROM [2am].[dbo].[ScoreCardAttributeRange] WHERE ScoreCardAttributeRangeKey = @PrimaryKey";
        public const string scorecardattributerangedatamodel_delete = "DELETE FROM [2am].[dbo].[ScoreCardAttributeRange] WHERE ScoreCardAttributeRangeKey = @PrimaryKey";
        public const string scorecardattributerangedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ScoreCardAttributeRange] WHERE";
        public const string scorecardattributerangedatamodel_insert = "INSERT INTO [2am].[dbo].[ScoreCardAttributeRange] (ScoreCardAttributeRangeKey, ScoreCardAttributeKey, Min, Max, Points) VALUES(@ScoreCardAttributeRangeKey, @ScoreCardAttributeKey, @Min, @Max, @Points); ";
        public const string scorecardattributerangedatamodel_update = "UPDATE [2am].[dbo].[ScoreCardAttributeRange] SET ScoreCardAttributeRangeKey = @ScoreCardAttributeRangeKey, ScoreCardAttributeKey = @ScoreCardAttributeKey, Min = @Min, Max = @Max, Points = @Points WHERE ScoreCardAttributeRangeKey = @ScoreCardAttributeRangeKey";



        public const string batchservicedatamodel_selectwhere = "SELECT BatchServiceKey, BatchServiceTypeKey, RequestedDate, RequestedBy, BatchCount, FileContent, FileName FROM [2am].[dbo].[BatchService] WHERE";
        public const string batchservicedatamodel_selectbykey = "SELECT BatchServiceKey, BatchServiceTypeKey, RequestedDate, RequestedBy, BatchCount, FileContent, FileName FROM [2am].[dbo].[BatchService] WHERE BatchServiceKey = @PrimaryKey";
        public const string batchservicedatamodel_delete = "DELETE FROM [2am].[dbo].[BatchService] WHERE BatchServiceKey = @PrimaryKey";
        public const string batchservicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchService] WHERE";
        public const string batchservicedatamodel_insert = "INSERT INTO [2am].[dbo].[BatchService] (BatchServiceTypeKey, RequestedDate, RequestedBy, BatchCount, FileContent, FileName) VALUES(@BatchServiceTypeKey, @RequestedDate, @RequestedBy, @BatchCount, @FileContent, @FileName); select cast(scope_identity() as int)";
        public const string batchservicedatamodel_update = "UPDATE [2am].[dbo].[BatchService] SET BatchServiceTypeKey = @BatchServiceTypeKey, RequestedDate = @RequestedDate, RequestedBy = @RequestedBy, BatchCount = @BatchCount, FileContent = @FileContent, FileName = @FileName WHERE BatchServiceKey = @BatchServiceKey";



        public const string financialservicepaymenttypedatamodel_selectwhere = "SELECT FinancialServicePaymentTypeKey, Description FROM [2am].[dbo].[FinancialServicePaymentType] WHERE";
        public const string financialservicepaymenttypedatamodel_selectbykey = "SELECT FinancialServicePaymentTypeKey, Description FROM [2am].[dbo].[FinancialServicePaymentType] WHERE FinancialServicePaymentTypeKey = @PrimaryKey";
        public const string financialservicepaymenttypedatamodel_delete = "DELETE FROM [2am].[dbo].[FinancialServicePaymentType] WHERE FinancialServicePaymentTypeKey = @PrimaryKey";
        public const string financialservicepaymenttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FinancialServicePaymentType] WHERE";
        public const string financialservicepaymenttypedatamodel_insert = "INSERT INTO [2am].[dbo].[FinancialServicePaymentType] (FinancialServicePaymentTypeKey, Description) VALUES(@FinancialServicePaymentTypeKey, @Description); ";
        public const string financialservicepaymenttypedatamodel_update = "UPDATE [2am].[dbo].[FinancialServicePaymentType] SET FinancialServicePaymentTypeKey = @FinancialServicePaymentTypeKey, Description = @Description WHERE FinancialServicePaymentTypeKey = @FinancialServicePaymentTypeKey";



        public const string employmentdatamodel_selectwhere = "SELECT EmploymentKey, EmployerKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmploymentStartDate, EmploymentEndDate, ContactPerson, ContactPhoneNumber, ContactPhoneCode, ConfirmedBy, ConfirmedDate, UserID, ChangeDate, Department, BasicIncome, Commission, Overtime, Shift, Performance, Allowances, PAYE, UIF, PensionProvident, MedicalAid, ConfirmedBasicIncome, ConfirmedCommission, ConfirmedOvertime, ConfirmedShift, ConfirmedPerformance, ConfirmedAllowances, ConfirmedPAYE, ConfirmedUIF, ConfirmedPensionProvident, ConfirmedMedicalAid, JobTitle, MonthlyIncome, ConfirmedIncome, ConfirmedEmploymentFlag, ConfirmedIncomeFlag, EmploymentConfirmationSourceKey, SalaryPaymentDay, UnionMember FROM [2am].[dbo].[Employment] WHERE";
        public const string employmentdatamodel_selectbykey = "SELECT EmploymentKey, EmployerKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmploymentStartDate, EmploymentEndDate, ContactPerson, ContactPhoneNumber, ContactPhoneCode, ConfirmedBy, ConfirmedDate, UserID, ChangeDate, Department, BasicIncome, Commission, Overtime, Shift, Performance, Allowances, PAYE, UIF, PensionProvident, MedicalAid, ConfirmedBasicIncome, ConfirmedCommission, ConfirmedOvertime, ConfirmedShift, ConfirmedPerformance, ConfirmedAllowances, ConfirmedPAYE, ConfirmedUIF, ConfirmedPensionProvident, ConfirmedMedicalAid, JobTitle, MonthlyIncome, ConfirmedIncome, ConfirmedEmploymentFlag, ConfirmedIncomeFlag, EmploymentConfirmationSourceKey, SalaryPaymentDay, UnionMember FROM [2am].[dbo].[Employment] WHERE EmploymentKey = @PrimaryKey";
        public const string employmentdatamodel_delete = "DELETE FROM [2am].[dbo].[Employment] WHERE EmploymentKey = @PrimaryKey";
        public const string employmentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Employment] WHERE";
        public const string employmentdatamodel_insert = "INSERT INTO [2am].[dbo].[Employment] (EmployerKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmploymentStartDate, EmploymentEndDate, ContactPerson, ContactPhoneNumber, ContactPhoneCode, ConfirmedBy, ConfirmedDate, UserID, ChangeDate, Department, BasicIncome, Commission, Overtime, Shift, Performance, Allowances, PAYE, UIF, PensionProvident, MedicalAid, ConfirmedBasicIncome, ConfirmedCommission, ConfirmedOvertime, ConfirmedShift, ConfirmedPerformance, ConfirmedAllowances, ConfirmedPAYE, ConfirmedUIF, ConfirmedPensionProvident, ConfirmedMedicalAid, JobTitle, ConfirmedEmploymentFlag, ConfirmedIncomeFlag, EmploymentConfirmationSourceKey, SalaryPaymentDay, UnionMember) VALUES(@EmployerKey, @EmploymentTypeKey, @RemunerationTypeKey, @EmploymentStatusKey, @LegalEntityKey, @EmploymentStartDate, @EmploymentEndDate, @ContactPerson, @ContactPhoneNumber, @ContactPhoneCode, @ConfirmedBy, @ConfirmedDate, @UserID, @ChangeDate, @Department, @BasicIncome, @Commission, @Overtime, @Shift, @Performance, @Allowances, @PAYE, @UIF, @PensionProvident, @MedicalAid, @ConfirmedBasicIncome, @ConfirmedCommission, @ConfirmedOvertime, @ConfirmedShift, @ConfirmedPerformance, @ConfirmedAllowances, @ConfirmedPAYE, @ConfirmedUIF, @ConfirmedPensionProvident, @ConfirmedMedicalAid, @JobTitle, @ConfirmedEmploymentFlag, @ConfirmedIncomeFlag, @EmploymentConfirmationSourceKey, @SalaryPaymentDay, @UnionMember); select cast(scope_identity() as int)";
        public const string employmentdatamodel_update = "UPDATE [2am].[dbo].[Employment] SET EmployerKey = @EmployerKey, EmploymentTypeKey = @EmploymentTypeKey, RemunerationTypeKey = @RemunerationTypeKey, EmploymentStatusKey = @EmploymentStatusKey, LegalEntityKey = @LegalEntityKey, EmploymentStartDate = @EmploymentStartDate, EmploymentEndDate = @EmploymentEndDate, ContactPerson = @ContactPerson, ContactPhoneNumber = @ContactPhoneNumber, ContactPhoneCode = @ContactPhoneCode, ConfirmedBy = @ConfirmedBy, ConfirmedDate = @ConfirmedDate, UserID = @UserID, ChangeDate = @ChangeDate, Department = @Department, BasicIncome = @BasicIncome, Commission = @Commission, Overtime = @Overtime, Shift = @Shift, Performance = @Performance, Allowances = @Allowances, PAYE = @PAYE, UIF = @UIF, PensionProvident = @PensionProvident, MedicalAid = @MedicalAid, ConfirmedBasicIncome = @ConfirmedBasicIncome, ConfirmedCommission = @ConfirmedCommission, ConfirmedOvertime = @ConfirmedOvertime, ConfirmedShift = @ConfirmedShift, ConfirmedPerformance = @ConfirmedPerformance, ConfirmedAllowances = @ConfirmedAllowances, ConfirmedPAYE = @ConfirmedPAYE, ConfirmedUIF = @ConfirmedUIF, ConfirmedPensionProvident = @ConfirmedPensionProvident, ConfirmedMedicalAid = @ConfirmedMedicalAid, JobTitle = @JobTitle, ConfirmedEmploymentFlag = @ConfirmedEmploymentFlag, ConfirmedIncomeFlag = @ConfirmedIncomeFlag, EmploymentConfirmationSourceKey = @EmploymentConfirmationSourceKey, SalaryPaymentDay = @SalaryPaymentDay, UnionMember = @UnionMember WHERE EmploymentKey = @EmploymentKey";



        public const string statementdefinitiondatamodel_selectwhere = "SELECT StatementDefinitionKey, Description, ApplicationName, StatementName FROM [2am].[dbo].[StatementDefinition] WHERE";
        public const string statementdefinitiondatamodel_selectbykey = "SELECT StatementDefinitionKey, Description, ApplicationName, StatementName FROM [2am].[dbo].[StatementDefinition] WHERE StatementDefinitionKey = @PrimaryKey";
        public const string statementdefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[StatementDefinition] WHERE StatementDefinitionKey = @PrimaryKey";
        public const string statementdefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StatementDefinition] WHERE";
        public const string statementdefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[StatementDefinition] (Description, ApplicationName, StatementName) VALUES(@Description, @ApplicationName, @StatementName); select cast(scope_identity() as int)";
        public const string statementdefinitiondatamodel_update = "UPDATE [2am].[dbo].[StatementDefinition] SET Description = @Description, ApplicationName = @ApplicationName, StatementName = @StatementName WHERE StatementDefinitionKey = @StatementDefinitionKey";



        public const string allocationmandatesetdatamodel_selectwhere = "SELECT AllocationMandateSetKey, AllocationMandateSetGroupKey FROM [2am].[dbo].[AllocationMandateSet] WHERE";
        public const string allocationmandatesetdatamodel_selectbykey = "SELECT AllocationMandateSetKey, AllocationMandateSetGroupKey FROM [2am].[dbo].[AllocationMandateSet] WHERE AllocationMandateSetKey = @PrimaryKey";
        public const string allocationmandatesetdatamodel_delete = "DELETE FROM [2am].[dbo].[AllocationMandateSet] WHERE AllocationMandateSetKey = @PrimaryKey";
        public const string allocationmandatesetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AllocationMandateSet] WHERE";
        public const string allocationmandatesetdatamodel_insert = "INSERT INTO [2am].[dbo].[AllocationMandateSet] (AllocationMandateSetGroupKey) VALUES(@AllocationMandateSetGroupKey); select cast(scope_identity() as int)";
        public const string allocationmandatesetdatamodel_update = "UPDATE [2am].[dbo].[AllocationMandateSet] SET AllocationMandateSetGroupKey = @AllocationMandateSetGroupKey WHERE AllocationMandateSetKey = @AllocationMandateSetKey";



        public const string offerdeclarationanswerdatamodel_selectwhere = "SELECT OfferDeclarationAnswerKey, Description FROM [2am].[dbo].[OfferDeclarationAnswer] WHERE";
        public const string offerdeclarationanswerdatamodel_selectbykey = "SELECT OfferDeclarationAnswerKey, Description FROM [2am].[dbo].[OfferDeclarationAnswer] WHERE OfferDeclarationAnswerKey = @PrimaryKey";
        public const string offerdeclarationanswerdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDeclarationAnswer] WHERE OfferDeclarationAnswerKey = @PrimaryKey";
        public const string offerdeclarationanswerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDeclarationAnswer] WHERE";
        public const string offerdeclarationanswerdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDeclarationAnswer] (OfferDeclarationAnswerKey, Description) VALUES(@OfferDeclarationAnswerKey, @Description); ";
        public const string offerdeclarationanswerdatamodel_update = "UPDATE [2am].[dbo].[OfferDeclarationAnswer] SET OfferDeclarationAnswerKey = @OfferDeclarationAnswerKey, Description = @Description WHERE OfferDeclarationAnswerKey = @OfferDeclarationAnswerKey";



        public const string statementparameterdatamodel_selectwhere = "SELECT StatementParameterKey, ParameterName, ParameterTypeKey, StatementDefinitionKey, ParameterLength, DisplayName, Required, PopulationStatementDefinitionKey FROM [2am].[dbo].[StatementParameter] WHERE";
        public const string statementparameterdatamodel_selectbykey = "SELECT StatementParameterKey, ParameterName, ParameterTypeKey, StatementDefinitionKey, ParameterLength, DisplayName, Required, PopulationStatementDefinitionKey FROM [2am].[dbo].[StatementParameter] WHERE StatementParameterKey = @PrimaryKey";
        public const string statementparameterdatamodel_delete = "DELETE FROM [2am].[dbo].[StatementParameter] WHERE StatementParameterKey = @PrimaryKey";
        public const string statementparameterdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StatementParameter] WHERE";
        public const string statementparameterdatamodel_insert = "INSERT INTO [2am].[dbo].[StatementParameter] (ParameterName, ParameterTypeKey, StatementDefinitionKey, ParameterLength, DisplayName, Required, PopulationStatementDefinitionKey) VALUES(@ParameterName, @ParameterTypeKey, @StatementDefinitionKey, @ParameterLength, @DisplayName, @Required, @PopulationStatementDefinitionKey); select cast(scope_identity() as int)";
        public const string statementparameterdatamodel_update = "UPDATE [2am].[dbo].[StatementParameter] SET ParameterName = @ParameterName, ParameterTypeKey = @ParameterTypeKey, StatementDefinitionKey = @StatementDefinitionKey, ParameterLength = @ParameterLength, DisplayName = @DisplayName, Required = @Required, PopulationStatementDefinitionKey = @PopulationStatementDefinitionKey WHERE StatementParameterKey = @StatementParameterKey";



        public const string batchtransactionbackupdatamodel_selectwhere = "SELECT BatchTransactionBackUpKey, BatchTransactionKey, BulkBatchKey, AccountKey, LegalEntityKey, TransactionTypeNumber, EffectiveDate, Amount, Reference, UserID, BatchTransactionStatusKey, HistoryType FROM [2am].[dbo].[BatchTransactionBackup] WHERE";
        public const string batchtransactionbackupdatamodel_selectbykey = "SELECT BatchTransactionBackUpKey, BatchTransactionKey, BulkBatchKey, AccountKey, LegalEntityKey, TransactionTypeNumber, EffectiveDate, Amount, Reference, UserID, BatchTransactionStatusKey, HistoryType FROM [2am].[dbo].[BatchTransactionBackup] WHERE BatchTransactionBackUpKey = @PrimaryKey";
        public const string batchtransactionbackupdatamodel_delete = "DELETE FROM [2am].[dbo].[BatchTransactionBackup] WHERE BatchTransactionBackUpKey = @PrimaryKey";
        public const string batchtransactionbackupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BatchTransactionBackup] WHERE";
        public const string batchtransactionbackupdatamodel_insert = "INSERT INTO [2am].[dbo].[BatchTransactionBackup] (BatchTransactionKey, BulkBatchKey, AccountKey, LegalEntityKey, TransactionTypeNumber, EffectiveDate, Amount, Reference, UserID, BatchTransactionStatusKey, HistoryType) VALUES(@BatchTransactionKey, @BulkBatchKey, @AccountKey, @LegalEntityKey, @TransactionTypeNumber, @EffectiveDate, @Amount, @Reference, @UserID, @BatchTransactionStatusKey, @HistoryType); select cast(scope_identity() as int)";
        public const string batchtransactionbackupdatamodel_update = "UPDATE [2am].[dbo].[BatchTransactionBackup] SET BatchTransactionKey = @BatchTransactionKey, BulkBatchKey = @BulkBatchKey, AccountKey = @AccountKey, LegalEntityKey = @LegalEntityKey, TransactionTypeNumber = @TransactionTypeNumber, EffectiveDate = @EffectiveDate, Amount = @Amount, Reference = @Reference, UserID = @UserID, BatchTransactionStatusKey = @BatchTransactionStatusKey, HistoryType = @HistoryType WHERE BatchTransactionBackUpKey = @BatchTransactionBackUpKey";



        public const string offerroledomiciliumdatamodel_selectwhere = "SELECT OfferRoleDomiciliumKey, LegalEntityDomiciliumKey, OfferRoleKey, ChangeDate, ADUserKey FROM [2am].[dbo].[OfferRoleDomicilium] WHERE";
        public const string offerroledomiciliumdatamodel_selectbykey = "SELECT OfferRoleDomiciliumKey, LegalEntityDomiciliumKey, OfferRoleKey, ChangeDate, ADUserKey FROM [2am].[dbo].[OfferRoleDomicilium] WHERE OfferRoleDomiciliumKey = @PrimaryKey";
        public const string offerroledomiciliumdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferRoleDomicilium] WHERE OfferRoleDomiciliumKey = @PrimaryKey";
        public const string offerroledomiciliumdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferRoleDomicilium] WHERE";
        public const string offerroledomiciliumdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferRoleDomicilium] (LegalEntityDomiciliumKey, OfferRoleKey, ChangeDate, ADUserKey) VALUES(@LegalEntityDomiciliumKey, @OfferRoleKey, @ChangeDate, @ADUserKey); select cast(scope_identity() as int)";
        public const string offerroledomiciliumdatamodel_update = "UPDATE [2am].[dbo].[OfferRoleDomicilium] SET LegalEntityDomiciliumKey = @LegalEntityDomiciliumKey, OfferRoleKey = @OfferRoleKey, ChangeDate = @ChangeDate, ADUserKey = @ADUserKey WHERE OfferRoleDomiciliumKey = @OfferRoleDomiciliumKey";



        public const string auditresetdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ResetKey, ResetDate, RunDate, ResetConfigurationKey, JIBARRate, JIBARDiscountRate FROM [2am].[dbo].[AuditReset] WHERE";
        public const string auditresetdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ResetKey, ResetDate, RunDate, ResetConfigurationKey, JIBARRate, JIBARDiscountRate FROM [2am].[dbo].[AuditReset] WHERE AuditNumber = @PrimaryKey";
        public const string auditresetdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditReset] WHERE AuditNumber = @PrimaryKey";
        public const string auditresetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditReset] WHERE";
        public const string auditresetdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditReset] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ResetKey, ResetDate, RunDate, ResetConfigurationKey, JIBARRate, JIBARDiscountRate) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @ResetKey, @ResetDate, @RunDate, @ResetConfigurationKey, @JIBARRate, @JIBARDiscountRate); select cast(scope_identity() as int)";
        public const string auditresetdatamodel_update = "UPDATE [2am].[dbo].[AuditReset] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, ResetKey = @ResetKey, ResetDate = @ResetDate, RunDate = @RunDate, ResetConfigurationKey = @ResetConfigurationKey, JIBARRate = @JIBARRate, JIBARDiscountRate = @JIBARDiscountRate WHERE AuditNumber = @AuditNumber";



        public const string assetliabilitysubtypedatamodel_selectwhere = "SELECT AssetLiabilitySubTypeKey, AssetLiabilityTypeKey, Description FROM [2am].[dbo].[AssetLiabilitySubType] WHERE";
        public const string assetliabilitysubtypedatamodel_selectbykey = "SELECT AssetLiabilitySubTypeKey, AssetLiabilityTypeKey, Description FROM [2am].[dbo].[AssetLiabilitySubType] WHERE AssetLiabilitySubTypeKey = @PrimaryKey";
        public const string assetliabilitysubtypedatamodel_delete = "DELETE FROM [2am].[dbo].[AssetLiabilitySubType] WHERE AssetLiabilitySubTypeKey = @PrimaryKey";
        public const string assetliabilitysubtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AssetLiabilitySubType] WHERE";
        public const string assetliabilitysubtypedatamodel_insert = "INSERT INTO [2am].[dbo].[AssetLiabilitySubType] (AssetLiabilityTypeKey, Description) VALUES(@AssetLiabilityTypeKey, @Description); select cast(scope_identity() as int)";
        public const string assetliabilitysubtypedatamodel_update = "UPDATE [2am].[dbo].[AssetLiabilitySubType] SET AssetLiabilityTypeKey = @AssetLiabilityTypeKey, Description = @Description WHERE AssetLiabilitySubTypeKey = @AssetLiabilitySubTypeKey";



        public const string invoicelineitemdatamodel_selectwhere = "SELECT InvoiceLineItemKey, ThirdPartyInvoiceKey, InvoiceLineItemDescriptionKey, Amount, IsVATItem, VATAmount, TotalAmountIncludingVAT FROM [2am].[dbo].[InvoiceLineItem] WHERE";
        public const string invoicelineitemdatamodel_selectbykey = "SELECT InvoiceLineItemKey, ThirdPartyInvoiceKey, InvoiceLineItemDescriptionKey, Amount, IsVATItem, VATAmount, TotalAmountIncludingVAT FROM [2am].[dbo].[InvoiceLineItem] WHERE InvoiceLineItemKey = @PrimaryKey";
        public const string invoicelineitemdatamodel_delete = "DELETE FROM [2am].[dbo].[InvoiceLineItem] WHERE InvoiceLineItemKey = @PrimaryKey";
        public const string invoicelineitemdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[InvoiceLineItem] WHERE";
        public const string invoicelineitemdatamodel_insert = "INSERT INTO [2am].[dbo].[InvoiceLineItem] (ThirdPartyInvoiceKey, InvoiceLineItemDescriptionKey, Amount, IsVATItem, VATAmount, TotalAmountIncludingVAT) VALUES(@ThirdPartyInvoiceKey, @InvoiceLineItemDescriptionKey, @Amount, @IsVATItem, @VATAmount, @TotalAmountIncludingVAT); select cast(scope_identity() as int)";
        public const string invoicelineitemdatamodel_update = "UPDATE [2am].[dbo].[InvoiceLineItem] SET ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey, InvoiceLineItemDescriptionKey = @InvoiceLineItemDescriptionKey, Amount = @Amount, IsVATItem = @IsVATItem, VATAmount = @VATAmount, TotalAmountIncludingVAT = @TotalAmountIncludingVAT WHERE InvoiceLineItemKey = @InvoiceLineItemKey";



        public const string propertytypedatamodel_selectwhere = "SELECT PropertyTypeKey, Description FROM [2am].[dbo].[PropertyType] WHERE";
        public const string propertytypedatamodel_selectbykey = "SELECT PropertyTypeKey, Description FROM [2am].[dbo].[PropertyType] WHERE PropertyTypeKey = @PrimaryKey";
        public const string propertytypedatamodel_delete = "DELETE FROM [2am].[dbo].[PropertyType] WHERE PropertyTypeKey = @PrimaryKey";
        public const string propertytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PropertyType] WHERE";
        public const string propertytypedatamodel_insert = "INSERT INTO [2am].[dbo].[PropertyType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string propertytypedatamodel_update = "UPDATE [2am].[dbo].[PropertyType] SET Description = @Description WHERE PropertyTypeKey = @PropertyTypeKey";



        public const string correspondencemediumdatamodel_selectwhere = "SELECT CorrespondenceMediumKey, Description FROM [2am].[dbo].[CorrespondenceMedium] WHERE";
        public const string correspondencemediumdatamodel_selectbykey = "SELECT CorrespondenceMediumKey, Description FROM [2am].[dbo].[CorrespondenceMedium] WHERE CorrespondenceMediumKey = @PrimaryKey";
        public const string correspondencemediumdatamodel_delete = "DELETE FROM [2am].[dbo].[CorrespondenceMedium] WHERE CorrespondenceMediumKey = @PrimaryKey";
        public const string correspondencemediumdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CorrespondenceMedium] WHERE";
        public const string correspondencemediumdatamodel_insert = "INSERT INTO [2am].[dbo].[CorrespondenceMedium] (CorrespondenceMediumKey, Description) VALUES(@CorrespondenceMediumKey, @Description); ";
        public const string correspondencemediumdatamodel_update = "UPDATE [2am].[dbo].[CorrespondenceMedium] SET CorrespondenceMediumKey = @CorrespondenceMediumKey, Description = @Description WHERE CorrespondenceMediumKey = @CorrespondenceMediumKey";



        public const string riskmatrixdimensionscorecarddatamodel_selectwhere = "SELECT RiskMatrixDimensionScoreCardKey, RiskMatrixDimensionKey, ScoreCardKey FROM [2am].[dbo].[RiskMatrixDimensionScoreCard] WHERE";
        public const string riskmatrixdimensionscorecarddatamodel_selectbykey = "SELECT RiskMatrixDimensionScoreCardKey, RiskMatrixDimensionKey, ScoreCardKey FROM [2am].[dbo].[RiskMatrixDimensionScoreCard] WHERE RiskMatrixDimensionScoreCardKey = @PrimaryKey";
        public const string riskmatrixdimensionscorecarddatamodel_delete = "DELETE FROM [2am].[dbo].[RiskMatrixDimensionScoreCard] WHERE RiskMatrixDimensionScoreCardKey = @PrimaryKey";
        public const string riskmatrixdimensionscorecarddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RiskMatrixDimensionScoreCard] WHERE";
        public const string riskmatrixdimensionscorecarddatamodel_insert = "INSERT INTO [2am].[dbo].[RiskMatrixDimensionScoreCard] (RiskMatrixDimensionScoreCardKey, RiskMatrixDimensionKey, ScoreCardKey) VALUES(@RiskMatrixDimensionScoreCardKey, @RiskMatrixDimensionKey, @ScoreCardKey); ";
        public const string riskmatrixdimensionscorecarddatamodel_update = "UPDATE [2am].[dbo].[RiskMatrixDimensionScoreCard] SET RiskMatrixDimensionScoreCardKey = @RiskMatrixDimensionScoreCardKey, RiskMatrixDimensionKey = @RiskMatrixDimensionKey, ScoreCardKey = @ScoreCardKey WHERE RiskMatrixDimensionScoreCardKey = @RiskMatrixDimensionScoreCardKey";



        public const string externallifepolicydatamodel_selectwhere = "SELECT ExternalLifePolicyKey, InsurerKey, PolicyNumber, CommencementDate, LifePolicyStatusKey, CloseDate, SumInsured, PolicyCeded, LegalEntityKey FROM [2am].[dbo].[ExternalLifePolicy] WHERE";
        public const string externallifepolicydatamodel_selectbykey = "SELECT ExternalLifePolicyKey, InsurerKey, PolicyNumber, CommencementDate, LifePolicyStatusKey, CloseDate, SumInsured, PolicyCeded, LegalEntityKey FROM [2am].[dbo].[ExternalLifePolicy] WHERE ExternalLifePolicyKey = @PrimaryKey";
        public const string externallifepolicydatamodel_delete = "DELETE FROM [2am].[dbo].[ExternalLifePolicy] WHERE ExternalLifePolicyKey = @PrimaryKey";
        public const string externallifepolicydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ExternalLifePolicy] WHERE";
        public const string externallifepolicydatamodel_insert = "INSERT INTO [2am].[dbo].[ExternalLifePolicy] (InsurerKey, PolicyNumber, CommencementDate, LifePolicyStatusKey, CloseDate, SumInsured, PolicyCeded, LegalEntityKey) VALUES(@InsurerKey, @PolicyNumber, @CommencementDate, @LifePolicyStatusKey, @CloseDate, @SumInsured, @PolicyCeded, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string externallifepolicydatamodel_update = "UPDATE [2am].[dbo].[ExternalLifePolicy] SET InsurerKey = @InsurerKey, PolicyNumber = @PolicyNumber, CommencementDate = @CommencementDate, LifePolicyStatusKey = @LifePolicyStatusKey, CloseDate = @CloseDate, SumInsured = @SumInsured, PolicyCeded = @PolicyCeded, LegalEntityKey = @LegalEntityKey WHERE ExternalLifePolicyKey = @ExternalLifePolicyKey";



        public const string offerexpensedatamodel_selectwhere = "SELECT OfferExpenseKey, OfferKey, LegalEntityKey, ExpenseTypeKey, ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, ToBeSettled, OverRidden FROM [2am].[dbo].[OfferExpense] WHERE";
        public const string offerexpensedatamodel_selectbykey = "SELECT OfferExpenseKey, OfferKey, LegalEntityKey, ExpenseTypeKey, ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, ToBeSettled, OverRidden FROM [2am].[dbo].[OfferExpense] WHERE OfferExpenseKey = @PrimaryKey";
        public const string offerexpensedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferExpense] WHERE OfferExpenseKey = @PrimaryKey";
        public const string offerexpensedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferExpense] WHERE";
        public const string offerexpensedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferExpense] (OfferKey, LegalEntityKey, ExpenseTypeKey, ExpenseAccountNumber, ExpenseAccountName, ExpenseReference, TotalOutstandingAmount, MonthlyPayment, ToBeSettled, OverRidden) VALUES(@OfferKey, @LegalEntityKey, @ExpenseTypeKey, @ExpenseAccountNumber, @ExpenseAccountName, @ExpenseReference, @TotalOutstandingAmount, @MonthlyPayment, @ToBeSettled, @OverRidden); select cast(scope_identity() as int)";
        public const string offerexpensedatamodel_update = "UPDATE [2am].[dbo].[OfferExpense] SET OfferKey = @OfferKey, LegalEntityKey = @LegalEntityKey, ExpenseTypeKey = @ExpenseTypeKey, ExpenseAccountNumber = @ExpenseAccountNumber, ExpenseAccountName = @ExpenseAccountName, ExpenseReference = @ExpenseReference, TotalOutstandingAmount = @TotalOutstandingAmount, MonthlyPayment = @MonthlyPayment, ToBeSettled = @ToBeSettled, OverRidden = @OverRidden WHERE OfferExpenseKey = @OfferExpenseKey";



        public const string campaigntargetdatamodel_selectwhere = "SELECT CampaignTargetKey, CampaignDefinitionKey, GenericKey, ADUserKey, GenericKeyTypeKey FROM [2am].[dbo].[CampaignTarget] WHERE";
        public const string campaigntargetdatamodel_selectbykey = "SELECT CampaignTargetKey, CampaignDefinitionKey, GenericKey, ADUserKey, GenericKeyTypeKey FROM [2am].[dbo].[CampaignTarget] WHERE CampaignTargetKey = @PrimaryKey";
        public const string campaigntargetdatamodel_delete = "DELETE FROM [2am].[dbo].[CampaignTarget] WHERE CampaignTargetKey = @PrimaryKey";
        public const string campaigntargetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CampaignTarget] WHERE";
        public const string campaigntargetdatamodel_insert = "INSERT INTO [2am].[dbo].[CampaignTarget] (CampaignDefinitionKey, GenericKey, ADUserKey, GenericKeyTypeKey) VALUES(@CampaignDefinitionKey, @GenericKey, @ADUserKey, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string campaigntargetdatamodel_update = "UPDATE [2am].[dbo].[CampaignTarget] SET CampaignDefinitionKey = @CampaignDefinitionKey, GenericKey = @GenericKey, ADUserKey = @ADUserKey, GenericKeyTypeKey = @GenericKeyTypeKey WHERE CampaignTargetKey = @CampaignTargetKey";



        public const string allocationmandatesetuserorganisationstructuredatamodel_selectwhere = "SELECT AllocationMandateSetUserOrganisationStructureKey, AllocationMandateSetKey, UserOrganisationStructureKey FROM [2am].[dbo].[AllocationMandateSetUserOrganisationStructure] WHERE";
        public const string allocationmandatesetuserorganisationstructuredatamodel_selectbykey = "SELECT AllocationMandateSetUserOrganisationStructureKey, AllocationMandateSetKey, UserOrganisationStructureKey FROM [2am].[dbo].[AllocationMandateSetUserOrganisationStructure] WHERE AllocationMandateSetUserOrganisationStructureKey = @PrimaryKey";
        public const string allocationmandatesetuserorganisationstructuredatamodel_delete = "DELETE FROM [2am].[dbo].[AllocationMandateSetUserOrganisationStructure] WHERE AllocationMandateSetUserOrganisationStructureKey = @PrimaryKey";
        public const string allocationmandatesetuserorganisationstructuredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AllocationMandateSetUserOrganisationStructure] WHERE";
        public const string allocationmandatesetuserorganisationstructuredatamodel_insert = "INSERT INTO [2am].[dbo].[AllocationMandateSetUserOrganisationStructure] (AllocationMandateSetKey, UserOrganisationStructureKey) VALUES(@AllocationMandateSetKey, @UserOrganisationStructureKey); select cast(scope_identity() as int)";
        public const string allocationmandatesetuserorganisationstructuredatamodel_update = "UPDATE [2am].[dbo].[AllocationMandateSetUserOrganisationStructure] SET AllocationMandateSetKey = @AllocationMandateSetKey, UserOrganisationStructureKey = @UserOrganisationStructureKey WHERE AllocationMandateSetUserOrganisationStructureKey = @AllocationMandateSetUserOrganisationStructureKey";



        public const string reasontypedatamodel_selectwhere = "SELECT ReasonTypeKey, Description, GenericKeyTypeKey, ReasonTypeGroupKey FROM [2am].[dbo].[ReasonType] WHERE";
        public const string reasontypedatamodel_selectbykey = "SELECT ReasonTypeKey, Description, GenericKeyTypeKey, ReasonTypeGroupKey FROM [2am].[dbo].[ReasonType] WHERE ReasonTypeKey = @PrimaryKey";
        public const string reasontypedatamodel_delete = "DELETE FROM [2am].[dbo].[ReasonType] WHERE ReasonTypeKey = @PrimaryKey";
        public const string reasontypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReasonType] WHERE";
        public const string reasontypedatamodel_insert = "INSERT INTO [2am].[dbo].[ReasonType] (ReasonTypeKey, Description, GenericKeyTypeKey, ReasonTypeGroupKey) VALUES(@ReasonTypeKey, @Description, @GenericKeyTypeKey, @ReasonTypeGroupKey); ";
        public const string reasontypedatamodel_update = "UPDATE [2am].[dbo].[ReasonType] SET ReasonTypeKey = @ReasonTypeKey, Description = @Description, GenericKeyTypeKey = @GenericKeyTypeKey, ReasonTypeGroupKey = @ReasonTypeGroupKey WHERE ReasonTypeKey = @ReasonTypeKey";



        public const string propertydatamodel_selectwhere = "SELECT PropertyKey, PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, AddressKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, DataProviderKey FROM [2am].[dbo].[Property] WHERE";
        public const string propertydatamodel_selectbykey = "SELECT PropertyKey, PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, AddressKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, DataProviderKey FROM [2am].[dbo].[Property] WHERE PropertyKey = @PrimaryKey";
        public const string propertydatamodel_delete = "DELETE FROM [2am].[dbo].[Property] WHERE PropertyKey = @PrimaryKey";
        public const string propertydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Property] WHERE";
        public const string propertydatamodel_insert = "INSERT INTO [2am].[dbo].[Property] (PropertyTypeKey, TitleTypeKey, AreaClassificationKey, OccupancyTypeKey, AddressKey, PropertyDescription1, PropertyDescription2, PropertyDescription3, DeedsOfficeValue, CurrentBondDate, ErfNumber, ErfPortionNumber, SectionalSchemeName, SectionalUnitNumber, DeedsPropertyTypeKey, ErfSuburbDescription, ErfMetroDescription, DataProviderKey) VALUES(@PropertyTypeKey, @TitleTypeKey, @AreaClassificationKey, @OccupancyTypeKey, @AddressKey, @PropertyDescription1, @PropertyDescription2, @PropertyDescription3, @DeedsOfficeValue, @CurrentBondDate, @ErfNumber, @ErfPortionNumber, @SectionalSchemeName, @SectionalUnitNumber, @DeedsPropertyTypeKey, @ErfSuburbDescription, @ErfMetroDescription, @DataProviderKey); select cast(scope_identity() as int)";
        public const string propertydatamodel_update = "UPDATE [2am].[dbo].[Property] SET PropertyTypeKey = @PropertyTypeKey, TitleTypeKey = @TitleTypeKey, AreaClassificationKey = @AreaClassificationKey, OccupancyTypeKey = @OccupancyTypeKey, AddressKey = @AddressKey, PropertyDescription1 = @PropertyDescription1, PropertyDescription2 = @PropertyDescription2, PropertyDescription3 = @PropertyDescription3, DeedsOfficeValue = @DeedsOfficeValue, CurrentBondDate = @CurrentBondDate, ErfNumber = @ErfNumber, ErfPortionNumber = @ErfPortionNumber, SectionalSchemeName = @SectionalSchemeName, SectionalUnitNumber = @SectionalUnitNumber, DeedsPropertyTypeKey = @DeedsPropertyTypeKey, ErfSuburbDescription = @ErfSuburbDescription, ErfMetroDescription = @ErfMetroDescription, DataProviderKey = @DataProviderKey WHERE PropertyKey = @PropertyKey";



        public const string assetliabilitydatamodel_selectwhere = "SELECT AssetLiabilityKey, AssetLiabilityTypeKey, AssetLiabilitySubTypeKey, AddressKey, AssetValue, LiabilityValue, CompanyName, Cost, Date, Description FROM [2am].[dbo].[AssetLiability] WHERE";
        public const string assetliabilitydatamodel_selectbykey = "SELECT AssetLiabilityKey, AssetLiabilityTypeKey, AssetLiabilitySubTypeKey, AddressKey, AssetValue, LiabilityValue, CompanyName, Cost, Date, Description FROM [2am].[dbo].[AssetLiability] WHERE AssetLiabilityKey = @PrimaryKey";
        public const string assetliabilitydatamodel_delete = "DELETE FROM [2am].[dbo].[AssetLiability] WHERE AssetLiabilityKey = @PrimaryKey";
        public const string assetliabilitydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AssetLiability] WHERE";
        public const string assetliabilitydatamodel_insert = "INSERT INTO [2am].[dbo].[AssetLiability] (AssetLiabilityTypeKey, AssetLiabilitySubTypeKey, AddressKey, AssetValue, LiabilityValue, CompanyName, Cost, Date, Description) VALUES(@AssetLiabilityTypeKey, @AssetLiabilitySubTypeKey, @AddressKey, @AssetValue, @LiabilityValue, @CompanyName, @Cost, @Date, @Description); select cast(scope_identity() as int)";
        public const string assetliabilitydatamodel_update = "UPDATE [2am].[dbo].[AssetLiability] SET AssetLiabilityTypeKey = @AssetLiabilityTypeKey, AssetLiabilitySubTypeKey = @AssetLiabilitySubTypeKey, AddressKey = @AddressKey, AssetValue = @AssetValue, LiabilityValue = @LiabilityValue, CompanyName = @CompanyName, Cost = @Cost, Date = @Date, Description = @Description WHERE AssetLiabilityKey = @AssetLiabilityKey";



        public const string auditdisbursementdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, DisbursementKey, AccountKey, ACBBankCode, ACBBranchCode, ACBTypeNumber, PreparedDate, ActionDate, AccountName, AccountNumber, Amount, DisbursementStatusKey, DisbursementTransactionTypeKey, CapitalAmount, GuaranteeAmount, InterestRate, InterestStartDate, InterestApplied, PaymentAmount FROM [2am].[dbo].[AuditDisbursement] WHERE";
        public const string auditdisbursementdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, DisbursementKey, AccountKey, ACBBankCode, ACBBranchCode, ACBTypeNumber, PreparedDate, ActionDate, AccountName, AccountNumber, Amount, DisbursementStatusKey, DisbursementTransactionTypeKey, CapitalAmount, GuaranteeAmount, InterestRate, InterestStartDate, InterestApplied, PaymentAmount FROM [2am].[dbo].[AuditDisbursement] WHERE AuditNumber = @PrimaryKey";
        public const string auditdisbursementdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditDisbursement] WHERE AuditNumber = @PrimaryKey";
        public const string auditdisbursementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditDisbursement] WHERE";
        public const string auditdisbursementdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditDisbursement] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, DisbursementKey, AccountKey, ACBBankCode, ACBBranchCode, ACBTypeNumber, PreparedDate, ActionDate, AccountName, AccountNumber, Amount, DisbursementStatusKey, DisbursementTransactionTypeKey, CapitalAmount, GuaranteeAmount, InterestRate, InterestStartDate, InterestApplied, PaymentAmount) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @DisbursementKey, @AccountKey, @ACBBankCode, @ACBBranchCode, @ACBTypeNumber, @PreparedDate, @ActionDate, @AccountName, @AccountNumber, @Amount, @DisbursementStatusKey, @DisbursementTransactionTypeKey, @CapitalAmount, @GuaranteeAmount, @InterestRate, @InterestStartDate, @InterestApplied, @PaymentAmount); select cast(scope_identity() as int)";
        public const string auditdisbursementdatamodel_update = "UPDATE [2am].[dbo].[AuditDisbursement] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, DisbursementKey = @DisbursementKey, AccountKey = @AccountKey, ACBBankCode = @ACBBankCode, ACBBranchCode = @ACBBranchCode, ACBTypeNumber = @ACBTypeNumber, PreparedDate = @PreparedDate, ActionDate = @ActionDate, AccountName = @AccountName, AccountNumber = @AccountNumber, Amount = @Amount, DisbursementStatusKey = @DisbursementStatusKey, DisbursementTransactionTypeKey = @DisbursementTransactionTypeKey, CapitalAmount = @CapitalAmount, GuaranteeAmount = @GuaranteeAmount, InterestRate = @InterestRate, InterestStartDate = @InterestStartDate, InterestApplied = @InterestApplied, PaymentAmount = @PaymentAmount WHERE AuditNumber = @AuditNumber";



        public const string offerdeclarationquestiondatamodel_selectwhere = "SELECT OfferDeclarationQuestionKey, Description, DisplayQuestionDate, DisplaySequence FROM [2am].[dbo].[OfferDeclarationQuestion] WHERE";
        public const string offerdeclarationquestiondatamodel_selectbykey = "SELECT OfferDeclarationQuestionKey, Description, DisplayQuestionDate, DisplaySequence FROM [2am].[dbo].[OfferDeclarationQuestion] WHERE OfferDeclarationQuestionKey = @PrimaryKey";
        public const string offerdeclarationquestiondatamodel_delete = "DELETE FROM [2am].[dbo].[OfferDeclarationQuestion] WHERE OfferDeclarationQuestionKey = @PrimaryKey";
        public const string offerdeclarationquestiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferDeclarationQuestion] WHERE";
        public const string offerdeclarationquestiondatamodel_insert = "INSERT INTO [2am].[dbo].[OfferDeclarationQuestion] (OfferDeclarationQuestionKey, Description, DisplayQuestionDate, DisplaySequence) VALUES(@OfferDeclarationQuestionKey, @Description, @DisplayQuestionDate, @DisplaySequence); ";
        public const string offerdeclarationquestiondatamodel_update = "UPDATE [2am].[dbo].[OfferDeclarationQuestion] SET OfferDeclarationQuestionKey = @OfferDeclarationQuestionKey, Description = @Description, DisplayQuestionDate = @DisplayQuestionDate, DisplaySequence = @DisplaySequence WHERE OfferDeclarationQuestionKey = @OfferDeclarationQuestionKey";



        public const string debitorderdaydatamodel_selectwhere = "SELECT DebitOrderDayKey, DebitOrderDay FROM [2am].[dbo].[DebitOrderDay] WHERE";
        public const string debitorderdaydatamodel_selectbykey = "SELECT DebitOrderDayKey, DebitOrderDay FROM [2am].[dbo].[DebitOrderDay] WHERE DebitOrderDayKey = @PrimaryKey";
        public const string debitorderdaydatamodel_delete = "DELETE FROM [2am].[dbo].[DebitOrderDay] WHERE DebitOrderDayKey = @PrimaryKey";
        public const string debitorderdaydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DebitOrderDay] WHERE";
        public const string debitorderdaydatamodel_insert = "INSERT INTO [2am].[dbo].[DebitOrderDay] (DebitOrderDay) VALUES(@DebitOrderDay); select cast(scope_identity() as int)";
        public const string debitorderdaydatamodel_update = "UPDATE [2am].[dbo].[DebitOrderDay] SET DebitOrderDay = @DebitOrderDay WHERE DebitOrderDayKey = @DebitOrderDayKey";



        public const string conditiontypedatamodel_selectwhere = "SELECT ConditionTypeKey, Description FROM [2am].[dbo].[ConditionType] WHERE";
        public const string conditiontypedatamodel_selectbykey = "SELECT ConditionTypeKey, Description FROM [2am].[dbo].[ConditionType] WHERE ConditionTypeKey = @PrimaryKey";
        public const string conditiontypedatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionType] WHERE ConditionTypeKey = @PrimaryKey";
        public const string conditiontypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionType] WHERE";
        public const string conditiontypedatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string conditiontypedatamodel_update = "UPDATE [2am].[dbo].[ConditionType] SET Description = @Description WHERE ConditionTypeKey = @ConditionTypeKey";



        public const string resetdatamodel_selectwhere = "SELECT ResetKey, ResetDate, RunDate, ResetConfigurationKey, JIBARRate, JIBARDiscountRate FROM [2am].[dbo].[Reset] WHERE";
        public const string resetdatamodel_selectbykey = "SELECT ResetKey, ResetDate, RunDate, ResetConfigurationKey, JIBARRate, JIBARDiscountRate FROM [2am].[dbo].[Reset] WHERE ResetKey = @PrimaryKey";
        public const string resetdatamodel_delete = "DELETE FROM [2am].[dbo].[Reset] WHERE ResetKey = @PrimaryKey";
        public const string resetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Reset] WHERE";
        public const string resetdatamodel_insert = "INSERT INTO [2am].[dbo].[Reset] (ResetDate, RunDate, ResetConfigurationKey, JIBARRate, JIBARDiscountRate) VALUES(@ResetDate, @RunDate, @ResetConfigurationKey, @JIBARRate, @JIBARDiscountRate); select cast(scope_identity() as int)";
        public const string resetdatamodel_update = "UPDATE [2am].[dbo].[Reset] SET ResetDate = @ResetDate, RunDate = @RunDate, ResetConfigurationKey = @ResetConfigurationKey, JIBARRate = @JIBARRate, JIBARDiscountRate = @JIBARDiscountRate WHERE ResetKey = @ResetKey";



        public const string itccreditscoredatamodel_selectwhere = "SELECT ITCCreditScoreKey, ITCKey, ScoreCardKey, RiskMatrixRevisionKey, EmpiricaScore, SBCScore, RiskMatrixCellKey, CreditScoreDecisionKey, GeneralStatusKey, ScoreDate, ADUserName, LegalEntityKey FROM [2am].[dbo].[ITCCreditScore] WHERE";
        public const string itccreditscoredatamodel_selectbykey = "SELECT ITCCreditScoreKey, ITCKey, ScoreCardKey, RiskMatrixRevisionKey, EmpiricaScore, SBCScore, RiskMatrixCellKey, CreditScoreDecisionKey, GeneralStatusKey, ScoreDate, ADUserName, LegalEntityKey FROM [2am].[dbo].[ITCCreditScore] WHERE ITCCreditScoreKey = @PrimaryKey";
        public const string itccreditscoredatamodel_delete = "DELETE FROM [2am].[dbo].[ITCCreditScore] WHERE ITCCreditScoreKey = @PrimaryKey";
        public const string itccreditscoredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ITCCreditScore] WHERE";
        public const string itccreditscoredatamodel_insert = "INSERT INTO [2am].[dbo].[ITCCreditScore] (ITCKey, ScoreCardKey, RiskMatrixRevisionKey, EmpiricaScore, SBCScore, RiskMatrixCellKey, CreditScoreDecisionKey, GeneralStatusKey, ScoreDate, ADUserName, LegalEntityKey) VALUES(@ITCKey, @ScoreCardKey, @RiskMatrixRevisionKey, @EmpiricaScore, @SBCScore, @RiskMatrixCellKey, @CreditScoreDecisionKey, @GeneralStatusKey, @ScoreDate, @ADUserName, @LegalEntityKey); select cast(scope_identity() as int)";
        public const string itccreditscoredatamodel_update = "UPDATE [2am].[dbo].[ITCCreditScore] SET ITCKey = @ITCKey, ScoreCardKey = @ScoreCardKey, RiskMatrixRevisionKey = @RiskMatrixRevisionKey, EmpiricaScore = @EmpiricaScore, SBCScore = @SBCScore, RiskMatrixCellKey = @RiskMatrixCellKey, CreditScoreDecisionKey = @CreditScoreDecisionKey, GeneralStatusKey = @GeneralStatusKey, ScoreDate = @ScoreDate, ADUserName = @ADUserName, LegalEntityKey = @LegalEntityKey WHERE ITCCreditScoreKey = @ITCCreditScoreKey";



        public const string accountexternallifedatamodel_selectwhere = "SELECT AccountKey, ExternalLifePolicyKey FROM [2am].[dbo].[AccountExternalLife] WHERE";
        public const string accountexternallifedatamodel_selectbykey = "SELECT AccountKey, ExternalLifePolicyKey FROM [2am].[dbo].[AccountExternalLife] WHERE  = @PrimaryKey";
        public const string accountexternallifedatamodel_delete = "DELETE FROM [2am].[dbo].[AccountExternalLife] WHERE  = @PrimaryKey";
        public const string accountexternallifedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountExternalLife] WHERE";
        public const string accountexternallifedatamodel_insert = "INSERT INTO [2am].[dbo].[AccountExternalLife] (AccountKey, ExternalLifePolicyKey) VALUES(@AccountKey, @ExternalLifePolicyKey); ";
        public const string accountexternallifedatamodel_update = "UPDATE [2am].[dbo].[AccountExternalLife] SET AccountKey = @AccountKey, ExternalLifePolicyKey = @ExternalLifePolicyKey WHERE  = @";



        public const string operatorgroupdatamodel_selectwhere = "SELECT OperatorGroupKey, Description FROM [2am].[dbo].[OperatorGroup] WHERE";
        public const string operatorgroupdatamodel_selectbykey = "SELECT OperatorGroupKey, Description FROM [2am].[dbo].[OperatorGroup] WHERE OperatorGroupKey = @PrimaryKey";
        public const string operatorgroupdatamodel_delete = "DELETE FROM [2am].[dbo].[OperatorGroup] WHERE OperatorGroupKey = @PrimaryKey";
        public const string operatorgroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OperatorGroup] WHERE";
        public const string operatorgroupdatamodel_insert = "INSERT INTO [2am].[dbo].[OperatorGroup] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string operatorgroupdatamodel_update = "UPDATE [2am].[dbo].[OperatorGroup] SET Description = @Description WHERE OperatorGroupKey = @OperatorGroupKey";



        public const string auditroledatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, AccountKey, RoleTypeKey, AccountRoleKey, GeneralStatusKey, StatusChangeDate FROM [2am].[dbo].[AuditRole] WHERE";
        public const string auditroledatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, AccountKey, RoleTypeKey, AccountRoleKey, GeneralStatusKey, StatusChangeDate FROM [2am].[dbo].[AuditRole] WHERE AuditNumber = @PrimaryKey";
        public const string auditroledatamodel_delete = "DELETE FROM [2am].[dbo].[AuditRole] WHERE AuditNumber = @PrimaryKey";
        public const string auditroledatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditRole] WHERE";
        public const string auditroledatamodel_insert = "INSERT INTO [2am].[dbo].[AuditRole] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityKey, AccountKey, RoleTypeKey, AccountRoleKey, GeneralStatusKey, StatusChangeDate) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @LegalEntityKey, @AccountKey, @RoleTypeKey, @AccountRoleKey, @GeneralStatusKey, @StatusChangeDate); select cast(scope_identity() as int)";
        public const string auditroledatamodel_update = "UPDATE [2am].[dbo].[AuditRole] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, LegalEntityKey = @LegalEntityKey, AccountKey = @AccountKey, RoleTypeKey = @RoleTypeKey, AccountRoleKey = @AccountRoleKey, GeneralStatusKey = @GeneralStatusKey, StatusChangeDate = @StatusChangeDate WHERE AuditNumber = @AuditNumber";



        public const string quickcashfeesdatamodel_selectwhere = "SELECT FeeRange, Fee, FeePercentage FROM [2am].[dbo].[QuickCashFees] WHERE";
        public const string quickcashfeesdatamodel_selectbykey = "SELECT FeeRange, Fee, FeePercentage FROM [2am].[dbo].[QuickCashFees] WHERE FeeRange = @PrimaryKey";
        public const string quickcashfeesdatamodel_delete = "DELETE FROM [2am].[dbo].[QuickCashFees] WHERE FeeRange = @PrimaryKey";
        public const string quickcashfeesdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[QuickCashFees] WHERE";
        public const string quickcashfeesdatamodel_insert = "INSERT INTO [2am].[dbo].[QuickCashFees] (FeeRange, Fee, FeePercentage) VALUES(@FeeRange, @Fee, @FeePercentage); ";
        public const string quickcashfeesdatamodel_update = "UPDATE [2am].[dbo].[QuickCashFees] SET FeeRange = @FeeRange, Fee = @Fee, FeePercentage = @FeePercentage WHERE FeeRange = @FeeRange";



        public const string expensetypegroupdatamodel_selectwhere = "SELECT ExpenseTypeGroupKey, Description, Fee, Expense FROM [2am].[dbo].[ExpenseTypeGroup] WHERE";
        public const string expensetypegroupdatamodel_selectbykey = "SELECT ExpenseTypeGroupKey, Description, Fee, Expense FROM [2am].[dbo].[ExpenseTypeGroup] WHERE ExpenseTypeGroupKey = @PrimaryKey";
        public const string expensetypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[ExpenseTypeGroup] WHERE ExpenseTypeGroupKey = @PrimaryKey";
        public const string expensetypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ExpenseTypeGroup] WHERE";
        public const string expensetypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[ExpenseTypeGroup] (Description, Fee, Expense) VALUES(@Description, @Fee, @Expense); select cast(scope_identity() as int)";
        public const string expensetypegroupdatamodel_update = "UPDATE [2am].[dbo].[ExpenseTypeGroup] SET Description = @Description, Fee = @Fee, Expense = @Expense WHERE ExpenseTypeGroupKey = @ExpenseTypeGroupKey";



        public const string offerexpenseofferinformationquickcashdetaildatamodel_selectwhere = "SELECT OfferExpenseOfferInformationQuickCashDetailKey, OfferExpenseKey, OfferInformationQuickCashDetailKey FROM [2am].[dbo].[OfferExpenseOfferInformationQuickCashDetail] WHERE";
        public const string offerexpenseofferinformationquickcashdetaildatamodel_selectbykey = "SELECT OfferExpenseOfferInformationQuickCashDetailKey, OfferExpenseKey, OfferInformationQuickCashDetailKey FROM [2am].[dbo].[OfferExpenseOfferInformationQuickCashDetail] WHERE OfferExpenseOfferInformationQuickCashDetailKey = @PrimaryKey";
        public const string offerexpenseofferinformationquickcashdetaildatamodel_delete = "DELETE FROM [2am].[dbo].[OfferExpenseOfferInformationQuickCashDetail] WHERE OfferExpenseOfferInformationQuickCashDetailKey = @PrimaryKey";
        public const string offerexpenseofferinformationquickcashdetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferExpenseOfferInformationQuickCashDetail] WHERE";
        public const string offerexpenseofferinformationquickcashdetaildatamodel_insert = "INSERT INTO [2am].[dbo].[OfferExpenseOfferInformationQuickCashDetail] (OfferExpenseKey, OfferInformationQuickCashDetailKey) VALUES(@OfferExpenseKey, @OfferInformationQuickCashDetailKey); select cast(scope_identity() as int)";
        public const string offerexpenseofferinformationquickcashdetaildatamodel_update = "UPDATE [2am].[dbo].[OfferExpenseOfferInformationQuickCashDetail] SET OfferExpenseKey = @OfferExpenseKey, OfferInformationQuickCashDetailKey = @OfferInformationQuickCashDetailKey WHERE OfferExpenseOfferInformationQuickCashDetailKey = @OfferExpenseOfferInformationQuickCashDetailKey";



        public const string campaigntargetcontactdatamodel_selectwhere = "SELECT CampaignTargetContactKey, CampaignTargetKey, LegalEntityKey, ChangeDate, AdUserKey, CampaignTargetResponseKey FROM [2am].[dbo].[CampaignTargetContact] WHERE";
        public const string campaigntargetcontactdatamodel_selectbykey = "SELECT CampaignTargetContactKey, CampaignTargetKey, LegalEntityKey, ChangeDate, AdUserKey, CampaignTargetResponseKey FROM [2am].[dbo].[CampaignTargetContact] WHERE CampaignTargetContactKey = @PrimaryKey";
        public const string campaigntargetcontactdatamodel_delete = "DELETE FROM [2am].[dbo].[CampaignTargetContact] WHERE CampaignTargetContactKey = @PrimaryKey";
        public const string campaigntargetcontactdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CampaignTargetContact] WHERE";
        public const string campaigntargetcontactdatamodel_insert = "INSERT INTO [2am].[dbo].[CampaignTargetContact] (CampaignTargetKey, LegalEntityKey, ChangeDate, AdUserKey, CampaignTargetResponseKey) VALUES(@CampaignTargetKey, @LegalEntityKey, @ChangeDate, @AdUserKey, @CampaignTargetResponseKey); select cast(scope_identity() as int)";
        public const string campaigntargetcontactdatamodel_update = "UPDATE [2am].[dbo].[CampaignTargetContact] SET CampaignTargetKey = @CampaignTargetKey, LegalEntityKey = @LegalEntityKey, ChangeDate = @ChangeDate, AdUserKey = @AdUserKey, CampaignTargetResponseKey = @CampaignTargetResponseKey WHERE CampaignTargetContactKey = @CampaignTargetContactKey";



        public const string disbursementdatamodel_selectwhere = "SELECT DisbursementKey, AccountKey, ACBBankCode, ACBBranchCode, ACBTypeNumber, PreparedDate, ActionDate, AccountName, AccountNumber, Amount, DisbursementStatusKey, DisbursementTransactionTypeKey, CapitalAmount, GuaranteeAmount, InterestRate, InterestStartDate, InterestApplied, PaymentAmount FROM [2am].[dbo].[Disbursement] WHERE";
        public const string disbursementdatamodel_selectbykey = "SELECT DisbursementKey, AccountKey, ACBBankCode, ACBBranchCode, ACBTypeNumber, PreparedDate, ActionDate, AccountName, AccountNumber, Amount, DisbursementStatusKey, DisbursementTransactionTypeKey, CapitalAmount, GuaranteeAmount, InterestRate, InterestStartDate, InterestApplied, PaymentAmount FROM [2am].[dbo].[Disbursement] WHERE DisbursementKey = @PrimaryKey";
        public const string disbursementdatamodel_delete = "DELETE FROM [2am].[dbo].[Disbursement] WHERE DisbursementKey = @PrimaryKey";
        public const string disbursementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Disbursement] WHERE";
        public const string disbursementdatamodel_insert = "INSERT INTO [2am].[dbo].[Disbursement] (AccountKey, ACBBankCode, ACBBranchCode, ACBTypeNumber, PreparedDate, ActionDate, AccountName, AccountNumber, Amount, DisbursementStatusKey, DisbursementTransactionTypeKey, CapitalAmount, GuaranteeAmount, InterestRate, InterestStartDate, InterestApplied, PaymentAmount) VALUES(@AccountKey, @ACBBankCode, @ACBBranchCode, @ACBTypeNumber, @PreparedDate, @ActionDate, @AccountName, @AccountNumber, @Amount, @DisbursementStatusKey, @DisbursementTransactionTypeKey, @CapitalAmount, @GuaranteeAmount, @InterestRate, @InterestStartDate, @InterestApplied, @PaymentAmount); select cast(scope_identity() as int)";
        public const string disbursementdatamodel_update = "UPDATE [2am].[dbo].[Disbursement] SET AccountKey = @AccountKey, ACBBankCode = @ACBBankCode, ACBBranchCode = @ACBBranchCode, ACBTypeNumber = @ACBTypeNumber, PreparedDate = @PreparedDate, ActionDate = @ActionDate, AccountName = @AccountName, AccountNumber = @AccountNumber, Amount = @Amount, DisbursementStatusKey = @DisbursementStatusKey, DisbursementTransactionTypeKey = @DisbursementTransactionTypeKey, CapitalAmount = @CapitalAmount, GuaranteeAmount = @GuaranteeAmount, InterestRate = @InterestRate, InterestStartDate = @InterestStartDate, InterestApplied = @InterestApplied, PaymentAmount = @PaymentAmount WHERE DisbursementKey = @DisbursementKey";



        public const string correspondencetemplatedatamodel_selectwhere = "SELECT CorrespondenceTemplateKey, Name, Subject, Template, ContentTypeKey, DefaultEmail FROM [2am].[dbo].[CorrespondenceTemplate] WHERE";
        public const string correspondencetemplatedatamodel_selectbykey = "SELECT CorrespondenceTemplateKey, Name, Subject, Template, ContentTypeKey, DefaultEmail FROM [2am].[dbo].[CorrespondenceTemplate] WHERE CorrespondenceTemplateKey = @PrimaryKey";
        public const string correspondencetemplatedatamodel_delete = "DELETE FROM [2am].[dbo].[CorrespondenceTemplate] WHERE CorrespondenceTemplateKey = @PrimaryKey";
        public const string correspondencetemplatedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CorrespondenceTemplate] WHERE";
        public const string correspondencetemplatedatamodel_insert = "INSERT INTO [2am].[dbo].[CorrespondenceTemplate] (CorrespondenceTemplateKey, Name, Subject, Template, ContentTypeKey, DefaultEmail) VALUES(@CorrespondenceTemplateKey, @Name, @Subject, @Template, @ContentTypeKey, @DefaultEmail); ";
        public const string correspondencetemplatedatamodel_update = "UPDATE [2am].[dbo].[CorrespondenceTemplate] SET CorrespondenceTemplateKey = @CorrespondenceTemplateKey, Name = @Name, Subject = @Subject, Template = @Template, ContentTypeKey = @ContentTypeKey, DefaultEmail = @DefaultEmail WHERE CorrespondenceTemplateKey = @CorrespondenceTemplateKey";



        public const string reasontypegroupdatamodel_selectwhere = "SELECT ReasonTypeGroupKey, Description, ParentKey FROM [2am].[dbo].[ReasonTypeGroup] WHERE";
        public const string reasontypegroupdatamodel_selectbykey = "SELECT ReasonTypeGroupKey, Description, ParentKey FROM [2am].[dbo].[ReasonTypeGroup] WHERE ReasonTypeGroupKey = @PrimaryKey";
        public const string reasontypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[ReasonTypeGroup] WHERE ReasonTypeGroupKey = @PrimaryKey";
        public const string reasontypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReasonTypeGroup] WHERE";
        public const string reasontypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[ReasonTypeGroup] (ReasonTypeGroupKey, Description, ParentKey) VALUES(@ReasonTypeGroupKey, @Description, @ParentKey); ";
        public const string reasontypegroupdatamodel_update = "UPDATE [2am].[dbo].[ReasonTypeGroup] SET ReasonTypeGroupKey = @ReasonTypeGroupKey, Description = @Description, ParentKey = @ParentKey WHERE ReasonTypeGroupKey = @ReasonTypeGroupKey";



        public const string generickeytypeparameterdatamodel_selectwhere = "SELECT GenericKeyTypeParameterKey, GenericKeyTypeKey, ParameterName, ParameterTypeKey FROM [2am].[dbo].[GenericKeyTypeParameter] WHERE";
        public const string generickeytypeparameterdatamodel_selectbykey = "SELECT GenericKeyTypeParameterKey, GenericKeyTypeKey, ParameterName, ParameterTypeKey FROM [2am].[dbo].[GenericKeyTypeParameter] WHERE GenericKeyTypeParameterKey = @PrimaryKey";
        public const string generickeytypeparameterdatamodel_delete = "DELETE FROM [2am].[dbo].[GenericKeyTypeParameter] WHERE GenericKeyTypeParameterKey = @PrimaryKey";
        public const string generickeytypeparameterdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[GenericKeyTypeParameter] WHERE";
        public const string generickeytypeparameterdatamodel_insert = "INSERT INTO [2am].[dbo].[GenericKeyTypeParameter] (GenericKeyTypeKey, ParameterName, ParameterTypeKey) VALUES(@GenericKeyTypeKey, @ParameterName, @ParameterTypeKey); select cast(scope_identity() as int)";
        public const string generickeytypeparameterdatamodel_update = "UPDATE [2am].[dbo].[GenericKeyTypeParameter] SET GenericKeyTypeKey = @GenericKeyTypeKey, ParameterName = @ParameterName, ParameterTypeKey = @ParameterTypeKey WHERE GenericKeyTypeParameterKey = @GenericKeyTypeParameterKey";



        public const string propertydatadatamodel_selectwhere = "SELECT PropertyDataKey, PropertyKey, PropertyDataProviderDataServiceKey, PropertyID, Data, InsertDate FROM [2am].[dbo].[PropertyData] WHERE";
        public const string propertydatadatamodel_selectbykey = "SELECT PropertyDataKey, PropertyKey, PropertyDataProviderDataServiceKey, PropertyID, Data, InsertDate FROM [2am].[dbo].[PropertyData] WHERE PropertyDataKey = @PrimaryKey";
        public const string propertydatadatamodel_delete = "DELETE FROM [2am].[dbo].[PropertyData] WHERE PropertyDataKey = @PrimaryKey";
        public const string propertydatadatamodel_deletewhere = "DELETE FROM [2am].[dbo].[PropertyData] WHERE";
        public const string propertydatadatamodel_insert = "INSERT INTO [2am].[dbo].[PropertyData] (PropertyKey, PropertyDataProviderDataServiceKey, PropertyID, Data, InsertDate) VALUES(@PropertyKey, @PropertyDataProviderDataServiceKey, @PropertyID, @Data, @InsertDate); select cast(scope_identity() as int)";
        public const string propertydatadatamodel_update = "UPDATE [2am].[dbo].[PropertyData] SET PropertyKey = @PropertyKey, PropertyDataProviderDataServiceKey = @PropertyDataProviderDataServiceKey, PropertyID = @PropertyID, Data = @Data, InsertDate = @InsertDate WHERE PropertyDataKey = @PropertyDataKey";



        public const string operatordatamodel_selectwhere = "SELECT OperatorKey, Description, OperatorGroupKey FROM [2am].[dbo].[Operator] WHERE";
        public const string operatordatamodel_selectbykey = "SELECT OperatorKey, Description, OperatorGroupKey FROM [2am].[dbo].[Operator] WHERE OperatorKey = @PrimaryKey";
        public const string operatordatamodel_delete = "DELETE FROM [2am].[dbo].[Operator] WHERE OperatorKey = @PrimaryKey";
        public const string operatordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Operator] WHERE";
        public const string operatordatamodel_insert = "INSERT INTO [2am].[dbo].[Operator] (Description, OperatorGroupKey) VALUES(@Description, @OperatorGroupKey); select cast(scope_identity() as int)";
        public const string operatordatamodel_update = "UPDATE [2am].[dbo].[Operator] SET Description = @Description, OperatorGroupKey = @OperatorGroupKey WHERE OperatorKey = @OperatorKey";



        public const string externalroledeclarationdatamodel_selectwhere = "SELECT ExternalRoleDeclarationKey, ExternalRoleKey, OfferDeclarationQuestionKey, OfferDeclarationAnswerKey, ExternalRoleDeclarationDate FROM [2am].[dbo].[ExternalRoleDeclaration] WHERE";
        public const string externalroledeclarationdatamodel_selectbykey = "SELECT ExternalRoleDeclarationKey, ExternalRoleKey, OfferDeclarationQuestionKey, OfferDeclarationAnswerKey, ExternalRoleDeclarationDate FROM [2am].[dbo].[ExternalRoleDeclaration] WHERE ExternalRoleDeclarationKey = @PrimaryKey";
        public const string externalroledeclarationdatamodel_delete = "DELETE FROM [2am].[dbo].[ExternalRoleDeclaration] WHERE ExternalRoleDeclarationKey = @PrimaryKey";
        public const string externalroledeclarationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ExternalRoleDeclaration] WHERE";
        public const string externalroledeclarationdatamodel_insert = "INSERT INTO [2am].[dbo].[ExternalRoleDeclaration] (ExternalRoleKey, OfferDeclarationQuestionKey, OfferDeclarationAnswerKey, ExternalRoleDeclarationDate) VALUES(@ExternalRoleKey, @OfferDeclarationQuestionKey, @OfferDeclarationAnswerKey, @ExternalRoleDeclarationDate); select cast(scope_identity() as int)";
        public const string externalroledeclarationdatamodel_update = "UPDATE [2am].[dbo].[ExternalRoleDeclaration] SET ExternalRoleKey = @ExternalRoleKey, OfferDeclarationQuestionKey = @OfferDeclarationQuestionKey, OfferDeclarationAnswerKey = @OfferDeclarationAnswerKey, ExternalRoleDeclarationDate = @ExternalRoleDeclarationDate WHERE ExternalRoleDeclarationKey = @ExternalRoleDeclarationKey";



        public const string tokenconditiondatamodel_selectwhere = "SELECT TokenKey, Token, StatementName, ApplicationName FROM [2am].[dbo].[TokenCondition] WHERE";
        public const string tokenconditiondatamodel_selectbykey = "SELECT TokenKey, Token, StatementName, ApplicationName FROM [2am].[dbo].[TokenCondition] WHERE TokenKey = @PrimaryKey";
        public const string tokenconditiondatamodel_delete = "DELETE FROM [2am].[dbo].[TokenCondition] WHERE TokenKey = @PrimaryKey";
        public const string tokenconditiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TokenCondition] WHERE";
        public const string tokenconditiondatamodel_insert = "INSERT INTO [2am].[dbo].[TokenCondition] (Token, StatementName, ApplicationName) VALUES(@Token, @StatementName, @ApplicationName); select cast(scope_identity() as int)";
        public const string tokenconditiondatamodel_update = "UPDATE [2am].[dbo].[TokenCondition] SET Token = @Token, StatementName = @StatementName, ApplicationName = @ApplicationName WHERE TokenKey = @TokenKey";



        public const string legalentityassetliabilitydatamodel_selectwhere = "SELECT LegalEntityAssetLiabilityKey, LegalEntityKey, AssetLiabilityKey, GeneralStatusKey FROM [2am].[dbo].[LegalEntityAssetLiability] WHERE";
        public const string legalentityassetliabilitydatamodel_selectbykey = "SELECT LegalEntityAssetLiabilityKey, LegalEntityKey, AssetLiabilityKey, GeneralStatusKey FROM [2am].[dbo].[LegalEntityAssetLiability] WHERE LegalEntityAssetLiabilityKey = @PrimaryKey";
        public const string legalentityassetliabilitydatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityAssetLiability] WHERE LegalEntityAssetLiabilityKey = @PrimaryKey";
        public const string legalentityassetliabilitydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityAssetLiability] WHERE";
        public const string legalentityassetliabilitydatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityAssetLiability] (LegalEntityKey, AssetLiabilityKey, GeneralStatusKey) VALUES(@LegalEntityKey, @AssetLiabilityKey, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string legalentityassetliabilitydatamodel_update = "UPDATE [2am].[dbo].[LegalEntityAssetLiability] SET LegalEntityKey = @LegalEntityKey, AssetLiabilityKey = @AssetLiabilityKey, GeneralStatusKey = @GeneralStatusKey WHERE LegalEntityAssetLiabilityKey = @LegalEntityAssetLiabilityKey";



        public const string offerinformationtypedatamodel_selectwhere = "SELECT OfferInformationTypeKey, Description FROM [2am].[dbo].[OfferInformationType] WHERE";
        public const string offerinformationtypedatamodel_selectbykey = "SELECT OfferInformationTypeKey, Description FROM [2am].[dbo].[OfferInformationType] WHERE OfferInformationTypeKey = @PrimaryKey";
        public const string offerinformationtypedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformationType] WHERE OfferInformationTypeKey = @PrimaryKey";
        public const string offerinformationtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformationType] WHERE";
        public const string offerinformationtypedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformationType] (OfferInformationTypeKey, Description) VALUES(@OfferInformationTypeKey, @Description); ";
        public const string offerinformationtypedatamodel_update = "UPDATE [2am].[dbo].[OfferInformationType] SET OfferInformationTypeKey = @OfferInformationTypeKey, Description = @Description WHERE OfferInformationTypeKey = @OfferInformationTypeKey";



        public const string reportformattypedatamodel_selectwhere = "SELECT ReportFormatTypeKey, Description, ReportServicesFormatType, FileExtension, ContentType, DisplayOrder FROM [2am].[dbo].[ReportFormatType] WHERE";
        public const string reportformattypedatamodel_selectbykey = "SELECT ReportFormatTypeKey, Description, ReportServicesFormatType, FileExtension, ContentType, DisplayOrder FROM [2am].[dbo].[ReportFormatType] WHERE ReportFormatTypeKey = @PrimaryKey";
        public const string reportformattypedatamodel_delete = "DELETE FROM [2am].[dbo].[ReportFormatType] WHERE ReportFormatTypeKey = @PrimaryKey";
        public const string reportformattypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReportFormatType] WHERE";
        public const string reportformattypedatamodel_insert = "INSERT INTO [2am].[dbo].[ReportFormatType] (ReportFormatTypeKey, Description, ReportServicesFormatType, FileExtension, ContentType, DisplayOrder) VALUES(@ReportFormatTypeKey, @Description, @ReportServicesFormatType, @FileExtension, @ContentType, @DisplayOrder); ";
        public const string reportformattypedatamodel_update = "UPDATE [2am].[dbo].[ReportFormatType] SET ReportFormatTypeKey = @ReportFormatTypeKey, Description = @Description, ReportServicesFormatType = @ReportServicesFormatType, FileExtension = @FileExtension, ContentType = @ContentType, DisplayOrder = @DisplayOrder WHERE ReportFormatTypeKey = @ReportFormatTypeKey";



        public const string productcategorydatamodel_selectwhere = "SELECT ProductCategoryKey, OriginationSourceProductKey, MarginKey, CategoryKey FROM [2am].[dbo].[ProductCategory] WHERE";
        public const string productcategorydatamodel_selectbykey = "SELECT ProductCategoryKey, OriginationSourceProductKey, MarginKey, CategoryKey FROM [2am].[dbo].[ProductCategory] WHERE ProductCategoryKey = @PrimaryKey";
        public const string productcategorydatamodel_delete = "DELETE FROM [2am].[dbo].[ProductCategory] WHERE ProductCategoryKey = @PrimaryKey";
        public const string productcategorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ProductCategory] WHERE";
        public const string productcategorydatamodel_insert = "INSERT INTO [2am].[dbo].[ProductCategory] (OriginationSourceProductKey, MarginKey, CategoryKey) VALUES(@OriginationSourceProductKey, @MarginKey, @CategoryKey); select cast(scope_identity() as int)";
        public const string productcategorydatamodel_update = "UPDATE [2am].[dbo].[ProductCategory] SET OriginationSourceProductKey = @OriginationSourceProductKey, MarginKey = @MarginKey, CategoryKey = @CategoryKey WHERE ProductCategoryKey = @ProductCategoryKey";



        public const string offerexternallifedatamodel_selectwhere = "SELECT OfferKey, ExternalLifePolicyKey FROM [2am].[dbo].[OfferExternalLife] WHERE";
        public const string offerexternallifedatamodel_selectbykey = "SELECT OfferKey, ExternalLifePolicyKey FROM [2am].[dbo].[OfferExternalLife] WHERE  = @PrimaryKey";
        public const string offerexternallifedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferExternalLife] WHERE  = @PrimaryKey";
        public const string offerexternallifedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferExternalLife] WHERE";
        public const string offerexternallifedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferExternalLife] (OfferKey, ExternalLifePolicyKey) VALUES(@OfferKey, @ExternalLifePolicyKey); ";
        public const string offerexternallifedatamodel_update = "UPDATE [2am].[dbo].[OfferExternalLife] SET OfferKey = @OfferKey, ExternalLifePolicyKey = @ExternalLifePolicyKey WHERE  = @";



        public const string auditemployerdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, EmployerKey, Name, TelephoneNumber, TelephoneCode, ContactPerson, ContactEmail, AccountantName, AccountantContactPerson, AccountantTelephoneCode, AccountantTelephoneNumber, AccountantEmail, EmployerBusinessTypeKey, UserID, ChangeDate, EmploymentSectorKey FROM [2am].[dbo].[AuditEmployer] WHERE";
        public const string auditemployerdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, EmployerKey, Name, TelephoneNumber, TelephoneCode, ContactPerson, ContactEmail, AccountantName, AccountantContactPerson, AccountantTelephoneCode, AccountantTelephoneNumber, AccountantEmail, EmployerBusinessTypeKey, UserID, ChangeDate, EmploymentSectorKey FROM [2am].[dbo].[AuditEmployer] WHERE AuditNumber = @PrimaryKey";
        public const string auditemployerdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditEmployer] WHERE AuditNumber = @PrimaryKey";
        public const string auditemployerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditEmployer] WHERE";
        public const string auditemployerdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditEmployer] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, EmployerKey, Name, TelephoneNumber, TelephoneCode, ContactPerson, ContactEmail, AccountantName, AccountantContactPerson, AccountantTelephoneCode, AccountantTelephoneNumber, AccountantEmail, EmployerBusinessTypeKey, UserID, ChangeDate, EmploymentSectorKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @EmployerKey, @Name, @TelephoneNumber, @TelephoneCode, @ContactPerson, @ContactEmail, @AccountantName, @AccountantContactPerson, @AccountantTelephoneCode, @AccountantTelephoneNumber, @AccountantEmail, @EmployerBusinessTypeKey, @UserID, @ChangeDate, @EmploymentSectorKey); select cast(scope_identity() as int)";
        public const string auditemployerdatamodel_update = "UPDATE [2am].[dbo].[AuditEmployer] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, EmployerKey = @EmployerKey, Name = @Name, TelephoneNumber = @TelephoneNumber, TelephoneCode = @TelephoneCode, ContactPerson = @ContactPerson, ContactEmail = @ContactEmail, AccountantName = @AccountantName, AccountantContactPerson = @AccountantContactPerson, AccountantTelephoneCode = @AccountantTelephoneCode, AccountantTelephoneNumber = @AccountantTelephoneNumber, AccountantEmail = @AccountantEmail, EmployerBusinessTypeKey = @EmployerBusinessTypeKey, UserID = @UserID, ChangeDate = @ChangeDate, EmploymentSectorKey = @EmploymentSectorKey WHERE AuditNumber = @AuditNumber";



        public const string deedspropertytypedatamodel_selectwhere = "SELECT DeedsPropertyTypeKey, Description FROM [2am].[dbo].[DeedsPropertyType] WHERE";
        public const string deedspropertytypedatamodel_selectbykey = "SELECT DeedsPropertyTypeKey, Description FROM [2am].[dbo].[DeedsPropertyType] WHERE DeedsPropertyTypeKey = @PrimaryKey";
        public const string deedspropertytypedatamodel_delete = "DELETE FROM [2am].[dbo].[DeedsPropertyType] WHERE DeedsPropertyTypeKey = @PrimaryKey";
        public const string deedspropertytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DeedsPropertyType] WHERE";
        public const string deedspropertytypedatamodel_insert = "INSERT INTO [2am].[dbo].[DeedsPropertyType] (DeedsPropertyTypeKey, Description) VALUES(@DeedsPropertyTypeKey, @Description); ";
        public const string deedspropertytypedatamodel_update = "UPDATE [2am].[dbo].[DeedsPropertyType] SET DeedsPropertyTypeKey = @DeedsPropertyTypeKey, Description = @Description WHERE DeedsPropertyTypeKey = @DeedsPropertyTypeKey";



        public const string itcdatamodel_selectwhere = "SELECT ITCKey, LegalEntityKey, AccountKey, ChangeDate, ResponseXML, ResponseStatus, UserID, RequestXML FROM [2am].[dbo].[ITC] WHERE";
        public const string itcdatamodel_selectbykey = "SELECT ITCKey, LegalEntityKey, AccountKey, ChangeDate, ResponseXML, ResponseStatus, UserID, RequestXML FROM [2am].[dbo].[ITC] WHERE ITCKey = @PrimaryKey";
        public const string itcdatamodel_delete = "DELETE FROM [2am].[dbo].[ITC] WHERE ITCKey = @PrimaryKey";
        public const string itcdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ITC] WHERE";
        public const string itcdatamodel_insert = "INSERT INTO [2am].[dbo].[ITC] (LegalEntityKey, AccountKey, ChangeDate, ResponseXML, ResponseStatus, UserID, RequestXML) VALUES(@LegalEntityKey, @AccountKey, @ChangeDate, @ResponseXML, @ResponseStatus, @UserID, @RequestXML); select cast(scope_identity() as int)";
        public const string itcdatamodel_update = "UPDATE [2am].[dbo].[ITC] SET LegalEntityKey = @LegalEntityKey, AccountKey = @AccountKey, ChangeDate = @ChangeDate, ResponseXML = @ResponseXML, ResponseStatus = @ResponseStatus, UserID = @UserID, RequestXML = @RequestXML WHERE ITCKey = @ITCKey";



        public const string accountpropertydatamodel_selectwhere = "SELECT AccountPropertyKey, AccountKey, PropertyKey FROM [2am].[dbo].[AccountProperty] WHERE";
        public const string accountpropertydatamodel_selectbykey = "SELECT AccountPropertyKey, AccountKey, PropertyKey FROM [2am].[dbo].[AccountProperty] WHERE AccountPropertyKey = @PrimaryKey";
        public const string accountpropertydatamodel_delete = "DELETE FROM [2am].[dbo].[AccountProperty] WHERE AccountPropertyKey = @PrimaryKey";
        public const string accountpropertydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountProperty] WHERE";
        public const string accountpropertydatamodel_insert = "INSERT INTO [2am].[dbo].[AccountProperty] (AccountKey, PropertyKey) VALUES(@AccountKey, @PropertyKey); select cast(scope_identity() as int)";
        public const string accountpropertydatamodel_update = "UPDATE [2am].[dbo].[AccountProperty] SET AccountKey = @AccountKey, PropertyKey = @PropertyKey WHERE AccountPropertyKey = @AccountPropertyKey";



        public const string offermarketingsurveydatamodel_selectwhere = "SELECT OfferMarketingSurveyKey, OfferKey, OfferMarketingSurveyTypeKey FROM [2am].[dbo].[OfferMarketingSurvey] WHERE";
        public const string offermarketingsurveydatamodel_selectbykey = "SELECT OfferMarketingSurveyKey, OfferKey, OfferMarketingSurveyTypeKey FROM [2am].[dbo].[OfferMarketingSurvey] WHERE OfferMarketingSurveyKey = @PrimaryKey";
        public const string offermarketingsurveydatamodel_delete = "DELETE FROM [2am].[dbo].[OfferMarketingSurvey] WHERE OfferMarketingSurveyKey = @PrimaryKey";
        public const string offermarketingsurveydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferMarketingSurvey] WHERE";
        public const string offermarketingsurveydatamodel_insert = "INSERT INTO [2am].[dbo].[OfferMarketingSurvey] (OfferKey, OfferMarketingSurveyTypeKey) VALUES(@OfferKey, @OfferMarketingSurveyTypeKey); select cast(scope_identity() as int)";
        public const string offermarketingsurveydatamodel_update = "UPDATE [2am].[dbo].[OfferMarketingSurvey] SET OfferKey = @OfferKey, OfferMarketingSurveyTypeKey = @OfferMarketingSurveyTypeKey WHERE OfferMarketingSurveyKey = @OfferMarketingSurveyKey";



        public const string thirdpartypaymentbankaccountdatamodel_selectwhere = "SELECT ThirdPartyPaymentBankAccountKey, Id, BankAccountKey, ThirdPartyKey, GeneralStatusKey, BeneficiaryBankCode, EmailAddress FROM [2am].[dbo].[ThirdPartyPaymentBankAccount] WHERE";
        public const string thirdpartypaymentbankaccountdatamodel_selectbykey = "SELECT ThirdPartyPaymentBankAccountKey, Id, BankAccountKey, ThirdPartyKey, GeneralStatusKey, BeneficiaryBankCode, EmailAddress FROM [2am].[dbo].[ThirdPartyPaymentBankAccount] WHERE ThirdPartyPaymentBankAccountKey = @PrimaryKey";
        public const string thirdpartypaymentbankaccountdatamodel_delete = "DELETE FROM [2am].[dbo].[ThirdPartyPaymentBankAccount] WHERE ThirdPartyPaymentBankAccountKey = @PrimaryKey";
        public const string thirdpartypaymentbankaccountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ThirdPartyPaymentBankAccount] WHERE";
        public const string thirdpartypaymentbankaccountdatamodel_insert = "INSERT INTO [2am].[dbo].[ThirdPartyPaymentBankAccount] (Id, BankAccountKey, ThirdPartyKey, GeneralStatusKey, BeneficiaryBankCode, EmailAddress) VALUES(@Id, @BankAccountKey, @ThirdPartyKey, @GeneralStatusKey, @BeneficiaryBankCode, @EmailAddress); select cast(scope_identity() as int)";
        public const string thirdpartypaymentbankaccountdatamodel_update = "UPDATE [2am].[dbo].[ThirdPartyPaymentBankAccount] SET Id = @Id, BankAccountKey = @BankAccountKey, ThirdPartyKey = @ThirdPartyKey, GeneralStatusKey = @GeneralStatusKey, BeneficiaryBankCode = @BeneficiaryBankCode, EmailAddress = @EmailAddress WHERE ThirdPartyPaymentBankAccountKey = @ThirdPartyPaymentBankAccountKey";



        public const string allocationmandateoperatordatamodel_selectwhere = "SELECT AllocationMandateOperatorKey, AllocationMandateSetKey, AllocationMandateKey, [Order], OperatorKey FROM [2am].[dbo].[AllocationMandateOperator] WHERE";
        public const string allocationmandateoperatordatamodel_selectbykey = "SELECT AllocationMandateOperatorKey, AllocationMandateSetKey, AllocationMandateKey, [Order], OperatorKey FROM [2am].[dbo].[AllocationMandateOperator] WHERE AllocationMandateOperatorKey = @PrimaryKey";
        public const string allocationmandateoperatordatamodel_delete = "DELETE FROM [2am].[dbo].[AllocationMandateOperator] WHERE AllocationMandateOperatorKey = @PrimaryKey";
        public const string allocationmandateoperatordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AllocationMandateOperator] WHERE";
        public const string allocationmandateoperatordatamodel_insert = "INSERT INTO [2am].[dbo].[AllocationMandateOperator] (AllocationMandateSetKey, AllocationMandateKey, [Order], OperatorKey) VALUES(@AllocationMandateSetKey, @AllocationMandateKey, @Order, @OperatorKey); select cast(scope_identity() as int)";
        public const string allocationmandateoperatordatamodel_update = "UPDATE [2am].[dbo].[AllocationMandateOperator] SET AllocationMandateSetKey = @AllocationMandateSetKey, AllocationMandateKey = @AllocationMandateKey, [Order] = @Order, OperatorKey = @OperatorKey WHERE AllocationMandateOperatorKey = @AllocationMandateOperatorKey";



        public const string bondregistrationrangedatamodel_selectwhere = "SELECT BondRegistrationRangeKey, OriginationSourceProductKey, Range, MinimumBond FROM [2am].[dbo].[BondRegistrationRange] WHERE";
        public const string bondregistrationrangedatamodel_selectbykey = "SELECT BondRegistrationRangeKey, OriginationSourceProductKey, Range, MinimumBond FROM [2am].[dbo].[BondRegistrationRange] WHERE BondRegistrationRangeKey = @PrimaryKey";
        public const string bondregistrationrangedatamodel_delete = "DELETE FROM [2am].[dbo].[BondRegistrationRange] WHERE BondRegistrationRangeKey = @PrimaryKey";
        public const string bondregistrationrangedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BondRegistrationRange] WHERE";
        public const string bondregistrationrangedatamodel_insert = "INSERT INTO [2am].[dbo].[BondRegistrationRange] (OriginationSourceProductKey, Range, MinimumBond) VALUES(@OriginationSourceProductKey, @Range, @MinimumBond); select cast(scope_identity() as int)";
        public const string bondregistrationrangedatamodel_update = "UPDATE [2am].[dbo].[BondRegistrationRange] SET OriginationSourceProductKey = @OriginationSourceProductKey, Range = @Range, MinimumBond = @MinimumBond WHERE BondRegistrationRangeKey = @BondRegistrationRangeKey";



        public const string stagedefinitionstagedefinitiongroupdatamodel_selectwhere = "SELECT StageDefinitionStageDefinitionGroupKey, StageDefinitionGroupKey, StageDefinitionKey FROM [2am].[dbo].[StageDefinitionStageDefinitionGroup] WHERE";
        public const string stagedefinitionstagedefinitiongroupdatamodel_selectbykey = "SELECT StageDefinitionStageDefinitionGroupKey, StageDefinitionGroupKey, StageDefinitionKey FROM [2am].[dbo].[StageDefinitionStageDefinitionGroup] WHERE StageDefinitionStageDefinitionGroupKey = @PrimaryKey";
        public const string stagedefinitionstagedefinitiongroupdatamodel_delete = "DELETE FROM [2am].[dbo].[StageDefinitionStageDefinitionGroup] WHERE StageDefinitionStageDefinitionGroupKey = @PrimaryKey";
        public const string stagedefinitionstagedefinitiongroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StageDefinitionStageDefinitionGroup] WHERE";
        public const string stagedefinitionstagedefinitiongroupdatamodel_insert = "INSERT INTO [2am].[dbo].[StageDefinitionStageDefinitionGroup] (StageDefinitionStageDefinitionGroupKey, StageDefinitionGroupKey, StageDefinitionKey) VALUES(@StageDefinitionStageDefinitionGroupKey, @StageDefinitionGroupKey, @StageDefinitionKey); ";
        public const string stagedefinitionstagedefinitiongroupdatamodel_update = "UPDATE [2am].[dbo].[StageDefinitionStageDefinitionGroup] SET StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey, StageDefinitionGroupKey = @StageDefinitionGroupKey, StageDefinitionKey = @StageDefinitionKey WHERE StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey";



        public const string offermarketingsurveytypedatamodel_selectwhere = "SELECT OfferMarketingSurveyTypeKey, Description, OfferMarketingSurveyTypeGroupKey FROM [2am].[dbo].[OfferMarketingSurveyType] WHERE";
        public const string offermarketingsurveytypedatamodel_selectbykey = "SELECT OfferMarketingSurveyTypeKey, Description, OfferMarketingSurveyTypeGroupKey FROM [2am].[dbo].[OfferMarketingSurveyType] WHERE OfferMarketingSurveyTypeKey = @PrimaryKey";
        public const string offermarketingsurveytypedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferMarketingSurveyType] WHERE OfferMarketingSurveyTypeKey = @PrimaryKey";
        public const string offermarketingsurveytypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferMarketingSurveyType] WHERE";
        public const string offermarketingsurveytypedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferMarketingSurveyType] (Description, OfferMarketingSurveyTypeGroupKey) VALUES(@Description, @OfferMarketingSurveyTypeGroupKey); select cast(scope_identity() as int)";
        public const string offermarketingsurveytypedatamodel_update = "UPDATE [2am].[dbo].[OfferMarketingSurveyType] SET Description = @Description, OfferMarketingSurveyTypeGroupKey = @OfferMarketingSurveyTypeGroupKey WHERE OfferMarketingSurveyTypeKey = @OfferMarketingSurveyTypeKey";



        public const string offerroleattributetypedatamodel_selectwhere = "SELECT OfferRoleAttributeTypeKey, Description FROM [2am].[dbo].[OfferRoleAttributeType] WHERE";
        public const string offerroleattributetypedatamodel_selectbykey = "SELECT OfferRoleAttributeTypeKey, Description FROM [2am].[dbo].[OfferRoleAttributeType] WHERE OfferRoleAttributeTypeKey = @PrimaryKey";
        public const string offerroleattributetypedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferRoleAttributeType] WHERE OfferRoleAttributeTypeKey = @PrimaryKey";
        public const string offerroleattributetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferRoleAttributeType] WHERE";
        public const string offerroleattributetypedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferRoleAttributeType] (OfferRoleAttributeTypeKey, Description) VALUES(@OfferRoleAttributeTypeKey, @Description); ";
        public const string offerroleattributetypedatamodel_update = "UPDATE [2am].[dbo].[OfferRoleAttributeType] SET OfferRoleAttributeTypeKey = @OfferRoleAttributeTypeKey, Description = @Description WHERE OfferRoleAttributeTypeKey = @OfferRoleAttributeTypeKey";



        public const string bondmortgageloandatamodel_selectwhere = "SELECT BondMortgageLoanKey, FinancialServiceKey, BondKey FROM [2am].[dbo].[BondMortgageLoan] WHERE";
        public const string bondmortgageloandatamodel_selectbykey = "SELECT BondMortgageLoanKey, FinancialServiceKey, BondKey FROM [2am].[dbo].[BondMortgageLoan] WHERE BondMortgageLoanKey = @PrimaryKey";
        public const string bondmortgageloandatamodel_delete = "DELETE FROM [2am].[dbo].[BondMortgageLoan] WHERE BondMortgageLoanKey = @PrimaryKey";
        public const string bondmortgageloandatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BondMortgageLoan] WHERE";
        public const string bondmortgageloandatamodel_insert = "INSERT INTO [2am].[dbo].[BondMortgageLoan] (FinancialServiceKey, BondKey) VALUES(@FinancialServiceKey, @BondKey); select cast(scope_identity() as int)";
        public const string bondmortgageloandatamodel_update = "UPDATE [2am].[dbo].[BondMortgageLoan] SET FinancialServiceKey = @FinancialServiceKey, BondKey = @BondKey WHERE BondMortgageLoanKey = @BondMortgageLoanKey";



        public const string callingcontexttypedatamodel_selectwhere = "SELECT CallingContextTypeKey, Description FROM [2am].[dbo].[CallingContextType] WHERE";
        public const string callingcontexttypedatamodel_selectbykey = "SELECT CallingContextTypeKey, Description FROM [2am].[dbo].[CallingContextType] WHERE CallingContextTypeKey = @PrimaryKey";
        public const string callingcontexttypedatamodel_delete = "DELETE FROM [2am].[dbo].[CallingContextType] WHERE CallingContextTypeKey = @PrimaryKey";
        public const string callingcontexttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CallingContextType] WHERE";
        public const string callingcontexttypedatamodel_insert = "INSERT INTO [2am].[dbo].[CallingContextType] (CallingContextTypeKey, Description) VALUES(@CallingContextTypeKey, @Description); ";
        public const string callingcontexttypedatamodel_update = "UPDATE [2am].[dbo].[CallingContextType] SET CallingContextTypeKey = @CallingContextTypeKey, Description = @Description WHERE CallingContextTypeKey = @CallingContextTypeKey";



        public const string auditsubsidydatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, SubsidyKey, SubsidyProviderKey, EmploymentKey, LegalEntityKey, SalaryNumber, Paypoint, Notch, Rank, GeneralStatusKey, StopOrderAmount, GEPFMember FROM [2am].[dbo].[AuditSubsidy] WHERE";
        public const string auditsubsidydatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, SubsidyKey, SubsidyProviderKey, EmploymentKey, LegalEntityKey, SalaryNumber, Paypoint, Notch, Rank, GeneralStatusKey, StopOrderAmount, GEPFMember FROM [2am].[dbo].[AuditSubsidy] WHERE AuditNumber = @PrimaryKey";
        public const string auditsubsidydatamodel_delete = "DELETE FROM [2am].[dbo].[AuditSubsidy] WHERE AuditNumber = @PrimaryKey";
        public const string auditsubsidydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditSubsidy] WHERE";
        public const string auditsubsidydatamodel_insert = "INSERT INTO [2am].[dbo].[AuditSubsidy] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, SubsidyKey, SubsidyProviderKey, EmploymentKey, LegalEntityKey, SalaryNumber, Paypoint, Notch, Rank, GeneralStatusKey, StopOrderAmount, GEPFMember) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @SubsidyKey, @SubsidyProviderKey, @EmploymentKey, @LegalEntityKey, @SalaryNumber, @Paypoint, @Notch, @Rank, @GeneralStatusKey, @StopOrderAmount, @GEPFMember); select cast(scope_identity() as int)";
        public const string auditsubsidydatamodel_update = "UPDATE [2am].[dbo].[AuditSubsidy] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, SubsidyKey = @SubsidyKey, SubsidyProviderKey = @SubsidyProviderKey, EmploymentKey = @EmploymentKey, LegalEntityKey = @LegalEntityKey, SalaryNumber = @SalaryNumber, Paypoint = @Paypoint, Notch = @Notch, Rank = @Rank, GeneralStatusKey = @GeneralStatusKey, StopOrderAmount = @StopOrderAmount, GEPFMember = @GEPFMember WHERE AuditNumber = @AuditNumber";



        public const string catspaymentbatchtypedatamodel_selectwhere = "SELECT CATSPaymentBatchTypeKey, Description, CATSProfile, CATSFileNamePrefix, CATSEnvironment, NextCATSFileSequenceNo FROM [2am].[dbo].[CATSPaymentBatchType] WHERE";
        public const string catspaymentbatchtypedatamodel_selectbykey = "SELECT CATSPaymentBatchTypeKey, Description, CATSProfile, CATSFileNamePrefix, CATSEnvironment, NextCATSFileSequenceNo FROM [2am].[dbo].[CATSPaymentBatchType] WHERE CATSPaymentBatchTypeKey = @PrimaryKey";
        public const string catspaymentbatchtypedatamodel_delete = "DELETE FROM [2am].[dbo].[CATSPaymentBatchType] WHERE CATSPaymentBatchTypeKey = @PrimaryKey";
        public const string catspaymentbatchtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CATSPaymentBatchType] WHERE";
        public const string catspaymentbatchtypedatamodel_insert = "INSERT INTO [2am].[dbo].[CATSPaymentBatchType] (Description, CATSProfile, CATSFileNamePrefix, CATSEnvironment, NextCATSFileSequenceNo) VALUES(@Description, @CATSProfile, @CATSFileNamePrefix, @CATSEnvironment, @NextCATSFileSequenceNo); select cast(scope_identity() as int)";
        public const string catspaymentbatchtypedatamodel_update = "UPDATE [2am].[dbo].[CATSPaymentBatchType] SET Description = @Description, CATSProfile = @CATSProfile, CATSFileNamePrefix = @CATSFileNamePrefix, CATSEnvironment = @CATSEnvironment, NextCATSFileSequenceNo = @NextCATSFileSequenceNo WHERE CATSPaymentBatchTypeKey = @CATSPaymentBatchTypeKey";



        public const string offermarketingsurveytypegroupdatamodel_selectwhere = "SELECT OfferMarketingSurveyTypeGroupKey, Description FROM [2am].[dbo].[OfferMarketingSurveyTypeGroup] WHERE";
        public const string offermarketingsurveytypegroupdatamodel_selectbykey = "SELECT OfferMarketingSurveyTypeGroupKey, Description FROM [2am].[dbo].[OfferMarketingSurveyTypeGroup] WHERE OfferMarketingSurveyTypeGroupKey = @PrimaryKey";
        public const string offermarketingsurveytypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferMarketingSurveyTypeGroup] WHERE OfferMarketingSurveyTypeGroupKey = @PrimaryKey";
        public const string offermarketingsurveytypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferMarketingSurveyTypeGroup] WHERE";
        public const string offermarketingsurveytypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferMarketingSurveyTypeGroup] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string offermarketingsurveytypegroupdatamodel_update = "UPDATE [2am].[dbo].[OfferMarketingSurveyTypeGroup] SET Description = @Description WHERE OfferMarketingSurveyTypeGroupKey = @OfferMarketingSurveyTypeGroupKey";



        public const string detailtypedatamodel_selectwhere = "SELECT DetailTypeKey, Description, DetailClassKey, GeneralStatusKey, AllowUpdateDelete, AllowUpdate, AllowScreen FROM [2am].[dbo].[DetailType] WHERE";
        public const string detailtypedatamodel_selectbykey = "SELECT DetailTypeKey, Description, DetailClassKey, GeneralStatusKey, AllowUpdateDelete, AllowUpdate, AllowScreen FROM [2am].[dbo].[DetailType] WHERE DetailTypeKey = @PrimaryKey";
        public const string detailtypedatamodel_delete = "DELETE FROM [2am].[dbo].[DetailType] WHERE DetailTypeKey = @PrimaryKey";
        public const string detailtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DetailType] WHERE";
        public const string detailtypedatamodel_insert = "INSERT INTO [2am].[dbo].[DetailType] (DetailTypeKey, Description, DetailClassKey, GeneralStatusKey, AllowUpdateDelete, AllowUpdate, AllowScreen) VALUES(@DetailTypeKey, @Description, @DetailClassKey, @GeneralStatusKey, @AllowUpdateDelete, @AllowUpdate, @AllowScreen); ";
        public const string detailtypedatamodel_update = "UPDATE [2am].[dbo].[DetailType] SET DetailTypeKey = @DetailTypeKey, Description = @Description, DetailClassKey = @DetailClassKey, GeneralStatusKey = @GeneralStatusKey, AllowUpdateDelete = @AllowUpdateDelete, AllowUpdate = @AllowUpdate, AllowScreen = @AllowScreen WHERE DetailTypeKey = @DetailTypeKey";



        public const string lifepolicy_snapshotdatamodel_selectwhere = "SELECT FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, CurrentArrearBalance, CurrentYearlyBalance, MonthlyPremium, YearlyPremium, CapitalizedYearlyBalance, CapitalizedMonthlyBalance, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall FROM [2am].[dbo].[LifePolicy_Snapshot] WHERE";
        public const string lifepolicy_snapshotdatamodel_selectbykey = "SELECT FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, CurrentArrearBalance, CurrentYearlyBalance, MonthlyPremium, YearlyPremium, CapitalizedYearlyBalance, CapitalizedMonthlyBalance, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall FROM [2am].[dbo].[LifePolicy_Snapshot] WHERE  = @PrimaryKey";
        public const string lifepolicy_snapshotdatamodel_delete = "DELETE FROM [2am].[dbo].[LifePolicy_Snapshot] WHERE  = @PrimaryKey";
        public const string lifepolicy_snapshotdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifePolicy_Snapshot] WHERE";
        public const string lifepolicy_snapshotdatamodel_insert = "INSERT INTO [2am].[dbo].[LifePolicy_Snapshot] (FinancialServiceKey, DeathBenefit, InstallmentProtectionBenefit, DeathBenefitPremium, InstallmentProtectionPremium, PolicyStatusKey, DateOfCommencement, DateOfExpiry, DeathRetentionLimit, InstallmentProtectionRetentionLimit, UpliftFactor, JointDiscountFactor, DateOfCancellation, CurrentArrearBalance, CurrentYearlyBalance, MonthlyPremium, YearlyPremium, CapitalizedYearlyBalance, CapitalizedMonthlyBalance, DateOfAcceptance, SumAssured, DateLastUpdated, Consultant, ClaimStatusKey, CurrentSumAssured, PremiumShortfall) VALUES(@FinancialServiceKey, @DeathBenefit, @InstallmentProtectionBenefit, @DeathBenefitPremium, @InstallmentProtectionPremium, @PolicyStatusKey, @DateOfCommencement, @DateOfExpiry, @DeathRetentionLimit, @InstallmentProtectionRetentionLimit, @UpliftFactor, @JointDiscountFactor, @DateOfCancellation, @CurrentArrearBalance, @CurrentYearlyBalance, @MonthlyPremium, @YearlyPremium, @CapitalizedYearlyBalance, @CapitalizedMonthlyBalance, @DateOfAcceptance, @SumAssured, @DateLastUpdated, @Consultant, @ClaimStatusKey, @CurrentSumAssured, @PremiumShortfall); ";
        public const string lifepolicy_snapshotdatamodel_update = "UPDATE [2am].[dbo].[LifePolicy_Snapshot] SET FinancialServiceKey = @FinancialServiceKey, DeathBenefit = @DeathBenefit, InstallmentProtectionBenefit = @InstallmentProtectionBenefit, DeathBenefitPremium = @DeathBenefitPremium, InstallmentProtectionPremium = @InstallmentProtectionPremium, PolicyStatusKey = @PolicyStatusKey, DateOfCommencement = @DateOfCommencement, DateOfExpiry = @DateOfExpiry, DeathRetentionLimit = @DeathRetentionLimit, InstallmentProtectionRetentionLimit = @InstallmentProtectionRetentionLimit, UpliftFactor = @UpliftFactor, JointDiscountFactor = @JointDiscountFactor, DateOfCancellation = @DateOfCancellation, CurrentArrearBalance = @CurrentArrearBalance, CurrentYearlyBalance = @CurrentYearlyBalance, MonthlyPremium = @MonthlyPremium, YearlyPremium = @YearlyPremium, CapitalizedYearlyBalance = @CapitalizedYearlyBalance, CapitalizedMonthlyBalance = @CapitalizedMonthlyBalance, DateOfAcceptance = @DateOfAcceptance, SumAssured = @SumAssured, DateLastUpdated = @DateLastUpdated, Consultant = @Consultant, ClaimStatusKey = @ClaimStatusKey, CurrentSumAssured = @CurrentSumAssured, PremiumShortfall = @PremiumShortfall WHERE  = @";



        public const string conditionsetuistatementdatamodel_selectwhere = "SELECT ConditionSetUIStatementKey, ConditionSetKey, UIStatementName FROM [2am].[dbo].[ConditionSetUIStatement] WHERE";
        public const string conditionsetuistatementdatamodel_selectbykey = "SELECT ConditionSetUIStatementKey, ConditionSetKey, UIStatementName FROM [2am].[dbo].[ConditionSetUIStatement] WHERE ConditionSetUIStatementKey = @PrimaryKey";
        public const string conditionsetuistatementdatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionSetUIStatement] WHERE ConditionSetUIStatementKey = @PrimaryKey";
        public const string conditionsetuistatementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionSetUIStatement] WHERE";
        public const string conditionsetuistatementdatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionSetUIStatement] (ConditionSetKey, UIStatementName) VALUES(@ConditionSetKey, @UIStatementName); select cast(scope_identity() as int)";
        public const string conditionsetuistatementdatamodel_update = "UPDATE [2am].[dbo].[ConditionSetUIStatement] SET ConditionSetKey = @ConditionSetKey, UIStatementName = @UIStatementName WHERE ConditionSetUIStatementKey = @ConditionSetUIStatementKey";



        public const string legalentityexceptiondatamodel_selectwhere = "SELECT LegalEntityExceptionKey, LegalEntityKey, LegalEntityExceptionReasonKey FROM [2am].[dbo].[LegalEntityException] WHERE";
        public const string legalentityexceptiondatamodel_selectbykey = "SELECT LegalEntityExceptionKey, LegalEntityKey, LegalEntityExceptionReasonKey FROM [2am].[dbo].[LegalEntityException] WHERE LegalEntityExceptionKey = @PrimaryKey";
        public const string legalentityexceptiondatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityException] WHERE LegalEntityExceptionKey = @PrimaryKey";
        public const string legalentityexceptiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityException] WHERE";
        public const string legalentityexceptiondatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityException] (LegalEntityKey, LegalEntityExceptionReasonKey) VALUES(@LegalEntityKey, @LegalEntityExceptionReasonKey); select cast(scope_identity() as int)";
        public const string legalentityexceptiondatamodel_update = "UPDATE [2am].[dbo].[LegalEntityException] SET LegalEntityKey = @LegalEntityKey, LegalEntityExceptionReasonKey = @LegalEntityExceptionReasonKey WHERE LegalEntityExceptionKey = @LegalEntityExceptionKey";



        public const string itcxsldatamodel_selectwhere = "SELECT ITCXslKey, EffectiveDate, StyleSheet FROM [2am].[dbo].[ITCXsl] WHERE";
        public const string itcxsldatamodel_selectbykey = "SELECT ITCXslKey, EffectiveDate, StyleSheet FROM [2am].[dbo].[ITCXsl] WHERE ITCXslKey = @PrimaryKey";
        public const string itcxsldatamodel_delete = "DELETE FROM [2am].[dbo].[ITCXsl] WHERE ITCXslKey = @PrimaryKey";
        public const string itcxsldatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ITCXsl] WHERE";
        public const string itcxsldatamodel_insert = "INSERT INTO [2am].[dbo].[ITCXsl] (EffectiveDate, StyleSheet) VALUES(@EffectiveDate, @StyleSheet); select cast(scope_identity() as int)";
        public const string itcxsldatamodel_update = "UPDATE [2am].[dbo].[ITCXsl] SET EffectiveDate = @EffectiveDate, StyleSheet = @StyleSheet WHERE ITCXslKey = @ITCXslKey";



        public const string callingcontextdatamodel_selectwhere = "SELECT CallingContextKey, CallingContextTypeKey, CallingProcess, CallingMethod, CallingState FROM [2am].[dbo].[CallingContext] WHERE";
        public const string callingcontextdatamodel_selectbykey = "SELECT CallingContextKey, CallingContextTypeKey, CallingProcess, CallingMethod, CallingState FROM [2am].[dbo].[CallingContext] WHERE CallingContextKey = @PrimaryKey";
        public const string callingcontextdatamodel_delete = "DELETE FROM [2am].[dbo].[CallingContext] WHERE CallingContextKey = @PrimaryKey";
        public const string callingcontextdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CallingContext] WHERE";
        public const string callingcontextdatamodel_insert = "INSERT INTO [2am].[dbo].[CallingContext] (CallingContextKey, CallingContextTypeKey, CallingProcess, CallingMethod, CallingState) VALUES(@CallingContextKey, @CallingContextTypeKey, @CallingProcess, @CallingMethod, @CallingState); ";
        public const string callingcontextdatamodel_update = "UPDATE [2am].[dbo].[CallingContext] SET CallingContextKey = @CallingContextKey, CallingContextTypeKey = @CallingContextTypeKey, CallingProcess = @CallingProcess, CallingMethod = @CallingMethod, CallingState = @CallingState WHERE CallingContextKey = @CallingContextKey";



        public const string auditemploymentdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, EmploymentKey, EmployerKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmploymentStartDate, EmploymentEndDate, MonthlyIncome, ContactPerson, ContactPhoneNumber, ContactPhoneCode, ConfirmedIncome, ConfirmedBy, ConfirmedDate, UserID, ChangeDate, Department, BasicIncome, Commission, Overtime, Shift, Performance, Allowances, PAYE, UIF, PensionProvident, MedicalAid, ConfirmedBasicIncome, ConfirmedCommission, ConfirmedOvertime, ConfirmedShift, ConfirmedPerformance, ConfirmedAllowances, ConfirmedPAYE, ConfirmedUIF, ConfirmedPensionProvident, ConfirmedMedicalAid, JobTitle, SalaryPaymentDay, UnionMember FROM [2am].[dbo].[AuditEmployment] WHERE";
        public const string auditemploymentdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, EmploymentKey, EmployerKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmploymentStartDate, EmploymentEndDate, MonthlyIncome, ContactPerson, ContactPhoneNumber, ContactPhoneCode, ConfirmedIncome, ConfirmedBy, ConfirmedDate, UserID, ChangeDate, Department, BasicIncome, Commission, Overtime, Shift, Performance, Allowances, PAYE, UIF, PensionProvident, MedicalAid, ConfirmedBasicIncome, ConfirmedCommission, ConfirmedOvertime, ConfirmedShift, ConfirmedPerformance, ConfirmedAllowances, ConfirmedPAYE, ConfirmedUIF, ConfirmedPensionProvident, ConfirmedMedicalAid, JobTitle, SalaryPaymentDay, UnionMember FROM [2am].[dbo].[AuditEmployment] WHERE AuditNumber = @PrimaryKey";
        public const string auditemploymentdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditEmployment] WHERE AuditNumber = @PrimaryKey";
        public const string auditemploymentdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditEmployment] WHERE";
        public const string auditemploymentdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditEmployment] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, EmploymentKey, EmployerKey, EmploymentTypeKey, RemunerationTypeKey, EmploymentStatusKey, LegalEntityKey, EmploymentStartDate, EmploymentEndDate, MonthlyIncome, ContactPerson, ContactPhoneNumber, ContactPhoneCode, ConfirmedIncome, ConfirmedBy, ConfirmedDate, UserID, ChangeDate, Department, BasicIncome, Commission, Overtime, Shift, Performance, Allowances, PAYE, UIF, PensionProvident, MedicalAid, ConfirmedBasicIncome, ConfirmedCommission, ConfirmedOvertime, ConfirmedShift, ConfirmedPerformance, ConfirmedAllowances, ConfirmedPAYE, ConfirmedUIF, ConfirmedPensionProvident, ConfirmedMedicalAid, JobTitle, SalaryPaymentDay, UnionMember) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @EmploymentKey, @EmployerKey, @EmploymentTypeKey, @RemunerationTypeKey, @EmploymentStatusKey, @LegalEntityKey, @EmploymentStartDate, @EmploymentEndDate, @MonthlyIncome, @ContactPerson, @ContactPhoneNumber, @ContactPhoneCode, @ConfirmedIncome, @ConfirmedBy, @ConfirmedDate, @UserID, @ChangeDate, @Department, @BasicIncome, @Commission, @Overtime, @Shift, @Performance, @Allowances, @PAYE, @UIF, @PensionProvident, @MedicalAid, @ConfirmedBasicIncome, @ConfirmedCommission, @ConfirmedOvertime, @ConfirmedShift, @ConfirmedPerformance, @ConfirmedAllowances, @ConfirmedPAYE, @ConfirmedUIF, @ConfirmedPensionProvident, @ConfirmedMedicalAid, @JobTitle, @SalaryPaymentDay, @UnionMember); select cast(scope_identity() as int)";
        public const string auditemploymentdatamodel_update = "UPDATE [2am].[dbo].[AuditEmployment] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, EmploymentKey = @EmploymentKey, EmployerKey = @EmployerKey, EmploymentTypeKey = @EmploymentTypeKey, RemunerationTypeKey = @RemunerationTypeKey, EmploymentStatusKey = @EmploymentStatusKey, LegalEntityKey = @LegalEntityKey, EmploymentStartDate = @EmploymentStartDate, EmploymentEndDate = @EmploymentEndDate, MonthlyIncome = @MonthlyIncome, ContactPerson = @ContactPerson, ContactPhoneNumber = @ContactPhoneNumber, ContactPhoneCode = @ContactPhoneCode, ConfirmedIncome = @ConfirmedIncome, ConfirmedBy = @ConfirmedBy, ConfirmedDate = @ConfirmedDate, UserID = @UserID, ChangeDate = @ChangeDate, Department = @Department, BasicIncome = @BasicIncome, Commission = @Commission, Overtime = @Overtime, Shift = @Shift, Performance = @Performance, Allowances = @Allowances, PAYE = @PAYE, UIF = @UIF, PensionProvident = @PensionProvident, MedicalAid = @MedicalAid, ConfirmedBasicIncome = @ConfirmedBasicIncome, ConfirmedCommission = @ConfirmedCommission, ConfirmedOvertime = @ConfirmedOvertime, ConfirmedShift = @ConfirmedShift, ConfirmedPerformance = @ConfirmedPerformance, ConfirmedAllowances = @ConfirmedAllowances, ConfirmedPAYE = @ConfirmedPAYE, ConfirmedUIF = @ConfirmedUIF, ConfirmedPensionProvident = @ConfirmedPensionProvident, ConfirmedMedicalAid = @ConfirmedMedicalAid, JobTitle = @JobTitle, SalaryPaymentDay = @SalaryPaymentDay, UnionMember = @UnionMember WHERE AuditNumber = @AuditNumber";



        public const string valuationdatamodel_selectwhere = "SELECT ValuationKey, ValuatorKey, ValuationDate, ValuationAmount, ValuationHOCValue, ValuationMunicipal, ValuationUserID, PropertyKey, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, ChangeDate, ValuationClassificationKey, ValuationEscalationPercentage, ValuationStatusKey, Data, ValuationDataProviderDataServiceKey, IsActive, HOCRoofKey FROM [2am].[dbo].[Valuation] WHERE";
        public const string valuationdatamodel_selectbykey = "SELECT ValuationKey, ValuatorKey, ValuationDate, ValuationAmount, ValuationHOCValue, ValuationMunicipal, ValuationUserID, PropertyKey, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, ChangeDate, ValuationClassificationKey, ValuationEscalationPercentage, ValuationStatusKey, Data, ValuationDataProviderDataServiceKey, IsActive, HOCRoofKey FROM [2am].[dbo].[Valuation] WHERE ValuationKey = @PrimaryKey";
        public const string valuationdatamodel_delete = "DELETE FROM [2am].[dbo].[Valuation] WHERE ValuationKey = @PrimaryKey";
        public const string valuationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Valuation] WHERE";
        public const string valuationdatamodel_insert = "INSERT INTO [2am].[dbo].[Valuation] (ValuatorKey, ValuationDate, ValuationAmount, ValuationHOCValue, ValuationMunicipal, ValuationUserID, PropertyKey, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, ChangeDate, ValuationClassificationKey, ValuationEscalationPercentage, ValuationStatusKey, Data, ValuationDataProviderDataServiceKey, IsActive, HOCRoofKey) VALUES(@ValuatorKey, @ValuationDate, @ValuationAmount, @ValuationHOCValue, @ValuationMunicipal, @ValuationUserID, @PropertyKey, @HOCThatchAmount, @HOCConventionalAmount, @HOCShingleAmount, @ChangeDate, @ValuationClassificationKey, @ValuationEscalationPercentage, @ValuationStatusKey, @Data, @ValuationDataProviderDataServiceKey, @IsActive, @HOCRoofKey); select cast(scope_identity() as int)";
        public const string valuationdatamodel_update = "UPDATE [2am].[dbo].[Valuation] SET ValuatorKey = @ValuatorKey, ValuationDate = @ValuationDate, ValuationAmount = @ValuationAmount, ValuationHOCValue = @ValuationHOCValue, ValuationMunicipal = @ValuationMunicipal, ValuationUserID = @ValuationUserID, PropertyKey = @PropertyKey, HOCThatchAmount = @HOCThatchAmount, HOCConventionalAmount = @HOCConventionalAmount, HOCShingleAmount = @HOCShingleAmount, ChangeDate = @ChangeDate, ValuationClassificationKey = @ValuationClassificationKey, ValuationEscalationPercentage = @ValuationEscalationPercentage, ValuationStatusKey = @ValuationStatusKey, Data = @Data, ValuationDataProviderDataServiceKey = @ValuationDataProviderDataServiceKey, IsActive = @IsActive, HOCRoofKey = @HOCRoofKey WHERE ValuationKey = @ValuationKey";



        public const string offerroletypedatamodel_selectwhere = "SELECT OfferRoleTypeKey, Description, OfferRoleTypeGroupKey FROM [2am].[dbo].[OfferRoleType] WHERE";
        public const string offerroletypedatamodel_selectbykey = "SELECT OfferRoleTypeKey, Description, OfferRoleTypeGroupKey FROM [2am].[dbo].[OfferRoleType] WHERE OfferRoleTypeKey = @PrimaryKey";
        public const string offerroletypedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferRoleType] WHERE OfferRoleTypeKey = @PrimaryKey";
        public const string offerroletypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferRoleType] WHERE";
        public const string offerroletypedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferRoleType] (OfferRoleTypeKey, Description, OfferRoleTypeGroupKey) VALUES(@OfferRoleTypeKey, @Description, @OfferRoleTypeGroupKey); ";
        public const string offerroletypedatamodel_update = "UPDATE [2am].[dbo].[OfferRoleType] SET OfferRoleTypeKey = @OfferRoleTypeKey, Description = @Description, OfferRoleTypeGroupKey = @OfferRoleTypeGroupKey WHERE OfferRoleTypeKey = @OfferRoleTypeKey";



        public const string capntureasondatamodel_selectwhere = "SELECT CapNTUReasonKey, Description FROM [2am].[dbo].[CapNTUReason] WHERE";
        public const string capntureasondatamodel_selectbykey = "SELECT CapNTUReasonKey, Description FROM [2am].[dbo].[CapNTUReason] WHERE CapNTUReasonKey = @PrimaryKey";
        public const string capntureasondatamodel_delete = "DELETE FROM [2am].[dbo].[CapNTUReason] WHERE CapNTUReasonKey = @PrimaryKey";
        public const string capntureasondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CapNTUReason] WHERE";
        public const string capntureasondatamodel_insert = "INSERT INTO [2am].[dbo].[CapNTUReason] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string capntureasondatamodel_update = "UPDATE [2am].[dbo].[CapNTUReason] SET Description = @Description WHERE CapNTUReasonKey = @CapNTUReasonKey";



        public const string genericcolumndefinitiondatamodel_selectwhere = "SELECT GenericColumnDefinitionKey, Description, TableName, ColumnName FROM [2am].[dbo].[GenericColumnDefinition] WHERE";
        public const string genericcolumndefinitiondatamodel_selectbykey = "SELECT GenericColumnDefinitionKey, Description, TableName, ColumnName FROM [2am].[dbo].[GenericColumnDefinition] WHERE GenericColumnDefinitionKey = @PrimaryKey";
        public const string genericcolumndefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[GenericColumnDefinition] WHERE GenericColumnDefinitionKey = @PrimaryKey";
        public const string genericcolumndefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[GenericColumnDefinition] WHERE";
        public const string genericcolumndefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[GenericColumnDefinition] (Description, TableName, ColumnName) VALUES(@Description, @TableName, @ColumnName); select cast(scope_identity() as int)";
        public const string genericcolumndefinitiondatamodel_update = "UPDATE [2am].[dbo].[GenericColumnDefinition] SET Description = @Description, TableName = @TableName, ColumnName = @ColumnName WHERE GenericColumnDefinitionKey = @GenericColumnDefinitionKey";



        public const string documentversiondatamodel_selectwhere = "SELECT DocumentVersionKey, DocumentTypeKey, Version, EffectiveDate, ActiveIndicator FROM [2am].[dbo].[DocumentVersion] WHERE";
        public const string documentversiondatamodel_selectbykey = "SELECT DocumentVersionKey, DocumentTypeKey, Version, EffectiveDate, ActiveIndicator FROM [2am].[dbo].[DocumentVersion] WHERE DocumentVersionKey = @PrimaryKey";
        public const string documentversiondatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentVersion] WHERE DocumentVersionKey = @PrimaryKey";
        public const string documentversiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentVersion] WHERE";
        public const string documentversiondatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentVersion] (DocumentTypeKey, Version, EffectiveDate, ActiveIndicator) VALUES(@DocumentTypeKey, @Version, @EffectiveDate, @ActiveIndicator); select cast(scope_identity() as int)";
        public const string documentversiondatamodel_update = "UPDATE [2am].[dbo].[DocumentVersion] SET DocumentTypeKey = @DocumentTypeKey, Version = @Version, EffectiveDate = @EffectiveDate, ActiveIndicator = @ActiveIndicator WHERE DocumentVersionKey = @DocumentVersionKey";



        public const string offercreditscoredatamodel_selectwhere = "SELECT OfferCreditScoreKey, OfferKey, OfferAggregateDecisionKey, ScoreDate, CallingContextKey FROM [2am].[dbo].[OfferCreditScore] WHERE";
        public const string offercreditscoredatamodel_selectbykey = "SELECT OfferCreditScoreKey, OfferKey, OfferAggregateDecisionKey, ScoreDate, CallingContextKey FROM [2am].[dbo].[OfferCreditScore] WHERE OfferCreditScoreKey = @PrimaryKey";
        public const string offercreditscoredatamodel_delete = "DELETE FROM [2am].[dbo].[OfferCreditScore] WHERE OfferCreditScoreKey = @PrimaryKey";
        public const string offercreditscoredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferCreditScore] WHERE";
        public const string offercreditscoredatamodel_insert = "INSERT INTO [2am].[dbo].[OfferCreditScore] (OfferKey, OfferAggregateDecisionKey, ScoreDate, CallingContextKey) VALUES(@OfferKey, @OfferAggregateDecisionKey, @ScoreDate, @CallingContextKey); select cast(scope_identity() as int)";
        public const string offercreditscoredatamodel_update = "UPDATE [2am].[dbo].[OfferCreditScore] SET OfferKey = @OfferKey, OfferAggregateDecisionKey = @OfferAggregateDecisionKey, ScoreDate = @ScoreDate, CallingContextKey = @CallingContextKey WHERE OfferCreditScoreKey = @OfferCreditScoreKey";



        public const string creditcriteriaattributetypedatamodel_selectwhere = "SELECT CreditCriteriaAttributeTypeKey, Description FROM [2am].[dbo].[CreditCriteriaAttributeType] WHERE";
        public const string creditcriteriaattributetypedatamodel_selectbykey = "SELECT CreditCriteriaAttributeTypeKey, Description FROM [2am].[dbo].[CreditCriteriaAttributeType] WHERE CreditCriteriaAttributeTypeKey = @PrimaryKey";
        public const string creditcriteriaattributetypedatamodel_delete = "DELETE FROM [2am].[dbo].[CreditCriteriaAttributeType] WHERE CreditCriteriaAttributeTypeKey = @PrimaryKey";
        public const string creditcriteriaattributetypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CreditCriteriaAttributeType] WHERE";
        public const string creditcriteriaattributetypedatamodel_insert = "INSERT INTO [2am].[dbo].[CreditCriteriaAttributeType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string creditcriteriaattributetypedatamodel_update = "UPDATE [2am].[dbo].[CreditCriteriaAttributeType] SET Description = @Description WHERE CreditCriteriaAttributeTypeKey = @CreditCriteriaAttributeTypeKey";



        public const string legalentitycleanupdatamodel_selectwhere = "SELECT LegalEntityCleanUpKey, LegalEntityKey, LegalEntityExceptionReasonKey, Description, Surname, Firstnames, IDNumber, Accounts FROM [2am].[dbo].[LegalEntityCleanUp] WHERE";
        public const string legalentitycleanupdatamodel_selectbykey = "SELECT LegalEntityCleanUpKey, LegalEntityKey, LegalEntityExceptionReasonKey, Description, Surname, Firstnames, IDNumber, Accounts FROM [2am].[dbo].[LegalEntityCleanUp] WHERE LegalEntityCleanUpKey = @PrimaryKey";
        public const string legalentitycleanupdatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityCleanUp] WHERE LegalEntityCleanUpKey = @PrimaryKey";
        public const string legalentitycleanupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityCleanUp] WHERE";
        public const string legalentitycleanupdatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityCleanUp] (LegalEntityKey, LegalEntityExceptionReasonKey, Description, Surname, Firstnames, IDNumber, Accounts) VALUES(@LegalEntityKey, @LegalEntityExceptionReasonKey, @Description, @Surname, @Firstnames, @IDNumber, @Accounts); select cast(scope_identity() as int)";
        public const string legalentitycleanupdatamodel_update = "UPDATE [2am].[dbo].[LegalEntityCleanUp] SET LegalEntityKey = @LegalEntityKey, LegalEntityExceptionReasonKey = @LegalEntityExceptionReasonKey, Description = @Description, Surname = @Surname, Firstnames = @Firstnames, IDNumber = @IDNumber, Accounts = @Accounts WHERE LegalEntityCleanUpKey = @LegalEntityCleanUpKey";



        public const string acbbankdatamodel_selectwhere = "SELECT ACBBankCode, ACBBankDescription FROM [2am].[dbo].[ACBBank] WHERE";
        public const string acbbankdatamodel_selectbykey = "SELECT ACBBankCode, ACBBankDescription FROM [2am].[dbo].[ACBBank] WHERE ACBBankCode = @PrimaryKey";
        public const string acbbankdatamodel_delete = "DELETE FROM [2am].[dbo].[ACBBank] WHERE ACBBankCode = @PrimaryKey";
        public const string acbbankdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ACBBank] WHERE";
        public const string acbbankdatamodel_insert = "INSERT INTO [2am].[dbo].[ACBBank] (ACBBankCode, ACBBankDescription) VALUES(@ACBBankCode, @ACBBankDescription); ";
        public const string acbbankdatamodel_update = "UPDATE [2am].[dbo].[ACBBank] SET ACBBankCode = @ACBBankCode, ACBBankDescription = @ACBBankDescription WHERE ACBBankCode = @ACBBankCode";



        public const string conditionconfigurationdatamodel_selectwhere = "SELECT ConditionConfigurationKey, GenericKeyTypeKey, GenericColumnDefinitionKey, GenericColumnDefinitionValue FROM [2am].[dbo].[ConditionConfiguration] WHERE";
        public const string conditionconfigurationdatamodel_selectbykey = "SELECT ConditionConfigurationKey, GenericKeyTypeKey, GenericColumnDefinitionKey, GenericColumnDefinitionValue FROM [2am].[dbo].[ConditionConfiguration] WHERE ConditionConfigurationKey = @PrimaryKey";
        public const string conditionconfigurationdatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionConfiguration] WHERE ConditionConfigurationKey = @PrimaryKey";
        public const string conditionconfigurationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionConfiguration] WHERE";
        public const string conditionconfigurationdatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionConfiguration] (GenericKeyTypeKey, GenericColumnDefinitionKey, GenericColumnDefinitionValue) VALUES(@GenericKeyTypeKey, @GenericColumnDefinitionKey, @GenericColumnDefinitionValue); select cast(scope_identity() as int)";
        public const string conditionconfigurationdatamodel_update = "UPDATE [2am].[dbo].[ConditionConfiguration] SET GenericKeyTypeKey = @GenericKeyTypeKey, GenericColumnDefinitionKey = @GenericColumnDefinitionKey, GenericColumnDefinitionValue = @GenericColumnDefinitionValue WHERE ConditionConfigurationKey = @ConditionConfigurationKey";



        public const string disbursementstatusdatamodel_selectwhere = "SELECT DisbursementStatusKey, Description FROM [2am].[dbo].[DisbursementStatus] WHERE";
        public const string disbursementstatusdatamodel_selectbykey = "SELECT DisbursementStatusKey, Description FROM [2am].[dbo].[DisbursementStatus] WHERE DisbursementStatusKey = @PrimaryKey";
        public const string disbursementstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[DisbursementStatus] WHERE DisbursementStatusKey = @PrimaryKey";
        public const string disbursementstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisbursementStatus] WHERE";
        public const string disbursementstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[DisbursementStatus] (DisbursementStatusKey, Description) VALUES(@DisbursementStatusKey, @Description); ";
        public const string disbursementstatusdatamodel_update = "UPDATE [2am].[dbo].[DisbursementStatus] SET DisbursementStatusKey = @DisbursementStatusKey, Description = @Description WHERE DisbursementStatusKey = @DisbursementStatusKey";



        public const string audittradedatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, TradeKey, TradeType, Company, TradeDate, StartDate, EndDate, ResetConfigurationKey, StrikeRate, TradeBalance, CapBalance, Premium, CapTypeKey, TrancheTypeKey FROM [2am].[dbo].[AuditTrade] WHERE";
        public const string audittradedatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, TradeKey, TradeType, Company, TradeDate, StartDate, EndDate, ResetConfigurationKey, StrikeRate, TradeBalance, CapBalance, Premium, CapTypeKey, TrancheTypeKey FROM [2am].[dbo].[AuditTrade] WHERE AuditNumber = @PrimaryKey";
        public const string audittradedatamodel_delete = "DELETE FROM [2am].[dbo].[AuditTrade] WHERE AuditNumber = @PrimaryKey";
        public const string audittradedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditTrade] WHERE";
        public const string audittradedatamodel_insert = "INSERT INTO [2am].[dbo].[AuditTrade] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, TradeKey, TradeType, Company, TradeDate, StartDate, EndDate, ResetConfigurationKey, StrikeRate, TradeBalance, CapBalance, Premium, CapTypeKey, TrancheTypeKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @TradeKey, @TradeType, @Company, @TradeDate, @StartDate, @EndDate, @ResetConfigurationKey, @StrikeRate, @TradeBalance, @CapBalance, @Premium, @CapTypeKey, @TrancheTypeKey); select cast(scope_identity() as int)";
        public const string audittradedatamodel_update = "UPDATE [2am].[dbo].[AuditTrade] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, TradeKey = @TradeKey, TradeType = @TradeType, Company = @Company, TradeDate = @TradeDate, StartDate = @StartDate, EndDate = @EndDate, ResetConfigurationKey = @ResetConfigurationKey, StrikeRate = @StrikeRate, TradeBalance = @TradeBalance, CapBalance = @CapBalance, Premium = @Premium, CapTypeKey = @CapTypeKey, TrancheTypeKey = @TrancheTypeKey WHERE AuditNumber = @AuditNumber";



        public const string correspondencedetaildatamodel_selectwhere = "SELECT CorrespondenceDetailKey, CorrespondenceKey, CorrespondenceText FROM [2am].[dbo].[CorrespondenceDetail] WHERE";
        public const string correspondencedetaildatamodel_selectbykey = "SELECT CorrespondenceDetailKey, CorrespondenceKey, CorrespondenceText FROM [2am].[dbo].[CorrespondenceDetail] WHERE CorrespondenceDetailKey = @PrimaryKey";
        public const string correspondencedetaildatamodel_delete = "DELETE FROM [2am].[dbo].[CorrespondenceDetail] WHERE CorrespondenceDetailKey = @PrimaryKey";
        public const string correspondencedetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CorrespondenceDetail] WHERE";
        public const string correspondencedetaildatamodel_insert = "INSERT INTO [2am].[dbo].[CorrespondenceDetail] (CorrespondenceKey, CorrespondenceText) VALUES(@CorrespondenceKey, @CorrespondenceText); select cast(scope_identity() as int)";
        public const string correspondencedetaildatamodel_update = "UPDATE [2am].[dbo].[CorrespondenceDetail] SET CorrespondenceKey = @CorrespondenceKey, CorrespondenceText = @CorrespondenceText WHERE CorrespondenceDetailKey = @CorrespondenceDetailKey";



        public const string uistatementtypedatamodel_selectwhere = "SELECT StatementTypeKey, Description FROM [2am].[dbo].[uiStatementType] WHERE";
        public const string uistatementtypedatamodel_selectbykey = "SELECT StatementTypeKey, Description FROM [2am].[dbo].[uiStatementType] WHERE StatementTypeKey = @PrimaryKey";
        public const string uistatementtypedatamodel_delete = "DELETE FROM [2am].[dbo].[uiStatementType] WHERE StatementTypeKey = @PrimaryKey";
        public const string uistatementtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[uiStatementType] WHERE";
        public const string uistatementtypedatamodel_insert = "INSERT INTO [2am].[dbo].[uiStatementType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string uistatementtypedatamodel_update = "UPDATE [2am].[dbo].[uiStatementType] SET Description = @Description WHERE StatementTypeKey = @StatementTypeKey";



        public const string acbbranchdatamodel_selectwhere = "SELECT ACBBranchCode, ACBBankCode, ACBBranchDescription, ActiveIndicator FROM [2am].[dbo].[ACBBranch] WHERE";
        public const string acbbranchdatamodel_selectbykey = "SELECT ACBBranchCode, ACBBankCode, ACBBranchDescription, ActiveIndicator FROM [2am].[dbo].[ACBBranch] WHERE ACBBranchCode = @PrimaryKey";
        public const string acbbranchdatamodel_delete = "DELETE FROM [2am].[dbo].[ACBBranch] WHERE ACBBranchCode = @PrimaryKey";
        public const string acbbranchdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ACBBranch] WHERE";
        public const string acbbranchdatamodel_insert = "INSERT INTO [2am].[dbo].[ACBBranch] (ACBBranchCode, ACBBankCode, ACBBranchDescription, ActiveIndicator) VALUES(@ACBBranchCode, @ACBBankCode, @ACBBranchDescription, @ActiveIndicator); ";
        public const string acbbranchdatamodel_update = "UPDATE [2am].[dbo].[ACBBranch] SET ACBBranchCode = @ACBBranchCode, ACBBankCode = @ACBBankCode, ACBBranchDescription = @ACBBranchDescription, ActiveIndicator = @ActiveIndicator WHERE ACBBranchCode = @ACBBranchCode";



        public const string translatableitemdatamodel_selectwhere = "SELECT TranslatableItemKey, Description FROM [2am].[dbo].[TranslatableItem] WHERE";
        public const string translatableitemdatamodel_selectbykey = "SELECT TranslatableItemKey, Description FROM [2am].[dbo].[TranslatableItem] WHERE TranslatableItemKey = @PrimaryKey";
        public const string translatableitemdatamodel_delete = "DELETE FROM [2am].[dbo].[TranslatableItem] WHERE TranslatableItemKey = @PrimaryKey";
        public const string translatableitemdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TranslatableItem] WHERE";
        public const string translatableitemdatamodel_insert = "INSERT INTO [2am].[dbo].[TranslatableItem] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string translatableitemdatamodel_update = "UPDATE [2am].[dbo].[TranslatableItem] SET Description = @Description WHERE TranslatableItemKey = @TranslatableItemKey";



        public const string stagedefinitioncompositedatamodel_selectwhere = "SELECT StageDefinitionCompositeKey, StageDefinitionStageDefinitionGroupCompositeKey, StageDefinitionStageDefinitionGroupKey, UseThisDate, Sequence, UseThisReason FROM [2am].[dbo].[StageDefinitionComposite] WHERE";
        public const string stagedefinitioncompositedatamodel_selectbykey = "SELECT StageDefinitionCompositeKey, StageDefinitionStageDefinitionGroupCompositeKey, StageDefinitionStageDefinitionGroupKey, UseThisDate, Sequence, UseThisReason FROM [2am].[dbo].[StageDefinitionComposite] WHERE StageDefinitionCompositeKey = @PrimaryKey";
        public const string stagedefinitioncompositedatamodel_delete = "DELETE FROM [2am].[dbo].[StageDefinitionComposite] WHERE StageDefinitionCompositeKey = @PrimaryKey";
        public const string stagedefinitioncompositedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StageDefinitionComposite] WHERE";
        public const string stagedefinitioncompositedatamodel_insert = "INSERT INTO [2am].[dbo].[StageDefinitionComposite] (StageDefinitionStageDefinitionGroupCompositeKey, StageDefinitionStageDefinitionGroupKey, UseThisDate, Sequence, UseThisReason) VALUES(@StageDefinitionStageDefinitionGroupCompositeKey, @StageDefinitionStageDefinitionGroupKey, @UseThisDate, @Sequence, @UseThisReason); select cast(scope_identity() as int)";
        public const string stagedefinitioncompositedatamodel_update = "UPDATE [2am].[dbo].[StageDefinitionComposite] SET StageDefinitionStageDefinitionGroupCompositeKey = @StageDefinitionStageDefinitionGroupCompositeKey, StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey, UseThisDate = @UseThisDate, Sequence = @Sequence, UseThisReason = @UseThisReason WHERE StageDefinitionCompositeKey = @StageDefinitionCompositeKey";



        public const string creditcriteriaattributedatamodel_selectwhere = "SELECT CreditCriteriaAttributeKey, CreditCriteriaKey, CreditCriteriaAttributeTypeKey FROM [2am].[dbo].[CreditCriteriaAttribute] WHERE";
        public const string creditcriteriaattributedatamodel_selectbykey = "SELECT CreditCriteriaAttributeKey, CreditCriteriaKey, CreditCriteriaAttributeTypeKey FROM [2am].[dbo].[CreditCriteriaAttribute] WHERE CreditCriteriaAttributeKey = @PrimaryKey";
        public const string creditcriteriaattributedatamodel_delete = "DELETE FROM [2am].[dbo].[CreditCriteriaAttribute] WHERE CreditCriteriaAttributeKey = @PrimaryKey";
        public const string creditcriteriaattributedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CreditCriteriaAttribute] WHERE";
        public const string creditcriteriaattributedatamodel_insert = "INSERT INTO [2am].[dbo].[CreditCriteriaAttribute] (CreditCriteriaKey, CreditCriteriaAttributeTypeKey) VALUES(@CreditCriteriaKey, @CreditCriteriaAttributeTypeKey); select cast(scope_identity() as int)";
        public const string creditcriteriaattributedatamodel_update = "UPDATE [2am].[dbo].[CreditCriteriaAttribute] SET CreditCriteriaKey = @CreditCriteriaKey, CreditCriteriaAttributeTypeKey = @CreditCriteriaAttributeTypeKey WHERE CreditCriteriaAttributeKey = @CreditCriteriaAttributeKey";



        public const string offerroletypegroupdatamodel_selectwhere = "SELECT OfferRoleTypeGroupKey, Description FROM [2am].[dbo].[OfferRoleTypeGroup] WHERE";
        public const string offerroletypegroupdatamodel_selectbykey = "SELECT OfferRoleTypeGroupKey, Description FROM [2am].[dbo].[OfferRoleTypeGroup] WHERE OfferRoleTypeGroupKey = @PrimaryKey";
        public const string offerroletypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferRoleTypeGroup] WHERE OfferRoleTypeGroupKey = @PrimaryKey";
        public const string offerroletypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferRoleTypeGroup] WHERE";
        public const string offerroletypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferRoleTypeGroup] (OfferRoleTypeGroupKey, Description) VALUES(@OfferRoleTypeGroupKey, @Description); ";
        public const string offerroletypegroupdatamodel_update = "UPDATE [2am].[dbo].[OfferRoleTypeGroup] SET OfferRoleTypeGroupKey = @OfferRoleTypeGroupKey, Description = @Description WHERE OfferRoleTypeGroupKey = @OfferRoleTypeGroupKey";



        public const string acbtypedatamodel_selectwhere = "SELECT ACBTypeNumber, ACBTypeDescription FROM [2am].[dbo].[ACBType] WHERE";
        public const string acbtypedatamodel_selectbykey = "SELECT ACBTypeNumber, ACBTypeDescription FROM [2am].[dbo].[ACBType] WHERE ACBTypeNumber = @PrimaryKey";
        public const string acbtypedatamodel_delete = "DELETE FROM [2am].[dbo].[ACBType] WHERE ACBTypeNumber = @PrimaryKey";
        public const string acbtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ACBType] WHERE";
        public const string acbtypedatamodel_insert = "INSERT INTO [2am].[dbo].[ACBType] (ACBTypeNumber, ACBTypeDescription) VALUES(@ACBTypeNumber, @ACBTypeDescription); ";
        public const string acbtypedatamodel_update = "UPDATE [2am].[dbo].[ACBType] SET ACBTypeNumber = @ACBTypeNumber, ACBTypeDescription = @ACBTypeDescription WHERE ACBTypeNumber = @ACBTypeNumber";



        public const string offercapitecdetaildatamodel_selectwhere = "SELECT OfferCapitecDetailKey, OfferKey, Branch, Consultant FROM [2am].[dbo].[OfferCapitecDetail] WHERE";
        public const string offercapitecdetaildatamodel_selectbykey = "SELECT OfferCapitecDetailKey, OfferKey, Branch, Consultant FROM [2am].[dbo].[OfferCapitecDetail] WHERE OfferCapitecDetailKey = @PrimaryKey";
        public const string offercapitecdetaildatamodel_delete = "DELETE FROM [2am].[dbo].[OfferCapitecDetail] WHERE OfferCapitecDetailKey = @PrimaryKey";
        public const string offercapitecdetaildatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferCapitecDetail] WHERE";
        public const string offercapitecdetaildatamodel_insert = "INSERT INTO [2am].[dbo].[OfferCapitecDetail] (OfferKey, Branch, Consultant) VALUES(@OfferKey, @Branch, @Consultant); select cast(scope_identity() as int)";
        public const string offercapitecdetaildatamodel_update = "UPDATE [2am].[dbo].[OfferCapitecDetail] SET OfferKey = @OfferKey, Branch = @Branch, Consultant = @Consultant WHERE OfferCapitecDetailKey = @OfferCapitecDetailKey";



        public const string adcheckvaluationidstatusdatamodel_selectwhere = "SELECT AdCheckValuationIDStatusKey, Description FROM [2am].[dbo].[AdCheckValuationIDStatus] WHERE";
        public const string adcheckvaluationidstatusdatamodel_selectbykey = "SELECT AdCheckValuationIDStatusKey, Description FROM [2am].[dbo].[AdCheckValuationIDStatus] WHERE AdCheckValuationIDStatusKey = @PrimaryKey";
        public const string adcheckvaluationidstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[AdCheckValuationIDStatus] WHERE AdCheckValuationIDStatusKey = @PrimaryKey";
        public const string adcheckvaluationidstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AdCheckValuationIDStatus] WHERE";
        public const string adcheckvaluationidstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[AdCheckValuationIDStatus] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string adcheckvaluationidstatusdatamodel_update = "UPDATE [2am].[dbo].[AdCheckValuationIDStatus] SET Description = @Description WHERE AdCheckValuationIDStatusKey = @AdCheckValuationIDStatusKey";



        public const string auditfinancialservicedatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, AccountKey, BankAccountKey, Payment, FinancialServiceTypeKey, DebitOrderDay, TradeKey, CategoryKey, AccountStatusKey, NextResetDate, ParentFinancialServiceKey, OpenDate, CloseDate FROM [2am].[dbo].[AuditFinancialService] WHERE";
        public const string auditfinancialservicedatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, AccountKey, BankAccountKey, Payment, FinancialServiceTypeKey, DebitOrderDay, TradeKey, CategoryKey, AccountStatusKey, NextResetDate, ParentFinancialServiceKey, OpenDate, CloseDate FROM [2am].[dbo].[AuditFinancialService] WHERE AuditNumber = @PrimaryKey";
        public const string auditfinancialservicedatamodel_delete = "DELETE FROM [2am].[dbo].[AuditFinancialService] WHERE AuditNumber = @PrimaryKey";
        public const string auditfinancialservicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditFinancialService] WHERE";
        public const string auditfinancialservicedatamodel_insert = "INSERT INTO [2am].[dbo].[AuditFinancialService] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceKey, AccountKey, BankAccountKey, Payment, FinancialServiceTypeKey, DebitOrderDay, TradeKey, CategoryKey, AccountStatusKey, NextResetDate, ParentFinancialServiceKey, OpenDate, CloseDate) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @FinancialServiceKey, @AccountKey, @BankAccountKey, @Payment, @FinancialServiceTypeKey, @DebitOrderDay, @TradeKey, @CategoryKey, @AccountStatusKey, @NextResetDate, @ParentFinancialServiceKey, @OpenDate, @CloseDate); select cast(scope_identity() as int)";
        public const string auditfinancialservicedatamodel_update = "UPDATE [2am].[dbo].[AuditFinancialService] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, FinancialServiceKey = @FinancialServiceKey, AccountKey = @AccountKey, BankAccountKey = @BankAccountKey, Payment = @Payment, FinancialServiceTypeKey = @FinancialServiceTypeKey, DebitOrderDay = @DebitOrderDay, TradeKey = @TradeKey, CategoryKey = @CategoryKey, AccountStatusKey = @AccountStatusKey, NextResetDate = @NextResetDate, ParentFinancialServiceKey = @ParentFinancialServiceKey, OpenDate = @OpenDate, CloseDate = @CloseDate WHERE AuditNumber = @AuditNumber";



        public const string stagedefinitiondatamodel_selectwhere = "SELECT StageDefinitionKey, Description, GeneralStatusKey, IsComposite, Name, CompositeTypeName, HasCompositeLogic FROM [2am].[dbo].[StageDefinition] WHERE";
        public const string stagedefinitiondatamodel_selectbykey = "SELECT StageDefinitionKey, Description, GeneralStatusKey, IsComposite, Name, CompositeTypeName, HasCompositeLogic FROM [2am].[dbo].[StageDefinition] WHERE StageDefinitionKey = @PrimaryKey";
        public const string stagedefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[StageDefinition] WHERE StageDefinitionKey = @PrimaryKey";
        public const string stagedefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StageDefinition] WHERE";
        public const string stagedefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[StageDefinition] (StageDefinitionKey, Description, GeneralStatusKey, IsComposite, Name, CompositeTypeName, HasCompositeLogic) VALUES(@StageDefinitionKey, @Description, @GeneralStatusKey, @IsComposite, @Name, @CompositeTypeName, @HasCompositeLogic); ";
        public const string stagedefinitiondatamodel_update = "UPDATE [2am].[dbo].[StageDefinition] SET StageDefinitionKey = @StageDefinitionKey, Description = @Description, GeneralStatusKey = @GeneralStatusKey, IsComposite = @IsComposite, Name = @Name, CompositeTypeName = @CompositeTypeName, HasCompositeLogic = @HasCompositeLogic WHERE StageDefinitionKey = @StageDefinitionKey";



        public const string offerexceptiontypegroupdatamodel_selectwhere = "SELECT OfferExceptionTypeGroupKey, Description FROM [2am].[dbo].[OfferExceptionTypeGroup] WHERE";
        public const string offerexceptiontypegroupdatamodel_selectbykey = "SELECT OfferExceptionTypeGroupKey, Description FROM [2am].[dbo].[OfferExceptionTypeGroup] WHERE OfferExceptionTypeGroupKey = @PrimaryKey";
        public const string offerexceptiontypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferExceptionTypeGroup] WHERE OfferExceptionTypeGroupKey = @PrimaryKey";
        public const string offerexceptiontypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferExceptionTypeGroup] WHERE";
        public const string offerexceptiontypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferExceptionTypeGroup] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string offerexceptiontypegroupdatamodel_update = "UPDATE [2am].[dbo].[OfferExceptionTypeGroup] SET Description = @Description WHERE OfferExceptionTypeGroupKey = @OfferExceptionTypeGroupKey";



        public const string tmp_life_ipbauditdatamodel_selectwhere = "SELECT FinancialServiceKey, AccountKey, LegalEntityKey, DateOfAcceptance, PolicyStatusKey, DateAddedToPolicy, SourceTable, SourceTablePrimaryKey, AgeNext, AgeAtAcceptance, AgeAtDateAdded FROM [2am].[dbo].[tmp_Life_IPBAudit] WHERE";
        public const string tmp_life_ipbauditdatamodel_selectbykey = "SELECT FinancialServiceKey, AccountKey, LegalEntityKey, DateOfAcceptance, PolicyStatusKey, DateAddedToPolicy, SourceTable, SourceTablePrimaryKey, AgeNext, AgeAtAcceptance, AgeAtDateAdded FROM [2am].[dbo].[tmp_Life_IPBAudit] WHERE  = @PrimaryKey";
        public const string tmp_life_ipbauditdatamodel_delete = "DELETE FROM [2am].[dbo].[tmp_Life_IPBAudit] WHERE  = @PrimaryKey";
        public const string tmp_life_ipbauditdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[tmp_Life_IPBAudit] WHERE";
        public const string tmp_life_ipbauditdatamodel_insert = "INSERT INTO [2am].[dbo].[tmp_Life_IPBAudit] (FinancialServiceKey, AccountKey, LegalEntityKey, DateOfAcceptance, PolicyStatusKey, DateAddedToPolicy, SourceTable, SourceTablePrimaryKey, AgeNext, AgeAtAcceptance, AgeAtDateAdded) VALUES(@FinancialServiceKey, @AccountKey, @LegalEntityKey, @DateOfAcceptance, @PolicyStatusKey, @DateAddedToPolicy, @SourceTable, @SourceTablePrimaryKey, @AgeNext, @AgeAtAcceptance, @AgeAtDateAdded); ";
        public const string tmp_life_ipbauditdatamodel_update = "UPDATE [2am].[dbo].[tmp_Life_IPBAudit] SET FinancialServiceKey = @FinancialServiceKey, AccountKey = @AccountKey, LegalEntityKey = @LegalEntityKey, DateOfAcceptance = @DateOfAcceptance, PolicyStatusKey = @PolicyStatusKey, DateAddedToPolicy = @DateAddedToPolicy, SourceTable = @SourceTable, SourceTablePrimaryKey = @SourceTablePrimaryKey, AgeNext = @AgeNext, AgeAtAcceptance = @AgeAtAcceptance, AgeAtDateAdded = @AgeAtDateAdded WHERE  = @";



        public const string domainfielddatamodel_selectwhere = "SELECT DomainFieldKey, Description, DisplayDescription, FormatTypeKey FROM [2am].[dbo].[DomainField] WHERE";
        public const string domainfielddatamodel_selectbykey = "SELECT DomainFieldKey, Description, DisplayDescription, FormatTypeKey FROM [2am].[dbo].[DomainField] WHERE DomainFieldKey = @PrimaryKey";
        public const string domainfielddatamodel_delete = "DELETE FROM [2am].[dbo].[DomainField] WHERE DomainFieldKey = @PrimaryKey";
        public const string domainfielddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DomainField] WHERE";
        public const string domainfielddatamodel_insert = "INSERT INTO [2am].[dbo].[DomainField] (DomainFieldKey, Description, DisplayDescription, FormatTypeKey) VALUES(@DomainFieldKey, @Description, @DisplayDescription, @FormatTypeKey); ";
        public const string domainfielddatamodel_update = "UPDATE [2am].[dbo].[DomainField] SET DomainFieldKey = @DomainFieldKey, Description = @Description, DisplayDescription = @DisplayDescription, FormatTypeKey = @FormatTypeKey WHERE DomainFieldKey = @DomainFieldKey";



        public const string accountdatamodel_selectwhere = "SELECT AccountKey, FixedPayment, AccountStatusKey, InsertedDate, OriginationSourceProductKey, OpenDate, CloseDate, RRR_ProductKey, RRR_OriginationSourceKey, UserID, ChangeDate, SPVKey, ParentAccountKey FROM [2am].[dbo].[Account] WHERE";
        public const string accountdatamodel_selectbykey = "SELECT AccountKey, FixedPayment, AccountStatusKey, InsertedDate, OriginationSourceProductKey, OpenDate, CloseDate, RRR_ProductKey, RRR_OriginationSourceKey, UserID, ChangeDate, SPVKey, ParentAccountKey FROM [2am].[dbo].[Account] WHERE AccountKey = @PrimaryKey";
        public const string accountdatamodel_delete = "DELETE FROM [2am].[dbo].[Account] WHERE AccountKey = @PrimaryKey";
        public const string accountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Account] WHERE";
        public const string accountdatamodel_insert = "INSERT INTO [2am].[dbo].[Account] (AccountKey, FixedPayment, AccountStatusKey, InsertedDate, OriginationSourceProductKey, OpenDate, CloseDate, RRR_ProductKey, RRR_OriginationSourceKey, UserID, ChangeDate, SPVKey, ParentAccountKey) VALUES(@AccountKey, @FixedPayment, @AccountStatusKey, @InsertedDate, @OriginationSourceProductKey, @OpenDate, @CloseDate, @RRR_ProductKey, @RRR_OriginationSourceKey, @UserID, @ChangeDate, @SPVKey, @ParentAccountKey); ";
        public const string accountdatamodel_update = "UPDATE [2am].[dbo].[Account] SET AccountKey = @AccountKey, FixedPayment = @FixedPayment, AccountStatusKey = @AccountStatusKey, InsertedDate = @InsertedDate, OriginationSourceProductKey = @OriginationSourceProductKey, OpenDate = @OpenDate, CloseDate = @CloseDate, RRR_ProductKey = @RRR_ProductKey, RRR_OriginationSourceKey = @RRR_OriginationSourceKey, UserID = @UserID, ChangeDate = @ChangeDate, SPVKey = @SPVKey, ParentAccountKey = @ParentAccountKey WHERE AccountKey = @AccountKey";



        public const string disbursementtransactiontypedatamodel_selectwhere = "SELECT DisbursementTransactionTypeKey, Description, TransactionTypeNumber FROM [2am].[dbo].[DisbursementTransactionType] WHERE";
        public const string disbursementtransactiontypedatamodel_selectbykey = "SELECT DisbursementTransactionTypeKey, Description, TransactionTypeNumber FROM [2am].[dbo].[DisbursementTransactionType] WHERE DisbursementTransactionTypeKey = @PrimaryKey";
        public const string disbursementtransactiontypedatamodel_delete = "DELETE FROM [2am].[dbo].[DisbursementTransactionType] WHERE DisbursementTransactionTypeKey = @PrimaryKey";
        public const string disbursementtransactiontypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisbursementTransactionType] WHERE";
        public const string disbursementtransactiontypedatamodel_insert = "INSERT INTO [2am].[dbo].[DisbursementTransactionType] (DisbursementTransactionTypeKey, Description, TransactionTypeNumber) VALUES(@DisbursementTransactionTypeKey, @Description, @TransactionTypeNumber); ";
        public const string disbursementtransactiontypedatamodel_update = "UPDATE [2am].[dbo].[DisbursementTransactionType] SET DisbursementTransactionTypeKey = @DisbursementTransactionTypeKey, Description = @Description, TransactionTypeNumber = @TransactionTypeNumber WHERE DisbursementTransactionTypeKey = @DisbursementTransactionTypeKey";



        public const string translatedtextdatamodel_selectwhere = "SELECT TranslatedTextKey, TranslatableItemKey, LanguageKey, TranslatedText FROM [2am].[dbo].[TranslatedText] WHERE";
        public const string translatedtextdatamodel_selectbykey = "SELECT TranslatedTextKey, TranslatableItemKey, LanguageKey, TranslatedText FROM [2am].[dbo].[TranslatedText] WHERE TranslatedTextKey = @PrimaryKey";
        public const string translatedtextdatamodel_delete = "DELETE FROM [2am].[dbo].[TranslatedText] WHERE TranslatedTextKey = @PrimaryKey";
        public const string translatedtextdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TranslatedText] WHERE";
        public const string translatedtextdatamodel_insert = "INSERT INTO [2am].[dbo].[TranslatedText] (TranslatableItemKey, LanguageKey, TranslatedText) VALUES(@TranslatableItemKey, @LanguageKey, @TranslatedText); select cast(scope_identity() as int)";
        public const string translatedtextdatamodel_update = "UPDATE [2am].[dbo].[TranslatedText] SET TranslatableItemKey = @TranslatableItemKey, LanguageKey = @LanguageKey, TranslatedText = @TranslatedText WHERE TranslatedTextKey = @TranslatedTextKey";



        public const string workflowstatedatamodel_selectwhere = "SELECT WorkflowStateKey, OriginationSourceProductKey, Description FROM [2am].[dbo].[WorkflowState] WHERE";
        public const string workflowstatedatamodel_selectbykey = "SELECT WorkflowStateKey, OriginationSourceProductKey, Description FROM [2am].[dbo].[WorkflowState] WHERE WorkflowStateKey = @PrimaryKey";
        public const string workflowstatedatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowState] WHERE WorkflowStateKey = @PrimaryKey";
        public const string workflowstatedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowState] WHERE";
        public const string workflowstatedatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowState] (WorkflowStateKey, OriginationSourceProductKey, Description) VALUES(@WorkflowStateKey, @OriginationSourceProductKey, @Description); ";
        public const string workflowstatedatamodel_update = "UPDATE [2am].[dbo].[WorkflowState] SET WorkflowStateKey = @WorkflowStateKey, OriginationSourceProductKey = @OriginationSourceProductKey, Description = @Description WHERE WorkflowStateKey = @WorkflowStateKey";



        public const string flofferdatamodel_selectwhere = "SELECT Offerkey, AccountKey, ReservedAccountKey, OfferStartDate, OfferTypeKey, OfferStatusKey, OriginationSourceKey, Reference FROM [2am].[dbo].[FLOffer] WHERE";
        public const string flofferdatamodel_selectbykey = "SELECT Offerkey, AccountKey, ReservedAccountKey, OfferStartDate, OfferTypeKey, OfferStatusKey, OriginationSourceKey, Reference FROM [2am].[dbo].[FLOffer] WHERE  = @PrimaryKey";
        public const string flofferdatamodel_delete = "DELETE FROM [2am].[dbo].[FLOffer] WHERE  = @PrimaryKey";
        public const string flofferdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FLOffer] WHERE";
        public const string flofferdatamodel_insert = "INSERT INTO [2am].[dbo].[FLOffer] (Offerkey, AccountKey, ReservedAccountKey, OfferStartDate, OfferTypeKey, OfferStatusKey, OriginationSourceKey, Reference) VALUES(@Offerkey, @AccountKey, @ReservedAccountKey, @OfferStartDate, @OfferTypeKey, @OfferStatusKey, @OriginationSourceKey, @Reference); ";
        public const string flofferdatamodel_update = "UPDATE [2am].[dbo].[FLOffer] SET Offerkey = @Offerkey, AccountKey = @AccountKey, ReservedAccountKey = @ReservedAccountKey, OfferStartDate = @OfferStartDate, OfferTypeKey = @OfferTypeKey, OfferStatusKey = @OfferStatusKey, OriginationSourceKey = @OriginationSourceKey, Reference = @Reference WHERE  = @";



        public const string conditionconfigurationconditionsetdatamodel_selectwhere = "SELECT ConditionConfigurationConditionSetKey, ConditionConfigurationKey, ConditionSetKey FROM [2am].[dbo].[ConditionConfigurationConditionSet] WHERE";
        public const string conditionconfigurationconditionsetdatamodel_selectbykey = "SELECT ConditionConfigurationConditionSetKey, ConditionConfigurationKey, ConditionSetKey FROM [2am].[dbo].[ConditionConfigurationConditionSet] WHERE ConditionConfigurationConditionSetKey = @PrimaryKey";
        public const string conditionconfigurationconditionsetdatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionConfigurationConditionSet] WHERE ConditionConfigurationConditionSetKey = @PrimaryKey";
        public const string conditionconfigurationconditionsetdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionConfigurationConditionSet] WHERE";
        public const string conditionconfigurationconditionsetdatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionConfigurationConditionSet] (ConditionConfigurationKey, ConditionSetKey) VALUES(@ConditionConfigurationKey, @ConditionSetKey); select cast(scope_identity() as int)";
        public const string conditionconfigurationconditionsetdatamodel_update = "UPDATE [2am].[dbo].[ConditionConfigurationConditionSet] SET ConditionConfigurationKey = @ConditionConfigurationKey, ConditionSetKey = @ConditionSetKey WHERE ConditionConfigurationConditionSetKey = @ConditionConfigurationConditionSetKey";



        public const string adcheckvaluationiddatamodel_selectwhere = "SELECT AdCheckValuationIDKey, AdCheckValuationIDStatusKey, OfferKey, ValuationKey, ChangeDate FROM [2am].[dbo].[AdCheckValuationID] WHERE";
        public const string adcheckvaluationiddatamodel_selectbykey = "SELECT AdCheckValuationIDKey, AdCheckValuationIDStatusKey, OfferKey, ValuationKey, ChangeDate FROM [2am].[dbo].[AdCheckValuationID] WHERE AdCheckValuationIDKey = @PrimaryKey";
        public const string adcheckvaluationiddatamodel_delete = "DELETE FROM [2am].[dbo].[AdCheckValuationID] WHERE AdCheckValuationIDKey = @PrimaryKey";
        public const string adcheckvaluationiddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AdCheckValuationID] WHERE";
        public const string adcheckvaluationiddatamodel_insert = "INSERT INTO [2am].[dbo].[AdCheckValuationID] (AdCheckValuationIDKey, AdCheckValuationIDStatusKey, OfferKey, ValuationKey, ChangeDate) VALUES(@AdCheckValuationIDKey, @AdCheckValuationIDStatusKey, @OfferKey, @ValuationKey, @ChangeDate); ";
        public const string adcheckvaluationiddatamodel_update = "UPDATE [2am].[dbo].[AdCheckValuationID] SET AdCheckValuationIDKey = @AdCheckValuationIDKey, AdCheckValuationIDStatusKey = @AdCheckValuationIDStatusKey, OfferKey = @OfferKey, ValuationKey = @ValuationKey, ChangeDate = @ChangeDate WHERE AdCheckValuationIDKey = @AdCheckValuationIDKey";



        public const string offeritccreditscoredatamodel_selectwhere = "SELECT OfferITCCreditScoreKey, OfferCreditScoreKey, ITCCreditScoreKey, CreditScoreDecisionKey, ScoreDate, PrimaryApplicant FROM [2am].[dbo].[OfferITCCreditScore] WHERE";
        public const string offeritccreditscoredatamodel_selectbykey = "SELECT OfferITCCreditScoreKey, OfferCreditScoreKey, ITCCreditScoreKey, CreditScoreDecisionKey, ScoreDate, PrimaryApplicant FROM [2am].[dbo].[OfferITCCreditScore] WHERE OfferITCCreditScoreKey = @PrimaryKey";
        public const string offeritccreditscoredatamodel_delete = "DELETE FROM [2am].[dbo].[OfferITCCreditScore] WHERE OfferITCCreditScoreKey = @PrimaryKey";
        public const string offeritccreditscoredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferITCCreditScore] WHERE";
        public const string offeritccreditscoredatamodel_insert = "INSERT INTO [2am].[dbo].[OfferITCCreditScore] (OfferCreditScoreKey, ITCCreditScoreKey, CreditScoreDecisionKey, ScoreDate, PrimaryApplicant) VALUES(@OfferCreditScoreKey, @ITCCreditScoreKey, @CreditScoreDecisionKey, @ScoreDate, @PrimaryApplicant); select cast(scope_identity() as int)";
        public const string offeritccreditscoredatamodel_update = "UPDATE [2am].[dbo].[OfferITCCreditScore] SET OfferCreditScoreKey = @OfferCreditScoreKey, ITCCreditScoreKey = @ITCCreditScoreKey, CreditScoreDecisionKey = @CreditScoreDecisionKey, ScoreDate = @ScoreDate, PrimaryApplicant = @PrimaryApplicant WHERE OfferITCCreditScoreKey = @OfferITCCreditScoreKey";



        public const string foreclosuredatamodel_selectwhere = "SELECT ForeclosureKey, AccountKey, GeneralStatusKey, ForeclosureOutcomeKey, ForeclosureDateTime FROM [2am].[dbo].[Foreclosure] WHERE";
        public const string foreclosuredatamodel_selectbykey = "SELECT ForeclosureKey, AccountKey, GeneralStatusKey, ForeclosureOutcomeKey, ForeclosureDateTime FROM [2am].[dbo].[Foreclosure] WHERE ForeclosureKey = @PrimaryKey";
        public const string foreclosuredatamodel_delete = "DELETE FROM [2am].[dbo].[Foreclosure] WHERE ForeclosureKey = @PrimaryKey";
        public const string foreclosuredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Foreclosure] WHERE";
        public const string foreclosuredatamodel_insert = "INSERT INTO [2am].[dbo].[Foreclosure] (AccountKey, GeneralStatusKey, ForeclosureOutcomeKey, ForeclosureDateTime) VALUES(@AccountKey, @GeneralStatusKey, @ForeclosureOutcomeKey, @ForeclosureDateTime); select cast(scope_identity() as int)";
        public const string foreclosuredatamodel_update = "UPDATE [2am].[dbo].[Foreclosure] SET AccountKey = @AccountKey, GeneralStatusKey = @GeneralStatusKey, ForeclosureOutcomeKey = @ForeclosureOutcomeKey, ForeclosureDateTime = @ForeclosureDateTime WHERE ForeclosureKey = @ForeclosureKey";



        public const string legalentityaffordabilitydatamodel_selectwhere = "SELECT LegalEntityAffordabilityKey, LegalEntityKey, AffordabilityTypeKey, Amount, Description, OfferKey FROM [2am].[dbo].[LegalEntityAffordability] WHERE";
        public const string legalentityaffordabilitydatamodel_selectbykey = "SELECT LegalEntityAffordabilityKey, LegalEntityKey, AffordabilityTypeKey, Amount, Description, OfferKey FROM [2am].[dbo].[LegalEntityAffordability] WHERE LegalEntityAffordabilityKey = @PrimaryKey";
        public const string legalentityaffordabilitydatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityAffordability] WHERE LegalEntityAffordabilityKey = @PrimaryKey";
        public const string legalentityaffordabilitydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityAffordability] WHERE";
        public const string legalentityaffordabilitydatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityAffordability] (LegalEntityKey, AffordabilityTypeKey, Amount, Description, OfferKey) VALUES(@LegalEntityKey, @AffordabilityTypeKey, @Amount, @Description, @OfferKey); select cast(scope_identity() as int)";
        public const string legalentityaffordabilitydatamodel_update = "UPDATE [2am].[dbo].[LegalEntityAffordability] SET LegalEntityKey = @LegalEntityKey, AffordabilityTypeKey = @AffordabilityTypeKey, Amount = @Amount, Description = @Description, OfferKey = @OfferKey WHERE LegalEntityAffordabilityKey = @LegalEntityAffordabilityKey";



        public const string offerexceptiontypedatamodel_selectwhere = "SELECT OfferExceptionTypeKey, Description, OfferExceptionTypeGroupKey FROM [2am].[dbo].[OfferExceptionType] WHERE";
        public const string offerexceptiontypedatamodel_selectbykey = "SELECT OfferExceptionTypeKey, Description, OfferExceptionTypeGroupKey FROM [2am].[dbo].[OfferExceptionType] WHERE OfferExceptionTypeKey = @PrimaryKey";
        public const string offerexceptiontypedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferExceptionType] WHERE OfferExceptionTypeKey = @PrimaryKey";
        public const string offerexceptiontypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferExceptionType] WHERE";
        public const string offerexceptiontypedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferExceptionType] (Description, OfferExceptionTypeGroupKey) VALUES(@Description, @OfferExceptionTypeGroupKey); select cast(scope_identity() as int)";
        public const string offerexceptiontypedatamodel_update = "UPDATE [2am].[dbo].[OfferExceptionType] SET Description = @Description, OfferExceptionTypeGroupKey = @OfferExceptionTypeGroupKey WHERE OfferExceptionTypeKey = @OfferExceptionTypeKey";



        public const string lifepremiumratesdatamodel_selectwhere = "SELECT NextAge, Death, LifePolicyTypeKey FROM [2am].[dbo].[LifePremiumRates] WHERE";
        public const string lifepremiumratesdatamodel_selectbykey = "SELECT NextAge, Death, LifePolicyTypeKey FROM [2am].[dbo].[LifePremiumRates] WHERE  = @PrimaryKey";
        public const string lifepremiumratesdatamodel_delete = "DELETE FROM [2am].[dbo].[LifePremiumRates] WHERE  = @PrimaryKey";
        public const string lifepremiumratesdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LifePremiumRates] WHERE";
        public const string lifepremiumratesdatamodel_insert = "INSERT INTO [2am].[dbo].[LifePremiumRates] (NextAge, Death, LifePolicyTypeKey) VALUES(@NextAge, @Death, @LifePolicyTypeKey); ";
        public const string lifepremiumratesdatamodel_update = "UPDATE [2am].[dbo].[LifePremiumRates] SET NextAge = @NextAge, Death = @Death, LifePolicyTypeKey = @LifePolicyTypeKey WHERE  = @";



        public const string conditionmappingtempdatamodel_selectwhere = "SELECT ConditionNumber, ConditionKey FROM [2am].[dbo].[ConditionMappingTEMP] WHERE";
        public const string conditionmappingtempdatamodel_selectbykey = "SELECT ConditionNumber, ConditionKey FROM [2am].[dbo].[ConditionMappingTEMP] WHERE  = @PrimaryKey";
        public const string conditionmappingtempdatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionMappingTEMP] WHERE  = @PrimaryKey";
        public const string conditionmappingtempdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionMappingTEMP] WHERE";
        public const string conditionmappingtempdatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionMappingTEMP] (ConditionNumber, ConditionKey) VALUES(@ConditionNumber, @ConditionKey); ";
        public const string conditionmappingtempdatamodel_update = "UPDATE [2am].[dbo].[ConditionMappingTEMP] SET ConditionNumber = @ConditionNumber, ConditionKey = @ConditionKey WHERE  = @";



        public const string stagedefinitioncompositelogicdatamodel_selectwhere = "SELECT StageDefinitionCompositeLogicKey, StageDefinitionCompositeKey, [Order], OperatorKey, StageDefinitionStageDefinitionGroupKey FROM [2am].[dbo].[StageDefinitionCompositeLogic] WHERE";
        public const string stagedefinitioncompositelogicdatamodel_selectbykey = "SELECT StageDefinitionCompositeLogicKey, StageDefinitionCompositeKey, [Order], OperatorKey, StageDefinitionStageDefinitionGroupKey FROM [2am].[dbo].[StageDefinitionCompositeLogic] WHERE StageDefinitionCompositeLogicKey = @PrimaryKey";
        public const string stagedefinitioncompositelogicdatamodel_delete = "DELETE FROM [2am].[dbo].[StageDefinitionCompositeLogic] WHERE StageDefinitionCompositeLogicKey = @PrimaryKey";
        public const string stagedefinitioncompositelogicdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StageDefinitionCompositeLogic] WHERE";
        public const string stagedefinitioncompositelogicdatamodel_insert = "INSERT INTO [2am].[dbo].[StageDefinitionCompositeLogic] (StageDefinitionCompositeKey, [Order], OperatorKey, StageDefinitionStageDefinitionGroupKey) VALUES(@StageDefinitionCompositeKey, @Order, @OperatorKey, @StageDefinitionStageDefinitionGroupKey); select cast(scope_identity() as int)";
        public const string stagedefinitioncompositelogicdatamodel_update = "UPDATE [2am].[dbo].[StageDefinitionCompositeLogic] SET StageDefinitionCompositeKey = @StageDefinitionCompositeKey, [Order] = @Order, OperatorKey = @OperatorKey, StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey WHERE StageDefinitionCompositeLogicKey = @StageDefinitionCompositeLogicKey";



        public const string captypedatamodel_selectwhere = "SELECT CapTypeKey, Description, Value FROM [2am].[dbo].[CapType] WHERE";
        public const string captypedatamodel_selectbykey = "SELECT CapTypeKey, Description, Value FROM [2am].[dbo].[CapType] WHERE CapTypeKey = @PrimaryKey";
        public const string captypedatamodel_delete = "DELETE FROM [2am].[dbo].[CapType] WHERE CapTypeKey = @PrimaryKey";
        public const string captypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CapType] WHERE";
        public const string captypedatamodel_insert = "INSERT INTO [2am].[dbo].[CapType] (Description, Value) VALUES(@Description, @Value); select cast(scope_identity() as int)";
        public const string captypedatamodel_update = "UPDATE [2am].[dbo].[CapType] SET Description = @Description, Value = @Value WHERE CapTypeKey = @CapTypeKey";



        public const string workflowdatamodel_selectwhere = "SELECT WorkflowKey, AccountKey, WorkflowStateKey, EntryDate, UserName FROM [2am].[dbo].[Workflow] WHERE";
        public const string workflowdatamodel_selectbykey = "SELECT WorkflowKey, AccountKey, WorkflowStateKey, EntryDate, UserName FROM [2am].[dbo].[Workflow] WHERE WorkflowKey = @PrimaryKey";
        public const string workflowdatamodel_delete = "DELETE FROM [2am].[dbo].[Workflow] WHERE WorkflowKey = @PrimaryKey";
        public const string workflowdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Workflow] WHERE";
        public const string workflowdatamodel_insert = "INSERT INTO [2am].[dbo].[Workflow] (WorkflowKey, AccountKey, WorkflowStateKey, EntryDate, UserName) VALUES(@WorkflowKey, @AccountKey, @WorkflowStateKey, @EntryDate, @UserName); ";
        public const string workflowdatamodel_update = "UPDATE [2am].[dbo].[Workflow] SET WorkflowKey = @WorkflowKey, AccountKey = @AccountKey, WorkflowStateKey = @WorkflowStateKey, EntryDate = @EntryDate, UserName = @UserName WHERE WorkflowKey = @WorkflowKey";



        public const string errorrepositorydatamodel_selectwhere = "SELECT ErrorRepositoryKey, Description, Active FROM [2am].[dbo].[ErrorRepository] WHERE";
        public const string errorrepositorydatamodel_selectbykey = "SELECT ErrorRepositoryKey, Description, Active FROM [2am].[dbo].[ErrorRepository] WHERE ErrorRepositoryKey = @PrimaryKey";
        public const string errorrepositorydatamodel_delete = "DELETE FROM [2am].[dbo].[ErrorRepository] WHERE ErrorRepositoryKey = @PrimaryKey";
        public const string errorrepositorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ErrorRepository] WHERE";
        public const string errorrepositorydatamodel_insert = "INSERT INTO [2am].[dbo].[ErrorRepository] (ErrorRepositoryKey, Description, Active) VALUES(@ErrorRepositoryKey, @Description, @Active); ";
        public const string errorrepositorydatamodel_update = "UPDATE [2am].[dbo].[ErrorRepository] SET ErrorRepositoryKey = @ErrorRepositoryKey, Description = @Description, Active = @Active WHERE ErrorRepositoryKey = @ErrorRepositoryKey";



        public const string conditionmigrationtempdatamodel_selectwhere = "SELECT OfferKey, ConditionNumber, ConditionPhrase, AfrikaansConditionPhrase FROM [2am].[dbo].[ConditionMigrationTEMP] WHERE";
        public const string conditionmigrationtempdatamodel_selectbykey = "SELECT OfferKey, ConditionNumber, ConditionPhrase, AfrikaansConditionPhrase FROM [2am].[dbo].[ConditionMigrationTEMP] WHERE  = @PrimaryKey";
        public const string conditionmigrationtempdatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionMigrationTEMP] WHERE  = @PrimaryKey";
        public const string conditionmigrationtempdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionMigrationTEMP] WHERE";
        public const string conditionmigrationtempdatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionMigrationTEMP] (OfferKey, ConditionNumber, ConditionPhrase, AfrikaansConditionPhrase) VALUES(@OfferKey, @ConditionNumber, @ConditionPhrase, @AfrikaansConditionPhrase); ";
        public const string conditionmigrationtempdatamodel_update = "UPDATE [2am].[dbo].[ConditionMigrationTEMP] SET OfferKey = @OfferKey, ConditionNumber = @ConditionNumber, ConditionPhrase = @ConditionPhrase, AfrikaansConditionPhrase = @AfrikaansConditionPhrase WHERE  = @";



        public const string conditionsetconditiondatamodel_selectwhere = "SELECT ConditionSetConditionKey, ConditionSetKey, ConditionKey, RequiredCondition FROM [2am].[dbo].[ConditionSetCondition] WHERE";
        public const string conditionsetconditiondatamodel_selectbykey = "SELECT ConditionSetConditionKey, ConditionSetKey, ConditionKey, RequiredCondition FROM [2am].[dbo].[ConditionSetCondition] WHERE ConditionSetConditionKey = @PrimaryKey";
        public const string conditionsetconditiondatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionSetCondition] WHERE ConditionSetConditionKey = @PrimaryKey";
        public const string conditionsetconditiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionSetCondition] WHERE";
        public const string conditionsetconditiondatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionSetCondition] (ConditionSetKey, ConditionKey, RequiredCondition) VALUES(@ConditionSetKey, @ConditionKey, @RequiredCondition); select cast(scope_identity() as int)";
        public const string conditionsetconditiondatamodel_update = "UPDATE [2am].[dbo].[ConditionSetCondition] SET ConditionSetKey = @ConditionSetKey, ConditionKey = @ConditionKey, RequiredCondition = @RequiredCondition WHERE ConditionSetConditionKey = @ConditionSetConditionKey";



        public const string disbursementtypedatamodel_selectwhere = "SELECT DisbursementTypeKey, Description FROM [2am].[dbo].[DisbursementType] WHERE";
        public const string disbursementtypedatamodel_selectbykey = "SELECT DisbursementTypeKey, Description FROM [2am].[dbo].[DisbursementType] WHERE DisbursementTypeKey = @PrimaryKey";
        public const string disbursementtypedatamodel_delete = "DELETE FROM [2am].[dbo].[DisbursementType] WHERE DisbursementTypeKey = @PrimaryKey";
        public const string disbursementtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DisbursementType] WHERE";
        public const string disbursementtypedatamodel_insert = "INSERT INTO [2am].[dbo].[DisbursementType] (DisbursementTypeKey, Description) VALUES(@DisbursementTypeKey, @Description); ";
        public const string disbursementtypedatamodel_update = "UPDATE [2am].[dbo].[DisbursementType] SET DisbursementTypeKey = @DisbursementTypeKey, Description = @Description WHERE DisbursementTypeKey = @DisbursementTypeKey";



        public const string audituistatementdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, StatementKey, ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate FROM [2am].[dbo].[AudituiStatement] WHERE";
        public const string audituistatementdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, StatementKey, ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate FROM [2am].[dbo].[AudituiStatement] WHERE AuditNumber = @PrimaryKey";
        public const string audituistatementdatamodel_delete = "DELETE FROM [2am].[dbo].[AudituiStatement] WHERE AuditNumber = @PrimaryKey";
        public const string audituistatementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AudituiStatement] WHERE";
        public const string audituistatementdatamodel_insert = "INSERT INTO [2am].[dbo].[AudituiStatement] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, StatementKey, ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @StatementKey, @ApplicationName, @StatementName, @ModifyDate, @Version, @ModifyUser, @Statement, @Type, @LastAccessedDate); select cast(scope_identity() as int)";
        public const string audituistatementdatamodel_update = "UPDATE [2am].[dbo].[AudituiStatement] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, StatementKey = @StatementKey, ApplicationName = @ApplicationName, StatementName = @StatementName, ModifyDate = @ModifyDate, Version = @Version, ModifyUser = @ModifyUser, Statement = @Statement, Type = @Type, LastAccessedDate = @LastAccessedDate WHERE AuditNumber = @AuditNumber";



        public const string foreclosureauctiondatamodel_selectwhere = "SELECT ForeclosureAuctionKey, ForeclosureInformationKey, GeneralStatusKey, ForeclosureAuctionOutcomeKey, AccountKey FROM [2am].[dbo].[ForeclosureAuction] WHERE";
        public const string foreclosureauctiondatamodel_selectbykey = "SELECT ForeclosureAuctionKey, ForeclosureInformationKey, GeneralStatusKey, ForeclosureAuctionOutcomeKey, AccountKey FROM [2am].[dbo].[ForeclosureAuction] WHERE ForeclosureAuctionKey = @PrimaryKey";
        public const string foreclosureauctiondatamodel_delete = "DELETE FROM [2am].[dbo].[ForeclosureAuction] WHERE ForeclosureAuctionKey = @PrimaryKey";
        public const string foreclosureauctiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ForeclosureAuction] WHERE";
        public const string foreclosureauctiondatamodel_insert = "INSERT INTO [2am].[dbo].[ForeclosureAuction] (ForeclosureInformationKey, GeneralStatusKey, ForeclosureAuctionOutcomeKey, AccountKey) VALUES(@ForeclosureInformationKey, @GeneralStatusKey, @ForeclosureAuctionOutcomeKey, @AccountKey); select cast(scope_identity() as int)";
        public const string foreclosureauctiondatamodel_update = "UPDATE [2am].[dbo].[ForeclosureAuction] SET ForeclosureInformationKey = @ForeclosureInformationKey, GeneralStatusKey = @GeneralStatusKey, ForeclosureAuctionOutcomeKey = @ForeclosureAuctionOutcomeKey, AccountKey = @AccountKey WHERE ForeclosureAuctionKey = @ForeclosureAuctionKey";



        public const string stagedefinitiongroupdatamodel_selectwhere = "SELECT StageDefinitionGroupKey, Description, GenericKeyTypeKey, GeneralStatusKey, ParentKey FROM [2am].[dbo].[StageDefinitionGroup] WHERE";
        public const string stagedefinitiongroupdatamodel_selectbykey = "SELECT StageDefinitionGroupKey, Description, GenericKeyTypeKey, GeneralStatusKey, ParentKey FROM [2am].[dbo].[StageDefinitionGroup] WHERE StageDefinitionGroupKey = @PrimaryKey";
        public const string stagedefinitiongroupdatamodel_delete = "DELETE FROM [2am].[dbo].[StageDefinitionGroup] WHERE StageDefinitionGroupKey = @PrimaryKey";
        public const string stagedefinitiongroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StageDefinitionGroup] WHERE";
        public const string stagedefinitiongroupdatamodel_insert = "INSERT INTO [2am].[dbo].[StageDefinitionGroup] (StageDefinitionGroupKey, Description, GenericKeyTypeKey, GeneralStatusKey, ParentKey) VALUES(@StageDefinitionGroupKey, @Description, @GenericKeyTypeKey, @GeneralStatusKey, @ParentKey); ";
        public const string stagedefinitiongroupdatamodel_update = "UPDATE [2am].[dbo].[StageDefinitionGroup] SET StageDefinitionGroupKey = @StageDefinitionGroupKey, Description = @Description, GenericKeyTypeKey = @GenericKeyTypeKey, GeneralStatusKey = @GeneralStatusKey, ParentKey = @ParentKey WHERE StageDefinitionGroupKey = @StageDefinitionGroupKey";



        public const string offerexceptiondatamodel_selectwhere = "SELECT OfferExceptionKey, OfferKey, OfferExceptionTypeKey, OverRidden FROM [2am].[dbo].[OfferException] WHERE";
        public const string offerexceptiondatamodel_selectbykey = "SELECT OfferExceptionKey, OfferKey, OfferExceptionTypeKey, OverRidden FROM [2am].[dbo].[OfferException] WHERE OfferExceptionKey = @PrimaryKey";
        public const string offerexceptiondatamodel_delete = "DELETE FROM [2am].[dbo].[OfferException] WHERE OfferExceptionKey = @PrimaryKey";
        public const string offerexceptiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferException] WHERE";
        public const string offerexceptiondatamodel_insert = "INSERT INTO [2am].[dbo].[OfferException] (OfferKey, OfferExceptionTypeKey, OverRidden) VALUES(@OfferKey, @OfferExceptionTypeKey, @OverRidden); select cast(scope_identity() as int)";
        public const string offerexceptiondatamodel_update = "UPDATE [2am].[dbo].[OfferException] SET OfferKey = @OfferKey, OfferExceptionTypeKey = @OfferExceptionTypeKey, OverRidden = @OverRidden WHERE OfferExceptionKey = @OfferExceptionKey";



        public const string rateadjustmentelementcreditcriteriadatamodel_selectwhere = "SELECT RateAdjustmentElementKey, CreditCriteriaKey FROM [2am].[dbo].[RateAdjustmentElementCreditCriteria] WHERE";
        public const string rateadjustmentelementcreditcriteriadatamodel_selectbykey = "SELECT RateAdjustmentElementKey, CreditCriteriaKey FROM [2am].[dbo].[RateAdjustmentElementCreditCriteria] WHERE  = @PrimaryKey";
        public const string rateadjustmentelementcreditcriteriadatamodel_delete = "DELETE FROM [2am].[dbo].[RateAdjustmentElementCreditCriteria] WHERE  = @PrimaryKey";
        public const string rateadjustmentelementcreditcriteriadatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RateAdjustmentElementCreditCriteria] WHERE";
        public const string rateadjustmentelementcreditcriteriadatamodel_insert = "INSERT INTO [2am].[dbo].[RateAdjustmentElementCreditCriteria] (RateAdjustmentElementKey, CreditCriteriaKey) VALUES(@RateAdjustmentElementKey, @CreditCriteriaKey); ";
        public const string rateadjustmentelementcreditcriteriadatamodel_update = "UPDATE [2am].[dbo].[RateAdjustmentElementCreditCriteria] SET RateAdjustmentElementKey = @RateAdjustmentElementKey, CreditCriteriaKey = @CreditCriteriaKey WHERE  = @";



        public const string offerstatusdatamodel_selectwhere = "SELECT OfferStatusKey, Description FROM [2am].[dbo].[OfferStatus] WHERE";
        public const string offerstatusdatamodel_selectbykey = "SELECT OfferStatusKey, Description FROM [2am].[dbo].[OfferStatus] WHERE OfferStatusKey = @PrimaryKey";
        public const string offerstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferStatus] WHERE OfferStatusKey = @PrimaryKey";
        public const string offerstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferStatus] WHERE";
        public const string offerstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferStatus] (OfferStatusKey, Description) VALUES(@OfferStatusKey, @Description); ";
        public const string offerstatusdatamodel_update = "UPDATE [2am].[dbo].[OfferStatus] SET OfferStatusKey = @OfferStatusKey, Description = @Description WHERE OfferStatusKey = @OfferStatusKey";



        public const string conditionlisttempdatamodel_selectwhere = "SELECT pk, FixedConditionNumber, ConditionNumber, ConditionTypeNumber, ProspectNumber, LoanNumber, OfferKey FROM [2am].[dbo].[ConditionListTEMP] WHERE";
        public const string conditionlisttempdatamodel_selectbykey = "SELECT pk, FixedConditionNumber, ConditionNumber, ConditionTypeNumber, ProspectNumber, LoanNumber, OfferKey FROM [2am].[dbo].[ConditionListTEMP] WHERE pk = @PrimaryKey";
        public const string conditionlisttempdatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionListTEMP] WHERE pk = @PrimaryKey";
        public const string conditionlisttempdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionListTEMP] WHERE";
        public const string conditionlisttempdatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionListTEMP] (FixedConditionNumber, ConditionNumber, ConditionTypeNumber, ProspectNumber, LoanNumber, OfferKey) VALUES(@FixedConditionNumber, @ConditionNumber, @ConditionTypeNumber, @ProspectNumber, @LoanNumber, @OfferKey); select cast(scope_identity() as int)";
        public const string conditionlisttempdatamodel_update = "UPDATE [2am].[dbo].[ConditionListTEMP] SET FixedConditionNumber = @FixedConditionNumber, ConditionNumber = @ConditionNumber, ConditionTypeNumber = @ConditionTypeNumber, ProspectNumber = @ProspectNumber, LoanNumber = @LoanNumber, OfferKey = @OfferKey WHERE pk = @pk";



        public const string bankaccountdatamodel_selectwhere = "SELECT BankAccountKey, ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate FROM [2am].[dbo].[BankAccount] WHERE";
        public const string bankaccountdatamodel_selectbykey = "SELECT BankAccountKey, ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate FROM [2am].[dbo].[BankAccount] WHERE BankAccountKey = @PrimaryKey";
        public const string bankaccountdatamodel_delete = "DELETE FROM [2am].[dbo].[BankAccount] WHERE BankAccountKey = @PrimaryKey";
        public const string bankaccountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BankAccount] WHERE";
        public const string bankaccountdatamodel_insert = "INSERT INTO [2am].[dbo].[BankAccount] (ACBBranchCode, AccountNumber, ACBTypeNumber, AccountName, UserID, ChangeDate) VALUES(@ACBBranchCode, @AccountNumber, @ACBTypeNumber, @AccountName, @UserID, @ChangeDate); select cast(scope_identity() as int)";
        public const string bankaccountdatamodel_update = "UPDATE [2am].[dbo].[BankAccount] SET ACBBranchCode = @ACBBranchCode, AccountNumber = @AccountNumber, ACBTypeNumber = @ACBTypeNumber, AccountName = @AccountName, UserID = @UserID, ChangeDate = @ChangeDate WHERE BankAccountKey = @BankAccountKey";



        public const string invertedkeysdatamodel_selectwhere = "SELECT F1, ValuationKeyA, ValuationKeyB FROM [2am].[dbo].[InvertedKeys] WHERE";
        public const string invertedkeysdatamodel_selectbykey = "SELECT F1, ValuationKeyA, ValuationKeyB FROM [2am].[dbo].[InvertedKeys] WHERE  = @PrimaryKey";
        public const string invertedkeysdatamodel_delete = "DELETE FROM [2am].[dbo].[InvertedKeys] WHERE  = @PrimaryKey";
        public const string invertedkeysdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[InvertedKeys] WHERE";
        public const string invertedkeysdatamodel_insert = "INSERT INTO [2am].[dbo].[InvertedKeys] (F1, ValuationKeyA, ValuationKeyB) VALUES(@F1, @ValuationKeyA, @ValuationKeyB); ";
        public const string invertedkeysdatamodel_update = "UPDATE [2am].[dbo].[InvertedKeys] SET F1 = @F1, ValuationKeyA = @ValuationKeyA, ValuationKeyB = @ValuationKeyB WHERE  = @";



        public const string stagetransitioncompositedatamodel_selectwhere = "SELECT StageTransitionCompositeKey, StageTransitionKey, GenericKey, ADUserKey, TransitionDate, Comments, StageDefinitionStageDefinitionGroupKey, StageTransitionReasonKey FROM [2am].[dbo].[StageTransitionComposite] WHERE";
        public const string stagetransitioncompositedatamodel_selectbykey = "SELECT StageTransitionCompositeKey, StageTransitionKey, GenericKey, ADUserKey, TransitionDate, Comments, StageDefinitionStageDefinitionGroupKey, StageTransitionReasonKey FROM [2am].[dbo].[StageTransitionComposite] WHERE StageTransitionCompositeKey = @PrimaryKey";
        public const string stagetransitioncompositedatamodel_delete = "DELETE FROM [2am].[dbo].[StageTransitionComposite] WHERE StageTransitionCompositeKey = @PrimaryKey";
        public const string stagetransitioncompositedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[StageTransitionComposite] WHERE";
        public const string stagetransitioncompositedatamodel_insert = "INSERT INTO [2am].[dbo].[StageTransitionComposite] (StageTransitionKey, GenericKey, ADUserKey, TransitionDate, Comments, StageDefinitionStageDefinitionGroupKey, StageTransitionReasonKey) VALUES(@StageTransitionKey, @GenericKey, @ADUserKey, @TransitionDate, @Comments, @StageDefinitionStageDefinitionGroupKey, @StageTransitionReasonKey); select cast(scope_identity() as int)";
        public const string stagetransitioncompositedatamodel_update = "UPDATE [2am].[dbo].[StageTransitionComposite] SET StageTransitionKey = @StageTransitionKey, GenericKey = @GenericKey, ADUserKey = @ADUserKey, TransitionDate = @TransitionDate, Comments = @Comments, StageDefinitionStageDefinitionGroupKey = @StageDefinitionStageDefinitionGroupKey, StageTransitionReasonKey = @StageTransitionReasonKey WHERE StageTransitionCompositeKey = @StageTransitionCompositeKey";



        public const string featuredatamodel_selectwhere = "SELECT FeatureKey, ShortName, LongName, HasAccess, ParentKey, Sequence FROM [2am].[dbo].[Feature] WHERE";
        public const string featuredatamodel_selectbykey = "SELECT FeatureKey, ShortName, LongName, HasAccess, ParentKey, Sequence FROM [2am].[dbo].[Feature] WHERE FeatureKey = @PrimaryKey";
        public const string featuredatamodel_delete = "DELETE FROM [2am].[dbo].[Feature] WHERE FeatureKey = @PrimaryKey";
        public const string featuredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Feature] WHERE";
        public const string featuredatamodel_insert = "INSERT INTO [2am].[dbo].[Feature] (FeatureKey, ShortName, LongName, HasAccess, ParentKey, Sequence) VALUES(@FeatureKey, @ShortName, @LongName, @HasAccess, @ParentKey, @Sequence); ";
        public const string featuredatamodel_update = "UPDATE [2am].[dbo].[Feature] SET FeatureKey = @FeatureKey, ShortName = @ShortName, LongName = @LongName, HasAccess = @HasAccess, ParentKey = @ParentKey, Sequence = @Sequence WHERE FeatureKey = @FeatureKey";



        public const string offerroledatamodel_selectwhere = "SELECT OfferRoleKey, LegalEntityKey, OfferKey, OfferRoleTypeKey, GeneralStatusKey, StatusChangeDate FROM [2am].[dbo].[OfferRole] WHERE";
        public const string offerroledatamodel_selectbykey = "SELECT OfferRoleKey, LegalEntityKey, OfferKey, OfferRoleTypeKey, GeneralStatusKey, StatusChangeDate FROM [2am].[dbo].[OfferRole] WHERE OfferRoleKey = @PrimaryKey";
        public const string offerroledatamodel_delete = "DELETE FROM [2am].[dbo].[OfferRole] WHERE OfferRoleKey = @PrimaryKey";
        public const string offerroledatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferRole] WHERE";
        public const string offerroledatamodel_insert = "INSERT INTO [2am].[dbo].[OfferRole] (LegalEntityKey, OfferKey, OfferRoleTypeKey, GeneralStatusKey, StatusChangeDate) VALUES(@LegalEntityKey, @OfferKey, @OfferRoleTypeKey, @GeneralStatusKey, @StatusChangeDate); select cast(scope_identity() as int)";
        public const string offerroledatamodel_update = "UPDATE [2am].[dbo].[OfferRole] SET LegalEntityKey = @LegalEntityKey, OfferKey = @OfferKey, OfferRoleTypeKey = @OfferRoleTypeKey, GeneralStatusKey = @GeneralStatusKey, StatusChangeDate = @StatusChangeDate WHERE OfferRoleKey = @OfferRoleKey";



        public const string validatortypedatamodel_selectwhere = "SELECT ValidatorTypeKey, Description FROM [2am].[dbo].[ValidatorType] WHERE";
        public const string validatortypedatamodel_selectbykey = "SELECT ValidatorTypeKey, Description FROM [2am].[dbo].[ValidatorType] WHERE ValidatorTypeKey = @PrimaryKey";
        public const string validatortypedatamodel_delete = "DELETE FROM [2am].[dbo].[ValidatorType] WHERE ValidatorTypeKey = @PrimaryKey";
        public const string validatortypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValidatorType] WHERE";
        public const string validatortypedatamodel_insert = "INSERT INTO [2am].[dbo].[ValidatorType] (ValidatorTypeKey, Description) VALUES(@ValidatorTypeKey, @Description); ";
        public const string validatortypedatamodel_update = "UPDATE [2am].[dbo].[ValidatorType] SET ValidatorTypeKey = @ValidatorTypeKey, Description = @Description WHERE ValidatorTypeKey = @ValidatorTypeKey";



        public const string employerdatamodel_selectwhere = "SELECT EmployerKey, Name, TelephoneNumber, TelephoneCode, ContactPerson, ContactEmail, AccountantName, AccountantContactPerson, AccountantTelephoneCode, AccountantTelephoneNumber, AccountantEmail, EmployerBusinessTypeKey, UserID, ChangeDate, EmploymentSectorKey FROM [2am].[dbo].[Employer] WHERE";
        public const string employerdatamodel_selectbykey = "SELECT EmployerKey, Name, TelephoneNumber, TelephoneCode, ContactPerson, ContactEmail, AccountantName, AccountantContactPerson, AccountantTelephoneCode, AccountantTelephoneNumber, AccountantEmail, EmployerBusinessTypeKey, UserID, ChangeDate, EmploymentSectorKey FROM [2am].[dbo].[Employer] WHERE EmployerKey = @PrimaryKey";
        public const string employerdatamodel_delete = "DELETE FROM [2am].[dbo].[Employer] WHERE EmployerKey = @PrimaryKey";
        public const string employerdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Employer] WHERE";
        public const string employerdatamodel_insert = "INSERT INTO [2am].[dbo].[Employer] (Name, TelephoneNumber, TelephoneCode, ContactPerson, ContactEmail, AccountantName, AccountantContactPerson, AccountantTelephoneCode, AccountantTelephoneNumber, AccountantEmail, EmployerBusinessTypeKey, UserID, ChangeDate, EmploymentSectorKey) VALUES(@Name, @TelephoneNumber, @TelephoneCode, @ContactPerson, @ContactEmail, @AccountantName, @AccountantContactPerson, @AccountantTelephoneCode, @AccountantTelephoneNumber, @AccountantEmail, @EmployerBusinessTypeKey, @UserID, @ChangeDate, @EmploymentSectorKey); select cast(scope_identity() as int)";
        public const string employerdatamodel_update = "UPDATE [2am].[dbo].[Employer] SET Name = @Name, TelephoneNumber = @TelephoneNumber, TelephoneCode = @TelephoneCode, ContactPerson = @ContactPerson, ContactEmail = @ContactEmail, AccountantName = @AccountantName, AccountantContactPerson = @AccountantContactPerson, AccountantTelephoneCode = @AccountantTelephoneCode, AccountantTelephoneNumber = @AccountantTelephoneNumber, AccountantEmail = @AccountantEmail, EmployerBusinessTypeKey = @EmployerBusinessTypeKey, UserID = @UserID, ChangeDate = @ChangeDate, EmploymentSectorKey = @EmploymentSectorKey WHERE EmployerKey = @EmployerKey";



        public const string offerconditiondatamodel_selectwhere = "SELECT OfferConditionKey, OfferKey, ConditionKey, TranslatableItemKey, ConditionSetKey FROM [2am].[dbo].[OfferCondition] WHERE";
        public const string offerconditiondatamodel_selectbykey = "SELECT OfferConditionKey, OfferKey, ConditionKey, TranslatableItemKey, ConditionSetKey FROM [2am].[dbo].[OfferCondition] WHERE OfferConditionKey = @PrimaryKey";
        public const string offerconditiondatamodel_delete = "DELETE FROM [2am].[dbo].[OfferCondition] WHERE OfferConditionKey = @PrimaryKey";
        public const string offerconditiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferCondition] WHERE";
        public const string offerconditiondatamodel_insert = "INSERT INTO [2am].[dbo].[OfferCondition] (OfferKey, ConditionKey, TranslatableItemKey, ConditionSetKey) VALUES(@OfferKey, @ConditionKey, @TranslatableItemKey, @ConditionSetKey); select cast(scope_identity() as int)";
        public const string offerconditiondatamodel_update = "UPDATE [2am].[dbo].[OfferCondition] SET OfferKey = @OfferKey, ConditionKey = @ConditionKey, TranslatableItemKey = @TranslatableItemKey, ConditionSetKey = @ConditionSetKey WHERE OfferConditionKey = @OfferConditionKey";



        public const string foreclosureauctioninformationdatamodel_selectwhere = "SELECT ForeclosureAuctionInformationKey, AuctionCompanyLegalEntityKey, ForeclosureAuctionOutcomeKey, MemoKey, ADUserKey, ForeclosureAuctionKey, AuctionDateTime, ForcedSaleValue, ReservePrice, SalePrice, LossApproved, BalanceAtOutcome, ChangeDate, WorkflowUser FROM [2am].[dbo].[ForeclosureAuctionInformation] WHERE";
        public const string foreclosureauctioninformationdatamodel_selectbykey = "SELECT ForeclosureAuctionInformationKey, AuctionCompanyLegalEntityKey, ForeclosureAuctionOutcomeKey, MemoKey, ADUserKey, ForeclosureAuctionKey, AuctionDateTime, ForcedSaleValue, ReservePrice, SalePrice, LossApproved, BalanceAtOutcome, ChangeDate, WorkflowUser FROM [2am].[dbo].[ForeclosureAuctionInformation] WHERE ForeclosureAuctionInformationKey = @PrimaryKey";
        public const string foreclosureauctioninformationdatamodel_delete = "DELETE FROM [2am].[dbo].[ForeclosureAuctionInformation] WHERE ForeclosureAuctionInformationKey = @PrimaryKey";
        public const string foreclosureauctioninformationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ForeclosureAuctionInformation] WHERE";
        public const string foreclosureauctioninformationdatamodel_insert = "INSERT INTO [2am].[dbo].[ForeclosureAuctionInformation] (AuctionCompanyLegalEntityKey, ForeclosureAuctionOutcomeKey, MemoKey, ADUserKey, ForeclosureAuctionKey, AuctionDateTime, ForcedSaleValue, ReservePrice, SalePrice, LossApproved, BalanceAtOutcome, ChangeDate, WorkflowUser) VALUES(@AuctionCompanyLegalEntityKey, @ForeclosureAuctionOutcomeKey, @MemoKey, @ADUserKey, @ForeclosureAuctionKey, @AuctionDateTime, @ForcedSaleValue, @ReservePrice, @SalePrice, @LossApproved, @BalanceAtOutcome, @ChangeDate, @WorkflowUser); select cast(scope_identity() as int)";
        public const string foreclosureauctioninformationdatamodel_update = "UPDATE [2am].[dbo].[ForeclosureAuctionInformation] SET AuctionCompanyLegalEntityKey = @AuctionCompanyLegalEntityKey, ForeclosureAuctionOutcomeKey = @ForeclosureAuctionOutcomeKey, MemoKey = @MemoKey, ADUserKey = @ADUserKey, ForeclosureAuctionKey = @ForeclosureAuctionKey, AuctionDateTime = @AuctionDateTime, ForcedSaleValue = @ForcedSaleValue, ReservePrice = @ReservePrice, SalePrice = @SalePrice, LossApproved = @LossApproved, BalanceAtOutcome = @BalanceAtOutcome, ChangeDate = @ChangeDate, WorkflowUser = @WorkflowUser WHERE ForeclosureAuctionInformationKey = @ForeclosureAuctionInformationKey";



        public const string auditfinancialservicebankaccountdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceBankAccountKey, FinancialServiceKey, BankAccountKey, Percentage, DebitOrderDay, GeneralStatusKey, UserID, ChangeDate, FinancialServicePaymentTypeKey, PaymentSplitTypeKey, ProviderKey, IsNaedoCompliant FROM [2am].[dbo].[AuditFinancialServiceBankAccount] WHERE";
        public const string auditfinancialservicebankaccountdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceBankAccountKey, FinancialServiceKey, BankAccountKey, Percentage, DebitOrderDay, GeneralStatusKey, UserID, ChangeDate, FinancialServicePaymentTypeKey, PaymentSplitTypeKey, ProviderKey, IsNaedoCompliant FROM [2am].[dbo].[AuditFinancialServiceBankAccount] WHERE AuditNumber = @PrimaryKey";
        public const string auditfinancialservicebankaccountdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditFinancialServiceBankAccount] WHERE AuditNumber = @PrimaryKey";
        public const string auditfinancialservicebankaccountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditFinancialServiceBankAccount] WHERE";
        public const string auditfinancialservicebankaccountdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditFinancialServiceBankAccount] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceBankAccountKey, FinancialServiceKey, BankAccountKey, Percentage, DebitOrderDay, GeneralStatusKey, UserID, ChangeDate, FinancialServicePaymentTypeKey, PaymentSplitTypeKey, ProviderKey, IsNaedoCompliant) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @FinancialServiceBankAccountKey, @FinancialServiceKey, @BankAccountKey, @Percentage, @DebitOrderDay, @GeneralStatusKey, @UserID, @ChangeDate, @FinancialServicePaymentTypeKey, @PaymentSplitTypeKey, @ProviderKey, @IsNaedoCompliant); select cast(scope_identity() as int)";
        public const string auditfinancialservicebankaccountdatamodel_update = "UPDATE [2am].[dbo].[AuditFinancialServiceBankAccount] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, FinancialServiceBankAccountKey = @FinancialServiceBankAccountKey, FinancialServiceKey = @FinancialServiceKey, BankAccountKey = @BankAccountKey, Percentage = @Percentage, DebitOrderDay = @DebitOrderDay, GeneralStatusKey = @GeneralStatusKey, UserID = @UserID, ChangeDate = @ChangeDate, FinancialServicePaymentTypeKey = @FinancialServicePaymentTypeKey, PaymentSplitTypeKey = @PaymentSplitTypeKey, ProviderKey = @ProviderKey, IsNaedoCompliant = @IsNaedoCompliant WHERE AuditNumber = @AuditNumber";



        public const string deedsofficedatamodel_selectwhere = "SELECT DeedsOfficeKey, Description FROM [2am].[dbo].[DeedsOffice] WHERE";
        public const string deedsofficedatamodel_selectbykey = "SELECT DeedsOfficeKey, Description FROM [2am].[dbo].[DeedsOffice] WHERE DeedsOfficeKey = @PrimaryKey";
        public const string deedsofficedatamodel_delete = "DELETE FROM [2am].[dbo].[DeedsOffice] WHERE DeedsOfficeKey = @PrimaryKey";
        public const string deedsofficedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DeedsOffice] WHERE";
        public const string deedsofficedatamodel_insert = "INSERT INTO [2am].[dbo].[DeedsOffice] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string deedsofficedatamodel_update = "UPDATE [2am].[dbo].[DeedsOffice] SET Description = @Description WHERE DeedsOfficeKey = @DeedsOfficeKey";



        public const string catspaymentbatchstatusdatamodel_selectwhere = "SELECT CATSPaymentBatchStatusKey, Description FROM [2am].[dbo].[CATSPaymentBatchStatus] WHERE";
        public const string catspaymentbatchstatusdatamodel_selectbykey = "SELECT CATSPaymentBatchStatusKey, Description FROM [2am].[dbo].[CATSPaymentBatchStatus] WHERE CATSPaymentBatchStatusKey = @PrimaryKey";
        public const string catspaymentbatchstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[CATSPaymentBatchStatus] WHERE CATSPaymentBatchStatusKey = @PrimaryKey";
        public const string catspaymentbatchstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CATSPaymentBatchStatus] WHERE";
        public const string catspaymentbatchstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[CATSPaymentBatchStatus] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string catspaymentbatchstatusdatamodel_update = "UPDATE [2am].[dbo].[CATSPaymentBatchStatus] SET Description = @Description WHERE CATSPaymentBatchStatusKey = @CATSPaymentBatchStatusKey";



        public const string itcdecisionreasondatamodel_selectwhere = "SELECT ITCDecisionReasonKey, ITCCreditScoreKey, OfferCreditScoreKey, CreditScoreDecisionKey, ReasonKey FROM [2am].[dbo].[ITCDecisionReason] WHERE";
        public const string itcdecisionreasondatamodel_selectbykey = "SELECT ITCDecisionReasonKey, ITCCreditScoreKey, OfferCreditScoreKey, CreditScoreDecisionKey, ReasonKey FROM [2am].[dbo].[ITCDecisionReason] WHERE ITCDecisionReasonKey = @PrimaryKey";
        public const string itcdecisionreasondatamodel_delete = "DELETE FROM [2am].[dbo].[ITCDecisionReason] WHERE ITCDecisionReasonKey = @PrimaryKey";
        public const string itcdecisionreasondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ITCDecisionReason] WHERE";
        public const string itcdecisionreasondatamodel_insert = "INSERT INTO [2am].[dbo].[ITCDecisionReason] (ITCCreditScoreKey, OfferCreditScoreKey, CreditScoreDecisionKey, ReasonKey) VALUES(@ITCCreditScoreKey, @OfferCreditScoreKey, @CreditScoreDecisionKey, @ReasonKey); select cast(scope_identity() as int)";
        public const string itcdecisionreasondatamodel_update = "UPDATE [2am].[dbo].[ITCDecisionReason] SET ITCCreditScoreKey = @ITCCreditScoreKey, OfferCreditScoreKey = @OfferCreditScoreKey, CreditScoreDecisionKey = @CreditScoreDecisionKey, ReasonKey = @ReasonKey WHERE ITCDecisionReasonKey = @ITCDecisionReasonKey";



        public const string businessruleareadatamodel_selectwhere = "SELECT BusinessRuleAreaKey, Description FROM [2am].[dbo].[BusinessRuleArea] WHERE";
        public const string businessruleareadatamodel_selectbykey = "SELECT BusinessRuleAreaKey, Description FROM [2am].[dbo].[BusinessRuleArea] WHERE BusinessRuleAreaKey = @PrimaryKey";
        public const string businessruleareadatamodel_delete = "DELETE FROM [2am].[dbo].[BusinessRuleArea] WHERE BusinessRuleAreaKey = @PrimaryKey";
        public const string businessruleareadatamodel_deletewhere = "DELETE FROM [2am].[dbo].[BusinessRuleArea] WHERE";
        public const string businessruleareadatamodel_insert = "INSERT INTO [2am].[dbo].[BusinessRuleArea] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string businessruleareadatamodel_update = "UPDATE [2am].[dbo].[BusinessRuleArea] SET Description = @Description WHERE BusinessRuleAreaKey = @BusinessRuleAreaKey";



        public const string offertypedatamodel_selectwhere = "SELECT OfferTypeKey, Description FROM [2am].[dbo].[OfferType] WHERE";
        public const string offertypedatamodel_selectbykey = "SELECT OfferTypeKey, Description FROM [2am].[dbo].[OfferType] WHERE OfferTypeKey = @PrimaryKey";
        public const string offertypedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferType] WHERE OfferTypeKey = @PrimaryKey";
        public const string offertypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferType] WHERE";
        public const string offertypedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferType] (OfferTypeKey, Description) VALUES(@OfferTypeKey, @Description); ";
        public const string offertypedatamodel_update = "UPDATE [2am].[dbo].[OfferType] SET OfferTypeKey = @OfferTypeKey, Description = @Description WHERE OfferTypeKey = @OfferTypeKey";



        public const string validatordatamodel_selectwhere = "SELECT ValidatorKey, DomainFieldKey, ValidatorTypeKey, ErrorRepositoryKey, InitialValue, RegularExpression, MinimumValue, MaximumValue FROM [2am].[dbo].[Validator] WHERE";
        public const string validatordatamodel_selectbykey = "SELECT ValidatorKey, DomainFieldKey, ValidatorTypeKey, ErrorRepositoryKey, InitialValue, RegularExpression, MinimumValue, MaximumValue FROM [2am].[dbo].[Validator] WHERE ValidatorKey = @PrimaryKey";
        public const string validatordatamodel_delete = "DELETE FROM [2am].[dbo].[Validator] WHERE ValidatorKey = @PrimaryKey";
        public const string validatordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Validator] WHERE";
        public const string validatordatamodel_insert = "INSERT INTO [2am].[dbo].[Validator] (DomainFieldKey, ValidatorTypeKey, ErrorRepositoryKey, InitialValue, RegularExpression, MinimumValue, MaximumValue) VALUES(@DomainFieldKey, @ValidatorTypeKey, @ErrorRepositoryKey, @InitialValue, @RegularExpression, @MinimumValue, @MaximumValue); select cast(scope_identity() as int)";
        public const string validatordatamodel_update = "UPDATE [2am].[dbo].[Validator] SET DomainFieldKey = @DomainFieldKey, ValidatorTypeKey = @ValidatorTypeKey, ErrorRepositoryKey = @ErrorRepositoryKey, InitialValue = @InitialValue, RegularExpression = @RegularExpression, MinimumValue = @MinimumValue, MaximumValue = @MaximumValue WHERE ValidatorKey = @ValidatorKey";



        public const string financialservicedatamodel_selectwhere = "SELECT FinancialServiceKey, AccountKey, Payment, FinancialServiceTypeKey, TradeKey, CategoryKey, AccountStatusKey, NextResetDate, ParentFinancialServiceKey, OpenDate, CloseDate FROM [2am].[dbo].[FinancialService] WHERE";
        public const string financialservicedatamodel_selectbykey = "SELECT FinancialServiceKey, AccountKey, Payment, FinancialServiceTypeKey, TradeKey, CategoryKey, AccountStatusKey, NextResetDate, ParentFinancialServiceKey, OpenDate, CloseDate FROM [2am].[dbo].[FinancialService] WHERE FinancialServiceKey = @PrimaryKey";
        public const string financialservicedatamodel_delete = "DELETE FROM [2am].[dbo].[FinancialService] WHERE FinancialServiceKey = @PrimaryKey";
        public const string financialservicedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FinancialService] WHERE";
        public const string financialservicedatamodel_insert = "INSERT INTO [2am].[dbo].[FinancialService] (AccountKey, Payment, FinancialServiceTypeKey, TradeKey, CategoryKey, AccountStatusKey, NextResetDate, ParentFinancialServiceKey, OpenDate, CloseDate) VALUES(@AccountKey, @Payment, @FinancialServiceTypeKey, @TradeKey, @CategoryKey, @AccountStatusKey, @NextResetDate, @ParentFinancialServiceKey, @OpenDate, @CloseDate); select cast(scope_identity() as int)";
        public const string financialservicedatamodel_update = "UPDATE [2am].[dbo].[FinancialService] SET AccountKey = @AccountKey, Payment = @Payment, FinancialServiceTypeKey = @FinancialServiceTypeKey, TradeKey = @TradeKey, CategoryKey = @CategoryKey, AccountStatusKey = @AccountStatusKey, NextResetDate = @NextResetDate, ParentFinancialServiceKey = @ParentFinancialServiceKey, OpenDate = @OpenDate, CloseDate = @CloseDate WHERE FinancialServiceKey = @FinancialServiceKey";



        public const string educationdatamodel_selectwhere = "SELECT EducationKey, Description FROM [2am].[dbo].[Education] WHERE";
        public const string educationdatamodel_selectbykey = "SELECT EducationKey, Description FROM [2am].[dbo].[Education] WHERE EducationKey = @PrimaryKey";
        public const string educationdatamodel_delete = "DELETE FROM [2am].[dbo].[Education] WHERE EducationKey = @PrimaryKey";
        public const string educationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Education] WHERE";
        public const string educationdatamodel_insert = "INSERT INTO [2am].[dbo].[Education] (EducationKey, Description) VALUES(@EducationKey, @Description); ";
        public const string educationdatamodel_update = "UPDATE [2am].[dbo].[Education] SET EducationKey = @EducationKey, Description = @Description WHERE EducationKey = @EducationKey";



        public const string foreclosureauctionoutcomedatamodel_selectwhere = "SELECT ForeclosureAuctionOutcomeKey, Description FROM [2am].[dbo].[ForeclosureAuctionOutcome] WHERE";
        public const string foreclosureauctionoutcomedatamodel_selectbykey = "SELECT ForeclosureAuctionOutcomeKey, Description FROM [2am].[dbo].[ForeclosureAuctionOutcome] WHERE ForeclosureAuctionOutcomeKey = @PrimaryKey";
        public const string foreclosureauctionoutcomedatamodel_delete = "DELETE FROM [2am].[dbo].[ForeclosureAuctionOutcome] WHERE ForeclosureAuctionOutcomeKey = @PrimaryKey";
        public const string foreclosureauctionoutcomedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ForeclosureAuctionOutcome] WHERE";
        public const string foreclosureauctionoutcomedatamodel_insert = "INSERT INTO [2am].[dbo].[ForeclosureAuctionOutcome] (ForeclosureAuctionOutcomeKey, Description) VALUES(@ForeclosureAuctionOutcomeKey, @Description); ";
        public const string foreclosureauctionoutcomedatamodel_update = "UPDATE [2am].[dbo].[ForeclosureAuctionOutcome] SET ForeclosureAuctionOutcomeKey = @ForeclosureAuctionOutcomeKey, Description = @Description WHERE ForeclosureAuctionOutcomeKey = @ForeclosureAuctionOutcomeKey";



        public const string corebusinessobjectmenudatamodel_selectwhere = "SELECT CoreBusinessObjectKey, ParentKey, Description, NodeType, URL, StatementNameKey, Sequence, MenuIcon, FeatureKey, HasOriginationSource, IsRemovable, ExpandLevel, IncludeParentHeaderIcons, GenericKeyTypeKey FROM [2am].[dbo].[CoreBusinessObjectMenu] WHERE";
        public const string corebusinessobjectmenudatamodel_selectbykey = "SELECT CoreBusinessObjectKey, ParentKey, Description, NodeType, URL, StatementNameKey, Sequence, MenuIcon, FeatureKey, HasOriginationSource, IsRemovable, ExpandLevel, IncludeParentHeaderIcons, GenericKeyTypeKey FROM [2am].[dbo].[CoreBusinessObjectMenu] WHERE CoreBusinessObjectKey = @PrimaryKey";
        public const string corebusinessobjectmenudatamodel_delete = "DELETE FROM [2am].[dbo].[CoreBusinessObjectMenu] WHERE CoreBusinessObjectKey = @PrimaryKey";
        public const string corebusinessobjectmenudatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CoreBusinessObjectMenu] WHERE";
        public const string corebusinessobjectmenudatamodel_insert = "INSERT INTO [2am].[dbo].[CoreBusinessObjectMenu] (CoreBusinessObjectKey, ParentKey, Description, NodeType, URL, StatementNameKey, Sequence, MenuIcon, FeatureKey, HasOriginationSource, IsRemovable, ExpandLevel, IncludeParentHeaderIcons, GenericKeyTypeKey) VALUES(@CoreBusinessObjectKey, @ParentKey, @Description, @NodeType, @URL, @StatementNameKey, @Sequence, @MenuIcon, @FeatureKey, @HasOriginationSource, @IsRemovable, @ExpandLevel, @IncludeParentHeaderIcons, @GenericKeyTypeKey); ";
        public const string corebusinessobjectmenudatamodel_update = "UPDATE [2am].[dbo].[CoreBusinessObjectMenu] SET CoreBusinessObjectKey = @CoreBusinessObjectKey, ParentKey = @ParentKey, Description = @Description, NodeType = @NodeType, URL = @URL, StatementNameKey = @StatementNameKey, Sequence = @Sequence, MenuIcon = @MenuIcon, FeatureKey = @FeatureKey, HasOriginationSource = @HasOriginationSource, IsRemovable = @IsRemovable, ExpandLevel = @ExpandLevel, IncludeParentHeaderIcons = @IncludeParentHeaderIcons, GenericKeyTypeKey = @GenericKeyTypeKey WHERE CoreBusinessObjectKey = @CoreBusinessObjectKey";



        public const string catspaymentbatchdatamodel_selectwhere = "SELECT CATSPaymentBatchKey, CATSPaymentBatchTypeKey, CreatedDate, ProcessedDate, CATSPaymentBatchStatusKey, CATSFileSequenceNo, CATSFileName FROM [2am].[dbo].[CATSPaymentBatch] WHERE";
        public const string catspaymentbatchdatamodel_selectbykey = "SELECT CATSPaymentBatchKey, CATSPaymentBatchTypeKey, CreatedDate, ProcessedDate, CATSPaymentBatchStatusKey, CATSFileSequenceNo, CATSFileName FROM [2am].[dbo].[CATSPaymentBatch] WHERE CATSPaymentBatchKey = @PrimaryKey";
        public const string catspaymentbatchdatamodel_delete = "DELETE FROM [2am].[dbo].[CATSPaymentBatch] WHERE CATSPaymentBatchKey = @PrimaryKey";
        public const string catspaymentbatchdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CATSPaymentBatch] WHERE";
        public const string catspaymentbatchdatamodel_insert = "INSERT INTO [2am].[dbo].[CATSPaymentBatch] (CATSPaymentBatchTypeKey, CreatedDate, ProcessedDate, CATSPaymentBatchStatusKey, CATSFileSequenceNo, CATSFileName) VALUES(@CATSPaymentBatchTypeKey, @CreatedDate, @ProcessedDate, @CATSPaymentBatchStatusKey, @CATSFileSequenceNo, @CATSFileName); select cast(scope_identity() as int)";
        public const string catspaymentbatchdatamodel_update = "UPDATE [2am].[dbo].[CATSPaymentBatch] SET CATSPaymentBatchTypeKey = @CATSPaymentBatchTypeKey, CreatedDate = @CreatedDate, ProcessedDate = @ProcessedDate, CATSPaymentBatchStatusKey = @CATSPaymentBatchStatusKey, CATSFileSequenceNo = @CATSFileSequenceNo, CATSFileName = @CATSFileName WHERE CATSPaymentBatchKey = @CATSPaymentBatchKey";



        public const string xmlhistorydatamodel_selectwhere = "SELECT XMLHistoryKey, GenericKeyTypeKey, GenericKey, XMLData, InsertDate FROM [2am].[dbo].[XMLHistory] WHERE";
        public const string xmlhistorydatamodel_selectbykey = "SELECT XMLHistoryKey, GenericKeyTypeKey, GenericKey, XMLData, InsertDate FROM [2am].[dbo].[XMLHistory] WHERE XMLHistoryKey = @PrimaryKey";
        public const string xmlhistorydatamodel_delete = "DELETE FROM [2am].[dbo].[XMLHistory] WHERE XMLHistoryKey = @PrimaryKey";
        public const string xmlhistorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[XMLHistory] WHERE";
        public const string xmlhistorydatamodel_insert = "INSERT INTO [2am].[dbo].[XMLHistory] (GenericKeyTypeKey, GenericKey, XMLData, InsertDate) VALUES(@GenericKeyTypeKey, @GenericKey, @XMLData, @InsertDate); select cast(scope_identity() as int)";
        public const string xmlhistorydatamodel_update = "UPDATE [2am].[dbo].[XMLHistory] SET GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey, XMLData = @XMLData, InsertDate = @InsertDate WHERE XMLHistoryKey = @XMLHistoryKey";



        public const string ruleparameterdatamodel_selectwhere = "SELECT RuleParameterKey, RuleItemKey, Name, ParameterTypeKey, Value FROM [2am].[dbo].[RuleParameter] WHERE";
        public const string ruleparameterdatamodel_selectbykey = "SELECT RuleParameterKey, RuleItemKey, Name, ParameterTypeKey, Value FROM [2am].[dbo].[RuleParameter] WHERE RuleParameterKey = @PrimaryKey";
        public const string ruleparameterdatamodel_delete = "DELETE FROM [2am].[dbo].[RuleParameter] WHERE RuleParameterKey = @PrimaryKey";
        public const string ruleparameterdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[RuleParameter] WHERE";
        public const string ruleparameterdatamodel_insert = "INSERT INTO [2am].[dbo].[RuleParameter] (RuleItemKey, Name, ParameterTypeKey, Value) VALUES(@RuleItemKey, @Name, @ParameterTypeKey, @Value); select cast(scope_identity() as int)";
        public const string ruleparameterdatamodel_update = "UPDATE [2am].[dbo].[RuleParameter] SET RuleItemKey = @RuleItemKey, Name = @Name, ParameterTypeKey = @ParameterTypeKey, Value = @Value WHERE RuleParameterKey = @RuleParameterKey";



        public const string reasondefinitiondatamodel_selectwhere = "SELECT ReasonDefinitionKey, ReasonTypeKey, AllowComment, ReasonDescriptionKey, EnforceComment, GeneralStatusKey FROM [2am].[dbo].[ReasonDefinition] WHERE";
        public const string reasondefinitiondatamodel_selectbykey = "SELECT ReasonDefinitionKey, ReasonTypeKey, AllowComment, ReasonDescriptionKey, EnforceComment, GeneralStatusKey FROM [2am].[dbo].[ReasonDefinition] WHERE ReasonDefinitionKey = @PrimaryKey";
        public const string reasondefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[ReasonDefinition] WHERE ReasonDefinitionKey = @PrimaryKey";
        public const string reasondefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ReasonDefinition] WHERE";
        public const string reasondefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[ReasonDefinition] (ReasonTypeKey, AllowComment, ReasonDescriptionKey, EnforceComment, GeneralStatusKey) VALUES(@ReasonTypeKey, @AllowComment, @ReasonDescriptionKey, @EnforceComment, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string reasondefinitiondatamodel_update = "UPDATE [2am].[dbo].[ReasonDefinition] SET ReasonTypeKey = @ReasonTypeKey, AllowComment = @AllowComment, ReasonDescriptionKey = @ReasonDescriptionKey, EnforceComment = @EnforceComment, GeneralStatusKey = @GeneralStatusKey WHERE ReasonDefinitionKey = @ReasonDefinitionKey";



        public const string auditvaluationdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ValuationKey, ValuatorKey, ValuationDate, ValuationAmount, ValuationHOCValue, ValuationMunicipal, ValuationUserID, PropertyKey, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, ChangeDate, ValuationClassificationKey, ValuationEscalationPercentage, ValuationStatusKey, Data, ValuationDataProviderDataServiceKey, IsActive, HOCRoofKey FROM [2am].[dbo].[AuditValuation] WHERE";
        public const string auditvaluationdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ValuationKey, ValuatorKey, ValuationDate, ValuationAmount, ValuationHOCValue, ValuationMunicipal, ValuationUserID, PropertyKey, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, ChangeDate, ValuationClassificationKey, ValuationEscalationPercentage, ValuationStatusKey, Data, ValuationDataProviderDataServiceKey, IsActive, HOCRoofKey FROM [2am].[dbo].[AuditValuation] WHERE AuditNumber = @PrimaryKey";
        public const string auditvaluationdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditValuation] WHERE AuditNumber = @PrimaryKey";
        public const string auditvaluationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditValuation] WHERE";
        public const string auditvaluationdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditValuation] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, ValuationKey, ValuatorKey, ValuationDate, ValuationAmount, ValuationHOCValue, ValuationMunicipal, ValuationUserID, PropertyKey, HOCThatchAmount, HOCConventionalAmount, HOCShingleAmount, ChangeDate, ValuationClassificationKey, ValuationEscalationPercentage, ValuationStatusKey, Data, ValuationDataProviderDataServiceKey, IsActive, HOCRoofKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @ValuationKey, @ValuatorKey, @ValuationDate, @ValuationAmount, @ValuationHOCValue, @ValuationMunicipal, @ValuationUserID, @PropertyKey, @HOCThatchAmount, @HOCConventionalAmount, @HOCShingleAmount, @ChangeDate, @ValuationClassificationKey, @ValuationEscalationPercentage, @ValuationStatusKey, @Data, @ValuationDataProviderDataServiceKey, @IsActive, @HOCRoofKey); select cast(scope_identity() as int)";
        public const string auditvaluationdatamodel_update = "UPDATE [2am].[dbo].[AuditValuation] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, ValuationKey = @ValuationKey, ValuatorKey = @ValuatorKey, ValuationDate = @ValuationDate, ValuationAmount = @ValuationAmount, ValuationHOCValue = @ValuationHOCValue, ValuationMunicipal = @ValuationMunicipal, ValuationUserID = @ValuationUserID, PropertyKey = @PropertyKey, HOCThatchAmount = @HOCThatchAmount, HOCConventionalAmount = @HOCConventionalAmount, HOCShingleAmount = @HOCShingleAmount, ChangeDate = @ChangeDate, ValuationClassificationKey = @ValuationClassificationKey, ValuationEscalationPercentage = @ValuationEscalationPercentage, ValuationStatusKey = @ValuationStatusKey, Data = @Data, ValuationDataProviderDataServiceKey = @ValuationDataProviderDataServiceKey, IsActive = @IsActive, HOCRoofKey = @HOCRoofKey WHERE AuditNumber = @AuditNumber";



        public const string foreclosureinformationdatamodel_selectwhere = "SELECT ForeclosureInformationKey, ForeclosureKey, ForeclosureOutcomeKey, AttorneyKey, AccountKey, SPVKey, CurrentBalance, LTV, MonthsInArrears, ForeclosureInformationDateTime FROM [2am].[dbo].[ForeclosureInformation] WHERE";
        public const string foreclosureinformationdatamodel_selectbykey = "SELECT ForeclosureInformationKey, ForeclosureKey, ForeclosureOutcomeKey, AttorneyKey, AccountKey, SPVKey, CurrentBalance, LTV, MonthsInArrears, ForeclosureInformationDateTime FROM [2am].[dbo].[ForeclosureInformation] WHERE ForeclosureInformationKey = @PrimaryKey";
        public const string foreclosureinformationdatamodel_delete = "DELETE FROM [2am].[dbo].[ForeclosureInformation] WHERE ForeclosureInformationKey = @PrimaryKey";
        public const string foreclosureinformationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ForeclosureInformation] WHERE";
        public const string foreclosureinformationdatamodel_insert = "INSERT INTO [2am].[dbo].[ForeclosureInformation] (ForeclosureKey, ForeclosureOutcomeKey, AttorneyKey, AccountKey, SPVKey, CurrentBalance, LTV, MonthsInArrears, ForeclosureInformationDateTime) VALUES(@ForeclosureKey, @ForeclosureOutcomeKey, @AttorneyKey, @AccountKey, @SPVKey, @CurrentBalance, @LTV, @MonthsInArrears, @ForeclosureInformationDateTime); select cast(scope_identity() as int)";
        public const string foreclosureinformationdatamodel_update = "UPDATE [2am].[dbo].[ForeclosureInformation] SET ForeclosureKey = @ForeclosureKey, ForeclosureOutcomeKey = @ForeclosureOutcomeKey, AttorneyKey = @AttorneyKey, AccountKey = @AccountKey, SPVKey = @SPVKey, CurrentBalance = @CurrentBalance, LTV = @LTV, MonthsInArrears = @MonthsInArrears, ForeclosureInformationDateTime = @ForeclosureInformationDateTime WHERE ForeclosureInformationKey = @ForeclosureInformationKey";



        public const string generalstatusdatamodel_selectwhere = "SELECT GeneralStatusKey, Description FROM [2am].[dbo].[GeneralStatus] WHERE";
        public const string generalstatusdatamodel_selectbykey = "SELECT GeneralStatusKey, Description FROM [2am].[dbo].[GeneralStatus] WHERE GeneralStatusKey = @PrimaryKey";
        public const string generalstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[GeneralStatus] WHERE GeneralStatusKey = @PrimaryKey";
        public const string generalstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[GeneralStatus] WHERE";
        public const string generalstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[GeneralStatus] (GeneralStatusKey, Description) VALUES(@GeneralStatusKey, @Description); ";
        public const string generalstatusdatamodel_update = "UPDATE [2am].[dbo].[GeneralStatus] SET GeneralStatusKey = @GeneralStatusKey, Description = @Description WHERE GeneralStatusKey = @GeneralStatusKey";



        public const string invoicelineitemcategorydatamodel_selectwhere = "SELECT InvoiceLineItemCategoryKey, InvoiceLineItemCategory FROM [2am].[dbo].[InvoiceLineItemCategory] WHERE";
        public const string invoicelineitemcategorydatamodel_selectbykey = "SELECT InvoiceLineItemCategoryKey, InvoiceLineItemCategory FROM [2am].[dbo].[InvoiceLineItemCategory] WHERE InvoiceLineItemCategoryKey = @PrimaryKey";
        public const string invoicelineitemcategorydatamodel_delete = "DELETE FROM [2am].[dbo].[InvoiceLineItemCategory] WHERE InvoiceLineItemCategoryKey = @PrimaryKey";
        public const string invoicelineitemcategorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[InvoiceLineItemCategory] WHERE";
        public const string invoicelineitemcategorydatamodel_insert = "INSERT INTO [2am].[dbo].[InvoiceLineItemCategory] (InvoiceLineItemCategoryKey, InvoiceLineItemCategory) VALUES(@InvoiceLineItemCategoryKey, @InvoiceLineItemCategory); ";
        public const string invoicelineitemcategorydatamodel_update = "UPDATE [2am].[dbo].[InvoiceLineItemCategory] SET InvoiceLineItemCategoryKey = @InvoiceLineItemCategoryKey, InvoiceLineItemCategory = @InvoiceLineItemCategory WHERE InvoiceLineItemCategoryKey = @InvoiceLineItemCategoryKey";



        public const string bonddatamodel_selectwhere = "SELECT BondKey, DeedsOfficeKey, AttorneyKey, BondRegistrationNumber, BondRegistrationDate, BondRegistrationAmount, BondLoanAgreementAmount, UserID, ChangeDate, OfferKey FROM [2am].[dbo].[Bond] WHERE";
        public const string bonddatamodel_selectbykey = "SELECT BondKey, DeedsOfficeKey, AttorneyKey, BondRegistrationNumber, BondRegistrationDate, BondRegistrationAmount, BondLoanAgreementAmount, UserID, ChangeDate, OfferKey FROM [2am].[dbo].[Bond] WHERE BondKey = @PrimaryKey";
        public const string bonddatamodel_delete = "DELETE FROM [2am].[dbo].[Bond] WHERE BondKey = @PrimaryKey";
        public const string bonddatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Bond] WHERE";
        public const string bonddatamodel_insert = "INSERT INTO [2am].[dbo].[Bond] (DeedsOfficeKey, AttorneyKey, BondRegistrationNumber, BondRegistrationDate, BondRegistrationAmount, BondLoanAgreementAmount, UserID, ChangeDate, OfferKey) VALUES(@DeedsOfficeKey, @AttorneyKey, @BondRegistrationNumber, @BondRegistrationDate, @BondRegistrationAmount, @BondLoanAgreementAmount, @UserID, @ChangeDate, @OfferKey); select cast(scope_identity() as int)";
        public const string bonddatamodel_update = "UPDATE [2am].[dbo].[Bond] SET DeedsOfficeKey = @DeedsOfficeKey, AttorneyKey = @AttorneyKey, BondRegistrationNumber = @BondRegistrationNumber, BondRegistrationDate = @BondRegistrationDate, BondRegistrationAmount = @BondRegistrationAmount, BondLoanAgreementAmount = @BondLoanAgreementAmount, UserID = @UserID, ChangeDate = @ChangeDate, OfferKey = @OfferKey WHERE BondKey = @BondKey";



        public const string employmentstatusdatamodel_selectwhere = "SELECT EmploymentStatusKey, Description FROM [2am].[dbo].[EmploymentStatus] WHERE";
        public const string employmentstatusdatamodel_selectbykey = "SELECT EmploymentStatusKey, Description FROM [2am].[dbo].[EmploymentStatus] WHERE EmploymentStatusKey = @PrimaryKey";
        public const string employmentstatusdatamodel_delete = "DELETE FROM [2am].[dbo].[EmploymentStatus] WHERE EmploymentStatusKey = @PrimaryKey";
        public const string employmentstatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[EmploymentStatus] WHERE";
        public const string employmentstatusdatamodel_insert = "INSERT INTO [2am].[dbo].[EmploymentStatus] (EmploymentStatusKey, Description) VALUES(@EmploymentStatusKey, @Description); ";
        public const string employmentstatusdatamodel_update = "UPDATE [2am].[dbo].[EmploymentStatus] SET EmploymentStatusKey = @EmploymentStatusKey, Description = @Description WHERE EmploymentStatusKey = @EmploymentStatusKey";



        public const string monthendstatsdatamodel_selectwhere = "SELECT MonthEndStatsKey, EntryDate, TotalOpenAccount, TotalOpenAccountCurrentBalance, TotalClosedAccounts, TotalClosedthismonth, TotalOpenedthisMonth, OpenSuperLowAccounts, OpenVariableAccounts, OpenVariFixAccounts, MonthEnd910Count, TotalOf910TranAmount, MonthEnd210Count, TotalOf210TranAmount, MonthEnd211Count, MonthEnd310Count, MonthEnd465Count, MonthEnd466Count, MonthEnd470Count, MonthEnd921Count, MonthEnd922Count, MonthEnd960Count, MonthEnd965Count, MonthEnd485Count, MonthEnd265Count FROM [2am].[dbo].[MonthEndStats] WHERE";
        public const string monthendstatsdatamodel_selectbykey = "SELECT MonthEndStatsKey, EntryDate, TotalOpenAccount, TotalOpenAccountCurrentBalance, TotalClosedAccounts, TotalClosedthismonth, TotalOpenedthisMonth, OpenSuperLowAccounts, OpenVariableAccounts, OpenVariFixAccounts, MonthEnd910Count, TotalOf910TranAmount, MonthEnd210Count, TotalOf210TranAmount, MonthEnd211Count, MonthEnd310Count, MonthEnd465Count, MonthEnd466Count, MonthEnd470Count, MonthEnd921Count, MonthEnd922Count, MonthEnd960Count, MonthEnd965Count, MonthEnd485Count, MonthEnd265Count FROM [2am].[dbo].[MonthEndStats] WHERE MonthEndStatsKey = @PrimaryKey";
        public const string monthendstatsdatamodel_delete = "DELETE FROM [2am].[dbo].[MonthEndStats] WHERE MonthEndStatsKey = @PrimaryKey";
        public const string monthendstatsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[MonthEndStats] WHERE";
        public const string monthendstatsdatamodel_insert = "INSERT INTO [2am].[dbo].[MonthEndStats] (EntryDate, TotalOpenAccount, TotalOpenAccountCurrentBalance, TotalClosedAccounts, TotalClosedthismonth, TotalOpenedthisMonth, OpenSuperLowAccounts, OpenVariableAccounts, OpenVariFixAccounts, MonthEnd910Count, TotalOf910TranAmount, MonthEnd210Count, TotalOf210TranAmount, MonthEnd211Count, MonthEnd310Count, MonthEnd465Count, MonthEnd466Count, MonthEnd470Count, MonthEnd921Count, MonthEnd922Count, MonthEnd960Count, MonthEnd965Count, MonthEnd485Count, MonthEnd265Count) VALUES(@EntryDate, @TotalOpenAccount, @TotalOpenAccountCurrentBalance, @TotalClosedAccounts, @TotalClosedthismonth, @TotalOpenedthisMonth, @OpenSuperLowAccounts, @OpenVariableAccounts, @OpenVariFixAccounts, @MonthEnd910Count, @TotalOf910TranAmount, @MonthEnd210Count, @TotalOf210TranAmount, @MonthEnd211Count, @MonthEnd310Count, @MonthEnd465Count, @MonthEnd466Count, @MonthEnd470Count, @MonthEnd921Count, @MonthEnd922Count, @MonthEnd960Count, @MonthEnd965Count, @MonthEnd485Count, @MonthEnd265Count); select cast(scope_identity() as int)";
        public const string monthendstatsdatamodel_update = "UPDATE [2am].[dbo].[MonthEndStats] SET EntryDate = @EntryDate, TotalOpenAccount = @TotalOpenAccount, TotalOpenAccountCurrentBalance = @TotalOpenAccountCurrentBalance, TotalClosedAccounts = @TotalClosedAccounts, TotalClosedthismonth = @TotalClosedthismonth, TotalOpenedthisMonth = @TotalOpenedthisMonth, OpenSuperLowAccounts = @OpenSuperLowAccounts, OpenVariableAccounts = @OpenVariableAccounts, OpenVariFixAccounts = @OpenVariFixAccounts, MonthEnd910Count = @MonthEnd910Count, TotalOf910TranAmount = @TotalOf910TranAmount, MonthEnd210Count = @MonthEnd210Count, TotalOf210TranAmount = @TotalOf210TranAmount, MonthEnd211Count = @MonthEnd211Count, MonthEnd310Count = @MonthEnd310Count, MonthEnd465Count = @MonthEnd465Count, MonthEnd466Count = @MonthEnd466Count, MonthEnd470Count = @MonthEnd470Count, MonthEnd921Count = @MonthEnd921Count, MonthEnd922Count = @MonthEnd922Count, MonthEnd960Count = @MonthEnd960Count, MonthEnd965Count = @MonthEnd965Count, MonthEnd485Count = @MonthEnd485Count, MonthEnd265Count = @MonthEnd265Count WHERE MonthEndStatsKey = @MonthEndStatsKey";



        public const string valuationimprovementtypedatamodel_selectwhere = "SELECT ValuationImprovementTypeKey, Description FROM [2am].[dbo].[ValuationImprovementType] WHERE";
        public const string valuationimprovementtypedatamodel_selectbykey = "SELECT ValuationImprovementTypeKey, Description FROM [2am].[dbo].[ValuationImprovementType] WHERE ValuationImprovementTypeKey = @PrimaryKey";
        public const string valuationimprovementtypedatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationImprovementType] WHERE ValuationImprovementTypeKey = @PrimaryKey";
        public const string valuationimprovementtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationImprovementType] WHERE";
        public const string valuationimprovementtypedatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationImprovementType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string valuationimprovementtypedatamodel_update = "UPDATE [2am].[dbo].[ValuationImprovementType] SET Description = @Description WHERE ValuationImprovementTypeKey = @ValuationImprovementTypeKey";



        public const string organisationtypedatamodel_selectwhere = "SELECT OrganisationTypeKey, Description FROM [2am].[dbo].[OrganisationType] WHERE";
        public const string organisationtypedatamodel_selectbykey = "SELECT OrganisationTypeKey, Description FROM [2am].[dbo].[OrganisationType] WHERE OrganisationTypeKey = @PrimaryKey";
        public const string organisationtypedatamodel_delete = "DELETE FROM [2am].[dbo].[OrganisationType] WHERE OrganisationTypeKey = @PrimaryKey";
        public const string organisationtypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OrganisationType] WHERE";
        public const string organisationtypedatamodel_insert = "INSERT INTO [2am].[dbo].[OrganisationType] (OrganisationTypeKey, Description) VALUES(@OrganisationTypeKey, @Description); ";
        public const string organisationtypedatamodel_update = "UPDATE [2am].[dbo].[OrganisationType] SET OrganisationTypeKey = @OrganisationTypeKey, Description = @Description WHERE OrganisationTypeKey = @OrganisationTypeKey";



        public const string flstagetrandatamodel_selectwhere = "SELECT GenericKey, ADUserKey, eStageName, eCreationTime FROM [2am].[dbo].[FLStageTran] WHERE";
        public const string flstagetrandatamodel_selectbykey = "SELECT GenericKey, ADUserKey, eStageName, eCreationTime FROM [2am].[dbo].[FLStageTran] WHERE  = @PrimaryKey";
        public const string flstagetrandatamodel_delete = "DELETE FROM [2am].[dbo].[FLStageTran] WHERE  = @PrimaryKey";
        public const string flstagetrandatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FLStageTran] WHERE";
        public const string flstagetrandatamodel_insert = "INSERT INTO [2am].[dbo].[FLStageTran] (GenericKey, ADUserKey, eStageName, eCreationTime) VALUES(@GenericKey, @ADUserKey, @eStageName, @eCreationTime); ";
        public const string flstagetrandatamodel_update = "UPDATE [2am].[dbo].[FLStageTran] SET GenericKey = @GenericKey, ADUserKey = @ADUserKey, eStageName = @eStageName, eCreationTime = @eCreationTime WHERE  = @";



        public const string foreclosureoutcomedatamodel_selectwhere = "SELECT ForeclosureOutcomeKey, Description FROM [2am].[dbo].[ForeclosureOutcome] WHERE";
        public const string foreclosureoutcomedatamodel_selectbykey = "SELECT ForeclosureOutcomeKey, Description FROM [2am].[dbo].[ForeclosureOutcome] WHERE ForeclosureOutcomeKey = @PrimaryKey";
        public const string foreclosureoutcomedatamodel_delete = "DELETE FROM [2am].[dbo].[ForeclosureOutcome] WHERE ForeclosureOutcomeKey = @PrimaryKey";
        public const string foreclosureoutcomedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ForeclosureOutcome] WHERE";
        public const string foreclosureoutcomedatamodel_insert = "INSERT INTO [2am].[dbo].[ForeclosureOutcome] (ForeclosureOutcomeKey, Description) VALUES(@ForeclosureOutcomeKey, @Description); ";
        public const string foreclosureoutcomedatamodel_update = "UPDATE [2am].[dbo].[ForeclosureOutcome] SET ForeclosureOutcomeKey = @ForeclosureOutcomeKey, Description = @Description WHERE ForeclosureOutcomeKey = @ForeclosureOutcomeKey";



        public const string auditfinancialservicerecurringtransactionsdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceRecurringTransactionKey, FinancialServiceKey, RecurringTransactionTypeKey, InsertDate, Frequency, TransactionTypeNumber, FrequencyType, NumUntilNextRun, Reference, Active, StartDate, Term, RemainingTerm, TransactionDay, HourOfRun, Amount, StatementName, PreviousRunDate, UserName, Notes, BankAccountKey FROM [2am].[dbo].[AuditFinancialServiceRecurringTransactions] WHERE";
        public const string auditfinancialservicerecurringtransactionsdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceRecurringTransactionKey, FinancialServiceKey, RecurringTransactionTypeKey, InsertDate, Frequency, TransactionTypeNumber, FrequencyType, NumUntilNextRun, Reference, Active, StartDate, Term, RemainingTerm, TransactionDay, HourOfRun, Amount, StatementName, PreviousRunDate, UserName, Notes, BankAccountKey FROM [2am].[dbo].[AuditFinancialServiceRecurringTransactions] WHERE AuditNumber = @PrimaryKey";
        public const string auditfinancialservicerecurringtransactionsdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditFinancialServiceRecurringTransactions] WHERE AuditNumber = @PrimaryKey";
        public const string auditfinancialservicerecurringtransactionsdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditFinancialServiceRecurringTransactions] WHERE";
        public const string auditfinancialservicerecurringtransactionsdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditFinancialServiceRecurringTransactions] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, FinancialServiceRecurringTransactionKey, FinancialServiceKey, RecurringTransactionTypeKey, InsertDate, Frequency, TransactionTypeNumber, FrequencyType, NumUntilNextRun, Reference, Active, StartDate, Term, RemainingTerm, TransactionDay, HourOfRun, Amount, StatementName, PreviousRunDate, UserName, Notes, BankAccountKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @FinancialServiceRecurringTransactionKey, @FinancialServiceKey, @RecurringTransactionTypeKey, @InsertDate, @Frequency, @TransactionTypeNumber, @FrequencyType, @NumUntilNextRun, @Reference, @Active, @StartDate, @Term, @RemainingTerm, @TransactionDay, @HourOfRun, @Amount, @StatementName, @PreviousRunDate, @UserName, @Notes, @BankAccountKey); select cast(scope_identity() as int)";
        public const string auditfinancialservicerecurringtransactionsdatamodel_update = "UPDATE [2am].[dbo].[AuditFinancialServiceRecurringTransactions] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, FinancialServiceRecurringTransactionKey = @FinancialServiceRecurringTransactionKey, FinancialServiceKey = @FinancialServiceKey, RecurringTransactionTypeKey = @RecurringTransactionTypeKey, InsertDate = @InsertDate, Frequency = @Frequency, TransactionTypeNumber = @TransactionTypeNumber, FrequencyType = @FrequencyType, NumUntilNextRun = @NumUntilNextRun, Reference = @Reference, Active = @Active, StartDate = @StartDate, Term = @Term, RemainingTerm = @RemainingTerm, TransactionDay = @TransactionDay, HourOfRun = @HourOfRun, Amount = @Amount, StatementName = @StatementName, PreviousRunDate = @PreviousRunDate, UserName = @UserName, Notes = @Notes, BankAccountKey = @BankAccountKey WHERE AuditNumber = @AuditNumber";



        public const string invoicelineitemdescriptiondatamodel_selectwhere = "SELECT InvoiceLineItemDescriptionKey, InvoiceLineItemCategoryKey, InvoiceLineItemDescription FROM [2am].[dbo].[InvoiceLineItemDescription] WHERE";
        public const string invoicelineitemdescriptiondatamodel_selectbykey = "SELECT InvoiceLineItemDescriptionKey, InvoiceLineItemCategoryKey, InvoiceLineItemDescription FROM [2am].[dbo].[InvoiceLineItemDescription] WHERE InvoiceLineItemDescriptionKey = @PrimaryKey";
        public const string invoicelineitemdescriptiondatamodel_delete = "DELETE FROM [2am].[dbo].[InvoiceLineItemDescription] WHERE InvoiceLineItemDescriptionKey = @PrimaryKey";
        public const string invoicelineitemdescriptiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[InvoiceLineItemDescription] WHERE";
        public const string invoicelineitemdescriptiondatamodel_insert = "INSERT INTO [2am].[dbo].[InvoiceLineItemDescription] (InvoiceLineItemCategoryKey, InvoiceLineItemDescription) VALUES(@InvoiceLineItemCategoryKey, @InvoiceLineItemDescription); select cast(scope_identity() as int)";
        public const string invoicelineitemdescriptiondatamodel_update = "UPDATE [2am].[dbo].[InvoiceLineItemDescription] SET InvoiceLineItemCategoryKey = @InvoiceLineItemCategoryKey, InvoiceLineItemDescription = @InvoiceLineItemDescription WHERE InvoiceLineItemDescriptionKey = @InvoiceLineItemDescriptionKey";



        public const string legalentitybankaccountdatamodel_selectwhere = "SELECT LegalEntityBankAccountKey, LegalEntityKey, BankAccountKey, GeneralStatusKey, UserID, ChangeDate FROM [2am].[dbo].[LegalEntityBankAccount] WHERE";
        public const string legalentitybankaccountdatamodel_selectbykey = "SELECT LegalEntityBankAccountKey, LegalEntityKey, BankAccountKey, GeneralStatusKey, UserID, ChangeDate FROM [2am].[dbo].[LegalEntityBankAccount] WHERE LegalEntityBankAccountKey = @PrimaryKey";
        public const string legalentitybankaccountdatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityBankAccount] WHERE LegalEntityBankAccountKey = @PrimaryKey";
        public const string legalentitybankaccountdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityBankAccount] WHERE";
        public const string legalentitybankaccountdatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityBankAccount] (LegalEntityKey, BankAccountKey, GeneralStatusKey, UserID, ChangeDate) VALUES(@LegalEntityKey, @BankAccountKey, @GeneralStatusKey, @UserID, @ChangeDate); select cast(scope_identity() as int)";
        public const string legalentitybankaccountdatamodel_update = "UPDATE [2am].[dbo].[LegalEntityBankAccount] SET LegalEntityKey = @LegalEntityKey, BankAccountKey = @BankAccountKey, GeneralStatusKey = @GeneralStatusKey, UserID = @UserID, ChangeDate = @ChangeDate WHERE LegalEntityBankAccountKey = @LegalEntityBankAccountKey";



        public const string controlgroupdatamodel_selectwhere = "SELECT ControlGroupKey, Description FROM [2am].[dbo].[ControlGroup] WHERE";
        public const string controlgroupdatamodel_selectbykey = "SELECT ControlGroupKey, Description FROM [2am].[dbo].[ControlGroup] WHERE ControlGroupKey = @PrimaryKey";
        public const string controlgroupdatamodel_delete = "DELETE FROM [2am].[dbo].[ControlGroup] WHERE ControlGroupKey = @PrimaryKey";
        public const string controlgroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ControlGroup] WHERE";
        public const string controlgroupdatamodel_insert = "INSERT INTO [2am].[dbo].[ControlGroup] (ControlGroupKey, Description) VALUES(@ControlGroupKey, @Description); ";
        public const string controlgroupdatamodel_update = "UPDATE [2am].[dbo].[ControlGroup] SET ControlGroupKey = @ControlGroupKey, Description = @Description WHERE ControlGroupKey = @ControlGroupKey";



        public const string valuationclassificationdatamodel_selectwhere = "SELECT ValuationClassificationKey, Description FROM [2am].[dbo].[ValuationClassification] WHERE";
        public const string valuationclassificationdatamodel_selectbykey = "SELECT ValuationClassificationKey, Description FROM [2am].[dbo].[ValuationClassification] WHERE ValuationClassificationKey = @PrimaryKey";
        public const string valuationclassificationdatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationClassification] WHERE ValuationClassificationKey = @PrimaryKey";
        public const string valuationclassificationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationClassification] WHERE";
        public const string valuationclassificationdatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationClassification] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string valuationclassificationdatamodel_update = "UPDATE [2am].[dbo].[ValuationClassification] SET Description = @Description WHERE ValuationClassificationKey = @ValuationClassificationKey";



        public const string contextmenudatamodel_selectwhere = "SELECT ContextKey, CoreBusinessObjectKey, ParentKey, Description, URL, FeatureKey, Sequence FROM [2am].[dbo].[ContextMenu] WHERE";
        public const string contextmenudatamodel_selectbykey = "SELECT ContextKey, CoreBusinessObjectKey, ParentKey, Description, URL, FeatureKey, Sequence FROM [2am].[dbo].[ContextMenu] WHERE ContextKey = @PrimaryKey";
        public const string contextmenudatamodel_delete = "DELETE FROM [2am].[dbo].[ContextMenu] WHERE ContextKey = @PrimaryKey";
        public const string contextmenudatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ContextMenu] WHERE";
        public const string contextmenudatamodel_insert = "INSERT INTO [2am].[dbo].[ContextMenu] (ContextKey, CoreBusinessObjectKey, ParentKey, Description, URL, FeatureKey, Sequence) VALUES(@ContextKey, @CoreBusinessObjectKey, @ParentKey, @Description, @URL, @FeatureKey, @Sequence); ";
        public const string contextmenudatamodel_update = "UPDATE [2am].[dbo].[ContextMenu] SET ContextKey = @ContextKey, CoreBusinessObjectKey = @CoreBusinessObjectKey, ParentKey = @ParentKey, Description = @Description, URL = @URL, FeatureKey = @FeatureKey, Sequence = @Sequence WHERE ContextKey = @ContextKey";



        public const string legalentityrelationshipdatamodel_selectwhere = "SELECT LegalEntityRelationshipKey, LegalEntityKey, RelatedLegalEntityKey, RelationshipTypeKey FROM [2am].[dbo].[LegalEntityRelationship] WHERE";
        public const string legalentityrelationshipdatamodel_selectbykey = "SELECT LegalEntityRelationshipKey, LegalEntityKey, RelatedLegalEntityKey, RelationshipTypeKey FROM [2am].[dbo].[LegalEntityRelationship] WHERE LegalEntityRelationshipKey = @PrimaryKey";
        public const string legalentityrelationshipdatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityRelationship] WHERE LegalEntityRelationshipKey = @PrimaryKey";
        public const string legalentityrelationshipdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityRelationship] WHERE";
        public const string legalentityrelationshipdatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityRelationship] (LegalEntityKey, RelatedLegalEntityKey, RelationshipTypeKey) VALUES(@LegalEntityKey, @RelatedLegalEntityKey, @RelationshipTypeKey); select cast(scope_identity() as int)";
        public const string legalentityrelationshipdatamodel_update = "UPDATE [2am].[dbo].[LegalEntityRelationship] SET LegalEntityKey = @LegalEntityKey, RelatedLegalEntityKey = @RelatedLegalEntityKey, RelationshipTypeKey = @RelationshipTypeKey WHERE LegalEntityRelationshipKey = @LegalEntityRelationshipKey";



        public const string hocreassurance_snapshotdatamodel_selectwhere = "SELECT MonthEndDate, PolicyNumber, LoanNumber, InsuredAmount, MonthlyPremium, ReassurancePremium, SASRIAAmount, SpvKey, HOCAdministrationFee, HOCProRataPremium, TotalHOCPremium, PostedHOCPremium FROM [2am].[dbo].[HOCReassurance_Snapshot] WHERE";
        public const string hocreassurance_snapshotdatamodel_selectbykey = "SELECT MonthEndDate, PolicyNumber, LoanNumber, InsuredAmount, MonthlyPremium, ReassurancePremium, SASRIAAmount, SpvKey, HOCAdministrationFee, HOCProRataPremium, TotalHOCPremium, PostedHOCPremium FROM [2am].[dbo].[HOCReassurance_Snapshot] WHERE  = @PrimaryKey";
        public const string hocreassurance_snapshotdatamodel_delete = "DELETE FROM [2am].[dbo].[HOCReassurance_Snapshot] WHERE  = @PrimaryKey";
        public const string hocreassurance_snapshotdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HOCReassurance_Snapshot] WHERE";
        public const string hocreassurance_snapshotdatamodel_insert = "INSERT INTO [2am].[dbo].[HOCReassurance_Snapshot] (MonthEndDate, PolicyNumber, LoanNumber, InsuredAmount, MonthlyPremium, ReassurancePremium, SASRIAAmount, SpvKey, HOCAdministrationFee, HOCProRataPremium, TotalHOCPremium, PostedHOCPremium) VALUES(@MonthEndDate, @PolicyNumber, @LoanNumber, @InsuredAmount, @MonthlyPremium, @ReassurancePremium, @SASRIAAmount, @SpvKey, @HOCAdministrationFee, @HOCProRataPremium, @TotalHOCPremium, @PostedHOCPremium); ";
        public const string hocreassurance_snapshotdatamodel_update = "UPDATE [2am].[dbo].[HOCReassurance_Snapshot] SET MonthEndDate = @MonthEndDate, PolicyNumber = @PolicyNumber, LoanNumber = @LoanNumber, InsuredAmount = @InsuredAmount, MonthlyPremium = @MonthlyPremium, ReassurancePremium = @ReassurancePremium, SASRIAAmount = @SASRIAAmount, SpvKey = @SpvKey, HOCAdministrationFee = @HOCAdministrationFee, HOCProRataPremium = @HOCProRataPremium, TotalHOCPremium = @TotalHOCPremium, PostedHOCPremium = @PostedHOCPremium WHERE  = @";



        public const string employmenttypedatamodel_selectwhere = "SELECT EmploymentTypeKey, Description FROM [2am].[dbo].[EmploymentType] WHERE";
        public const string employmenttypedatamodel_selectbykey = "SELECT EmploymentTypeKey, Description FROM [2am].[dbo].[EmploymentType] WHERE EmploymentTypeKey = @PrimaryKey";
        public const string employmenttypedatamodel_delete = "DELETE FROM [2am].[dbo].[EmploymentType] WHERE EmploymentTypeKey = @PrimaryKey";
        public const string employmenttypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[EmploymentType] WHERE";
        public const string employmenttypedatamodel_insert = "INSERT INTO [2am].[dbo].[EmploymentType] (EmploymentTypeKey, Description) VALUES(@EmploymentTypeKey, @Description); ";
        public const string employmenttypedatamodel_update = "UPDATE [2am].[dbo].[EmploymentType] SET EmploymentTypeKey = @EmploymentTypeKey, Description = @Description WHERE EmploymentTypeKey = @EmploymentTypeKey";



        public const string reasondatamodel_selectwhere = "SELECT ReasonKey, ReasonDefinitionKey, GenericKey, Comment, StageTransitionKey FROM [2am].[dbo].[Reason] WHERE";
        public const string reasondatamodel_selectbykey = "SELECT ReasonKey, ReasonDefinitionKey, GenericKey, Comment, StageTransitionKey FROM [2am].[dbo].[Reason] WHERE ReasonKey = @PrimaryKey";
        public const string reasondatamodel_delete = "DELETE FROM [2am].[dbo].[Reason] WHERE ReasonKey = @PrimaryKey";
        public const string reasondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Reason] WHERE";
        public const string reasondatamodel_insert = "INSERT INTO [2am].[dbo].[Reason] (ReasonDefinitionKey, GenericKey, Comment, StageTransitionKey) VALUES(@ReasonDefinitionKey, @GenericKey, @Comment, @StageTransitionKey); select cast(scope_identity() as int)";
        public const string reasondatamodel_update = "UPDATE [2am].[dbo].[Reason] SET ReasonDefinitionKey = @ReasonDefinitionKey, GenericKey = @GenericKey, Comment = @Comment, StageTransitionKey = @StageTransitionKey WHERE ReasonKey = @ReasonKey";



        public const string internetleadusersdatamodel_selectwhere = "SELECT InternetLeadUsersKey, ADUserKey, Flag, CaseCount, LastCaseKey, GeneralStatusKey FROM [2am].[dbo].[InternetLeadUsers] WHERE";
        public const string internetleadusersdatamodel_selectbykey = "SELECT InternetLeadUsersKey, ADUserKey, Flag, CaseCount, LastCaseKey, GeneralStatusKey FROM [2am].[dbo].[InternetLeadUsers] WHERE InternetLeadUsersKey = @PrimaryKey";
        public const string internetleadusersdatamodel_delete = "DELETE FROM [2am].[dbo].[InternetLeadUsers] WHERE InternetLeadUsersKey = @PrimaryKey";
        public const string internetleadusersdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[InternetLeadUsers] WHERE";
        public const string internetleadusersdatamodel_insert = "INSERT INTO [2am].[dbo].[InternetLeadUsers] (ADUserKey, Flag, CaseCount, LastCaseKey, GeneralStatusKey) VALUES(@ADUserKey, @Flag, @CaseCount, @LastCaseKey, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string internetleadusersdatamodel_update = "UPDATE [2am].[dbo].[InternetLeadUsers] SET ADUserKey = @ADUserKey, Flag = @Flag, CaseCount = @CaseCount, LastCaseKey = @LastCaseKey, GeneralStatusKey = @GeneralStatusKey WHERE InternetLeadUsersKey = @InternetLeadUsersKey";



        public const string affordabilitytypegroupdatamodel_selectwhere = "SELECT AffordabilityTypeGroupKey, Description FROM [2am].[dbo].[AffordabilityTypeGroup] WHERE";
        public const string affordabilitytypegroupdatamodel_selectbykey = "SELECT AffordabilityTypeGroupKey, Description FROM [2am].[dbo].[AffordabilityTypeGroup] WHERE  = @PrimaryKey";
        public const string affordabilitytypegroupdatamodel_delete = "DELETE FROM [2am].[dbo].[AffordabilityTypeGroup] WHERE  = @PrimaryKey";
        public const string affordabilitytypegroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AffordabilityTypeGroup] WHERE";
        public const string affordabilitytypegroupdatamodel_insert = "INSERT INTO [2am].[dbo].[AffordabilityTypeGroup] (AffordabilityTypeGroupKey, Description) VALUES(@AffordabilityTypeGroupKey, @Description); ";
        public const string affordabilitytypegroupdatamodel_update = "UPDATE [2am].[dbo].[AffordabilityTypeGroup] SET AffordabilityTypeGroupKey = @AffordabilityTypeGroupKey, Description = @Description WHERE  = @";



        public const string inputgenerictypedatamodel_selectwhere = "SELECT InputGenericTypeKey, CoreBusinessObjectKey, GenericKeyTypeParameterKey FROM [2am].[dbo].[InputGenericType] WHERE";
        public const string inputgenerictypedatamodel_selectbykey = "SELECT InputGenericTypeKey, CoreBusinessObjectKey, GenericKeyTypeParameterKey FROM [2am].[dbo].[InputGenericType] WHERE InputGenericTypeKey = @PrimaryKey";
        public const string inputgenerictypedatamodel_delete = "DELETE FROM [2am].[dbo].[InputGenericType] WHERE InputGenericTypeKey = @PrimaryKey";
        public const string inputgenerictypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[InputGenericType] WHERE";
        public const string inputgenerictypedatamodel_insert = "INSERT INTO [2am].[dbo].[InputGenericType] (CoreBusinessObjectKey, GenericKeyTypeParameterKey) VALUES(@CoreBusinessObjectKey, @GenericKeyTypeParameterKey); select cast(scope_identity() as int)";
        public const string inputgenerictypedatamodel_update = "UPDATE [2am].[dbo].[InputGenericType] SET CoreBusinessObjectKey = @CoreBusinessObjectKey, GenericKeyTypeParameterKey = @GenericKeyTypeParameterKey WHERE InputGenericTypeKey = @InputGenericTypeKey";



        public const string memodatamodel_selectwhere = "SELECT MemoKey, GenericKeyTypeKey, GenericKey, InsertedDate, Memo, ADUserKey, ChangeDate, GeneralStatusKey, ReminderDate, ExpiryDate FROM [2am].[dbo].[Memo] WHERE";
        public const string memodatamodel_selectbykey = "SELECT MemoKey, GenericKeyTypeKey, GenericKey, InsertedDate, Memo, ADUserKey, ChangeDate, GeneralStatusKey, ReminderDate, ExpiryDate FROM [2am].[dbo].[Memo] WHERE MemoKey = @PrimaryKey";
        public const string memodatamodel_delete = "DELETE FROM [2am].[dbo].[Memo] WHERE MemoKey = @PrimaryKey";
        public const string memodatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Memo] WHERE";
        public const string memodatamodel_insert = "INSERT INTO [2am].[dbo].[Memo] (GenericKeyTypeKey, GenericKey, InsertedDate, Memo, ADUserKey, ChangeDate, GeneralStatusKey, ReminderDate, ExpiryDate) VALUES(@GenericKeyTypeKey, @GenericKey, @InsertedDate, @Memo, @ADUserKey, @ChangeDate, @GeneralStatusKey, @ReminderDate, @ExpiryDate); select cast(scope_identity() as int)";
        public const string memodatamodel_update = "UPDATE [2am].[dbo].[Memo] SET GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey, InsertedDate = @InsertedDate, Memo = @Memo, ADUserKey = @ADUserKey, ChangeDate = @ChangeDate, GeneralStatusKey = @GeneralStatusKey, ReminderDate = @ReminderDate, ExpiryDate = @ExpiryDate WHERE MemoKey = @MemoKey";



        public const string casestatusdatamodel_selectwhere = "SELECT CaseStatusKey, Description, Background, ProductKey FROM [2am].[dbo].[CaseStatus] WHERE";
        public const string casestatusdatamodel_selectbykey = "SELECT CaseStatusKey, Description, Background, ProductKey FROM [2am].[dbo].[CaseStatus] WHERE CaseStatusKey = @PrimaryKey";
        public const string casestatusdatamodel_delete = "DELETE FROM [2am].[dbo].[CaseStatus] WHERE CaseStatusKey = @PrimaryKey";
        public const string casestatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[CaseStatus] WHERE";
        public const string casestatusdatamodel_insert = "INSERT INTO [2am].[dbo].[CaseStatus] (Description, Background, ProductKey) VALUES(@Description, @Background, @ProductKey); select cast(scope_identity() as int)";
        public const string casestatusdatamodel_update = "UPDATE [2am].[dbo].[CaseStatus] SET Description = @Description, Background = @Background, ProductKey = @ProductKey WHERE CaseStatusKey = @CaseStatusKey";



        public const string offerroleattributedatamodel_selectwhere = "SELECT OfferRoleAttributeKey, OfferRoleKey, OfferRoleAttributeTypeKey FROM [2am].[dbo].[OfferRoleAttribute] WHERE";
        public const string offerroleattributedatamodel_selectbykey = "SELECT OfferRoleAttributeKey, OfferRoleKey, OfferRoleAttributeTypeKey FROM [2am].[dbo].[OfferRoleAttribute] WHERE OfferRoleAttributeKey = @PrimaryKey";
        public const string offerroleattributedatamodel_delete = "DELETE FROM [2am].[dbo].[OfferRoleAttribute] WHERE OfferRoleAttributeKey = @PrimaryKey";
        public const string offerroleattributedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferRoleAttribute] WHERE";
        public const string offerroleattributedatamodel_insert = "INSERT INTO [2am].[dbo].[OfferRoleAttribute] (OfferRoleKey, OfferRoleAttributeTypeKey) VALUES(@OfferRoleKey, @OfferRoleAttributeTypeKey); select cast(scope_identity() as int)";
        public const string offerroleattributedatamodel_update = "UPDATE [2am].[dbo].[OfferRoleAttribute] SET OfferRoleKey = @OfferRoleKey, OfferRoleAttributeTypeKey = @OfferRoleAttributeTypeKey WHERE OfferRoleAttributeKey = @OfferRoleAttributeKey";



        public const string originationsourcedatamodel_selectwhere = "SELECT OriginationSourceKey, Description FROM [2am].[dbo].[OriginationSource] WHERE";
        public const string originationsourcedatamodel_selectbykey = "SELECT OriginationSourceKey, Description FROM [2am].[dbo].[OriginationSource] WHERE OriginationSourceKey = @PrimaryKey";
        public const string originationsourcedatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSource] WHERE OriginationSourceKey = @PrimaryKey";
        public const string originationsourcedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSource] WHERE";
        public const string originationsourcedatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSource] (OriginationSourceKey, Description) VALUES(@OriginationSourceKey, @Description); ";
        public const string originationsourcedatamodel_update = "UPDATE [2am].[dbo].[OriginationSource] SET OriginationSourceKey = @OriginationSourceKey, Description = @Description WHERE OriginationSourceKey = @OriginationSourceKey";



        public const string offerconditioninserttempdatamodel_selectwhere = "SELECT pk, ConditionKey, OfferKey, LanguageKey, TranslatableItemDesc, TranslatedText FROM [2am].[dbo].[OfferConditionInsertTEMP] WHERE";
        public const string offerconditioninserttempdatamodel_selectbykey = "SELECT pk, ConditionKey, OfferKey, LanguageKey, TranslatableItemDesc, TranslatedText FROM [2am].[dbo].[OfferConditionInsertTEMP] WHERE pk = @PrimaryKey";
        public const string offerconditioninserttempdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferConditionInsertTEMP] WHERE pk = @PrimaryKey";
        public const string offerconditioninserttempdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferConditionInsertTEMP] WHERE";
        public const string offerconditioninserttempdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferConditionInsertTEMP] (ConditionKey, OfferKey, LanguageKey, TranslatableItemDesc, TranslatedText) VALUES(@ConditionKey, @OfferKey, @LanguageKey, @TranslatableItemDesc, @TranslatedText); select cast(scope_identity() as int)";
        public const string offerconditioninserttempdatamodel_update = "UPDATE [2am].[dbo].[OfferConditionInsertTEMP] SET ConditionKey = @ConditionKey, OfferKey = @OfferKey, LanguageKey = @LanguageKey, TranslatableItemDesc = @TranslatableItemDesc, TranslatedText = @TranslatedText WHERE pk = @pk";



        public const string valuationimprovementdatamodel_selectwhere = "SELECT ValuationImprovementKey, ValuationKey, ImprovementDate, ValuationImprovementTypeKey, ImprovementValue FROM [2am].[dbo].[ValuationImprovement] WHERE";
        public const string valuationimprovementdatamodel_selectbykey = "SELECT ValuationImprovementKey, ValuationKey, ImprovementDate, ValuationImprovementTypeKey, ImprovementValue FROM [2am].[dbo].[ValuationImprovement] WHERE ValuationImprovementKey = @PrimaryKey";
        public const string valuationimprovementdatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationImprovement] WHERE ValuationImprovementKey = @PrimaryKey";
        public const string valuationimprovementdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationImprovement] WHERE";
        public const string valuationimprovementdatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationImprovement] (ValuationKey, ImprovementDate, ValuationImprovementTypeKey, ImprovementValue) VALUES(@ValuationKey, @ImprovementDate, @ValuationImprovementTypeKey, @ImprovementValue); select cast(scope_identity() as int)";
        public const string valuationimprovementdatamodel_update = "UPDATE [2am].[dbo].[ValuationImprovement] SET ValuationKey = @ValuationKey, ImprovementDate = @ImprovementDate, ValuationImprovementTypeKey = @ValuationImprovementTypeKey, ImprovementValue = @ImprovementValue WHERE ValuationImprovementKey = @ValuationImprovementKey";



        public const string originationsourceproductreasondefinitiondatamodel_selectwhere = "SELECT OriginationSourceProductReasonDefinitionKey, OriginationSourceProductKey, ReasonDefinitionKey FROM [2am].[dbo].[OriginationSourceProductReasonDefinition] WHERE";
        public const string originationsourceproductreasondefinitiondatamodel_selectbykey = "SELECT OriginationSourceProductReasonDefinitionKey, OriginationSourceProductKey, ReasonDefinitionKey FROM [2am].[dbo].[OriginationSourceProductReasonDefinition] WHERE OriginationSourceProductReasonDefinitionKey = @PrimaryKey";
        public const string originationsourceproductreasondefinitiondatamodel_delete = "DELETE FROM [2am].[dbo].[OriginationSourceProductReasonDefinition] WHERE OriginationSourceProductReasonDefinitionKey = @PrimaryKey";
        public const string originationsourceproductreasondefinitiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OriginationSourceProductReasonDefinition] WHERE";
        public const string originationsourceproductreasondefinitiondatamodel_insert = "INSERT INTO [2am].[dbo].[OriginationSourceProductReasonDefinition] (OriginationSourceProductKey, ReasonDefinitionKey) VALUES(@OriginationSourceProductKey, @ReasonDefinitionKey); select cast(scope_identity() as int)";
        public const string originationsourceproductreasondefinitiondatamodel_update = "UPDATE [2am].[dbo].[OriginationSourceProductReasonDefinition] SET OriginationSourceProductKey = @OriginationSourceProductKey, ReasonDefinitionKey = @ReasonDefinitionKey WHERE OriginationSourceProductReasonDefinitionKey = @OriginationSourceProductReasonDefinitionKey";



        public const string accountcasestatusdatamodel_selectwhere = "SELECT AccountCaseStatusKey, AccountKey, CaseStatusKey, UserName, UpdateDate, Notes FROM [2am].[dbo].[AccountCaseStatus] WHERE";
        public const string accountcasestatusdatamodel_selectbykey = "SELECT AccountCaseStatusKey, AccountKey, CaseStatusKey, UserName, UpdateDate, Notes FROM [2am].[dbo].[AccountCaseStatus] WHERE AccountCaseStatusKey = @PrimaryKey";
        public const string accountcasestatusdatamodel_delete = "DELETE FROM [2am].[dbo].[AccountCaseStatus] WHERE AccountCaseStatusKey = @PrimaryKey";
        public const string accountcasestatusdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AccountCaseStatus] WHERE";
        public const string accountcasestatusdatamodel_insert = "INSERT INTO [2am].[dbo].[AccountCaseStatus] (AccountKey, CaseStatusKey, UserName, UpdateDate, Notes) VALUES(@AccountKey, @CaseStatusKey, @UserName, @UpdateDate, @Notes); select cast(scope_identity() as int)";
        public const string accountcasestatusdatamodel_update = "UPDATE [2am].[dbo].[AccountCaseStatus] SET AccountKey = @AccountKey, CaseStatusKey = @CaseStatusKey, UserName = @UserName, UpdateDate = @UpdateDate, Notes = @Notes WHERE AccountCaseStatusKey = @AccountCaseStatusKey";



        public const string translatableitemtempdatamodel_selectwhere = "SELECT TranslatableItemKey, Description FROM [2am].[dbo].[TranslatableItemTEMP] WHERE";
        public const string translatableitemtempdatamodel_selectbykey = "SELECT TranslatableItemKey, Description FROM [2am].[dbo].[TranslatableItemTEMP] WHERE TranslatableItemKey = @PrimaryKey";
        public const string translatableitemtempdatamodel_delete = "DELETE FROM [2am].[dbo].[TranslatableItemTEMP] WHERE TranslatableItemKey = @PrimaryKey";
        public const string translatableitemtempdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TranslatableItemTEMP] WHERE";
        public const string translatableitemtempdatamodel_insert = "INSERT INTO [2am].[dbo].[TranslatableItemTEMP] (TranslatableItemKey, Description) VALUES(@TranslatableItemKey, @Description); ";
        public const string translatableitemtempdatamodel_update = "UPDATE [2am].[dbo].[TranslatableItemTEMP] SET TranslatableItemKey = @TranslatableItemKey, Description = @Description WHERE TranslatableItemKey = @TranslatableItemKey";



        public const string auditlegalentityaddressdatamodel_selectwhere = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityAddressKey, LegalEntityKey, AddressKey, AddressTypeKey, EffectiveDate, GeneralStatusKey FROM [2am].[dbo].[AuditLegalEntityAddress] WHERE";
        public const string auditlegalentityaddressdatamodel_selectbykey = "SELECT AuditNumber, AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityAddressKey, LegalEntityKey, AddressKey, AddressTypeKey, EffectiveDate, GeneralStatusKey FROM [2am].[dbo].[AuditLegalEntityAddress] WHERE AuditNumber = @PrimaryKey";
        public const string auditlegalentityaddressdatamodel_delete = "DELETE FROM [2am].[dbo].[AuditLegalEntityAddress] WHERE AuditNumber = @PrimaryKey";
        public const string auditlegalentityaddressdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[AuditLegalEntityAddress] WHERE";
        public const string auditlegalentityaddressdatamodel_insert = "INSERT INTO [2am].[dbo].[AuditLegalEntityAddress] (AuditLogin, AuditHostName, AuditProgramName, AuditDate, AuditAddUpdateDelete, LegalEntityAddressKey, LegalEntityKey, AddressKey, AddressTypeKey, EffectiveDate, GeneralStatusKey) VALUES(@AuditLogin, @AuditHostName, @AuditProgramName, @AuditDate, @AuditAddUpdateDelete, @LegalEntityAddressKey, @LegalEntityKey, @AddressKey, @AddressTypeKey, @EffectiveDate, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string auditlegalentityaddressdatamodel_update = "UPDATE [2am].[dbo].[AuditLegalEntityAddress] SET AuditLogin = @AuditLogin, AuditHostName = @AuditHostName, AuditProgramName = @AuditProgramName, AuditDate = @AuditDate, AuditAddUpdateDelete = @AuditAddUpdateDelete, LegalEntityAddressKey = @LegalEntityAddressKey, LegalEntityKey = @LegalEntityKey, AddressKey = @AddressKey, AddressTypeKey = @AddressTypeKey, EffectiveDate = @EffectiveDate, GeneralStatusKey = @GeneralStatusKey WHERE AuditNumber = @AuditNumber";



        public const string featuregroupdatamodel_selectwhere = "SELECT FeatureGroupKey, ADUserGroup, FeatureKey FROM [2am].[dbo].[FeatureGroup] WHERE";
        public const string featuregroupdatamodel_selectbykey = "SELECT FeatureGroupKey, ADUserGroup, FeatureKey FROM [2am].[dbo].[FeatureGroup] WHERE FeatureGroupKey = @PrimaryKey";
        public const string featuregroupdatamodel_delete = "DELETE FROM [2am].[dbo].[FeatureGroup] WHERE FeatureGroupKey = @PrimaryKey";
        public const string featuregroupdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FeatureGroup] WHERE";
        public const string featuregroupdatamodel_insert = "INSERT INTO [2am].[dbo].[FeatureGroup] (ADUserGroup, FeatureKey) VALUES(@ADUserGroup, @FeatureKey); select cast(scope_identity() as int)";
        public const string featuregroupdatamodel_update = "UPDATE [2am].[dbo].[FeatureGroup] SET ADUserGroup = @ADUserGroup, FeatureKey = @FeatureKey WHERE FeatureGroupKey = @FeatureGroupKey";



        public const string formattypedatamodel_selectwhere = "SELECT FormatTypeKey, Description, Format FROM [2am].[dbo].[FormatType] WHERE";
        public const string formattypedatamodel_selectbykey = "SELECT FormatTypeKey, Description, Format FROM [2am].[dbo].[FormatType] WHERE FormatTypeKey = @PrimaryKey";
        public const string formattypedatamodel_delete = "DELETE FROM [2am].[dbo].[FormatType] WHERE FormatTypeKey = @PrimaryKey";
        public const string formattypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[FormatType] WHERE";
        public const string formattypedatamodel_insert = "INSERT INTO [2am].[dbo].[FormatType] (FormatTypeKey, Description, Format) VALUES(@FormatTypeKey, @Description, @Format); ";
        public const string formattypedatamodel_update = "UPDATE [2am].[dbo].[FormatType] SET FormatTypeKey = @FormatTypeKey, Description = @Description, Format = @Format WHERE FormatTypeKey = @FormatTypeKey";



        public const string workflowmenudatamodel_selectwhere = "SELECT WorkflowMenuKey, WorkflowName, StateName, CoreBusinessObjectKey, ProcessName FROM [2am].[dbo].[WorkflowMenu] WHERE";
        public const string workflowmenudatamodel_selectbykey = "SELECT WorkflowMenuKey, WorkflowName, StateName, CoreBusinessObjectKey, ProcessName FROM [2am].[dbo].[WorkflowMenu] WHERE WorkflowMenuKey = @PrimaryKey";
        public const string workflowmenudatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowMenu] WHERE WorkflowMenuKey = @PrimaryKey";
        public const string workflowmenudatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowMenu] WHERE";
        public const string workflowmenudatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowMenu] (WorkflowName, StateName, CoreBusinessObjectKey, ProcessName) VALUES(@WorkflowName, @StateName, @CoreBusinessObjectKey, @ProcessName); select cast(scope_identity() as int)";
        public const string workflowmenudatamodel_update = "UPDATE [2am].[dbo].[WorkflowMenu] SET WorkflowName = @WorkflowName, StateName = @StateName, CoreBusinessObjectKey = @CoreBusinessObjectKey, ProcessName = @ProcessName WHERE WorkflowMenuKey = @WorkflowMenuKey";



        public const string mediumdatamodel_selectwhere = "SELECT MediumKey, Description FROM [2am].[dbo].[Medium] WHERE";
        public const string mediumdatamodel_selectbykey = "SELECT MediumKey, Description FROM [2am].[dbo].[Medium] WHERE MediumKey = @PrimaryKey";
        public const string mediumdatamodel_delete = "DELETE FROM [2am].[dbo].[Medium] WHERE MediumKey = @PrimaryKey";
        public const string mediumdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Medium] WHERE";
        public const string mediumdatamodel_insert = "INSERT INTO [2am].[dbo].[Medium] (MediumKey, Description) VALUES(@MediumKey, @Description); ";
        public const string mediumdatamodel_update = "UPDATE [2am].[dbo].[Medium] SET MediumKey = @MediumKey, Description = @Description WHERE MediumKey = @MediumKey";



        public const string documenttypereferenceobjectdatamodel_selectwhere = "SELECT DocumentTypeReferenceObjectKey, DocumentTypeKey, GenericKeyTypeKey FROM [2am].[dbo].[DocumentTypeReferenceObject] WHERE";
        public const string documenttypereferenceobjectdatamodel_selectbykey = "SELECT DocumentTypeReferenceObjectKey, DocumentTypeKey, GenericKeyTypeKey FROM [2am].[dbo].[DocumentTypeReferenceObject] WHERE DocumentTypeReferenceObjectKey = @PrimaryKey";
        public const string documenttypereferenceobjectdatamodel_delete = "DELETE FROM [2am].[dbo].[DocumentTypeReferenceObject] WHERE DocumentTypeReferenceObjectKey = @PrimaryKey";
        public const string documenttypereferenceobjectdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DocumentTypeReferenceObject] WHERE";
        public const string documenttypereferenceobjectdatamodel_insert = "INSERT INTO [2am].[dbo].[DocumentTypeReferenceObject] (DocumentTypeKey, GenericKeyTypeKey) VALUES(@DocumentTypeKey, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string documenttypereferenceobjectdatamodel_update = "UPDATE [2am].[dbo].[DocumentTypeReferenceObject] SET DocumentTypeKey = @DocumentTypeKey, GenericKeyTypeKey = @GenericKeyTypeKey WHERE DocumentTypeReferenceObjectKey = @DocumentTypeReferenceObjectKey";



        public const string offerinformationdatamodel_selectwhere = "SELECT OfferInformationKey, OfferInsertDate, OfferKey, OfferInformationTypeKey, UserName, ChangeDate, ProductKey FROM [2am].[dbo].[OfferInformation] WHERE";
        public const string offerinformationdatamodel_selectbykey = "SELECT OfferInformationKey, OfferInsertDate, OfferKey, OfferInformationTypeKey, UserName, ChangeDate, ProductKey FROM [2am].[dbo].[OfferInformation] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationdatamodel_delete = "DELETE FROM [2am].[dbo].[OfferInformation] WHERE OfferInformationKey = @PrimaryKey";
        public const string offerinformationdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferInformation] WHERE";
        public const string offerinformationdatamodel_insert = "INSERT INTO [2am].[dbo].[OfferInformation] (OfferInsertDate, OfferKey, OfferInformationTypeKey, UserName, ChangeDate, ProductKey) VALUES(@OfferInsertDate, @OfferKey, @OfferInformationTypeKey, @UserName, @ChangeDate, @ProductKey); select cast(scope_identity() as int)";
        public const string offerinformationdatamodel_update = "UPDATE [2am].[dbo].[OfferInformation] SET OfferInsertDate = @OfferInsertDate, OfferKey = @OfferKey, OfferInformationTypeKey = @OfferInformationTypeKey, UserName = @UserName, ChangeDate = @ChangeDate, ProductKey = @ProductKey WHERE OfferInformationKey = @OfferInformationKey";



        public const string offerhocpolicydatamodel_selectwhere = "SELECT OfferKey, HOCInsurerKey, HOCPolicyNumber, HOCMonthlyPremium, HOCRoofKey, HOCSubsidenceKey, HOCConstructionKey, Ceded FROM [2am].[dbo].[OfferHOCPolicy] WHERE";
        public const string offerhocpolicydatamodel_selectbykey = "SELECT OfferKey, HOCInsurerKey, HOCPolicyNumber, HOCMonthlyPremium, HOCRoofKey, HOCSubsidenceKey, HOCConstructionKey, Ceded FROM [2am].[dbo].[OfferHOCPolicy] WHERE OfferKey = @PrimaryKey";
        public const string offerhocpolicydatamodel_delete = "DELETE FROM [2am].[dbo].[OfferHOCPolicy] WHERE OfferKey = @PrimaryKey";
        public const string offerhocpolicydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[OfferHOCPolicy] WHERE";
        public const string offerhocpolicydatamodel_insert = "INSERT INTO [2am].[dbo].[OfferHOCPolicy] (OfferKey, HOCInsurerKey, HOCPolicyNumber, HOCMonthlyPremium, HOCRoofKey, HOCSubsidenceKey, HOCConstructionKey, Ceded) VALUES(@OfferKey, @HOCInsurerKey, @HOCPolicyNumber, @HOCMonthlyPremium, @HOCRoofKey, @HOCSubsidenceKey, @HOCConstructionKey, @Ceded); ";
        public const string offerhocpolicydatamodel_update = "UPDATE [2am].[dbo].[OfferHOCPolicy] SET OfferKey = @OfferKey, HOCInsurerKey = @HOCInsurerKey, HOCPolicyNumber = @HOCPolicyNumber, HOCMonthlyPremium = @HOCMonthlyPremium, HOCRoofKey = @HOCRoofKey, HOCSubsidenceKey = @HOCSubsidenceKey, HOCConstructionKey = @HOCConstructionKey, Ceded = @Ceded WHERE OfferKey = @OfferKey";



        public const string genericsettypedatamodel_selectwhere = "SELECT GenericSetTypeKey, Description, GenericKeyTypeKey FROM [2am].[dbo].[GenericSetType] WHERE";
        public const string genericsettypedatamodel_selectbykey = "SELECT GenericSetTypeKey, Description, GenericKeyTypeKey FROM [2am].[dbo].[GenericSetType] WHERE GenericSetTypeKey = @PrimaryKey";
        public const string genericsettypedatamodel_delete = "DELETE FROM [2am].[dbo].[GenericSetType] WHERE GenericSetTypeKey = @PrimaryKey";
        public const string genericsettypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[GenericSetType] WHERE";
        public const string genericsettypedatamodel_insert = "INSERT INTO [2am].[dbo].[GenericSetType] (Description, GenericKeyTypeKey) VALUES(@Description, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string genericsettypedatamodel_update = "UPDATE [2am].[dbo].[GenericSetType] SET Description = @Description, GenericKeyTypeKey = @GenericKeyTypeKey WHERE GenericSetTypeKey = @GenericSetTypeKey";



        public const string workfloworganisationstructuredatamodel_selectwhere = "SELECT WorkflowOrganisationStructureKey, OrganisationStructureKey, WorkflowName, ProcessName FROM [2am].[dbo].[WorkflowOrganisationStructure] WHERE";
        public const string workfloworganisationstructuredatamodel_selectbykey = "SELECT WorkflowOrganisationStructureKey, OrganisationStructureKey, WorkflowName, ProcessName FROM [2am].[dbo].[WorkflowOrganisationStructure] WHERE WorkflowOrganisationStructureKey = @PrimaryKey";
        public const string workfloworganisationstructuredatamodel_delete = "DELETE FROM [2am].[dbo].[WorkflowOrganisationStructure] WHERE WorkflowOrganisationStructureKey = @PrimaryKey";
        public const string workfloworganisationstructuredatamodel_deletewhere = "DELETE FROM [2am].[dbo].[WorkflowOrganisationStructure] WHERE";
        public const string workfloworganisationstructuredatamodel_insert = "INSERT INTO [2am].[dbo].[WorkflowOrganisationStructure] (OrganisationStructureKey, WorkflowName, ProcessName) VALUES(@OrganisationStructureKey, @WorkflowName, @ProcessName); select cast(scope_identity() as int)";
        public const string workfloworganisationstructuredatamodel_update = "UPDATE [2am].[dbo].[WorkflowOrganisationStructure] SET OrganisationStructureKey = @OrganisationStructureKey, WorkflowName = @WorkflowName, ProcessName = @ProcessName WHERE WorkflowOrganisationStructureKey = @WorkflowOrganisationStructureKey";



        public const string tokendatamodel_selectwhere = "SELECT TokenKey, Description, TokenTypeKey, StatementDefinitionKey, MustTranslate, ParameterTypeKey FROM [2am].[dbo].[Token] WHERE";
        public const string tokendatamodel_selectbykey = "SELECT TokenKey, Description, TokenTypeKey, StatementDefinitionKey, MustTranslate, ParameterTypeKey FROM [2am].[dbo].[Token] WHERE TokenKey = @PrimaryKey";
        public const string tokendatamodel_delete = "DELETE FROM [2am].[dbo].[Token] WHERE TokenKey = @PrimaryKey";
        public const string tokendatamodel_deletewhere = "DELETE FROM [2am].[dbo].[Token] WHERE";
        public const string tokendatamodel_insert = "INSERT INTO [2am].[dbo].[Token] (Description, TokenTypeKey, StatementDefinitionKey, MustTranslate, ParameterTypeKey) VALUES(@Description, @TokenTypeKey, @StatementDefinitionKey, @MustTranslate, @ParameterTypeKey); select cast(scope_identity() as int)";
        public const string tokendatamodel_update = "UPDATE [2am].[dbo].[Token] SET Description = @Description, TokenTypeKey = @TokenTypeKey, StatementDefinitionKey = @StatementDefinitionKey, MustTranslate = @MustTranslate, ParameterTypeKey = @ParameterTypeKey WHERE TokenKey = @TokenKey";



        public const string helpdeskcategorydatamodel_selectwhere = "SELECT HelpDeskCategoryKey, Description, GeneralStatusKey FROM [2am].[dbo].[HelpDeskCategory] WHERE";
        public const string helpdeskcategorydatamodel_selectbykey = "SELECT HelpDeskCategoryKey, Description, GeneralStatusKey FROM [2am].[dbo].[HelpDeskCategory] WHERE HelpDeskCategoryKey = @PrimaryKey";
        public const string helpdeskcategorydatamodel_delete = "DELETE FROM [2am].[dbo].[HelpDeskCategory] WHERE HelpDeskCategoryKey = @PrimaryKey";
        public const string helpdeskcategorydatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HelpDeskCategory] WHERE";
        public const string helpdeskcategorydatamodel_insert = "INSERT INTO [2am].[dbo].[HelpDeskCategory] (Description, GeneralStatusKey) VALUES(@Description, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string helpdeskcategorydatamodel_update = "UPDATE [2am].[dbo].[HelpDeskCategory] SET Description = @Description, GeneralStatusKey = @GeneralStatusKey WHERE HelpDeskCategoryKey = @HelpDeskCategoryKey";



        public const string employmentsectordatamodel_selectwhere = "SELECT EmploymentSectorKey, Description, GeneralStatusKey FROM [2am].[dbo].[EmploymentSector] WHERE";
        public const string employmentsectordatamodel_selectbykey = "SELECT EmploymentSectorKey, Description, GeneralStatusKey FROM [2am].[dbo].[EmploymentSector] WHERE EmploymentSectorKey = @PrimaryKey";
        public const string employmentsectordatamodel_delete = "DELETE FROM [2am].[dbo].[EmploymentSector] WHERE EmploymentSectorKey = @PrimaryKey";
        public const string employmentsectordatamodel_deletewhere = "DELETE FROM [2am].[dbo].[EmploymentSector] WHERE";
        public const string employmentsectordatamodel_insert = "INSERT INTO [2am].[dbo].[EmploymentSector] (Description, GeneralStatusKey) VALUES(@Description, @GeneralStatusKey); select cast(scope_identity() as int)";
        public const string employmentsectordatamodel_update = "UPDATE [2am].[dbo].[EmploymentSector] SET Description = @Description, GeneralStatusKey = @GeneralStatusKey WHERE EmploymentSectorKey = @EmploymentSectorKey";



        public const string valuationcottagedatamodel_selectwhere = "SELECT ValuationKey, ValuationRoofTypeKey, Extent, Rate FROM [2am].[dbo].[ValuationCottage] WHERE";
        public const string valuationcottagedatamodel_selectbykey = "SELECT ValuationKey, ValuationRoofTypeKey, Extent, Rate FROM [2am].[dbo].[ValuationCottage] WHERE ValuationKey = @PrimaryKey";
        public const string valuationcottagedatamodel_delete = "DELETE FROM [2am].[dbo].[ValuationCottage] WHERE ValuationKey = @PrimaryKey";
        public const string valuationcottagedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ValuationCottage] WHERE";
        public const string valuationcottagedatamodel_insert = "INSERT INTO [2am].[dbo].[ValuationCottage] (ValuationKey, ValuationRoofTypeKey, Extent, Rate) VALUES(@ValuationKey, @ValuationRoofTypeKey, @Extent, @Rate); ";
        public const string valuationcottagedatamodel_update = "UPDATE [2am].[dbo].[ValuationCottage] SET ValuationKey = @ValuationKey, ValuationRoofTypeKey = @ValuationRoofTypeKey, Extent = @Extent, Rate = @Rate WHERE ValuationKey = @ValuationKey";



        public const string tokentypedatamodel_selectwhere = "SELECT TokenTypeKey, Description, UserID, RunStatement FROM [2am].[dbo].[TokenType] WHERE";
        public const string tokentypedatamodel_selectbykey = "SELECT TokenTypeKey, Description, UserID, RunStatement FROM [2am].[dbo].[TokenType] WHERE TokenTypeKey = @PrimaryKey";
        public const string tokentypedatamodel_delete = "DELETE FROM [2am].[dbo].[TokenType] WHERE TokenTypeKey = @PrimaryKey";
        public const string tokentypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[TokenType] WHERE";
        public const string tokentypedatamodel_insert = "INSERT INTO [2am].[dbo].[TokenType] (Description, UserID, RunStatement) VALUES(@Description, @UserID, @RunStatement); select cast(scope_identity() as int)";
        public const string tokentypedatamodel_update = "UPDATE [2am].[dbo].[TokenType] SET Description = @Description, UserID = @UserID, RunStatement = @RunStatement WHERE TokenTypeKey = @TokenTypeKey";



        public const string tmp_life_leadcreatedatamodel_selectwhere = "SELECT LoanNumber, Date FROM [2am].[dbo].[tmp_Life_LeadCreate] WHERE";
        public const string tmp_life_leadcreatedatamodel_selectbykey = "SELECT LoanNumber, Date FROM [2am].[dbo].[tmp_Life_LeadCreate] WHERE  = @PrimaryKey";
        public const string tmp_life_leadcreatedatamodel_delete = "DELETE FROM [2am].[dbo].[tmp_Life_LeadCreate] WHERE  = @PrimaryKey";
        public const string tmp_life_leadcreatedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[tmp_Life_LeadCreate] WHERE";
        public const string tmp_life_leadcreatedatamodel_insert = "INSERT INTO [2am].[dbo].[tmp_Life_LeadCreate] (LoanNumber, Date) VALUES(@LoanNumber, @Date); ";
        public const string tmp_life_leadcreatedatamodel_update = "UPDATE [2am].[dbo].[tmp_Life_LeadCreate] SET LoanNumber = @LoanNumber, Date = @Date WHERE  = @";



        public const string detailclassdatamodel_selectwhere = "SELECT DetailClassKey, Description FROM [2am].[dbo].[DetailClass] WHERE";
        public const string detailclassdatamodel_selectbykey = "SELECT DetailClassKey, Description FROM [2am].[dbo].[DetailClass] WHERE DetailClassKey = @PrimaryKey";
        public const string detailclassdatamodel_delete = "DELETE FROM [2am].[dbo].[DetailClass] WHERE DetailClassKey = @PrimaryKey";
        public const string detailclassdatamodel_deletewhere = "DELETE FROM [2am].[dbo].[DetailClass] WHERE";
        public const string detailclassdatamodel_insert = "INSERT INTO [2am].[dbo].[DetailClass] (DetailClassKey, Description) VALUES(@DetailClassKey, @Description); ";
        public const string detailclassdatamodel_update = "UPDATE [2am].[dbo].[DetailClass] SET DetailClassKey = @DetailClassKey, Description = @Description WHERE DetailClassKey = @DetailClassKey";



        public const string conditiontokendatamodel_selectwhere = "SELECT ConditionTokenKey, ConditionKey, TokenKey FROM [2am].[dbo].[ConditionToken] WHERE";
        public const string conditiontokendatamodel_selectbykey = "SELECT ConditionTokenKey, ConditionKey, TokenKey FROM [2am].[dbo].[ConditionToken] WHERE ConditionTokenKey = @PrimaryKey";
        public const string conditiontokendatamodel_delete = "DELETE FROM [2am].[dbo].[ConditionToken] WHERE ConditionTokenKey = @PrimaryKey";
        public const string conditiontokendatamodel_deletewhere = "DELETE FROM [2am].[dbo].[ConditionToken] WHERE";
        public const string conditiontokendatamodel_insert = "INSERT INTO [2am].[dbo].[ConditionToken] (ConditionKey, TokenKey) VALUES(@ConditionKey, @TokenKey); select cast(scope_identity() as int)";
        public const string conditiontokendatamodel_update = "UPDATE [2am].[dbo].[ConditionToken] SET ConditionKey = @ConditionKey, TokenKey = @TokenKey WHERE ConditionTokenKey = @ConditionTokenKey";



        public const string headericontypedatamodel_selectwhere = "SELECT HeaderIconTypeKey, Description, Icon, StatementName FROM [2am].[dbo].[HeaderIconType] WHERE";
        public const string headericontypedatamodel_selectbykey = "SELECT HeaderIconTypeKey, Description, Icon, StatementName FROM [2am].[dbo].[HeaderIconType] WHERE HeaderIconTypeKey = @PrimaryKey";
        public const string headericontypedatamodel_delete = "DELETE FROM [2am].[dbo].[HeaderIconType] WHERE HeaderIconTypeKey = @PrimaryKey";
        public const string headericontypedatamodel_deletewhere = "DELETE FROM [2am].[dbo].[HeaderIconType] WHERE";
        public const string headericontypedatamodel_insert = "INSERT INTO [2am].[dbo].[HeaderIconType] (Description, Icon, StatementName) VALUES(@Description, @Icon, @StatementName); select cast(scope_identity() as int)";
        public const string headericontypedatamodel_update = "UPDATE [2am].[dbo].[HeaderIconType] SET Description = @Description, Icon = @Icon, StatementName = @StatementName WHERE HeaderIconTypeKey = @HeaderIconTypeKey";



        public const string legalentitymarketingoptiondatamodel_selectwhere = "SELECT LegalEntityMarketingOptionKey, LegalEntityKey, MarketingOptionKey, ChangeDate, UserID FROM [2am].[dbo].[LegalEntityMarketingOption] WHERE";
        public const string legalentitymarketingoptiondatamodel_selectbykey = "SELECT LegalEntityMarketingOptionKey, LegalEntityKey, MarketingOptionKey, ChangeDate, UserID FROM [2am].[dbo].[LegalEntityMarketingOption] WHERE LegalEntityMarketingOptionKey = @PrimaryKey";
        public const string legalentitymarketingoptiondatamodel_delete = "DELETE FROM [2am].[dbo].[LegalEntityMarketingOption] WHERE LegalEntityMarketingOptionKey = @PrimaryKey";
        public const string legalentitymarketingoptiondatamodel_deletewhere = "DELETE FROM [2am].[dbo].[LegalEntityMarketingOption] WHERE";
        public const string legalentitymarketingoptiondatamodel_insert = "INSERT INTO [2am].[dbo].[LegalEntityMarketingOption] (LegalEntityKey, MarketingOptionKey, ChangeDate, UserID) VALUES(@LegalEntityKey, @MarketingOptionKey, @ChangeDate, @UserID); select cast(scope_identity() as int)";
        public const string legalentitymarketingoptiondatamodel_update = "UPDATE [2am].[dbo].[LegalEntityMarketingOption] SET LegalEntityKey = @LegalEntityKey, MarketingOptionKey = @MarketingOptionKey, ChangeDate = @ChangeDate, UserID = @UserID WHERE LegalEntityMarketingOptionKey = @LegalEntityMarketingOptionKey";



    }
}