using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI.Controls;
using AjaxControlToolkit;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Use for capturing correspondence options when sending correspondence
    /// </summary>
    public class CorrespondenceGridOptionsPanel : SAHLPanel, INamingContainer
    {
        #region Private varibles
        private SAHLCheckbox _chkFax;
        private SAHLCheckbox _chkEmail;
        private SAHLCheckbox _chkPost;

        private SAHLPhone _txtFax;
        private SAHLTextBox _txtEmail;
        private SAHLDropDownList _ddlAddress;
        private SAHLLabel _lblAddress;
        #endregion

        #region Properties
        private ILegalEntity _legalEntity;
        private List<SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders.CorrespondenceMediums> _correspondenceMediums;
        private int _genericKey;
        private SAHL.Common.Globals.GenericKeyTypes _genericKeyType;
        private bool _showMailingAddress;

        private bool _horizontalStack;
        public bool HorizontalStack
        {
            get { return _horizontalStack; }
            set { _horizontalStack = value; }
        }

        /// <summary>
        /// Determines whether we are in design mode (standard DesignMode not reliable).
        /// </summary>
        protected static new bool DesignMode
        {
            get
            {
                return (HttpContext.Current == null);
            }
        }
        /// <summary>
        /// Get/set the LegalEntity
        /// </summary>
        public ILegalEntity LegalEntity
        {
            set { _legalEntity = value; }
            get { return _legalEntity; }
        }

        /// <summary>
        /// Get/set the Correspondence Mediums
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Fock Off.")]
        public List<SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders.CorrespondenceMediums> CorrespondenceMediums
        {
            set { _correspondenceMediums = value; }
        }

        /// <summary>
        /// Get/set the GenericKey
        /// </summary>
        public int GenericKey
        {
            set { _genericKey = value; }
            get { return _genericKey; }
        }

        /// <summary>
        /// GenericKeyType
        /// </summary>
        public SAHL.Common.Globals.GenericKeyTypes GenericKeyType
        {
            set { _genericKeyType = value; }
            get { return _genericKeyType; }
        }

        public bool ShowMailingAddress
        {
            set { _showMailingAddress = value; }
        }

        public IEventList<ILegalEntityAddress> LegalEntityAddresses { get; set; }
        public IAddress AccountMailingAddress { get; set; }

        public bool SelectedFax
        {
            get { return _chkFax.Checked; }
            set { _chkFax.Checked = value; }
        }

        public bool SelectedEmail
        {
            get { return _chkEmail.Checked; }
            set { _chkEmail.Checked = value; }
        }

        public bool SelectedPost
        {
            get { return _chkPost.Checked; }
            set { _chkPost.Checked = value; }
        }

        public string FaxCode
        {
            get { return _txtFax.Code; }
            set { _txtFax.Code = value; }
        }

        public string FaxNumber
        {
            get { return _txtFax.Number; }
            set { _txtFax.Number = value; }
        }

        public string EmailAddress
        {
            get { return _txtEmail.Text.Trim(); }
            set { _txtEmail.Text = value; }
        }

        public int AddressKey
        {
            get 
            {
                if (_ddlAddress.SelectedItem == null)
                    return -1;

                return Convert.ToInt32(_ddlAddress.SelectedItem.Value); 
            }
            set { _ddlAddress.SelectedValue = value.ToString(); }
        }

        private bool _setEmailOptionChecked;
        public bool SetEmailOptionChecked
        {
            get { return _setEmailOptionChecked; }
            set { _setEmailOptionChecked = value; }
        }

        private bool _disableCorrespondenceOptionEntry;
        public bool DisableCorrespondenceOptionEntry
        {
            get { return _disableCorrespondenceOptionEntry; }
            set { _disableCorrespondenceOptionEntry = value; }
        }

        public bool AddressParameterRequired { get; set; }

        public bool DisplayAttorneyAddresses { get; set; }

        public bool DisplayDebtCounsellorAddresses { get; set; }

        #endregion

        public CorrespondenceGridOptionsPanel()
        {
            _chkFax = new SAHLCheckbox();
            _chkFax.ID = "chkFax";
            _chkFax.Text = "Fax";

            _chkEmail = new SAHLCheckbox();
            _chkEmail.ID = "chkEmail";
            _chkEmail.Text = "Email";

            _chkPost = new SAHLCheckbox();
            _chkPost.ID = "chkPost";
            _chkPost.Text = "Post";

            _txtFax = new SAHLPhone();
            _txtFax.ID = "txtFax";

            _txtEmail = new SAHLTextBox();
            _txtEmail.ID = "txtEmail";
            _txtEmail.Width = 200;

            _ddlAddress = new SAHLDropDownList();
            _ddlAddress.ID = "ddlAddress";
            _ddlAddress.PleaseSelectItem = false;

            _lblAddress = new SAHLLabel();
            _lblAddress.ID = "lblAddress";
        }

        /// <summary>
        /// Populates the controls with the information supplied (though the LoanDetails property)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //setup panel details
            base.HorizontalAlign = HorizontalAlign.Left;
            base.Width = new Unit(100, UnitType.Percentage);
            base.CssClass = "correspondenceGridOptionsPanel";

            if (DesignMode)
                return;
        }

        /// <summary>
        /// Sets up the controls for render.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetupAllowedCorrespondenceMediums();

            if (_legalEntity != null)
                BindCorrespondenceMediumData();

            HtmlTable _htmlTable = new HtmlTable();
            _htmlTable.Width = "100%";
            _htmlTable.Attributes.Add("class", "tableStandard");

            HtmlTableCell spacerCell1 = new HtmlTableCell();
            spacerCell1.Width = "10px";

            HtmlTableCell spacerCell2 = new HtmlTableCell();
            spacerCell2.Width = "10px";

            HtmlTableCell faxCell1 = new HtmlTableCell();
            faxCell1.Attributes.Add("class", "faxCell1");
            faxCell1.Controls.Add(_chkFax);

            HtmlTableCell faxCellSpacer = new HtmlTableCell();
            faxCellSpacer.Attributes.Add("class", "faxCellSpacer");

            HtmlTableCell faxCell2 = new HtmlTableCell();
            faxCell2.Attributes.Add("class", "faxCell2");
            faxCell2.Controls.Add(_txtFax);

            HtmlTableCell emailCell1 = new HtmlTableCell();
            emailCell1.Attributes.Add("class", "emailCell1");
            emailCell1.Controls.Add(_chkEmail);

            HtmlTableCell emailCellSpacer = new HtmlTableCell();
            emailCellSpacer.Attributes.Add("class", "emailCellSpacer");

            HtmlTableCell emailCell2 = new HtmlTableCell();
            emailCell2.Attributes.Add("class", "emailCell2");
            emailCell2.Controls.Add(_txtEmail);

            HtmlTableCell postCell1 = new HtmlTableCell();
            postCell1.Attributes.Add("class", "postCell1");
            postCell1.Controls.Add(_chkPost);

            HtmlTableCell postCellSpacer = new HtmlTableCell();
            postCellSpacer.Attributes.Add("class", "postCellSpacer");

            HtmlTableCell postCell2 = new HtmlTableCell();
            postCell2.Attributes.Add("class", "postCell2");
            postCell2.Controls.Add(_ddlAddress);
            //postCell2.Controls.Add(_lblAddress);

            if (_horizontalStack)
            {
                HtmlTableRow row = new HtmlTableRow();
                row.Cells.Add(faxCell1);
                row.Cells.Add(faxCellSpacer);
                row.Cells.Add(faxCell2);
                row.Cells.Add(spacerCell1);
                row.Cells.Add(emailCell1);
                row.Cells.Add(emailCellSpacer);
                row.Cells.Add(emailCell2);
                row.Cells.Add(spacerCell2);
                row.Cells.Add(postCell1);
                row.Cells.Add(postCellSpacer);
                row.Cells.Add(postCell2);

                _htmlTable.Rows.Add(row);
            }
            else
            {
                #region Vertical Stack Layout
                HtmlTableRow _rowFax = new HtmlTableRow();
                _rowFax.ID = "rowFax";
                _rowFax.Cells.Add(faxCell1);
                _rowFax.Cells.Add(faxCell2);

                _rowFax.Cells.Add(emailCell1);
                _rowFax.Cells.Add(emailCell2);

                _htmlTable.Rows.Add(_rowFax);

                HtmlTableRow _rowPost = new HtmlTableRow();
                _rowPost.ID = "rowPost";
                _rowPost.Cells.Add(postCell1);

                postCell2.ColSpan = 4;
                _rowPost.Cells.Add(postCell2);
                _htmlTable.Rows.Add(_rowPost);
                #endregion
            }


            base.Controls.Add(_htmlTable);

        }

        protected override void OnPreRender(EventArgs e)
        {
            if (_disableCorrespondenceOptionEntry)
            {
                _txtEmail.Enabled = false;
                _txtFax.Enabled = false;
                _ddlAddress.Enabled = false;
                _lblAddress.Visible = false;
            }

            if (_setEmailOptionChecked)
            {
                _chkEmail.Checked = true;
            }
        }

        protected void SetupAllowedCorrespondenceMediums()
        {
            _chkFax.Enabled = false;
            _txtFax.ReadOnly = true;

            _chkEmail.Enabled = false;
            _txtEmail.ReadOnly = true;

            _chkPost.Enabled = false;
            _ddlAddress.Enabled = false;
            _lblAddress.Visible = false;

            // setup allowable correspondence mediums (based on first report in the list)
            if (_correspondenceMediums != null && _correspondenceMediums.Count > 0)
            {
                foreach (SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders.CorrespondenceMediums cm in _correspondenceMediums)
                {
                    switch (cm.CorrespondenceMediumKey)
                    {
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Post:
                            _chkPost.Enabled = true;
                            _ddlAddress.Enabled = true;
                            _lblAddress.Visible = true;
                            break;
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Email:
                            _chkEmail.Enabled = true;
                            _txtEmail.ReadOnly = false;
                            break;
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Fax:
                            _chkFax.Enabled = true;
                            _txtFax.ReadOnly = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void BindCorrespondenceMediumData()
        {
            _txtEmail.Text = _legalEntity.EmailAddress;
            _txtFax.Code = _legalEntity.FaxCode;
            _txtFax.Number = _legalEntity.FaxNumber;

            if (AddressParameterRequired) // only show address dropwown if the report has an address parameter
            {
                Dictionary<int, string> dicAddresses = new Dictionary<int, string>();
                if (_showMailingAddress)
                {
                    // get a account mailing addresses
                    if (AccountMailingAddress != null)
                    {
                        // only add address if it doesnt exist
                        if (dicAddresses.ContainsKey(AccountMailingAddress.Key) == false)
                            dicAddresses.Add(AccountMailingAddress.Key, AccountMailingAddress.GetFormattedDescription(AddressDelimiters.Comma));
                    }
                }
                else
                {
                    // get a list of the legalentity addresses
                    if (LegalEntityAddresses != null && LegalEntityAddresses.Count > 0)
                    {
                        foreach (ILegalEntityAddress legalEntityAddress in LegalEntityAddresses)
                        {
                            // only add address if it doesnt exist
                            if (dicAddresses.ContainsKey(legalEntityAddress.Address.Key) == false)
                                dicAddresses.Add(legalEntityAddress.Address.Key, legalEntityAddress.Address.GetFormattedDescription(AddressDelimiters.Comma));
                        }
                    }
                }
                _ddlAddress.DataSource = dicAddresses;
                _ddlAddress.DataBind();

                if (dicAddresses.Count < 0)
                {
                    _chkPost.Checked = false;
                    _chkPost.Enabled = false;
                    _ddlAddress.Visible = false;
                    _lblAddress.Visible = false;
                }


                if (_showMailingAddress)
                    _lblAddress.Text = "  (Account Mailing Address)";
                else
                {
                    if (DisplayAttorneyAddresses)
                        _lblAddress.Text = "  (Attorney Address)";
                    else if (DisplayDebtCounsellorAddresses)
                        _lblAddress.Text = "  (Debt Counsellor Address)";
                    else
                        _lblAddress.Text = "  (Select an Address)";
                }
            }
            else
            {
                _ddlAddress.Visible = false;
                _lblAddress.Visible = false;
            }
        }
    }
}
