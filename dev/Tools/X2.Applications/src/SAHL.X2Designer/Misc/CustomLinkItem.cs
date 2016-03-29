using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Misc
{
    public class CustomLinkItem
    {
        private CustomLink m_CustomLink;
        private CutCopyOperationType m_OperationType;

        public CustomLink customLink
        {
            get
            {
                return m_CustomLink;
            }
            set
            {
                m_CustomLink = value;
            }
        }

        public CutCopyOperationType OperationType
        {
            get
            {
                return m_OperationType;
            }
            set
            {
                m_OperationType = value;
            }
        }
    }
}