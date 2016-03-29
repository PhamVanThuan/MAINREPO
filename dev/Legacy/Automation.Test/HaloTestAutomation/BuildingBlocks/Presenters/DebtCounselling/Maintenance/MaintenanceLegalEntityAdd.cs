using System;
using WatiN.Core;
using ObjectMaps;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks
{
    public static partial class Views
    {
        public class AdminPaymentDistributionAgentLegalEntityAdd : MaintenanceLegalEntityAddControls
        {
            public void AddLegalEntityDetails_Company( 
                string type, 
                string organisationType,
                string coRegisteredName,
                string coRegisteredNumber,
                string workPhoneCode, 
                string workPhoneNumber, 
                string faxCode, 
                string faxNumber, 
                string emailAddress,
                btn buttonLabel)
            {

                base.linkLegalEntityDetails.Click();

                if (!string.IsNullOrEmpty(type)) 
                    base.ctl00MainapLegalEntityDetailscontentddlLegalEntityTypes.Option(type).Select();
                if (!string.IsNullOrEmpty(organisationType))
                {
                    SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                    while (base.ctl00MainapLegalEntityDetailscontentddlOrganisationType.SelectedOption.Text != organisationType)
                    {
                        if (timer.Elapsed) 
                            break;
                        base.ctl00MainapLegalEntityDetailscontentddlOrganisationType.Option(organisationType).Select();
                    }
                }
                System.Threading.Thread.Sleep(1000);
                if (coRegisteredName != null) 
                    base.ctl00MainapLegalEntityDetailscontentCORegisteredNameUpdate.TypeText(coRegisteredName);
                if (coRegisteredNumber != null) 
                    base.ctl00MainapLegalEntityDetailscontentCORegistrationNumberUpdate.TypeText(coRegisteredNumber);
                if (workPhoneCode != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtWorkPhoneCode.TypeText(workPhoneCode);
                if (workPhoneNumber != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtWorkPhoneNumber.TypeText(workPhoneNumber);
                if (faxCode != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtFaxCode.TypeText(faxCode);
                if (faxNumber != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtFaxNumber.TypeText(faxNumber);
                if (emailAddress != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtEmailAddress.TypeText(emailAddress);
                ClickButton(buttonLabel);
            }

            public void AddLegalEntityDetails_Company(
                string type,
                string organisationType,
                string coRegisteredName,
                string coRegisteredNumber,
                string workPhoneCode,
                string workPhoneNumber,
                string faxCode,
                string faxNumber,
                string emailAddress)
            {
                AddLegalEntityDetails_Company(
                    type,
                    organisationType,
                    coRegisteredName,
                    coRegisteredNumber,
                    workPhoneCode,
                    workPhoneNumber,
                    faxCode,
                    faxNumber,
                    emailAddress,
                    btn.None);
            }

            public void AddLegalEntityDetails_Company(
                string organisationType,
                string coRegisteredName,
                string coRegisteredNumber,
                string workPhoneCode,
                string workPhoneNumber,
                btn buttonLabel)
            {
                AddLegalEntityDetails_Company(
                    "Company",
                    organisationType,
                    coRegisteredName,
                    coRegisteredNumber,
                    workPhoneCode,
                    workPhoneNumber,
                    null,
                    null,
                    null,
                    buttonLabel);
            }

            public void AddLegalEntityDetails_NaturalPerson(
                string type,
                string Role,
                string idNumber,
                string salutation,
                string initials,
                string firstNames,
                string surname,
                string preferredname,
                string gender,
                string status,
                string homePhoneCode,
                string homePhoneNumber,
                string workPhoneCode,
                string workPhoneNumber,
                string faxCode,
                string faxNumber,
                string cellphoneNo,
                string emailAddress,
                btn buttonLabel)
            {

                if (!string.IsNullOrEmpty(type)) 
                    base.ctl00MainapLegalEntityDetailscontentddlLegalEntityTypes.Option(type).Select();
                if (!string.IsNullOrEmpty(Role))
                    base.ctl00MainapLegalEntityDetailscontentddlOSDescriptionTypeAdd.Option(Role).Select();
                if (idNumber != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtNatAddIDNumber.TypeText(idNumber);
                if (!string.IsNullOrEmpty(salutation)) 
                    base.ctl00MainapLegalEntityDetailscontentddlNatAddSalutation.Option(salutation).Select();
                if (initials != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtNatAddInitials.TypeText(initials);
                if (firstNames != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtNatAddFirstNames.TypeText(firstNames);
                if (surname != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtNatAddSurname.TypeText(surname);
                if (preferredname != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtNatAddPreferredName.TypeText(preferredname);
                if (!string.IsNullOrEmpty(gender)) 
                    base.ctl00MainapLegalEntityDetailscontentddlNatAddGender.Option(gender).Select();
                if (!string.IsNullOrEmpty(status)) 
                    base.ctl00MainapLegalEntityDetailscontentddlNatAddStatus.Option(status).Select();
                if (homePhoneCode != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtHomePhoneCode.TypeText(homePhoneCode);
                if (homePhoneNumber != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtHomePhoneNumber.TypeText(homePhoneNumber);
                if (workPhoneCode != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtWorkPhoneCode.TypeText(workPhoneCode);
                if (workPhoneNumber != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtWorkPhoneNumber.TypeText(workPhoneNumber);
                if (faxCode != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtFaxCode.TypeText(faxCode);
                if (faxNumber != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtFaxNumber.TypeText(faxNumber);
                if (cellphoneNo != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtCellPhoneNumber.TypeText(cellphoneNo);
                if (emailAddress != null) 
                    base.ctl00MainapLegalEntityDetailscontenttxtEmailAddress.TypeText(emailAddress);
                ClickButton(buttonLabel);
            }

            public void AddLegalEntityDetails_NaturalPerson(
               string type,
               string Role,
               string idNumber,
               string salutation,
               string initials,
               string firstNames,
               string surname,
               string preferredname,
               string gender,
               string status,
               string homePhoneCode,
               string homePhoneNumber,
               string workPhoneCode,
               string workPhoneNumber,
               string faxCode,
               string faxNumber,
               string cellphoneNo,
               string emailAddress)
            {
                AddLegalEntityDetails_NaturalPerson(
                   type,
                   Role,
                   idNumber,
                   salutation,
                   initials,
                   firstNames,
                   surname,
                   preferredname,
                   gender,
                   status,
                   homePhoneCode,
                   homePhoneNumber,
                   workPhoneCode,
                   workPhoneNumber,
                   faxCode,
                   faxNumber,
                   cellphoneNo,
                   emailAddress,
                   btn.None);
            }

            public void AddLegalEntityDetails_NaturalPerson(
               string salutation,
               string firstNames,
               string surname,
               string homePhoneCode,
               string homePhoneNumber)
            {
                AddLegalEntityDetails_NaturalPerson(
                   "Natural Person",
                   "Contact",
                   null,
                   salutation,
                   null,
                   firstNames,
                   surname,
                   null,
                   null,
                   null,
                   homePhoneCode,
                   homePhoneNumber,
                   null,
                   null,
                   null,
                   null,
                   null,
                   null,
                   btn.Add);
            }

            public void AddLegalEntityAddressDetails(
                string addressType,
                string addressFormat,
                DateTime effectiveDate,
                int unitNumber,
                int buildingNumber,
                string buildingName,
                int streetNumber,
                string streetname,
                string country,
                string province,
                string suburb,
                btn buttonLabel)
            {

                base.linkLegalEntityAddressDetails.Click();

                if (!string.IsNullOrEmpty(addressType)) 
                    base.ctl00MainapLegalEntityAddDetailscontentddlAddressType.Option(addressType).Select();
                if (!string.IsNullOrEmpty(addressFormat)) 
                    base.ctl00MainapLegalEntityAddDetailscontentddlAddressFormat.Option(addressFormat).Select();
                if (effectiveDate != null) 
                    base.ctl00MainapLegalEntityAddDetailscontentdtEffectiveDate.Value = BuildingBlocks.Common.General.ConvertToDateString(effectiveDate);
                if (unitNumber != -1) 
                    base.ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtUnitNumber.TypeText(unitNumber.ToString());
                if (buildingNumber != -1) 
                    base.ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtBuildingNumber.TypeText(buildingNumber.ToString());
                if (buildingName != null) 
                    base.ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtBuildingName.TypeText(buildingName);
                if (streetNumber != -1) 
                    base.ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtStreetNumber.TypeText(streetNumber.ToString());
                if (streetname != null) 
                    base.ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtStreetName.TypeText(streetname);
                if (!string.IsNullOrEmpty(country)) 
                    base.ctl00MainapLegalEntityAddDetailscontentaddressDetailsddlCountry.Option(country).Select();
                if (!string.IsNullOrEmpty(province)) 
                    base.ctl00MainapLegalEntityAddDetailscontentaddressDetailsddlProvince.Option(province).Select();
                if (suburb != null) base.ctl00MainapLegalEntityAddDetailscontentaddressDetailstxtSuburb.TypeText(suburb);

                bool executed = false;
                SimpleTimer timer = new SimpleTimer(TimeSpan.FromSeconds(30));
                while (!timer.Elapsed)
                {
                    if (base.SAHLAutoComplete_DefaultItem_Collection().Count > 0)
                    {
                        base.SAHLAutoComplete_DefaultItem_Collection()[0].MouseDown();
                        executed = true;
                        break;
                    }
                }
                if (!executed) 
                    throw new WatiN.Core.Exceptions.TimeoutException("attempting to select Suburb from ajax control");

                ClickButton(buttonLabel);
            }

            public void AddLegalEntityAddressDetails(
                string addressType,
                string addressFormat,
                DateTime effectiveDate,
                int unitNumber,
                int buildingNumber,
                string buildingName,
                int streetNumber,
                string streetname,
                string country,
                string province,
                string suburb)
            {
                AddLegalEntityAddressDetails(
                addressType,
                addressFormat,
                effectiveDate,
                unitNumber,
                buildingNumber,
                buildingName,
                streetNumber,
                streetname,
                country,
                province,
                suburb,
                btn.None);
            }

            public void AddLegalEntityAddressDetails(
                DateTime effectiveDate,
                int streetNumber,
                string streetname,
                string province,
                string suburb)
            {
                AddLegalEntityAddressDetails( 
                    "Residential", 
                    "Street",
                    effectiveDate, 
                    -1, 
                    -1, 
                    null,
                    streetNumber,
                    streetname, 
                    null,
                    province,
                    suburb,
                    btn.Add);
            }

            public enum btn
            {
                Add = 1,
                Cancel = 2,
                None = 3
            }

            public void ClickButton(btn buttonLabel)
            {
                switch (buttonLabel)
                {
                    case btn.Add:
                        base.ctl00MainbtnSubmitButton.Click();
                        break;
                    case btn.Cancel:
                        base.ctl00MainbtnCancelButton.Click();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
