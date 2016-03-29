using System;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ActivityStartRequest : X2RequestBase
    {
        X2FieldInputList m_FieldInputList;
        string m_SessionId;
        Int64 m_InstanceId;
        string m_ActivityName;

        public X2ActivityStartRequest(string p_SessionId, Int64 p_InstanceId, string p_ActivityName, bool IgnoreWarnings, object Data)
            : base(IgnoreWarnings, Data)
        {
            m_RequestType = RequestType.ActivityStartRequest;
            m_SessionId = p_SessionId;
            m_InstanceId = p_InstanceId;
            m_ActivityName = p_ActivityName;
        }

        public X2FieldInputList FieldInputList
        {
            get
            {
                if (m_FieldInputList == null)
                    m_FieldInputList = new X2FieldInputList();
                return m_FieldInputList;
            }
        }

        public string SessionId
        {
            get { return m_SessionId; }
            set { m_SessionId = value; }
        }

        public Int64 InstanceId
        {
            get { return m_InstanceId; }
            set { m_InstanceId = value; }
        }

        public string ActivityName
        {
            get { return m_ActivityName; }
            set { m_ActivityName = value; }
        }
    }
}