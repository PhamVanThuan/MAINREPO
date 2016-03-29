using System.Dynamic;

namespace SAHL.Services.Query.Connector
{
    public interface IQueryWithIncludes
    {
        IQueryWithIncludes IncludeField(string fieldName);
        IQueryWithIncludes IncludeRelationship(string fieldName);
        dynamic Execute();
        string AsJson();
    }
}