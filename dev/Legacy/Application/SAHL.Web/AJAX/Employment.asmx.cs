using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using AjaxControlToolkit;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using System.Collections.ObjectModel;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for Employment
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Employment : System.Web.Services.WebService
    {

        /// <summary>
        /// Gets a list of employers whose name starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix">The starting letters of the employer's name.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetEmployers(string prefix)
        {
            IEmploymentRepository repository = RepositoryFactory.GetRepository<IEmploymentRepository>();
            IReadOnlyEventList<IEmployer> employers = repository.GetEmployersByPrefix(prefix, 10);
            SAHLAutoCompleteItem[] items = new SAHLAutoCompleteItem[employers.Count];

            for (int i = 0; i < items.Length; i++)
            {
                IEmployer e = employers[i];
                items[i] = new SAHLAutoCompleteItem(e.Key.ToString(), e.Name);
            }
            return items;
        }

          /// <summary>
        /// Gets a list of remuneration types applicable to an employment type.
        /// </summary>
        /// <param name="knownCategoryValues">A string the employment.</param>
        /// <param name="category">The category - currently not used in this query.</param>
        /// <returns>A list of drop down values that will be returned for the dependent drop down list.</returns>
        /// <remarks>This method is for use with the <see cref="AjaxToolKit.CascadingDropDown"/> extender control only.</remarks>
        [WebMethod]
        [ScriptMethod]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Parameter required by the AjaxToolKit even if it is not used")]
        public CascadingDropDownNameValue[] GetRemunerationTypesByEmploymentType(string knownCategoryValues, string category)
        {
            List<CascadingDropDownNameValue> lstOptions = new List<CascadingDropDownNameValue>();
            StringDictionary dictValues = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

            Type enumRemunTypes = null;

            // this should only ever receive one value
            foreach (DictionaryEntry de in dictValues)
            {
                if (de.Value.ToString() == SAHLDropDownList.PleaseSelectValue)
                    break;

                EmploymentTypes empTypes = (EmploymentTypes)Convert.ToInt32(de.Value);
                switch (empTypes)
                {
                    case EmploymentTypes.Salaried:
                        enumRemunTypes = typeof(EmploymentSalariedRemunerationTypes);
                        break;
                    case EmploymentTypes.SelfEmployed:
                        enumRemunTypes = typeof(EmploymentSelfEmployedRemunerationTypes);
                        break;
                    case EmploymentTypes.SalariedwithDeduction:
                        enumRemunTypes = typeof(EmploymentSubsidisedRemunerationTypes);
                        break;
                    case EmploymentTypes.Unemployed:
                        enumRemunTypes = typeof(EmploymentUnemployedRemunerationTypes);
                        break;
                }

                // add all the options
                if (enumRemunTypes != null)
                {
                    foreach (string val in Enum.GetNames(enumRemunTypes))
                    {
                        IRemunerationType remunType = lookups.RemunerationTypes.ObjectDictionary[Convert.ToInt32(Enum.Parse(enumRemunTypes, val)).ToString()];
                        lstOptions.Add(new CascadingDropDownNameValue(remunType.Description, remunType.Key.ToString()));
                    }
                }

                // only one value sent - exit out immediately
                break;
            }

            // sort the options
            lstOptions.Sort(delegate(CascadingDropDownNameValue c1, CascadingDropDownNameValue c2)
              {
                  return c1.name.CompareTo(c2.name);
              });

            // copy into an array
            CascadingDropDownNameValue[] result = new CascadingDropDownNameValue[lstOptions.Count];
            lstOptions.CopyTo(result);
            return result;
        }

        /// <summary>
        /// Gets a list of subsidy providers whose name starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix">The starting letters of the subsidy providers's description.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetSubsidyProviders(string prefix)
        {
            IEmploymentRepository repository = RepositoryFactory.GetRepository<IEmploymentRepository>();
            ReadOnlyCollection<ISubsidyProvider> providers = repository.GetSubsidyProvidersByPrefix(prefix, 10);
            SAHLAutoCompleteItem[] items = new SAHLAutoCompleteItem[providers.Count];

            for (int i = 0; i < items.Length; i++)
            {
                ISubsidyProvider p = providers[i];
                var displayName = p.GEPFAffiliate ? string.Format("{0} (GEPF)", p.LegalEntity.DisplayName) : p.LegalEntity.DisplayName;
                items[i] = new SAHLAutoCompleteItem(p.Key.ToString(), displayName);
            }
            return items;
        }

        [WebMethod]
        public string Test(ComplexObject co)
        {
            return co.Name + co.Description;
        }


    }

    public class ComplexObject 
    {

        public string Name;
        public string Description;
    }

}
