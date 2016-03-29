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
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    ///// <summary>
    ///// 
    ///// </summary>
    //public class CommonReasonProcessorQuery : CommonReasonBase
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="view"></param>
    //    /// <param name="controller"></param>
    //    public CommonReasonProcessorQuery(ICommonReason view, SAHLCommonBaseController controller)
    //        : base (view, controller)
    //        {
    //        }


    //    protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
    //    {
    //        base._view_OnSubmitButtonClicked(sender, e);
    //        CompleteActivityAndNavigate();
    //    }

    //    public override void CancelActivity()
    //    {
    //        base.X2Service.CancelActivity(_view.CurrentPrincipal);
    //        base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);

    //    }

    //    public override void CompleteActivityAndNavigate()
    //    {
    //        X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
    //        if (base.sdsdgKeys.Count > 0)
    //        {
    //            UpdateReasonsWithStageTransitionKey();
    //        } 
    //        base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
    //    }
    //}
}
