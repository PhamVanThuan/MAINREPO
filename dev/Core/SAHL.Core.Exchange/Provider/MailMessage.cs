using System;
using System.Collections.Generic;

namespace SAHL.Core.Exchange.Provider
{
    public class MailMessage : IMailMessage
    {
        public MailMessage()
        {
            Attachments = new List<IMailAttachment>();
        }

        public string UniqueExchangeId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateRecieved { get; set; }
        public DateTime DateSent { get; set; }
        public List<IMailAttachment> Attachments { get; set; }
    }
}