using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters.Address
{
    /// <summary>
    /// Presenter used to display an address for a legal entity.
    /// </summary>
    public class LegalEntityAddressDisplay : LegalEntityAddressBase
    {

        public LegalEntityAddressDisplay(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // add event handlers
            _view.AuditButtonClicked += new EventHandler(View_AuditClicked);

        }

        void View_AuditClicked(object sender, EventArgs e)
        {
            // _view.Navigator.Navigate("AddressAuditTrail");
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            // set panel display properties
            _view.AddressListHeaderText = "Legal Entity Addresses";
            _view.AddressListVisible = true;
            _view.AddressDetailsVisible = true;

            // set audit trail button visibility
            _view.AuditTrailButtonVisible = false;
        }

    }
}
