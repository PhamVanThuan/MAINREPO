using System.Drawing;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Misc
{
    public class SelectionItem
    {
        private BaseItem m_BaseItem;
        private string m_NewName;
        private PointF m_NewLocation;
        private string m_OriginalName;
        private BaseItem m_NewItem;
        private WorkFlow m_OriginalWorkFlow;

        private CutCopyOperationType m_OperationType;

        public BaseItem baseItem
        {
            get
            {
                return m_BaseItem;
            }
            set
            {
                m_BaseItem = value;
            }
        }

        public string NewName
        {
            get
            {
                return m_NewName;
            }
            set
            {
                m_NewName = value;
            }
        }

        public PointF NewLocation
        {
            get
            {
                return m_NewLocation;
            }
            set
            {
                m_NewLocation = value;
            }
        }

        public string OriginalName
        {
            get
            {
                return m_OriginalName;
            }
            set
            {
                m_OriginalName = value;
            }
        }

        public BaseItem NewItem
        {
            get
            {
                return m_NewItem;
            }
            set
            {
                m_NewItem = value;
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

        public WorkFlow OriginalWorkflow
        {
            get
            {
                return m_OriginalWorkFlow;
            }
            set
            {
                m_OriginalWorkFlow = value;
            }
        }
    }
}