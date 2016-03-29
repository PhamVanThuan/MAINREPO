using System;
using System.Collections.Generic;
using System.Text;
//using SAHL.X2EngineCommon;
//using SAHL.X2EngineProvider;
using SAHL.Common.X2;
using SAHL.X2.Framework.Interfaces;


namespace SAHL.Common.Web.UI
{
    /// <summary>
    /// Class to hold information about a particular eWork process
    /// </summary>
    [Serializable]
    public class X2Info
    {
        #region Variable Declaration

        private Int64 m_InstanceID = -1;
        private string m_StateName = "";
        private string m_WorkflowName = "";
        private string m_ProcessName = "";
        private string m_SessionID = "";
        private string m_CurrentActivity = "";
        private string m_ChainedActivity = "";
        private X2FieldInputList m_FieldInputs;

        #endregion

        #region Properties

        /// <summary>
        /// The eWork processes FolderID
        /// </summary>
        public Int64 InstanceID
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

        /// <summary>
        /// The current stage of the eWork process
        /// </summary>
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

        /// <summary>
        /// The eWork MapName the process is an instance of.
        /// </summary>
        public string WorkflowName
        {
            get
            {
                return m_WorkflowName;
            }
        }

        public string Processname
        {
            get
            {
                return m_ProcessName;
            }
        }

        /// <summary>
        /// The eWork Session ID obtained by logging in via the Transaction Protocol.
        /// </summary>
        public string SessionID
        {
            get
            {
                return m_SessionID;
            }
            set
            {
                m_SessionID = value;
            }
        }

        /// <summary>
        /// If there is a chained action, this holds the action name
        /// </summary>
        public string ChainedActivity
        {
            get
            {
                return m_ChainedActivity;
            }
            set
            {
                m_ChainedActivity = value;
            }
        }

        /// <summary>
        /// The current action being performed on the eWork process.
        /// </summary>
        public string CurrentActivity
        {
            get
            {
                return m_CurrentActivity;
            }
            set
            {
                m_CurrentActivity = value;
            }
        }

        public X2FieldInputList FieldInputs
        {
            get
            {
                if (m_FieldInputs == null)
                    m_FieldInputs = new X2FieldInputList();
                return m_FieldInputs;
            }
        }

        public void AddFieldInput(string Key, string Value)
        {
            if (m_FieldInputs == null)
                m_FieldInputs = new X2FieldInputList();
            m_FieldInputs.Add(Key, Value);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_InstanceID"></param>
        /// <param name="p_StateName"></param>
        /// <param name="p_WorkflowName"></param>
        /// <param name="CurrentActivity"></param>
        /// <param name="ProcessName"></param>
        public X2Info(Int64 p_InstanceID, string p_StateName, string p_WorkflowName, string CurrentActivity, string ProcessName)
        {
            m_InstanceID = p_InstanceID;
            m_StateName = p_StateName;
            m_WorkflowName = p_WorkflowName;
            m_CurrentActivity = CurrentActivity;
            m_ProcessName = ProcessName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_SessionID"></param>
        /// <param name="p_InstanceID"></param>
        /// <param name="p_StateName"></param>
        /// <param name="p_WorkflowName"></param>
        /// <param name="CurrentActivity"></param>
        /// <param name="ProcessName"></param>
        public X2Info(string p_SessionID, Int64 p_InstanceID, string p_StateName, string p_WorkflowName, string CurrentActivity, string ProcessName)
        {
            m_SessionID = p_SessionID;
            m_InstanceID = p_InstanceID;
            m_StateName = p_StateName;
            m_WorkflowName = p_WorkflowName;
            m_CurrentActivity = CurrentActivity;
            m_ProcessName = ProcessName;
        }

        public void RemoveFieldInput(string Name)
        {
            if (FieldInputs.ContainsKey(Name))
                FieldInputs.Remove(Name);
        }


        #endregion
    }
}
