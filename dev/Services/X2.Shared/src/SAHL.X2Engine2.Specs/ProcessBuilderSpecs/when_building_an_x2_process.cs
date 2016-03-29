using System.Linq;
using Machine.Specifications;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Builders;

namespace SAHL.X2Engine2.Specs.ProcessBuilderSpecs
{
    public class when_building_an_x2_process
    {
        private static StructureMap.AutoMocking.AutoMocker<ProcessBuilder> autoMocker;
        private static X2Process builtProcess;
        private static string expectedProcessName;
        private static string[] expectedWorkflowNames;
        private static X2Workflow[] expectedWorkflows;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<ProcessBuilder>();
            expectedProcessName = "processName";
            expectedWorkflowNames = new[] { "Credit", "Life" };
            expectedWorkflows = new[]
            {
                new X2Workflow("processName","Credit"),
                new X2Workflow("processName","Life")
            };
        };

        private Because of = () =>
        {
            builtProcess = autoMocker.ClassUnderTest.Build("processName", expectedWorkflowNames);
        };

        private It should_create_a_new_process_given_the_process_name_and_the_workflow_name = () =>
        {
            builtProcess.ProcessName.ShouldEqual(expectedProcessName);
            var actualWorkflows = builtProcess.Workflows.ToArray();
            foreach (var execpectedWF in expectedWorkflows)
            {
                var workflow = actualWorkflows.Where(x => x.ProcessName == execpectedWF.ProcessName
                                                    && x.WorkflowName == execpectedWF.WorkflowName).FirstOrDefault();
            }
        };
    }
}