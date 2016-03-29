using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.Communications.Commands
{
    public class SendComcorpLiveReplyCommand : ServiceCommand, ICommunicationsServiceCommand
    {
        public Guid Id { get; protected set; }

        public string BankReference { get; protected set; } // ApplicationKey

        public string BondAccountNo { get; protected set; } // AccountKey

        public string ComcorpReference { get; protected set; }

        public string EventComment { get; protected set; }

        public DateTime EventDate { get; protected set; }

        public int EventId { get; protected set; }

        public double OfferedAmount { get; protected set; }

        public double RegisteredAmount { get; protected set; }

        public double RequestedAmount { get; protected set; }

        public SendComcorpLiveReplyCommand(Guid id, string bankReference, string bondAccountNo, string comcorpReference, string eventComment, 
            DateTime eventDate, int eventId, double offeredAmount, double registeredAmount, double requestedAmount)
        {
            this.Id = id;
            this.BankReference = bankReference;
            this.BondAccountNo = bondAccountNo;
            this.ComcorpReference = comcorpReference;
            this.EventComment = eventComment;
            this.EventDate = eventDate;
            this.EventId = eventId;
            this.OfferedAmount = offeredAmount;
            this.RegisteredAmount = registeredAmount;
            this.RequestedAmount = requestedAmount;
        }
    }
}