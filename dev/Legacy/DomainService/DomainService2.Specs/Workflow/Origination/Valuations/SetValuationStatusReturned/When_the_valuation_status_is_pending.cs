using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using DomainService2.Workflow.Origination.Valuations;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using DomainService2.Specs.DomainService.Stubs;

namespace DomainService2.Specs.Workflow.Origination.Valuations.SetValuationStatusReturned
{
    [Subject(typeof(SetValuationStatusReturnedCommandHandler))]
    public class When_the_valuation_status_is_pending : SetValuationStatusReturnedCommandHandlerBase
    {
        private static ILookupRepository lookupRepository;
        private static IPropertyRepository propertyRepository;
        private static IValuation valuation;

        Establish context = () =>
            {
                messages = new DomainMessageCollection();

                IValuationStatus valuationPendingStatus = An<IValuationStatus>();
                valuationPendingStatus.WhenToldTo(x => x.Key).Return(1); // 1 = Pending

                valuation = new Valuation();
                valuation.ValuationStatus = valuationPendingStatus;

                IValuationStatus valuationReturnedStatus = An<IValuationStatus>();
                valuationReturnedStatus.WhenToldTo(x => x.Key).Return(4); // 4 = Returned
                valuationReturnedStatus.WhenToldTo(x => x.Description).Return("Returned");

                propertyRepository = An<IPropertyRepository>();
                propertyRepository.WhenToldTo(x => x.GetValuationByKey(Param.IsAny<int>())).Return(valuation);
                
                Dictionary<string, IValuationStatus> objDict = new Dictionary<string, IValuationStatus>();
                objDict["4"] = valuationReturnedStatus;

                IEventList<IValuationStatus> valuationStatuses = An<IEventList<IValuationStatus>>();
                valuationStatuses.WhenToldTo(x => x.ObjectDictionary).Return(objDict);

                lookupRepository = An<ILookupRepository>();
                lookupRepository.WhenToldTo(x => x.ValuationStatus).Return(valuationStatuses);

                command = new SetValuationStatusReturnedCommand(1);
                handler = new SetValuationStatusReturnedCommandHandler(propertyRepository, lookupRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_change_the_valuation_status_to_Returned = () =>
            {
                valuation.ValuationStatus.Key.ShouldEqual(4);
            };

        It should_save_the_valuation = () =>
            {
                propertyRepository.WasToldTo(x => x.SaveValuation(Param.IsAny<IValuation>()));
            };
    }
}
