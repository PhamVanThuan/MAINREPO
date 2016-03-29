using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MessageTypeDataModel :  IDataModel
    {
        public MessageTypeDataModel(int messageTypeKey, string description)
        {
            this.MessageTypeKey = messageTypeKey;
            this.Description = description;
		
        }		

        public int MessageTypeKey { get; set; }

        public string Description { get; set; }
    }
}