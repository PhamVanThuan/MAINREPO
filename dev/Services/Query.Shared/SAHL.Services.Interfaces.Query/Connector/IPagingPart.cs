namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface IPagingPart
    {
        IPageSizePart SetCurrentPageTo(int page);
        string AsString();
    }
}