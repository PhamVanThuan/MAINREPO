using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.SendClientSurveyExternalEmailCommandHandlerSpecs
{
    //will always be true
    //this test is mearly here to ensure that should the method change at any point
    //the test should be updated.
    [Subject(typeof(SendClientSurveyExternalEmailCommandHandler))]
    internal class When_SendClientSurveyExternalEmailCommand_Handled : DomainServiceSpec<SendClientSurveyExternalEmailCommand, SendClientSurveyExternalEmailCommandHandler>
    {
        protected static ISurveyRepository surveyRepository;
        //Arrange
        Establish context = () =>
        {
            surveyRepository = An<ISurveyRepository>();
            command = new SendClientSurveyExternalEmailCommand(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything);
            handler = new SendClientSurveyExternalEmailCommandHandler(surveyRepository);
        };
        //Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        //Assert
        It should_send_mail = () =>
        {
            surveyRepository.WasToldTo(x => x.SendClientSurveyEmail(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything, Param.Is(true)));
        };
    }
}