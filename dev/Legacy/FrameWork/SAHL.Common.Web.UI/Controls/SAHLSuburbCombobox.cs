using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace SAHL.Common.Web.UI.Controls
{
    [ToolboxBitmap(typeof(SAHLSuburbCombobox), "Resources.SAHLSuburbCombobox.bmp")]
    [ToolboxData("<{0}:SAHLSuburbCombobox runat=server></{0}:SAHLSuburbCombobox>")]
    [Obsolete("Use SAHLAutoComplete instead", true)]
    public class SAHLSuburbCombobox : System.Web.UI.WebControls.TextBox, ICallbackEventHandler
    {
        public delegate void FillSuburbsEventHandler(string PartialSuburbText, int ProvinceKey, List<SuburbListItem> Suburbs);
        public event FillSuburbsEventHandler OnFillSuburbsList;

        protected string EventArgument = "";
        protected string m_PostCodeInput;
        protected string m_CityCombobox;
        protected string m_ProvinceCombobox;
        protected string m_Suburb = "";
        protected SelectListPositionEnum m_SelectListPosition = SelectListPositionEnum.Bottom;

        #region ICallbackEventHandler Members

        public string GetCallbackResult()
        {
            string[] Args = EventArgument.Split(new char[] { '|' });
            if(Args.Length != 2)
                return "";
            string Suburb = Args[0];
            string Province = Args[1];

            if ((Suburb != null) && (Suburb != "") && (OnFillSuburbsList != null) && (SAHL.Common.MathCalcs.IsInteger(Province)))
            {
                List<SuburbListItem> Data = new List<SuburbListItem>();
                OnFillSuburbsList(Suburb, int.Parse(Province), Data);

                string Result = "";
                for (int i = 0; i < Data.Count; i++)
                {
                    Result += (Data[i].SuburbKey + "|" + Data[i].SuburbName + "|" + Data[i].CityKey + "|" + Data[i].CityName + "|" + Data[i].PostalCode);
                    if (i < Data.Count - 1)
                        Result += ",";
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

        #region properties

        ///
        /// The ID of the control that will receive the selected items ID value 
        /// If nothing is specified then this control is used
        /// 
        [Bindable(true), PersistenceMode(PersistenceMode.Attribute)]
        public SelectListPositionEnum SelectListPosition
        {
            get { return m_SelectListPosition; }
            set { m_SelectListPosition = value; }
        }

        [Bindable(true), PersistenceMode(PersistenceMode.Attribute)]
        public string CityCombobox
        {
            get
            {
                return m_CityCombobox;
            }
            set
            {
                m_CityCombobox = value;
            }
        }

        [Bindable(true), PersistenceMode(PersistenceMode.Attribute)]
        public string ProvinceCombobox
        {
            get
            {
                return m_ProvinceCombobox;
            }
            set
            {
                m_ProvinceCombobox = value;
            }
        }

        [Bindable(true), PersistenceMode(PersistenceMode.Attribute)]
        public string PostCodeInput
        {
            get
            {
                return m_PostCodeInput;
            }
            set
            {
                m_PostCodeInput = value;
            }
        }

        [Bindable(false)]
        public string Suburb
        {
            get
            {
                if (!this.DesignMode)
                {
                    return m_Suburb;
                    //if (Page.Request.Form[this.ClientID + "_Suburb"] != null)
                    //    return Page.Request.Form[this.ClientID + "_Suburb"];
                    //else
                    //    return "";
                }
                return "";
            }
            set
            {
                m_Suburb = value;
            }
        }

        #endregion

        #region overrides

        protected override void OnPreRender(EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            // add a few cutom expando properties
            Control C = null;
            if(m_ProvinceCombobox != null)
                C = NamingContainer.FindControl(m_ProvinceCombobox);
            if (C != null)
                cs.RegisterExpandoAttribute(this.ClientID, "ProvinceCombobox", C.ClientID);
            if(m_PostCodeInput != null)
                C = NamingContainer.FindControl(m_PostCodeInput);
            if (C != null)
                cs.RegisterExpandoAttribute(this.ClientID, "PostCodeInput", C.ClientID);
            if(m_CityCombobox != null)
                C = NamingContainer.FindControl(m_CityCombobox);
            if (C != null)
                cs.RegisterExpandoAttribute(this.ClientID, "CityCombobox", C.ClientID);

            base.OnPreRender(e);
        }

        //Add the dropdown list, initially hidden.
        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {
            EnsureChildControls();
            this.CssClass = "InText";
            base.Render(output);
            // design time support, don't do anything extra, the base is just rendered above, else in runtime do below
            if (this.Context != null)
            {
                if (this.SelectListPosition == SelectListPositionEnum.Bottom)
                    output.Write("<Span Position='bottom' style='z-index:1000;position: absolute;visibility:hidden;' class='dropdownlist_dropdown' id='" + this.ClientID + "_dropdown'></span>");
                else
                    output.Write("<Span Position='top' style='z-index:1000;position: absolute;visibility:hidden;' class='dropdownlist_dropdown' id='" + this.ClientID + "_dropdown'></span>");
                
                output.Write(@"<IFRAME style='z-index:999;display: none; left: 0px; top: 0px;position: absolute;' src='javascript:false;' frameBorder='0' scrolling='no' id='" + this.ClientID + "_dropdown_iframe'></IFRAME>");
            }
        }

        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Page.IsPostBack)
            {
                // only set the value if the client hasn't overrridden
                if(m_Suburb.Equals(""))
                    if (Page.Request.Form[this.ClientID + "_Suburb"] != null)
                        m_Suburb = Page.Request.Form[this.ClientID + "_Suburb"];
            }

            this.Attributes.Add("onkeypress", "checkForEnterSelect('" + this.ClientID + "_dropdown');");
            this.Attributes.Add("onkeyup", "getSuburbResultMatches(event, this.value, '" + this.ClientID + "', '" + this.ClientID + "_dropdown');");
            this.Attributes.Add("onblur", "hideSuburbDropDown('" + this.ClientID + "', '" + this.ClientID + "_dropdown', true);");

            ClientScriptManager cs = Page.ClientScript;
            Type rsType = this.GetType();
            string location = null;

            if (Page.Header != null && Page.Header.FindControl("SAHLSuburbComboboxStyles") == null)
            {
                // add the stylesheet
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLSuburbCombobox.css");
                HtmlLink cssHtmlLink = new HtmlLink();
                cssHtmlLink.Href = location;
                cssHtmlLink.Attributes.Add("rel", "stylesheet");
                cssHtmlLink.Attributes.Add("type", "text/css");
                cssHtmlLink.ID = "SAHLSuburbComboboxStyles";
                Page.Header.Controls.Add(cssHtmlLink);
            }

            //The javascript function getResultMatches is called onkeydown of the textbox.  It calls the server-side method
            //with all the info it will need to figure out which method it needs to call.
            //GetMatchedResults and showDropDown are called when the server method returns.
            //selectLink is called when the user hits the up or down arrow keys in the textbox and implements
            //the UI response to that event.
            if (!cs.IsClientScriptIncludeRegistered(rsType, "SAHLSuburbsComboFunctions"))
            {
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLSuburbCombobox.js");
                cs.RegisterClientScriptInclude(rsType, "SAHLSuburbsComboFunctions", location);
            }

            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "GetSuburbMatchedResults"))
            {
                String cbReference =
                            Page.ClientScript.GetCallbackEventReference(this,
                            "arg", "ReceiveSuburbServerData", "context");
                String callbackScript;
                callbackScript = "function GetSuburbMatchedResults(arg, context)" +
                    "{ " + cbReference + "} ;";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "GetSuburbMatchedResults", callbackScript, true);
            }


            cs.RegisterHiddenField(this.ClientID + "_Suburb", m_Suburb);
        }
        #endregion
    }

    public class SuburbListItem
    {
        public int SuburbKey;
        public string SuburbName;
        public int CityKey;
        public string CityName;
        public string PostalCode;
    }
}
