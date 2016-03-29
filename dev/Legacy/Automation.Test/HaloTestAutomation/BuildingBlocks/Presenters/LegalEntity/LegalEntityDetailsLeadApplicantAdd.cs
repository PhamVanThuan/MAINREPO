using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityDetailsLeadApplicantAdd : LegalEntityDetailsLeadApplicantAddControls
    {
        private ICommonService commonService;

        public LegalEntityDetailsLeadApplicantAdd()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// Select and add an existing legal entity
        /// </summary>
        public void AddExistingLegalEntity(string idNumber)
        {
            base.textfieldIDNumber.TypeText(idNumber);
            base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
            base.SAHLAutoComplete_DefaultItem(idNumber).MouseDown();
            base.ctl00MainddlUpdDocumentLanguage.Option(Language.English).Select();
            base.btnCreateLead.Click();
        }

        public void AddPersonalLoanLegalEntity(string role,string id, string salutation, string initials, string firstname,
            string surname, string preferredName, string gender, string maritalStatus, string populationGroup, string education, string citizenshipType, string dob,
            string passportNumber, string taxNumber, string homeLanguage, string documentLanguage, string status, string homeCode, string homeNumber, string workCode,
            string workNumber, string faxCode, string faxNumber, string cellNumber, string emailAddress, bool telemarketing, bool marketing, bool customerList,
            bool email, bool sms, ButtonTypeEnum btn)
        {
            //Calculate DOB from IDNumber
            if (dob == "auto" && !String.IsNullOrEmpty(id))
            {
                dob = commonService.GetDateOfBirthFromIDNumber(id);
            }

            //Generate Preferred Name from first 3 letters of Firstname
            if (preferredName == "auto" && firstname != null)
            {
                if (firstname.Length > 3) preferredName = firstname.Remove(3);
                else preferredName = firstname;
            }

            //Generate Initials from first letter of Firstname
            if (initials == "auto" && firstname != null)
                initials = firstname.Remove(1);

            if (role != null) base.selectRole.Option(role).Select();
            //Set Income Contributor checkbox
            if (id != null) base.textfieldIDNumber.TypeText(id);
            if (salutation != null) base.selectSalutation.Option(salutation).Select();
            if (initials != null && initials != "auto") base.textfieldInitials.Value = (initials);
            if (firstname != null) base.textfieldFirstNames.Value = (firstname);
            if (surname != null) base.textfieldSurname.Value = (surname);
            if (preferredName != null && preferredName != "auto") base.textfieldPreferredName.Value = (preferredName);
            if (gender != null) base.selectGender.Option(gender).Select();
            if (maritalStatus != null) base.selectMaritalStatus.Option(maritalStatus).Select();
            if (populationGroup != null) base.selectPopulationGroup.Option(populationGroup).Select();
            if (education != null) base.selectEducation.Option(education).Select();
            if (citizenshipType != null) base.selectCitizenshipType.Option(citizenshipType).Select();
            if (dob != null && dob != "auto") base.textfieldDateOfBirth.Value = dob;
            if (passportNumber != null) base.textfieldPassportNumber.TypeText(passportNumber);
            if (taxNumber != null) base.textfieldTaxNumber.TypeText(taxNumber);
            if (homeLanguage != null) base.selectHomeLanguage.Option(homeLanguage).Select();
            if (documentLanguage != null) base.selectDocumentLanguage.Option(documentLanguage).Select();
            if (status != null) base.selectStatus.Option(status).Select();
            //Contact Details
            if (homeCode != null) base.textfieldHomePhoneCode.Value = (homeCode);
            if (homeNumber != null) base.textfieldHomePhoneNumber.Value = (homeNumber);
            if (workCode != null) base.textfieldWorkPhoneCode.Value = (workCode);
            if (workNumber != null) base.textfieldWorkPhoneNumber.Value = (workNumber);
            if (faxCode != null) base.textfieldFaxCode.Value = (faxCode);
            if (faxNumber != null) base.textfieldFaxNumber.Value = (faxNumber);
            if (cellNumber != null) base.textfieldCellphoneNo.Value = (cellNumber);
            if (emailAddress != null) base.textfieldEmailAddress.Value = (emailAddress);
            //Set Telemarketing checkbox
            base.checkboxTelemarketing.Checked = telemarketing;
            //Set Marketing checkbox
            base.checkboxMarketing.Checked = marketing;
            //Set Custimer List checkbox
            base.checkboxCustomerLists.Checked = customerList;
            //Set Email checkbox
            base.checkboxEmail.Checked = email;
            //set SMS checkbox
            base.checkboxSMS.Checked = sms;

            switch (btn)
            {
                case ButtonTypeEnum.CreateLead:
                    base.btnCreateLead.Click();
                    break;
            }
        }


        /// <summary>
        /// Add Legal Entity of Type Natural Person
        /// </summary>
        /// <param name="TestBrowser">WatiN.Core.TestBrowser object</param>
        /// <param name="role">e.g. Lead - Main Applicant, Lead - Suretor</param>
        /// <param name="incomeContributor">Boolean Value.  true = Check and false = Uncheck "Income Contributor" checkbox</param>
        /// <param name="id">ID Number text field</param>
        /// <param name="salutation">Salutaion select list</param>
        /// <param name="initials">Initials text field.  If set to "auto", the first letter of the "firstname" value will be used</param>
        /// <param name="firstname">Firs Names text field</param>
        /// <param name="surname">Surname text field</param>
        /// <param name="preferredName">Preferred Name text field.  If set to "auto", the first 3 letters of the "firstname" value will be used</param>
        /// <param name="gender">Gender select list</param>
        /// <param name="maritalStatus">Marital Status select list</param>
        /// <param name="populationGroup">Population Group select list</param>
        /// <param name="education">Education select list</param>
        /// <param name="citizenshipType">Citizenship Type select list</param>
        /// <param name="dob">Date of Birth text field.  If set to "Auto", "dob" will be generated from "id" value</param>
        /// <param name="passportNumber">Passport Number text field</param>
        /// <param name="taxNumber">Tax Number text field</param>
        /// <param name="homeLanguage">Home Language select list</param>
        /// <param name="documentLanguage">Document Language select list</param>
        /// <param name="status">Status select list</param>
        /// <param name="homeCode"></param>
        /// <param name="homeNumber"></param>
        /// <param name="workCode"></param>
        /// <param name="workNumber"></param>
        /// <param name="faxCode"></param>
        /// <param name="faxNumber"></param>
        /// <param name="cellNumber"></param>
        /// <param name="emailAddress"></param>
        /// <param name="telemarketing"></param>
        /// <param name="marketing"></param>
        /// <param name="customerList"></param>
        /// <param name="email"></param>
        /// <param name="sms"></param>
        /// <param name="btn"></param>
        public void AddLegalEntity(string role, bool incomeContributor, string id, string salutation, string initials, string firstname,
            string surname, string preferredName, string gender, string maritalStatus, string populationGroup, string education, string citizenshipType, string dob,
            string passportNumber, string taxNumber, string homeLanguage, string documentLanguage, string status, string homeCode, string homeNumber, string workCode,
            string workNumber, string faxCode, string faxNumber, string cellNumber, string emailAddress, bool telemarketing, bool marketing, bool customerList,
            bool email, bool sms, ButtonTypeEnum btn)
        {
            //Calculate DOB from IDNumber
            if (dob == "auto" && !String.IsNullOrEmpty(id))
            {
                dob = commonService.GetDateOfBirthFromIDNumber(id);
            }

            //Generate Preferred Name from first 3 letters of Firstname
            if (preferredName == "auto" && firstname != null)
            {
                if (firstname.Length > 3) preferredName = firstname.Remove(3);
                else preferredName = firstname;
            }

            //Generate Initials from first letter of Firstname
            if (initials == "auto" && firstname != null)
                initials = firstname.Remove(1);

            if (role != null) base.selectRole.Option(role).Select();
            //Set Income Contributor checkbox
            base.checkboxIncomeContributor.Checked = incomeContributor;
            if (id != null) base.textfieldIDNumber.TypeText(id);
            if (salutation != null) base.selectSalutation.Option(salutation).Select();
            if (initials != null && initials != "auto") base.textfieldInitials.Value = (initials);
            if (firstname != null) base.textfieldFirstNames.Value = (firstname);
            if (surname != null) base.textfieldSurname.Value = (surname);
            if (preferredName != null && preferredName != "auto") base.textfieldPreferredName.Value = (preferredName);
            if (gender != null) base.selectGender.Option(gender).Select();
            if (maritalStatus != null) base.selectMaritalStatus.Option(maritalStatus).Select();
            if (populationGroup != null) base.selectPopulationGroup.Option(populationGroup).Select();
            if (education != null) base.selectEducation.Option(education).Select();
            if (citizenshipType != null) base.selectCitizenshipType.Option(citizenshipType).Select();
            if (dob != null && dob != "auto") base.textfieldDateOfBirth.Value = dob;
            if (passportNumber != null) base.textfieldPassportNumber.TypeText(passportNumber);
            if (taxNumber != null) base.textfieldTaxNumber.TypeText(taxNumber);
            if (homeLanguage != null) base.selectHomeLanguage.Option(homeLanguage).Select();
            if (documentLanguage != null) base.selectDocumentLanguage.Option(documentLanguage).Select();
            if (status != null) base.selectStatus.Option(status).Select();
            //Contact Details
            if (homeCode != null) base.textfieldHomePhoneCode.Value = (homeCode);
            if (homeNumber != null) base.textfieldHomePhoneNumber.Value = (homeNumber);
            if (workCode != null) base.textfieldWorkPhoneCode.Value = (workCode);
            if (workNumber != null) base.textfieldWorkPhoneNumber.Value = (workNumber);
            if (faxCode != null) base.textfieldFaxCode.Value = (faxCode);
            if (faxNumber != null) base.textfieldFaxNumber.Value = (faxNumber);
            if (cellNumber != null) base.textfieldCellphoneNo.Value = (cellNumber);
            if (emailAddress != null) base.textfieldEmailAddress.Value = (emailAddress);
            //Set Telemarketing checkbox
            base.checkboxTelemarketing.Checked = telemarketing;
            //Set Marketing checkbox
            base.checkboxMarketing.Checked = marketing;
            //Set Custimer List checkbox
            base.checkboxCustomerLists.Checked = customerList;
            //Set Email checkbox
            base.checkboxEmail.Checked = email;
            //set SMS checkbox
            base.checkboxSMS.Checked = sms;

            switch (btn)
            {
                case ButtonTypeEnum.Next:
                    base.btnCreateLead.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancel.Click();
                    break;
            }
        }

        /// <summary>
        /// This will add a legalentity provided with a LegalEntityDetail parameter object.
        /// </summary>
        /// <param name="legalEntity"></param>
        public void AddLegalEntity(Automation.DataModels.LegalEntity legalEntity)
        {
            switch (legalEntity.LegalEntityTypeKey)
            {
                #region Company/Trust/CC

                case LegalEntityTypeEnum.Company:
                case LegalEntityTypeEnum.CloseCorporation:
                case LegalEntityTypeEnum.Trust:
                    {
                        base.selectLegalEntityType.SelectByValue(((int)legalEntity.LegalEntityTypeKey).ToString());
                        base.textfieldCompanyName.Value = legalEntity.RegisteredName;
                        base.textfieldTradingName.Value = legalEntity.TradingName;
                        base.textfieldRegistrationNumber.Value = legalEntity.RegistrationNumber;
                        base.CompanyTaxNumber.Value = legalEntity.TaxNumber;
                        base.textfieldWorkPhoneCode.Value = legalEntity.WorkPhoneCode.ToString();
                        base.textfieldWorkPhoneNumber.Value = legalEntity.WorkPhoneNumber.ToString();
                        break;
                    }

                #endregion Company/Trust/CC

                #region NaturalPerson

                case LegalEntityTypeEnum.NaturalPerson:
                    {
                        string dateOfBirth = legalEntity.CitizenTypeKey == CitizenTypeEnum.SACitizen ?
                            commonService.GetDateOfBirthFromIDNumber(legalEntity.IdNumber) : "01/01/1970";
                        base.selectMaritalStatus.SelectByValue(((int)legalEntity.MaritalStatusKey).ToString());
                        base.selectGender.SelectByValue(((int)legalEntity.GenderKey).ToString());
                        base.selectPopulationGroup.SelectByValue(((int)legalEntity.PopulationGroupKey).ToString());
                        base.selectSalutation.SelectByValue(((int)legalEntity.SalutationKey).ToString());

                        base.textfieldFirstNames.Value = legalEntity.FirstNames;
                        base.textfieldInitials.Value = legalEntity.Initials;
                        base.textfieldSurname.Value = legalEntity.Surname;
                        base.textfieldPreferredName.Value = legalEntity.PreferredName;
                        base.textfieldTaxNumber.Value = legalEntity.TaxNumber;
                        base.textfieldIDNumber.Value = legalEntity.IdNumber;
                        base.textfieldPassportNumber.Value = legalEntity.PassportNumber;
                        base.textfieldTaxNumber.Value = legalEntity.TaxNumber;
                        base.textfieldDateOfBirth.Value = dateOfBirth;
                        base.textfieldHomePhoneCode.Value = legalEntity.HomePhoneCode.ToString();
                        base.textfieldHomePhoneNumber.Value = legalEntity.HomePhoneNumber.ToString();
                        base.textfieldWorkPhoneCode.Value = legalEntity.WorkPhoneCode.ToString();
                        base.textfieldWorkPhoneNumber.Value = legalEntity.WorkPhoneNumber.ToString();
                        base.textfieldCellphoneNo.Value = legalEntity.CellPhoneNumber.ToString();
                        base.textfieldEmailAddress.Value = legalEntity.EmailAddress;
                        base.textfieldFaxCode.Value = legalEntity.FaxCode.ToString();
                        base.textfieldFaxNumber.Value = legalEntity.FaxNumber.ToString();
                        base.selectCitizenshipType.SelectByValue(((int)legalEntity.CitizenTypeKey).ToString());
                        base.selectEducation.SelectByValue(((int)legalEntity.EducationKey).ToString());
                        base.selectHomeLanguage.SelectByValue(((int)legalEntity.HomeLanguageKey).ToString());
                        base.selectDocumentLanguage.SelectByValue(((int)legalEntity.DocumentLanguageKey).ToString());

                        break;
                    }

                #endregion NaturalPerson
            }

            base.btnCreateLead.Click();
        }

    }
}