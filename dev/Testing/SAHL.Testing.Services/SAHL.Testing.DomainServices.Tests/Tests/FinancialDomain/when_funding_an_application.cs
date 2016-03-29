using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.FinancialDomain
{
    [TestFixture]
    public class when_funding_an_application : ServiceTestBase<IFinancialDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetAlphaHousingApplicationQuery query = new GetAlphaHousingApplicationQuery(SPVKey: -1, LTV: 0.8, isAccepted: false);
            base.PerformQuery(query);
            var application = query.Result.Results.First();
            var applicationInfoQuery = new GetLatestApplicationInformationVariableLoanQuery(application.OfferKey);
            base.PerformQuery(applicationInfoQuery);
            var applicationInformation = applicationInfoQuery.Result.Results.First();
            var setSPVCommand = new SetApplicationSPVToZeroCommand(applicationInformation.OfferInformationKey);
            base.PerformCommand(setSPVCommand);
            var command = new FundNewBusinessApplicationCommand(application.OfferKey);
            base.Execute<FundNewBusinessApplicationCommand>(command);
            var appInfo = TestApiClient.GetByKey<OfferInformationVariableLoanDataModel>(applicationInformation.OfferInformationKey);
            Assert.IsTrue(appInfo.SPVKey > 0, string.Format(@"SPV was not updated"));
        }

        [Test]
        public void when_unsuccessful()
        {
            var applications = TestApiClient.Get<OfferDataModel>(new Where(OfferStatus.Accepted, OfferType.NewPurchaseLoan), 1);
            var application = applications.FirstOrDefault();
            var command = new FundNewBusinessApplicationCommand(application.OfferKey);
            base.Execute<FundNewBusinessApplicationCommand>(command).AndExpectThatErrorMessagesContain("No open application could be found against your application number.");
        }

        [Test]
        public void when_unsuccessful_invalid_application_number()
        {
            var command = new FundNewBusinessApplicationCommand(0);
            base.Execute<FundNewBusinessApplicationCommand>(command).AndExpectThatErrorMessagesContain("There was a validation error: [Invalid Application Number. {in: root}], processing has been halted");
        }

        internal class Where
        {
            public Where(OfferStatus offerStatusKey, OfferType offerTypeKey)
            {
                this.OfferStatusKey = (int)offerStatusKey;
                this.OfferTypeKey = (int)offerTypeKey;
            }

            public int OfferStatusKey { get; set; }

            public int OfferTypeKey { get; set; }
        }
    }
}