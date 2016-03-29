using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class SendClientSurveyInternalEmailCommandHandler : IHandlesDomainServiceCommand<SendClientSurveyInternalEmailCommand>
    {
        ISurveyRepository surveyRepository;

        public SendClientSurveyInternalEmailCommandHandler(ISurveyRepository surveyRepository)
        {
            this.surveyRepository = surveyRepository;
        }

        public void Handle(IDomainMessageCollection messages, SendClientSurveyInternalEmailCommand command)
        {
            surveyRepository.SendClientSurveyEmail(command.BusinessEventQuestionnaireKey, command.ApplicationKey, command.ADUserName, false);
        }
    }
}