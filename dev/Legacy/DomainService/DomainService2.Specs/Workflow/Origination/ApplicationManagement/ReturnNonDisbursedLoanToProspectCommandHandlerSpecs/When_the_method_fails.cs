using System;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.ReturnNonDisbursedLoanToProspectCommandHandlerSpecs
{
    [Subject(typeof(ReturnNonDisbursedLoanToProspectCommandHandler))]
    public class When_the_method_fails : WithFakes
    {
        protected static ReturnNonDisbursedLoanToProspectCommand command;
        protected static ReturnNonDisbursedLoanToProspectCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static ICommonRepository commonRepository;
        protected static Exception exception;

        // Arrange
        Establish context = () =>
        {
            messages = An<IDomainMessageCollection>();
            applicationRepository = An<IApplicationRepository>();
            commonRepository = An<ICommonRepository>();

            applicationRepository.WhenToldTo(x => x.ReturnNonDisbursedLoanToProspect(Param<int>.IsAnything)).Throw(new Exception());

            commonRepository.RefreshDAOObject<IApplication>(Param<int>.IsAnything);

            command = new ReturnNonDisbursedLoanToProspectCommand(Param<int>.IsAnything);
            handler = new ReturnNonDisbursedLoanToProspectCommandHandler(applicationRepository, commonRepository);
        };

        // Act
        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        // Assert
        It should_return_an_exception = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}