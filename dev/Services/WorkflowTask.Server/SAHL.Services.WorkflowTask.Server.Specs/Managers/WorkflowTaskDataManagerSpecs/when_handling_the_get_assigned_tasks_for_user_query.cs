using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.WorkflowTask.Queries;
using SAHL.Services.WorkflowTask.Server.QueryHandlers;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.WorkflowTask.Server.Specs.Managers.WorkflowTaskDataManagerSpecs
{
    public class when_handling_the_get_assigned_tasks_for_user_query : WithFakes
    {

        Establish that = () =>
        {
            coordinator = An<ITaskQueryCoordinator>();
            handler = new GetAssignedTasksForUserQueryHandler(coordinator);
            username = @"SAHL\Someone";
            roles = new List<string>();
            query = new GetAssignedTasksForUserQuery(username, roles);
        };


        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_not_have_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_have_attempted_to_retrieve_workflow_tasks_via_the_coordinator = () =>
        {
            coordinator.WasToldTo(a => a.GetWorkflowTasks(Param.Is(username), Param.Is(roles))).OnlyOnce();
        };

        private static ITaskQueryCoordinator coordinator;
        private static GetAssignedTasksForUserQueryHandler handler;
        private static string username;
        private static List<string> roles;
        private static GetAssignedTasksForUserQuery query;
        private static ISystemMessageCollection messages;
    }
}
