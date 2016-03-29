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
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Web.Controls;
using System.Collections.Generic;
using SAHL.Web.AJAX;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Security;


namespace SAHL.Web.Views.Common
{
    public partial class ClientSuperSearch : SAHLCommonBaseView, IClientSuperSearch
    {
        IEventList<ILegalEntity> _legalEntities;
        private int rowId;
        protected const string CookieSearchType = "ClientSuperSearchType";
        protected bool addToCBO;

        enum LegalEntityType
        {
            Both,
            Person,
            Company
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterWebService(ServiceConstants.LegalEntity);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            txtSearch.Attributes["onkeypress"] = String.Format("doSearch('{0}')", btnSearchAdvanced.ClientID);
            txtFirstName.Attributes["onkeypress"] = String.Format("doSearch('{0}')", btnSearchBasic.ClientID);
            txtSurname.Attributes["onkeypress"] = String.Format("doSearch('{0}')", btnSearchBasic.ClientID);
            txtID.Attributes["onkeypress"] = String.Format("doSearch('{0}')", btnSearchBasic.ClientID);
            txtAccountKey.Attributes["onkeypress"] = String.Format("doSearch('{0}')", btnSearchBasic.ClientID);

            int leCount = (_legalEntities == null ? 0 : _legalEntities.Count);
            lblTip.Visible = (leCount > 0);

            // tabAdvanced.Style.Add(HtmlTextWriterStyle.Visibility, (hidAdvanced.Value == "0" ? "hidden" : "visible"));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AddTrace(this, "checkSecurityStart");
            if (SecurityHelper.CheckSecurity(gridSearchResults.SecurityTag, this))
                addToCBO = true;
            AddTrace(this, "checkSecurityEnd");

            if (IsPostBack && !String.IsNullOrEmpty(Request.Form["hidAppKey"]) && ApplicationClicked != null)
            {
                int appKey = Int32.Parse(Request.Form["hidAppKey"]);
                ApplicationClicked(this, new KeyChangedEventArgs(appKey));
            }
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            base.RaisePostBackEvent(sourceControl, eventArgument);
            if (sourceControl == gridSearchResults)
            {
                int leKey;
                if (Int32.TryParse(eventArgument, out leKey))
                {
                    this.ClientSelectedClicked(sourceControl, new ClientSuperSearchSelectedEventArgs(leKey));
                }
            }
        }

        #region IClientSuperSearch Members

        public event EventHandler<EventArgs> SearchClientClicked;

        /// <summary>
        /// Raised when an application is clicked within client details.
        /// </summary>
        public event KeyChangedEventHandler ApplicationClicked;

        public event EventHandler<ClientSuperSearchSelectedEventArgs> ClientSelectedClicked;

        public event EventHandler CreateNewClientClicked;

        public event EventHandler CancelClicked;

