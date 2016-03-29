namespace SAHL.Services.Interfaces.Search.Models
{
    public class SearchForTaskQueryResult
    {
        public long InstanceId { get; set; }

        public string UserName { get; set; }

        public string Process { get; set; }

        public string Workflow { get; set; }

        public string State { get; set; }

        public string Subject { get; set; }

        public string GenericKeyType { get; set; }

        public string GenericKeyValue { get; set; }

        public string GenericKeyTypeKey { get; set; }

        public string Attribute1Type { get; set; }

        public string Attribute1Value { get; set; }

        public string Attribute1DataType { get; set; }

        public string Attribute2Type { get; set; }

        public string Attribute2Value { get; set; }

        public string Attribute2DataType { get; set; }

        public string Attribute3Type { get; set; }

        public string Attribute3Value { get; set; }

        public string Attribute3DataType { get; set; }

        public string Status { get; set; }
    }
}