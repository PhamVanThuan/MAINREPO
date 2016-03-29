using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    public class LegalEntityDetailsAddBase : SAHLCommonBasePresenter<ILegalEntityDetails>
    {
        private ILookupRepository _lookupRepository;
        private ILegalEntityRepository _legalEntityRepository;
        private ILegalEntity _legalEntity;
        private int _selectedLegalEntityType;

        protected int SelectedLegalEntityType
        {
            get { return _selectedLegalEntityType; }
            set { _selectedLegalEntityType = value; }
        }

        protected ILegalEntity LegalEntity
        {
            get { return _legalEntity; }
            set { _legalEntity = value; }
        }

        protected ILegalEntityRepository LegalEntityRepository
        {
            get { return _legalEntityRepository; }
            set { _legalEntityRepository = value; }
        }

        protected ILookupRepository LookupRepository
        {
            set { _lookupRepository = value; }
            get { return _lookupRepository; }
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
                        _selectedRoleTypeKey = _view.SelectedRoleTypAdd;
                        break;

                    case (int)SAHL.Common.Globals.LegalEntityTypes.Company:
                    case (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation:
                    case (int)SAHL.Common.Globals.LegalEntityTypes.Trust:
                        _selectedRoleTypeKey = _view.SelectedRoleTypAdd;
                        break;

                    default:
                        break;
                }

                return _selectedRoleTypeKey;
            }
        }

        public LegalEntityDetailsAddBase(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Hook the additional events
            _view.onCancelButtonClicked += new EventHandler(CancelButtonClicked);
            _view.onSubmitButtonClicked += new EventHandler(SubmitButtonClicked);
            _view.onLegalEntityTypeChange += new KeyChangedEventHandler(LegalEntityTypeChange);
            _view.OnReBindLegalEntity += new KeyChangedEventHandler(ReBindLegalEntity);

            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
        }

        protected virtual void ReBindLegalEntity(object sender, KeyChangedEventArgs e)
        {
            _legalEntity = _legalEntityRepository.GetLegalEntityByKey(Convert.ToInt32(e.Key));

            // Persist the LegalEntityKey in the Global cache (and call the next presenter)
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, e.Key, new List<ICacheObjectLifeTime>());

            // The inheriting class needs to call the next node (Add Existing).
        }

        protected virtual void LegalEntityTypeChange(object sender, KeyChangedEventArgs e)
        {
            SelectedLegalEntityType = (int)e.Key;
        }

        protected virtual void SubmitButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Submit");
        }

        protected void PersistLegalEntity()
        {
            // Save stuff to the Global Cache
            if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                GlobalCacheData.Remove(ViewConstants.LegalEntity);

            GlobalCacheData.Add(ViewConstants.LegalEntity, LegalEntity, new List<ICacheObjectLifeTime>());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Justification = "New rule added to FxCop, avoiding refactor")]
        protected virtual void SaveLegalEntity()
        {
            // Create a blank LE populate it and save it
            _legalEntity = LegalEntityRepository.GetEmptyLegalEntity((LegalEntityTypes)View.SelectedLegalEntityType);

            // Get the details from the screen
            _view.PopulateLegalEntityDetailsForAdd(_legalEntity);

            // Save
            TransactionScope txn = new TransactionScope(TransactionMode.Inherits);
            try
            {
                LegalEntityRepository.SaveLegalEntity(_legalEntity, true);
                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                //db can rollback txn when rules fail, need to not throw ex
                //if view is valid
                try
                {
                    txn.Dispose();
                }
                catch (Exception)
                {
                    if (_view.IsValid)
                        throw;
                }
            }
        }

        protected virtual void CancelButtonClicked(object sender, EventArgs e)
        {
            ClearGlobalCache();
            Navigator.Navigate("Cancel");
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            // if we have got here via a menu clickthen clear out the cache
            if (_view.IsMenuPostBack)
                ClearGlobalCache();

            if (!_view.ShouldRunPage)
                return;

            LookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            // Bind LegalEntity Types (Remove the 'Unknown' type) - Default is NaturalPerson
            IDictionary<string, string> letypes = LookupRepository.LegalEntityTypes.BindableDictionary;
            foreach (KeyValuePair<string, string> letype in letypes)
            {
                if (letype.Key == Convert.ToString((int)SAHL.Common.Globals.LegalEntityTypes.Unknown))
                {
                    letypes.Remove(letype);
                    break;
                }
            }
            _view.BindLegalEntityTypes(letypes, Convert.ToString((int)LegalEntityTypes.NaturalPerson));

            _view.BindSalutation(LookupRepository.Salutations.BindableDictionary, String.Empty);
            _view.BindGender(LookupRepository.Genders.BindableDictionary, String.Empty);
            _view.BindMaritalStatus(LookupRepository.MaritalStatuses.BindableDictionary, String.Empty);
            _view.BindCitizenType(LookupRepository.CitizenTypes.BindableDictionary, String.Empty);
            _view.BindPopulationGroup(LookupRepository.PopulationGroups.BindableDictionary, Convert.ToString((int)SAHL.Common.Globals.PopulationGroups.Unknown));
            _view.BindEducation(LookupRepository.Educations.BindableDictionary, Convert.ToString((int)SAHL.Common.Globals.Educations.Unknown));
            _view.BindHomeLanguage(LookupRepository.Languages.Values, Convert.ToString((int)SAHL.Common.Globals.Languages.Unknown));
            _view.BindDocumentLanguage(LookupRepository.LanguagesTranslatable, Convert.ToString((int)SAHL.Common.Globals.Languages.English));
            _view.BindLegalEntityStatus(LookupRepository.LegalEntityStatuses.BindableDictionary, Convert.ToString((int)SAHL.Common.Globals.LegalEntityStatuses.Alive));
            _view.BindResidenceStatus(LookupRepository.ResidenceStatuses.BindableDictionary, String.Empty);
            _view.SelectAllMarketingOptionsExcluded = false;
            _view.BindMarketingOptons(LookupRepository.MarketingOptionsActive);
            _view.BindIntroductionDate(DateTime.Now);
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

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            // Indicate which controls need to be displayed/hidden
            _view.PanelAddVisible = true;
            _view.UpdateControlsVisible = true;
            // Additionally, on binding LegalEntityType, expect the default option to be NaturalPerson
            _view.PanelNaturalPersonAddVisible = true;

            _view.PanelCompanyAddVisible = false;
            _view.LockedUpdateControlsVisible = false;
            _view.NonContactDetailsDisabled = false;
            _view.MarketingOptionsEnabled = true;
            _view.CancelButtonVisible = true;
            _view.SubmitButtonVisible = true;

            _selectedLegalEntityType = _view.SelectedLegalEntityType;

            // Ask the view what is selected before deciding on what to display
            if (_selectedLegalEntityType == (int)LegalEntityTypes.NaturalPerson)
            {
                _view.PanelNaturalPersonAddVisible = true;
                _view.PanelCompanyAddVisible = false;
            }
            else
            {
                _view.PanelNaturalPersonAddVisible = false;
                _view.PanelCompanyAddVisible = true;
            }

            _view.AddRoleTypeVisible = false;
            _view.UpdateRoleTypeVisible = false;
            _view.DisplayRoleTypeVisible = false;
        }

        // Helper method: populate marketing option check for additions and deletions
        protected void PopulateMarketingOptions()
        {
            bool found = false;

            ListItemCollection marketingOptionListBoxItems = _view.MarketingOptionsExcluded;

            // Handle possible Deletions
            foreach (ILegalEntityMarketingOption legalEntityMarketingOption in _legalEntity.LegalEntityMarketingOptions)
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
    }
}