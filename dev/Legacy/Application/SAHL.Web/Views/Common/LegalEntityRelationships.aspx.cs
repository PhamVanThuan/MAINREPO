using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Authentication;
using SAHL.Common;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.AJAX;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Diagnostics.CodeAnalysis;


namespace SAHL.Web.Views.Common
{
    public partial class LegalEntityRelationships : SAHLCommonBaseView, ILegalEntityRelationships
    {

        #region Private Structures

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private bool _addToCBOButtonVisible = true;
        private bool _cancelButtonVisible = true;
        private bool _submitButtonVisible = true;
        private bool _actionTableVisible = true;
        private bool _legalEntityInfoTableVisible = true;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private bool _addToCBOButtonEnabled = true;
        private bool _submitButtonEnabled = true;
        private GridPostBackType _gridPostBackType;
        private string _submitButtonText = "";
        private string _messageText;
        private IEventList<ILegalEntityRelationship> _legalEntityRelationships;
        private bool _deleteConfirmationVisible;

        private enum GridColumnsIndex
        {
            LegalEntityName = 0,
            PreferredTradingName = 1,
            IDPassPortNumber = 2,
            Relationship = 3
        }

        #endregion

        #region Constants
        const string DEFAULTDROPDOWNITEM = "-select-";
        #endregion

        #region Protected Functions Section


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage) return;

            btnAddToCbo.Visible = _addToCBOButtonVisible;
            btnCancelButton.Visible = _cancelButtonVisible;
            btnSubmitButton.Visible = _submitButtonVisible;
            btnSubmitButton.Enabled = _submitButtonEnabled;
            tblActionTable.Visible = _actionTableVisible;
            tblLEInfo.Visible = _legalEntityInfoTableVisible;
            btnSubmitButton.Text = _submitButtonText;
            lblMessage.Text = _messageText;

