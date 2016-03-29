using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityDetailsUpdateApplicant : LegalEntityDetailsUpdateApplicantControls
    {
        private readonly ICommonService commonService;

        public LegalEntityDetailsUpdateApplicant()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// Updates a natural person legal entity to have a new id number with a set of default legal entity details.
        /// </summary>
        /// <param name="id"></param>
        public void UpdateLegalEntityDetails_NaturalPerson(string id)
        {
            string preferredName = String.Empty;
            string DOB = commonService.GetDateOfBirthFromIDNumber(id);
            base.txtIDNumberUpdate.TypeText(id);
            base.ddlSalutationUpdate.Option(SalutationType.Mr).Select();
            string initials = base.txtFirstNamesUpdate.Value.Remove(1);
            base.txtInitialsUpdate.TypeText(initials);
            if (base.txtFirstNamesUpdate.Value.Length > 3) preferredName = base.txtFirstNamesUpdate.Value.Remove(3);
            else preferredName = base.txtFirstNamesUpdate.Value;
            base.txtUpdPreferredName.TypeText(preferredName);
            base.ddlGenderUpdate.Option(Gender.Male).Select();
            base.ddlMaritalStatusUpdate.Option(MaritalStatus.Single).Select();
            base.ddlUpdPopulationGroup.Option(PopulationGroup.White).Select();
            base.ddlEducation.Option(Education.Matric).Select();
            base.ddlCitizenshipTypeUpdate.Option(CitizenType.SACitizen).Select();
            base.txtUpdDateOfBirth.Value = DOB;
            base.ddlUpdHomeLanguage.Option(Language.English).Select();
            base.btnSubmitButton.Click();
        }

        /// <summary>
        /// Updates the details of a legal entity natural person
        /// </summary>
        /// <param name="role"></param>
        /// <param name="incomeContributor"></param>
        /// <param name="id"></param>
        /// <param name="salutation"></param>
        /// <param name="initials"></param>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="preferredName"></param>
        /// <param name="gender"></param>
        /// <param name="maritalStatus"></param>
        /// <param name="populationGroup"></param>
        /// <param name="education"></param>
        /// <param name="citizenshipType"></param>
        /// <param name="dob"></param>
        /// <param name="passportNumber"></param>
        /// <param name="taxNumber"></param>
        /// <param name="homeLanguage"></param>
        /// <param name="documentLanguage"></param>
        /// <param name="status"></param>
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
        public void UpdateLegalEntityDetails_NaturalPerson(string role, bool incomeContributor, string id, string salutation, string initials,
            string firstname, string surname, string preferredName, string gender, string maritalStatus, string populationGroup,
            string education, string citizenshipType, string dob, string passportNumber, string taxNumber, string homeLanguage,
            string documentLanguage, string status, string homeCode, string homeNumber, string workCode, string workNumber, string faxCode,
            string faxNumber, string cellNumber, string emailAddress, bool telemarketing, bool marketing, bool customerList, bool email, bool sms,
            ButtonTypeEnum btn)
        {
            //Calculate DOB from IDNumber
            if (dob == "auto" && id != null)
            {
                dob = "";
                for (int index = 4; index >= 0; index = (index - 2))
                {
                    if (index == 0)
                    {
                        if (Convert.ToInt32(id.Substring(index, 2)) > 20) dob = dob + "19" + id.Substring(index, 2);
                        else dob = dob + "20" + id.Substring(index, 2);
                    }
                    else dob = dob + id.Substring(index, 2) + "/";
                }
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

            if (role != null) base.ddlRoleTypeUpdate.Option(role).Select();
            //Set Income Contributor checkbox
            if (incomeContributor && !base.chkNatUpdIncomeContributor.Checked)
            {
                base.chkNatUpdIncomeContributor.Click();
            }
            if (!incomeContributor && base.chkNatUpdIncomeContributor.Checked)
            {
                base.chkNatUpdIncomeContributor.Click();
            }
            if (id != null) base.txtIDNumberUpdate.TypeText(id);
            if (salutation != null) base.ddlSalutationUpdate.Option(salutation).Select();
            if (initials != null && initials != "auto") base.txtInitialsUpdate.TypeText(initials);
            if (firstname != null) base.txtFirstNamesUpdate.TypeText(firstname);
            if (surname != null) base.txtSurnameUpdate.TypeText(surname);
            if (preferredName != null && preferredName != "auto") base.txtUpdPreferredName.TypeText(preferredName);
            if (gender != null) base.ddlGenderUpdate.Option(gender).Select();
            if (maritalStatus != null) base.ddlMaritalStatusUpdate.Option(maritalStatus).Select();
            if (populationGroup != null) base.ddlUpdPopulationGroup.Option(populationGroup).Select();
            if (education != null) base.ddlEducation.Option(education).Select();
            if (citizenshipType != null) base.ddlCitizenshipTypeUpdate.Option(citizenshipType).Select();
            if (dob != null && dob != "auto") base.txtUpdDateOfBirth.TypeText(dob);
            if (passportNumber != null) base.txtUpdPassportNumber.TypeText(passportNumber);
            if (taxNumber != null) base.txtUpdTaxNumber.TypeText(taxNumber);
            if (homeLanguage != null) base.ddlUpdHomeLanguage.Option(homeLanguage).Select();
            if (documentLanguage != null) base.ddlUpdDocumentLanguage.Option(documentLanguage).Select();
            if (status != null) base.ddlUpdStatus.Option(status).Select();
            //Contact Details
            if (homeCode != null) base.txtHomePhoneCode.TypeText(homeCode);
            if (homeNumber != null) base.txtHomePhoneNumber.TypeText(homeNumber);
            if (workCode != null) base.txtWorkPhoneCode.TypeText(workCode);
            if (workNumber != null) base.txtWorkPhoneNumber.TypeText(workNumber);
            if (faxCode != null) base.txtFaxCode.TypeText(faxCode);
            if (faxNumber != null) base.txtFaxNumber.TypeText(faxNumber);
            if (cellNumber != null) base.txtCellPhoneNumber.TypeText(cellNumber);
            if (emailAddress != null) base.txtEmailAddress.TypeText(emailAddress);
            //Set Telemarketing checkbox
            if (telemarketing && !base.checkboxTelemarketing.Checked)
            {
                base.checkboxTelemarketing.Click();
            }
            if (!telemarketing && base.checkboxTelemarketing.Checked)
            {
                base.checkboxTelemarketing.Click();
            }
            //Set Marketing checkbox
            if (marketing && !base.checkboxMarketing.Checked)
            {
                base.checkboxMarketing.Click();
            }
            if (!marketing && base.checkboxMarketing.Checked)
            {
                base.checkboxMarketing.Click();
            }
            //Set Custimer List checkbox
            if (customerList && !base.checkboxCustomerLists.Checked)
            {
                base.checkboxCustomerLists.Click();
            }
            if (!customerList && base.checkboxCustomerLists.Checked)
            {
                base.checkboxCustomerLists.Click();
            }
            //Set Email checkbox
            if (email && !base.checkboxEmail.Checked)
            {
                base.checkboxEmail.Click();
            }
            if (!email && base.checkboxEmail.Checked)
            {
                base.checkboxEmail.Click();
            }
            //set SMS checkbox
            if (sms && !base.checkboxSMS.Checked)
            {
                base.checkboxSMS.Click();
            }
            if (!sms && base.checkboxSMS.Checked)
            {
                base.checkboxSMS.Click();
            }

            switch (btn)
            {
                case ButtonTypeEnum.Update:
                    base.btnSubmitButton.Click();
                    break;

                case ButtonTypeEnum.Cancel:
                    base.btnCancelButton.Click();
                    break;
            }
        }

        /// <summary>
        /// Updates the legal entity details but takes in the legal entity model.
        /// </summary>
        /// <param name="legalEntity"></param>
        public void UpdateLegalEntityDetails(Automation.DataModels.LegalEntity legalEntity)
        {
            switch (legalEntity.LegalEntityTypeKey)
            {
                #region Company

                case Common.Enums.LegalEntityTypeEnum.Company:
                    {
                        base.RegisteredNameUpdate.Value = legalEntity.RegisteredName;
                        base.txtCOUpdTradingName.Value = legalEntity.TradingName;
                        base.RegistrationNumberUpdate.Value = legalEntity.RegistrationNumber;
                        base.txtCOUpdTaxNumber.Value = legalEntity.TaxNumber;
                        break;
                    }

                #endregion Company

                #region NaturalPerson

                case LegalEntityTypeEnum.NaturalPerson:
                    {
                        string dateOfBirth = legalEntity.CitizenTypeKey == CitizenTypeEnum.SACitizen ?
                            commonService.GetDateOfBirthFromIDNumber(legalEntity.IdNumber) : "01/01/1970";
                        base.ddlMaritalStatusUpdate.SelectByValue(((int)legalEntity.MaritalStatusKey).ToString());
                        base.ddlGenderUpdate.SelectByValue(((int)legalEntity.GenderKey).ToString());
                        base.ddlUpdPopulationGroup.SelectByValue(((int)legalEntity.PopulationGroupKey).ToString());
                        base.ddlSalutationUpdate.SelectByValue(((int)legalEntity.SalutationKey).ToString());
                        base.txtFirstNamesUpdate.Value = legalEntity.FirstNames;
                        base.txtInitialsUpdate.Value = legalEntity.Initials;
                        base.txtSurnameUpdate.Value = legalEntity.Surname;
                        base.txtUpdPreferredName.Value = legalEntity.PreferredName;
                        base.txtUpdTaxNumber.Value = legalEntity.TaxNumber;
                        base.txtIDNumberUpdate.Value = legalEntity.IdNumber;
                        base.txtUpdPassportNumber.Value = legalEntity.PassportNumber;
                        base.txtUpdTaxNumber.Value = legalEntity.PassportNumber;
                        base.txtUpdDateOfBirth.Value = dateOfBirth;
                        base.txtHomePhoneCode.Value = legalEntity.HomePhoneCode.ToString();
                        base.txtHomePhoneNumber.Value = legalEntity.HomePhoneNumber.ToString();
                        base.txtWorkPhoneCode.Value = legalEntity.WorkPhoneCode.ToString();
                        base.txtWorkPhoneNumber.Value = legalEntity.WorkPhoneNumber.ToString();
                        base.txtCellPhoneNumber.Value = legalEntity.CellPhoneNumber.ToString();
                        base.txtEmailAddress.Value = legalEntity.EmailAddress;
                        base.txtFaxCode.Value = legalEntity.FaxCode.ToString();
                        base.txtFaxNumber.Value = legalEntity.FaxNumber.ToString();

                        base.ddlCitizenshipTypeUpdate.SelectByValue(((int)legalEntity.CitizenTypeKey).ToString());
                        base.ddlEducation.SelectByValue(((int)legalEntity.EducationKey).ToString());
                        base.ddlUpdHomeLanguage.SelectByValue(((int)legalEntity.HomeLanguageKey).ToString());
                        base.ddlUpdDocumentLanguage.SelectByValue(((int)legalEntity.DocumentLanguageKey).ToString());

                        break;
                    }

                #endregion NaturalPerson
            }

            base.btnSubmitButton.Click();
        }
    }
}