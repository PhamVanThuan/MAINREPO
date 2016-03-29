using ObjectMaps.Pages;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.InternetComponents
{
    public abstract class ClientSurveyControls : BasePageControls
    {
        public Table SurveyAnswerTable
        {
            get
            {
                return base.Document.Table("dnn_ctr888_XSSmartModule_ctl00_tblSurvey");
            }
        }

        public TextField Comments
        {
            get
            {
                return base.Document.TextField(new Regex("answerControlComment"));
            }
        }

        public Element Submit
        {
            get
            {
                return base.Document.Element(Find.ById(new Regex("ctl00_btnSubmit")));
            }
        }
    }
}