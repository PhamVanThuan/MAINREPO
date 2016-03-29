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
using SAHL.Common.BusinessModel.Interfaces;
using System.Text;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Displays and returns information about the Loan. Use the InterestOnly property to enable Interest Only features.
    /// </summary>
    public class QuickCashPanel : Panel, INamingContainer
    {
        #region Private varibles
        private string _titleText = "Quick Cash Details";
        private HtmlTable _htmlTable;
        private SAHLLabel _lblCashPortion;
        private SAHLLabel _lblMaximumQuickCash;
        private SAHLCurrencyBox _txtTotalAmountApproved;
        private SAHLCurrencyBox _txtUpfrontPaymentApproved;
        private SAHLButton _btnQCDeclineReasons;
        private bool _isReadOnly;
        private bool _hasQCDeclineReasons;
        //private bool _shouldRunPage = true;
        #endregion

        public event EventHandler QCDeclineReasons;

        /// <summary>
        /// Constructor. Initializes a number of controls used.
        /// </summary>
        public QuickCashPanel()
        {
            base.GroupingText = _titleText;

            _htmlTable = new HtmlTable();
            base.Width = new Unit(500, UnitType.Pixel);
            _htmlTable.Width = "99%";

            if (DesignMode)
                return;

            _lblCashPortion = new SAHLLabel();
            _lblCashPortion.ID = "_txtCashPortion";

            _lblMaximumQuickCash = new SAHLLabel();
            _lblMaximumQuickCash.ID = "_txtMaximumQuickCash";

            _txtTotalAmountApproved = new SAHLCurrencyBox();
            _txtTotalAmountApproved.ID = "_txtTotalAmountApproved";
            
            _txtUpfrontPaymentApproved = new SAHLCurrencyBox();
            _txtUpfrontPaymentApproved.ID = "_txtUpfrontPaymentApproved";

            _btnQCDeclineReasons = new SAHLButton();
            _btnQCDeclineReasons.Click += new EventHandler(QCDeclineReasonsClick);
            _btnQCDeclineReasons.Text = "QC Decline Reasons";
            _btnQCDeclineReasons.Width = new Unit(122, UnitType.Pixel);
            _btnQCDeclineReasons.CssClass = "SAHLButton5";


        }

        void QCDeclineReasonsClick(object sender, EventArgs e)
        {
            if (QCDeclineReasons != null)
                QCDeclineReasons(sender, e);
        }

        private void RegisterClientScripts()
        {
            _txtTotalAmountApproved.Attributes.Add("onblur", "editQCValues()");
            _txtUpfrontPaymentApproved.Attributes.Add("onblur", "editQCValues()");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("function editQCValues()");
            sb.AppendLine("{");
            sb.AppendLine("     var ttlAmountApproved = SAHLCurrencyBox_getValue('" + _txtTotalAmountApproved.ClientID + "');");
            sb.AppendLine("     var upfrontPaymentApproved = SAHLCurrencyBox_getValue('" + _txtUpfrontPaymentApproved.ClientID + "');");
            sb.AppendLine("     var btnQCDeclineReasons = document.getElementById('" + _btnQCDeclineReasons.ClientID + "');");
            sb.AppendLine("     if(ttlAmountApproved > 0 || upfrontPaymentApproved > 0)");
            sb.AppendLine("     {");
            sb.AppendLine("         btnQCDeclineReasons.disabled = true;");
            sb.AppendLine("         enableUpdateButton(true);");
            sb.AppendLine("     }");
            sb.AppendLine("     else");
            sb.AppendLine("     {");
            sb.AppendLine("         enableUpdateButton(false);");
            sb.AppendLine("         btnQCDeclineReasons.disabled = false;");
            sb.AppendLine("     }");
            sb.AppendLine("}");

            if (!Page.ClientScript.IsClientScriptBlockRegistered("editQCValues"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "editQCValues", sb.ToString(), true);
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

            base.GroupingText = _titleText;

            AddRowContents("Cash Portion", _lblCashPortion, null);
            AddRowContents("Maximum QC", _lblMaximumQuickCash, null);
            AddRowContents("Total Amount Approved", _txtTotalAmountApproved, null);
            AddRowContents("Upfront Amount Approved", _txtUpfrontPaymentApproved, _btnQCDeclineReasons);

            base.Controls.Add(_htmlTable);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            RegisterClientScripts();
        }

        /// <summary>
        /// A helper method to render the caption-control pairs for each row.
        /// </summary>
        /// <param name="cellCaption"></param>
        /// <param name="control"></param>
        /// <param name="commandButton"></param>
        protected void AddRowContents(string cellCaption, WebControl control, SAHLButton commandButton)
        {

            HtmlTableRow htmlTableRow = new HtmlTableRow();

            HtmlTableCell rowCaption = new HtmlTableCell();
            rowCaption.Attributes.Add("class", "TitleText");
            rowCaption.InnerText = cellCaption;
            htmlTableRow.Cells.Add(rowCaption);

            HtmlTableCell rowValue = new HtmlTableCell();
            rowValue.Controls.Add(control);
            htmlTableRow.Cells.Add(rowValue);

            HtmlTableCell buttonCell = new HtmlTableCell();
            if (commandButton != null)
                buttonCell.Controls.Add(commandButton);

            htmlTableRow.Cells.Add(buttonCell);

            _htmlTable.Rows.Add(htmlTableRow);
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

        #region ShouldRunPage from Rodders
        ///// <summary>
        ///// 
        ///// </summary>
        //public bool ShouldRunPage
        //{
        //    set
        //    {
        //        _shouldRunPage = value;
        //    }
        //}
        #endregion

        /// <summary>
        /// Sets whether the control is rendered in a readonly mode.
        /// </summary>
        public bool IsReadOnly
        {
            set { _isReadOnly = value; }
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Binds the QuickCash Details
        /// </summary>
        /// <param name="ApplicationInformationQuickCash"></param>
        /// <param name="CashPortion"></param>
        /// <param name="HasQCDeclineReasons"></param>
        public void BindQuickCash(IApplicationInformationQuickCash ApplicationInformationQuickCash, double CashPortion, bool HasQCDeclineReasons)
        {
            _hasQCDeclineReasons = HasQCDeclineReasons;

            _lblCashPortion.Text = CashPortion.ToString(SAHL.Common.Constants.CurrencyFormat);
            _lblMaximumQuickCash.Text = ApplicationInformationQuickCash.GetMaximumQuickCash().ToString(SAHL.Common.Constants.CurrencyFormat);
            if (_hasQCDeclineReasons)
            {
                _txtTotalAmountApproved.Text = 0D.ToString();
                _txtUpfrontPaymentApproved.Text = 0D.ToString();
                _txtTotalAmountApproved.Enabled = false;
                _txtUpfrontPaymentApproved.Enabled = false;
            }
            else
            {
                _txtTotalAmountApproved.Enabled = true;
                _txtUpfrontPaymentApproved.Enabled = true;

                _txtTotalAmountApproved.Text = ApplicationInformationQuickCash.CreditApprovedAmount.ToString();
                _txtUpfrontPaymentApproved.Text = ApplicationInformationQuickCash.CreditUpfrontApprovedAmount.ToString();
                

                if (ApplicationInformationQuickCash.CreditApprovedAmount > 0 || ApplicationInformationQuickCash.CreditUpfrontApprovedAmount > 0)
                    _btnQCDeclineReasons.Enabled = false; ;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        public void GetQuickCashDetails(IApplicationInformationQuickCash ApplicationInformationQuickCash)
        {

            double creditApprovedAmount = 0.0;
            double creditUpfrontApprovedAmount = 0.0;

            Double.TryParse(_txtTotalAmountApproved.Text, out creditApprovedAmount);
            Double.TryParse(_txtUpfrontPaymentApproved.Text, out creditUpfrontApprovedAmount);

            if (_hasQCDeclineReasons)
            {
                ApplicationInformationQuickCash.CreditApprovedAmount = 0D;
                ApplicationInformationQuickCash.CreditUpfrontApprovedAmount = 0D;
            }
            else
            {
                ApplicationInformationQuickCash.CreditApprovedAmount = creditApprovedAmount;
                ApplicationInformationQuickCash.CreditUpfrontApprovedAmount = creditUpfrontApprovedAmount;
            }
        }
        /// <summary>
        /// Property to determine whether not yet persisted values have been 
        /// captured by a user
        /// </summary>
        public bool HasQCValue
        {
            get
            {
                if (_txtTotalAmountApproved.Amount.HasValue &&
                    _txtTotalAmountApproved.Amount.Value > 0)
                    return true;

                return false;
            }
        }
    }
}
