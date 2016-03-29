using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using SAHL.Common.BusinessModel.CorrespondenceGeneration;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceStrategies;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System.Linq;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LifeDomain.Models;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    /// <summary>
    ///
    /// </summary>
    public class CorrespondenceProcessingBase : SAHLCommonBasePresenter<ICorrespondenceProcessing>
    {
        private CBOMenuNode _node;
        protected bool _getRootContextNode = false;
        public int _genericKey, _genericKeyTypeKey, _loanNumber;
        private int _stageDefinitionGenericKey, _parentAccountKey = -1, _applicationAccountKey = -1;
        private int _originationSourceProductKey;
        private IAccount _account;
        private IApplication _application;
        private IAccountRepository _accountRepo;
        private IFinancialServiceRepository _financialServiceRepo;
        private IApplicationRepository _applicationRepo;
        private ICorrespondenceRepository _correspondenceRepo;
        private IReportRepository _reportRepo;
        private ILookupRepository _lookupRepo;
        private ICapRepository _capRepo;
        public ILegalEntityRepository _legalEntityRepo;
        public IDebtCounsellingRepository _debtCounsellingRepo;
        private IReadOnlyEventList<ILegalEntity> _lstLegalEntities;
        private List<ReportData> _reportDataList;
        private IEventList<IAddress> _lstMailingAddresses;
        private IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
        private IADUser _adUser;
        protected ILegalEntity legalEntity;
        private IV3ServiceManager v3ServiceManager;
        private IMortgageLoanDomainService mortgageLoanDomainService;
        private ILifeDomainService lifeDomainService;
        
        public CBOMenuNode Node
        {
            get { return _node; }
        }

        public int GenericKey
        {
            get { return _genericKey; }
        }

        /// <summary>
        /// Gets the <see cref="IADUser"/> for the current principal.
        /// </summary>
        protected IADUser CurrentADUser
        {
            get
            {
                if (_adUser == null)
                {
                    ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
                    _adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);
                }
                return _adUser;
            }
        }

        public int StageDefinitionGenericKey
        {
            get { return _stageDefinitionGenericKey; }
        }

        private bool _useParentAccountKeyParameter;

        public bool UseParentAccountKeyParameter
        {
            set { _useParentAccountKeyParameter = value; }
        }

        private bool _useApplicationAccountKeyParameter;

        public bool UseApplicationAccountKeyParameter
        {
            get { return _useApplicationAccountKeyParameter; }
            set { _useApplicationAccountKeyParameter = value; }
        }

        private bool _userDebtCounsellingAccountKey;

        public bool UserDebtCounsellingAccountKey
        {
            set { _userDebtCounsellingAccountKey = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceProcessingBase(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
            {
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);
            }

            v3ServiceManager = V3ServiceManager.Instance;
            mortgageLoanDomainService = v3ServiceManager.Get<IMortgageLoanDomainService>();
            lifeDomainService = v3ServiceManager.Get<ILifeDomainService>();

            // get the generic key and type
            if (_node is InstanceNode)
            {
                InstanceNode inode = _node as InstanceNode;

                // Get the Instance Data
                Dictionary<string, object> x2Data = inode.X2Data as Dictionary<string, object>;

                if (x2Data.ContainsKey("ApplicationKey"))
                    _genericKey = x2Data["ApplicationKey"] == System.DBNull.Value ? -1 : Convert.ToInt32(x2Data["ApplicationKey"]);
                else if (x2Data.ContainsKey("OfferKey"))
                    _genericKey = x2Data["OfferKey"] == System.DBNull.Value ? -1 : Convert.ToInt32(x2Data["OfferKey"]);
                else if (x2Data.ContainsKey("CapOfferKey"))
                    _genericKey = x2Data["CapOfferKey"] == System.DBNull.Value ? -1 : Convert.ToInt32(x2Data["CapOfferKey"]);
                else if (x2Data.ContainsKey("DebtCounsellingKey"))
                    _genericKey = x2Data["DebtCounsellingKey"] == System.DBNull.Value ? -1 : Convert.ToInt32(x2Data["DebtCounsellingKey"]);
                else if (x2Data.ContainsKey("DisabilityClaimKey"))
                    _genericKey = x2Data["DisabilityClaimKey"] == System.DBNull.Value ? -1 : Convert.ToInt32(x2Data["DisabilityClaimKey"]);

                _genericKeyTypeKey = inode.GenericKeyTypeKey;
            }
            else
            {
                _genericKey = _node.GenericKey;
                _genericKeyTypeKey = _node.GenericKeyTypeKey;

                // if this is a static node : ie _node.GenericKey == -1
                // then we must use the values off the parent node
                if (_node.GenericKey == -1)
                {
                    if (_node.ParentNode != null)
                    {
                        _genericKey = _node.ParentNode.GenericKey;
                        _genericKeyTypeKey = _node.ParentNode.GenericKeyTypeKey;
                    }
                }
            }

            _view.GenericKey = _genericKey;
            _view.GenericKeyTypeKey = _genericKeyTypeKey;

            _stageDefinitionGenericKey = _genericKey;

            // hide the life workflow header control
            _view.ShowLifeWorkFlowHeader = false;
            _view.ShowCancelButton = false;

            // set the default mode to single recipient
            _view.MultipleRecipientMode = false;
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (_getRootContextNode)
            {
                // walk up tree till we get a node with debtcounselling key
                SAHL.Common.UI.CBONode node = Node.GetParentNodeByType(GenericKeyTypes.DebtCounselling2AM);
                if (node != null)
                {
                    _genericKey = node.GenericKey;
                    _genericKeyTypeKey = (int)GenericKeyTypes.DebtCounselling2AM;
                }
            }
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // initialise repositories
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
            _reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            _financialServiceRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            _capRepo = RepositoryFactory.GetRepository<ICapRepository>();

            // initialise events
            _view.OnSendButtonClicked += new EventHandler(OnSendButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.OnAddressAddButtonClicked += new KeyChangedEventHandler(OnAddressAddButtonClicked);
            _view.OnRecipientsGridSelectedIndexChanged += new KeyChangedEventHandler(OnRecipientsGridSelectedIndexChanged);
            _view.OnPreviewButtonClicked += new EventHandler(OnPreviewButtonClicked);

            _account = null;
            _application = null;

            _view.DocumentLanguageKey = (int)SAHL.Common.Globals.Languages.English;
            _view.DocumentLanguageDesc = _lookupRepo.Languages[(int)SAHL.Common.Globals.Languages.English].Description;

            _lstMailingAddresses = new EventList<IAddress>();

            _originationSourceProductKey = 1;

            switch (_genericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                case (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                    if (_genericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService)
                    {
                        // Get the Financial service object
                        IFinancialService financialService = _financialServiceRepo.GetFinancialServiceByKey(_genericKey);

                        // Get the Account from the financialservice
                        _account = financialService.Account;

                        // set the generic key to be account
                        _genericKey = _account.Key;
                        _genericKeyTypeKey = (int)SAHL.Common.Globals.GenericKeyTypes.Account;
                    }
                    else
                    {
                        // Get the Account Object
                        _account = _accountRepo.GetAccountByKey(_genericKey);
                    }

                    _originationSourceProductKey = _account.OriginationSourceProduct.Key;

                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:

                    // Get the Application Object
                    _application = _applicationRepo.GetApplicationByKey(_genericKey);

                    if (_application.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Life)
                    {
                        _account = _application.Account;
                        _genericKey = _account.Key;
                        _genericKeyTypeKey = (int)SAHL.Common.Globals.GenericKeyTypes.Account;
                        _originationSourceProductKey = _account.OriginationSourceProduct.Key;
                    }

                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.CapOffer:
                    ICapApplication _capApplication = _capRepo.GetCapOfferByKey(_genericKey);
                    _account = _capApplication.Account;
                    _genericKey = _account.Key;
                    _genericKeyTypeKey = (int)SAHL.Common.Globals.GenericKeyTypes.Account;
                    break;

                case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                    _account = _debtCounsellingRepo.GetDebtCounsellingByKey(_genericKey).Account;
                    break;
                case (int)SAHL.Common.Globals.GenericKeyTypes.LegalEntity:
                    {
                        legalEntity = _legalEntityRepo.GetLegalEntityByKey(_genericKey);
                        break;
                    }
                case (int)SAHL.Common.Globals.GenericKeyTypes.DisabilityClaim:
                    {
                        DisabilityClaimDetailModel disabilityClaim = null;
                        GetDisabilityClaimByKeyQuery getDisabilityClaimByKeyQuery = new GetDisabilityClaimByKeyQuery(_genericKey);
                        ISystemMessageCollection systemMessageCollection = lifeDomainService.PerformQuery(getDisabilityClaimByKeyQuery);

                        if (!systemMessageCollection.HasErrors)
                        {
                            disabilityClaim = getDisabilityClaimByKeyQuery.Result.Results.FirstOrDefault();
                            _loanNumber = disabilityClaim.LifeAccountKey;
                        }
                        else
                        {
                            v3ServiceManager.HandleSystemMessages(systemMessageCollection);
                            return;
                        }

                        legalEntity = _legalEntityRepo.GetLegalEntityByKey(disabilityClaim.LegalEntityKey);
                        break;
                    }
                default:
                    break;
            }

            if (_account != null)
            {
                // set the loan number to use in dataSTOR
                if (_account is IAccountLifePolicy || _account is IAccountHOC)
                    _loanNumber = _account.ParentAccount.Key;
                else
                    _loanNumber = _account.Key;

                // Get Parent AccountKey if any
                IAccount parmAccount = _account;
                _parentAccountKey = -1;
                if (_useParentAccountKeyParameter)
                {
                    if (_account.ParentAccount != null)
                    {
                        parmAccount = _account.ParentAccount;
                        _parentAccountKey = parmAccount.Key;
                        _loanNumber = _parentAccountKey;
                    }
                }

                // Get the roles off the account
                int[] roleTypes = new int[3] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife, (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor };
                _lstLegalEntities = parmAccount.GetLegalEntitiesByRoleType(_view.Messages, roleTypes, GeneralStatusKey.Active);

                // if the account is a life policy account and there are no assured lives on it then use the roles off the loan account
                if (parmAccount is IAccountLifePolicy && _lstLegalEntities.Count < 1)
                {
                    _lstLegalEntities = parmAccount.ParentAccount.GetLegalEntitiesByRoleType(_view.Messages, roleTypes, GeneralStatusKey.Active);
                }

                // set the mailing address
                if (parmAccount.MailingAddresses.Count > 0)
                {
                    _lstMailingAddresses.Add(_view.Messages, parmAccount.MailingAddresses[0].Address);

                    // set the languagekey
                    _view.DocumentLanguageKey = parmAccount.MailingAddresses[0].Language.Key;
                    _view.DocumentLanguageDesc = parmAccount.MailingAddresses[0].Language.Description;
                }
            }
            else if (_application != null)
            {
                // Get Applications AccountKey if any
                _applicationAccountKey = _application.ReservedAccount != null ? _application.ReservedAccount.Key : -1;
                _loanNumber = _applicationAccountKey;

                // Get the roles off the application

                // TODO: Nasty hack for any application per unsecured lending
                if (_application.ApplicationType.Key < (int)OfferTypes.UnsecuredLending)
                {
                    SAHL.Common.Globals.OfferRoleTypes[] offerRoleTypes = new SAHL.Common.Globals.OfferRoleTypes[5] { SAHL.Common.Globals.OfferRoleTypes.AssuredLife, SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant, SAHL.Common.Globals.OfferRoleTypes.LeadSuretor, SAHL.Common.Globals.OfferRoleTypes.MainApplicant, SAHL.Common.Globals.OfferRoleTypes.Suretor };
                    _lstLegalEntities = _application.GetLegalEntitiesByRoleType(offerRoleTypes, GeneralStatusKey.Active);
                }
                else
                {
                    _lstLegalEntities = _legalEntityRepo.GetLegalEntitiesByExternalRoleTypeGroup(_node.GenericKey, _node.GenericKeyTypeKey, ExternalRoleTypeGroups.Client, GeneralStatuses.Active);
                }

                // set the mailing address
                if (_application.ApplicationMailingAddresses.Count > 0)
                {
                    _lstMailingAddresses.Add(_view.Messages, _application.ApplicationMailingAddresses[0].Address);

                    // set the languagekey
                    _view.DocumentLanguageKey = _application.ApplicationMailingAddresses[0].Language.Key;
                    _view.DocumentLanguageDesc = _application.ApplicationMailingAddresses[0].Language.Description;
                }
            }
            else if (legalEntity != null)
            {
                _lstLegalEntities = new ReadOnlyEventList<ILegalEntity>(new ILegalEntity[] { legalEntity });
                ILegalEntityAddress[] addresses = legalEntity.LegalEntityAddresses.Where(x=>x.AddressType.Key == (int)SAHL.Common.Globals.AddressTypes.Postal).ToArray();
                foreach (ILegalEntityAddress legalEntityAddress in addresses)
                {
                    _lstMailingAddresses.Add(_view.Messages, legalEntityAddress.Address);
                }
            }

            // get the attorney roles if required
            if (_view.DisplayAttorneyRole && _application != null)
            {
                SAHL.Common.Globals.OfferRoleTypes[] offerRoleTypes = new SAHL.Common.Globals.OfferRoleTypes[1] { SAHL.Common.Globals.OfferRoleTypes.ConveyanceAttorney };
                _lstLegalEntities = _application.GetLegalEntitiesByRoleType(offerRoleTypes, GeneralStatusKey.Active);
            }

            // get the debt counsellors if required
            if (_view.DisplayDebtCounsellors)
            {
                IEventList<ILegalEntity> debtCounsellors = _debtCounsellingRepo.GetAllDebtCounsellorsForDebtCounselling(_genericKey);

                _lstLegalEntities = new ReadOnlyEventList<ILegalEntity>(debtCounsellors);

                if (_view.DisplayClientsAndNCR)
                {
                    IOrganisationStructureRepository _organisationStructureRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                    IDebtCounselling debtCounselling = _debtCounsellingRepo.GetDebtCounsellingByKey(_genericKey);
                    IList<ILegalEntity> tmpLegalEntitiesList = new List<ILegalEntity>(debtCounselling.Clients);

                    foreach (ILegalEntity debtCounsellor in _lstLegalEntities)
                    {
                        tmpLegalEntitiesList.Add(debtCounsellor);
                    }

                    // get list of NCRs
                    IOrganisationStructure osNCR = _organisationStructureRepo.GetRootOrganisationStructureForDescription("National Credit Regulator");
                    if (osNCR != null)
                    {
                        IEventList<ILegalEntity> ncrs = _organisationStructureRepo.GetLegalEntitiesForOrganisationStructureKey(osNCR.Key, true);

                        foreach (ILegalEntity ncr in ncrs)
                        {
                            bool includeRecipient = true;

                            // filter out the top level NCR
                            if (ncr.LegalEntityType.Key == (int)SAHL.Common.Globals.LegalEntityTypes.Company)
                            {
                                ILegalEntityCompany lec = ncr as ILegalEntityCompany;
                                if (String.Compare(lec.RegisteredName, "National Credit Regulator", true) == 0)
                                    includeRecipient = false;
                            }
                            if (includeRecipient)
                                tmpLegalEntitiesList.Add(ncr);
                        }
                    }

                    _lstLegalEntities = new ReadOnlyEventList<ILegalEntity>(tmpLegalEntitiesList);
                }
            }

            if (_view.DisplayAttorneyRole)
            {
                if (_genericKeyTypeKey == (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM)
                {
                    IDebtCounselling debtCounselling = _debtCounsellingRepo.GetDebtCounsellingByKey(_genericKey);
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                    IRuleService svc = ServiceFactory.GetService<IRuleService>();
                    svc.ExecuteRule(spc.DomainMessages, "DebtCounsellingHasLitigationAttorney", debtCounselling);
                    if (_view.IsValid)
                    {
                        IAttorney litigationAttorney = debtCounselling.LitigationAttorney;
                        _lstLegalEntities = new ReadOnlyEventList<ILegalEntity>(litigationAttorney.GetContacts(ExternalRoleTypes.DebtCounselling, GeneralStatuses.Active));
                    }
                    else
                        _lstLegalEntities = new ReadOnlyEventList<ILegalEntity> { };
                }
            }

            // Get the ReportData using the CorrespondenceStrategyWorker
            _reportDataList = CorrespondenceStrategyWorker.GetReportData(_view.ViewName, _originationSourceProductKey);

            bool addressParameterRequired = false;

            // Setup header
            string sHeaderText = "";
            int i = 0;
            foreach (ReportData rd in _reportDataList)
            {
                if (i == 0)
                    sHeaderText = rd.ReportName;
                else
                    sHeaderText += " , " + rd.ReportName;

                if (!String.IsNullOrEmpty(rd.AddressParameterName))
                    addressParameterRequired = true;

                i++;
            }

            _view.CorrespondenceDocuments = sHeaderText;
            _view.AddressParameterRequired = addressParameterRequired;

            if (addressParameterRequired || _view.DisplayAttorneyRole)
                _view.ShowMailingAddress = false;
            else
                _view.ShowMailingAddress = true;

            _view.AllowPreview = _reportDataList[0].AllowPreview;

            // set this here for use in the grid correspondence control
            if (_lstMailingAddresses != null && _lstMailingAddresses.Count > 0)
                _view.AccountMailingAddress = _lstMailingAddresses[0];

            // Sets up the allowed Correspondence Mediums
            _view.SetupAllowedCorrespondenceMediums(_reportDataList);

            // Bind Recipients Grid
            _view.BindRecipientData(_genericKey, _genericKeyTypeKey, _lstLegalEntities);

            if (_lstLegalEntities.Count > 0)
            {
                // Bind Correspondence Data (telephone,fax, email etc)
                _view.BindCorrespondenceMediumData(_lstLegalEntities[0]);

                // Bind Legal Entity Address Data
                _view.BindAddressData(_lstLegalEntities[0].LegalEntityAddresses, _lstMailingAddresses);
            }

            // if we have come here via a menu click then clear out the cache
            // otherwise if we have navigated back from the preview screen
            // we want to re-populate the parameter controls using the values in the cache
            //ReportData cachedReportData = null;
            List<ReportData> cachedReportDataList = null;

            if (_view.IsMenuPostBack)
                ClearGlobalCacheObjects();
            else
            {
                PopulateCorrespondenceOptionsFromCache();
                if (GlobalCacheData.ContainsKey(ViewConstants.CorrespondenceReportDataList))
                {
                    cachedReportDataList = GlobalCacheData[ViewConstants.CorrespondenceReportDataList] as List<ReportData>;

                    //cachedReportData = cachedReportDataList[0];
                }
            }

            // Sets up the controls for any extra report parameters
            // pass in the cached values so we can re-populate the controls after preview
            _view.SetupExtraCorrespondenceParameters(_reportDataList, cachedReportDataList);
        }

        private void PopulateCorrespondenceOptionsFromCache()
        {
            // correspondence medium info
            if (GlobalCacheData.ContainsKey(ViewConstants.CorrespondenceMediumInfo))
                _view.SelectedCorrespondenceMediumInfo.AddRange(GlobalCacheData[ViewConstants.CorrespondenceMediumInfo] as List<CorrespondenceMediumInfo>);
        }

        void OnRecipientsGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            // Get the selected Legal Entity
            ILegalEntity le = _legalEntityRepo.GetLegalEntityByKey(Convert.ToInt32(e.Key));

            // Bind Correspondence Data (telephone,fax, email etc)
            _view.BindCorrespondenceMediumData(le);

            // Bind Legal Entity Address Data
            _view.BindAddressData(le.LegalEntityAddresses, _lstMailingAddresses);
        }

        void OnAddressAddButtonClicked(object sender, KeyChangedEventArgs e)
        {
            if (ValidateAddressInput() == false)
                return;
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, LifeTimes);

            // populate the 'NavigateTo' global cache variable so the next view knows where to come back to
            GlobalCacheData.Remove(ViewConstants.NavigateTo);
            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, LifeTimes);

            _view.Navigator.Navigate("CorrespondenceAddressAdd");
        }

        protected virtual void OnCancelButtonClicked(object sender, EventArgs e)
        {
            ClearGlobalCacheObjects();

            _view.Navigator.Navigate("Cancel");
        }

        protected virtual void OnSendButtonClicked(object sender, EventArgs e)
        {
            // perform screen validation
            if (ValidateScreenInput(false) == false)
                return;

            // run the warning rule to see if correspondence has already been sent.
            List<int> reportStatementKeys = new List<int>();
            foreach (ReportData rd in _reportDataList)
            {
                reportStatementKeys.Add(rd.ReportStatementKey);
            }

            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            int rulePassed = svc.ExecuteRule(_view.Messages, "CorrespondenceAlreadySent", reportStatementKeys, _genericKey, _genericKeyTypeKey);
            if (rulePassed == 0)
                return;

            // before running reports, and if required, update the loan conditions
            if (_reportDataList[0].UpdateConditions)
                UpdateLoanConditions(_genericKey, _genericKeyTypeKey);

            // if no rule failures then process the reports
            ProcessCorrespondenceReports();

            // clear out all our global cache objects
            ClearGlobalCacheObjects();
        }

        protected void OnPreviewButtonClicked(object sender, EventArgs e)
        {
            // perform screen validation
            if (ValidateScreenInput(true) == false)
                return;

            PreviewCorrespondenceReports();
        }

        private void PreviewCorrespondenceReports()
        {
            ClearCorrespondenceParametersCache();

            // add the selected correspondence mediums to the global cache
            GlobalCacheData.Add(ViewConstants.CorrespondenceMediumInfo, _view.SelectedCorrespondenceMediumInfo, LifeTimes);

            
            // Populate the Report Parameters

            foreach (ReportData rd in _reportDataList)
            {
                // set the parameter values
                SetReportParameterValues(rd, SAHL.Common.Globals.CorrespondenceMediums.Email, _view.SelectedCorrespondenceMediumInfo[0].LegalEntityKey, _view.SelectedCorrespondenceMediumInfo[0].AddressKey);
            }

            // add the reportdata list to the global cache
            GlobalCacheData.Remove(ViewConstants.CorrespondenceReportDataList);
            GlobalCacheData.Add(ViewConstants.CorrespondenceReportDataList, _reportDataList, LifeTimes);

            GlobalCacheData.Remove(ViewConstants.CorrespondenceNavigateTo);
            GlobalCacheData.Add(ViewConstants.CorrespondenceNavigateTo, _view.ViewName, LifeTimes);

            // navigate to the preview page
            _view.Navigator.Navigate("CorrespondencePreview");
        }

        protected virtual void PopulateExternalRoleTypeForCorrespondenceMediumInfo(CorrespondenceMediumInfo correspondenceMediumInfo)
        {
            //_reportDataList;
            try
            {
                int externalRoleTypeKey = _debtCounsellingRepo.GetExternalRoleTypeKeyForDebtCounsellingKeyAndLegalEntityKey(_genericKey, correspondenceMediumInfo.LegalEntityKey);
                correspondenceMediumInfo.ExternalRoleType = externalRoleTypeKey;//(int)SAHL.Common.Globals.ExternalRoleTypes.Client;
            }
            catch (ArgumentException ae)
            {
                throw;
            }
        }

        protected virtual List<CorrespondenceMediumInfo> GenerateNewInfoList(List<int> uniqueLegalEntityKeys)
        {
            List<CorrespondenceMediumInfo> infos = new List<CorrespondenceMediumInfo>();
            if (_view.CCDebtCounsellor)
            {
                /* for the first DC entrey overrite the LEKey with the first in the collection
                 * After create NEW DC entries with the next LEKey in the list
                 * 
                 */

                foreach (CorrespondenceMediumInfo correspondenceMediumInfo in _view.SelectedCorrespondenceMediumInfo)
                {
                    // 0 is the NCR people
                    if ((correspondenceMediumInfo.ExternalRoleType == (int)SAHL.Common.Globals.ExternalRoleTypes.DebtCounsellor && correspondenceMediumInfo.CorrespondenceMediumsSelected.Count > 0) ||
                        (correspondenceMediumInfo.ExternalRoleType == 0 && correspondenceMediumInfo.CorrespondenceMediumsSelected.Count > 0))
                    {
                        // 1st time we want to replace the legalentiykey with the first (if more than one) client LEKey
                        // After create NEW DC records with the NEXT LEKey and so on.

                        //for each set of clients sharing a domicilium we only want to send the email once for the DC or NCR
                        //create a new list, cause that seems to be the correspondence way ...
                        List<int> processedDomiciliumAddressKeys = new List<int>();

                        for (int i = 0; i < uniqueLegalEntityKeys.Count; i++)
                        {
                            bool process = true;
                            if (_view.EmailSingleCopyForSharedDomiciliums == true)
                            {
                                //get the domicilium address, add it to the list and dont process it again

                                var le = _legalEntityRepo.GetLegalEntityByKey(uniqueLegalEntityKeys[i]);
                                if (le != null && le.ActiveDomiciliumAddress != null && le.ActiveDomiciliumAddress.Address != null)
                                {
                                    if (processedDomiciliumAddressKeys.Contains(le.ActiveDomiciliumAddress.Address.Key))
                                        process = false;
                                    else
                                        processedDomiciliumAddressKeys.Add(le.ActiveDomiciliumAddress.Address.Key);
                                }
                            }

                            if (process)
                            {
                                if (i == 0)
                                {
                                    correspondenceMediumInfo.LegalEntityKey = uniqueLegalEntityKeys[i];
                                    infos.Add(correspondenceMediumInfo);
                                }
                                else
                                {
                                    CorrespondenceMediumInfo newInfo = new CorrespondenceMediumInfo();
                                    newInfo.AddressKey = correspondenceMediumInfo.AddressKey;
                                    newInfo.CellPhoneNumber = correspondenceMediumInfo.CellPhoneNumber;
                                    newInfo.CorrespondenceMediumsSelected.AddRange(correspondenceMediumInfo.CorrespondenceMediumsSelected);
                                    newInfo.EmailAddress = correspondenceMediumInfo.EmailAddress;
                                    newInfo.ExternalRoleType = correspondenceMediumInfo.ExternalRoleType;
                                    newInfo.FaxCode = correspondenceMediumInfo.FaxCode;
                                    newInfo.FaxNumber = correspondenceMediumInfo.FaxNumber;
                                    newInfo.LegalEntityKey = uniqueLegalEntityKeys[i];
                                    infos.Add(newInfo);
                                }
                            }
                        }
                    }
                    else
                    {
                        // still add it to the infos collection just dont modify it cause its not a DC.
                        infos.Add(correspondenceMediumInfo);
                    }
                }
                return infos;
            }
            else
            {
                return _view.SelectedCorrespondenceMediumInfo;
            }
        }

        protected virtual List<int> RemoveDuplicateAddressesAndGenerateUniqueAddressList()
        {
            List<int> uniqueAddressKeys = new List<int>();
            List<int> addressKeys = new List<int>();
            foreach (CorrespondenceMediumInfo correspondenceMediumInfo in _view.SelectedCorrespondenceMediumInfo)
            {
                PopulateExternalRoleTypeForCorrespondenceMediumInfo(correspondenceMediumInfo);
                // we are only worried about "post" options

                // get the legal entity
                ILegalEntity legalentity = _legalEntityRepo.GetLegalEntityByKey(correspondenceMediumInfo.LegalEntityKey);
                if (correspondenceMediumInfo.CorrespondenceMediumsSelected.Contains(SAHL.Common.Globals.CorrespondenceMediums.Post))
                {


                    // check if they have a domicilium address
                    if (legalentity.ActiveDomiciliumAddress != null)
                    {
                        // if the address key is in the list then remove the 'post' option from the CorrespondenceMediumsSelected
                        if (addressKeys.Contains(legalentity.ActiveDomiciliumAddress.Address.Key))
                            correspondenceMediumInfo.CorrespondenceMediumsSelected.Remove(SAHL.Common.Globals.CorrespondenceMediums.Post);
                        else // if its not in the list then add the address key
                        {
                            if (correspondenceMediumInfo.ExternalRoleType == (int)SAHL.Common.Globals.ExternalRoleTypes.Client)
                            {
                                addressKeys.Add(legalentity.ActiveDomiciliumAddress.Address.Key);
                                uniqueAddressKeys.Add(legalentity.ActiveDomiciliumAddress.Address.Key);
                            }
                        }
                    }
                }
                else
                {
                    if (correspondenceMediumInfo.ExternalRoleType == (int)SAHL.Common.Globals.ExternalRoleTypes.Client)
                    {
                        if (!addressKeys.Contains(legalentity.ActiveDomiciliumAddress.Address.Key))
                            uniqueAddressKeys.Add(legalentity.ActiveDomiciliumAddress.Address.Key);
                    }
                }
            }
            return uniqueAddressKeys;
        }

        protected virtual List<int> GenerateUniqueLegalEntityList()
        {
            List<int> uniqueLegalEntityKeys = new List<int>();
            foreach (CorrespondenceMediumInfo correspondenceMediumInfo in _view.SelectedCorrespondenceMediumInfo)
            {
                if (correspondenceMediumInfo.CorrespondenceMediumsSelected.Count > 0)// we chose to send to this person somehow
                {
                    if (correspondenceMediumInfo.ExternalRoleType == 1)// the actual clients
                    {
                        if (!uniqueLegalEntityKeys.Contains(correspondenceMediumInfo.LegalEntityKey))
                            uniqueLegalEntityKeys.Add(correspondenceMediumInfo.LegalEntityKey);
                    }
                }
            }
            return uniqueLegalEntityKeys;
        }

        private void ProcessCorrespondenceReports()
        {
            // All documents that are emailed or faxed will be processed straight away
            // Documents that are printed will either be processed no or queued for batch printing depending on their setup

            //Dictionary<ICorrespondence, byte[]> dicCorrespondence = new Dictionary<ICorrespondence, byte[]>();
            string renderedMessage = "";

            List<int> uniqueAddressKeys = null;
            List<int> uniqueLegalEntityKeys = null;
            if (_view.PostSingleCopyForSharedDomiciliums==true)
            {
                uniqueAddressKeys = RemoveDuplicateAddressesAndGenerateUniqueAddressList();
                uniqueLegalEntityKeys = GenerateUniqueLegalEntityList();
            }
            
            // Build a list of report data for each report and its parameter values
            List<CorrespondenceMediumInfo> infos = GenerateNewInfoList(uniqueLegalEntityKeys);

            foreach (CorrespondenceMediumInfo correspondenceMediumInfo in infos)
            {
                if (correspondenceMediumInfo.CorrespondenceMediumsSelected.Count > 0)
                {
                    Dictionary<CorrespondenceExt, byte[]> dicCorrespondence = new Dictionary<CorrespondenceExt, byte[]>();

                    ILegalEntity selectedLegalEntity = _legalEntityRepo.GetLegalEntityByKey(correspondenceMediumInfo.LegalEntityKey);

                    foreach (SAHL.Common.Globals.CorrespondenceMediums cm in correspondenceMediumInfo.CorrespondenceMediumsSelected)
                    {
                        #region Process Correspondence Reports

                        int correspondenceMediumKey = (int)cm;

                        // Populate the Report Parameters
                        foreach (ReportData rd in _reportDataList)
                        {
                            // create our new reportdata object for saving
                            ReportData saveReportData = new ReportData(rd.ReportName, rd.OriginationSourceProductKey, rd.CorrespondenceReportElement, rd.BatchPrint, rd.AllowPreview, rd.DataStorName, rd.UpdateConditions, rd.SendUserConfirmationEmail, rd.EmailProcessedPDFtoConsultant, rd.CorrespondenceTemplate, rd.CombineDocumentsIfEmailing);
                            foreach (ReportDataParameter parm in rd.ReportParameters)
                            {
                                ReportDataParameter saveParm = new ReportDataParameter(parm.ReportParameterKey, parm.ParameterName, parm.ParameterValue);
                                saveReportData.ReportParameters.Add(saveParm);
                            }

                            // set the parameter values
                            SetReportParameterValues(saveReportData, cm, correspondenceMediumInfo.LegalEntityKey, correspondenceMediumInfo.AddressKey);

                            IReportStatement reportStatement = _reportRepo.GetReportStatementByKey(rd.ReportStatementKey);

                            #region setup the correspondence and corresponcenceparameters(if required)

                            ICorrespondence correspondence = _correspondenceRepo.CreateEmptyCorrespondence();
                            correspondence.GenericKey = _genericKey;
                            correspondence.GenericKeyType = _lookupRepo.GenericKeyType.ObjectDictionary[Convert.ToString(_genericKeyTypeKey)];
                            correspondence.ReportStatement = reportStatement;
                            correspondence.CorrespondenceMedium = _lookupRepo.CorrespondenceMediums.ObjectDictionary[Convert.ToString(correspondenceMediumKey)];
                            switch (cm)
                            {
                                case SAHL.Common.Globals.CorrespondenceMediums.Email:
                                    correspondence.DestinationValue = correspondenceMediumInfo.EmailAddress;
                                    break;
                                case SAHL.Common.Globals.CorrespondenceMediums.Fax:
                                    correspondence.DestinationValue = correspondenceMediumInfo.FaxCode + correspondenceMediumInfo.FaxNumber.TrimStart('0');
                                    break;
                                case SAHL.Common.Globals.CorrespondenceMediums.SMS:
                                    correspondence.DestinationValue = correspondenceMediumInfo.CellPhoneNumber;
                                    break;
                                default:
                                    break;
                            }
                            correspondence.DueDate = DateTime.Now;
                            correspondence.ChangeDate = DateTime.Now;
                            correspondence.UserID = _view.CurrentPrincipal.Identity.Name;
                            correspondence.LegalEntity = selectedLegalEntity;

                            // setup correspondence parameters
                            // only insert into correspondenceparameters if :-
                            // 1. report is configured for "batch printing"
                            // 2. "print" has been selected
                            // 3. reportparameters exist in the reportparameter table
                            string dataStorParameters = "";
                            foreach (ReportDataParameter parm in saveReportData.ReportParameters)
                            {
                                // build this string of params here for use later in datastor
                                dataStorParameters += parm.ParameterName + ":" + parm.ParameterValue + " | ";

                                if (cm == SAHL.Common.Globals.CorrespondenceMediums.Post && saveReportData.BatchPrint && parm.ReportParameterKey > 0)
                                {
                                    ICorrespondenceParameters correspondenceParm = _correspondenceRepo.CreateEmptyCorrespondenceParameter();
                                    correspondenceParm.Correspondence = correspondence;
                                    correspondenceParm.ReportParameter = _reportRepo.GetReportParameterByKey(parm.ReportParameterKey);
                                    correspondenceParm.ReportParameterValue = parm.ParameterValue.ToString();

                                    // add the correspondenceparameter object to the correspondence object
                                    correspondence.CorrespondenceParameters.Add(_view.Messages, correspondenceParm);
                                }
                            }

                            // add the string of params temporarily to the correspondence record here for use later in datastor
                            // this will get overwritten when we save the correspondence record a little later
                            correspondence.OutputFile = dataStorParameters;

                            #endregion setup the correspondence and corresponcenceparameters(if required)

                            // render the report : only render if user has selected :-
                            // 1. Email
                            // 2. Fax
                            // 3. Print (and the report is not configured for batch print)
                            renderedMessage = "";
                            byte[] renderedReport = null;
                            if (cm != SAHL.Common.Globals.CorrespondenceMediums.Post || saveReportData.BatchPrint == false)
                            {
                                IDictionary<string, string> reportParameters = new Dictionary<string, string>();
                                foreach (ReportDataParameter parm in saveReportData.ReportParameters)
                                {
                                    reportParameters.Add(parm.ParameterName, parm.ParameterValue.ToString());
                                }

                                switch (reportStatement.ReportType.Key)
                                {
                                    case (int)SAHL.Common.Globals.ReportTypes.ReportingServicesReport:
                                        renderedReport = _reportRepo.RenderSQLReport(rd.StatementName, reportParameters, out renderedMessage);
                                        break;
                                    case (int)SAHL.Common.Globals.ReportTypes.StaticPDF:
                                    case (int)SAHL.Common.Globals.ReportTypes.PDFReport:
                                        string renderedPDF = String.Empty;

                                        if (reportStatement.ReportType.Key == (int)SAHL.Common.Globals.ReportTypes.StaticPDF)
                                            renderedPDF = reportStatement.StatementName;
                                        else
                                            renderedPDF = _reportRepo.GeneratePDFReport(reportStatement.Key, reportParameters, out renderedMessage);

                                        ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
                                        WindowsImpersonationContext wic = securityService.BeginImpersonation();

                                        try
                                        {
                                            // get the pdf report int a byte stream for use later
                                            FileStream fs = new FileStream(renderedPDF, FileMode.Open, FileAccess.Read);
                                            renderedReport = new byte[fs.Length];

                                            //Read block of bytes from stream into the byte array
                                            fs.Read(renderedReport, 0, System.Convert.ToInt32(fs.Length));

                                            //Close the File Stream
                                            fs.Close();
                                        }
                                        finally
                                        {
                                            // end impersonation
                                            securityService.EndImpersonation(wic);
                                        }

                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (String.IsNullOrEmpty(renderedMessage))
                            {
                                // add the Correspondence object and its rendered report to the collection
                                CorrespondenceExt correspondenceExt = new CorrespondenceExt(correspondence, saveReportData.ExcludeFromDataSTOR);
                                dicCorrespondence.Add(correspondenceExt, renderedReport);
                            }
                        }

                        #endregion Process Correspondence Reports
                    }

                    if (dicCorrespondence.Count == 0)
                    {
                        _view.Messages.Add(new Error("Error Rendering Reports - No reports processed./r/n" + renderedMessage, "Error Rendering Reports - No reports processed./r/n" + renderedMessage));
                        return;
                    }

                    // save the correspondence
                    SaveCorrespondenceReports(dicCorrespondence, selectedLegalEntity);
                }
            }
        }


        private bool SaveCorrespondenceReports(Dictionary<CorrespondenceExt, byte[]> dicCorrespondence, ILegalEntity selectedLegalEntity)
        {
            bool success = true;

            WindowsImpersonationContext wic = null;

            string msg = "";
            if (dicCorrespondence.Count <= 0)
            {
                msg = "Correspondence List Empty - No reports processed.";
                _view.Messages.Add(new Error(msg, msg));
                return false;
            }

            IDataStorRepository dataStorRepository = RepositoryFactory.GetRepository<IDataStorRepository>();

            List<string> emailAttachments = new List<string>();

            // get the STOR object
            string dataStorName = _reportDataList[0].DataStorName;
            ISTOR stor = dataStorRepository.GetSTORByName(dataStorName);

            IADUser adUser = RepositoryFactory.GetRepository<ISecurityRepository>().GetADUserByPrincipal(_view.CurrentPrincipal);
            bool sendUserConfirmationEmail = _reportDataList[0].SendUserConfirmationEmail;
            bool emailProcessedPDFtoConsultant = _reportDataList[0].EmailProcessedPDFtoConsultant;

            if (adUser.LegalEntity == null)
                throw new Exception("ADUser record for " + adUser.ADUserName + " has no LegalEntity record - ADUserKey: " + adUser.Key);
            else if (adUser.LegalEntity.EmailAddress.Trim().Length <= 0)
                throw new Exception("LegalEntity record for ADUser " + adUser.ADUserName + " has no email address - LegalEntityKey: " + adUser.LegalEntity.Key.ToString());

            string userEmailAddress = adUser.LegalEntity.EmailAddress;
            string subject = "";
            string body = "";
            string destinationDesc = "";
            string clientEmailAddress = "";
            int genericKey = 0;

            CorrespondenceTemplates correspondenceTemplate = (_reportDataList[0].CorrespondenceTemplate == null) ? CorrespondenceTemplates.EmailCorrespondenceGeneric : _reportDataList[0].CorrespondenceTemplate;

            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();

            List<string> confirmationEmailDocuments = new List<string>();

            foreach (KeyValuePair<CorrespondenceExt, byte[]> CorrespondenceKeyValuePair in dicCorrespondence)
            {
                CorrespondenceExt correspondenceExt = CorrespondenceKeyValuePair.Key;
                ICorrespondence correspondence = correspondenceExt.Correspondence;

                byte[] renderedReport = CorrespondenceKeyValuePair.Value;

                string outputFileDataSTOR = "", dataSTORDirectory = "";
                string guid = "";
                string outputFileClientEmail = "";
                string dataStageDirectory = "";
                decimal insertedDataKey = 0;
                string dataSTORlink = "";
                string dataStorParameters = correspondence.OutputFile.Trim();
                bool emailPrintedPDFtoConsultant = false;
                
                genericKey = correspondence.GenericKey;
                clientEmailAddress = correspondence.DestinationValue;

                bool batchPrint = false;
                bool writeToDataStor = String.IsNullOrEmpty(dataStorName) ? false : true;
                if (writeToDataStor && correspondenceExt.ExcludeFromDataSTOR == true)
                    writeToDataStor = false;

                if (correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Post)
                {
                    if (_reportDataList[0].BatchPrint)
                    {
                        writeToDataStor = false;
                        batchPrint = true;
                    }
                    if (emailProcessedPDFtoConsultant)
                        emailPrintedPDFtoConsultant = true;
                }

                // if this is not a batch print then update the completed date on the correspondence record
                if (batchPrint == false)
                    correspondence.CompletedDate = DateTime.Now;

                // if we are writing to data STOR then we need to store the output file path in the correspondence record
                if (writeToDataStor)
                {
                    guid = System.Guid.NewGuid().ToString("B").ToUpper();
                    dataSTORDirectory = stor.Folder + @"\" + DateTime.Today.Year.ToString() + @"\" + DateTime.Today.Month.ToString("00");
                    outputFileDataSTOR = dataSTORDirectory + @"\" + guid;
                    dataStageDirectory = _lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.CorrespondenceDataStagingFolder].ControlText;

                    correspondence.OutputFile = outputFileDataSTOR;
                }

                // insert the Correspondence record into the database
                _correspondenceRepo.SaveCorrespondence(correspondence);

                // add description of correspondence to confirmation email body
                confirmationEmailDocuments.Add(correspondence.ReportStatement.ReportName + " : " + correspondence.CorrespondenceMedium.Description + (!String.IsNullOrEmpty(correspondence.DestinationValue) ? " : " + correspondence.DestinationValue : ""));

                if (writeToDataStor)
                {
                    #region write to data STOR

                    // create an empty data object
                    IData data = dataStorRepository.CreateEmptyData();
                    string origSource = "";
                    IOriginationSourceProduct osp = _applicationRepo.GetOriginationSourceProductByKey(_originationSourceProductKey);
                    if (osp != null)
                        origSource = osp.OriginationSource.Description + " : " + osp.Product.Description;

                    // populate the data object
                    DateTime dateNow = System.DateTime.Now;
                    string now = dateNow.ToString("yyyy-MM-dd hh:mm:ss");
                    string userName = adUser.LegalEntity == null ? adUser.ADUserName : adUser.LegalEntity.GetLegalName(LegalNameFormat.FullNoSalutation) + " (" + adUser.ADUserName + ")";

                    data.ArchiveDate = now;
                    data.STOR = stor.Key;
                    data.GUID = guid;
                    data.Extension = "PDF";
                    data.MsgSubject = _loanNumber.ToString();
                    data.Key1 = _loanNumber.ToString();
                    data.Key2 = origSource;
                    data.Key3 = _genericKey.ToString();
                    data.Key4 = correspondence.ReportStatement.ReportName;
                    data.Key5 = correspondence.CorrespondenceMedium.Description;
                    data.Key6 = String.IsNullOrEmpty(correspondence.DestinationValue) ? "" : correspondence.DestinationValue;
                    data.Key7 = now;
                    data.Key8 = userName;
                    data.Key9 = !String.IsNullOrEmpty(dataStorParameters) ? dataStorParameters.Trim('|') : "";
                    data.Key10 = correspondence.ReportStatement.Key.ToString();

                    // setup the filename to use for emailing/faxing
                    // filename is made up as follows: ReportName_LoanNumber_CorrespondenceMedium_DateTime.pdf
                    string dateTime = dateNow.Year.ToString() + dateNow.Month.ToString("00") + dateNow.Day.ToString("00") + dateNow.Hour.ToString("00") + dateNow.Minute.ToString("00") + dateNow.Second.ToString("00");
                    string reportNameFormatted = data.Key4.Replace(" - ", "-"); // strip out spaces around the hypen
                    string origFileName = reportNameFormatted + "_";
                    if (_view.LegalEntityCorrespondence)
                        origFileName += SAHL.Common.Utils.StringUtils.RemoveSpecialCharacters(selectedLegalEntity.GetLegalName(LegalNameFormat.Full));
                    else
                        origFileName += _loanNumber.ToString();

                    origFileName += "_" + correspondence.CorrespondenceMedium.Description + "_" + dateTime + ".pdf";

                    data.OriginalFilename = origFileName;
                    data.Key11 = origFileName;

                    if (_view.LegalEntityCorrespondence)
                    {
                        string accountString = "";
                        var accounts = selectedLegalEntity.Roles.Where(x => x.Account.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open).Select(x => x.Account).ToList();
                        foreach (var acc in accounts)
                        {
                            if (acc.Product.Key == (int)SAHL.Common.Globals.Products.VariableLoan
                                || acc.Product.Key == (int)SAHL.Common.Globals.Products.VariFixLoan
                                || acc.Product.Key == (int)SAHL.Common.Globals.Products.DefendingDiscountRate
                                || acc.Product.Key == (int)SAHL.Common.Globals.Products.NewVariableLoan
                                || acc.Product.Key == (int)SAHL.Common.Globals.Products.Edge)
                            {
                                accountString = accountString + acc.Key + ",";
                            }                           
                        }
                        data.Key1 = accountString.Substring(0,accountString.LastIndexOf(","));

                        data.Key12 = selectedLegalEntity.GetLegalName(LegalNameFormat.Full);

                        switch (selectedLegalEntity.LegalEntityType.Key)
                        {
                            case (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson:
                                data.Key13 = ((ILegalEntityNaturalPerson)selectedLegalEntity).IDNumber;
                                break;
                            case (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation:
                                data.Key13 = ((ILegalEntityCloseCorporation)selectedLegalEntity).RegistrationNumber;
                                break;
                            case (int)SAHL.Common.Globals.LegalEntityTypes.Company:
                                data.Key13 = ((ILegalEntityCompany)selectedLegalEntity).RegistrationNumber;
                                break;
                            case (int)SAHL.Common.Globals.LegalEntityTypes.Trust:
                                data.Key13 = ((ILegalEntityTrust)selectedLegalEntity).RegistrationNumber;
                                break;
                            case (int)SAHL.Common.Globals.LegalEntityTypes.Unknown:
                                data.Key13 = "";
                                break;
                            default:
                                break;
                        }
                    }

                    outputFileClientEmail = dataStageDirectory + @"\" + data.OriginalFilename;

                    // save the data
                    dataStorRepository.SaveData(data);

                    // get the key of the inserted data record
                    insertedDataKey = data.Key;

                    // save the rendered pdf - note that we need to impersonate a user with local permissions and rights
                    // to the remote folder otherwise we get authentication issues thanks to Kerberos vs. the SAHL network setup
                    wic = securityService.BeginImpersonation();

                    try
                    {
                        // Check if our output directory exists - if not then create
                        if (!Directory.Exists(dataSTORDirectory))
                            Directory.CreateDirectory(dataSTORDirectory);

                        // save to the report to the dataSTOR server path
                        FileStream fs = new FileStream(outputFileDataSTOR, FileMode.Create);
                        fs.Write(renderedReport, 0, renderedReport.Length);
                        fs.Close();
                        fs.Dispose();

                        if (correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Email
                        || correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Fax
                        || emailPrintedPDFtoConsultant == true)
                        {
                            // create a copy of the document in the correspondence data staging directory
                            // from where it will be emailed/faxed
                            // we r saving a copy of the doc to print here aswell so we can email it to comsultant
                            if (!String.IsNullOrEmpty(dataStageDirectory))
                            {
                                FileStream fs2 = new FileStream(outputFileClientEmail, FileMode.Create);
                                fs2.Write(renderedReport, 0, renderedReport.Length);
                                fs2.Close();
                                fs2.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        // end impersonation
                        securityService.EndImpersonation(wic);
                    }

                    #endregion write to data STOR
                }
                else
                {
                    // If its a static PDF and not written to DataSTOR, use the StatementName as the Path of the doc
                    ICorrespondence corresp = CorrespondenceKeyValuePair.Key.Correspondence;
                    IReportStatement reportStatement = corresp.ReportStatement;
                    if (reportStatement.ReportType.Key == (int)ReportTypes.StaticPDF)
                    {
                        outputFileClientEmail = reportStatement.StatementName;
                    }
                }

                if (batchPrint == false && !String.IsNullOrEmpty(outputFileClientEmail))
                {
                    if (writeToDataStor)
                    {
                        #region create a dataSTOR link to email the user

                        try
                        {
                            // activate impersonation so we can create the file on the server
                            wic = securityService.BeginImpersonation();

                            dataSTORlink = outputFileClientEmail.Replace(".pdf", ".STOR");

                            StreamWriter sw = new StreamWriter(dataSTORlink);
                            sw.WriteLine("[General]");
                            sw.WriteLine("Requested By=" + _view.CurrentPrincipal.Identity.Name);
                            sw.WriteLine("Requested Date=" + System.DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            sw.WriteLine("[STOR]");
                            sw.WriteLine("STORid=" + stor.Key.ToString());
                            sw.WriteLine("STORname=" + stor.Name);
                            sw.WriteLine("Total GUIDs=2");
                            sw.WriteLine("GUID001=ID" + insertedDataKey.ToString());
                            sw.WriteLine("GUID002=");
                            sw.Close();
                            sw.Dispose();
                        }
                        finally
                        {
                            // cancel impersonation
                            securityService.EndImpersonation(wic);
                        }

                        #endregion create a dataSTOR link to email the user
                    }

                    #region send correspondence to client

                    switch (correspondence.CorrespondenceMedium.Key)
                    {
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Email:
                            destinationDesc = "Email Address       : ";

                            //set the subject body

                            _correspondenceRepo.GetEmailTemplate(selectedLegalEntity, adUser.LegalEntity.GetLegalName(LegalNameFormat.Full), userEmailAddress, _loanNumber, correspondenceTemplate, out subject, out body);

                            // send the email to the Client using the attachement created above
                            if (_reportDataList[0].CombineDocumentsIfEmailing)
                            {
                                emailAttachments.Add(outputFileClientEmail);
                            }
                            else
                            {
                                messageService.SendEmailExternal(correspondence.GenericKey
                                   , userEmailAddress
                                   , correspondence.DestinationValue
                                   , ""
                                   , ""
                                   , subject
                                   , body
                                   , outputFileClientEmail
                                   , ""
                                   , "");
                            }
                            break;
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Fax:

                            // send the fax to the Client using the attachement created above
                            destinationDesc = "Fax Number          : ";
                            messageService.SendFax(correspondence.GenericKey
                               , userEmailAddress
                               , correspondence.DestinationValue
                               , outputFileClientEmail);

                            break;

                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Post:
                            destinationDesc = "";
                            break;
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.SMS:
                            destinationDesc = "Cell Number          : ";
                            break;
                        default:
                            break;
                    }

                    #endregion send correspondence to client

                    #region send confirmation email to user

                    if (sendUserConfirmationEmail && !String.IsNullOrEmpty(userEmailAddress))
                    {
                        // send confirmation email to user
                        subject = "HALO Correspondence : "
                            + (correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Post ? "Printing Required" : "Confirmation")
                            + " : "
                            + correspondence.ReportStatement.ReportName
                            + " : "
                            + "Loan Number " + _loanNumber.ToString();

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("The following correspondence has been sent from HALO");
                        sb.AppendLine("----------------------------------------------------");
                        sb.AppendLine("Document Name       : " + correspondence.ReportStatement.ReportName);
                        sb.AppendLine("Medium              : " + correspondence.CorrespondenceMedium.Description);
                        if (!String.IsNullOrEmpty(destinationDesc))
                            sb.AppendLine(destinationDesc + correspondence.DestinationValue);
                        if (writeToDataStor)
                        {
                            sb.AppendLine("Written to DataSTOR : " + (writeToDataStor == true ? "Yes" : "No"));
                            sb.AppendLine("");
                            if (correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Post)
                            {
                                sb.AppendLine("Clicking the attached link will enable you to Print the document from DataSTOR.");
                            }
                            else
                                sb.AppendLine("Clicking the attached link will enable you to View the document in DataSTOR.");

                            if (emailPrintedPDFtoConsultant)
                            {
                                sb.AppendLine("");
                                sb.AppendLine("Alternatively you can Print the document from the PDF attached to this email.");
                            }
                        }

                        IList<string> attachements = new List<string>();

                        // attach the datastor link
                        attachements.Add(dataSTORlink);

                        // attach the processed pdf to the email if required
                        if (emailPrintedPDFtoConsultant)
                            attachements.Add(outputFileClientEmail);

                        messageService.SendEmailInternal("halo@sahomeloans.com", userEmailAddress, "", "", subject, sb.ToString(), false, attachements);
                    }

                    #endregion send confirmation email to user
                }
            }

            if (_reportDataList[0].CombineDocumentsIfEmailing && emailAttachments.Count > 0)
            {
                messageService.SendEmailExternal(genericKey
                                   , userEmailAddress
                                   , clientEmailAddress
                                   , ""
                                   , ""
                                   , subject
                                   , body
                                   , emailAttachments.ToArray()
                                    );
            }

            return success;
        }

        private void SetReportParameterValues(ReportData reportData, SAHL.Common.Globals.CorrespondenceMediums correspondenceMedium, int legalEntityKey, int addressKey)
        {
            foreach (ReportDataParameter parm in reportData.ReportParameters)
            {
                if (parm.ParameterName.ToLower() == reportData.GenericKeyParameterName.ToLower())
                {
                    if (_useParentAccountKeyParameter && _parentAccountKey > -1)
                        parm.ParameterValue = _parentAccountKey.ToString();
                    else if (_useApplicationAccountKeyParameter && _applicationAccountKey > -1)
                        parm.ParameterValue = _applicationAccountKey.ToString();
                    else if (_userDebtCounsellingAccountKey && _account.Key > -1)
                        parm.ParameterValue = _account.Key.ToString();
                    else
                        parm.ParameterValue = _genericKey.ToString();
                }
                else if (parm.ParameterName.ToLower() == reportData.MailingTypeParameterName.ToLower())
                    parm.ParameterValue = Convert.ToString((int)correspondenceMedium);
                else if (parm.ParameterName.ToLower() == reportData.LegalEntityParameterName.ToLower())
                    parm.ParameterValue = legalEntityKey.ToString();
                else if (parm.ParameterName.ToLower() == reportData.AddressParameterName.ToLower())
                    parm.ParameterValue = addressKey.ToString();
                else if (parm.ParameterName.ToLower() == reportData.LanguageKeyParameterName.ToLower())
                    parm.ParameterValue = _view.DocumentLanguageKey.ToString();
                else
                {
                    // this param is an extra param so go look for it in the extra params collection to get its value
                    foreach (CorrespondenceExtraParameter extraParm in _view.ExtraCorrespondenceParameters)
                    {
                        if (parm.ParameterName.ToLower() == extraParm.ReportParameter.ParameterName.ToLower())
                        {
                            parm.ParameterValue = extraParm.ParameterValue;
                            break;
                        }
                    }
                }
            }
        }

        protected void ClearGlobalCacheObjects()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.CorrespondenceReportDataList))
                GlobalCacheData.Remove(ViewConstants.CorrespondenceReportDataList);

            if (GlobalCacheData.ContainsKey(ViewConstants.CorrespondenceNavigateTo))
                GlobalCacheData.Remove(ViewConstants.CorrespondenceNavigateTo);

            if (GlobalCacheData.ContainsKey(ViewConstants.CorrespondenceMessage))
                GlobalCacheData.Remove(ViewConstants.CorrespondenceMessage);

            ClearCorrespondenceParametersCache();
        }

        private void ClearCorrespondenceParametersCache()
        {
            // clear correspondence mediums global cache
            GlobalCacheData.Remove(ViewConstants.CorrespondenceMediumInfo);
        }

        private bool ValidateAddressInput()
        {
            bool valid = true;
            string errorMessage = "";

            // if we are displaying attorneys then validate that an attorney has been selected
            if (_view.DisplayAttorneyRole && _view.SelectedCorrespondenceMediumInfo[0].LegalEntityKey <= 0)
            {
                errorMessage = "Must select an Attorney before continuing";
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                valid = false;
            }

            return valid;
        }

        private bool ValidateScreenInput(bool previewMode)
        {
            bool valid = true;
            string errorMessage = "";

            // if we have an addressparameter then enforce address selection
            if (!String.IsNullOrEmpty(_reportDataList[0].AddressParameterName))
            {
                bool addressSelected = true;
                foreach (CorrespondenceMediumInfo correspondenceMediumInfo in _view.SelectedCorrespondenceMediumInfo)
                {
                    if (correspondenceMediumInfo.AddressKey <= 0)
                    {
                        addressSelected = false;
                        break;
                    }
                }
                if (addressSelected == false)
                {
                    errorMessage = "Report requires an Address to be selected for each recipient. If none exist, please add before continuing.";
                    _view.Messages.Add(new Error(errorMessage, errorMessage));
                    valid = false;
                }
            }

            // only validate correspondence mediums if we are not previewing
            if (previewMode == false)
            {
                // if we are displaying attorneys then validate that an attorney has been selected
                if (_view.DisplayAttorneyRole && _view.SelectedCorrespondenceMediumInfo[0].LegalEntityKey <= 0)
                {
                    errorMessage = "Must select an Attorney before continuing";
                    _view.Messages.Add(new Error(errorMessage, errorMessage));
                    valid = false;
                }

                // validate correspondence options
                bool noOptionSelected = true;
                foreach (var cmi in _view.SelectedCorrespondenceMediumInfo)
                {
                    if (cmi.CorrespondenceMediumsSelected.Count > 0)
                    {
                        noOptionSelected = false;
                        break;
                    }
                }
                if (noOptionSelected)
                {
                    errorMessage = "Must select at least one Correspondence Option";
                    _view.Messages.Add(new Error(errorMessage, errorMessage));
                    valid = false;
                }

                if (valid)
                {
                    bool emailError = false, faxError = false, smsError = false, postError = false;
                    Regex regxEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

                    foreach (CorrespondenceMediumInfo correspondenceMediumInfo in _view.SelectedCorrespondenceMediumInfo)
                    {
                        foreach (SAHL.Common.Globals.CorrespondenceMediums cm in correspondenceMediumInfo.CorrespondenceMediumsSelected)
                        {
                            switch (cm)
                            {
                                case SAHL.Common.Globals.CorrespondenceMediums.Email:
                                    if (!emailError && (String.IsNullOrEmpty(correspondenceMediumInfo.EmailAddress) || !regxEmail.IsMatch(correspondenceMediumInfo.EmailAddress)))
                                        emailError = true;
                                    break;
                                case SAHL.Common.Globals.CorrespondenceMediums.Fax:
                                    if (!faxError && (String.IsNullOrEmpty(correspondenceMediumInfo.FaxCode) || String.IsNullOrEmpty(correspondenceMediumInfo.FaxNumber)))
                                        faxError = true;
                                    break;
                                case SAHL.Common.Globals.CorrespondenceMediums.Post:
                                    if (!postError && correspondenceMediumInfo.AddressKey <= 0)
                                        postError = true;
                                    break;
                                case SAHL.Common.Globals.CorrespondenceMediums.SMS:
                                    if (!smsError && String.IsNullOrEmpty(correspondenceMediumInfo.CellPhoneNumber))
                                        smsError = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    if (emailError)
                    {
                        errorMessage = "Must enter a valid Email Address";
                        if (_view.MultipleRecipientMode)
                            errorMessage += " for each Recipient selected";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                    if (faxError)
                    {
                        errorMessage = "Must enter a vaid Fax Code & Number";
                        if (_view.MultipleRecipientMode)
                            errorMessage += " for each Recipient selected";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                    if (smsError)
                    {
                        errorMessage = "Must enter a vaid Cellphone Number";
                        if (_view.MultipleRecipientMode)
                            errorMessage += " for each Recipient selected";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                    if (postError && _view.AddressParameterRequired)
                    {
                        if (_view.MultipleRecipientMode)
                            errorMessage = "Must select an Address for each recipient. If none exist, please add before continuing.";
                        else
                            errorMessage = "Must select a Recipient Address. If none exist, please add before continuing.";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                }
            }

            // validate extra parameters
            if (_view.ExtraCorrespondenceParameters.Count > 0)
            {
                foreach (CorrespondenceExtraParameter extraParm in _view.ExtraCorrespondenceParameters)
                {
                    if (extraParm.ValidInput == false)
                    {
                        errorMessage = extraParm.ReportParameter.DisplayName + " is Required";
                        _view.Messages.Add(new Error(errorMessage, errorMessage));
                        valid = false;
                    }
                }
            }

            return valid;
        }

        private static void UpdateLoanConditions(int genericKey, int genericKeyType)
        {
            IConditionsRepository conditionsRepository = RepositoryFactory.GetRepository<IConditionsRepository>();

            conditionsRepository.UpdateLoanConditions(genericKey, genericKeyType);
        }
    }
}