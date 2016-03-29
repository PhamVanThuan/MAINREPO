using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using System;

namespace BuildingBlocks.Services
{
    public class PropertyService : _2AMDataHelper, IPropertyService
    {
        private readonly ICommonService commonService;
        private readonly IAddressService addressService;

        public PropertyService(ICommonService commonService, IAddressService addressService)
        {
            this.commonService = commonService;
            this.addressService = addressService;
        }

        /// <summary>
        /// Retrieves the property detail for the property linked to an Offer
        /// </summary>
        /// <param name="offerkey"></param>
        /// <returns></returns>
        public int GetPropertyKeyByOfferKey(int offerkey)
        {
            return base.GetProperty(offerkey: offerkey).PropertyKey;
        }

        public Automation.DataModels.Property GetPropertyByPropertyKey(int propertykey)
        {
            var property =  base.GetProperty(propertyKey: propertykey);
            property.PropertyAddress = addressService.GetAddress(addresskey: property.AddressKey);
            return property;
        }

        /// <summary>
        /// Get a formatted address for the property.
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public Automation.DataModels.Property GetFormattedPropertyAddressByAccountKey(int accountkey)
        {
            return base.GetProperty(accountkey: accountkey);
        }

        /// <summary>
        /// Returns a formatted Property Address
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public string GetLegalEntityPropertyAddress(int accountkey)
        {
            string propAddress = String.Empty;
            var property = GetFormattedPropertyAddressByAccountKey(accountkey);

            var propertydescription = property.PropertyDescription3;
            var propertyaddress = property.FormattedPropertyAddress;

            propAddress = String.Format("{0} {1}", propertydescription, propertyaddress);
            return propAddress;
        }

        /// <summary>
        /// Retrieves the property detail for the property linked to an Account
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public Automation.DataModels.Property GetPropertyByAccountKey(int accountKey)
        {
            return base.GetProperty(accountkey: accountKey);
        }

        public void DBUpdateDeedsOfficeDetails(int offerKey)
        {
            base.UpdateDeedsOfficeDetails(offerKey);
        }

        /// <summary>
        /// Provides an empty property object based on the PropertyModel class
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Property GetEmptyProperty()
        {
            var property = new Automation.DataModels.Property();

            var pleaseSelectVal = "- Please Select -";
            property.DeedsPropertyTypeDescription = pleaseSelectVal;
            property.PropertyTypeDescription = pleaseSelectVal;
            property.TitleTypeDescription = pleaseSelectVal;
            property.AreaClassificationDescription = pleaseSelectVal;
            property.OccupancyTypeDescription = pleaseSelectVal;
            property.DeedsOfficeName = pleaseSelectVal;

            property.PropertyDescription1 = string.Empty;
            property.PropertyDescription2 = string.Empty;
            property.PropertyDescription3 = string.Empty;

            property.ErfNumber = string.Empty;
            property.ErfPortionNumber = string.Empty;
            property.ErfSuburbDescription = string.Empty;
            property.ErfMetroDescription = string.Empty;
            property.SectionalSchemeName = string.Empty;
            property.SectionalUnitNumber = string.Empty;

            property.TitleDeedNumber = string.Empty;
            property.BondAccountNumber = string.Empty;

            return property;
        }

        public Automation.DataModels.Property GetChangedProperty(Automation.DataModels.Property currentProperty, bool changeDeedsDetails = false)
        {
            currentProperty.DeedsOfficeName = "Umtata";

            var randomNumber = new Random().Next(Int32.MaxValue);

            currentProperty.DeedsPropertyTypeDescription = commonService.GetRandomTypeDescription((int)currentProperty.DeedsPropertyTypeKey,
                typeof(DeedsPropertyTypeEnum), typeof(DeedsPropertyType));
            currentProperty.OccupancyTypeDescription = commonService.GetRandomTypeDescription((int)currentProperty.OccupancyTypeKey, typeof(OccupancyTypeEnum),
                typeof(OccupancyType));
            currentProperty.AreaClassificationDescription = commonService.GetRandomTypeDescription((int)currentProperty.AreaClassificationKey,
                typeof(AreaClassificationEnum), typeof(AreaClassification));
            currentProperty.PropertyTypeDescription = commonService.GetRandomTypeDescription((int)currentProperty.PropertyTypeKey,
                typeof(PropertyTypeEnum), typeof(PropertyType));
            currentProperty.TitleTypeDescription = commonService.GetRandomTypeDescription((int)currentProperty.TitleTypeKey, typeof(TitleTypeEnum),
                typeof(TitleType));

            currentProperty.PropertyDescription1 = String.Format("Update{0}", randomNumber);
            currentProperty.PropertyDescription2 = String.Format("Update{0}", randomNumber);
            currentProperty.PropertyDescription3 = String.Format("Update{0}", randomNumber);

            if (changeDeedsDetails)
            {
                currentProperty.BondAccountNumber = String.Format("Update{0}", randomNumber);
                currentProperty.DeedsOfficeName = commonService.GetRandomTypeDescription(currentProperty.DeedsOfficeName,
                    typeof(Common.Constants.DeedsOffice));
                currentProperty.TitleDeedNumber = String.Format("Update{0}", randomNumber);
                currentProperty.ErfNumber = String.Format("Update{0}", randomNumber);
                currentProperty.ErfPortionNumber = String.Format("Update{0}", randomNumber);
                currentProperty.ErfSuburbDescription = String.Format("Update{0}", randomNumber);
                currentProperty.ErfMetroDescription = String.Format("Update{0}", randomNumber);
                currentProperty.SectionalSchemeName = String.Format("Update{0}", randomNumber);
                currentProperty.SectionalUnitNumber = String.Format("Update{0}", randomNumber);
            }

            return currentProperty;
        }
    }
}