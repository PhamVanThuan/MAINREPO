using NUnit.Framework;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System.Linq;

namespace SAHL.Testing.Services.Tests.ApplicationDomain
{
    public class when_adding_applicant_debit_order : ServiceTestBase<IApplicationDomainServiceClient>
    {
        [Test]
        public void when_successful()
        {
            var query = new GetApplicationWithApplicantBankAccountQuery(false);
            base.PerformQuery(query);
            var model = query.Result.Results.FirstOrDefault();
            var applicationDebitOrderModel = new ApplicationDebitOrderModel(model.ApplicationNumber, model.DebitOrderDay, model.ClientBankAccountKey);
            var command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
            base.Execute<AddApplicationDebitOrderCommand>(command).WithoutErrors();
            var debitOrder = TestApiClient.Get<OfferDebitOrderDataModel>(new { offerkey = model.ApplicationNumber });
            Assert.IsNotNull(debitOrder.FirstOrDefault());
        }

        [Test]
        public void when_unsuccessful()
        {
            var query = new GetApplicationWithApplicantBankAccountQuery(hasDebitOrder: true);
            base.PerformQuery(query);
            var model = query.Result.Results.FirstOrDefault();
            var applicationDebitOrderModel = new ApplicationDebitOrderModel(model.ApplicationNumber, model.DebitOrderDay, model.ClientBankAccountKey);
            var command = new AddApplicationDebitOrderCommand(applicationDebitOrderModel);
            base.Execute<AddApplicationDebitOrderCommand>(command)
                .AndExpectThatErrorMessagesContain("An existing application debit order is in place.");
        }
    }
}