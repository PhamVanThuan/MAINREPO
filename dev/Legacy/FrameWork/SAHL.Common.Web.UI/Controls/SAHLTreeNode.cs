using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.Web.UI.Controls
{

    #region Enumerations

    public enum SAHLTreeNodePosition
    {
        First = 0,
        Middle,
        Last,
        Only
    }

    #endregion

    /// <summary>
    /// Defines a single node that can be added to a <see cref="SAHLTreeView"/>.
    /// </summary>
    public class SAHLTreeNode
    {
        #region Private Attributes

        private string _text = "";
        private string _value = "";
        private string _toolTipText = "";
        private SAHLTreeNode _parentNode = null;
        private bool _expanded = false;
        private bool _hasChildren = false;
        private SAHLTreeNodeCollection _lstNodes = null;
        private char _valueSeparator = '/';
        private SAHLTreeNodePosition _position = SAHLTreeNodePosition.Only;
        private List<SAHLTreeNodeIcon> _lstIcons = new List<SAHLTreeNodeIcon>();
        private string _cssClass = "";
        private string _navigateValue = "";
        private string _onClientClick = "";
        private bool _autoPostBack = true;
        private bool _checkBoxVisible;
        private bool _checkBoxSelected;
        private bool _checkBoxDisabled = false;

        #endregion


        #region Constructors

        public SAHLTreeNode()
            : this("", "")
        {
        }

        public SAHLTreeNode(string text)
            : this(text, "")
        {
        }

        public SAHLTreeNode(string text, string value)
        {
            _lstNodes = new SAHLTreeNodeCollection(this);
            _text = text;
            _value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets whether the checkbox is disabled or not.
        /// </summary>
        public bool CheckBoxDisabled
        {
            get { return _checkBoxDisabled; }
            set { _checkBoxDisabled = value; }
        }

        /// <summary>
        /// Gets/sets whether the node should automatically post back.
        /// </summary>
        public bool AutoPostBack
        {
            get
            {
                return _autoPostBack;
            }
            set
            {
                _autoPostBack = value;
            }
        }

        /// <summary>
        /// Gets/sets whether the checkbox is checked or not.
        /// </summary>
        public bool CheckBoxSelected
        {
            get { return _checkBoxSelected; }
            set { _checkBoxSelected = value; }
        }

        /// <summary>
        /// Gets/sets whether the checkbox is visible or not.
        /// </summary>
        public bool CheckBoxVisible
        {
            get { return _checkBoxVisible; }
            set { _checkBoxVisible = value; }
        }

        /// <summary>
        /// Gets/sets the CSS class applied to the node. This will affect the node and child nodes.  By default, all nodes 
        /// have the TreeNode class applied - this property allows you to add any other styles to a child (e.g. background 
        /// colour).
        /// </summary>
        public string CssClass
        {
            get
            {
                return _cssClass;
            }
            set
            {
                _cssClass = value;
            }
        }

        /// <summary>
        /// Gets the depth of the node in the tree list.
        /// </summary>
        public int Depth
        {
            get
            {
                int depth = 0;
                SAHLTreeNode node = this;

                while (true)
                {
                    node = node.ParentNode;
                    if (node == null) break;
                    depth++;
                }

                return depth;
            }
        }

        /// <summary>
        /// Gets/sets whether the node is expanded.
        /// </summary>
        public bool Expanded
        {
            get
            {
                return _expanded;
            }
            set
            {
                _expanded = value;
            }
        }


        /// <summary>
        /// Used to determine if the node has child nodes.  This will return true if the there are nodes in the 
        /// <see cref="Nodes"/> collection, but can also be set to force the node to display with the expand 
        /// image when the node needs to be populated at a later stage (e.g. after another postback or an AJAX call).
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return (Nodes.Count > 0 || _hasChildren);
            }
            set
            {
                _hasChildren = value;
            }
        }

        /// <summary>
        /// List used to store the icons that are rendered with the node.
        /// </summary>
        public List<SAHLTreeNodeIcon> Icons
        {
            get
            {
                return _lstIcons;
            }
        }

        /// <summary>
        /// Gets/sets the value of a JavaScript function that will be executed BEFORE the postback on the node 
        /// occurs.
        /// </summary>
        public string OnClientClick
        {
            get
            {
                return _onClientClick;
            }
            set
            {
                _onClientClick = value;
            }
        }

        /// <summary>
        /// Gets the position of the node in the tree relative to it's parent.
        /// </summary>
        public SAHLTreeNodePosition Position
        {
            get
            {
                return _position;
            }
        }

        /// <summary>
        /// Gets/sets the navigation value applied to the node.
        /// </summary>
        public string NavigateValue
        {
            get
            {
                return _navigateValue;
            }
            set
            {
                _navigateValue = value;
            }
        }

        /// <summary>
        /// Gets the collection of child nodes assigned to the node.
        /// </summary>
        public SAHLTreeNodeCollection Nodes
        {
            get
            {
                return _lstNodes;
            }
        }

        /// <summary>
        /// Gets/sets the parent node of the node.
        /// </summary>
        public SAHLTreeNode ParentNode
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

        /// <summary>
        /// Gets/sets the text on the node
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        /// <summary>
        /// Gets/sets the tool tip text of the node
        /// </summary>
        /// 
        public string ToolTipText
        {
            get
            {
                return _toolTipText;
            }
            set
            {
                _toolTipText = value;
            }
        }


        /// <summary>
        /// Gets/sets the value stored against the node.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// This is the ID that is used when rendering the node.  This is built up using the <see cref="ValuePath"/>, 
        /// but the <see cref="ValueSeparator"/> is replaced with an underscore.
        /// </summary>
        public string UniqueID
        {
            get
            {
                return this.ValuePath.Replace(ValueSeparator, '_');
            }
        }

        /// <summary>
        /// Gets the full value path of the node.
        /// </summary>
        public string ValuePath
        {
            get
            {
                SAHLTreeNode node = this;
                StringBuilder sb = new StringBuilder();
                sb.Append(Value);
                while ((node = node.ParentNode) != null)
                {
                    sb.Insert(0, "/");
                    sb.Insert(0, node.Value);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Get/sets the separator that is used to build up the <see cref="ValuePath"/>.
        /// </summary>
        public char ValueSeparator
        {
            get
            {
                return _valueSeparator;
            }
            set
            {
                _valueSeparator = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This is used internally by the <see cref="SAHLTreeView"/> to set the node position.  This is purely 
        /// for performance reasons - it saves us having to iterate through parents to determine what image 
        /// to render for the node.
        /// </summary>
        /// <param name="position"></param>
        internal void SetNodePosition(SAHLTreeNodePosition position)
        {
            _position = position;
        }

        #endregion

    }
    
        
}
