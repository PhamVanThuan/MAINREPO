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

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Summary description for AdUser
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class AdUser : System.Web.Services.WebService
    {

        /// <summary>
        /// Gets a list of ad user names matching <c>prefix</c>.  This does a LIKE comparison, so is limited to 10 records.
        /// </summary>
        /// <param name="match">A portion of the user's login name.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetAdUsers(string match)
        {
            IOrganisationStructureRepository repository = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IEventList<IADUser> users = repository.GetAdUsersByPartialName(match, 10);
            SAHLAutoCompleteItem[] items = new SAHLAutoCompleteItem[users.Count];

            for (int i = 0; i < items.Length; i++)
            {
                IADUser u = users[i];
                items[i] = new SAHLAutoCompleteItem(u.Key.ToString(), u.ADUserName);
            }
            return items;
        }
    }
}
