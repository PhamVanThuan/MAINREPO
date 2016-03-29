using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Specs.SharedServices.Common.CreateAccountForApplicationCommandHandlerSpecs
{
    [Subject(typeof(CreateAccountForApplicationCommandHandler))]
    public class When_readvance_furtheradvance : DomainServiceSpec<CreateAccountForApplicationCommand, CreateAccountForApplicationCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected static ICommonRepository commonRepository;


        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            applicationRepository = An<IApplicationRepository>();
            IApplication application = An<IApplication>();
            IApplicationType applicationType = An<IApplicationType>();

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            application.WhenToldTo(x => x.ApplicationType).Return(applicationType);
            applicationType.WhenToldTo(x => x.Key).Return((int)OfferTypes.FurtherAdvance);

            command = new CreateAccountForApplicationCommand(Param.IsAny<int>(), Param.IsAny<string>());
            handler = new CreateAccountForApplicationCommandHandler(applicationRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_create_account = () =>
        {
            applicationRepository.WasNotToldTo(x => x.CreateAccountFromApplication(Param.IsAny<int>(), Param.IsAny<string>()));
        };
    }
}