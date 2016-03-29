using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.CreateCommissionableConsultantRoleCommandHandlerSpecs
{
    [Subject(typeof(CreateCommissionableConsultantRoleCommandHandler))]
    public class When_the_legal_entity_and_application_was_found : DomainServiceSpec<CreateCommissionableConsultantRoleCommand, CreateCommissionableConsultantRoleCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        private static IOrganisationStructureRepository organisationStructureRepository;
        private static IADUser adUser;
        private static IApplication application;
        private static ILegalEntityNaturalPerson legalEntity;

        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            organisationStructureRepository = An<IOrganisationStructureRepository>();
            adUser = An<IADUser>();
            application = An<IApplication>();
            legalEntity = An<ILegalEntityNaturalPerson>();

            adUser.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            organisationStructureRepository.WhenToldTo(x => x.GetAdUserForAdUserName(Param.IsAny<string>())).Return(adUser);

            command = new CreateCommissionableConsultantRoleCommand(Param.IsAny<int>(), Param.IsAny<string>());
            handler = new CreateCommissionableConsultantRoleCommandHandler(applicationRepository, organisationStructureRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_save_application = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(Param.IsAny<IApplication>()));
        };
    }
}