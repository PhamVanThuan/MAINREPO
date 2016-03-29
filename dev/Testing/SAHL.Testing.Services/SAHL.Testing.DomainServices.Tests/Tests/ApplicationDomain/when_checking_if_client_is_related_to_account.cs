using NUnit.Framework;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_checking_if_client_is_related_to_account : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_unsuccessful()
        {
            var command = new DoesAccountBelongToClientQuery(1935261, "7004200226087");
            base.Execute<DoesAccountBelongToClientQuery>(command);

            Assert.IsTrue(command.Result.Results.First().OfferKey > 0);
        }
    }
}