using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Items
{
    public class RoleItem
    {
        private string m_Name;
        private string m_Description;
        private bool m_IsDynamic;

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

        public bool IsDynamic
        {
            get
            {
                return m_IsDynamic;
            }
            set
            {
                m_IsDynamic = value;
            }
        }
    }
}
