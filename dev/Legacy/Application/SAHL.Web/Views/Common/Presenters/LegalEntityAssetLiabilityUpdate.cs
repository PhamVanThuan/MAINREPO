using System;
using SAHL.Common.DomainMessages;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityAssetLiabilityUpdate : LegalEntityAssetLiabilityBase
    {
        int selectedType;
        IEventList<ILegalEntityAssetLiability> leAssetLiabilityList;

        public LegalEntityAssetLiabilityUpdate(ILegalEntityAssetLiabilityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.BindAssetLiabilitySubTypes(lookups.AssetLiabilitySubTypes);

            if (legalEntity.LegalEntityAssetLiabilities != null && legalEntity.LegalEntityAssetLiabilities.Count > 0)
            {
                _view.ShowUpdateButton = true;
                _view.ShowCancelButton = true;
                _view.IsAddUpdate = true;
            }

            if (_view.IsPostBack && PrivateCacheData.ContainsKey("SelectedIndex"))
                gridSelectedIndex = Convert.ToInt32(PrivateCacheData["SelectedIndex"]);
            else
                gridSelectedIndex = 0;

            _view.OngrdAssetLiabilitySelectedIndexChanged += _view_OngrdAssetLiabilitySelectedIndexChanged;
            _view.OnAddButtonClicked += _view_OnAddButtonClicked;
        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            RefreshLEAList();

            if (leAssetLiabilityList.Count > 0)
            {
                _view.BindAssetLiabilityGrid(_view.ViewName, leAssetLiabilityList);
                GetViewSelectedType();
                SetUpViewForType(selectedType);
                _view.BindUpdatePanel(leAssetLiabilityList[gridSelectedIndex]);
            }
            else
                _view.BindAssetLiabilityGrid(_view.ViewName, null);

        }

        private void RefreshLEAList()
        {
            leAssetLiabilityList = new EventList<ILegalEntityAssetLiability>();
            if (legalEntity.LegalEntityAssetLiabilities != null)
            {
                foreach (ILegalEntityAssetLiability _leal in legalEntity.LegalEntityAssetLiabilities)
                {
                    if (_leal.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        leAssetLiabilityList.Add(null, _leal);
                }
            }
        }

        private void GetViewSelectedType()
        {
            if (leAssetLiabilityList[gridSelectedIndex].AssetLiability is IAssetLiabilityFixedProperty)
                selectedType = (int)AssetLiabilityTypes.FixedProperty;
            if (leAssetLiabilityList[gridSelectedIndex].AssetLiability is IAssetLiabilityInvestmentPrivate)
                selectedType = (int)AssetLiabilityTypes.UnlistedInvestments;
            if (leAssetLiabilityList[gridSelectedIndex].AssetLiability is IAssetLiabilityInvestmentPublic)
                selectedType = (int)AssetLiabilityTypes.ListedInvestments;
            if (leAssetLiabilityList[gridSelectedIndex].AssetLiability is IAssetLiabilityLiabilityLoan)
                selectedType = (int)AssetLiabilityTypes.LiabilityLoan;
            if (leAssetLiabilityList[gridSelectedIndex].AssetLiability is IAssetLiabilityLiabilitySurety)
                selectedType = (int)AssetLiabilityTypes.LiabilitySurety;
            if (leAssetLiabilityList[gridSelectedIndex].AssetLiability is IAssetLiabilityLifeAssurance)
                selectedType = (int)AssetLiabilityTypes.LifeAssurance;
            if (leAssetLiabilityList[gridSelectedIndex].AssetLiability is IAssetLiabilityOther)
                selectedType = (int)AssetLiabilityTypes.OtherAsset;
            if (leAssetLiabilityList[gridSelectedIndex].AssetLiability is IAssetLiabilityFixedLongTermInvestment)
                selectedType = (int)AssetLiabilityTypes.FixedLongTermInvestment;
        }

        protected void _view_OngrdAssetLiabilitySelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            gridSelectedIndex = Convert.ToInt32(e.Key);

            PrivateCacheData.Remove("SelectedIndex");
            PrivateCacheData.Add("SelectedIndex", gridSelectedIndex);
        }

        protected void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            int selectedIndex = _view.GetSelectedIndexOnGrid;

            if (leAssetLiabilityList == null)
                RefreshLEAList();


            if (leAssetLiabilityList != null)
            {
                int legalEntityAssetLiabilityKey = leAssetLiabilityList[selectedIndex].Key;

                PrivateCacheData.Remove("SelectedIndex");
                PrivateCacheData.Add("SelectedIndex", selectedIndex);

                TransactionScope txn = new TransactionScope();
                try
                {
                    for (int i = 0; i < legalEntity.LegalEntityAssetLiabilities.Count; i++)
                    {
                        if (legalEntity.LegalEntityAssetLiabilities[i].Key == legalEntityAssetLiabilityKey)
                        {
                            //if (_view.CheckStringsForZeroLength(legalEntity.LegalEntityAssetLiabilities[i].AssetLiability))
                            //{
                                legalEntity.LegalEntityAssetLiabilities[i].AssetLiability = _view.GetAssetLiablityForUpdate(legalEntity.LegalEntityAssetLiabilities[i].AssetLiability);
                                leRepo.SaveLegalEntityAssetLiability(legalEntity.LegalEntityAssetLiabilities[i]);
                                txn.VoteCommit();
                                break;
                            //}
                            //else
                            //{
                            //    View.Messages.Add(new Warning("You cannot add zero-length strings to mandatory text boxes.", "You cannot add zero-length strings to mandatory text boxes."));
                            //    break;
                            //}
                        }
                    }
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
            }

            if (_view.IsValid)
                Navigator.Navigate("Cancel");
        }
    }
}
