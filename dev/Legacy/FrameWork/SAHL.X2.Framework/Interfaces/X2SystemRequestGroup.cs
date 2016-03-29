using System;
using System.Collections.Generic;

namespace SAHL.X2.Framework.Interfaces
{
    [Serializable]
    public class X2SystemRequestGroup : X2RequestBase
    {
        protected Int64 m_InstanceId;
        protected List<string> m_Activities;
        protected List<bool> m_ProcessActivities;
        protected object _Data = null;

        public X2SystemRequestGroup(Int64 InstanceId, List<string> p_Activities, bool IgnoreWarnings, object Data)
            : base(IgnoreWarnings, Data)
        {
            m_RequestType = RequestType.SystemRequestGroup;
            m_InstanceId = InstanceId;
            m_Activities = new List<string>();
            for (int i = 0; i < p_Activities.Count; i++)
                m_Activities.Add(p_Activities[i]);
        }

        public Int64 InstanceId
        {
            get { return m_InstanceId; }
            set { m_InstanceId = value; }
        }

        public List<string> Activities
        {
            get { return m_Activities; }
        }
    }
}