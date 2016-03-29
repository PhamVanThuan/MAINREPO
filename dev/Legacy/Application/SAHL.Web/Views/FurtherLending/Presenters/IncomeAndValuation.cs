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
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class IncomeAndValuation : SAHLCommonBasePresenter<IIncomeAndValuation>
    {
        private InstanceNode _node;

        // TODO: Use or remove unused attribute
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "This needs to be either removed or fixed!")]
        private bool _IncomeChecked;

        // TODO: Use or remove unused attribute
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "This needs to be either removed or fixed!")]
        private bool _ValuationChecked;

        private string _IncomeCheckedString = "Income";
        private string _ValuationCheckedString = "Valuation";

        public IncomeAndValuation(IIncomeAndValuation view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Set the CBO Node manually for testing purposes : NB !!! MUST BE REMOVED ONCE CBO IS WORKING
            InstanceNode IN = new InstanceNode(0, null, "A Sample Origination Offer Node", "A Sample Origination Offer Node", 36826, null);
            _node = IN;

            // Get the CBO Node  -- uncomment the line below once the cbo is working     
            //_node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            // Get the FurtherLending Data to check if Income/Valuations screen has been completed
            Dictionary<string, object> DC = _node.X2Data as Dictionary<string, object>;
            _IncomeChecked = DC[_IncomeCheckedString] == null ? false : Convert.ToBoolean(DC[_IncomeCheckedString]);
            _ValuationChecked = DC[_ValuationCheckedString] == null ? false : Convert.ToBoolean(DC[_ValuationCheckedString]); 
        }


        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
        }

        /// <summary>
        /// Handles the event fired by the view when the Submit button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
            
            if (svc.IsViewDefaultFormForState(_view.CurrentPrincipal, _view.ViewName))
            {
                //save the valuation/income check states to x2 variables
                Dictionary<string, string> dic = new Dictionary<string, string>();

                dic.Add(_IncomeCheckedString, _view.IncomeChecked.ToString());
                dic.Add(_ValuationCheckedString, _view.ValuationChecked.ToString());
                
                svc.CompleteActivity(_view.CurrentPrincipal, dic, false);

                // Navigate to the next State by performing the activity
                svc.WorkFlowWizardNext(_view.CurrentPrincipal, "", _view.Navigator);
            }
            else // Form - navigate back to the calling page
            {
                svc.WorkFlowWizardNext(_view.CurrentPrincipal, "", _view.Navigator);
            }
        }
    }
}
