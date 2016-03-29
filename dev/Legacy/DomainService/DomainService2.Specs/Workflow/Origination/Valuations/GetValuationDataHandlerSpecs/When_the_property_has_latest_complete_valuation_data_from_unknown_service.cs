using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.Valuations.GetValuationData
{
    [Subject(typeof(GetValuationDataCommandHandler))]
    public class When_the_property_has_latest_complete_valuation_data_from_unknown_service : GetValuationDataCommandHandlerBase
    {
        Establish context = () =>
        {
            messages = new DomainMessageCollection();

            IProperty property = BuildPropertyWithLatestValuation(999);

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

        It should_return_the_correct_valuation_key = () =>
        {
            ((int)command.ValuationDataResult["ValuationKey"]).ShouldEqual(TEST_VALUATION_KEY);
        };

        It should_return_the_correct_propertykey = () =>
        {
            ((int)command.ValuationDataResult["PropertyKey"]).ShouldEqual(TEST_PROPERTY_KEY);
        };

        It should_return_the_correct_valuation_data_provider_data_service_key = () =>
        {
            ((int)command.ValuationDataResult["ValuationDataProviderDataServiceKey"]).ShouldEqual(VALUATION_DATA_PROVIDER_SERVICE_KEY);
        };

        It should_not_return_valuation_data_provider_property_id = () =>
        {
            (command.ValuationDataResult.ContainsKey("LightstonePropertyID") ||
                command.ValuationDataResult.ContainsKey("AdcheckPropertyID")).ShouldBeFalse();
        };
    }
}