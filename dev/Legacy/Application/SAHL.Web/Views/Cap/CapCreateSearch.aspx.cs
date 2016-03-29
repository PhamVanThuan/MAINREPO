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
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Web.Views.Cap.Interfaces;
using System.Text;

namespace SAHL.Web.Views.Cap
{
    public partial class CapCreateSearch : SAHLCommonBaseView, ICapCreateSearch
    {
        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSearchButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnGridSelectDoubleClick;

        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public int AccountNumber
        {
            get 
            {
                if (!string.IsNullOrEmpty(txtAccountNumber.Text))
                    return Convert.ToInt32(txtAccountNumber.Text);
                else
                    return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int AccountTypeDropDown
        {
            set { ddlAccountType.SelectedValue = value.ToString(); }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AccountTypeEnabled
        {
            set { ddlAccountType.Enabled = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set { SubmitButton.Enabled = value; }
        }

        #endregion

        #region Methods

        public void SearchViewCustomSetUp()
        {
            txtAccountNumber.Attributes["onkeypress"] = String.Format("doSearch('{0}')", SearchButton.ClientID);
        }

        public void BindSearchResultsGrid(DataTable SearchResults)
        {
            CapSearchGrid.Columns.Clear();
            CapSearchGrid.AddGridBoundColumn("LegalEntityName", "Legal Entity Name", Unit.Percentage(30), HorizontalAlign.Left, true);
            CapSearchGrid.AddGridBoundColumn("LENumber", "ID/Company Number", Unit.Percentage(20), HorizontalAlign.Left, true);
            CapSearchGrid.AddGridBoundColumn("AccountNumber", "Account", Unit.Percentage(15), HorizontalAlign.Left, true);
            CapSearchGrid.AddGridBoundColumn("RoleType", "Role", Unit.Percentage(15), HorizontalAlign.Left, true);
            CapSearchGrid.AddGridBoundColumn("AccountType", "Account Type", Unit.Percentage(15), HorizontalAlign.Left, true);
            CapSearchGrid.DataSource = SearchResults;
            CapSearchGrid.DataBind();
        }

        public void BindAccountTypeDropdown()
        {
            Dictionary<int, string> _accountTypeDict = new Dictionary<int, string>();
            _accountTypeDict.Add((int)AccountTypes.HOC, AccountTypes.HOC.ToString());
            _accountTypeDict.Add((int)AccountTypes.Life, AccountTypes.Life.ToString());
            _accountTypeDict.Add((int)AccountTypes.MortgageLoan, AccountTypes.MortgageLoan.ToString());

            ddlAccountType.DataSource = _accountTypeDict;
            ddlAccountType.DataTextField = "Value";
            ddlAccountType.DataValueField = "Key";
            ddlAccountType.DataBind();
        }

        #endregion

        #region Event Handlers

        protected void SearchGridDoubleClick(object sender, GridSelectEventArgs e)
        {
            if (CapSearchGrid.SelectedIndex > -1 && OnGridSelectDoubleClick != null)
                OnGridSelectDoubleClick(sender, new KeyChangedEventArgs(CapSearchGrid.SelectedIndex));
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (OnSearchButtonClicked != null)
                OnSearchButtonClicked(sender, e);
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if ( OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (CapSearchGrid.SelectedIndex > -1  && OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        #endregion
    }
}
