using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SAHL.X2InstanceManager.Items
{
    public class InstanceItem
    {
        private int m_ID;
        private string m_Name;
        private string m_Subject;
        private string m_CreatorADUserName;
        private string m_CreationDate;
        private string m_StateChangeDate;
        private string m_DeadlineDate;
        private string m_ActivityDate;
        private string m_ActivityADUserName;
        private string m_ActivityName;
        private string m_Priority;
        private int m_ParentInstance;
        private bool m_Locked;

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
        public string Subject
        {
            get
            {
                return m_Subject;
            }
            set
            {
                m_Subject = value;
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
        public string ActivityADUserName
        {
            get
            {
                return m_ActivityADUserName;
            }
            set
            {
                m_ActivityADUserName = value;
            }
        }
        public string ActivityName
        {
            get
            {
                return m_ActivityName;
            }
            set
            {
                m_ActivityName = value;
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

        public int ParentInstance
        {
            get
            {
                return m_ParentInstance;
            }
            set
            {
                m_ParentInstance = value;
            }
        }

        public bool Locked
        {
            get
            {
                return m_Locked;
            }
            set
            {
                m_Locked = value;
            }
        }
    }

    public class SingleInstance
    {
        Int64 _InstanceID;
        public Int64 ID { get { return _InstanceID; } }
        string _Subject;
        public string Subject { get { return _Subject; } }
        string _Name;
        public string Name { get { return _Name; } }
        string _CreateorADUserName;
        public string CreatorADUserName { get { return _CreateorADUserName; } }
        string _ActivityName;
        public string LockedActivity { get { return _ActivityName; } }
        string _ActivityADUSerName;
        public string LockUser { get { return _ActivityADUSerName; } }
        string _ADUserName;
        public string WorkListUser { get { return _ADUserName; } }
        public SingleInstance(SqlDataReader rdr)
        {
            _InstanceID = Convert.ToInt64(rdr[0]);
            _Name = rdr[1].ToString();
            _Subject = rdr[2].ToString();
            _CreateorADUserName = rdr[3].ToString();
            _ActivityADUSerName = rdr[4].ToString();
            _ActivityName = rdr[5].ToString();
            _ADUserName = rdr[6].ToString();
        }
    }

}
