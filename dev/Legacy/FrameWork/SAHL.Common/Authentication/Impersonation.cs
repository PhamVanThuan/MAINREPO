using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using SAHL.Common.Logging;

namespace SAHL.Common.Authentication
{
    /// <summary>
    /// Provides functionality
    /// </summary>
    public class Impersonation
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        /// <summary>
        /// This allows you (at runtime) to impersonate the specified user.
        /// <para>
        /// <strong>Usage:</strong>
        /// <code>
        ///   WindowsIdentity wi = Common.Impersonation.GetImpersonationIdentity(UserName, Domain, Password);
        ///   WindowsImpersonationContext wic = null;
        ///
        ///    try
        ///    {
        ///        if (wi != null) wic = wi.Impersonate();
        ///    }
        ///    finally
        ///    {
        ///        if (wic != null)
        ///        {
        ///            wic.Undo();
        ///            wic.Dispose();
        ///        }
        ///    }
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="DomainName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public static WindowsIdentity GetImpersonationIdentity(string UserName, string DomainName, string Password)
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            //This parameter causes LogonUser to create a primary token.
            const int LOGON32_LOGON_INTERACTIVE = 2;

            IntPtr tokenHandle = new IntPtr(0);

            tokenHandle = IntPtr.Zero;
            WindowsIdentity winId = null;

            try
            {
                // Call LogonUser to obtain a handle to an access token.
                bool returnValue = LogonUser(UserName, DomainName, Password,
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                    ref tokenHandle);

                if (false == returnValue)
                {
                    int ret = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(ret);
                }

                winId = new WindowsIdentity(tokenHandle);
            }
            catch(Exception e)
            {
                LogPlugin.Logger.LogErrorMessageWithException(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, string.Empty, e);
            }
            finally
            {
                // Free the tokens.
                if (tokenHandle != IntPtr.Zero)
                    CloseHandle(tokenHandle);
            }
            return winId;
        }
    }
}

#region Original Code

//public class ImpersonationDemo
//{
//    // Test harness.
//    // If you incorporate this code into a DLL, be sure to demand FullTrust.
//    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
//    public static void Main(string[] args)
//    {
//        IntPtr tokenHandle = new IntPtr(0);
//        IntPtr dupeTokenHandle = new IntPtr(0);
//        try
//        {
//            string userName, domainName;
//            // Get the user token for the specified user, domain, and password using the
//            // unmanaged LogonUser method.
//            // The local machine name can be used for the domain name to impersonate a user on this machine.
//            Console.Write("Enter the name of the domain on which to log on: ");
//            domainName = Console.ReadLine();

//            Console.Write("Enter the login of a user on {0} that you wish to impersonate: ", domainName);
//            userName = Console.ReadLine();

//            Console.Write("Enter the password for {0}: ", userName);

//            Console.WriteLine("LogonUser called.");

//            if (false == returnValue)
//            {
//                int ret = Marshal.GetLastWin32Error();
//                Console.WriteLine("LogonUser failed with error code : {0}", ret);
//                throw new System.ComponentModel.Win32Exception(ret);
//            }

//            Console.WriteLine("Did LogonUser Succeed? " + (returnValue? "Yes" : "No"));
//            Console.WriteLine("Value of Windows NT token: " + tokenHandle);

//            // Check the identity.
//            Console.WriteLine("Before impersonation: "
//                + WindowsIdentity.GetCurrent().Name);
//            // Use the token handle returned by LogonUser.
//            WindowsIdentity newId = new WindowsIdentity(tokenHandle);
//            WindowsImpersonationContext impersonatedUser = newId.Impersonate();

//            // Check the identity.
//            Console.WriteLine("After impersonation: "
//                + WindowsIdentity.GetCurrent().Name);

//            // Stop impersonating the user.
//            impersonatedUser.Undo();

//            // Check the identity.
//            Console.WriteLine("After Undo: " + WindowsIdentity.GetCurrent().Name);

//            // Free the tokens.
//            if (tokenHandle != IntPtr.Zero)
//                CloseHandle(tokenHandle);

//        }
//        catch(Exception ex)
//        {
//            Console.WriteLine("Exception occurred. " + ex.Message);
//        }

//    }

//}

#endregion Original Code