using System;

namespace SAHL.Web.Views
{
    /// <summary>
    /// 
    /// </summary>
    public static class ViewConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string GenericKey = "GENERICKEY";

		/// <summary>
		/// State ID
		/// </summary>
		public const string StateID = "STATEID";
        /// <summary>
        /// 
        /// </summary>
        public const string GenericKeyType = "GENERICKEYTYPE";
        /// <summary>
        /// 
        /// </summary>
        public const string ReportParameters = "REPORTPARAMETERS";

        /// <summary>
        /// 
        /// </summary>
        public const string ReportStatement = "REPORTSTATEMENT";
        /// <summary>
        /// 
        /// </summary>
        public const string ReportStatementData = "REPORTSTATEMENTDATA";
        /// <summary>
        /// 
        /// </summary>
        public const string ReportParameterData = "REPORTPARAMETERDATA";
        /// <summary>
        /// 
        /// </summary>
        public const string SelReportIndex = "SELREPORTINDEX";
        /// <summary>
        /// 
        /// </summary>
        public const string ReportGroupData = "REPORTGROUPDATA";
        /// <summary>
        /// 
        /// </summary>
        public const string ReportGroupIndex = "REPORTGROUPINDEX";

        /// <summary>
        /// 
        /// </summary>
        public const string ReportGroup = "REPORTGROUP";

        /// <summary>
        /// 
        /// </summary>
        public const string OriginalReportParameterValues = "ORIGINALREPORTPARAMETERVALUES";

        /// <summary>
        /// 
        /// </summary>
        public const string OrganisationStructureKey = "ORGANISATIONSTRUCTUREKEY";

        /// <summary>
        /// 
        /// </summary>
        public const string ParentOrganisationStructureKey = "PARENTORGANISATIONSTRUCTUREKEY";

        /// <summary>
        /// 
        /// </summary>     
        public const string SelectedTreeNodeKey = "SELECTEDTREENODEKEY";

        /// <summary>
        /// 
        /// </summary>
        public const string SelectedTreeNodeParentKey = "SELECTEDTREENODEPARENTKEY";

        /// <summary>
        /// 
        /// </summary>
        public const string ReportRequestString = "REPORTREQUESTSTRING";

        /// <summary>
        /// 
        /// </summary>
        public const string ReportPath = "REPORTPATH";

        /// <summary>
        /// 
        /// </summary>
        public const string LegalEntityList = "LEGALENTITYLIST";

        /// <summary>
        /// 
        /// </summary>
        public const string SelectedAjaxLegalEntityKey = "SELECTEDAJAXLEGALENTITYKEY";
        /// <summary>
        /// 
        /// </summary>
        public const string SelectedLegalEntityKey = "SELECTEDLEGALENTITYKEY";

        /// <summary>
        /// 
        /// </summary>
        public const string SelectedLifeAccountKey = "SELECTEDLIFEACCOUNTKEY";

        /// <summary>
        /// 
        /// </summary>
        public const string SelectedPropertyKey = "SELECTEDPROPERTYKEY";

        /// <summary>
        /// constant to represent the global cache object for an ILegalEntity
        /// </summary>
        public const string LegalEntity = "LEGALENTITY";

        /// <summary>
        /// constant to represent the global cache object for a LegalEntityKey
        /// </summary>
        public const string LegalEntityKey = "LEGALENTITYKEY";

        public const string SelectedAccountKey = "SELECTEDACCOUNTKEY";

        /// <summary>
        /// Constant to represent the global cache object for an IApplication
        /// </summary>
        public const string Application = "APPLICATION";
        public const string ApplicationKey = "APPLICATIONKEY";
        public const string ApplicationRoleKey = "APPLICATIONROLEKEY";

        /// <summary>
        /// Dictionary containing the required values to calculate the Amortisation Schedule
        /// </summary>
        public const string AmortisationSchedule = "AmortisationSchedule";
        public const string GenericCalc = "GenericCalc";

        //public const string InstanceID = "INSTANCEID";

