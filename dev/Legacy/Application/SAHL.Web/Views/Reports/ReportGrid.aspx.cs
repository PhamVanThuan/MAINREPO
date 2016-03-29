using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI.Controls;
using System.Web.SessionState;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Threading;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;
using System.Text.RegularExpressions;

namespace SAHL.Web.Views.Reports
{

    /// <summary>
    /// 
    /// </summary>
    public partial class ReportGrid : SAHLCommonBaseView, IReportGrid
    {
        #region Variable Declarations

        DataTable _resultsTableToBind;
        private string _reportGridData;
        private string _reportName;
        private bool _showReportPanel;
        private long _recordCount;    

        const int numcols = 2;

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.ShouldRunPage)
                return;


            BindGrid();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!ShouldRunPage) return;
        }
        protected override void OnPreRender(EventArgs e)
        {
            
            base.OnPreRender(e);
            if (!this.ShouldRunPage)
                return;

            RegisterClientScripts();

            if (IsPostBack)
            {
                if (_reportGridData != null)
                {
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("content-disposition", "attachment; filename = " + _reportName + ".xls");
                    Response.Write(_reportGridData);
                    Response.End();
                    _reportGridData = null;
                }
            }
            if (_showReportPanel)
            {
                ReportDataGrid.Visible = false;
                pnlBatchReport.Visible = true;
                btnExport.Visible = false;
                btnGo.Visible = true;
            }
        }


        private void BindGrid()
        {
            if (_resultsTableToBind != null)
            {
                ReportDataGrid.KeyFieldName = _resultsTableToBind.Columns[0].ColumnName;

                ReportDataGrid.Settings.UseFixedTableLayout = false;
                ReportDataGrid.AutoGenerateColumns = true;
                ReportDataGrid.DataSource = _resultsTableToBind;
                ReportDataGrid.DataBind();
            }
        }

        protected void GenerateReport_Click(object sender, EventArgs e)
        {

            int statementKey = -1;
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (statementKey > 0)
            {
                SqlParameter p = new SqlParameter();
                p.Direction = ParameterDirection.Input;
                parameters.Add(p);
            }
        }

         protected void ExportReport_Click(object sender, EventArgs e)
        {
            if (_recordCount > 65000)
            {
                tblTooManyRecords.Visible = true;
                return;
            }            

            OnExportButtonClicked(sender, e);          
        }

        private void RegisterClientScripts()
        {
            StringBuilder sb = new StringBuilder();


            sb.AppendLine("function MyOnLoad()");
            sb.AppendLine("{");
            sb.AppendLine("}");


            //add the reason to the selected list, remove from available list and create a text area for the comment if necessary 
            sb.AppendLine("function addItem()");
            sb.AppendLine("{");
            sb.AppendLine(" if(txtEmailAddressAdd.value.length < 1)");
            sb.AppendLine("return;");
            sb.AppendLine(" var opt = new Option(txtEmailAddressAdd.value, txtEmailAddressAdd.value);");
            sb.AppendLine(" lstEmailAddresses.options.add(opt);");
            sb.AppendLine(" txtEmailAddressAdd.value = '';");
            sb.AppendLine(" SaveSelection();");
            sb.AppendLine("}");


            //remove from selected list, add back to available list
            sb.AppendLine("function removeItem()");
            sb.AppendLine("{");
            sb.AppendLine(" var idx = lstEmailAddresses.selectedIndex;");
            sb.AppendLine(" if(idx < 0) return;");
            sb.AppendLine(" lstEmailAddresses.options.remove(idx);");
            sb.AppendLine(" if(lstEmailAddresses.options.length > idx)");
            sb.AppendLine("     lstEmailAddresses.selectedIndex = idx;");
            sb.AppendLine(" else");
            sb.AppendLine("     lstEmailAddresses.selectedIndex = lstEmailAddresses.options.length-1;");
            sb.AppendLine(" SaveSelection();");
            sb.AppendLine("}");


            sb.AppendLine("function SaveSelection()");
            sb.AppendLine("{");
            sb.AppendLine(" listHiddenItems.value = '';");
            sb.AppendLine(" for (i=0;i<lstEmailAddresses.options.length;i++)");
            sb.AppendLine(" {");
            sb.AppendLine("     var arr = lstEmailAddresses.options[i].value;");
            sb.AppendLine("     listHiddenItems.value += '~/~' + lstEmailAddresses[i].value;");
            sb.AppendLine(" }");
            sb.AppendLine("}");

            sb.AppendLine("function btnConfirmClick()");
            sb.AppendLine("{");
            sb.AppendLine(" if(lstEmailAddresses.options.length < 1 )");
            sb.AppendLine(" {");
            sb.AppendLine("     alert('Please specify at least one email address');");
            sb.AppendLine("     event.returnValue = false;");
            //sb.AppendLine("     event.cancel = true;");
            sb.AppendLine(" }");
            sb.AppendLine(" else");
            sb.AppendLine(" if(lstEmailAddresses.options.length > 0)");
            sb.AppendLine(" {");
            sb.AppendLine("     if(confirm('Are you sure you want to submit the specified email addresses?'))");
            sb.AppendLine("     {");
            sb.AppendLine("         SaveSelection();");
            sb.AppendLine("     }");
            sb.AppendLine("     else");
            sb.AppendLine("         event.returnValue = false;");
            sb.AppendLine(" }");
            sb.AppendLine("}");


            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "MyOnLoad"))
            {
                StringBuilder load = new StringBuilder();
                //
                load.AppendLine("var txtEmailAddressAdd = document.getElementById('" + txtEmailAddressAdd.ClientID + "');");
                load.AppendLine("var lstEmailAddresses = document.getElementById('" + lstEmailAddresses.ClientID + "');");
                load.AppendLine("var HiddenInd = document.getElementById('" + HiddenInd.ClientID + "');");
                load.AppendLine("var listHiddenItems = document.getElementById('" + hiddenSelection.ClientID + "');");
                load.AppendLine("MyOnLoad();");

                Page.ClientScript.RegisterStartupScript(GetType(), "MyOnLoad", load.ToString(), true);
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", sb.ToString(), true);
            }
        }

        #region IReportService Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnExportButtonClicked;

        public DataTable ResultsTableToBind
        {
            set { _resultsTableToBind = value; }
        }


        public string ExportData
        {
            set { _reportGridData = value; }
        }


        public string ReportName
        {
            set { _reportName = value; }
        }

        #endregion

        #region IReportGrid Members      

        public bool Cancelled
        {
            get
            {
                if (Request.Form["__EVENTTARGET"] != null)
                {
                    if (Request.Form["__EVENTTARGET"] == btnCancel.UniqueID)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }


        public bool ShowEmailPanel
        {
            set { _showReportPanel = value; }
        }

        public event KeyChangedEventHandler OnGoButtonClicked;


        public long RecordCount
        {
            get
            {
               return _recordCount;
            }
            set
            {
                _recordCount = value;
            }
        }


        #endregion

        protected void btnGo_Click(object sender, EventArgs e)
        {           
            if (hiddenSelection.Value.Length > 0)
            {
                List<string> selections = new List<string>();
                while (hiddenSelection.Value.Contains("~/~"))
                {
                    int posOfDelimiter = hiddenSelection.Value.IndexOf("~/~");
                    if (posOfDelimiter == 0)
                    {
                        hiddenSelection.Value = hiddenSelection.Value.Substring(posOfDelimiter + 3, hiddenSelection.Value.Length - 3);
                    }
                    else
                    {
                        selections.Add(hiddenSelection.Value.Substring(0, posOfDelimiter));
                        hiddenSelection.Value = hiddenSelection.Value.Substring(posOfDelimiter + 3, hiddenSelection.Value.Length - posOfDelimiter - 3);
                    }
                }
                selections.Add(hiddenSelection.Value);

                if (selections.Count < 1)
                {
                    string msg = "Please specify at least one email address!";
                    Messages.Add(new Error(msg, msg));
                    return;
                }

                int cnt = 0;
                foreach (string l in selections)
                {
                    bool res = ValidateEmailAddress(l);
                    if (!res)
                    {
                        cnt++;
                        string msg = "'" + l + "' is an invalid email address";
                        Messages.Add(new Error(msg, msg));
                    }
                }
                if (cnt > 0)
                {
                    foreach (string s in selections)
                    {
                        lstEmailAddresses.Items.Add(new ListItem(s));
                    }
                    return;
                }

                string lst = string.Empty;
                for (int x = 0; x < selections.Count; x++)
                {
                    lst = lst + selections[x] + ",";
                }
                lst = lst.TrimEnd(',');

                KeyChangedEventArgs args = new KeyChangedEventArgs(lst);
            
                OnGoButtonClicked(sender, args);                          
            }            
        }

        private static bool ValidateEmailAddress(string address)
        {
            Regex regxEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (String.IsNullOrEmpty(address) || !regxEmail.IsMatch(address))
            {
                return false;
            }
            else
            {
                address = address.ToLower();
                if (address.Contains("@sahomeloans.com")
                 || address.Contains("@sahomeloans.co.za"))
                {
                    return true;
                }
            }
            return false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }
    }
}