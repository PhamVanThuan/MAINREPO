using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxHtmlEditor;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Correspondence.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;

namespace SAHL.Web.Views.Correspondence
{
    /// <summary>
    ///
    /// </summary>
    public partial class ClientCommunication : SAHLCommonBaseView, IClientCommunication
    {
        int _genericKey, _genericKeyTypeKey;
        string _cellPhoneNumber, _emailAddress;
        SAHL.Common.Globals.CorrespondenceMediums _correspondenceMedium;
        ILegalEntityRepository _leRepo;
        ILookupRepository _lookupRepo;
        string _bankDetails;

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;

            if (_leRepo == null)
                _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            if (_lookupRepo == null)
                _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;

            //RegisterWebService(ServiceConstants.LegalEntity);

            // setup the html editor
            if (htmlEmailEditor.Toolbars.Count == 0)
            {
                //Create Custom Toolbar
                htmlEmailEditor.Toolbars.Add(CreateCustomToolbar("CustomToolbar"));

                //We have to create the standard toolbars first to hide them
                htmlEmailEditor.Toolbars.Add(HtmlEditorToolbar.CreateStandardToolbar1());
                htmlEmailEditor.Toolbars["StandardToolbar1"].Visible = false;
                htmlEmailEditor.Toolbars.Add(HtmlEditorToolbar.CreateStandardToolbar2());
                htmlEmailEditor.Toolbars["StandardToolbar2"].Visible = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage)
                return;

            //setup the screen in the correct mode
            switch (_correspondenceMedium)
            {
                case CorrespondenceMediums.Email:
                    trEmailSubject.Visible = true;
                    trEmailBody.Visible = true;

                    trSMSType.Visible = false;
                    trSMSBody.Visible = false;

                    btnSend.ToolTip = "Send Email";
                    btnSend.ImageUrl = "../../Images/EmailForward.gif";

                    break;

                case CorrespondenceMediums.SMS:
                    trSMSType.Visible = true;
                    trSMSBody.Visible = true;

                    trEmailSubject.Visible = false;
                    trEmailBody.Visible = false;

                    btnSend.ToolTip = "Send SMS";
                    btnSend.ImageUrl = "../../Images/send_sms.png";

                    hidBankDetails.Value = _bankDetails;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string contactColumnName = "";

            SelectedClientCommuncation selectedCommunication = new SelectedClientCommuncation();
            selectedCommunication.CorrespondenceMedium = _correspondenceMedium;
            selectedCommunication.SMSType = ddlSMSType.SelectedValue;
            if (_correspondenceMedium == CorrespondenceMediums.Email)
            {
                contactColumnName = "EmailAddress";
                selectedCommunication.Subject = txtEmailSubject.Text.Trim();
                selectedCommunication.Body = htmlEmailEditor.Html;
            }
            else if (_correspondenceMedium == CorrespondenceMediums.SMS)
            {
                contactColumnName = "CellPhoneNumber";
                selectedCommunication.Subject = "";
                selectedCommunication.Body = txtSMSText.Text.Trim();
            }

            selectedCommunication.SelectedRecipients = new List<BindableRecipient>();

            List<object> selectedFields = gridRecipients.GetSelectedFieldValues("Key", contactColumnName);

            foreach (object rec in selectedFields)
            {
                object[] fields = rec as object[];
                int legalEntityKey = Convert.ToInt32(fields[0]);
                string contact = Convert.ToString(fields[1]);

                if (_correspondenceMedium == CorrespondenceMediums.Email)
                    selectedCommunication.SelectedRecipients.Add(new BindableRecipient(legalEntityKey, "", "", contact, ""));
                else if (_correspondenceMedium == CorrespondenceMediums.SMS)
                    selectedCommunication.SelectedRecipients.Add(new BindableRecipient(legalEntityKey, "", contact, "", ""));
            }

            OnSendButtonClicked(sender, new KeyChangedEventArgs(selectedCommunication));
        }

        #region ICorrespondenceProcessing Members

        /// <summary>
        ///
        /// </summary>
        public event KeyChangedEventHandler OnSendButtonClicked;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        /// <param name="lstRecipients"></param>
        public void BindRecipients(IList<BindableRecipient> lstRecipients)
        {
            gridRecipients.SettingsBehavior.AllowSelectSingleRowOnly = false;

            switch (_correspondenceMedium)
            {
                case CorrespondenceMediums.Email:
                    gridRecipients.Columns["EmailAddress"].Visible = true;
                    gridRecipients.Columns["CellPhoneNumber"].Visible = false;
                    break;

                case CorrespondenceMediums.SMS:
                    gridRecipients.Columns["CellPhoneNumber"].Visible = true;
                    gridRecipients.Columns["EmailAddress"].Visible = false;
                    break;

                default:
                    break;
            }

            gridRecipients.DataSource = lstRecipients;

            gridRecipients.DataBind();
            if (SelectFirstItem)
                gridRecipients.Selection.SelectRow(0);

        }

