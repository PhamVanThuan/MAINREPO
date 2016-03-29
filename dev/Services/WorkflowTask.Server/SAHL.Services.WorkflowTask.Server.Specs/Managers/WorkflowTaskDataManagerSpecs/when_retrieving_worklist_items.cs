using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.WorkflowTask.Server.Managers;
using SAHL.Services.WorkflowTask.Server.Managers.Statements;

namespace SAHL.Services.WorkflowTask.Server.Specs.Managers.WorkflowTaskDataManagerSpecs
{
    public class when_retrieving_worklist_items : WithFakes
    {
        private static string username;
        private static FakeDbFactory dbFactory;
        private static WorkflowTaskDataManager workflowTaskDataManager;

        private Establish context = () =>
        {
            username = @"SAHL\HaloUser";
            dbFactory = An<FakeDbFactory>();
            workflowTaskDataManager = new WorkflowTaskDataManager(dbFactory);
        };

        private Because of = () =>
        {
            workflowTaskDataManager.RetrieveWorkFlowStatesForWorkListItemsForUsername(username, new List<string>());
        };

        private It should_have_executed_the_query_statement = () =>
        {
            dbFactory.FakedDb.InReadOnlyWorkflowContext()
                .WasToldTo(a => a.Select(Arg.Any<GetWorkFlowStatesForWorkListItemsForUsernameStatement>()));
        };
    }
}
