using NUnit.Framework;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using SAHL.Services.Interfaces.ITC;
using SAHL.Services.Interfaces.ITC.Commands;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ITC
{
    [TestFixture]
    public class when_performing_itc_check_for_an_existing_client : ServiceTestBase<IItcServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetClientsForAccountQuery();
            base.PerformQuery(query);
            var result = query.Result.Results.First();
            var command = new PerformClientITCCheckCommand(result.IdNumber, result.AccountNumber, @"SAHL\HaloUser");
            base.Execute<PerformClientITCCheckCommand>(command).WithoutErrors();
            Assert.IsFalse(messages.HasErrors);
        }
    }
}