        public void BindSMSTypes(IList<SAHL.Common.Globals.SMSTypes> lstSMSTypes)
        {
            // strip out the "_" values from the enumerators
            IDictionary<int, string> dicSMSTypes = new Dictionary<int, string>();
            foreach (var smstype in lstSMSTypes)
            {
                dicSMSTypes.Add((int)smstype, smstype.ToString().Replace("_", " "));
            }

            ddlSMSType.DataSource = dicSMSTypes;
            ddlSMSType.DataBind();
        }

        protected void gridRecipients_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            BindableRecipient recipientRow = (sender as DXGridView).GetRow(e.VisibleIndex) as BindableRecipient;
            if (recipientRow != null)
            {
                if (_correspondenceMedium == CorrespondenceMediums.Email && String.IsNullOrEmpty(recipientRow.EmailAddress))
                    e.Cell.Enabled = false;
                else if (_correspondenceMedium == CorrespondenceMediums.SMS && String.IsNullOrEmpty(recipientRow.CellPhoneNumber))
                    e.Cell.Enabled = false;
            }
        }

        protected void gridRecipients_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.ButtonType != DevExpress.Web.ASPxGridView.ColumnCommandButtonType.SelectCheckbox)
                return;

            // DataRow row = (sender as DXGridView).GetRow(e.VisibleIndex);
            BindableRecipient recipientRow = (sender as DXGridView).GetRow(e.VisibleIndex) as BindableRecipient;
            if (recipientRow != null)
            {
                if (_correspondenceMedium == CorrespondenceMediums.Email && String.IsNullOrEmpty(recipientRow.EmailAddress))
                    e.Visible = false;
                else if (_correspondenceMedium == CorrespondenceMediums.SMS && String.IsNullOrEmpty(recipientRow.CellPhoneNumber))
                    e.Visible = false;
            }
        }

        protected static HtmlEditorToolbar CreateCustomToolbar(string name)
        {
            HtmlEditorToolbar customToolbar = new HtmlEditorToolbar(
                name,
                new ToolbarUndoButton(),
                new ToolbarRedoButton(),
                new ToolbarFontNameEdit(),
                new ToolbarFontSizeEdit(),
                new ToolbarJustifyLeftButton(true),
                new ToolbarJustifyCenterButton(),
                new ToolbarJustifyRightButton(),
                new ToolbarJustifyFullButton(),
                new ToolbarBoldButton(),
                new ToolbarItalicButton(),
                new ToolbarUnderlineButton(),
                new ToolbarInsertUnorderedListButton(),
                new ToolbarInsertOrderedListButton()
            ).CreateDefaultItems();

            return customToolbar;
        }

        /// <summary>
        ///
        /// </summary>
        public string EmailSubject
        {
            set
            {
                txtEmailSubject.Text = value.ToString();
            }
            get
            {
                return txtEmailSubject.Text.Trim();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string EmailBody
        {
            set
            {
                htmlEmailEditor.Html = value.ToString();
            }
            get
            {
                return htmlEmailEditor.Html;
            }
        }

        /// <summary>
        /// Gets/Sets the SMS Text
        /// </summary>
        public string SMSText
        {
            set
            {
                txtSMSText.Text = value.ToString();
            }
            get
            {
                return txtSMSText.Text;
            }
        }

        /// <summary>
        /// Gets/Sets the SMS Type
        /// </summary>
        public string SMSType
        {
            set
            {
                ddlSMSType.SelectedValue = value.ToString();
            }
            get
            {
                return ddlSMSType.SelectedValue;
            }
        }

        /// <summary>
        /// Gets/Sets the GenericKey
        /// </summary>
        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        /// <summary>
        /// Gets/Sets the GenericKeyTypeKey
        /// </summary>
        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
            set { _genericKeyTypeKey = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string CellPhoneNumber
        {
            get { return _cellPhoneNumber; }
            set { _cellPhoneNumber = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public SAHL.Common.Globals.CorrespondenceMediums CorrespondenceMedium
        {
            get { return _correspondenceMedium; }
            set { _correspondenceMedium = value; }
        }

        public string BankDetails
        {
            get { return _bankDetails; }
            set { _bankDetails = value; }
        }

        public bool SelectFirstItem { get; set; }

        #endregion ICorrespondenceProcessing Members
    }
}