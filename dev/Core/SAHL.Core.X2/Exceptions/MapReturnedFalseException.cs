using SAHL.Core.SystemMessages;
using System;
using System.Runtime.Serialization;

namespace SAHL.Core.X2.Exceptions
{
    [Serializable]
    public class MapReturnedFalseException : Exception
    {
        public ISystemMessageCollection Messages { get; protected set; }

        public MapReturnedFalseException(SerializationInfo info, StreamingContext context)
        {
        }

        public MapReturnedFalseException(ISystemMessageCollection messages)
            : base("Map Returned False Exception")
        {
            this.Messages = messages;
        }
    }
}