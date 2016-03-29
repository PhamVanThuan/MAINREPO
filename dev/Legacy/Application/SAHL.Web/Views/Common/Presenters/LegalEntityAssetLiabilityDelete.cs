using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using System;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityAssetLiabilityDelete : LegalEntityAssetLiabilityBase
    {

        IEventList<ILegalEntityAssetLiability> leAssetLiabilityList; 
        public LegalEntityAssetLiabilityDelete(ILegalEntityAssetLiabilityDetails view, SAHLCommonBaseController controller)
        : base(view, controller)
    
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            gridSelectedIndex = 0;

            _view.ShowUpdate = false;

            if (legalEntity.LegalEntityAssetLiabilities != null && legalEntity.LegalEntityAssetLiabilities.Count > 0)
            {
                bool displaybuttons = false;

                foreach (ILegalEntityAssetLiability _leal in legalEntity.LegalEntityAssetLiabilities)
                {
                    if (_leal.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        displaybuttons = true;

                }

                _view.ShowDeleteButton = displaybuttons;
                _view.ShowCancelButton = displaybuttons;
            }
            _view.OngrdAssetLiabilitySelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OngrdAssetLiabilitySelectedIndexChanged);
            _view.OnDeleteButtonClicked+=new EventHandler(_view_OnDeleteButtonClicked);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            RefreshLEAList();

            if (leAssetLiabilityList.Count > 0)
                _view.BindAssetLiabilityGrid(_view.ViewName, leAssetLiabilityList);
            else
                _view.BindAssetLiabilityGrid(_view.ViewName, null);

            if (leAssetLiabilityList.Count > 0)
                _view.BindDisplayPanel(leAssetLiabilityList[gridSelectedIndex]);   
           
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

        protected void _view_OngrdAssetLiabilitySelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            gridSelectedIndex = Convert.ToInt32(e.Key);
        }

        protected void _view_OnDeleteButtonClicked(object sender, EventArgs e)
        {
            int selectedIndex = _view.GetSelectedIndexOnGrid;

            if (leAssetLiabilityList == null)
                RefreshLEAList();

            int legalEntityAssetLiabilityKey = leAssetLiabilityList[selectedIndex].Key;

            TransactionScope txn = new TransactionScope();

            try
            {
                for (int i = 0; i < legalEntity.LegalEntityAssetLiabilities.Count; i++)
                {
                    if (legalEntity.LegalEntityAssetLiabilities[i].Key == legalEntityAssetLiabilityKey)
                    {
                        legalEntity.LegalEntityAssetLiabilities[i].GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Inactive];
                        leRepo.SaveLegalEntityAssetLiability(legalEntity.LegalEntityAssetLiabilities[i]);
                        txn.VoteCommit();
                        break;
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

            if (_view.IsValid)
                Navigator.Navigate("Cancel");

        }
    }
}
