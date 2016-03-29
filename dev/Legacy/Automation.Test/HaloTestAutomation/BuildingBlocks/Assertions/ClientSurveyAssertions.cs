using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using System;

namespace BuildingBlocks.Assertions
{
    public static class ClientSurveyAssertions
    {
        private static IClientSurveyService clientSurveyService;

        static ClientSurveyAssertions()
        {
            clientSurveyService = ServiceLocator.Instance.GetService<IClientSurveyService>();
        }

        /// <summary>
        /// Checks that answers were saved for the client survey
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string AssertClientSurveyAnswers(string guid)
        {
            QueryResults answersResults = clientSurveyService.GetClientSurveyAnswersByGUID(guid);
            QueryResults clientQuestionnairResults = clientSurveyService.GetClientQuestionnaireByGUID(guid);
            int generickey = clientQuestionnairResults.Rows(0).Column("generickey").GetValueAs<int>();
            if (!answersResults.HasResults)
                return String.Format("No answers saved for client questionnair. GenericKey:{0}", generickey);
            return String.Format("All answers was saved for client questionnair. GenericKey:{0}", generickey);
        }
    }
}