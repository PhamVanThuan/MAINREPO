﻿using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.RollbackDisbursmentCommandHandlerSpecs
{
    [Subject(typeof(ReturnDisbursedLoanToRegistrationCommandHandler))]
    public class When_application_has_been_disbursed : WithFakes
    {
        protected static ReturnDisbursedLoanToRegistrationCommand command;
        protected static ReturnDisbursedLoanToRegistrationCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static IDisbursementRepository disbursementRepository;
        protected static ICommonRepository commonRepository;


        // Arrange
        Establish context = () =>
            {
                commonRepository = An<ICommonRepository>();
                messages = An<IDomainMessageCollection>();
                applicationRepository = An<IApplicationRepository>();
                disbursementRepository = An<IDisbursementRepository>();

                IApplication application = An<IApplication>();
                IAccountSequence accountSequence = An<IAccountSequence>();

                accountSequence.WhenToldTo(x => x.Key).Return(1);
                application.WhenToldTo(x => x.ReservedAccount).Return(accountSequence);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
                disbursementRepository.WhenToldTo(x => x.ReturnDisbursedLoanToRegistration(Param<int>.IsAnything));

                command = new ReturnDisbursedLoanToRegistrationCommand(Param<int>.IsAnything);
                handler = new ReturnDisbursedLoanToRegistrationCommandHandler(applicationRepository, disbursementRepository, commonRepository);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_call_the_method_ReturnDisbursedLoanToRegistration = () =>
            {
                disbursementRepository.WasToldTo(x => x.ReturnDisbursedLoanToRegistration(Param<int>.IsAnything));
            };
    }
}