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
using SAHL.X2.Framework.Interfaces;
using Castle.ActiveRecord;
using System.Collections.Generic;

namespace SAHL.Web.Views.Correspondence.Presenters.Correspondence
{
	public class CorrespondenceProcessingMultipleDebtCounsellor : CorrespondenceProcessingBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
		public CorrespondenceProcessingMultipleDebtCounsellor(ICorrespondenceProcessing view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
			_getRootContextNode = true;
        }

		/// <summary>
		/// Hook the events fired by the view and call relevant methods to bind control data
		/// </summary>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			//set the the screen to handle Multiple Recipients
			_view.MultipleRecipientMode = true;
			_view.DisplayDebtCounsellors = true;

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
			_view.Navigator.Navigate("Submit");
		}
    }
}
