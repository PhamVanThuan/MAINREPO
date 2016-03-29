using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.SendClientSurveyInternalEmailCommandHandlerSpecs
{
    //will always be true
    //this test is mearly here to ensure that should the method change at any point
    //the test should be updated.
    [Subject(typeof(SendClientSurveyInternalEmailCommandHandler))]
    internal class When_SendClientSurveyInternalEmailCommand_Handled : DomainServiceSpec<SendClientSurveyInternalEmailCommand, SendClientSurveyInternalEmailCommandHandler>
    {
        protected static ISurveyRepository surveyRepository;
        //Arrange
        Establish context = () =>
        {
            surveyRepository = An<ISurveyRepository>();
            command = new SendClientSurveyInternalEmailCommand(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything);
            handler = new SendClientSurveyInternalEmailCommandHandler(surveyRepository);
        };
        //Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        //Assert
        It should_send_mail = () =>
        {
            surveyRepository.WasToldTo(x => x.SendClientSurveyEmail(Param<int>.IsAnything, Param<int>.IsAnything, Param<string>.IsAnything, Param.Is(false)));
        };
    }
}