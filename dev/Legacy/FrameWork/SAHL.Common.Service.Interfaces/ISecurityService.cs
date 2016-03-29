using System.Security.Principal;

namespace SAHL.Common.Service.Interfaces
{
    /// <summary>
    /// Represents a security service for handling security related functions
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Starts Impersonation using the default user credentials in the control table
        /// </summary>
        /// <returns>WindowsImpersonationContext</returns>
        WindowsImpersonationContext BeginImpersonation();

        /// <summary>
        /// Starts Impersonation using the specified user credentials
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>WindowsImpersonationContext</returns>
        WindowsImpersonationContext BeginImpersonation(string domain, string user, string password);

        /// <summary>
        /// Ends Impersonation
        /// </summary>
        /// <param name="windowsImpersonationContext"></param>
        void EndImpersonation(WindowsImpersonationContext windowsImpersonationContext);
    }
}