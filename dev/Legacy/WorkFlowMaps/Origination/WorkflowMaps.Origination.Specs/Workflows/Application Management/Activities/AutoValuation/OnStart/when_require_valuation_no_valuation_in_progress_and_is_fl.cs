﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.AutoValuation.OnStart
{
    [Subject("Activity => AutoValuation => OnStart")]
    internal class when_require_valuation_no_valuation_in_progress_and_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static bool expectedResult;
        private static ICommon common;
        private static IValuations valuations;
        private static IFL fl;

        private Establish context = () =>
        {
            result = false;
            expectedResult = true;
            workflowData.RequireValuation = true;
            workflowData.IsFL = true;

            common = An<ICommon>();
            common.WhenToldTo(x => x.IsValuationInProgress(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<int>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            valuations = An<IValuations>();
            domainServiceLoader.RegisterMockForType<IValuations>(valuations);

            fl = An<IFL>();
            fl.WhenToldTo(x => x.ValuationRequired(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_AutoValuation(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_valuation_is_in_progress = () =>
        {
            common.WasToldTo(x => x.IsValuationInProgress((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey));
        };

        private It should_check_if_valuation_required = () =>
        {
            fl.WasToldTo(x => x.ValuationRequired((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_whether_or_not_valuation_required = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}