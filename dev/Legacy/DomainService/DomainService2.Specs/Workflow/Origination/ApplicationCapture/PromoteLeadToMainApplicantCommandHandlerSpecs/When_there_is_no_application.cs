using System;
using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.PromoteLeadToMainApplicantCommandHandlerSpecs
{
    [Subject(typeof(PromoteLeadToMainApplicantCommandHandler))]
    public class When_there_is_no_application : WithFakes
    {
        private static PromoteLeadToMainApplicantCommandHandler handler;
        private static PromoteLeadToMainApplicantCommand command;

        private static IApplicationRepository applicationRepository;
        private static IDomainMessageCollection messages;

        private static IApplication application;
        private static Exception exception;
        Establish context = () =>
        {
            application = null;
            applicationRepository = An<IApplicationRepository>();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            applicationRepository.WhenToldTo(x => x.PromoteLeadToMain(application)).Throw(new Exception());

            messages = An<IDomainMessageCollection>();

            handler = new PromoteLeadToMainApplicantCommandHandler(applicationRepository);
            command = new PromoteLeadToMainApplicantCommand(Param.IsAny<int>());
        };

        Because of_exception = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}