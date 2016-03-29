using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using AjaxControlToolkit;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Displays and returns information about the Loan. Use the InterestOnly property to enable Interest Only features.
    /// </summary>
    public class VarifixDetailsPanel : Panel, INamingContainer
    {

        #region Misc private variables
        private string _titleText = "Varifix Details";
        private HtmlTable _htmlTable;
        private IApplicationProductVariFixLoan _applicationProductVariFixLoan;
        private IApplication _application;
        private bool _isDiscountReadOnly;

        #endregion

        #region Header controls
        private SAHLCurrencyBox _txtFixedPercentage;
        private CheckBox _chkReset5yr;
        //private CheckBox _chkReset6month;
        private SAHLLabel _lblLinkRate;
        private SAHLLabel _lblFixedPercentage;
        private SAHLButton _btnUseMaximum;

        #endregion

        #region DiscountControls

        private HtmlTableRow _rowDiscountInput;
        private NumericUpDownExtender _nudDiscount;
        private SAHLTextBox _txtDiscount;

        #endregion

        #region Varifix Information Declarations
        private SAHLLabel _lblPercentageFixed;
        private SAHLLabel _lblRandValueFixed;
        private SAHLLabel _lblMarketRatePercentageFixed;
        private SAHLLabel _lblLinkRateFixed;
        private SAHLLabel _lblEffectiveRateFixed;
        private SAHLLabel _lblInstalmentFixed;
        private SAHLLabel _lblPercentageVariable;
        private SAHLLabel _lblRandValueVariable;
        private SAHLLabel _lblMarketRatePercentageVariable;
        private SAHLLabel _lblLinkRateVariable;
        private SAHLLabel _lblEffectiveRateVariable;
        private SAHLLabel _lblInstalmentVariable;


        private SAHLLabel _lblMarketRatePercentageFixedCurrent;
        private SAHLLabel _lblLinkRateFixedCurrent;
        private SAHLLabel _lblEffectiveRateFixedCurrent;
        private SAHLLabel _lblInstalmentFixedCurrent;

        private SAHLLabel _lblMarketRatePercentageVariableCurrent;
        private SAHLLabel _lblLinkRateVariableCurrent;
        private SAHLLabel _lblEffectiveRateVariableCurrent;
        private SAHLLabel _lblInstalmentVariableCurrent;
        private SAHLLabel _lblDiscountFixed;
        private SAHLLabel _lblDiscountVariable;
        private SAHLLabel _lblDiscountFixedCurrent;
        private SAHLLabel _lblDiscountVariableCurrent;
        private SAHLLabel _lblSpacer;
        private SAHLLabel _lblEmpty;


        private bool _isReadOnly;
        //private bool _shouldRunPage = true;

		private SAHLLabel _lblPricingAdjustmentFixed;
		private SAHLLabel _lblPricingAdjustmentFixedCurrent;
		private SAHLLabel _lblPricingAdjustmentVariable;
		private SAHLLabel _lblPricingAdjustmentVariableCurrent;

        #endregion

        public event EventHandler<VarifixLoanInfoEventArgs> OnRecalculateVarifixDetails;
        public event EventHandler<VarifixLoanInfoEventArgs> OnCalculateMaximumFixedPercentage;

        public VarifixDetailsPanel()
        {
            Controls.Clear();

            _btnUseMaximum = new SAHLButton();
            _txtFixedPercentage = new SAHLCurrencyBox();
            _chkReset5yr = new CheckBox();
            //_chkReset6month = new CheckBox();
            _lblPercentageFixed = new SAHLLabel();
            _lblRandValueFixed = new SAHLLabel();
            _lblMarketRatePercentageFixed = new SAHLLabel();
            _lblLinkRateFixed = new SAHLLabel();
            _lblEffectiveRateFixed = new SAHLLabel();
            _lblInstalmentFixed = new SAHLLabel();
            _lblPercentageVariable = new SAHLLabel();
            _lblRandValueVariable = new SAHLLabel();
            _lblMarketRatePercentageVariable = new SAHLLabel();
            _lblLinkRateVariable = new SAHLLabel();
            _lblEffectiveRateVariable = new SAHLLabel();
            _lblInstalmentVariable = new SAHLLabel();
            _lblLinkRate = new SAHLLabel();
            _lblFixedPercentage = new SAHLLabel();
            _lblDiscountFixed = new SAHLLabel();
            _lblDiscountVariable = new SAHLLabel();
            _lblDiscountFixedCurrent = new SAHLLabel();
            _lblDiscountVariableCurrent = new SAHLLabel();

            _lblSpacer = new SAHLLabel();
            _lblEmpty = new SAHLLabel();

            // Add to new table

            _lblMarketRatePercentageFixedCurrent = new SAHLLabel();
            _lblLinkRateFixedCurrent = new SAHLLabel();
            _lblEffectiveRateFixedCurrent = new SAHLLabel();
            _lblInstalmentFixedCurrent = new SAHLLabel();


            _lblMarketRatePercentageVariableCurrent = new SAHLLabel();
            _lblLinkRateVariableCurrent = new SAHLLabel();
            _lblEffectiveRateVariableCurrent = new SAHLLabel();
            _lblInstalmentVariableCurrent = new SAHLLabel();
            _lblSpacer = new SAHLLabel();

			_lblPricingAdjustmentFixed = new SAHLLabel();
			_lblPricingAdjustmentFixed.ID = "lblPricingAdjustmentFixed";
			_lblPricingAdjustmentFixedCurrent = new SAHLLabel();
			_lblPricingAdjustmentFixedCurrent.ID = "lblPricingAdjustmentFixedCurrent";
			_lblPricingAdjustmentVariable = new SAHLLabel();
			_lblPricingAdjustmentVariable.ID = "lblPricingAdjustmentVariable";
			_lblPricingAdjustmentVariableCurrent = new SAHLLabel();
			_lblPricingAdjustmentVariableCurrent.ID = "lblPricingAdjustmentVariableCurrent";

            _lblSpacer.ID = "_lblSpacer";
            _lblEmpty.ID = "_lblEmpty";
            _btnUseMaximum.ID = "_btnUseMaximum";
            _txtFixedPercentage.ID = "_txtFixedPercentage";
            _chkReset5yr.ID = "_chkReset5yr";
            _chkReset5yr.Text = "5 yrs";
            //_chkReset5yr.Attributes.Add("OnClick", "SetResetPeriod(this);");
            //_chkReset6month.ID = "_chkReset6month";
            //_chkReset6month.Text = "6 months";
            //_chkReset6month.Attributes.Add("OnClick", "SetResetPeriod(this);");

            _lblPercentageFixed.ID = "_lblPercentageFixed";
            _lblRandValueFixed.ID = "_lblRandValueFixed";
            _lblMarketRatePercentageFixed.ID = "_lblMarketRatePercentageFixed";
            _lblLinkRateFixed.ID = "_lblLinkRateFixed";
            _lblEffectiveRateFixed.ID = "_lblEffectiveRateFixed";
            _lblInstalmentFixed.ID = "_lblInstalmentFixed";
            _lblPercentageVariable.ID = "_lblPercentageVariable";
            _lblRandValueVariable.ID = "_lblRandValueVariable";
            _lblMarketRatePercentageVariable.ID = "_lblMarketRatePercentageVariable";
            _lblLinkRateVariable.ID = "_lblLinkRateVariable";
            _lblEffectiveRateVariable.ID = "_lblEffectiveRateVariable";
            _lblInstalmentVariable.ID = "_lblInstalmentVariable";
            _lblLinkRate.ID = "_lblLinkRate";
            _lblFixedPercentage.ID = "_lblFixedPercentage";
            _lblDiscountFixed.ID = "_lblDiscountFixed";
            _lblDiscountVariable.ID = "_lblDiscountVariable";
            _lblDiscountFixedCurrent.ID = "_lblDiscountFixedCurrent";
            _lblDiscountVariableCurrent.ID = "_lblDiscountVariableCurrent";

            _lblMarketRatePercentageFixedCurrent.ID = "_lblMarketRatePercentageFixedCurrent";
            _lblLinkRateFixedCurrent.ID = "_lblLinkRateFixedCurrent";
            _lblEffectiveRateFixedCurrent.ID = "_lblEffectiveRateFixedCurrent";
            _lblInstalmentFixedCurrent.ID = "_lblInstalmentFixedCurrent";


            _lblMarketRatePercentageVariableCurrent.ID = "_lblMarketRatePercentageVariableCurrent";
            _lblLinkRateVariableCurrent.ID = "_lblLinkRateVariableCurrent";
            _lblEffectiveRateVariableCurrent.ID = "_lblEffectiveRateVariableCurrent";
            _lblInstalmentVariableCurrent.ID = "_lblInstalmentVariableCurrent";

            #region Discount Stuff

            _rowDiscountInput = new HtmlTableRow();
            _rowDiscountInput.ID = "rowDiscountInput";
            _rowDiscountInput.Attributes.Add("class", "rowStandard");

            _txtDiscount = new SAHLTextBox();
            _txtDiscount.ID = "_txtDiscount";
            _txtDiscount.AllowNegative = true;
            _txtDiscount.Height = Unit.Pixel(13);
            _txtDiscount.Font.Size = FontUnit.Point(8);
            _txtDiscount.DisplayInputType = InputType.Number;

            _nudDiscount = new NumericUpDownExtender();
            _nudDiscount.ID = "udDiscount";
            _nudDiscount.TargetControlID = _txtDiscount.ID;
            _nudDiscount.Minimum = -2.1;
            _nudDiscount.Maximum = 10;
            _nudDiscount.Step = 0.1;
            _nudDiscount.Width = 60;

            #endregion



            this.Controls.Add(_btnUseMaximum);
            this.Controls.Add(_txtFixedPercentage);
            this.Controls.Add(_chkReset5yr);
            //this.Controls.Add(_chkReset6month);
            this.Controls.Add(_lblPercentageFixed);
            this.Controls.Add(_lblRandValueFixed);
            this.Controls.Add(_lblMarketRatePercentageFixed);
            this.Controls.Add(_lblLinkRateFixed);
            this.Controls.Add(_lblEffectiveRateFixed);
            this.Controls.Add(_lblInstalmentFixed);
            this.Controls.Add(_lblPercentageVariable);
            this.Controls.Add(_lblRandValueVariable);
            this.Controls.Add(_lblMarketRatePercentageVariable);
            this.Controls.Add(_lblLinkRateVariable);
            this.Controls.Add(_lblEffectiveRateVariable);
            this.Controls.Add(_lblInstalmentVariable);
            // this.Controls.Add(_lblLinkRate);


            this.Controls.Add(_lblMarketRatePercentageVariableCurrent);
            this.Controls.Add(_lblLinkRateVariableCurrent);
            this.Controls.Add(_lblEffectiveRateVariableCurrent);
            this.Controls.Add(_lblInstalmentVariableCurrent);


            this.Controls.Add(_lblMarketRatePercentageFixedCurrent);
            this.Controls.Add(_lblLinkRateFixedCurrent);
            this.Controls.Add(_lblEffectiveRateFixedCurrent);
            this.Controls.Add(_lblInstalmentFixedCurrent);
            this.Controls.Add(_lblSpacer);

            this.Controls.Add(_lblDiscountVariableCurrent);
            this.Controls.Add(_lblDiscountFixedCurrent);
            this.Controls.Add(_lblDiscountVariable);
            this.Controls.Add(_lblDiscountFixed);

            // this.Controls.Add(_lblLinkRate);

            // this.Controls.Add(_lblFixedPercentage);

            _txtFixedPercentage.TextChanged += new EventHandler(_txtFixedPercentage_TextChanged);
            _btnUseMaximum.Click += new EventHandler(_btnUseMaximum_Click);

        }

        private void RegisterJavaScript()
        {
            System.Text.StringBuilder JS = new System.Text.StringBuilder();
            JS.AppendLine("function doVariFixDiscount(visible)");
            JS.AppendLine("{");
            JS.AppendLine("var rowDiscountInput = document.getElementById('" + _rowDiscountInput.ClientID + "');");
            JS.AppendLine("if (rowDiscountInput != null)");
            JS.AppendLine("{");
            JS.AppendLine("if (visible)");
            JS.AppendLine("{");
            JS.AppendLine("rowDiscountInput.style.display = 'inline';");
            JS.AppendLine("}");
            JS.AppendLine("else");
            JS.AppendLine("{");
            JS.AppendLine("rowDiscountInput.style.display = 'none';");
            JS.AppendLine("}");
            JS.AppendLine("}");
            JS.AppendLine("}");

            if (!Page.ClientScript.IsClientScriptBlockRegistered("doVariFixDiscount"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "doVariFixDiscount", JS.ToString(), true);
        }

        void _txtFixedPercentage_TextChanged(object sender, EventArgs e)
        {
            //if (!_shouldRunPage)
            //    return;

            if (_applicationProductVariFixLoan != null)
            {
                _applicationProductVariFixLoan.FixedPercentage = Convert.ToDouble(_txtFixedPercentage.Text) / 100;

                if (OnRecalculateVarifixDetails != null)
                    OnRecalculateVarifixDetails(sender, null);
            }
        }

        /// <summary>
        /// Populates the controls with the information supplied (though the LoanDetails property)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //if (!_shouldRunPage)
            //    return;

            _btnUseMaximum.CssClass = "SAHLButton4";
            base.GroupingText = _titleText;
            _htmlTable = new HtmlTable();
            _htmlTable.Width = "99%";

            if (DesignMode)
                return;

        }

        protected void _btnUseMaximum_Click(object sender, EventArgs e)
        {
            //if (!_shouldRunPage)
            //    return;

            OnCalculateMaximumFixedPercentage(sender, null);
        }

        /// <summary>
        /// Sets up the controls for render.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (!_shouldRunPage)
            //    return;

            // build the header variables

            AddHeadersRow();

            base.GroupingText = _titleText;

            AddRowContents("Percentage", _lblPercentageFixed, _lblPercentageVariable, _lblEmpty, _lblEmpty);
            AddRowContents("Rand Value", _lblRandValueFixed, _lblRandValueVariable, _lblEmpty, _lblEmpty);
            AddRowContents("Market Value %", _lblMarketRatePercentageFixed, _lblMarketRatePercentageVariable, _lblMarketRatePercentageFixedCurrent, _lblMarketRatePercentageVariableCurrent);
            AddRowContents("Link Rate", _lblLinkRateFixed, _lblLinkRateVariable, _lblLinkRateFixedCurrent, _lblLinkRateVariableCurrent);
            //Add discount input control
            AddRowDiscountInput("Discount/Surcharge", _nudDiscount, _txtDiscount);
            AddRowContents("Discount", _lblDiscountFixed, _lblDiscountVariable, _lblDiscountFixedCurrent, _lblDiscountVariableCurrent);
			AddRowContents("Pricing Adjustment", _lblPricingAdjustmentFixed, _lblPricingAdjustmentVariable, _lblPricingAdjustmentFixedCurrent, _lblPricingAdjustmentVariableCurrent);
            AddRowContents("Effective Rate", _lblEffectiveRateFixed, _lblEffectiveRateVariable, _lblEffectiveRateFixedCurrent, _lblEffectiveRateVariableCurrent);
            AddRowContents("Instalment", _lblInstalmentFixed, _lblInstalmentVariable, _lblInstalmentFixedCurrent, _lblInstalmentVariableCurrent);

            // AddFillerRow();

            base.Controls.Add(_htmlTable);

            RegisterJavaScript();
        }

        private void PopulateControls()
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _applicationProductVariFixLoan as ISupportsVariableLoanApplicationInformation;
            if (supportsVariableLoanApplicationInformation != null)
                _lblLinkRate.Text = supportsVariableLoanApplicationInformation.VariableLoanInformation.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);

            if (_applicationProductVariFixLoan != null)
            {
                //No longer allow 6mnth reset.
                //if (_applicationProductVariFixLoan.VariFixInformation.MarketRate.Key == (int)MarketRates.TwentyYearFixedMortgageRate5yr)
                //{
                _chkReset5yr.Visible = false;
                _chkReset5yr.Checked = true;
                //_chkReset6month.Checked = false;
                //}
                //else
                //{
                //    _chkReset5yr.Checked = false;
                //    _chkReset6month.Checked = true;
                //}

				double pricingAdjustment = _application.GetRateAdjustments();
                double percentage = _applicationProductVariFixLoan.FixedPercentage.HasValue ? _applicationProductVariFixLoan.FixedPercentage.Value : 0D;
                _lblPercentageFixed.Text = percentage.ToString(SAHL.Common.Constants.RateFormat);
                _lblFixedPercentage.Text = percentage.ToString(SAHL.Common.Constants.RateFormat);
                _txtFixedPercentage.Text = Convert.ToString(percentage * 100);

                percentage = _applicationProductVariFixLoan.VariablePercentage.HasValue ? _applicationProductVariFixLoan.VariablePercentage.Value : 0D;
                _lblPercentageVariable.Text = percentage.ToString(SAHL.Common.Constants.RateFormat);


                _lblRandValueFixed.Text = _applicationProductVariFixLoan.FixedRandValue.HasValue ? _applicationProductVariFixLoan.FixedRandValue.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : (0D).ToString(SAHL.Common.Constants.CurrencyFormat);
                _lblRandValueVariable.Text = _applicationProductVariFixLoan.VariableRandValue.HasValue ? _applicationProductVariFixLoan.VariableRandValue.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : (0D).ToString(SAHL.Common.Constants.CurrencyFormat);


                //TODO: Add JIBAR Values

                _lblMarketRatePercentageFixed.Text = _applicationProductVariFixLoan.FixedMarketRate.HasValue ? _applicationProductVariFixLoan.FixedMarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);
                _lblMarketRatePercentageVariableCurrent.Text = _applicationProductVariFixLoan.VariableMarketRate.HasValue ? _applicationProductVariFixLoan.VariableMarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);
                _lblMarketRatePercentageVariable.Text = supportsVariableLoanApplicationInformation.VariableLoanInformation.MarketRate.HasValue ? supportsVariableLoanApplicationInformation.VariableLoanInformation.MarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);

                _lblMarketRatePercentageFixedCurrent.Text = _applicationProductVariFixLoan.FixedMarketRate.HasValue ? _applicationProductVariFixLoan.FixedMarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);




                double linkRate = _applicationProductVariFixLoan.LinkRate.HasValue ? _applicationProductVariFixLoan.LinkRate.Value : 0D;
                _lblLinkRateFixed.Text = linkRate.ToString(SAHL.Common.Constants.RateFormat);
                _lblLinkRateVariable.Text = linkRate.ToString(SAHL.Common.Constants.RateFormat);
                _lblLinkRateVariableCurrent.Text = supportsVariableLoanApplicationInformation.VariableLoanInformation.RateConfiguration.Margin.Value.ToString(SAHL.Common.Constants.RateFormat);
                _lblLinkRateFixedCurrent.Text = linkRate.ToString(SAHL.Common.Constants.RateFormat);
                _lblInstalmentVariable.Text = _applicationProductVariFixLoan.VariableInstalment.HasValue ? _applicationProductVariFixLoan.VariableInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : (0D).ToString(SAHL.Common.Constants.CurrencyFormat);
                _lblInstalmentFixedCurrent.Text = _applicationProductVariFixLoan.FixedInstalment.HasValue ? _applicationProductVariFixLoan.FixedInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : (0D).ToString(SAHL.Common.Constants.CurrencyFormat);

                double variableRandValue = _applicationProductVariFixLoan.VariableRandValue.Value;
				double variableEffectiveRate = _applicationProductVariFixLoan.VariableEffectiveRate.Value + pricingAdjustment;
                double term = (Double)supportsVariableLoanApplicationInformation.VariableLoanInformation.Term.Value;

                double instalmentVariableCurrent = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(variableRandValue, variableEffectiveRate, term, false);




                _lblInstalmentVariableCurrent.Text = instalmentVariableCurrent.ToString(SAHL.Common.Constants.CurrencyFormat);

                _lblInstalmentFixed.Text = _applicationProductVariFixLoan.FixedInstalment.HasValue ? _applicationProductVariFixLoan.FixedInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : (0D).ToString(SAHL.Common.Constants.CurrencyFormat);
                //if there is no discount, dont let it be editable
                IsDiscountReadOnly = !_application.HasFinancialAdjustment(FinancialAdjustmentTypeSources.DiscountedLinkrate);

                IApplicationProductMortgageLoan appProd = _application.CurrentProduct as IApplicationProductMortgageLoan;
                double discount = appProd.ManualDiscount.HasValue ? appProd.ManualDiscount.Value : 0D;
				double effectiveRateVariable = supportsVariableLoanApplicationInformation.VariableLoanInformation.MarketRate.Value + linkRate + discount + pricingAdjustment;
                string displayDiscount = discount.ToString(SAHL.Common.Constants.RateFormat);

                _lblDiscountVariable.Text = displayDiscount;
                _lblDiscountFixed.Text = displayDiscount;
                _lblDiscountVariableCurrent.Text = displayDiscount;
                _lblDiscountFixedCurrent.Text = displayDiscount;

                _txtDiscount.Text = (discount * 100).ToString();

				_lblPricingAdjustmentFixed.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);
				_lblPricingAdjustmentFixedCurrent.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);
				_lblPricingAdjustmentVariable.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);
				_lblPricingAdjustmentVariableCurrent.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);

				double effectiveRateFixed = _applicationProductVariFixLoan.FixedEffectiveRate.HasValue ? _applicationProductVariFixLoan.FixedEffectiveRate.Value + pricingAdjustment : 0 + pricingAdjustment;
				double effectiveRateVariableCurrent = _applicationProductVariFixLoan.VariableEffectiveRate.HasValue ? _applicationProductVariFixLoan.VariableEffectiveRate.Value + pricingAdjustment : 0 + pricingAdjustment;
				_lblEffectiveRateFixed.Text = effectiveRateFixed.ToString(SAHL.Common.Constants.RateFormat);
				_lblEffectiveRateVariable.Text = effectiveRateVariable.ToString(SAHL.Common.Constants.RateFormat);
				_lblEffectiveRateFixedCurrent.Text = effectiveRateFixed.ToString(SAHL.Common.Constants.RateFormat);
				_lblEffectiveRateVariableCurrent.Text = effectiveRateVariableCurrent.ToString(SAHL.Common.Constants.RateFormat);
			}
        }

        public void GetApplicationDetails(IApplicationMortgageLoan ApplicationMortgageLoan)
        {
            IApplicationProductVariFixLoan applicationProductVariFixLoan = ApplicationMortgageLoan.CurrentProduct as IApplicationProductVariFixLoan;
            if (applicationProductVariFixLoan != null
                && !String.IsNullOrEmpty(_txtFixedPercentage.Text))
            {
                // Get the fixed percentage from the control
                applicationProductVariFixLoan.FixedPercentage = Convert.ToDouble(_txtFixedPercentage.Text) / 100;
            }


            if (applicationProductVariFixLoan != null)
            {
                ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                if (_chkReset5yr.Checked)// == (int)MarketRates.TwentyYearFixedMortgageRate5yr)
                    applicationProductVariFixLoan.VariFixInformation.MarketRate = lookupRepo.MarketRates.ObjectDictionary[Convert.ToString((int)MarketRates.FiveYearResetFixedMortgageRate)];
                else
                    applicationProductVariFixLoan.VariFixInformation.MarketRate = lookupRepo.MarketRates.ObjectDictionary[Convert.ToString((int)MarketRates.TwentyYearFixedMortgageRate)];
            }

        }

        protected void AddHeadersRow()
        {

            #region Build the first row
            HtmlTableRow tableRow = new HtmlTableRow();

            HtmlTableCell tableCellLinkRate = new HtmlTableCell();
            tableCellLinkRate.Attributes.Add("class", "TitleText");
            tableRow.Cells.Add(tableCellLinkRate);

            HtmlTableCell tableCellFixedPercentage = new HtmlTableCell();
            tableCellFixedPercentage.Attributes.Add("class", "TitleText");
            Label lblFixedPercentage = new Label();
            lblFixedPercentage.Text = "Fixed % ";
            tableCellFixedPercentage.Controls.Add(lblFixedPercentage);
            _txtFixedPercentage.Width = new Unit(50, UnitType.Pixel);

            _chkReset5yr.Enabled = false;

            if (_isReadOnly)
            {
                tableCellFixedPercentage.Controls.Add(_lblFixedPercentage);
                _txtFixedPercentage.Visible = false;
                //_chkReset5yr.Enabled = false;
                //_chkReset6month.Enabled = false;
            }
            else
                tableCellFixedPercentage.Controls.Add(_txtFixedPercentage);

            tableRow.Cells.Add(tableCellFixedPercentage);

            HtmlTableCell tableCellUseMax = new HtmlTableCell();
            _btnUseMaximum.Text = "Use Maximum";
            if (_isReadOnly)
                _btnUseMaximum.Visible = false;
            else
                _btnUseMaximum.Enabled = true;

            tableCellUseMax.Controls.Add(_btnUseMaximum);
            tableRow.Cells.Add(tableCellUseMax);

            _htmlTable.Rows.Add(tableRow);

            HtmlTableRow tr = new HtmlTableRow();

            HtmlTableCell td = new HtmlTableCell();
            td.Attributes.Add("class", "TitleText");
            td.InnerText = "Reset period";
            tr.Cells.Add(td);

            HtmlTableCell td1 = new HtmlTableCell();
            //td1.Controls.Add(_chkReset6month);
            td1.InnerHtml = "5 year";
            tr.Cells.Add(td1);

            HtmlTableCell td2 = new HtmlTableCell();
            td2.Controls.Add(_chkReset5yr);
            tr.Cells.Add(td2);
            _htmlTable.Rows.Add(tr);


            #endregion

            #region Build the 'column headers'

            AddFillerRow();

            HtmlTableRow htmlTableRowHeaders = new HtmlTableRow();
            HtmlTableRow htmlTableRowRateHeaders = new HtmlTableRow();

            HtmlTableCell htmlTableTitleCellSpacer = new HtmlTableCell();
            HtmlTableCell htmlTableCellSpacer = new HtmlTableCell();
            HtmlTableCell htmlTableCellFixed = new HtmlTableCell();
            HtmlTableCell htmlTableCellVariable = new HtmlTableCell();
            HtmlTableCell htmlTableCellFixedCurrent = new HtmlTableCell();
            HtmlTableCell htmlTableCellVariableCurrent = new HtmlTableCell();

            //Rate Headers

            HtmlTableCell htmlTableCellApplication = new HtmlTableCell();
            HtmlTableCell htmlTableCellJIBAR = new HtmlTableCell();

            htmlTableCellApplication.InnerText = "On Application";
            htmlTableCellApplication.ColSpan = 2;
            htmlTableCellApplication.Attributes.Add("class", "TitleText");

            htmlTableCellJIBAR.InnerText = "At Current JIBAR";
            htmlTableCellJIBAR.ColSpan = 2;
            htmlTableCellJIBAR.Attributes.Add("class", "TitleText");

            htmlTableCellFixed.InnerText = "Fixed Portion";
            htmlTableCellFixed.Attributes.Add("class", "TitleText");

            htmlTableCellVariable.InnerText = "Variable Portion";
            htmlTableCellVariable.Attributes.Add("class", "TitleText");


            htmlTableCellFixedCurrent.InnerText = "Fixed Portion";
            htmlTableCellFixedCurrent.Attributes.Add("class", "TitleText");

            htmlTableCellVariableCurrent.InnerText = "Variable Portion";
            htmlTableCellVariableCurrent.Attributes.Add("class", "TitleText");

            htmlTableCellSpacer.InnerText = "";
            htmlTableCellSpacer.Attributes.Add("class", "TitleText");

            htmlTableTitleCellSpacer.InnerText = "";
            htmlTableTitleCellSpacer.Attributes.Add("class", "TitleText");


            htmlTableRowHeaders.Cells.Add(htmlTableCellSpacer);
            htmlTableRowHeaders.Cells.Add(htmlTableCellFixed);
            htmlTableRowHeaders.Cells.Add(htmlTableCellVariable);
            htmlTableRowHeaders.Cells.Add(htmlTableCellFixedCurrent);
            htmlTableRowHeaders.Cells.Add(htmlTableCellVariableCurrent);

            //Rate Titles
            htmlTableRowRateHeaders.Cells.Add(htmlTableTitleCellSpacer);
            htmlTableRowRateHeaders.Cells.Add(htmlTableCellApplication);
            htmlTableRowRateHeaders.Cells.Add(htmlTableCellJIBAR);
            _htmlTable.Rows.Add(htmlTableRowRateHeaders);
            _htmlTable.Rows.Add(htmlTableRowHeaders);

            #endregion

        }

        protected void AddRowContents(string cellCaption, WebControl fixedPortion, WebControl variablePortion, WebControl fixedPortionCurrent, WebControl variablePortionCurrent)
        {

            HtmlTableRow htmlTableRow = new HtmlTableRow();

            Unit width = new Unit(100, UnitType.Pixel);
            fixedPortion.Width = width;
            variablePortion.Width = width;

            HtmlTableCell rowCaption = new HtmlTableCell();
            rowCaption.Attributes.Add("class", "TitleText");
            rowCaption.InnerText = cellCaption;
            htmlTableRow.Cells.Add(rowCaption);

            HtmlTableCell rowFixedPortion = new HtmlTableCell();
            //rowFixedPortion.Align = "centre";
            rowFixedPortion.Controls.Add(fixedPortion);
            htmlTableRow.Cells.Add(rowFixedPortion);
            _htmlTable.Rows.Add(htmlTableRow);

            HtmlTableCell rowVariablePortion = new HtmlTableCell();
            rowVariablePortion.Controls.Add(variablePortion);
            htmlTableRow.Cells.Add(rowVariablePortion);
            _htmlTable.Rows.Add(htmlTableRow);

            HtmlTableCell rowFixedPortionCurrent = new HtmlTableCell();
            //rowFixedPortion.Align = "centre";
            rowFixedPortionCurrent.Controls.Add(fixedPortionCurrent);
            htmlTableRow.Cells.Add(rowFixedPortionCurrent);
            _htmlTable.Rows.Add(htmlTableRow);

            HtmlTableCell rowVariablePortionCurrent = new HtmlTableCell();
            rowVariablePortionCurrent.Controls.Add(variablePortionCurrent);
            htmlTableRow.Cells.Add(rowVariablePortionCurrent);
            _htmlTable.Rows.Add(htmlTableRow);
        }

        protected static void AddRow(string cellCaption, WebControl control, bool visible, HtmlTableRow tableRow, HtmlTable parentTable)
        {
            HtmlTableCell rowCaption = new HtmlTableCell();
            rowCaption.InnerText = cellCaption;
            tableRow.Cells.Add(rowCaption);

            HtmlTableCell rowValue = new HtmlTableCell();
            rowValue.Controls.Add(control);
            tableRow.Cells.Add(rowValue);

            if (!visible)
                tableRow.Attributes.Add("style", "display: none");

            ///_htmlTable.Rows.Add(tableRow);
            parentTable.Rows.Add(tableRow);
        }

        protected void AddRowDiscountInput(string cellCaption, Control control, WebControl webcontrol)
        {

            HtmlTableCell rowCaption = new HtmlTableCell();
            rowCaption.Attributes.Add("class", "TitleText");
            rowCaption.InnerText = cellCaption;
            _rowDiscountInput.Cells.Add(rowCaption);

            HtmlTableCell rowValue = new HtmlTableCell();
            rowValue.Controls.Add(webcontrol);
            rowValue.Controls.Add(control);
            _rowDiscountInput.Cells.Add(rowValue);

            _htmlTable.Rows.Add(_rowDiscountInput);
        }

        private void AddFillerRow()
        {
            HtmlTableRow htmlTableRow = new HtmlTableRow();
            HtmlTableCell htmlTableCell = new HtmlTableCell();
            htmlTableCell.ColSpan = 5;

            HtmlGenericControl spacer = new HtmlGenericControl("div");
            spacer.Style.Add(HtmlTextWriterStyle.Height, "10px");
            htmlTableCell.Controls.Add(spacer);

            htmlTableRow.Cells.Add(htmlTableCell);
            _htmlTable.Rows.Add(htmlTableRow);
        }

        public void BindApplicationProduct(IApplicationProductVariFixLoan ApplicationProductVariFixLoan, IApplication Application)
        {
            _applicationProductVariFixLoan = ApplicationProductVariFixLoan;
            _application = Application;
            PopulateControls();
        }

        public void Reload(bool discountEditable)
        {
            if (discountEditable)
                _rowDiscountInput.Attributes.Add("style", "display: inline");
            else
                _rowDiscountInput.Attributes.Add("style", "display: none");
        }

        /// <summary>
        /// Determines whether we are in design mode (standard DesignMode not reliable).
        /// </summary>
        protected static new bool DesignMode
        {
            get
            {
                return (HttpContext.Current == null);
            }
        }

        /// <summary>
        /// Sets the TitleText of the panel.
        /// </summary>
        public string TitleText
        {
            set { _titleText = value; }
            get { return _titleText; }
        }

        /// <summary>
        /// Sets whether the control is rendered in a readonly mode.
        /// </summary>
        public bool IsReadOnly
        {
            set
            {
                _isReadOnly = value;

                IsDiscountReadOnly = value;
            }
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Get/set the discount value
        /// </summary>
        public double Discount
        {
            get { return (Convert.ToDouble(_txtDiscount.Text.Length > 0 ? _txtDiscount.Text : "0") / 100); }
            //set { _txtDiscount.Text = (value * 100).ToString(); }
        }

        public bool IsDiscountReadOnly
        {
            set
            {
                if (value == true)
                    _rowDiscountInput.Attributes.Add("style", "display: none");
                //else
                //    _rowDiscountInput.Attributes.Add("style", "display: inline");

                _isDiscountReadOnly = value;
            }
            get { return _isDiscountReadOnly; }
        }
    }

    public class VarifixLoanInfoEventArgs : EventArgs
    {
        private IVarifixLoanInfo _varifixLoanInfo;

        public VarifixLoanInfoEventArgs()
        {

        }

        public VarifixLoanInfoEventArgs(IVarifixLoanInfo varifixLoanInfo)
        {
            _varifixLoanInfo = varifixLoanInfo;
        }

        public IVarifixLoanInfo VarifixLoanInfo
        {
            set { _varifixLoanInfo = value; }
            get { return _varifixLoanInfo; }
        }
    }
}
