using System;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Administration.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Views.Administration.Presenters
{
	public class MarketRatesEdit : MarketRatesBase
	{
		public MarketRatesEdit(IMarketRates view, SAHLCommonBaseController Controller)
			: base(view, Controller)
		{
		}

		#region Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void OnViewInitialised(object sender, EventArgs e)
		{
			base.OnViewInitialised(sender, e);
			if (!_view.ShouldRunPage)
				return;

			_view.CancelClick += new EventHandler(CancelClick);
			_view.SubmitClick += new EventHandler(SubmitClick);
		}

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

			_view.SubmitButtonVisible = true;
			_view.CancelButtonVisible = true;
			_view.txtMarketRateValueVisible = true;
			_view.lblMarketRateValueVisible = false;
		}

		protected void CancelClick(object sender, EventArgs e)
		{
			_view.Navigator.Navigate("MarketRatesView");
		}

		protected void SubmitClick(object sender, EventArgs e)
		{
			int marketRateKey = _view.SelectedMarketRateKey;

			if (marketRateKey > 0)
			{
                TransactionScope txn = new TransactionScope();
                try
                {

                    IMarketRate marketRate = MarketRateRepository.GetMarketRateByKey(marketRateKey);
                    double? newMarketRateValue = _view.MarketRateValue;
                    if (newMarketRateValue.HasValue)
                    {
                        if (Math.Round(newMarketRateValue.Value / 100, 5) != Math.Round(marketRate.Value, 5))
                        {
                            double oldval = marketRate.Value;
                            marketRate.Value = Math.Round(newMarketRateValue.Value / 100, 5);
                            if (MarketRateRepository.UpdateMarketRate(marketRate, oldval, _view.CurrentPrincipal.Identity.Name))
                            {
                                //update success
                                txn.VoteCommit();
                                LookupRepository.ResetLookup(LookupKeys.MarketRates);
                            }
                            else
                            {
                                //update fail
                                txn.VoteRollBack();
                            }
                        }
                        else
                        {
                            //Value not changed
                        }
                    }
                    else
                    {
                        //Changed value is null or ""
                    }
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
			else
			{
				//Nothing selected
			}
			_view.Navigator.Navigate("MarketRatesView");
		}

		#endregion

		#region Methods

		#endregion
	}
}
