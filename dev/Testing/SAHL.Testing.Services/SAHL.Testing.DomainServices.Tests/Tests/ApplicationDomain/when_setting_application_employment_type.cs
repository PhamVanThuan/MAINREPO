using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    public class when_setting_application_employment_type : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetOpenNewBusinessApplicationQuery query = new GetOpenNewBusinessApplicationQuery(hasDebitOrder: true, hasMailingAddress: true, hasProperty: true,
                isAccepted: false, householdIncome: 6000);
            base.PerformQuery(query);
            var applicationNumber = query.Result.Results.First().OfferKey;
            var setEmploymentTypeCommand = new SetApplicationEmploymentTypeToUnknownCommand(applicationNumber);
            base.PerformCommand(setEmploymentTypeCommand);
            var command = new SetApplicationEmploymentTypeCommand(applicationNumber);
            base.Execute<SetApplicationEmploymentTypeCommand>(command);
            var applicationInfoQuery = new GetLatestApplicationInformationVariableLoanQuery(applicationNumber);
            base.PerformQuery(applicationInfoQuery);
            var offerInformation = applicationInfoQuery.Result.Results.First();
            Assert.IsTrue(offerInformation.EmploymentTypeKey != (int)EmploymentType.Unknown);
        }

        [Test]
        public void when_unsuccesful()
        {
            GetOpenNewBusinessApplicationQuery query = new GetOpenNewBusinessApplicationQuery(hasDebitOrder: true, hasMailingAddress: true, hasProperty: true,
                isAccepted: true, householdIncome: 6000);
            base.PerformQuery(query);
            var applicationNumber = query.Result.Results.First().OfferKey;
            var setEmploymentTypeCommand = new SetApplicationEmploymentTypeCommand(applicationNumber);
            base.Execute<SetApplicationEmploymentTypeCommand>(setEmploymentTypeCommand)
                .AndExpectThatErrorMessagesContain("Application employment type cannot be set once the application has been accepted");
        }
    }
}