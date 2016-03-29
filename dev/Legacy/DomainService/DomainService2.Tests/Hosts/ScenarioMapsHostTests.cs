using DomainService2.IOC;
using NUnit.Framework;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.ScenarioMaps;

namespace DomainService2.Tests.Hosts
{
    [TestFixture]
    public class ScenarioMapsHostTests
    {
        [Ignore("Used for dev testing.")]
        [Test]
        public void ExceptionTest()
        {
            IDomainMessageCollection messages = new DomainMessageCollection();
            IScenarioMaps scenarioMapsHost = DomainServiceLoader.Instance.Get<IScenarioMaps>();

            //scenarioMapsHost.ThrowDAO_Exception(messages, false);
            //scenarioMapsHost.ThrowSqlException(messages, false);
            //scenarioMapsHost.ThrowSqlTimeOutException(messages, false);
            //scenarioMapsHost.ThrowDomainValidationException(messages, false);
            //scenarioMapsHost.ThrowDomainMessageException(messages, false);
            //scenarioMapsHost.ThrowExceptionWithMessages(messages, false);
            //scenarioMapsHost.ThrowExceptionWithoutMessages(messages, false);
        }
    }
}