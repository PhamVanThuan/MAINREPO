﻿using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.PromoteLeadToMainApplicantCommandHandlerSpecs
{
    [Subject(typeof(PromoteLeadToMainApplicantCommandHandler))]
    public class When_there_is_a_mortgage_loan_application : WithFakes
    {
        private static PromoteLeadToMainApplicantCommandHandler handler;
        private static PromoteLeadToMainApplicantCommand command;

        private static IApplicationRepository applicationRepository;
        private static IDomainMessageCollection messages;

        private static IApplication application;
        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();

            application = An<IApplication>();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            applicationRepository.WhenToldTo(x => x.PromoteLeadToMain(application));

            messages = new DomainMessageCollection();

            handler = new PromoteLeadToMainApplicantCommandHandler(applicationRepository);
            command = new PromoteLeadToMainApplicantCommand(Param.IsAny<int>());
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}