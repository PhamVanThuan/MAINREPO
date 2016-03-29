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
using AjaxControlToolkit;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Use to display Loan Information for the Edge Home Loan
    /// </summary>
    public class EdgeDetailsPanel : Panel, INamingContainer
    {
        #region Private varibles
        private string _titleText = "";
        private HtmlTable _htmlTable;
        private SAHLLabel _lblMarketRatePerc;
        private SAHLLabel _lblMarketRatePercCurrent;
        private SAHLLabel _lblLinkRate;
        private SAHLLabel _lblLinkRateCurrent;
        private SAHLLabel _lblPricingAdjustment;
        private SAHLLabel _lblPricingAdjustmentCurrent;
        private SAHLLabel _lblEffectiveRate;
        private SAHLLabel _lblEffectiveRateCurrent;
        private SAHLLabel _lblAmortisingInstalment;
        private SAHLLabel _lblAmortisingInstalmentCurrent;
        private SAHLLabel _lblFullTermInstalment;
        private SAHLLabel _lblFullTermInstalmentCurrent;
        private SAHLLabel _lblInterestOnlyInstalment;
        private SAHLLabel _lblInterestOnlyInstalmentCurrent;
        private SAHLLabel _lblFullTerm;
        private SAHLLabel _lblAmortisationTerm;
        private SAHLLabel _lblInterestOnlyTerm;
        private NumericUpDownExtender _nudDiscount;
        private SAHLTextBox _txtDiscount;
        private SAHLLabel _lblDiscountedLinkRate;
        private SAHLLabel _lblDiscountedLinkRateCurrent;
        private SAHLLabel _lblDiscount;
        private SAHLLabel _lblEmpty;
        private SAHLLabel _lblDiscountCurrent;
        private HtmlTableRow _rowDiscountInput;
        private HtmlTableRow _rowDiscountLabelJB;
        private HtmlTableRow _rowDiscountLabel;
        private EdgeDetails _edgeDetails;
        private bool _isReadOnly;
        private bool _isDiscountVisible;
        private IApplication _application;
        private bool _isDiscountReadOnly;

        #endregion

        public EdgeDetailsPanel()
        {
            _lblMarketRatePerc = new SAHLLabel();
            _lblMarketRatePerc.ID = "lblMarketRatePerc";

            _lblMarketRatePercCurrent = new SAHLLabel();
            _lblMarketRatePercCurrent.ID = "lblMarketRatePercCurrent";

            _lblLinkRate = new SAHLLabel();
            _lblLinkRate.ID = "lblLinkRate";

            _lblLinkRateCurrent = new SAHLLabel();
            _lblLinkRateCurrent.ID = "lblLinkRateCurrent";

			_lblPricingAdjustment = new SAHLLabel();
			_lblPricingAdjustment.ID = "lblPricingAdjustment";

			_lblPricingAdjustmentCurrent = new SAHLLabel();
			_lblPricingAdjustmentCurrent.ID = "lblPricingAdjustmentCurrent";

            _lblEffectiveRate = new SAHLLabel();
            _lblEffectiveRate.ID = "lblEffectiveRate";

            _lblEffectiveRateCurrent = new SAHLLabel();
            _lblEffectiveRateCurrent.ID = "lblEffectiveRateCurrent";

            _lblEmpty = new SAHLLabel();
            _lblEmpty.ID = "lblEmpty";

            _lblAmortisingInstalment = new SAHLLabel();
            _lblAmortisingInstalment.ID = "lblAmortisingInstalment";

            _lblAmortisingInstalmentCurrent = new SAHLLabel();
            _lblAmortisingInstalmentCurrent.ID = "lblAmortisingInstalmentCurrent";

            _lblFullTermInstalment = new SAHLLabel();
            _lblFullTermInstalment.ID = "lblFullTermInstalment";

            _lblFullTermInstalmentCurrent = new SAHLLabel();
            _lblFullTermInstalmentCurrent.ID = "lblFullTermInstalmentCurrent";

            _lblInterestOnlyInstalment = new SAHLLabel();
            _lblInterestOnlyInstalment.ID = "lblInterestOnlyInstalment";

            _lblInterestOnlyInstalmentCurrent = new SAHLLabel();
            _lblInterestOnlyInstalmentCurrent.ID = "lblInterestOnlyInstalmentCurrent";

            _lblFullTerm = new SAHLLabel();
            _lblFullTerm.ID = "lblFullTerm";

            _lblInterestOnlyTerm= new SAHLLabel();
            _lblInterestOnlyTerm.ID = "lblInterestOnlyTerm";

            _lblAmortisationTerm = new SAHLLabel();
            _lblAmortisationTerm.ID = "lblAmortisationTerm";

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

           
            _lblDiscount = new SAHLLabel();
            _lblDiscount.ID = "lblDiscount";

            _lblDiscountCurrent = new SAHLLabel();
            _lblDiscountCurrent.ID = "lblDiscountCurrent";

            _lblDiscountedLinkRate = new SAHLLabel();
            _lblDiscountedLinkRate.ID = "lblDiscountedLinkRate";

            _lblDiscountedLinkRateCurrent = new SAHLLabel();
            _lblDiscountedLinkRateCurrent.ID = "lblDiscountedLinkRateCurrent";

            _rowDiscountInput = new HtmlTableRow();
            _rowDiscountInput.ID = "rowDiscountInput";
            _rowDiscountInput.Attributes.Add("class", "rowStandard");

            _rowDiscountLabel = new HtmlTableRow();
            _rowDiscountLabel.ID = "rowDiscountLabel";
            _rowDiscountLabel.Attributes.Add("class", "rowStandard");

            _rowDiscountLabelJB = new HtmlTableRow();
            _rowDiscountLabelJB.ID = "rowDiscountLabelJB";
            _rowDiscountLabelJB.Attributes.Add("class", "rowStandard");
        }

        #region Javascript

        private void RegisterJavaScript()
        {
            System.Text.StringBuilder JS = new System.Text.StringBuilder();
            JS.AppendLine("function hideDiscountEHL(visible)");
            JS.AppendLine("{");
            JS.AppendLine("var rowDiscountInput = document.getElementById('" + _rowDiscountInput.ClientID + "');");
            JS.AppendLine("var rowDiscountLabel = document.getElementById('" + _rowDiscountLabel.ClientID + "');");
            JS.AppendLine("var rowDiscountLabelJB = document.getElementById('" + _rowDiscountLabelJB.ClientID + "');");
            JS.AppendLine("if (rowDiscountInput != null)");
            JS.AppendLine("{");
            JS.AppendLine("rowDiscountLabelJB.style.display = 'inline';");
            JS.AppendLine("if (visible)");
            JS.AppendLine("{");
            JS.AppendLine("rowDiscountInput.style.display = 'inline';");
            JS.AppendLine("rowDiscountLabel.style.display = 'none';");
            JS.AppendLine("}");
            JS.AppendLine("else");
            JS.AppendLine("{");
            JS.AppendLine("rowDiscountInput.style.display = 'none';");
            JS.AppendLine("rowDiscountLabel.style.display = 'inline';");
            JS.AppendLine("}");
            JS.AppendLine("}");
            JS.AppendLine("else");
            JS.AppendLine("{");
            JS.AppendLine("doVariFixDiscount(visible);");
            JS.AppendLine("}");
            JS.AppendLine("}");
            JS.AppendLine(" ");
            JS.AppendLine("function noKeyPressAllowed()");
            JS.AppendLine("{");
            JS.AppendLine("window.event.returnValue = false;");
            JS.AppendLine("}");

            if (!Page.ClientScript.IsClientScriptBlockRegistered("EHLloanDetailsJS"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "EHLloanDetailsJS", JS.ToString(), true);
        }

        #endregion

        /// <summary>
        /// Populates the controls with the information supplied (though the LoanDetails property)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            base.GroupingText = _titleText;

            _htmlTable = new HtmlTable();
            // base.Width = new Unit(500, UnitType.Pixel);
            _htmlTable.Width = "99%";

            if (DesignMode)
                return;
        }

        /// <summary>
        /// Sets up the controls for render.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            base.GroupingText = _titleText;

            if (_application == null)
                return;

            // Get the data from the _application object
            PopulateLoanDetailProperties();

            PopulateControls();
            HtmlTable tb1 = new HtmlTable();
            HtmlTable tb2 = new HtmlTable();

            HtmlTableRow masterRow = new HtmlTableRow();
            HtmlTableCell masterDiv1 = new HtmlTableCell();
            masterDiv1.Width = "70%";
            HtmlTableCell masterDiv2 = new HtmlTableCell();
            masterDiv2.Width = "30%";
            masterDiv1.Controls.Add(tb1);
            masterDiv2.Controls.Add(tb2);
            masterRow.Cells.Add(masterDiv1);
            masterRow.Cells.Add(masterDiv2);
            _htmlTable.Rows.Add(masterRow);

            SAHLLabel _lblHeader1 = new SAHLLabel();
            SAHLLabel _lblHeader2 = new SAHLLabel();

            _lblEmpty.Text = "-";
            _lblHeader1.Text = "On application";
            _lblHeader2.Text = "At current JIBAR";
            _lblHeader1.Font.Bold = true;
            _lblHeader2.Font.Bold = true;
            AddRowContents("", _lblHeader1, true, tb1);
            AddRowContents("", _lblHeader2, true, tb2);

            AddRowContents("Market Rate", _lblMarketRatePerc, true, tb1);
            AddRowContents("", _lblMarketRatePercCurrent, true, tb2);

            AddRowContents("Link Rate", _lblLinkRate, true, tb1);
            AddRowContents("", _lblLinkRateCurrent, true, tb2);

            //The 2 different Discount rows are: Input and Label for editable and readonly
            AddRowDiscountInput("Discount / Surcharge", _nudDiscount, _txtDiscount, tb1); //always add the row, even if it is not currently visible
            AddRow("Discount / Surcharge", _lblDiscount, _isDiscountVisible, _rowDiscountLabel, tb1);
            //Only one row will ever be visible, so only add 1 row to table 2
            AddRow("", _lblDiscountCurrent, _isDiscountVisible, _rowDiscountLabelJB, tb2);

            if (_isDiscountReadOnly || !_isDiscountVisible)
                _rowDiscountInput.Attributes.Add("style", "display: none");
            else
                _rowDiscountLabel.Attributes.Add("style", "display: none");

            AddRowContents("Discounted Link Rate", _lblDiscountedLinkRate, _isDiscountVisible, tb1);
            AddRowContents("", _lblDiscountedLinkRateCurrent, _isDiscountVisible, tb2);

            AddRowContents("Pricing Adjustment", _lblPricingAdjustment, true, tb1);
            AddRowContents("", _lblPricingAdjustmentCurrent, true, tb2);

            AddRowContents("Effective Rate", _lblEffectiveRate, true, tb1);
            AddRowContents("", _lblEffectiveRateCurrent, true, tb2);

            AddRowContents("Interest Only Instalment", _lblInterestOnlyInstalment, true, tb1);
            AddRowContents("", _lblInterestOnlyInstalmentCurrent, true, tb2);

            AddRowContents("Amortising Instalment", _lblAmortisingInstalment, true, tb1);
            AddRowContents("", _lblAmortisingInstalmentCurrent, true, tb2);

            AddRowContents("Full Term Instalment", _lblFullTermInstalment, true, tb1);
            AddRowContents("", _lblFullTermInstalmentCurrent, true, tb2);

            AddRowContents("", new SAHLLabel(), true, tb1);
            AddRowContents("", new SAHLLabel(), true, tb1);
            AddRowContents("Interest Only Term", _lblInterestOnlyTerm, true, tb1);
            AddRowContents("Amortising Term", _lblAmortisationTerm, true, tb1);
            AddRowContents("Full Term", _lblFullTerm, true, tb1);

            base.Controls.Add(_htmlTable);

            RegisterJavaScript();
        }

        protected override void OnPreRender(EventArgs e)
        {
            _txtDiscount.ReadOnly = true;
            _txtDiscount.Attributes.Add("onkeypress", "noKeyPressAllowed()");
        }

        private void PopulateLoanDetailProperties()
        {
            if (_edgeDetails == null)
                _edgeDetails = new EdgeDetails();

            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = _application.CurrentProduct as ISupportsVariableLoanApplicationInformation;
            if (supportsVariableLoanApplicationInformation != null)
            {
                _edgeDetails.MarketRate = supportsVariableLoanApplicationInformation.VariableLoanInformation.MarketRate;
                _edgeDetails.LinkRateKey = supportsVariableLoanApplicationInformation.VariableLoanInformation.RateConfiguration.Margin.Key;
                _edgeDetails.LinkRate = supportsVariableLoanApplicationInformation.VariableLoanInformation.RateConfiguration.Margin.Value;
                _edgeDetails.CurrentJibar = supportsVariableLoanApplicationInformation.VariableLoanInformation.RateConfiguration.MarketRate.Value;

            }

            IApplicationProductMortgageLoan applicationProductMortgageLoan = _application.CurrentProduct as IApplicationProductMortgageLoan;
            if (applicationProductMortgageLoan != null)
            {
                _edgeDetails.EffectiveRate = applicationProductMortgageLoan.EffectiveRate.Value + applicationProductMortgageLoan.DiscountedLinkRate.Value;
                _edgeDetails.Discount = applicationProductMortgageLoan.ManualDiscount;
                _edgeDetails.DiscountedLinkRate = applicationProductMortgageLoan.DiscountedLinkRate;
                _edgeDetails.LoanAmount = (double)applicationProductMortgageLoan.LoanAgreementAmount;
                _edgeDetails.FullTerm = (int)applicationProductMortgageLoan.Term;
            }

            IApplicationProductEdge applicationProductEdgeLoan = _application.CurrentProduct as IApplicationProductEdge;
            if (applicationProductEdgeLoan != null)
            {
                _edgeDetails.InterestOnlyTerm = applicationProductEdgeLoan.EdgeInformation.InterestOnlyTerm;
                _edgeDetails.AmortisationTerm = (int)applicationProductEdgeLoan.Term - applicationProductEdgeLoan.EdgeInformation.InterestOnlyTerm;
                _edgeDetails.AmortisationTermInstalment = applicationProductEdgeLoan.EdgeInformation.AmortisationTermInstalment;
                _edgeDetails.InterestOnlyInstalment = applicationProductEdgeLoan.EdgeInformation.InterestOnlyInstalment;
                _edgeDetails.FullTermInstalment = applicationProductEdgeLoan.EdgeInformation.FullTermInstalment;
            }
        }

        private void PopulateControls()
        {
            _lblMarketRatePerc.Text = _edgeDetails.MarketRate.HasValue ? _edgeDetails.MarketRate.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);
            _lblMarketRatePercCurrent.Text = _edgeDetails.CurrentJibar.HasValue ? _edgeDetails.CurrentJibar.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);

            _isDiscountVisible = _application.HasFinancialAdjustment(FinancialAdjustmentTypeSources.DiscountedLinkrate);

            double linkrate = _edgeDetails.LinkRate.HasValue ? _edgeDetails.LinkRate.Value : 0D;
            double pricingAdjustment = _application.GetRateAdjustments();
            double effectiveRateCurrent = _edgeDetails.CurrentJibar.Value + _edgeDetails.DiscountedLinkRate.Value + pricingAdjustment;
            //
            double amortisingInstalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(_edgeDetails.LoanAmount, effectiveRateCurrent, (_edgeDetails.FullTerm - _edgeDetails.InterestOnlyTerm), false);
            double interestOnlyInstalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(_edgeDetails.LoanAmount, effectiveRateCurrent, -1, true);
            double fullTermInstalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(_edgeDetails.LoanAmount, effectiveRateCurrent, _edgeDetails.FullTerm, false);
            //
            double effectiveRate = _edgeDetails.MarketRate.Value + _edgeDetails.DiscountedLinkRate.Value + pricingAdjustment;
            //
            _lblLinkRate.Text = linkrate.ToString(SAHL.Common.Constants.RateFormat);
            _lblLinkRateCurrent.Text = linkrate.ToString(SAHL.Common.Constants.RateFormat);
            _nudDiscount.Minimum = -(linkrate * 100);
            _lblPricingAdjustment.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);
            _lblPricingAdjustmentCurrent.Text = pricingAdjustment.ToString(SAHL.Common.Constants.RateFormat);
            _lblEffectiveRate.Text = effectiveRate.ToString(SAHL.Common.Constants.RateFormat);
            _lblEffectiveRateCurrent.Text = effectiveRateCurrent.ToString(SAHL.Common.Constants.RateFormat);
            // 1               
            _lblAmortisingInstalment.Text = _edgeDetails.AmortisationTermInstalment.HasValue ? _edgeDetails.AmortisationTermInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : (0D).ToString(SAHL.Common.Constants.CurrencyFormat);
            _lblAmortisingInstalmentCurrent.Text = amortisingInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            // 2
            _lblFullTermInstalment.Text = _edgeDetails.FullTermInstalment.HasValue ? _edgeDetails.FullTermInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : (0D).ToString(SAHL.Common.Constants.CurrencyFormat);
            _lblFullTermInstalmentCurrent.Text = fullTermInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            // 3 
            _lblInterestOnlyInstalment.Text = _edgeDetails.InterestOnlyInstalment.HasValue ? _edgeDetails.InterestOnlyInstalment.Value.ToString(SAHL.Common.Constants.CurrencyFormat) : (0D).ToString(SAHL.Common.Constants.CurrencyFormat);
            _lblInterestOnlyInstalmentCurrent.Text = interestOnlyInstalment.ToString(SAHL.Common.Constants.CurrencyFormat);
            //
            _lblFullTerm.Text = _edgeDetails.FullTerm.ToString();
            _lblInterestOnlyTerm.Text = _edgeDetails.InterestOnlyTerm.ToString();
            _lblAmortisationTerm.Text = _edgeDetails.AmortisationTerm.ToString();
            //
            _txtDiscount.Text = _edgeDetails.Discount.HasValue ? (_edgeDetails.Discount.Value * 100).ToString() : (0D).ToString();
            _lblDiscount.Text = _edgeDetails.Discount.HasValue ? _edgeDetails.Discount.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);
            _lblDiscountCurrent.Text = _edgeDetails.Discount.HasValue ? _edgeDetails.Discount.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);
            _lblDiscountedLinkRate.Text = _edgeDetails.DiscountedLinkRate.HasValue ? _edgeDetails.DiscountedLinkRate.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);
            _lblDiscountedLinkRateCurrent.Text = _edgeDetails.DiscountedLinkRate.HasValue ? _edgeDetails.DiscountedLinkRate.Value.ToString(SAHL.Common.Constants.RateFormat) : (0D).ToString(SAHL.Common.Constants.RateFormat);
        }

        public void Reload(bool discountEditable)
        {
            // Get the data from the _application object
            PopulateLoanDetailProperties();

            PopulateControls();

            if (discountEditable)
            {
                _rowDiscountInput.Attributes.Add("style", "display: inline");
                _rowDiscountLabel.Attributes.Add("style", "display: none");
            }
            else
            {
                _rowDiscountInput.Attributes.Add("style", "display: none");
                _rowDiscountLabel.Attributes.Add("style", "display: inline");
            }
            //we need to add the item to table2 if one of the above are added
            _rowDiscountLabelJB.Attributes.Add("style", "display: inline");

        }

        protected void AddRowDiscountInput(string cellCaption, Control control, WebControl webcontrol, HtmlTable parentTable)
        {

            HtmlTableCell rowCaption = new HtmlTableCell();
            rowCaption.Attributes.Add("class", "TitleText");
            rowCaption.InnerText = cellCaption;
            _rowDiscountInput.Cells.Add(rowCaption);

            HtmlTableCell rowValue = new HtmlTableCell();
            rowValue.Controls.Add(webcontrol);
            rowValue.Controls.Add(control);
            _rowDiscountInput.Cells.Add(rowValue);
            parentTable.Rows.Add(_rowDiscountInput);
        }

        protected static void AddRow(string cellCaption, WebControl control, bool visible, HtmlTableRow tableRow, HtmlTable parentTable)
        {
            HtmlTableCell rowCaption = new HtmlTableCell();
            rowCaption.Attributes.Add("class", "TitleText");
            rowCaption.InnerText = cellCaption;
            tableRow.Cells.Add(rowCaption);

            HtmlTableCell rowValue = new HtmlTableCell();
            rowValue.Controls.Add(control);
            tableRow.Cells.Add(rowValue);

            if (!visible)
                tableRow.Attributes.Add("style", "display: none");

            parentTable.Rows.Add(tableRow);
        }

        /// <summary>
        /// A helper method to render the caption-control pairs for each row.
        /// </summary>
        /// <param name="cellCaption"></param>
        /// <param name="control"></param>
        /// <param name="visible"></param>
        /// <param name="parentControl"></param>
        protected static void AddRowContents(string cellCaption, WebControl control, bool visible, HtmlTable parentControl)
        {
            HtmlTableRow htmlTableRow = new HtmlTableRow();
            HtmlTableCell rowCaption = new HtmlTableCell();
            rowCaption.Attributes.Add("class", "TitleText");
            rowCaption.InnerText = cellCaption;
            htmlTableRow.Cells.Add(rowCaption);

            HtmlTableCell rowValue = new HtmlTableCell();
            rowValue.Controls.Add(control);
            htmlTableRow.Cells.Add(rowValue);

            if (!visible)
                htmlTableRow.Attributes.Add("style", "display: none");

            parentControl.Rows.Add(htmlTableRow);
        }

        protected static void AddRowContentsIO(string cellCaption, WebControl control, bool visible, HtmlTable parentControl, HtmlTableRow htmlTableRow)
        {
            HtmlTableCell rowCaption = new HtmlTableCell();
            rowCaption.Attributes.Add("class", "TitleText");
            rowCaption.InnerText = cellCaption;
            htmlTableRow.Cells.Add(rowCaption);

            HtmlTableCell rowValue = new HtmlTableCell();
            rowValue.Controls.Add(control);
            htmlTableRow.Cells.Add(rowValue);

            if (!visible)
                htmlTableRow.Attributes.Add("style", "display: none");

            parentControl.Rows.Add(htmlTableRow);
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
        /// Get/set the discount value
        /// </summary>
        public double Discount
        {
            get { return (Convert.ToDouble(_txtDiscount.Text.Length > 0 ? _txtDiscount.Text : "0") / 100); }
        }

        /// <summary>
        /// Sets whether the control is rendered in a readonly mode.
        /// </summary>
        public bool IsReadOnly
        {
            set
            {
                _isReadOnly = value;
                _isDiscountReadOnly = value;
            }
            get { return _isReadOnly; }
        }

        public bool IsDiscountVisible
        {
            set { _isDiscountVisible = value; }
            get { return _isDiscountVisible; }
        }

        public bool IsDiscountReadOnly
        {
            set { _isDiscountReadOnly = value; }
            get { return _isDiscountReadOnly; }
        }

        public IApplication Application
        {
            set
            {
                _application = value;
            }
        }
    }

    /// <summary>
    /// Used to ease the passing of LoanDetails information.
    /// </summary>
    public class EdgeDetails
    {
        #region Private Declarations

        private double? _fullTermInstalment;
        private double? _amortisationTermInstalment;
        private double? _interestOnlyInstalment;
        private int _interestOnlyTerm;
        private int _amortisationTerm;
        private int _fullTerm;
        private double? _marketRate;
        private int? _linkRateKey;
        private double? _effectiveRate;
        private double? _discountedLinkRate;
        private double? _discount;
        private double? _linkRate;
        private double _loanAmount;
        private double? _currentJibar;

        #endregion

        #region Constructors

        public EdgeDetails()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public double? FullTermInstalment
        {
            set { _fullTermInstalment = value; }
            get { return _fullTermInstalment; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? AmortisationTermInstalment
        {
            set { _amortisationTermInstalment = value; }
            get { return _amortisationTermInstalment; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? InterestOnlyInstalment
        {
            set { _interestOnlyInstalment = value; }
            get { return _interestOnlyInstalment; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int FullTerm
        {
            set { _fullTerm = value; }
            get { return _fullTerm; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int InterestOnlyTerm
        {
            set { _interestOnlyTerm = value; }
            get { return _interestOnlyTerm; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AmortisationTerm
        {
            set { _amortisationTerm = value; }
            get { return _amortisationTerm; }
        }


        /// <summary>
        /// Nullable property. Indicates the Market Rate of the Loan.
        /// </summary>
        public double? MarketRate
        {
            set { _marketRate = value; }
            get { return _marketRate; }
        }

        /// <summary>
        /// Nullable property. Refers to the key of the Link Rate in the lookup.
        /// </summary>
        public int? LinkRateKey
        {
            set { _linkRateKey = value; }
            get { return _linkRateKey; }
        }

        /// <summary>
        /// Nullable property. 
        /// </summary>
        public double? EffectiveRate
        {
            set { _effectiveRate = value; }
            get { return _effectiveRate; }
        }

        public double LoanAmount
        {
            set { _loanAmount = value; }
            get { return _loanAmount; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? Discount
        {
            set { _discount = value; }
            get { return _discount; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? DiscountedLinkRate
        {
            set { _discountedLinkRate = value; }
            get { return _discountedLinkRate; }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? LinkRate
        {
            set { _linkRate = value; }
            get { return _linkRate; }
        }

        public double? CurrentJibar
        {
            set { _currentJibar = value; }
            get { return _currentJibar; }
        }

        #endregion
    }
}
