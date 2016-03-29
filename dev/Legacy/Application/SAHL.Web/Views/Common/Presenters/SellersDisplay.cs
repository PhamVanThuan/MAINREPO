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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// Displays the list of Sellers (no actions permitted).
    /// </summary>
    public class SellersDisplay : ApplicantsOfferBase
    {
        /// <summary>
        /// Consructor. Gets the View and controller pairs.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public SellersDisplay(IApplicants view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;

            // set the applicationroletypes to display
            base.ApplicationRoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)SAHL.Common.Globals.OfferRoleTypes.Seller);
            base.ApplicationRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.ApplicationRoleTypes[(int)OfferRoleTypes.Seller]));

            _view.GridHeading = "Sellers";

            // call the base initialise to handle the binding etc
            base.OnViewInitialised(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage) return;

            base.OnViewPreRender(sender, e);

            _view.ButtonsVisible = false;
       }

    }
}
