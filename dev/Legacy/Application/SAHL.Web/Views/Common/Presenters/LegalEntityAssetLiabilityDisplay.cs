using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityAssetLiabilityDisplay : LegalEntityAssetLiabilityBase
    {

        public LegalEntityAssetLiabilityDisplay(ILegalEntityAssetLiabilityDetails view, SAHLCommonBaseController controller)
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
            _view.OngrdAssetLiabilitySelectedIndexChanged+=new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OngrdAssetLiabilitySelectedIndexChanged);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            IEventList<ILegalEntityAssetLiability> leAssetLiabilityList = new EventList<ILegalEntityAssetLiability>();
            if (legalEntity.LegalEntityAssetLiabilities != null)
            {
                foreach (ILegalEntityAssetLiability _leal in legalEntity.LegalEntityAssetLiabilities)
                {
                    if (_leal.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        leAssetLiabilityList.Add(null,_leal);
                }
            }

                _view.BindAssetLiabilityGrid(_view.ViewName, leAssetLiabilityList);

            if (leAssetLiabilityList.Count > 0)
                _view.BindDisplayPanel(leAssetLiabilityList[gridSelectedIndex]);
        }

        protected void _view_OngrdAssetLiabilitySelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            gridSelectedIndex = Convert.ToInt32(e.Key);
        }
    }
}
