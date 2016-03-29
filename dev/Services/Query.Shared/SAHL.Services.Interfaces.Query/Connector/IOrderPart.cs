namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IOrderPart
    {
        IQuery Asc();
        IQuery Desc();
        string AsString();
    }
}