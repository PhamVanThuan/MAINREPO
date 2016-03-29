using System;
using DomainService2.Workflow.Origination.ApplicationManagement;
using EWorkConnector;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.CreateEWorkPipelineCaseCommandHandlerSpecs
{
    [Subject(typeof(CreateEWorkPipelineCaseCommandHandler))]
    public class When_unable_to_find_BranchConsultant_or_FLProcessor_offer_role_for_case : WithFakes
    {
        private static CreateEWorkPipelineCaseCommand command;
        private static CreateEWorkPipelineCaseCommandHandler handler;
        private static IDomainMessageCollection messages;

        private static Exception exception;

        Establish context = () =>
            {
                IeWork eWorkEngine = An<IeWork>();
                IOrganisationStructureRepository orgStructureRepository = An<IOrganisationStructureRepository>();
                IApplicationRepository applicationRepository = An<IApplicationRepository>();

                IReadOnlyEventList<IApplicationRole> roles = null;

                IApplication application = An<IApplication>();
                application.WhenToldTo(x => x.GetApplicationRolesByType(Param.IsAny<OfferRoleTypes>())).Return(roles);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                command = new CreateEWorkPipelineCaseCommand(1);
                handler = new CreateEWorkPipelineCaseCommandHandler(eWorkEngine, applicationRepository, orgStructureRepository);
                messages = new DomainMessageCollection();
            };

        Because of = () =>
            {
                exception = Catch.Exception(() => handler.Handle(messages, command));
            };

        It should_thow_an_exception = () =>
            {
                exception.ShouldNotBeNull();
                exception.ShouldBeOfType(typeof(Exception));
            };
    }
}