namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IPageSizePart
    {
        IQuery WithPageSize(int pageSize);
        string AsString();
    }
}