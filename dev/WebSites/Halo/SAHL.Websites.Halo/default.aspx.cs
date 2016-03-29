using System;
using System.Net;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.Helpers;
using System.Web.Configuration;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using SAHL.Websites.Halo.Shared;
using SAHL.Services.Interfaces.UserProfile.Models;

namespace SAHL.Websites.Halo
{
    public class _default : Page
    {

        public bool IsDebug
        {
            get
            {
#if (DEBUG)
                return true;
#else
                return false;
#endif
            }
        }



        public GetUserDetailsForUserQueryResult UserInfo { get; private set; }
        public IEnumerable<string> UserRoles { get; private set; }
        public Exception Exception { get; set; }
        public string UserCapabilities { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.Objects,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            try
            {
                var unitOfWorkExecutor = new UnitOfWorkExecutor(Page.User);

                if (!unitOfWorkExecutor.Execute<IUnitOfWorkAction>())
                {
                    return;
                }
            }
            catch (Exception exception)
            {
                Exception = exception;
            }

            this.UserInfo = UserDetailsStartupAction.UserInfo;
            this.UserRoles = UserDetailsStartupAction.UserRoles;
            this.UserCapabilities = UserDetailsStartupAction.Capabilities != null && UserDetailsStartupAction.Capabilities.Count() > 0 ? 
                (UserDetailsStartupAction.Capabilities.Count() > 1 ?
                UserDetailsStartupAction.Capabilities.Aggregate((c, n) => c + "," + n) : UserDetailsStartupAction.Capabilities.First())
                : "";
        }

        protected string MakeSafe(string unsafeString)
        {
            return unsafeString.Replace("\\", "\\\\");
        }
    }
}
