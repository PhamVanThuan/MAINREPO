using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class SAHLWebsiteAffordabilityCalculatorControls : BasePageControls
    {
        private Browser b;

        public SAHLWebsiteAffordabilityCalculatorControls(Browser browser)
        {
            b = browser;
        }

        public TextField txtIncome01
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr760_XSSmartModule_ctl00_txtIncome1"));
            }
        }

        public TextField txtIncome02
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr760_XSSmartModule_ctl00_txtIncome2"));
            }
        }

        public TextField txtProfitFromSale
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr760_XSSmartModule_ctl00_txtProfitFromSale"));
            }
        }

        public TextField txtOtherContribution
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr760_XSSmartModule_ctl00_txtOtherContribution"));
            }
        }

        public TextField txtMonthlyInstalment
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr760_XSSmartModule_ctl00_txtMonthlyInstalment"));
            }
        }

        public TextField txtLoanTermYears
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr760_XSSmartModule_ctl00_txtLoanTerm"));
            }
        }

        public TextField txtInterestRate
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr760_XSSmartModule_ctl00_txtInterestRate"));
            }
        }

        public Element btnCalculate
        {
            get
            {
                return b.Element(Find.ById("dnn_ctr760_XSSmartModule_ctl00_calcButton"));
            }
        }

        public Element btnApply
        {
            get
            {
                return b.Element(Find.ById("dnn_ctr760_XSSmartModule_ctl00_bntApplyy"));
            }
        }

        public TextField hiddenMaxInstalment
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr760_XSSmartModule_ctl00_hiddenMaxInstalment"));
            }
        }

        public Div ErrorText
        {
            get
            {
                return b.Div(Find.ByClass("errorText"));
            }
        }

        public Div UpToValue
        {
            get
            {
                return b.Div(Find.ById("lblUpToValue"));
            }
        }
    }
}