using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.Origination.Valuations.GetValuationData
{
    [Subject(typeof(GetValuationDataCommandHandler))]
    public class When_the_property_has_no_latest_complete_valuation_data : GetValuationDataCommandHandlerBase
    {
        Establish context = () =>
            {
                IValuation valuation = null;

                IProperty property = An<IProperty>();
                property.WhenToldTo(x => x.LatestCompleteValuation).Return(valuation);

                IApplicationMortgageLoan applicationMortgageLoan = An<IApplicationMortgageLoan>();
                applicationMortgageLoan.WhenToldTo(x => x.Property).Return(property);

                applicationRepository = An<IApplicationReadOnlyRepository>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(applicationMortgageLoan);

                command = new GetValuationDataCommand(0);
                handler = new GetValuationDataCommandHandler(applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_an_empty_valuation_dataset = () =>
            {
                command.ValuationDataResult.ShouldBeEmpty();
            };
    }
}