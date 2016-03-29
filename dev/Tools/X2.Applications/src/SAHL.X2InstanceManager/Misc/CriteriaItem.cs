using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.X2InstanceManager.Misc
{
    public enum AndOr
    {
        NotSet,
        And,
        Or
    }

    public enum EqualsType
    {
        NotSet,
        EqualTo,
        NotEqualTo
    }



    public class CriteriaItem
    {
        private string m_Description;
        private AndOr m_AndOr;
        private EqualsType m_EqualsType;
        private string m_Value;
        private bool m_Explicit;

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

        public  AndOr andOr
        {
            get
            {
                return m_AndOr;
            }
            set
            {
                m_AndOr = value;
            }
        }

        public EqualsType equalsType
        {
            get
            {
                return m_EqualsType;
            }
            set
            {
                m_EqualsType = value;
            }
        }

        public bool Explicit
        {
            get
            {
                return m_Explicit;
            }
            set
            {
                m_Explicit = value;
            }
        }
        public string value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }
    }
}
