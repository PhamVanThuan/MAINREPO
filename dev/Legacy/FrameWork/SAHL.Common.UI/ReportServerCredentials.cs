using System;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Security.Principal;
using System.Web;

namespace SAHL.Common.Authentication
{
/// <summary>
/// This class is used by the ReportViewer Control to authenticate against the SQL Server Reporting Services Server
/// </summary>
/// <summary>
/// Local implementation of IReportServerCredentials
/// </summary>
    public class ReportServerCredentials : IReportServerCredentials
    {
        private string _userName;
        private string _password;
        private string _domain;
        WindowsIdentity _WindowsIdentity;

        /// <summary>
        /// Construct a new instance using the current HttpContext. Grabs the Context.User.Identity to authenticate
        /// </summary>
        /// <param name="Context"></param>
        public ReportServerCredentials(HttpContext Context)
        {
            //if (Context.User.Identity.AuthenticationType == "NTLM") coule be Kerberos or Negotiate as well
            _WindowsIdentity = (WindowsIdentity)Context.User.Identity;
        }

        /// <summary>
        /// Create 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        public ReportServerCredentials(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }

        /// <summary>
        /// Gets the WindowsIdentity that was passed in the Context object on construction
        /// </summary>
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return _WindowsIdentity;
            }
        }

        /// <summary>
        /// Returns the DefaultNetworkCredentials, DefaultCredentials or NetworkCredential using the passed in username, password and domain
        /// </summary>
        public ICredentials NetworkCredentials
        {
            get
            {
                ICredentials retval = null;

                // We're getting the user from web.config for now

                // will be in Domain\User format
                if (
                        (Properties.Settings.Default["ReportServerUser"] != null) &&
                        (Properties.Settings.Default["ReportServerUserNotPassword"] != null)
                    )
                {

                    string szReportServerUser = (string)Properties.Settings.Default["ReportServerUser"];

                    // stored reversed in the web config, advanced encryption standard
                    string szReportServerUserPwd = (string)Properties.Settings.Default["ReportServerUserNotPassword"];

                    Reverse(ref szReportServerUserPwd);

                    string[] szReportServerUserDomain = szReportServerUser.Split('\\');

                    if (szReportServerUserDomain.Length == 2)
                        retval = new NetworkCredential(szReportServerUserDomain[1], szReportServerUserPwd, szReportServerUserDomain[0]);
                    else
                        retval = new NetworkCredential(szReportServerUserDomain[0], szReportServerUserPwd);

                    return retval;
                }


                if (CredentialCache.DefaultNetworkCredentials != null)
                    retval = CredentialCache.DefaultNetworkCredentials;
                else
                    if (CredentialCache.DefaultCredentials != null)
                        retval = CredentialCache.DefaultCredentials;
                    else
                        if ((_userName != null) && (_password != null) && (_domain != null))
                            retval = new NetworkCredential(_userName, _password, _domain);

                return retval;
            }
        }


        /// <summary>
        /// Returns nothing so don't bother
        /// </summary>
        /// <param name="authCookie"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="authority"></param>
        /// <returns></returns>
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            // Do not use forms credentials to authenticate.
            authCookie = null;
            user = password = authority = null;
            return false;
        }

        private void Reverse(ref string a)
        {
            char[] array = a.ToCharArray();
            Array.Reverse(array);
            a = new string(array);
        }

    }
}