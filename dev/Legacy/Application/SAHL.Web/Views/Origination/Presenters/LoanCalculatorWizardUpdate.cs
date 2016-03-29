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

namespace SAHL.Web.Views.Origination.Presenters
{
    public class LoanCalculatorWizardUpdate : LoanCalculatorBase
    {
        private int _applicationKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LoanCalculatorWizardUpdate(SAHL.Web.Views.Origination.Interfaces.ILoanCalculator view, SAHLCommonBaseController controller)
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

            //get application Key out of the global cache
            
            _applicationKey = Convert.ToInt32(0);
            //_applicationKey = 347897;
            _app = AppRepo.GetApplicationByKey(_applicationKey);

            throw new NotImplementedException("Need to get the application key out of the global cache");

            //TODO: application must be product new variable loan

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnCreateApplicationButtonClicked += new EventHandler(_view_OnCreateApplicationButtonClicked);
            _view.CreateApplicationButtonText = "Next";
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
