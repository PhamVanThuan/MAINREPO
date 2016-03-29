using Castle.ActiveRecord;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Correspondence.Interfaces;
using SAHL.Web.Views.Correspondence.Presenters.Correspondence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.Correspondence.Presenters
{
    public class CorrespondenceProcessingLegalEntity: CorrespondenceProcessingBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CorrespondenceProcessingLegalEntity(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
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

            // set properties
            _view.ShowLifeWorkFlowHeader = false;
            _view.ShowCancelButton = true;

            // setting this to true will cause datastor record to be stored in the context of a legal entity and not an account
            _view.LegalEntityCorrespondence = true;

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

            
                ts.VoteCommit();

                _view.Navigator.Navigate("Submit");

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