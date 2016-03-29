using System;
using System.Collections.Generic;
using System.Text;

namespace EWorkTest
{
    [Serializable]
    public class eWorkInfo
    {
        #region Variable Declaration

        private string m_FolderID;
        private string m_StageName;
        private string m_MapName;
        private string m_SessionID;
        private string m_ResponseData;
        private string m_CurrentAction;

        #endregion

        #region Properties

        /// <summary>
        /// The eWork processes FolderID
        /// </summary>
        public string FolderID
        {
            get
            {
                return m_FolderID;
            }
        }

        /// <summary>
        /// The current stage of the eWork process
        /// </summary>
        public string StageName
        {
            get
            {
                return m_StageName;
            }
            set
            {
                m_StageName = value;
            }
        }

        /// <summary>
        /// The eWork MapName the process is an instance of.
        /// </summary>
        public string MapName
        {
            get
            {
                return m_MapName;
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
        /// Information returned in the last reponse when using the transaction protocol.
        /// </summary>
        public string ResponseData
        {
            get
            {
                return m_ResponseData;
            }
            set
            {
                m_ResponseData = value;
            }
        }

        /// <summary>
        /// The current action being performed on the eWork process.
        /// </summary>
        public string CurrentAction
        {
            get
            {
                return m_CurrentAction;
            }
            set
            {
                m_CurrentAction = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_FolderID">An eWork FolderID</param>
        /// <param name="p_StageName">The stage of the eWork process</param>
        /// <param name="p_MapName">The name of the eWork Map</param>
        public eWorkInfo(string p_FolderID, string p_StageName, string p_MapName)
        {
            m_FolderID = p_FolderID;
            m_StageName = p_StageName;
            m_MapName = p_MapName;
        }

        #endregion
    }
}
