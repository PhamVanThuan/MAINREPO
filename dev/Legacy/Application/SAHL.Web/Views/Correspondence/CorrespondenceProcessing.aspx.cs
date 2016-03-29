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
using System.Text.RegularExpressions;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Correspondence.Interfaces;
using System.Web.Configuration;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Web.Views.Reports.Interfaces;
using System.Collections.Specialized;
using System.Globalization;
using System.Drawing;
using DevExpress.Web.ASPxGridView;
using SAHL.Web.Controls;
using SAHL.Web.AJAX;
using System.Linq;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;
using SAHL.Common.BusinessModel.CorrespondenceGeneration;

namespace SAHL.Web.Views.Correspondence
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CorrespondenceProcessing : SAHLCommonBaseView, ICorrespondenceProcessing
    {
        private List<ReportData> _reportDataList;
        private int _genericKey, _genericKeyTypeKey;
        private List<CorrespondenceMediumInfo> _selectedCorrespondenceMediumInfo = new List<CorrespondenceMediumInfo>();
        private string _correspondenceDocuments, _faxCode, _faxNumber, _cellPhoneNumber, _emailAddress;
        private int _documentLanguageKey;
        private string _documentLanguageDesc;
        private bool _showMailingAddress;
        private bool _allowPreview;
        private bool _extraParametersVisible;     
        private List<CorrespondenceExtraParameter> _extraCorrespondenceParameters;
        private bool _disableCorrespondenceOptionEntry;
        private bool _displayAttorneyRole,_supressConfirmationMessage,_setEmailOptionChecked;
        private bool _multipleRecipientMode;
        private ILookupRepository _lookupRepo;
        private IAddress _accountMailingAddress;
        private int rowId;
        private bool _displayDebtCounsellors;
        private bool _addressParameterRequired;
        private bool _displayClientsAndNCR;

        private ILegalEntityRepository _legalEntityRepo;

        public ILegalEntityRepository LegalEntityRepo
        {
            get
            {
                if (_legalEntityRepo == null)
                    _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _legalEntityRepo;
            }
        }

        private enum AddressGridColumnPositions
        {
            LegalEntityAddressKey = 0,
            LegalEntityKey = 1,
            AddressType = 2,
            AddressFormat = 3,
            AddressKey = 4,
            AddressDescription = 5,
            LegalEntityName = 6,
            MailingAddressIndicator = 7,
            EffectiveDate = 8,
            Status = 9
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;

            //if (_legalEntityRepo == null)
            //    _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
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
            RegisterWebService(ServiceConstants.LegalEntity);
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

            lblDocument.Text = _correspondenceDocuments;

            lblDocumentLanguage.Text = _documentLanguageKey.ToString(); 
            lblDocumentLanguage.Text = _documentLanguageDesc;
            lblDocumentLanguageHeader.Visible = false;
            lblDocumentLanguage.Visible = false;

            btnPreview.Visible = _allowPreview;

            if (_multipleRecipientMode)
            {
                #region Set Correspondence Options from cache - Multiple Correspondence
                tblSendOptions.Visible = false;

                // set correspondence mediums if the have been retreived from cache
                if (_selectedCorrespondenceMediumInfo != null && _selectedCorrespondenceMediumInfo.Count > 0)
                {
                    foreach (CorrespondenceMediumInfo correspondenceMediumInfo in _selectedCorrespondenceMediumInfo)
                    {
                        // loop thru gird and find relevant correspondence options panel and set selected values
                        for (int i = 0; i < gridRecipientsMultiple.Rows.Count; i++)
                        {
                            int legalEntityKey = Convert.ToInt32(gridRecipientsMultiple.Rows[i].Cells[0].Text);
                            if (legalEntityKey == correspondenceMediumInfo.LegalEntityKey)
                            {
                                // get the correspondence options panel control
                                CorrespondenceGridOptionsPanel pnl = (CorrespondenceGridOptionsPanel)gridRecipientsMultiple.Rows[i].FindControl("pnlOptions_" + legalEntityKey);

                                // set checkboxes
                                foreach (SAHL.Common.Globals.CorrespondenceMediums cm in correspondenceMediumInfo.CorrespondenceMediumsSelected)
                                {
                                    switch (cm)
                                    {
                                        case SAHL.Common.Globals.CorrespondenceMediums.Email:
                                            pnl.SelectedEmail = true;
                                            break;
                                        case SAHL.Common.Globals.CorrespondenceMediums.Fax:
                                            pnl.SelectedFax = true;
                                            break;
                                        case SAHL.Common.Globals.CorrespondenceMediums.Post:
                                            pnl.SelectedPost = true;
                                            break;
                                        case SAHL.Common.Globals.CorrespondenceMediums.SMS:
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                //set values
                                if (!String.IsNullOrEmpty(correspondenceMediumInfo.FaxCode))
                                    pnl.FaxCode = correspondenceMediumInfo.FaxCode;
                                if (!String.IsNullOrEmpty(correspondenceMediumInfo.FaxNumber))
                                    pnl.FaxNumber = correspondenceMediumInfo.FaxNumber;

                                if (!String.IsNullOrEmpty(correspondenceMediumInfo.EmailAddress))
                                    pnl.EmailAddress = correspondenceMediumInfo.EmailAddress;

                                if (correspondenceMediumInfo.AddressKey > 0)
                                    pnl.AddressKey = correspondenceMediumInfo.AddressKey;

                            }
                        }
                    }
                }

                #endregion
            }
            else
            {
                #region Set Correspondence Options from cache - Single Correspondence

                tblSendOptions.Visible = true;

                // set correspondence mediums if the have been retreived from cache
                if (_selectedCorrespondenceMediumInfo != null && _selectedCorrespondenceMediumInfo.Count > 0)
                {
                    // set checkboxes
                    foreach (SAHL.Common.Globals.CorrespondenceMediums cm in _selectedCorrespondenceMediumInfo[0].CorrespondenceMediumsSelected)
                    {
                        switch (cm)
                        {
                            case SAHL.Common.Globals.CorrespondenceMediums.Email:
                                chkEmail.Checked = true;
                                break;
                            case SAHL.Common.Globals.CorrespondenceMediums.Fax:
                                chkFax.Checked = true;
                                break;
                            case SAHL.Common.Globals.CorrespondenceMediums.Post:
                                chkPost.Checked = true;
                                break;
                            case SAHL.Common.Globals.CorrespondenceMediums.SMS:
                                chkSMS.Checked = true;
                                break;
                            default:
                                break;
                        }
                    }

                    //set values
                    if (!String.IsNullOrEmpty(_selectedCorrespondenceMediumInfo[0].FaxCode))
                        txtFaxCode.Text = _selectedCorrespondenceMediumInfo[0].FaxCode;
                    if (!String.IsNullOrEmpty(_selectedCorrespondenceMediumInfo[0].FaxNumber))
                        txtFax.Text = _selectedCorrespondenceMediumInfo[0].FaxNumber;

                    if (!String.IsNullOrEmpty(_selectedCorrespondenceMediumInfo[0].EmailAddress))
                        txtEmail.Text = _selectedCorrespondenceMediumInfo[0].EmailAddress;

                    if (!String.IsNullOrEmpty(_selectedCorrespondenceMediumInfo[0].CellPhoneNumber))
                        txtSMS.Text = _selectedCorrespondenceMediumInfo[0].CellPhoneNumber;

                }

                #endregion

                if (_disableCorrespondenceOptionEntry)
                {
                    txtEmail.Enabled = false;
                    txtFaxCode.Enabled = false;
                    txtFax.Enabled = false;
                }
                if (_setEmailOptionChecked)
                    chkEmail.Checked = true;
            }


            if (_supressConfirmationMessage == false)
                btnSend.Attributes.Add("onclick", "return confirm('Are you sure you want to send the selected documents ?');");

            // enable/disable extra parameters  
            pnlExtraParameters.Visible = _extraParametersVisible;
            if (_extraParametersVisible == false)
            {
                tblSendOptions.Width = "100%";
                tblExtraParameters.Visible = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRecipients_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnRecipientsGridSelectedIndexChanged(sender, new KeyChangedEventArgs(gridRecipients.SelectedLegalEntity.Key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAdress_Click(object sender, EventArgs e)
        {
            if (gridRecipients.SelectedLegalEntity != null)
            {
                OnAddressAddButtonClicked(sender, new KeyChangedEventArgs(gridRecipients.SelectedLegalEntity.Key));
            }
            else
            {
                OnAddressAddButtonClicked(sender, new KeyChangedEventArgs(null));
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

            ProcessCorrespondence(sender, e, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ProcessCorrespondence(sender, e, true);
        }

        private void ProcessCorrespondence(object sender, EventArgs e, bool previewMode)
        {
            SetCorrespondenceMediums();

            // set extra report parameters
            if (_extraCorrespondenceParameters != null)
            {
                StringCollection SC = new StringCollection();
                SC.AddRange(Request.Form.AllKeys);                

                foreach (CorrespondenceExtraParameter extraParm in _extraCorrespondenceParameters)
                {
                    string controlName = "";
                    extraParm.ValidInput = true;

                    // if the control is a checklist then change the name accordingly
                    if (extraParm.ReportParameter.ReportParameterType.Key == (int)ReportParameterTypes.DropDownusingDescription)
                    {
                        #region checked list
                        string checkedValues = "";
                        int i = 0;
                        foreach (KeyValuePair<string, object> dicValues in extraParm.ReportParameter.ValidValues)
                        {
                            controlName = GetControlName(extraParm.ReportParameter) + "$" + i;

                            if (SC.Contains(pnlExtraParameters.NamingContainer.UniqueID + '$' + controlName))
                            {
                                // get the value of the checked control
                                string formCheckedValue = Request.Form[pnlExtraParameters.NamingContainer.UniqueID + '$' + controlName];
                                if (String.Compare(formCheckedValue, "on", true) == 0)
                                {
                                    // get the actual selected value
                                    object formValue = GetFormValue(dicValues.Key, extraParm.ReportParameter.ReportParameterType);
                                    if (formValue != null)
                                        checkedValues += formValue.ToString() + "|";
                                }
                            }
                            i++;
                        }
                        extraParm.ParameterValue = checkedValues.TrimEnd(new char[] { '|' }); ;

                        #endregion
                    }
                    else
                    {
                        #region all other controls
                        controlName = GetControlName(extraParm.ReportParameter);
                        if (SC.Contains(pnlExtraParameters.NamingContainer.UniqueID + '$' + controlName))
                        {
                            // get the value entered in the control
                            string formStringValue = Request.Form[pnlExtraParameters.NamingContainer.UniqueID + '$' + controlName];
                            // vlaidate the input
                            if (!ValidateParameter(extraParm.ReportParameter, formStringValue))
                            {
                                extraParm.ValidInput = false;
                                continue;
                            }

                            object formValue = GetFormValue(formStringValue, extraParm.ReportParameter.ReportParameterType);

                            if (formValue != null)
                            {
                                // this is for multiline controls
                                if (formStringValue.ToString().Contains("\r\n"))
                                {
                                    // must delimit with "|" for sql report
                                    string str = formValue.ToString().Replace("\r\n", "|");
                                    str = str.TrimEnd(new char[] { '|' });
                                    formValue = str;
                                }
                            }
                            else
                            {
                                if (!ValidateParameter(extraParm.ReportParameter, formValue))
                                {
                                    extraParm.ValidInput = false;
                                    continue;
                                }

                                formValue = string.Empty;
                            }

                            extraParm.ParameterValue = formValue;
                        }
                        #endregion
                    }
                }
            }

            if (previewMode)
                OnPreviewButtonClicked(sender, e);
            else
                OnSendButtonClicked(sender, e);
        }

        private static bool ValidateParameter(IReportParameter p, object paramValue)
        {
            if (p.Required.HasValue && p.Required.Value)
            {
                if (paramValue == null || paramValue.ToString().Length == 0)
                    return false;
            }
            return true;
        }

        private static bool ValidateParameter(IReportParameter p, string paramValue)
        {
            if (p.Required.HasValue && p.Required.Value)
            {
                if (paramValue.Length == 0)
                    return false;
            }
            return true;
        }
        
        private void SetCorrespondenceMediums()
        {
            _selectedCorrespondenceMediumInfo = new List<CorrespondenceMediumInfo>();

            if (_multipleRecipientMode)
            {
                // loop thru grid
                for (int i = 0; i < gridRecipientsMultiple.Rows.Count; i++)
                {
                    // get the correspondence options panel control
                    int selectedLegalEntityKey = Convert.ToInt32(gridRecipientsMultiple.Rows[i].Cells[0].Text);
                    CorrespondenceGridOptionsPanel pnl = (CorrespondenceGridOptionsPanel)gridRecipientsMultiple.Rows[i].FindControl("pnlOptions_" + selectedLegalEntityKey);
                    
                    CorrespondenceMediumInfo cmi = new CorrespondenceMediumInfo();

                    cmi.LegalEntityKey = selectedLegalEntityKey;
                    cmi.AddressKey = pnl.AddressKey;
                    cmi.EmailAddress = pnl.EmailAddress;
                    cmi.FaxCode = pnl.FaxCode;
                    cmi.FaxNumber = pnl.FaxNumber;
                    
                    if (pnl.SelectedEmail)
                        cmi.CorrespondenceMediumsSelected.Add(SAHL.Common.Globals.CorrespondenceMediums.Email);
                    if (pnl.SelectedFax)
                        cmi.CorrespondenceMediumsSelected.Add(SAHL.Common.Globals.CorrespondenceMediums.Fax);
                    if (pnl.SelectedPost)
                        cmi.CorrespondenceMediumsSelected.Add(SAHL.Common.Globals.CorrespondenceMediums.Post);

                    _selectedCorrespondenceMediumInfo.Add(cmi);
                }
            }
            else
            {
                CorrespondenceMediumInfo cmi = new CorrespondenceMediumInfo();

                // always set the legalentity key & address key
                cmi.LegalEntityKey = GetSingleSelectedLegalEntityKey();
                cmi.AddressKey = GetSingleSelectedAddressKey();

                if (chkEmail.Checked)
                {
                    cmi.CorrespondenceMediumsSelected.Add(SAHL.Common.Globals.CorrespondenceMediums.Email);
                    cmi.EmailAddress = txtEmail.Text.Trim();
                }
                if (chkSMS.Checked)
                {
                    cmi.CorrespondenceMediumsSelected.Add(SAHL.Common.Globals.CorrespondenceMediums.SMS);
                    cmi.CellPhoneNumber = txtSMS.Text.Trim();
                }
                if (chkFax.Checked)
                {
                    cmi.CorrespondenceMediumsSelected.Add(SAHL.Common.Globals.CorrespondenceMediums.Fax);
                    cmi.FaxCode = txtFaxCode.Text;
                    cmi.FaxNumber = txtFax.Text.TrimStart('0');
                }
                if (chkPost.Checked)
                {
                    cmi.CorrespondenceMediumsSelected.Add(SAHL.Common.Globals.CorrespondenceMediums.Post);
                }
                _selectedCorrespondenceMediumInfo.Add(cmi);
            }
        }


        private int GetSingleSelectedLegalEntityKey()
        {
            int selectedLegalEntityKey = 0;

            if (gridRecipients.Rows.Count > 0 && gridRecipients.SelectedLegalEntity != null)
                selectedLegalEntityKey = gridRecipients.SelectedLegalEntity.Key;

            return selectedLegalEntityKey;
        }

        private int GetSingleSelectedAddressKey()
        {
            int selectedAddressKey = 0;
            if (_showMailingAddress)
            {
                IAddress address = gridAddress.SelectedAddress as IAddress;
                selectedAddressKey = address != null ? address.Key : 0;
            }
            else
            {
                ILegalEntityAddress leAddress = gridAddress.SelectedAddress as ILegalEntityAddress;
                selectedAddressKey = leAddress != null ? leAddress.Address.Key : 0;
            }
            return selectedAddressKey;
        }

        #region ICorrespondenceProcessing Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSendButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnPreviewButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnAddressAddButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnRecipientsGridSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportDataList"></param>
        public void SetupAllowedCorrespondenceMediums(List<ReportData> reportDataList)
        {
            _reportDataList = reportDataList;

            // setup allowable correspondence mediums (based on first report in the list)
            foreach (SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders.CorrespondenceMediums cm in reportDataList[0].CorrespondenceMediums)
            {
                switch (cm.CorrespondenceMediumKey)
                {
                    case (int)SAHL.Common.Globals.CorrespondenceMediums.Post:
                        chkPost.Enabled = true;
                        break;
                    case (int)SAHL.Common.Globals.CorrespondenceMediums.Email:
                        chkEmail.Enabled = true;
                        txtEmail.ReadOnly = false;
                        break;
                    case (int)SAHL.Common.Globals.CorrespondenceMediums.Fax:
                        chkFax.Enabled = true;
                        txtFaxCode.ReadOnly = false;
                        txtFax.ReadOnly = false;
                        break;
                    case (int)SAHL.Common.Globals.CorrespondenceMediums.SMS:
                        chkSMS.Enabled = true;
                        txtSMS.ReadOnly = false;
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportDataList"></param>
        /// <param name="cachedReportDataList"></param>
        public void SetupExtraCorrespondenceParameters(List<ReportData> reportDataList, List<ReportData> cachedReportDataList)
        {
            _extraParametersVisible = false;
            tblParameters.Rows.Clear();
            //IReportRepository reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            List<IReportParameter> reportParms = GetDistinctParameters(reportDataList);
            List<ReportDataParameter> cachedParms = new List<ReportDataParameter>();

            cachedParms = cachedReportDataList == null ? null : cachedReportDataList.SelectMany(x => x.ReportParameters).ToList();

            IDictionary<string, object> reportParameterValues = new Dictionary<string, object>();
            _extraCorrespondenceParameters = new List<CorrespondenceExtraParameter>();

            if (reportParms == null)
                return;

            TableRow tr;

            foreach (IReportParameter parm in reportParms)
            {
                //if (String.Compare(parm.ParameterName, reportData.GenericKeyParameterName,true)==0
                //    || String.Compare(parm.ParameterName, reportData.LegalEntityParameterName,true)==0
                //    || String.Compare(parm.ParameterName, reportData.MailingTypeParameterName, true) == 0
                //    || String.Compare(parm.ParameterName, reportData.AddressParameterName, true) == 0)
                //{
                //    // dont worry about the "generic" parameters
                //    continue;
                //}
                ReportDataParameter cachedParam = null;

                if (cachedParms != null)
                    cachedParam = cachedParms.Where(x => String.Compare(x.ParameterName, parm.ParameterName, true) == 0).FirstOrDefault();
                
                object cachedParameterValue = cachedParam == null ? null : cachedParam.ParameterValue;
                //object cachedParameterValue = GetCachedParameterValue(cachedReportData, parm.ParameterName);

                _extraParametersVisible = true;

                List<string> reportParameterDefaultValues = new List<string>();

                CorrespondenceExtraParameter extraParm = new CorrespondenceExtraParameter(parm);
                _extraCorrespondenceParameters.Add(extraParm);

                tr = new TableRow();

                // Add the Label
                TableCell tcLabel = new TableCell();
                tcLabel.Text = parm.DisplayName;
                tcLabel.CssClass = "TitleText";
                tcLabel.Width = new Unit(30, UnitType.Percentage);

                // Add the Input control
                TableCell tcInput = new TableCell();
                Control inputControl = null;
                HtmlGenericControl divHint = null;

                // Get Parameter Values & Default Values
                if (parm.ReportStatement.StatementName != null || parm.ReportStatement.ReportType.Key == (int)SAHL.Common.Globals.ReportTypes.ReportingServicesReport)
                {
                    ISqlReportParameter sqlReportParameter = parm as ISqlReportParameter;

                    // get valid values
                    reportParameterValues = sqlReportParameter.ValidValues;

                    // get default values
                    foreach (string k in sqlReportParameter.DefaultValues)
                    {
                        reportParameterDefaultValues.Add(k);
                    }
                }

                bool reportParameterRequired = parm.Required.HasValue ? parm.Required.Value : false;

                bool defaultReportParametersExist = reportParameterDefaultValues != null && reportParameterDefaultValues.Count > 0 ? true : false;

                switch (parm.ReportParameterType.Key)
                {
                    case (int)SAHL.Common.Globals.ReportParameterTypes.DropDown:
                        #region DropDown
                        SAHLDropDownList ddlDropDown = new SAHLDropDownList();
                        ddlDropDown.Mandatory = reportParameterRequired;
                        // set valid values
                        if (reportParameterValues != null)
                        {
                            foreach (string desc in reportParameterValues.Keys)
                            {
                                string key = reportParameterValues[desc].ToString();
                                ddlDropDown.Items.Add(new ListItem(key, desc));
                            }
                        }

                        if (cachedParameterValue != null)
                        {
                            for (int w = 0; w < ddlDropDown.Items.Count; w++)
                            {
                                if (ddlDropDown.Items[w].Value == Convert.ToString(cachedParameterValue))
                                {
                                    ddlDropDown.SelectedValue = ddlDropDown.Items[w].Value;
                                    break;
                                }
                            }
                        }
                        else if (defaultReportParametersExist) // set default values
                        {
                            for (int p = 0; p < reportParameterDefaultValues.Count; p++)
                            {
                                for (int w = 0; w < ddlDropDown.Items.Count; w++)
                                {
                                    if (ddlDropDown.Items[w].Value == reportParameterDefaultValues[p])
                                    {
                                        ddlDropDown.SelectedValue = ddlDropDown.Items[w].Value;
                                        break;
                                    }
                                }
                            }
                        }

                        inputControl = ddlDropDown;
                        #endregion
                        break;

                    case (int)SAHL.Common.Globals.ReportParameterTypes.Boolean:
                        #region Boolean
                        CheckBox cBox = new CheckBox();
                        bool isChecked = false;
                        string cachedVal = Convert.ToString(cachedParameterValue);
                        if (cachedParameterValue != null)
                        {
                            if (String.Compare(cachedVal, "1") == 0 || String.Compare(cachedVal, "true", true) == 0)
                                isChecked = true;
                        }
                        else if (defaultReportParametersExist) // set default values
                        {
                            if (String.Compare(reportParameterDefaultValues[0], "1") == 0 || String.Compare(reportParameterDefaultValues[0], "true", true) == 0)
                                isChecked = true;
                        }

                        cBox.Checked = isChecked;
                        inputControl = cBox;
                        #endregion
                        break;

                    case (int)SAHL.Common.Globals.ReportParameterTypes.DateTime:
                        #region DateTime
                        SAHLDateBox dateBox = new SAHLDateBox();
                        dateBox.Mandatory = reportParameterRequired;

                        // if there is a cached value then populate control with this
                        if (cachedParameterValue != null)
                        {
                            DateTime dt1;
                            if (DateTime.TryParse(Convert.ToString(cachedParameterValue), CultureInfo.GetCultureInfo(SAHL.Common.Web.UI.Controls.Constants.CultureGb), DateTimeStyles.None, out dt1))
                                dateBox.Date = dt1;
                        }
                        else if (defaultReportParametersExist)  // set default values
                        {
                            DateTime dt1;
                            if (DateTime.TryParse(reportParameterDefaultValues[0], CultureInfo.GetCultureInfo(SAHL.Common.Web.UI.Controls.Constants.CultureGb), DateTimeStyles.None, out dt1))
                                dateBox.Date = dt1;
                        }

                        inputControl = dateBox;
                        #endregion
                        break;

                    case (int)SAHL.Common.Globals.ReportParameterTypes.Float:
                        #region Float
                        SAHLCurrencyBox txtFloat = new SAHLCurrencyBox();
                        txtFloat.Mandatory = reportParameterRequired;

                        // if there is a cached value then populate control with this
                        if (cachedParameterValue != null)
                            txtFloat.Text = Convert.ToString(cachedParameterValue);
                        else if (defaultReportParametersExist) // set default values
                            txtFloat.Text = reportParameterDefaultValues[0];

                        inputControl = txtFloat;
                        #endregion
                        break;

                    case (int)SAHL.Common.Globals.ReportParameterTypes.Integer:
                        #region Integer
                        SAHLTextBox txtInteger = new SAHLTextBox();
                        txtInteger.Mandatory = reportParameterRequired;
                        txtInteger.DisplayInputType = InputType.Number;

                        // if there is a cached value then populate control with this
                        if (cachedParameterValue != null)
                            txtInteger.Text = Convert.ToString(cachedParameterValue);
                        else if (defaultReportParametersExist) // set default values
                            txtInteger.Text = reportParameterDefaultValues[0];

                        inputControl = txtInteger;
                        #endregion
                        break;

                    case (int)SAHL.Common.Globals.ReportParameterTypes.String:
                        #region String
                        SAHLTextBox txtString = new SAHLTextBox();
                        txtString.Width = new Unit(95, UnitType.Percentage);
                        txtString.Mandatory = reportParameterRequired;
                        txtString.DisplayInputType = InputType.Normal;

                        // if there is a cached value then populate control with this
                        if (cachedParameterValue != null)
                            txtString.Text = Convert.ToString(cachedParameterValue);
                        else if (defaultReportParametersExist) // set default values
                            txtString.Text = reportParameterDefaultValues[0];

                        inputControl = txtString;
                        #endregion
                        break;

                    case (int)SAHL.Common.Globals.ReportParameterTypes.MultiStringDropDown:
                    case (int)SAHL.Common.Globals.ReportParameterTypes.MultiIntegerDropDown:
                        #region MultiValue
                        SAHLTextBox txtMulti = new SAHLTextBox();

                        if (parm.ReportParameterType.Key == (int)SAHL.Common.Globals.ReportParameterTypes.MultiIntegerDropDown)
                            txtMulti.DisplayInputType = InputType.Number;
                        else
                            txtMulti.DisplayInputType = InputType.Normal;

                        txtMulti.Mandatory = reportParameterRequired;
                        txtMulti.TextMode = TextBoxMode.MultiLine;
                        txtMulti.Rows = 4;

                        // set default values
                        if (defaultReportParametersExist)
                        {
                            for (int p = 0; p < reportParameterDefaultValues.Count; p++)
                            {
                                txtMulti.Text += reportParameterDefaultValues[p] + "\r\n";
                            }
                        }

                        // wrap the input control in a div so that we can float left and have the "hint" div next to it on the right
                        HtmlGenericControl divInput = new HtmlGenericControl("div");
                        divInput.Style.Add("float", "left");

                        divHint = new HtmlGenericControl("div");
                        divHint.Attributes.Add("class", "cellDisplay");
                        divHint.InnerHtml = @"<strong>Tip:</strong> Multiple " + tcLabel.Text + @" values must be entered on <em>separate lines</em>.";
                        divHint.Style.Add("float", "left");
                        divHint.Style.Add(HtmlTextWriterStyle.Padding, "5px 0px 0px 5px"); // pad 5px top and left

                        // set the Id of the input control to the parametername
                        txtMulti.ID = GetControlName(parm);
                        // add the textbox control to the div
                        divInput.Controls.Add(txtMulti);
                        // set the whole div to the generic input control
                        inputControl = divInput;
                        #endregion

                        break;

                    case (int)SAHL.Common.Globals.ReportParameterTypes.DropDownusingDescription:
                        #region Checked List
                        SAHLCheckboxList chkList = new SAHLCheckboxList();

                        if (reportParameterValues != null)
                        {
                            foreach (string desc in reportParameterValues.Keys)
                            {
                                string key = reportParameterValues[desc].ToString();
                                chkList.Items.Add(new ListItem(key, desc));
                            }
                        }

                        chkList.Height = new Unit(100, UnitType.Pixel);
                        chkList.Width = new Unit(60, UnitType.Percentage);
                        chkList.BorderStyle = BorderStyle.Solid;
                        chkList.BorderWidth = 1;
                        chkList.BorderColor = Color.Gray;

                        // if there are cached values then populate control with this
                        if (cachedParameterValue != null)
                        {
                            //split the cached values int a string array
                            string[] cachedValues = cachedParameterValue.ToString().Split('|');
                            foreach (string cachedValue in cachedValues)
                            {
                                for (int w = 0; w < chkList.Items.Count; w++)
                                {
                                    if (chkList.Items[w].Value == cachedValue)
                                    {
                                        chkList.Items[w].Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (defaultReportParametersExist) // set default values
                        {
                            foreach (string defaultValue in reportParameterDefaultValues)
                            {
                                for (int w = 0; w < chkList.Items.Count; w++)
                                {
                                    if (chkList.Items[w].Value == defaultValue)
                                    {
                                        chkList.Items[w].Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        inputControl = chkList;
                        #endregion
                        break;

                }

                if (inputControl != null)
                {
                    string controlID = GetControlName(parm);
                    // if our input control is wrapped in a div then set the name with the 'div' prefix
                    inputControl.ID = divHint != null ? "div" + controlID : controlID;

                    tcInput.Controls.Add(inputControl);
                    // if we have a hint div, then add it to the table cell
                    if (divHint != null)
                        tcInput.Controls.Add(divHint);
                }

                tr.Cells.Add(tcLabel);
                tr.Cells.Add(tcInput);
                tr.CssClass = "tableStandard";
                tblParameters.Rows.Add(tr);
            }
        }

        private static List<IReportParameter> GetDistinctParameters(List<ReportData> reportDataList)
        {
            IReportRepository reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            List<IReportParameter> reportParams = new List<IReportParameter>();

            foreach (ReportData reportData in reportDataList)
            {
                IReportStatement reportStatement = reportRepo.GetReportStatementByKey(reportData.ReportStatementKey);
                IList<IReportParameter> parms = reportStatement.ReportParameters as IList<IReportParameter>;

                //linq reportParams awesome
                reportParams = reportParams.Concat(parms.Where(x =>
                                                        string.Compare(x.ParameterName, reportData.GenericKeyParameterName, true) != 0
                                                        && string.Compare(x.ParameterName, reportData.LegalEntityParameterName, true) != 0
                                                        && string.Compare(x.ParameterName, reportData.MailingTypeParameterName, true) != 0
                                                        && string.Compare(x.ParameterName, reportData.AddressParameterName, true) != 0
                                                    )
                                                   )
                                                  .GroupBy(x => x.ParameterName)
                                                  .Select(y => y.FirstOrDefault())
                                                  .ToList();
            }

            return reportParams;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cachedReportData"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        //private static object GetCachedParameterValue(ReportData cachedReportData, string parameterName)
        //{
        //    object parameterValue = null;

        //    if (cachedReportData != null)
        //    {
        //        foreach (ReportDataParameter reportParm in cachedReportData.ReportParameters)
        //        {
        //            if (String.Compare(parameterName, reportParm.ParameterName, true) == 0)
        //            {
        //                parameterValue = reportParm.ParameterValue;
        //                break;
        //            }
        //        }
        //    }
        //    return parameterValue;
        //}
        ///// <summary>
        /// 
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="lstLegalEntities"></param>
        public void BindRecipientData(int genericKey, int genericKeyTypeKey, IReadOnlyEventList<ILegalEntity> lstLegalEntities)
        {
            if (_multipleRecipientMode)
            {
                #region Multiple Recipients - Setup the grid

                trRecipients.Height = "300px";
                gridRecipients.Visible = false;
                gridAddress.Visible = false;
                btnAddAddress.Visible = false;


                if (_displayAttorneyRole)
                    gridRecipientsMultiple.HeaderCaption = "Select Attorney's Correspondence Options";

                else if (_displayDebtCounsellors)
                    gridRecipientsMultiple.HeaderCaption = "Select Debt Counsellor's Correspondence Options";
                else
                    gridRecipientsMultiple.HeaderCaption = "Select Recipient's Correspondence Options";

                gridRecipientsMultiple.Columns.Clear();
                gridRecipientsMultiple.AutoGenerateColumns = false;
                gridRecipientsMultiple.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
                gridRecipientsMultiple.AddGridBoundColumn("", "", Unit.Percentage(100), HorizontalAlign.Left, true);

                gridRecipientsMultiple.DataSource = lstLegalEntities;
                gridRecipientsMultiple.DataBind();

                #endregion

            }
            else
            {
                #region Single Recipient - Setup the grid

                gridRecipients.Visible = true;
                gridAddress.Visible = true;
                gridRecipientsMultiple.Visible = false;

                if (_displayAttorneyRole)
                    gridRecipients.HeaderCaption = "Select an Attorney";
                else if (_displayDebtCounsellors)
                    gridRecipients.HeaderCaption = "Select a Debt Counsellor";
                else
                    gridRecipients.HeaderCaption = "Select a Recipient";

                gridRecipients.PostBackType = SAHL.Common.Web.UI.Controls.GridPostBackType.SingleClick;

                gridRecipients.GridHeight = 100;
                gridRecipients.ColumnIDPassportVisible = true;
                gridRecipients.ColumnIDPassportHeadingType = SAHL.Web.Controls.LegalEntityGrid.GridColumnIDPassportHeadingType.IDAndPassportNumber;
                gridRecipients.ColumnRoleVisible = true;
                gridRecipients.ColumnLegalEntityStatusVisible = true;

                // Set the Data related properties
                switch (genericKeyTypeKey)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                    case (int)SAHL.Common.Globals.GenericKeyTypes.ParentAccount:
                    case (int)SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                        gridRecipients.AccountKey = genericKey;
                        break;
                    case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                        gridRecipients.ApplicationKey = genericKey;
                        break;

                    default:
                        break;
                }

                // Bind the grid
                gridRecipients.BindLegalEntities(lstLegalEntities);
                #endregion
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridRecipientsMultiple_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Style.Add(HtmlTextWriterStyle.Padding, "0px");
            if (e.Row.RowType == DataControlRowType.Header)
            {
                BuildRowContents(e.Row, "","","", "Name", "Options", false);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ILegalEntity legalEntity = e.Row.DataItem as ILegalEntity;
                BuildRowContents(e.Row, legalEntity.Key.ToString(), _genericKey.ToString(),_genericKeyTypeKey.ToString(), legalEntity.DisplayName, legalEntity.LegalNumber, true);

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="legalEntityKey"></param>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="nameText"></param>
        /// <param name="numberText"></param>
        /// <param name="displayExtraInfo"></param>
        private void BuildRowContents(GridViewRow row, string legalEntityKey, string genericKey, string genericKeyTypeKey, string nameText, string numberText, bool displayExtraInfo)
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
            divDisplayName.Style.Add(HtmlTextWriterStyle.Width, "30%");
            divDisplayName.Style.Add(HtmlTextWriterStyle.Padding, "2px 4px 2px 4px");

            if (row.RowType == DataControlRowType.DataRow)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"<span style=""float:left;""><a href=""#"" onclick=""toggleExtraInfo(this, '" + legalEntityKey + @"', '" + genericKey + @"', '" + genericKeyTypeKey + @"')""><img src=""../../Images/arrow_blue_right.gif"" /></a></span>
                    <span style=""float:left;margin-left:3px;padding:2px;"">");
                sb.Append(nameText + "</span>");

                divDisplayName.InnerHtml = sb.ToString();
            }
            else
                divDisplayName.InnerHtml = String.Format(@"<span style=""float:left;width:16px;"">&nbsp;</span><span style=""float:left;margin-left:3px;"">{0}</span>", nameText);

            // correspondence options
            HtmlGenericControl divCorrespondenceOptions = new HtmlGenericControl("div");
            divRow.Controls.Add(divCorrespondenceOptions);
            divCorrespondenceOptions.Attributes.Add("class", "cell");
            divCorrespondenceOptions.Style.Add(HtmlTextWriterStyle.Width, "65%");
            divCorrespondenceOptions.Style.Add(HtmlTextWriterStyle.Padding, "2px 4px 2px 4px");

            if (row.RowType == DataControlRowType.DataRow)
            {
                //setup the correspondence panel
                var legalEntity = LegalEntityRepo.GetLegalEntityByKey(Convert.ToInt32(legalEntityKey));

                CorrespondenceGridOptionsPanel pnlOptions = new CorrespondenceGridOptionsPanel();
                pnlOptions.ID = "pnlOptions_" + legalEntity.Key.ToString();
                pnlOptions.HorizontalStack = false;
                pnlOptions.LegalEntity = legalEntity;
                pnlOptions.CorrespondenceMediums = _reportDataList[0].CorrespondenceMediums;

                pnlOptions.ShowMailingAddress = _showMailingAddress;
                pnlOptions.AddressParameterRequired = _addressParameterRequired;
                pnlOptions.LegalEntityAddresses = legalEntity.LegalEntityAddresses;
                pnlOptions.AccountMailingAddress = _accountMailingAddress;
                pnlOptions.DisplayAttorneyAddresses = _displayAttorneyRole;
                pnlOptions.DisplayDebtCounsellorAddresses = _displayDebtCounsellors;

                HtmlGenericControl spanCtl = new HtmlGenericControl("span");
                spanCtl.Style.Add(HtmlTextWriterStyle.Padding, "2px");
                //spanCtl.InnerText = "Craigs Test";
                spanCtl.Controls.Add(pnlOptions);

                divCorrespondenceOptions.Controls.Add(spanCtl);
            }
            else
            {
                divCorrespondenceOptions.InnerHtml = @"<span style=""padding:2px;"">" + numberText + "</span>";
            }

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
            }

            row.Cells[1].Controls.Add(divOuter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstLegalEntityAddress"></param>
        /// <param name="lstMailingAddress"></param>
        public void BindAddressData(IEventList<ILegalEntityAddress> lstLegalEntityAddress, IEventList<IAddress> lstMailingAddress)
        {
            btnAddAddress.Visible = false;

            gridAddress.GridHeight = 100;
            gridAddress.AddressColumnVisible = true;
            gridAddress.FormatColumnVisible = false;
            gridAddress.StatusColumnVisible = false;
            gridAddress.ShowInactive = false;

            if (_showMailingAddress)
            {
                gridAddress.HeaderCaption = "Mailing Address";
                gridAddress.TypeColumnVisible = false;
                gridAddress.EffectiveDateColumnVisible = false;
                gridAddress.BindAddressList(lstMailingAddress);
                btnAddAddress.Visible = false;
            }
            else
            {
                if (_displayAttorneyRole)
                {
                    gridAddress.HeaderCaption = "Attorney Addresses";
                    gridAddress.PostBackType = GridPostBackType.None;
                    btnAddAddress.Visible = false;
                }
                else if (_displayDebtCounsellors)
                {
                    gridAddress.HeaderCaption = "Debt Counsellor Addresses";
                    btnAddAddress.Visible = false;
                }
                else
                {
                    gridAddress.HeaderCaption = "Select an Address";
                }

                gridAddress.LegalEntityColumnVisible = false;
                gridAddress.BindAddressList(lstLegalEntityAddress);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="legalEntity"></param>
        public void BindCorrespondenceMediumData(ILegalEntity legalEntity)
        {
            txtEmail.Text = legalEntity.EmailAddress;
            txtFaxCode.Text = legalEntity.FaxCode;
            txtFax.Text = legalEntity.FaxNumber;
            txtSMS.Text = legalEntity.CellPhoneNumber;
        }
        /// <summary>
        /// Sets whether the screen functions in Single or Multiple recipient mode
        /// </summary>
        public bool MultipleRecipientMode 
        {
            get { return _multipleRecipientMode; }
            set { _multipleRecipientMode = value; }
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
        /// Gets/Sets the Correspondence Documents
        /// </summary>
        public string CorrespondenceDocuments
        {
            get { return _correspondenceDocuments; }
            set { _correspondenceDocuments = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FaxCode
        {
            get { return _faxCode; }
            set { _faxCode = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FaxNumber
        {
            get { return _faxNumber; }
            set { _faxNumber = value; }
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
        public bool ShowLifeWorkFlowHeader
        {
            set
            {
                WorkFlowHeader1.Visible = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowCancelButton
        {
            set
            {
                btnCancel.Visible = value;
            }
        }
        /// <summary>
        /// Gets the selected CorrespondenceMediums
        /// </summary>
        public List<CorrespondenceMediumInfo> SelectedCorrespondenceMediumInfo
        {
            get { return _selectedCorrespondenceMediumInfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<CorrespondenceExtraParameter> ExtraCorrespondenceParameters
        {
            get { return _extraCorrespondenceParameters; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DocumentLanguageKey
        {
            get { return _documentLanguageKey; }
            set { _documentLanguageKey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DocumentLanguageDesc
        {
            set { _documentLanguageDesc = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowMailingAddress
        {
            set { _showMailingAddress = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowPreview
        {
            set { _allowPreview = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool DisableCorrespondenceOptionEntry
        {
            set {_disableCorrespondenceOptionEntry = value;}
        }
        /// <summary>
        /// 
        /// </summary>
        public bool DisplayAttorneyRole
        {
            get { return _displayAttorneyRole; }
            set { _displayAttorneyRole = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool DisplayDebtCounsellors
        {
            get { return _displayDebtCounsellors; }
            set { _displayDebtCounsellors = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool DisplayClientsAndNCR
        {
            get { return _displayClientsAndNCR; }
            set { _displayClientsAndNCR = value; }
        }

        public bool CCDebtCounsellor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool SupressConfirmationMessage
        {
            set { _supressConfirmationMessage = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SetEmailOptionChecked
        {
            set { _setEmailOptionChecked = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IAddress AccountMailingAddress
        {
            get
            {
                return _accountMailingAddress;
            }
            set
            {
                _accountMailingAddress = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AddressParameterRequired
        {
            get
            {
                return _addressParameterRequired;
            }
            set
            {
                _addressParameterRequired = value;
            }
        }


        /// <summary>
        /// Setting this to TRUE will print one copy of the report for legalentities that share a domicilium address
        /// This will only apply to Post option only
        /// </summary>
        public bool PostSingleCopyForSharedDomiciliums
        {
            get;
            set;
        }

        /// <summary>
        /// Setting this to TRUE will print one copy of the report for legalentities that share a domicilium address
        /// This will only apply to Email option only
        /// </summary>
        public bool EmailSingleCopyForSharedDomiciliums
        {
            get;
            set;
        }
        
         /// <summary>
        /// 
        /// </summary>
        public bool LegalEntityCorrespondence { get; set; }
        
        #endregion

        
        private static string GetControlName(IReportParameter Parameter)
        {
            switch (Parameter.ReportParameterType.Description)
            {
                case "Drop Down":
                case "Boolean":
                case "DateTime":
                case "Float":
                case "Integer":
                case "String":
                case "Multi Integer Drop Down":
                case "Multi String Drop Down":
                case "Drop Down using Description":
                    return Parameter.ParameterName;
            }

            throw new ArgumentOutOfRangeException("Parameter", "Invalid Report Parameter Type");
        }

        private static object GetFormValue(string formStringValue, IReportParameterType reportParameterType)
        {
            switch (reportParameterType.Description)
            {
                case "Drop Down":
                    return formStringValue;

                case "Boolean":
                    bool val = false;
                    if (String.Compare(formStringValue, "on", true) == 0 || String.Compare(formStringValue, "1") == 0)
                        val = true;

                    return val;
                
                case "DateTime":
                        System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
                        return Convert.ToDateTime(formStringValue, enGB);
                
                case "Float":
                    return String.IsNullOrEmpty(formStringValue) ? 0 : Convert.ToSingle(formStringValue);

                case "Integer":
                    return String.IsNullOrEmpty(formStringValue) ? 0 : Convert.ToInt32(formStringValue);

                case "String":
                    return formStringValue;
                
                case "Multi Integer Drop Down":
                case "Multi String Drop Down":
                    return formStringValue;

                case "Drop Down using Description":
                    return formStringValue;

                default:
                    return null;
            }
        }
    }
}
