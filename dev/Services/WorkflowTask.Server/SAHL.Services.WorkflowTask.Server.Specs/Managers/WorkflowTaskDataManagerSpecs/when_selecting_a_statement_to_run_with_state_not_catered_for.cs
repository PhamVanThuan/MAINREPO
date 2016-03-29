using System;
using System.Reflection;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using SAHL.Services.WorkflowTask.Server.Specs.Fakes.Statements.TaskStatements.FakeBusinessProcess;

namespace SAHL.Services.WorkflowTask.Server.Specs.Managers.WorkflowTaskDataManagerSpecs
{
    public class when_selecting_a_statement_to_run_with_workflow_not_catered_for : WithFakes
    {
        Establish that = () =>
        {
            statementSelector = new TaskStatementSelector(Assembly.GetExecutingAssembly());
        };

        private Because of = () =>
        {
            statement = statementSelector.GetStatementTypeForWorkFlow("FakeBusinessProcess", "", "");
        };

        private It should_return_a_non_empty_statement = () =>
        {
            statement.ShouldNotBeNull();
        };

        private It should_be_the_generic_statement = () =>
        {
            statement.ShouldEqual(typeof(FakeBusinessProcessStatement));
        };

        private static TaskStatementSelector statementSelector;
        private static Type statement;
    }
}