        /// <summary>
        /// constant to represent the global cache object for an IAddress
        /// </summary>
        public const string Address = "ADDRESS";

        /// <summary>
        /// Used to pass back the Insurable Interest Type for a newly added Legal Entity
        /// </summary>
        public const string LifeInsurableInterestTypeKey = "LIFEINSURABLEINTERESTTYPEKEY";

        /// <summary>
        /// holds the search criteria from life lead creation search
        /// </summary>
        public const string LifeSearchCriteria = "LIFESEARCHCRITERIA";

        /// <summary>
        /// holds the search results from life lead creation search
        /// </summary>
        public const string LifeSearchResults = "LIFESEARCHRESULTS";

        /// <summary>
        /// holds the list of adusers for the selected roletype
        /// </summary>
        public const string BatchReassignUsers = "BATCHREASSIGNUSERS";

        /// <summary>
        /// holds the selected user
        /// </summary>
        public const string BatchReassignSelectedSearchUser = "BATCHREASSIGNSELECTEDSEARCHUSER";

        /// <summary>
        /// 
        /// </summary>
        public const string BatchReassignSelectedReassignUser = "BATCHREASSIGNSELECTEDREASSIGNUSER";

        /// <summary>
        /// 
        /// </summary>
        public const string BatchReassignSelectedRoleType = "BATCHREASSIGNSELECTEDROLETYPE";

        /// <summary>
        /// 
        /// </summary>
        public const string ApplicationInstanceDictionary = "APPLICATIONINSTANCEDICTIONARY";

        /// <summary>
        /// holds the search criteria from workflow batch reassign search
        /// </summary>
        public const string ApplicationSearchCriteria = "APPLICATIONSEARCHCRITERIA";

        /// <summary>
        /// holds the search results from workflow batch reassign search
        /// </summary>
        public const string ApplicationSearchResults = "APPLICATIONSEARCHRESULTS";

        /// <summary>
        /// Used to pass back the message from the correspondence page to the calling page
        /// </summary>
        public const string CorrespondenceMessage = "CORRESPONDENCEMESSAGE";

        //public const string CorrespondenceList = "CORRESPONDENCELIST";
        public const string CorrespondenceReportDataList = "CORRESPONDENCEREPORTDATALIST";

        public const string CorrespondenceNavigateTo = "CORRESPONDENCENAVIGATETO";

        public const string CorrespondenceMediumInfo = "CORRESPONDENCEMEDIUMINFO";

        /// <summary>
        /// constant to represent the global cache object for an offer condition set
        /// </summary>
        public const string ConditionSet = "CONDITIONSET";

        /// <summary>
        /// Constant used for storing an <see cref="SAHL.Common.BusinessModel.Interfaces.IEmployer">IEmployer</see> key in the cache.
        /// </summary>
        public const string EmployerKey = "EmployerKey";

        /// <summary>
        /// Constant used for storing an <see cref="SAHL.Common.BusinessModel.Interfaces.IEmployment">IEmployment</see>  entity in the cache.
        /// </summary>
        public const string Employment = "Employment";

        /// <summary>
        /// Constant used for storing an <see cref="SAHL.Common.BusinessModel.Interfaces.IEmployment">IEmployment</see> key in the cache.
        /// </summary>
        public const string EmploymentKey = "EmploymentKey";

        // todo: Remove this once we have implemted "LifeTimes" between views
        /// <summary>
        /// Used to hold the view name of the last visted view - to enable 'cancel' button navigation back to it
        /// </summary>
        public const string NavigateTo = "NAVIGATETO";

        /// <summary>
        /// Selected Susbidy Provider
        /// </summary>
        public const string SubsidyProvider = "SelectedSusbidyProvider";

        /// <summary>
        /// constant to represent the global cache object for a Release and Variations Data set
        /// </summary>
        public const string ReleaseAndVariationsDataSet = "RELEASEANDVARIATIONSDATASET";

        /// <summary>
        /// constant to represent the global cache object for a New unsaved Application
        /// </summary>
        public const string CreateApplication = "CREATEAPPLICATION";

