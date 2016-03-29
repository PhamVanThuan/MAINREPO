using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common;
using SAHL.Common.Exceptions;
using SAHL.X2.Framework.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.CacheData;


namespace SAHL.Web.Views.Common.Presenters
{
	public class WorkflowYesNoPresenter : SAHLCommonBasePresenter<IWorkFlowConfirm>
	{
		public WorkflowYesNoPresenter(IWorkFlowConfirm view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage) return;

			base._view.OnYesButtonClicked += new EventHandler(_view_OnYesButtonClicked);
			base._view.OnNoButtonClicked += new EventHandler(_view_OnNoButtonClicked);

			base._view.ShowControls(true);
			IX2Info info = X2Service.GetX2Info(_view.CurrentPrincipal);

			if (info != null && !String.IsNullOrEmpty(info.ActivityName))
				_view.TitleText = String.Format("Confirm {0}", info.ActivityName);
			else
				_view.TitleText = "Confirm Activity";
		}

		void _view_OnNoButtonClicked(object sender, EventArgs e)
		{
			//cancel activity
			X2Service.CancelActivity(_view.CurrentPrincipal);
			//Navigate();
			this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
		}

		void _view_OnYesButtonClicked(object sender, EventArgs e)
		{
			SAHLPrincipalCache spc = null;
			try
			{
				spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
				this.X2Service.CompleteActivity(_view.CurrentPrincipal, null, spc.IgnoreWarnings);
				this.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
			}
			catch (Exception ex)
			{
				var messages = spc.DomainMessages;
				foreach (var message in messages)
				{
					System.Diagnostics.Trace.WriteLine(String.Format("{0} - {1}", message.Message, message.MessageType.ToString()));
				}
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
	}
}
