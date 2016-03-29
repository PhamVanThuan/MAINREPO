using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.Configuration;
using System.Configuration;

namespace SAHL.Common.Test
{
    [TestFixture]
    public class RedirectionConfigurationTest
    {
        [Test]
        public void GetConfiguration()
        {
            try
            {
                object mieeu = ConfigurationManager.GetSection("SAHLFactories");

                SAHLRedirectionSection RedirectionSection = (SAHLRedirectionSection)ConfigurationManager.GetSection("RedirectionConfiguration");
                if (RedirectionSection != null)
                {
                    int i = RedirectionSection.EntryPoints.Count;
                    RedirectionElement Redirect = RedirectionSection.GetRedirection(RedirectionSection.GetType(), "ReleaseAndVariationsConditions");
                }
            }
            catch (Exception E)
            {
                string msg = E.Message;
                throw;
            }

        }
    }
}
