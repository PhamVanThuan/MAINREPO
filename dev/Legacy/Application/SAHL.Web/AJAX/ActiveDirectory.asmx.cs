using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ComponentModel;
using System.Web.Script.Services;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.DataAccess;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.AJAX
{
    /// <summary>
    /// Provides web methods pertaining to Active Directory.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ActiveDirectory : System.Web.Services.WebService
    {
        IActiveDirectoryRepository activeDirectoryRepo;

        /// <summary>
        /// Used to discover if the webservice is responding to requests.
        /// </summary>
        [WebMethod]
        public bool Ping()
        {
            return true;
        }
        /// <summary>
        /// Gets a list of active directory users where the userid starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix">The starting letters of the active directory userid.</param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public SAHLAutoCompleteItem[] GetActiveDirectoryUsersByName(string prefix)
        {
            List<SAHLAutoCompleteItem> items = new List<SAHLAutoCompleteItem>();

            // we need to search AD and return a list of users
            if (activeDirectoryRepo==null)
                activeDirectoryRepo = RepositoryFactory.GetRepository<IActiveDirectoryRepository>();

            IList<ActiveDirectoryUserBindableObject> users = activeDirectoryRepo.GetActiveDirectoryUsers(prefix);
            foreach (ActiveDirectoryUserBindableObject user in users)
            {
                string value = user.ADUserName;
                string text = String.Format("{0} {1} {2}", user.FirstName, user.Surname, "(" + user.ADUserName + ")");
                string displayText = String.Format("{0} {1}", user.FirstName, user.Surname);
                items.Add(new SAHLAutoCompleteItem(value, text, displayText));
            }

            return items.ToArray();
        }
    }
}
