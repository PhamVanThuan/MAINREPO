using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Rapid_Readvance.OnStart
{
    [Subject("Activity => Rapid_Readvance => OnStart")]
    internal class when_offer_type_is_readvance_and_check_rapid_should_go_to_credit_rules_fail : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IWorkflowAssignment assignment;
        private static IFL fl;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = true;
            workflowData.OfferTypeKey = (int)SAHL.Common.Globals.OfferTypes.ReAdvance;
            dys = new List<string>() { "New Business Processor D",
            "FL Processor D",
            "FL Supervisor D",
            "FL Manager D" };
            workflowData.ApplicationKey = 1;
            ((ParamsDataStub)paramsData).IgnoreWarning = false;
            ((InstanceDataStub)instanceData).ID = 2;

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            fl = An<IFL>();
            fl.WhenToldTo(x => x.CheckRapidShouldGotoCreditRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rapid_Readvance(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_rapid_should_go_to_credit = () =>
        {
            fl.WasToldTo(x => x.CheckRapidShouldGotoCreditRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_deactive_user_for_instance_and_process = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages,
                instanceData.ID,
                workflowData.ApplicationKey,
                dys,
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}