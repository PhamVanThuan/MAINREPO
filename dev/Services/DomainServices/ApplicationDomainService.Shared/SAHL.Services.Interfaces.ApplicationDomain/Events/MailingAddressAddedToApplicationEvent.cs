using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using System;

namespace SAHL.Services.Interfaces.ApplicationDomain.Events
{
    public class MailingAddressAddedToApplicationEvent : Event
    {
        public MailingAddressAddedToApplicationEvent(DateTime date, int applicationNumber, int clientAddressKey,
            CorrespondenceLanguage correspondenceLanguage, bool onlineStatementRequired, OnlineStatementFormat onlineStatementFormat,
            CorrespondenceMedium correspondenceMedium, int? clientToUseForEmailCorrespondence)
            : base(date)
        {
            this.ApplicationNumber = applicationNumber;
            this.ClientAddressKey = clientAddressKey;
            this.CorrespondenceLanguage = correspondenceLanguage;
            this.OnlineStatementRequired = onlineStatementRequired;
            this.OnlineStatementFormat = onlineStatementFormat;
            this.CorrespondenceMedium = correspondenceMedium;
            this.ClientToUseForEmailCorrespondence = clientToUseForEmailCorrespondence;
        }

        public int ApplicationNumber { get; protected set; }

        public int ClientAddressKey { get; protected set; }

        public CorrespondenceLanguage CorrespondenceLanguage { get; protected set; }

        public bool OnlineStatementRequired { get; protected set; }

        public OnlineStatementFormat OnlineStatementFormat { get; protected set; }

        public CorrespondenceMedium CorrespondenceMedium { get; protected set; }

        public int? ClientToUseForEmailCorrespondence { get; protected set; }
    }
}