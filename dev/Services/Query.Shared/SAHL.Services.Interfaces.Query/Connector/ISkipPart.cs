namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface ISkipPart
    {
        int SkipCount { get; set; }
        string AsString();
    }
}