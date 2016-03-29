using System;
using System.Collections.Generic;

namespace SAHL.Core.Exchange
{
    public interface IMailMessage
    {
        string UniqueExchangeId { get; set; }
        string From { get; set; } 
        string To { get; set; } 
        string Subject { get; set; }
        string Body { get; set; }
        DateTime DateRecieved { get; set; }
        DateTime DateSent { get; set; }
        List<IMailAttachment> Attachments { get; set; }
    }

}