using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public struct X2FieldInput
    {
        public string Name;
        public string Value;
    }

    [Serializable]
    public class X2FieldInputList : Dictionary<string, string>
    {
        public X2FieldInputList()
            : base()
        {
        }

        public X2FieldInputList(IDictionary<string, string> Inputs)
            : base(Inputs)
        {
        }

        public X2FieldInputList(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<FieldInputs>");
            string[] arrKeys = new string[this.Count];
            Keys.CopyTo(arrKeys, 0);
            for (int i = 0; i < this.Keys.Count; i++)
            {
                sb.AppendFormat("<FieldInput Name=\"{0}\">{1}</FieldInput>", arrKeys[i], this[arrKeys[i]]);
            }
            sb.Append("</FieldInputs>");
            return sb.ToString();
        }
    }

    [Serializable]
    public class ListRequestItem
    {
        public Int64 InstnaceID;
        public string ActivityName;

        public ListRequestItem(Int64 InstanceID, string ActivityName)
        {
            this.InstnaceID = InstanceID;
            this.ActivityName = ActivityName;
        }
    }

    public static class RequestIDFactory
    {
        private static int val = 0;

        public static int GetNext()
        {
            return Interlocked.Increment(ref val);
        }
    }
}