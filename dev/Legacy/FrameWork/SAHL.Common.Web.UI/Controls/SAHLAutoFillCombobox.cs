using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(SAHLAutoFillCombobox), "Resources.SAHLAutoFillCombobox.bmp")]
    [ToolboxData("<{0}:SAHLAutoFillCombobox runat=server></{0}:SAHLAutoFillCombobox>")]
    [Obsolete("Use SAHLAutoComplete instead", true)]
    public class SAHLAutoFillCombobox : System.Web.UI.WebControls.TextBox, ICallbackEventHandler
    {

        /// <summary>
        /// When implementing this method you must fill the Results string collection
        /// You can either fill it will Pipe delimited pairs of values which are interpreted as value|name
        /// or single values which are the name only
        /// </summary>
        /// <param name="PartialText"></param>
        /// <param name="Results"></param>
        public delegate void FillDataEventHandler(string PartialText, StringCollection Results);
        public event FillDataEventHandler OnFillListData;

        protected string EventArgument = "";
        protected string m_TargetTextControl;
        protected string m_TargetIDControl;
        protected SelectListPositionEnum m_SelectListPosition = SelectListPositionEnum.Bottom;

        #region ICallbackEventHandler Members

        public string GetCallbackResult()
        {
            if (EventArgument != null && EventArgument != "" && OnFillListData != null)
            {
                StringCollection Data = new StringCollection();
                OnFillListData(EventArgument, Data);

                string Result = "";
                for (int i = 0; i < Data.Count; i++)
                {
                    Result += Data[i];
                    if (i < Data.Count - 1)
                        Result += "!";
                }
                return Result;
            }

            return "";
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            EventArgument = eventArgument;
        }

        #endregion

        
        //Add the dropdown list, initially hidden.
        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {
            base.Render(output);
            // design time support, don't do anything extra, the base is juset rendered above, else in runtime do below
            if (this.Context != null)
            {
                if (this.SelectListPosition == SelectListPositionEnum.Bottom)
                    output.Write("<Span Position='bottom' style='z-index:1000;position: absolute;visibility:hidden;' class='dropdownlist_dropdown' id='" + this.ClientID + "_dropdown'></span>");
                else
                    output.Write("<Span Position='top' style='z-index:1000;position: absolute;visibility:hidden;' class='dropdownlist_dropdown' id='" + this.ClientID + "_dropdown'></span>");
                output.Write(@"<IFRAME style='z-index:999;display: none; left: 0px; top: 0px;position: absolute;background-color:transparent;' src='javascript:false;' frameBorder='0' scrolling='no' id='" + this.ClientID + "_dropdown_iframe'></IFRAME>");
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {


            string getResultMatches = "getResultMatches_" + this.ClientID;

            if (this.AutoPostBack)
                writer.AddAttribute("postbackEvent", Page.ClientScript.GetPostBackEventReference(this, ""));

            writer.AddAttribute("onkeypress", "checkForEnterSelect('" + this.ClientID + "_dropdown');");
            writer.AddAttribute("onkeyup", getResultMatches + "(event, this.value, '" + this.ClientID + "');");
            writer.AddAttribute("onblur", "hideDropDown('" + this.ClientID + "_dropdown');");
            //writer.AddAttribute("onchange", "return false;");

            Control C = null;

            // Register the TextControl Attibute
            C = NamingContainer.FindControl(TargetTextControl);
            if (C != null)
                writer.AddAttribute("TargetTextControl", C.ClientID);

            // Register the IDControl Attibute
            C = NamingContainer.FindControl(TargetIDControl);
            if (C != null)
                writer.AddAttribute("TargetIDControl", C.ClientID);

            base.AddAttributesToRender(writer);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string getResultMatches = "getResultMatches_" + this.ClientID;


            ClientScriptManager cs = Page.ClientScript;
            Type rsType = this.GetType();
            string location = null;

            if (Page.Header != null && Page.Header.FindControl("SAHLAutoFillComboboxStyles") == null)
            {
                // add the stylesheet
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLAutoFillCombobox.css");
                HtmlLink cssHtmlLink = new HtmlLink();
                cssHtmlLink.Href = location;
                cssHtmlLink.Attributes.Add("rel", "stylesheet");
                cssHtmlLink.Attributes.Add("type", "text/css");
                cssHtmlLink.ID = "SAHLAutoFillComboboxStyles";
                Page.Header.Controls.Add(cssHtmlLink);
            }


            //The javascript function getResultMatches is called onkeydown of the textbox.  It calls the server-side method
            //with all the info it will need to figure out which method it needs to call.
            //GetMatchedResults and showDropDown are called when the server method returns.
            //selectLink is called when the user hits the up or down arrow keys in the textbox and implements
            //the UI response to that event.
            if (!cs.IsClientScriptIncludeRegistered(rsType, "GeneralDropDownListFunctions"))
            {
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLAutoFillCombobox.js");
                cs.RegisterClientScriptInclude(rsType, "GeneralDropDownListFunctions", location);
            }

            string GetMatchedResults = "GetMatchedResults_" + this.ClientID;

            if (!cs.IsClientScriptBlockRegistered(this.GetType(), GetMatchedResults))
            {
                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData"  , "context");
                String callbackScript;
                callbackScript = "function " + GetMatchedResults + "(arg, context){ " + cbReference + "} ;";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), GetMatchedResults, callbackScript, true);
            }

            if (!cs.IsClientScriptBlockRegistered(this.GetType(), getResultMatches))
            {

                // Register the Keyup event as well
                StringBuilder sb = new StringBuilder(); 
                sb.AppendLine("\r\nfunction " + getResultMatches + "(event, partialResultText, context)");
                sb.AppendLine("{");
                sb.AppendLine("var key = window.event ? event.keyCode : event.which;");
                sb.AppendLine("if (key == 38 || key == 40)	{ selectLink(key == 40);return;	}");
                sb.AppendLine("if (partialResultText.length > 0 && (key >= 32 || key == 8)){ " + GetMatchedResults + "(partialResultText,context);}");
                sb.AppendLine("else{hideDropDown();}");
                sb.AppendLine("return key;");
                sb.AppendLine("}");

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), getResultMatches, sb.ToString(), true);

            }
        }


        /// 
        ///
        /// Where the select list will be displayed, above or below the text control
        /// 
        [Bindable(true), PersistenceMode(PersistenceMode.Attribute)]
        public SelectListPositionEnum SelectListPosition
        {
            get{return m_SelectListPosition; }
            set { m_SelectListPosition = value; }
        }
        ///
        /// The ID of the control that will receive the selected items ID value 
        /// If nothing is specified then this control is used
        /// 
        [Bindable(true), PersistenceMode(PersistenceMode.Attribute)]
        public string TargetIDControl
        {
            get
            {
                if (m_TargetIDControl == null)
                    return this.ID;
                else
                    return m_TargetIDControl;

                
            }
            set { m_TargetIDControl = value; }
        }

        ///
        /// The ID of the control that will receive the selected items Text value 
        /// If nothing is specified then this control is used
        /// 
        [Bindable(true), PersistenceMode(PersistenceMode.Attribute)]
        public string TargetTextControl
        {
            get
            {
                if (m_TargetTextControl == null)
                    return this.ID;
                else
                    return m_TargetTextControl;
            }
            set { m_TargetTextControl = value; }
        }

    }
}
