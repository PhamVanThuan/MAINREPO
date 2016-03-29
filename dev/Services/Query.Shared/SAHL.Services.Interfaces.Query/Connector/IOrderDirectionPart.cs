namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IOrderDirectionPart
    {
        IQuery Asc();
        IQuery Desc();
        string AsString();
    }
}