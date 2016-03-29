using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using DomainService2.Workflow.Origination.Valuations;
using SAHL.Common.Collections;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationStatusReturned
{
    [Subject(typeof(SetValuationStatusReturnedCommandHandler))]
    public class When_the_valuation_status_is_not_pending : SetValuationStatusReturnedCommandHandlerBase
    {
        private static ILookupRepository lookupRepository;
        private static IPropertyRepository propertyRepository;

        Establish context = () =>
            {
                messages = new DomainMessageCollection();

                IValuationStatus valuationCompleteStatus = An<IValuationStatus>();
                valuationCompleteStatus.WhenToldTo(x => x.Key).Return(2); // 2 = Complete
                valuationCompleteStatus.WhenToldTo(x => x.Description).Return("Complete");

                IValuation valuation = An<IValuation>();
                valuation.WhenToldTo(x => x.ValuationStatus).Return(valuationCompleteStatus);

                propertyRepository = An<IPropertyRepository>();
                propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>())).Return(valuation);
                
                lookupRepository = An<ILookupRepository>();


                command = new SetValuationStatusReturnedCommand(1);
                handler = new SetValuationStatusReturnedCommandHandler(propertyRepository, lookupRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_not_do_anything = () =>
            {
                propertyRepository.WasNotToldTo(x => x.SaveValuation(Param.IsAny<IValuation>()));
            };
    }
}
