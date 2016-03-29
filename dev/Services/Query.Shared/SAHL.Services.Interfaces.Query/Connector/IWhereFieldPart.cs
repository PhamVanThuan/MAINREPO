namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IWhereFieldPart
    {
        IWhereValuePart Field(string field);
        string AsString();
    }
}