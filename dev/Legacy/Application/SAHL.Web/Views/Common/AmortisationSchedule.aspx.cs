using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Controls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Globals;
using SAHL.Common.DataSets;

namespace SAHL.Web.Views.Common
{
    public partial class AmortisationSchedule : SAHLCommonBaseView,IAmortisationSchedule
    {
        private double _currentBalanceV;
        private double _interestRateV;
        private double _instalmentTotalV;
        private int _remainingTermV;

        private double _currentBalanceF;
        private double _interestRateF;
        private double _instalmentTotalF;
        private int _remainingTermF;
        private bool _displayLoanValues;
        private bool _displayFixedAndVariableGrids;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            lblInitialBalance.Text = _currentBalanceV.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblInterestRate.Text = _interestRateV.ToString(SAHL.Common.Constants.RateFormat);
            lblTerm.Text = _remainingTermV.ToString();

            trValues1.Visible = _displayLoanValues;
            trValues2.Visible = _displayLoanValues;
            trValues3.Visible = _displayLoanValues;

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            OnBackClicked(sender, e);
        }

        public void BindAmortisationGrid(int finServTypeKey, LoanCalculations.AmortisationScheduleDataTable dt)
        {
            SAHL.Common.Web.UI.Controls.DXGridView grd = null;

            if (finServTypeKey == (int)FinancialServiceTypes.VariableLoan)
                grd = grdVariableAmortisationSchedule;

            if (finServTypeKey == (int)FinancialServiceTypes.FixedLoan)
                grd = grdFixedAmortisationSchedule;

            if (grd != null)
            {
                grd.Visible = true;
                SetupGrid(grd);
                grd.DataSource = dt;
                grd.DataBind();

                // if we are displaying bith grids then decrease the page size to 8 so bothr grids fit on the screen
                if (_displayFixedAndVariableGrids) 
                    grd.SettingsPager.PageSize = 8;
                else
                    grd.SettingsPager.PageSize = 20;
            }
        }

        private static void AddGridColumn(SAHL.Common.Web.UI.Controls.DXGridView grd, string fieldName, string caption, GridFormatType formatType, string format, HorizontalAlign align, bool visible)
        {
            DXGridViewFormattedTextColumn col = new DXGridViewFormattedTextColumn();
            col.FieldName = fieldName;
            col.Caption = caption;
            //col.Width = Unit.Percentage(width);
            col.Format = formatType;
            if (!String.IsNullOrEmpty(format))
                col.FormatString = format;
            col.Visible = visible;
            col.CellStyle.HorizontalAlign = align;
            grd.Columns.Add(col);
        }

        private static void SetupGrid(SAHL.Common.Web.UI.Controls.DXGridView grd)
        {
            AddGridColumn(grd, "Period", "Period (yrs)", GridFormatType.GridString, null, HorizontalAlign.Right, true);
            AddGridColumn(grd, "Opening", "Opening Balance", GridFormatType.GridCurrency, SAHL.Common.Constants.CurrencyFormat, HorizontalAlign.Left, true);
            AddGridColumn(grd, "Payment", "Instalment", GridFormatType.GridCurrency, SAHL.Common.Constants.CurrencyFormat, HorizontalAlign.Left, true);
            AddGridColumn(grd, "Interest", "Interest", GridFormatType.GridCurrency, SAHL.Common.Constants.CurrencyFormat, HorizontalAlign.Left, true);
            AddGridColumn(grd, "Capital", "Capital", GridFormatType.GridCurrency, SAHL.Common.Constants.CurrencyFormat, HorizontalAlign.Center, true);
            AddGridColumn(grd, "Closing", "Closing Balance", GridFormatType.GridCurrency, SAHL.Common.Constants.CurrencyFormat, HorizontalAlign.Center, true);
        }

        #region IAmortisationSchedule Members


        public event EventHandler OnBackClicked;

        public double CurrentBalanceV
        {
            get { return _currentBalanceV; }
            set { _currentBalanceV = value; }
        }

        public double InterestRateV
        {
            get { return _interestRateV; }
            set { _interestRateV = value; }
        }


        public double InstalmentTotalV
        {
            get { return _instalmentTotalV; }
            set { _instalmentTotalV = value; }
        }

        public int RemainingTermV
        {
            get { return _remainingTermV; }
            set { _remainingTermV = value; }
        }

        public double CurrentBalanceF
        {
            get { return _currentBalanceF; }
            set { _currentBalanceF = value; }
        }

        public double InterestRateF
        {
            get { return _interestRateF; }
            set { _interestRateF = value; }
        }

        public double InstalmentTotalF
        {
            get { return _instalmentTotalF; }
            set { _instalmentTotalF = value; }
        }

        public int RemainingTermF
        {
            get { return _remainingTermF; }
            set { _remainingTermF = value; }
        }

        public bool DisplayLoanValues
        {
            set { _displayLoanValues = value; }
        }

        public bool DisplayFixedAndVariableGrids
        {
            set { _displayFixedAndVariableGrids = value; }
        }
        #endregion
    }
}
