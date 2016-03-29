using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SAHL.X2InstanceManager.Items
{
    public class WorkFlowInstanceItem
    {
        private string m_StateName;
        private string m_StateType;
        private Int64 m_ID;
        private string m_InstanceName;
        private string m_Subject;
        private string m_CreatorADUserName;
        private string m_ActivityADUserName;
        private string m_ActivityName;
        private string _AssignedUser;
        private Int64 _ParentInstance = -1;
        private Int64 _SourceInstance = -1;
        private bool m_Locked;
        int _StateID;

        public WorkFlowInstanceItem(SqlDataReader rdr)
        {
            m_StateName = rdr[0].ToString();
            m_StateType = rdr[1].ToString();
            m_ID = Convert.ToInt64(rdr[2]);
            m_InstanceName = rdr[3].ToString();
            m_Subject = rdr[4].ToString();
            m_CreatorADUserName = rdr[5].ToString();
            m_ActivityADUserName = rdr[6].ToString();
            m_ActivityName = rdr[7].ToString();
            _AssignedUser = rdr[8].ToString();
            if (!DBNull.ReferenceEquals(DBNull.Value, rdr[9]))
                _ParentInstance = Convert.ToInt64(rdr[9]);
            if (!DBNull.ReferenceEquals(DBNull.Value, rdr[10]))
                _SourceInstance = Convert.ToInt64(rdr[10]);
            _StateID = Convert.ToInt32(rdr[11]);
        }

        public Int64 ID
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

        public string StateName
        {
            get
            {
                return m_StateName;
            }
            set
            {
                m_StateName = value;
            }     
        }

        public string StateType
        {
            get
            {
                return m_StateType;
            }
            set
            {
                m_StateType = value;
            }
        }

        public string InstanceName
        {
            get
            {
                return m_InstanceName;
            }
            set
            {
                m_InstanceName = value;
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

        public bool Locked
        {
            get
            {
                if (string.IsNullOrEmpty(m_ActivityName))
                    return false;
                return true;
            }

        }

        public Int64 ParentInstanceID { get { return _ParentInstance; } }
        public Int64 SourceInstanceID { get { return _SourceInstance; } }
        public string AssignedUser { get { return _AssignedUser; } }
        public int StateID { get { return _StateID; } }
        
    }
}
