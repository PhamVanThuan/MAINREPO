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
using System.Drawing;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Globals;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SAHL.Web.Views.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RelatedLegalEntity : SAHLCommonBaseView, IRelatedLegalEntity
    {

        private IEventList<IRole> _legalEntityRoles;
        private bool _addToMenuButtonEnabled, _removeButtonEnabled, _cancelButtonEnabled, _allowGridSelect, _allowGridDoubleClick;

        private enum GridColumnPositions
        {
            Name = 0,
            IDCompanyNumber = 1,
            AccountNumber = 2,
            Product = 3,
            Role = 4,
            Status = 5,
            LegalEntityKey = 6
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {         

        }

        protected override void OnPreRender(EventArgs e)
        {
            
            base.OnPreRender(e);
            if (!ShouldRunPage)
                return;

            AddToMenuButton.Visible = _addToMenuButtonEnabled;
            RemoveButton.Visible = _removeButtonEnabled;
            CancelButton.Visible = _cancelButtonEnabled;

            // if we in 'remove' mode then 
            if (RemoveButton.Visible)
            {
                // 1. setup the javascript for the remove button
                RegisterClientJavascript();
                RemoveButton.Attributes.Add("onclick", "return ConfirmRemove()");

                // 2. only enable the remove button if the currently selected row is an active suretor
                int SelectedIndex = RelatedLEGrid.SelectedIndex;
                RemoveButton.Enabled = false;
                if (SelectedIndex >= 0)
                {
                    string role = RelatedLEGrid.SelectedRow.Cells[(int)GridColumnPositions.Role].Text;
                    string status = RelatedLEGrid.SelectedRow.Cells[(int)GridColumnPositions.Status].Text;
                    if (String.Compare(role,SAHL.Common.Globals.RoleTypes.Suretor.ToString(), true) == 0
                        && String.Compare(status, SAHL.Common.Globals.GeneralStatuses.Active.ToString(), true) == 0)
                    {
                        RemoveButton.Enabled = true;
                    }
                }
            }
        }

        protected void RelatedLEGrid_RowDataBound(Object s, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                IRole legalEntityRole = e.Row.DataItem as IRole;

                // Legal Name
                cells[(int)GridColumnPositions.Name].Text = legalEntityRole.LegalEntity.GetLegalName(LegalNameFormat.Full);
                
                // ID/Company Number
                string idCompanyNumber = "";
                switch ((LegalEntityTypes)legalEntityRole.LegalEntity.LegalEntityType.Key)
                {
                    case LegalEntityTypes.NaturalPerson:
                        idCompanyNumber = ((ILegalEntityNaturalPerson)legalEntityRole.LegalEntity).IDNumber;
                        break;

                    case LegalEntityTypes.CloseCorporation:
                        idCompanyNumber = ((ILegalEntityCloseCorporation)legalEntityRole.LegalEntity).RegistrationNumber;
                        break;

                    case LegalEntityTypes.Company:
                        idCompanyNumber = ((ILegalEntityCompany)legalEntityRole.LegalEntity).RegistrationNumber;
                        break;

                    case LegalEntityTypes.Trust:
                        idCompanyNumber = ((ILegalEntityTrust)legalEntityRole.LegalEntity).RegistrationNumber;
                        break;

                    default:
                        break;
                }
                cells[(int)GridColumnPositions.IDCompanyNumber].Text = idCompanyNumber;
          
                // Account Number
                cells[(int)GridColumnPositions.AccountNumber].Text = legalEntityRole.Account.Key.ToString();

                // Product
                cells[(int)GridColumnPositions.Product].Text = legalEntityRole.Account.Product.Description;

                // Role
                cells[(int)GridColumnPositions.Role].Text = legalEntityRole.RoleType.Description;

                // Status
                cells[(int)GridColumnPositions.Status].Text = legalEntityRole.GeneralStatus.Description;
                if (legalEntityRole.GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Inactive))
                    cells[(int)GridColumnPositions.Status].BackColor = Color.Salmon;
            }
        }

        protected void AddToMenuButton_Click(object sender, EventArgs e)
        {
            int SelectedIndex = RelatedLEGrid.SelectedIndex;

            if (SelectedIndex > -1 && RelatedLEGrid.SelectedIndex < _legalEntityRoles.Count)
            {
                // Get the Key and pass it
                OnSelectLegalEntity(sender, new KeyChangedEventArgs(_legalEntityRoles[SelectedIndex].LegalEntity.Key));
            }
        }

        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            int SelectedIndex = RelatedLEGrid.SelectedIndex;

            if (SelectedIndex > -1 && RelatedLEGrid.SelectedIndex < _legalEntityRoles.Count)
            {
                // Get the Key and pass it
                OnRemoveButtonClicked(sender, new KeyChangedEventArgs(_legalEntityRoles[SelectedIndex].Key));
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        protected void RelatedLEGrid_GridDoubleClick(object sender, GridSelectEventArgs e)
        {

            int SelectedIndex = RelatedLEGrid.SelectedIndex;

            if (SelectedIndex > -1 && RelatedLEGrid.SelectedIndex < _legalEntityRoles.Count)
            {
                // Get the Key and pass it
                OnSelectLegalEntity(sender, new KeyChangedEventArgs(_legalEntityRoles[SelectedIndex].LegalEntity.Key));
            }
        }

        private void BindData()
        {
            RelatedLEGrid.PostBackType = GridPostBackType.None;
            if (_allowGridDoubleClick)
                RelatedLEGrid.PostBackType = GridPostBackType.DoubleClickWithClientSelect;
            else if (_allowGridSelect)
                RelatedLEGrid.PostBackType = GridPostBackType.SingleClick;

            // Create columns and bind th grid
            RelatedLEGrid.Columns.Clear();

            RelatedLEGrid.AddGridBoundColumn("", "Name", Unit.Percentage(30), HorizontalAlign.Left, true);
            RelatedLEGrid.AddGridBoundColumn("", "ID / Company Number", Unit.Percentage(18), HorizontalAlign.Left, true);
            RelatedLEGrid.AddGridBoundColumn("", "Account Number", Unit.Percentage(10), HorizontalAlign.Left, true);
            RelatedLEGrid.AddGridBoundColumn("", "Product", Unit.Percentage(18), HorizontalAlign.Left, true);
            RelatedLEGrid.AddGridBoundColumn("", "Role", Unit.Percentage(14), HorizontalAlign.Left, true);
            RelatedLEGrid.AddGridBoundColumn("", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);
            RelatedLEGrid.AddGridBoundColumn("", "LegalEntityKey", Unit.Percentage(0), HorizontalAlign.Left, false);

            RelatedLEGrid.DataSource = _legalEntityRoles;
            RelatedLEGrid.DataBind();
        }

        private void RegisterClientJavascript()
        {
            //string legalEntityName = RelatedLEGrid.SelectedRow.Cells[0].ToString();

            StringBuilder sbJavascript = new StringBuilder();
            sbJavascript.AppendLine("function ConfirmRemove ()");
            sbJavascript.AppendLine("{");
            //sbJavascript.AppendLine("return confirm('Are you sure you want to remove \"" + legalEntityName + "\" as Suretor ?');");
            sbJavascript.AppendLine("return confirm('Are you sure you want to remove this Suretor ?');");
            sbJavascript.AppendLine("}");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("ConfirmRemove"))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ConfirmRemove", sbJavascript.ToString(), true);
        }

        #region IRelatedLegalEntity Members

        public event KeyChangedEventHandler OnSelectLegalEntity;
        public event KeyChangedEventHandler OnRemoveButtonClicked;
        public event EventHandler OnCancelButtonClicked;


        /// <summary>
        /// see <see cref="IRelatedLegalEntity.BindLegalEntityGrid"/>
        /// </summary>
        /// <param name="LegalEntityRoles"></param>
        public void BindLegalEntityGrid(IEventList<IRole> LegalEntityRoles)
        {
            _legalEntityRoles = LegalEntityRoles;

            BindData();
        }

        /// <summary>
        /// See <see cref="IRelatedLegalEntity.AddToMenuButtonEnabled"/>
        /// </summary>
        public bool AddToMenuButtonEnabled
        {
            set 
            {
                _addToMenuButtonEnabled = value;
            }
        }

        /// <summary>
        /// See <see cref="IRelatedLegalEntity.RemoveButtonEnabled"/>
        /// </summary>
        public bool RemoveButtonEnabled
        {
            set
            {
                _removeButtonEnabled = value;
            }
        }
        /// <summary>
        /// See <see cref="IRelatedLegalEntity.CancelButtonEnabled"/>
        /// </summary>
        public bool CancelButtonEnabled
        {
            set
            {
                _cancelButtonEnabled = value;
            }
        }

        /// <summary>
        /// See <see cref="IRelatedLegalEntity.AllowGridSelect"/>
        /// </summary>
        public bool AllowGridSelect
        {
            set
            {
                _allowGridSelect = value;
            }
        }

        /// <summary>
        /// See <see cref="IRelatedLegalEntity.AllowGridDoubleClick"/>
        /// </summary>
        public bool AllowGridDoubleClick
        {
            set
            {
                _allowGridDoubleClick = value;
            }
        }
        #endregion


    }
}