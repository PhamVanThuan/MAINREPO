using System;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using System.Collections;
using System.Web.UI.MobileControls;
using SAHL.Common.Service.Interfaces;
using SAHL.Web.Views.Administration.Interfaces;

namespace SAHL.Web.Views.Administration.Presenters.EstateAgentLegalEntity
{
    /// <summary>
    /// EstateAgentLegalEntity Base
    /// </summary>
    public class EstateAgentLegalEntityBase : SAHLCommonBasePresenter<IExternalOrganisationStructureLegalEntity>
    {
        #region Constants
        protected const string DEFAULTDROPDOWNITEM = "-select-";
        private const string AddressType = "AddressTypeKey";
        #endregion

        protected bool _leKeySet;
        protected Int32 _leKey;
        protected ILegalEntity _legalEntity;
        protected bool _osKeySet;
        protected int _osKey;
        protected bool _osParentKeySet;
        protected int _osKeyParent;
        private List<ICacheObjectLifeTime> _lifeTimes;
        IDictionary<string, string> _letypes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public EstateAgentLegalEntityBase(IExternalOrganisationStructureLegalEntity view, SAHLCommonBaseController controller)
            : base(view, controller)
        { }

        #region Page Life Cycle Events

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            if (GlobalCacheData.ContainsKey(ViewConstants.ParentOrganisationStructureKey))
            {
                Int32.TryParse(GlobalCacheData[ViewConstants.ParentOrganisationStructureKey].ToString(), out _osKeyParent);
                _osParentKeySet = true;
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.OrganisationStructureKey))
            {
                Int32.TryParse(GlobalCacheData[ViewConstants.OrganisationStructureKey].ToString(), out _osKey);
                _osKeySet = true;
            }

            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntityKey))
            {
                Int32.TryParse(GlobalCacheData[ViewConstants.LegalEntityKey].ToString(), out _leKey);
                _leKeySet = true;
            }

            _view.OnReBindLegalEntity += new KeyChangedEventHandler(ReBindLegalEntity);
            _view.OnAddressTypeSelectedIndexChanged += new KeyChangedEventHandler(_view_OnAddressTypeSelectedIndexChanged);
            _view.OnAddressFormatSelectedIndexChanged += new KeyChangedEventHandler(_view_OnAddressFormatSelectedIndexChanged);
            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);

            _view.AddressCaptureEnabled = true;
        }

        #endregion

        #region Events

        protected virtual void ReBindLegalEntity(object sender, KeyChangedEventArgs e)
        {
            _legalEntity = LERepo.GetLegalEntityByKey(Convert.ToInt32(e.Key));

            // Persist the LegalEntityKey in the Global cache (and call the next presenter)
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.LegalEntityKey);

            GlobalCacheData.Add(ViewConstants.LegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());
            _view.BindLegalEntity(_legalEntity, string.Empty, string.Empty);
            //IsLegalEntityEditable();
        }

        protected void _view_OnAddressTypeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (_view.GetAddressTypeSelectedValue != Convert.ToInt32(GlobalCacheData[AddressType]))
            {
                GlobalCacheData[AddressType] = _view.GetAddressTypeSelectedValue;
                if (_view.GetAddressTypeSelectedValue == (int)AddressTypes.Postal)
                    _view.GetAddressFormatSelectedValue = (int)AddressFormats.Box;
                else
                    _view.GetAddressFormatSelectedValue = (int)AddressFormats.Street;
            }
        }

        protected void _view_OnAddressFormatSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {

        }

        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }

        #endregion

        #region Methods

        protected bool InputValidation()
        {
            string msg = String.Empty;

            if (_view.SelectedLegalEntityType == -1)
            {
                msg = "Type is a required item";
                _view.Messages.Add(new Error(msg, msg));
                return false;
            }
            else if (String.IsNullOrEmpty(_view.OrganisationStructureDescription)
                || _view.OrganisationStructureDescription == DEFAULTDROPDOWNITEM
                || _view.OrganisationType == null)
            {

                if (_view.SelectedLegalEntityType == (int)LegalEntityTypes.NaturalPerson)
                {
                    msg = "Role is a required item";
                    _view.Messages.Add(new Error(msg, msg));
                }
                else
                {
                    if (_view.OrganisationType == null)
                    {
                        msg = "Organisation Type is required.";
                        _view.Messages.Add(new Error(msg, msg));
                    }

                    if (String.IsNullOrEmpty(_view.OrganisationStructureDescription))
                    {
                        msg = "Company Name is required.";
                        _view.Messages.Add(new Error(msg, msg));
                    }
                }

                return false;
            }
            return true;
        }

        protected void CaptureAddress(ILegalEntity le, bool isUpdate)
        {
            IAddressType addressType = null;
            if (_view.GetAddressTypeSelectedValue != -1 && _view.GetAddressFormatSelectedValue != -1)
                addressType = AddressRepo.GetAddressTypeByKey(_view.GetAddressTypeSelectedValue);

            _view.GetSetAddressFormat = (AddressFormats)_view.GetAddressFormatSelectedValue;

            IAddress address = _view.GetCapturedAddress;
            address.ValidateEntity();

            if (address != null)
            {
                if (!isUpdate)
                    LERepo.SaveAddress(addressType, le, address, _view.GetSetEffectiveDate);
                else
                {
                    // We need to get the latest address as this is what is happening on the screen
                    ILegalEntityAddress leAddress = le.LegalEntityAddresses[0];

                    // Get the latest address that has been added to the Legal Entity
                    foreach (ILegalEntityAddress leAdd in le.LegalEntityAddresses)
                    {
                        if (leAdd.Key > leAddress.Key)
                            leAddress = leAdd;
                    }

                    leAddress.EffectiveDate = _view.GetSetEffectiveDate;
                    LERepo.SaveLegalEntityAddress(leAddress, address);
                }
            }

            // Only validate an LE if it is completely new OR
            // If existing and not linked to an Account or Application
            if ((_legalEntity.Key > 0 && !LERepo.HasNonLeadRoles(_legalEntity)) || (_legalEntity.Key == 0))
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(spc.DomainMessages, "LegalEntityEstateAgencyMandatoryAddress", _legalEntity);
            }
        }

        protected void SetUpControlsForAdd()
        {
            //_letypes = LRepo.LegalEntityTypes.BindableDictionary;

            List<string> filterLst = new List<string>();
            filterLst.Add(Convert.ToString((int)LegalEntityTypes.Unknown));
            FilterLEType(filterLst);

            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntityKey))
            {
                _legalEntity = LERepo.GetLegalEntityByKey(Convert.ToInt32(GlobalCacheData[ViewConstants.LegalEntityKey]));
                _view.BindLegalEntityTypes(_letypes, _legalEntity.LegalEntityType.Key.ToString());
            }
            else
                _view.BindLegalEntityTypes(_letypes, string.Empty);

            _view.BindSalutation(LRepo.Salutations.BindableDictionary, String.Empty);
            _view.BindGender(LRepo.Genders.BindableDictionary, String.Empty);
            _view.BindLegalEntityStatus(LRepo.LegalEntityStatuses.BindableDictionary, Convert.ToString((int)SAHL.Common.Globals.LegalEntityStatuses.Alive));
            _view.BindIntroductionDate(DateTime.Now);

            string osDesc = String.Empty;
            string otDesc = String.Empty;
            string otDescription = String.Empty;

            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntityKey))
                _view.BindLegalEntity(_legalEntity, osDesc, otDescription);

            ////get the os for the selected LE to update, to set the default selected item
            _view.BindOrganisationType(OrganisationTypes, otDesc);
            _view.BindOrgStructureDesctiption(LENPRoleTypes, osDesc);

            // Bind Address Drop Downs
            _view.BindAddressTypeDropDown(LRepo.AddressTypes);
            _view.BindAddressFormatDropDown(LRepo.AddressFormatsByAddressType((AddressTypes)_view.GetAddressTypeSelectedValue));

            if (!_view.IsPostBack)
            {
                GlobalCacheData.Remove(AddressType);
                GlobalCacheData.Add(AddressType, _view.GetAddressTypeSelectedValue, LifeTimes);
            }
        }

        protected void SetUpControlsForUpdate()
        {
            string osDesc = String.Empty;
            string otDesc = String.Empty;
            string otDescription = String.Empty;

            // LOAD LE FROM CACHE - UPDATE MODE
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntityKey))
            {
                _leKey = Convert.ToInt32(GlobalCacheData[ViewConstants.LegalEntityKey]);
                _legalEntity = LERepo.GetLegalEntityByKey(_leKey);

                if (_osKeySet && _osKey > 0)
                {
                    osDesc = SelectedOrgNode.Description;
                    otDesc = SelectedOrgNode.OrganisationType.Key.ToString();
                    otDescription = SelectedOrgNode.OrganisationType.Description;
                }
            }

            // FILTER DROP DOWN
            List<string> filterLst = new List<string>();
            filterLst.Add(Convert.ToString((int)LegalEntityTypes.Unknown));

            // Check if we have an LE then we assume we updating i.e. Update Method... :)
            if (_legalEntity != null)
            {
                // FILTER DROP DOWN BASED OF LE TYPE
                if (_legalEntity.LegalEntityType.Key == (int)LegalEntityTypes.NaturalPerson)
                {
                    filterLst.Add(Convert.ToString((int)LegalEntityTypes.CloseCorporation));
                    filterLst.Add(Convert.ToString((int)LegalEntityTypes.Company));
                    filterLst.Add(Convert.ToString((int)LegalEntityTypes.Trust));
                }
                else
                {
                    filterLst.Add(Convert.ToString((int)LegalEntityTypes.NaturalPerson));
                }
            }

            FilterLEType(filterLst);
            _view.BindLegalEntityTypes(_letypes, _legalEntity.LegalEntityType.Key.ToString());
            _view.BindSalutation(LRepo.Salutations.BindableDictionary, String.Empty);
            _view.BindGender(LRepo.Genders.BindableDictionary, String.Empty);
            _view.BindLegalEntityStatus(LRepo.LegalEntityStatuses.BindableDictionary, Convert.ToString((int)SAHL.Common.Globals.LegalEntityStatuses.Alive));
            _view.BindIntroductionDate(DateTime.Now);

            if (_legalEntity != null)
            {
                _view.BindLegalEntity(_legalEntity, osDesc, otDescription);
            }

            ////get the os for the selected LE to update, to set the default selected item
            _view.BindOrganisationType(OrganisationTypes, otDesc);
            _view.BindOrgStructureDesctiption(LENPRoleTypes, osDesc);

            // Bind Address Drop Downs
            _view.BindAddressTypeDropDown(LRepo.AddressTypes);
            _view.BindAddressFormatDropDown(LRepo.AddressFormatsByAddressType((AddressTypes)_view.GetAddressTypeSelectedValue));

            if (!_view.IsPostBack)
            {
                GlobalCacheData.Remove(AddressType);
                GlobalCacheData.Add(AddressType, _view.GetAddressTypeSelectedValue, LifeTimes);
            }
        }

        protected void SetUpViewForAdd()
        {
            IsLegalEntityEditable();
            SetUpView();
            _view.SubmitButtonText = "Add";
            _view.SubmitButtonVisible = true;

        }

        protected void SetUpViewForUpdate()
        {
            // Limit the number the items that user can edit
            IsLegalEntityEditable();
            _view.AddressTypeVisible = false;
            _view.AddressFormatVisible = false;
            SetUpView();
            _view.SetUpAddressReadOnly();
            _view.SubmitButtonText = "Update";
            _view.SubmitButtonVisible = true;
        }

        protected void SetUpViewForDisplay()
        {
            _view.SetupDisplay(true);
            _view.LegalEntityTypeReadOnly = true;
            _view.OSDescriptionTypeAddReadOnly = true;
            _view.OrganisationTypeReadOnly = true;
            SetUpView();
            //_view.SetUpControlForViewOnly();
            _view.SetUpAddressReadOnly();
        }

        private void SetUpView()
        {
            _view.GetSetAddressFormat = (AddressFormats)_view.GetAddressFormatSelectedValue;

            if (_view.GetAjaxAddressKey != -1)
            {
                IAddress addressSelected = AddressRepo.GetAddressByKey(_view.GetAjaxAddressKey);
                _view.BindAddressDetails(addressSelected);
            }

            // Ask the view what is selected before deciding on what to display
            if (_view.SelectedLegalEntityType == (int)LegalEntityTypes.NaturalPerson)
            {
                _view.PanelNaturalPersonAddVisible = true;
                _view.PanelCompanyAddVisible = false;
            }
            else
            {
                _view.PanelNaturalPersonAddVisible = false;
                _view.PanelCompanyAddVisible = true;
            }
        }

        private void FilterLEType(List<string> filterList)
        {
            IDictionary<string, string> leDict = LRepo.LegalEntityTypes.BindableDictionary;
            _letypes = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> leType in leDict)
            {
                if (!filterList.Contains(leType.Key))
                    _letypes.Add(leType.Key, leType.Value);
            }
        }

        private void IsLegalEntityEditable()
        {
            if (_legalEntity != null && LERepo.HasNonLeadRoles(_legalEntity))
            {
                _view.SetupDisplay(true);
                _view.LegalEntityTypeReadOnly = true;
                _view.OSDescriptionTypeAddReadOnly = false;
                _view.OrganisationTypeReadOnly = false;
            }
            else
            {
                _view.SetupDisplay(false);
                _view.LegalEntityTypeReadOnly = false;
                _view.OSDescriptionTypeAddReadOnly = false;
                _view.OrganisationTypeReadOnly = false;
            }
        }

        #endregion

        #region Properties

        private IDictionary<string, string> _lenpRoleTypes;
        public IDictionary<string, string> LENPRoleTypes
        {
            get
            {
                if (_lenpRoleTypes == null)
                {
                    _lenpRoleTypes = new Dictionary<string, string>();
                    _lenpRoleTypes.Add(SAHL.Common.Constants.EstateAgent.Principal, SAHL.Common.Constants.EstateAgent.Principal);
                    _lenpRoleTypes.Add(SAHL.Common.Constants.EstateAgent.Consultant, SAHL.Common.Constants.EstateAgent.Consultant);
                }

                return _lenpRoleTypes;
            }
        }

        private IDictionary<string, string> _organisationTypes;
        public IDictionary<string, string> OrganisationTypes
        {
            get
            {
                if (_organisationTypes == null)
                {
                    _organisationTypes = new Dictionary<string, string>();

                    // Dont't allow a LE to be ADDED as type - Region_Channel
                    foreach (IOrganisationType ot in LRepo.OrganisationTypes)
                    {
                        if (ot.Key == (int)SAHL.Common.Globals.OrganisationTypes.Company
                            //|| ot.Key == (int)SAHL.Common.Globals.OrganisationTypes.Region_Channel
                            || ot.Key == (int)SAHL.Common.Globals.OrganisationTypes.Branch_Originator)
                            _organisationTypes.Add(ot.Key.ToString(), ot.Description);
                    }

                }
                return _organisationTypes;
            }
        }

        private ILegalEntityRepository _leRepo;
        public ILegalEntityRepository LERepo
        {
            get
            {
                if (_leRepo == null)
                    _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _leRepo;
            }
        }

        private IEstateAgentRepository _eaRepo;
        public IEstateAgentRepository EARepo
        {
            get
            {
                if (_eaRepo == null)
                    _eaRepo = RepositoryFactory.GetRepository<IEstateAgentRepository>();

                return _eaRepo;
            }
        }

        private IOrganisationStructureRepository _osRepo;
        public IOrganisationStructureRepository OSRepo
        {
            get
            {
                if (_osRepo == null)
                    _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _osRepo;
            }
        }

        private ILookupRepository _lRepo;
        public ILookupRepository LRepo
        {
            get
            {
                if (_lRepo == null)
                    _lRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lRepo;
            }
        }

        private IEstateAgentOrganisationNode _osSel;
        public IEstateAgentOrganisationNode SelectedOrgNode
        {
            get
            {
                if (_osKeySet && _osKey > 0)
                {
                    if (_osSel == null)
                        _osSel = (IEstateAgentOrganisationNode)EARepo.GetEstateAgentOrganisationNodeForKey(_osKey);

                    return _osSel;
                }

                return null;
            }
        }

        private IEstateAgentOrganisationNode _osParent;
        public IEstateAgentOrganisationNode SelectedParentOrgNode
        {
            get
            {
                if (_osParentKeySet && _osKeyParent > 0)
                {
                    if (_osParent == null)
                        _osParent = (IEstateAgentOrganisationNode)EARepo.GetEstateAgentOrganisationNodeForKey(_osKeyParent);

                    return _osParent;
                }

                return null;
            }
        }

        private ICommonRepository _cRepo;
        public ICommonRepository CRepo
        {
            get
            {
                if (_cRepo == null)
                    _cRepo = RepositoryFactory.GetRepository<ICommonRepository>();

                return _cRepo;
            }
        }

        private IAddressRepository _addressRepo;
        public IAddressRepository AddressRepo
        {
            get
            {
                if (_addressRepo == null)
                    _addressRepo = RepositoryFactory.GetRepository<IAddressRepository>();

                return _addressRepo;

            }
        }

        private IRuleService _ruleSvc;
        public IRuleService RuleSvc
        {
            get 
            { 
                if (_ruleSvc == null)
                    _ruleSvc = ServiceFactory.GetService<IRuleService>();
                return _ruleSvc; 
            }
        }
	

        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("AdminEstateAgentLegalEntityAdd");
                    views.Add("AdminEstateAgentLegalEntityUpdate");
                    views.Add("AdminEstateAgentLegalEntityView");
                    views.Add("AdminEstateAgentLegalEntityRemove");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        #endregion

    }
}