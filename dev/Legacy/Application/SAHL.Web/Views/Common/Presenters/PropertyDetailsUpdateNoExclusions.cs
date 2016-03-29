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

namespace SAHL.Web.Views.Common.Presenters
{
    public class PropertyDetailsUpdateNoExclusions : PropertyDetailsUpdate
    {
        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PropertyDetailsUpdateNoExclusions(IPropertyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewInitialised(sender, e);

            base.ExcludePropertyUpdateRules = false;

            if (!ValidateOnViewInitialised())
                _view.ButtonRowVisible = false;
        }
    }
}
