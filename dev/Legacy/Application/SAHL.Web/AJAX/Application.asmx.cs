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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Provides web services for HALO dealing with Application requests.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class Application : System.Web.Services.WebService
    {

        /// <summary>
        /// Gets a list of application keys matching <c>prefix</c>.  This does a LIKE% comparison, 
        /// so is limited to 10 records.
        /// </summary>
        /// <param name="match">A portion of the application key.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetApplicationKeys(string match)
        {
            IApplicationRepository repository = RepositoryFactory.GetRepository<IApplicationRepository>();
            IList<int> items = repository.GetApplicationKeys(match, 10);
            SAHLAutoCompleteItem[] results = new SAHLAutoCompleteItem[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                string key = items[i].ToString();
                results[i] = new SAHLAutoCompleteItem(key, key);
            }
            return results;
        }
    }
}
