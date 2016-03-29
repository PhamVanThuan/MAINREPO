using NUnit.Framework;
using SAHL.Core.Identity;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Models;

namespace SAHL.Testing.Services.Tests.DomainServiceChecks
{
    public class when_requires_client : ServiceTestBase<IClientDomainServiceClient>
    {
        [Test]
        public void when_unsuccessful()
        {
            int clientKey = 123;
            var model = new FixedLongTermInvestmentLiabilityModel(CombGuid.Instance.GenerateString(), 333500D);
            var command = new AddFixedLongTermInvestmentLiabilityToClientCommand(model, clientKey);
            base.Execute<AddFixedLongTermInvestmentLiabilityToClientCommand>(command)
                .AndExpectThatErrorMessagesContain("The client provided, does not exist.");
        }
    }
}