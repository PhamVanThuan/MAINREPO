using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityAssetLiabilityAdd : LegalEntityAssetLiabilityBase
    {
        int selectedType;

        public LegalEntityAssetLiabilityAdd(ILegalEntityAssetLiabilityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        private ILegalEntityRepository _legalEntityRepository;
        protected ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                if (_legalEntityRepository == null)
                    _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                return _legalEntityRepository;
            }
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.ShowUpdate = true;
            _view.SetUpdateButtonText("Add");
            _view.IsAddUpdate = true;

            IEventList<ILegalEntityAssetLiability> appAssets = new EventList<ILegalEntityAssetLiability>();

            if (legalEntity != null && legalEntity.LegalEntityAssetLiabilities != null && legalEntity.LegalEntityAssetLiabilities.Count > 0)
                for (int i = 0; i < legalEntity.LegalEntityAssetLiabilities.Count; i++)
                {
                    // Re TRAC #12275 - only add active assets and liabilities to appAssets
                    if (legalEntity.LegalEntityAssetLiabilities[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
                        appAssets.Add(_view.Messages, legalEntity.LegalEntityAssetLiabilities[i]);
                }


            //    _view.BindAssetLiabilityGrid(_view.ViewName, legalEntity.LegalEntityAssetLiabilities);
            if (appAssets.Count > 0)
                _view.BindAssetLiabilityGrid(_view.ViewName, appAssets);


            _view.BindAssetLiabilityTypes(lookups.AssetLiabilityTypes);
            _view.BindAssetLiabilitySubTypes(lookups.AssetLiabilitySubTypes);

            //
            BindlegalEntityAddress();

            if (legalEntity != null)
            {
                int legalEntityKey = legalEntity.Key;
                GlobalCacheData.Remove("legalEntityKey");
                GlobalCacheData.Add("legalEntityKey", legalEntityKey, LifeTimes);
            }

            _view.OnddlTypeSelectedIndexChanged += (_view_OnddlTypeSelectedIndexChanged);
            _view.OnAddButtonClicked += (_view_OnAddButtonClicked);
            _view.OnAddAddressButtonClicked += (_view_OnAddAddressButtonClicked);
        }

        /// <summary>
        /// This sets up and binds the drop down list for selecting properties (creates a diff between legal entity properties and the assets and liabilities)
        /// </summary>
        private void BindlegalEntityAddress()
        {

            IDictionary<string, string> leAddressLst = new Dictionary<string, string>();
            foreach (ILegalEntityAddress leAddress in legalEntity.LegalEntityAddresses)
            {
                //make sure the address is not already captured as an active asset
                if (leAddress.AddressType.Key == (int)AddressTypes.Residential)
                {
                    bool found = false;    
                    for (int i = 0; i < legalEntity.LegalEntityAssetLiabilities.Count; i++)
                    {

                        if (legalEntity.LegalEntityAssetLiabilities[i].AssetLiability is IAssetLiabilityFixedProperty)
                        {
                            IAssetLiabilityFixedProperty assetFixedProperty = legalEntity.LegalEntityAssetLiabilities[i].AssetLiability as IAssetLiabilityFixedProperty;
                            if ((assetFixedProperty.Address.Key == leAddress.Address.Key) && (legalEntity.LegalEntityAssetLiabilities[i].GeneralStatus.Key == (int)GeneralStatuses.Active))
                                found = true;
                        }

                    }

                    if (!found && !leAddressLst.ContainsKey(leAddress.Address.Key.ToString()))
                    {
                        leAddressLst.Add(leAddress.Address.Key.ToString(), leAddress.Address.GetFormattedDescription(AddressDelimiters.Comma));
                    }
                }
            }

            if (legalEntity != null) _view.BindlegalEntityAddress(leAddressLst);
        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAssetLiabilityType))
            {
                selectedType = Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedAssetLiabilityType]);
                _view.RestoreAssetTypeValueForFixedProperty(Convert.ToInt32(GlobalCacheData[ViewConstants.SelectedAssetLiabilityType]));
                _view.LiabilityValue = Convert.ToDouble(GlobalCacheData[ViewConstants.SelectedAssetLiabilityLiabilityValue]);
                _view.AssetValue = Convert.ToDouble(GlobalCacheData[ViewConstants.SelectedAssetLiabilityAssetValue]);
            }

            SetUpViewForType(selectedType);
            if (selectedType > 0)
            {
                _view.ShowCancelButton = true;
                _view.ShowUpdateButton = true;
            }
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedAssetLiabilityDateAcquired) && GlobalCacheData[ViewConstants.SelectedAssetLiabilityDateAcquired] != null)
                _view.DateAcquired = Convert.ToDateTime(GlobalCacheData[ViewConstants.SelectedAssetLiabilityDateAcquired]);
        }

        void _view_OnAddAddressButtonClicked(object sender, EventArgs e)
        {
            SaveFormDataToGlobalCache();

            Navigator.Navigate("LegalEntityAddressAdd");
        }

        private void SaveFormDataToGlobalCache()
        {
            // Save the data captured on the View to Global Cache as it will be needed when Navigating back to AssetLiability
            // View , from Address View
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
            GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, _genericKey, LifeTimes);

            GlobalCacheData.Remove(ViewConstants.NavigateTo);
            GlobalCacheData.Add(ViewConstants.NavigateTo, _view.ViewName, LifeTimes);

            GlobalCacheData.Remove(ViewConstants.SelectedAssetLiabilityType);
            GlobalCacheData.Add(ViewConstants.SelectedAssetLiabilityType, selectedType, LifeTimes);

            GlobalCacheData.Remove(ViewConstants.SelectedAssetLiabilityDateAcquired);
            GlobalCacheData.Add(ViewConstants.SelectedAssetLiabilityDateAcquired, _view.DateAcquired, LifeTimes);

            GlobalCacheData.Remove(ViewConstants.SelectedAssetLiabilityAssetValue);
            GlobalCacheData.Add(ViewConstants.SelectedAssetLiabilityAssetValue, _view.AssetValue, LifeTimes);

            GlobalCacheData.Remove(ViewConstants.SelectedAssetLiabilityLiabilityValue);
            GlobalCacheData.Add(ViewConstants.SelectedAssetLiabilityLiabilityValue, _view.LiabilityValue, LifeTimes);
        }

        void _view_OnddlTypeSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToString(e.Key) != "-select-")
            {
                selectedType = Convert.ToInt32(e.Key);

                if (selectedType != (int)AssetLiabilityTypes.FixedProperty)
                    ClearGlobalCache();
            }
        }

        void _view_OnAddButtonClicked(object sender, EventArgs e)
        {

            // check for 
            int legalEntityKey = Convert.ToInt32(GlobalCacheData["legalEntityKey"]);
            legalEntity = LegalEntityRepository.GetLegalEntityByKey(legalEntityKey);
            ILegalEntityAssetLiability leAssetLiability = leRepo.GetEmptyLegalEntityAssetLiability();

            IAssetLiability assetLiability = leRepo.GetEmptyAssetLiability((AssetLiabilityTypes)selectedType);
            //if (_view.CheckStringsForZeroLength(assetLiability))
            //{
                assetLiability = _view.GetAssetLiablityForAdd(assetLiability);

                leAssetLiability.AssetLiability = assetLiability;
                leAssetLiability.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];
                leAssetLiability.LegalEntity = legalEntity;

                legalEntity.LegalEntityAssetLiabilities.Add(_view.Messages, leAssetLiability);
                // Calling Validate as the save fails for Fixed property but does not throw the errors !
                leAssetLiability.AssetLiability.ValidateEntity();

                if (!_view.IsValid)
                    return;

                TransactionScope txn = new TransactionScope();

                try
                {
                    //LegalEntityAddressDoNotDeleteOnRole 
                    ExclusionSets.Add(RuleExclusionSets.LegalEntityAssetLiabilityDetailsView);
                    leRepo.SaveLegalEntity(legalEntity, false);
                    txn.VoteCommit();
                    ExclusionSets.Remove(RuleExclusionSets.LegalEntityAssetLiabilityDetailsView);
                }

                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }

                finally
                {
                    txn.Dispose();
                }
            //}
            //else
            //    View.Messages.Add(new Warning("You cannot add zero-length strings to mandatory text boxes.", "You cannot add zero-length strings to mandatory text boxes."));


            if (_view.IsValid)
            {
                ClearGlobalCache();
                _view.Navigator.Navigate(_view.ViewName);
            }

        }

        private void ClearGlobalCache()
        {
            GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);

            GlobalCacheData.Remove(ViewConstants.NavigateTo);

            GlobalCacheData.Remove(ViewConstants.SelectedAssetLiabilityType);

            GlobalCacheData.Remove(ViewConstants.SelectedAssetLiabilityDateAcquired);

            GlobalCacheData.Remove(ViewConstants.SelectedAssetLiabilityAssetValue);

            GlobalCacheData.Remove(ViewConstants.SelectedAssetLiabilityLiabilityValue);
        }




    }
}
