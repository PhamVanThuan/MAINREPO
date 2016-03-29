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
    public class PropertyDetailsUpdateDeeds : PropertyDetailsUpdate
    {
                /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public PropertyDetailsUpdateDeeds(IPropertyDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // setup screen mode
            _view.PropertyDetailsUpdateMode = PropertyDetailsUpdateMode.Deeds;
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
            _view.ShowDeedsTransfersGrid = false;
        }
    }
}
