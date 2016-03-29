using System.Collections.Generic;
using DomainService2.Workflow.Origination.Valuations;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationStatus
{
    [Subject(typeof(SetValuationStatusCommandHandler))]
    public class When_valuation_key_exists : SetValuationStatusCommandHandlerBase
    {
        //if (command.ValuationKey == 0)
        //{
        //    return;
        //}

        //IValuation valuation = propertyRepository.GetValuationByKey(command.ValuationKey);

        //valuation.ValuationStatus = lookupRepository.ValuationStatus.ObjectDictionary[(command.ValuationStatusKey).ToString()];
        //propertyRepository.SaveValuation(valuation);

        private static IPropertyRepository propertyRepository;

        Establish context = () =>
            {
                IValuation valuation = An<IValuation>();

                propertyRepository = An<IPropertyRepository>();
                propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>())).Return(valuation);

                IValuationStatus valuationCompleteStatus = An<IValuationStatus>();
                valuationCompleteStatus.WhenToldTo(x => x.Key).Return(2); // 2 = Complete
                valuationCompleteStatus.WhenToldTo(x => x.Description).Return("Complete");

                Dictionary<string, IValuationStatus> objDict = new Dictionary<string, IValuationStatus>();
                objDict["2"] = valuationCompleteStatus;

                IEventList<IValuationStatus> valuationStatuses = An<IEventList<IValuationStatus>>();
                valuationStatuses.WhenToldTo(x => x.ObjectDictionary).Return(objDict);

                ILookupRepository lookupRepository = An<ILookupRepository>();
                lookupRepository.WhenToldTo(x => x.ValuationStatus).Return(valuationStatuses);

                messages = new DomainMessageCollection();
                command = new SetValuationStatusCommand(1, 2);
                handler = new SetValuationStatusCommandHandler(propertyRepository, lookupRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_save_the_valuation = () =>
            {
                propertyRepository.WasToldTo(x => x.SaveValuation(Param.IsAny<IValuation>()));
            };
    }
}