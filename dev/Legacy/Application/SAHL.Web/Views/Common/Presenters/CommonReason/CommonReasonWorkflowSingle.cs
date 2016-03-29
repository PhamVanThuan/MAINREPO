using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
	/// <summary>
	/// Common Reason
	/// </summary>
	public class CommonReasonWorkflowSingle : CommonReasonBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public CommonReasonWorkflowSingle(ICommonReason view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			if (_reasonRepo == null)
			{
				_reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
			}

			if (GlobalCacheData.ContainsKey(ViewConstants.ReasonHistoryTitle))
			{
				_view.SetHistoryPanelGroupingText = GlobalCacheData[ViewConstants.ReasonHistoryTitle].ToString();
			}

			if (GlobalCacheData.ContainsKey(ViewConstants.GenericKey))
			{
				GenericKey = int.Parse(GlobalCacheData[ViewConstants.GenericKey].ToString());
				if (GlobalCacheData.ContainsKey(ViewConstants.ReasonTypeKey))
				{
					int reasonTypeKey = int.Parse(GlobalCacheData[ViewConstants.ReasonTypeKey].ToString());
					IReadOnlyEventList<IReason> _reasons = _reasonRepo.GetReasonByGenericKeyAndReasonTypeKey(GenericKey, reasonTypeKey);

					//Show reasons if they exist
					if (_reasons != null && _reasons.Count > 0)
					{
						_view.HistoryDisplay = true;
						_view.ShowHistoryPanel = true;
						_view.ShowUpdatePanel = false;
						_view.ShowSubmitButtons = true;
						_view.CancelButtonVisible = true;
						_view.SubmitButtonVisible = false;

						_view.BindReasonHistoryGrid(_reasons);

						_view.CancelButtonText = "Done";
					}
				}
			}

			_view.OnlyOneReasonCanBeSelected = true;

			base.OnViewInitialised(sender, e);
		}

		/// <summary>
		/// On Submit Button Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
		{
			if (GlobalCacheData.ContainsKey(ViewConstants.GenericKey))
			{
				GenericKey = int.Parse(GlobalCacheData[ViewConstants.GenericKey].ToString());
			}
			base._view_OnSubmitButtonClicked(sender, e);

			//We should actually have a return value for OnSubmitClick to determine whether something went wrong, so that we don't navigate away
			if (View.IsValid)
			{
				CompleteActivityAndNavigate();
			}
		}

		/// <summary>
		///  Complete Activity and Navigate
		/// </summary>
		public override void CompleteActivityAndNavigate()
		{
			try
			{               
				SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
				X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
                if (base.sdsdgKeys.Count > 0)
                {
                    UpdateReasonsWithStageTransitionKey();
                }
				this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
			}
			catch (Exception)
			{
				// we must cancel the activity here, otherwise if the user navigates to another node and
				// tries to perform a workflow action, X2 may try to perform the action on the wrong
				// activity
				if (_view.IsValid)
				{
					this.X2Service.CancelActivity(_view.CurrentPrincipal);
					throw;
				}
			}
		}

		/// <summary>
		/// Cancel Activity
		/// </summary>
		public override void CancelActivity()
		{
			if (GlobalCacheData.ContainsKey(ViewConstants.CancelView))
			{
				Navigator.Navigate(GlobalCacheData[ViewConstants.CancelView].ToString());
			}
            else
                Navigator.Navigate(ViewConstants.CancelView);
		}
	}
}