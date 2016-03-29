using System;
using System.Collections.Generic;
using System.Text;

using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.CacheData;

namespace SAHL.Common.BusinessModel.Validation
{
    public class ValidationHelper
    {
        public static void StringLengthValidator(IList<IDomainMessage> Messages, int MinLength, int MaxLength)
        {

        }

        /// <summary>
        /// Used by repositories to determine if the current principal has validation errors.
        /// </summary>
        public static bool PrincipalHasValidationErrors()
        {
            return PrincipalHasValidationErrors(SAHLPrincipal.GetCurrent());
        }

        /// <summary>
        /// Used by repositories to determine if a principal has validation errors.
        /// </summary>
        public static bool PrincipalHasValidationErrors(SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            if (spc == null)
                return true;
            else
            {
                if (spc.IgnoreWarnings)
                    return spc.DomainMessages.HasErrorMessages;
                else
                    return (spc.DomainMessages.Count > 0);
            }

        }

    }
}