            if (_deleteConfirmationVisible)
                btnSubmitButton.Attributes["onclick"] = "if(!confirm('Are you sure you want to delete this item?')) return false";
        }

        #endregion

        protected void grdRelatedLegalEntities_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            int rowIndex;
            string legalEntityName = String.Empty;
            string legalEntityAlias = String.Empty;
            string idPassportNumber = String.Empty;

            ILegalEntityRelationship legalEntityRelationship;
            TableCellCollection cells = e.Row.Cells;
            rowIndex = e.Row.RowIndex;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                legalEntityRelationship = _legalEntityRelationships[rowIndex];

                legalEntityName = legalEntityRelationship.RelatedLegalEntity.GetLegalName(LegalNameFormat.Full);
                switch ((LegalEntityTypes)legalEntityRelationship.RelatedLegalEntity.LegalEntityType.Key)
                {
                    case LegalEntityTypes.CloseCorporation:
                        {
                            ILegalEntityCloseCorporation leCompany = legalEntityRelationship.RelatedLegalEntity as ILegalEntityCloseCorporation;
                            legalEntityAlias = leCompany.TradingName == null ? String.Empty : leCompany.TradingName;
                            idPassportNumber = leCompany.RegistrationNumber == null ? String.Empty : leCompany.RegistrationNumber;
                        }
                        break;

                    case LegalEntityTypes.Company:
                        {
                            ILegalEntityCompany leCompany = legalEntityRelationship.RelatedLegalEntity as ILegalEntityCompany;
                            legalEntityAlias = leCompany.TradingName == null ? String.Empty : leCompany.TradingName;
                            idPassportNumber = leCompany.RegistrationNumber == null ? String.Empty : leCompany.RegistrationNumber;
                        }
                        break;

                    case LegalEntityTypes.NaturalPerson:
                        {
                            ILegalEntityNaturalPerson leNaturalPerson = legalEntityRelationship.RelatedLegalEntity as ILegalEntityNaturalPerson;
                            legalEntityAlias = leNaturalPerson.PreferredName == null ? String.Empty : leNaturalPerson.PreferredName;
                            idPassportNumber = leNaturalPerson.IDNumber == null ? String.Empty : leNaturalPerson.IDNumber;
                            if (String.IsNullOrEmpty(idPassportNumber))
                                idPassportNumber = leNaturalPerson.PassportNumber == null ? String.Empty : leNaturalPerson.PassportNumber;
                        }
                        break;

                    case LegalEntityTypes.Trust:
                        {
                            ILegalEntityTrust leCompany = legalEntityRelationship.RelatedLegalEntity as ILegalEntityTrust;
                            legalEntityAlias = leCompany.TradingName == null ? String.Empty : leCompany.TradingName;
                            idPassportNumber = leCompany.RegistrationNumber == null ? String.Empty : leCompany.RegistrationNumber;
                        }
                        break;

                    case LegalEntityTypes.Unknown:
                        break;

                    default:
                        break;
                }

                cells[(int)GridColumnsIndex.LegalEntityName].Text = legalEntityName;
                cells[(int)GridColumnsIndex.PreferredTradingName].Text = legalEntityAlias;
                cells[(int)GridColumnsIndex.IDPassPortNumber].Text = idPassportNumber;
                cells[(int)GridColumnsIndex.Relationship].Text = legalEntityRelationship.LegalEntityRelationshipType.Description;
            }
        }

        protected void btnCancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClick(sender, e);
        }

        protected void btnSubmitButton_Click(object sender, EventArgs e)
        {

            int selectedRelationshipTypeKey = Convert.ToInt32(ddlRelationshipType.SelectedIndex);

            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(selectedRelationshipTypeKey);

            OnSubmitButtonClick(sender, keyChangedEventArgs);

        }

        protected void grdRelatedLegalEntities_GridDoubleClick(object sender, GridSelectEventArgs e)
        {
            btnAddToCbo_Click(sender, null);
        }

        protected void btnAddToCbo_Click(object sender, EventArgs e)
        {
            string key = String.Empty;

            if (grdRelatedLegalEntities.SelectedIndex > -1)
                key = _legalEntityRelationships[grdRelatedLegalEntities.SelectedIndex].RelatedLegalEntity.Key.ToString();

            OnAddToCBO(sender, new KeyChangedEventArgs(key));
        }

        #region ILegalEntityRelationships Members

        public bool AddToCBOButtonVisible
        {
            set { _addToCBOButtonVisible = value; }
        }

        public bool CancelButtonVisible
        {
            set { _cancelButtonVisible = value; }
        }

        public bool SubmitButtonVisible
        {
            set { _submitButtonVisible = value; }
        }

        public bool ActionTableVisible
        {
            set { _actionTableVisible = value; }
        }

        public bool LegalEntityInfoTableVisible
        {
            set { _legalEntityInfoTableVisible = value; }
        }

        public bool AddToCBOButtonEnabled
        {
            set { _addToCBOButtonEnabled = value ; }
        }

        public bool SubmitButtonEnabled
        {
            set { _submitButtonEnabled = value ; }
        }

        public GridPostBackType GridPostBackType
        {
            set { _gridPostBackType = value; }
        }

        public string SubmitButtonText
        {
            set { _submitButtonText = value; }
        }

        public int SelectedLegalEntityRelationshipTypeKey
        {
            get 
            {
                return ddlRelationshipType.SelectedIndex;
            }
        }

        public int SelectedLegalEntityRelationshipIndex
        {
            get
            {
                return grdRelatedLegalEntities.SelectedIndex;
            }
            set
            {
                grdRelatedLegalEntities.SelectedIndex = value;
            }
        }

        public void BindRelationshipTypes(IDictionary<string, string> relationshipTypes, string defaultValue)
        {
            PopulateDropDown(ddlRelationshipType, relationshipTypes, defaultValue);
        }

        public void BindLabelMessage(string messageText)
        {
            _messageText = messageText;
        }

        public void BindRelationshipGrid(IEventList<ILegalEntityRelationship> legalEntityRelationships)
        {
            grdRelatedLegalEntities.PostBackType = _gridPostBackType;
            if (!IsPostBack)
                grdRelatedLegalEntities.SelectedIndex = 0;

            grdRelatedLegalEntities.Columns.Clear();

            _legalEntityRelationships = legalEntityRelationships;

            grdRelatedLegalEntities.AddGridBoundColumn("", "Legal Entity Name", Unit.Percentage(0), HorizontalAlign.Left, true);
            grdRelatedLegalEntities.AddGridBoundColumn("", "Preferred Name/Trading Name", Unit.Percentage(0), HorizontalAlign.Left, true);
            grdRelatedLegalEntities.AddGridBoundColumn("", "ID/Company Number ", Unit.Percentage(0), HorizontalAlign.Left, true);
            grdRelatedLegalEntities.AddGridBoundColumn("", "Relationship", Unit.Percentage(0), HorizontalAlign.Left, true);

            grdRelatedLegalEntities.DataSource = _legalEntityRelationships;
            grdRelatedLegalEntities.DataBind();

        }

        public event KeyChangedEventHandler OnSubmitButtonClick;

        public event EventHandler OnCancelButtonClick;

        public event KeyChangedEventHandler OnAddToCBO;

        public event KeyChangedEventHandler OnGridItemSelected;

        public bool DeleteConfirmationVisible
        {
            set { _deleteConfirmationVisible = value; }
        }
        #endregion

        #region Private Helper Functions

        private static void PopulateDropDown(SAHLDropDownList dropDownList, IDictionary<string, string> dataItems, string DefaultValue)
        {
            dropDownList.DataSource = dataItems;
            dropDownList.DataBind();
            dropDownList.VerifyPleaseSelect();

            // Set the default value if supplied
            if (!String.IsNullOrEmpty(DefaultValue))
                dropDownList.SelectedValue = DefaultValue;

        }
        #endregion


        protected void grdRelatedLegalEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = String.Empty;

            if (grdRelatedLegalEntities.SelectedIndex > -1)
                key = _legalEntityRelationships[grdRelatedLegalEntities.SelectedIndex].LegalEntityRelationshipType.Key.ToString();

            KeyChangedEventArgs keyChangedEventArgs = new KeyChangedEventArgs(key);

            OnGridItemSelected(sender, keyChangedEventArgs);
        }

    }
}