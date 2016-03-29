using System.Web.Services;
using System.ComponentModel;
using SAHL.Common.Globals;
using System.Collections.Generic;


namespace SAHL.Web.Services
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://webservices.sahomeloans.com/",
     Description = "This is the SA Homeloans Survey Web Service.")]

    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Survey : WebService
    {
        [WebMethod(Description = "Gets the ClientQuestionnaire by GUID")]
        public ClientQuestionnaire GetClientQuestionnaireByGUID(string GUID)
        {
            SurveyBase Survey = new SurveyBase();
            return Survey.GetClientQuestionnaireByGUID(GUID);
        }

        [WebMethod(Description = "Saves the ClientQuestionnaire")]
        public bool SaveClientQuestionnaire(SurveyResult surveyResult)
        {
            SurveyBase Survey = new SurveyBase();
            return Survey.SaveClientQuestionnaire(surveyResult);
        }
    }
}
