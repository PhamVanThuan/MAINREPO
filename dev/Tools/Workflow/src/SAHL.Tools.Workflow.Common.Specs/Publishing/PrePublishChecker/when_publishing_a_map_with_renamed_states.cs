using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using publish = SAHL.Tools.Workflow.Common.Database.Publishing;
using xmlElements = SAHL.Tools.Workflow.Common.WorkflowElements;
using System;

namespace SAHL.Tools.Workflow.Common.Specs.Publishing.PrePublishChecker
{
    public class when_publishing_a_map_with_renamed_states : WithFakes
    {
        static publish.PrePublishChecker prePublishChecker;
        static xmlElements.Process newProcess;
        static List<publish.PublisherError> prePublishErrors;

        Establish context = () =>
            {
                prePublishErrors = new List<publish.PublisherError>();
                prePublishChecker = new publish.PrePublishChecker();

                // create a process that has a snigle state and a common state that is not applied to the single state
                newProcess = new xmlElements.Process("test", "testver", "testmapver", "false","false","3");
                xmlElements.Workflow workflow = new xmlElements.Workflow("testflow", 0, 0, 1);
                xmlElements.UserState state1 = new xmlElements.UserState("State1", 0, 0, new xmlElements.CodeSection("OnEnter"), new xmlElements.CodeSection("OnExit"), Guid.NewGuid());
                workflow.AddState(state1);

                newProcess.AddWorkflow(workflow);
            };

        Because of = () =>
            {
            };

        It should = () =>
            {
            };
    }
}