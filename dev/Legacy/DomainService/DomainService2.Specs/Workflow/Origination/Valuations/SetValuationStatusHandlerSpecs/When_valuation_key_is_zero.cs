using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationStatus
{
    [Subject(typeof(SetValuationStatusCommandHandler))]
    public class When_valuation_key_is_zero : SetValuationStatusCommandHandlerBase
    {
        private static IPropertyRepository propertyRepository;

        Establish context = () =>
            {
                propertyRepository = An<IPropertyRepository>();
                ILookupRepository lookupRepository = An<ILookupRepository>();

                messages = new DomainMessageCollection();
                command = new SetValuationStatusCommand(0, 1);
                handler = new SetValuationStatusCommandHandler(propertyRepository, lookupRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_do_nothing = () =>
            {
                propertyRepository.WasNotToldTo(x => x.SaveValuation(Param.IsAny<IValuation>()));
            };
    }
}