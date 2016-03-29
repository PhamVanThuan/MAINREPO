using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Helpers
{
    /// <summary>
    /// Helper methods for dealing with origination sources.
    /// </summary>
    public sealed class OriginationSourceHelper
    {

        private OriginationSourceHelper()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adUserName">The domain name of the user (e.g. SAHL\MattS)</param>
        /// <returns></returns>
        public static int PrimaryOriginationSourceKey(string adUserName)
        {
            if (adUserName.ToLower().StartsWith("rcshl"))
                return (int)SAHL.Common.Globals.OriginationSources.RCS;
            else
                return (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal">The user principal</param>
        /// <returns></returns>
        public static int PrimaryOriginationSourceKey(SAHLPrincipal principal)
        {
            return PrimaryOriginationSourceKey(principal.Identity.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ADUser"></param>
        /// <returns></returns>
        public static int PrimaryOriginationSourceKey(IADUser ADUser)
        {
            return PrimaryOriginationSourceKey(ADUser.ADUserName);
        }
    }
}
