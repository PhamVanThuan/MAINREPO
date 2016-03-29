using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.SuretySignedConfirmedCommandHandlerSpecs
{
    [Subject(typeof(SuretySignedConfirmedCommandHandler))]
    public class When_surety_signed_confirmed : WithFakes
    {
        static SuretySignedConfirmedCommand command;
        static SuretySignedConfirmedCommandHandler handler;
        static IDomainMessageCollection messages;
        static IAccountRepository accountRepository;
        static IApplication application;

        Establish context = () =>
            {
                IApplicationRepository applicationRepository = An<IApplicationRepository>();
                accountRepository = An<IAccountRepository>();
                application = An<IApplication>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                messages = new DomainMessageCollection();
                command = new SuretySignedConfirmedCommand(1111);
                handler = new SuretySignedConfirmedCommandHandler(applicationRepository, accountRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };
        It should_create_offer_roles_not_in_account = () =>
            {
                accountRepository.WasToldTo(x => x.CreateOfferRolesNotInAccount(Param.IsAny<IApplication>()));
            };
    }
}