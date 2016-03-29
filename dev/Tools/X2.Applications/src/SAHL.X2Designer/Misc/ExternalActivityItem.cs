using System;

namespace SAHL.X2Designer.Misc
{
    [Serializable]
    public class ExternalActivityItem
    {
        String m_ExternalActivity = "";
        String m_Description = "";

        public ExternalActivityItem()
        {
        }

        public string ExternalActivity
        {
            get
            {
                return m_ExternalActivity;
            }
            set
            {
                m_ExternalActivity = value;
            }
        }

        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }
    }
}