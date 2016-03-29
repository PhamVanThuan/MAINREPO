using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SAHL.Web.Services.Internal.DataModel
{
    [DataContract(Name = "ServiceMessageType")]
    public enum ServiceMessageType
    {
        [EnumMember]
        Info,
        [EnumMember]
        Success,
        [EnumMember]
        Warning,
        [EnumMember]
        Error
    }

    [DataContract]
    public class ServiceMessage
    {
        public ServiceMessage() 
        {
            ServiceMessages = new List<Message>(); 
        }

        public ServiceMessage(bool success)
        {
            ServiceMessages = new List<Message>(); 
        }

        [DataMember]
        public bool Success {
            get
            {
                return ServiceMessages.Count == 0;
            }
            set
            {

            }
        }

        [DataMember]
        public List<Message> ServiceMessages { get; set; }
    }

    [DataContract]
    public class Message
    {
        public Message() { }

        public Message(ServiceMessageType serviceMessagetype, string des)
        {
            Description = des;
            MessageType = serviceMessagetype;
        }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public ServiceMessageType MessageType { get; set; }

    }
}