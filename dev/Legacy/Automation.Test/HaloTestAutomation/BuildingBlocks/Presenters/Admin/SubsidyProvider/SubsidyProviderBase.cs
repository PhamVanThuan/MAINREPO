using BuildingBlocks.Services.Contracts;
using BuildingBlocks.Timers;
using NUnit.Framework;

using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Admin
{
    public abstract class SubsidyProviderBase : ObjectMaps.Pages.SubsidyProviderDetailsControls
    {
        private readonly IWatiNService watinService;

        public SubsidyProviderBase()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public virtual void ClearContactPerson(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.ContactPersonEdit.TypeText(subsidyProvider.ContactPerson);
        }

        public virtual void ClearContactNumber(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.PhoneCode.TypeText(subsidyProvider.LegalEntity.WorkPhoneCode);
            base.PhoneNumber.TypeText(subsidyProvider.LegalEntity.WorkPhoneNumber);
        }

        public virtual void EnterWrongEmail(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.EmailAddressEdit.TypeText("test.com");
        }

        public virtual void PopulateWithoutAddress(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.ContactPersonEdit.TypeText(subsidyProvider.ContactPerson);
            base.EmailAddressEdit.TypeText(subsidyProvider.LegalEntity.EmailAddress);
            base.PhoneCode.TypeText(subsidyProvider.LegalEntity.WorkPhoneCode);
            base.PhoneNumber.TypeText(subsidyProvider.LegalEntity.WorkPhoneNumber);
            base.txtBoxNumber.Clear();
            base.ctl00MainaddressDetailstxtPostOffice.Clear();
        }

        public virtual void PopulateWithoutAddressBoxNumber(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.ContactPersonEdit.TypeText(subsidyProvider.ContactPerson);
            base.EmailAddressEdit.TypeText(subsidyProvider.LegalEntity.EmailAddress);
            base.PhoneCode.TypeText(subsidyProvider.LegalEntity.WorkPhoneCode);
            base.PhoneNumber.TypeText(subsidyProvider.LegalEntity.WorkPhoneNumber);
            base.txtBoxNumber.Clear();
        }

        public virtual void PopulateWithoutAddressPostOffice(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.ContactPersonEdit.TypeText(subsidyProvider.ContactPerson);
            base.EmailAddressEdit.TypeText(subsidyProvider.LegalEntity.EmailAddress);
            base.PhoneCode.TypeText(subsidyProvider.LegalEntity.WorkPhoneCode);
            base.PhoneNumber.TypeText(subsidyProvider.LegalEntity.WorkPhoneNumber);
            base.ctl00MainaddressDetailstxtPostOffice.Clear();
        }

        public virtual void Populate(Automation.DataModels.SubsidyProvider subsidyProvider)
        {           
            if (subsidyProvider.LegalEntity.LegalEntityAddress != null)
            {
                var addressFormat = ((int)subsidyProvider.LegalEntity.LegalEntityAddress.Address.AddressFormatKey).ToString();
                if (addressFormat == "0")
                    addressFormat = "-select-";
                base.ddlAddressFormat.SelectByValue(addressFormat);
                var address = subsidyProvider.LegalEntity.LegalEntityAddress.Address;
                switch (subsidyProvider.LegalEntity.LegalEntityAddress.Address.AddressFormatKey)
                {
                    case Common.Enums.AddressFormatEnum.Street:
                        base.ddlAddressType.Option(subsidyProvider.SubsidyProviderTypeDescription).Select();
                        base.ddlAddressFormat.Option(address.AddressFormatDescription).Select();
                        base.txtUnitNumber.TypeText(address.UnitNumber);
                        base.txtBuildingNumber.TypeText(address.BuildingNumber);
                        base.txtBuildingName.TypeText(address.BuildingName);
                        base.txtStreetNumber.TypeText(address.StreetNumber);
                        base.txtStreetName.TypeText(address.StreetName);
                        base.ddlCountry.Option(address.RRR_CountryDescription).Select();
                        base.txtSuburb.TypeText(address.RRR_SuburbDescription);
                        SelectAddressIfExist(address.RRR_SuburbDescription);
                        break;

                    case Common.Enums.AddressFormatEnum.Box:
                        base.txtBoxNumber.TypeText(address.BoxNumber);
                        base.ctl00MainaddressDetailstxtPostOffice.TypeText(address.RRR_SuburbDescription);
                        SelectAddressIfExist(address.RRR_SuburbDescription);
                        break;

                    case Common.Enums.AddressFormatEnum.ClusterBox:
                        base.ClusterBoxNumber.TypeText(address.BoxNumber);
                        base.ctl00MainaddressDetailstxtPostOffice.TypeText(address.RRR_SuburbDescription);
                        SelectAddressIfExist(address.RRR_SuburbDescription);
                        break;

                    case Common.Enums.AddressFormatEnum.PostNetSuite:
                        base.PostNetSuiteNumber.TypeText(address.PostnetSuiteNumber);
                        base.PrivateBag.TypeText(address.PrivateBag);
                        base.ctl00MainaddressDetailstxtPostOffice.TypeText(address.RRR_SuburbDescription);
                        SelectAddressIfExist(address.RRR_SuburbDescription);
                        break;

                    case Common.Enums.AddressFormatEnum.PrivateBag:
                        base.PrivateBag.TypeText(address.PrivateBag);
                        base.ctl00MainaddressDetailstxtPostOffice.TypeText(address.RRR_SuburbDescription);
                        SelectAddressIfExist(address.RRR_SuburbDescription);
                        break;

                    case Common.Enums.AddressFormatEnum.None:
                        break;
                }
            }

            base.ContactPersonEdit.TypeText(subsidyProvider.ContactPerson);
            base.EmailAddressEdit.TypeText(subsidyProvider.LegalEntity.EmailAddress);
            base.PhoneCode.TypeText(subsidyProvider.LegalEntity.WorkPhoneCode);
            base.PhoneNumber.TypeText(subsidyProvider.LegalEntity.WorkPhoneNumber);
        }

        public void Submit()
        {
            base.SubmitButton.Click();
        }

        public void Cancel()
        {
            base.CancelButton.Click();
        }

        public void AssertView(string viewName)
        {
            Assert.AreEqual(viewName, base.ViewName.Text, "Expected {0}, but was {1}", viewName, base.ViewName.Text);
        }

        public void AssertSubsidyProviderContactPersonMessageExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Contact Person is a mandatory field"));
        }

        public void AssertSubsidyProviderContactNumberMessageExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains
                ("At least one contact number must be provided. Note that for non-cellphone numbers both a dialing code and number must be provided"));
        }

        public void AssertSubsidyProviderMandatoryMessageExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Legal Entity Company Name Required"));
        }

        public void AssertSubsidyProviderTypeMandatoryMessageExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Subsidy Provider Type is a mandatory field"));
        }

        public void AssertEmailIncorrectFormatMessageExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Please enter email address in correct format"));
        }

        public void AssertUpdateAddressBoxNumberRequiredMessageExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Box Number is a mandatory field"));
        }

        public void AssertUpdateAddressPostBoxRequiredMessageExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Post Office is a mandatory field"));
        }

        public void AssertAddAddressRequiredMessageExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("An address must be provided with a Subsidy Provider"));
        }

        public void AssertSubsidyProviderAlreadyExist()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("This Subsidy Provider already exists"));
        }

        #region Helpers

        private void SelectAddressIfExist(string suburbDescription)
        {
            GeneralTimer.Wait(2000);
            var autoCompleteDev = base.SAHLAutoComplete_DefaultItem_Collection().FirstOrDefault();
            if (autoCompleteDev != null)
            {
                autoCompleteDev.MouseDown();
                GeneralTimer.Wait(2000);
                if (base.addressSearchAjax.Exists)
                {
                    var addressLink = base.addressSearchAjax.Link(Find.ByText(new Regex(suburbDescription)));
                    if (addressLink.Exists)
                    {
                        watinService.HandleConfirmationPopup(addressLink);
                    }
                }
            }
        }

        #endregion Helpers
    }
}