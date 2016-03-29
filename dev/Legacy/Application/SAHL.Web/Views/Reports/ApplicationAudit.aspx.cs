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
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;
using SAHL.Web.AJAX;
using SAHL.Common.Collections.Interfaces;
using System.Xml;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Reports
{
    public partial class ApplicationAudit : SAHLCommonBaseView, IApplicationAudit
    {
        private string _backgroundCssClass;

        #region Page Life Cycle Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            repAudit.ItemDataBound += new RepeaterItemEventHandler(repAudit_ItemDataBound);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            base.RegisterWebService(ServiceConstants.Application);
        }
            
        #endregion

        #region Event Handlers

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }

        private void repAudit_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Separator)
                return;

            Repeater repValues = (Repeater)e.Item.FindControl("repValues");
            IAudit audit = (IAudit)e.Item.DataItem;

            // extract the xml values 
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(audit.AuditData);

            XmlNodeList nodesPrevious = xmlDoc.GetElementsByTagName("PreviousValues");
            XmlNodeList nodesCurrent = xmlDoc.GetElementsByTagName("CurrentValues");

            XmlNode previous = null;
            XmlNode current = null;

            // work out if we're using post New-Origination data or not
            if (nodesPrevious.Count == 0)
            {
                nodesPrevious = xmlDoc.GetElementsByTagName("diffgr:before");
                nodesCurrent = xmlDoc.GetElementsByTagName("AuditDataSet");
                if (nodesPrevious.Count > 0)
                    previous = nodesPrevious[0].ChildNodes[0];
                if (nodesCurrent.Count > 0)
                    current = nodesCurrent[0].ChildNodes[0];
            }
            else
            {
                previous = nodesPrevious[0];
                current = nodesCurrent[0];
            }

            Dictionary<string, string> dictPrevious = CreateDictionaryFromNode(previous);
            Dictionary<string, string> dictCurrent = CreateDictionaryFromNode(current);

            // build the list of audit items from the dictionaries
            List<BindableAuditItem> auditItems = new List<BindableAuditItem>();

            foreach (KeyValuePair<string, string> kvp in dictPrevious)
            {
                BindableAuditItem bai = new BindableAuditItem();
                bai.Title = kvp.Key;
                bai.Previous = kvp.Value;
                if (dictCurrent.ContainsKey(kvp.Key))
                {
                    bai.Current = dictCurrent[kvp.Key];
                    dictCurrent.Remove(kvp.Key);
                }
                auditItems.Add(bai);
            }
            foreach (KeyValuePair<string, string> kvp in dictCurrent)
            {
                BindableAuditItem bai = new BindableAuditItem();
                bai.Title = kvp.Key;
                bai.Current = kvp.Value;
                auditItems.Add(bai);
            }
            repValues.DataSource = auditItems;
            repValues.DataBind();
        }

        #endregion

        #region IApplicationAudit Members

        /// <summary>
        /// Event raised when the submit button is clicked.
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// Gets the application key entered in the input box.
        /// </summary>
        public string ApplicationKey
        {
            get
            {
                return txtApplicationKey.Text;
            }
        }

        /// <summary>
        /// Binds a list of audit information to the screen.
        /// </summary>
        /// <param name="auditData"></param>
        public void BindAuditData(IEventList<IAudit> auditData)
        {
            if (auditData.Count > 0)
            {
                repAudit.DataSource = auditData;
                repAudit.DataBind();
                repAudit.Visible = true;
            }
            else
            {
                lblNoData.Visible = true;
            }
        }


        #endregion

        #region Private and Protected Members

        private static Dictionary<string, string> CreateDictionaryFromNode(XmlNode node)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (node != null)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    dict.Add(childNode.Name, childNode.InnerText);
                }
            }
            return dict;
        }

        protected string BackgroundCssClass
        {
            get
            {
                _backgroundCssClass = (_backgroundCssClass == "backgroundLight" ? "backgroundDark" : "backgroundLight");
                return _backgroundCssClass;

            }
        }

        #endregion

        protected class BindableAuditItem
        {
            private string _title;
            private string _previous;
            private string _current;

            public string Title
            {
                get { return _title; }
                set { _title = value; }
            }

            public string Previous
            {
                get { return _previous; }
                set { _previous = value; }
            }

            public string Current
            {
                get { return _current; }
                set { _current = value; }
            }
	
	
	
        }

    }
}

