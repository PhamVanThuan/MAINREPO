using SAHL.Services.Interfaces.Query.Connector.Enums;

namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IWhereOperatorPart
    {
        QueryOperator QueryOperator { get; }
        IWhereFieldPart WhereFieldPart { get; }

        IWhereFieldPart Equals();
        IWhereFieldPart IsGreaterThan();
        IWhereFieldPart IsLessThan();
        IWhereFieldPart IsGreaterThanEqualTo();
        IWhereFieldPart IsLessThanEqualTo();
        IWhereFieldPart Like();
        IWhereFieldPart StartsWith();
        IWhereFieldPart EndsWith();
        IWhereFieldPart Contains();
        string AsString();
    }
}