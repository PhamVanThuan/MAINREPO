using System;
using System.Collections.Generic;
using System.Text;


namespace SAHL.X2InstanceManager.Items
{
    public class StateItem
    {
        private int m_ID;
        private string m_Name;
        private string m_Type;
        private string m_ForwardState;

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

        public string Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        public string ForwardState
        {
            get
            {
                return m_ForwardState;
            }
            set
            {
                m_ForwardState = value;
            }
        }
        public StateItem() { }
        public StateItem(SAHL.X2.Framework.DataSets.X2.StateRow dr)
        {
            m_ID = dr.ID;
            m_Name = dr.Name;
        }
        public override string ToString()
        {
            return m_Name;
        }
    }
}
