using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using NHibernate;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsUpdateBase : SAHLCommonBasePresenter<ILegalEntityDetails>
    {
        private ILegalEntity _legalEntity;
        private int _legalEntityKey;
        private ILookupRepository _lookupRepository;
        protected ILegalEntityRepository _legalEntityRepository;
        protected IControlRepository _ctrlRepository;
        protected ICommonRepository _commonRepository;
        private IApplicationRepository _applicationRepository;

        protected IApplication _application;

        private CBOMenuNode _cboMenuNode;

        public CBOMenuNode CBOMenuNode
        {
            get { return _cboMenuNode; }
            set { _cboMenuNode = value; }
        }

        protected ILegalEntityRepository LegalEntityRepository
        {
            get { return _legalEntityRepository; }
            set { _legalEntityRepository = value; }
        }

        protected int LegalEntityKey
        {
            get { return _legalEntityKey; }
        }

        protected IControlRepository CtrlRepository
        {
            set { _ctrlRepository = value; }
            get { return _ctrlRepository; }
        }

        protected ILookupRepository LookupRepository
        {
            set { _lookupRepository = value; }
            get { return _lookupRepository; }
        }

        protected IApplicationRepository ApplicationRepository
        {
            set { _applicationRepository = value; }
            get { return _applicationRepository; }
        }

        public ILegalEntity LegalEntity
        {
            set { _legalEntity = value; }
            get { return _legalEntity; }
        }

        protected IApplication Application
        {
            get { return _application; }
            set { _application = value; }
        }

        private int _selectedRoleTypeKey;

        protected int SelectedRoleTypeKey
        {
            get
            {
                _selectedRoleTypeKey = -1;
                switch (this.LegalEntity.LegalEntityType.Key)
                {
                    case (int)SAHL.Common.Globals.LegalEntityTypes.NaturalPerson:
                    case (int)SAHL.Common.Globals.LegalEntityTypes.Unknown:
                        _selectedRoleTypeKey = _view.SelectedRoleTypeUpdateNaturalPerson;
                        break;

                    case (int)SAHL.Common.Globals.LegalEntityTypes.Company:
                    case (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation:
                    case (int)SAHL.Common.Globals.LegalEntityTypes.Trust:
                        _selectedRoleTypeKey = _view.SelectedRoleTypeUpdateCompany;
                        break;

                    default:
                        break;
                }

                return _selectedRoleTypeKey;
            }
        }

        public LegalEntityDetailsUpdateBase(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.onCancelButtonClicked += new EventHandler(OnCancelButtonClicked);
            _view.onSubmitButtonClicked += new EventHandler(OnSubmitButtonClicked);

            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            _ctrlRepository = RepositoryFactory.GetRepository<IControlRepository>();
        }

        protected virtual void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            ClearGlobalCache();
            Navigator.Navigate("Submit");
        }

        protected virtual void OnCancelButtonClicked(object sender, EventArgs e)
        {
            ClearGlobalCache();
            Navigator.Navigate("Cancel");
        }

        protected void ClearGlobalCache()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                GlobalCacheData.Remove(ViewConstants.LegalEntity);
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationRoleKey))
                GlobalCacheData.Remove(ViewConstants.ApplicationRoleKey);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            // if we have got here via a menu clickthen clear out the cache
            if (_view.IsMenuPostBack)
                ClearGlobalCache();

            if (!_view.ShouldRunPage)
                return;

            _application = null;
            if (GlobalCacheData.ContainsKey(ViewConstants.CreateApplication))
                _application = GlobalCacheData[ViewConstants.CreateApplication] as IApplication;

            //// disable the ajax functionality so that the users cannot use the idnumber ajax to "pull in" another legal entities information
            //_view.DisableAjaxFunctionality = true;

            // Bind the lookups
            BindLookups();
        }

        protected void BindLookups()
        {
            BindLookups(true);
        }

        protected void BindLookups(bool bindRoleTypes)
        {
            // bind role types
            if (_application != null && bindRoleTypes)
            {
                IDictionary<string, string> RoleTypes = _applicationRepository.GetApplicantRoleTypesForApplication(_application);
                _view.BindRoleTypes(RoleTypes, String.Empty);
            }

            // Only bind non-natural person types ...
            IDictionary<string, string> legalEntityTypes = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> legalEntityType in LookupRepository.LegalEntityTypes.BindableDictionary)
            {
                if (!(Convert.ToInt32(legalEntityType.Key) == (int)LegalEntityTypes.NaturalPerson || Convert.ToInt32(legalEntityType.Key) == (int)LegalEntityTypes.Unknown))
                    legalEntityTypes.Add(legalEntityType);
            }

            _view.BindLegalEntityTypes(legalEntityTypes, String.Empty);
            _view.BindSalutation(LookupRepository.Salutations.BindableDictionary, String.Empty);
            _view.BindGender(LookupRepository.Genders.BindableDictionary, String.Empty);
            _view.BindMaritalStatus(LookupRepository.MaritalStatuses.BindableDictionary, String.Empty);
            _view.BindPopulationGroup(LookupRepository.PopulationGroups.BindableDictionary, String.Empty);
            _view.BindEducation(LookupRepository.Educations.BindableDictionary, String.Empty);
            _view.BindCitizenType(LookupRepository.CitizenTypes.BindableDictionary, String.Empty);
            _view.BindHomeLanguage(LookupRepository.Languages.Values, String.Empty);
            _view.BindDocumentLanguage(LookupRepository.LanguagesTranslatable, String.Empty);
            _view.BindLegalEntityStatus(LookupRepository.LegalEntityStatuses.BindableDictionary, String.Empty);
            _view.BindResidenceStatus(LookupRepository.ResidenceStatuses.BindableDictionary, String.Empty);
            _view.SelectAllMarketingOptionsExcluded = true;
            _view.BindMarketingOptons(LookupRepository.MarketingOptionsActive);
        }

        protected void BindLegalEntity()
        {
            // Call the bind method
            if (LegalEntity is ILegalEntityNaturalPerson)
                View.BindLegalEntityUpdatableNaturalPerson((ILegalEntityNaturalPerson)LegalEntity);
            else
                View.BindLegalEntityUpdatableCompany(LegalEntity);
        }

        protected void LoadLegalEntityFromCBO()
        {
            _cboMenuNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            _legalEntityKey = Convert.ToInt32(_cboMenuNode.GenericKey);
            LegalEntity = LegalEntityRepository.GetLegalEntityByKey(_legalEntityKey);
        }

        protected void LoadLegalEntityFromLogin()
        {
            ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
            IADUser adUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

            if (adUser != null)
            {
                if (adUser.LegalEntity != null)
                    LegalEntity = adUser.LegalEntity;
            }
        }

        protected void LoadLegalEntityFromGlobalCache()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                _legalEntityKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLegalEntityKey]);
            else
                throw new Exception("HARD-CODED KEYS NOT ALLOWED");

            LegalEntity = LegalEntityRepository.GetLegalEntityByKey(_legalEntityKey);
        }

        protected void LoadApplicationLegalEntityFromGlobalCache()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                _legalEntityKey = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedLegalEntityKey]);

            LegalEntity = LegalEntityRepository.GetLegalEntityByKey(_legalEntityKey);
        }

        protected void PersistGlobalCacheVariables()
        {
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                GlobalCacheData.Remove(ViewConstants.LegalEntity);

            GlobalCacheData.Add(ViewConstants.LegalEntity, LegalEntity, new List<ICacheObjectLifeTime>());
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.PanelAddVisible = false;
            _view.PanelNaturalPersonAddVisible = false;
            _view.PanelCompanyAddVisible = false;
            _view.UpdateControlsVisible = true;

            _view.InsurableInterestDisplayVisible = false;
            _view.InsurableInterestUpdateVisible = false;

            _view.MarketingOptionsEnabled = true;
            _view.CancelButtonVisible = true;
            _view.SubmitButtonVisible = true;

            if (_legalEntity is ILegalEntityNaturalPerson)
            {
                _view.PanelCompanyDisplayVisible = false;
                _view.PanelNaturalPersonDisplayVisible = true;
            }
            else
            {
                _view.PanelCompanyDisplayVisible = true;
                _view.PanelNaturalPersonDisplayVisible = false;
            }

            // If LE has active financial service and no override
            if (_legalEntity.IsUpdatable == false)
                _view.LockedUpdateControlsVisible = false;
            else
                _view.LockedUpdateControlsVisible = true;

            _view.AddRoleTypeVisible = false;
            _view.UpdateRoleTypeVisible = false;
            _view.DisplayRoleTypeVisible = false;
        }

        // Helper method: populate marketing option check for additions and deletions
        protected void PopulateMarketingOptions()
        {
            bool found = false;

            ListItemCollection marketingOptionListBoxItems = _view.MarketingOptionsExcluded;

            // get a list of the origional marketing options for the legalentity
            IEventList<ILegalEntityMarketingOption> origionalMarketingOptions = new EventList<ILegalEntityMarketingOption>(_legalEntity.LegalEntityMarketingOptions);

            // Handle possible Deletions
            foreach (ILegalEntityMarketingOption legalEntityMarketingOption in origionalMarketingOptions)
            {
                ListItem hasItem = marketingOptionListBoxItems.FindByValue(legalEntityMarketingOption.MarketingOption.Key.ToString());
                if (hasItem != null && hasItem.Selected != false)
                    _legalEntity.LegalEntityMarketingOptions.Remove(_view.Messages, legalEntityMarketingOption);
            }

            // Handle possible Additions
            foreach (ListItem marketingOption in marketingOptionListBoxItems)
            {
                found = false;

                if (marketingOption.Selected != true)
                {
                    // Find the option in the domain object; If selected then add it
                    foreach (ILegalEntityMarketingOption legalEntityMarketingOption in _legalEntity.LegalEntityMarketingOptions)
                    {
                        if (legalEntityMarketingOption.MarketingOption.Key == Convert.ToInt32(marketingOption.Value))
                        {
                            found = true;
                            break;
                        }
                    }

                    // If not found, add it.
                    if (!found)
                    {
                        ILegalEntityMarketingOption legalEntityMarketingOption = _legalEntityRepository.GetEmptyLegalEntityMarketingOption();
                        legalEntityMarketingOption.MarketingOption = _lookupRepository.MarketingOptions.ObjectDictionary[marketingOption.Value];
                        legalEntityMarketingOption.LegalEntity = _legalEntity;

                        _legalEntity.LegalEntityMarketingOptions.Add(_view.Messages, legalEntityMarketingOption);
                    }
                }
            }
        }

        protected void ReloadLegalEntityIfTypeChanged()
        {
            // do this check below so we can pick up if the user has changed the legalentity type (company types only)
            switch (_legalEntity.LegalEntityType.Key)
            {
                case (int)LegalEntityTypes.Company:
                case (int)LegalEntityTypes.CloseCorporation:
                case (int)LegalEntityTypes.Trust:
                    int legalEntityTypeKey = _view.SelectedCOLegalEntityTypeUpdate;
                    if (legalEntityTypeKey > 0 && legalEntityTypeKey != _legalEntity.LegalEntityType.Key)
                    {
                        // if we are here then the legalentity type has changed
                        if (_commonRepository == null)
                            _commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();

                        // get the legalentitykey
                        int legalEntityKey = _legalEntity.Key;

                        // get the nhibernate session
                        ISession Sess = _commonRepository.GetNHibernateSession(_legalEntity);
                        // clear the nhibernate session
                        Sess.Clear();
                        // update the legalentity type via sql
                        _legalEntityRepository.UpdateLegalEntityType(legalEntityKey, legalEntityTypeKey);
                        // re-get the legal entity
                        _legalEntity = _legalEntityRepository.GetLegalEntityByKey(legalEntityKey);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}