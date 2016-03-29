using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using System.Text.RegularExpressions;
using SAHL.Common.Factories;
using System.Collections.Specialized;
using SAHL.Common.UI;
using SAHL.Common.CacheData;

namespace SAHL.Common.UI.CBOSecurityFilters
{

    /// <summary>
    /// This class is used to associate a collection of nodes with a usergroup.  It can be used 
    /// </summary>
    public class UserGroupNodes
    {
        private string _userGroup;
        private StringCollection _nodes;

        public UserGroupNodes()
        {
            _nodes = new StringCollection();
        }

        public UserGroupNodes(string UserGroup, StringCollection Nodes)
        {
            _userGroup = UserGroup;
            _nodes = Nodes;
        }

        public string UserGroup
        {
            get { return _userGroup; }
            set { _userGroup = value; }
        }

        public StringCollection Nodes
        {
            get { return _nodes; }
        }

    }


    /// <summary>
    /// This is an abstract filter that will filter out any context nodes that match the regulare expressions in the 
    /// filters collection. This class needs to be inherited from.
    /// </summary>
    public abstract class GenericRegExFilter : ICBOSecurityFilter
    {
        protected IDomainMessageCollection _messages;
        protected List<Regex> _filters;
        protected SAHLPrincipal _currentPrincipal;
        protected InstanceNode _instanceNode;
        protected CBONode _currentNode;
        protected List<UserGroupNodes> _userGroupNodes;
        protected StringCollection _NodesNotToFilter;
        protected int _applicationKey;



        public GenericRegExFilter()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            _messages = spc.DomainMessages;
            _currentPrincipal = SAHLPrincipal.GetCurrent();
            
            //ICBOService CBOService = ServiceFactory.GetService<ICBOService>();
            CBOManager CBOManager = new CBOManager();
            CBONode thisNode = CBOManager.GetCurrentCBONode(_currentPrincipal) as CBONode;
            _currentNode = thisNode;

            while (thisNode as InstanceNode == null)
            {
                if (thisNode == null)
                    break;

                if (thisNode.ParentNode == null)
                    break;

                thisNode = thisNode.ParentNode;
            }

            _instanceNode = thisNode as InstanceNode;

			//If this is an instance node, we should determine whether the application key exists, if it doesn't
			//Determine what process this is and find the appropriate key
			//This is bad
			if (_instanceNode != null)
			{
				if (_instanceNode.ProcessName == SAHL.Common.Constants.WorkFlowProcessName.DebtCounselling)
				{
					_applicationKey = (int)_instanceNode.X2Data["DebtCounsellingKey"];
				}
				else
				{
					_applicationKey = (int)_instanceNode.X2Data["ApplicationKey"];
				}
			}

            _filters = new List<Regex>();
            _userGroupNodes = new List<UserGroupNodes>();

            _NodesNotToFilter = new StringCollection();
        }
       
        #region ICBOSecurityFilter Members

        public virtual bool ApplyToChildren
        {
            get { return true; }
        }

        public void FilterContextNodes(List<CBONode> ContextNodes)
        {

            if (!ShouldWeFilter())
                return;

            CBONode Node; // in ContextNodes

            for (int i = 0; i < ContextNodes.Count; i++)
            {
                Node = ContextNodes[i];
                if (FilterNode(Node))
                {
                    if ((Node.IsRemovable == true) && (Node.ChildNodes.Count == 0))
                    {
                        ContextNodes.Remove(Node);
                        i--;
                    }
                }
            }
            return;
        }

        // returns true if the node should be filtered.
        protected bool FilterNode(CBONode Node)
        {
            // check in UserGroupNodes if we should disregard this node (and children) when filtering.
            foreach (UserGroupNodes UGN in _userGroupNodes)
            {
                if (_currentPrincipal.IsInRole(UGN.UserGroup))
                {
                    if (UGN.Nodes.Contains(Node.Description))
                        return false;
                }
            }

            if (_NodesNotToFilter.Contains(Node.Description))
                return false;

            if ((Node.ChildNodes != null) && (Node.ChildNodes.Count > 0) && (this.ApplyToChildren == true))
            {
                for (int i = 0; i < Node.ChildNodes.Count; i++)
                {
                    if (FilterNode(Node.ChildNodes[i]))
                    {
                        Node.ChildNodes.Remove(Node.ChildNodes[i]);
                        i--;
                    }
                }
            }

            bool killIt = false;

            foreach (Regex filter in _filters)
            {
                if (filter.IsMatch(Node.Description))
                {
                    killIt = true;
                    break;
                }
            }

            return killIt;
        }

        /// <summary>
        /// this function always returns true, it needs to be overridden to change this.
        /// </summary>
        /// <returns></returns>
        virtual protected bool ShouldWeFilter()
        {
            return true;
        }

        #endregion
    }
}
