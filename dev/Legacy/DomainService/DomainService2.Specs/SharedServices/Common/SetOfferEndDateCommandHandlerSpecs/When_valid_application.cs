using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.SetOfferEndDateCommandHandlerSpecs
{
    [Subject(typeof(SetOfferEndDateCommandHandler))]
    public class When_valid_application : DomainServiceSpec<SetOfferEndDateCommand, SetOfferEndDateCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;

        Establish context = () =>
            {
                applicationRepository = An<IApplicationRepository>();

                IApplication application = An<IApplication>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                command = new SetOfferEndDateCommand(Param.IsAny<int>());
                handler = new SetOfferEndDateCommandHandler(applicationRepository);
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