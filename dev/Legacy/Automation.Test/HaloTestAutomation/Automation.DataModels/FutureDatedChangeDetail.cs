using System;

namespace Automation.DataModels
{
    public class FutureDatedChangeDetail
    {
        public int FutureDatedChangeDetailKey { get; set; }

        public int ReferenceKey { get; set; }

        public string Action { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string Value { get; set; }

        public string UserID { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}