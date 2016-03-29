using System;
using System.Collections.Generic;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Class used for containing a collection <see cref="SAHLTreeNode"/> objects.
    /// </summary>
    public class SAHLTreeNodeCollection : List<SAHLTreeNode>
    {
        private SAHLTreeNode parentNode = null;

        public SAHLTreeNodeCollection(SAHLTreeNode parentNode)
        {
            this.parentNode = parentNode;
        }

        public new void Add(SAHLTreeNode node)
        {
            base.Add(node);
            node.ParentNode = parentNode;
        }

        /// <summary>
        /// Clears all items from the collection.  This is a recursive clear.
        /// </summary>
        public new void Clear() 
        {
            foreach (SAHLTreeNode node in this) 
            {
                if (node.Nodes.Count > 0)
                    node.Nodes.Clear();
            }
            base.Clear();
        }
    }
}
