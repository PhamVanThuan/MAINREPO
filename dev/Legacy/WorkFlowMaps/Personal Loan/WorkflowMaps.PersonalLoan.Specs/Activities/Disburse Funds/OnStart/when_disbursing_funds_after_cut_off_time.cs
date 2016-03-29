﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.PersonalLoan;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Disburse_Funds.OnStart
{
    [Subject("Activity => Disburse_Funds => OnStart")]
    internal class when_disbursing_funds_after_cut_off_time : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan client;

        private Establish context = () =>
        {
            result = true;
            client = An<IPersonalLoan>();
            domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
            client.WhenToldTo(x => x.CheckDisbursementCutOffTimeRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Disburse_Funds(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}