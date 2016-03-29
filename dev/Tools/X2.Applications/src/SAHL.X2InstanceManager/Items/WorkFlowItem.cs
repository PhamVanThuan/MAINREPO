using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Items
{
    public class WorkFlowItem
    {
        private int m_WorkFlowID;
        private string m_WorkFlowName;

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
