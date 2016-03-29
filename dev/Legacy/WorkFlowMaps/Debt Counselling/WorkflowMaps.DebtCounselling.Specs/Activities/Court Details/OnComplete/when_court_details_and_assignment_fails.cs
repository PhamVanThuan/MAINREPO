﻿using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Details.OnComplete
{
    [Subject("Activity => Court_Details => OnComplete")]
    internal class when_court_details_and_assignment_fails : WorkflowSpecDebtCounselling
    {
        static IWorkflowAssignment wfa;
        static bool result;
        static string message;
        Establish context = () =>
        {
            result = true;
            wfa = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(),
                Param.IsAny<List<string>>(), Param.IsAny<bool>(), Param.IsAny<bool>())).Return(false);
        };

        Because of = () =>
        {
            result = workflow.OnCompleteActivity_Court_Details(messages, workflowData, instanceData, paramsData, ref message);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}