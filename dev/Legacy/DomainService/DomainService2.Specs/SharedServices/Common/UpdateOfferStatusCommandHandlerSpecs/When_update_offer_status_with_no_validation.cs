using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.UpdateOfferStatusCommandHandleSpecs
{
    [Subject(typeof(UpdateOfferStatusCommandHandler))]
    public class When_update_offer_status_with_no_validation : DomainServiceSpec<UpdateOfferStatusCommand, UpdateOfferStatusCommandHandler>
    {
        protected static IApplicationRepository appRepository;

        Establish context = () =>
        {
            appRepository = An<IApplicationRepository>();

            command = new UpdateOfferStatusCommand(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>());
            handler = new UpdateOfferStatusCommandHandler(appRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_repository_method_to_update_offer_status = () =>
        {
            appRepository.WasToldTo(x => x.UpdateOfferStatusWithNoValidation(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<int>()));
        };
    }
}