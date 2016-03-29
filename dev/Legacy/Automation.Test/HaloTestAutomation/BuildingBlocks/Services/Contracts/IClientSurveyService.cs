using Automation.DataAccess;

namespace BuildingBlocks.Services.Contracts
{
    public interface IClientSurveyService
    {
        QueryResults GetClientSurveyAnswersByGUID(string guid);

        string GetClientQuestionnaireGUID(int generickey);

        QueryResults GetClientQuestionnaireByGUID(string guid);
    }
}