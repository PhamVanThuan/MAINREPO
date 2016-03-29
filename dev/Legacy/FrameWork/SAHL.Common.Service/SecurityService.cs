using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Attributes;
using System.Security.Principal;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Authentication;
using SAHL.Common.Logging;
using System.Reflection;

namespace SAHL.Common.Service
{
  /// <summary>
  /// Concrete implementation of an <see href="ISecurityService"/>
  /// </summary>
    [FactoryType(typeof(ISecurityService))]
    public class SecurityService : ISecurityService
    {
        #region ISecurityService Members

        /// <summary>
        /// Starts Impersonation using the default user credentials in the control table
        /// </summary>
        /// <returns>WindowsImpersonationContext</returns>
        public WindowsImpersonationContext BeginImpersonation()
        {
            return BeginImpersonation(null, null, null);
        }

        /// <summary>
        /// Starts Impersonation using the specified user credentials
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>WindowsImpersonationContext</returns>
        public WindowsImpersonationContext BeginImpersonation(string domain, string user, string password)
        {
            WindowsIdentity wi = null;
            WindowsImpersonationContext wic = null;

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            string serviceUser = user;
            string servicePassword = password;
            string serviceDomain = domain;

            if (String.IsNullOrEmpty(domain) && String.IsNullOrEmpty(user) && String.IsNullOrEmpty(password))
            {
                // get the user credentials from the control table
                serviceUser = Properties.Settings.Default.ServiceUser;
                servicePassword = Properties.Settings.Default.ServicePassword;
                serviceDomain = Properties.Settings.Default.ServiceDomain;
            }

            try
            {
                wi = Impersonation.GetImpersonationIdentity(serviceUser, serviceDomain, servicePassword);
                wic = wi.Impersonate();
            }
            catch(Exception e)
            {
                if (wic != null)
                {
                    wic.Undo();
                    wic.Dispose();
                }

                Dictionary<string, object> methodParameters = new Dictionary<string, object>();
                methodParameters.Add("domain", domain);
                methodParameters.Add("user", user);
                methodParameters.Add("password", password);

                LogPlugin.Logger.LogErrorMessageWithException("SecurityService.BeginImpersonation", e.Message, e, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });

            }

            return wic;
        }
        /// <summary>
        /// Ends Impersonation
        /// </summary>
        /// <param name="windowsImpersonationContext"></param>
        public void EndImpersonation(WindowsImpersonationContext windowsImpersonationContext)
        {
            if (windowsImpersonationContext != null)
            {
                windowsImpersonationContext.Undo();
                windowsImpersonationContext.Dispose();
            }
        }

        #endregion
    }
}
