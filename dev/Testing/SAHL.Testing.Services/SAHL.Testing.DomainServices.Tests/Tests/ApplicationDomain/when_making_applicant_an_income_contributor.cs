using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_making_applicant_an_income_contributor : ServiceTestBase<IApplicationDomainServiceClient>
    {
        private ActiveNewBusinessApplicantsDataModel applicant;

        [TearDown]
        new public void OnTestTearDown()
        {
            if (applicant != null)
            {
                var command = new RemoveApplicantFromActiveNewBusinessApplicantsCommand(applicant.OfferRoleKey);
                base.PerformCommand(command);
                applicant = null;
            }
            base.OnTestTearDown();
        }

        [Test]
        public void when_successful()
        {
            applicant = TestApiClient.GetAny<ActiveNewBusinessApplicantsDataModel>(new { isincomecontributor = 0, hasemployment = 1 }, limit: 500);
            var command = new MakeApplicantAnIncomeContributorCommand(applicant.OfferRoleKey);
            base.Execute<MakeApplicantAnIncomeContributorCommand>(command);
            var applicationRoles = TestApiClient.Get<OfferRoleAttributeDataModel>(new { offerrolekey = applicant.OfferRoleKey });
            Assert.IsTrue(applicationRoles.Where(x => x.OfferRoleAttributeTypeKey == (int)OfferRoleAttributeType.IncomeContributor).Count() == 1,
                string.Format("Income contributor, offer role attribute does not exist for legal entity: {0} on application: {1}", applicant.LegalEntityKey, applicant.OfferKey));
        }

        [Test]
        public void when_unsuccessful()
        {
            applicant = TestApiClient.GetAny<ActiveNewBusinessApplicantsDataModel>(new { isincomecontributor = 1, hasemployment = 1 }, limit: 500);
            var command = new MakeApplicantAnIncomeContributorCommand(applicant.OfferRoleKey);
            base.Execute<MakeApplicantAnIncomeContributorCommand>(command)
                .AndExpectThatErrorMessagesContain("Applicant is already an income contributor");
        }
    }
}