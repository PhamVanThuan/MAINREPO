using NUnit.Framework;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.DomainServiceChecks
{
    public class when_requires_active_client_role : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_unsuccessful()
        {
            var query = new GetInactiveOfferRoleQuery();
            base.PerformQuery(query);
            int offerRoleKey = query.Result.Results.First();
            var command = new MakeApplicantAnIncomeContributorCommand(offerRoleKey);
            base.Execute<MakeApplicantAnIncomeContributorCommand>(command)
                .AndExpectThatErrorMessagesContain("There is no active client role for the provided offer role.");
        }
    }
}