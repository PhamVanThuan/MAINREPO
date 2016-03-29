using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using AjaxControlToolkit;
using System.Collections.Specialized;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Provides AJAX methods for retrieving Address information.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Address : System.Web.Services.WebService
    {

        /// <summary>
        /// Gets a list of provinces for a specified country.  This method is for use with the 
        /// <see cref="AjaxControlToolkit.CascadingDropDown"/> extender control only.
        /// </summary>
        /// <param name="knownCategoryValues">A string containing the list of countries - this is set by the AjaxToolKit extender.</param>
        /// <param name="category">The category - currently not used in this query.</param>
        /// <returns>A list of drop down values that will be returned for the dependent drop down list.</returns>
        [WebMethod]
        [ScriptMethod]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification="Parameter required by the AjaxToolKit even if it is not used")]
        public CascadingDropDownNameValue[] GetProvincesByCountry(string knownCategoryValues, string category) 
        {
            List<CascadingDropDownNameValue> lstOptions = new List<CascadingDropDownNameValue>();
            StringDictionary dictValues = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            foreach (DictionaryEntry de in dictValues) 
            {
                int countryKey = 0;
                if (!Int32.TryParse(de.Value.ToString(), out countryKey))
                    continue;

                ILookupRepository _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
                IDictionary<int, string> provinces = _lookups.ProvincesByCountry(countryKey);
                foreach (KeyValuePair<int, string> kvp in provinces) 
                {
                    lstOptions.Add(new CascadingDropDownNameValue(kvp.Value, kvp.Key.ToString()));
                }
            }

            // copy into an array
            CascadingDropDownNameValue[] result = new CascadingDropDownNameValue[lstOptions.Count];
            lstOptions.CopyTo(result);
            return result;
        }

        /// <summary>
        /// Gets additional details of a suburb.
        /// </summary>
        /// <param name="suburbKey">The suburb to get the information for.</param>
        /// <returns>An array containing the following information: [0] - Suburb.City.Description, [1] - Suburb.PostalCode.</returns>
        [WebMethod]
        [ScriptMethod]
        public string[] GetSuburbDetails(int suburbKey)
        {
            string[] result = new string[2];
            IAddressRepository _repository = RepositoryFactory.GetRepository<IAddressRepository>();
            ISuburb suburb = _repository.GetSuburbByKey(suburbKey);
            if (suburb != null)
            {
                result[0] = (suburb.City != null ? suburb.City.Description : "");
                result[1] = (suburb.PostalCode != null ? suburb.PostalCode : "");
            }
            return result;
        }

        /// <summary>
        /// Gets a list of post offices starting with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetPostOffices(string prefix)
        {
            IAddressRepository repository = RepositoryFactory.GetRepository<IAddressRepository>();
            IReadOnlyEventList<IPostOffice> postOffices = repository.GetPostOfficesByPrefix(prefix, 10);
            SAHLAutoCompleteItem[] items = new SAHLAutoCompleteItem[postOffices.Count];

            for (int i = 0; i < items.Length; i++)
            {
                IPostOffice po = postOffices[i];
                string itemText = po.Description;
                if (po.PostalCode != null && po.PostalCode.Length > 0)
                    itemText = itemText + " (" + po.PostalCode + ")";
                items[i] = new SAHLAutoCompleteItem(po.Key.ToString(), itemText);
            }
            return items;
        }

        /// <summary>
        /// Gets a list of suburbs for a specified province. 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="provinceKey"></param>
        /// <returns>A list of drop down values that will be returned for the dependent drop down list.</returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetSuburbsByProvince(string prefix, string provinceKey) 
        {
            int key = 0;
            if (Int32.TryParse(provinceKey, out key))
            {
                IAddressRepository addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();
                IProvince province = addressRepository.GetProvinceByKey(Int32.Parse(provinceKey));
                IReadOnlyEventList<ISuburb> suburbs = province.SuburbsByPrefix(prefix, 10);
                SAHLAutoCompleteItem[] items = new SAHLAutoCompleteItem[suburbs.Count];
                for (int i = 0; i < items.Length; i++)
                {
                    string displayText = suburbs[i].Description;
                    string itemText = displayText;
                    if (suburbs[i].City != null)
                        itemText = itemText + ", " + suburbs[i].City.Description;
                    if (suburbs[i].PostalCode != null && suburbs[i].PostalCode.Length > 0)
                        itemText = itemText + ", " + suburbs[i].PostalCode;

                    items[i] = new SAHLAutoCompleteItem(suburbs[i].Key.ToString(), itemText, displayText);
                }
                return items;
            }
            else
            {
                return new SAHLAutoCompleteItem[0];
            }
        }

        [WebMethod]
        [ScriptMethod]
        public AjaxItem[] SearchAddresses(string addressFormat, string[] inputValues)
        {
            // convert the input values into a dictionary
            Dictionary<string, string> dict = new Dictionary<string, string>();
            char[] delim = { '=' };
            foreach (string inputValue in inputValues)
            {

                string[] split = inputValue.Split(delim);
                if (!dict.ContainsKey(split[0])) dict.Add(split[0], split[1].Trim());
            }

            // create search criteria, depending on the addressFormat
            AddressFormats addrFormat = (AddressFormats)Int32.Parse(addressFormat);
            IAddressSearchCriteria searchCriteria = new AddressSearchCriteria();
            searchCriteria.AddressFormat = addrFormat;

            switch (addrFormat)
            {
                case AddressFormats.Box:
                    searchCriteria.BoxNumber = dict["BoxNumber"];
                    if (dict["PostOfficeKey"].Length > 0)
                        searchCriteria.PostOfficeKey = Int32.Parse(dict["PostOfficeKey"]);
                    break;
                case AddressFormats.ClusterBox:
                    searchCriteria.ClusterBoxNumber = dict["ClusterBoxNumber"];
                    if (dict["PostOfficeKey"].Length > 0)
                        searchCriteria.PostOfficeKey = Int32.Parse(dict["PostOfficeKey"]);
                    break;
                case AddressFormats.FreeText:
                    searchCriteria.FreeTextLine1 = dict["FreeTextLine1"];
                    searchCriteria.FreeTextLine2 = dict["FreeTextLine2"];
                    searchCriteria.FreeTextLine3 = dict["FreeTextLine3"];
                    searchCriteria.FreeTextLine4 = dict["FreeTextLine4"];
                    searchCriteria.FreeTextLine5 = dict["FreeTextLine5"];
                    if (dict["Country"] != SAHLDropDownList.PleaseSelectText)
                        searchCriteria.Country = dict["Country"];
                    break;
                case AddressFormats.PostNetSuite:
                    searchCriteria.PostnetSuiteNumber = dict["PostnetSuiteNumber"];
                    searchCriteria.PrivateBagNumber = dict["PrivateBagNumber"];
                    if (dict["PostOfficeKey"].Length > 0)
                        searchCriteria.PostOfficeKey = Int32.Parse(dict["PostOfficeKey"]);
                    break;
                case AddressFormats.PrivateBag:
                    searchCriteria.PrivateBagNumber = dict["PrivateBagNumber"];
                    if (dict["PostOfficeKey"].Length > 0)
                        searchCriteria.PostOfficeKey = Int32.Parse(dict["PostOfficeKey"]);
                    break;
                case AddressFormats.Street:
                    searchCriteria.BuildingNumber = dict["BuildingNumber"];
                    searchCriteria.BuildingName = dict["BuildingName"];
                    if (dict["Country"] != SAHLDropDownList.PleaseSelectText)
                        searchCriteria.Country = dict["Country"];
                    if (dict["Province"] != SAHLDropDownList.PleaseSelectText)
                        searchCriteria.Province = dict["Province"];
                    searchCriteria.StreetNumber = dict["StreetNumber"];
                    searchCriteria.StreetName = dict["StreetName"];
                    if (dict["SuburbKey"].Length > 0)
                        searchCriteria.SuburbKey = Int32.Parse(dict["SuburbKey"]);
                    searchCriteria.UnitNumber = dict["UnitNumber"];
                    break;
                default:
                    throw new NotImplementedException();
            }

            // run the search
            IAddressRepository repository = RepositoryFactory.GetRepository<IAddressRepository>();
            IEventList<IAddress> addresses = repository.SearchAddresses(searchCriteria, 10);

            // extract the readable description
            AjaxItem[] results = new AjaxItem[addresses.Count];
            for (int i=0; i<addresses.Count; i++)
            {
                results[i] = new AjaxItem(addresses[i].GetFormattedDescription(AddressDelimiters.Comma), addresses[i].Key.ToString());
            }
            return results;
        }
    }
}
