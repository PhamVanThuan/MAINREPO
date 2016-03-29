using SAHL.Core.IoC;
using System;

namespace SAHL.Services.Communications.Managers.LiveReplies
{
    public interface ILiveRepliesManager : IStartable, IStoppable
    {
        string ComcorpLiveRepliesServiceVersion { get; }

        ProcessBankLiveRepliesRequestServiceHeaderBankId? ComcorpLiveRepliesServiceBankId { get; }

        string InternalEmailFromAddress { get; }

        string CreateComcorpMessageAuthenticationCodeFromXml(string xmlString);

        string GenerateXmlStringFromObject<T>(T liveReplyRequest);

        ProcessBankLiveRepliesRequestLiveReplyEventId? GetProcessBankLiveRepliesRequestLiveReplyEventIdFromEventId(int eventId);

        string ProcessBankLiveReplies(string requestXML);

        Tuple<int, string> GetComcorpLiveRepliesReplyStatus(string resultXml);
    }
}