using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Events;
using System.Collections;

namespace SAHL.Common.Web.UI.Controls
{

    /// <summary>
    /// Specialised tree control for SAHL.  This is NOT a feature-complete control, but has been written 
    /// specifically for SAHL requirements.  There is no designer support, and nodes need to be created and 
    /// added at RUN time.  It is the view's responsibility to populate the tree view with data after each page 
    /// request, as the tree has been created with the assumption that there is no available ViewState.  The 
    /// <see cref="ExpandedValues"/> collection can be used to determine which nodes were expanded before 
    /// post back, although expanded nodes will render expanded after the postback.  
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SAHLTreeView runat=server></{0}:SAHLTreeView>")]
    public class SAHLTreeView : SAHLWebControl, IPostBackEventHandler
    {

        #region Private Attributes

        private SAHLTreeNodeCollection _lstNodes = null;
        private HiddenField _hidExpanded = null;
        //private HiddenField _hidAjaxMethod = null;
        //private HiddenField _hidPostBackMethod = null;
        //private string _populateNodesMethod = "";
        private char _nodeValueSeparator = '/';
        private SAHLTreeNode _selectedNode = null;
        private List<String> _lstExpandedNodes = null;
        private List<string> _lstUnique = new List<string>();
        private bool _maintainVerticalScrollPosition = true;
        private StringBuilder _expandedValues = new StringBuilder();
        private bool _checkBoxesVisible;
        private List<string> _checkedValuePaths = new List<string>();
        private List<string> _checkedValues = new List<string>();
        private string _imagePath = "~/Images/Tree/";
        private string _scriptPath = "~/Scripts/SAHLTreeView.js";
        private string _cssPath = "~/CSS/SAHLTreeView.css";

        private const string EventText = "TEXT_";
        private const string EventImage = "IMG_";

        public event SAHLTreeNodeEventHandler NodeSelected;
        public event SAHLTreeNodeEventHandler NodeIconClicked;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public SAHLTreeView()
        {
            _lstNodes = new SAHLTreeNodeCollection(null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name used for check boxes - this is unique to the tree control.
        /// </summary>
        public string CheckBoxFormName
        {
            get
            {
                return "chk" + this.ClientID;
            }
        }

        /// <summary>
        /// Gets/sets whether to display checkboxes next to each node.  Note that if this is 
        /// false, checkboxes can also be displayed on individual nodes using the CheckBoxVisible 
        /// property on the SAHLTreeNode.
        /// </summary>
        public bool CheckBoxesVisible
        {
            get
            {
                return _checkBoxesVisible;
            }
            set
            {
                _checkBoxesVisible = value;
            }
        }

        /// <summary>
        /// Gets a list of value paths of nodes that had their check boxes selected.  
        /// </summary>
        public IList<string> CheckedValuePaths
        {
            get
            {
                return _checkedValuePaths;
            }
        }

        /// <summary>
        /// Gets a list of values of nodes that had their check boxes selected.  
        /// </summary>
        public IList<string> CheckedValues
        {
            get
            {
                return _checkedValues;
            }
        }

        /// <summary>
        /// Provides a list of all nodes that were expanded before the last postback occurred.  This list contains 
        /// a collection node <see cref="SAHLTreeNode.ValuePath">node ValuePaths</see>.
        /// </summary>
        public List<String> ExpandedValues
        {
            get
            {
                if (_lstExpandedNodes == null)
                {
                    _lstExpandedNodes = new List<string>();
                    string sExpandedValues = Page.Request.Form[_hidExpanded.UniqueID];
                    if (sExpandedValues != null && sExpandedValues.Length > 0)
                    {
                        char[] separators = { '|' };
                        string[] arrValues = sExpandedValues.Split(separators);
                        foreach (string val in arrValues)
                        {
                            if (val.Length > 0 && !_lstExpandedNodes.Contains(val))
                                _lstExpandedNodes.Add(val);
                        }
                    }
                }
                return _lstExpandedNodes;
            }
        }

        /// <summary>
        /// Gets/sets the location of the image folder where the images used for the tree are stored.  This 
        /// defaults to ~/Images/Tree.
        /// </summary>
        /// <remarks>This was added in favour of web resources - web resources were bloating the output HTML too much due to large URLs.</remarks>
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                _imagePath = value;
            }
        }

