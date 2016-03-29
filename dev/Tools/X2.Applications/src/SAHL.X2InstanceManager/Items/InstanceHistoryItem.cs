using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Items
{
    public class InstanceHistoryItem
    {
        private int m_ID;
        private string m_State;
        private string m_Activity;
        private string m_CreatorADUserName;
        private string m_CreationDate;
        private string m_StateChangeDate;
        private string m_DeadlineDate;
        private string m_ActivityDate;
        private string m_ADUserName;
        private string m_Priority;

        public int ID
        {
            get
            {
                return m_ID;
            }
            set
            {
                m_ID = value;
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

        public string CreatorADUserName
        {
            get
            {
                return m_CreatorADUserName;
            }
            set
            {
                m_CreatorADUserName = value;
            }
        }
        public string CreationDate
        {
            get
            {
                return m_CreationDate;
            }
            set
            {
                m_CreationDate = value;
            }
        }
        public string StateChangeDate
        {
            get
            {
                return m_StateChangeDate;
            }
            set
            {
                m_StateChangeDate = value;
            }
        }
        public string DeadlineDate
        {
            get
            {
                return m_DeadlineDate;
            }
            set
            {
                m_DeadlineDate = value;
            }
        }
        public string ActivityDate
        {
            get
            {
                return m_ActivityDate;
            }
            set
            {
                m_ActivityDate = value;
            }
        }
        public string ADUserName
        {
            get
            {
                return m_ADUserName;
            }
            set
            {
                m_ADUserName = value;
            }
        }
        public string Priority
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
    }
}
