using System;
using System.Collections.Generic;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// this is the starting point for any process,
    /// it only has an outgoing port, nothing can attach to it inbound
    /// </summary>

    [Serializable]
    public class InvisibleAnchorNode : BaseItem
    {
        protected string m_Subject = "";
        protected RolesCollectionItem m_LimitAccessTo = null;
        protected List<string> m_StateOrder;
        protected CustomVariableItem m_KeyVariable;
        protected static string m_Name = "InvisibleAnchorNode";

        public InvisibleAnchorNode()
        {
            m_IconName = "";
            m_AvailableCodeSections = new string[0];
        }

        public override object Properties
        {
            get
            {
                return new InvisibleAnchorNodeProperties();
            }
        }
    }

    public class InvisibleAnchorNodeProperties
    {
        public InvisibleAnchorNodeProperties()
        {
        }
    }
}