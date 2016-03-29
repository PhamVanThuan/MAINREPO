using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class SAHLWebsiteClientSurveyControls : BasePageControls
    {
        private Browser b;

        public SAHLWebsiteClientSurveyControls(Browser browser)
        {
            b = browser;
        }

        public Table SurveyAnswerTable
        {
            get
            {
                return b.Table("dnn_ctr888_XSSmartModule_ctl00_tblSurvey");
            }
        }

        public TextField Comments
        {
            get
            {
                return this.b.TextField(new Regex("answerControlComment"));
            }
        }

        public Element Submit
        {
            get
            {
                return this.b.Element(Find.ById(new Regex("ctl00_btnSubmit")));
            }
        }
    }
}