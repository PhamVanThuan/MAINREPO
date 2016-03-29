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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Utils;
using SAHL.Common.Globals;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Display instalment breakdown information on a panel.  
    /// <para>
    /// <list type="bullet">
    ///     <listheader>
    ///         <description>Notes on usage</description>
    ///     </listheader>
    ///     <item>
    ///         <description>
    ///         This control needs to be populated with an instalment record on every page load.  
    ///         It will, however, retain values in input fields on postback.
    ///         </description>
    ///     </item>
    ///    
    /// </list>
    /// </para>This control needs to be supplied with an instalment 
    /// record on every page load
    /// </summary>
    public class InstalmentBreakdown : DetailsPanel
    {

        private Panel _pnlMain;

        private IAccountRepository _accountRepository;
        private IAccount _account;
      
        private SAHLLabel _lblTotalLoanInstalment;
        private SAHLLabel _lblTotalShortTermLoanInstalment;
        private SAHLLabel _lblTotalPremiums;
        private SAHLLabel _lblTotalAmountDue;
        private SAHLLabel _lblFixedDebitOrderAmount;
        private SAHLLabel _lblMonthsInArrears;
        private SAHLLabel _lblAmmortisingInstallment;
        private bool _isInterestOnly;          


        /// <summary>
        /// Constructor.  Sets default values for the control.
        /// </summary>
        public InstalmentBreakdown()
        {
            GroupingText = "Instalment Breakdown";
            if (DesignMode) return;

            _pnlMain = new Panel();
            _pnlMain.ID = "pnlMain";
            this.Controls.Add(_pnlMain);

            _lblTotalLoanInstalment = new SAHLLabel();
            _lblTotalLoanInstalment.ID = "lblTotalLoanInstalment";
            _pnlMain.Controls.Add(_lblTotalLoanInstalment);

            _lblTotalShortTermLoanInstalment = new SAHLLabel();
            _lblTotalShortTermLoanInstalment.ID = "lblTotalShortTermLoanInstalment";
            _pnlMain.Controls.Add(_lblTotalShortTermLoanInstalment);

            _lblTotalPremiums = new SAHLLabel();
            _lblTotalPremiums.ID = "lblTotalPremiums";
            _pnlMain.Controls.Add(_lblTotalPremiums);

            _lblTotalAmountDue = new SAHLLabel();
            _lblTotalAmountDue.ID = "lblTotalAmountDue";
            _pnlMain.Controls.Add(_lblTotalAmountDue);

            _lblFixedDebitOrderAmount = new SAHLLabel();
            _lblFixedDebitOrderAmount.ID = "lblFixedDebitOrderAmount";
            _pnlMain.Controls.Add(_lblFixedDebitOrderAmount);

            _lblMonthsInArrears = new SAHLLabel();
            _lblMonthsInArrears.ID = "lblMonthsInArrears";
            _pnlMain.Controls.Add(_lblMonthsInArrears);

            if (_isInterestOnly)
            {
                _lblAmmortisingInstallment = new SAHLLabel();
                _lblAmmortisingInstallment.ID = "lblAmmortisingInstallment";
                _pnlMain.Controls.Add(_lblAmmortisingInstallment);
            }
        }

        #region Properties

        /// <summary>
        /// Sets the account record for which the installment breakdown information should be displayed.
        /// </summary>
        /// <returns></returns>
        public IAccount account
        {

            set
            {
                _account = value;

                if (_isInterestOnly)
                {
                    _lblAmmortisingInstallment.Text = _account.InstallmentSummary.AmortisingInstallment.ToString();
                }
                _lblFixedDebitOrderAmount.Text = _account.FixedPayment.ToString();
                _lblMonthsInArrears.Text = _account.InstallmentSummary.MonthsInArrears.ToString();
                _lblTotalAmountDue.Text = _account.InstallmentSummary.TotalAmountDue.ToString();
                _lblTotalLoanInstalment.Text = _account.InstallmentSummary.TotalLoanInstallment.ToString();
                _lblTotalPremiums.Text = _account.InstallmentSummary.TotalPremium.ToString();
                _lblTotalShortTermLoanInstalment.Text = _account.InstallmentSummary.TotalShortTermLoanInstallment.ToString();
                _isInterestOnly = _account.InstallmentSummary.IsInterestOnly;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        public bool InterestOnlyInstalment
        {
            get
            {
                return _isInterestOnly;
            }
        }

        #endregion

        #region Methods

        private void AddRow(string title, string displayValue, Control c, bool readOnly)
        {
            if (readOnly)
                base.AddRow(_pnlMain, title, displayValue);
            else
            {
                base.AddRow(_pnlMain, title, c);
            }

            c.Visible = !readOnly;

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DesignMode) return;

            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();

        }

        /// <summary>
        /// Overridden.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (DesignMode) return;

            base.LabelWidth = 250;

            // if account is null, then create a blank one so we don't get null exceptions

            if (_account == null)
                _account = _accountRepository.GetAccountByKey(1235896);

            // add the rows to the display

            AddRow("Total Loan Instalment", _account.InstallmentSummary.TotalLoanInstallment.ToString(SAHL.Common.Constants.CurrencyFormat), _lblTotalLoanInstalment, true);
            AddRow("Total Short Term Loan Instalment", _account.InstallmentSummary.TotalShortTermLoanInstallment.ToString(SAHL.Common.Constants.CurrencyFormat), _lblTotalShortTermLoanInstalment, true);
            AddRow("Total Premiums", _account.InstallmentSummary.TotalPremium.ToString(SAHL.Common.Constants.CurrencyFormat), _lblTotalPremiums, true);
            AddRow("Total Amount Due", _account.InstallmentSummary.TotalAmountDue.ToString(SAHL.Common.Constants.CurrencyFormat), _lblTotalAmountDue, true);
            AddRow("Fixed Debit Order Amount", _account.FixedPayment.ToString(SAHL.Common.Constants.CurrencyFormat), _lblFixedDebitOrderAmount, true);
            AddRow("Months in Arrears", Math.Round(_account.InstallmentSummary.MonthsInArrears,2).ToString() + " Months", _lblMonthsInArrears, true);
            if (_isInterestOnly)
            {
                AddRow("Amortising Instalment", _account.InstallmentSummary.MonthsInArrears.ToString(), _lblAmmortisingInstallment, true);
            }
            


        }

        #endregion
    }
}
