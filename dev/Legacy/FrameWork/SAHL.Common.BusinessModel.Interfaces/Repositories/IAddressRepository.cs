using System;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IAddressRepository
    {
        // string fGetFormattedAddressDelimited(IAddress address, bool UseCarriageReturns);

        /// <summary>
        /// Determines whether an address already exists in the database.
        /// </summary>
        /// <param name="address">The address to check against.  If it is found, this value will be populated with the existing address.</param>
        bool AddressExists(ref IAddress address);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ILegalEntityAddress CreateEmptyLegalEntityAddress();

        /// <summary>
        /// Gets an address by a specified key.
        /// </summary>
        /// <param name="addressKey">The unique identifier for the address.</param>
        /// <returns>An <see cref="IAddress"/> object matching the supplied <c>addressKey</c>, or null if no address is found.</returns>
        IAddress GetAddressByKey(int addressKey);

        /// <summary>
        /// Gets an <see cref="IAddressFormat"/> by a specified key.
        /// </summary>
        /// <param name="key">The unique identifier for the IAddressFi=ormat.</param>
        /// <returns>An <see cref="IAddressFormat"/> object matching the supplied <c>key</c>.</returns>
        IAddressFormat GetAddressFormatByKey(AddressFormats key);

        /// <summary>
        /// Gets an address type by a specified key.
        /// </summary>
        /// <param name="key">The unique identifier for the address type.</param>
        /// <returns>An <see cref="IAddressType"/> object matching the supplied <c>key</c>, or null if no addresstype is found.</returns>
        IAddressType GetAddressTypeByKey(int key);

        /// <summary>
        /// Gets a new address object.
        /// </summary>
        /// <param name="type">The address type to return e.g. typeof(IAddressBox).</param>
        IAddress GetEmptyAddress(Type type);

        /// <summary>
        /// Returns the post office for a foreign country (not south africa) - for foreign countries
        /// there is always one country, one province, one city and once postoffice in the database.
        /// </summary>
        /// <param name="countryKey"></param>
        /// <returns></returns>
        IPostOffice GetForeignCountryPostOffice(int countryKey);

        /// <summary>
        /// Gets a legal entity address by the key.
        /// </summary>
        /// <param name="legalEntityAddressKey">The unique identifier for the legal entity address.</param>
        /// <returns>An <see cref="ILegalEntityAddress"/> object matching the supplied <c>legalEntityAddressKey</c>, or null if no record is found.</returns>
        ILegalEntityAddress GetLegalEntityAddressByKey(int legalEntityAddressKey);

        /// <summary>
        /// Gets a failed legal entity address by key.
        /// </summary>
        /// <param name="failedLegalEntityAddressKey"></param>
        /// <returns></returns>
        IFailedLegalEntityAddress GetFailedLegalEntityAddressByKey(int failedLegalEntityAddressKey);

        /// <summary>
        /// Gets a post office object by the key.
        /// </summary>
        /// <param name="postOfficeKey"></param>
        /// <returns></returns>
        IPostOffice GetPostOfficeByKey(int postOfficeKey);

        /// <summary>
        /// Gets a Province by the Province Key.
        /// </summary>
        /// <param name="provinceKey">The province key.</param>
        /// <returns>The <see cref="IProvince">province</see> found using the supplied key. Returns null if no province is found.</returns>
        IProvince GetProvinceByKey(int provinceKey);

        /// <summary>
        /// Gets a Suburb by the Suburb Key.
        /// </summary>
        /// <param name="suburbKey">The suburb key.</param>
        /// <returns>The <see cref="ISuburb">suburb </see> found using the supplied key. Returns null if no suburb is found.</returns>
        ISuburb GetSuburbByKey(int suburbKey);

        /// <summary>
        /// Gets a list of post offices according to a specified <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount">The maximum number of records to return.  Set to -1 for an unlimited result set.</param>
        /// <returns></returns>
        IReadOnlyEventList<IPostOffice> GetPostOfficesByPrefix(string prefix, int maxRowCount);

        /// <summary>
        /// USed to perform a search against addresses.
        /// </summary>
        /// <param name="searchCriteria">The search criteria used to perform the search.</param>
        /// <param name="maxRowCount">The maximum number of records to return.</param>
        /// <returns></returns>
        IEventList<IAddress> SearchAddresses(IAddressSearchCriteria searchCriteria, int maxRowCount);

        /// <summary>
        /// create empty mailind address
        /// </summary>
        /// <returns></returns>
        IMailingAddress CreateEmptyMailingAddress();

        /// <summary>
        /// save mailing address
        /// </summary>
        /// <param name="mailingAddress"></param>
        void SaveMailingAddress(IMailingAddress mailingAddress);

        /// <summary>
        /// Saves an address.  This will check to see if the address already exists in the database - if it
        /// does the address will not be saved and <c>address</c> will be updated to reflect the existing
        /// address in order to prevent duplicates in the database.
        /// </summary>
        /// <param name="address"></param>
        void SaveAddress(ref IAddress address);

        /// <summary>
        /// Get Empty application mailing address
        /// </summary>
        /// <returns></returns>
        IApplicationMailingAddress CreateEmptyApplicationMailingAddress();

        /// <summary>
        ///  Save application mailing address
        /// </summary>
        /// <param name="mailingAddress"></param>
        void SaveApplicationMailingAddress(IApplicationMailingAddress mailingAddress);

        /// <summary>
        /// Marks a dirty address as cleaned.  This will check to see if it contains a postal
        /// or street address, and mark the relevant column as cleaned.
        /// </summary>
        /// <param name="dirtyAddress"></param>
        void CleanDirtyAddress(IFailedLegalEntityAddress dirtyAddress);

        void SaveLegalEntityAddress(ILegalEntityAddress legalEntityAddress);
    }
}