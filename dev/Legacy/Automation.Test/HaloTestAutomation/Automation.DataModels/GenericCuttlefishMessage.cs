using System;

namespace Automation.DataModels
{
    public class GenericCuttlefishMessage : IDataModel
    {
        public int ID { get; set; }

        public DateTime MessageDate { get; set; }

        public string MessageContent { get; set; }

        public string MessageType { get; set; }

        public int GenericStatusID { get; set; }

        public int BatchID { get; set; }
    }
}