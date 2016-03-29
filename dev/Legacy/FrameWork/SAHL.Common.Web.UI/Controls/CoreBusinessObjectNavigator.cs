///
/// Core Business Object Navigation Control
/// Developer: Marius Smit 
/// Date : 2006
/// Description : The CBO navigator control is a web control that displays a CBO navigation tree and related context menu
/// on a web page
/// 
/// Developer : Donald Massyn
/// Date : August 2006
/// Description : Handed over by Marius for further development
///

/// November 2006 - Integrated new X2 engine

using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.Datasets;
using SAHL.Common.Web.UI;
using SAHL.Common;
using System.Xml;
using System.IO;

[assembly: TagPrefix("SAHL.Common.Web.UI.Controls", "SAHL")]
namespace SAHL.Common.Web.UI.Controls
{

    [ToolboxBitmap(typeof(CoreBusinessObjectNavigator), "Resources.CoreBusinessObjectNavigator.bmp")]
    [ToolboxData("<{0}:CoreBusinessObjectNavigator runat=server></{0}:CoreBusinessObjectNavigator>")]
    public class CoreBusinessObjectNavigator : WebControl
    {
        public const string DisplayVersionNumber = "v 1.0.10";

        public delegate void CboNavigateEventHandler(object sender, CboNavigateEventArgs cbone);
        public event CboNavigateEventHandler CboNavigate;

        public delegate void CboRemoveEventHandler(object sender, CboNavigateEventArgs cboRe);
        public event CboRemoveEventHandler CboRemove;

        public delegate void CboItemSelectedHandler(object sender, CboNavigateEventArgs ea);
        public event CboItemSelectedHandler ItemSelected;

        private enum ChangeReason
        {
            NoChange,
            SelectedItemChanged,
            SelectedContextItemChanged,
            NavigateUrlChanged,
            ContextUrlChanged,
            PageViewChanged,
            ItemWasAdded,
            UserSelectsChanged
        }

        // NOTE: These constants are defined in the HLTree.js file as well. Update accordingly!!
        private const string MAIN_TREENAME      = "MainTree";
        private const string CBO_TREENAME       = "CboTree";
        private const string CTX_TREENAME       = "CtxTree";
        private const string DATA_TREENAME      = "CboExtraData";

        private const string CBO_TXT_SELITEM    = "CboSelectedItem";
        private const string CBO_TXT_SELCTXITEM = "CboSelectedCtxItem";
        private const string CBO_TXT_STATE      = "CboNodeState";
        private const string CBO_TXT_STATECTX   = "CboNodeCtxState";
        private const string CBO_TXT_SUBMIT     = "CboHiddenSubmit";
        private const string CBO_TXT_REMOVE     = "CboHiddenRemove";

        //private const string CBO_MINUS_URL      = "~/Images/Minus.png";
        //private const string CBO_PLUS_URL       = "~/Images/Plus.png";
        //private const string CBO_DOT_URL        = "~/Images/trans.gif";
        //private const string CBO_SPACER_URL     = "~/Images/Spacer.png";
        

        // these are use for outputting the XMl Document

        private const string DOCUMENT_ROOT = "root";
        private const string CBO_DATA_CTXNODE = "SelectedCtx";
        private const string CBO_DATA_CBONODE = "SelectedCbo";

        private CBO                 m_Cbo;
        private XmlDocument         m_CboDataDoc; // XML Document that holds the cbo menu, context menu and other data, all relevant to the CBO tree
        // For now the m_DataDoc is populated and used to create the treeview controls but ultimately should be used as the xml source for a XSLT 
        // transform to create the client side HTML


        private CCboMenuNode        m_SelectedCboMenu;
        private CContextMenuNode    m_SelectedCtxMenu;
        private string              m_MyControlId = "";
        private string              m_ContextUrl = "";
        private string              m_NavigateUrl = "";
        private string              m_CurrentNodeValue = "";
        private string              m_AddedToName;
        private string              m_NodeState;
        private string              m_NodeCtxState;
        private string              m_PageViewName = "";
        private string              m_AddedKey = "-1";
        private int                 m_AddedType = -1;
        private int                 m_SelectedUserSelects = -1;
        private string m_SelectedItemPath;
        private ChangeReason m_ChangedReason;
        private string  m_RemovedItem;
        private bool m_CanRender = true;


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string MyControlId
        {
            get { return m_MyControlId; }
            set { m_MyControlId = value; }
        }

        /// <summary>
        /// Get, set the Datasource for the CBO tree
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public CBO DataSource
        {
            get{return m_Cbo;}
            set { 
                m_Cbo = value;
                ClearProperties();
            }
        }

        /// <summary>
        /// Get, Set the Added Key?
        /// </summary>
        public string AddedKey
        {
            get { return m_AddedKey; }
            set { m_AddedKey = value; }
        }

        /// <summary>
        /// Get, set the added type
        /// </summary>
        public int AddedType
        {
            get { return m_AddedType; }
            set { m_AddedType = value; }
        }

        public int SelectedUserSelects
        {
            get { return m_SelectedUserSelects; }
            set
            {
                m_SelectedUserSelects = value;
                m_ChangedReason = ChangeReason.UserSelectsChanged;
            }
        }

        /// <summary>
        /// Get. set the navigation URL
        /// </summary>
        public string NavigateUrl
        {
            get { return m_NavigateUrl; }
            set
            {
                if (m_NavigateUrl != value)
                {
                    DataSource.CboProperties[0].NavigateUrl = value;
                    m_NavigateUrl = value;
                    m_ChangedReason = ChangeReason.NavigateUrlChanged;
                }
            }
        }

        /// <summary>
        /// Get, set the Context URL
        /// </summary>
        public string ContextUrl
        {
            get { return m_ContextUrl; }
            set
            {
                if (m_ContextUrl != value)
                {
                    DataSource.CboProperties[0].ContextUrl = value;
                    m_ContextUrl = value;
                    m_ChangedReason = ChangeReason.ContextUrlChanged;
                }
            }
        }

        /// <summary>
        /// Get, set the Current Node Value
        /// </summary>
        public string CurrentNodeValue
        {
            get { return m_CurrentNodeValue; }
            set { m_CurrentNodeValue = value; }
        }

        /// <summary>
        /// Get, set the Page View Name
        /// </summary>
        public string PageViewName
        {
            get { return m_PageViewName; }
            set { 
                m_PageViewName = value;
                m_ChangedReason = ChangeReason.PageViewChanged;
            }
        }

        /// <summary>
        /// Get, set the currently selected item
        /// </summary>
        public CBO.UserSelectsRow SelectedItem
        {
            get {
                if (m_SelectedCboMenu != null)
                    return m_SelectedCboMenu.UserSelectsRow;
                else
                    return null;
            }
            set
            {
                CCboMenuNode tmp = CCboMenuNode.Construct(value);
                if (!m_SelectedCboMenu.Equals(tmp,false))
                {
                    SetSelectedCbo(tmp);
                    m_ChangedReason = ChangeReason.SelectedItemChanged;

                    // set the new Item Path
                    m_SelectedItemPath = GetSelectedItemPathString();

                    if (m_SelectedItemPath.Length > 0)
                    {
                        CboNavigateEventArgs cbone = new CboNavigateEventArgs();
                        cbone.SelectedItemPath = m_SelectedItemPath;
                        ItemSelected(this, cbone);
                    }
                }
            }
        }

        /// <summary>
        /// Get the selected item's group parent. 
        /// </summary>
        public CBO.UserSelectsRow SelectedItemGroupParent
        {
            get
            {
                CBO.UserSelectsRow retval = null;

                if (m_SelectedCboMenu != null)
                {
                    int GroupNodeID = m_SelectedCboMenu.NodeId;
                    if (GroupNodeID >= 0)
                    {
                        // find a parent node (if any) that match the selected type

                        string xPath = "//" + CCboMenuNode.CBO_NODE_NAME;

                        string XPathQuery = string.Format("//{0}[@{1}='{2}']", CCboMenuNode.CBO_NODE_NAME, CCboMenuNode.c_UniqueID, m_SelectedCboMenu.TreeUniqueId);

                        XmlNode parentNode = null;
                        if (CboRootNode.SelectNodes(XPathQuery).Count > 0)
                        {
                            parentNode = CboRootNode.SelectSingleNode(XPathQuery);

                            while ((parentNode != null) && (parentNode.Name == CCboMenuNode.CBO_NODE_NAME))
                            {
                                if (parentNode.Attributes[CCboMenuNode.c_NodeId] != null)
                                    if (parentNode.Attributes[CCboMenuNode.c_NodeId].Value != m_SelectedCboMenu.NodeId.ToString())
                                        break;

                                parentNode = parentNode.ParentNode;
                            }
                        }

                        if ((parentNode != null) && (parentNode.Name == CCboMenuNode.CBO_NODE_NAME))
                        {
                            CCboMenuNode menu = CCboMenuNode.Construct(parentNode);
                            if (menu.ParentKey == -1)
                                return retval;

                            retval = menu.UserSelectsRow;
                        }
                    }
                }
                return retval;
            }

        }

        private void SetSelectedCbo(CCboMenuNode NewSelectedNode)
        {
            DataSource.CboProperties[0].SelectedItem = "";
            m_SelectedCboMenu = null;

            if (NewSelectedNode != null)
            {
                // check that the new node exists, and set it up with correct parent 

                if (m_CboDataDoc == null)
                    createDataDocument();

                string xPath = "//" + CCboMenuNode.CBO_NODE_NAME;

                string XPathQuery = string.Format("//{0}[@{1}='{2}']", CCboMenuNode.CBO_NODE_NAME, CCboMenuNode.c_UserSelectsKey, NewSelectedNode.UserSelectsKey);

                XmlNode parentNode = null;
                CCboMenuNode parentMenu = null;

                if (CboRootNode.SelectNodes(XPathQuery).Count > 0)
                {
                    parentNode = CboRootNode.SelectSingleNode(XPathQuery).ParentNode;

                    if ((parentNode != null) && (parentNode.Name == CCboMenuNode.CBO_NODE_NAME))
                        parentMenu = CCboMenuNode.Construct(parentNode);
                }

                if (parentMenu == null)
                    m_SelectedCboMenu = NewSelectedNode;
                else
                    m_SelectedCboMenu = CCboMenuNode.Construct(parentMenu, NewSelectedNode.UserSelectsRow);

                DataSource.CboProperties[0].SelectedItem = m_SelectedCboMenu.XmlNode.OuterXml;
                
            }
        }

        /// <summary>
        /// Get, set the Currently selected Context Item
        /// </summary>
        public CBO.ContextMenuRow SelectedCtxItem
        {
            get {
                if (m_SelectedCtxMenu != null)
                    if (m_SelectedCtxMenu.NodeType == CContextMenuNode.ContextMenuTypes.General)
                        return m_SelectedCtxMenu.ContextMenuRow;
                return null;
            }
            set
            {
                CGenericCtxMenu tmp = new CGenericCtxMenu(null, value);
                if (!tmp.Equals(m_SelectedCtxMenu))
                {
                    SetSelectedCtx(tmp);
                    m_ChangedReason = ChangeReason.SelectedContextItemChanged;
                }
            }
        }

        /// <summary>
        /// Get, set the Currently selected Wokflow Context Item
        /// </summary>
        public CBO.WorkflowContextMenuRow SelectedWflCtxItem
        {
            get
            {
                if (m_SelectedCtxMenu != null)
                    if (m_SelectedCtxMenu.NodeType == CContextMenuNode.ContextMenuTypes.Workflow)
                        return ((CWorkflowCtxMenu)m_SelectedCtxMenu).WorkflowContextMenuRow;
                
                return null;
            }
            set
            {
                CWorkflowCtxMenu tmp = new CWorkflowCtxMenu(null, value);

                if (!tmp.Equals(m_SelectedCtxMenu))
                {
                    SetSelectedCtx(tmp);
                    m_ChangedReason = ChangeReason.SelectedContextItemChanged;
                }
            }
        }

        private void SetSelectedCtx(CContextMenuNode NewSelectedNode)
        {
            if (NewSelectedNode == null)
            {
                DataSource.CboProperties[0].SelectedCtxItem = "";
                m_SelectedCtxMenu = null;
            }
            else
            {
                DataSource.CboProperties[0].SelectedCtxItem = NewSelectedNode.XmlNode.OuterXml;
                m_SelectedCtxMenu = NewSelectedNode;
            }
        }
        /// <summary>
        /// Get, set the Node State
        /// </summary>
        public string NodeState
        {
            get { if (m_NodeState == null) return ""; else return m_NodeState; }
            set
            {
                if (value == null)
                {
                    DataSource.CboProperties[0].NodeState = "";
                    m_NodeState = "";
                }
                else
                {
                    DataSource.CboProperties[0].NodeState = value;
                    m_NodeState = value;
                }
                    
            }
        }

        /// <summary>
        /// Get, set the Context node state
        /// </summary>
        public string NodeCtxState
        {
            get { if (m_NodeCtxState == null) return ""; else return m_NodeCtxState; }
            set
            {
                if (value == null)
                {
                    DataSource.CboProperties[0].NodeCtxState = "";
                    m_NodeCtxState = "";
                }
                else
                {
                    DataSource.CboProperties[0].NodeCtxState = value;
                    m_NodeCtxState = value;
                }
            }
        }

        /// <summary>
        /// Clears any saved properties in the CBO Navigator
        /// </summary>
        public void ClearProperties()
        {
            if (DataSource != null)
            {
                if (DataSource.CboProperties.Rows.Count == 0)
                {
                    CBO.CboPropertiesRow row = DataSource.CboProperties.NewCboPropertiesRow();
                    row.NavigateUrl = "";
                    row.ContextUrl = "";
                    row.CurrentNodeValue = "";
                    row.SelectedItem = "";
                    row.SelectedCtxItem = "";
                    row.NodeState = "";
                    row.NodeCtxState = "";
                    row.RemovedItem = "";
                    row.InformationDisplay = "";
                    DataSource.CboProperties.AddCboPropertiesRow(row);
                }
            }
        }

        public void RefreshWorkflowNodeAndNavigate(int p_UserSelectsKey, Metrics p_MI)
        {
            SAHL.Common.ServiceFacade.CBO_SFE SFE = new SAHL.Common.ServiceFacade.CBO_SFE();
            SFE.RefreshWorkFlowNode(m_Cbo, p_UserSelectsKey, p_MI);
        }
        
        public CBO.WorkFlowInstanceInfoRow GetRelatedWorkflowMenuRow(CBO.UserSelectsRow p_Row)
        {
            // peet - 17th Dec 2007 - to prevent exceptions
            if (p_Row == null)
                return null;

            if (p_Row.GenericTypeKey != (int)SAHL.Common.CBONodeType.WorkFlowType)
                throw new Exception("Only valid for Workflow node at present");

            //CBO.WorkFlowInstanceInfoRow r = null;
            foreach (CBO.WorkFlowInstanceInfoRow row in DataSource.WorkFlowInstanceInfo)
                if (p_Row.GenericKey == row.InstanceID.ToString() )
                    return row;
            return null;

        }


        /// <summary>
        /// gets a string representing the path to the selected item, i.e. the route of the nodes tot he current selected
        /// node including the node type, UserSelects key and Generickey
        /// </summary>
        /// <returns></returns>
        private string GetSelectedItemPathString()
        {
            string retval = "";

            string xPath = "//" + CCboMenuNode.CBO_NODE_NAME ;

            string XPathQuery = string.Format("//{0}[@{1}='{2}']", CCboMenuNode.CBO_NODE_NAME, CCboMenuNode.c_UniqueID, m_SelectedCboMenu.TreeUniqueId);

            XmlNode parentNode = null;
            if (CboRootNode.SelectNodes(XPathQuery).Count > 0)
            {
                parentNode = CboRootNode.SelectSingleNode(XPathQuery);

                while ((parentNode != null) && (parentNode.Name == CCboMenuNode.CBO_NODE_NAME))
                {
                    if (retval.Length == 0)
                        retval = NodePathString(parentNode);
                    else
                        retval = NodePathString(parentNode) + ";" + retval;
                    parentNode = parentNode.ParentNode;
                }
            }
            return retval;

        }

