﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.PersonalLoan;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Rollback_Disbursement.OnStart
{
    [Subject("Activity => Rollback_Disbursement => OnStart")]
    internal class when_rollback_disbursement_before_cut_off_time : WorkflowSpecPersonalLoans
    {
        private static bool result;
        private static IPersonalLoan client;

        private Establish context = () =>
        {
            result = false;
            client = An<IPersonalLoan>();
            domainServiceLoader.RegisterMockForType<IPersonalLoan>(client);
            client.WhenToldTo(x => x.CheckDisbursementCutOffTimeRule(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rollback_Disbursement(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}