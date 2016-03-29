using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.DomainMessages
{
  [Serializable()]
    public class Information : DomainMessage
    {
        public Information(string Message, string Details)
            : base(Message, Details)
        {
          _messageType = DomainMessageType.Info;
        }
    }
}