        private string NodePathString(XmlNode node)
        {
            string retval = "";

            if (node.Attributes[CCboMenuNode.c_NodeType] != null)
                retval = node.Attributes[CCboMenuNode.c_NodeType].Value.Trim() + ",";
            else
                retval = ",";

            if (node.Attributes[CCboMenuNode.c_UserSelectsKey] != null)
                retval += node.Attributes[CCboMenuNode.c_UserSelectsKey].Value.Trim() + ",";
            else
                retval += ",";

            if (node.Attributes[CCboMenuNode.c_GenericKey] != null)
                retval += node.Attributes[CCboMenuNode.c_GenericKey].Value.Trim() + ",";
            else
                retval += ",";

            if (node.Attributes[CCboMenuNode.c_GenericTypeKey] != null)
                retval += node.Attributes[CCboMenuNode.c_GenericTypeKey].Value.Trim();

            return retval;

        }

        /// <summary>
        /// Get the selected items parent of a given type
        /// </summary>
        /// <param name="nType"></param>
        /// <returns></returns>
        public CBO.UserSelectsRow GetSelectedItemsParentByType(CBONodeType nType)
        {
            CBO.UserSelectsRow retval = null;

            if (m_SelectedCboMenu == null)
                return retval;

            int type = (int)nType;

            if (m_CboDataDoc == null)
                createDataDocument();

            if (m_SelectedCboMenu.ParentKey == -1)
                return retval;


            // find a parent node (if any) that match the selected type

            string xPath = "//" + CCboMenuNode.CBO_NODE_NAME ;

            string XPathQuery = string.Format("//{0}[@{1}='{2}']", CCboMenuNode.CBO_NODE_NAME, CCboMenuNode.c_UniqueID, m_SelectedCboMenu.TreeUniqueId);

            XmlNode parentNode = null;
            if (CboRootNode.SelectNodes(XPathQuery).Count > 0)
            {
                parentNode = CboRootNode.SelectSingleNode(XPathQuery);

                while ((parentNode != null) && (parentNode.Name == CCboMenuNode.CBO_NODE_NAME))
                {
                    if (parentNode.Attributes[CCboMenuNode.c_GenericTypeKey] != null)
                        if (parentNode.Attributes[CCboMenuNode.c_GenericTypeKey].Value == type.ToString())
                            break;

                    parentNode = parentNode.ParentNode;
                }
            }

            if ((parentNode != null) && (parentNode.Name == CCboMenuNode.CBO_NODE_NAME))
            {
                CCboMenuNode menu = CCboMenuNode.Construct(parentNode);
                if (menu.ParentKey == -1)
                    return retval;

                retval = menu.UserSelectsRow;
            }

            return retval;
        }

        public XmlDocument MenuTree
        {
            get { return m_CboDataDoc; }
        }

        /// <summary>
        /// Shortcut method to quickly get the CboRoot Node 
        /// </summary>
        public XmlNode CboRootNode
        {
            get
            {
                XmlNode retval = null;

                if (m_CboDataDoc != null)
                {
                    if (m_CboDataDoc.SelectNodes(DOCUMENT_ROOT + "/" + CBO_TREENAME).Count == 1)
                        retval = m_CboDataDoc.SelectSingleNode(DOCUMENT_ROOT + "/" + CBO_TREENAME);
                }
                return retval;
            }
        }

        /// <summary>
        /// Shortcut method to quickly get the CtxRoot Node 
        /// </summary>
        public XmlNode CtxRootNode
        {
            get
            {
                XmlNode retval = null;

                if (m_CboDataDoc != null)
                {
                    if (m_CboDataDoc.SelectNodes(DOCUMENT_ROOT + "/" + CTX_TREENAME).Count == 1)
                        retval = m_CboDataDoc.SelectSingleNode(DOCUMENT_ROOT + "/" + CTX_TREENAME);
                }
                return retval;
            }
        }

        /// <summary>
        /// Shortcut method to quickly get the CboData Node 
        /// </summary>
        private XmlNode CboDataNode
        {
            get
            {
                XmlNode retval = null;

                if (m_CboDataDoc != null)
                {
                    if (m_CboDataDoc.SelectNodes(DOCUMENT_ROOT + "/" + DATA_TREENAME).Count == 1)
                        retval = m_CboDataDoc.SelectSingleNode(DOCUMENT_ROOT + "/" + DATA_TREENAME);
                }
                return retval;
            }
        }


        /// <summary>
        /// Removes the selected CBO item and all its children.
        /// </summary>
        /// <returns>The URL of the new selected item.</returns>
        private string RemoveSelectedItem()
        {
            
            // we don't need to actually remove the item as this will be done by our caller
            // all we need to do is set the SelectedCbo to the removed items parent

            string retval = "";

            if (m_CboDataDoc == null)
                createDataDocument();

            if (m_SelectedCboMenu != null)
            {

                string XPathQuery = string.Format("//{0}[@{1}='{2}']", CCboMenuNode.CBO_NODE_NAME, CGenericCboNode.c_UserSelectsKey, m_SelectedCboMenu.UserSelectsKey);

                XmlNode xRemove = null;
                XmlNode removeParent = null;

                if (CboRootNode.SelectNodes(XPathQuery).Count == 1)
                    xRemove = CboRootNode.SelectSingleNode(XPathQuery);

                if (xRemove != null)
                {
                    // set to the parent node
                    if (xRemove.ParentNode != null)
                    {
                        removeParent = xRemove.ParentNode;
                        CCboMenuNode newSelected = CCboMenuNode.Construct(removeParent);
                        SetSelectedCbo(newSelected);
                        retval = SelectFirstCtx();
                        if (retval.Length == 0)
                            retval = m_SelectedCboMenu.Url;
                    }
                    else
                        retval = SelectFirstCbo();
                }
                else
                    retval = SelectFirstCbo();
            }
            return retval;
        }

        /// <summary>
        /// Recursively delete child rows from the UserSelects table for the given parent Node
        /// </summary>
        /// <param name="?"></param>
        private void DeleteChildrenOf(int ParentKey)
        {
            CBO.UserSelectsRow[] usr = (CBO.UserSelectsRow[])DataSource.UserSelects.Select("ParentKey=" + ParentKey.ToString(), "");
            foreach (CBO.UserSelectsRow r in usr)
            {
                DeleteChildrenOf(r.UserSelectsKey);
                r.Delete();
            }
        }

        /// <summary>
        /// Clear By Memory Tag
        /// </summary>
        private void ClearByMemoryTag()
        {
            List<string> keysToClear = new List<string>();

            for (int i = Page.Session.Keys.Count - 1; i >= 0; --i )
                if (Page.Session.Keys[i].StartsWith(SAHL.Common.Constants.CboMemoryTag))
                    Page.Session.Remove(Page.Session.Keys[i]);

        }

        public CoreBusinessObjectNavigator()
        {
        }

        public override void Dispose()
        {
            if (m_Cbo != null)
                m_Cbo.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Everything pretty much happens in here, prints out everything that needs to be printed out to the client
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            /*
             There are a few ways of getting to this point and all need to be considered when processing this method
             * 
             They are:
             - On initial creation. The Request will be empty and the DataSource.CboProperties will be empty
                  We need to find the first CBO and then find the first context menu and then call CboNavigate navigate to there
            
             - Browser Post 
              - By the user clicking an item in the CBO or context menu. 
                  In this case m_PostType will be set to "CBO" or "CTX" depending on which menu was clicked
                  so we need to navigate to the selected CBO or Context menu item, by reading either the m_PostSelected
                  or the m_PostCtxSelected Requests to see where the client wanted to navigate to
                  If we're navigating to a new CBO the the context menu gets reloaded so navigate to the first 
                  item in the context menu, else to the CBO url
 
              - By a the page being browsed calling a navigation event
                  The client can reload the page as many times as they want for whatever reason they want to
                  In this case the m_PostType will be empty, we just save the posted values and into DataSource.CboProperties
                  table and then wait for us to get called again. The client can 

             - Server Post. By calling CboNavigate from here or from a client this event will fire again
                  Values will have been saved in the DataSource.CboProperties table so we need to read them
                  The client may have changed the NavigateUrl or ContextUrl properties on the object , if so 
                  then we need to find the cbo or context that matches the new value and do a navigate to there

            */


            // cannot do anything without a Cbo DataSet
            if (DataSource == null) return;

            string[] names = this.UniqueID.Split('$');
            m_AddedToName = names[0];

            // Have to do a replace of these chars cause somehow $ chars magically appear in the generated jscript!
            string s = this.UniqueID;
            s = s.Replace('_', 'x');
            s = s.Replace('$', 'x');
            MyControlId = s;

            // The control is either called from a client browser in which case all the Page.Request.Form Values are set
            // or else it being created on a page on the server in which case the Page.request values are all empty

            // If the page.request value are empty then use the values saved in the DataSource.CboProperties table to populate the control

            // the first time round, the requets is empyt and the DataSource.CboProperties is empty



            // get the Request Values
            string m_PostType = Page.Request.Form[m_AddedToName + "$" + CBO_TXT_SUBMIT]; ;
            string postState = Page.Request.Form[m_AddedToName + "$" + CBO_TXT_STATE];
            string postCtxState = Page.Request.Form[m_AddedToName + "$" + CBO_TXT_STATECTX];
            string m_PostSelected = Page.Request.Form[m_AddedToName + "$" + CBO_TXT_SELITEM];
            string m_PostCtxSelected = Page.Request.Form[m_AddedToName + "$" + CBO_TXT_SELCTXITEM];

            CCboMenuNode cboMenu = null;
            CContextMenuNode ctxMenu = null;

            if (m_PostType == null) m_PostType = "";
            if (postState == null) postState = "";
            if (postCtxState == null) postCtxState = "";
            if (m_PostSelected == null) m_PostSelected = "";
            if (m_PostCtxSelected == null) m_PostCtxSelected = "";


            if (Page.IsPostBack)
            {
                DataSource.CboProperties[0].InformationDisplay = "";
                MarkTime("A");
                m_ChangedReason = ChangeReason.NoChange;
                DataSource.CboProperties[0].RemovedItem = "";
                //m_PostType will be null if the page is submiting and not the control

                // We have not set up the CboProperite yet so use the Request.form Values
                NodeState = postState;
                NodeCtxState = postCtxState;

                if (!m_PostSelected.Equals(""))
                {
                    m_PostSelected = CCboMenuNode.reXml(m_PostSelected);
                    cboMenu = CCboMenuNode.Construct(m_PostSelected);
                    NavigateUrl = cboMenu.Url;
                }

                // selecting a cbo should clear the ctx
                if (m_PostType == "CBO")
                {
                    SetSelectedCtx(null);
                    ContextUrl = "";
                }

                // selecting a new context or a client is calling us
                if ((m_PostType == "CTX") || (m_PostType == ""))
                {
                    //Get the Selected Context Item
                    if (!m_PostCtxSelected.Equals(""))
                    {
                        m_PostCtxSelected = CCboMenuNode.reXml(m_PostCtxSelected);
                        ctxMenu = CContextMenuNode.Construct(m_PostCtxSelected);
                        if (ctxMenu != null) ContextUrl = ctxMenu.Url;
                    }
                }

                // client is calling us so we must not have a Navigate or Context URL
                if (m_PostType == "")
                {
                    NavigateUrl = "";
                    ContextUrl = "";
                }
            }
            else
            {
                // can get here from a) first time startup or b) roundtrip postback from iis
                MarkTime("B");
                
                NodeState = DataSource.CboProperties[0].NodeState;
                NodeCtxState = DataSource.CboProperties[0].NodeCtxState;

                // Get the selected CBO
                if (DataSource.CboProperties[0].SelectedItem.Length > 0)
                    cboMenu = CCboMenuNode.Construct(DataSource.CboProperties[0].SelectedItem);

                if (DataSource.CboProperties[0].NavigateUrl.Length > 0)
                    NavigateUrl = DataSource.CboProperties[0].NavigateUrl;

                // Get the Selected Ctx Menu
                if (DataSource.CboProperties[0].SelectedCtxItem.Length > 0)
                {
                    ctxMenu = CContextMenuNode.Construct(DataSource.CboProperties[0].SelectedCtxItem);
                    if (DataSource.CboProperties[0].ContextUrl.Length > 0)
                        ContextUrl = DataSource.CboProperties[0].ContextUrl;
                }
                else
                {
                    ctxMenu = null;
                    ContextUrl = "";
                }
            }

            if (cboMenu == null) // No Cbo Selected, Select the first one
            {
                NavigateUrl = SelectFirstCbo();
                m_PostType = "CBO";
            }
            else // cbo 
            {
                SetSelectedCbo(cboMenu);
                if (ctxMenu == null)
                {
                    ContextUrl = SelectFirstCtx();
                    if ((ContextUrl.Length > 0) && (m_PostType != "REMOVE"))
                        m_PostType = "CTX";
                }
                else
                {
                    // check that the page view name has not changed the context item

                    if ((PageViewName != ctxMenu.Url) && (!Page.IsPostBack))
                    {
                        ctxMenu = SelectPageView(ctxMenu);
                        ContextUrl = PageViewName;
                    }

                    SetSelectedCtx(ctxMenu);
                }
            }

            if (ContextUrl == "REMOVE_CBO")
                m_PostType = "REMOVE";
    

            // Can now fill out the XmlDocument with you Cbo Menu, Context Menu and the selected cbo and context items

            // check what sort of post this is
            if (m_PostType != null)
            {

                // Generate the data document as we will be using it
                createDataDocument();
                switch (m_PostType)
                {
                    case "REMOVE": // Remove CBO was clicked
                        if (m_SelectedCboMenu != null)
                        {
                            m_RemovedItem = m_SelectedCboMenu.XmlNode.OuterXml;
                            DataSource.CboProperties[0].RemovedItem = m_RemovedItem;
                        }
                        
                        NavigateUrl = RemoveSelectedItem();
                        ContextUrl = "";
                        if (m_SelectedCboMenu != null)
                        {
                            NavigateUrl = m_SelectedCboMenu.Url;
                            if (m_SelectedCtxMenu != null)
                                ContextUrl = m_SelectedCtxMenu.Url;
                        }

                        // fall through since the item has been removed and the parent cbo selected
                        goto case "CBO";

                    case "CBO": // CBO was clicked
                        if (NavigateUrl.Length > 0)
                        {
                            CboNavigateEventArgs cbone = new CboNavigateEventArgs(NavigateUrl);
                            CboNavigate(this, cbone);
                        }
                        m_ChangedReason = ChangeReason.NoChange;
                        break;
                    case "CTX": // Remove CTX was cliecked
                        if (ContextUrl.Length > 0)
                        {
                            CboNavigateEventArgs cbone = new CboNavigateEventArgs(ContextUrl);
                            CboNavigate(this, cbone);
                        }
                        m_ChangedReason = ChangeReason.NoChange;
                        break;

                }
                if (m_PostType.Length > 0)
                    ClearByMemoryTag();
            }

            // If we get here and there is no selected item, then the user has no access
            // Also navigate to this item's page.
            if (m_SelectedCboMenu == null) 
            {
                // The user has no security access cause no navigation options were returned.
                // Navigate him/her to the relevant page that will inform him/her of this.
                if (!PageViewName.Equals("CESecurityNoAccess") && !PageViewName.Equals("Error"))
                {
                    CboNavigateEventArgs cbone = new CboNavigateEventArgs("CESecurityNoAccess");
                    CboNavigate(this, cbone);
                }
            }
            else
                if (ItemSelected != null)
                {
                    m_SelectedItemPath = GetSelectedItemPathString();

                    if (m_SelectedItemPath.Length > 0)
                    {
                        CboNavigateEventArgs cbone = new CboNavigateEventArgs();
                        cbone.SelectedItemPath = m_SelectedItemPath;
                        ItemSelected(this, cbone);
                    }
                }

            // If the selected context item has the same url as the selected cbo item
            // then it creates abit of confustion with the ShouldRunPage call
            // so we set the NavigateUrl to blank if thi is the case

            if ((m_SelectedCboMenu != null) && (m_SelectedCtxMenu != null))
            {
                if (m_SelectedCboMenu.Url == m_SelectedCtxMenu.Url)
                    NavigateUrl = "";
            }

            // if anythign has changed it's because we changed it so reset all the flags
            m_ChangedReason = ChangeReason.NoChange;

        }

