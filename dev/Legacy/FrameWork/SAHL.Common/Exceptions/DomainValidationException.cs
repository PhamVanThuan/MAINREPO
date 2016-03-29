using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SAHL.Common.CacheData;
using SAHL.Common.Security;

namespace SAHL.Common.Exceptions
{
    [Serializable]
    [JsonObject]
    public class DomainValidationException : Exception
    {
        public DomainValidationException()
            : this("Unable to perform save due to validation errors.")
        {
        }

        public DomainValidationException(string message)
            : base(message)
        {
            DomainErrorMessages = new List<string>();
            DomainWarningMessages = new List<string>();

            try
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                if (spc != null)
                {
                    IgnoreWarnings = spc.IgnoreWarnings;

                    if (spc.DomainMessages != null)
                    {
                        foreach (var errMsg in spc.DomainMessages.ErrorMessages)
                        {
                            DomainErrorMessages.Add(errMsg.Message);
                        }

                        foreach (var warnMsg in spc.DomainMessages.WarningMessages)
                        {
                            DomainWarningMessages.Add(warnMsg.Message);
                        }
                    }
                }
            }
            catch
            {
                // yes we are really eating this, we are making a change to extend what is logged and do not want to cause an inadvertent problem, 
                // if this fails let it and try again next time 03/10/2012
            }
        }

        public List<String> DomainErrorMessages { get; protected set; }

        public List<String> DomainWarningMessages { get; protected set; }

        public bool IgnoreWarnings { get; protected set; }
    }
}