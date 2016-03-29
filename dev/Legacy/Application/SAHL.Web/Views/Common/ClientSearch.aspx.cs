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
using SAHL.Web.Views.Common.Interfaces;
using NHibernate.Expression;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using System.Text;

namespace SAHL.Web.Views.Common
{
    public partial class ClientSearch : SAHLCommonBaseView, IClientSearch
    {
        bool _searchButtonVisible = true;
        // bool _clientSelectButtonVisible = true;
        bool _createNewClientButtonVisible; // = false;
        bool _cancelButtonVisible = true;
        IEventList<ILegalEntity> _legalEntities;
        // ClientSearchType _searchType;

        #region Page Life Cycle Events

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // show or hide buttons
            btnCancel.Visible = _cancelButtonVisible;
            btnNewLegalEntity.Visible = _createNewClientButtonVisible;
            btnCancel.Visible = _cancelButtonVisible;
            btnSearch.Visible = _searchButtonVisible;

            StringBuilder script = new StringBuilder();
            script.AppendLine("function pushTheButton()");
            script.AppendLine("{");
            script.AppendLine("    if ( window.event.keyCode == 13 )");
            script.AppendLine("    {");
            script.AppendLine("        var o = document.getElementById('" + btnSearch.ClientID + "');");
            script.AppendLine("        if ( o != null );");
            script.AppendLine("            o.click();");
            script.AppendLine("    }");
            script.AppendLine("}");
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), this.UniqueID, script.ToString(), true);

            AccountNumber.Focus();
        }

        #endregion

        #region Control Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            if (!Page.IsValid)
                return;

            IClientSearchCriteria CSC = TypeFactory.CreateType<IClientSearchCriteria>();
            // build our list of search criteria
            if (SearchClientClicked != null)
            {
                if(cbxSearchType.SelectedItem.Text == "Natural Person")
                    CSC.SearchType = ClientSearchType.NaturalPerson;
                else
                    CSC.SearchType = ClientSearchType.Company;

                if (CSC.SearchType == ClientSearchType.NaturalPerson)
                {
                    CSC.Surname = Surname.Text.Trim();
                    CSC.SalaryNo = PersalNumber.Text.Trim();

                    if (NameType.Text == "First Names")
                    {
                        CSC.FirstNames = SearchName.Text.Trim();
                        CSC.PreferredName = "";
                    }
                    else
                    {
                        CSC.PreferredName = SearchName.Text.Trim();
                        CSC.FirstNames = "";
                    }

                    if (IDType.Text == "ID Number")
                    {
                        CSC.IDNumber = IDNumber.Text.Trim();
                        CSC.PassportNumber = "";
                    }
                    else
                    {
                        CSC.PassportNumber = Passport.Text.Trim();
                        CSC.IDNumber = "";
                    }
                }
                else
                {
                    CSC.AccountNumber = Convert.ToInt32(AccountNumber.Text.Trim());
                    CSC.CompanyNumber = CompanyNumber.Text.Trim();

                    if (CompNameType.Text == "Registered Name")
                    {
                        CSC.CompanyRegisteredName = CompanyName.Text.Trim();
                        CSC.CompanyTradingName = "";
                    }
                    else
                    {
                        CSC.CompanyTradingName = CompanyName.Text.Trim();
                        CSC.CompanyRegisteredName = "";
                    }
                }

                if ((CSC.FirstNames.Length == 0) && (CSC.Surname.Length == 0) && (CSC.PreferredName.Length == 0) && (CSC.IDNumber.Length == 0) && (CSC.PassportNumber.Length == 0) && (CSC.SalaryNo.Length == 0))
                    CSC.SearchType = ClientSearchType.AccountOnly;

                // fire our event
                SearchClientClicked(sender, new ClientSearchClickedEventArgs(CSC));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if(CancelClicked != null)
                CancelClicked(sender, e);
        }

        protected void btnNewLegalEntity_Click(object sender, EventArgs e)
        {

        }

        protected void Select_Click(object sender, EventArgs e)
        {
            if (null != ClientSelectedClicked)
            {
                if (null != SearchGrid.SelectedRow)
                {
                    ILegalEntity LE = SearchGrid.SelectedRow.DataItem as ILegalEntity;
                    this.ClientSelectedClicked(this, new ClientSearchSelectedEventArgs(LE.Key));
                }
            }
        }

        protected void SearchGrid_GridDoubleClick(object sender, SAHL.Common.Web.UI.Controls.GridSelectEventArgs e)
        {
            if (null != ClientSelectedClicked)
            {
                if (null != SearchGrid.SelectedRow)
                {
                    int LEIndex = SearchGrid.SelectedRow.DataItemIndex;
                    ILegalEntity LE = _legalEntities[LEIndex];
                    this.ClientSelectedClicked(this, new ClientSearchSelectedEventArgs(LE.Key));
                }
            }
        }

        #endregion

        #region Data Events

        protected void SearchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


        #endregion

        #region Validation

        protected void ValidateSearch(object source, ServerValidateEventArgs args)
        {

        }

        #endregion

        #region IClientSearch Members

        public event EventHandler<ClientSearchClickedEventArgs> SearchClientClicked;

        public event EventHandler<ClientSearchSelectedEventArgs> ClientSelectedClicked;

        public event EventHandler CreateNewClientClicked;

        public event EventHandler CancelClicked;
        
        public event EventHandler TMPClickHandler;

        public bool SearchButtonVisible
        {
            set { _searchButtonVisible = value; }
        }

        //public bool ClientSelectButtonVisible
        //{
        //    set { _clientSelectButtonVisible = value; }
        //}

        public bool CreateNewClientButtonVisible
        {
            set { _createNewClientButtonVisible = value; }
        }

        public bool CancelButtonVisible
        {
            set { _cancelButtonVisible = value; }
        }

        public void BindAccountTypes(IDictionary<int, string> AccountTypes)
        {
            AccountType.DataSource = AccountTypes;
            AccountType.DataBind();
        }

        public void BindSearchResults(IEventList<ILegalEntity> legalEntities, ClientSearchType searchType)
        {
            _legalEntities = legalEntities;
            // _searchType = searchType;

            SearchGrid.Columns.Clear();
            SearchGrid.AutoGenerateColumns = false;
            SearchGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            SearchGrid.AddGridBoundColumn("DisplayName", "Legal Entity Name", Unit.Percentage(40), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("IDNumber", "ID / Company Number", Unit.Percentage(26), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Account", Unit.Percentage(17), HorizontalAlign.Left, true);
            SearchGrid.AddGridBoundColumn("", "Role", Unit.Percentage(17), HorizontalAlign.Left, true);
            SearchGrid.DataSource = legalEntities;
            SearchGrid.DataBind();
            
        }

        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (TMPClickHandler != null)
                TMPClickHandler(this, null);
        }
    }
}
