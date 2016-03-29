using SAHL.Common.Logging;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Services
{
    public class CommunicationService : ICommunicationService
    {
        private ICommunicationsServiceClient communicationsServiceClient;

        public CommunicationService(ICommunicationsServiceClient communicationsServiceClient)
        {
            this.communicationsServiceClient = communicationsServiceClient;
        }

        public ISystemMessageCollection SendComcorpLiveReply(Guid id, string bankReference, string bondAccountNo, string comcorpReference, string eventComment, DateTime eventDate, int eventId, int offeredAmount, int registeredAmount, int requestedAmount)
        {
            try
            {
                var command = new SendComcorpLiveReplyCommand(id, bankReference, bondAccountNo, comcorpReference, eventComment, eventDate, eventId, offeredAmount, registeredAmount, requestedAmount);
                var response = communicationsServiceClient.PerformCommand(command, null);
                return response;
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessage("SendComcorpLiveReply", string.Format("problem sending request {0}", ex.ToString()));
                throw;
            }

        }


        public ICommunicationsServiceClient Client
        {
            get { return this.communicationsServiceClient; }
        }
    }
}
