using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Security;
using SAHL.Common.Globals;

namespace SAHL.Common.UI
{
    public class CBONode
    {
        protected int _cboNodeKey = -1;

        protected string _cboUniqueKey = "";

        protected int _genericKey = -1;

        protected int _genericKeyTypeKey = -1;

        protected int _originationSourceKey = -1;

        protected string _description = "";

        protected string _longDescription = "";

        protected bool _isRemovable;

        protected string _url;

        protected string _menuIcon;

        protected string _originationSourceIcon;

        protected IEventList<CBONode> _childNodes;

        protected CBONode _parentNode;

        public CBONode(int GenericKey, CBONode Parent, string Description, string LongDescription)
        {
            _genericKey = GenericKey;
            _parentNode = Parent;
            _description = Description;
            _longDescription = LongDescription;
            _cboNodeKey = CBONodeKeyGenerator.GetNextKey();

            //if (_genericKey != -1)
            //    _cboUniqueKey = "CN_" + _genericKey;
            //else
                _cboUniqueKey = "CN_" + _cboNodeKey;
        }
  
        public CBONode(Dictionary<string, object> NodeData)
        {
            CheckNodeData(NodeData);
            _genericKey = Convert.ToInt32(NodeData["GENERICKEY"]);
            _parentNode = NodeData["PARENTNODE"] as CBONode;
            _description = NodeData["DESCRIPTION"]as string;
            _longDescription = NodeData["LONGDESCRIPTION"] as string;
            _originationSourceKey = Convert.ToInt32(NodeData["ORIGINATIONSOURCEKEY"]);
        }

        private void CheckNodeData(Dictionary<string, object> NodeData)
        {
            if (!NodeData.ContainsKey("GENERICKEY"))
                NodeData.Add("GENERICKEY", -1);
            if (NodeData["GENERICKEY"] == null || NodeData["GENERICKEY"] == DBNull.Value)
                NodeData["GENERICKEY"] = -1;
            if (!NodeData.ContainsKey("PARENTNODE"))
                NodeData.Add("PARENTNODE", null);
            if (!NodeData.ContainsKey("DESCRIPTION"))
                NodeData.Add("DESCRIPTION", "");
            if (!NodeData.ContainsKey("LONGDESCRIPTION"))
                NodeData.Add("LONGDESCRIPTION", "");
            if (!NodeData.ContainsKey("ORIGINATIONSOURCEKEY"))
                NodeData.Add("ORIGINATIONSOURCEKEY", -1);
            if (NodeData["ORIGINATIONSOURCEKEY"] == null || NodeData["ORIGINATIONSOURCEKEY"] == DBNull.Value)
                NodeData["ORIGINATIONSOURCEKEY"] = -1;

            _cboNodeKey = CBONodeKeyGenerator.GetNextKey();
            
            //if (_genericKey != -1)
            //    _cboUniqueKey = "CN_" + _genericKey;
            //else
                _cboUniqueKey = "CN_" + _cboNodeKey;
        }

        //public virtual int CBONodeKey
        //{
        //    get
        //    {
        //        return _cboNodeKey;
        //    }
        //}

        public virtual string CBOUniqueKey
        {
            get
            {
                return _cboUniqueKey;
            }
        }

        public virtual int GenericKey
        {
            get
            {
                return _genericKey;
            }
        }

        public virtual int GenericKeyTypeKey
        {
            get
            {
                return _genericKeyTypeKey;
            }
        }

        public virtual int OriginationSourceKey
        {
            get
            {
                return _originationSourceKey;
            }
        }

        public virtual string Description
        {
            get
            {
                return _description;
            }
        }

        public virtual string LongDescription
        {
            get
            {
                return _longDescription;
            }
        }

        public virtual bool IsRemovable
        {
            get
            {
                return _isRemovable;
            }
        }

        public virtual string URL
        {
            get
            {
                 return _url;
            }
        }

        public virtual string MenuIcon
        {
            get
            {
                return _menuIcon;
            }
        }

        public virtual string OriginationSourceIcon
        {
            get
            {
                if (_originationSourceKey != -1)
                    return "~\\Images\\originationsources\\" + _originationSourceKey.ToString() + ".gif";
                else
                    return "";
            }
        }

        public virtual CBONode ParentNode
        {
            get
            {
                return _parentNode;
            }
            set
            {
                _parentNode = value;
            }
        }

        public virtual IEventList<CBONode> ChildNodes
        {
            get
            {
                if (_childNodes == null)
                    _childNodes = new EventList<CBONode>();

                return _childNodes;
            }
        }

        /// <summary>
        /// Gets a node with the specified GenericKeyType according to <c>genericKeyType</c> by traversing up the menu tree.
        /// </summary>
        /// <param name="genericKeyType"></param>
        /// <returns>The first node having a GenericTypeKey matching <c>genericKeyType</c>.  If the 
        /// current node is of the specified it will be returned, otherwise the method will traverse up the tree to find the 
        /// first matching node.</returns>
        public CBONode GetNodeByType(GenericKeyTypes genericKeyType)
        {
            CBONode node = this;
            int compareValue = (int)genericKeyType;
            while (node != null)
            {
                if (node.GenericKeyTypeKey == compareValue)
                    return node;
                node = node.ParentNode as CBONode;
            }
            return null;
        }
    }
}
