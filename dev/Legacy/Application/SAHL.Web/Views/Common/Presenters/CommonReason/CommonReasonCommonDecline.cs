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
using System.Collections.Generic;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Exceptions;
using Castle.ActiveRecord;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    public class CommonReasonCommonDecline : CommonReasonBase
    {

        public CommonReasonCommonDecline(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            //_view.OnSubmitButtonClicked +=new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnSubmitButtonClicked);
        }


        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            // write away the reason data
            base._view_OnSubmitButtonClicked(sender, e);

            if (_view.Messages.Count > 0)
                return;

            CompleteActivityAndNavigate();
        }

        public override void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        public override void CompleteActivityAndNavigate()
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
                if (base.sdsdgKeys.Count > 0)
                {
                    UpdateReasonsWithStageTransitionKey();
                }
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
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
