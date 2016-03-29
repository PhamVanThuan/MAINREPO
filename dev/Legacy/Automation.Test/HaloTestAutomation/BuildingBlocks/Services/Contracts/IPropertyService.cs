using Automation.DataAccess;
using System.Collections;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IPropertyService
    {
        int GetPropertyKeyByOfferKey(int offerkey);

        bool UpdateOfferMortgageLoanPropertyKey(int propertyKey, int offerKey);

        QueryResults GetAddressDetailsForPropertyValuation(int marketValue, bool greaterThan12Months, bool equalTo);

        QueryResults GetPropertyByOfferKey(int OfferKey);

        QueryResults GetAddressDetailsForPropertyValuationLessThan12MonthsOld(int marketValue);

        QueryResults GetAddressDetailsForPropertyValuationGreaterThan12MonthsOld(int marketValue);

        QueryResults GetAddressDetailsForPropertyValuation12MonthsOld(int marketValue);

        QueryResults GetLatestPropertyValuationData(int OfferKey);

        Automation.DataModels.Property GetFormattedPropertyAddressByAccountKey(int accountkey);

        string GetLegalEntityPropertyAddress(int accountkey);

        Automation.DataModels.Property GetPropertyByPropertyKey(int propertykey);

        Automation.DataModels.Property GetPropertyByAccountKey(int accountKey);

        QueryResults GetProperty(string propertyKey);

        Automation.DataModels.Property GetProperty(int propertyKey = 0, int accountkey = 0, int offerkey = 0);

        void DBUpdateDeedsOfficeDetails(int offerKey);

        Automation.DataModels.Property InsertProperty(Automation.DataModels.Property property);

        Automation.DataModels.Property UpdatePropertyAddress(int propertyKey, int addressKey);

        Automation.DataModels.Property GetEmptyProperty();

        Automation.DataModels.Property GetChangedProperty(Automation.DataModels.Property currentProperty, bool changeDeedsDetails = false);

        IEnumerable<Automation.DataModels.Property> GetMortgageLoanPropertiesForLegalEntity(int legalEntityKey);
    }
}