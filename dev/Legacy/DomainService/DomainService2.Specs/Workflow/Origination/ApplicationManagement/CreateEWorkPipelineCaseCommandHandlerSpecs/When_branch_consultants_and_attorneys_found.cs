using System.Collections.Generic;
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
    public class When_branch_consultants_and_attorneys_found : WithFakes
    {
        private static CreateEWorkPipelineCaseCommand command;
        private static CreateEWorkPipelineCaseCommandHandler handler;
        private static IDomainMessageCollection messages;

        Establish context = () =>
            {
                IeWork eWorkEngine = An<IeWork>();
                IOrganisationStructureRepository orgStructureRepository = An<IOrganisationStructureRepository>();
                IApplicationRepository applicationRepository = An<IApplicationRepository>();

                IGeneralStatus generalStatus = An<IGeneralStatus>();
                generalStatus.WhenToldTo(x => x.Key).Return(1);

                IApplicationRole role = An<IApplicationRole>();
                role.WhenToldTo(x => x.LegalEntity.Key).Return(1);
                role.WhenToldTo(x => x.GeneralStatus).Return(generalStatus);

                List<IApplicationRole> tempList = new List<IApplicationRole>() { role };

                IReadOnlyEventList<IApplicationRole> roles = new ReadOnlyEventList<IApplicationRole>(tempList);

                IApplication application = An<IApplication>();
                application.WhenToldTo(x => x.GetApplicationRolesByType(Param.IsAny<OfferRoleTypes>())).Return(roles);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                IAttorney attorney = An<IAttorney>();
                attorney.WhenToldTo(x => x.Key).Return(1);

                orgStructureRepository.WhenToldTo(x => x.GetAttorneyByLegalEntityKey(Param.IsAny<int>())).Return(attorney);

                IADUser consultant = An<IADUser>();
                consultant.WhenToldTo(x => x.Key).Return(1);
                orgStructureRepository.WhenToldTo(x => x.GetAdUserByLegalEntityKey(Param.IsAny<int>())).Return(consultant);

                command = new CreateEWorkPipelineCaseCommand(1);
                handler = new CreateEWorkPipelineCaseCommandHandler(eWorkEngine, applicationRepository, orgStructureRepository);
                messages = new DomainMessageCollection();
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