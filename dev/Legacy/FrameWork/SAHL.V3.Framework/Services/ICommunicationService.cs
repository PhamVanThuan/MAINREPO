using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Services
{
    public interface ICommunicationService : IV3Service
    {
        ISystemMessageCollection SendComcorpLiveReply(Guid id, string bankReference, string bondAccountNo, string comcorpReference, string eventComment, DateTime eventDate, int eventId, int offeredAmount, int registeredAmount, int requestedAmount);

        ICommunicationsServiceClient Client { get; }
    }
}
