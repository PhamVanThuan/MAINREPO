using System;
using System.Collections.Generic;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.CacheData;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Web.Views.Common.Presenters
{
	/// <summary>
	/// 
	/// </summary>
	public class ClientSuperSearchForHelpDeskAdd : ClientSuperSearch
	{

		private IX2Info XI; // = null;

		/// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
		public ClientSuperSearchForHelpDeskAdd(IClientSuperSearch view, SAHLCommonBaseController controller)
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
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // setup the buttons
            _view.CancelButtonVisible = true;
            _view.CreateNewClientButtonVisible = false;
            _view.CreateNewClientButtonText = "Select";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        protected override void OnClientSelectedClicked(object sender, ClientSuperSearchSelectedEventArgs ea)
        {
			if (ea != null)
			{
				Int64 instanceID = CreateHelpDeskWorkflow(ea.LegalEntityKey);
				if (instanceID > 0)
				{
					// add the instanceID to the global cache for our redirect view to use
					GlobalCacheData.Remove(ViewConstants.InstanceID);
					GlobalCacheData.Add(ViewConstants.InstanceID, instanceID, new List<ICacheObjectLifeTime>());
					// navigate to the workflow redirect view
					Navigator.Navigate("X2InstanceRedirect");
				}
			}
        }

		private Int64 CreateHelpDeskWorkflow(int legalEntityKey)
		{
			try
			{
				// once we have an application create a workflow case
				Dictionary<string, string> Inputs = new Dictionary<string, string>();
				Inputs.Add("LegalEntityKey", legalEntityKey.ToString());
				Inputs.Add("CurrentConsultant", _view.CurrentPrincipal.Identity.Name);
				XI = X2Service.GetX2Info(_view.CurrentPrincipal);

				if (XI == null || String.IsNullOrEmpty(XI.SessionID))
					X2Service.LogIn(_view.CurrentPrincipal);

				X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowProcessName.HelpDesk, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.HelpDesk, SAHL.Common.Constants.WorkFlowActivityName.HelpDeskCreate, Inputs, false);
				X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, false, null);
				return XI.InstanceID;
			}

			catch (Exception)
			{
				if (XI != null)
				{
					if (XI.InstanceID > 0)
					{
						X2Service.CancelActivity(_view.CurrentPrincipal);
					}
				}
				return 0;
			}
		}
	}
}
