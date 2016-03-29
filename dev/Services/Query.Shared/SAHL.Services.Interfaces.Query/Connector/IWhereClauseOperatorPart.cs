using SAHL.Services.Interfaces.Query.Connector.Enums;

namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IWhereClauseOperatorPart
    {
        ClauseOperator ClauseOperator { get; }
        IWhereOperatorPart OperatorPart { get; }
        IWhereOperatorPart And();
        IWhereOperatorPart Or();
        string AsString();
    }
}