        /// <summary>
        /// Set the SelectedItem and Selected Context ot the first item in the DataSet
        /// returning the new url
        /// </summary>
        private string SelectFirstCbo()
        {
            string retval = "";

            DataRow[] dr = DataSource.UserSelects.Select("ParentKey is null", "");

            if (dr.Length > 0)
            {
                CBO.UserSelectsRow cboRow = (CBO.UserSelectsRow)dr[0];
                CGenericCboNode selCbo = new CGenericCboNode(null, cboRow);
                SetSelectedCbo(selCbo);

                retval = SelectFirstCtx();
                if (selCbo.Url.Length >0)
                    retval = selCbo.Url;
            }
            return retval;
        }

        /// <summary>
        /// Set the selected contetx menu to the first one matching the currently selected cbo
        /// returning the new url
        /// </summary>
        private string SelectFirstCtx()
        {
            string retval = "";

            if (m_SelectedCboMenu != null)
            {

                // if the selected CBO Url is empty, select the first Ctx menu
                // if the Url is not empty, select the first context menu that matches 

                DataRow[] dr;

                // only select the first context menu, if the CBO doesn't have a navigate Url
                if (m_SelectedCboMenu.Url.Equals(""))
                    dr = DataSource.ContextMenu.Select("CoreBusinessObjectKey=" + m_SelectedCboMenu.CoreBusinessObjectKey.ToString(), "Sequence");
                else
                    dr = DataSource.ContextMenu.Select("CoreBusinessObjectKey=" + m_SelectedCboMenu.CoreBusinessObjectKey.ToString() + " AND Url='" + m_SelectedCboMenu.Url.Trim() + "'", "Sequence");

                if (dr.Length > 0)
                {
                    CBO.ContextMenuRow ctxRow = (CBO.ContextMenuRow)dr[0];
                    CGenericCtxMenu selCtx = new CGenericCtxMenu(null, ctxRow);
                    SetSelectedCtx(selCtx);
                    retval = selCtx.Url;
                }
                else 
                {
                    if (m_SelectedCboMenu.Url.Equals(""))
                        dr = DataSource.WorkflowContextMenu.Select("UserSelectsKey=" + m_SelectedCboMenu.UserSelectsKey.ToString(), "");    
                    else
                        dr = DataSource.WorkflowContextMenu.Select("UserSelectsKey=" + m_SelectedCboMenu.UserSelectsKey.ToString() + " AND Url='" + m_SelectedCboMenu.Url.Trim() + "'", "");

                    if (dr.Length > 0)
                    {
                        CBO.WorkflowContextMenuRow ctxRow = (CBO.WorkflowContextMenuRow)dr[0];
                        CWorkflowCtxMenu selCtx = new CWorkflowCtxMenu(null, ctxRow);
                        SetSelectedCtx(selCtx);
                        retval = selCtx.Url;
                    }
                }

            }
            return retval;
        }

        /// <summary>
        /// If the PageView Name is different to the currently selected Context Menu
        /// Then we have to search first the ContextMenu table, then the WorkflowMenu table
        /// for an item that matches the PageViewName in order to select it
        /// </summary>
        /// <param name="ExistingItem"></param>
        /// <returns></returns>
        private CContextMenuNode SelectPageView(CContextMenuNode ExistingItem)
        {
            CContextMenuNode retval = ExistingItem;

            if (DataSource != null)
            {
                DataRow[] dr;
                string ContextQuery = "";
                string WorkflowQuery = "";

                if (m_SelectedCboMenu == null)
                {
                    ContextQuery = "Url = '" + PageViewName.Trim() + "'";
                    WorkflowQuery = "Url = '" + PageViewName.Trim() + "'";
                }
                else
                {
                    ContextQuery = string.Format("CoreBusinessObjectKey={0} AND Url='{1}'", m_SelectedCboMenu.CoreBusinessObjectKey, PageViewName);
                    WorkflowQuery = string.Format("UserSelectsKey={0} AND Url='{1}'", m_SelectedCboMenu.UserSelectsKey, PageViewName);
                }

                dr = DataSource.ContextMenu.Select(ContextQuery, "Sequence");
                if (dr.Length > 0)
                {
                    retval = new CGenericCtxMenu(null, (CBO.ContextMenuRow)dr[0]);
                }
                else
                {
                    dr = DataSource.WorkflowContextMenu.Select(WorkflowQuery, "");
                    if (dr.Length > 0)
                        retval = new CWorkflowCtxMenu(null, (CBO.WorkflowContextMenuRow)dr[0]);
                }
            }
            return retval;
        }
        /// <summary>
        /// Because of the navigation event fired by this control, the postback values are lost.
        /// Just set the properties again based on the saved posted values that are in CboProperties.
        /// </summary>
        private void checkSelectedProperties()
        {
            if (m_SelectedCboMenu == null)
            {
                if (DataSource.CboProperties[0].SelectedItem.Length > 0)
                {
                    CCboMenuNode selectedMenu =  CCboMenuNode.Construct(DataSource.CboProperties[0].SelectedItem);
                    SetSelectedCbo(selectedMenu);
                }
            }

            if (m_SelectedCtxMenu == null)
            {
                if (DataSource.CboProperties[0].SelectedCtxItem.Length > 0)
                {
                    CContextMenuNode selectedCtxMenu = CContextMenuNode.Construct(DataSource.CboProperties[0].SelectedCtxItem);
                    SetSelectedCtx(selectedCtxMenu);
                }
            }

            if (NodeState == null)
                if (DataSource.CboProperties[0].NodeState.Length > 0)
                    NodeState = DataSource.CboProperties[0].NodeState;

            if (NodeCtxState == null)
                if (DataSource.CboProperties[0].NodeCtxState.Length > 0)
                    NodeCtxState = DataSource.CboProperties[0].NodeCtxState;

            if (NavigateUrl == null)
                if (DataSource.CboProperties[0].NavigateUrl.Length > 0)
                    NavigateUrl = DataSource.CboProperties[0].NavigateUrl;

            if (ContextUrl == null)
                if (DataSource.CboProperties[0].ContextUrl.Length > 0)
                    ContextUrl = DataSource.CboProperties[0].NavigateUrl;

            m_RemovedItem = DataSource.CboProperties[0].RemovedItem;

        }

        /// <summary>
        /// Make sure the selected CBO and Ctx exist in our current CBO tree, if not then select the first ones 
        /// return true is the anything has changed
        /// </summary>
        private bool reCheckSelectedProperties()
        {
            bool retval = false;
            if (m_SelectedCboMenu != null)
            {
                // find the node that matches out selected cbo
                string XPathQuery = string.Format("//{0}[@{1}='{2}']", CCboMenuNode.CBO_NODE_NAME, CGenericCboNode.c_Url, m_SelectedCboMenu.Url);
                if (CboRootNode.SelectNodes(XPathQuery).Count == 0)
                {
                    NavigateUrl = SelectFirstCbo();
                    retval = true;
                    if (NavigateUrl.Length == 0)
                        NavigateUrl = SelectFirstCtx();
                }
                else // the cbo menu item still exists, but the context menu might have changed to check it too
                {
                    if (m_SelectedCtxMenu != null)
                    {
                        XPathQuery = string.Format("//{0}[@{1}='{2}']", CContextMenuNode.CTX_NODE_NAME, CContextMenuNode.c_Url, m_SelectedCtxMenu.Url);
                        if (CtxRootNode.SelectNodes(XPathQuery).Count == 0)
                        {
                            ContextUrl = SelectFirstCtx();
                            if (ContextUrl.Length > 0)
                            {
                                retval = true;
                            }
                        }
                    }
                }
            }
            return retval;
        }

        /// <summary>
        /// AddedKey has been changed, so the rule says always select the added item
        /// </summary>
        private bool SelectAddedKey()
        {
            bool retval = false;

            string XPathQuery = string.Format("//{0}[@{1}='{2}' and @{3}='{4}' ]", CCboMenuNode.CBO_NODE_NAME, CGenericCboNode.c_GenericKey, AddedKey, CGenericCboNode.c_GenericTypeKey, AddedType);

            if (CboRootNode.SelectNodes(XPathQuery).Count > 0)
            {
                XmlNode newNode = CboRootNode.SelectSingleNode(XPathQuery);

                if (newNode.Attributes[CCboMenuNode.c_SelectedItem] != null)
                    newNode.Attributes[CCboMenuNode.c_SelectedItem].Value = "1";

                SetSelectedCbo(CCboMenuNode.Construct(newNode));
                if (m_SelectedCboMenu != null)
                    NavigateUrl = m_SelectedCboMenu.Url;
                else
                    NavigateUrl = "";

                SetSelectedCtx(null);
                ContextUrl = "";

                retval = true;
            }

            return retval;

        }

        /// <summary>
        /// Our caller has has the opportunity to change our properties at this stage
        /// So we need to check if the changes are going to result in a re-navigation
        /// if so, do the appropriate navigate and set a flag so that we don't do the rendering on the render event
        /// if we're good to go then set the flag so that we render correctly on the Render event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            bool bWasChanged = false;
            string szNewUrl = "";

            m_CanRender = true;

            // cannot do anything if the datasource is null
            if (DataSource == null)
            {
                m_CanRender = false;
                return;
            }

            if (Page.IsPostBack)
                MarkTime("C");
            else
                MarkTime("D");

            checkSelectedProperties();

            // see if we need to fire the CboRemove event
            if ((m_RemovedItem.Length > 0) && (CboRemove != null))
            {
                CCboMenuNode remCbo = CCboMenuNode.Construct(m_RemovedItem);
                CboNavigateEventArgs cboRe = new CboNavigateEventArgs(remCbo.GenericKey, remCbo.UserSelectsKey);
                CboRemove(this, cboRe);
                m_RemovedItem = "";
                DataSource.CboProperties[0].RemovedItem = "";
                bWasChanged = true;
                if (NavigateUrl.Length > 0)
                    szNewUrl = NavigateUrl;
                else
                    szNewUrl = ContextUrl;

            }
            
            createDataDocument();

            // A number of things could have changed between the OnInit and this point
            // SelectedItem, SelectedCtxItem , NavigateUrl or ContextUrl could have changed
            // If so then change the selection and navigate to the new Page

            // Second, the AddedKey could have been set if so then find the new item and select it


            // Finally, the selected item could have been removed, select the top levelitem

            // The decisions need to be made in that order, by order



            // the SelectItem , SelctedCtxItem, NavigateUrl, or ContextUrl coul dhave been changed by out client
            // if they have then we need to change to navigate to the new selected item, context item, or urls'
            switch (m_ChangedReason)
            {
                case ChangeReason.SelectedContextItemChanged:
                    if (m_SelectedCtxMenu.Url.Length > 0)
                    {
                        szNewUrl = m_SelectedCtxMenu.Url;
                        bWasChanged = true;
                    }
                    break;
                case ChangeReason.ContextUrlChanged:
                    if (ContextUrl.Length > 0)
                    {
                        if (ChangeSelectedCtxToUrl(ContextUrl))
                        {
                            szNewUrl = ContextUrl;
                            bWasChanged = true;
                        }
                    }
                    break;
                case ChangeReason.NavigateUrlChanged:
                    if (NavigateUrl.Length > 0)
                    {
                        if (ChangeSelectedCboToUrl(NavigateUrl))
                        {
                            szNewUrl = NavigateUrl;
                            bWasChanged = true;
                        }
                    }
                    break;
                case ChangeReason.PageViewChanged:
                    if (PageViewName.Length > 0)
                    {
                        if (ChangeSelectedCboToUrl(PageViewName))
                        {
                            szNewUrl = PageViewName;
                            bWasChanged = true;
                        }
                    }
                    break;
                case ChangeReason.SelectedItemChanged:
                    if (m_SelectedCboMenu.Url.Length > 0)
                    {
                        szNewUrl = m_SelectedCboMenu.Url;
                        bWasChanged = true;
                    }
                    break;
            }

            // if the Navigation has not been changed by a change in the NavigateUrl
            // then, if a new item has been added, select that item
            if (
                (!bWasChanged) &&
                (AddedKey != "-1")
                )
            {
                bWasChanged = SelectAddedKey();
                szNewUrl = NavigateUrl;
            }

            // this checkss to 
            if (!bWasChanged)
            {
                m_ChangedReason = ChangeReason.NoChange;

                if (reCheckSelectedProperties()) // the selected items have been changed
                {
                    switch (m_ChangedReason)
                    {
                        case ChangeReason.SelectedContextItemChanged:
                            if (m_SelectedCtxMenu.Url.Length > 0)
                            {
                                szNewUrl = m_SelectedCtxMenu.Url;
                                bWasChanged = true;
                            }
                            break;
                        case ChangeReason.ContextUrlChanged:
                            if (ContextUrl.Length > 0)
                            {
                                if (ChangeSelectedCtxToUrl(ContextUrl))
                                {
                                    szNewUrl = ContextUrl;
                                    bWasChanged = true;
                                }
                            }
                            break;
                        case ChangeReason.NavigateUrlChanged:
                            if (NavigateUrl.Length > 0)
                            {
                                if (ChangeSelectedCboToUrl(NavigateUrl))
                                {
                                    szNewUrl = NavigateUrl;
                                    bWasChanged = true;
                                }
                            }
                            break;
                        case ChangeReason.PageViewChanged:
                            if (PageViewName.Length > 0)
                            {
                                if (ChangeSelectedCboToUrl(PageViewName))
                                {
                                    szNewUrl = PageViewName;
                                    bWasChanged = true;
                                }
                            }
                            break;
                        case ChangeReason.SelectedItemChanged:
                            if (m_SelectedCboMenu.Url.Length > 0)
                            {
                                szNewUrl = m_SelectedCboMenu.Url;
                                bWasChanged = true;
                            }
                            break;
                    }
                }
            }

            // if the we have changed the selected 
            // item because the NavigateUrl is different to the selected item
            // then Navigate to the new Page over here
            if (bWasChanged)
            {
                // There is a new selected item.

                if (ItemSelected != null)
                {
                    m_SelectedItemPath = GetSelectedItemPathString();

                    if (m_SelectedItemPath.Length > 0)
                    {
                        CboNavigateEventArgs cbone = new CboNavigateEventArgs();
                        cbone.SelectedItemPath = m_SelectedItemPath;
                        ItemSelected(this, cbone);
                    }
                }

                if (szNewUrl.Length > 0)
                {
                    MarkTime("N+" + szNewUrl);
                    CboNavigateEventArgs cbone = new CboNavigateEventArgs(szNewUrl);
                    CboNavigate(this, cbone);
                    m_CanRender = false;
                    return;
                }
            }
        }


