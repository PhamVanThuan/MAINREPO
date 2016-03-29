using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.Valuations.RecalcHOC
{
	[Subject(typeof(RecalcHOCCommandHandler))]
	public class When_valuation_is_an_adcheck_physical_valuation : RecalcHOCCommandHandlerBase
	{
		private static IPropertyRepository propertyRepository;

		Establish context = () =>
		{
			IValuation valuation = An<IValuation>();
			valuation.WhenToldTo(v => v.ValuationDataProviderDataService.DataProviderDataService.Key)
					 .Return((int)SAHL.Common.Globals.DataProviderDataServices.AdCheckPhysicalValuation);

			propertyRepository = An<IPropertyRepository>();
			propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>()))
							  .Return(valuation);
			messages = new DomainMessageCollection();
			command = new RecalcHOCCommand(1, 1, true);
			handler = new RecalcHOCCommandHandler(propertyRepository);
		};

		Because of = () =>
		{
			handler.Handle(messages, command);
		};

		It should_not_update_hoc_valuation = () =>
		{
			propertyRepository.WasNotToldTo(x => x.ValuationUpdateHOC(Param.IsAny<int>(), Param.Is<SAHL.Common.Globals.GenericKeyTypes>(SAHL.Common.Globals.GenericKeyTypes.Offer), Param.IsAny<int>()));
		};

		It should_update_adcheck_hoc_valuation = () =>
		{
			propertyRepository.WasToldTo(x => x.AdcheckValuationUpdateHOC(Param.IsAny<int>(), Param.IsAny<int>()));
		};
	}
}