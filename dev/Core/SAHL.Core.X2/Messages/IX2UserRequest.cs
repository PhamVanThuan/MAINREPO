namespace SAHL.Core.X2.Messages
{
    public interface IX2UserRequest : IX2Request
    {
        string ActivityName { get; }
    }
}