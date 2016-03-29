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
using SAHL.Common.Web.UI;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using NHibernate.Mapping;
using System.Collections;
using System.Collections.Generic;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Legal Entity Domicilium Address - Display Presenter
    /// </summary>
    public class LegalEntityDomiciliumAddressDisplay : LegalEntityDomiciliumAddressBase
    {
        /// <summary>
        /// Constructor - Legal Entity Domicilium Address - Display
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDomiciliumAddressDisplay(ILegalEntityDomiciliumAddress view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }
        /// <summary>
        /// OnView Initialised event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            // We are in loan servicing mode - show the active LegalEntityDomicilium
            if (base.legalEntity != null && base.legalEntity.ActiveDomicilium != null)
            {
                _view.BindDomiciliumAddress(base.legalEntity.ActiveDomicilium.LegalEntityAddress.Address.GetFormattedDescription(AddressDelimiters.Comma));
            }

            _view.SetControlsForDisplay("Legal Entity Domicilium Address");
        }
    }
}
