namespace SAHL.X2.Framework.DataAccess
{
    public class UpdateInformation
    {
        public UpdateInformation()
        { }

        public UpdateInformation(string p_sUpdateName, string p_sInsertName, string p_sDeleteName, string p_sApplicationName, bool p_bInitiateCollections)
        {
            m_UpdateName = p_sUpdateName;
            m_DeleteName = p_sDeleteName;
            m_InsertName = p_sInsertName;
            m_ApplicationName = p_sApplicationName;
            if (p_bInitiateCollections)
            {
                m_DeleteParameters = new ParameterCollection();
                m_InsertParameters = new ParameterCollection();
                m_UpdateParameters = new ParameterCollection();
            }
        }

        #region Variable Declaration

        private string m_UpdateName;
        private string m_InsertName;
        private string m_DeleteName;
        private string m_ApplicationName;

        private ParameterCollection m_UpdateParameters;
        private ParameterCollection m_InsertParameters;
        private ParameterCollection m_DeleteParameters;

        #endregion Variable Declaration

        /// <summary>
        /// Set the names of the queries to use
        /// </summary>
        /// <param name="p_sUpdateName">The name of the query to be used for updates.</param>
        /// <param name="p_sInsertName">The name of the query to be used for inserts.</param>
        /// <param name="p_sDeleteName">The name of the query to be used for deletes.</param>
        /// <param name="p_sApplicationName">The application name associated with all queries.</param>
        public void SetNames(string p_sUpdateName, string p_sInsertName, string p_sDeleteName, string p_sApplicationName)
        {
            m_UpdateName = p_sUpdateName;
            m_DeleteName = p_sDeleteName;
            m_InsertName = p_sInsertName;
            m_ApplicationName = p_sApplicationName;
        }

        public string UpdateName
        {
            get { return m_UpdateName; }
            set { m_UpdateName = value; }
        }

        public string InsertName
        {
            get { return m_InsertName; }
            set { m_InsertName = value; }
        }

        public string DeleteName
        {
            get { return m_DeleteName; }
            set { m_DeleteName = value; }
        }

        public string ApplicationName
        {
            get { return m_ApplicationName; }
            set { m_ApplicationName = value; }
        }

        //  public string UpdateName, InsertName, DeleteName, ApplicationName;
        public ParameterCollection UpdateParameters
        {
            get { return m_UpdateParameters; }
            set { m_UpdateParameters = value; }
        }

        public ParameterCollection InsertParameters
        {
            get { return m_InsertParameters; }
            set { m_InsertParameters = value; }
        }

        public ParameterCollection DeleteParameters
        {
            get { return m_DeleteParameters; }
            set { m_DeleteParameters = value; }
        }
    }
}