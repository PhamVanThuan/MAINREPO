using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Lightweight class to allow the return of text/value pair objects to be returned in AJAX calls to the client.
    /// </summary>
    [Serializable()]
    public class AjaxItem
    {
        private string _text = "";
        private string _value = "";

        /// <summary>
        /// Parameterless constructor - this is for serialization purposes only and should not be used in code.
        /// </summary>
        public AjaxItem() : this("", "")
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text"></param>
        public AjaxItem(string text) : this(text, "")
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public AjaxItem(string text, string value)
        {
            _text = text;
            _value = value;

        }

        /// <summary>
        /// The text of the item - this will be a string that is usually visible on the web page.
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
        /// The value of the item - usually a key or something that is not displayed on the web page.
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


    }
}
