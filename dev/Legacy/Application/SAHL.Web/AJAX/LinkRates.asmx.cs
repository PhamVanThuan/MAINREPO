using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using AjaxControlToolkit;
using System.Collections.Generic;
using System.Collections.Specialized;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for LinkRates
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class LinkRates : System.Web.Services.WebService
    {
        /// <summary>
        /// Used to discover if the webservice is responding to requests.
        /// </summary>
        [WebMethod]
        public bool Ping()
        {
            return true;
        }

        /// <summary>
        /// Gets a list of link rates applicable to an originationsource.
        /// </summary>
        /// <param name="knownCategoryValues">The OriginationSourceKey for the Link Rates required.</param>
        /// <param name="category">parameter not used in this query - required by ajax toolkit.</param>
        /// <returns>A list of drop down values that will be returned for the dependent drop down list.</returns>
        /// <remarks>This method is for use with the <see cref="AjaxToolKit.CascadingDropDown"/> extender control only.</remarks>
        [WebMethod]
        [ScriptMethod]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Parameter required by the AjaxToolKit even if it is not used")]
        public CascadingDropDownNameValue[] GetLinkRatesByOriginationSource(string knownCategoryValues, string category)
        {
            // This method will return a StringDictionary containing the name/value pairs of the currently selected values
            StringDictionary dictValues = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            // get the value that is selected in the dropdownlist 
            // this should only ever receive one value
            int originationSourceKey = -1;
            foreach (DictionaryEntry de in dictValues)
            {
                if (de.Value.ToString() == SAHLDropDownList.PleaseSelectValue)
                    break;

                originationSourceKey = Convert.ToInt32(de.Value);
                break;
            }

            // get the link rates for the selected originationsource
            ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
            Dictionary<int, string> LinkRateList = commonRepo.GetLinkRatesByOriginationSource(originationSourceKey);

            // load the link rates into the list
            List<CascadingDropDownNameValue> lstRates = new List<CascadingDropDownNameValue>();
            foreach (KeyValuePair<int, string> kpv in LinkRateList)
            {
                lstRates.Add(new CascadingDropDownNameValue(kpv.Value.ToString(), kpv.Key.ToString()));
            }

            // copy into an array
            CascadingDropDownNameValue[] result = new CascadingDropDownNameValue[lstRates.Count];
            lstRates.CopyTo(result);

            // return the result
            return result;
        }
    }
}
