using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class SAHLWebsiteApplicantDetailsControls : BasePageControls
    {
        private Browser b;

        public SAHLWebsiteApplicantDetailsControls(Browser browser)
        {
            b = browser;
        }

        public TextField txtFirstNames
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr642_XSSmartModule_ctl00_txtFirstNames"));
            }
        }

        public TextField txtSurname
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr642_XSSmartModule_ctl00_txtSurname"));
            }
        }

        public TextField txtPhoneCode
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr642_XSSmartModule_ctl00_phCode"));
            }
        }

        public TextField txtPhoneNumber
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr642_XSSmartModule_ctl00_phNumber"));
            }
        }

        public TextField txtNumApplicants
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr642_XSSmartModule_ctl00_tbNumApplicants"));
            }
        }

        public TextField txtEmailAddress
        {
            get
            {
                return b.TextField(Find.ById("dnn_ctr642_XSSmartModule_ctl00_txtEmailAddress"));
            }
        }

        public Element btnSubmit
        {
            get
            {
                return b.Element(Find.ById("dnn_ctr642_XSSmartModule_ctl00_btnApply"));
            }
        }
    }
}