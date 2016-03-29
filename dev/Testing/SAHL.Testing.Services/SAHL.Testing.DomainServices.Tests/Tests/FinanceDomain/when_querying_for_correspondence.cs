using System.Linq;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using SAHL.Core.Data.Models.EventStore;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Core.Data.Models.FETest;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using System;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;
using SAHL.Core.Services;
using System.Threading;

namespace SAHL.Testing.Services.Tests.Tests.FinanceDomain
{
    [TestFixture]
    public class when_querying_for_correspondence : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private int _thirdPartyInvoiceKey = 0;
        private EmptyThirdPartyInvoicesDataModel emptyInvoice;

        [SetUp]
        new public void OnTestSetup()
        {
            base.OnTestSetup();
            emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            _thirdPartyInvoiceKey = emptyInvoice.ThirdPartyInvoiceKey;
            var invoiceProcessor = TestApiClient.Get<HaloUsersDataModel>(new { ADUserName = "InvoiceProcessor" }).FirstOrDefault();
            var orgStructureKey = invoiceProcessor.UserOrganisationStructureKey;
            var userCapabilities = invoiceProcessor.Capabilities;
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, orgStructureKey.ToString());
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, userCapabilities);
            _metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERNAME, string.Concat(@"SAHL\", invoiceProcessor.ADUserName));
        }

        [Test]
        public void when_successfully_querying_post_approval()
        {
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            decimal lineItemAmount = 100.00M;
            decimal lineItem2Amount = 200.00M;
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItemAmount, true),
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItem2Amount, false)
            };
            
            DateTime invoiceDate = DateTime.Now;
            string paymentReference = "Query integration test";
            ThirdPartyInvoiceModel invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, thirdPartyId, string.Format("SAHL-AutomatedTest-{0}", base.randomizer.Next()),
                invoiceDate, lineItems, false, paymentReference);
            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            Execute(command).WithoutErrors();
            var approveCommand = new ApproveThirdPartyInvoiceCommand(_thirdPartyInvoiceKey);
            Execute(approveCommand).WithoutErrors();
            
            var legalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdParty.LegalEntityKey });
            var projectionBefore = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName });
            int processedCountBeforeQuery = projectionBefore.FirstOrDefault().Processed;
            int unprocessedCountBeforeQuery = projectionBefore.FirstOrDefault().Unprocessed;

            var queryCommand = new QueryThirdPartyInvoiceCommand(_thirdPartyInvoiceKey, "Query integration test");
            Execute(queryCommand).WithoutErrors();
            Thread.Sleep(5000);
            var projectionAfter = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName });
            int processedCountAfterQuery = projectionAfter.FirstOrDefault().Processed;
            int unprocessedCountAfterQuery = projectionAfter.FirstOrDefault().Unprocessed;

            //Since the invoice has been approved, it has been flagged as processed
            Assert.That(processedCountAfterQuery == processedCountBeforeQuery-1);
            Assert.That(unprocessedCountAfterQuery == unprocessedCountBeforeQuery+1);
        }

        [Test]
        public void when_successfully_querying_pre_approval()
        {
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            decimal lineItemAmount = 100.00M;
            decimal lineItem2Amount = 200.00M;
            List<InvoiceLineItemModel> lineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItemAmount, true),
                new InvoiceLineItemModel(null, _thirdPartyInvoiceKey, 1, lineItem2Amount, false)
            };

            DateTime invoiceDate = DateTime.Now;
            string paymentReference = "Query integration test";
            ThirdPartyInvoiceModel invoice = new ThirdPartyInvoiceModel(_thirdPartyInvoiceKey, thirdPartyId, string.Format("SAHL-AutomatedTest-{0}", base.randomizer.Next()),
                invoiceDate, lineItems, false, paymentReference);
            var command = new CaptureThirdPartyInvoiceCommand(invoice);
            Execute(command).WithoutErrors();

            var legalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdParty.LegalEntityKey });
            var projectionBefore = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName });
            int processedCountBeforeQuery = projectionBefore.FirstOrDefault().Processed;
            int unprocessedCountBeforeQuery = projectionBefore.FirstOrDefault().Unprocessed;

            var queryCommand = new QueryThirdPartyInvoiceCommand(_thirdPartyInvoiceKey, "Query integration test");
            Execute(queryCommand).WithoutErrors();
            Thread.Sleep(5000);
            var projectionAfter = TestApiClient.Get<AttorneyInvoiceMonthlyBreakdownDataModel>(new { AttorneyName = legalEntity.FirstOrDefault().RegisteredName });
            int processedCountAfterQuery = projectionAfter.FirstOrDefault().Processed;
            int unprocessedCountAfterQuery = projectionAfter.FirstOrDefault().Unprocessed;

            //Since the invoice has not been approved, it is not marked as processed
            Assert.That(processedCountAfterQuery == processedCountBeforeQuery);
            Assert.That(unprocessedCountAfterQuery == unprocessedCountBeforeQuery);
        }

        [TearDown]
        new public void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (emptyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(_thirdPartyInvoiceKey);
                PerformCommand(command);
                _thirdPartyInvoiceKey = 0;
                emptyInvoice = null;
            }
        }

    }
}