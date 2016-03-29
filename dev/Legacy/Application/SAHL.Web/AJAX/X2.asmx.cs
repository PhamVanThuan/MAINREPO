using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using AjaxControlToolkit;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Specialized;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Provides web service methods for X2 data.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class X2 : System.Web.Services.WebService
    {

        /// <summary>
        /// Gets a list of workflow states for a supplied workflow.  These will be sorted by name.
        /// </summary>
        /// <param name="knownCategoryValues">The id of the workflows to get the information for.</param>
		/// <param name="category">StateType e.g. User or Archive - used to filter the list of states by type</param>
        /// <returns>A list of states.</returns>
        [WebMethod]
        [ScriptMethod]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Parameter required by the AjaxToolKit even if it is not used")]
        public CascadingDropDownNameValue[] GetWorkFlowStatesByWorkFlowKey(string knownCategoryValues, string category) 
        {

            List<CascadingDropDownNameValue> lstOptions = new List<CascadingDropDownNameValue>();
            StringDictionary dictValues = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

            foreach (DictionaryEntry de in dictValues)
            {
                int workflowKey = 0;
                if (!Int32.TryParse(de.Value.ToString(), out workflowKey))
                    continue;

                IX2Repository repo = RepositoryFactory.GetRepository<IX2Repository>();
                IWorkFlow workFlow = repo.GetWorkFlowByKey(workflowKey);

                // move to a list and sort
                List<IState> states = new List<IState>();
				foreach (IState state in workFlow.States)
				{
					if (state.StateType.Name.ToLower() == category.ToLower() || string.IsNullOrEmpty(category))
				    {
						states.Add(state);
					}
				}

                states.Sort(
                  delegate(IState s1, IState s2)
                  {
                      return s1.Name.CompareTo(s2.Name);
                  });

                foreach (IState state in states)
					lstOptions.Add(new CascadingDropDownNameValue(state.Name, state.ID.ToString()));
            }

            // copy into an array
            CascadingDropDownNameValue[] result = new CascadingDropDownNameValue[lstOptions.Count];
            lstOptions.CopyTo(result);
            return result;
        }
    }
}
