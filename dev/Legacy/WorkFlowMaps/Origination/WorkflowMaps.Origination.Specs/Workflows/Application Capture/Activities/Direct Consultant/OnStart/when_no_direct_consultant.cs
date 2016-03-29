using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Direct_Consultant.OnStart
{
    [Subject("Activity => Direct_Consultant => OnStart")]
    internal class when_no_direct_consultant : WorkflowSpecApplicationCapture
    {
        private static Exception expectedException;
        private static int expectedManagerOSKey;
        private static IWorkflowAssignment assignment;
        private static IApplicationCapture appCap;
        private static string adUsername;

        private Establish context = () =>
        {
            workflowData.LeadType = 2;
            expectedManagerOSKey = 1;
            adUsername = string.Empty;
            workflowData.ApplicationKey = 2;
            ((InstanceDataStub)instanceData).ID = 3;
            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.GetBranchManagerOrgStructureKey(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>()))
                .Return(expectedManagerOSKey);
            assignment.Expect(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "Branch Consultant D", workflowData.ApplicationKey, 95, instanceData.ID, "Contact Client", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.DirectConsultant)).Return(adUsername);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            appCap = An<IApplicationCapture>();
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
            assignment.Expect(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "Branch Consultant D", workflowData.ApplicationKey, 94, instanceData.ID, "Contact Client", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.DirectConsultant)).Return(adUsername);
            assignment.Expect(x => x.AssignBranchManagerForOrgStrucKey((IDomainMessageCollection)messages, instanceData.ID, "Branch Manager D", expectedManagerOSKey, workflowData.ApplicationKey, "Direct Consultant", SAHL.Common.Globals.Process.Origination)).Return(adUsername);
        };

        private Because of = () =>
        {
            expectedException = Catch.Exception(() => workflow.OnStartActivity_Direct_Consultant(instanceData, workflowData, paramsData, messages));
        };

        private It should_throw_exception = () =>
        {
            expectedException.Message.ShouldEqual("Unable to assign Direct users");
        };
    }
}