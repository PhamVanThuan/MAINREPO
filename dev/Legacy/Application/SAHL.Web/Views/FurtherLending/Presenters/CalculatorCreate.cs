using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.UI;

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class CalculatorCreate : CalculatorBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CalculatorCreate(IFurtherLendingCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnGenerateButtonClicked += new EventHandler(_view_GenerateButtonClicked);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClickedNoActivity);

            //need extra check for NTU's if there is no message already
            if (_view.ExistingApplicationMessage.Length == 0)
                base.CheckOpenX2NTU();
        }
    }
}
