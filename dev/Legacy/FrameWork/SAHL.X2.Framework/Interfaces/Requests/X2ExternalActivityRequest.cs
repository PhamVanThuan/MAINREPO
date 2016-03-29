using System;
using System.Xml;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2ExternalActivityRequest : X2SystemRequest
    {
        private int m_ID;
        private int m_ExternalActivityID;
        private int m_WorkFlowID;
        private DateTime m_ActivationTime;
        private X2FieldInputList m_Inputs;
        private Int64? _SourceInstanceID = null;
        private Int32? _ReturnActivityID = null;

        public X2ExternalActivityRequest(int ID, Int64 ActivatingInstanceID, int ExternalActivityID, int WorkFlowID, DateTime ActivationTime, string ActivityXMLData, bool IgnoreWarnings)
            :
            base(ActivatingInstanceID, "", IgnoreWarnings, null)
        {
            Populate(ID, ActivatingInstanceID, ExternalActivityID, WorkFlowID, ActivationTime, ActivityXMLData, null, null);
        }

        public X2ExternalActivityRequest(int ID, Int64 ActivatingInstanceID, int ExternalActivityID, int WorkFlowID, DateTime ActivationTime, string ActivityXMLData, Int64? SourceInstanceID, Int32? ReturnActivityID, bool IgnoreWarnings)
            :
            base(ActivatingInstanceID, "", IgnoreWarnings, null)
        {
            Populate(ID, ActivatingInstanceID, ExternalActivityID, WorkFlowID, ActivationTime, ActivityXMLData, SourceInstanceID, ReturnActivityID);
        }

        protected void Populate(int ID, Int64 ActivatingInstanceID, int ExternalActivityID, int WorkFlowID, DateTime ActivationTime, string ActivityXMLData, Int64? SourceInstanceID, Int32? ReturnActivityID)
        {
            m_RequestType = RequestType.ExternalActivityRequest;
            m_ID = ID;
            m_ExternalActivityID = ExternalActivityID;
            m_WorkFlowID = WorkFlowID;
            m_ActivationTime = ActivationTime;

            // process the data
            if (ActivityXMLData != null && ActivityXMLData.Length > 0)
            {
                try
                {
                    // load the xml and make the items into field inputs
                    XmlDocument XDoc = new XmlDocument();
                    XDoc.LoadXml(ActivityXMLData);
                    XmlNodeList Fields = XDoc.SelectNodes("./FieldInputs/FieldInput");
                    if (Fields != null)
                    {
                        for (int i = 0; i < Fields.Count; i++)
                        {
                            Inputs.Add(Fields[i].Attributes[0].Value, Fields[i].InnerXml);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public int ExternalActivityID
        {
            get { return m_ExternalActivityID; }
            set { m_ExternalActivityID = value; }
        }

        public int WorkFlowID
        {
            get { return m_WorkFlowID; }
            set { m_WorkFlowID = value; }
        }

        public DateTime ActivationTime
        {
            get { return m_ActivationTime; }
            set { m_ActivationTime = value; }
        }

        public Int64 ActivatingInstanceID
        {
            get { return m_InstanceId; }
            set { m_InstanceId = value; }
        }

        public X2FieldInputList Inputs
        {
            get
            {
                if (m_Inputs == null)
                    m_Inputs = new X2FieldInputList();
                return m_Inputs;
            }
            set { m_Inputs = value; }
        }

        public Int64? SourceInstanceID
        {
            get
            {
                return _SourceInstanceID;
            }
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
    }
}