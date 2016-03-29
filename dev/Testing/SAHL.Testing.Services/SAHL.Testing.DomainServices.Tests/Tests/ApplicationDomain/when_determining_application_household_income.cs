using NUnit.Framework;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    [TestFixture]
    public class when_determining_application_household_income : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetOpenNewBusinessApplicationQuery query = new GetOpenNewBusinessApplicationQuery(hasDebitOrder: true, hasMailingAddress: true, hasProperty: true,
                isAccepted: false, householdIncome: 6000);
            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.First().OfferKey;
            var command = new SetHouseholdIncomeToZeroCommand(applicationNumber);
            base.PerformCommand(command);
            var determineHouseholdIncomeCommand = new DetermineApplicationHouseholdIncomeCommand(applicationNumber);
            base.Execute<DetermineApplicationHouseholdIncomeCommand>(determineHouseholdIncomeCommand);
            var applicationInfoQuery = new GetLatestApplicationInformationVariableLoanQuery(applicationNumber);
            base.PerformQuery(applicationInfoQuery);
            var offerInformation = applicationInfoQuery.Result.Results.First();
            Assert.IsTrue(offerInformation.HouseholdIncome > 0);
        }

        [Test]
        public void when_unsuccesful()
        {
            GetOpenNewBusinessApplicationQuery query = new GetOpenNewBusinessApplicationQuery(hasDebitOrder: true, hasMailingAddress: true, hasProperty: true,
                isAccepted: true, householdIncome: 6000);
            base.PerformQuery(query);
            var applicationNumber = query.Result.Results.First().OfferKey;
            var setEmploymentTypeCommand = new DetermineApplicationHouseholdIncomeCommand(applicationNumber);
            base.Execute<DetermineApplicationHouseholdIncomeCommand>(setEmploymentTypeCommand)
                .AndExpectThatErrorMessagesContain("Household Income cannot be determined once the application has been accepted");
        }
    }
}