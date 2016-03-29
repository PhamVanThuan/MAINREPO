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

namespace SAHL.Web.Views.Common.Presenters.Address
{
    public class PropertyAddressUpdateNoExclusions : PropertyAddressUpdate
    {
        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PropertyAddressUpdateNoExclusions(IAddressView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewInitialised(sender, e);

            base.ExcludePropertyAddressUpdateRules = false;

            if (!ValidateOnViewInitialised())
                _view.ButtonRowVisible = false;
        }
    }
}
