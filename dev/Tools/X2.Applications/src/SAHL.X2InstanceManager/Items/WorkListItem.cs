using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Items
{
    public class WorkListTrackListItem
    {
        private string m_ADUserName;
        private string m_ListDate;
        private string m_Message;

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

        public string ListDate
        {
            get
            {
                return m_ListDate;
            }
            set
            {
                m_ListDate = value;
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
    }
}
