using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    //public class CommonReasonDCCancelled : CommonReasonBase
    //{
    //      /// <summary>
    //    /// Constructor for CommonReasonDCCancelled
    //    /// </summary>
    //    /// <param name="view"></param>
    //    /// <param name="controller"></param>
    //    public CommonReasonDCCancelled(ICommonReason view, SAHLCommonBaseController controller)
    //        : base(view, controller)
    //    {
    //    }
    //    /// <summary>
    //    /// OnViewInitialised event
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    protected override void OnViewInitialised(object sender, EventArgs e)
    //    {
    //        base.OnViewInitialised(sender, e);

    //        if (!_view.ShouldRunPage) 
    //            return;

    //        _view.SetHiddenIndText = "1";
    //        _view.CancelButtonVisible = true;

    //        // limit the selection to one reason
    //        _view.OnlyOneReasonCanBeSelected = true;
    //    }

    //    /// <summary>
    //    /// Overrides the base OnSubmitButtonClicked event so that specific actions can be performed
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
    //    {
    //        _selectedReasons = (List<SelectedReason>)e.Key;

    //        if (_selectedReasons.Count <= 0)
    //        {
    //            string errorMessage = "Must select at least one reason";
    //            _view.Messages.Add(new Error(errorMessage, errorMessage));
    //        }

    //        if (_view.IsValid && _selectedReasons.Count > 0) // only save if reason has been selected
    //        {      
    //            base._view_OnSubmitButtonClicked(sender, e);

    //            TransactionScope tx = new TransactionScope();
    //            try
    //            {
    //                CompleteActivityAndNavigate();
    //            }
    //            catch (Exception)
    //            {
    //                tx.VoteRollBack();
    //                if (_view.IsValid)
    //                    throw;
    //            }
    //            finally
    //            {
    //                tx.Dispose();
    //            }

    //        }
    //    }

    //    public override void CancelActivity()
    //    {
    //        base.X2Service.CancelActivity(_view.CurrentPrincipal);
    //        base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
    //    }


    //    public override void CompleteActivityAndNavigate()
    //    {
    //        SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
    //        X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
    //        if (base.sdsdgKeys.Count > 0)
    //        {
    //            UpdateReasonsWithStageTransitionKey();
    //        }
    //        base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
    //    }
    //}
}