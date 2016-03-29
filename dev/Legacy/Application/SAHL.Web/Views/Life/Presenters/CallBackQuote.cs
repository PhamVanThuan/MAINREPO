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
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Collections;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service;

namespace SAHL.Web.Views.Life.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CallBackQuote : CallBack
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CallBackQuote(SAHL.Web.Views.Life.Interfaces.ICallBack view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            if (!_view.ShouldRunPage)
                return;

            // Set the callback reason to be 'Quoted'
            CallbackReasonDescription = "Quoted";

            base.OnViewInitialised(sender, e);
        }

        protected override void OnCancelButtonClicked(object sender, EventArgs e)
        {
            //Cancel the workflow activity 
            X2Service.CancelActivity(_view.CurrentPrincipal);
            //Navigate
            X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);
        }

        protected override void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            base.CreateCallBack();

            if (_view.IsValid)
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    // complete the x2 activity 
                    X2Service svc = ServiceFactory.GetService<IX2Service>() as X2Service;
                    svc.CompleteActivity(_view.CurrentPrincipal, null, false);

                    //Navigate
                    X2Service.WorkflowNavigate(_view.CurrentPrincipal, Navigator);

                    txn.VoteCommit();
                }
                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }
                finally
                {
                    txn.Dispose();
                }
            }
        }
    }
}
