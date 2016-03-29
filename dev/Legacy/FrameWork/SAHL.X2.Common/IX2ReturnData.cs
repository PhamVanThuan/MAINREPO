namespace SAHL.X2.Common
{
    public interface IX2ReturnData
    {
        IX2MessageCollection MC { get; }

        object Data { get; }
    }
}