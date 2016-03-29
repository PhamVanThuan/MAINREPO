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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.UI;
using SAHL.X2.Framework.Interfaces;


namespace SAHL.Web.Views.Origination.Presenters
{
    public class LoanCalculatorLead : LoanCalculatorBase
    {
        private int _applicationKey;
        private CBONode _cboNode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanCalculatorLead(SAHL.Web.Views.Origination.Interfaces.ILoanCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            //// Get the CBO Node  -- uncomment the line below once the cbo is working     
            _cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);
            if (_cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _applicationKey = Convert.ToInt32(_cboNode.GenericKey);
            //_applicationKey = 347897;
            _app = AppRepo.GetApplicationByKey(_applicationKey);

            //_view.OnCalculateButtonClicked += new EventHandler(_view_OnCalculateButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnWorkFlowCancelButtonClicked);
            _view.OnCreateApplicationButtonClicked += new EventHandler(_view_OnCreateApplicationButtonClicked);

            _view.HideEstateAgent = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnCreateApplicationButtonClicked(object sender, EventArgs e)
        {
            SaveAndMoveOn();
        }
    }
}
