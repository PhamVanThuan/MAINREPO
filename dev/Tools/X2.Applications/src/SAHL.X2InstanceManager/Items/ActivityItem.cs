using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Items
{
    public class ActivityItem
    {
        private string m_Name;
        private string m_Type;
        private string m_FromState;
        private string m_ToState;
        private int m_Priority;
        private string m_ExternalActivityTarget;
        private string m_ActivatedByExternalActivity;

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        public int Priority
        {
            get
            {
                return m_Priority;
            }
            set
            {
                m_Priority = value;
            }
        }

        public string FromState
        {
            get
            {
                return m_FromState;
            }
            set
            {
                m_FromState = value;
            }
        }

        public string ToState
        {
            get
            {
                return m_ToState;
            }
            set
            {
                m_ToState = value;
            }
        }

        public string ExternalActivityTarget
        {
            get
            {
                return m_ExternalActivityTarget;
            }
            set
            {
                m_ExternalActivityTarget = value;
            }
        }

        public string ActivatedByExternalActivity
        {
            get
            {
                return m_ActivatedByExternalActivity;
            }
            set
            {
                m_ActivatedByExternalActivity = value;
            }
        }
    }
}
