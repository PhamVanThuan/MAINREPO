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

namespace SAHL.Web.Views.Common.Presenters
{
    public class PropertyDetailsDisplay : PropertyDetailsBase
    {
        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PropertyDetailsDisplay(IPropertyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // setup screen mode
            _view.PropertyDetailsUpdateMode = PropertyDetailsUpdateMode.Display;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.ShowPropertyGrid = true;
            _view.ShowDeedsTransfersGrid = true;
        }
    }
}
