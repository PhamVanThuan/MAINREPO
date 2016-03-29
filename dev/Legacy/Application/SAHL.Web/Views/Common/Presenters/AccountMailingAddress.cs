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
    /// Account Mailing Address - Display Presenter
    /// </summary>
    public class AccountMailingAddress : AccountMailingAddressBase
    {
        /// <summary>
        /// Constructor - Account Mailing Address - Display
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AccountMailingAddress(IAccountMailingAddress view, SAHLCommonBaseController controller)
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

            if (!_view.ShouldRunPage) return;

            _view.SetControlsForDisplay();

            //_view.BindMailingAddressLstDisplay(mailingAddressLst);
            _view.CorrespondenceMediumRowVisible = true;
           
            if (accMailingAddress !=null && accMailingAddress.Count > 0)
                _view.BindDisplayFields(accMailingAddress[0]);

        }
    }
}
