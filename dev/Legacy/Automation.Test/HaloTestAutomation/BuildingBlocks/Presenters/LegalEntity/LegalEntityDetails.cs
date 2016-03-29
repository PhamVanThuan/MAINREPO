using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityDetails : LegalEntityDetailControls
    {
        private ICommonService commonService;

        public LegalEntityDetails()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        public void AddLegalEntity(bool LegalEntityExist, string id, string role, bool IsIncomeContributor, string salutation, string initials, string firstname,
                string surname, string preferredName, string gender, string maritalStatus, string populationGroup, string education, string citizenshipType,
                string passportNumber, string taxNumber, string homeLanguage, string documentLanguage, string status, string homeCode, string homeNumber,
                string workCode, string workNumber, string faxCode, string faxNumber, string cellNumber, string emailAddress, bool telemarketing, bool marketing,
                bool customerList, bool email, bool sms, string InsurableInterest, string dob)
        {
            if (IsIncomeContributor)
                base.AddIncomeContributor.Checked = true;

            base.AddSalutation.Select(salutation);
            base.AddInitials.Value = (initials);
            base.AddFirstNames.Value = (firstname);
            base.AddSurname.Value = (surname);
            base.AddPreferredName.Value = (preferredName);
            base.AddGender.Select(gender);
            base.AddMaritalStatus.Select(maritalStatus);
            base.AddPopulationGroup.Select(populationGroup);
            base.AddEducation.Select(education);
            base.AddCitizenshipType.Select(citizenshipType);
            base.AddDateOfBirth.Value = dob;
            if (!string.IsNullOrEmpty(passportNumber))
            {
                base.AddPassportNumber.TypeText(passportNumber);
            }
            else
            {
                if (LegalEntityExist)
                {
                    base.AddIDNumber.TypeText(id);
                    base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
                    base.SAHLAutoComplete_DefaultItem(id).MouseDown();
                }
                else
                {
                    base.AddIDNumber.TypeText(id);
                }
            }
            base.AddTaxNumber.Value = (taxNumber);
            base.AddHomeLanguage.Select(homeLanguage);
            base.AddDocumentLanguage.Select(documentLanguage);
            base.txtHomePhoneCode.Value = homeCode;
            base.txtHomePhoneNumber.Value = homeNumber;
            base.txtWorkPhoneCode.Value = workCode;
            base.txtWorkPhoneNumber.Value = workNumber;
            base.txtFaxCode.Value = faxCode;
            base.txtFaxNumber.Value = faxNumber;
            base.CellPhoneNumber.Value = cellNumber;
            base.txtEmailAddress.Value = emailAddress;
            base.A.Checked = telemarketing;
            base.B.Checked = marketing;
            base.C.Checked = customerList;
            base.D.Checked = email;
            base.E.Checked = sms;
            if (InsurableInterest != null)
            {
                if (!string.IsNullOrEmpty(InsurableInterest))
                    base.InsurableInterest.Option(InsurableInterest).Select();
            }
            base.btnSubmitButton.Click();
        }

        public void PopulateLegalEntity(Automation.DataModels.LegalEntity legalEntity = null, string insurableInterest = "")
        {
            if (!String.IsNullOrEmpty(insurableInterest))
                base.InsurableInterest.Option(insurableInterest).Select();
            if (legalEntity != null)
            {
                //all legalentities have this
                base.txtWorkPhoneCode.Value = legalEntity.WorkPhoneCode;
                base.txtWorkPhoneNumber.Value = legalEntity.WorkPhoneNumber;
                base.txtFaxCode.Value = legalEntity.FaxCode;
                base.txtFaxNumber.Value = legalEntity.FaxNumber;
                base.txtEmailAddress.Value = legalEntity.EmailAddress;
                base.MainMarketingOption0.Checked = legalEntity.TeleMarketing;
                base.MainMarketingOption1.Checked = legalEntity.Marketing;
                base.MainMarketingOption2.Checked = legalEntity.CustomerList;
                base.MainMarketingOption3.Checked = legalEntity.Email;
                base.MainMarketingOption4.Checked = legalEntity.Sms;
                base.CellPhoneNumber.Value = legalEntity.CellPhoneNumber;

                //only natural persons
                if (legalEntity.LegalEntityTypeKey == LegalEntityTypeEnum.NaturalPerson)
                {
                    base.txtHomePhoneCode.Value = legalEntity.HomePhoneCode;
                    base.txtHomePhoneNumber.Value = legalEntity.HomePhoneNumber;

                    if (base.ViewName.Text.Equals("Life_LegalEntityAddNew_Admin"))
                    {
                        base.AddSalutation.Select(legalEntity.SalutationDescription);
                        base.AddInitials.Value = legalEntity.Initials;
                        base.AddFirstNames.Value = legalEntity.FirstNames;
                        base.AddSurname.Value = legalEntity.Surname;
                        base.AddPreferredName.Value = legalEntity.PreferredName;
                        base.AddGender.Select(legalEntity.GenderDescription);
                        base.AddMaritalStatus.Select(legalEntity.MaritalStatusDescription);
                        base.AddPopulationGroup.Select(legalEntity.PopulationGroupDescription);
                        base.AddEducation.Select(legalEntity.EducationDescription);
                        base.AddCitizenshipType.Select(legalEntity.CitizenTypeDescription);
                        base.AddDateOfBirth.Value = legalEntity.DateOfBirth.Value.ToString(Formats.DateFormat);

                        Assert.That(!String.IsNullOrEmpty(legalEntity.PassportNumber) ||
                            !String.IsNullOrEmpty(legalEntity.IdNumber), "Idnumber or passport number needs to be specified.");

                        if (!String.IsNullOrEmpty(legalEntity.PassportNumber))
                            base.AddPassportNumber.Value = legalEntity.PassportNumber;
                        if (!String.IsNullOrEmpty(legalEntity.IdNumber))
                        {
                            base.AddIDNumber.TypeText(legalEntity.IdNumber);
                            var defaultItem = base.SAHLAutoComplete_DefaultItem(legalEntity.IdNumber);
                            if (defaultItem.Exists)
                                defaultItem.MouseDown();
                        }

                        base.AddTaxNumber.Value = legalEntity.TaxNumber;
                        base.AddHomeLanguage.Select(legalEntity.HomeLanguageDescription);
                        base.AddDocumentLanguage.Select(legalEntity.DocumentLanguageDescription);
                    }
                    else //need to find which view/views uses this
                    {
                        base.ctl00MainddlSalutationUpdate.Select(legalEntity.SalutationDescription);
                        base.ctl00MaintxtInitialsUpdate.Value = legalEntity.Initials;
                        base.ctl00MaintxtInitialsUpdate.Value = legalEntity.Initials;
                        base.ctl00MaintxtFirstNamesUpdate.Value = legalEntity.FirstNames;
                        base.ctl00MaintxtSurnameUpdate.Value = legalEntity.Surname;
                        base.ctl00MaintxtUpdPreferredName.Value = legalEntity.PreferredName;
                        base.ctl00MainddlGenderUpdate.Select(legalEntity.GenderDescription);
                        base.ctl00MainddlMaritalStatusUpdate.Select(legalEntity.MaritalStatusDescription);
                        base.ctl00MainddlUpdPopulationGroup.Select(legalEntity.PopulationGroupDescription);
                        base.ctl00MainddlEducation.Select(legalEntity.EducationDescription);
                        base.ctl00MainddlCitizenshipTypeUpdate.Select(legalEntity.CitizenTypeDescription);
                        base.ctl00MaintxtUpdDateOfBirth.Value = legalEntity.DateOfBirth.Value.ToString(Formats.DateFormat);
                        base.ctl00MaintxtUpdPassportNumber.Value = legalEntity.PassportNumber;
                        base.TaxNumberUpdate.Value = legalEntity.TaxNumber;
                        base.ctl00MainddlUpdHomeLanguage.Select(legalEntity.HomeLanguageDescription);
                        base.ctl00MainddlUpdDocumentLanguage.Select(legalEntity.DocumentLanguageDescription);
                        base.ctl00MaintxtIDNumberUpdate.TypeText(legalEntity.IdNumber);
                        base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(10);
                        var defaultItem = base.SAHLAutoComplete_DefaultItem(legalEntity.IdNumber);
                        if (defaultItem.Exists)
                            defaultItem.MouseDown();
                    }
                }
                //only non-natural persons
                else if (legalEntity.LegalEntityTypeKey == LegalEntityTypeEnum.CloseCorporation ||
                         legalEntity.LegalEntityTypeKey == LegalEntityTypeEnum.Company ||
                         legalEntity.LegalEntityTypeKey == LegalEntityTypeEnum.Trust)
                {
                    base.LegalEntityTypeUpdate.Select(legalEntity.LegalEntityTypeDescription);
                    base.IntroductionDateUpdate.Value = legalEntity.IntroductionDate.Value.ToString(Formats.DateFormat);
                    base.RegisteredNameUpdate.Value = legalEntity.RegisteredName;
                    base.RegistrationNumberUpdate.Value = legalEntity.RegistrationNumber;
                    base.TradingNameUpdate.Value = legalEntity.TradingName;
                    base.TaxNumberCompanyUpdate.Value = legalEntity.TaxNumber;
                    base.CompanyDocumentLanguageUpdate.Select(legalEntity.DocumentLanguageDescription);
                }
            }
        }

        public void UpdateLegalEntityContactDetail
         (
             string homeCode,
             string homeNumber,
             string workCode,
             string workNumber,
             string faxCode,
             string faxNumber,
             string cellNumber,
             string emailAddress
         )
        {
            base.txtHomePhoneCode.TypeText(homeCode);
            base.txtHomePhoneNumber.TypeText(homeNumber);
            base.txtWorkPhoneCode.TypeText(workCode);
            base.txtWorkPhoneNumber.TypeText(workNumber);
            base.txtFaxCode.TypeText(faxCode);
            base.txtFaxNumber.TypeText(faxNumber);
            base.CellPhoneNumber.TypeText(cellNumber);
            base.txtEmailAddress.TypeText(emailAddress);
            base.btnSubmitButton.Click();
        }

        public string GetIDNumber()
        {
            if (base.AddIDNumber.Exists)
                return base.AddIDNumber.Text;
            if (base.ctl00MaintxtIDNumberUpdate.Exists)
                return base.ctl00MaintxtIDNumberUpdate.Text;
            return null;
        }

        public bool IsNaturalPerson()
        {
            if (base.ddlLegalEntityTypes.Text == LegalEntityType.NaturalPerson)
                return true;
            return false;
        }

        public void ClickUpdate()
        {
            base.btnSubmitButton.Click();
        }

        public void AddExisting(string idnumber)
        {
            base.AddIDNumber.TypeText(idnumber);
            base.SAHLAutoCompleteDiv_iframe.WaitUntilExists(30);
            base.SAHLAutoComplete_DefaultItem(idnumber).MouseDown();
            if (base.ctl00MainddlUpdDocumentLanguage.Option("- Please select -").Selected)
            {
                base.ctl00MainddlUpdDocumentLanguage.Options[1].Select();
            }
            base.btnSubmitButton.Click();
        }

        public void Update(MaritalStatusEnum maritalStatus, LanguageEnum language)
        {
            if (base.ctl00MainddlMaritalStatusUpdate.Exists && ctl00MainddlMaritalStatusUpdate.Enabled)
            {
                base.ctl00MainddlMaritalStatusUpdate.SelectByValue(((int)maritalStatus).ToString());
            }
            if (base.ctl00MainddlUpdDocumentLanguage.Exists && ctl00MainddlUpdDocumentLanguage.Enabled)
            {
                base.ctl00MainddlUpdDocumentLanguage.SelectByValue(((int)language).ToString());
            }
            base.btnSubmitButton.Click();
        }

        public bool IsIDNumberEditable()
        {
            return base.IDNumberUpdateHidden.Enabled;
        }

        public bool IsMaritalStatusEditable()
        {
            return base.MaritalStatusHidden.Exists;
        }

        public void AddLegalEntity(string role, bool incomeContributor, string id, string firstname, string surname, string homePhoneCode, string homePhoneNumber)
        {
            string preferredName = String.Empty;
            string DOB = String.Empty;
            //Calculate DOB from IDNumber
            if (id != null)
            {
                DOB = commonService.GetDateOfBirthFromIDNumber(id);
            }

            base.ddlLegalEntityTypes.Option("Natural Person").Select();
            if (base.ddlRoleTypeAdd.Enabled)
                base.ddlRoleTypeAdd.Option(role).Select();
            if (base.AddIncomeContributor.Exists)
            {
                if (base.AddIncomeContributor.Checked && !incomeContributor)
                    base.AddIncomeContributor.Click();
                else if (!base.AddIncomeContributor.Checked && incomeContributor)
                    base.AddIncomeContributor.Click();
            }
            base.AddIDNumber.TypeText(id);
            base.AddSalutation.SelectByValue("1");
            string initials = firstname.Remove(1);
            base.AddInitials.Value = (initials);
            base.AddFirstNames.Value = (firstname);
            base.AddSurname.Value = (surname);
            if (firstname.Length > 3) preferredName = firstname.Remove(3);
            else preferredName = firstname;
            base.AddPreferredName.Value = (preferredName);
            base.AddGender.SelectByValue("1");
            base.AddMaritalStatus.Option(MaritalStatus.Single).Select();
            base.AddPopulationGroup.SelectByValue("1");
            base.AddEducation.SelectByValue("1");
            base.AddCitizenshipType.Option(CitizenType.SACitizen).Select();
            if (base.AddDateOfBirthControl.Exists)
            {
                base.AddDateOfBirthControl.Value = DOB;
            }
            if (base.AddDateOfBirth.Exists)
            {
                base.AddDateOfBirth.Value = DOB;
            }
            base.AddHomeLanguage.SelectByValue("1");
            base.txtHomePhoneCode.Value = (homePhoneCode);
            base.txtHomePhoneNumber.Value = (homePhoneNumber);

            base.btnSubmitButton.Click();
        }

        public void AddLegalEntityCompany(string legalEntityType, string companyName, string workPhoneCode, string workPhoneNumber)
        {
            base.ddlLegalEntityTypes.Option(legalEntityType).Select();
            base.txtCOAddCompanyName.TypeText(companyName);
            base.txtWorkPhoneCode.TypeText(workPhoneCode);
            base.txtWorkPhoneNumber.TypeText(workPhoneNumber);
            base.btnSubmitButton.Click();
        }

        /// <summary>
        /// Clicks the Submit Button
        /// </summary>
        public void ClickSubmitButton()
        {
            base.btnSubmitButton.Click();
        }

        /// <summary>
        /// Asserts the contents of the Role Type Dropdown
        /// </summary>
        /// <param name="expectedContents"></param>
        public void AssertRoleDropdownContents(List<string> expectedContents, bool assertCount)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlRoleTypeAdd, expectedContents, assertCount);
        }

        /// <summary>
        /// Asserts the contents of the Legal Entity Type Dropdown
        /// </summary>
        /// <param name="expectedContents"></param>
        public void AssertLegalEntityTypeDropdownContents(List<string> expectedContents)
        {
            Assertions.WatiNAssertions.AssertSelectListContents(base.ddlLegalEntityTypes, expectedContents, false);
        }

        public void AssertViewDisplayed(string viewname)
        {
            Assert.AreEqual(viewname, base.ViewName.Text, "Current view is not being displayed. Current View Displayed: {0}", base.ViewName.Text);
        }
    }
}