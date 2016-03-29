using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Communications.Managers.LiveReplies;
using SAHL.Services.Interfaces.Communications.Commands;
using System;
using System.Globalization;

namespace SAHL.Services.Communications.CommandHandlers
{
    public class SendComcorpLiveReplyCommandHandler : IServiceCommandHandler<SendComcorpLiveReplyCommand>
    {
        private ILiveRepliesManager communicationsManager;

        public SendComcorpLiveReplyCommandHandler(ILiveRepliesManager communicationsManager)
        {
            this.communicationsManager = communicationsManager;
        }

        public ISystemMessageCollection HandleCommand(SendComcorpLiveReplyCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            ProcessBankLiveRepliesRequestLiveReplyEventId? processBankLiveRepliesRequestLiveReplyEventId = 
                communicationsManager.GetProcessBankLiveRepliesRequestLiveReplyEventIdFromEventId(command.EventId);

            if (processBankLiveRepliesRequestLiveReplyEventId == null)
            {
                messages.AddMessage(new SystemMessage(string.Format("Invalid event id provided: {0}.", command.EventId), SystemMessageSeverityEnum.Error));
                return messages;
            }

            ProcessBankLiveRepliesRequestLiveReply liveReply = new ProcessBankLiveRepliesRequestLiveReply();
            liveReply.BankReference = command.BankReference;
            liveReply.BondAccountNo = command.BondAccountNo;
            liveReply.ComcorpReference = command.ComcorpReference;
            liveReply.EventComment = command.EventComment;
            liveReply.EventDate = command.EventDate;
            liveReply.EventId = processBankLiveRepliesRequestLiveReplyEventId.Value;
            liveReply.OfferedAmount = command.OfferedAmount.ToString();
            liveReply.RegisteredAmount = command.RegisteredAmount.ToString();
            liveReply.RequestedAmount = command.RequestedAmount.ToString();

            ProcessBankLiveRepliesRequestLiveReply[] liveReplies = new ProcessBankLiveRepliesRequestLiveReply[1];
            liveReplies[0] = liveReply;

            string requestXML = communicationsManager.GenerateXmlStringFromObject(liveReplies);
            string messageAuthenticationCode = communicationsManager.CreateComcorpMessageAuthenticationCodeFromXml(requestXML);

            ProcessBankLiveRepliesRequestServiceHeader serviceHeader = new ProcessBankLiveRepliesRequestServiceHeader();
            serviceHeader.ApplicationMac = messageAuthenticationCode;
            serviceHeader.BankId = communicationsManager.ComcorpLiveRepliesServiceBankId.Value;
            serviceHeader.RequestDateTime = DateTime.Now.ToString(@"yyyy/MM/dd hh:mm:ss tt", CultureInfo.InvariantCulture);
            serviceHeader.ServiceVersion = communicationsManager.ComcorpLiveRepliesServiceVersion;

            ProcessBankLiveRepliesRequest liveReplyRequest = new ProcessBankLiveRepliesRequest();
            liveReplyRequest.LiveReplies = liveReplies;
            liveReplyRequest.ServiceHeader = serviceHeader;

            requestXML = communicationsManager.GenerateXmlStringFromObject(liveReplyRequest);

            string resultXML = communicationsManager.ProcessBankLiveReplies(requestXML);

            Tuple<int, string> replyStatus = communicationsManager.GetComcorpLiveRepliesReplyStatus(resultXML);

            if (replyStatus == null ||
                (replyStatus != null && replyStatus.Item1 == 0))
            {
                messages.AddMessage(new SystemMessage("Invalid reply status.", SystemMessageSeverityEnum.Error));
                return messages;
            }

            if (replyStatus != null && replyStatus.Item1 > 1)
            {
                messages.AddMessage(new SystemMessage(string.Format("Comcorp reply: {0}.", replyStatus.Item2), SystemMessageSeverityEnum.Error));
                return messages;
            }

            return messages;
        }
    }
}