namespace SAHL.Services.Interfaces.Query.Connector
{
    public interface ILimitPart
    {
        int Count { get; set; }
        string AsString();
    }
}