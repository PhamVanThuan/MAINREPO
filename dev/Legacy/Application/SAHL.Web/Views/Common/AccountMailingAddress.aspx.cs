using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Common
{
    public partial class AccountMailingAddress : SAHLCommonBaseView, IAccountMailingAddress
    {
        bool _reBind = true;

        readonly ILookupRepository lookUps = RepositoryFactory.GetRepository<ILookupRepository>();
        private ICommonRepository _commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();

        public bool ShowUpdateButton
        {
            set { SubmitButton.Visible = value; }
        }
        /// <summary>
        /// Set Controls for Display
        /// </summary>
        public void SetControlsForDisplay()
        {
            AddressDropDown.Visible = false;
            chkOnlineStatement.Enabled = false;
            ddlOnlineStatementFormat.Visible = false;
            ViewButtons.Visible = false;
            ddlCorrespondenceLanguage.Visible = false;
            ddlCorrespondenceMedium.Visible = false;
            ddlCorrespondenceMailAddress.Visible = false;
            pnlMailingAddressDetails.Visible = false;
           // btnAuditTrail.Visible = true;
        }
        /// <summary>
        /// Set View Controls for Update
        /// </summary>
        public void SetControlsForUpdate()
        {
            lblCorrespondenceLanguage.Visible = false;
            lblCorrespondenceMedium.Visible = false;
            lblCorrespondenceMailAddress.Visible = false;
            pnlMailingAddressDetDisp.Visible = false;
            btnAuditTrail.Visible = false;
        }
        /// <summary>
        /// Populate Mailing Address Drop Down
        /// </summary>
        /// <param name="accMailingAddress"></param>
        public void PopulateMailingAddressDropDown(IDictionary<string, string> accMailingAddress)
        {
            //leAddress = accMailingAddress;
         
            ddlMailingAddress.DataSource = accMailingAddress;
            ddlMailingAddress.DataTextField = "Values";
            ddlMailingAddress.DataValueField = "Key";
            ddlMailingAddress.DataBind();
        }

        public void BindEmailAddressDropDown(IList<ILegalEntity> LegalEntityList)
        {
            Dictionary<int, string> emailDict = new Dictionary<int, string>();
            
            foreach (ILegalEntity le in LegalEntityList)
            {
                emailDict.Add(le.Key,string.Format("{0} ({1})", le.EmailAddress, le.DisplayName));
            }

            ddlCorrespondenceMailAddress.DataSource = emailDict;
            ddlCorrespondenceMailAddress.DataTextField = "Value";
            ddlCorrespondenceMailAddress.DataValueField = "Key";
            ddlCorrespondenceMailAddress.DataBind();
        }
        /// <summary>
        /// Bind Mailing address on Display
        /// </summary>
        /// <param name="mailingAddressLst"></param>
        public void BindMailingAddressLstDisplay(IList<string> mailingAddressLst)
        {
            string AddressText = "";

            for (int i = 0; i < mailingAddressLst.Count; i++)
                AddressText += mailingAddressLst[i];

            AddressLineDisp.Text = AddressText;
        }
        /// <summary>
        /// Bind Mailing Address when user changes address selected on Drop Down
        /// </summary>
        /// <param name="leAddress"></param>
        public void BindMailingAddressUpdate(string leAddress)
        {
            AddressLine.Text = leAddress;
        }
        /// <summary>
        /// Bind Display fields
        /// </summary>
        /// <param name="mailingAddress"></param>
        public void BindDisplayFields(IMailingAddress mailingAddress)
        {
            AddressLineDisp.Text = mailingAddress.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma);
            pnlMailingAddressDetDisp.GroupingText = string.Format("{0} Address", mailingAddress.Address.AddressFormat.Description);

            lblCorrespondenceLanguage.Text = mailingAddress.Language.Description;

            if (mailingAddress.CorrespondenceMedium != null)
                lblCorrespondenceMedium.Text = mailingAddress.CorrespondenceMedium.Description;
            else
                lblCorrespondenceMedium.Text = "-";

            if (mailingAddress.CorrespondenceMedium != null &&
                mailingAddress.CorrespondenceMedium.Key == (int)CorrespondenceMediums.Email)
            {
                CorrespondenceMailAddressRowVisible = true;

                if (mailingAddress.LegalEntity != null)
                    lblCorrespondenceMailAddress.Text = mailingAddress.LegalEntity.EmailAddress;
                else
                    lblCorrespondenceMailAddress.Text = "-";
            }

            if (mailingAddress.OnlineStatement)
                chkOnlineStatement.Checked = true;
            else
                chkOnlineStatement.Checked = false;

            OnlineStatementFormat.Text = lookUps.OnlineStatementFormats.ObjectDictionary[mailingAddress.OnlineStatementFormat.Key.ToString()].Description;
        }
        /// <summary>
        /// Bind Display Fields for Application Mailing Address Display
        /// </summary>
        /// <param name="mailingAddress"></param>

        public void BindDisplayFieldsForApplication(IApplicationMailingAddress mailingAddress)
        {
            lblCorrespondenceLanguage.Text = mailingAddress.Language.Description;

            AddressLineDisp.Text = mailingAddress.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma);
            pnlMailingAddressDetDisp.GroupingText = string.Format("{0} Address", mailingAddress.Address.AddressFormat.Description);

            if (mailingAddress.CorrespondenceMedium != null)
                lblCorrespondenceMedium.Text = mailingAddress.CorrespondenceMedium.Description;
            else
                lblCorrespondenceMedium.Text = "-";

            if (mailingAddress.CorrespondenceMedium != null && mailingAddress.CorrespondenceMedium.Key == (int)CorrespondenceMediums.Email)
            {
                CorrespondenceMailAddressRowVisible = true;

                if (mailingAddress.LegalEntity != null)
                    lblCorrespondenceMailAddress.Text = mailingAddress.LegalEntity.EmailAddress;
                else
                    lblCorrespondenceMailAddress.Text = "-";
            }

            if (mailingAddress.OnlineStatement == true)
                chkOnlineStatement.Checked = true;
            else
                chkOnlineStatement.Checked = false;

            OnlineStatementFormat.Text = lookUps.OnlineStatementFormats.ObjectDictionary[mailingAddress.OnlineStatementFormat.Key.ToString()].Description;
        }
        /// <summary>
        /// Bind Fields that are updateable
        /// </summary>
        /// <param name="mailingAddress"></param>
        public void BindUpdateableFields(IMailingAddress mailingAddress)
        {
            BindLookUps();

            AddressLine.Text = mailingAddress.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma);

            ddlOnlineStatementFormat.SelectedValue = lookUps.OnlineStatementFormats.ObjectDictionary[mailingAddress.OnlineStatementFormat.Key.ToString()].Key.ToString();

            if (mailingAddress.OnlineStatement)
            {
                chkOnlineStatement.Checked = true;
                ddlOnlineStatementFormat.Enabled = true;
            }
            else
            {
                chkOnlineStatement.Checked = false;
                ddlOnlineStatementFormat.Enabled = false;
            }

            ddlCorrespondenceLanguage.SelectedValue = mailingAddress.Language.Key.ToString();
            ddlMailingAddress.SelectedValue = mailingAddress.Address.Key.ToString();

            if (mailingAddress.CorrespondenceMedium != null)
            {
                ddlCorrespondenceMedium.SelectedValue = lookUps.CorrespondenceMediums.ObjectDictionary[mailingAddress.CorrespondenceMedium.Key.ToString()].Key.ToString();
                ddlCorrespondenceMedium.SelectedValue = mailingAddress.CorrespondenceMedium.Key.ToString();
            }

            if (mailingAddress.CorrespondenceMedium != null && mailingAddress.CorrespondenceMedium.Key == (int)CorrespondenceMediums.Email)
            {
                CorrespondenceMailAddressRowVisible = true;
                
                if (mailingAddress.LegalEntity != null)
                    ddlCorrespondenceMailAddress.SelectedValue = mailingAddress.LegalEntity.Key.ToString();
            }
        }

        public void BindUpdateableFieldsForApplication(IApplicationMailingAddress mailingAddress)
        {
            BindLookUps();

            if (!_reBind)
                return;

            AddressLine.Text = mailingAddress.Address.GetFormattedDescription(SAHL.Common.Globals.AddressDelimiters.Comma);

            ddlOnlineStatementFormat.SelectedValue = lookUps.OnlineStatementFormats.ObjectDictionary[mailingAddress.OnlineStatementFormat.Key.ToString()].Key.ToString();

            if (mailingAddress.OnlineStatement == true)
            {
                chkOnlineStatement.Checked = true;
                ddlOnlineStatementFormat.Enabled = true;
            }
            else
            {
                chkOnlineStatement.Checked = false;
                ddlOnlineStatementFormat.Enabled = false;
            }

            ddlCorrespondenceLanguage.SelectedValue = mailingAddress.Language.Key.ToString();
            ddlMailingAddress.SelectedValue = mailingAddress.Address.Key.ToString();

            if (mailingAddress.CorrespondenceMedium != null)
            {
                ddlCorrespondenceMedium.SelectedValue = lookUps.CorrespondenceMediums.ObjectDictionary[mailingAddress.CorrespondenceMedium.Key.ToString()].Key.ToString();
                ddlCorrespondenceMedium.SelectedValue = mailingAddress.CorrespondenceMedium.Key.ToString();
            }

            if (mailingAddress.CorrespondenceMedium != null && mailingAddress.CorrespondenceMedium.Key == (int)CorrespondenceMediums.Email)
            {
                CorrespondenceMailAddressRowVisible = true;

                if (mailingAddress.LegalEntity != null)
                    ddlCorrespondenceMailAddress.SelectedValue = mailingAddress.LegalEntity.Key.ToString();
            }
        }
        /// <summary>
        /// Bind the Drop Downs 
        /// </summary>
        public void BindLookUpsForUpdate()
        {
            BindLookUps();
        }
        
        void BindLookUps()
        {
            ddlOnlineStatementFormat.DataSource = lookUps.OnlineStatementFormats;
            ddlOnlineStatementFormat.DataTextField = "Description";
            ddlOnlineStatementFormat.DataValueField = "Key";
            ddlOnlineStatementFormat.DataBind();
            ddlOnlineStatementFormat.Enabled = false;

            ddlCorrespondenceLanguage.DataSource = lookUps.LanguagesTranslatable;
            ddlCorrespondenceLanguage.DataTextField = "Description";
            ddlCorrespondenceLanguage.DataValueField = "Key";
            ddlCorrespondenceLanguage.DataBind();

            ddlCorrespondenceMedium.DataSource = lookUps.CorrespondenceMediums;
            ddlCorrespondenceMedium.DataTextField = "Description";
            ddlCorrespondenceMedium.DataValueField = "Key";
            ddlCorrespondenceMedium.DataBind();
        }
        /// <summary>
        /// Get Captured MailindAddress object for Save / Update
        /// </summary>
        /// <param name="iMailingAddress"></param>
        /// <returns></returns>
        public IMailingAddress GetCapturedMailingAddress(IMailingAddress iMailingAddress)
        {
            if (ddlCorrespondenceLanguage.SelectedValue != "-select-")
                iMailingAddress.Language = _commonRepo.GetLanguageByKey(Int32.Parse(ddlCorrespondenceLanguage.SelectedValue));

            if (ddlCorrespondenceMedium.SelectedValue != "-select-")
                iMailingAddress.CorrespondenceMedium = lookUps.CorrespondenceMediums.ObjectDictionary[ddlCorrespondenceMedium.SelectedValue];
            
            if (chkOnlineStatement.Checked)
                iMailingAddress.OnlineStatement = true;
            else
                iMailingAddress.OnlineStatement = false;
            
            if (ddlOnlineStatementFormat.SelectedValue == "-select-")
                iMailingAddress.OnlineStatementFormat = lookUps.OnlineStatementFormats.ObjectDictionary[((int)OnlineStatementFormats.NotApplicable).ToString()];
            else
                iMailingAddress.OnlineStatementFormat = lookUps.OnlineStatementFormats.ObjectDictionary[(ddlOnlineStatementFormat.SelectedValue)];

            return iMailingAddress;
        }

        public IApplicationMailingAddress GetCapturedApplicationMailingAddress(IApplicationMailingAddress appMailingAddress)
        {
            if (ddlCorrespondenceLanguage.SelectedValue != "-select-")
               appMailingAddress.Language = _commonRepo.GetLanguageByKey(Int32.Parse(ddlCorrespondenceLanguage.SelectedValue));

           if (ddlCorrespondenceMedium.SelectedValue != "-select-")
               appMailingAddress.CorrespondenceMedium = lookUps.CorrespondenceMediums.ObjectDictionary[ddlCorrespondenceMedium.SelectedValue];

            if (chkOnlineStatement.Checked)
                appMailingAddress.OnlineStatement = true;
            else
                appMailingAddress.OnlineStatement = false;

            if (ddlOnlineStatementFormat.SelectedValue == "-select-")
                appMailingAddress.OnlineStatementFormat = lookUps.OnlineStatementFormats.ObjectDictionary[((int)OnlineStatementFormats.NotApplicable).ToString()];
            else
                appMailingAddress.OnlineStatementFormat = lookUps.OnlineStatementFormats.ObjectDictionary[ddlOnlineStatementFormat.SelectedValue];

            return appMailingAddress;
        }
        /// <summary>
        /// Get Selected AddressKey
        /// </summary>
        public int GetSelectedAddressKey 
        {
            get 
            {
                if (ddlMailingAddress.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlMailingAddress.SelectedValue);

                    return 0;
            }
           
        }
        /// <summary>
        /// Change event of Mailing Address drop down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMailingAddressSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMailingAddress.SelectedIndex >= 0)
                OnddlMailingAddressSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlMailingAddress.SelectedValue));

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCorrespondenceMedium_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCorrespondenceMedium.SelectedIndex >=0)
                OnddlCorrespondenceMediumSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlCorrespondenceMedium.SelectedValue));
        }

        /// <summary>
        /// Check changes event of OnLineStatement check box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChkOnlineStatement_CheckChanged(object sender, EventArgs e)
        {
            if (chkOnlineStatement.Checked)
            {
                ddlOnlineStatementFormat.Enabled = true;
                if (ddlOnlineStatementFormat.SelectedValue == ((int)OnlineStatementFormats.NotApplicable).ToString())
                    ddlOnlineStatementFormat.SelectedIndex = -1;
            }
            else
            {
                ddlOnlineStatementFormat.SelectedValue = Convert.ToString((int)OnlineStatementFormats.NotApplicable);
                ddlOnlineStatementFormat.Enabled = false;
            }
        }
        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (onCancelButtonClicked != null)
                onCancelButtonClicked(sender, e);
        }
        /// <summary>
        /// Submit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (onSubmitButtonClicked != null)
                onSubmitButtonClicked(sender, e);
        }
        /// <summary>
        /// Audit Trail button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAuditTrail_Click(object sender, EventArgs e)
        {
            if (onAuditTrailButtonClicked != null)
                onAuditTrailButtonClicked(sender, e);
        }


        /// <summary>
        /// Implements IAccountMailingAddress.CorrespondenceLanguageKey
        /// </summary>
        public string CorrespondenceLanguageKey
        {
            get { return ddlCorrespondenceLanguage.SelectedItem.Value; }
        }

        /// <summary>
        /// Implements IAccountMailingAddress.CorrespondenceMediumKey
        /// </summary>
        public string CorrespondenceMediumKey
        {
            get { return ddlCorrespondenceMedium.SelectedItem.Value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CorrespondenceMailAddressKey
        {
            get { return ddlCorrespondenceMailAddress.SelectedItem.Value; }
        }

        /// <summary>
        /// Implements IAccountMailingAddress.OnlineStatementFormatKey
        /// </summary>
        public string OnlineStatementFormatKey
        {
            get { return ddlOnlineStatementFormat.SelectedItem.Value; }
        }

        /// <summary>
        /// Implements IAccountMailingAddress.OnlineStatementRequired
        /// </summary>
        public bool OnlineStatementRequired
        {
            get { return chkOnlineStatement.Checked; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CorrespondenceMediumRowVisible
        {
            set { CorrespondenceMediumRow.Visible = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CorrespondenceMailAddressRowVisible
        {
            set 
            { 
                CorrespondenceMailAddressRow.Visible = value;
                valCorrespondenceMailAddress.Enabled = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ReBind
        {
            set { _reBind = value; }
        }

        #region EventHandlers
        /// <summary>
        /// Event Handler for Mailing Address Selected Index Change
        /// </summary>
        public event KeyChangedEventHandler OnddlMailingAddressSelectedIndexChanged;
        /// <summary>
        /// Event Handler for Cancel Button 
        /// </summary>
        public event EventHandler onCancelButtonClicked;
        /// <summary>
        /// Event Handler for Submit Button
        /// </summary>
        public event EventHandler onSubmitButtonClicked;
        /// <summary>
        /// Event Handler for Audit Trail button
        /// </summary>
        public event EventHandler onAuditTrailButtonClicked;

        public event KeyChangedEventHandler OnddlCorrespondenceMediumSelectedIndexChanged;

        #endregion


        #region IAccountMailingAddress Members


      
        #endregion

    }
}
       