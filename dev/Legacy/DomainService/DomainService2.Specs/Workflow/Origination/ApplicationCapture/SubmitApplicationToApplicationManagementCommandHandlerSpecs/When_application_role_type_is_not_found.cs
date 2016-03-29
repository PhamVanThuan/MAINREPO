using System;
using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.SubmitApplicationToApplicationManagementCommandHandlerSpecs
{
    [Subject(typeof(SubmitApplicationToApplicationManagementCommandHandler))]
    public class When_application_role_type_is_not_found : DomainServiceSpec<SubmitApplicationToApplicationManagementCommand, SubmitApplicationToApplicationManagementCommandHandler>
    {
        private static IOrganisationStructureRepository organisationStructureRepository;
        private static IX2Repository x2Repository;

        private static IADUser adUser;
        private static IApplicationRole applicationRole;

        private static Exception exception;

        Establish context = () =>
        {
            adUser = An<IADUser>();
            applicationRole = An<IApplicationRole>();

            organisationStructureRepository = An<IOrganisationStructureRepository>();
            x2Repository = An<IX2Repository>();

            organisationStructureRepository.WhenToldTo(x => x.GetLatestApplicationRoleByApplicationKeyAndRoleTypeKey(Param.IsAny<int>(), Param.IsAny<int>())).Return((IApplicationRole)null);
            organisationStructureRepository.WhenToldTo(x => x.GetAdUserByLegalEntityKey(Param.IsAny<int>())).Return(adUser);
            x2Repository.WhenToldTo(x => x.CreateAndSaveActiveExternalActivity(Param.IsAny<string>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));

            command = new SubmitApplicationToApplicationManagementCommand(Param.IsAny<long>(), Param.IsAny<int>());
            handler = new SubmitApplicationToApplicationManagementCommandHandler(organisationStructureRepository, x2Repository);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_not_create_external_activity = () =>
        {
            x2Repository.WasNotToldTo(x => x.CreateAndSaveActiveExternalActivity(Param.IsAny<string>(), Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };
    }
}