        /// <summary>
        /// Check the m_CanRender flag so see iwe we need to render or we're going to renavigate
        /// Render our page contents
        /// </summary>
        /// <param name="writer"></param>
        private void RenderCbo(HtmlTextWriter writer)
        {

            if (m_CanRender)
            {
                writer.AddAttribute("class", "CboBox");

                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderSizingButtons(writer);
                RenderCboMenu(writer);
                RenderContextMenu(writer);
                RenderTimeoutRow(writer);
                RenderUserNameStatusRow(writer);
                RenderHiddenFormControls(writer);

                writer.RenderEndTag(); //HtmlTextWriterTag.Div
            }
        }

        private void RenderSizingButtons(HtmlTextWriter writer)
        {
            //todo
        }

        private string ImagesPath = "";

        private void RenderCboMenu(HtmlTextWriter writer)
        {

            writer.AddAttribute("class", "CboHeaderText");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write("User Menu");
            writer.RenderEndTag(); //(HtmlTextWriterTag.div);

            writer.AddAttribute("id", "cbolist");
            writer.AddAttribute("class", "CboMenu");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            ImagesPath = Page.ResolveClientUrl("~/Images/");

            if (CboRootNode != null)
            {
                writer.AddAttribute("id", "cbotree");
                writer.AddAttribute("class", "open");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                for(int i = 0; i < CboRootNode.ChildNodes.Count; ++i)
                {
                    XmlNode Item = CboRootNode.ChildNodes[i];

                    CCboMenuNode node = CCboMenuNode.Construct(Item);
                    node.ParentControlID = MyControlId;
                    node.ImagesPath = ImagesPath;
                    node.NamingContainer = m_AddedToName;
                    if (i == CboRootNode.ChildNodes.Count -1)
                        node.RenderListItem(writer, Item.HasChildNodes, false, true);
                    else if (i == 0)
                        node.RenderListItem(writer, Item.HasChildNodes, true, false);
                    else
                        node.RenderListItem(writer, Item.HasChildNodes, false, false);

                    RenderChildTreeItems(Item, writer);
                    writer.RenderEndTag(); //li  was opened by the callee
                }

                writer.RenderEndTag(); //ul
                
            }

            writer.RenderEndTag(); // (HtmlTextWriterTag.Div);

        }

        private void RenderContextMenu(HtmlTextWriter writer)
        {
            writer.AddAttribute("class", "CboHeaderText");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write("Actions");
            writer.RenderEndTag(); //(HtmlTextWriterTag.div);

            writer.AddAttribute("id", "ctxlist");
            writer.AddAttribute("class", "CboMenu");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (CtxRootNode != null)
            {
                writer.AddAttribute("id", "cbotree");
                writer.AddAttribute("class", "open");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                for (int i = 0; i < CtxRootNode.ChildNodes.Count; ++i)
                {
                    XmlNode Item = CtxRootNode.ChildNodes[i];
                    CContextMenuNode node = CContextMenuNode.Construct(Item);

                    // make sure the node is meant to be rendered!
                    if (!node.Visible) continue;

                    node.ParentControlID = MyControlId;
                    node.ImagesPath = ImagesPath;
                    node.NamingContainer = m_AddedToName;

                    if (i == CtxRootNode.ChildNodes.Count - 1)
                        node.RenderListItem(writer, Item.HasChildNodes, false, true);
                    else if (i == 0)
                        node.RenderListItem(writer, Item.HasChildNodes, true, false);
                    else
                        node.RenderListItem(writer, Item.HasChildNodes, false, false); 

                    RenderChildTreeItems(Item, writer);
                    writer.RenderEndTag(); //li  was opened by the callee
                }

                writer.RenderEndTag(); //ul
            }

            writer.RenderEndTag(); // (HtmlTextWriterTag.Div);        
        }

        private void RenderTimeoutRow(HtmlTextWriter writer)
        {
            writer.AddAttribute("class","CboStatusBox");

            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute("cellspacing","0");
            writer.AddAttribute("cellpadding","0");
            writer.AddAttribute("border","0");
            writer.AddAttribute("style","width:100%;border-collapse:collapse;");

            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute("class","CboBoxHeader");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write("Status");
            writer.RenderEndTag(); // (HtmlTextWriterTag.Tr);

            writer.AddAttribute("id", m_AddedToName + "_CboTimeoutCell");
            writer.AddAttribute("class", "CboBoxHeader");
            writer.AddAttribute("align", "right");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            int iTimeout = Page.Session.Timeout - 5;

            if (iTimeout <= 0)
                writer.Write("<font color='red'>NO TIMEOUT!!!!</font>");
            else
                writer.Write("(Timeout in " + iTimeout.ToString() + " mins)");

            writer.RenderEndTag(); // (HtmlTextWriterTag.Tr);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Tr);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Table);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Td);
        }

        private void RenderUserNameStatusRow(HtmlTextWriter writer)
        {
            
            writer.AddAttribute("id", m_AddedToName + "_CboStatusCell");
            writer.AddAttribute("class", "CboBoxStatus");
            writer.AddAttribute("align", "left");
            writer.AddAttribute("valign", "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.RenderEndTag(); // (HtmlTextWriterTag.Td);

        }

        private void RenderHiddenFormControls(HtmlTextWriter writer)
        {
            // Username and info row
            writer.AddAttribute("class", "NotVisible");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            // NodeState CboNodeState
            writer.AddAttribute("name", m_AddedToName + "$" + CBO_TXT_STATE);
            writer.AddAttribute("type", "text");
            writer.AddAttribute("value", NodeState);
            writer.AddAttribute("id", m_AddedToName + "_" + CBO_TXT_STATE);

            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Input);

            // Ctx Node State
            writer.AddAttribute("name", m_AddedToName + "$" + CBO_TXT_STATECTX);
            writer.AddAttribute("type", "text");
            writer.AddAttribute("value", NodeCtxState);
            writer.AddAttribute("id", m_AddedToName + "_" + CBO_TXT_STATECTX);

            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Input);


