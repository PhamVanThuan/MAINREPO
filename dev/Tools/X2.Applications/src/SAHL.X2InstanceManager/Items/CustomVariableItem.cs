using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Items
{
    public class CustomVariableItem
    {
        private string m_Name;
        private string m_Type;

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

 

    }
}
