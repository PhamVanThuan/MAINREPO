using System;

namespace Automation.DataModels
{
    public class X2WorkflowList : IDataModel
    {
        public int ID { get; set; }

        public Int64 InstanceID { get; set; }

        public string ADUserName { get; set; }

        public DateTime? ListDate { get; set; }

        public string Message { get; set; }

        public string StateName { get; set; }
    }
}