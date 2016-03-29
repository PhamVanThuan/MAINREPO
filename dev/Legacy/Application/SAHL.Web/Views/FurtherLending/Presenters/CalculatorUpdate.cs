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

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class CalculatorUpdate : CalculatorBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CalculatorUpdate(IFurtherLendingCalculator view, SAHLCommonBaseController controller)
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
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.CanUpdate = true;

            // if we are reworking an application determone if we are a helpdesk user as they will have different correspondnce options to FL
           if (_view.CurrentPrincipal.IsInRole("HelpDesk"))
               _view.HelpDeskRework = true;
            else
               _view.HelpDeskRework = false;
        }
    }
}