        public bool SearchButtonVisible
        {
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public bool CreateNewClientButtonVisible
        {
            set { this.btnNewLegalEntity.Visible = value; }
        }

        public string CreateNewClientButtonText
        {
            set { this.btnNewLegalEntity.Text = value; }
        }

        public bool AccountTypesVisible
        {
            set
            {
                cellAccountType.Visible = value;
                cellAccountTypeLabel.Visible = value;
            }
        }

        public bool CancelButtonVisible
        {
            set { this.btnCancel.Visible = value; }
        }

        public void BindAccountTypes(System.Collections.Generic.IDictionary<int, string> AccountTypes)
        {
            foreach (KeyValuePair<int, string> AccType in AccountTypes)
            {
                cbxAccountType.Items.Add(new ListItem(AccType.Value, AccType.Key.ToString()));
            }
        }

        public void BindSearchResults(IEventList<SAHL.Common.BusinessModel.Interfaces.ILegalEntity> legalEntities)
        {
            _legalEntities = legalEntities;

            gridSearchResults.Columns.Clear();
            gridSearchResults.AutoGenerateColumns = false;
            gridSearchResults.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            gridSearchResults.AddGridBoundColumn("", "", Unit.Percentage(100), HorizontalAlign.Left, true);

            gridSearchResults.DataSource = legalEntities;
            gridSearchResults.DataBind();

        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (SearchClientClicked != null)
            {
                SearchClientClicked(this, e);
            }
        }

        private void BuildRowContents(GridViewRow row, string key, string nameText, string numberText, bool displayExtraInfo)
        {
            // create an outer div that will be the container for
            HtmlGenericControl divOuter = new HtmlGenericControl("div");

            // create the top row, which is the row that is visible in the search
            HtmlGenericControl divRow = new HtmlGenericControl("div");
            divRow.Attributes.Add("class", "row");
            divOuter.Controls.Add(divRow);

            // display name
            HtmlGenericControl divDisplayName = new HtmlGenericControl("div");
            divRow.Controls.Add(divDisplayName);
            divDisplayName.Attributes.Add("class", "cell");
            divDisplayName.Style.Add(HtmlTextWriterStyle.Width, "45%");
            divDisplayName.Style.Add(HtmlTextWriterStyle.Padding, "2px 4px 2px 4px");

            if (row.RowType == DataControlRowType.DataRow)
            {
                string url = Page.ClientScript.GetPostBackEventReference(gridSearchResults, key);

                StringBuilder sb = new StringBuilder();

                sb.Append(@"<span style=""float:left;""><a href=""#"" onclick=""toggleExtraInfo(this, '" + key + @"')""><img src=""../../Images/arrow_blue_right.gif"" /></a></span>
                    <span style=""float:left;margin-left:3px;padding:2px;"">");
                
                //#15584 check if the user is allowed to add to the CBO
                if (addToCBO)
                    sb.Append(@"<a href=""javascript:" + url + @""" title=""Select " + nameText + @""" class=""underline"">" + nameText + "</a></span>");
                else
                    sb.Append(nameText + "</span>");

                divDisplayName.InnerHtml = sb.ToString();
            }
            else
                divDisplayName.InnerHtml = String.Format(@"<span style=""float:left;width:16px;"">&nbsp;</span><span style=""float:left;margin-left:3px;"">{0}</span>", nameText);

            // display number
            HtmlGenericControl divLegalNumber = new HtmlGenericControl("div");
            divRow.Controls.Add(divLegalNumber);
            divLegalNumber.Attributes.Add("class", "cell");
            divLegalNumber.Style.Add(HtmlTextWriterStyle.Width, "45%");
            divLegalNumber.Style.Add(HtmlTextWriterStyle.Padding, "2px 4px 2px 4px");
            //string html = @"<span style=""float:left;"">{0}</span><span style=""float:right;display:none;""><a href=""#"" onclick=""cancelToggle();{1};"">Select</a></span>";
            // divLegalNumber.InnerHtml = String.Format(html, numberText, url);
            divLegalNumber.InnerHtml = @"<span style=""padding:2px;"">" + numberText + "</span>";

            if (displayExtraInfo)
            {
                // div that will display the extra info
                HtmlGenericControl divExtraInfo = new HtmlGenericControl("div");
                divExtraInfo.Attributes.Add("class", "row");
                divExtraInfo.Style.Add(HtmlTextWriterStyle.Padding, "5px 4px 5px 4px");
                divExtraInfo.Style.Add(HtmlTextWriterStyle.Display, "none");
                divExtraInfo.InnerText = "";
                divOuter.Controls.Add(divExtraInfo);

                // add client click handler - set an ID on the cell so we have something to work with on the client
                row.Cells[1].ID = String.Format("CS_Row_{0}", rowId++);
                //row.Cells[1].Attributes["onclick"] = String.Format("toggleExtraInfo(this, {0})", key);
                //row.Cells[1].Attributes["ondblclick"] = "cancelToggle()";
            }

            row.Cells[1].Controls.Add(divOuter);
        }

        protected void SearchGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Style.Add(HtmlTextWriterStyle.Padding, "0px");
            if (e.Row.RowType == DataControlRowType.Header)
            {
                BuildRowContents(e.Row, "", "Name", "ID / Company Number", false);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ILegalEntity legalEntity = e.Row.DataItem as ILegalEntity;
                BuildRowContents(e.Row, legalEntity.Key.ToString(), legalEntity.DisplayName, legalEntity.LegalNumber, true);

            }
        }

        protected void SearchGrid_GridDoubleClick(object sender, SAHL.Common.Web.UI.Controls.GridSelectEventArgs e)
        {
            if (null != ClientSelectedClicked)
            {
                int selectedRowIndex = e.m_RowNum;
                if (selectedRowIndex != -1)
                {
                    ILegalEntity le = _legalEntities[selectedRowIndex];
                    this.ClientSelectedClicked(this, new ClientSuperSearchSelectedEventArgs(le.Key));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNewLegalEntity_Click(object sender, EventArgs e)
        {
            CreateNewClientClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if (null != ClientSelectedClicked)
            {
                int SelectedRowIndex = gridSearchResults.SelectedIndex;
                if (SelectedRowIndex != -1)
                {
                    ILegalEntity LE = _legalEntities[SelectedRowIndex];
                    this.ClientSelectedClicked(sender, new ClientSuperSearchSelectedEventArgs(LE.Key));
                }
            }
        }

        /// <summary>
        /// Gets the search criteria captured.  If the search type is advanced or basic this will be populated, 
        /// for simple name searches this will return null.
        /// </summary>
        /// <seealso cref="ClientSearchCriteria"/>
        public IClientSuperSearchCriteria ClientSuperSearchCriteria
        {
            get
            {
                if (hidSearch.Value != "ADVANCED")
                    return null;

                string LETypes = "";
                string ACCTypes = ""; // added by CF  : 31/01/2008
                switch (ddlLeType.SelectedIndex)
                {
                    case 0:
                        LETypes = ((int)LegalEntityTypes.NaturalPerson).ToString();
                        break;
                    case 1:
                        LETypes = ((int)LegalEntityTypes.Company).ToString() + "," + ((int)LegalEntityTypes.CloseCorporation).ToString() + "," + ((int)LegalEntityTypes.Trust).ToString();
                        break;
                    case 2:
                        LETypes = ((int)LegalEntityTypes.NaturalPerson).ToString() + "," + ((int)LegalEntityTypes.Company).ToString() + "," + ((int)LegalEntityTypes.CloseCorporation).ToString() + "," + ((int)LegalEntityTypes.Trust).ToString();
                        break;
                }

                if (cbxAccountType.SelectedIndex == 0)
                    ACCTypes = "1,4,5";
                else if (cbxAccountType.SelectedIndex == -1)
                    ACCTypes = "None";
                else
                    ACCTypes = cbxAccountType.SelectedValue;

                return new ClientSuperSearchCriteria(txtSearch.Text, ACCTypes, LETypes, CurrentPrincipal);

            }
        }

        /// <summary>
        /// Gets the search criteria captured.  If the search type is by name, this will be populated, for 
        /// basic and advanced searches this will return null.
        /// </summary>
        /// <seealso cref="ClientSuperSearchCriteria"/>
        public IClientSearchCriteria ClientSearchCriteria
        {
            get
            {
                if (hidSearch.Value != "BASIC")
                    return null;

                IClientSearchCriteria css = new ClientSearchCriteria();
                css.FirstNames = txtFirstName.Text.Trim();
                css.Surname = txtSurname.Text.Trim();
                css.IDNumber = txtID.Text.Trim();
                css.AccountNumber = txtAccountKey.Text.Trim();
                css.SalaryNumber = txtSalaryNumber.Text.Trim();
                return css;
            }
        }
    }
}
