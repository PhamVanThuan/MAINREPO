using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinancialDomain;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;
using System.Web.Script.Serialization;

namespace SAHL.Testing.Services.Tests.FinancialDomain
{
    public class when_pricing_an_application : ServiceTestBase<IFinancialDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            GetAlphaHousingApplicationQuery query = new GetAlphaHousingApplicationQuery(-1, 0.8, isAccepted: false);

            base.PerformQuery(query);
            int applicationNumber = query.Result.Results.First().OfferKey;
            var appInfoQuery = new GetLatestApplicationInformationVariableLoanQuery(applicationNumber);
            base.PerformQuery(appInfoQuery);
            var applicationInfo = appInfoQuery.Result.Results.First();
            var applicationInformationVLBefore = TestApiClient.GetByKey<OfferInformationVariableLoanDataModel>(applicationInfo.OfferInformationKey);
            applicationInformationVLBefore.LoanAmountNoFees += 10000;
            applicationInformationVLBefore.LoanAgreementAmount += 10000;
            var updateCommand = new UpdateVariableLoanApplicationInformationCommand(applicationInformationVLBefore);
            base.PerformCommand(updateCommand);
            var command = new PriceNewBusinessApplicationCommand(applicationNumber);
            base.Execute<PriceNewBusinessApplicationCommand>(command);
            var applicationInformationVLAfter = TestApiClient.GetByKey<OfferInformationVariableLoanDataModel>(applicationInfo.OfferInformationKey);
            var jsSerializer = new JavaScriptSerializer();
            Assert.IsTrue(applicationInformationVLBefore.LTV != applicationInformationVLAfter.LTV &&
                applicationInformationVLBefore.MonthlyInstalment != applicationInformationVLAfter.MonthlyInstalment &&
                applicationInformationVLBefore.PTI != applicationInformationVLAfter.PTI,
                string.Format("Pricing information was not updated.  \r\n Before: {0} \r\n After: {1}", jsSerializer.Serialize(applicationInformationVLBefore),
                jsSerializer.Serialize(applicationInformationVLAfter)));
        }

        [Test]
        public void when_unsuccessful()
        {
            var applications = TestApiClient.Get<OfferDataModel>(new Where(OfferStatus.Open, OfferType.Life), 1);
            var application = applications.FirstOrDefault();
            var command = new PriceNewBusinessApplicationCommand(application.OfferKey);
            base.Execute<PriceNewBusinessApplicationCommand>(command).AndExpectThatErrorMessagesContain("The latest application information for your application is not open.");
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