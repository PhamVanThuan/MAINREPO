using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;
using SAHL.Web.Views.Correspondence.Presenters.Correspondence;
using System.Collections.Generic;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    /// <summary>
    /// 
    /// </summary>
    public class CorrespondenceProcessingMultipleWorkflow : CorrespondenceProcessingBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceProcessingMultipleWorkflow(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            //set the the screen to handle Multiple Recipients
            _view.MultipleRecipientMode = true;

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // set properties
            _view.ShowLifeWorkFlowHeader = false;
            _view.ShowCancelButton = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCancelButtonClicked(object sender, EventArgs e)
        {
            //Cancel the workflow activity 
            IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
            if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                X2Service.LogIn(_view.CurrentPrincipal);

            X2Service.CancelActivity(_view.CurrentPrincipal);
            //Navigate
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnSendButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();

            try
            {
                // render and save the correspondence in the background.
                base.OnSendButtonClicked(sender, e);

                // this will check if a rule error/warning has been thrown in the base class
                if (!_view.IsValid)
                    return;

                // Complete the activity and to the next State
                // If this is the default view for the state then it will navigate to the X2WorkList
                // otherwise it will navigate back to the view for the current state
                Dictionary<string, string> inputFields = new Dictionary<string, string>();
                if (_view.Messages.ErrorMessages.Count == 0)
                {
                    //X2Service.CancelActivity(_view.CurrentPrincipal);
                    X2Service.CompleteActivity(_view.CurrentPrincipal, inputFields, false);
                    X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }

                ts.VoteCommit();
            }
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                ts.Dispose();
            }
        }
    }
}
