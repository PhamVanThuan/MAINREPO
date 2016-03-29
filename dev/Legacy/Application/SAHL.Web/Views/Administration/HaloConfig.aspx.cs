using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Web.AJAX;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using System.Security.Principal;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SAHL.Web.Views.Administration
{
    /// <summary>
    /// View used to flush various items from the cache.
    /// </summary>
    public partial class HaloConfig : SAHLCommonBaseView, IHaloConfig
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            BindConfigValues();

        }

        /// <summary>
        /// Cleans password characters out of db connection strings
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        private static string CleanConnString(string connString)
        {
            return Regex.Replace(connString, @"password=[\w|\W|\d|\D|\s|\S]*;", "Password=**********;", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Binds the config values from the config files to the grid.
        /// </summary>
        protected void BindConfigValues()
        {
            List<ConfigItem> configItems = new List<ConfigItem>();

            // add each of the connection strings
            foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            {
                configItems.Add(new ConfigItem(css.Name, CleanConnString(css.ConnectionString), "Web.config"));
            }

            // load each of the config files - these are NOT available in ConfigurationManager.AppSettings
            DirectoryInfo di = new DirectoryInfo(Page.MapPath("~") + "\\Config");
            foreach (FileInfo fi in di.GetFiles("*.config"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fi.FullName);
                XmlNodeList settings = xmlDoc.GetElementsByTagName("setting");
                foreach (XmlNode node in settings)
                {
                    // check to see if the section is a list
                    XmlNodeList listNodes = node.SelectNodes("value/ArrayOfString/string");
                    if (listNodes.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (XmlNode listNode in listNodes)
                        {
                            if (sb.Length > 0) sb.Append(System.Environment.NewLine);
                            sb.Append(listNode.InnerText);
                        }
                        configItems.Add(new ConfigItem(node.Attributes["name"].InnerText,
                            sb.ToString(), fi.Name));
                    }
                    else
                    {
                        configItems.Add(new ConfigItem(node.Attributes["name"].InnerText,
                            node.SelectNodes("value")[0].InnerText, fi.Name));
                    }
                }
            }
            configItems.Sort(
              delegate(ConfigItem c1, ConfigItem c2)
              {
                  return c1.ItemName.CompareTo(c2.ItemName);
              });

            grdConfig.DataSource = configItems;
            grdConfig.DataBind();
        }

        /// <summary>
        /// Binds a list of Control objects to the view.
        /// </summary>
        /// <param name="controlValues"></param>
        public void BindControlTableValues(IEventList<IControl> controlValues)
        {
            List<IControl> sortedList = new List<IControl>();
            foreach (IControl control in controlValues)
                sortedList.Add(control);

            sortedList.Sort(
              delegate(IControl c1, IControl c2)
              {
                  return c1.ControlDescription.CompareTo(c2.ControlDescription);
              });
            grdControl.DataSource = sortedList;
            grdControl.DataBind();

        }

        protected void grdControl_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IControl control = (IControl)e.Row.DataItem;
                e.Row.Cells[0].Text = control.ControlDescription;
                if (control.ControlNumeric.HasValue)
                    e.Row.Cells[1].Text = control.ControlNumeric.Value.ToString();

                string text = String.Empty;
                // this is a hack to try and hide any passwords - this should be dealt with correctly by 
                // encrypting the values in the database
                if (!String.IsNullOrEmpty(control.ControlDescription) && control.ControlDescription.ToLower().Contains("password"))
                    text = "**********";
                else
                    text = control.ControlText;

                if (String.IsNullOrEmpty(text))
                    text = "";

                if (text.ToLower().Contains("password"))
                {
                    text = CleanConnString(text);
                }

                e.Row.Cells[2].Text = text;
                
            }

        }

        protected void grdConfig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ConfigItem ci = (ConfigItem)e.Row.DataItem;
                e.Row.Cells[1].Text = ci.ItemValue.Replace(System.Environment.NewLine, "<br>");
            }

        }


        /// <summary>
        /// Private class used to bind config items to the grid.
        /// </summary>
        private class ConfigItem
        {
            private string _itemName;
            private string _itemValue;
            private string _source;

            public ConfigItem(string itemName, string itemValue, string source)
            {
                _itemName = itemName;
                _itemValue = itemValue;
                _source = source;
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Class used for binding.")]
            public string ItemName
            {
                get { return _itemName; }
                set { _itemName = value; }
            }


            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Class used for binding.")]
            public string ItemValue
            {
                get { return _itemValue; }
                set { _itemValue = value; }
            }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Class used for binding.")]
            public string Source
            {
                get { return _source; }
                set { _source = value; }
            }
	
	
	
        }

    }
}
