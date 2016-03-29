using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Administration.Interfaces
{
	public interface IMarketRates : IViewBase
	{
		#region Properties

		#endregion

		#region Members

		/// <summary>
		/// Sets whether the SubmitButton control should be Visible.
		/// </summary>
		bool SubmitButtonVisible { set; }

		/// <summary>
		/// Sets whether the SubmitButton control should be Enabled.
		/// </summary>
		bool SubmitButtonEnabled { set; }

		/// <summary>
		/// Sets whether the CancelButton control should be Visible.
		/// </summary>
		bool CancelButtonVisible { set; }

		/// <summary>
		/// Sets whether the txtMarketRateValue control should be Visible.
		/// </summary>
		bool txtMarketRateValueVisible { set; }

		/// <summary>
		/// Sets whether the txtMarketRateValue control should be Enabled.
		/// </summary>
		bool txtMarketRateValueEnabled { set; }

		/// <summary>
		/// Sets whether the lblMarketRateValue control should be Visible.
		/// </summary>
		bool lblMarketRateValueVisible { set; }

        ///// <summary>
        ///// Gets the SelectedValue value of the MarketRateGrid control.
        ///// </summary>
		//string MarketRateGridSelectedValue { get; }

		/// <summary>
		/// Sets the Text value of the lblMarketRateKey control.
		/// </summary>
		string lblMarketRateKeyText { set; }

		/// <summary>
		/// Sets the Text value of the lblMarketRateDescription control.
		/// </summary>
		string lblMarketRateDescriptionText { set; }

		/// <summary>
		/// Sets the Text value of the lblMarketRateValue control.
		/// </summary>
		string lblMarketRateValueText { set; }

		/// <summary>
		/// Gets/sets the market rate value that can be edited.
		/// </summary>
        double? MarketRateValue { get; set; }

        /// <summary>
        /// Gets the market rate key selected on the grid.
        /// </summary>
        int SelectedMarketRateKey { get; }

		#endregion

		#region EventHandlers

		#endregion

		#region Events

		/// <summary>
		/// 
		/// </summary>
		event EventHandler CancelClick;

		/// <summary>
		/// 
		/// </summary>
		event EventHandler SubmitClick;

		/// <summary>
		/// 
		/// </summary>
		event EventHandler MarketRateSelected;

		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		void BindMarketRatesGrid(IReadOnlyEventList<IMarketRate> marketRates);

		#endregion
	}
}
