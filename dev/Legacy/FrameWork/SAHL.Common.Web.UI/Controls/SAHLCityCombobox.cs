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
    [ToolboxBitmap(typeof(SAHLCityCombobox), "Resources.SAHLCityCombobox.bmp")]
    [ToolboxData("<{0}:SAHLCityCombobox runat=server></{0}:SAHLCityCombobox>")]
    [Obsolete("Use SAHLAutoComplete instead", true)]
    public class SAHLCityCombobox : System.Web.UI.WebControls.TextBox, ICallbackEventHandler
    {

        public delegate void FillCitiesEventHandler(string PartialCityText, int ProvinceKey, List<CityListItem> Citys);
        public event FillCitiesEventHandler OnFillCitiesList;

        protected string EventArgument = "";
        protected string m_SuburbCombobox;
        protected string m_ProvinceCombobox;
        protected string m_PostCodeInput;
        protected string m_City = "";
        protected SelectListPositionEnum m_SelectListPosition = SelectListPositionEnum.Bottom;

        #region ICallbackEventHandler Members

        public string GetCallbackResult()
        {
            string[] Args = EventArgument.Split(new char[] { '|' });
            if (Args.Length != 2)
                return "";
            string City = Args[0];
            string Province = Args[1];

            if ((City != null) && (City != "") && (OnFillCitiesList != null) && (MathCalcs.IsInteger(Province)))
            {
                List<CityListItem> Data = new List<CityListItem>();
                OnFillCitiesList(City, int.Parse(Province), Data);

                string Result = "";
                for (int i = 0; i < Data.Count; i++)
                {
                    Result += (Data[i].CityKey + "|" + Data[i].CityName);
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
        public string SuburbCombobox
        {
            get {return m_SuburbCombobox;}
            set {m_SuburbCombobox = value;}
        }

        [Bindable(false)]
        public string City
        {
            get
            {
                if (!this.DesignMode)
                {
                    return m_City;
                }
                return "";
            }
            set
            {
                m_City = value;
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

        #endregion

        #region overrides

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

        protected override void OnPreRender(EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            // add a few cutom expando properties
            Control C = null;
            if(m_ProvinceCombobox != null)
                C = NamingContainer.FindControl(m_ProvinceCombobox);
            if (C != null)
                cs.RegisterExpandoAttribute(this.ClientID, "ProvinceCombobox", C.ClientID);
            if(m_SuburbCombobox != null)
                C = NamingContainer.FindControl(m_SuburbCombobox);
            if (C != null)
                cs.RegisterExpandoAttribute(this.ClientID, "SuburbCombobox", C.ClientID);
            if(m_PostCodeInput != null)
                C = NamingContainer.FindControl(m_PostCodeInput);
            if (C != null)
                cs.RegisterExpandoAttribute(this.ClientID, "PostCodeInput", C.ClientID);
            cs.RegisterHiddenField(this.ClientID + "_City", m_City);

            base.OnPreRender(e);
        }
        //
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Page.IsPostBack)
            {
                if (Page.Request.Form[this.ClientID + "_City"] != null)
                    m_City = Page.Request.Form[this.ClientID + "_City"];
            }

            this.Attributes.Add("onkeypress", "checkForEnterSelect('" + this.ClientID + "_dropdown');");
            this.Attributes.Add("onkeyup", "getCityResultMatches(event, this.value, '" + this.ClientID + "', '" + this.ClientID + "_dropdown');");
            this.Attributes.Add("onblur", "hideCityDropDown('" + this.ClientID + "', '" + this.ClientID + "_dropdown', true);");

            ClientScriptManager cs = Page.ClientScript;
            Type rsType = typeof(SAHLSuburbCombobox);
            string location = null;

            if (Page.Header != null && Page.Header.FindControl("SAHLCityComboboxStyles") == null)
            {
                // add the stylesheet
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLCityCombobox.css");
                HtmlLink cssHtmlLink = new HtmlLink();
                cssHtmlLink.Href = location;
                cssHtmlLink.Attributes.Add("rel", "stylesheet");
                cssHtmlLink.Attributes.Add("type", "text/css");
                cssHtmlLink.ID = "SAHLCityComboboxStyles";
                Page.Header.Controls.Add(cssHtmlLink);
            }

            //The javascript function getResultMatches is called onkeydown of the textbox.  It calls the server-side method
            //with all the info it will need to figure out which method it needs to call.
            //GetMatchedResults and showDropDown are called when the server method returns.
            //selectLink is called when the user hits the up or down arrow keys in the textbox and implements
            //the UI response to that event.
            if (!cs.IsClientScriptIncludeRegistered(rsType, "SAHLCityComboFunctions"))
            {
                location = cs.GetWebResourceUrl(rsType, "SAHL.Common.Web.UI.Controls.Resources.SAHLCityCombobox.js");
                cs.RegisterClientScriptInclude(rsType, "SAHLCityComboFunctions", location);
            }

            if (!cs.IsClientScriptBlockRegistered(this.GetType(), "GetCityMatchedResults"))
            {
                String cbReference =
                            Page.ClientScript.GetCallbackEventReference(this,
                            "arg", "ReceiveCityServerData", "context");
                String callbackScript;
                callbackScript = "function GetCityMatchedResults(arg, context)" +
                    "{ " + cbReference + "} ;";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                    "GetCityMatchedResults", callbackScript, true);
            }
        }

        #endregion

    }

    public class CityListItem
    {
        public int CityKey;
        public string CityName;
    }
}
