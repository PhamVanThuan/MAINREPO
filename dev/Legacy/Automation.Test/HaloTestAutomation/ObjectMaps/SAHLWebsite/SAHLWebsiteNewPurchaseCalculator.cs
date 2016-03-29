using System;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class SAHLWebsiteNewPurchaseCalculatorControls : BasePageControls
    {
        private Browser b;

        public SAHLWebsiteNewPurchaseCalculatorControls(Browser browser)
        {
            b = browser;
        }

        public TextField txtPurchasePrice
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr646_XSSmartModule_ctl00_tbPurchasePrice"));
            }
        }

        public TextField txtCashDeposit
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr646_XSSmartModule_ctl00_tbCashDeposit"));
            }
        }

        public TextField txtHouseholdIncome
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr646_XSSmartModule_ctl00_tbHouseholdIncome"));
            }
        }

        public bool IsFixPercentageVisible
        {
            get
            {
                var tableRow = this.b.TableRow(Find.ById("rowVarifix"));
                if (String.IsNullOrEmpty(tableRow.Style.CssText))
                    return true;
                return false;
            }
        }

        public TextField txtFixPercentage
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr646_XSSmartModule_ctl00_tbFixPercentage"));
            }
        }

        public TextField txtLoanTerm
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr646_XSSmartModule_ctl00_tbLoanTerm"));
            }
        }

        public RadioButton rbSalaried
        {
            get
            {
                return b.RadioButton(Find.ById("dnn_ctr646_XSSmartModule_ctl00_rbSalaried"));
            }
        }

        public RadioButton rbSelfEmployed
        {
            get
            {
                return b.RadioButton(Find.ById("dnn_ctr646_XSSmartModule_ctl00_rbSelfEmployed"));
            }
        }

        public RadioButton rbVariableRateLoan
        {
            get
            {
                return b.RadioButton(Find.ById("dnn_ctr646_XSSmartModule_ctl00_rbProduct3"));
            }
        }

        public RadioButton rbFixedRateLoan
        {
            get
            {
                return b.RadioButton(Find.ById("dnn_ctr646_XSSmartModule_ctl00_rbProduct1"));
            }
        }

        public CheckBox chkInterestOnly
        {
            get
            {
                return b.CheckBox(Find.ById("dnn_ctr646_XSSmartModule_ctl00_chkInterestOnly"));
            }
        }

        public Element btnCalculate
        {
            get
            {
                return b.Element(Find.ById("dnn_ctr646_XSSmartModule_ctl00_calcButton"));
            }
        }

        public Element btnApply
        {
            get
            {
                return b.Element(Find.ById("dnn_ctr646_XSSmartModule_ctl00_btnApply"));
            }
        }

        public Div ErrorText
        {
            get
            {
                return b.Div(Find.ByClass("errorText"));
            }
        }
    }
}