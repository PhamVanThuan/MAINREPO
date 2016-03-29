using System.Drawing;
using Northwoods.Go;

namespace SAHL.X2Designer.Misc
{
    public class NewlyCopiedItem
    {
        private GoObject m_NewItem;
        private PointF m_Location;

        public GoObject NewItem
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

        public PointF Location
        {
            get
            {
                return m_Location;
            }
            set
            {
                m_Location = value;
            }
        }
    }
}