using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SAHL.Common.BusinessModel.Interfaces.ReadOnly;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// A list of keys used for items in the lookup cache.
    /// </summary>
    [Serializable()]
    public enum LookupKeys
    {
        AccountIndications,
        AccountStatuses,
        AddressFormats,
        AddressTypes,
        ADUserGroups,
        AffordabilityAssessmentStatuses,
        AffordabilityTypes,
        ApplicantTypes,
        ApplicationAttributesTypes,
        ApplicationDeclarationAnswers,
        ApplicationInformationTypes,
        ApplicationOriginators,
        ApplicationRoleAttributesTypes,
        ApplicationRoleTypes,
        ApplicationSources,
        ApplicationStatuses,
        ApplicationTypes,
        AreaClassifications,
        AssetLiabilitySubTypes,
        AssetLiabilityTypes,
        Attorneys,
        BankAccounts,
        BankAccountTypes,
        BankBranches,
        Banks,
        BatchTransactionStatuses,
        BulkBatchStatuses,
        BulkBatchTypes,
        CancellationTypes,
        CapStatuses,
        Categories,
        CBOInputGenericTypes,
        CBOMenus,
        CitizenTypes,
        ClaimStatuses,
        ClaimTypes,
        ContextMenus,
        Controls,
        CorrespondenceMediums,
        Countries,
        CourtTypes,
        CreditScoreDecisions,
        DataProviderDataServices,
        DataProviders,
        DataServices,
        DebitOrderDays,
        DebtCounsellingStatuses,
        DeedsOffice,
        DeedsPropertyTypes,
        DetailClasses,
        DetailTypes,
        DisabilityTypes,
        DisbursementStatuses,
        DisbursementTypes,
        Educations,
        EmployerBusinessTypes,
        EmploymentConfirmationSources,
        EmploymentVerificationProcessTypes,
        EmploymentSectors,
        EmploymentSectorsActive,
        EmploymentStatuses,
        EmploymentTypes,
        ExpenseTypes,
        ExternalRoleTypes,
        FeatureGroups,
        FinancialServiceGroups,
        FinancialServicePaymentTypes,
        FutureDatedChangeTypes,
        Genders,
        GeneralStatuses,
        GenericKeyType,
        HearingTypes,
        HelpDeskCategories,
        HOCConstruction,
        HOCInsurers,
        HOCRoof,
        HOCStatus,
        HOCSubsidence,
        ImportStatuses,
        Insurers,
        Languages,
        LegalEntityExceptionStatuses,
        LegalEntityRelationshipTypes,
        LegalEntityStatuses,
        LegalEntityTypes,
        LifeInsurableInterestTypes,
        LifePolicyStatuses,
        LifePolicyTypes,
        LitigationExternalRoleTypes,
        Margins,
        MarketingOptionRelevance,
        MarketingOptions,
        MarketingOptionsActive,
        MarketRates,
        MaritalStatuses,
        MessageTypes,
        MortgageLoanPurposes,
        OccupancyTypes,
        OnlineStatementFormats,
        OrganisationStructureOriginationSources,
        OrganisationStructures,
        OrganisationTypes,
        OriginationSourceProducts,
        OriginationSources,
        ParameterTypes,
        PopulationGroups,
        Priorities,
        Products,
        PropertyDataProviderDataServices,
        PropertyTypes,
        ProposalStatuses,
        ProposalTypes,
        ProvincesByCountry,
        QuickCashPaymentTypes,
        FinancialAdjustmentTypes,
        FinancialAdjustmentSources,
        FinancialAdjustmentStatuses,
        ReasonDescriptions,
        ReasonTypeGroups,
        ReasonTypes,
        RecurringTransactionTypes,
        RemunerationTypes,
        ReportFormatTypes,
        ReportGroups,
        ReportParameterTypes,
        ResidenceStatuses,
        RiskMatrixDimensions,
        RoleTypes,
        RuleExclusionSets,
        RuleItems,
        Salutations,
        SPVs,
        StageDefinitionGroups,
        SubsidyProviders,
        SubsidyProviderTypes,
        TitleTypes,
        TransactionTypes,
        UIStatementTypes,
        ValuationClassifications,
        ValuationDataProviderDataServices,
        ValuationImprovementTypes,
        ValuationRoofTypes,
        ValuationStatuses,
        WorkflowMenus,
        WorkflowRuleSets
    }

    /// <summary>
    /// Repository containing cached data.  All properties are by default not loaded, and get loaded once accessed.  Once
    /// loaded, the data will not get retrieved from the database again unless it is cleared out using the <see cref="ResetLookup"/>
    /// or the <see cref="ResetAll()"/> methods.
    /// <para>
    /// <strong>Developers:</strong> Do not cache ActiveRecord or ActiveRecord-based objects - there are very few cases
    /// where this is necessary (for example the RuleItems which are loaded by the RuleService).  In almost all cases
    /// a dictionary will suffice (an ISAHLDictionary) can be used, or if more than a key/description pair is required
    /// the use a basic read-only object (e.g. ICountryReadOnly).
    /// </para>
    /// </summary>
    public interface ILookupRepository
    {
        /// <summary>
        /// This will remove a lookup item from memory, forcing it to be reloaded the next time it is accessed.
        /// </summary>
        /// <param name="key">One of the keys in <see cref="LookupKeys"/></param>
        /// <remarks>This will automatically call the appropriate <see cref="IX2Repository"/> method to clear out X2's corresponding cache.</remarks>
        void ResetLookup(LookupKeys key);

        void ClearRuleCache();

        /// <summary>
        /// Removes all lookup items from memory, forcing them to be reloaded the next time they are accessed.
        /// </summary>
        /// <remarks>This will automatically call the appropriate <see cref="IX2Repository"/> method to clear out X2's corresponding cache.</remarks>
        void ResetAll();

        /// <summary>
        /// Gets a dictionary of AccountStatus Key/Description values.
        /// </summary>
        IDictionary<int, string> AccountStatuses { get; }

        /// <summary>
        /// Returns a dictionary containing a list of address format values for a specified <see cref="IAddressType"/>.
        /// </summary>
        /// <param name="addressType"></param>
        /// <returns></returns>
        ISAHLDictionary<int, string> AddressFormatsByAddressType(AddressTypes addressType);

        /// <summary>
        /// Gets a dictionary containing all <see cref="IAddressFormat"/> Key/Description values.
        /// </summary>
        /// <returns></returns>
        ISAHLDictionary<int, string> AddressFormats { get; }

        /// <summary>
        /// Gets a dictionary containing all <see cref="IAddressType"/> Key/Description values.
        /// </summary>
        IDictionary<int, string> AddressTypes { get; }

        /// <summary>
        /// Gets a list of all ADUserGroups in the FeatureGroup table.
        /// </summary>
        string[] ADUserGroups { get; }

        IEventList<IApplicationDeclarationAnswer> ApplicationDeclarationAnswers { get; }

        IEventList<IAffordabilityType> AffordabilityTypes { get; }

        /// <summary>
        /// Gets a list of all CBOMenus (not specific to user).
        /// </summary>
        IEventList<ICBOMenu> CBOMenus { get; }

        /// <summary>
        /// Gets a list of all ContextMenus (not specific to user).
        /// </summary>
        IEventList<IContextMenu> ContextMenus { get; }

        IEventList<IApplicationType> ApplicationTypes { get; }

        IEventList<IApplicantType> ApplicantTypes { get; }

        IEventList<IAssetLiabilitySubType> AssetLiabilitySubTypes { get; }

        IEventList<IApplicationSource> ApplicationSources { get; }

        IEventList<IApplicationAttributeType> ApplicationAttributesTypes { get; }

        IEventList<IApplicationRoleAttributeType> ApplicationRoleAttributesTypes { get; }

        /// <summary>
        /// Gets a dictionary of application role types.
        /// </summary>
        IDictionary<int, string> ApplicationRoleTypes { get; }

        IEventList<IAreaClassification> AreaClassifications { get; }

        IEventList<IAssetLiabilityType> AssetLiabilityTypes { get; }

        IEventList<IAttorney> Attorneys { get; }

        IEventList<IACBBank> Banks { get; }

        IEventList<IACBBranch> BankBranches { get; }

        IEventList<IACBType> BankAccountTypes { get; }

        IEventList<IBatchTransactionStatus> BatchTransactionStatuses { get; }

        IEventList<IBulkBatchStatus> BulkBatchStatuses { get; }

        IEventList<IBulkBatchType> BulkBatchTypes { get; }

        IEventList<ICancellationType> CancellationTypes { get; }

        IEventList<ICapStatus> CapStatuses { get; }

        IDictionary<int, IDictionary<int, string>> CBOInputGenericTypes { get; }

        IEventList<ICitizenType> CitizenTypes { get; }

        IEventList<IControl> Controls { get; }

        IEventList<IParameterType> ParameterTypes { get; }

        IEventList<IReportParameterType> ReportParameterTypes { get; }

        IEventList<IRecurringTransactionType> RecurringTransactionTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<ICorrespondenceMedium> CorrespondenceMediums { get; }

        /// <summary>
        /// Gets a list of countries.
        /// </summary>
        IDictionary<int, ICountryReadOnly> Countries { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IDataProvider> DataProviders { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IValuationDataProviderDataService> ValuationDataProviderDataServices { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IPropertyDataProviderDataService> PropertyDataProviderDataServices { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IDataService> DataServices { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IDebitOrderDay> DebitOrderDays { get; }

        IEventList<IDeedsPropertyType> DeedsPropertyTypes { get; }

        IEventList<IDeedsOffice> DeedsOffice { get; }

        IEventList<IDetailClass> DetailClasses { get; }

        IEventList<IDetailType> DetailTypes { get; }

        IEventList<IDisbursementStatus> DisbursementStatuses { get; }

        IEventList<IApplicationOriginator> ApplicationOriginators { get; }

        IEventList<IEducation> Educations { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IEmployerBusinessType> EmployerBusinessTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IEmploymentSector> EmploymentSectors { get; }

        /// <summary>
        ///Get only active Employment sectors
        /// </summary>
        ///
        IDictionary<int, string> EmploymentSectorsActive { get; }

        IEventList<IEmploymentType> EmploymentTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IEmploymentStatus> EmploymentStatuses { get; }

        IEventList<IExpenseType> ExpenseTypes { get; }

        IEventList<IExternalRoleType> ExternalRoleTypes { get; }

        IEventList<IFeatureGroup> FeatureGroups { get; }

        IEventList<IFinancialServiceGroup> FinancialServiceGroups { get; }

        IEventList<IFinancialServicePaymentType> FinancialServicePaymentTypes { get; }

        IEventList<IFutureDatedChangeType> FutureDatedChangeTypes { get; }

        IEventList<IGender> Genders { get; }

        /// <summary>
        /// Gets a dictionary of <see cref="IGeneralStatus"/> key/objects.  The actual objects themselves can
        /// be stored as there are no entity type properties.
        /// </summary>
        IDictionary<GeneralStatuses, IGeneralStatus> GeneralStatuses { get; }
		IDictionary<GeneralStatuses, IGeneralStatus> GetGeneralStatuses(params GeneralStatuses[] exclusions);

        /// <summary>
        ///
        /// </summary>
        IEventList<IGenericKeyType> GenericKeyType { get; }

        IEventList<IHelpDeskCategory> HelpDeskCategories { get; }

        IEventList<IHOCInsurer> HOCInsurers { get; }

        IEventList<IHOCConstruction> HOCConstruction { get; }

        IEventList<IHOCSubsidence> HOCSubsidence { get; }

        IEventList<IHOCRoof> HOCRoof { get; }

        IEventList<IHOCStatus> HOCStatus { get; }

        IEventList<IInsurer> Insurers { get; }

        IEventList<ILegalEntityExceptionStatus> LegalEntityExceptionStatuses { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<ILegalEntityRelationshipType> LegalEntityRelationshipTypes { get; }

        IEventList<ILegalEntityStatus> LegalEntityStatuses { get; }

        /// <summary>
        /// Gets a list of languages stored by key.
        /// </summary>
        IDictionary<int, ILanguageReadOnly> Languages { get; }

        /// <summary>
        /// Gets a list of translatable languages.
        /// </summary>
        ReadOnlyCollection<ILanguageReadOnly> LanguagesTranslatable { get; }

        IEventList<ILegalEntityType> LegalEntityTypes { get; }

        IEventList<ILifeInsurableInterestType> LifeInsurableInterestTypes { get; }

        IEventList<ILifePolicyStatus> LifePolicyStatuses { get; }

        IEventList<ILifePolicyType> LifePolicyTypes { get; }

        IEventList<IMargin> Margins { get; }

        IEventList<IMaritalStatus> MaritalStatuses { get; }

        IEventList<IMarketingOption> MarketingOptions { get; }

        /// <summary>
        /// Gets all active marketing options.
        /// </summary>
        IDictionary<int, string> MarketingOptionsActive { get; }

        IEventList<IMarketRate> MarketRates { get; }

        IEventList<IMessageType> MessageTypes { get; }

        IEventList<IMortgageLoanPurpose> MortgageLoanPurposes { get; }

        IEventList<IOnlineStatementFormat> OnlineStatementFormats { get; }

        IEventList<IOrganisationStructure> OrganisationStructure { get; }

        IEventList<IOrganisationStructureOriginationSource> OrganisationStructureOrgStructure { get; }

        IEventList<IOriginationSource> OriginationSources { get; }

        IEventList<IProduct> Products { get; }

        IEventList<IPopulationGroup> PopulationGroups { get; }

        IEventList<IPropertyType> PropertyTypes { get; }

        IEventList<ITitleType> TitleTypes { get; }

        IEventList<IOccupancyType> OccupancyTypes { get; }

        /// <summary>
        /// Returns a list of provinces for a specific country.
        /// </summary>
        /// <param name="countryKey">The key of the country.</param>
        /// <returns></returns>
        IDictionary<int, string> ProvincesByCountry(int countryKey);

        IEventList<IPriority> PrioritiesByOSP(int originationSourceProductKey);

        ////IEventList<IOriginationSourceProduct> OriginationSourceProducts { get;}

        IEventList<IResidenceStatus> ResidenceStatuses { get; }

        IEventList<IRoleType> RoleTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        IEventList<IReportFormatType> ReportFormatTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IReportGroup> ReportGroups { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IReasonDescription> ReasonDescriptions { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IReasonType> ReasonTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IReasonTypeGroup> ReasonTypeGroups { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IRemunerationType> RemunerationTypes { get; }

        /// <summary>
        /// Gets a list of types that can be used as rule param types. EG Int16, Int32, Int64, Bool, String etc
        /// </summary>
        IEventList<ISalutation> Salutations { get; }

        IEventList<ISPV> SPVList { get; }

        //IEventList<ISubsidyProvider> SubsidyProviders { get; }
        IEventList<ISubsidyProviderType> SubsidyProviderTypes { get; }

        IEventList<ITransactionType> TransactionTypes { get; }

        /// <summary>
        /// Gets a list of organisation types (Branch, Division etc)
        /// </summary>
        IEventList<IOrganisationType> OrganisationTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IStageDefinitionGroup> StageDefinitionGroups { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IApplicationStatus> ApplicationStatuses { get; }

        IEventList<IDisbursementType> DisbursementTypes { get; }

        IEventList<IValuationStatus> ValuationStatus { get; }

        IEventList<IValuationImprovementType> ValuationImprovementType { get; }

        IEventList<IValuationClassification> ValuationClassification { get; }

        IEventList<IValuationRoofType> ValuationRoofTypes { get; }

        /// <summary>
        /// Gets a keyed list of workflow menus, the sub menus that appear for a particular state under an instance node.
        /// </summary>
        IDictionary<string, IWorkflowMenu> WorkflowMenus { get; }

        /// <summary>
        /// Gets a keyed list of ApplicationInformationTypes.
        /// </summary>
        IEventList<IApplicationInformationType> ApplicationInformationTypes { get; }

        /// <summary>
        /// Gets a keyed list of ICategories.
        /// </summary>
        IEventList<ICategory> Categories { get; }

        /// <summary>
        /// Gets a keyed list of IFinancialAdjustmentTypes.
        /// </summary>
        IEventList<IFinancialAdjustmentType> FinancialAdjustmentTypes { get; }

        /// <summary>
        /// Gets a keyed list of IFinancialAdjustmentSource.
        /// </summary>
        IEventList<IFinancialAdjustmentSource> FinancialAdjustmentSources { get; }

        /// <summary>
        /// Get list of Quick cash payment types
        /// </summary>
        IEventList<IQuickCashPaymentType> QuickCashPaymentTypes { get; }

        /// <summary>
        /// Get list of ImportStatuses
        /// </summary>
        IEventList<IImportStatus> ImportStatuses { get; }

        /// <summary>
        /// Get list of RuleExclusionSets
        /// </summary>
        IEventList<IRuleExclusionSet> RuleExclusionSets { get; }

        /// <summary>
        /// Gets a list of RuleItems with the key being the rule key.
        /// </summary>
        /// <seealso cref="RuleItemsByName"/>
        IEventList<IRuleItem> RuleItems { get; }

        /// <summary>
        /// Gets list of RuleItems, with the key being the rule name.
        /// </summary>
        /// <see cref="RuleItems"/>
        IDictionary<string, IRuleItem> RuleItemsByName { get; }

        /// <summary>
        /// Gets a list of AccountIndicators
        /// </summary>
        /// <seealso cref="RuleItemsByName"/>
        IEventList<IAccountIndication> AccountIndications { get; }

        /// <summary>
        /// Returns a list of Active HelpDeskCategories.
        /// </summary>
        /// <returns></returns>
        IList<IHelpDeskCategory> HelpDeskCategoriesActive(int selectedHelpDeskCategoryKey);

        /// <summary>
        /// Gets a list of workflow RuleSets
        /// </summary>
        IEventList<IWorkflowRuleSet> WorkflowRuleSets { get; }

        /// <summary>
        ///
        /// </summary>
        IList<IWorkflowRuleSet> WorkflowRuleSetSorted { get; }

        /// <summary>
        ///
        /// </summary>
        IList<IRuleItem> RuleItemList { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IEmploymentConfirmationSource> EmploymentConfirmationSources { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IEmploymentVerificationProcessType> EmploymentVerificationProcessTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IRiskMatrixDimension> RiskMatrixDimensions { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<CreditScoreDecisions, ICreditScoreDecision> CreditScoreDecisions { get; }

        /// <summary>
        /// Gets a dictionary of <see cref="IMarketingOptionRelevance"/> key/objects.  The actual objects themselves can
        /// be stored as there are no entity type properties.
        /// </summary>
        IDictionary<MarketingOptionRelevances, IMarketingOptionRelevance> MarketingOptionRelevances { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<ProposalStatuses, IProposalStatus> ProposalStatuses { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<DebtCounsellingStatuses, IDebtCounsellingStatus> DebtCounsellingStatuses { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<ProposalTypes, IProposalType> ProposalTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<int, string> CourtTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IDictionary<int, string> HearingTypes { get; }

        /// <summary>
        /// Litigation Attorney Role Types
        /// </summary>
        IDictionary<int, string> LitigationAttorneyRoleTypes { get; }

        /// <summary>
        ///
        /// </summary>
        IEventList<IFinancialAdjustmentStatus> FinancialAdjustmentStatuses { get; }

        IDictionary<int, string> ClaimTypes { get; }

        IDictionary<int, string> ClaimStatuses { get; }

        IDictionary<int, string> DisabilityTypes { get; }

        IEventList<IAffordabilityAssessmentStatus> AffordabilityAssessmentStatuses { get; }
    }
}