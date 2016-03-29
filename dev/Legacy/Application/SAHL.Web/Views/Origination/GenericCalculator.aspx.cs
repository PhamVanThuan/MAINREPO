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
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.AJAX;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Origination
{
    public partial class GenericCalculator : SAHLCommonBaseView, IGenericCalculator
    {
        private IList<IOriginationSource> _companyList;
        private Dictionary<int, string> _linkRateList;
        private Dictionary<string, double> _calcDict;

        private double _marketRate;
        private int _zeroValue;
        private int _term;
        private double _currentBalance;
        private double _instalmentTotal;
        private int _term240 = 240;
        private bool _enableAmortisationSchedule;
        private bool _reload;
        private string _linkRateSelectedValue;
        private string _companySelectedValue;
        private string _calcSelected;
        private double _maxBondAmount;
        private int _maxTerm;
        private const string _monthFormat = "##0";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

            this.RegisterWebService(ServiceConstants.LinkRates);

            cboLnkRate.Attributes.Add("onchange", "SetInterestRate()");

            // initialise controls
            _zeroValue = 0;
            txtCurrentBalance.Amount = 0;
            lblInstallmentCapital.Text = _zeroValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblInstallmentInterest.Text = _zeroValue.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblTotalInterest.Text = _zeroValue.ToString(SAHL.Common.Constants.CurrencyFormat);

            // bind the data
            BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage)
                return; 

            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(txtRemainingTerm.Text))
                    txtRemainingTerm.Text = _term240.ToString(_monthFormat);
                txtCurrentBalance.Amount = 0; // _LV.ToString(SAHL.Common.Constants.CurrencyFormat);
                ValueToCalculate_SelectedIndexChanged(sender, e);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

            switch (cboValueToCalculate.SelectedValue)
            {
                case "Term":
                    txtRemainingTerm.ReadOnly = true;
                    txtCurrentBalance.ReadOnly = false;
                    txtInstallmentTotal.ReadOnly = false;

                    txtRemainingTerm.CssClass = txtRemainingTerm.CssClass + " genericCalcResult";
                    txtRemainingTerm.Mandatory = false;
                    txtCurrentBalance.Mandatory = true;
                    txtInstallmentTotal.Mandatory = true;
                    break;
                case "Instalment":
                    txtRemainingTerm.ReadOnly = false;
                    txtCurrentBalance.ReadOnly = false;
                    txtInstallmentTotal.ReadOnly = true;
                    txtInstallmentTotal.CssClass = txtInstallmentTotal.CssClass + " genericCalcResult";
                    txtInstallmentTotal.Mandatory = false;
                    txtRemainingTerm.Mandatory = true;
                    txtCurrentBalance.Mandatory = true;
                    break;
                case "Current Balance":
                    txtRemainingTerm.ReadOnly = false;
                    txtCurrentBalance.ReadOnly = true;
                    txtInstallmentTotal.ReadOnly = false;

                    txtCurrentBalance.CssClass = txtCurrentBalance.CssClass + " genericCalcResult";
                    txtCurrentBalance.Mandatory = false;
                    txtRemainingTerm.Mandatory = true;
                    txtInstallmentTotal.Mandatory = true;
                    break;
            }

            // set the Interest Rate
            lblInterestRate.Text = GetInterestRate().ToString(SAHL.Common.Constants.RateFormat);

            Amortisation.Enabled = _enableAmortisationSchedule;
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            Amortisation.Enabled = false;
            _enableAmortisationSchedule = false;
            Navigator.Navigate(ViewName);
        }

        protected void AmortisationScheduleButton_Click(object sender, EventArgs e)
        {
            CalculateButton_Click(sender, e);

            OnAmortisationScheduleButtonClicked(sender, e);
        }

        protected void CalculateButton_Click(object sender, EventArgs e)
        {
            if (ValidateValues())
            {
                if (CalculateValues())
                {
                    Amortisation.Enabled = true;
                    _enableAmortisationSchedule = true;
                }
                else
                {
                    Amortisation.Enabled = false;
                    _enableAmortisationSchedule = false;
                }
            }
        }

        protected void cboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnCompanySelectedIndexChanged(sender, new KeyChangedEventArgs(Request.Form[cboCompany.UniqueID]));
        }

        protected void ValueToCalculate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Request.Form[Calculate.UniqueID] == null)
            {
                Amortisation.Enabled = false;
                _enableAmortisationSchedule = false;

                if (ReloadView)
                {
                    LoadScreenFromDictionary();
                }
                else
                {
                    lblInstallmentCapital.Text = _zeroValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblInstallmentInterest.Text = _zeroValue.ToString(SAHL.Common.Constants.CurrencyFormat);
                    lblTotalInterest.Text = _zeroValue.ToString(SAHL.Common.Constants.CurrencyFormat);

                    if (String.IsNullOrEmpty(txtRemainingTerm.Text))
                        txtRemainingTerm.Text = _term240.ToString(_monthFormat);

                    switch (cboValueToCalculate.SelectedValue)
                    {
                        case "Term":
                            txtRemainingTerm.Text = "";
                            break;
                        case "Instalment":
                            txtInstallmentTotal.Amount = 0;
                            break;
                        case "Current Balance":
                            txtCurrentBalance.Amount = 0;
                            break;
                    }
                }
            }
        }

        #region Custom Methods

        private void BindData()
        {
            if (_companyList.Count == 0)
                throw new Exception("No Company Access Found");

            // bind the Company dropdown
            cboCompany.DataSource = _companyList;
            cboCompany.DataValueField = "Key";
            cboCompany.DataTextField = "Description";
            cboCompany.DataBind();

            // bind the Value To Calculate dropdown
            cboValueToCalculate.Items.Clear();
            cboValueToCalculate.Items.Add(new ListItem("Instalment", "Instalment"));
            cboValueToCalculate.Items.Add(new ListItem("Term", "Term"));
            cboValueToCalculate.Items.Add(new ListItem("Current Balance", "Current Balance"));

            // bind the Link Rates dropdown
            cboLnkRate.DataSource = _linkRateList;
            cboLnkRate.DataBind();

            // set the Market Rate
            lblMarketRate.Text = _marketRate.ToString(SAHL.Common.Constants.RateFormat);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        private double GetInterestRate()
        {
            double linkRate = 0;
            Double.TryParse(cboLnkRate.SelectedItem.Text.Replace("%", string.Empty), out linkRate);
            return (linkRate / 100) + _marketRate;
        }

        private bool ValidateValues()
        {            
            bool validTerm = true;
            bool validCurrentBalance = true;
            bool validInstalment = true;

            switch (cboValueToCalculate.SelectedValue)
            {
                case "Instalment":
                    validTerm = ValidateRemainingTerm();
                    validCurrentBalance = ValidateCurrentBalance();
                    return (validTerm && validCurrentBalance);

                case "Term":
                    validCurrentBalance = ValidateCurrentBalance();
                    validInstalment = ValidateInstalment();
                    return (validCurrentBalance && validInstalment);

                case "Current Balance":
                    validTerm = ValidateRemainingTerm();
                    validInstalment = ValidateInstalment();
                    return (validTerm && validInstalment);
            }

            return false;
        }

        bool ValidateRemainingTerm()
        {
            string textVal = Request.Form[txtRemainingTerm.UniqueID];
            int remainingTerm = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToInt32(textVal);
            string errorMessage = "";

            if (String.IsNullOrEmpty(textVal))
            {
                errorMessage = "Please enter a remaining term.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }
            if (remainingTerm <= 0)
            {
                errorMessage = "The remaining term must be greater than zero.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            if (remainingTerm > _maxTerm)
            {
                errorMessage = "The remaining term must be less or equal " + _maxTerm + "  months.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }
            return true;
        }

        bool ValidateCurrentBalance()
        {
            string textVal = Request.Form[txtCurrentBalance.UniqueID];
            double currentBalance = String.IsNullOrEmpty(textVal) ? 0 : Convert.ToDouble(textVal);

            string errorMessage = "";

            if (currentBalance <= 0)
            {
                errorMessage = "The current balance must be greater than zero.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }
            if (currentBalance > _maxBondAmount) 
            {
                errorMessage = "The current balance must be less than or equal to " + _maxBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat); 
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }
            return true;
        }

        bool ValidateInstalment()
        {
            double instalmentTotal = txtInstallmentTotal.Amount.HasValue ? txtInstallmentTotal.Amount.Value : 0;
            double currentBalance = txtCurrentBalance.Amount.HasValue ? txtCurrentBalance.Amount.Value : 0;

            double instalmentValidation = 0;
            string errorMessage = "";

            if (instalmentTotal == 0)
            {
                errorMessage = "The instalment must be greater than zero.";
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            if (instalmentTotal > _maxBondAmount)
            {
                errorMessage = "The instalment must be less than or equal to " + _maxBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat); 

                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            instalmentValidation = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, (InterestRate), _maxTerm, false);
            
            // we must only check to 2 decimal places - which is why we are doing this string manipulation below
            string instalmentString = instalmentValidation.ToString("###########0.00");
            instalmentValidation = Convert.ToDouble(instalmentString);

            if (instalmentValidation > instalmentTotal)
            {
                string Amnt = instalmentValidation.ToString(SAHL.Common.Constants.CurrencyFormat);
                errorMessage = string.Format("The instalment must be more than {0}", Amnt);
                this.Messages.Add(new Error(errorMessage, errorMessage));
                return false;
            }

            return true;
        }

        private bool CalculateValues()
        {
            double totalInterest = 0;       // Total Interest
            double instalmentCapital = 0;   // Instalment Capital
            double instalmentInterest = 0;  // Instalment interest

            string errorMessage = "";

            double currentBalance = txtCurrentBalance.Amount.HasValue ? txtCurrentBalance.Amount.Value : 0;
            double instalmentTotal = txtInstallmentTotal.Amount.HasValue ? txtInstallmentTotal.Amount.Value : 0;
            int remainingTerm = String.IsNullOrEmpty(txtRemainingTerm.Text) ? 0 : Convert.ToInt32(txtRemainingTerm.Text);

            switch (cboValueToCalculate.SelectedValue)
            {
                case "Instalment":
                    // Interest Rate
                    // Installment: Total
                    // Installment: Interest
                    // Installment: Capital
                    // Total Interest

                    instalmentTotal = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, InterestRate, remainingTerm, false);
                    if (instalmentTotal > _maxBondAmount)
                    {
                        errorMessage = "The resulting instalment will be greater than " + _maxBondAmount.ToString(SAHL.Common.Constants.CurrencyFormat); 

                        this.Messages.Add(new Error(errorMessage, errorMessage));
                        return false;
                    }

                    SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstalmentPortion(currentBalance, InterestRate, instalmentTotal, 0D, out instalmentInterest, out instalmentCapital);
                    totalInterest = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateTotalInterest(instalmentTotal, remainingTerm, currentBalance);

                    break;
                case "Term":
                    // Interest Rate
                    // Remaining Term
                    // Installment: Interest
                    // Installment: Capital
                    // Total Interest

                    double newTerm = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateLoanTerm(currentBalance, InterestRate, instalmentTotal);
                    if ((int)(Math.Ceiling(newTerm)) - newTerm > 0)
                    {
                        //we have rounded the value
                        instalmentTotal = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(currentBalance, InterestRate, (int)(Math.Ceiling(newTerm)), false);
                        instalmentTotal = ((double)((int)(Math.Ceiling(instalmentTotal * 100)))) / 100;
                    }

                    remainingTerm = (int)(Math.Ceiling(newTerm));

                    SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstalmentPortion(currentBalance, InterestRate, instalmentTotal, 0D, out instalmentInterest, out instalmentCapital);
                    totalInterest = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateTotalInterest(instalmentTotal, remainingTerm, currentBalance);

                    txtRemainingTerm.Text = remainingTerm.ToString(_monthFormat);

                    break;
                case "Current Balance":
                    // Interest Rate
                    // Current Balance
                    // Installment: Interest
                    // Installment: Capital
                    // Total Interest

                    currentBalance = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateCurrentBalance(instalmentTotal, remainingTerm, InterestRate);

                    SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstalmentPortion(currentBalance, InterestRate, instalmentTotal, 0D, out instalmentInterest, out instalmentCapital);
                    totalInterest = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateTotalInterest(instalmentTotal, remainingTerm, currentBalance);

                    break;
            }

            txtCurrentBalance.Amount = currentBalance; //.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblInstallmentCapital.Text = instalmentCapital.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblInstallmentInterest.Text = instalmentInterest.ToString(SAHL.Common.Constants.CurrencyFormat);
            txtInstallmentTotal.Amount = instalmentTotal; //.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblTotalInterest.Text = totalInterest.ToString(SAHL.Common.Constants.CurrencyFormat);

            return true;
        }

        private void LoadScreenFromDictionary()
        {
            txtRemainingTerm.Text = _term.ToString(_monthFormat);
            txtCurrentBalance.Amount = _currentBalance; 
            txtInstallmentTotal.Amount = _instalmentTotal; 
            cboLnkRate.SelectedValue = _linkRateSelectedValue;
            cboCompany.SelectedValue = _companySelectedValue;
            cboValueToCalculate.SelectedValue = _calcSelected;

            CalculateValues();
        }

        #endregion

        #region IGenericCalculator Members

        public event KeyChangedEventHandler OnCompanySelectedIndexChanged;

        public event EventHandler OnAmortisationScheduleButtonClicked;

        public bool EnableAmortisation
        {
            get
            {
                return _enableAmortisationSchedule;
            }
            set
            {
                _enableAmortisationSchedule = value;
            }
        }

        public IList<IOriginationSource> CompanyList
        {
            get
            {
                return _companyList;
            }
            set
            {
                _companyList = value;
            }
        }

        public Dictionary<int, string> LinkRateList
        {
            get 
            {
                if (_linkRateList == null)
                    _linkRateList = new Dictionary<int, string>();

                return _linkRateList;
            }
        }

        public Dictionary<string, double> CalcDict
        {
            get
            {
                if (_calcDict == null)
                    _calcDict = new Dictionary<string, double>();

                return _calcDict;
            }
        }

        public double MarketRate
        {
            get { return _marketRate; }
            set { _marketRate = value; }
        }

        public double InterestRate
        {
            get 
            {
                return GetInterestRate();
            }
        }

        public int Term
        {
            get { return String.IsNullOrEmpty(txtRemainingTerm.Text) ? 0 : Convert.ToInt32(txtRemainingTerm.Text); }
            set { _term = value; }
        }

        public double CurrentBalance
        {
            get {return txtCurrentBalance.Amount.HasValue ? txtCurrentBalance.Amount.Value : 0; }
            set {_currentBalance = value; }
        }

        public double InstalmentTotal
        {
            get {return txtInstallmentTotal.Amount.HasValue ? txtInstallmentTotal.Amount.Value : 0; }
            set {_instalmentTotal = value;}
        }

        public string CompanySelectedValue
        {
            get {return cboCompany.SelectedValue;}
            set { _companySelectedValue = value; }
        }

        public string LinkRateSelectedValue
        {
            get {return cboLnkRate.SelectedValue;}
            set {_linkRateSelectedValue = value;}
        }

        public string CalcType
        {
            get {return cboValueToCalculate.SelectedValue;}
            set {_calcSelected = value; }
        }

        public bool ReloadView
        {
            get {return _reload;}
            set {_reload = value;}
        }

        public double MaxBondAmount
        {
            get {return _maxBondAmount;}
            set {_maxBondAmount = value;}
        }

        public int MaxTerm
        {
            get {return _maxTerm;}
            set {_maxTerm = value;}
        }

        #endregion
    }
}
