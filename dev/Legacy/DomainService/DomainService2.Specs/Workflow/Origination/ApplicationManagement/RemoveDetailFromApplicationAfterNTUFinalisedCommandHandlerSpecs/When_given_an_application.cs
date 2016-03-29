using System.Collections.Generic;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.RemoveDetailFromApplicationAfterNTUFinalisedCommandHandlerSpecs
{
    [Subject(typeof(RemoveDetailFromApplicationAfterNTUFinalisedCommandHandler))]
    public class When_given_an_application : DomainServiceSpec<RemoveDetailFromApplicationAfterNTUFinalisedCommand, RemoveDetailFromApplicationAfterNTUFinalisedCommandHandler>
    {
        private static IApplicationRepository applicationRepository;
        private static IApplication application;

        Establish context = () =>
            {
                application = An<IApplication>();

                applicationRepository = An<IApplicationRepository>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                IAccountRepository accountRepository = An<IAccountRepository>();
                accountRepository.WhenToldTo(x => x.GetDetailTypeKeyByDescription(Param.IsAny<string>())).Return(1);

                command = new RemoveDetailFromApplicationAfterNTUFinalisedCommand(1);
                handler = new RemoveDetailFromApplicationAfterNTUFinalisedCommandHandler(applicationRepository, accountRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_remove_the_new_legal_agreement_signed_detail_from_the_application = () =>
            {
                applicationRepository.WasToldTo(x => x.RemoveDetailFromApplication(Param.IsAny<IApplication>(), Param.IsAny<List<int>>()));
            };
    }
}