        public const string EstateAgentApplication = "ESTATEAGENTAPPLICATION";

        public const string SelectedAssetLiabilityType = "SelectedLegalEntityAssetLiabilityType";

        public const string SelectedAssetLiabilityDateAcquired = "SelectedLegalEntityAssetDateAcquired";

        public const string SelectedAssetLiabilityAssetValue = "SelectedLegalEntityAssetValue";

        public const string SelectedAssetLiabilityLiabilityValue = "SelectedLegalEntityLiabilityValue";

        public const string SelectedValuationKey = "SelectedValuationKey";

        public const string ValuationManual = "ValuationManual";
        public const string ValuationClassification = "ValuationClassification";
        public const string ValuationEscalationPercentage = "ValuationEscalationPercentage";
        public const string ValuationMainBuilding = "ValuationMainBuilding";
        public const string ValuationCottage = "ValuationCottage";

        public const string Properties = "Properties";

        public const string ValuationPresenter = "ValuationPresenter";

        public const string DeclineReasonsHistory = "DeclineReasonsHistory";

        /// <summary>
        /// constant to represent the global cache object for an offer condition set
        /// </summary>
        public const string LightStonePropertyData = "LightStonePropertyData";
        /// <summary>
        /// OfferInformationQuickCash Details screen - used to pass value of selected grid index
        /// from one Third Party Payment Screen to the Next.
        /// </summary>
        public const string QuickCashDetailKey = "QuickCashDetailKey";

        public const string InstanceID = "INSTANCEID";
        public const string InstanceReloadAttempts = "INSTANCEIDRELOADATTEMPTS";

        /// <summary>
        /// BulkBatch screens pass the key around so it's selected on subsequent screens.
        /// </summary>
        public const string BulkBatchTypeKey = "BulkBatchTypeKey";

        /// <summary>
        /// The key used for storing Workflow Search data in the cache.
        /// </summary>
        public const string WorkFlowSearchResultsKey = "WorkFlowSearchResults";

        /// <summary>
        /// 
        /// </summary>
        public const string DeleteEmploymentVerificationProcessList = "DeleteEmploymentVerificationProcess";

        /// <summary>
        /// 
        /// </summary>
        public const string AddEmploymentVerificationProcessList = "AddEmploymentVerificationProcess";

        /// <summary>
        /// 
        /// </summary>
        public const string EmploymentConfirmationSourceKey = "EmploymentConfirmationSourceKey";

        /// <summary>
        /// 
        /// </summary>
        public const string ProposalKey = "PROPOSALKEY";

        /// <summary>
        /// 
        /// </summary>
        public const string Proposal = "PROPOSAL";

        /// <summary>
        /// 
        /// </summary>
        public const string DebtCounsellingKey = "DEBTCOUNSELLINGKEY";

        /// <summary>
        /// 
        /// </summary>
        public const string DebtCounsellorLegalEntityKey = "DebtCounsellorLegalEntityKey";

		/// <summary>
		/// 
		/// </summary>
		public const string PaymentDistributionAgenctLegalEntityKey = "PaymentDistributionAgenctLegalEntityKey";

        /// <summary>
        /// 
        /// </summary>
        public const string CacheReasons = "CacheReasons";

        /// <summary>
        /// 
        /// </summary>
        public const string WizardPage = "WizardPage";

        /// <summary>
        /// 
        /// </summary>
        public const string CancelView = "CancelView";

        /// <summary>
        /// 
        /// </summary>
        public const string SelectView = "SelectView";

        /// <summary>
        /// 
        /// </summary>
        public const string ReasonTypeKey = "ReasonTypeKey";

        public const string ReasonHistoryTitle = "ReasonHistoryTitle";

        public const string OldMutualDeveloperLoan = "OldMutualDeveloperLoan";

        /// <summary>
        /// 
        /// </summary>
        public const string AffordabilityAssessmentKey = "AFFORDABILITYASSESSMENTKEY";

    }
}
