﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Query_on_Application.OnComplete
{
    [Subject("Activity => Query_on_Application => OnComplete")]
    internal class when_query_on_application_and_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment assignment;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = true;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            common.WhenToldTo(x => x.CheckApplicationMinimumIncomeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Query_on_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_application_minimum_income_rules = () =>
        {
            common.WasToldTo(x => x.CheckApplicationMinimumIncomeRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()));
        };

        private It should_not_deactive_users_for_instance_and_process = () =>
        {
            assignment.WasNotToldTo(x => x.DeActiveUsersForInstanceAndProcess(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(),
                Param.IsAny<int>(),
                Param.IsAny<List<string>>(),
                Param.IsAny<SAHL.Common.Globals.Process>()));
        };

        private It should_not_reactive_branch_users_for_origination = () =>
        {
            assignment.WasNotToldTo(x => x.ReActivateBranchUsersForOrigination(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(),
                Param.IsAny<long>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<SAHL.Common.Globals.Process>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}