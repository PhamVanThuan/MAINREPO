using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;

namespace SAHL.Common.UI
{
    public class CBONodeSet
    {
        public const string CBONODESET = "CBO";
        public const string X2NODESET = "X2";

        private string _nodeSetName;
        IEventList<CBONode> _nodes;
        IEventList<CBONode> _contextNodes;

        CBOMenuNode _selectedNode;
        CBOContextNode _selectedContextNode;
        string _selectedNodeKey;

        public CBONodeSet(string NodeSetName)
        {
            _nodeSetName = NodeSetName;
            _nodes = new EventList<CBONode>();
            _contextNodes = new EventList<CBONode>();
        }

        public IEventList<CBONode> Nodes
        {
            get
            {
                return _nodes;
            }
        }

        public IEventList<CBONode> ContextNodes
        {
            get
            {
                return _contextNodes;
            }
        }

        public string SelectedNodeKey
        {
            get
            {
                return _selectedNodeKey;
            }
            set
            {
                _selectedNodeKey = value;
            }
        }


        public CBOMenuNode SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            set
            {
                _selectedNode = value;
            }
        }

        public CBOContextNode SelectedContextNode
        {
            get
            {
                return _selectedContextNode;
            }
            set
            {
                _selectedContextNode = value;
            }
        }

        /// <summary>
        /// Clears all node collections
        /// </summary>
        public void ClearAll()
        {
            _nodes = new EventList<CBONode>();
            _contextNodes = new EventList<CBONode>();
        }
    }
}
