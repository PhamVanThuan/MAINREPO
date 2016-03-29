namespace SAHL.X2.Framework.Common
{
    public interface IX2Params
    {
        string StateName { get; }

        string ActivityName { get; }

        string ADUserName { get; }

        bool IgnoreWarning { get; }

        object Data { get; }
    }
}