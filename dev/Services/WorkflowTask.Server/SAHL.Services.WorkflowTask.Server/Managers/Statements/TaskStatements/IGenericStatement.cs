using SAHL.Core.Data;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements
{
    /// <summary>
    /// Used to identify an ISqlStatement as the default statement to execute when no other matching one can be found
    /// </summary>
    public interface IGenericStatement : ISqlStatement<object>
    {
    }
}