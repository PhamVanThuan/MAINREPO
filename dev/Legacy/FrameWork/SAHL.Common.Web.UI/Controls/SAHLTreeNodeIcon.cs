using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SAHL.Common.Web.UI.Controls
{
    /// <summary>
    /// Icons that can be added to an <see cref="SAHLTreeNode"/>.
    /// </summary>
    public class SAHLTreeNodeIcon
    {
        private string _icon = "";
        private string _hoverIcon = "";
        private string _alternateText = "";
        private bool _autoPostBack = false;
        private string _clientClickHandler = "";

        public SAHLTreeNodeIcon(string icon) : this(icon, "", "")
        {
        }

        public SAHLTreeNodeIcon(string icon, string hoverIcon, string alternateText)
        {
            _icon = icon;
            _hoverIcon = hoverIcon;
            _alternateText = alternateText;
        }

        /// <summary>
        /// Gets/sets the tooltip text displayed over the icon.
        /// </summary>
        public string AlternateText
        {
            get
            {
                return _alternateText;
            }
            set
            {
                _alternateText = value;
            }
        }

        /// <summary>
        /// Gets/sets whether a postback is performed when the icon is clicked.
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
        /// Gets/sets a JavaScript function that is called when the node is clicked.
        /// </summary>
        public string ClientClickHandler
        {
            get
            {
                return _clientClickHandler;
            }
            set
            {
                _clientClickHandler = value;
            }
        }
        /// <summary>
        /// Gets/sets the icon to display when the mouse hovers over the icon.  This can be set to null.
        /// </summary>
        public string HoverIcon
        {
            get
            {
                return _hoverIcon;
            }
            set
            {
                _hoverIcon = value;
            }
        }

        /// <summary>
        /// The icon displayed when the node is rendered.
        /// </summary>
        public string Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
            }
        }

    }
}
