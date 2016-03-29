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

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Legal Entity Domicilium Address - Display Presenter
    /// </summary>
    public class LegalEntityDomiciliumAddressDisplayWorkflow : LegalEntityDomiciliumAddressBase
    {
        /// <summary>
        /// Constructor - Legal Entity Domicilium Address - Display
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDomiciliumAddressDisplayWorkflow(ILegalEntityDomiciliumAddress view, SAHLCommonBaseController controller)
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

            // walk up tree till we get a node with application key
            CBONode parentNode = node.GetParentNodeByType(SAHL.Common.Globals.GenericKeyTypes.Offer);

            // get the application role for this legalentity
            IApplicationRole applicationRole = base.legalEntity.GetApplicationRoleClient(parentNode.GenericKey);

            // we in workflow mode, show the offerroledomicilium
            if (applicationRole.ApplicationRoleDomicilium != null)
            {
                _view.BindDomiciliumAddress(applicationRole.ApplicationRoleDomicilium.LegalEntityDomicilium.LegalEntityAddress.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma));
            }

            _view.SetControlsForDisplay("Proposed Pending Domicilium Address");
        }
    }
}
