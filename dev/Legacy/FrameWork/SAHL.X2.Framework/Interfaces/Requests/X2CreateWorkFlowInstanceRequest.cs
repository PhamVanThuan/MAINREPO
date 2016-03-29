using System;
using System.Collections.Generic;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2CreateWorkFlowInstanceRequest : X2RequestBase
    {
        X2FieldInputList m_FieldInputList;
        string m_SessionId;
        string m_ProcessName;
        string m_WorkFlowName;
        string m_ProcessVersion;
        string m_ActivityName;
        Int64? _SourceInstanceID = null;
        Int32? _ReturnActivityID = null;
        //bool _IgnoreWarnings = false;

        public X2CreateWorkFlowInstanceRequest(string p_SessionId, string p_ProcessName, string p_ProcessVersion, string p_WorkFlowName, string p_ActivityName, bool IgnoreWarnings, object Data)
            :
            base(IgnoreWarnings, Data)
        {
            Populate(p_SessionId, p_ProcessName, p_ProcessVersion, p_WorkFlowName, p_ActivityName, null, null, null);
        }

        public X2CreateWorkFlowInstanceRequest(string p_SessionId, string p_ProcessName, string p_ProcessVersion, string p_WorkFlowName, string p_ActivityName, Dictionary<string, string> FieldInputs, bool IgnoreWarnings, object Data)
            :
            base(IgnoreWarnings, Data)
        {
            Populate(p_SessionId, p_ProcessName, p_ProcessVersion, p_WorkFlowName, p_ActivityName, null, null, FieldInputs);
        }

        public X2CreateWorkFlowInstanceRequest(string p_SessionId, string p_ProcessName, string p_ProcessVersion, string p_WorkFlowName, string p_ActivityName, Int64? SourceInstanceID, Int32? ReturnActivityID, bool IgnoreWarnings, object Data)
            : base(IgnoreWarnings, Data)
        {
            Populate(p_SessionId, p_ProcessName, p_ProcessVersion, p_WorkFlowName, p_ActivityName, SourceInstanceID, ReturnActivityID, null);
        }

        protected void Populate(string p_SessionId, string p_ProcessName, string p_ProcessVersion, string p_WorkFlowName, string p_ActivityName, Int64? SourceInstanceID, Int32? ReturnActivityID, Dictionary<string, string> FieldInputs)
        {
            m_RequestType = RequestType.CreateWorkFlowInstanceRequest;
            m_SessionId = p_SessionId;
            m_ProcessName = p_ProcessName;
            m_ProcessVersion = p_ProcessVersion;
            m_WorkFlowName = p_WorkFlowName;
            m_ActivityName = p_ActivityName;
            _SourceInstanceID = SourceInstanceID;
            _ReturnActivityID = ReturnActivityID;
            _IgnoreWarnings = IgnoreWarnings;
            if (FieldInputs != null)
            {
                m_FieldInputList = new X2FieldInputList(FieldInputs);
            }
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

        public string ProcessName
        {
            get { return m_ProcessName; }
            set { m_ProcessName = value; }
        }

        public string ProcessVersion
        {
            get { return m_ProcessVersion; }
            set { m_ProcessVersion = value; }
        }

        public string WorkFlowName
        {
            get { return m_WorkFlowName; }
            set { m_WorkFlowName = value; }
        }

        public string ActivityName
        {
            get { return m_ActivityName; }
            set { m_ActivityName = value; }
        }

        public Int64? SourceInstanceID
        {
            get { return _SourceInstanceID; }
            set
            {
                _SourceInstanceID = value;
            }
        }

        public Int32? ReturnActivityID
        {
            get { return _ReturnActivityID; }
            set { _ReturnActivityID = value; }
        }

        //public bool IgnoreWarnings { get { return _IgnoreWarnings; } }
    }
}