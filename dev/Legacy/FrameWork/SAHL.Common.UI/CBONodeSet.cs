using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.UI;


namespace SAHL.Common.UI
{
    public class CBONodeSet
    {
        //public const string CBONODESET = "CBO";
        //public const string X2NODESET = "X2";

        private CBONodeSetType _nodeSetName;
        List<CBONode> _nodes;
        List<CBONode> _contextNodes;

        CBOMenuNode _selectedNode;
        CBOContextNode _selectedContextNode;
        string _selectedNodeKey;

        public CBONodeSet(CBONodeSetType NodeSetName)
        {
            _nodeSetName = NodeSetName;
            _nodes = new List<CBONode>();
            _contextNodes = new List<CBONode>();
        }

        public List<CBONode> Nodes
        {
            get
            {
                return _nodes;
            }
        }

        public List<CBONode> ContextNodes
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
            _nodes = new List<CBONode>();
            _contextNodes = new List<CBONode>();
        }
    }
}
