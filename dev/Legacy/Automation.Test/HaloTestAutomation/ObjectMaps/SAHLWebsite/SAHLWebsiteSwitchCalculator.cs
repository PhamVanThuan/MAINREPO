using System;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class SAHLWebsiteSwitchCalculatorControls : BasePageControls
    {
        private Browser b;

        public SAHLWebsiteSwitchCalculatorControls(Browser browser)
        {
            b = browser;
        }

        public TextField tbMarketValue
        {
            get
            {
                return b.TextField("dnn_ctr647_XSSmartModule_ctl00_tbMarketValue");
            }
        }

        public TextField tbCurrentLoanAmount
        {
            get
            {
                return b.TextField("dnn_ctr647_XSSmartModule_ctl00_tbCurrentLoan");
            }
        }

        public TextField tbCashOut
        {
            get
            {
                return b.TextField("dnn_ctr647_XSSmartModule_ctl00_tbCashOut");
            }
        }

        public RadioButton rbSalaried
        {
            get
            {
                return b.RadioButton("dnn_ctr647_XSSmartModule_ctl00_rbSalaried");
            }
        }

        public RadioButton rbSelfEmployed
        {
            get
            {
                return b.RadioButton("dnn_ctr647_XSSmartModule_ctl00_rbSelfEmployed");
            }
        }

        public TextField tbGrossMonthlyIncome
        {
            get
            {
                return b.TextField("dnn_ctr647_XSSmartModule_ctl00_tbHouseholdIncome");
            }
        }

        public RadioButton rbVariableRateLoan
        {
            get
            {
                return b.RadioButton("dnn_ctr647_XSSmartModule_ctl00_rbProduct3");
            }
        }

        public RadioButton rbFixedRateLoan
        {
            get
            {
                return b.RadioButton("dnn_ctr647_XSSmartModule_ctl00_rbProduct1");
            }
        }

        public TextField tbTermOfLoan
        {
            get
            {
                return b.TextField("dnn_ctr647_XSSmartModule_ctl00_tbLoanTerm");
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
                return b.TextField("dnn_ctr647_XSSmartModule_ctl00_tbFixPercentage");
            }
        }

        public CheckBox chkCapitaliseFees
        {
            get
            {
                return b.CheckBox("dnn_ctr647_XSSmartModule_ctl00_chkCapitaliseFees");
            }
        }

        public CheckBox chkInterestOnly
        {
            get
            {
                return b.CheckBox("dnn_ctr647_XSSmartModule_ctl00_chkInterestOnly");
            }
        }

        public Image btnHowMuchCanISave
        {
            get
            {
                return b.Image("dnn_ctr647_XSSmartModule_ctl00_calcButton");
            }
        }

        public Image btnApplyForAHomeLoanNow
        {
            get
            {
                return b.Image("dnn_ctr647_XSSmartModule_ctl00_btnApply");
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