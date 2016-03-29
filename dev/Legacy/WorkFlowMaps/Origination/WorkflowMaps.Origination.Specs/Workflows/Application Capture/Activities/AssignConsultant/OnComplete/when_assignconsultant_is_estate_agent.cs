using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.AssignConsultant.OnComplete
{
    [Subject("Activity => AssignConsultant => OnComplete")]
    internal class when_assignconsultant_is_estate_agent : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment client;

        private Establish context = () =>
        {
            message = String.Empty;
            result = false;
            workflowData.IsEA = true;
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_AssignConsultant(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_resolve_branch_consultant_dynamic_role = () =>
        {
            client.WasNotToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(),
                                                                        Param.IsAny<string>(), Param.IsAny<long>()));
        };

        private It should_not_insert_commissionable_consultant = () =>
        {
            client.WasNotToldTo(x => x.InsertCommissionableConsultant(Param.IsAny<IDomainMessageCollection>(),
                                                Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}