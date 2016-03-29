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
    public class RelatedLegalEntitySuretor : RelatedLegalEntity
    {
        /// <summary>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public RelatedLegalEntitySuretor(IRelatedLegalEntity view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _view.AllowGridSelect = false;
            _view.AllowGridDoubleClick = false;
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewInitialised(sender, e);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            base.OnViewPreRender(sender, e);

            _view.AddToMenuButtonEnabled = false;
            _view.RemoveButtonEnabled = false;
            _view.CancelButtonEnabled = false;

        }
    }
}
