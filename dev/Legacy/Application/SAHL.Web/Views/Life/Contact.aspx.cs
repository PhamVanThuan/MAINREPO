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
using SAHL.Common;
using System.Text;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Life
{
    public partial class Contact : SAHLCommonBaseView, IContact
    {
        private bool _assuredLivesMode;
        private int _selectedLegalEntityKey;
        private bool _showUpdateContactButton;
        private bool _showUpdateAddressButton;
        private bool _showAddAddressButton;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ShouldRunPage)
                return;

            btnUpdateDetails.Visible = _showUpdateContactButton;
            btnUpdateAddress.Visible = _showUpdateAddressButton;
            btnAddAddress.Visible = _showAddAddressButton;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegalEntityGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnLegalEntityGridSelectedIndexChanged(sender, new KeyChangedEventArgs(LegalEntityGrid.SelectedLegalEntity.Key.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateDetails_Click(object sender, EventArgs e)
        {
            OnUpdateContactDetailsButtonClicked(sender, new KeyChangedEventArgs(LegalEntityGrid.SelectedLegalEntity.Key.ToString()));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAddress_Click(object sender, EventArgs e)
        {
            OnAddAddressButtonClicked(sender, new KeyChangedEventArgs(LegalEntityGrid.SelectedLegalEntity.Key.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateAddress_Click(object sender, EventArgs e)
        {
            OnUpdateAddressButtonClicked(sender, new KeyChangedEventArgs(LegalEntityGrid.SelectedLegalEntity.Key.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            OnNextButtonClicked(sender, e);
        }

        #region IContact Members

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnAddAddressButtonClicked;
       
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnUpdateAddressButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnUpdateContactDetailsButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnLegalEntityGridSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnNextButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool AssuredLivesMode
        {
            set { _assuredLivesMode = value; }
        }

        public bool ShowUpdateContactButton
        {
            set { _showUpdateContactButton = value; }
        }

        public bool ShowUpdateAddressButton
        {
            set { _showUpdateAddressButton = value; }
        }

        public bool ShowAddAddressButton
        {
            set { _showAddAddressButton = value; }
        }
        /// <summary>
        /// Sets whether to show or hide the workflow header
        /// When the view is used in worflow this should be set to TRUE
        /// othewise it should be set to false
        /// </summary>
        public bool ShowWorkFlowHeader
        {
            set { WorkFlowHeader1.Visible = value; }
        }
        public bool ShowNextButton
        {
            set { btnNext.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedLegalEntityKey
        {
            get { return _selectedLegalEntityKey; }
            set { _selectedLegalEntityKey = value; }
        }

        /// <summary>
        /// implements <see cref="IContact.BindAssuredLivesDetails"/>
        /// </summary>
        /// <param name="legalEntity"></param>
        public void BindAssuredLivesDetails(ILegalEntity legalEntity)
        {
            ILegalEntityNaturalPerson NP = legalEntity as ILegalEntityNaturalPerson;

            if (NP != null)
            {

                lblSalutation.Text = NP.Salutation == null ? "-" : NP.Salutation.Description;
                lblFirstNamesText.Text = "First Names";
                lblFirstNames.Text = String.IsNullOrEmpty(NP.FirstNames) ? "-" : NP.FirstNames;
                lblSurnameText.Text = "Surname";
                lblSurname.Text = String.IsNullOrEmpty(NP.Surname) ? "-" : NP.Surname;
                lblPreferredName.Text = String.IsNullOrEmpty(NP.PreferredName) ? "-" : NP.PreferredName;
                lblGender.Text = NP.Gender == null ? "-" : NP.Gender.Description;
                lblIDText.Text = "ID/Passport No";
                lblIDNumber.Text = String.IsNullOrEmpty(NP.IDNumber) ? (String.IsNullOrEmpty(NP.PassportNumber) ? "-" : NP.PassportNumber) : NP.IDNumber;
                lblBirthDate.Text = NP.DateOfBirth.HasValue ? NP.DateOfBirth.Value.ToString(SAHL.Common.Constants.DateFormat) : "-";
                lblAgeNextBirthday.Text = NP.AgeNextBirthday.HasValue ? NP.AgeNextBirthday.ToString() : "-";
            }
            else
            {
                switch ((LegalEntityTypes)legalEntity.LegalEntityType.Key)
                {
                    case LegalEntityTypes.CloseCorporation:
                        ILegalEntityCloseCorporation CC = (ILegalEntityCloseCorporation)legalEntity;
                        lblFirstNames.Text = String.IsNullOrEmpty(CC.RegisteredName) ? "-" : CC.RegisteredName;
                        lblSurname.Text = String.IsNullOrEmpty(CC.TradingName) ? "-" : CC.TradingName;
                        lblPreferredName.Text = CC.GetLegalName(LegalNameFormat.Full);
                        lblIDNumber.Text = String.IsNullOrEmpty(CC.RegistrationNumber) ? "-" : CC.RegistrationNumber;
                        break;

                    case LegalEntityTypes.Company:
                        ILegalEntityCompany CO = (ILegalEntityCompany)legalEntity;
                        lblFirstNames.Text = String.IsNullOrEmpty(CO.RegisteredName) ? "-" : CO.RegisteredName;
                        lblSurname.Text = String.IsNullOrEmpty(CO.TradingName) ? "-" : CO.TradingName;
                        lblPreferredName.Text = CO.GetLegalName(LegalNameFormat.Full);
                        lblIDNumber.Text = String.IsNullOrEmpty(CO.RegistrationNumber) ? "-" : CO.RegistrationNumber;
                        break;

                    case LegalEntityTypes.Trust:
                        ILegalEntityTrust CT = (ILegalEntityTrust)legalEntity;
                        lblFirstNames.Text = String.IsNullOrEmpty(CT.RegisteredName) ? "-" : CT.RegisteredName;
                        lblSurname.Text = String.IsNullOrEmpty(CT.TradingName) ? "-" : CT.TradingName;
                        lblPreferredName.Text = CT.GetLegalName(LegalNameFormat.Full);
                        lblIDNumber.Text = String.IsNullOrEmpty(CT.RegistrationNumber) ? "-" : CT.RegistrationNumber;
                        break;

                    default:
                        break;
                }
                lblFirstNamesText.Text = "Registered Name";
                lblSurnameText.Text = "Trading as";
                lblIDText.Text = "Registration No";

                lblGender.Text = "-";
                lblSalutation.Text = "-";
                lblBirthDate.Text = "-";
                lblAgeNextBirthday.Text = "-";
            }

            // Populate Contact Details
            string sCode = "", sNumber = "";
            sCode = String.IsNullOrEmpty(legalEntity.HomePhoneCode) ? "" : legalEntity.HomePhoneCode;
            sNumber = String.IsNullOrEmpty(legalEntity.HomePhoneNumber) ? "" : legalEntity.HomePhoneNumber;
            if (!String.IsNullOrEmpty(sCode)) sCode = "(" + sCode + ")";
            //lblHomePhone.Text = sCode + " " + sNumber;
            lblHomePhone.Text = String.IsNullOrEmpty(sCode + sNumber) ? "-" : sCode + " " + sNumber;

            sCode = legalEntity.WorkPhoneCode == null ? "" : legalEntity.WorkPhoneCode;
            sNumber = legalEntity.WorkPhoneNumber == null ? "" : legalEntity.WorkPhoneNumber;
            if (!String.IsNullOrEmpty(sCode)) sCode = "(" + sCode + ")";
            //lblWorkPhone.Text = sCode + " " + sNumber;
            lblWorkPhone.Text = String.IsNullOrEmpty(sCode + sNumber) ? "-" : sCode + " " + sNumber;

            sCode = legalEntity.FaxCode == null ? "" : legalEntity.FaxCode;
            sNumber = legalEntity.FaxNumber == null ? "" : legalEntity.FaxNumber;
            if (!String.IsNullOrEmpty(sCode)) sCode = "(" + sCode + ")";
            lblFaxNumber.Text = String.IsNullOrEmpty(sCode + sNumber) ? "-" : sCode + " " + sNumber;

            lblCellphone.Text = String.IsNullOrEmpty(legalEntity.CellPhoneNumber) ? "-" : legalEntity.CellPhoneNumber;
            lblEmailAddress.Text = String.IsNullOrEmpty(legalEntity.EmailAddress) ? "-" : legalEntity.EmailAddress;

        }

        /// <summary>
        /// implements <see cref="IContact.BindLegalEntityGrid"/>
        /// </summary>
        /// <param name="lstLegalEntities"></param>
        /// <param name="accountKey"></param>
        public void BindLegalEntityGrid(IReadOnlyEventList<ILegalEntity> lstLegalEntities, int accountKey)
        {
            // Setup the grid
            if (_assuredLivesMode == true)
                LegalEntityGrid.HeaderCaption = "Assured Life Details";
            else
                LegalEntityGrid.HeaderCaption = "Applicant Details";

            LegalEntityGrid.GridHeight = 100;
            LegalEntityGrid.ColumnIDPassportVisible = true;
            LegalEntityGrid.ColumnIDPassportHeadingType = SAHL.Web.Controls.LegalEntityGrid.GridColumnIDPassportHeadingType.IDAndPassportNumber;
            LegalEntityGrid.ColumnRoleVisible = true;
            LegalEntityGrid.ColumnLegalEntityStatusVisible = true;

            // Set the Data related properties
            LegalEntityGrid.AccountKey = accountKey;

            // Bind the grid
            LegalEntityGrid.BindLegalEntities(lstLegalEntities);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstLegalEntityAddress"></param>
        public void BindAddressData(IEventList<ILegalEntityAddress> lstLegalEntityAddress)
        {
            if (_assuredLivesMode == true)
                AddressGrid.HeaderCaption = "Assured Life Addresses";
            else
                AddressGrid.HeaderCaption = "Applicant Addresses";

            AddressGrid.GridHeight = 100;

            AddressGrid.BindAddressList(lstLegalEntityAddress);
        }

        #endregion
    }
}