        /// <summary>
        /// Gets/sets the location of the path to the script file for the tree.  This
        /// defaults to ~/Scripts/SAHLTreeView.js.
        /// </summary>
        /// <remarks>This was added in favour of web resources - web resources were bloating the output HTML too much due to large URLs.</remarks>
        public string ScriptPath
        {
            get
            {
                return _scriptPath;
            }
            set
            {
                _scriptPath = value;
            }
        }

        /// <summary>
        /// Gets/sets the location of the path to the css file for the tree.  This
        /// defaults to ~/Scripts/SAHLTreeView.css.
        /// </summary>
        /// <remarks>This was added in favour of web resources - web resources were bloating the output HTML too much due to large URLs.</remarks>
        public string CssPath
        {
            get
            {
                return _cssPath;
            }
            set
            {
                _cssPath = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the vertical scroll position of the tree should be maintained after postbacks.  This 
        /// defaults to true.
        /// </summary>
        public bool MaintainVerticalScrollPosition
        {
            get
            {
                return _maintainVerticalScrollPosition;
            }
            set
            {
                _maintainVerticalScrollPosition = value;
            }
        }

        /// <summary>
        /// Gets the collection of nodes assigned to the tree.
        /// </summary>
        public SAHLTreeNodeCollection Nodes
        {
            get
            {
                return _lstNodes;
            }
        }

        /// <summary>
        /// Get/sets the separator that is used to build up the node <see cref="NodeValueSeparator"/>.
        /// </summary>
        public char NodeValueSeparator
        {
            get
            {
                return _nodeValueSeparator;
            }
            set
            {
                _nodeValueSeparator = value;
            }
        }


        ///// <summary>
        ///// Gets/sets the method to be called to populate nodes via AJAX. This must be fully qualified, or by default 
        ///// the parent page of the control will be used as the type.
        ///// </summary>
        //public string PopulateNodesMethod
        //{
        //    get
        //    {
        //        return _populateNodesMethod;
        //    }
        //    set
        //    {
        //        _populateNodesMethod = value;
        //    }
        //}

        /// <summary>
        /// Gets/sets the currently selected node.
        /// </summary>
        public SAHLTreeNode SelectedNode
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


        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="NodeIconClicked"/> event.
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="index"></param>
        public void ClickIcon(SAHLTreeNode treeNode, int index)
        {
            if (NodeIconClicked != null)
                NodeIconClicked(this, new SAHLTreeNodeEventArgs(treeNode, index));
        }

        /// <summary>
        /// Finds a node with the specified <code>valuePath</code> in the tree.
        /// </summary>
        /// <param name="valuePath"></param>
        /// <returns></returns>
        public SAHLTreeNode FindNode(string valuePath)
        {
            return FindNode(this.Nodes, valuePath);
        }

        /// <summary>
        /// Finds a node with the specified <code>valuePath</code> in the supplied <see cref="SAHLTreeNodeCollection"/>.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="valuePath"></param>
        /// <returns></returns>
        private SAHLTreeNode FindNode(SAHLTreeNodeCollection nodes, string valuePath) 
        {
            SAHLTreeNode nodePartial = null;
            foreach (SAHLTreeNode node in nodes)
            {
                if (node.ValuePath == valuePath)
                    return node;

                // try for a partial match
                if (valuePath.StartsWith(node.ValuePath + node.ValueSeparator))
                    nodePartial = node;
            }

            if (nodePartial != null)
                return FindNode(nodePartial.Nodes, valuePath);
            else
                return null;
        }

        /// <summary>
        /// Gets the client ID of a single node (which are written out as Divs).
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetNodeClientID(SAHLTreeNode node)
        {
            return this.ClientID + "_" + node.ValuePath.Replace(node.ValueSeparator, '_');
        }

        ///// <summary>
        ///// Gets the full position path for a node (i.e. top/middle/bottom for the node and all it's parent 
        ///// nodes up the tree).  This is used so when AJAX calls are made, we can work out what icons to 
        ///// display when creating the new dynamic nodes.
        ///// </summary>
        ///// <param name="node"></param>
        ///// <returns></returns>
        //private string GetPositionPath(SAHLTreeNode node) 
        //{
        //    StringBuilder sb = new StringBuilder();
        //    while (node != null)
        //    {
        //        if (sb.Length > 0) sb.Insert(0, node.ValueSeparator);
        //        sb.Insert(0, ((int)node.Position).ToString());
        //        node = node.ParentNode;
        //    }
        //    return sb.ToString();
        //}

        /// <summary>
        /// Raises the <see cref="NodeSelected"/> event.
        /// </summary>
        /// <param name="treeNode"></param>
        public void SelectNode(SAHLTreeNode treeNode)
        {
            if (NodeSelected != null)
                NodeSelected(this, new SAHLTreeNodeEventArgs(treeNode));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!DesignMode)
            {
                // create the hidden field that is used to store expanded values
                _hidExpanded = new HiddenField();
                _hidExpanded.ID = this.ClientID + "_expanded";
                this.Controls.Add(_hidExpanded);

                // build the list of checked value paths received from post back
                string postedValues = this.Page.Request.Form[CheckBoxFormName];
                if (!String.IsNullOrEmpty(postedValues))
                {
                    string[] checkedValues = postedValues.Split(',');
                    foreach (string s in checkedValues)
                    {
                        _checkedValuePaths.Add(s);
                        if (s.IndexOf("/") > -1)
                            _checkedValues.Add(s.Substring(s.LastIndexOf("/") + 1));
                        else
                            _checkedValues.Add(s);
                    }
                }
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!DesignMode)
            {
                RegisterCommonScript();

                // add javascript to ensure the scroll position is maintained
                if (MaintainVerticalScrollPosition)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(SAHLTreeView),
                        "SAHLTreeView_" + this.UniqueID,
                        String.Format(@"SAHLTreeView_scrollToSelectedNode('{0}');" + Environment.NewLine, this.ClientID),
                        true);
                }


                // set the value of the hidden field that holds the Ajax method - if it is not fully qualified then 
                // use the parent Page's type
                //if (PopulateNodesMethod.IndexOf(".") == -1)
                //    _hidAjaxMethod.Value = this.Page.GetType().FullName + "." + PopulateNodesMethod;
                //else
                //    _hidAjaxMethod.Value = PopulateNodesMethod;
            }
        }

