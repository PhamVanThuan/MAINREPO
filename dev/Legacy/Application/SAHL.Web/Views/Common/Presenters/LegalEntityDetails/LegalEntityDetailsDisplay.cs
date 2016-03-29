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

namespace SAHL.Web.Views.Common.Presenters.LegalEntityDetails
{
    /// <summary>
    /// Default display presenter.
    /// </summary>
    public class LegalEntityDetailsDisplay : LegalEntityDetailsDisplayBase
    {
        /// <summary>
        /// Presenter Constructor. Takes the <see cref="ILegalEntityDetails">view</see> and the <see cref="SAHLCommonBaseController">Controller</see> as parameters.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityDetailsDisplay(ILegalEntityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            _view.InsurableInterestDisplayVisible = false;
            _view.InsurableInterestUpdateVisible= false;

        }
    }
}
