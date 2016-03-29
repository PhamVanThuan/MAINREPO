using DomainService2.Specs.DomainService.Stubs;
using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationActiveAndSave
{
    [Subject(typeof(SetValuationActiveAndSaveCommandHandler))]
    public class When_given_a_valuation : SetValuationActiveAndSaveCommandHandlerBase
    {
        private static IValuation valuation;
        private static IPropertyRepository propertyRepository;

        Establish context = () =>
            {
                valuation = new Valuation();

                propertyRepository = An<IPropertyRepository>();
                propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>())).Return(valuation);

                messages = new DomainMessageCollection();
                command = new SetValuationActiveAndSaveCommand(1);
                handler = new SetValuationActiveAndSaveCommandHandler(propertyRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_make_the_valuation_active = () =>
            {
                valuation.IsActive.ShouldBeTrue();
            };

        It should_save_the_valuation = () =>
            {
                propertyRepository.WasToldTo(x => x.SaveValuation(Param.IsAny<IValuation>()));
            };
    }
}