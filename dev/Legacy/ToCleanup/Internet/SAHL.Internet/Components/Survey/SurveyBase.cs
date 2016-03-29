using SAHL.Internet.SAHL.Web.Services.Survey;

namespace SAHL.Internet.Components.Survey
{
    /// <summary>
    ///  This Class is used as an interim method for storing preprospects in SAHLDB
    /// This Class will need to be modified to add the Pre-Prospect to the leads workflow
    /// in 2AM when HALO goes live
    /// </summary>
    public class SurveyBase
    {
        private readonly WebServiceBase _webserviceBase;

        /// <summary>
        /// Returns the client questionnaire with the specified Guid.
        /// </summary>
        /// <param name="guid">The Guid of the questionnaire to locate.</param>
        /// <returns>
        /// A client questionnaire with the specified Guid. 
        /// </returns>
        public ClientQuestionnaire GetClientQuestionnaireByGuid(string guid)
        {
            return _webserviceBase.Survey.GetClientQuestionnaireByGUID(guid);
        }

        /// <summary>
        /// Submits the questionnaire result.
        /// </summary>
        /// <param name="surveyResult">The <see cref="SurveyResult"/> to save.</param>
        /// <returns></returns>
        public bool SaveClientQuestionnaire(SurveyResult surveyResult)
        {
            return _webserviceBase.Survey.SaveClientQuestionnaire(surveyResult);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SurveyBase"/> class.
        /// </summary>
        public SurveyBase()
        {
            _webserviceBase = new WebServiceBase();
        }
    }
}