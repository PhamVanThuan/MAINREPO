using System.Dynamic;

namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IQuery
    {
        IQuery Limit(int count);
        IOrderPart OrderBy(string column);
        IQuery Skip(int count);
        IQuery IncludeField(string fieldName);
        IQuery IncludeFields(params string[] fieldNames);
        IQuery IncludeRelationship(string fieldName);
        IQuery IncludeRelationships(params string[] fieldNames);
        IWhereClauseOperatorPart Where();
        IPagingPart IncludePaging();
        dynamic Execute();
        string AsJson();
    }
}