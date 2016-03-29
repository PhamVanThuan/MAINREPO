using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class XmlHistory : IComparable<XmlHistory>
    {
        public int XMLHistoryKey { get; set; }

        public GenericKeyTypeEnum GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public string XMLData { get; set; }

        public DateTime InsertDate { get; set; }

        public int CompareTo(XmlHistory other)
        {
            throw new NotImplementedException();
        }
    }
}