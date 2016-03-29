using System;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.NTUCaseCommandHandlerSpecs
{
    [Subject(typeof(NTUCaseCommandHandler))]
    public class When_an_application_does_not_exist : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static NTUCaseCommand command;
        protected static NTUCaseCommandHandler handler;
        protected static IApplicationRepository applicationRepository;
        protected static IApplication application;
        protected static IApplicationStatus appstat;
        protected static ILookupRepository lookUpRepo;
        static Exception exception;
        protected IEventList<IApplicationStatus> applicationStatuses;
        // Arrange
        Establish context = () =>
        {
            int applicationKey = -1;
            lookUpRepo = An<ILookupRepository>();
            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();
            appstat = An<IApplicationStatus>();
            IEventList<IApplicationStatus> applicationStatuses = new StubEventList<IApplicationStatus>();
            applicationStatuses.ObjectDictionary.Add(Convert.ToString((int)OfferStatuses.NTU), null);

            lookUpRepo.WhenToldTo(x => x.ApplicationStatuses).Return(applicationStatuses);

            appstat.WhenToldTo(x => x.Key).Return((int)OfferStatuses.NTU);
            application.WhenToldTo(x => x.ApplicationStatus).Return(appstat);
            application = null;
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Throw(new Exception());

            command = new NTUCaseCommand(applicationKey);
            handler = new NTUCaseCommandHandler(applicationRepository, lookUpRepo);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_return_exception = () =>
        {
            exception.ShouldBeOfType(typeof(Exception));
            exception.ShouldNotBeNull();
        };

        It should_not_save_application = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}