using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.CacheData;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
	/// <summary>
	/// Payment Distribution Agent
	/// </summary>
	public class PaymentDistributionAgentUpdate : ExternalRoleUpdateBase
	{
		private IList<ICacheObjectLifeTime> _lifeTimes;
		public IList<ICacheObjectLifeTime> LifeTimes
		{
			get
			{
				if (_lifeTimes == null)
				{
					List<string> views = new List<string>();
					views.Add("PDAUpdateSelect");
					_lifeTimes = new List<ICacheObjectLifeTime>();
					_lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
				}

				return _lifeTimes;
			}
		}

		/// <summary>
		/// Constructor for Payment Distribution Agent
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public PaymentDistributionAgentUpdate(IExternalRoleUpdate view, SAHLCommonBaseController controller)
			: base(view, controller)
		{

		}

		/// <summary>
		/// On View Initialized
		/// </summary>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage)
				return;

			_view.RoleType = ExternalRoleTypes.PaymentDistributionAgent;
			_view.GridHeader = "Update Payment Distribution Agent";

			if (GlobalCacheData.ContainsKey(ViewConstants.SelectedTreeNodeKey))
				GlobalCacheData.Remove(ViewConstants.SelectedTreeNodeKey);

			if (DebtCounselling.PaymentDistributionAgent != null)
			{
				GlobalCacheData.Add(ViewConstants.SelectedTreeNodeKey, DebtCounselling.PaymentDistributionAgentLEOrganisationStructure.Key, LifeTimes);
			}

			if (GlobalCacheData.ContainsKey(ViewConstants.PaymentDistributionAgenctLegalEntityKey))
			{
				_view.NewLegalEntity = LERepo.GetLegalEntityByKey((int)GlobalCacheData[ViewConstants.PaymentDistributionAgenctLegalEntityKey]);
			}
		}

		/// <summary>
		/// On View Loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewLoaded(object sender, EventArgs e)
		{
			base.OnViewLoaded(sender, e);
			if (!_view.ShouldRunPage)
				return;
		}
	}
}