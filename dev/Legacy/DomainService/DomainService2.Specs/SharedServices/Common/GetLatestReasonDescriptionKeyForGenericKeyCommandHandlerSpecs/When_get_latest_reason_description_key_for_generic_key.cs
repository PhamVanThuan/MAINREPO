using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetLatestReasonDescriptionKeyForGenericKeyCommandHandlerSpecs
{
    [Subject(typeof(GetLatestReasonDescriptionKeyForGenericKeyCommandHandler))]
    public class When_get_latest_reason_description_key_for_generic_key : DomainServiceSpec<GetLatestReasonDescriptionKeyForGenericKeyCommand, GetLatestReasonDescriptionKeyForGenericKeyCommandHandler>
    {
        protected static IReasonRepository reasonRepository;

        Establish context = () =>
        {
            reasonRepository = An<IReasonRepository>();

            command = new GetLatestReasonDescriptionKeyForGenericKeyCommand(Param.IsAny<int>(), Param.IsAny<int>());
            handler = new GetLatestReasonDescriptionKeyForGenericKeyCommandHandler(reasonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_repository_method_to_get_latest_reason_description_key_for_generic_key = () =>
        {
            reasonRepository.WasToldTo(x => x.GetLatestReasonDescriptionKeyForGenericKey(Param.IsAny<int>(), Param.IsAny<int>()));
        };
    }
}