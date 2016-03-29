using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.CreateCommissionableConsultantRoleCommandHandlerSpecs
{
    [Subject(typeof(CreateCommissionableConsultantRoleCommandHandler))]
    public class When_the_legal_entity_was_not_found : DomainServiceSpec<CreateCommissionableConsultantRoleCommand, CreateCommissionableConsultantRoleCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        private static IOrganisationStructureRepository organisationStructureRepository;
        private static IApplication application;

        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            organisationStructureRepository = An<IOrganisationStructureRepository>();
            application = An<IApplication>();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            organisationStructureRepository.WhenToldTo(x => x.GetAdUserForAdUserName(Param.IsAny<string>())).Return((IADUser)null);

            command = new CreateCommissionableConsultantRoleCommand(Param.IsAny<int>(), Param.IsAny<string>());
            handler = new CreateCommissionableConsultantRoleCommandHandler(applicationRepository, organisationStructureRepository);
        };

        Because of = () =>
        {
            Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_not_add_application_role_to_application = () =>
        {
            application.WasNotToldTo(x => x.AddRole(Param.IsAny<int>(), Param.IsAny<ILegalEntity>()));
        };

        It should_not_attempt_to_save_application = () =>
        {
            applicationRepository.WasNotToldTo(x => x.SaveApplication(Param.IsAny<IApplication>()));
        };
    }
}