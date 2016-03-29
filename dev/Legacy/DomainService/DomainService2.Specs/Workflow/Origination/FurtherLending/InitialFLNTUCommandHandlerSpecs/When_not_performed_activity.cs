using System.Collections.Generic;
using DomainService2.SharedServices;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.InitialFLNTUCommandHandlerSpecs
{
    [Subject(typeof(InitialFLNTUCommandHandler))]
    public class When_not_performed_activity : WithFakes
    {
        static InitialFLNTUCommand command;
        static InitialFLNTUCommandHandler handler;
        static IDomainMessageCollection messages;
        static IApplicationRepository applicationRepository;
        static IApplication application;

        Establish context = () =>
            {
                IX2WorkflowService x2WorkflowService = An<IX2WorkflowService>();
                applicationRepository = An<IApplicationRepository>();
                IOrganisationStructureRepository organisationStructureRepository = An<IOrganisationStructureRepository>();
                application = An<IApplication>();
                IADUser adUser = An<IADUser>();
                ILegalEntityNaturalPerson legalEntityNaturalPerson = An<ILegalEntityNaturalPerson>();
                ISAHLPrincipalCacheProvider principalCacheProvider = An<ISAHLPrincipalCacheProvider>();
                ISAHLPrincipalCache cache = An<ISAHLPrincipalCache>();
                List<RuleExclusionSets> exclusionSets = new List<RuleExclusionSets>();

                x2WorkflowService.WhenToldTo(x => x.HasInstancePerformedActivity(Param.IsAny<long>(), Param.IsAny<string>())).Return(false);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                organisationStructureRepository.WhenToldTo(x => x.GetAdUserForAdUserName(Param.IsAny<string>())).Return(adUser);
                adUser.WhenToldTo(x => x.LegalEntity).Return(legalEntityNaturalPerson);

                principalCacheProvider.WhenToldTo(x => x.GetPrincipalCache()).
                    Return(cache);

                cache.WhenToldTo(x => x.ExclusionSets)
                    .Return(exclusionSets);

                messages = new DomainMessageCollection();
                command = new InitialFLNTUCommand(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<long>());
                handler = new InitialFLNTUCommandHandler(x2WorkflowService, applicationRepository, organisationStructureRepository, principalCacheProvider);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_save_application = () =>
            {
                applicationRepository.WasToldTo(x => x.SaveApplication(application));
            };
    }
}