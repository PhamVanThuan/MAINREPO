using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class SendClientSurveyExternalEmailCommandHandler : IHandlesDomainServiceCommand<SendClientSurveyExternalEmailCommand>
    {
        ISurveyRepository surveyRepository;

        public SendClientSurveyExternalEmailCommandHandler(ISurveyRepository surveyRepository)
        {
            this.surveyRepository = surveyRepository;
        }

        public void Handle(IDomainMessageCollection messages, SendClientSurveyExternalEmailCommand command)
        {
            surveyRepository.SendClientSurveyEmail(command.BusinessEventQuestionnaireKey, command.ApplicationKey, command.ADUserName, true);
        }
    }
}