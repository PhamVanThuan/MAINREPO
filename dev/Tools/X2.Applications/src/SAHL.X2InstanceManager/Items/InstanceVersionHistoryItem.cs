using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Items
{
    public class InstanceVersionHistoryItem
    {
        private int m_InstanceID;
        private int m_WorkFlowID;
        private int m_WorkFlowHistoryID;
        private string m_WorkFlowName;



        public int InstanceID
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

        public int WorkFlowID
        {
            get
            {
                return m_WorkFlowID;
            }
            set
            {
                m_WorkFlowID = value;
            }
        }

        public int WorkFlowHistoryID
        {
            get
            {
                return m_WorkFlowHistoryID;
            }
            set
            {
                m_WorkFlowHistoryID = value;
            }
        }

        public string WorkFlowName
        {
            get
            {
                return m_WorkFlowName;
            }
            set
            {
                m_WorkFlowName = value;
            }
        }
    }
}
