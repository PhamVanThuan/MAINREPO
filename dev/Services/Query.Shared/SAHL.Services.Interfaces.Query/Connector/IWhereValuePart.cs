namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IWhereValuePart
    {
        IQuery Value(string value);
        string AsString();
    }
}