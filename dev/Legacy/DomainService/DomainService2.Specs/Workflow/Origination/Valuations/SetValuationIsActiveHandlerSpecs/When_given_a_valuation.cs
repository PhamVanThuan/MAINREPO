using DomainService2.Specs.DomainService.Stubs;
using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationIsActive
{
    [Subject(typeof(SetValuationIsActiveCommandHandler))]
    public class When_given_a_valuation : SetValuationIsActiveCommandHandlerBase
    {
        private static IValuation valuation;
        private static IValuation otherValuation1;
        private static IValuation otherValuation2;
        private static IValuation otherValuation3;

        private static IPropertyRepository propertyRepository;

        Establish context = () =>
        {
            otherValuation1 = new Valuation();
            otherValuation2 = new Valuation();
            otherValuation3 = new Valuation();

            IEventList<IValuation> valuations = new EventList<IValuation>();
            valuations.Add(null, otherValuation1);
            valuations.Add(null, otherValuation2);
            valuations.Add(null, otherValuation3);

            IProperty property = An<IProperty>();
            property.WhenToldTo(x => x.Valuations).Return(valuations);

            valuation = new Valuation();
            valuation.Property = property;
            //valuation.WhenToldTo(x => x.Property).Return(property);

            propertyRepository = An<IPropertyRepository>();
            propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>())).Return(valuation);

            messages = new DomainMessageCollection();
            command = new SetValuationIsActiveCommand(1);
            handler = new SetValuationIsActiveCommandHandler(propertyRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_make_the_valuation_the_only_active_valuation_for_the_property = () =>
        {
            otherValuation1.IsActive.ShouldBeFalse(); // WasToldTo(v => { v.IsActive = false; })
            otherValuation2.IsActive.ShouldBeFalse();
            otherValuation3.IsActive.ShouldBeFalse();

            valuation.IsActive.ShouldBeTrue(); //WasToldTo(v => v.IsActive = true);
        };
    }
}