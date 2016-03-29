using System.Dynamic;

namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IQueryWithWhere
    {
        IWhereClauseOperatorPart Where();
        ExpandoObject Execute();
        string AsJson();
    }
}