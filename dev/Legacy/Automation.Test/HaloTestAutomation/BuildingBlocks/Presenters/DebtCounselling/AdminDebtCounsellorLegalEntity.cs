using System;
using System.Threading;
using WatiN.Core;
using ObjectMaps;
using System.Text.RegularExpressions;
using CommonData.Constants;
using CommonData.Enums;
using System.Collections.Generic;
using SQLQuerying.Models;
using ObjectMaps.Pages;

namespace BuildingBlocks
{
    public static partial class Views
    {
        public sealed class AdminDebtCounsellorLegalEntity : AdminDebtCounsellorLegalEntityControls
        {
            /// <summary>
            /// Will populate the fields on this view regardless regardless of adding/updating
            /// </summary>
            public void Populate(SQLQuerying.Models.LegalEntityOrganisationStructure legalEntityOrgStructure)
            {
                if (base.LegalEntityType.Exists && base.LegalEntityType.Enabled)
                    base.LegalEntityType.Option(legalEntityOrgStructure.LegalEntity.LegalEntityTypeDescription).Select();
                if (base.Role.Exists && base.LegalEntityType.Enabled)
                    base.Role.Option("Contact").Select();
                if (base.OrganisationType.Exists && base.OrganisationType.Enabled)
                    base.OrganisationType.Option(legalEntityOrgStructure.OrganisationStructure.OrganisationTypeDescription).Select();
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
                    base.HomePhoneNumber.Value = legalEntityOrgStructure.LegalEntity.HomePhoneCode.ToString();
                if (base.Salutation.Exists && base.Salutation.Enabled)
                    base.Salutation.Option(legalEntityOrgStructure.LegalEntity.SalutationDescription).Select();
                if (base.Gender.Exists && base.Gender.Enabled)
                    base.Salutation.Option(legalEntityOrgStructure.LegalEntity.GenderDescription).Select();
                if (base.IDNumber.Exists && base.IDNumber.Enabled)
                    base.IDNumber.Value = legalEntityOrgStructure.LegalEntity.WorkPhoneCode.ToString();
                if (base.Initials.Exists && base.Initials.Enabled)
                    base.Initials.Value = legalEntityOrgStructure.LegalEntity.WorkPhoneCode.ToString();
                if (base.Surname.Exists && base.Surname.Enabled)
                    base.Surname.Value = legalEntityOrgStructure.LegalEntity.WorkPhoneCode.ToString();
                if (base.PreferredName.Exists && base.PreferredName.Enabled)
                    base.PreferredName.Value = legalEntityOrgStructure.LegalEntity.WorkPhoneCode.ToString();
                if (base.CellPhoneNumber.Exists && base.CellPhoneNumber.Enabled)
                    base.CellPhoneNumber.Value = legalEntityOrgStructure.LegalEntity.WorkPhoneCode.ToString();
            }
            /// <summary>
            /// Click the Submit button on the view regardless of Adding/Updating
            /// </summary>
            /// <param name="TestBrowser"></param>
            public void Submit()
            {
                base.SubmitButton.Click();
            }
            /// <summary>
            /// This will capture a residential address provided a legalentityaddress
            /// </summary>
            public void CaptureResidentialAddress(SQLQuerying.Models.LegalEntityAddress legalentityAddress)
            {
                base.linkLegalEntityAddressDetails.Click();
                base.AddressType.Option(legalentityAddress.AddressTypDescription).Select();
                base.AddressFormat.Option(legalentityAddress.Address.AddressFormatDescription).Select();
                switch (legalentityAddress.AddressTypeKey)
                {
                    case AddressTypeEnum.Residential:
                        base.UnitNumber.Value = legalentityAddress.Address.UnitNumber;
                        base.BuildingNumber.Value = legalentityAddress.Address.UnitNumber;
                        base.BuildingName.Value = legalentityAddress.Address.UnitNumber;
                        base.StreetNumber.Value = legalentityAddress.Address.StreetNumber;
                        base.StreetName.Value = legalentityAddress.Address.StreetName;
                        base.Country.Option(legalentityAddress.Address.RRR_CountryDescription).Select();
                        base.Province.Option(legalentityAddress.Address.RRR_ProvinceDescription).Select();
                        base.Suburb.TypeText(legalentityAddress.Address.RRR_SuburbDescription);
                        base.SAHLAutoComplete_DefaultItem.WaitUntilExists(30);
                        base.SAHLAutoComplete_DefaultItem.MouseDown();
                        break;
                }
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
            /// <summary>
            /// Click the Remove button on the view.
            /// </summary>
            /// <param name="TestBrowser"></param>
            public void ClickRemove()
            {
                base.SubmitButton.Click();
            }
        }
    }
}