            // Selected CBO
            writer.AddAttribute("name", m_AddedToName + "$" + CBO_TXT_SELITEM);
            writer.AddAttribute("type", "text");
            if (m_SelectedCboMenu == null)
                writer.AddAttribute("value", "");
            else
                writer.AddAttribute("value", CCboMenuNode.deXml(m_SelectedCboMenu.XmlNode.OuterXml));
            writer.AddAttribute("id", m_AddedToName + "_" + CBO_TXT_SELITEM);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Input);

            // Selected Ctx
            writer.AddAttribute("name", m_AddedToName + "$" + CBO_TXT_SELCTXITEM);
            writer.AddAttribute("type", "text");
            if (m_SelectedCtxMenu == null)
                writer.AddAttribute("value", "");
            else
                writer.AddAttribute("value", CCboMenuNode.deXml(m_SelectedCtxMenu.XmlNode.OuterXml));
            writer.AddAttribute("id", m_AddedToName + "_" + CBO_TXT_SELCTXITEM);

            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Input);

            // Submit button for Navigate
            writer.AddAttribute("id", m_AddedToName + "_" + CBO_TXT_SUBMIT);
            writer.AddAttribute("type", "submit");
            writer.AddAttribute("value", "");
            writer.AddAttribute("name", m_AddedToName + "$" + CBO_TXT_SUBMIT);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Input);

            // Submit button for Remove
            writer.AddAttribute("name", m_AddedToName + "$" + CBO_TXT_REMOVE);
            writer.AddAttribute("type", "submit");
            writer.AddAttribute("value", "");
            writer.AddAttribute("id", m_AddedToName + "_" + CBO_TXT_REMOVE);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag(); // (HtmlTextWriterTag.Input);

            writer.RenderEndTag(); // (HtmlTextWriterTag.div);
        }

        /// <summary>
        /// Create all the controls that make up the CBO tree and the Context tree
        /// </summary>
        protected override  void CreateChildControls()
        {
            StringBuilder clientScript = new StringBuilder();
            clientScript.Append("\n<script type=\"text/javascript\" src=\"" + Page.ResolveClientUrl("~/Scripts/HLTree.js") + "\"></script>\n");
            clientScript.Append("<script language=\"javascript\">\n");

            clientScript.Append("var o" + MyControlId + " = new HLTree();\n");

            clientScript.Append("</script>\n");

            if (!Page.ClientScript.IsClientScriptBlockRegistered(MyControlId + CBO_TREENAME))
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), MyControlId + CBO_TREENAME, clientScript.ToString());


        }

        /// <summary>
        /// Change the Selected Cbo to the first one matching the NavigateUrl
        /// </summary>
        /// <returns></returns>
        private bool ChangeSelectedCboToUrl(string Url)
        {
            bool retval = false;

            string XPathQuery = string.Format("//{0}[@{1}='{2}']", CCboMenuNode.CBO_NODE_NAME, CCboMenuNode.c_Url, Url);

            if (CboRootNode.SelectNodes(XPathQuery).Count > 0)
            {
                XmlNode newNode = CboRootNode.SelectSingleNode(XPathQuery);

                SetSelectedCbo(CCboMenuNode.Construct(newNode));
                SetSelectedCtx(null);
                retval = true;
            }

            return retval;
        }

        /// <summary>
        /// Change the Selected Ctx to the first one matching the ContextUrl
        /// </summary>
        /// <returns></returns>
        private bool ChangeSelectedCtxToUrl(string Url)
        {
            bool retval = false;

            string XPathQuery = string.Format("//{0}[@{1}='{2}']", CContextMenuNode.CTX_NODE_NAME, CCboMenuNode.c_Url, Url);

            if (CtxRootNode.SelectNodes(XPathQuery).Count > 0)
            {
                XmlNode newNode = CtxRootNode.SelectSingleNode(XPathQuery);
                SetSelectedCtx(CContextMenuNode.Construct(newNode));
                retval = true;
            }

            return retval;
        }


        #region New CBO Code

        private void createDataDocument()
        {
            m_CboDataDoc = new XmlDocument();

            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xtw = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xtw.Formatting = Formatting.Indented;

            xtw.WriteStartDocument();
            xtw.WriteStartElement(DOCUMENT_ROOT);
            xtw.WriteAttributeString("xmlns", "treeview", null, "urn:sahl.loanservicing");

            generateCboTreeNodes(xtw);
            generateContextMenuNodes(xtw);
            xtw.WriteEndElement(); //DOCUMENT_ROOT
            xtw.WriteEndDocument();

            xtw.Flush();
            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            memoryStream.Seek(0, SeekOrigin.Begin);

            m_CboDataDoc.Load(memoryStream);

            memoryStream.Close();
            xtw.Close();
        }

        /// <summary>
        /// Generate the xml for the CBO menu nodes
        /// </summary>
        /// <param name="xw"></param>
        private void generateCboTreeNodes(XmlTextWriter xw)
        {
            xw.WriteStartElement(CBO_TREENAME);
            DataRow[] dr = DataSource.UserSelects.Select("ParentKey is null", "");
            foreach (CBO.UserSelectsRow userRow in dr)
            {

                CGenericCboNode newNode = new CGenericCboNode(null, userRow);
                if (newNode != null)
                {
                    xw.WriteStartElement(CCboMenuNode.CBO_NODE_NAME);

                    if (newNode.Equals(m_SelectedCboMenu))
                        newNode.isSelectedItem = true;

                    newNode.isExpanded = true;
                    if (NodeState.Contains("+" + CCboMenuNode.UL_CBO + newNode.TreeUniqueId + ";"))
                        newNode.isExpanded = false;

                    newNode.WriteXmlNode(xw);
                    addRowChildren(xw, newNode);
                    xw.WriteEndElement(); //cbo
                }
                
            }
            xw.WriteEndElement(); //CBO_TREENAME
        }

        /// <summary>
        /// Generates an xml fragment for the CBO Context Menu
        /// </summary>
        private void generateContextMenuNodes(XmlTextWriter xw)
        {
            xw.WriteStartElement(CTX_TREENAME);

            if (m_SelectedCboMenu != null)
            {
                // Add the general Context Nodes
                string szFilter = "CoreBusinessObjectKey=" + m_SelectedCboMenu.CoreBusinessObjectKey.ToString();
                DataRow[] dr = DataSource.ContextMenu.Select(szFilter, "Sequence");

                foreach (CBO.ContextMenuRow ctxRow in dr)
                {
                    xw.WriteStartElement(CContextMenuNode.CTX_NODE_NAME);
                    CGenericCtxMenu newNode = new CGenericCtxMenu(null, ctxRow);

                    if (newNode.Equals(m_SelectedCtxMenu))
                        newNode.isSelectedItem = true;

                    newNode.isExpanded = false;

                    if (NodeCtxState.Contains("-" + CContextMenuNode.UL_CTX + newNode.TreeUniqueId + ";"))
                        newNode.isExpanded = true;

                    newNode.WriteXmlNode(xw);

                    if (getChildContextKey(ctxRow.ContextKey) != -1)
                        appendContextNodeChildren(xw, newNode);

                    xw.WriteEndElement(); //CTX_NODE_NAME
                }


                szFilter = "UserSelectsKey=" + ((CGenericCboNode)m_SelectedCboMenu).UserSelectsKey.ToString();
                dr = DataSource.WorkflowContextMenu.Select(szFilter, "");
                bool displayFormNode = true;

                foreach (CBO.WorkflowContextMenuRow wflCtxRow in dr)
                {
                    
                    xw.WriteStartElement(CContextMenuNode.CTX_NODE_NAME);
                    CWorkflowCtxMenu newNode = new CWorkflowCtxMenu(null, wflCtxRow);

                    if (newNode.Equals(m_SelectedCtxMenu))
                        newNode.isSelectedItem = true;
                    
                    newNode.isExpanded = false;

                    if (NodeCtxState.Contains("-" + CContextMenuNode.UL_CTX + newNode.TreeUniqueId + ";"))
                        newNode.isExpanded = true;

                    // only one node of type "F" can be written to the screen, so if we have already 
                    // written one, then set the rest to invisible
                    if (wflCtxRow.MenuType == "F")
                    {
                        newNode.Visible = displayFormNode;
                        displayFormNode = false;
                    }

                    newNode.WriteXmlNode(xw);
                    xw.WriteEndElement(); //CTX_NODE_NAME

                    //break;
                }

            }
            xw.WriteEndElement(); //CBO_CTXTREE_NAME

        }


        private void addRowChildren(XmlTextWriter xw, CGenericCboNode parentNode)
        {
            DataRow[] dr = DataSource.UserSelects.Select("ParentKey = " + parentNode.UserSelectsKey, "");
            foreach (CBO.UserSelectsRow userRow in dr)
            {
                CGenericCboNode newChild = new CGenericCboNode(parentNode, userRow);
                if (newChild != null)
                {
                    xw.WriteStartElement(CCboMenuNode.CBO_NODE_NAME);

                    newChild.isExpanded = true;

                    if (NodeState.Contains("+" + CCboMenuNode.UL_CBO + newChild.TreeUniqueId +";"))
                        newChild.isExpanded = false;

                    if (m_SelectedCboMenu != null)
                    {
                        if (newChild.TreeUniqueId == m_SelectedCboMenu.TreeUniqueId)
                            newChild.isSelectedItem = true;

                        if ((m_SelectedCboMenu.NodeId == newChild.NodeId) && (newChild.NodeId != -1))
                            newChild.isHighlightedGroup = true;


                    }

                    newChild.WriteXmlNode(xw);

                    addRowChildren(xw, newChild);
                    xw.WriteEndElement(); //cbo
                }
            }
        }

        /// <summary>
        /// Add a workflow folder child
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        private void addWorflowInstance(XmlTextWriter xw, CCboMenuNode parentNode, CBO.UserSelectsRow userRow)
        {
            DataRow[] dr = DataSource.WorkFlowInstanceInfo.Select("UserSelectsKey = " + parentNode.UserSelectsKey , "");
            foreach (CBO.WorkFlowInstanceInfoRow wflRow in dr)
            {
                CWorkflowInstanceNode newNode = new CWorkflowInstanceNode(parentNode, userRow, wflRow);

                if (newNode != null)
                {
                    xw.WriteStartElement(CCboMenuNode.CBO_NODE_NAME);
                    if (newNode.TreeUniqueId == m_SelectedCboMenu.TreeUniqueId)
                        newNode.isSelectedItem = true;

                    if ((m_SelectedCboMenu.NodeId == newNode.NodeId) && (newNode.NodeId != -1))
                        newNode.isHighlightedGroup = true;

                    newNode.WriteXmlNode(xw);
                    xw.WriteEndElement(); //cbo
                }
            }


        }


        /// <summary>
        /// Append the context nodes for the selected item 
        /// </summary>
        /// <param name="tnode"></param>
        /// <param name="parentKey"></param>
        private void appendContextNodeChildren(XmlTextWriter xw, CGenericCtxMenu parentNode)
        {
            // todo: if workflow context nodes ever have children, then this needs to be changed
            // to add the workflow context nodes as well

            string szFilter = "ParentKey=" + parentNode.ContextKey.ToString();
            DataRow[] dr = DataSource.ContextMenu.Select( szFilter, "Sequence" );

            foreach (CBO.ContextMenuRow ctxRow in dr)
            {
                xw.WriteStartElement(CContextMenuNode.CTX_NODE_NAME);

                CGenericCtxMenu newNode = new CGenericCtxMenu(parentNode, ctxRow);

                if (newNode.Equals(m_SelectedCtxMenu))
                    newNode.isSelectedItem = true;

                newNode.isExpanded = false;

                if (NodeCtxState.Contains("-" + CContextMenuNode.UL_CTX + newNode.TreeUniqueId + ";"))
                    newNode.isExpanded = true;

                newNode.WriteXmlNode(xw);

                if (getChildContextKey(newNode.ContextKey) != -1)
                    appendContextNodeChildren(xw, newNode);

                xw.WriteEndElement(); //CTX_NODE_NAME
            }
        }


        private int getChildContextKey(int contextKey)
        {
            // Return the next child context key.
            string szFilter = "ParentKey=" + contextKey.ToString();
            DataRow[] dr = DataSource.ContextMenu.Select(szFilter);
            if (dr.Length > 0)
                return (int)dr[0]["ContextKey"];

            return -1;
        }

        #endregion

        #region New CBO HTML Generate


        /// <summary>
        /// New version which is going to pop out the raw html to be styled
        /// 
        /// </summary>
        /// <param name="containerPanel"></param>
        /// <param name="rootNode"></param>
        /// <param name="IndentLevel"></param>
        private void RenderNavMenu(HtmlTextWriter writer, XmlNode rootNode)
        {

        }

        private void RenderChildTreeItems(XmlNode rootNode, HtmlTextWriter htw)
        {
            if (rootNode.ChildNodes.Count > 0)
            {
                string id = "";
                string szNodeState = "";

                if (rootNode.Attributes[CCboMenuNode.c_UniqueID] != null)
                {
                    if (rootNode.Name == CCboMenuNode.CBO_NODE_NAME)
                    {
                        CCboMenuNode node = CCboMenuNode.Construct(rootNode);
                        node.ParentControlID = MyControlId;
                        node.ImagesPath = ImagesPath;
                        node.NamingContainer = m_AddedToName;
                        node.RenderListHeader(htw);
                    }

                    if (rootNode.Name == CContextMenuNode.CTX_NODE_NAME)
                    {
                        CContextMenuNode node = CContextMenuNode.Construct(rootNode);
                        node.ParentControlID = MyControlId;
                        node.ImagesPath = ImagesPath;
                        node.NamingContainer = m_AddedToName;
                        node.RenderListHeader(htw);
                    }
                }

                
                for (int i = 0 ; i < rootNode.ChildNodes.Count; ++i)
                {
                    XmlNode Item = rootNode.ChildNodes[i];
                    if (Item.Name == CCboMenuNode.CBO_NODE_NAME)
                    {
                        CCboMenuNode node = CCboMenuNode.Construct(Item);
                        node.ParentControlID = MyControlId;
                        node.ImagesPath = ImagesPath;
                        node.NamingContainer = m_AddedToName;

                        if (i == rootNode.ChildNodes.Count - 1)
                            node.RenderListItem(htw, Item.HasChildNodes, false, true);
                        else if (i == 0)
                            node.RenderListItem(htw, Item.HasChildNodes, true, false);
                        else
                            node.RenderListItem(htw, Item.HasChildNodes, false, false);
                    }

                    if (Item.Name == CContextMenuNode.CTX_NODE_NAME)
                    {
                        CContextMenuNode node = CContextMenuNode.Construct(Item);
                        node.ParentControlID = MyControlId;
                        node.ImagesPath = ImagesPath;
                        node.NamingContainer = m_AddedToName;

                        if (i == rootNode.ChildNodes.Count - 1)
                            node.RenderListItem(htw, Item.HasChildNodes, false, true);
                        else if (i == 0)
                            node.RenderListItem(htw, Item.HasChildNodes, true, false);
                        else
                            node.RenderListItem(htw, Item.HasChildNodes, false, false);
                    }

                    RenderChildTreeItems(Item, htw);
                    htw.RenderEndTag(); //li  was opened by the callee


                }
                htw.RenderEndTag(); //ul
            }
        }

        private void RenderCboMenuNode(XmlNode node, HtmlTextWriter htw)
        {
 

            // Write out a Contex node, mostly the same but it has no icons
            if (node.Name == CContextMenuNode.CTX_NODE_NAME)
            {
            }

            // </li> will be written for you by the caller

        }

        #endregion

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.DesignMode)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("< class='CboBox'>");
                sb.Append("<tr><td class='CboBoxHeader'>SAHL Tree</td></tr>");
                sb.Append("<tr><td>Node1<br>Node2</td></tr>");
                sb.Append("</table>");
                output.Write(sb.ToString());
            }
            else
            {
                EnsureChildControls();
                GenerateFocusScript();
                GenerateRequestTimeScript();
                RenderCbo(output);
            }
        }

        private void GenerateFocusScript()
        {
            StringBuilder sb = new StringBuilder();

            // Cbo
            if (m_SelectedCboMenu != null)
            {
                string id = CCboMenuNode.LI_CBO + m_SelectedCboMenu.TreeUniqueId;
                sb.AppendLine("focusCboTreeItem('" + id + "');");
            }

            // Context
            if (m_SelectedCtxMenu != null)
            {
                string id = CContextMenuNode.LI_CTX +m_SelectedCtxMenu.TreeUniqueId;
                sb.AppendLine("focusCboTreeItem('" + id + "');");
            }

            if (sb.Length > 0)
                Page.ClientScript.RegisterStartupScript(this.GetType(), MyControlId + CBO_TREENAME + "FOCUS", sb.ToString(), true);
        }
        //TODO mm
        public void MarkTime(string EventName)
        {
            //string szDisplay = DataSource.CboProperties[0].InformationDisplay;

            //if (szDisplay.Length > 0)
            //{
            //    string[] szTime = szDisplay.Split(':');
            //    if (szTime.Length > 4)
            //    {
            //        DateTime StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, int.Parse(szTime[0]), int.Parse(szTime[1]), int.Parse(szTime[2]), int.Parse(szTime[3]));
            //        TimeSpan ts = DateTime.Now - StartTime;

            //        DataSource.CboProperties[0].InformationDisplay += string.Format("-{0}.{1};{2}", System.Math.Floor(ts.TotalSeconds), ts.Milliseconds, EventName);
            //    }
            //}
            //else
            //{
            //    DataSource.CboProperties[0].InformationDisplay = DateTime.Now.ToString("HH:mm:ss:FFF:") + EventName.Trim();
            //}
        }

        // TODO: Check if this must be removed.
        /// <summary>
        /// Times the request as a cbo item is clicked.
        /// The time is measured client side so it is end to end.
        /// </summary>
        private void GenerateRequestTimeScript()
        {
            StringBuilder sb = new StringBuilder();
            int iTimeout = Page.Session.Timeout - 5;

            
            sb.AppendLine("var s = GetCookie(\"SAHLReqTime\");");
            sb.AppendLine("var stimeout = " + iTimeout.ToString() + ";");
            sb.AppendLine("if (s != null && s != '-1' )");
            sb.AppendLine("{");
            sb.AppendLine("    var dt = new Date();");
            sb.AppendLine("    var o = document.getElementById(\"" + m_AddedToName + "_CboStatusCell\");");
            sb.AppendLine("    if ( o != null )");
            sb.AppendLine("    {");
            sb.AppendLine("        var milli = parseFloat(s);");
            sb.AppendLine("        var nowTime = parseFloat(dt.getTime());");
            sb.AppendLine("        s = nowTime - milli;");
            sb.AppendLine("        o.innerHTML = \"Request took <b>\" + (s/1000) + \"</b> s.\";");
            sb.AppendLine("        o.innerHTML += \"<br>User: <b>" + Authentication.Authenticator.GetSimpleWindowsUserName() + " " + DisplayVersionNumber + "</b>\";");
            sb.AppendLine("    }");
            sb.AppendLine("    SetCookie('SAHLReqTime',-1);");
            sb.AppendLine("}");

            sb.AppendLine("var interval = window.setInterval('updateTimeOut()',60000);");
            sb.AppendLine("function updateTimeOut()");
            sb.AppendLine("{");
            sb.AppendLine("    var o = document.getElementById(\"" + m_AddedToName + "_CboTimeoutCell\");");
            sb.AppendLine("    if ( o != null )");
            sb.AppendLine("    {");
            sb.AppendLine("        if ( stimeout != 0)");
            sb.AppendLine("            stimeout = stimeout - 1;");
            sb.AppendLine("        if ( stimeout == 0)");
            sb.AppendLine("        {");
            sb.AppendLine("            o.innerHTML = \"!!Session Timed Out!!\";");
            sb.AppendLine("            o.className = \"CboBoxTimeout\";");
            sb.AppendLine("            window.clearInterval(interval);"); // Stop the timer.
            sb.AppendLine("            doTimeoutNav();");
            sb.AppendLine("        }");
            sb.AppendLine("        else");
            sb.AppendLine("            o.innerHTML = \"(Timeout in \" + stimeout + \" min)\";");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            sb.AppendLine("function doTimeoutNav()");
            sb.AppendLine("{");
            sb.AppendLine("    var sBase = window.location.protocol + \"//\";");
            sb.AppendLine("    var isOpera=(navigator.userAgent.indexOf(\"Opera\")!=-1)?true:false;");

            sb.AppendLine("    if ( isOpera )");
            sb.AppendLine("        sBase += window.location.hostname;");
            sb.AppendLine("    else");
            sb.AppendLine("        sBase += window.location.host;");
            
            // Navigate to the timeout page so that the user can start a new session.
            sb.AppendLine("    window.location.replace('" + Page.ResolveClientUrl("~/TimeoutError.aspx") + "');");
            sb.AppendLine("}");

            Page.ClientScript.RegisterStartupScript(this.GetType(), MyControlId + CBO_TREENAME + "TIMER", sb.ToString(), true);
        }
    }

    
    public class CboNavigateEventArgs: EventArgs
    {
        public CboNavigateEventArgs(string navurl)
        {
		    this.navurl = navurl;
	    }

        public CboNavigateEventArgs(string GenericKey, int UserSelectsKey)
        {
            this.GenericKey = GenericKey;
            this.UserSelectsKey = UserSelectsKey;
        }

        public CboNavigateEventArgs()
        {
            navurl = "";
            GenericKey = "";
            UserSelectsKey = -1;
            SelectedItemPath = "";
        }

	    public string navurl;
        public string GenericKey;
        public int UserSelectsKey;
        public string SelectedItemPath;
    }


    #region CboMenuClasses
    /// <summary>
    /// A class that represents a node in the Cbo Menu
    /// Must be contructed from a UserSelects Row
    /// </summary>
    public abstract class CCboMenuNode
    {
        public const string LI_CBO = "li_cbo";
        public const string UL_CBO = "ul_cbo";
        private const string CBO_REMOVE_URL = "Cancel.png";

        public static string CBO_NODE_NAME = "CboData";

        // UserSelects columns
        public const string c_UserSelectsKey = "us";
        public const string c_CoreBusinessObjectKey = "cb";
        public const string c_GenericKey = "gk";
        public const string c_ShortDescription = "sd";
        public const string c_LongDescription = "ld";
        public const string c_ParentKey = "pk";
        public const string c_MenuIcon = "mi";
        public const string c_GenericTypeKey = "gt";
        public const string c_NodeId = "ni";
        public const string c_CanRemove = "rm";
        public const string c_isExpanded = "ie";
        public const string c_Url = "ur";
        public const string c_includeParentIcons = "ip";
        public const string c_NodeType = "nt";
        public const string c_OriginationSource = "or";

        // Derived Values
        public const string c_UniqueID = "ui";
        public const string c_Level = "lv";
        public const string c_SelectedItem = "si";
        public const string c_HighlightedGroup = "hg";


        private string m_UniqueID;
        private int m_Level;
        private int m_NodeId;
        private int m_CoreBusinessObjectKey;
        private string m_ShortDescription;
        private string m_LongDescription;
        private bool m_CanRemove;
        private string m_Url;
        private string m_MenuIcon;
        private int m_GenericTypeKey;
        private bool m_SelectedItem; // true if this item is selected
        private bool m_HighlightedGroup; // true if this item is part of the highlighted group
        private int m_ParentKey;
        public char m_NodeType;
        private int m_UserSelectsKey;
        private string m_GenericKey;
        private bool m_isExpanded;
        private bool m_includeParentIcons;
        private int m_OriginationSource;

        // used for rendering, must be set up by the call that want me to render
        private string m_ImagesPath = "";
        private string m_NamingContainer = "";
        private string m_ParentControlID = "";


        /// <summary>
        /// A Node Id groups a node with it's parent, all nodes withthe same id belongto the same parent (ultimately)
        /// </summary>
        public int NodeId
        {
            get { return m_NodeId; }
            set { m_NodeId = value; }
        }

        /// <summary>
        /// A Node Id groups a node with it's parent, all nodes withthe same id belongto the same parent (ultimately)
        /// </summary>
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        /// <summary>
        /// The Core Business Object Key, i.e. the sub type of the CBO
        /// </summary>
        public int CoreBusinessObjectKey
        {
            get {return m_CoreBusinessObjectKey;}
            set {m_CoreBusinessObjectKey = value;}
        }

        /// <summary>
        /// A Description of the CBO
        /// </summary>
        public string ShortDescription
        {
            get {return m_ShortDescription;}
            set {m_ShortDescription = value;}
        }

        /// <summary>
        /// A Description of the CBO
        /// </summary>
        public string LongDescription
        {
            get {return m_LongDescription;}
            set {m_LongDescription = value;}
        }

        /// <summary>
        /// True if the user can remove the node from the tree
        /// </summary>
        public bool CanRemove
        {
            get {return m_CanRemove;}
            set {m_CanRemove = value;}
        }

        /// <summary>
        /// True if the item is currently selcted
        /// </summary>
        public bool isSelectedItem
        {
            get { return m_SelectedItem; }
            set { m_SelectedItem = value; }
        }

        /// <summary>
        /// True if the item is part of the highlighted group
        /// </summary>
        public bool isHighlightedGroup
        {
            get { return m_HighlightedGroup; }
            set { m_HighlightedGroup = value; }
        }

        /// <summary>
        /// The Url of the Menu Item
        /// </summary>
        public string Url
        {
            get {return m_Url;}
            set {m_Url = value;}
        }

        /// <summary>
        /// The MenuIcon image file which represents the menu icon
        /// </summary>
        public string MenuIcon
        {
            get {return m_MenuIcon;}
            set {m_MenuIcon = value;}
        }

        /// <summary>
        /// The key of the generic type
        /// </summary>
        public int GenericTypeKey
        {
            get {return m_GenericTypeKey;}
            set {m_GenericTypeKey = value;}
        }

        /// <summary>
        /// The The Sequence in the menu
        /// </summary>
        public int ParentKey
        {
            get { return m_ParentKey; }
            set { m_ParentKey = value; }
        }

        /// <summary>
        /// A char representinf the type of node, either Static, Dynamic or Workflow
        /// </summary>
        public char NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }

        /// <summary>
        /// The UserSelectsKey, probably unique
        /// </summary>
        public int UserSelectsKey
        {
            get { return m_UserSelectsKey; }
            set { m_UserSelectsKey = value; }
        }

        /// <summary>
        /// The Generic Key, usually the Legal Entity key or some other such thing
        /// </summary>
        public string GenericKey
        {
            get { return m_GenericKey; }
            set { m_GenericKey = value; }
        }

        /// <summary>
        /// Is the menu node expanded initially
        /// </summary>
        public bool isExpanded
        {
            get { return m_isExpanded; }
            set { m_isExpanded = value; }
        }

        /// <summary>
        /// Included parent icons in this when displaying this nodes header icons
        /// </summary>
        public bool includeParentIcons
        {
            get { return m_includeParentIcons; }
            set { m_includeParentIcons = value; }
        }

        /// <summary>
        /// The origination source of the node, will cause a company icon to be displayed next to the main icon
        /// </summary>
        public int OriginationSource
        {
            get { return m_OriginationSource; }
            set { m_OriginationSource = value; }
        }

        public string ImagesPath
        {
            get { return m_ImagesPath; }
            set { m_ImagesPath = value; }
        }

        public string NamingContainer
        {
            get { return m_NamingContainer; }
            set { m_NamingContainer = value; }
        }

        public string ParentControlID
        {
            get { return m_ParentControlID; }
            set { m_ParentControlID = value; }
        }

        /// <summary>
        /// A string represetining a unique identifier for the node
        /// </summary>
        public abstract string MyUniqueID
        {
            get;
        }

        public string TreeUniqueId
        {
            get { return m_UniqueID; }
            set { m_UniqueID = value; }
        }

        /// <summary>
        /// All menu nodes must be capable of returning a UserSelectsRow to be able to add them to selected nodes
        /// </summary>
        public abstract CBO.UserSelectsRow UserSelectsRow
        {
            get;
        }

        public string ImageUrl
        {
            get
            {
                return MenuIcon;
            }
        }

        /// <summary>
        /// Return the node as a set of pipe delimited values that can be used to uniquely identify the node
        /// </summary>
        /// <returns></returns>
        public virtual XmlNode XmlNode
        {
            get
            {
                XmlDocument retval = new XmlDocument();
                
                retval.LoadXml("<" + CBO_NODE_NAME +"/>");

                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_UniqueID));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_Level));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_NodeType));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_NodeId));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_CoreBusinessObjectKey));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_ShortDescription));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_LongDescription));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_CanRemove));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_Url));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_MenuIcon));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_GenericTypeKey));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_SelectedItem));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_HighlightedGroup));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_ParentKey));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_GenericKey));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_UserSelectsKey));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_isExpanded));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_includeParentIcons));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_OriginationSource));
                

                retval.DocumentElement.Attributes[c_UserSelectsKey].Value = UserSelectsKey.ToString();
                retval.DocumentElement.Attributes[c_GenericKey].Value = GenericKey.Trim(); ;
                retval.DocumentElement.Attributes[c_UniqueID].Value = TreeUniqueId.Trim();
                retval.DocumentElement.Attributes[c_Level].Value = Level.ToString();
                retval.DocumentElement.Attributes[c_NodeType].Value = NodeType.ToString();
                retval.DocumentElement.Attributes[c_NodeId].Value = NodeId.ToString();
                retval.DocumentElement.Attributes[c_CoreBusinessObjectKey].Value = CoreBusinessObjectKey.ToString();
                retval.DocumentElement.Attributes[c_ShortDescription].Value = ShortDescription.Trim();
                retval.DocumentElement.Attributes[c_LongDescription].Value = LongDescription.Trim();
                retval.DocumentElement.Attributes[c_Url].Value = Url.Trim();
                retval.DocumentElement.Attributes[c_MenuIcon].Value = MenuIcon.Trim(); ;
                retval.DocumentElement.Attributes[c_GenericTypeKey].Value = GenericTypeKey.ToString();
                retval.DocumentElement.Attributes[c_ParentKey].Value = ParentKey.ToString();
                retval.DocumentElement.Attributes[c_OriginationSource].Value = OriginationSource.ToString(); 


                if (CanRemove) retval.DocumentElement.Attributes[c_CanRemove].Value = "1";
                if (isSelectedItem) retval.DocumentElement.Attributes[c_SelectedItem].Value = "1";
                if (isHighlightedGroup) retval.DocumentElement.Attributes[c_HighlightedGroup].Value = "1";

                if (isExpanded)
                    retval.DocumentElement.Attributes[c_isExpanded].Value = "1";
                else
                    retval.DocumentElement.Attributes[c_isExpanded].Value = "0";

                if (includeParentIcons)
                    retval.DocumentElement.Attributes[c_includeParentIcons].Value = "1";
                else
                    retval.DocumentElement.Attributes[c_includeParentIcons].Value = "0";


                return retval.DocumentElement;
            }
        }

        /// <summary>
        /// Given the XmlTextWriter write a new new node that contains the node data
        /// </summary>
        /// <param name="xtw"></param>
        /// <returns></returns>
        public virtual void WriteXmlNode(XmlTextWriter xtw)
        {
            xtw.WriteAttributeString(c_NodeType, NodeType.ToString());
            xtw.WriteAttributeString(c_UniqueID, TreeUniqueId);
            xtw.WriteAttributeString(c_Level, Level.ToString());
            xtw.WriteAttributeString(c_NodeId, NodeId.ToString());
            xtw.WriteAttributeString(c_CoreBusinessObjectKey, CoreBusinessObjectKey.ToString());
            xtw.WriteAttributeString(c_ShortDescription, ShortDescription.Trim());
            xtw.WriteAttributeString(c_LongDescription, LongDescription.Trim());
            xtw.WriteAttributeString(c_Url, Url.Trim());
            xtw.WriteAttributeString(c_MenuIcon, MenuIcon.Trim());
            xtw.WriteAttributeString(c_GenericTypeKey, GenericTypeKey.ToString());
            xtw.WriteAttributeString(c_ParentKey, ParentKey.ToString());
            xtw.WriteAttributeString(c_GenericKey, GenericKey);
            xtw.WriteAttributeString(c_UserSelectsKey, UserSelectsKey.ToString());
            xtw.WriteAttributeString(c_OriginationSource, OriginationSource.ToString());

            if (CanRemove) xtw.WriteAttributeString(c_CanRemove, "1");
            if (isSelectedItem) xtw.WriteAttributeString(c_SelectedItem, "1");
            if (isHighlightedGroup) xtw.WriteAttributeString(c_HighlightedGroup, "1");

            if (isExpanded)
                xtw.WriteAttributeString(c_isExpanded, "1");
            else
                xtw.WriteAttributeString(c_isExpanded, "0");

            if (includeParentIcons)
                xtw.WriteAttributeString(c_includeParentIcons, "1");
            else
                xtw.WriteAttributeString(c_includeParentIcons, "0");

        }

        public virtual bool Equals(CCboMenuNode compare)
        {
            return Equals(compare, true);
        }

        public virtual bool Equals(CCboMenuNode compare, bool BeStrict)
        {
            if (compare == null) return false;


            // the node it not always contructed with the parent node passed in 
            // which is the only way to know the TreeUniqueID , so if we
            // are not being string then just make sure the items unique id
            // is part of the TreeUnique id
            if (BeStrict)
            {
                if (this.TreeUniqueId.Length > 0)
                    if (compare.TreeUniqueId != this.TreeUniqueId) return false;
            }
            else
            {
                if (this.MyUniqueID.Length > 0)
                    if (compare.MyUniqueID != this.MyUniqueID) return false;
            }

            if (compare.CoreBusinessObjectKey != this.CoreBusinessObjectKey) return false;
            if (compare.GenericTypeKey != this.GenericTypeKey) return false;
            if (compare.LongDescription != this.LongDescription) return false;
            if (compare.NodeType != this.NodeType) return false;
            if (compare.ShortDescription != this.ShortDescription) return false;
            if (compare.Url != this.Url) return false;
            if (compare.ParentKey != this.ParentKey) return false;
            if (compare.GenericKey != this.GenericKey) return false;
            if (compare.GenericTypeKey != this.GenericTypeKey) return false;

            return true;
        }

        /// <summary>
        /// Generic Constructor
        /// </summary>
        protected CCboMenuNode()
        {
            CoreBusinessObjectKey = -1;
            ShortDescription = "Unknown Node Type";
            LongDescription = "Unknown Node Type";
            CanRemove = false;
            isSelectedItem = false;
            isHighlightedGroup = false;
            Url = "";
            MenuIcon = "";
            GenericTypeKey = -1;
            NodeId = -1;
            ParentKey = -1;
            TreeUniqueId = "";
            Level = 0;
            NodeType = 'U';
            GenericKey = "-1";
            OriginationSource = -1;
        }


        /// <summary>
        /// Contruct from a CoreBusinessObjectMenuRow Dataset row
        /// </summary>
        /// <param name="cboRow"></param>
        public CCboMenuNode(CCboMenuNode parentNode, CBO.UserSelectsRow userRow)
        {
            NodeType = userRow.NodeType[0];
            UserSelectsKey = userRow.UserSelectsKey;
            CoreBusinessObjectKey = userRow.CoreBusinessObjectKey;
            GenericTypeKey = userRow.GenericTypeKey;
            CanRemove = userRow.IsRemovable;
            isExpanded = userRow.IsExpanded;
            includeParentIcons = userRow.IncludeParentHeaderIcons;

            if (userRow.IsOriginationSourceKeyNull())
                OriginationSource = -1;
            else
                OriginationSource = userRow.OriginationSourceKey;

            if (userRow.IsMenuIconNull())
                MenuIcon = "";
            else
                MenuIcon = userRow.MenuIcon;

            if (userRow.IsParentKeyNull())
                ParentKey = -1;
            else
                ParentKey = userRow.ParentKey;

            if (userRow.IsURLNull())
                Url = "";
            else
                Url = userRow.URL;

            if (userRow.IsDynamicDescriptionNull())
                ShortDescription = "Dynamic Object";
            else
                ShortDescription = userRow.DynamicDescription;

            if (userRow.IsGenericKeyNull())
                GenericKey = "-1";
            else
                GenericKey = userRow.GenericKey;

            if (userRow.IsLongDescriptionNull())
                LongDescription = "Dynamic Object";
            else
                LongDescription = userRow.LongDescription;

            if (userRow.IsNodeIDNull())
                NodeId = -1;
            else
                NodeId = userRow.NodeID;


        }

        

        /// <summary>
        /// Construct using the Xmlnode that was written using from the WriteXmlNode methid
        /// </summary>
        /// <param name="row"></param>
        public CCboMenuNode(XmlNode node)
        {
            LoadFromNode(node);
        }

        /// <summary>
        /// Construct using the an Xml String that was written with the WriteXmlNode method
        /// </summary>
        /// <param name="row"></param>
        public CCboMenuNode(string row)
        {
            XmlDocument xDocIn = new XmlDocument();
            xDocIn.LoadXml(row);
            LoadFromNode(xDocIn);
        }

        private void LoadFromNode(XmlNode node)
        {

            if (node.Attributes[c_NodeType] != null)
                NodeType = node.Attributes[c_NodeType].Value[0];

            if (node.Attributes[c_UniqueID] != null)
                TreeUniqueId = node.Attributes[c_UniqueID].Value.Trim();

            if (node.Attributes[c_CoreBusinessObjectKey] != null)
                CoreBusinessObjectKey = int.Parse(node.Attributes[c_CoreBusinessObjectKey].Value);

            if (node.Attributes[c_NodeId] != null)
                NodeId = int.Parse(node.Attributes[c_NodeId].Value);

            if (node.Attributes[c_ShortDescription] != null)
                ShortDescription = node.Attributes[c_ShortDescription].Value;

            if (node.Attributes[c_LongDescription] != null)
                LongDescription = node.Attributes[c_LongDescription].Value;

            if (node.Attributes[c_CanRemove] != null)
                if (node.Attributes[c_CanRemove].Value == "1") CanRemove = true;

            if (node.Attributes[c_SelectedItem] != null)
                if (node.Attributes[c_SelectedItem].Value == "1") isSelectedItem = true;

            if (node.Attributes[c_HighlightedGroup] != null)
                if (node.Attributes[c_HighlightedGroup].Value == "1") isHighlightedGroup = true;

            if (node.Attributes[c_Url] != null)
                Url = node.Attributes[c_Url].Value;

            if (node.Attributes[c_MenuIcon] != null)
                MenuIcon = node.Attributes[c_MenuIcon].Value;

            if (node.Attributes[c_GenericTypeKey] != null)
                GenericTypeKey = int.Parse(node.Attributes[c_GenericTypeKey].Value);

            if (node.Attributes[c_ParentKey] != null)
                ParentKey = int.Parse(node.Attributes[c_ParentKey].Value);

            if (node.Attributes[c_GenericKey] != null)
                GenericKey = node.Attributes[c_GenericKey].Value;

            if (node.Attributes[c_UserSelectsKey] != null)
                UserSelectsKey = int.Parse(node.Attributes[c_UserSelectsKey].Value);

            if (node.Attributes[c_OriginationSource] != null)
                OriginationSource = int.Parse(node.Attributes[c_OriginationSource].Value);

            isExpanded = false;
            if (node.Attributes[c_isExpanded] != null)
                if (node.Attributes[c_isExpanded].Value == "1") isExpanded = true;

            includeParentIcons = false;
            if (node.Attributes[c_includeParentIcons] != null)
                if (node.Attributes[c_includeParentIcons].Value == "1") includeParentIcons = true;

        }

        public static CboMenuNodeTypes ToNodeType(char NodeType)
        {
            CboMenuNodeTypes retval = CboMenuNodeTypes.Unknown;

            switch (NodeType)
            {
                case (char)CboMenuNodeTypes.Dynamic:
                    retval = CboMenuNodeTypes.Dynamic; break;
                case (char)CboMenuNodeTypes.Static:
                    retval = CboMenuNodeTypes.Static; break;
                case (char)CboMenuNodeTypes.TNode:
                    retval = CboMenuNodeTypes.TNode; break;
                case (char)CboMenuNodeTypes.Workflow:
                    retval = CboMenuNodeTypes.Workflow; break;
                case (char)CboMenuNodeTypes.WorkflowFolder:
                    retval = CboMenuNodeTypes.WorkflowFolder; break;
            }
            return retval;
        }

        public static CCboMenuNode Construct(XmlNode InputNode)
        {
            CCboMenuNode retval = null;
            retval = new CGenericCboNode(InputNode);
            return retval;
        }

        public static CCboMenuNode Construct(CCboMenuNode parentNode, CBO.UserSelectsRow InputRow)
        {
            CCboMenuNode retval = null;
            retval = new CGenericCboNode(parentNode, InputRow);
            return retval;
        }

        public static CCboMenuNode Construct(string row)
        {
            // the input string is XML
            if (row == null) return null;
            if (row.Length == 0) return null;

            try
            {
                XmlDocument xNodeIn = new XmlDocument();
                xNodeIn.LoadXml(row);
                return CCboMenuNode.Construct(xNodeIn.DocumentElement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static CCboMenuNode Construct(CBO.UserSelectsRow cboRow)
        {
            CCboMenuNode retval = null;
            retval = new CGenericCboNode(null, cboRow);
            return retval;
        }

        /// <summary>
        /// Render the node as a <li> html tag
        /// </summary>
        /// <param name="writer"></param>
        public virtual void RenderListItem(HtmlTextWriter htw, bool HasChildren, bool isFirst, bool isLast)
        {
            string id = LI_CBO + TreeUniqueId;
            string pid = "cbo" + TreeUniqueId;

            htw.AddAttribute("id", id);

            if (isHighlightedGroup)
                htw.AddAttribute("class", "highlight");

            htw.RenderBeginTag(HtmlTextWriterTag.Li);



            // the plus/ minus or dash images, implemented as classes
            if (HasChildren) // only put script if there are child nodes
            {
                htw.AddAttribute("href", "#");
                htw.AddAttribute("name", id);
                htw.AddAttribute("onclick", "o" + ParentControlID + ".toggle('" + NamingContainer + "_', '" + pid + "')");

                htw.RenderBeginTag(HtmlTextWriterTag.A);

                //div placeholder for the plus , minus and dash images

                if (isExpanded)
                    htw.AddAttribute("class", "minus");
                else
                    htw.AddAttribute("class", "plus");

                htw.AddAttribute("id", "img_" + id);

                htw.RenderBeginTag(HtmlTextWriterTag.Div);

                htw.RenderEndTag(); // img
                htw.RenderEndTag(); // A
            }
            else
            {

                //div placeholder for the plus , minus and dash images
                string szClass = "dash";

                if (isFirst)
                    szClass = "dash_first";
                if (isLast)
                    szClass = "dash_last";

                htw.AddAttribute("class", szClass);

                htw.AddAttribute("id", "img_" + id);
                htw.RenderBeginTag(HtmlTextWriterTag.Div);
                htw.RenderEndTag(); // img
            }

            string szTagText = CCboMenuNode.deXml(XmlNode.OuterXml);

            // the node icon
            //if you can remove the item generate an icon with some script to remove
            if (CanRemove)
            {
                //Peet van der Walt - Escaping the ' in the text of the name
                string RemoveScript = "o" + ParentControlID + ".onCboRemove('" + NamingContainer + "_','" + szTagText + "')";

                htw.AddAttribute("href", "#");
                htw.AddAttribute("onclick", RemoveScript);
                htw.RenderBeginTag(HtmlTextWriterTag.A);


                htw.AddAttribute("src", ImagesPath + ImageUrl);
                htw.AddAttribute("class", "removable");
                htw.AddAttribute("alt", "Remove Item");
                htw.AddAttribute("onmouseover", "this.src='" + ImagesPath + CBO_REMOVE_URL  + "';");
                htw.AddAttribute("onmouseout", "this.src='" + ImagesPath + ImageUrl  + "';");
                htw.RenderBeginTag(HtmlTextWriterTag.Img);
                htw.RenderEndTag(); // img


                htw.RenderEndTag(); //a

            }
            else
            {
                htw.AddAttribute("src", ImagesPath + ImageUrl);
                htw.RenderBeginTag(HtmlTextWriterTag.Img);
                htw.RenderEndTag(); // img
            }

            //Malcolm TODO
            //add the Origination Source icon if required
            string OrigSourceIcon = "";

            switch (OriginationSource)
            {
                case -1:
                    OrigSourceIcon = ""; break;
                case 4: 
                    OrigSourceIcon = "RCSIcon.gif"; break;
                default: 
                    OrigSourceIcon = "SAHLIcon.gif"; break;
            }
            
            if (OrigSourceIcon!="")
            {
                htw.AddAttribute("src", ImagesPath + OrigSourceIcon);
                htw.RenderBeginTag(HtmlTextWriterTag.Img);
                htw.RenderEndTag(); // img
            }


            // Write text description with click event to change selected item
            string onClickText = "o" + ParentControlID + ".onCboClick('" + NamingContainer + "_','" + szTagText + "')";
            htw.AddAttribute("href", "#");
            htw.AddAttribute("title", LongDescription);
            if (isSelectedItem)
            {
                htw.AddAttribute("name", "cbo");
                htw.AddAttribute("class", "selected");

            }
            else // don' generate the onclick for the selected node
                htw.AddAttribute("onclick", onClickText);

            htw.RenderBeginTag(HtmlTextWriterTag.A);
            
            htw.Write(ShortDescription);
            
            htw.RenderEndTag(); //A

        }
        
        /// <summary>
        /// Render the node as as <ul> html tag , without closing the tag
        /// </summary>
        /// <param name="writer"></param>
        public virtual void RenderListHeader(HtmlTextWriter htw)
        {
            string id = UL_CBO + TreeUniqueId;

            htw.AddAttribute("id", id);
            if (isExpanded)
                htw.AddAttribute("class", "open");
            else
                htw.AddAttribute("class", "closed");

            htw.RenderBeginTag(HtmlTextWriterTag.Ul);
        }

        /// <summary>
        /// Utility function to chage the xml so is doesn't look like html to ASP as you may not post html like text 
        /// to an asp page
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        public static string deXml(string Xml)
        {
            if (Xml.StartsWith ("<"))
                Xml = Xml.Substring(1);

            Xml = Xml.Replace("\"", "$");
            //Peet van der Walt - Escaping the ' in the text of the name
            Xml = Xml.Replace("'", @"\'");
            return Xml;
        }

        /// <summary>
        /// Reverse the what was done in the deHtml methid
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        public static string reXml(string Xml)
        {
            if (Xml.Length > 0)
            {
                Xml = "<" + Xml;
                Xml = Xml.Replace("$", "\"");
                //Peet van der Walt - Reversing Escaping the ' in the text of the name
                Xml = Xml.Replace(@"'\", "'");
            }
            return Xml;
        }


    }

    public class CGenericCboNode : CCboMenuNode
    {
        public override string MyUniqueID
        {
            get { return ((char)NodeType).ToString() + UserSelectsKey.ToString(); }
        }

        public override CBO.UserSelectsRow UserSelectsRow
        {
            get
            {
                CBO cboDummy = new CBO();
                CBO.UserSelectsRow retval = cboDummy.UserSelects.NewUserSelectsRow();

                retval.DynamicDescription = ShortDescription;
                retval.CoreBusinessObjectKey = CoreBusinessObjectKey;
                retval.ParentKey = ParentKey;
                retval.LongDescription = LongDescription;
                retval.GenericKey = GenericKey;
                retval.NodeType = NodeType.ToString();
                retval.NodeID = NodeId;
                retval.GenericTypeKey = GenericTypeKey;
                retval.UserSelectsKey = UserSelectsKey;
                retval.URL = Url;
                retval.IsExpanded = this.isExpanded;
                retval.IsRemovable = this.CanRemove;
                retval.IncludeParentHeaderIcons = this.includeParentIcons;
                

                return retval;
            }
        }

        public override XmlNode XmlNode
        {
            get
            {
                XmlDocument retval = new XmlDocument();

                retval.AppendChild(retval.ImportNode(base.XmlNode, true));

                return retval.DocumentElement;
            }
        }

        public override void WriteXmlNode(XmlTextWriter xtw)
        {
            base.WriteXmlNode(xtw);
        }

        public override bool Equals(CCboMenuNode compare, bool BeStrict)
        {
            bool retval = base.Equals(compare, BeStrict);

            if (retval)
            {
                CGenericCboNode compareThis = compare as CGenericCboNode;
                if (BeStrict)
                {
                    if (this.TreeUniqueId.Length > 0)
                        if (compareThis.TreeUniqueId != this.TreeUniqueId) return false;
                }

            }

            return retval;
        }

        /// <summary>
        /// Construct using a CoreBusinessObjectMenuRow
        /// </summary>
        /// <param name="row"></param>
        public CGenericCboNode(CCboMenuNode parentNode, CBO.UserSelectsRow userRow)
            : base (parentNode, userRow)
        {

            if (parentNode != null)
            {
                TreeUniqueId = parentNode.TreeUniqueId + "_" + MyUniqueID;
                Level = parentNode.Level + 1;
            }
            else
            {
                TreeUniqueId = MyUniqueID;
                Level = 0;
            }


        }

        public CGenericCboNode()
            : base()
        {
        }

        public CGenericCboNode(XmlNode node)
            : base(node)
        {
            LoadFromNode(node);
        }

        public CGenericCboNode(string row)
            : base(row)
        {
            XmlDocument xNodeIn = new XmlDocument();
            LoadFromNode(xNodeIn.DocumentElement);
        }

        private void LoadFromNode(XmlNode node)
        {

        }
    }

    public class CWorkflowInstanceNode : CCboMenuNode
    {
        public const string c_InstanceID = "fid";
        public const string c_WorkflowName = "mpn";
        public const string c_StateName = "sgn";
        public const string c_Subject = "sub";
        public const string c_InstanceName = "fon";

        private string m_InstanceID;
        private string m_WorkflowName;
        private string m_StateName;
        private string m_Subject;
        private string m_InstanceName;

        public string InstanceID
        {
            get { return m_InstanceID; }
            set { m_InstanceID = value; }
        }

        public string WorkflowName
        {
            get { return m_WorkflowName; }
            set { m_WorkflowName = value; }
        }

        public string StateName
        {
            get { return m_StateName; }
            set { m_StateName = value; }
        }

        public string Subject
        {
            get {return m_Subject;}
            set {m_Subject = value;}
        }

        public string InstanceName
        {
            get { return m_InstanceName; }
            set { m_InstanceName = value; }
        }

        public override string MyUniqueID
        {
            get { return ((char)NodeType).ToString() + UserSelectsKey.ToString(); }
        }

        public override CBO.UserSelectsRow UserSelectsRow
        {
            get
            {
                CBO cboDummy = new CBO();
                CBO.UserSelectsRow retval = cboDummy.UserSelects.NewUserSelectsRow();

                retval.DynamicDescription = ShortDescription;
                retval.CoreBusinessObjectKey = CoreBusinessObjectKey;
                retval.ParentKey = ParentKey;
                retval.LongDescription = LongDescription;
                retval.GenericKey = GenericKey;
                retval.NodeType = NodeType.ToString();
                retval.NodeID = NodeId;
                retval.GenericTypeKey = GenericTypeKey;
                retval.UserSelectsKey = UserSelectsKey;
                retval.URL = Url;
                retval.IsExpanded = this.isExpanded;
                retval.IsRemovable = this.CanRemove;
                retval.IncludeParentHeaderIcons = this.includeParentIcons;


                return retval;
            }
        }

        public override XmlNode XmlNode
        {
            get
            {
                XmlDocument retval = new XmlDocument();

                retval.AppendChild(retval.ImportNode(base.XmlNode, true));

                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_InstanceID));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_WorkflowName));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_StateName));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_Subject));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_InstanceName));

                retval.DocumentElement.Attributes[c_InstanceID].Value = InstanceID.Trim();
                retval.DocumentElement.Attributes[c_WorkflowName].Value = WorkflowName.Trim();
                retval.DocumentElement.Attributes[c_StateName].Value = StateName.Trim();
                retval.DocumentElement.Attributes[c_Subject].Value = Subject.Trim();
                retval.DocumentElement.Attributes[c_InstanceName].Value = InstanceName.Trim();


                return retval.DocumentElement;
            }
        }

        public override void WriteXmlNode(XmlTextWriter xtw)
        {
            base.WriteXmlNode(xtw);

            xtw.WriteAttributeString(c_InstanceID, InstanceID.Trim());
            xtw.WriteAttributeString(c_WorkflowName, WorkflowName.Trim());
            xtw.WriteAttributeString(c_StateName, StateName.Trim());
            xtw.WriteAttributeString(c_Subject, Subject.Trim());
            xtw.WriteAttributeString(c_InstanceName, InstanceName.Trim());


        }

        public override bool Equals(CCboMenuNode compare, bool BeStrict)
        {
            bool retval = base.Equals(compare, BeStrict);

            if (retval)
            {
                CWorkflowInstanceNode compareThis = compare as CWorkflowInstanceNode;
                if (compareThis == null) return false;

                if (BeStrict)
                {
                    if (this.TreeUniqueId.Length > 0)
                        if (compareThis.TreeUniqueId != this.TreeUniqueId) return false;
                }
                if (compareThis.InstanceID != this.InstanceID) return false;
                if (compareThis.WorkflowName != this.WorkflowName) return false;
                if (compareThis.StateName != this.StateName) return false;
                if (compareThis.Subject != this.Subject) return false;
                if (compareThis.InstanceName != this.InstanceName) return false;

            }

            return retval;
        }

        /// <summary>
        /// Construct using a CoreBusinessObjectMenuRow and WorkFlowInstanceInfoRow
        /// </summary>
        /// <param name="row"></param>
        public CWorkflowInstanceNode(CCboMenuNode parentNode, CBO.UserSelectsRow userRow, CBO.WorkFlowInstanceInfoRow wflRow)
            : base(parentNode, userRow)
        {

            InstanceID = wflRow.InstanceID.ToString();
            WorkflowName = wflRow.WorkFlowName;

            //if (wflRow.isi IseFolderNameNull())
            //    FolderName = "";
            //else
            InstanceName = wflRow.InstanceName;

            //if (wflRow.issIseStageNameNull())
            //    StageName = "";
            //else
            StateName = wflRow.StateName;

            if (wflRow.IsSubjectNull())
                Subject = "";
            else
                Subject = wflRow.Subject;


            ShortDescription = Subject;

            LongDescription = WorkflowName + "\\" + StateName + "\\" + InstanceName;


            if (parentNode != null)
            {
                TreeUniqueId = parentNode.TreeUniqueId + "_" + MyUniqueID;
                Level = parentNode.Level + 1;
            }
            else
            {
                TreeUniqueId = MyUniqueID;
                Level = 0;
            }

        }

        public CWorkflowInstanceNode()
            : base()
        {
            m_InstanceID = "";
            m_WorkflowName = "";;
            m_StateName = "";
            m_Subject = "";
            m_InstanceName = "";
        }

        public CWorkflowInstanceNode(XmlNode node)
            : base(node)
        {
            LoadFromNode(node);
        }

        public CWorkflowInstanceNode(string row)
            : base(row)
        {
            XmlDocument xNodeIn = new XmlDocument();
            LoadFromNode(xNodeIn.DocumentElement);
        }

        private void LoadFromNode(XmlNode node)
        {
            if (node.Attributes[c_InstanceID] != null)
                InstanceID = node.Attributes[c_InstanceID].Value.Trim();
            if (node.Attributes[c_WorkflowName] != null)
                WorkflowName = node.Attributes[c_WorkflowName].Value.Trim();
            if (node.Attributes[c_StateName] != null)
                StateName = node.Attributes[c_StateName].Value.Trim();
            if (node.Attributes[c_Subject] != null)
                Subject = node.Attributes[c_Subject].Value.Trim();
            if (node.Attributes[c_InstanceName] != null)
                InstanceName = node.Attributes[c_InstanceName].Value.Trim();
        }
    }
    
    
    #endregion


    #region ContextMenu Classes
    public abstract class CContextMenuNode
    {
        public const string LI_CTX = "li_ctx";
        public const string UL_CTX = "ul_ctx";

        public enum ContextMenuTypes
        {
            General,
            Workflow
        }

        public static string CTX_NODE_NAME = "CtxNode";

        public const string c_UniqueId = "ui";
        public const string c_NodeType = "nt";
        public const string c_SelectedItem = "si";
        public const string c_ContextKey = "ck";
        public const string c_Description = "ds";
        public const string c_Url = "ur";
        public const string c_isExpanded = "ie";
        public const string c_Visible = "vis";

        //Data
        private string m_UniqueID;
        private int m_ContextKey;
        private string m_Description;
        private string m_Url;

        // Appearance
        private bool m_isExpanded;
        private bool m_SelectedItem;

        // used for rendering, must be set up by the call that want me to render
        private string m_ImagesPath = "";
        private string m_NamingContainer = "";
        private string m_ParentControlID = "";
        private bool m_Visible = true;


        public abstract string MyUniqueID
        {
            get;
        }

        public string TreeUniqueId
        {
            get { return m_UniqueID; }
            set { m_UniqueID = value; }
        }


        public bool isSelectedItem
        {
            get { return m_SelectedItem; }
            set { m_SelectedItem = value; }
        }

        public int ContextKey
        {
            get { return m_ContextKey; }
            set { m_ContextKey = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }

        public string ImagesPath
        {
            get { return m_ImagesPath; }
            set { m_ImagesPath = value; }
        }

        public string NamingContainer
        {
            get { return m_NamingContainer; }
            set { m_NamingContainer = value; }
        }

        public string ParentControlID
        {
            get { return m_ParentControlID; }
            set { m_ParentControlID = value; }
        }

        public bool isExpanded
        {
            get { return m_isExpanded; }
            set { m_isExpanded = value; }
        }

        /// <summary>
        /// Gets/sets whether to display the context menu node at run time.  Some nodes need to 
        /// be created in the menu so we know they exists, but not displayed here (for example 
        /// workflow form items).
        /// </summary>
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }


        public abstract ContextMenuTypes NodeType
        {
            get;
        }

        
        public virtual void WriteXmlNode(XmlTextWriter xtw)
        {
            xtw.WriteAttributeString(c_NodeType, ((int)NodeType).ToString());
            xtw.WriteAttributeString(c_UniqueId, TreeUniqueId);
            if (isSelectedItem) xtw.WriteAttributeString(c_SelectedItem, "1");
            xtw.WriteAttributeString(c_ContextKey, ContextKey.ToString());
            xtw.WriteAttributeString(c_Description, Description.Trim());
            xtw.WriteAttributeString(c_Url, Url.Trim());
            xtw.WriteAttributeString(c_Visible, (Visible ? "1" : "0"));
            if (isExpanded) xtw.WriteAttributeString(c_isExpanded, "1"); else xtw.WriteAttributeString(c_isExpanded, "0");
            
        }

        public abstract CBO.ContextMenuRow ContextMenuRow
        {
            get;
        }

        public virtual bool Equals(CContextMenuNode compareTo)
        {
            if (compareTo == null) return false;
            if (compareTo.ContextKey != this.ContextKey) return false;
            if (compareTo.Description != this.Description) return false;
            if (compareTo.NodeType != this.NodeType) return false;
            if (compareTo.Url != this.Url) return false;
            if (compareTo.Visible != this.Visible) return false;
            return true;
        }
        protected CContextMenuNode()
        {
            m_UniqueID = "";
            isSelectedItem = false;
            ContextKey = -1;
            Description = "";
            Url = "";
        }

        public CContextMenuNode(XmlNode node) 
        {
            m_UniqueID = "";
            isSelectedItem = false;
            ContextKey = -1;
            Description = "";
            Url = "";
            isExpanded = false;
            LoadFromNode(node);
        }

        public CContextMenuNode(string row)
        {
            m_UniqueID = "";
            isSelectedItem = false;
            ContextKey = -1;
            Description = "";
            Url = "";
            m_isExpanded = false;
            XmlDocument xNodeIn = new XmlDocument();
            xNodeIn.LoadXml(row);
            LoadFromNode(xNodeIn.DocumentElement);
        }


        private void LoadFromNode(XmlNode node)
        {
            if (node.Attributes[c_UniqueId] != null)
                m_UniqueID = node.Attributes[c_UniqueId].Value;

            if (node.Attributes[c_SelectedItem] != null)
                if (node.Attributes[c_SelectedItem].Value == "1") isSelectedItem = true;

            if (node.Attributes[c_ContextKey] != null)
                ContextKey = int.Parse(node.Attributes[c_ContextKey].Value);

            if (node.Attributes[c_Description] != null)
                Description = node.Attributes[c_Description].Value.Trim();

            if (node.Attributes[c_Url] != null)
                Url = node.Attributes[c_Url].Value.Trim();

            if (node.Attributes[c_isExpanded] != null)
                if (node.Attributes[c_isExpanded].Value == "1")
                    m_isExpanded = true;

            if (node.Attributes[c_Visible] != null)
                m_Visible = (node.Attributes[c_Visible].Value == "1");

        }

        public static CContextMenuNode Construct(XmlNode node)
        {
            CContextMenuNode retval = null;

            if (node.Attributes[c_NodeType] != null)
            {
                int iNodeType = int.Parse(node.Attributes[c_NodeType].Value);

                if (iNodeType == (int)ContextMenuTypes.General)
                    retval = new CGenericCtxMenu(node);
                if (iNodeType == (int)ContextMenuTypes.Workflow)
                    retval = new CWorkflowCtxMenu(node);
            }

            return retval;
        }

        public static CContextMenuNode Construct(string row)
        {
            // the input string is XML
            if (row == null) return null;
            if (row.Length == 0) return null;

            try
            {
                XmlDocument xNodeIn = new XmlDocument();
                xNodeIn.LoadXml(row);
                return CContextMenuNode.Construct(xNodeIn.DocumentElement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // return null;
        }

        public virtual void RenderListItem(HtmlTextWriter htw, bool HasChildren, bool isFirst, bool isLast)
        {
            string id = LI_CTX + TreeUniqueId;
            string pid = "ctx" + TreeUniqueId;

            htw.AddAttribute("id", id);
            htw.RenderBeginTag(HtmlTextWriterTag.Li);

            //if (isSelectedItem)
            //{
            //    htw.AddAttribute("src", ImagesPath + "pixel.gif");
            //    htw.AddAttribute("id", "ctx_focus_me");
            //    htw.AddAttribute("style", "display:none;");
            //    htw.RenderBeginTag(HtmlTextWriterTag.Img);
            //    htw.RenderEndTag();
            //}

            if (HasChildren) // only put script if there are child nodes
            {

                htw.AddAttribute("href", "#");
                htw.AddAttribute("name", id);
                htw.AddAttribute("onclick", "o" + ParentControlID + ".toggle('" + NamingContainer + "_', '" + pid + "')");
                htw.RenderBeginTag(HtmlTextWriterTag.A);

                //div placeholder for the plus , minus and dash images
                if (isExpanded)
                    htw.AddAttribute("class", "minus");
                else
                    htw.AddAttribute("class", "plus");

                htw.AddAttribute("id", "img_" + id);
                htw.RenderBeginTag(HtmlTextWriterTag.Div);

                htw.RenderEndTag(); //img
                htw.RenderEndTag(); //a
            }
            else
            {
                //div placeholder for the plus , minus and dash images
                string szClass = "dash";

                if (isFirst)
                    szClass = "dash_first";
                if (isLast)
                    szClass = "dash_last";

                htw.AddAttribute("class", szClass);
                htw.AddAttribute("id", "img_" + id);
                htw.RenderBeginTag(HtmlTextWriterTag.Div);

                htw.RenderEndTag(); //img
            }


            string szTagText = CCboMenuNode.deXml(XmlNode.OuterXml);
            string onClickText = "o" + ParentControlID + ".onCtxClick('" + NamingContainer + "_','" + szTagText + "')";

            htw.AddAttribute("href", "#");

            if (isSelectedItem)
            {
                htw.AddAttribute("name", "ctx");
                htw.AddAttribute("class", "selected");
                htw.AddAttribute("ondblclick", onClickText);

            }
            else // don't generate onclick 
                htw.AddAttribute("onclick", onClickText);

            htw.RenderBeginTag(HtmlTextWriterTag.A);
            
            htw.Write(Description);
            
            htw.RenderEndTag(); //a

        }

        public virtual void RenderListHeader(HtmlTextWriter htw)
        {
            string id = UL_CTX + TreeUniqueId;

            htw.AddAttribute("id", id);

            if (isExpanded)
                htw.AddAttribute("class", "open");
            else
                htw.AddAttribute("class", "closed");

            htw.RenderBeginTag(HtmlTextWriterTag.Ul);

        }

        /// <summary>
        /// Return the node as a set of pipe delimited values that can be used to uniquely identify the node
        /// </summary>
        /// <returns></returns>
        public virtual XmlNode XmlNode
        {
            get
            {
                XmlDocument retval = new XmlDocument();

                retval.LoadXml("<" + CTX_NODE_NAME + "/>");

                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_NodeType));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_UniqueId));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_SelectedItem));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_ContextKey));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_Description));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_Url));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_isExpanded));
                


                retval.DocumentElement.Attributes[c_NodeType].Value = ((int)NodeType).ToString();
                retval.DocumentElement.Attributes[c_UniqueId].Value = TreeUniqueId;
                if (isSelectedItem) retval.DocumentElement.Attributes[c_SelectedItem].Value = "1";
                if (isExpanded) retval.DocumentElement.Attributes[c_isExpanded].Value = "1";
                else retval.DocumentElement.Attributes[c_isExpanded].Value = "0";

                retval.DocumentElement.Attributes[c_ContextKey].Value = ContextKey.ToString();
                retval.DocumentElement.Attributes[c_Description].Value = Description.Trim();
                retval.DocumentElement.Attributes[c_Url].Value = Url.Trim();

                return retval.DocumentElement;
            }
        }

    }

    public class CGenericCtxMenu : CContextMenuNode
    {
        public const string c_CoreBusinessObjectKey = "cb";
        public const string c_ParentKey = "pk";

        private int m_CoreBusinessObjectKey;
        private int m_ParentKey;

        public int CoreBusinessObjectKey
        {
            get { return m_CoreBusinessObjectKey; }
            set { m_CoreBusinessObjectKey = value; }
        }

        public int ParentKey
        {
            get { return m_ParentKey; }
            set { m_ParentKey = value; }
        }

        public override string MyUniqueID
        {
            get { return "C" + ContextKey.ToString(); }
        }

        public override ContextMenuTypes NodeType
        {
            get { return ContextMenuTypes.General; }
        }



        public override void WriteXmlNode(XmlTextWriter xtw)
        {
            base.WriteXmlNode(xtw);
            xtw.WriteAttributeString(c_CoreBusinessObjectKey, CoreBusinessObjectKey.ToString());
            xtw.WriteAttributeString(c_ParentKey, ParentKey.ToString());
        }

        public override CBO.ContextMenuRow ContextMenuRow
        {
            get
            {
                CBO cboDummy = new CBO();
                CBO.ContextMenuRow ctxRow = cboDummy.ContextMenu.NewContextMenuRow();

                ctxRow.ContextKey = ContextKey;
                ctxRow.CoreBusinessObjectKey = CoreBusinessObjectKey;
                ctxRow.ParentKey = ParentKey;
                ctxRow.Description = Description;
                ctxRow.URL = Url;
                return ctxRow;
            }
        }

        public override XmlNode  XmlNode
        {
            get
            {
                XmlDocument retval = new XmlDocument();

                retval.AppendChild(retval.ImportNode(base.XmlNode, true));

                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_CoreBusinessObjectKey));
                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_ParentKey));

                retval.DocumentElement.Attributes[c_CoreBusinessObjectKey].Value = CoreBusinessObjectKey.ToString(); ;
                retval.DocumentElement.Attributes[c_ParentKey].Value = ParentKey.ToString();

                return retval.DocumentElement;
            }
        }


        public CGenericCtxMenu(XmlNode node) 
            : base(node)
        {
            LoadFromNode(node);
        }

        public CGenericCtxMenu(string row)
            : base(row)
        {
            XmlDocument xNodeIn = new XmlDocument();
            xNodeIn.LoadXml(row);
            LoadFromNode(xNodeIn);
            
        }

        private void LoadFromNode(XmlNode node)
        {
            if (node.Attributes[c_CoreBusinessObjectKey] != null)
                CoreBusinessObjectKey = int.Parse(node.Attributes[c_CoreBusinessObjectKey].Value);
            if (node.Attributes[c_ParentKey] != null)
                ParentKey = int.Parse(node.Attributes[c_ParentKey].Value);
        }

        public CGenericCtxMenu(CContextMenuNode parentNode, CBO.ContextMenuRow row) 
            : base()
        {

            ContextKey = row.ContextKey;
            Description = row.Description;

            if (row.IsCoreBusinessObjectKeyNull())
                CoreBusinessObjectKey = -1;
            else
                CoreBusinessObjectKey = row.CoreBusinessObjectKey;

            if (row.IsParentKeyNull())
                ParentKey = -1;
            else
                ParentKey = row.ParentKey;

            if (row.IsURLNull())
                Url = "";
            else
                Url = row.URL;

            if (parentNode != null)
                TreeUniqueId = parentNode.TreeUniqueId + "_" + MyUniqueID;
            else
                TreeUniqueId = MyUniqueID;
        }
    }

    public class CWorkflowCtxMenu : CContextMenuNode
    {

        private static string c_UserSelectsKey = "UserSelectsKey";

        private int m_UserSelectsKey;

        public int UserSelectsKey
        {
            get { return m_UserSelectsKey; }
            set { m_UserSelectsKey = value; }
        }

        public override string MyUniqueID
        {
            get { return "W" + ContextKey.ToString(); }
        }

        public override ContextMenuTypes NodeType
        {
            get { return ContextMenuTypes.Workflow; }
        }

        public override void WriteXmlNode(XmlTextWriter xtw)
        {
            base.WriteXmlNode(xtw);
            xtw.WriteAttributeString(c_UserSelectsKey, UserSelectsKey.ToString());
        }

        public override CBO.ContextMenuRow ContextMenuRow
        {
            get
            {
                CBO cboDummy = new CBO();
                CBO.ContextMenuRow ctxRow = cboDummy.ContextMenu.NewContextMenuRow();

                ctxRow.ContextKey = UserSelectsKey;
                ctxRow.CoreBusinessObjectKey = -1;
                ctxRow.ParentKey = -1;
                ctxRow.Description = Description;
                ctxRow.URL = Url;
                return ctxRow;
            }
        }

        public CBO.WorkflowContextMenuRow WorkflowContextMenuRow
        {
            get
            {
                CBO cboDummy = new CBO();
                CBO.WorkflowContextMenuRow wflRow = cboDummy.WorkflowContextMenu.NewWorkflowContextMenuRow();

                wflRow.Description = this.Description;
                wflRow.UserSelectsKey = this.UserSelectsKey;
                wflRow.URL = this.Url;
                return wflRow;
            }
        }
        public override XmlNode XmlNode
        {
            get
            {
                XmlDocument retval = new XmlDocument();

                retval.AppendChild(retval.ImportNode(base.XmlNode, true));

                retval.DocumentElement.Attributes.Append(retval.CreateAttribute(c_UserSelectsKey));

                retval.DocumentElement.Attributes[c_UserSelectsKey].Value = UserSelectsKey.ToString(); ;

                return retval.DocumentElement;
            }
        }


        public CWorkflowCtxMenu(CContextMenuNode parentNode, CBO.WorkflowContextMenuRow row) 
            : base()
        {
            ContextKey = row.UserSelectsKey;
            UserSelectsKey = row.UserSelectsKey;
            Description = row.Description;
            Url = row.URL;

            if (parentNode != null)
                TreeUniqueId = parentNode.TreeUniqueId + "_" + MyUniqueID;
            else
                TreeUniqueId = MyUniqueID;
        }

        public CWorkflowCtxMenu(XmlNode node) 
            : base(node)
        {
            LoadFromNode(node);
        }

        public CWorkflowCtxMenu(string row) 
            : base(row)
        {
            XmlDocument xNodeIn = new XmlDocument();
            xNodeIn.LoadXml(row);
            LoadFromNode(xNodeIn);
        }

        private void LoadFromNode(XmlNode node)
        {
            if (node.Attributes[c_UserSelectsKey] != null)
                UserSelectsKey = int.Parse(node.Attributes[c_UserSelectsKey].Value);

        }
    }
    #endregion

}
