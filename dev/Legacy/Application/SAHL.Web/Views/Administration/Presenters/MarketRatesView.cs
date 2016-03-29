using System;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Administration.Presenters
{
	public class MarketRatesView : MarketRatesBase
	{
		public MarketRatesView(IMarketRates view, SAHLCommonBaseController Controller)
			: base(view, Controller)
		{
		}

		#region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
		{
			base.OnViewPreRender(sender, e);
			if (!View.ShouldRunPage)
				return;

			_view.SubmitButtonVisible = false;
			_view.CancelButtonVisible = false;
			_view.txtMarketRateValueVisible = false;
			_view.lblMarketRateValueVisible = true;
		}

		#endregion

		#region Methods

		#endregion

	}
}
