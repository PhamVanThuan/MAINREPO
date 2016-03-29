using System;
using System.Security.Principal;
using System.Threading;

namespace SAHL.Common.Security
{
    /// <summary>
    /// Extension of the standard <see cref="GenericPrincipal"/>, that allows us to add extra information to
    /// the principal.
    /// </summary>
    public class SAHLPrincipal : GenericPrincipal
    {
        private WindowsPrincipal wp;

        #region Constructors

        public SAHLPrincipal(IIdentity identity)
            : base(identity as WindowsIdentity, null)
        {
            if (identity is WindowsIdentity)
                wp = new WindowsPrincipal((WindowsIdentity)Identity);
        }

        public SAHLPrincipal(GenericIdentity identity)
            : base(identity, null)
        {
        }

        #endregion Constructors

        /// <summary>
        /// Gets the principal in the current process.
        /// </summary>
        /// <returns></returns>
        public static SAHLPrincipal GetCurrent()
        {
            if (SAHL.Common.Properties.Settings.Default.UseWindowsIdentity)
                return new SAHLPrincipal(WindowsIdentity.GetCurrent());

            //hack to get the SPC code in the web services to work!
            if (SAHL.Common.Properties.Settings.Default.UseCurrentPrincipal)
                return new SAHLPrincipal(new GenericIdentity(Thread.CurrentPrincipal.ToString()));

            return new SAHLPrincipal(new GenericIdentity(Thread.CurrentThread.ManagedThreadId.ToString()));
            
        }

        public override bool IsInRole(string role)
        {
            if (null != wp)
            {
                return wp.IsInRole(role);
            }
            throw new NotSupportedException("Unable to Evaluate from X2");
        }
    }
}