        /// <summary>
        /// Handles the postback event when a node is clicked.  Note that in order to receive notifications from 
        /// this event, the node that was clicked will need to be re-added to the tree BEFORE control events are fired 
        /// (generally by Page_Load).
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent(string eventArgument)
        {
            SAHLTreeNode node = null;
            if (eventArgument.StartsWith(EventText))
            {
                node = FindNode(eventArgument.Replace(EventText, ""));
                if (node != null)
                {
                    SelectedNode = node;
                    SelectNode(node);
                }
            }
            else if (eventArgument.StartsWith(EventImage))
            {
                // images pass event info in the following way: "IMG_<i>index</i>_<i>nodeValuePath</i>", where <i>index</i>
                // is the image index 
                eventArgument = eventArgument.Replace(EventImage, "");
                int loc = eventArgument.IndexOf("_");
                int index = Int32.Parse(eventArgument.Substring(0, loc));
                string nodeValuePath = eventArgument.Substring(loc + 1);
                node = FindNode(nodeValuePath);
                if (node != null)
                    ClickIcon(node, index);
 
            }

        }

        /// <summary>
        /// Registers common script files for the control.
        /// </summary>
        protected void RegisterCommonScript()
        {
            ClientScriptManager cs = Page.ClientScript;
            Type type = typeof(SAHLTreeView);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(type, "SAHLTreeViewScript"))
            {
                // javascript include
                cs.RegisterClientScriptInclude(type, "SAHLTreeViewScript", this.ResolveClientUrl(ScriptPath));

                // css include
                cs.RegisterClientScriptBlock(type, "SAHLTreeViewCss", "<link href=\"" + this.ResolveClientUrl(CssPath) + "\" type=\"text/css\" rel=\"stylesheet\">", false);

            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            NameValueCollection attributes = new NameValueCollection();
            foreach (string key in this.Attributes.Keys)
            {
                attributes.Add(key, this.Attributes[key]);
            }

            string cssClass = ("SAHLTreeView " + this.CssClass).Trim();
            BeginHtmlTag(writer, "div", cssClass, this.ClientID, attributes);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write("SAHLTreeView [" + UniqueID + "]");
                base.RenderContents(writer);
            }
            else
            {
                RenderNodes(writer, Nodes, true);
                writer.WriteLine();

                // add the hidden input controls
                _hidExpanded.Value = _expandedValues.ToString();
                _hidExpanded.RenderControl(writer);
                writer.WriteLine();
                //_hidAjaxMethod.RenderControl(writer);
                //writer.WriteLine();
                //_hidPostBackMethod.RenderControl(writer);
                //writer.WriteLine();
            }
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            EndHtmlTag(writer, "div");
        }

        /// <summary>
        /// Renders the expand image for a tree node.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="node"></param>
        private void RenderExpandImage(HtmlTextWriter writer, SAHLTreeNode node)
        {
            string imageName = "";
            string alt = "";
            string nodeClientId = GetNodeClientID(node);
            SAHLTreeNodePosition nodePosition = node.Position;
            string cssClass = "TreeImage";

            // determine the alt tag and which image to display if not blank
            if (node.HasChildren)
            {
                alt = (node.Expanded ? "Contract node" : "Expand node");
                imageName = "{0}_";
                cssClass += " Action";
            }
            else
            {
                imageName = "lines_";
            }

            if (nodePosition == SAHLTreeNodePosition.Only && node.ParentNode == null)
            {
                imageName += "single.gif";
            }
            else if (nodePosition == SAHLTreeNodePosition.Only && node.ParentNode != null)
            {
                imageName += "bottom.gif";
            }
            else if (nodePosition == SAHLTreeNodePosition.First && node.ParentNode == null)
            {
                imageName += "top.gif";
            }
            else if (nodePosition == SAHLTreeNodePosition.Last)
            {
                imageName += "bottom.gif";
            }
            else
            {
                imageName += "middle.gif";
            }

            // resolve the image to a web resource for page requests - for AJAX calls this is done on 
            // the client as there is no Page object
            imageName = String.Format(imageName, (node.Expanded ? "minus" : "plus"));
            string imageUrl = String.Format("{0}{1}", ImagePath, imageName);
            if (Page != null)
                imageUrl = ResolveClientUrl(imageUrl);

            writer.WriteLine();

            writer.Write("<td>");
            writer.WriteBeginTag("img");
            writer.WriteAttribute("src", imageUrl);
            writer.WriteAttribute("alt", alt);
            writer.WriteAttribute("width", "12");   // size is required to prevent initial rendering from jumping
            writer.WriteAttribute("height", "18");
            writer.WriteAttribute("id", nodeClientId + "_img");
            writer.WriteAttribute("class", cssClass);
            if (node.HasChildren)
                writer.WriteAttribute("onclick", "SAHLTreeView_toggleNode(this, '" + this.ClientID + "', '" + nodeClientId + "', '" + node.ValuePath + "', '" + _hidExpanded.ClientID + "')");

            writer.Write(HtmlTextWriter.SelfClosingTagEnd);

            writer.Write("</td>");

        }

        /// <summary>
        /// Renders node icons (if there are any).
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="node"></param>
        private void RenderIcons(HtmlTextWriter writer, SAHLTreeNode node)
        {
            for (int i=0; i<node.Icons.Count; i++)
            {
                SAHLTreeNodeIcon icon = node.Icons[i];
                string clientClickHandler = icon.ClientClickHandler;
                string cssClass = "TreeImage";
                writer.WriteLine();

                writer.Write("<td>");

                writer.WriteBeginTag("img");
                writer.WriteAttribute("src", icon.Icon);
                writer.WriteAttribute("alt", icon.AlternateText);
                writer.WriteAttribute("title", icon.AlternateText);
                if (icon.Icon != icon.HoverIcon && icon.HoverIcon.Length > 0)
                {
                    writer.WriteAttribute("onmouseover", "this.src='" + icon.HoverIcon + "'");
                    writer.WriteAttribute("onmouseout", "this.src='" + icon.Icon + "'");
                    cssClass += " Action";
                }
                if (icon.AutoPostBack)
                {
                    if (clientClickHandler.Length > 0) clientClickHandler += ";";
                    clientClickHandler += Page.ClientScript.GetPostBackClientHyperlink(this, EventImage + i.ToString() + "_" + node.ValuePath);
                }
                if (clientClickHandler.Length > 0)
                {
                    writer.WriteAttribute("onclick", clientClickHandler);
                    if (cssClass.IndexOf(" Action") == -1)
                        cssClass += " Action";
                }
                writer.WriteAttribute("class", cssClass);
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);

                writer.Write("</td>");

            }

        }

        /// <summary>
        /// Renders all the indent images for the node (up to but not including the expand image).
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="node"></param>
        private void RenderIndentImages(HtmlTextWriter writer, SAHLTreeNode node)
        {
            StringBuilder sbHtml = new StringBuilder();
            while ((node = node.ParentNode) != null)
            {
                string imageName = "line_vert.gif";
                if (node.Position == SAHLTreeNodePosition.Last || node.Position == SAHLTreeNodePosition.Only)
                {
                    imageName = "blank.gif";
                }

                string imageUrl = String.Format("{0}{1}", ImagePath, imageName);
                if (Page != null)
                    imageUrl = ResolveClientUrl(imageUrl);

                string html = Environment.NewLine + "<img src=\"{0}\" name=\"{1}\" width=\"12\" height=\"18\" alt=\"\" class=\"TreeImage\"/>";
                html = String.Format(html, imageUrl, imageName);
                html = "<td>" + html + "</td>";

                sbHtml.Insert(0, html);
            }
            writer.Write(sbHtml.ToString());

        }

        /// <summary>
        /// This is the primary function used for rendering the nodes in the tree. 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="node">The node to be rendered.</param>
        /// <param name="visible">Whether the node should initially be visible.</param>
        private void RenderNode(HtmlTextWriter writer, SAHLTreeNode node, bool visible)
        {
            // make sure a node with the same value path hasn't already been rendered - if so, we raise an exception
            if (_lstUnique.Contains(node.ValuePath))
                throw new Exception("Unable to add two nodes with the same value path [" + node.ValuePath + "]");
            else
                _lstUnique.Add(node.ValuePath);

            writer.WriteLine();
            writer.Indent++;

            // this is the outer div for the node
            NameValueCollection attributes = new NameValueCollection();
            if (!visible) attributes.Add("style", "display:none");
            BeginHtmlTag(writer, "div", ("TreeNode " + node.CssClass).Trim(), GetNodeClientID(node), attributes);
            
            // IE6 requires a table to prevent wrapping, so for now we're doing it this way
            writer.WriteLine();
            writer.Write("<table><tr>");

            // write the images for the node
            RenderIndentImages(writer, node);
            RenderExpandImage(writer, node);
            RenderIcons(writer, node);

            // write the check box image out - this is done as an image otherwise we get sizing issues 
            // on different browsers
            // add a check box if specified
            if (CheckBoxesVisible || node.CheckBoxVisible)
            {
                if (CheckedValuePaths.Contains(node.ValuePath))
                    node.CheckBoxSelected = true;

                writer.WriteLine();
                writer.Write("<td class=\"TreeCheckBox\">");
              //writer.Write(String.Format(@"<input type=""checkbox"" name=""{0}"" value=""{1}"" {2}/>", CheckBoxFormName, node.ValuePath, (node.CheckBoxSelected ? "checked " : "")));
                writer.Write(String.Format(@"<input type=""checkbox"" name=""{0}"" value=""{1}"" {2} {3}/>", CheckBoxFormName, node.ValuePath, (node.CheckBoxSelected ? "checked " : ""), (node.CheckBoxDisabled ? "disabled" : "")));
                writer.Write("</td>");
            }

            // write the text of the node out as a link button so we can get a post back
            string href = (node.AutoPostBack ? Page.ClientScript.GetPostBackClientHyperlink(this, EventText + node.ValuePath) : "#");
            writer.WriteLine();
            writer.Write("<td class=\"TreeText\">");

            if (!node.AutoPostBack && String.IsNullOrEmpty(node.OnClientClick))
            {
                writer.Write(node.Text);
            }
            else
            {
                writer.Write("<a" +
                    " href=\"" + href + "\"" +
                    ((node == SelectedNode) ? " class=\"SelectedNode\"" : "") +
                    " title=\"" + node.ToolTipText + "\"" +
                    " onclick=\"" + node.OnClientClick + "\">" +
                    node.Text +
                    "</a>");
            }

            writer.Write("</td>");
            writer.WriteLine();

            // close the row and table
            writer.Write("</tr></table>");

            // write any child nodes
            RenderNodes(writer, node.Nodes, node.Expanded);

            if (node.Expanded)
            {
                if (_expandedValues.Length > 0) _expandedValues.Append(",");
                _expandedValues.Append(node.ValuePath);
            }

            // close the outer div
            EndHtmlTag(writer, "div");
            writer.WriteLine();

            writer.Indent--;

        }

        /// <summary>
        /// Renders a collection of nodes in the tree.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="nodes">The collection of nodes to be rendered.</param>
        /// <param name="makeVisible">Whether or not the collection should be visible on the screen.</param>
        private void RenderNodes(HtmlTextWriter writer, SAHLTreeNodeCollection nodes, bool makeVisible)
        {
            int nodeCount = nodes.Count;
            if (nodeCount > 0)
            {
                writer.WriteLine();

                for (int i=0; i<nodeCount; i++)
                {
                    SAHLTreeNode node = nodes[i];
                    bool visible = makeVisible;

                    //  set the node position
                    if (nodeCount == 1)
                        node.SetNodePosition(SAHLTreeNodePosition.Only);
                    else if (i == 0)
                        node.SetNodePosition(SAHLTreeNodePosition.First);
                    else if (i == (nodeCount - 1))
                        node.SetNodePosition(SAHLTreeNodePosition.Last);
                    else
                        node.SetNodePosition(SAHLTreeNodePosition.Middle);

                    // if the node was expanded before the postback, make sure the expanded property is set to true
                    if (ExpandedValues.Contains(node.ValuePath))
                        node.Expanded = true;

                    RenderNode(writer, node, visible);
                }

            }
        }

        protected void BeginHtmlTag(HtmlTextWriter writer, string tagName, string className, string id)
        {
            BeginHtmlTag(writer, tagName, className, id);
        }

        protected void BeginHtmlTag(HtmlTextWriter writer, string tagName, string className, string id, NameValueCollection attributes)
        {
            writer.WriteLine();
            writer.WriteBeginTag(tagName);
            if (!String.IsNullOrEmpty(className))
            {
                writer.WriteAttribute("class", className);
            }
            if (!String.IsNullOrEmpty(id))
            {
                writer.WriteAttribute("id", id);
            }

            // add any other attributes
            if (attributes != null && attributes.Count > 0)
            {
                foreach (string key in attributes.Keys)
                {
                    writer.WriteAttribute(key, attributes[key]);
                }
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;
        }

        protected void EndHtmlTag(HtmlTextWriter writer, string tagName)
        {
            writer.Indent--;
            writer.WriteLine();
            writer.WriteEndTag(tagName);
        }

        #endregion

        #region Ajax Methods 

        ///// <summary>
        ///// This method is used to populate a node via AJAX.  This is called whenever a + icon is clicked 
        ///// next to a node that has no nodes beneath it (once populated, this call will not happen again).
        ///// </summary>
        ///// <param name="treeViewID">The unique ID of the tree that was clicked.</param>
        ///// <param name="nodeID">The ID of the node that was clicked.</param>
        ///// <param name="nodeValuePath">The <see cref="SAHLTreeNode">ValuePath</see> of the node that was clicked.</param>
        ///// <param name="nodeValueSeparator">The <see cref="SAHLTreeNode.ValueSeparator">ValueSeparator</see> of the node that was clicked.</param>
        ///// <param name="positionPath">The full position path of the node that was clicked.</param> // see GetPositionPath
        ///// <param name="populateNodesMethod">The method to be used to populate the nodes - see <see cref="PopulateNodesMethod"/>.  This must be sent through as AJAX works on a different HTTP handler and "this" does not return the same tree instance.</param>
        ///// <returns>A string array containing the following data:
        /////     <list type="number">
        /////         <item>
        /////             <term>nodeID</term>
        /////             <description>The ID of the node that was clicked (so the callback function knows where to insert the new nodes)</description>
        /////         </item>
        /////         <item>
        /////             <term>treeViewID</term>
        /////             <description>The ID of the tree that was clicked</description>
        /////         </item>
        /////         <item>
        /////             <term>nodeHTML</term>
        /////             <description>The HTML of the nodes that needs to be inserted into the tree.</description>
        /////         </item>
        /////     </list>
        ///// </returns>
        //[AjaxPro.AjaxMethod]
        //public string[] AjaxPopulateNode(string treeViewID, string nodeID, string nodeValuePath, 
        //    char nodeValueSeparator, string positionPath, string populateNodesMethod)
        //{
        //    // extract the type name to be used - these must be fully qualified
        //    if (populateNodesMethod.IndexOf(".") == -1)
        //        throw new Exception("The PopulateNodesMethod property must supply a fully qualified method name.");

        //    string typeName = populateNodesMethod.Substring(0, populateNodesMethod.LastIndexOf("."));
        //    string methodName = populateNodesMethod.Replace(typeName + ".", "");

        //    // dynamically load the type - if we go through all the assemblies and we're unable to load it, drop out
        //    Type type = null;
        //    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //    foreach (Assembly assembly in assemblies)
        //    {
        //        type = assembly.GetType(typeName);
        //        if (type != null) break;
        //    }
        //    if (type == null) throw new ApplicationException("Unable to dynamically load type " + typeName);

        //    // create an instance of the type
        //    object oInstance = Activator.CreateInstance(type);
        //    if (oInstance == null) throw new Exception("Unable to dynamically create instance of " + type.FullName);

        //    // create a hierarchy of dummy parent nodes for the AJAX call
        //    char[] separator = { nodeValueSeparator };
        //    string[] arrValuePath = nodeValuePath.Split(separator);
        //    string[] arrPositionPath = positionPath.Split(separator);
        //    SAHLTreeNode nodeLast = null;

        //    for (int i=0; i<arrValuePath.Length; i++)
        //    {   
        //        SAHLTreeNode nodeTemp = new SAHLTreeNode("", arrValuePath[i]);
        //        nodeTemp.SetNodePosition((SAHLTreeNodePosition)Int32.Parse(arrPositionPath[i]));

        //        if (nodeLast != null)
        //            nodeLast.Nodes.Add(nodeTemp);

        //        nodeLast = nodeTemp;
        //    }

        //    object[] arguments = new object[1];
        //    arguments[0] = nodeLast;
        //    type.InvokeMember(methodName,
        //         BindingFlags.Default | BindingFlags.InvokeMethod,
        //         null,
        //         oInstance,
        //         arguments);

        //    StringBuilder sbAjax = new StringBuilder();
        //    this.ID = treeViewID;
        //    RenderNodes(new HtmlTextWriter(new StringWriter(sbAjax)), nodeLast.Nodes, true);

        //    string[] result = new string[3];
        //    result[0] = nodeID;
        //    result[1] = treeViewID;
        //    result[2] = sbAjax.ToString();

        //    return result;

        //}

        #endregion


    }
}
