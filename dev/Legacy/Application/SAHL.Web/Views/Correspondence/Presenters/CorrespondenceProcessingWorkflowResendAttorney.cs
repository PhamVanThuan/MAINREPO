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
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
    public class CorrespondenceProcessingWorkflowResendAttorney : CorrespondenceProcessingBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceProcessingWorkflowResendAttorney(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.DisplayAttorneyRole = true;
            _view.SupressConfirmationMessage = true;

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
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            _view.DisableCorrespondenceOptionEntry = true;
            _view.SetEmailOptionChecked = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Orig_ApplicationSummaryRedirect");
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
                /*
                (Dictionary<string, string> inputFields = new Dictionary<string, string>();
                if (_view.Messages.ErrorMessages.Count == 0)
                {
                    X2Service.CompleteActivity(_view.CurrentPrincipal, inputFields, false);
                    X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
                }
                */
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

            _view.Navigator.Navigate("Orig_ApplicationSummaryRedirect");
        }
    }
}
