using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Security;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.UI;

namespace SAHL.Common.UI
{
    public class CBOMenuNode : CBONode
    {
        protected ICBOMenu _CBOMenu;

        public ICBOMenu CBOMenu
        {
            get
            {
                return _CBOMenu;
            }
        }

        public override string Description
        {
            get
            {
                if (_CBOMenu == null || _CBOMenu.NodeType == 'D')
                    return _description;
                else
                    return _CBOMenu.Description;
            }
        }

        public override string LongDescription
        {
            get
            {
                if (_CBOMenu == null || _CBOMenu.NodeType == 'D')
                    return _longDescription;
                else
                    return _CBOMenu.Description;
            }
        }

        public override bool IsRemovable
        {
            get
            {
                if (_CBOMenu != null)
                    return _CBOMenu.IsRemovable;

                return _isRemovable;
            }
        }

        public override string URL
        {
            get
            {
                if (_CBOMenu != null)
                    return _CBOMenu.URL;

                return _url;
            }
        }

        public override string MenuIcon
        {
            get
            {
                if (_CBOMenu != null)
                    return _CBOMenu.MenuIcon;

                return _menuIcon;
            }
        }

        public override int GenericKeyTypeKey
        {
            get
            {
                if (_CBOMenu.GenericKeyType != null)
                    return _CBOMenu.GenericKeyType.Key;
                else
                    return -1;
            }
        }
   
        public CBOMenuNode(ICBOMenu CBOMenu, int GenericKey, CBOMenuNode Parent, string Description, string LongDescription)
            : base(GenericKey, Parent as CBONode, Description, LongDescription)
        {
            _CBOMenu = CBOMenu;
        }

        public CBOMenuNode(ICBOMenu CBOMenu, int GenericKey, CBOMenuNode Parent, string Description, string LongDescription, string URL, string MenuIcon, bool IsRemovable)
            : base(GenericKey, Parent as CBONode, Description, LongDescription)
        {
            _CBOMenu = CBOMenu;
            _url = URL;
            _menuIcon = MenuIcon;
            _isRemovable = IsRemovable;
        }

        public CBOMenuNode(Dictionary<string, object> NodeData) : base (NodeData)
        {
            if (NodeData.ContainsKey("CBOMENU"))
                _CBOMenu = NodeData["CBOMENU"] as ICBOMenu;
            else
                _CBOMenu = null;
         }

        public virtual void GetContextNodes(SAHLPrincipal principal, List<CBONode> contextNodes, IDictionary<int, object> allowedContextMenus)
        {
            if (_CBOMenu != null)
            {
                // SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(principal);
                AddContextNodes(allowedContextMenus, GetSortedMenuItems(_CBOMenu.ContextMenus), contextNodes, null);
            }
        }

        private IList<IContextMenu> GetSortedMenuItems(IEventList<IContextMenu> unsortedMenus)
        {
            List<IContextMenu> sortedMenus = new List<IContextMenu>(unsortedMenus);
            sortedMenus.Sort(
              delegate(IContextMenu m1, IContextMenu m2)
              {
                  return m1.Sequence.CompareTo(m2.Sequence);
              });
            return sortedMenus;
        }

        private bool CheckForParentNodeByType(SAHLPrincipal principal, CBONode currentNode, Type parentNodeType)
        {
            bool nodeFound = false;

            if (currentNode.GetType() == parentNodeType)
                nodeFound = true;
            else if (currentNode.ParentNode != null && nodeFound == false)
                nodeFound = CheckForParentNodeByType(principal, currentNode.ParentNode, parentNodeType);

            return nodeFound;
        }

        private void AddContextNodes(IDictionary<int, object> allowedContextMenus, IList<IContextMenu> contextMenus, List<CBONode> contextNodes, CBONode parentNode)
        {

            foreach (IContextMenu contextMenu in contextMenus)
            {
                CBOContextNode node = new CBOContextNode(contextMenu, contextMenu.Key, parentNode, contextMenu.Description, contextMenu.Description, contextMenu.URL, false);

                if (parentNode == null)
                {
                    // if (CheckFeatureAccess(contextMenu, allowedContextMenus) == true)
                    if (allowedContextMenus.ContainsKey(contextMenu.Key))
                        contextNodes.Add(node);
                }
                else
                {
                    // if (CheckFeatureAccess(contextMenu, allowedContextMenus) == true)
                    if (allowedContextMenus.ContainsKey(contextMenu.Key))
                        parentNode.ChildNodes.Add(node);
                }

                if (contextMenu.ChildMenus.Count > 0)
                    AddContextNodes(allowedContextMenus, GetSortedMenuItems(contextMenu.ChildMenus), null, node);
            }

        }

        //private bool CheckFeatureAccess(IContextMenu contextMenu, IEventList<IContextMenu> allowedContextMenus)
        //{
        //    if (contextMenu.Feature == null)
        //        return true;

        //    foreach (IContextMenu cm in allowedContextMenus)
        //    {
        //        if (contextMenu.Key == cm.Key)
        //            return true;
        //    }

        //    return false;
        //}



    }
}
