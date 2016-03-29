using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Items
{
    public class LogItem
    {
        private string m_Date;
        private string m_Process;
        private string m_WorkFlow;
        private string m_InstanceID;
        private string m_State;
        private string m_Activity;
        private string m_ADUsername;
        private string m_Message;
        private string m_StackTrace;

        public string Date
        {
            get
            {
                return m_Date;
            }
            set
            {
                m_Date = value;
            }
        }

        public string Process
        {
            get
            {
                return m_Process;
            }
            set
            {
                m_Process = value;
            }
        }

        public string WorkFlow
        {
            get
            {
                return m_WorkFlow;
            }
            set
            {
                m_WorkFlow = value;
            }
        }

        public string InstanceID
        {
            get
            {
                return m_InstanceID;
            }
            set
            {
                m_InstanceID = value;
            }
        }

        public string State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
            }
        }

        public string Activity
        {
            get
            {
                return m_Activity;
            }
            set
            {
                m_Activity = value;
            }
        }

        public string ADUserName
        {
            get
            {
                return m_ADUsername;
            }
            set
            {
                m_ADUsername = value;
            }
        }

        public string Message
        {
            get
            {
                return m_Message;
            }
            set
            {
                m_Message = value;
            }
        }

        public string StackTrace
        {
            get
            {
                return m_StackTrace;
            }
            set
            {
                m_StackTrace = value;
            }
        }
    }
}
