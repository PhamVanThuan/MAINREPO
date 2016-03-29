using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Security;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.UI
{
    public class CBOContextNode : CBONode
    {
        protected IContextMenu _contextMenu;
        protected bool _isActivity;

        public IContextMenu ContextMenu
        {
            get { return _contextMenu; }
        }

        public override string Description
        {
            get
            {
                if (_contextMenu == null)
                    return _description;

                return _contextMenu.Description;
            }
        }

        public override string LongDescription
        {
            get
            {
                if (_contextMenu == null)
                    return _longDescription;

                return _contextMenu.Description;
            }
        }

        public override string URL
        {
            get
            {
                if (_contextMenu != null)
                    return _contextMenu.URL;

                return _url;
            }
        }

        public bool IsActivity
        {
            get { return _isActivity; }
        }

        public CBOContextNode(IContextMenu ContextMenu, int GenericKey, CBONode Parent, string Description, string LongDescription, string url, bool IsActivity)
            : base(GenericKey, Parent, Description, LongDescription)
        {
            _contextMenu = ContextMenu;
            _url = url;
            _isRemovable = false;
            _isActivity = IsActivity;
        }

        public CBOContextNode(Dictionary<string, object> NodeData)
            : base(NodeData)
        {
            if (NodeData.ContainsKey("CONTEXTMENU"))
                _contextMenu = NodeData["CONTEXTMENU"] as IContextMenu;
            else
                _contextMenu = null;
        }
    }
}
