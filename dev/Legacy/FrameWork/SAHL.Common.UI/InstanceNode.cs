using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Security;
using SAHL.Common.Attributes;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Security.Principal;
using SAHL.Common.CacheData;
using SAHL.Common.X2.BusinessModel.DAO;

namespace SAHL.Common.UI
{
    public class InstanceNode : CBOWorkflowNode
    {
        private long _instanceID;
        private string _selectedNodeGenericKey;
        private IInstance _instance;
        private string _workflowName;
        private string _processName;
        private string _stateName;
        private string _workflowMenuKey;
        private string _workflowMenuWildKey;

        public long InstanceID
        {
            get
            {
                return _instanceID;
            }
        }

        public IInstance Instance
        {
            get { return _instance; }
        }

        public string SelectedNodeGenericKey
        {
            get { return _selectedNodeGenericKey; }
            set
            {
                _selectedNodeGenericKey = value;
            }
        }

        public string StateName
        {
            get { return _stateName; }
        }

        public string WorkflowName
        {
            get { return _workflowName; }
        }

        public string ProcessName
        {
            get { return _processName; }
        }

        public string WorkflowMenuKey
        {
            get { return _workflowMenuKey; }
        }

        public string WorkflowMenuWildKey
        {
            get { return _workflowMenuWildKey; }
        }

        public override int GenericKeyTypeKey
        {
            get
            {
                if (base._genericKeyTypeKey == -1)
                    base._genericKeyTypeKey = _instance.WorkFlow.GenericKeyTypeKey;

                return base.GenericKeyTypeKey;
            }
        }

        public new string URL
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public IEventList<IForm> Forms
        {
            get { return _instance.State.Forms; }
        }

        public IDictionary<string, object> X2Data
        {
            get 
            {
                IX2Service x2Service =  ServiceFactory.GetService<IX2Service>();
                return x2Service.GetX2DataRow(this._instanceID);
            }
        }

        public InstanceNode(int GenericKey, CBONode Parent, string Description, string LongDescription, long InstanceID, string Url)
            : base(GenericKey, Parent, Description, LongDescription)
        {
            _instanceID = InstanceID;
            _isRemovable = true;
            _url = Url;
            _menuIcon = "Application_Form.gif";

            Refresh();

            if (_parentNode != null)
                _nodePath = String.Format("{0}/InstanceID:{1}", _parentNode.NodePath, this._instanceID);
            else
                _nodePath = String.Format("InstanceID:{0}", this._instanceID);
        }

        /// <summary>
        /// Updates the node to match the current state of the Instance
        /// </summary>
        public void Refresh()
        {
            Instance_DAO dao = Instance_DAO.Find(_instanceID);
            IBusinessModelTypeMapper mapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            _instance = mapper.GetMappedType<IInstance>(dao);
            _stateName = _instance.State.Name;
            _workflowName = _instance.WorkFlow.Name;
            _processName = _instance.WorkFlow.Process.Name;
            _workflowMenuWildKey = String.Format("*_{0}_{1}", _workflowName, _processName);

            string menuKey = String.Format("{0}_{1}_{2}", _stateName, _workflowName, _processName);

            if (menuKey != _workflowMenuKey)
                _isDirty = true;

            _workflowMenuKey = menuKey;

            if (_instance.State.Forms.Count > 0)
                _url = _instance.State.Forms[0].Name;
        }


        public override void GetContextNodes(SAHLPrincipal principal, List<CBONode> contextNodes, IDictionary<int, object> allowedContextMenus)
        {
            IX2Service _x2Service = ServiceFactory.GetService<IX2Service>();
            IEventList<IActivity> list = _x2Service.GetUserActivitiesForInstance(principal, _instanceID);

            foreach (IActivity act in list)
            {
                CBOContextNode node = null;

                if (act.Form == null)
                {
                    node = new CBOContextNode(null, (int)_instanceID, null, act.Name, act.Name, null, true);
                }
                else
                {
                    node = new CBOContextNode(null, (int)_instanceID, null, act.Name, act.Name, act.Form.Name, true);
                }

                contextNodes.Add(node);
            }

            IEventList<IForm> forms = this.Forms; 

            if (forms.Count > 1)
            {
                CBOContextNode formsNode = new CBOContextNode(null, (int)_instanceID, null, "Forms", "", null, false);

                foreach (IForm form in forms)
                {
                    string description = form.Description;

                    if (description == null || description == "")
                        description = form.Name;

                    CBOContextNode node = new CBOContextNode(null, (int)_instanceID, formsNode, description, description, form.Name, false);
                    formsNode.ChildNodes.Add(node);
                }

                contextNodes.Add(formsNode);
            }
        }

        public override List<CBONode> ChildNodes
        {
            get
            {
                if (_childNodes == null)
                    _childNodes = new List<CBONode>();

                return _childNodes;

            }
        }

   
		private CBONode FindNodeByGenericKey(List<CBONode> nodeList, int GenericKey)
        {
            if (nodeList == null)
                return null;

            for (int i = 0; i < nodeList.Count; i++)
            {
				if (nodeList[i].GenericKey == GenericKey)
				{
					return nodeList[i];
				}
				else if (nodeList[i].ChildNodes != null && nodeList[i].ChildNodes.Count > 0)
				{
					CBONode childNode = FindNodeByGenericKey(nodeList[i].ChildNodes, GenericKey);

					if (childNode != null)
						return childNode;
				}
            }

            return null;
        }

        private CBONode FindNodeByUniqueIdentifier(List<CBONode> nodeList, string uniqueIdentifier, bool useDescription)
        {
            if (nodeList == null)
                return null;

            string[] parts = uniqueIdentifier.Split('_');

            int parentGenericKey = int.Parse(parts[0]);
            string parentDescription = parts[1];
            int genericKey = int.Parse(parts[2]);
            int genericKeyTypeKey = int.Parse(parts[3]);
            string description = parts[4];

            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].GenericKey == genericKey
                    && nodeList[i].GenericKeyTypeKey == genericKeyTypeKey
                    && nodeList[i].ParentNode.GenericKey == parentGenericKey
                    && nodeList[i].ParentNode.Description == parentDescription
                    && (useDescription==false || ( useDescription && nodeList[i].Description == description)))
                {
                    return nodeList[i];
                }
                else if (nodeList[i].ChildNodes != null && nodeList[i].ChildNodes.Count > 0)
                {
                    CBONode childNode = FindNodeByUniqueIdentifier(nodeList[i].ChildNodes, uniqueIdentifier, useDescription);

                    if (childNode != null)
                        return childNode;
                }
            }

            return null;
        }

        public static InstanceNode GetInstanceNodeParentFromCBOMenuNode(CBOMenuNode Node)
        {
            return GetInstanceNodeParentFromCBOMenuNodeInternal(Node);
        }

        private static InstanceNode GetInstanceNodeParentFromCBOMenuNodeInternal(CBONode Node)
        {
            CBONode N = Node.ParentNode;
            if (N == null)
                return null;
            if (Node.ParentNode is InstanceNode)
                return Node.ParentNode as InstanceNode;
            else
            {
                return GetInstanceNodeParentFromCBOMenuNodeInternal(Node.ParentNode);
            }
        }
    }
}
