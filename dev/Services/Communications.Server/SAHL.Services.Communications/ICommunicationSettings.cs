namespace SAHL.Services.Communications
{
    public interface ICommunicationSettings
    {
        string ComcorpLiveRepliesServiceVersion { get; }

        string ComcorpLiveRepliesWebserviceURL { get; }

        string ComcorpLiveRepliesPublicKeyModulus { get; }

        string ComcorpLiveRepliesPublicKeyExponent { get; }

        string ComcorpLiveRepliesBankId { get; }

        string InternalEmailFromAddress { get; }
    }
}