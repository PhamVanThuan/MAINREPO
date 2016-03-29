using System;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration
{
	/// <summary>
	/// Market Rates
	/// </summary>
	public partial class MarketRates : SAHLCommonBaseView, IMarketRates
	{
		#region Properties

		#endregion

		#region Members

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.SubmitButtonVisible">IMarketRates.SubmitButtonVisible</see>.
		/// </summary>
		public bool SubmitButtonVisible
		{
			set { SubmitButton.Visible = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.SubmitButtonVisible">IMarketRates.SubmitButtonEnabled</see>.
		/// </summary>
		public bool SubmitButtonEnabled
		{
			set { SubmitButton.Enabled = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.CancelButtonVisible">IMarketRates.CancelButtonVisible</see>.
		/// </summary>
		public bool CancelButtonVisible
		{
			set { CancelButton.Visible = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.txtMarketRateValueVisible">IMarketRates.txtMarketRateValueVisible</see>.
		/// </summary>
		public bool txtMarketRateValueVisible
		{
			set { txtMarketRateValue.Visible = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.txtMarketRateValueEnabled">IMarketRates.txtMarketRateValueEnabled</see>.
		/// </summary>
		public bool txtMarketRateValueEnabled
		{
			set { txtMarketRateValue.Enabled = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.lblMarketRateValueVisible">IMarketRates.lblMarketRateValueVisible</see>.
		/// </summary>
		public bool lblMarketRateValueVisible
		{
			set { lblMarketRateValue.Visible = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.lblMarketRateKeyText">IMarketRates.lblMarketRateKeyText</see>.
		/// </summary>
		public string lblMarketRateKeyText
		{
			set { lblMarketRateKey.Text = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.lblMarketRateKeyText">IMarketRates.lblMarketRateDescriptionText</see>.
		/// </summary>
		public string lblMarketRateDescriptionText
		{
			set { lblMarketRateDescription.Text = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.lblMarketRateKeyText">IMarketRates.lblMarketRateValueText</see>.
		/// </summary>
		public string lblMarketRateValueText
		{
			set { lblMarketRateValue.Text = value; }
		}

		/// <summary>
		/// Implements <see cref="SAHL.Web.Views.Administration.Interfaces.IMarketRates.MarketRateValue">IMarketRates.MarketRateValue</see>.
		/// </summary>
		public double? MarketRateValue
		{
            get
            {
                double dOut;
                if (Double.TryParse(txtMarketRateValue.Text, out dOut))
                    return new double?(dOut);
                else
                    return new double?();
            }
            set
            {
                if (value.HasValue)
                {
                    txtMarketRateValue.Text = value.Value.ToString(SAHL.Common.Constants.NumberFormat3Decimal);
                }
                else
                {
                    txtMarketRateValue.Text = "";
                }
            }
		}

        /// <summary>
        /// Gets the market rate key selected on the grid.
        /// </summary>
        public int SelectedMarketRateKey
        {
            get
            {
                return Convert.ToInt32(MarketRateGrid.Rows[MarketRateGrid.SelectedIndex].Cells[0].Text);

            }
        }


		#endregion

		#region EventHandlers

		public event EventHandler CancelClick;

		public event EventHandler SubmitClick;

		public event EventHandler MarketRateSelected;

		#endregion

		#region Events

		/// <summary>
		/// Cancel button click event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Cancel_Click(object sender, EventArgs e)
		{
			if (CancelClick != null)
				CancelClick(sender, e);
		}

		/// <summary>
		/// Submit button click event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Submit_Click(object sender, EventArgs e)
		{
			if (SubmitClick != null)
				SubmitClick(sender, e);
		}

		/// <summary>
		/// MarketRateGrid selected event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void MarketRateGrid_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (MarketRateGrid.SelectedIndexInternal > 0)
			{
				if (MarketRateSelected != null)
					MarketRateSelected(sender, new GridRowSelectEventArgs(SelectedMarketRateKey));
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Get Market Rates
		/// </summary>
		/// <param name="marketRates"></param>
		public void BindMarketRatesGrid(IReadOnlyEventList<IMarketRate> marketRates)
		{
			MarketRateGrid.Columns.Clear();
			MarketRateGrid.AddGridBoundColumn("Key", "Rate Key", Unit.Percentage(10), HorizontalAlign.Left, true);
			MarketRateGrid.AddGridBoundColumn("Description", "Description", Unit.Percentage(60), HorizontalAlign.Left, true);
			MarketRateGrid.AddGridBoundColumn("Value", SAHL.Common.Constants.RateFormat3Decimal, GridFormatType.GridNumber, "Value", false, Unit.Percentage(30), HorizontalAlign.Left, true);
			MarketRateGrid.DataSource = marketRates;
			MarketRateGrid.DataBind();
		}

		#endregion
	}
}
