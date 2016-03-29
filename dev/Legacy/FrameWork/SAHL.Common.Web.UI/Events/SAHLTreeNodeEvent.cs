using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Common.Web.UI.Events
{
    /// <summary>
    /// Delegate for handling <see cref="SAHLTreeView"/> events.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void SAHLTreeNodeEventHandler(object source, SAHLTreeNodeEventArgs e);

    /// <summary>
    /// Class for holding event arguments for <see cref="SAHLTreeView"/> events.
    /// </summary>
    public class SAHLTreeNodeEventArgs : EventArgs
    {
        private SAHLTreeNode _treeNode = null;
        private int _iconIndex = -1;

        public SAHLTreeNodeEventArgs(SAHLTreeNode treeNode)
            : this(treeNode, -1)
        {
            _treeNode = treeNode;
        }

        public SAHLTreeNodeEventArgs(SAHLTreeNode treeNode, int iconIndex)
        {
            _treeNode = treeNode;
            _iconIndex = iconIndex;
        }

        /// <summary>
        /// Gets the index of the icon where the event occurred if applicable.  If an icon 
        /// was not involved in the event this value will be -1.
        /// </summary>
        public int IconIndex
        {
            get
            {
                return _iconIndex;
            }
        }

        /// <summary>
        /// The node where the event occurred.
        /// </summary>
        public SAHLTreeNode TreeNode
        {
            get
            {
                return _treeNode;
            }
        }
    }

}
