﻿using WatiN.Core;
using ObjectMaps;
using System;
using CommonData.Enums;
using WatiN.Core.UtilityClasses;
using System.Threading;
using SQLQuerying.Models;
using CommonData.Constants;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class MaintenanceLegalEntity : MaintenanceLegalEntityControls
    {
        /// <summary>
        /// Will populate the fields on this view regardless regardless of adding/updating
        /// </summary>
        public void PopulateLegalEntity(SQLQuerying.Models.LegalEntityOrganisationStructureModel legalEntityOrgStructure)
        {
            base.linkLegalEntityDetails.Click();
            if (base.LegalEntityType.Exists && base.LegalEntityType.Enabled)
                base.LegalEntityType.Option(legalEntityOrgStructure.LegalEntity.LegalEntityTypeDescription).Select();
            if (base.OrganisationType.Exists && base.OrganisationType.Enabled)
                base.OrganisationType.Option(legalEntityOrgStructure.OrganisationStructure.OrganisationTypeDescription).Select();
            if (base.Role.Exists && base.Role.Enabled)
                base.Role.Option(legalEntityOrgStructure.OrganisationStructure.OrganisationTypeDescription).Select();
            if (base.RegistrationNumberUpdate.Exists)
                base.RegistrationNumberUpdate.TypeText(legalEntityOrgStructure.LegalEntity.RegistrationNumber);
            if (base.FirstNames.Exists && base.FirstNames.Enabled)
                base.FirstNames.Value = legalEntityOrgStructure.LegalEntity.FirstNames;
            if (base.RegisteredName.Exists && base.RegisteredName.Enabled)
                base.RegisteredName.Value = legalEntityOrgStructure.LegalEntity.RegisteredName;
            if (base.WorkPhoneCode.Exists && base.WorkPhoneCode.Enabled)
                base.WorkPhoneCode.Value = legalEntityOrgStructure.LegalEntity.WorkPhoneCode.ToString();
            if (base.WorkPhoneNumber.Exists && base.WorkPhoneNumber.Enabled)
                base.WorkPhoneNumber.Value = legalEntityOrgStructure.LegalEntity.WorkPhoneNumber.ToString();
            if (base.FaxCode.Exists && base.FaxCode.Enabled)
                base.FaxCode.Value = legalEntityOrgStructure.LegalEntity.FaxCode.ToString();
            if (base.FaxNumber.Exists && base.FaxNumber.Enabled)
                base.FaxNumber.Value = legalEntityOrgStructure.LegalEntity.FaxNumber.ToString();
            if (base.EmailAddress.Exists && base.EmailAddress.Enabled)
                base.EmailAddress.Value = legalEntityOrgStructure.LegalEntity.EmailAddress.ToString();
            if (base.HomePhoneCode.Exists && base.HomePhoneCode.Enabled)
                base.HomePhoneCode.Value = legalEntityOrgStructure.LegalEntity.HomePhoneCode.ToString();
            if (base.HomePhoneNumber.Exists && base.HomePhoneNumber.Enabled)
                base.HomePhoneNumber.Value = legalEntityOrgStructure.LegalEntity.HomePhoneNumber.ToString();
            if (base.Salutation.Exists && base.Salutation.Enabled)
                base.Salutation.Option(legalEntityOrgStructure.LegalEntity.SalutationDescription).Select();
            if (base.Gender.Exists && base.Gender.Enabled)
                base.Gender.Option(legalEntityOrgStructure.LegalEntity.GenderDescription).Select();
            if (base.IDNumber.Exists && base.IDNumber.Enabled)
                base.IDNumber.Value = legalEntityOrgStructure.LegalEntity.IdNumber.ToString();
            if (base.Initials.Exists && base.Initials.Enabled)
                base.Initials.Value = legalEntityOrgStructure.LegalEntity.Initials.ToString();
            if (base.Surname.Exists && base.Surname.Enabled)
                base.Surname.Value = legalEntityOrgStructure.LegalEntity.Surname.ToString();
            if (base.PreferredName.Exists && base.PreferredName.Enabled)
                base.PreferredName.Value = legalEntityOrgStructure.LegalEntity.PreferredName.ToString();
            if (base.CellPhoneNumber.Exists && base.CellPhoneNumber.Enabled)
                base.CellPhoneNumber.Value = legalEntityOrgStructure.LegalEntity.CellPhoneNumber.ToString();
        }
        /// <summary>
        /// Get all validation messages on screen.
        /// </summary>
        /// <returns></returns>
        public string GetValidationMessages()
        {
            var elementCollection = base.SAHLValidationSummaryBody.ElementsWithTag("ol");
            if (elementCollection.Count == 0)
                return String.Empty;
            var messageElement = elementCollection[0];
            return messageElement.Text;
        }
        public void PopulateAddress(AddressModel address, string addressTypeDescription, string addressFormatDescription, DateTime effectiveDate)
        {

            base.linkLegalEntityAddressDetails.Click();

            base.AddressType.Option(addressTypeDescription).Select();
            base.AddressFormat.Option(addressFormatDescription).Select();
            base.EffectiveDate.Value = effectiveDate.ToString(Formats.DateFormat);
            base.UnitNumber.TypeText(address.UnitNumber);
            base.BuildingNumber.TypeText(address.BuildingNumber);
            base.BuildingName.TypeText(address.BuildingName);
            base.StreetNumber.TypeText(address.StreetNumber);
            base.StreetName.TypeText(address.StreetName);
            base.Country.Option(address.RRR_CountryDescription).Select();
            base.Province.Option(address.RRR_ProvinceDescription).Select();
            base.Suburb.TypeText(address.RRR_SuburbDescription);
            var timer = new SimpleTimer(TimeSpan.FromSeconds(30));
            while (!timer.Elapsed)
            {
                if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                {
                    base.SAHLAutoComplete_DefaultItem_Collection()[0].MouseDown();
                    break;
                }
            }
        }
        public void Add()
        {
            base.SubmitButton.Click();
        }
        public void Cancel()
        {
            base.CancelButton.Click();
        }
        public void Update()
        {
            base.SubmitButton.Click();
        }
        public void ClickRemove()
        {
            base.SubmitButton.Click();
        }
        public LegalEntityOrganisationStructureModel GetLegalEntityDetail()
        {
            var legalentity = new LegalEntityModel();
            var organisationStructure = new OrganisationStructure();

            if (base.lblLEType.Exists)
                legalentity.LegalEntityTypeDescription = base.lblLEType.Text;
            if (base.lblOrganisationTypeDisplay.Exists)
                organisationStructure.OrganisationTypeDescription = base.lblOrganisationTypeDisplay.Text;
            if (base.lblCORegistrationNumber.Exists)
                legalentity.RegistrationNumber = base.lblCORegistrationNumber.Text;
            if (base.lblFirstNames.Exists)
                legalentity.FirstNames = base.lblFirstNames.Text;
            if (base.lblCORegisteredName.Exists)
                legalentity.RegisteredName = base.lblCORegisteredName.Text;
            if (base.lblWorkPhone.Exists)
            {
                var split = base.lblWorkPhone.Text.Split(' ');
                split[0] = split[0].Replace("(", "");
                split[0] = split[0].Replace(")", "");

                legalentity.WorkPhoneCode = split[0];
                legalentity.WorkPhoneNumber = split[1];
            }
            if (base.FaxCode.Exists)
            {
                var split = base.lblFaxNumber.Text.Split(' ');
                split[0] = split[0].Replace("(", "");
                split[0] = split[0].Replace(")", "");

                legalentity.FaxCode = split[0];
                legalentity.FaxNumber = split[1];
            }
            if (base.lblEmailAddress.Exists)
                legalentity.EmailAddress = base.lblEmailAddress.Text;
            if (base.HomePhoneCode.Exists)
            {
                var split = base.lblHomePhone.Text.Split(' ');
                split[0] = split[0].Replace("(", "");
                split[0] = split[0].Replace(")", "");

                legalentity.HomePhoneCode = split[0];
                legalentity.HomePhoneNumber = split[1];
            }
            if (base.lblSalutation.Exists)
                legalentity.SalutationDescription = base.lblSalutation.Text;
            if (base.lblGender.Exists)
                legalentity.GenderDescription = base.lblGender.Text;
            if (base.lblIDNumber.Exists)
                legalentity.IdNumber = base.lblIDNumber.Text;
            if (base.lblInitials.Exists)
                legalentity.Initials = base.lblInitials.Text;
            if (base.lblSurname.Exists)
                legalentity.Surname = base.lblSurname.Text;
            if (base.lblPreferredName.Exists)
                legalentity.PreferredName = base.lblPreferredName.Text;
            if (base.lblCellphoneNumber.Exists)
                legalentity.CellPhoneNumber = base.lblCellphoneNumber.Text;

            return new LegalEntityOrganisationStructureModel()
            {
                LegalEntity = legalentity,
                OrganisationStructure = organisationStructure
            };
        }
    }
}
